#define DOTNET

namespace Edit2DLib
{
    /*
     * This object contains entry points for views to draw items. Its initialized and provided to the draw routine.
     * It will be overridden in Javascript and in .net with code particular to those environments
     */
    public partial class Edit2DBase
    {
        // There is a W3C ellipse function for the context but it doesn't compile, not sure of browser support
        public void DrawEllipse(string color, float Width, float Height, float CenterX, float CenterY)
        {
            // Don't allow it to become negative or 0
            if (Width < 0) Width = 1;
            if (Height < 0) Height = 1;
#if DOTNET
#else
        this.context.save();
        this.context.beginPath();
       
        this.context.translate(CenterX, CenterY);
        var scalex = 1;
        var scaley = 1;
        if (Width > Height) {
            scalex = Width / Height;
        }
        if (Height > Width) {
            scaley = Height / Width;
        }
        this.context.scale(scalex, scaley);
       
        this.context.arc(0, 0, Math.min(Width, Height), 0, 2 * Math.PI, false);
            
        // we call restore before calling stroke so that the line drawing of the ellipse isn't scaled, only the dimensions
        this.context.restore();
        this.context.lineWidth = 1;
        this.context.strokeStyle = color;
        this.context.stroke();
#endif
        }

    }
}
