using System.Drawing;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {

        //--------------------------------------------------------------------------
        /*
         * A Polygon can be selected in any of these ways:
         * 1. A vertex can be selected, which makes it the active vertex
         * 2. An edge can be selected, which makes it the active edge
         * 3. The move handle can be selected. The move handle is generated as the average of the points of the polygon
         */
        private bool TrySelectPolygon(LayoutHole oHole, int ScreenMouseX, int ScreenMouseY)
        {
            PointF MoveHandle = new PointF(0, 0);
            float dx;
            float dy;


            BoundaryPolygon oPolygon = BoundaryPolygonList.GetFrom(oHole.HoleTypeIndex);

            PointF ScreenOffset = this.W2S(oHole.OffsetX, oHole.OffsetY);
            /*
             * See if this is one of the vertices
             */
            for (int i=0; i < oPolygon.PointList.Length; i++)
            {
                Point3D p = oPolygon.PointList[i];
                dx = p.X / CurrentZoom;
                dy = p.Y / CurrentZoom;

                PointF Target = new PointF(ScreenOffset.X + dx, ScreenOffset.Y + dy);

                if (IsHitOnTarget(ScreenMouseX, ScreenMouseY, Target))
                {
                    CurrentlySelectedHole = oHole;
                    // Most recently selected is used for commands that happen after the mouse is up
                    MostRecentlySelectedHole = oHole;
                    CurrentlySelectedHandleType = eHandleType.VertexHandle;

                    CurrentlySelectedPolygonVertexIndex = i;
                    // The most recently used is for commands that happen after the mouse is up
                    MostRecentlySelectedPolygonVertexIndex = i;

                    return true;
                }
            }
            /*
             * See if this is one of the edges
             */
            for (int i = 0; i < oPolygon.PointList.Length; i++)
            {
                int FromIndex = i;
                int ToIndex = i + 1;
                if (ToIndex == oPolygon.PointList.Length) ToIndex = 0;

                Point3D FromPoint = oPolygon.PointList[FromIndex];

                dx = FromPoint.X / CurrentZoom;
                dy = FromPoint.Y / CurrentZoom;

                PointF pFrom = new PointF(ScreenOffset.X + dx, ScreenOffset.Y + dy);

                Point3D ToPoint = oPolygon.PointList[ToIndex];

                dx = ToPoint.X / CurrentZoom;
                dy = ToPoint.Y / CurrentZoom;

                PointF pTo = new PointF(ScreenOffset.X + dx, ScreenOffset.Y + dy);

                PointF Target = EdgeCenter(pFrom, pTo);

                if (IsHitOnTarget(ScreenMouseX, ScreenMouseY, Target))
                {
                    CurrentlySelectedHole = oHole;
                    // Most recently selected is used for commands that happen after the mouse is up
                    MostRecentlySelectedHole = oHole;
                    CurrentlySelectedHandleType = eHandleType.EdgeHandle;

                    CurrentlySelectedPolygonEdgeIndex = i;
                    // The most recently used is for commands that happen after the mouse is up
                    MostRecentlySelectedPolygonEdgeIndex = i;

                    return true;
                }
            }
            /*
             * See if this is the move handle at the center of the polygon
             */
            PointF pCenter = PolygonCenter(oPolygon);
            dx = pCenter.X / CurrentZoom;
            dy = pCenter.Y / CurrentZoom;
            pCenter = new PointF(ScreenOffset.X + dx, ScreenOffset.Y + dy);

            if (IsHitOnTarget(ScreenMouseX,ScreenMouseY,pCenter))
            {
                CurrentlySelectedHole = oHole;
                // Most recently selected is used for commands that happen after the mouse is up
                MostRecentlySelectedHole = oHole;
                CurrentlySelectedHandleType = eHandleType.MoveHandle;
                return true;
            }


            return false;
        }

        private PointF PolygonCenter(BoundaryPolygon oPolygon)
        {
            float x = 0;
            float y = 0;
            for (int i=0; i < oPolygon.PointList.Length; i++)
            {
                Point3D p = oPolygon.PointList[i];
                x += p.X;
                y += p.Y;
            }

            PointF oCenter = new PointF(x / oPolygon.PointList.Length, y / oPolygon.PointList.Length);

            return oCenter;

        }

     }
}
