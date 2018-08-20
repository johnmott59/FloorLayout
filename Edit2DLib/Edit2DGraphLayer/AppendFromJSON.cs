using ShapeTemplateLib.BasicShapes;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        public void AppendFromJSON(Edge[] EdgeArray, Vertex[] VertexArray)
        {
            // We need to adjust the vertex index values so they don't overlap with the current vertices
            int NewBase = 0;
            for (int i = 0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index > NewBase)
                {
                    NewBase = v.Index;
                }
            }

            // Set new base. Add this value to the vertex and edge vertex indeces
            NewBase++;

            /*
             * Add the new points and vertices to this layer, adjusting point indices
             */
            for (int i=0; i < VertexArray.Length; i++)
            {
                // Use 'copyfrom' because the vertexarray[] isn't a full object, its only properties in JSON
                Vertex v = Vertex.CopyFrom(VertexArray[i]);
                v.Index += NewBase;

                VertexList.Add(v);
            }

            for (int i = 0; i < EdgeArray.Length; i++)
            {
                // Use 'copyfrom' because the EdgeArray[] isn't a full object, its only properties in JSON
                Edge e = Edge.CopyFrom(EdgeArray[i]);
                e.p1 += NewBase;
                e.p2 += NewBase;
                e.HoleGroupID = "";
                e.ID = "";

                EdgeList.Add(e);
            }
        }
    }
}
