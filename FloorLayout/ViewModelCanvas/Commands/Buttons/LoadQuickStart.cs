using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Input;
using ShapeTemplateLib.Templates.User0;
using System.Xml.Linq;
using ShapeTemplateLib;
using Edit2DLib;
using System;
using System.Windows.Resources;
using System.Windows;

namespace FloorLayout
{
    public partial class ViewModelCanvas 
    {
        /// <summary>
        /// Load a floor
        /// </summary>
        public ICommand QuickStartCommand
        {
            get { return new DelegateCommand(QuickStart); }
        }


        private void QuickStart()
        {
            MenuItemNew();          

            string message = "";

            Uri uri = new Uri("/Samples/hello.xml", UriKind.Relative);
            StreamResourceInfo info = Application.GetContentStream(uri);

            XElement xo = XElement.Load(info.Stream);
            FloorLayoutInput fli = new FloorLayoutInput();
            fli.LoadProperties(xo, out message);

            LoadEditorsFromFloorLayoutInput(fli);


        }



    }
}
