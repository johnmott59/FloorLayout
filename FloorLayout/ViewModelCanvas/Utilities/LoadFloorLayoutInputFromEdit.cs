using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace FloorLayout
{
    public partial class ViewModelCanvas 
    {
        /// <summary>
        /// Load the drawing into floorlayout input template
        /// </summary>
        /// <returns></returns>
        public FloorLayoutInput LoadFloorLayoutInputFromEdit()
        {
            FloorLayoutInput oInput = new FloorLayoutInput();

            oFWRInput.OpenAreas.InvertHolePoints();
            oInput.OpenArea.oHoleGroup = oFWRInput.OpenAreas.HoleGroupList[0];
            oInput.OpenArea.EllipseArray = oFWRInput.OpenAreas.BoundaryEllipseList.ToArray();
            oInput.OpenArea.RectangleArray = oFWRInput.OpenAreas.BoundaryRectangleList.ToArray();
            oInput.OpenArea.PolygonArray = oFWRInput.OpenAreas.BoundaryPolygonList.ToArray();

            oFWRInput.OutlineAreas.InvertHolePoints();
            oInput.Outline.oHoleGroup = oFWRInput.OutlineAreas.HoleGroupList[0];
            oInput.Outline.EllipseArray = oFWRInput.OutlineAreas.BoundaryEllipseList.ToArray();
            oInput.Outline.RectangleArray = oFWRInput.OutlineAreas.BoundaryRectangleList.ToArray();
            oInput.Outline.PolygonArray = oFWRInput.OutlineAreas.BoundaryPolygonList.ToArray();

            List<LineSegment> linelist = new List<LineSegment>();

            Edit2DLib.Edit2DGraphLayer oLayer = oFWRInput.Walls.Edit2dGraphLayerList[0];

            foreach (Edge oEdge in oLayer.EdgeList)
            {
                Vertex p1 = oLayer.VertexList[oEdge.p1];
                Vertex p2 = oLayer.VertexList[oEdge.p2];

                linelist.Add(new LineSegment()
                {
                    From = new Point2D() { X = p1.X, Y = p1.Y },
                    To = new Point2D() { X = p2.X, Y = p2.Y }
                });
            }

            oInput.WallSegmentArray = linelist.ToArray();

            oFWRInput.OpenAreas.InvertHolePoints();
            oFWRInput.OutlineAreas.InvertHolePoints();
            /*
             * Re-invert the points in the drawing
             */

            return oInput;
        }
     
    }
}
