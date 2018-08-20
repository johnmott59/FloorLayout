#define DOTNET
using System.Collections.Generic;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DFloorLayoutInput
    {
        public override void DrawShapes()
        {
            this.Clear("#FFFFFF");
#if DOTNET
            // The model for .net is to invalidate and let the paint handler draw.
            if (oGraphics == null && DrawPrimitives == null) return;


#else
#endif
            // Draw the outline and the open area

            this.OutlineAreas.DrawShapes();
            this.OpenAreas.DrawShapes();
            this.Walls.DrawShapes();

            DrawGrid();
            DrawAxis();

        }
    }
}
