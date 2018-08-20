using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Input;

namespace FloorLayout
{
    public partial class ViewModelCanvas 
    {
        // Called once to get the address of the button that does the work
        public ICommand AddOutlineRectCommand
        {
            get { return new DelegateCommand(AddOutlineRect); }
        }

        // Called by the form when the button is pressed
        private void AddOutlineRect(PointF ScreenStart, float Width, float Height)
        {
            // Add at the origin

           // PointF p = oFWRInput.W2S(ScreenStart.X, ScreenStart.Y);
            oFWRInput.OutlineAreas.AddRectangle((int)ScreenStart.X, (int)ScreenStart.Y, (int) Width, (int) Height);

            // Since we're changing the list of shapes clear them to let them rebuild
            oCanvas.Children.Clear();

            oDrawPrimitives.StartDrawing();
            oFWRInput.DrawShapes();
        }

        private void AddOutlineRect()
        {
            AddOutlineRect(new PointF(0, 0), 100, 100); 
#if false
            // Add at the origin

            PointF p = oFWRInput.W2S(0, 0);
            oFWRInput.OutlineAreas.AddRectangle((int)p.X, (int)p.Y, 100,100);

            // Since we're changing the list of shapes clear them to let them rebuild
            oCanvas.Children.Clear();

            oDrawPrimitives.StartDrawing();
            oFWRInput.DrawShapes();
#endif
        }
    }
}
