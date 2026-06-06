# FloorLayout System Flowchart

## Visual Flow Diagram

```
┌─────────────────────────────────────────────────────────────────────────┐
│                         USER INTERACTION LAYER                          │
│                         (FloorLayout WPF App)                           │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                    User draws on canvas using Edit2DLib:
                    • Outline Areas (rectangles, ellipses, polygons)
                    • Open Areas (courtyards, atriums)
                    • Wall Segments (line divisions)
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                    EDIT2D DATA STRUCTURES                               │
│                                                                         │
│  oFWRInput.OutlineAreas.HoleGroupList[0]                              │
│      └─ HoleList[] → BoundaryRectangle/Ellipse/Polygon                │
│                                                                         │
│  oFWRInput.OpenAreas.HoleGroupList[0]                                 │
│      └─ HoleList[] → BoundaryRectangle/Ellipse/Polygon                │
│                                                                         │
│  oFWRInput.Walls.Edit2dGraphLayerList[0]                              │
│      └─ EdgeList[] + VertexList[] (wall line segments)                │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                          [Generate Button Clicked]
                                    │
                    LoadFloorLayoutInputFromEdit()
                    • Convert shapes to template format
                    • Invert Y coordinates (screen → world)
                    • Extract wall line segments
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                   FLOORLAYOUTINPUT TEMPLATE (XML)                       │
│                                                                         │
│  <floorlayoutinput>                                                    │
│    <outline>                                                           │
│      <holegroup>...</holegroup>                                        │
│      <rectanglearray>...</rectanglearray>                             │
│    </outline>                                                          │
│    <openarea>...</openarea>                                           │
│    <wallsegmentarray>                                                 │
│      <linesegment><from/><to/></linesegment>                         │
│    </wallsegmentarray>                                                │
│  </floorlayoutinput>                                                  │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ (XML serialized)
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                 API CALL #1: GetFloorLayout()                           │
│                 Entry: API.APIEntry("getfloorlayout", xml)             │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│               APILIB: RoomGenXML.Perform()                              │
│               Creates RoomGenWrapper(FloorLayoutInput)                  │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                    Converts to internal graph structures:
                    • PEOutline: List<PEdge> (outline boundary)
                    • PEOpenAreaList: List<List<PEdge>> (open areas)
                    • PEWallSectionList: List<List<PEdge>> (walls)
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│            ROOMGENWRAPPER.ALLSTEPS() - 7-STEP PIPELINE                  │
│                                                                         │
│  Step 1: Find intersections (outline ∩ open areas)                     │
│           └─ Split edges at intersection points                        │
│                                                                         │
│  Step 2: Remove invalid open areas                                     │
│           └─ Filter open areas outside outline                         │
│                                                                         │
│  Step 3: Finalize open area edges                                      │
│           └─ Create clean open area boundaries                         │
│                                                                         │
│  Step 4: Split walls against open areas                                │
│           └─ Cut walls at open area boundaries                         │
│           └─ Remove wall portions inside open areas                    │
│                                                                         │
│  Step 5: Eliminate walls inside outline                                │
│           └─ Remove non-contributing wall segments                     │
│                                                                         │
│  Step 6: Find wall intersections                                       │
│           └─ Split walls where they cross each other                   │
│                                                                         │
│  Step 7: ★ DETECT ROOMS ★                                              │
│           └─ Trace clockwise from each edge                            │
│           └─ Find closed loops = rooms                                 │
│           └─ Build RoomList: List<Room>                                │
│                                                                         │
│         PotentialRooms Algorithm:                                       │
│         • Start at wall endpoint                                        │
│         • Follow edges clockwise (right-hand rule)                     │
│         • Return to start = room found                                 │
│         • Each Room = closed loop of PEdge objects                     │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                   ROOM DATA STRUCTURE                                   │
│                                                                         │
│  RoomList: List<Room> {                                                │
│    Room {                                                              │
│      StartingEdge: PEdge                                               │
│      WallSegments: List<PEdge>  (ordered, clockwise loop)             │
│    }                                                                   │
│  }                                                                     │
│                                                                         │
│  PEdge contains:                                                       │
│    • From/To coordinates                                               │
│    • ID: "Outline", "OpenArea", "Wall"                                │
│    • Width, Height (for 3D extrusion)                                 │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                    RoomGenWrapper.GetFloorLayout()
                    • Extract unique vertices
                    • Extract unique edges (shared between rooms)
                    • Build room-edge relationships via indices
                    • Mark door/window candidates
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                    FLOORMAKER TEMPLATE                                  │
│                                                                         │
│  VertexList: Vertex[] {                                                │
│    Index, X, Y                                                         │
│  }                                                                     │
│                                                                         │
│  EdgeList: FMEdge[] {                                                  │
│    Index, p1, p2 (vertex indices)                                     │
│    IsExteriorEdge: 1/0                                                │
│    IsOpenSpaceEdge: 1/0                                               │
│    InteriorDoorCandidate: 1/0  ← DOOR MARKING                        │
│    ExteriorWindowCandidate: 1/0                                       │
│    HoleGroupID: "" (empty until door selection)                       │
│  }                                                                     │
│                                                                         │
│  AssembledRoomList: FMAssembledRoom[] {                                │
│    EdgeIndexList: int[] (indices into EdgeList)                       │
│    ConnectsToOpenArea: 1/0                                            │
│    BackRoom: 1/0                                                      │
│  }                                                                     │
│                                                                         │
│  Door Candidate Logic:                                                 │
│    • Edge.ID == "OpenArea" → InteriorDoorCandidate = 1                │
│    • Edge.ID == "Outline" → ExteriorWindowCandidate = 1               │
│    • Edge connects BackRoom to FrontRoom → InteriorDoorCandidate = 1  │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                      FloorMaker.GetProperties()
                      Serialize to XML, save to temp file
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│              API #1 RETURNS: FloorMaker XML (Temp File)                 │
│                                                                         │
│  <floormaker>                                                          │
│    <list name="vertexlist">...</list>                                 │
│    <list name="edgelist">                                             │
│      <fmedge ... interiordoorcandidate="1" holegroupid=""/>          │
│    </list>                                                            │
│    <list name="assembledroomlist">...</list>                         │
│  </floormaker>                                                        │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ (Returned to FloorLayout app)
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│           FLOORLAYOUT APP: DOOR SELECTION PHASE                         │
│                                                                         │
│  1. Load XML back into FloorMaker template                             │
│     • Fix element names for compatibility                              │
│     • FloorMaker.LoadProperties(xml)                                  │
│                                                                         │
│  2. Run door selection algorithm                                       │
│     • FloorMaker.RandomlySelectDoors()                                │
│                                                                         │
│     Algorithm:                                                         │
│     • For each destination room (open area):                           │
│       - Find rooms touching it                                         │
│       - Recursively find connected rooms                               │
│       - For each unvisited room:                                       │
│         * Find common edge with visited room                           │
│         * Set edge.HoleGroupID = "door_pattern_id" ← PLACE DOOR       │
│         * Mark room as visited                                         │
│         * Recurse to its neighbors                                     │
│                                                                         │
│     Result: Selected edges now have HoleGroupID set                    │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│              FLOORMAKER WITH DOORS ASSIGNED                             │
│                                                                         │
│  EdgeList: FMEdge[] {                                                  │
│    ...                                                                 │
│    HoleGroupID: "door_pattern_123" ← NOW SET FOR SELECTED EDGES       │
│  }                                                                     │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                          FloorMaker.Compile(5, 5)
                          • Convert to SimpleLayout
                          • Apply horizontal/vertical scaling
                          • Simplify for mesh generation
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                   SIMPLELAYOUT TEMPLATE                                 │
│                                                                         │
│  <simplelayout horizontalscale="5" verticalscale="5">                 │
│    <list name="vertexlist">                                           │
│      <vertex index="0" x="50" y="100"/>                               │
│    </list>                                                            │
│    <list name="edgelist">                                             │
│      <edge p1="0" p2="1"                                              │
│            width="10"                                                 │
│            height="96"                                                │
│            id="Outline"                                               │
│            holegroupid="door_pattern_123"/>  ← DOOR REFERENCE         │
│    </list>                                                            │
│  </simplelayout>                                                      │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                         Wrap in <scene> tag
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                 API CALL #2: GetMesh()                                  │
│                 Entry: API.APIEntry("getmesh", xml)                    │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│               APILIB: GetMesh.Perform()                                 │
│               → Template2Mesh / BasicShapeList2Mesh                     │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                  SHAPELIB: 3D GEOMETRY GENERATION                       │
│                                                                         │
│  For each edge in SimpleLayout:                                        │
│    1. Create Panel geometry                                            │
│       • Width × Height rectangle                                       │
│       • Position at vertex coordinates                                 │
│       • Orient along edge direction (From → To)                        │
│                                                                         │
│    2. If HoleGroupID is set:                                           │
│       • Look up hole pattern (door/window definition)                  │
│       • Apply boolean subtraction (CSG)                                │
│       • Cut door/window opening in panel                               │
│                                                                         │
│    3. Generate mesh data:                                              │
│       • Vertices (3D coordinates)                                      │
│       • Normals (surface directions)                                   │
│       • Triangles (face definitions)                                   │
│       • UV coordinates (texture mapping)                               │
│                                                                         │
│  Combine all panel meshes into single scene                            │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                       FBX EXPORT                                        │
│                       (via FBXHelper/FBXWrapper.dll)                    │
│                                                                         │
│  Write to specified output file:                                       │
│    • Mesh geometry (vertices, normals, triangles)                      │
│    • Material assignments                                              │
│    • Transform hierarchies                                             │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
                            ┌───────────────┐
                            │  OUTPUT.FBX   │
                            │  (3D Model)   │
                            └───────────────┘
```

## Key Decision Points

### 🔍 Room Detection (Step 7)
- **Input**: PEdge lists (outline, open areas, walls)
- **Algorithm**: Clockwise edge traversal forming closed loops
- **Output**: List of rooms (each = closed PEdge loop)
- **Failure**: Walls don't form closed regions → No rooms detected

### 🚪 Door Candidate Marking (GetFloorLayout)
- **Rule 1**: Edge touching open area → `InteriorDoorCandidate = 1`
- **Rule 2**: Edge on outline → `ExteriorWindowCandidate = 1`
- **Rule 3**: Edge connecting back room to front room → `InteriorDoorCandidate = 1`
- **Failure**: Incorrect edge IDs → No candidates marked

### 🎯 Door Selection (RandomlySelectDoors)
- **Input**: FloorMaker with door candidates marked
- **Algorithm**: Recursive room traversal from destination rooms (open areas)
- **Action**: Set `edge.HoleGroupID` for selected door locations
- **Failure**: No pathways found → No HoleGroupIDs assigned

### 🕳️ Hole Pattern Application (Mesh Generation)
- **Input**: SimpleLayout edges with `holegroupid` attribute
- **Lookup**: Find hole pattern definition (door/window shape)
- **Operation**: Boolean subtraction (CSG) to cut opening
- **Failure**: HoleGroupID not found → Solid wall generated (no door)

## Data Flow Summary

| Phase | Input Format | Process | Output Format |
|-------|-------------|---------|---------------|
| Drawing | Edit2D objects | User interaction | Canvas state |
| Template | Canvas state | LoadFloorLayoutInputFromEdit | FloorLayoutInput XML |
| API #1 | FloorLayoutInput XML | RoomGenWrapper (7 steps) | FloorMaker XML |
| Door Selection | FloorMaker XML | RandomlySelectDoors | FloorMaker XML (with HoleGroupIDs) |
| Compile | FloorMaker | Compile() | SimpleLayout XML |
| API #2 | SimpleLayout XML | ShapeLib + FBXHelper | FBX file |

## Critical Checkpoints for Missing Doors

✅ **Checkpoint 1**: After Step 7 - Do we have rooms?
- Check: `RoomList.Count > 0`

✅ **Checkpoint 2**: After GetFloorLayout - Are door candidates marked?
- Check: `EdgeList` has edges with `InteriorDoorCandidate = 1`

✅ **Checkpoint 3**: After RandomlySelectDoors - Are doors assigned?
- Check: `EdgeList` has edges with `HoleGroupID != ""`

✅ **Checkpoint 4**: After Compile - Are door references preserved?
- Check: SimpleLayout XML has `<edge holegroupid="..."/>`

✅ **Checkpoint 5**: During mesh generation - Are holes applied?
- Check: FBX has door openings (not solid walls)
