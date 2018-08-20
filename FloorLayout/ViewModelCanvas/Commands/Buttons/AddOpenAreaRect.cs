using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FloorLayout
{
    public partial class ViewModelCanvas 
    {
        public ICommand AddOpenAreaRectCommand
        {
            get { return new DelegateCommand(AddOpenAreaRect); }
        }

        // Called by the form when the button is pressed
        private void AddOpenAreaRect(PointF ScreenStart, float Width, float Height)
        {
            oFWRInput.OpenAreas.AddRectangle((int)ScreenStart.X, (int)ScreenStart.Y, (int)Width, (int)Height);

            // Since we're changing the list of shapes clear them to let them rebuild
            oCanvas.Children.Clear();

            oDrawPrimitives.StartDrawing();
            oFWRInput.DrawShapes();
        }

        // Called by the form when the button is pressed
        private void AddOpenAreaRect()
        {
            AddOpenAreaRect(new PointF(0, 0), 100, 100);
#if false
            // Add at the origin

            PointF p = oFWRInput.W2S(0, 0);
            oFWRInput.OpenAreas.AddRectangle((int)p.X, (int)p.Y, 100, 100);

            // Since we're changing the list of shapes clear them to let them rebuild
            oCanvas.Children.Clear();

            oDrawPrimitives.StartDrawing();
            oFWRInput.DrawShapes();
#endif
        }
    }
}
