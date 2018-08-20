namespace Edit2DLib
{
    public partial class Edit2DGraph
    {

        override public bool MouseMove(eMouseState MouseState, int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            if (MostRecentlySelectedLayer == null) return false;

            if (MostRecentlySelectedLayer.CurrentlySelectedVertex != null)
            {
                UpdateCurrentHandleToScreenPoint(ScreenMouseX, ScreenMouseY);
                return true;
            }

            if (MostRecentlySelectedLayer.CurrentlySelectedEdge != null)
            {
                MoveCurrentEdgeByScreenDelta(ScreenDeltaX, ScreenDeltaY);
                return true;
            }


            return false;
        }


    }
}
