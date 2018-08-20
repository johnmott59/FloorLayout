using System.Drawing;
using ShapeTemplateLib;

namespace Edit2DLib
{

    public partial class Edit2DHoleGroup
    {

        //------------------------------------------------------------------------------------------------
        protected void UpdatePolygon(int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            BoundaryPolygon oPolygon = BoundaryPolygonList.GetFrom(CurrentlySelectedHole.HoleTypeIndex);
            PointF pWorld = this.S2W(ScreenMouseX, ScreenMouseY);
            float newx = pWorld.X;
            float newy = pWorld.Y;

            // Update the world points to the nearest grid
            if (this.GridSize > 1)
            {
                newx = this.RoundToGrid((int)pWorld.X);
                newy = this.RoundToGrid((int)pWorld.Y);
            }

            switch (CurrentlySelectedHandleType)
            {
                case eHandleType.VertexHandle:
                    /*
                     * Retrieve this vertex and update 
                     */
                     Point3D p = oPolygon.PointList[CurrentlySelectedPolygonVertexIndex];

                    p.X += ScreenDeltaX * CurrentZoom;
                    p.Y += ScreenDeltaY * CurrentZoom;

                    break;
                case eHandleType.EdgeHandle:
                    /*
                     * The points associated with the edge are the ones at the value and after
                     */
                    int FromIndex = CurrentlySelectedPolygonEdgeIndex;
                    int ToIndex = FromIndex + 1;
                    if (ToIndex == oPolygon.PointList.Length) ToIndex = 0;

                    Point3D FromPoint = oPolygon.PointList[FromIndex];
                    Point3D ToPoint = oPolygon.PointList[ToIndex];

                    FromPoint.X += ScreenDeltaX * CurrentZoom;
                    FromPoint.Y += ScreenDeltaY * CurrentZoom;

                    ToPoint.X += ScreenDeltaX * CurrentZoom;
                    ToPoint.Y += ScreenDeltaY * CurrentZoom;
                    break;
                case eHandleType.MoveHandle:
                    /*
                     * Update the offset to move the whole shape
                     */
                    CurrentlySelectedHole.OffsetX = newx;
                    CurrentlySelectedHole.OffsetY = newy;
                    /*
                     * Update all points with the delta
                     */
#if false
                    for (int i=0; i < oPolygon.PointList.Length; i++)
                    {
                        Point3D pMove = oPolygon.PointList[i];
                        pMove.X += ScreenDeltaX;
                        pMove.Y += ScreenDeltaY;
                    }
#endif
                    break;
            }
        }


    }
}
