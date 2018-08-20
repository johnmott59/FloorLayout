#define DOTNET

using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        public void DeleteCurrentHole()
        {
            if (MostRecentlySelectedHole == null) return;

            int IndexToDelete = -1;

            switch (MostRecentlySelectedHole.HoleType)
            {
                case "ell":
                    // Delete this index
                    // Find all references to hole values greater than this one and reduce them by one so the indices are correct
                    IndexToDelete = -1;

                    for (int i = 0; i < GetHoleListLength(MostRecentlySelectedHoleGroup.HoleList); i++)
                    {
                        LayoutHole oHole = MostRecentlySelectedHoleGroup.HoleList[i];

                        if (MostRecentlySelectedHole == oHole) IndexToDelete = i;

                        if (oHole.HoleType == "ell" && oHole.HoleTypeIndex > MostRecentlySelectedHole.HoleTypeIndex)
                        {
                            oHole.HoleTypeIndex--;
                        }
                    }

                    // Delete this hole
                    RemoveHoleFromHoleGroup(MostRecentlySelectedHoleGroup, IndexToDelete);

                    // Delete this shape
                    BoundaryEllipseList.RemoveAt(MostRecentlySelectedHole.HoleTypeIndex);
                    break;
                case "rect":
                    // Delete this index
                    // Find all references to hole values greater than this one and reduce them by one so the indices are correct
                    IndexToDelete = -1;
                    for (int i=0; i < GetHoleListLength(MostRecentlySelectedHoleGroup.HoleList); i++)
                    {
                        LayoutHole oHole = MostRecentlySelectedHoleGroup.HoleList[i];

                        if (MostRecentlySelectedHole == oHole) IndexToDelete = i;

                        if (oHole.HoleType == "rect" && oHole.HoleTypeIndex > MostRecentlySelectedHole.HoleTypeIndex)
                        {
                            oHole.HoleTypeIndex--;
                        }
                    }

                    // Delete this hole
                    RemoveHoleFromHoleGroup(MostRecentlySelectedHoleGroup, IndexToDelete);

                    // Delete this shape
                    BoundaryRectangleList.RemoveAt(MostRecentlySelectedHole.HoleTypeIndex);
                    break;
                case "poly":

                    // Find all references to hole values greater than this one and reduce them by one so the indices are correct
                    IndexToDelete = -1;

                    for (int i = 0; i < GetHoleListLength(MostRecentlySelectedHoleGroup.HoleList); i++)
                    {
                        LayoutHole oHole = MostRecentlySelectedHoleGroup.HoleList[i];

                        if (MostRecentlySelectedHole == oHole) IndexToDelete = i;

                        if (oHole.HoleType == "poly" && oHole.HoleTypeIndex > MostRecentlySelectedHole.HoleTypeIndex)
                        {
                            oHole.HoleTypeIndex--;
                        }
                    }

                    // Delete this hole
                    RemoveHoleFromHoleGroup(MostRecentlySelectedHoleGroup, IndexToDelete);

                    // Delete this shape
                    BoundaryPolygonList.RemoveAt(MostRecentlySelectedHole.HoleTypeIndex);
                    break;
            }

            // trigger a repaint
            DrawShapes();
        }


    }
}
