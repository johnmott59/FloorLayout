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
        // if we open a file then remember its name so we can save to it. if we 
        // create a new drawing then we'll reset this so it will have a different name.

        public string DefaultFileToSaveTo { get; set; } = "";

        // Called once to get the address of the button that does the work
        public ICommand MenuItemSaveCommand
        {
            get { return new DelegateCommand(MenuItemSave); }
        }

        // Called by the form when the button is pressed
        private void MenuItemSave()
        {
            // Get the output file
            FloorLayoutInput fli = LoadFloorLayoutInputFromEdit();

            // If we've previously saved something save to the same location
            if (DefaultFileToSaveTo!= "")
            {
                // Save it to the where we saved it last time  
                fli.GetProperties().Save(DefaultFileToSaveTo);
                return;
            }

            // Prompt on a location to save
            SaveFileDialog x = new SaveFileDialog();
            x.DefaultExt = "XML | .xml";

            bool? sts = x.ShowDialog();

            if (sts == null || sts.Value == false) return;

            fli.GetProperties().Save(x.FileName);

            DefaultFileToSaveTo = x.FileName;

        }
    }

}
