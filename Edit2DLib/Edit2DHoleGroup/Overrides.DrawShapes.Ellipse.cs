using System.Drawing;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
 
        //-------------------------------------------------------
        private void DrawShapes_Ellipse(LayoutHole oHole, bool IsCurrentHole)
        {
            BoundaryEllipse oEllipse = BoundaryEllipseList.GetFrom(oHole.HoleTypeIndex);

            PointF Screen = this.W2S(oHole.OffsetX, oHole.OffsetY);

            float SWidth = oEllipse.Width / CurrentZoom;
            float SHeight = oEllipse.Height / CurrentZoom;

            string color = IsCurrentHole ? "#0000FF" : "rgba(255,0,0,.5)";
            // Draw the move handle
            this.DrawCircle(color, 10, Screen.X, Screen.Y);

            // draw the actual ellipse
            this.DrawEllipse(ShapeStrokeColor, SWidth, SHeight, Screen.X, Screen.Y);
          
            // draw the resize handle
            this.DrawRectangle("rgba(0,255,0,.5)", 10, 10, Screen.X + SWidth / 2, Screen.Y + SHeight / 2);
        }


    }
}
