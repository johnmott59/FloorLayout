using System.Drawing;
using System;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        /// <summary>
        /// Find the shape that's closest to the mouse X,Y
        /// </summary>
        /// <param name="ScreenMouseX"></param>
        /// <param name="ScreenMouseY"></param>
        /// <returns></returns>
        /// 
        protected Edge FindEdgeFromMouse (int ScreenMouseX, int ScreenMouseY)
        {
            if (MostRecentlySelectedLayer == null) return null;

            for (int i=0; i < MostRecentlySelectedLayer.EdgeList.Count; i++)
            {
                Edge pe = MostRecentlySelectedLayer.EdgeList.GetFrom(i);

                // Find the center point of the edge

                PointF Center = MostRecentlySelectedLayer.EdgeCenter(pe);

                PointF CenterScreen = this.W2S(Center.X, Center.Y);

                double distance = Math.Sqrt(
                                 (ScreenMouseX - CenterScreen.X) * (ScreenMouseX - CenterScreen.X) +
                                 (ScreenMouseY - CenterScreen.Y) * (ScreenMouseY - CenterScreen.Y));

                if (distance < 10)
                {
                    return pe;
                }
            }

            return null;
        }
    }
}
