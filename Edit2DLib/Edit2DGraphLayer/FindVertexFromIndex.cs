using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        public Vertex FindVertexFromIndex(int index)
        {
            for (int i=0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == index) return v;
            }

            return null;
        }
    }
}
