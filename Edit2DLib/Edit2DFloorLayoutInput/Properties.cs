#define DOTNET

namespace Edit2DLib
{
    public partial class Edit2DFloorLayoutInput
    {
        /*
         * The FWR input contains the polygons and lines used to build a floor
         */
         public Edit2DHoleGroup OutlineAreas { get; set; }

         public Edit2DHoleGroup OpenAreas { get; set; }

         // We'll use the graph editor for the walls 
         public Edit2DGraph Walls { get; set; }

    }
}
