using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        void DeleteHoleGroup(string HoleGroupID)
        {
            for (int i=0; i < HoleGroupList.Count; i++)
            {
                HoleGroup hg = HoleGroupList.GetFrom(i);
                if (hg.HoleGroupID == HoleGroupID)
                {
                    HoleGroupList.RemoveAt(i);
                    MostRecentlySelectedHole = null;
                    MostRecentlySelectedHoleGroup = null;
                    MostRecentlySelectedPolygonEdgeIndex = -1;
                    MostRecentlySelectedPolygonVertexIndex = -1;
                    return;
                }
            }
        }
    }
}
