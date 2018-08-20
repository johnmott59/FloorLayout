namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        // Flip p1 and p2, effectively changing what direction is considered the 'front' for purposes of placing holes

        public void FlipMostRecentlySelectedEdge()
        {
            if (MostRecentlySelectedLayer == null) return;

            MostRecentlySelectedLayer.FlipMostRecentlySelectedEdge();
#if false
            int tmp = MostRecentlySelectedEdge.p1;
            MostRecentlySelectedEdge.p1 = MostRecentlySelectedEdge.p2;
            MostRecentlySelectedEdge.p2 = tmp;
#endif

            DrawShapes();

        }
      
    }
}
