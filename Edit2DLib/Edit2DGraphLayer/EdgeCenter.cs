using System.Drawing;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        public PointF EdgeCenter(Edge oEdge)
        {
            PointF P1 = EdgeP1(oEdge);
            PointF P2 = EdgeP2(oEdge);

            float dx = P1.X - P2.X;
            float dy = P1.Y - P2.Y;
            /*
             * The connection point for this edge is the normal
             */
            return new PointF((dx) / 2 + P2.X, (dy) / 2 + P2.Y);
        }
    }
}
