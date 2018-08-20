namespace Edit2DLib
{
    public partial class Edit2DBase
    {
        public void BaseMouseUp(int ScreenMouseX, int ScreenMouseY)
        {
            // pass this to the draw operation object
            MouseUp(ScreenMouseX, ScreenMouseY);
            /*
             * Reset mouse state and cursor
             */
            MouseState = eMouseState.Nothing;
        }
    }
}
