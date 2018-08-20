using System.Collections.Generic;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public List<Edit2DGraphLayer> Edit2dGraphLayerList { get; set; }
        public Edit2DGraphLayer MostRecentlySelectedLayer { get; set; }

        // These values will scale the drawings as they are rendered. Horizontal in this case means X-Y plane, where Z is Vertical
        public float HorizontalScale { get; set; }
        public float VerticalScale { get; set; }

        // This is global for all layers to allow a preview
        public bool bShowHandles { get; set; }

        public int HandleSize { get; set; } = 20;

        // If desired the editor can merge two vertices that are moved over one another. This is the default
        // behavior but it can be useful to have use the graph and not merge vertices.
        public int AllowVertexMerge { get; set; } = 1;

    }
}
