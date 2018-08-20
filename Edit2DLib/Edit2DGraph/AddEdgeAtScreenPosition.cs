using System.Drawing;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public eOperationStatus AddEdgeAtScreenPosition(int OffsetX, int OffsetY, int ScreenLength, int Width, int Height)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            PointF p = new PointF(OffsetX, OffsetY);

            var WorldCenter = this.S2W(p.X,p.Y);
            float Scale = this.CurrentZoom * ScreenLength;

            PointF WorldFrom = new PointF(WorldCenter.X - Scale, WorldCenter.Y - Scale);
            PointF WorldTo = new PointF(WorldCenter.X + Scale, WorldCenter.Y + Scale);

            return AddEdge(WorldFrom, WorldTo, Width, Height);

        }
    }
}
