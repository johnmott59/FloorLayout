using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Edit2DLib;

namespace FloorLayout
{
    public partial class ViewModelCanvas 
    {
        // When we are sketching drawings that will be done 'on top' of the other drawings.
        // When the drawings are done they will be added to the correct categories

        public enum eSketchMode
        {
            None,
            SketchWall,
            SketchOutline,
            SketchOpenArea,
        }

        public int GridSize { get; set; } = 10;

        public eSketchMode CurrentSketchMode = eSketchMode.None;
        /*
         * This is the edit controller for the the drawing, it manages all the drawings
         */
        public Edit2DFloorLayoutInput oFWRInput = new Edit2DFloorLayoutInput();

        public Canvas oCanvas { get; set; }
        // Create a drawprimitives object that will let us render to the canvas
        public DrawPrimitives oDrawPrimitives { get; set; }



    }
}
