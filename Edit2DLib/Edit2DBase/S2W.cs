using System.Drawing;

namespace Edit2DLib
{
    public partial class Edit2DBase
    {

        public PointF S2W(float ScreenX, float ScreenY)
        {
            PointF WorldUpperLeft = GetUpperLeftWorld();

            // bias the screen points according to the zoom and add to the upper left corner

            return new PointF(WorldUpperLeft.X + ScreenX * CurrentZoom, WorldUpperLeft.Y + ScreenY * CurrentZoom);

        }

     
        
    }
}
