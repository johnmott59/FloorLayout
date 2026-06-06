# FloorLayout

WPF application for designing floor layouts (walls, doors, open areas) with interactive 2D editing and 3D mesh generation.

## Overview

FloorLayout provides a graphical editor for creating architectural floor plans that can be compiled into 3D geometry. It uses a layered approach for defining building elements:

- **Outline Areas**: Define the exterior boundaries of rooms/buildings
- **Open Areas**: Define interior open spaces (courtyards, atriums)
- **Walls**: Draw wall segments to subdivide spaces

The application generates both 2D floor layout descriptions and 3D meshes (FBX format) via integrated template compilation.

## Architecture

### Components

- **FloorLayout**: WPF application with MVVM architecture
- **Edit2DLib**: 2D drawing and editing library with mouse interaction
  - `Edit2DBase`: Core canvas, coordinate transform, drawing primitives
  - `Edit2DGraph`: Graph-based edge/vertex editing with layers
  - `Edit2DHoleGroup`: Hole pattern editing (rectangles, ellipses, polygons)
  - `Edit2DFloorLayoutInput`: Floor-specific editing orchestration

### Integration

This version is integrated with the DirectX shape generation ecosystem:

- **ShapeTemplateLib**: Provides parametric shape templates and XML serialization (project reference)
- **APILib**: Handles template compilation and FBX export (project reference)

Previous versions used ShapeTemplateLib v1.6.0 as a NuGet package and called a web API for mesh generation. This version has direct in-process integration for better performance and debugging.

## Features

- Interactive 2D drawing with pan/zoom
- Snap to grid (configurable)
- Multi-layer editing (outline, open areas, walls)
- Hole pattern support (windows, doors, openings)
- Real-time coordinate display
- Direct compilation to FloorLayout template XML
- 3D mesh generation via APILib

## Requirements

- .NET Framework 4.8
- Visual Studio 2015 or later
- ShapeTemplateLib (project reference)
- APILib (project reference)

## Project History

**2018**: Initial public release as standalone application with NuGet dependencies

**2026**: Integrated into DirectX shape generation system with:
- Direct APILib integration (removed web API dependency)
- Project references to ShapeTemplateLib and APILib
- Enhanced debugging and error handling
- Updated to work with modern template compilation pipeline

## Edit2DLib Cross-Platform Design

Edit2DLib was originally designed to be cross-platform between .NET and JavaScript for HTML5 canvas rendering. This .NET version is a specialized branch focused on WPF/Windows integration. The JavaScript version exists separately for web-based floor layout tools.

## Development

Built with Visual Studio 2015+. The solution includes:
- `FloorLayout.sln`: Main solution
- `FloorLayout/FloorLayout.csproj`: WPF application
- `Edit2DLib/Edit2DLib.csproj`: 2D editing library

## Related Projects

Part of the DirectX parametric 3D shape generation system. See the main DirectX repository for:
- ShapeTemplateLib: High-level parametric shape templates
- APILib: Template compilation and export orchestration
- ShapeLib: Low-level geometry generation

## License

See LICENSE file for details.
