namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public eOperationStatus DeleteCurrentEdge()
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            eOperationStatus sts = MostRecentlySelectedLayer.DeleteCurrentEdge();
            if (sts != eOperationStatus.OK) return sts;


            // Trigger redraw

            DrawShapes();

            return eOperationStatus.OK;
        }

    }
}
