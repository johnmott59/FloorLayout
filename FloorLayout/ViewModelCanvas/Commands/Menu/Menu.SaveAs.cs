using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using ShapeTemplateLib.Templates.User0;

namespace FloorLayout
{

    public partial class ViewModelCanvas
    {

        // Called once to get the address of the button that does the work
        public ICommand MenuItemSaveAsCommand
        {
            get { return new DelegateCommand(MenuItemSaveAs); }
        }

        // Called by the form when the button is pressed
        private void MenuItemSaveAs()
        {
            FloorLayoutInput fli = LoadFloorLayoutInputFromEdit();

            SaveFileDialog x = new SaveFileDialog();

            bool? sts = x.ShowDialog();
            x.DefaultExt = "XML | .xml";

            if (sts == null && sts.Value == false)
            {
                return;
            }

            fli.GetProperties().Save(x.FileName);

        }
    }

}
