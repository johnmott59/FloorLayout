using System.Drawing;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {

       
  
        //-------------------------------------------------------
        private void DrawShapes_Rectangle(LayoutHole oHole, bool IsCurrentHole)
        {
            BoundaryRectangle oRect = BoundaryRectangleList.GetFrom(oHole.HoleTypeIndex);

            PointF Screen = this.W2S(oHole.OffsetX, oHole.OffsetY);

            string color = IsCurrentHole ? "#0000FF" : "#ff0000";
            // Draw the move handle
            this.DrawCircle(color, HandleSize, Screen.X, Screen.Y);

            // Adjust the width and height for zoom
            float SWidth = oRect.Width / CurrentZoom;
            float SHeight = oRect.Height / CurrentZoom;

            // Draw the rectangle as a set of lines so we can control how its positioned. The offset is the lower left
            color = ShapeStrokeColor;

            // The offset is to the lower left
            PointF LowerLeft = Screen;

            PointF LowerRight = new PointF(LowerLeft.X + SWidth, LowerLeft.Y);
            this.DrawLine(color, 1, LowerLeft, LowerRight);

            PointF UpperRight = new PointF(LowerLeft.X + SWidth, LowerLeft.Y + SHeight);
            this.DrawLine(color, 1, LowerRight, UpperRight);

            PointF UpperLeft = new PointF(LowerLeft.X, LowerLeft.Y + SHeight);
            this.DrawLine(color, 1, UpperRight, UpperLeft);

            // Last line
            this.DrawLine(color, 1, UpperLeft, LowerLeft);

            // draw the resize handle
            this.DrawRectangle("#00FF00", HandleSize, HandleSize, Screen.X + SWidth , Screen.Y + SHeight - HandleSize);
        }


    }
}
