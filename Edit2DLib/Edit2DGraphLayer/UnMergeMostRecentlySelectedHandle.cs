using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {

        public eOperationStatus UnMergeMostRecentlySelectedHandle()
        {
            if (MostRecentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            // Delete this index. We'll replace all occurrences with new vertices

            for (int i=0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == MostRecentlySelectedVertex.Index)
                {
                    VertexList.RemoveAt(i);
                    break;
                }
            }
            /*
             * Now create new handles for each instance of the just deleted vertex
             */
            for (int i=0; i < EdgeList.Count; i++)
            {
                Edge oEdge = EdgeList.GetFrom(i);
                if (oEdge.p1 == MostRecentlySelectedVertex.Index)
                {
                    Vertex v = NewVertex(MostRecentlySelectedVertex.X, MostRecentlySelectedVertex.Y);
                    oEdge.p1 = v.Index;
                }
                if (oEdge.p2 == MostRecentlySelectedVertex.Index)
                {
                    Vertex v = NewVertex(MostRecentlySelectedVertex.X, MostRecentlySelectedVertex.Y);
                    oEdge.p2 = v.Index;
                }

            }

            return eOperationStatus.OK;

        }
           
    }
}
