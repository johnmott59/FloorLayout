namespace Edit2DLib
{
    /*
     * This object contains entry points for views to draw items. Its initialized and provided to the draw routine.
     * It will be overridden in Javascript and in .net with code particular to those environments
     */
    public partial class Edit2DBase
    {
        /*
         * Starting routine to draw shapes. This will be overridden by view objects to perform drawing,
         * and those classes will call methods in this class to do the drawing
         */
        public virtual void DrawShapes()
        {

        }

    }
}
