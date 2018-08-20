namespace Edit2DLib
{

    public partial class Edit2DHoleGroup
    {

        public eOperationStatus UpdateCurrentHole(int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            if (CurrentlySelectedHole == null) return eOperationStatus.NoVertexSelected;

            switch (CurrentlySelectedHole.HoleType)
            {
                case "rect":
                    UpdateRectangle(ScreenMouseX, ScreenMouseY, ScreenDeltaX, ScreenDeltaY);
                    break;
                case "poly":
                    UpdatePolygon(ScreenMouseX, ScreenMouseY, ScreenDeltaX, ScreenDeltaY);
                    break;
                case "ell":
                    UpdateEllipse(ScreenMouseX, ScreenMouseY, ScreenDeltaX, ScreenDeltaY);
                    break;
            }
   

            return eOperationStatus.OK;
        }


    }
}
