using System.Drawing;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        public PointF GetPointFromIndex(int index)
        {
            for (int i = 0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == index)
                {
                    return new PointF(v.X, v.Y);
                }
            }
            return new PointF(0, 0);
        }

    }
}
