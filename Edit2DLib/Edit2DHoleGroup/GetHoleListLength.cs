#define DOTNET

using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        // We have this in a separate routine to make it easier to separate out the JS/C# functionality
        public int GetHoleListLength(LayoutHole[] HoleList)
        {
#if DOTNET
            return HoleList.Length;
#else
            return HoleList.length;
#endif

        }

    }
}
