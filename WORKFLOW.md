# FloorLayout Workflow: From Drawing to 3D Mesh

This document traces the complete data flow from user drawing to final FBX mesh generation.

## High-Level Overview

```
User Drawing (WPF Canvas)
    ↓
FloorLayoutInput Template (XML)
    ↓
APILib: RoomGenWrapper (Room Generation Algorithm)
    ↓
FloorMaker Template (Rooms, Edges, Vertices)
    ↓
Door Selection Algorithm
    ↓
SimpleLayout Template (Compilation)
    ↓
FBX Mesh Generation
```

## Detailed Flow

### Phase 1: User Input (Edit2DLib)

**Location**: FloorLayout WPF Application

**Data Structures**:
- `Edit2DHoleGroup` (for Outline and OpenAreas) - stores rectangles, ellipses, polygons
- `Edit2DGraphLayer` (for Walls) - stores edges and vertices

**User Draws**:
1. **Outline Areas**: Exterior boundaries (rectangles, ellipses, or polygons)
2. **Open Areas**: Interior courtyards/atriums (rectangles, ellipses, or polygons)
3. **Walls**: Line segments that subdivide space

**Data Storage**:
```
oFWRInput.OutlineAreas.HoleGroupList[0]
    └── HoleList[] → references to rectangles/ellipses/polygons
    
oFWRInput.OpenAreas.HoleGroupList[0]
    └── HoleList[] → references to rectangles/ellipses/polygons
    
oFWRInput.Walls.Edit2dGraphLayerList[0]
    └── EdgeList[] → wall line segments
    └── VertexList[] → wall endpoints
```

---

### Phase 2: Template Conversion (Generate Button)

**File**: `ViewModelCanvas/Commands/Buttons/Generate.cs`

**Step 2.1**: Convert drawing data to `FloorLayoutInput` template

**Method**: `LoadFloorLayoutInputFromEdit()`

```csharp
FloorLayoutInput {
    Outline: SingleHoleGroup {
        oHoleGroup: HoleGroup
        EllipseArray: BoundaryEllipse[]
        RectangleArray: BoundaryRectangle[]
        PolygonArray: BoundaryPolygon[]
    }
    OpenArea: SingleHoleGroup {
        // Same structure as Outline
    }
    WallSegmentArray: LineSegment[] {
        From: Point2D { X, Y }
        To: Point2D { X, Y }
    }
}
```

**Key Operations**:
- Inverts Y coordinates (screen space → world space)
- Extracts hole group references (rectangles, ellipses, polygons)
- Converts wall edges to LineSegment array

---

### Phase 3: Room Generation (First API Call)

**API Command**: `"getfloorlayout"`

**Entry Point**: `APILib.API.APIEntry("getfloorlayout", xmldata)`

**Flow**:
```
API.cs (line 143)
    ↓
RoomGenXML.Perform(sts, FloorLayoutInput)
    ↓
RoomGenWrapper.AllSteps()
```

#### RoomGenWrapper Pipeline (7 Steps)

**Constructor**: Converts FloorLayoutInput to internal PEdge structures

```csharp
// Converts shapes to point lists, then to PEdge lists
PEOutline: List<PEdge>           // Outline boundary edges
PEOpenAreaList: List<List<PEdge>> // Open area boundary edges  
PEWallSectionList: List<List<PEdge>> // Wall segment edges
```

**PEdge Structure**:
```csharp
PEdge {
    From: PointF
    To: PointF
    ID: string  // "Outline", "OpenArea", or "Wall"
    IsWallSection: bool
    Width: float
    Height: float
    InsideOpenArea: bool
    RoomCount: int
}
```

**Step 1**: Find intersections between outline and open areas
- Splits edges where outline and open areas intersect
- Marks which outline edges are inside open areas

**Step 2**: Remove open areas not inside outline
- Filters out any open area edges that fall outside the outline

**Step 3**: Create open area edges
- Finalizes the list of valid open area boundaries

**Step 4**: Split wall sections against open areas
- Cuts wall segments where they cross open area boundaries
- Removes wall portions that fall inside open areas

**Step 5**: Eliminate wall sections inside outline sections
- Removes any wall segments that don't contribute to room division

**Step 6**: Find intersections of wall sections with each other
- Splits walls where they intersect
- Creates connection points for room boundaries

**Step 7**: **Find all rooms created by wall segments**

**Room Detection Algorithm** (`PotentialRooms`):
- Starting from each wall edge endpoint, trace clockwise around connected edges
- A room is found when the path returns to the starting point
- Each room is a closed loop of PEdge objects

**Output**:
```csharp
RoomList: List<Room> {
    Room {
        StartingEdge: PEdge
        WallSegments: List<PEdge>  // Ordered list forming closed loop
    }
}
```

#### Convert Rooms to FloorMaker Template

**Method**: `RoomGenWrapper.GetFloorLayout()`

**Conversion Process**:

1. **Extract unique vertices** from all room edges
   ```csharp
   VertexList: Vertex[] {
       Index: int
       X: float
       Y: float
   }
   ```

2. **Extract unique edges** (shared edges appear once)
   ```csharp
   EdgeList: FMEdge[] {
       Index: int
       p1: int  // vertex index
       p2: int  // vertex index
       IsExteriorEdge: 1 or 0  // (ID == "Outline")
       IsOpenSpaceEdge: 1 or 0  // (ID == "OpenArea")
       InteriorDoorCandidate: 1 or 0
       ExteriorWindowCandidate: 1 or 0
   }
   ```

3. **Create rooms** referencing edges by index
   ```csharp
   AssembledRoomList: FMAssembledRoom[] {
       EdgeIndexList: int[]  // Indices into EdgeList
       ConnectsToOpenArea: 1 or 0
       BackRoom: 1 or 0  // Doesn't directly connect to open area
   }
   ```

4. **Mark door candidates**:
   - Edges with `ID == "OpenArea"` → `InteriorDoorCandidate = 1`
   - Edges with `ID == "Outline"` → `ExteriorWindowCandidate = 1`
   - Edges connecting back rooms to front rooms → `InteriorDoorCandidate = 1`

**Output**: `FloorMaker` template with:
- All vertices (unique points)
- All edges (shared between rooms)
- All rooms (as edge index lists)
- Door/window candidate markings

**Serialization**: Converted to XML via `FloorMaker.GetProperties()` and saved to temp file

**XML Structure**:
```xml
<floormaker>
    <list name="vertexlist">
        <vertex index="0" x="10" y="20"/>
        ...
    </list>
    <list name="edgelist">
        <fmedge index="0" p1="0" p2="1" isexterioredge="0" .../>
        ...
    </list>
    <list name="assembledroomlist">
        <fmassembledroom>
            <list name="edgeindexlist">
                <int>0</int>
                <int>1</int>
                ...
            </list>
        </fmassembledroom>
        ...
    </list>
</floormaker>
```

---

### Phase 4: Door Selection (FloorLayout Application)

**File**: `Generate.cs` (line 44-96)

**Step 4.1**: Load XML back into FloorMaker template

```csharp
XElement xfl = GetFloorLayout(oInput.GetProperties());
FloorMaker oFloorLayout = new FloorMaker();
oFloorLayout.LoadProperties(xfl, out message);
```

**Note**: XML element name fixes required for compatibility:
- `<list name="roomlist">` → `<list name="assembledroomlist">`
- `<fmedge>` elements → `<fledge>` elements

**Step 4.2**: Door selection algorithm

```csharp
oFloorLayout.RandomlySelectDoors();
```

**Door Selection Process**:

**Case 1**: No destination rooms defined
- Picks first room as starting point
- Recursively finds all connected rooms
- Adds doors to connect them

**Case 2**: Destination rooms defined (open areas)
- For each destination room (open area):
  - Find all rooms directly connecting to it
  - Recursively find rooms connecting to those rooms
  - Build pathway by selecting door candidates on connecting edges

**Algorithm** (`BuildPathwayToDestinationRoom`):
- Starts from rooms touching the open area
- For each unvisited room:
  - Find common edges with visited rooms
  - Select one edge as door location
  - Mark edge with HoleGroupID (door pattern reference)
  - Add room to visited list
  - Recurse to rooms connected to this room

**Output**: `FloorMaker` with doors added
- Selected edges now have `HoleGroupID` set (door placement)

---

### Phase 5: Template Compilation

**File**: `Generate.cs` (line 96)

```csharp
XElement sl = oFloorLayout.Compile(5, 5);
```

**Method**: `FloorMaker.Compile(HorizontalScale, VerticalScale)`

**Conversion**: `FloorMaker` → `SimpleLayout`

**SimpleLayout Structure**:
```csharp
SimpleLayout {
    HorizontalScale: float
    VerticalScale: float
    VertexList: List<Vertex>
    EdgeList: List<Edge> {
        p1: int  // vertex index
        p2: int  // vertex index
        Width: int
        Height: int
        ID: string
        HoleGroupID: string  // References door/window pattern
    }
}
```

**Compilation**: `SimpleLayout.Compile()` generates XML for mesh generation

**Output XML** (BasicShapes):
```xml
<scene>
    <simplelayout>
        <list name="vertexlist">...</list>
        <list name="edgelist">
            <edge p1="0" p2="1" width="10" height="96" holegroupid="door_id"/>
            ...
        </list>
    </simplelayout>
</scene>
```

---

### Phase 6: Mesh Generation (Second API Call)

**API Command**: `"getmesh"`

**Entry Point**: `APILib.API.APIEntry("getmesh", xmldata)`

**File**: `ViewModelCanvas/Utilities/GetMesh.cs`

```csharp
XElement scene = new XElement("scene", sl);
GetMesh(outputFile, scene);
```

**API Flow**:
```
API.cs (line 185)
    ↓
GetMesh.Perform(sts, xmldata)
    ↓
Template2Mesh / BasicShapeList2Mesh
    ↓
ShapeLib geometry generation
    ↓
FBX export
```

**Mesh Generation Process**:

1. **Parse SimpleLayout XML**
2. **For each edge**:
   - Create 3D panel geometry (width × height)
   - Position at vertex coordinates
   - Orient along edge direction
   - If `HoleGroupID` is set: apply hole pattern (door/window cutout)
3. **Generate vertices, normals, triangles**
4. **Export to FBX format**

**Output**: FBX file at specified location

---

## XML Element Definitions: Who Creates What?

### FloorLayoutInput (Created by: FloorLayout WPF App)

**Source**: User drawing → `LoadFloorLayoutInputFromEdit()`

```xml
<floorlayoutinput>
    <outline>
        <holegroup>
            <holelist>
                <hole holetype="rect" holetypeindex="0" offsetx="0" offsety="0"/>
            </holelist>
        </holegroup>
        <list name="rectanglearray">
            <boundaryrectangle width="100" length="80"/>
        </list>
    </outline>
    <openarea>
        <!-- Same structure -->
    </openarea>
    <list name="wallsegmentarray">
        <linesegment>
            <from x="10" y="20"/>
            <to x="30" y="20"/>
        </linesegment>
    </list>
</floorlayoutinput>
```

### FloorMaker (Created by: APILib RoomGenWrapper)

**Source**: `RoomGenWrapper.GetFloorLayout()` → Room detection algorithm

```xml
<floormaker>
    <list name="vertexlist">
        <vertex index="0" x="10" y="20"/>
    </list>
    <list name="edgelist">
        <fmedge index="0" p1="0" p2="1" 
               isexterioredge="0" 
               isopenspaceedge="0"
               interiordoorcandidate="1"
               holegroupid=""/>
    </list>
    <list name="assembledroomlist">
        <fmassembledroom connectstoopenarea="1" backroom="0">
            <list name="edgeindexlist">
                <int>0</int>
                <int>1</int>
            </list>
        </fmassembledroom>
    </list>
</floormaker>
```

### SimpleLayout (Created by: FloorMaker.Compile())

**Source**: `FloorMaker.Compile()` → Simplified for mesh generation

```xml
<simplelayout horizontalscale="5" verticalscale="5">
    <list name="vertexlist">
        <vertex index="0" x="50" y="100"/>
    </list>
    <list name="edgelist">
        <edge p1="0" p2="1" 
              width="10" 
              height="96" 
              id="Outline"
              holegroupid="door_pattern_id"/>
    </list>
</simplelayout>
```

---

## Key Data Transformations

| Stage | Data Structure | Key Fields | Purpose |
|-------|---------------|------------|---------|
| User Input | `Edit2DHoleGroup`, `Edit2DGraphLayer` | Shape positions, wall vertices | Raw drawing data |
| FloorLayoutInput | `SingleHoleGroup`, `LineSegment[]` | Boundaries, walls as line segments | Serializable input format |
| PEdge Lists | `List<PEdge>` | Connected edge chains with IDs | Graph processing |
| Room Detection | `List<Room>` | Closed loops of PEdges | Detected room spaces |
| FloorMaker | `Vertex[]`, `FMEdge[]`, `FMAssembledRoom[]` | Shared vertices/edges, room definitions | Template with room topology |
| SimpleLayout | `List<Vertex>`, `List<Edge>` | Scaled coordinates, door references | Mesh generation input |
| 3D Mesh | Vertices, normals, triangles | Geometry data | FBX export |

---

## Why Doorways Might Be Missing

Based on the flow above, doors could fail to appear if:

1. **Room detection fails** (Step 7 of RoomGenWrapper)
   - Walls don't form closed loops
   - Open areas not properly integrated
   - Edge intersection issues

2. **Door candidate marking fails** (GetFloorLayout)
   - Edges not marked as `InteriorDoorCandidate`
   - Back room connection logic doesn't reach all rooms

3. **Door selection algorithm skips edges** (RandomlySelectDoors)
   - No destination rooms defined
   - Rooms already considered "connected"
   - Edge already marked as exterior/open space

4. **HoleGroupID not set** (RandomlySelectDoors)
   - Door placement logic doesn't assign hole pattern
   - XML compatibility issue with element names

5. **Hole pattern not applied during mesh generation** (GetMesh)
   - `HoleGroupID` not found in hole pattern library
   - Mesh generation doesn't process door cutouts

---

## Debugging Checklist

To diagnose missing doorways:

1. ✅ Check `FloorLayoutInput` has walls, outline, and open areas
2. ✅ Verify `RoomGenWrapper` produces rooms (check `RoomList.Count`)
3. ✅ Confirm `FloorMaker` has `AssembledRoomList` populated
4. ✅ Check `FMEdge` objects have `InteriorDoorCandidate = 1`
5. ✅ Verify `RandomlySelectDoors()` sets `HoleGroupID` on edges
6. ✅ Ensure `SimpleLayout` edges have `holegroupid` attribute
7. ✅ Check mesh generation applies hole patterns

Add debug logging at each transformation to trace where door data is lost.
