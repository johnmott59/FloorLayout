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
        public ICommand MenuItemCloseCommand
        {
            get { return new DelegateCommand(MenuItemClose); }
        }

        // Called by the form when the button is pressed
        private void MenuItemClose()
        {
            LoadEditorsFromFloorLayoutInput(new FloorLayoutInput());

            // Clear the default save to
            this.DefaultFileToSaveTo = "";
        }

    }

}
