using System.Drawing;
using ShapeTemplateLib;

namespace Edit2DLib
{

    public partial class Edit2DHoleGroup
    {

        private void UpdateRectangle(int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            BoundaryRectangle oRect = BoundaryRectangleList.GetFrom(CurrentlySelectedHole.HoleTypeIndex);
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
                case eHandleType.MoveHandle:

                    CurrentlySelectedHole.OffsetX = newx;
                    CurrentlySelectedHole.OffsetY = newy;
                    break;
                case eHandleType.ResizeHandle:
                    // Get the hole struct and change the size
                    switch (CurrentlySelectedHole.HoleType)
                    {
                        case "rect":
                            oRect.Width += ScreenDeltaX * CurrentZoom;
                            oRect.Height += ScreenDeltaY * CurrentZoom;
                            break;
                    }
                    break;
            }
        }



    }
}
