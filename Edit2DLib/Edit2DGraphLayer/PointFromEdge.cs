using System.Drawing;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        public PointF EdgeP1(Edge oEdge)
        {
            for (int i=0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == oEdge.p1)
                {
                    return new PointF(v.X, v.Y);
                }
            }

            return new PointF(0, 0);
        }

        public PointF EdgeP2(Edge oEdge)
        {
            for (int i = 0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == oEdge.p2)
                {
                    return new PointF(v.X, v.Y);
                }
            }

            return new PointF(0, 0);
        }

    }
}
