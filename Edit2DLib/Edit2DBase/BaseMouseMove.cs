using System.Drawing;
using System.Diagnostics;

namespace Edit2DLib
{
    public partial class Edit2DBase
    {
        /// <summary>
        /// The base mouse move is called to handle mouse movement. It remembers the previous positions and then calls
        /// the override handler.
        /// </summary>
        /// <param name="ScreenMouseX"></param>
        /// <param name="ScreenMouseY"></param>
        /// <returns></returns>
        public bool BaseMouseMove(int ScreenMouseX, int ScreenMouseY)
        {
            // Compute the screen delta x and delta y based on last mouse position
            int ScreenDeltaX = ScreenMouseX - LastScreenMousePosition.X;
            int ScreenDeltaY = ScreenMouseY - LastScreenMousePosition.Y;

            // Update the last mouse position
            LastScreenMousePosition = new Point(ScreenMouseX, ScreenMouseY);

            if (MouseState == eMouseState.MouseDown)
            {
                MouseState = eMouseState.MouseMovedWhileDown;
            }

            /*
             * Our action takes place if the mouse is down while its being moved.
             */
           
            if (MouseState != eMouseState.MouseMovedWhileDown) return false;

            if (MouseMove(MouseState, ScreenMouseX, ScreenMouseY, ScreenDeltaX, ScreenDeltaY))
            {
                Debug.WriteLine("Mouse Drag at " + System.DateTime.Now.ToLongTimeString());
                return true;
            }

            // Draw set didn't change things, we can scroll

            int WorldDeltaX = (int)((float)ScreenDeltaX * CurrentZoom);
            int WorldDeltaY = (int)((float)ScreenDeltaY * CurrentZoom);

            WorldPointScrollX -= WorldDeltaX;
            WorldPointScrollY -= WorldDeltaY;

            // After a scroll we need to repaint

            return true;
        }

    }
}
