using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Edit2DLib;
using Microsoft.Win32;
using ShapeTemplateLib.Templates.User0;

namespace FloorLayout
{

    public partial class ViewModelCanvas
    {

        // Called once to get the address of the button that does the work
        public ICommand MenuItemNewCommand
        {
            get { return new DelegateCommand(MenuItemNew); }
        }

        // Called by the form when the button is pressed
        private void MenuItemNew()
        {
            LoadEditorsFromFloorLayoutInput(new FloorLayoutInput());
        }

    }

}
