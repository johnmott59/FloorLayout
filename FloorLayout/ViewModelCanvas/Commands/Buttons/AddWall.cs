using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace FloorLayout
{
    public partial class ViewModelCanvas 
    {
        // Called once to get the address of the button that does the work
        public ICommand AddWallCommand
        {
            get { return new DelegateCommand(AddWall); }
        }

        // Called by the form when the button is pressed
        private void AddWall(PointF WorldStart, PointF WorldEnd)
        {
            oFWRInput.Walls.AddEdge(
                           WorldStart,
                           WorldEnd,
                           4, 10);

            // Since we're changing the list of shapes clear them to let them rebuild
            oCanvas.Children.Clear();

            oDrawPrimitives.StartDrawing();
            oFWRInput.DrawShapes();
        }


        private void AddWall()
        {
            AddWall(new PointF(0, 0), new PointF(100, 100));
        }
    }
}
