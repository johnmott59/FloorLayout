namespace Edit2DLib
{
    public partial class Edit2DGraph
    {

        public eOperationStatus UnMergeMostRecentlySelectedHandle()
        {

            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            eOperationStatus sts = MostRecentlySelectedLayer.UnMergeMostRecentlySelectedHandle();

            if (sts != eOperationStatus.OK) return sts;

            // trigger a redraw
            DrawShapes();

            return eOperationStatus.OK;

        }
           
    }
}
