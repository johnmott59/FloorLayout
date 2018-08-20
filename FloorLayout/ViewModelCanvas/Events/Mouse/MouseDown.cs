using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace FloorLayout
{
    public partial class ViewModelCanvas 
    {

        public Point StartSketch { get; set; }
        public Point EndSketch { get; set; }
        public int SketchShapeIndex { get; set; } = -1;

        public void MouseDown(MouseButtonEventArgs e)
        {
            Point ScreenPoint = e.GetPosition(oCanvas);
            /*
             * If we're in sketch mode we'll add a graphic and redraw that
             */
            if (SketchShapeIndex == -1)
            {
                StartSketch = ScreenPoint;
                EndSketch = ScreenPoint;
               
                switch (CurrentSketchMode)
                {
                    case eSketchMode.SketchWall:
                        // on the first mouse down add the shape

                        Line line = new Line();
                        line.Stroke = Brushes.OrangeRed;
                        line.StrokeThickness = 5;
                        line.X1 = (double)StartSketch.X;
                        line.Y1 = (double)StartSketch.Y;
                        line.X2 = (double)EndSketch.X;
                        line.Y2 = (double)EndSketch.Y;
                        line.Fill = Brushes.OrangeRed;

                        SketchShapeIndex = oCanvas.Children.Count;
                        oCanvas.Children.Add(line);
                        return;
                    case eSketchMode.SketchOutline:
                    case eSketchMode.SketchOpenArea:

                        Rectangle r = new Rectangle()
                        {
                            Width = EndSketch.X - StartSketch.X,
                            Height = EndSketch.Y - StartSketch.Y
                        };

                        r.Stroke = CurrentSketchMode == eSketchMode.SketchOpenArea ? Brushes.LawnGreen : Brushes.LightBlue;
                        r.StrokeThickness = 5;
                        r.Fill = Brushes.Transparent;

                        SketchShapeIndex = oCanvas.Children.Count;
                        oCanvas.Children.Add(r);

                        return;
                    case eSketchMode.None:
                        break;
                    default:
                        break;
                }
            }

            // If we're here we're not sketching shapes

            oFWRInput.BaseMouseDown((int)ScreenPoint.X, (int)ScreenPoint.Y);

        }
    }
}
