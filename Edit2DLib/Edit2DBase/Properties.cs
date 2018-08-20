#define DOTNET
using System.Drawing;

namespace Edit2DLib
{
    public partial class Edit2DBase
    {
#if DOTNET
        // Draw primites are a list of entry points
        public IDrawPrimitives DrawPrimitives { get; set; }
        public Graphics oGraphics { get; set; }
#else
        public CanvasRenderingContext2D context;
#endif

        // The drawing area may be set up to snap to a grid size.
        public int GridSize { get; set; } = 10;

        // If this control is a sub control we will only manage shapes, not clear the screen or show the grid or axis
        public int SubControl { get; set; } = 0;

        public Point LastScreenMousePosition { get; set; }
        public eMouseState MouseState = eMouseState.Nothing;

        // This is the bias for display
        public int WorldPointScrollX { get; set; } = 0;
        public int WorldPointScrollY { get; set; } = 0;

        public int PictureBoxWidth { get; set; }
        public int PictureBoxHeight { get; set; }

        public float CurrentZoom { get; set; } = 1;

    }
}
