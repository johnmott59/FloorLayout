using System.Collections.Generic;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        public void LoadFromJSON(Edge[] EdgeArray, Vertex[] VertexArray)
        {
            
            VertexList = new List<Vertex>();
            for (int i=0; i < VertexArray.Length; i++)
            {
                VertexList.Add(VertexArray[i]);
            }

            EdgeList = new List<Edge>();
            for (int i = 0; i < EdgeArray.Length; i++)
            {
                EdgeList.Add(EdgeArray[i]);
            }
        }
    }
}
