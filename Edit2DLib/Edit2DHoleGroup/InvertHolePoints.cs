using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        /*
         * This routine is used to invert the hole points, specifically the 'Y' points.
         * The html canvas has 0,0 at the upper left and 'Y' goes down. For the holegroup canvas 
         * we want a 'Y' that goes up, because that's how the holes are overlaid and appear in 
         * three dimensions, and how we think of them intuitively. To manage this we'll simply flip the 
         * Y points for the duration of the time that they are being edited and reflip them when its time to save
         */
        public void InvertHolePoints()
        {
            for (int i = 0; i < HoleGroupList.Count; i++)
            {
                HoleGroup hg = HoleGroupList.GetFrom(i); 

                for (int j = 0; j < hg.HoleList.Length; j++)
                {
                    /*
                     * For all hole types the offset Y is inverted
                     */
                    LayoutHole oHole = hg.HoleList[j];
                    oHole.OffsetY = -oHole.OffsetY;
                    /*
                     * For polygon hole types the individual Y's are inverted
                     */
                    if (oHole.HoleType == "poly")
                    {
                        BoundaryPolygon oPolygon = BoundaryPolygonList.GetFrom(oHole.HoleTypeIndex);
                        for ( int k=0; k < oPolygon.PointList.Length; k++)
                        {
                            Point3D p = oPolygon.PointList[k];
                            p.Y = -p.Y;
                        }
                    }
                }
            }
        }
    }
}
