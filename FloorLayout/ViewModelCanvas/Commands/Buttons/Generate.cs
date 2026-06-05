using System.Windows.Input;
using System.IO;
using System.Linq;
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
            ShapeTemplateLib.Templates.User0.FloorMaker oFloorLayout = new ShapeTemplateLib.Templates.User0.FloorMaker();

            // Fix XML element names for LoadProperties compatibility
            // XML has: <list name="roomlist"><flroom>...</flroom></list>
            // LoadProperties expects: <list name="assembledroomlist"><flroom>...</flroom></list>
            var roomList = xfl.Descendants("list")
                .FirstOrDefault(x => x.Attribute("name")?.Value == "roomlist");
            if (roomList != null)
            {
                roomList.SetAttributeValue("name", "assembledroomlist");
            }

            // Fix edge element names: LoadProperties expects "fledge" but GetProperties returns "fmedge"
            var edgeList = xfl.Descendants("list")
                .FirstOrDefault(x => x.Attribute("name")?.Value == "edgelist");
            if (edgeList != null)
            {
                foreach (var edgeElement in edgeList.Elements("fmedge").ToList())
                {
                    edgeElement.Name = "fledge";
                }
            }

            // DEBUG: Check LoadProperties result
            bool loadSuccess = oFloorLayout.LoadProperties(xfl, out message);
            System.Diagnostics.Debug.WriteLine($"LoadProperties success: {loadSuccess}, message: {message}");

            if (!loadSuccess)
            {
                System.Windows.MessageBox.Show($"Failed to load FloorMaker properties: {message}",
                    "Load Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            // DEBUG: Check what was loaded
            int roomCount = oFloorLayout.AssembledRoomList?.Length ?? 0;
            int edgeCount = oFloorLayout.EdgeList?.Length ?? 0;
            int vertexCount = oFloorLayout.VertexList?.Length ?? 0;

            System.Diagnostics.Debug.WriteLine($"After LoadProperties - Rooms: {roomCount}, Edges: {edgeCount}, Vertices: {vertexCount}");
            System.Windows.MessageBox.Show(
                $"FloorMaker loaded:\n- Rooms: {roomCount}\n- Edges: {edgeCount}\n- Vertices: {vertexCount}",
                "Debug Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

            oFloorLayout.RandomlySelectDoors();

            XElement sl = oFloorLayout.Compile(5,5);
            XElement scene = new XElement("scene", sl);

            File.Delete(DefaultFileToGenerateTo);
            GetMesh(DefaultFileToGenerateTo, scene);

        }

       

    
     }
}
