using System.Drawing;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        // Add a new line into the simple layout struct, new vertices and a new edge

        public void AddEdge(PointF WorldFrom, PointF WorldTo,int Width, int Height)
        {
            int nextindex = FindNextIndex();

            Vertex vP1 = new Vertex();
            vP1.Index = nextindex++;
            vP1.X = WorldFrom.X;
            vP1.Y = WorldFrom.Y;
            VertexList.Add(vP1);

            Vertex vP2 = new Vertex();
            vP2.Index = nextindex++;
            vP2.X = WorldTo.X;
            vP2.Y = WorldTo.Y;
            VertexList.Add(vP2);

            // Now add a segment

            Edge oEdge = new Edge();
            oEdge.Height = 30;
            oEdge.Width = 10;
            oEdge.p1 = vP1.Index;
            oEdge.p2 = vP2.Index;
            oEdge.ID = "";
            oEdge.HoleGroupID = "";

            EdgeList.Add(oEdge);

        }
    }
}
