namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        /*
         * Sub classes will override these methods to implement mouse operations on their canvas
         */
       override  public eMouseDownCapture MouseDown(int ScreenMouseX, int ScreenMouseY)
        {
            /*
              * Try to select a vertex handle first, then an edge handle. The order actually isn't important,
             * only that we try to select one or the other
             */
            if (TryHandleSelect(ScreenMouseX, ScreenMouseY)) return eMouseDownCapture.VertexHandle;

            // See if we selected an edge handle
            if (TryEdgeSelect(ScreenMouseX, ScreenMouseY)) return eMouseDownCapture.EdgeHandle;

            return eMouseDownCapture.Nothing;
        }

   
    }
}
