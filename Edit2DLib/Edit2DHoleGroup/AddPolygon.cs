#define DOTNET
using System;
using System.Drawing;
using ShapeTemplateLib;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DHoleGroup
    {
        // Add a polygon. This is managed with a simplelayout object 
        public void AddPolygon(int ScreenOffsetX, int ScreenOffsetY, int InitialSides, int Radius)
        {
            if (MostRecentlySelectedHoleGroup == null) return;

            BoundaryPolygon oPolygon = new BoundaryPolygon();

            oPolygon.PointList = new Point3D[InitialSides];

            float Angle = 0;
            float AngleDelta = (float) Math.PI * 2 / InitialSides;
            for (int i=0; i < InitialSides; i++)
            {
                Point3D p = new Point3D();
#if DOTNET
                float x = (float)Math.Cos(Angle) * Radius;
                float y = (float)Math.Sin(Angle) * Radius;
#else
                float x = (float) Math.cos(Angle) * Radius;
                float y = (float) Math.sin(Angle) * Radius;
#endif
                p.X = x;
                p.Y = y;

                oPolygon.PointList[i] = p;

                Angle += AngleDelta;
            }

            /*
             * Create a hole and boundary polygon object
             */
            PointF WorldOffset = this.S2W(ScreenOffsetX, ScreenOffsetY);
            LayoutHole oHole = new LayoutHole();
            oHole.HoleType = "poly";
            oHole.HoleTypeIndex = BoundaryPolygonList.Count;
            oHole.OffsetX = WorldOffset.X;
            oHole.OffsetY = WorldOffset.Y;

            AddHoleToHoleGroup(MostRecentlySelectedHoleGroup, oHole);

            CurrentlySelectedHole = oHole;
            MostRecentlySelectedHole = oHole;

            BoundaryPolygonList.Add(oPolygon);

            // trigger redraw

            DrawShapes();

        }
    }
}
