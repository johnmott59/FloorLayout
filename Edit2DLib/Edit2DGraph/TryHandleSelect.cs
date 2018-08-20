using System.Drawing;
using System;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public bool TryHandleSelect(int ScreenMouseX, int ScreenMouseY)
        {
            if (MostRecentlySelectedLayer == null) return false;

            for (int i=0; i < MostRecentlySelectedLayer.VertexList.Count; i++)
            {
                Vertex v = MostRecentlySelectedLayer.VertexList.GetFrom(i);

                PointF ScreenCoordinates = this.W2S(v.X, v.Y);

                double distance = Math.Sqrt(
                    (ScreenMouseX - ScreenCoordinates.X) * (ScreenMouseX - ScreenCoordinates.X) +
                    (ScreenMouseY - ScreenCoordinates.Y) * (ScreenMouseY - ScreenCoordinates.Y));

                if (distance < HandleSize)
                {
                    // For purposes of dragging the handle we want to know the currently selected handle. However, its 
                    // also useful to remember the most recently selected handle for purposes of operations

                    MostRecentlySelectedLayer.CurrentlySelectedVertex = v;
                    MostRecentlySelectedLayer.MostRecentlySelectedVertex = v;
                    return true;
                }
            }

            return false;   // no handle selected
        }
    }
}
