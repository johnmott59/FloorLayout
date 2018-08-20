using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public bool TryEdgeSelect(int ScreenMouseX, int ScreenMouseY)
        {
            if (MostRecentlySelectedLayer == null) return false;

            Edge oEdge = FindEdgeFromMouse(ScreenMouseX, ScreenMouseY);
            if (oEdge != null)
            {
                // Select this edge, this is used for moving the edge while drawing
                MostRecentlySelectedLayer.CurrentlySelectedEdge = oEdge;

                // Remember this as the most recently selected edge, for purposes of commands
                MostRecentlySelectedLayer.MostRecentlySelectedEdge = oEdge;

                return true;
            }

            return false;
        }
    }
}
