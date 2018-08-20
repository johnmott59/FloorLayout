namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        /*
         * Sub classes will override these methods to implement mouse operations on their canvas
         */
       override  public eMouseDownCapture MouseDown(int ScreenMouseX, int ScreenMouseY)
        {
            if (TryHoleSelect(ScreenMouseX, ScreenMouseY)) return eMouseDownCapture.VertexHandle;

            return eMouseDownCapture.Nothing;
        }

        override public bool MouseMove(eMouseState MouseState, int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            if (CurrentlySelectedHole != null)
            {
                UpdateCurrentHole(ScreenMouseX, ScreenMouseY, ScreenDeltaX, ScreenDeltaY);
                return true;
            }
            return false;
        }

        override public void MouseUp(int ScreenMouseX, int ScreenMouseY)
        {
            /*
             * Reset the currently selected items. Most recently selected remain active for any commands
             */
            CurrentlySelectedHole = null;
            CurrentlySelectedPolygonEdgeIndex = -1;
            CurrentlySelectedPolygonVertexIndex = -1;
        }
    }
}
