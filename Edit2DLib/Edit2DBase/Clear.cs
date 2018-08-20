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
        public void Clear(string color)
        {
            // This control might not need to clear the screen
            if (SubControl == 0) return;
#if DOTNET
            if (oGraphics == null) return;
            oGraphics.Clear(Color.White);

#else
            this.context.fillStyle = color;
            this.context.fillRect(0, 0,
                this.context.canvas.clientWidth,
                this.context.canvas.clientHeight);
#endif
        }
    }
}
