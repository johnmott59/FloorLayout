using System.Collections.Generic;
using System;
using System.Drawing;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        /// <summary>
        /// Find all handles that are close to the current screen position
        /// </summary>
        /// <param name="ScreenMouseX"></param>
        /// <param name="ScreenMouseY"></param>
        /// <returns></returns>
        /// 
        public List<Vertex> FindVertextListAtScreenPoint(int ScreenMouseX,int ScreenMouseY)
        {
            List<Vertex> list = new List<Vertex>();

            if (MostRecentlySelectedLayer == null) return list;

            for (int i=0; i < MostRecentlySelectedLayer.VertexList.Count; i++)
            {
                Vertex v = MostRecentlySelectedLayer.VertexList.GetFrom(i);               
                PointF ScreenCoordinates = W2S(v.X,v.Y);

                double distance = Math.Sqrt(
                     (ScreenMouseX - ScreenCoordinates.X) * (ScreenMouseX - ScreenCoordinates.X) +
                     (ScreenMouseY - ScreenCoordinates.Y) * (ScreenMouseY - ScreenCoordinates.Y));

                if (distance < 10)
                {
                    list.Add(v);
                }
            }

            return list;
        }
    }
}
