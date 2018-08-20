namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public eOperationStatus AppendEdge(int Width, int Height,int DeltaX, int DeltaY)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            eOperationStatus sts = MostRecentlySelectedLayer.AppendEdge(Width, Height, DeltaX, DeltaY);
            if (sts != eOperationStatus.OK) return sts;
#if false
            if (MostRecentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            // Add a new point
            int nextindex = FindNextIndex();

            Vertex vP = new Vertex();
            vP.Index = nextindex;
            vP.X = MostRecentlySelectedVertex.X + DeltaX;
            vP.Y = MostRecentlySelectedVertex.Y + DeltaY;
            VertexList.Add(vP);

            // Add an edge 

            Edge oEdge = new Edge();
            oEdge.Height = Height;
            oEdge.Width = Width;
            oEdge.p1 = MostRecentlySelectedVertex.Index;
            oEdge.p2 = vP.Index;
            oEdge.ID = "";
            oEdge.HoleGroupID = "";

            EdgeList.Add(oEdge);
#endif

            // Redraw shapes
            DrawShapes();

            return eOperationStatus.OK;
        }
    }
}
