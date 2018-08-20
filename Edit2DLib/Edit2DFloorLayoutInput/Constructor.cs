#define DOTNET
using System.Collections.Generic;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DFloorLayoutInput : Edit2DBase
    {
        public Edit2DFloorLayoutInput()
        {
            //  This is a trick to get code to show up when pasting into typescript.
            // Because this method is extracted from the class by a program that skips the top of the file
            // the #define won't be picked up. At the same time when the typescript paste occurs the
            // compiler will evaluate the #define and generate the 'else', which will be appropriate for javascript.
#if DOTNET
#else
            super();
#endif
            /*
             * Initialize the open area and outlines. Disable axis since this is a sub drawing
             */
            OpenAreas = new Edit2DHoleGroup();
            OpenAreas.SubControl = 1;
            OpenAreas.ShapeStrokeColor = "#00ff00";

            OutlineAreas = new Edit2DHoleGroup();
            OutlineAreas.SubControl = 1;
            OutlineAreas.ShapeStrokeColor = "#0000ff";

            Walls = new Edit2DGraph();
            Walls.SubControl = 1;
            // The graph editor will merge points if they are dropped on one another. we don't want that feature
            // because we want walls to be separate
            Walls.AllowVertexMerge = 0;

        }
    }
}
