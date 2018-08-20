#define DOTNET
using System.Collections.Generic;

namespace Edit2DLib
{
    public partial class Edit2DGraph : Edit2DBase
    {
        public Edit2DGraph()
        {
            //  This is a trick to get code to show up when pasting into typescript.
            // Because this method is extracted from the class by a program that skips the top of the file
            // the #define won't be picked up. At the same time when the typescript paste occurs the
            // compiler will evaluate the #define and generate the 'else', which will be appropriate for javascript.
#if DOTNET
#else
            super();
#endif
            // Create the layer list
            Edit2dGraphLayerList = new List<Edit2DGraphLayer>();

            // We would like to just call pushlayer() here, but it calls DrawShapes and that environment isn't set up
            Edit2DGraphLayer oNewLayer = new Edit2DGraphLayer();
            Edit2dGraphLayerList.Add(oNewLayer);
            MostRecentlySelectedLayer = oNewLayer;

            HorizontalScale = 1;
            VerticalScale = 1;

            bShowHandles = true;

            // set handle size 
            HandleSize = 20;

            // By default allow vertices to merge
            AllowVertexMerge = 1;
        }
    }
}
