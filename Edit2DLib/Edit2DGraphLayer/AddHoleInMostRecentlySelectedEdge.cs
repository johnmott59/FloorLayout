using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        public eOperationStatus AddHoleInMostRecentlySelectedEdge(int Width)
        {

            if (EdgeList.Count == 0)
            {
                return eOperationStatus.NoEdgesDefined;
            }

            if (MostRecentlySelectedEdge == null)
            {
                return eOperationStatus.NoEdgeSelected;
            }

            double thisEdgeLength = EdgeLength(MostRecentlySelectedEdge);

            if (Width >= thisEdgeLength) return eOperationStatus.EdgeNotWideEnoughForOperation;

            // Determine the interpolations based on the width and length

            float WidthPercent = (float)(Width / thisEdgeLength);

            Vertex v1 = FindVertexFromIndex(MostRecentlySelectedEdge.p1);
            Vertex v2 = FindVertexFromIndex(MostRecentlySelectedEdge.p2);
            /*
             * Create vectors of the end points
             */
            SVector2 f = new SVector2(v1.X, v1.Y);
            SVector2 t = new SVector2(v2.X, v2.Y);

            // Find a new end point and add a vertex for it
            float lowpercent = (float) .5 - WidthPercent;
            SVector2 LowPartVector = SVector2.Interpolate(f, t, lowpercent);
            Vertex vNew1 = NewVertex(LowPartVector.X, LowPartVector.Y);

            // Create a new edge and add it
            Edge newEdge = new Edge();
            newEdge.Width = MostRecentlySelectedEdge.Width;
            newEdge.Height = MostRecentlySelectedEdge.Height;
            newEdge.ID = "";
            newEdge.HoleGroupID = "";
            newEdge.p1 = MostRecentlySelectedEdge.p1;
            newEdge.p2 = vNew1.Index;
            EdgeList.Add(newEdge);

            // Add a new vertex for the other position on the other side of the hole
            float highpercent =  (float) .5 + WidthPercent;
            SVector2 HighPartVector = SVector2.Interpolate(f, t, highpercent);
            Vertex vNew2 = NewVertex(HighPartVector.X, HighPartVector.Y);

            // Modify the currently selected edge so that it starts at the vnew2 

            MostRecentlySelectedEdge.p1 = vNew2.Index;

            return eOperationStatus.OK;
        }
    }
}
