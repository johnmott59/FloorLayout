using System.Drawing;

namespace Edit2DLib
{
    public partial class Edit2DBase
    {
        protected void DrawGrid()
        {
            if (this.GridSize == 1) return;

            if (this.SubControl == 1) return;

            int clientWidth = PictureBoxWidth;
            int clientHeight = PictureBoxHeight;
            int GridSize =this.GridSize;

            PointF ScreenCenter = new PointF(clientWidth / 2, clientHeight / 2);
            PointF WorldCenter = this.S2W(ScreenCenter.X, ScreenCenter.Y);

            // Adjust the world coordinates to the grid size
            WorldCenter.X = RoundToGrid((int)WorldCenter.X);
            WorldCenter.Y = RoundToGrid((int)WorldCenter.Y);

            int HorizontalLines = clientHeight / this.GridSize / 2;
            int HorizontalWidth = clientWidth /  2;

            for (int i = -HorizontalLines; i < HorizontalLines; i++)
            {
                float Y = i * GridSize + WorldCenter.Y;
                PointF From = new PointF(-HorizontalWidth + WorldCenter.X, Y);
                PointF To = new PointF(HorizontalWidth + WorldCenter.X, Y);
                DrawLine("rgba(30,30,30,.25)",(float).25, this.W2S(From.X, From.Y), this.W2S(To.X, To.Y));
            }

            int VerticalLines = clientWidth / GridSize / 2;
            int VerticalWidth = clientHeight / 2;

            for (int i = -VerticalLines; i < VerticalLines; i++)
            {
                float X = i * GridSize + WorldCenter.X;
                PointF From = new PointF(X, -VerticalWidth + WorldCenter.Y);
                PointF To = new PointF(X, VerticalWidth + WorldCenter.Y);
                DrawLine("rgba(30,30,30,.25)", (float).25, this.W2S(From.X, From.Y), this.W2S(To.X, To.Y));
            }
        }
    }
}
