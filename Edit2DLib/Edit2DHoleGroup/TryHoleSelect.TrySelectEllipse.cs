using System.Drawing;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        //----------------------------------------------------------------------------
        private bool TrySelectEllipse(LayoutHole oHole,int ScreenMouseX, int ScreenMouseY)
        {
            PointF MoveHandle = new PointF(0, 0);
            PointF ResizeHandle = new PointF(0, 0);

            BoundaryEllipse oEllipse = BoundaryEllipseList.GetFrom(oHole.HoleTypeIndex);

            // The move handle  is the center
            MoveHandle = this.W2S(oHole.OffsetX, oHole.OffsetY);
            if (IsHitOnTarget(ScreenMouseX, ScreenMouseY, MoveHandle))
            {
                CurrentlySelectedHole = oHole;
                MostRecentlySelectedHole = oHole;
                CurrentlySelectedHandleType = eHandleType.MoveHandle;
                return true;
            }

            // The resize handle is the lower right corner
            ResizeHandle = this.W2S(oHole.OffsetX + oEllipse.Width / 2, oHole.OffsetY + oEllipse.Height / 2);
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
