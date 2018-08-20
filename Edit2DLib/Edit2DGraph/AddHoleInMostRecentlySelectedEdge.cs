namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public eOperationStatus AddHoleInMostRecentlySelectedEdge(int Width)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            eOperationStatus sts = MostRecentlySelectedLayer.AddHoleInMostRecentlySelectedEdge(Width);

            if (sts != eOperationStatus.OK) return sts;
        
            // trigger redraw
            DrawShapes();

            return eOperationStatus.OK;
        }
    }
}
