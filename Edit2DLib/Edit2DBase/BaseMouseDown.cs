namespace Edit2DLib
{
    public partial class Edit2DBase
    {
        public eMouseDownCapture BaseMouseDown(int ScreenMouseX, int ScreenMouseY)
        {
            // Indicate new mouse state
            MouseState = eMouseState.MouseDown;
            // Call the override mouse down method

            return MouseDown(ScreenMouseX, ScreenMouseY);
        }
    }
}
