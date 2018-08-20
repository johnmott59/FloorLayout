using System.Drawing;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {

        //------------------------------------------------------
        private void DrawShapes_Polygon(LayoutHole oHole, bool IsCurrentHole)
        {
            BoundaryPolygon bp = BoundaryPolygonList.GetFrom(oHole.HoleTypeIndex);

            PointF ScreenOffset = this.W2S(oHole.OffsetX, oHole.OffsetY);

            for (int i = 0; i < bp.PointList.Length; i++)
            {
                int FromIndex = i;
                int ToIndex = i + 1;
                if (ToIndex == bp.PointList.Length) ToIndex = 0; // Wrap at end

                Point3D FromPoint = bp.PointList[FromIndex];

                float fdx = FromPoint.X / CurrentZoom;
                float fdy = FromPoint.Y / CurrentZoom;

                Point3D ToPoint = bp.PointList[ToIndex];

                float tdx = ToPoint.X / CurrentZoom;
                float tdy = ToPoint.Y / CurrentZoom;

                PointF ScreenFrom = new PointF(ScreenOffset.X + fdx, ScreenOffset.Y + fdy);
                PointF ScreenTo = new PointF(ScreenOffset.X + tdx, ScreenOffset.Y + tdy);

                // Draw handles
                string color = "rgba(255,0,0,.5)"; 
                if (IsCurrentHole && i == MostRecentlySelectedPolygonVertexIndex)
                {
                    color = "#0000FF";
                }
                this.DrawCircle(color, 10, ScreenFrom.X, ScreenFrom.Y);

                PointF oCenter = EdgeCenter(ScreenFrom, ScreenTo);
                // Draw the handle for the edge
                color = "rgba(0,255,0,.5)";
                if (IsCurrentHole && i == MostRecentlySelectedPolygonEdgeIndex)
                {
                    color = "#0000FF";
                }
                this.DrawRectangle(color, 10, 10, oCenter.X, oCenter.Y);

                // Draw Line
                this.DrawLine(ShapeStrokeColor, (float)0.5, ScreenFrom, ScreenTo);

                // Draw the move handle
                PointF oMove = PolygonCenter(bp);
                float mdx = oMove.X / CurrentZoom;
                float mdy = oMove.Y / CurrentZoom;

                oMove = new PointF(ScreenOffset.X + mdx, ScreenOffset.Y + mdy);
                color = IsCurrentHole ? "#0000FF" : "rgba(255,0,0,.5)";

                // Draw the move handle
                this.DrawCircle(color, 10, oMove.X, oMove.Y);
            }
        }



    }
}
