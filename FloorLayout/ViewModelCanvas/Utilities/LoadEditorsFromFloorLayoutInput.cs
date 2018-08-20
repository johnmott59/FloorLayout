using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Edit2DLib;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace FloorLayout
{
    public partial class ViewModelCanvas 
    {
        public void LoadEditorsFromFloorLayoutInput(FloorLayoutInput fli)
        {
            oFWRInput = new Edit2DFloorLayoutInput();

            // Reset canvase
            oCanvas.Children.Clear();

            // define parameters for editor
            oFWRInput.DrawPrimitives = oDrawPrimitives;
            oFWRInput.PictureBoxHeight = (int)oCanvas.Height;
            oFWRInput.PictureBoxWidth = (int)oCanvas.Width;

            oDrawPrimitives?.StartDrawing(true);

            LoadEdit2d(oFWRInput.OutlineAreas, fli.Outline);

            LoadEdit2d(oFWRInput.OpenAreas, fli.OpenArea);

            oFWRInput.Walls.DrawPrimitives = oDrawPrimitives;
            oFWRInput.Walls.PictureBoxHeight = (int)oCanvas.Height;
            oFWRInput.Walls.PictureBoxWidth = (int)oCanvas.Width;

            foreach (LineSegment ls in fli.WallSegmentArray)
            {
                oFWRInput.Walls.AddEdge(new PointF(ls.From.X, ls.From.Y), new PointF(ls.To.X, ls.To.Y), 3, 10);
            }

            oDrawPrimitives?.StartDrawing(true);

            oFWRInput.DrawShapes();
        }

        protected void LoadEdit2d(Edit2DHoleGroup edit, SingleHoleGroup sg)
        {
            edit.DrawPrimitives = oDrawPrimitives;
            edit.PictureBoxHeight = (int)oCanvas.Height;
            edit.PictureBoxWidth = (int)oCanvas.Width;

            string hg = sg.oHoleGroup.HoleGroupID;
            edit.AddHoleGroup(hg);
            edit.SelectHoleGroup(hg);

            foreach (LayoutHole lh in sg.oHoleGroup.HoleList)
            {
                int hIndex = lh.HoleTypeIndex;
                float OffsetX = lh.OffsetX;
                float OffsetY = lh.OffsetY;
                PointF wp = oFWRInput.W2S(OffsetX, OffsetY);
                switch (lh.HoleType)
                {
                    case "rect":
                        BoundaryRectangle br = sg.RectangleArray[hIndex];
                        edit.AddRectangle((int)wp.X, (int)wp.Y, (int)br.Width, (int)br.Height);
                        break;
                }
            }
        }
    }
}
