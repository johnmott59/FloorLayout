using System.Collections.Generic;
using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        /// <summary>
        /// If there is a current handle see if we dropped it over another handle, and if so merge these points
        /// </summary>
        /// <param name="ScreenMouseX"></param>
        /// <param name="ScreenMouseY"></param>
        /// <returns></returns>
        public eOperationStatus TryMergeVertex(int ScreenMouseX, int ScreenMouseY) 
        {

            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            if (MostRecentlySelectedLayer.CurrentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            // Get all handles that are close to this point. The current handle will be one of them
            // There could conceivable be many handles if they are close. Pick the first handle that 
            // isn't the current handle and merge into that

            // get the first point that is within 10 pixels of the currently selected vertex
            // we want to set the x,y of the current vertex to the x,y of the closest vertex

            List<Vertex> Vertexlist = FindVertextListAtScreenPoint(ScreenMouseX, ScreenMouseY);

            // Pick the first one that isn't the currently selected edge

            Vertex MergeToVertex = null;
            for (int i=0; i < Vertexlist.Count; i++)
            {
                Vertex v = Vertexlist.GetFrom(i);
                if (v == MostRecentlySelectedLayer.CurrentlySelectedVertex) continue;

                MergeToVertex = v;
                break;

            }

            // If there were no other vertices close we're done
            if (MergeToVertex == null) return eOperationStatus.OK;

            /*
             * Replace all instances of the currently selected vertex with the merged vertex in existing edges
             */
            for (int i=0; i < MostRecentlySelectedLayer.EdgeList.Count; i++)
            {
                Edge oEdge = MostRecentlySelectedLayer. EdgeList.GetFrom(i);
                if (oEdge.p1 == MostRecentlySelectedLayer.CurrentlySelectedVertex.Index)
                {
                    oEdge.p1 = MergeToVertex.Index;
                }

                if (oEdge.p2 == MostRecentlySelectedLayer.CurrentlySelectedVertex.Index)
                {
                    oEdge.p2 = MergeToVertex.Index;
                }
            }

            // delete the currently selected vertex

            for (int i=0; i < MostRecentlySelectedLayer.VertexList.Count; i++)
            {
                Vertex v = MostRecentlySelectedLayer.VertexList.GetFrom(i);
                if (v.Index == MostRecentlySelectedLayer.CurrentlySelectedVertex.Index)
                {
                    MostRecentlySelectedLayer.VertexList.RemoveAt(i);
                    break;
                }
            }

            // Set the most recently selected vertex to the merge vertex

            MostRecentlySelectedLayer.MostRecentlySelectedVertex = MergeToVertex;

            return eOperationStatus.OK;

        }   
    }
}
