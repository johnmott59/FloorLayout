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
        public ICommand somethingCommand
        {
            get { return new DelegateCommand(something); }
        }

        // Called by the form when the button is pressed
        private void something()
        {
 
        }
    }
}
