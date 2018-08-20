namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public eOperationStatus DeleteSelectedVertex()
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            // If there is not a most recently selected handle return

            eOperationStatus sts = MostRecentlySelectedLayer.DeleteSelectedVertex();

            if (sts != eOperationStatus.OK) return sts;

#if false
            if (MostRecentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            /*
             * This vertex must have exactly two different edges coming into it
             */
            int ReferenceCount = 0;
            List<Edge> ConnectingEdgeList = new List<Edge>();

            for (int i=0; i < EdgeList.Count; i++)
            {
                Edge oEdge = EdgeList.GetFrom(i);
                if (oEdge.p1 == MostRecentlySelectedVertex.Index)
                {
                    ReferenceCount++;
                    ConnectingEdgeList.Add(oEdge);
                }
                if (oEdge.p2 == MostRecentlySelectedVertex.Index)
                {
                    ReferenceCount++;
                    ConnectingEdgeList.Add(oEdge);
                }
            }

            if (ReferenceCount != 2) return eOperationStatus.MustBeTwoConnectingEdgesForOperation;

            // Get the from and to points of the remade edge
            int NewP1 = -1;
            int NewP2 = -1;

            Edge oEdge0 = ConnectingEdgeList.GetFrom(0);
            if (oEdge0.p1 == MostRecentlySelectedVertex.Index)
            {
                NewP1 = oEdge0.p2;
            } else
            {
                NewP1 = oEdge0.p1;
            }

            Edge oEdge1 = ConnectingEdgeList.GetFrom(1);
            if (oEdge1.p2 == MostRecentlySelectedVertex.Index)
            {
                NewP2 = oEdge1.p1;
            }
            else
            {
                NewP2 = oEdge1.p2;
            }

            // Replace the p1 and p2 in the 0th edge and delete the 1th edge. We know that the current handle
            // will be redundant so we can delete it.

            oEdge0.p1 = NewP1;
            oEdge0.p2 = NewP2;

            // delete the current handle, it the one we're removing 
            for (int i=0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == MostRecentlySelectedVertex.Index)
                {
                    VertexList.RemoveAt(i);
                    break;
                }
            }

            // Delete the 1th edge. 

            for (int i=0; i < EdgeList.Count; i++)
            {
                Edge e = EdgeList.GetFrom(i);
                if (e == oEdge1)
                {
                    EdgeList.RemoveAt(i);
                    break;
                }
            }
#endif

            // trigger redraw
            DrawShapes();

            return eOperationStatus.OK;

        }
    }
}
