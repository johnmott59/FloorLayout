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
            // Show debug window and setup APILib callback
            DebugWindow.ShowWindow();
            APILib.DebugLogger.WriteLineCallback = DebugWindow.WriteLine;

            DebugWindow.WriteSeparator("GENERATE START");
            DebugWindow.WriteLineWithTime("Generate button clicked");

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
            DebugWindow.WriteLine("Inverting Y coordinates for room generation...");
            oFWRInput.OpenAreas.InvertHolePoints();
            oFWRInput.OutlineAreas.InvertHolePoints();

            DebugWindow.WriteSeparator("API CALL #1: GetFloorLayout");
            XElement xfl = GetFloorLayout(oInput.GetProperties());

            DebugWindow.WriteLine("Restoring Y coordinates...");
            oFWRInput.OpenAreas.InvertHolePoints();
            oFWRInput.OutlineAreas.InvertHolePoints();

            DebugWindow.WriteSeparator("LOAD FLOORMAKER");
            string message = "";
            ShapeTemplateLib.Templates.User0.FloorMaker oFloorLayout = new ShapeTemplateLib.Templates.User0.FloorMaker();

            // Fix XML element names for LoadProperties compatibility
            DebugWindow.WriteLine("Fixing XML element names for compatibility...");
            // XML has: <list name="roomlist"><flroom>...</flroom></list>
            // LoadProperties expects: <list name="assembledroomlist"><flroom>...</flroom></list>
            var roomList = xfl.Descendants("list")
                .FirstOrDefault(x => x.Attribute("name")?.Value == "roomlist");
            if (roomList != null)
            {
                DebugWindow.WriteLine("  - Renaming 'roomlist' to 'assembledroomlist'");
                roomList.SetAttributeValue("name", "assembledroomlist");
            }

            // Fix edge element names: LoadProperties expects "fledge" but GetProperties returns "fmedge"
            var edgeList = xfl.Descendants("list")
                .FirstOrDefault(x => x.Attribute("name")?.Value == "edgelist");
            if (edgeList != null)
            {
                int fmedgeCount = edgeList.Elements("fmedge").Count();
                DebugWindow.WriteLine($"  - Renaming {fmedgeCount} 'fmedge' elements to 'fledge'");
                foreach (var edgeElement in edgeList.Elements("fmedge").ToList())
                {
                    edgeElement.Name = "fledge";
                }
            }

            // Check LoadProperties result
            DebugWindow.WriteLine("Calling FloorMaker.LoadProperties()...");
            bool loadSuccess = oFloorLayout.LoadProperties(xfl, out message);
            DebugWindow.WriteLine($"LoadProperties result: {(loadSuccess ? "SUCCESS" : "FAILED")}");
            if (!string.IsNullOrEmpty(message))
            {
                DebugWindow.WriteLine($"  Message: {message}");
            }

            if (!loadSuccess)
            {
                System.Windows.MessageBox.Show($"Failed to load FloorMaker properties: {message}",
                    "Load Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            // Check what was loaded
            int roomCount = oFloorLayout.AssembledRoomList?.Length ?? 0;
            int edgeCount = oFloorLayout.EdgeList?.Length ?? 0;
            int vertexCount = oFloorLayout.VertexList?.Length ?? 0;

            DebugWindow.WriteLine("");
            DebugWindow.WriteLine($"FloorMaker contains:");
            DebugWindow.WriteLine($"  - Vertices: {vertexCount}");
            DebugWindow.WriteLine($"  - Edges: {edgeCount}");
            DebugWindow.WriteLine($"  - Rooms: {roomCount}");

            if (roomCount == 0)
            {
                DebugWindow.WriteLine("");
                DebugWindow.WriteLine("WARNING: No rooms detected! Check wall configuration.");
            }

            // Analyze door candidates
            if (edgeCount > 0)
            {
                int doorCandidates = oFloorLayout.EdgeList.Count(e => e.InteriorDoorCandidate == 1);
                int windowCandidates = oFloorLayout.EdgeList.Count(e => e.ExteriorWindowCandidate == 1);
                DebugWindow.WriteLine("");
                DebugWindow.WriteLine($"Door/Window candidates:");
                DebugWindow.WriteLine($"  - Interior door candidates: {doorCandidates}");
                DebugWindow.WriteLine($"  - Exterior window candidates: {windowCandidates}");
            }

            System.Windows.MessageBox.Show(
                $"FloorMaker loaded:\n- Rooms: {roomCount}\n- Edges: {edgeCount}\n- Vertices: {vertexCount}",
                "Debug Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

            DebugWindow.WriteSeparator("DOOR SELECTION");
            DebugWindow.WriteLine("Calling RandomlySelectDoors()...");
            oFloorLayout.RandomlySelectDoors();

            // Check how many doors were assigned
            int doorsAssigned = oFloorLayout.EdgeList?.Count(e => !string.IsNullOrEmpty(e.HoleGroupID)) ?? 0;
            DebugWindow.WriteLine($"Doors assigned: {doorsAssigned}");

            DebugWindow.WriteSeparator("COMPILE TO SIMPLELAYOUT");
            DebugWindow.WriteLine("Calling FloorMaker.Compile(5, 5)...");
            XElement sl = oFloorLayout.Compile(5, 5);
            XElement scene = new XElement("scene", sl);

            DebugWindow.WriteSeparator("API CALL #2: GetMesh");
            File.Delete(DefaultFileToGenerateTo);
            GetMesh(DefaultFileToGenerateTo, scene);

            DebugWindow.WriteSeparator("GENERATE COMPLETE");
            DebugWindow.WriteLineWithTime($"FBX saved to: {DefaultFileToGenerateTo}");
        }

       

    
     }
}
