using System.Drawing;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {



        //----------------------------------------------------------------------------
        private bool TrySelectRectangle(LayoutHole oHole,int ScreenMouseX, int ScreenMouseY)
        {
            PointF MoveHandle = new PointF(0, 0);
            PointF ResizeHandle = new PointF(0, 0);

            BoundaryRectangle oRect = BoundaryRectangleList.GetFrom(oHole.HoleTypeIndex);

            // The move handle  is the lower left
            MoveHandle = this.W2S(oHole.OffsetX, oHole.OffsetY);
            if (IsHitOnTarget(ScreenMouseX, ScreenMouseY, MoveHandle))
            {
                CurrentlySelectedHole = oHole;
                MostRecentlySelectedHole = oHole;
                CurrentlySelectedHandleType = eHandleType.MoveHandle;
                return true;
            }

            // The resize handle is the lower right corner
            // the position of the handle is based on the zoom

            float HandleOffsetX  = oRect.Width / this.CurrentZoom;
            float HandleOffsetY = oRect.Height / this.CurrentZoom;
            ResizeHandle = new PointF(MoveHandle.X + HandleOffsetX, MoveHandle.Y + HandleOffsetY);

            if (IsHitOnTarget(ScreenMouseX, ScreenMouseY, ResizeHandle))
            {
                CurrentlySelectedHole = oHole;
                MostRecentlySelectedHole = oHole;
                CurrentlySelectedHandleType = eHandleType.ResizeHandle;
                return true;
            }

            return false;
        }


 
    }
}
