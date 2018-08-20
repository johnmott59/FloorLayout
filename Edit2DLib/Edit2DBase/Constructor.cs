using System.Drawing;

namespace Edit2DLib
{
    public partial class Edit2DBase 
    {
        public Edit2DBase()
        {

            LastScreenMousePosition = new Point(0, 0);
            MouseState = eMouseState.Nothing;

            WorldPointScrollX = 0;
            WorldPointScrollY = 0;

            SubControl = 0;

            CurrentZoom = 1;

            GridSize = 1;
        }
    }
}
