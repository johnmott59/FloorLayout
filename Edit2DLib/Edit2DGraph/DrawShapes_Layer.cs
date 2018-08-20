using System.Drawing;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public void DrawShapes_Layer(Edit2DGraphLayer oLayer,bool IsCurrentLayer, bool bShowHandles)
        {
            // Draw the edges
            for (int i = 0; i < oLayer.EdgeList.Count; i++)
            {
                // Get screen From and To
                Edge oEdge = oLayer.EdgeList.GetFrom(i);

                bool bIsCurrentEdge = oEdge == oLayer.MostRecentlySelectedEdge;

                PointF ScreenFrom = oLayer.GetPointFromIndex(oEdge.p1);
                ScreenFrom = this.W2S(ScreenFrom.X, ScreenFrom.Y);
                PointF ScreenTo = oLayer.GetPointFromIndex(oEdge.p2);
                ScreenTo = this.W2S(ScreenTo.X, ScreenTo.Y);

                // If this is the current layer draw the handles
                if (IsCurrentLayer && bShowHandles)
                {
                    // Get the center point of this edge
                    PointF WorldEdgeCenter = oLayer.EdgeCenter(oEdge);
                    PointF ScreenEdgeCenterFrom = this.W2S(WorldEdgeCenter.X, WorldEdgeCenter.Y);
                    // Get the normal. This points to the 'front'. The front of the panel will 
                    // be where holes are laid out, left to right, while facing it.
                    float dx = ScreenFrom.X - ScreenTo.X;
                    float dy = ScreenFrom.Y - ScreenTo.Y;
                    SVector2 normal = new SVector2(dy, -dx);
                    SVector2 n2 = SVector2.Normalize(normal);
                    SVector2 n3 = SVector2.Scale(n2, 20);

                    // Draw a line at this normal -dy,dx from the center
                    PointF ScreenEdgeCenterTo = new PointF(ScreenEdgeCenterFrom.X + n3.X, ScreenEdgeCenterFrom.Y + n3.Y);
                    this.DrawLine("#ff0000", 1, ScreenEdgeCenterFrom, ScreenEdgeCenterTo);


                    // Draw handles
                    string color = "rgba(255,0,0,.5)";
                    if (oLayer.MostRecentlySelectedVertex != null && oEdge.p1 == oLayer.MostRecentlySelectedVertex.Index)
                    {
                        color = "rgba(0,0,255,1)";
                    }
                    this.DrawCircle(color, HandleSize, ScreenFrom.X, ScreenFrom.Y);

                    color = "rgba(255,0,0,.5)";
                    if (oLayer.MostRecentlySelectedVertex != null && oEdge.p2 == oLayer.MostRecentlySelectedVertex.Index)
                    {
                        color = "rgba(0,0,255,1)";
                    }
                    this.DrawCircle(color, HandleSize, ScreenTo.X, ScreenTo.Y);

                    PointF oCenter = oLayer.EdgeCenter(oEdge);
                    oCenter = this.W2S(oCenter.X, oCenter.Y);

                    // Draw the handle for the edge
                    color = "rgba(0,255,0,.5)";
                    if (bIsCurrentEdge)
                    {
                        color = "rgba(0,0,255,1)";
                    }

                    // Draw the handle for the edge
                    this.DrawRectangle(color, HandleSize, HandleSize, oCenter.X, oCenter.Y - HandleSize);
                }

                // Call draw routine to draw line
                this.DrawLine("#0000FF", (float)0.5, ScreenFrom, ScreenTo);
            }
        }

    }
}
