namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        override public void MouseUp(int ScreenMouseX, int ScreenMouseY)
        {
            if (MostRecentlySelectedLayer == null) return;

            // If we were dragging a vertex around see if we can merge it with an existing close vertex
            if (AllowVertexMerge == 1)
            {
                TryMergeVertex(ScreenMouseX, ScreenMouseY);
            }
            /*
             * Reset the current handle and current edge
             */
            MostRecentlySelectedLayer.CurrentlySelectedVertex = null;

            MostRecentlySelectedLayer.CurrentlySelectedEdge = null;
        }

    }
}
