using System.Drawing;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        public PointF EdgeCenter(PointF From, PointF To)
        {
            float dx = From.X - To.X;
            float dy = From.Y - To.Y;
            /*
             * The connection point for this edge is the normal
             */
            return new PointF((dx) / 2 + To.X, (dy) / 2 + To.Y);
        }
    }
}
