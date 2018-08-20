namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public eOperationStatus SplitMostRecentlySelectedEdge(float RelativeDistance)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            eOperationStatus sts = MostRecentlySelectedLayer.SplitMostRecentlySelectedEdge(RelativeDistance);

            if (sts != eOperationStatus.OK) return sts;

 
            // trigger redraw
            DrawShapes();

            return eOperationStatus.OK;
        }

    }
}
