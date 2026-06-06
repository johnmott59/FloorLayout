# Debug Window Feature

## Overview

The FloorLayout application now includes a dedicated debug window that displays real-time debugging output during the room generation and mesh export process. This replaces the need to check Visual Studio's Debug Output window or external logging files.

## Features

### Real-Time Debug Output
- All debug messages stream to a dedicated window with dark theme (Visual Studio style)
- Automatic scrolling keeps the latest output visible
- Monospace font (Consolas) for easy reading of structured output
- Color-coded UI with status bar

### Toolbar Controls
- **Clear**: Clear all debug output
- **Save Log...**: Export debug output to a text file (.txt or .log)
- **Auto-scroll**: Toggle automatic scrolling to latest output
- **Line Counter**: Shows total number of debug lines

### Integration Points

The debug window is integrated throughout the pipeline:

1. **FloorLayout Application** - User interactions and XML generation
2. **APILib** - Room generation (RoomGenWrapper 7-step pipeline)
3. **Door Selection** - RandomlySelectDoors algorithm
4. **Mesh Generation** - FBX export process

## How to Use

### Opening the Debug Window

**Method 1**: Menu
- Click `View` → `Show Debug Window`

**Method 2**: Automatic
- The debug window automatically opens when you click the "Generate FBX" button

### Reading the Output

The debug output is organized with clear separators:

```
═══════════════ GENERATE START ═══════════════
[12:34:56.789] Generate button clicked
Inverting Y coordinates for room generation...

═══════════════ API CALL #1: GetFloorLayout ═══════════════
FloorLayoutInput XML:
  - Outline holes: 1
  - Open area holes: 1
  - Wall segments: 4

Calling API.APIEntry('getfloorlayout')...
API Status: SUCCESS
  Output file: C:\Users\...\tmp12345.tmp

═══════════════ ROOMGENWRAPPER 7-STEP PIPELINE ═══════════════
Step 1: Find intersections (outline ∩ open areas)
Step 2: Remove invalid open areas
Step 3: Create open area edges
Step 4: Split walls against open areas
Step 5: Eliminate walls inside outline
Step 6: Find wall intersections
Step 7: Detect rooms (trace clockwise closed loops)

RoomGenWrapper.AllSteps() complete:
  - Total rooms detected: 3

═══════════════ LOAD FLOORMAKER ═══════════════
Fixing XML element names for compatibility...
  - Renaming 'roomlist' to 'assembledroomlist'
  - Renaming 12 'fmedge' elements to 'fledge'
Calling FloorMaker.LoadProperties()...
LoadProperties result: SUCCESS

FloorMaker contains:
  - Vertices: 16
  - Edges: 12
  - Rooms: 3

Door/Window candidates:
  - Interior door candidates: 4
  - Exterior window candidates: 8

═══════════════ DOOR SELECTION ═══════════════
Calling RandomlySelectDoors()...
Doors assigned: 2

═══════════════ COMPILE TO SIMPLELAYOUT ═══════════════
Calling FloorMaker.Compile(5, 5)...

═══════════════ API CALL #2: GetMesh ═══════════════
...

═══════════════ GENERATE COMPLETE ═══════════════
[12:35:12.456] FBX saved to: C:\output\building.fbx
```

## Debug Output Sections

### GENERATE START
- Shows when Generate button was clicked
- Confirms Y-coordinate inversion for room generation

### API CALL #1: GetFloorLayout
- Displays FloorLayoutInput statistics (outlines, open areas, walls)
- Shows API call status and output file location
- Reports RoomGenWrapper results (vertices, edges, rooms)
- **Key checkpoint**: Check if rooms > 0

### ROOMGENWRAPPER 7-STEP PIPELINE
- Shows progress through each of the 7 room detection steps
- Reports final room count
- **Warning messages** if no rooms detected with troubleshooting hints

### LOAD FLOORMAKER
- Shows XML element name fixes for compatibility
- Reports LoadProperties result
- Displays FloorMaker contents (vertices, edges, rooms)
- **Key checkpoint**: Check door/window candidate counts

### DOOR SELECTION
- Shows RandomlySelectDoors execution
- Reports number of doors assigned
- **Key checkpoint**: Check if doors assigned > 0

### COMPILE TO SIMPLELAYOUT
- Shows FloorMaker → SimpleLayout compilation
- Applies scale factors (5, 5)

### API CALL #2: GetMesh
- Shows mesh generation and FBX export

### GENERATE COMPLETE
- Final confirmation with output file path

## Troubleshooting Guide

### Problem: No Rooms Detected

**Symptom**:
```
RoomGenWrapper.AllSteps() complete:
  - Total rooms detected: 0

WARNING: No rooms detected! Possible issues:
  - Walls don't form closed loops
  - Outline/open areas incorrectly defined
```

**Solutions**:
1. Ensure walls form complete closed loops
2. Check that outline area is drawn
3. Verify walls intersect properly (use snap-to-grid)

### Problem: No Door Candidates

**Symptom**:
```
Door/Window candidates:
  - Interior door candidates: 0
  - Exterior window candidates: 0
```

**Solutions**:
1. Check that open areas are defined (creates interior door candidates)
2. Verify outline is drawn correctly (creates window candidates)
3. Ensure edges have correct IDs ("Outline", "OpenArea", "Wall")

### Problem: Doors Assigned = 0

**Symptom**:
```
Calling RandomlySelectDoors()...
Doors assigned: 0
```

**Solutions**:
1. No door candidates available (see above)
2. RandomlySelectDoors couldn't find pathways between rooms
3. All rooms are already "connected" to open areas

### Problem: FBX Has Solid Walls (No Door Openings)

**Symptom**: Doors were assigned but FBX shows solid walls

**Solutions**:
1. Check that HoleGroupID is preserved in SimpleLayout XML
2. Verify mesh generation applies hole patterns
3. Ensure door/window pattern library is loaded

## API Integration

### From FloorLayout Application

```csharp
// Show debug window and setup APILib callback
DebugWindow.ShowWindow();
APILib.DebugLogger.WriteLineCallback = DebugWindow.WriteLine;
```

This redirects all APILib debug output to the FloorLayout debug window.

### APILib Debug Logger

APILib uses `DebugLogger` class instead of `System.Diagnostics.Debug`:

```csharp
// In APILib code
DebugLogger.WriteLine("Step 1: Find intersections");
DebugLogger.WriteSeparator("SECTION TITLE");
```

When the callback is set, output goes to the debug window. Otherwise, it falls back to `System.Diagnostics.Debug.WriteLine`.

## Saving Debug Logs

Click **Save Log...** button to export the complete debug output:
- Default filename: `FloorLayout_Debug_YYYYMMDD_HHMMSS.txt`
- Formats: .txt, .log, or all files
- Useful for sharing with support or keeping records

## Code Architecture

### Files Added

**FloorLayout Project:**
- `DebugWindow.xaml` - XAML layout for debug window
- `DebugWindow.xaml.cs` - Debug window implementation (singleton)
- `WindowCanvas.xaml` - Added "View" menu with "Show Debug Window" item

**APILib Project:**
- `Utilities/DebugLogger.cs` - Debug output abstraction with callback support

### Key Classes

**`DebugWindow`** (FloorLayout)
- Singleton pattern for single debug window instance
- Thread-safe (dispatches to UI thread automatically)
- Static methods: `WriteLine()`, `WriteSeparator()`, `Clear()`, `Show()`
- Window hides instead of closing (preserves output)

**`DebugLogger`** (APILib)
- Static class for debug output
- `WriteLineCallback` property for external redirection
- Falls back to `System.Diagnostics.Debug` if no callback set

### Usage Pattern

```csharp
// FloorLayout: Setup callback
DebugWindow.ShowWindow();
APILib.DebugLogger.WriteLineCallback = DebugWindow.WriteLine;

// FloorLayout: Write debug output
DebugWindow.WriteLine("Message");
DebugWindow.WriteSeparator("SECTION");
DebugWindow.WriteLineWithTime("Timestamped message");

// APILib: Write debug output (goes to callback if set)
DebugLogger.WriteLine("Message from APILib");
DebugLogger.WriteSeparator("APILIB SECTION");
```

## Future Enhancements

Possible improvements:
- Color coding for different message types (info, warning, error)
- Filtering options (show only errors/warnings)
- Search functionality
- Export to structured formats (JSON, XML)
- Real-time performance metrics
- Pause/resume output streaming
- Syntax highlighting for XML output
