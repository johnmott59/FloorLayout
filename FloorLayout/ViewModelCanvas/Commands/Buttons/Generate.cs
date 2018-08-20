using System.Windows.Input;
using System.IO;
using System.Xml.Linq;
using Microsoft.Win32;
using ShapeTemplateLib.Templates.User0;

namespace FloorLayout
{
    public partial class ViewModelCanvas 
    {
        public string DefaultFileToGenerateTo { get; set; } = "";

        // Called once to get the address of the button that does the work
        public ICommand GenerateCommand
        {
            get { return new DelegateCommand(Generate); }
        }

        // Called by the form when the button is pressed
        private void Generate()
        {
            FloorLayoutInput oInput = LoadFloorLayoutInputFromEdit();

            if (DefaultFileToGenerateTo == "")
            {
                // Prompt on a location to save
                SaveFileDialog x = new SaveFileDialog();
                x.DefaultExt = "FBX|.fbx";

                bool? sts = x.ShowDialog();

                if (sts == null || sts.Value == false) return;

                DefaultFileToGenerateTo = x.FileName;
            }


            // Flip the 'y' before generating rooms

            oFWRInput.OpenAreas.InvertHolePoints();
            oFWRInput.OutlineAreas.InvertHolePoints();

            XElement xfl = GetFloorLayout(oInput.GetProperties());

            oFWRInput.OpenAreas.InvertHolePoints();
            oFWRInput.OutlineAreas.InvertHolePoints();

            string message = "";
            ShapeTemplateLib.Templates.User0.FloorLayout oFloorLayout = new ShapeTemplateLib.Templates.User0.FloorLayout();
            oFloorLayout.LoadProperties(xfl, out message);  


            oFloorLayout.RandomlySelectDoors();

            XElement sl = oFloorLayout.Compile(5,5);
            XElement scene = new XElement("scene", sl);

            File.Delete(DefaultFileToGenerateTo);
            GetMesh(DefaultFileToGenerateTo, scene);

        }

       

    
     }
}
