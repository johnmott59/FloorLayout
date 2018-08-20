using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        public void SelectEdgeByHoleGroupID(string HoleGroupID)
        {
            for (int i=0; i < EdgeList.Count; i++)
            {
                Edge oEdge = EdgeList.GetFrom(i);
                if (oEdge.HoleGroupID == HoleGroupID)
                {
                    MostRecentlySelectedEdge = oEdge;
                    return;
                }
            }
        }
      
    }
}
