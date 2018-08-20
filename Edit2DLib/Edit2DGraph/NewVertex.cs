using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        protected Vertex NewVertex(float X, float Y)
        {
#if false
            Vertex vNew = new Vertex();
            vNew.Index = FindNextIndex();
            vNew.X = X;
            vNew.Y = Y;

            VertexList.Add(vNew);

            return vNew;
#endif
            return new Vertex();
        }
      
    }
}
