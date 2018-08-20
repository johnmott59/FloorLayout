#define DOTNET
using System.Collections.Generic;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        public void AddHoleToHoleGroup(HoleGroup hg, LayoutHole oNewHole)
        {

#if DOTNET
            List<LayoutHole> tmp = new List<LayoutHole>(hg.HoleList);
            tmp.Add(oNewHole);
            hg.HoleList = tmp.ToArray();
#else
            hg.HoleList.push(oNewHole);
#endif
        }

    }
}
