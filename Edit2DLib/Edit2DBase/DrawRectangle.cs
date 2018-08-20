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
        public void DrawRectangle(string color, float width,float height, float CenterX, float CenterY)
        {
#if DOTNET
            Pen p = new Pen(Brushes.Black, 1);
            float upperleftX = CenterX - width / 2;
            float upperleftY = CenterY - height / 2;
            oGraphics?.DrawRectangle(p, upperleftX, upperleftY, width, height);
            DrawPrimitives?.DrawRectangle(color, width, height, CenterX, CenterY);
#else
            this.context.save();

            this.context.lineWidth = 1;
            this.context.beginPath();

            this.context.strokeStyle = color;

            this.context.strokeRect(CenterX - width/2, CenterY - height/2, width, height);

            this.context.closePath();
            this.context.stroke();

            this.context.restore();
#endif
        }

    }
}
