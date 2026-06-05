using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Windows;
using APILib;

namespace FloorLayout
{
    public partial class ViewModelCanvas
    {
        /// <summary>
        /// Send a FloorLayoutInput tag to the API to convert it to a FloorLayout tag
        /// </summary>
        /// <param name="ele"></param>
        /// <returns></returns>
        public XElement GetFloorLayout(XElement ele)
        {
            XElement result;

            // DEBUG: Log the input
            System.Diagnostics.Debug.WriteLine("=== GetFloorLayout INPUT ===");
            System.Diagnostics.Debug.WriteLine(ele.ToString());
            System.Diagnostics.Debug.WriteLine("==============================");

            // Call APILib directly instead of web API
            API api = new API();
            APIStatus status = api.APIEntry("getfloorlayout", ele.ToString());

            // DEBUG: Check API status
            System.Diagnostics.Debug.WriteLine($"API Success: {status.Success}");
            System.Diagnostics.Debug.WriteLine($"API ErrorMessage: {status.ErrorMessage}");
            System.Diagnostics.Debug.WriteLine($"API OutputFile: {status.OutputFile}");

            if (!status.Success)
            {
                string error = $"GetFloorLayout failed: {status.ErrorMessage}";
                System.Diagnostics.Debug.WriteLine($"ERROR: {error}");
                MessageBox.Show(error, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new System.Exception(error);
            }

            // The API writes the result to a temp file, load it
            if (!string.IsNullOrEmpty(status.OutputFile) && File.Exists(status.OutputFile))
            {
                FileInfo fi = new FileInfo(status.OutputFile);
                System.Diagnostics.Debug.WriteLine($"Output file size: {fi.Length} bytes");

                result = XElement.Load(status.OutputFile);

                // DEBUG: Log the output
                System.Diagnostics.Debug.WriteLine("=== GetFloorLayout OUTPUT ===");
                System.Diagnostics.Debug.WriteLine(result.ToString());
                System.Diagnostics.Debug.WriteLine("==============================");

                // Check if result has any rooms
                // The structure is: <list name="assembledroomlist"><rmassembledroom>...</rmassembledroom></list>
                var roomList = result.Descendants("list")
                    .FirstOrDefault(x => x.Attribute("name")?.Value == "assembledroomlist");

                if (roomList != null)
                {
                    int roomCount = roomList.Elements("rmassembledroom").Count();
                    System.Diagnostics.Debug.WriteLine($"Number of rooms found: {roomCount}");
                    MessageBox.Show($"FloorLayout generated successfully with {roomCount} rooms",
                        "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("WARNING: No assembledroomlist element found in output");
                    MessageBox.Show("WARNING: No rooms were generated. Check that you have defined outlines, open areas, and walls.",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                string error = $"GetFloorLayout did not produce an output file. OutputFile={status.OutputFile ?? "null"}";
                System.Diagnostics.Debug.WriteLine($"ERROR: {error}");
                MessageBox.Show(error, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new System.Exception(error);
            }

            return result;

#if false
            // OLD CODE - called web API
            string url;

            // Step 2. Retrieve XML descriptions of the Simple Panels based on these descriptions
            url = "http://www.meshola.com/API/APIAlpha";

            using (WebClient client = new WebClient())
            {
                /*
                 * Get the mesh of connected panels
                 */
                var reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("command", "getfloorlayout");
                reqparm.Add("xmldata", ele.ToString());

                byte[] responsebytes = client.UploadValues(url, "POST", reqparm);
                MemoryStream ms = new MemoryStream(responsebytes);

                result = XElement.Load(ms);

            }

            return result;
#endif
        }

    }
}
