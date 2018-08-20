namespace Edit2DLib
{
    public partial class Edit2DGraph
    {
        public void PushLayer()
        {
            // Insert a new layer at the end of the list. This is like a stack in which the 'top' is the end of the list

            Edit2DGraphLayer oNewLayer = new Edit2DGraphLayer();

            Edit2dGraphLayerList.Add(oNewLayer);

            MostRecentlySelectedLayer = oNewLayer;

            DrawShapes();
        }
      
    }
}
