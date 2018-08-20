#define DOTNET
using System.Drawing;
namespace Edit2DLib
{
    public partial class Edit2DBase
    {
#if DOTNET
        // For .NET we need to convert the string to a pen color
        public Pen GetColor(string color,float width)
        {
            /*
             * The string for the color will be in one of two formats; either #RRGGBB or rgb(r,g,b,a)
             */
            if (color[0] == '#')
            {
                string red = color.Substring(1, 2);
                string green = color.Substring(3, 2);
                string blue = color.Substring(5, 2);

                int ired = int.Parse(red, System.Globalization.NumberStyles.HexNumber);
                int igreen = int.Parse(green, System.Globalization.NumberStyles.HexNumber);
                int iblue = int.Parse(blue, System.Globalization.NumberStyles.HexNumber);

                return new Pen(Color.FromArgb(ired, igreen, iblue), width);
            }

            return Pens.Black;
        }
#endif
    }
}
