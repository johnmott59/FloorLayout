using System.Drawing;

namespace Edit2DLib
{
    public partial class Edit2DBase
    {
        public void DrawAxis()
        {
            // we may not want to display the axis
            if (SubControl == 1) return;

            PointF XaxisFrom = this.W2S(-100, 0);
            PointF XaxisTo = this.W2S(100, 0);

            DrawLine("rgba(150,150,150,.5)", (float).5, XaxisFrom, XaxisTo);

            PointF YaxisFrom = this.W2S(0, -100);
            PointF YaxisTo = this.W2S(0, 100);

            DrawLine("rgba(150,150,150,.5)", (float).5, YaxisFrom, YaxisTo);

        }
      

        
    }
}
