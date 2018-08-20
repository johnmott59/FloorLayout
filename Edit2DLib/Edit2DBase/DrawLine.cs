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

        public virtual void DrawLine(string color, float lineWidth, PointF ScreenFrom, PointF ScreenTo) {
#if DOTNET
            // try to call the draw primitive

            DrawPrimitives?.DrawLine(color, lineWidth, ScreenFrom, ScreenTo);
            
            // normal GDI
            Pen p = new Pen(Brushes.Blue, lineWidth);
            oGraphics?.DrawLine(p, ScreenFrom, ScreenTo);
            

#else
            this.context.save();

            this.context.lineWidth = lineWidth;
            this.context.beginPath();

            this.context.strokeStyle = color;

            this.context.moveTo(ScreenFrom.X, ScreenFrom.Y);
            this.context.lineTo(ScreenTo.X, ScreenTo.Y);

            this.context.closePath();
            this.context.stroke();

            this.context.restore();
#endif
        }

    }
}
