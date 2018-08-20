using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Shapes;
using System.Drawing;

namespace FloorLayout
{
    public partial class ViewModelCanvas 
    {
        public void MouseUp(MouseButtonEventArgs e)
        {
            System.Windows.Point ScreenPoint = e.GetPosition(oCanvas);

            /*
             * If we're in sketch mode we'll add a graphic and redraw that
             */
            switch (CurrentSketchMode)
            {
                case eSketchMode.SketchWall:
                    EndSketch = ScreenPoint;

                    // Remove this sketch and clear sketch parameters
                    oCanvas.Children.RemoveAt(SketchShapeIndex);
                    CurrentSketchMode = eSketchMode.None;
                    SketchShapeIndex = -1;

                    // Add the wall to the list of walls
                    PointF from = oFWRInput.S2W((float)StartSketch.X, (float)StartSketch.Y);
                    PointF to = oFWRInput.S2W((float)EndSketch.X, (float)EndSketch.Y);

                    AddWall(from, to);
                    return;
                case eSketchMode.SketchOutline:
                case eSketchMode.SketchOpenArea:
                    EndSketch = ScreenPoint;

                    // Remove this sketch and clear sketch parameters
                    oCanvas.Children.RemoveAt(SketchShapeIndex);
                    SketchShapeIndex = -1;

                    float width = (float)(EndSketch.X - StartSketch.X);
                    float height = (float) (EndSketch.Y - StartSketch.Y);
                    if (CurrentSketchMode == eSketchMode.SketchOpenArea)
                    {
                        AddOpenAreaRect(new PointF() { X = (float)StartSketch.X, Y = (float)StartSketch.Y }, width, height);
                    } else
                    {
                        AddOutlineRect(new PointF() { X = (float)StartSketch.X, Y = (float)StartSketch.Y }, width, height);
                    }

                    CurrentSketchMode = eSketchMode.None;

                    break;
            }
   

            oFWRInput.BaseMouseUp((int)ScreenPoint.X, (int)ScreenPoint.Y);



        }
    }
}
