using System.Collections.Generic;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        public List<Vertex> VertexList { get; set; }
        public List<Edge> EdgeList { get; set; }

        // While a handle is being moved this is the vertex associated with it
        public Vertex CurrentlySelectedVertex { get; set; } = null;

        // When a handle is selected we remember the most recent selection so that commands can be performed on it
        public Vertex MostRecentlySelectedVertex { get; set; } = null;

        // While an edge is being moved this is the index of that handle.
        public Edge CurrentlySelectedEdge = null;

        // When an edge is selected we remember the most recent selection so that commands can be performed on it
        public Edge MostRecentlySelectedEdge = null;

        // If this control is a sub control we will only manage shapes, not clear the screen or show the grid or axis
        public int SubControl { get; set; } = 0;

        public string ShapeStrokeColor { get; set; } = "#dddddd";
    }
}
