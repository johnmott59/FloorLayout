#define DOTNET
using System.Collections.Generic;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup : Edit2DBase
    {
        public Edit2DHoleGroup()
        {
            //  This is a trick to get code to show up when pasting into typescript.
            // Because this method is extracted from the class by a program that skips the top of the file
            // the #define won't be picked up. At the same time when the typescript paste occurs the
            // compiler will evaluate the #define and generate the 'else', which will be appropriate for javascript.
#if DOTNET
#else
            super();
#endif
            // This will manage one set of holes; one holegroup

            MostRecentlySelectedHoleGroup = null; 

            MostRecentlySelectedHole = null;

            MostRecentlySelectedPolygonVertexIndex = -1;
            MostRecentlySelectedPolygonEdgeIndex = -1;

            // These are the individual drawing patterns

            BoundaryRectangleList = new List<BoundaryRectangle>();
            BoundaryEllipseList = new List<BoundaryEllipse>();
            BoundaryPolygonList = new List<BoundaryPolygon>();

            // Initialize the list of hole groups
            HoleGroupList = new List<HoleGroup>();

            // Initially set no shaded area
            ShadeLength = -1;
            ShadeHeight = -1;

            // assume we are not a sub control
            SubControl = 0;

            // Initial color for shape strokes
            ShapeStrokeColor = "#0000ff";

            // Handle size
            HandleSize = 20;

    }
    }
}
