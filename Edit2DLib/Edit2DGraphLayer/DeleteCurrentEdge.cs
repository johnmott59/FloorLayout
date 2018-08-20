using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        public eOperationStatus DeleteCurrentEdge()
        {
            // delete the most recently selected edge, 
            Edge oEdgeToDelete = null;

            if (MostRecentlySelectedEdge == null)
            {
                return eOperationStatus.NoEdgeSelected;
            }
            int IndexOfEdgeToDelete = -1;
            for (int i=0; i < EdgeList.Count; i++)
            {
                oEdgeToDelete = EdgeList.GetFrom(i);

                if (oEdgeToDelete == MostRecentlySelectedEdge) { 
                    IndexOfEdgeToDelete = i;
                    break;
                }
            }

            if (IndexOfEdgeToDelete == -1) return eOperationStatus.NoEdgeSelected;
            
            /*
             * When deleting an edge we have to account for the vertices. Each of the vertices may be shared or not. 
             * If a vertex isn't shared we delete it as well
             */
            int p1ReferenceCount = 0;
            int p2ReferenceCount = 0;

            for (int i = 0; i < EdgeList.Count; i++)
            {
                Edge e = EdgeList.GetFrom(i);

                // Note that this will include the edge we are deleting. that's fine, we account for that

                if (e.p1 == oEdgeToDelete.p1) p1ReferenceCount++;
                if (e.p2 == oEdgeToDelete.p1) p1ReferenceCount++;

                if (e.p1 == oEdgeToDelete.p2) p2ReferenceCount++;
                if (e.p2 == oEdgeToDelete.p2) p2ReferenceCount++;
            }
            /*
             * If either of the reference counts are 1 that means that only the edge to be deleted contained it, 
             * and we can delete that vertex
             */
             if (p1ReferenceCount == 1)
            {
                for (int i=0; i < VertexList.Count; i++)
                {
                    Vertex v = VertexList.GetFrom(i);
                    if (v.Index == oEdgeToDelete.p1)
                    {
                        VertexList.RemoveAt(i);
                        break;
                    }
                }
            }

            if (p2ReferenceCount == 1)
            {
                for (int i = 0; i < VertexList.Count; i++)
                {
                    Vertex v = VertexList.GetFrom(i);
                    if (v.Index == oEdgeToDelete.p2)
                    {
                        VertexList.RemoveAt(i);
                        break;
                    }
                }
            }

            // Now delete the edge

            EdgeList.RemoveAt(IndexOfEdgeToDelete);

            // clear the most recently selected edge and vertex, in case it was on this edge
            MostRecentlySelectedEdge = null;
            MostRecentlySelectedVertex = null;
            CurrentlySelectedEdge = null;
            CurrentlySelectedVertex = null;

            return eOperationStatus.OK;

        }

    }
}
