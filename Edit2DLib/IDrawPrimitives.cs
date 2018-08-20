#define DOTNET

using System.Drawing;

namespace Edit2DLib
{
#if DOTNET
    /// <summary>
    /// This interface is used by .net code to pass in a list of entry points for drawing so that 
    /// this routine doesn't have to know the specifics of each drawing kind. The final screen coordinates
    /// are passed to the routine. This will not be exported to Javascript
    /// </summary>
    public interface IDrawPrimitives
    {
        void StartDrawing(bool ClearCanvas = false);
        void DrawLine(string Color, float lineWidth, PointF From, PointF To);
        void DrawEllipse(string Color, float CenterX, float CenterY, float Radius);
        void DrawRectangle(string Color, float width, float height, float CenterX, float CenterY);
    }
#endif
}
