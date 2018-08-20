using ShapeTemplateLib.Templates.User0;

namespace Edit2DLib
{
    public partial class Edit2DGraphLayer
    {
        protected int FindNextIndex()
        {
            // add two new vertices. Find the next available index
            int highestindex = -1;
            for (int i = 0; i < VertexList.Count; i++)
            {
                Vertex vTmp = VertexList.GetFrom(i);
                if (vTmp.Index > highestindex)
                {
                    highestindex = vTmp.Index;
                }
            }
            int nextindex = highestindex + 1;
            return nextindex;

        } 
    }
}
