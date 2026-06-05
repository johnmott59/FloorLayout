using System.IO;
using System.Net;
using System.Xml.Linq;
using APILib;

namespace FloorLayout
{
    public partial class ViewModelCanvas
    {
        /// <summary>
        /// Send a GetMesh command to the API. The mesh is wrapped in a <scene> tag and can have multiple elements</scene>
        /// </summary>
        /// <param name="OutputFile"></param>
        /// <param name="ele"></param>
        public void GetMesh(string OutputFile, XElement ele)
        {
            // Call APILib directly instead of web API
            API api = new API();
            APIStatus status = api.APIEntry("getmesh", ele.ToString());

            if (!status.Success)
            {
                throw new System.Exception($"GetMesh failed: {status.ErrorMessage}");
            }

            // The output file path is in status.OutputFile
            if (!string.IsNullOrEmpty(status.OutputFile) && File.Exists(status.OutputFile))
            {
                // Copy the generated file to the desired output location
                File.Copy(status.OutputFile, OutputFile, overwrite: true);
            }
            else
            {
                throw new System.Exception("GetMesh did not produce an output file");
            }

            return;

#if false
            // OLD CODE - called web API
            string url;

            url = "http://www.meshola.com/API/APIAlpha";

            using (WebClient client = new WebClient())
            {
                /*
                 * Get the mesh of connected panels
                 */
                var reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("command", "getmesh");
                reqparm.Add("xmldata", ele.ToString());

                byte[] responsebytes = client.UploadValues(url, "POST", reqparm);
                MemoryStream ms = new MemoryStream(responsebytes);

                // Save the generated files, ready to be exported into Unity
                File.WriteAllBytes(OutputFile, responsebytes);

            }

            return;
#endif
        }
    }
}
