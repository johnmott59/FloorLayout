using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        public void DuplicateCurrentHole()
        {
            if (MostRecentlySelectedHole == null) return;

            LayoutHole oHole = LayoutHole.CopyFrom(MostRecentlySelectedHole);

            // Add at an offset so it will be easy to see
            oHole.OffsetX += 10;
            oHole.OffsetY += 10;

            // Create a new version of the instance of the hole

            int ndx = MostRecentlySelectedHole.HoleTypeIndex;
            switch (MostRecentlySelectedHole.HoleType)
            {
                case "ell":
                    oHole.HoleTypeIndex = BoundaryEllipseList.Count;
                    BoundaryEllipse e = BoundaryEllipseList.GetFrom(ndx);
                    BoundaryEllipseList.Add(BoundaryEllipse.CopyFrom(e));
                    break;
                case "rect":
                    oHole.HoleTypeIndex = BoundaryRectangleList.Count;
                    BoundaryRectangle r = BoundaryRectangleList.GetFrom(ndx);
                    BoundaryRectangleList.Add(BoundaryRectangle.CopyFrom(r));
                    break;
                case "poly":
                    oHole.HoleTypeIndex = BoundaryPolygonList.Count;
                    BoundaryPolygon p = BoundaryPolygonList.GetFrom(ndx);
                    BoundaryPolygonList.Add(BoundaryPolygon.CopyFrom(p));
                    break;
            }

            // Add the new hole to the hole group
            AddHoleToHoleGroup(MostRecentlySelectedHoleGroup, oHole);

            // trigger a repaint
            DrawShapes();
        }

    }
}
