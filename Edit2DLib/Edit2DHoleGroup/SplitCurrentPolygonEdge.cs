#define DOTNET
using System.Collections.Generic;
using System.Drawing;
using ShapeTemplateLib;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        // If the cur
        public void SplitCurrentPolygonEdge()
        {
            if (MostRecentlySelectedHole == null) return;

            if (MostRecentlySelectedHole.HoleType != "poly") return;

            if (MostRecentlySelectedPolygonEdgeIndex == -1) return;

            BoundaryPolygon oPolygon = BoundaryPolygonList.GetFrom(MostRecentlySelectedHole.HoleTypeIndex);
            /*
             * Locate this edge and create a new point in between the surrounding points
             */
            int FromIndex = MostRecentlySelectedPolygonEdgeIndex;
            int ToIndex = FromIndex + 1;

            if (ToIndex == oPolygon.PointList.Length) ToIndex = 0;

            Point3D FromPoint = oPolygon.PointList[FromIndex];
            Point3D ToPoint = oPolygon.PointList[ToIndex];

            PointF pF = new PointF(FromPoint.X, FromPoint.Y);
            PointF pT = new PointF(ToPoint.X, ToPoint.Y);

            PointF oCenter = EdgeCenter(pF, pT);

            Point3D pNew = new Point3D();
            pNew.X = oCenter.X;
            pNew.Y = oCenter.Y;
#if DOTNET
            List<Point3D> tmp = new List<Point3D>(oPolygon.PointList);
            tmp.Insert(FromIndex + 1, pNew);
            oPolygon.PointList = tmp.ToArray();
#else
            oPolygon.PointList.splice(FromIndex + 1,0,pNew);
#endif



            // trigger repaint
            DrawShapes();
        }

    }
}
