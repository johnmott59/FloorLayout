using System.Drawing;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    /*
     * A single edge is defined by the points and whether or not its a door
     */
    public partial class Edit2DGraph
    {
        public PointF? FindInsideIntersectionPoint(Edge peGreen, Edge peBlue)
        {
            if (MostRecentlySelectedLayer == null) return null;

            return MostRecentlySelectedLayer.FindInsideIntersectionPoint(peGreen, peBlue);

#if false
            // We can see now if there is a common point. If there is a common point they don't intersect
            if (peGreen.p1 == peBlue.p1 || 
                peGreen.p1 == peBlue.p2 ||
                peGreen.p2 == peBlue.p1 ||
                peGreen.p2 == peBlue.p2)
            {
                // no intersection, there is a common point
                return null;
            }

            Vertex GreenFrom = FindVertexFromIndex(peGreen.p1);
            Vertex GreenTo = FindVertexFromIndex(peGreen.p2);

            Vertex BlueFrom = FindVertexFromIndex(peBlue.p1);
            Vertex BlueTo = FindVertexFromIndex(peBlue.p2);

            /*
             * Are they parallel?
             */

            float par = (float)((GreenTo.X - GreenFrom.X) * (BlueTo.Y - BlueFrom.Y) -
                           (GreenTo.Y - GreenFrom.Y) * (BlueTo.X - BlueFrom.X));

            if (par == 0)
            {
                return null;                               /* parallel lines */
            }
            /*
             * Find the proportional distance from one point to another
             */
            float tp = ((BlueFrom.X - GreenFrom.X) * (BlueTo.Y - BlueFrom.Y) - (BlueFrom.Y - GreenFrom.Y) * (BlueTo.X - BlueFrom.X)) / par;
            float tq = ((GreenTo.Y - GreenFrom.Y) * (BlueFrom.X - GreenFrom.X) - (GreenTo.X - GreenFrom.X) * (BlueFrom.Y - GreenFrom.Y)) / par;
            /*
             * If the distance isn't between 0 and 1 the segments don't intersect
             */
            if (tp < 0 || tp > 1 || tq < 0 || tq > 1)
            {
                return null;
            }

            return new PointF(GreenFrom.X + tp * (GreenTo.X - GreenFrom.X), GreenFrom.Y + tp * (GreenTo.Y - GreenFrom.Y));
#endif
        }
    }
}
