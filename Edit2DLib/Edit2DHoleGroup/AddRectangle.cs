using System.Drawing;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {

        public void AddRectangle(int ScreenOffsetX, int ScreenOffsetY, int Width, int Height)
        {
            if (MostRecentlySelectedHoleGroup == null) return;

            PointF WorldOffset = this.S2W(ScreenOffsetX, ScreenOffsetY);

            LayoutHole oHole = new LayoutHole();
            oHole.HoleType = "rect";
            oHole.OffsetX = WorldOffset.X;
            oHole.OffsetY = WorldOffset.Y;
            /*
             * Save this as the current and most recently selected
             */
            CurrentlySelectedHole = oHole;
            MostRecentlySelectedHole = oHole;
            /*
             * The index is the at the end of the current list. This means we will have to manage the index values
             * when they are deleted
             */          
            oHole.HoleTypeIndex = BoundaryRectangleList.Count;

            AddHoleToHoleGroup(MostRecentlySelectedHoleGroup, oHole);

            /*
             * Now create and add the rectangle
             */
            BoundaryRectangle rect = new BoundaryRectangle();
            rect.Width = Width;
            rect.Height = Height;

            BoundaryRectangleList.Add(rect);

            // trigger draw shapes
            DrawShapes();
        }


    }
}
