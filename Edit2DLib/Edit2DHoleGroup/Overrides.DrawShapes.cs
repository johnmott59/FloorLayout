#define DOTNET
using System.Drawing;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {

        // ---------------------------------------------------
        public override void DrawShapes()
        {

#if DOTNET
            // The model for .net is to invalidate and let the paint handler draw.
            if (oGraphics == null && DrawPrimitives == null) return;


#else
#endif
            bool bIsCurrentHole = false;
            if (SubControl == 0)
            {
                this.Clear("#FFFFFF");

                DrawGrid();

                DrawAxis();
            }

            /*
             * If specified draw in a shaded area that represents the current edge having holes designed for it
             */
            if (ShadeHeight > 0 && ShadeLength > 0)
            {
                PointF LowerLeft  = this.W2S(0, 0);
                PointF LowerRight = this.W2S(ShadeLength, 0);
                PointF UpperRight = this.W2S(ShadeLength, - ShadeHeight);
                PointF UpperLeft  = this.W2S(0, -ShadeHeight);

                this.DrawLine("rgba(100,100,150,.7", 1, LowerLeft, LowerRight);
                this.DrawLine("rgba(100,100,150,.7", 1, LowerRight, UpperRight);
                this.DrawLine("rgba(100,100,150,.7", 1, UpperRight, UpperLeft);
                this.DrawLine("rgba(100,100,150,.7", 1, UpperLeft, LowerLeft);
            }

            if (this.MostRecentlySelectedHoleGroup == null) return;

#if DOTNET
            int len = MostRecentlySelectedHoleGroup.HoleList.Length;
#else
            int len = MostRecentlySelectedHoleGroup.HoleList.length;
#endif

            for (int i=0; i < len; i++)
            {
                LayoutHole oHole = MostRecentlySelectedHoleGroup.HoleList[i];
                PointF Screen = this.W2S(oHole.OffsetX, oHole.OffsetY);

                bIsCurrentHole = oHole == MostRecentlySelectedHole;

                switch (oHole.HoleType)
                {
                    case "rect":

                        DrawShapes_Rectangle(oHole, bIsCurrentHole);
                        break;

                    case "poly":

                        DrawShapes_Polygon(oHole, bIsCurrentHole);
                        break;

                    case "ell":

                        DrawShapes_Ellipse(oHole, bIsCurrentHole);
                        break;
                }
            }
        }
   

  


    }
}
