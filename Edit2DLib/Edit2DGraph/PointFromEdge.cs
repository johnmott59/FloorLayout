using System.Drawing;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public PointF EdgeP1(Edge oEdge)
        {
            if (MostRecentlySelectedLayer == null) return new PointF(0, 0);

            return MostRecentlySelectedLayer.EdgeP1(oEdge);

            //return new PointF(0, 0);

        }

        public PointF EdgeP2(Edge oEdge)
        {
            if (MostRecentlySelectedLayer == null) return new PointF(0, 0);

            return MostRecentlySelectedLayer.EdgeP2(oEdge);

            //return new PointF(0, 0);
        }

    }
}
