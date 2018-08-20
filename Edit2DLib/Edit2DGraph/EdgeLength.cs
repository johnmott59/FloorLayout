using System;
using System.Drawing;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public double EdgeLength(Edge oEdge)
        {
            PointF P1 = EdgeP1(oEdge);
            PointF P2 = EdgeP2(oEdge);

            float dx = P1.X - P2.X;
            float dy = P1.Y - P2.Y;

            return Math.Sqrt((dx * dx + dy * dy));

        }
    }
}
