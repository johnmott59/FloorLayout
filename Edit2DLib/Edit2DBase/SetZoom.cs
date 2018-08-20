namespace Edit2DLib
{
    public partial class Edit2DBase
    {
        /*
         * When the zoom level changes we have to adjust the scroll to compensate.
         * to align the centers of the screen shift the world
         */
        public void SetZoom(float NewZoom)
        {
            CurrentZoom = NewZoom;
        }
    }
}
