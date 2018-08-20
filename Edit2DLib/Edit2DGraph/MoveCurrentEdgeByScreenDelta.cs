namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public eOperationStatus MoveCurrentEdgeByScreenDelta(int ScreenDeltaX, int ScreenDeltaY)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            int WorldDeltaX = (int)((float)ScreenDeltaX * this.CurrentZoom);
            int WorldDeltaY = (int)((float)ScreenDeltaY * this.CurrentZoom);

            return MostRecentlySelectedLayer.MoveCurrentEdgeByWorldDelta(WorldDeltaX, WorldDeltaY);

        }
    }
}
