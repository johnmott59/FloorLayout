using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        public void AddHoleGroup(string HoleGroupID)
        {
            HoleGroup hg = new HoleGroup();
            hg.HoleGroupID = HoleGroupID;
            hg.HoleList = new LayoutHole[0];

            HoleGroupList.Add(hg);
        }
    }
}
