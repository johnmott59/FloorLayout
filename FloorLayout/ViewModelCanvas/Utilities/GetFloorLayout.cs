using System.IO;
using System.Net;
using System.Xml.Linq;

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
            string url;
            XElement result;

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
        }

    }
}
