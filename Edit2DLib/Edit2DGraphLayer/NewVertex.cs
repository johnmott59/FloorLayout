using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        protected Vertex NewVertex(float X, float Y)
        {
            Vertex vNew = new Vertex();
            vNew.Index = FindNextIndex();
            vNew.X = X;
            vNew.Y = Y;

            VertexList.Add(vNew);

            return vNew;
        }
      
    }
}
