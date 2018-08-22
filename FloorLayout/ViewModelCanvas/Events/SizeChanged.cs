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
        // This is called when the canvas size has changed
        public void SizeChanged()
        {
            oFWRInput.PictureBoxHeight = (int) oCanvas.Height;
            oFWRInput.PictureBoxWidth = (int) oCanvas.Width;

            oFWRInput.OpenAreas.PictureBoxWidth = (int)oCanvas.Width;
            oFWRInput.OpenAreas.PictureBoxHeight = (int)oCanvas.Height;

            oFWRInput.OutlineAreas.PictureBoxHeight = (int)oCanvas.Height;
            oFWRInput.OutlineAreas.PictureBoxWidth = (int)oCanvas.Width;

            oFWRInput.Walls.PictureBoxWidth = (int)oCanvas.Width;
            oFWRInput.Walls.PictureBoxHeight = (int)oCanvas.Height;

            oDrawPrimitives.StartDrawing();
            oFWRInput.DrawShapes();
        }
    }
}
