using System.Drawing;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        public void AddEllipse(int ScreenOffsetX, int ScreenOffsetY, int Width, int Height)
        {
            if (MostRecentlySelectedHoleGroup == null) return;

            PointF WorldOffset = this.S2W(ScreenOffsetX, ScreenOffsetY);

            LayoutHole oHole = new LayoutHole();
            oHole.HoleType = "ell";
            oHole.OffsetX = WorldOffset.X;
            oHole.OffsetY = WorldOffset.Y;
            /*
             * Save this as the current and most recently selected
             * The most recently selected is used for commands after the mouse is up
             */
            CurrentlySelectedHole = oHole;
            MostRecentlySelectedHole = oHole;
            /*
             * The index is the at the end of the current list. This means we will have to manage the index values
             * when they are deleted
             */          
            oHole.HoleTypeIndex = BoundaryEllipseList.Count;
            AddHoleToHoleGroup(MostRecentlySelectedHoleGroup, oHole);
            /*
             * Now create and add the ellipse
             */
            BoundaryEllipse ell = new BoundaryEllipse();
            ell.Width = Width;
            ell.Height = Height;

            BoundaryEllipseList.Add(ell);

            // trigger redraw

            DrawShapes();
        }

        public void AddHoleToHoleList(HoleGroup hg, Hole oNewHole)
        {

        }


    }
}
