using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FloorLayout
{
    public partial class ViewModelCanvas 
    {

        // Called once to get the address of the button that does the work
        public ICommand SketchOpenAreaCommand
        {
            get { return new DelegateCommand(SketchOpenArea); }
        }

        // Called by the form when the button is pressed
        private void SketchOpenArea()
        {
            CurrentSketchMode = eSketchMode.SketchOpenArea;

            SketchShapeIndex = -1;

        }
        
    }
}
