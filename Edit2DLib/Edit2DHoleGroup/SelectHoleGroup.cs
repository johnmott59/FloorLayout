using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        public void SelectHoleGroup(string name)
        {
            for (int i=0; i < HoleGroupList.Count; i++)
            {
                HoleGroup hg = HoleGroupList.GetFrom(i);
                if (hg.HoleGroupID == name)
                {
                    MostRecentlySelectedHoleGroup = hg;
                    return;
                }
            }
        }

    }
}
