using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.Win32;
using ShapeTemplateLib.Templates.User0;

namespace FloorLayout
{

    public partial class ViewModelCanvas
    {

        // Called once to get the address of the button that does the work
        public ICommand MenuItemOpenCommand
        {
            get { return new DelegateCommand(MenuItemOpen); }
        }

        // Called by the form when the button is pressed
        private void MenuItemOpen()
        {
            string message = "";

            OpenFileDialog o = new OpenFileDialog();
            o.Multiselect = false;
            o.Filter = "XML | *.xml";
            bool? b = o.ShowDialog();

            if (b == null || b.Value == false)
            {
                return;
            }
           
            XElement xo = XElement.Load(o.FileName);
            FloorLayoutInput fli = new FloorLayoutInput();
            fli.LoadProperties(xo, out message);

            LoadEditorsFromFloorLayoutInput(fli);

            // Save this as the default file to save to.
            DefaultFileToSaveTo = o.FileName;

        }
    }

}
