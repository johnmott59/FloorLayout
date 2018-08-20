using System.Collections.Generic;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        public LayoutHole CurrentlySelectedHole { get; set; }
        public LayoutHole MostRecentlySelectedHole { get; set; }

        public eHandleType CurrentlySelectedHandleType { get; set; }

        public int CurrentlySelectedPolygonVertexIndex { get; set; }  // if this is a polygon, currently selected index
        public int MostRecentlySelectedPolygonVertexIndex { get; set; }

        public int CurrentlySelectedPolygonEdgeIndex { get; set; }  // if this is a polygon, currently selected edge
        public int MostRecentlySelectedPolygonEdgeIndex { get; set; }

        // This will manage a set of hole groups, of which there will be one that is active. All of the rectanges,
        // ellipses and polygons are in a list that the hole groups manage
        public List<HoleGroup> HoleGroupList { get; set; }

        public HoleGroup MostRecentlySelectedHoleGroup { get; set; }

        // The hole definitions are maintained in lists referenced by index. We do this this way so that this entire
        // structure can be serialized

        public List<BoundaryRectangle> BoundaryRectangleList { get; set; }
        public List<BoundaryEllipse> BoundaryEllipseList { get; set; }
        public List<BoundaryPolygon> BoundaryPolygonList { get; set; }

        // This shows holes over a specific edge, and we can show that edge as a shaded area if these values
        // are specified

        public float ShadeLength { get; set; }
        public float ShadeHeight { get; set; }

        public string ShapeStrokeColor { get; set; } = "#0000ff";

        public int HandleSize { get; set; } = 20;

    }
}
