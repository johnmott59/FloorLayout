#define DOTNET
using System.Collections.Generic;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        public void RemoveHoleFromHoleGroup(HoleGroup hg, int index)
        {

#if DOTNET
            List<LayoutHole> tmp = new List<LayoutHole>(hg.HoleList);
            tmp.RemoveAt(index);
            hg.HoleList = tmp.ToArray();
#else
            hg.HoleList.splice(index,1);
#endif
        }

    }
}
