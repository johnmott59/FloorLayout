using System.Drawing;
using System;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        /// <summary>
        /// Find the edge that's closest to the mouse X,Y
        /// </summary>
        /// <param name="ScreenMouseX"></param>
        /// <param name="ScreenMouseY"></param>
        /// <returns></returns>
        /// 
        public bool TryHoleSelect(int ScreenMouseX, int ScreenMouseY)
        {
            if (MostRecentlySelectedHoleGroup == null) return false;

            for (int i=0; i < GetHoleListLength(MostRecentlySelectedHoleGroup.HoleList); i++)
            {
                LayoutHole oHole = MostRecentlySelectedHoleGroup.HoleList[i];
                PointF MoveHandle = new PointF(0, 0);
                PointF ResizeHandle = new PointF(0, 0);

                switch (oHole.HoleType)
                {
                    case "rect":
                        if (TrySelectRectangle(oHole, ScreenMouseX, ScreenMouseY)) return true;
                        break;
                    case "poly":
                        if (TrySelectPolygon(oHole, ScreenMouseX, ScreenMouseY)) return true;
                        break;
                    case "ell":
                        if (TrySelectEllipse(oHole, ScreenMouseX, ScreenMouseY)) return true;
                        break;
                }
            }

            return false;
        }
 

        // See if there is a hit on the target point
        private bool IsHitOnTarget(int ScreenMouseX, int ScreenMouseY, PointF Target)
        {
            double distance = Math.Sqrt((ScreenMouseX - Target.X) * (ScreenMouseX - Target.X) +
                                                      (ScreenMouseY - Target.Y) * (ScreenMouseY - Target.Y));

            return distance < HandleSize;
        }
    }
}
