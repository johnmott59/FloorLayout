namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public void PopLayer()
        {
            // Remove the layer at the end of the list. this is like a stack where the 'top' is the end of the list

            if (Edit2dGraphLayerList.Count == 0) return;

            int ndx = Edit2dGraphLayerList.Count - 1;
            Edit2dGraphLayerList.RemoveAt(ndx);

            if (Edit2dGraphLayerList.Count == 0)
            {
                MostRecentlySelectedLayer = null;
            } else {
                MostRecentlySelectedLayer = Edit2dGraphLayerList.GetFrom(Edit2dGraphLayerList.Count - 1);
            }

            DrawShapes();
        }
    }
}
