namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public void SelectEdgeByHoleGroupID(string HoleGroupID)
        {
            if (MostRecentlySelectedLayer == null) return;

            MostRecentlySelectedLayer.SelectEdgeByHoleGroupID(HoleGroupID);

        }
      
    }
}
