#define DOTNET
using System.Drawing;

namespace Edit2DLib
{
    /*
     * This object contains entry points for views to draw items. Its initialized and provided to the draw routine.
     * It will be overridden in Javascript and in .net with code particular to those environments
     */
    public partial class Edit2DBase
    {
        public void DrawCircle(string color, float Radius, float CenterX, float CenterY)
        {
#if DOTNET
            Pen p = GetColor(color, 1);
            RectangleF rf = new RectangleF(CenterX - Radius, CenterY - Radius, Radius, Radius);
            oGraphics?.DrawEllipse(p, rf);
            DrawPrimitives?.DrawEllipse(color, CenterX, CenterY, Radius);
#else
            this.context.save();

            this.context.lineWidth = 1;
            this.context.beginPath();

            this.context.strokeStyle = color;

            this.context.arc(CenterX, CenterY, Radius, 0, 2 * Math.PI);

            this.context.closePath();
            this.context.stroke();

            this.context.restore();
#endif
        }

    }
}
