using System.Drawing;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        public eOperationStatus SplitMostRecentlySelectedEdge(float RelativeDistance)
        {
            if (EdgeList.Count == 0)
            {
                return eOperationStatus.NoEdgesDefined;
            }

            if (MostRecentlySelectedEdge == null)
            {
                return eOperationStatus.NoEdgeSelected;
            }

            Vertex v1 = FindVertexFromIndex(MostRecentlySelectedEdge.p1);
            Vertex v2 = FindVertexFromIndex(MostRecentlySelectedEdge.p2);

            SVector2 f = new SVector2(v1.X, v1.Y); 
            SVector2 t = new SVector2(v2.X, v2.Y);

            SVector2 interpolatedVector = SVector2.Interpolate(f, t, RelativeDistance);
            /*
             * Get the point at the at this distance mark
             */
            PointF InterpolatedPoint = new PointF(interpolatedVector.X, interpolatedVector.Y);

            // Add the interpolated point as a new vertex
            Vertex vInterpolated = new Vertex();
            vInterpolated.Index = FindNextIndex();
            vInterpolated.X = InterpolatedPoint.X;
            vInterpolated.Y = InterpolatedPoint.Y;

            VertexList.Add(vInterpolated);
            
            // Create a new edge with two points, the interpolated point and the current p2

            Edge newEdge = new Edge();
            newEdge.Width = MostRecentlySelectedEdge.Width;
            newEdge.Height = MostRecentlySelectedEdge.Height;
            newEdge.ID = "";
            newEdge.HoleGroupID = "";
            newEdge.p1 = vInterpolated.Index;
            newEdge.p2 = MostRecentlySelectedEdge.p2;

            EdgeList.Add(newEdge);

            // update the current edge, set its 'p2' point to the interpolated point. This will trim it
            // Also, clear out any holes, the split edge requires a reset

            MostRecentlySelectedEdge.p2 = vInterpolated.Index;
            MostRecentlySelectedEdge.HoleGroupID = "";

            return eOperationStatus.OK;
        }

    }
}
