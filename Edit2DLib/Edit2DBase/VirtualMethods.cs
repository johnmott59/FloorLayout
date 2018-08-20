namespace Edit2DLib
{
    public partial class Edit2DBase
    {
  
        /*
         * Sub classes will override these methods to implement mouse operations on their canvas
         */
        public virtual eMouseDownCapture MouseDown(int ScreenMouseX, int ScreenMouseY)
        {
            return eMouseDownCapture.Nothing;
        }

        public virtual bool MouseMove(eMouseState MouseState, int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            return false;
        }

        public virtual void MouseUp(int ScreenMouseX, int ScreenMouseY)
        {

        }
    }
}
