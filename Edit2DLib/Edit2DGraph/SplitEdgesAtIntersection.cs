#define DOTNET

namespace Edit2DLib
{
    /*
     * Method to find intersections in the current edges and break them into new edges
     */
    public partial class Edit2DGraph
    {
        public void SplitEdgesAtIntersection() {

            if (MostRecentlySelectedLayer == null) return;

            MostRecentlySelectedLayer.SplitEdgesAtIntersection();

#if false
            // Set a tolerence of the thinnest edge. 

            bool ThereAreSplits = true;

            while (ThereAreSplits)
            {
                ThereAreSplits = false;

                for (int i = 0; i < EdgeList.Count && ThereAreSplits == false; i++)
                {
                    Edge S1 = EdgeList.GetFrom(i);
                    
                    for (int j = i + 1; j < EdgeList.Count && ThereAreSplits == false; j++)
                    {
                        Edge S2 = EdgeList.GetFrom(j);
                        PointF? p = FindInsideIntersectionPoint(S1, S2);

                        if (p != null)
                        {
                            // Add this as a new vertex
                            Vertex vNew = new Vertex();
#if DOTNET
                            vNew.X = p.Value.X;
                            vNew.Y = p.Value.Y;
#else
                            vNew.X = p.X;
                            vNew.Y = p.Y;
#endif
                            vNew.Index = FindNextIndex();
                            VertexList.Add(vNew);
                            // TBD make sure its not too close to an existing point

                            // Create two new segments with the intersection as the 'from' point. 

                            Edge pe1 = new Edge();
                            pe1.Width = S1.Width;
                            pe1.Height = S1.Height;
                            pe1.ID = "";                // new segment, remove any id
                            pe1.HoleGroupID = "";       // new segment don't try to bring holes over
                            pe1.p1 = vNew.Index;
                            pe1.p2 = S1.p2;
                            EdgeList.Add(pe1);

                            Edge pe2 = new Edge();
                            pe2.Width = S1.Width;
                            pe2.Height = S1.Height;
                            pe2.ID = "";                // new segment, remove any id
                            pe2.HoleGroupID = "";       // new segment don't try to bring holes over
                            pe2.p1 = vNew.Index;
                            pe2.p2 = S2.p2;
                            EdgeList.Add(pe2);

                            // Truncate the existing segments to the new point

                            S1.p2 = vNew.Index;
                            S2.p2 = vNew.Index;

                            // Indicate that there are splits so that the list can be reprocessed

                            ThereAreSplits = true;

                        }

                    }
                }

            }
#endif

            // Trigger redraw of shapes
            DrawShapes();
        }

    }    

}
