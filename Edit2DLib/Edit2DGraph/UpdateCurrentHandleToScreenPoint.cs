using System.Drawing;

namespace Edit2DLib
{
    public partial class Edit2DGraph
    {

        public eOperationStatus UpdateCurrentHandleToScreenPoint(int ScreenMouseX, int ScreenMouseY)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            if (MostRecentlySelectedLayer.CurrentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            PointF pWorld = this.S2W(ScreenMouseX, ScreenMouseY);

            float newx = pWorld.X;
            float newy = pWorld.Y;

            // Update the world points to the nearest grid
            if ( this.GridSize > 1)
            {
                newx = this.RoundToGrid((int)pWorld.X);
                newy = this.RoundToGrid((int)pWorld.Y);
            }

            MostRecentlySelectedLayer.CurrentlySelectedVertex.X = (float) newx;
            MostRecentlySelectedLayer.CurrentlySelectedVertex.Y = (float) newy;    

            return eOperationStatus.OK;
        }

    }
}
