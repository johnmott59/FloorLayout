namespace Edit2DLib
{
    public partial class Edit2DFloorLayoutInput
    {
        /*
         * Sub classes will override these methods to implement mouse operations on their canvas
         */
       override  public eMouseDownCapture MouseDown(int ScreenMouseX, int ScreenMouseY)
        {
            /*
             * Try to capture something in the outline, then the open area, in turn
             */
            if (OutlineAreas.TryHoleSelect(ScreenMouseX, ScreenMouseY)) return eMouseDownCapture.VertexHandle;

            if (OpenAreas.TryHoleSelect(ScreenMouseX, ScreenMouseY)) return eMouseDownCapture.VertexHandle;

            eMouseDownCapture sts = Walls.MouseDown(ScreenMouseX, ScreenMouseY);

            return sts;

        }

        override public bool MouseMove(eMouseState MouseState, int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            /*
             * See if we are moving anything in the outline, then the open area
             */
            if (OutlineAreas.MouseMove(MouseState, ScreenMouseX, ScreenMouseY, ScreenDeltaX, ScreenDeltaY))
            {
                return true;
            }

            if (OpenAreas.MouseMove(MouseState, ScreenMouseX, ScreenMouseY, ScreenDeltaX, ScreenDeltaY))
            {
                return true;
            }

            if (Walls.MouseMove(MouseState,ScreenMouseX,ScreenMouseY,ScreenDeltaX,ScreenDeltaY))
            {
                return true;
            }

            /*
             * If we are here we scrolled the background. Update the dependent controls
             */
            int WorldDeltaX = (int)((float)ScreenDeltaX * CurrentZoom);
            int WorldDeltaY = (int)((float)ScreenDeltaY * CurrentZoom);

            OutlineAreas.WorldPointScrollX -= WorldDeltaX;
            OutlineAreas.WorldPointScrollY -= WorldDeltaY;

            OpenAreas.WorldPointScrollX -= WorldDeltaX;
            OpenAreas.WorldPointScrollY -= WorldDeltaY;

            Walls.WorldPointScrollX -= WorldDeltaX;
            Walls.WorldPointScrollY -= WorldDeltaY;

            return false;
        }

        override public void MouseUp(int ScreenMouseX, int ScreenMouseY)
        {
            /*
             * Reset the currently selected items. Most recently selected remain active for any commands
             */
            OutlineAreas.MouseUp(ScreenMouseX, ScreenMouseY);
            OpenAreas.MouseUp(ScreenMouseX, ScreenMouseY);
            Walls.MouseUp(ScreenMouseX, ScreenMouseY);
        }
    }
}
