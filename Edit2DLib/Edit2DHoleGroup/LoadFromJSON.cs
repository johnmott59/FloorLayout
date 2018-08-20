#define DOTNET
using System.Collections.Generic;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {

        public void LoadFromJSON(
            HoleGroup[] HoleGroupArray,
            BoundaryRectangle[] RectangleArray,
            BoundaryEllipse[] EllipseArray, 
            BoundaryPolygon[] PolygonArray)
        {

            HoleGroupList = new List<HoleGroup>();
            for (int i=0; i < HoleGroupArray.Length; i++)
            {
                HoleGroup hg = new HoleGroup();

                hg.HoleGroupID = HoleGroupArray[i].HoleGroupID;
                /*
                 * When the objects get here in JS the holelist is no longer a list type, its an array type because
                 * of serialization. 
                 */
                int len = GetHoleListLength(HoleGroupArray[i].HoleList);
                for (int j = 0; j < len; j++)
                {
                    LayoutHole oHoleToAdd = HoleGroupArray[i].HoleList[j];

                    AddHoleToHoleGroup(hg, oHoleToAdd);
                }

                HoleGroupList.Add(hg);
            }

            BoundaryRectangleList = new List<BoundaryRectangle>();
            for (int i=0; i < RectangleArray.Length; i++)
            {
                BoundaryRectangleList.Add(RectangleArray[i]);
            }

            BoundaryEllipseList = new List<BoundaryEllipse>();
            for (int i = 0; i < EllipseArray.Length; i++)
            {
                BoundaryEllipseList.Add(EllipseArray[i]);
            }

            BoundaryPolygonList = new List<BoundaryPolygon>();
            for (int i = 0; i < PolygonArray.Length; i++)
            {
                BoundaryPolygonList.Add(PolygonArray[i]);
            }
            /*
             * Now flip all of the Y points so that positive Y values go 'up'
             */
            InvertHolePoints();
        }

    }
}
