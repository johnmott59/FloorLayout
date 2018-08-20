using System.Drawing;

namespace Edit2DLib
{
    public partial class Edit2DBase
    {
        /// <summary>
        /// Get the upper left world coordinate of the current picture box
        /// </summary>
        /// <returns></returns>
        protected PointF GetUpperLeftWorld()
        {
            // based on the zoom get the world coordinates coordinates of the upper left corner of the screen

            float divisor = 1;

            if (CurrentZoom == .125) divisor = 16;
            if (CurrentZoom == .25) divisor = 8;
            if (CurrentZoom == .5) divisor = 4;
            if (CurrentZoom == 1) divisor = 2;
            if (CurrentZoom == 2) divisor = 1;
            if (CurrentZoom == 4) divisor = (float).5;
            if (CurrentZoom == 8) divisor = (float).25;

            return new PointF(WorldPointScrollX - PictureBoxWidth / divisor, WorldPointScrollY - PictureBoxHeight / divisor);
        }



    }
}
