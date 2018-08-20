using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public Edge FindEdgeWithHoleGroupID(string HoleGroupID)
        {
            for (int i=0; i < Edit2dGraphLayerList.Count; i++)
            {
                Edit2DGraphLayer oLayer = Edit2dGraphLayerList.GetFrom(i);

                for (int j=0; j < oLayer.EdgeList.Count; j++)
                {
                    Edge oEdge = oLayer.EdgeList.GetFrom(j);
                    if (oEdge.HoleGroupID == HoleGroupID) return oEdge;
                }
            }

            return null;
        }
      
    }
}
