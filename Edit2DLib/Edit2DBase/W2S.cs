using System.Drawing;

namespace Edit2DLib
{
    public partial class Edit2DBase
    {


        public PointF W2S(float WorldX, float WorldY)
        {
            PointF WorldUpperLeft = GetUpperLeftWorld();

            return new PointF((WorldX - WorldUpperLeft.X) / CurrentZoom, (WorldY - WorldUpperLeft.Y) / CurrentZoom);

        }


    }
}
