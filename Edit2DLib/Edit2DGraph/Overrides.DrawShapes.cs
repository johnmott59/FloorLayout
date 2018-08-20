namespace Edit2DLib
{
    public partial class Edit2DGraph
    {

        // ---------------------------------------------------
        public override void DrawShapes()
        {
            if (SubControl == 0)
            {
                this.Clear("#FFFFFF");

                DrawGrid();

                DrawAxis();
            }

            for (int i=0; i < Edit2dGraphLayerList.Count; i++)
            {
                Edit2DGraphLayer olayer = Edit2dGraphLayerList.GetFrom(i);

                DrawShapes_Layer(olayer, olayer == MostRecentlySelectedLayer,bShowHandles);
            }

         }
    }
}
