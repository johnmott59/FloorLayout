#define DOTNET
using System.Collections.Generic;
using ShapeTemplateLib;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
       
        public void RemoveCurrentPolygonVertex()
        {
            if (MostRecentlySelectedHole == null) return;

            if (MostRecentlySelectedHole.HoleType != "poly") return;

            if (MostRecentlySelectedPolygonVertexIndex == -1) return;

            BoundaryPolygon oPolygon = BoundaryPolygonList.GetFrom(MostRecentlySelectedHole.HoleTypeIndex);

            // Don't reduce to less than 3 points
            if (oPolygon.PointList.Length == 3) return;

            /*
             * Remove this point and clear the most recently selected edge
             */
#if DOTNET
            List<Point3D> tmp = new List<Point3D>(oPolygon.PointList);
            tmp.RemoveAt(MostRecentlySelectedPolygonVertexIndex);
            oPolygon.PointList = tmp.ToArray();
#else
            oPolygon.PointList.splice(this.MostRecentlySelectedPolygonVertexIndex, 1);
#endif

            MostRecentlySelectedPolygonVertexIndex = -1;

            // trigger a redraw

            DrawShapes();

          }

    }
}
