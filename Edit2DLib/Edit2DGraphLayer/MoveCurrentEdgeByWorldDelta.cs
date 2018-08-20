using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        public eOperationStatus MoveCurrentEdgeByWorldDelta(int WorldDeltaX, int WorldDeltaY)
        {
            if (CurrentlySelectedEdge == null) return eOperationStatus.NoEdgeSelected;
            /*
             * update the vertices associated with this edge. This will automatically move all edges associated with it
             */

            Vertex P1Vertex = FindVertexFromIndex(CurrentlySelectedEdge.p1);
            P1Vertex.X += WorldDeltaX;
            P1Vertex.Y += WorldDeltaY;


            Vertex P2Vertex = FindVertexFromIndex(CurrentlySelectedEdge.p2);
            P2Vertex.X += WorldDeltaX;
            P2Vertex.Y += WorldDeltaY;

            return eOperationStatus.OK;
        }
    }
}
