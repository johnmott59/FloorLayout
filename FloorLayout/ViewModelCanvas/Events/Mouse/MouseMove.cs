using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Shapes;

namespace FloorLayout
{
    public partial class ViewModelCanvas 
    {
     

        public void MouseMove(MouseEventArgs e)
        {
            System.Windows.Point ScreenPoint = e.GetPosition(oCanvas);

            if (SketchShapeIndex >= 0)
            {
                EndSketch = ScreenPoint;

                switch (CurrentSketchMode)
                {
                    case eSketchMode.SketchWall:
                        Line line = (Line)oCanvas.Children[SketchShapeIndex];

                        line.X2 = oFWRInput.RoundToGrid((int)EndSketch.X);
                        line.Y2 = oFWRInput.RoundToGrid((int)EndSketch.Y);

                        break;

                    case eSketchMode.SketchOutline:
                    case eSketchMode.SketchOpenArea:
                        Rectangle r = (Rectangle)oCanvas.Children[SketchShapeIndex];
                        Canvas.SetLeft(r,StartSketch.X);
                        Canvas.SetTop(r, StartSketch.Y);

                        double width = EndSketch.X - StartSketch.X;
                        double height = EndSketch.Y - StartSketch.Y;

                        int iwidth = oFWRInput.RoundToGrid((int)width);
                        int iheight = oFWRInput.RoundToGrid((int)height);

                        if (width > 0) r.Width = iwidth;
                        if (height > 0) r.Height = iheight;
                        break;
                }
                return;
            }
 

            if (oFWRInput.BaseMouseMove((int)ScreenPoint.X, (int)ScreenPoint.Y))
            {
                oDrawPrimitives?.StartDrawing();
                oFWRInput.DrawShapes();
            }

        }
    }
}
