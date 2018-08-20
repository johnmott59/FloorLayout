using System;
using System.Drawing;
using Edit2DLib;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace FloorLayout
{
    /// <summary>
    /// This class contains the drawing routines
    /// </summary>
    public partial class DrawPrimitives : IDrawPrimitives
    {
        public Canvas oCanvas { get; set; }
        public int ShapeIndex { get; set; } = 0;

        public void StartDrawing(bool ClearCanvas = false)
        {
            if (ClearCanvas) oCanvas.Children.Clear();
            ShapeIndex = 0;
        }


        public void DrawEllipse(string Color, float CenterX, float CenterY, float Radius)
        {
            Ellipse e;
            if (oCanvas.Children.Count <= ShapeIndex)
            {
                e = new Ellipse();
                oCanvas.Children.Add(e);
            }
            else
            {
                e = (Ellipse)oCanvas.Children[ShapeIndex];
            }

            e.Stroke = GetBrushColor(Color);
            e.Width = Radius;
            e.Height = Radius;
            e.Fill = System.Windows.Media.Brushes.Transparent;

            // Set the position using the canvas
            Canvas.SetLeft(e, CenterX - Radius/2);
            Canvas.SetTop(e, CenterY - Radius/2);

            ShapeIndex++;

        }

        public void DrawLine(string Color, float lineWidth, PointF From, PointF To)
        {
            Line line;
            if (oCanvas.Children.Count <= ShapeIndex)
            {
                line = new Line();
                oCanvas.Children.Add(line);
            }
            else
            {
                line = (Line) oCanvas.Children[ShapeIndex];
            }

            line.Stroke = GetBrushColor(Color);
            line.StrokeThickness = lineWidth;
            line.X1 = From.X;
            line.Y1 = From.Y;
            line.X2 = To.X;
            line.Y2 = To.Y;
            line.Fill = System.Windows.Media.Brushes.Transparent;

            ShapeIndex++;
        }

        public void DrawRectangle(string Color, float width, float height, float CenterX, float CenterY)
        {
            System.Windows.Shapes.Rectangle r;

            if (oCanvas.Children.Count <= ShapeIndex)
            {
                r = new System.Windows.Shapes.Rectangle();
                oCanvas.Children.Add(r);
            }
            else
            {
                r = (System.Windows.Shapes.Rectangle )oCanvas.Children[ShapeIndex];
            }

            r.Stroke = GetBrushColor(Color);
            r.Width = width;
            r.Height = height;
            r.Fill = System.Windows.Media.Brushes.Transparent;

            // Set the position using the canvas
            Canvas.SetLeft(r, CenterX - width / 2);
            Canvas.SetTop(r, CenterY + height / 2);

            ShapeIndex++;

        }


        // For .NET we need to convert the string to a pen color
        public System.Windows.Media.Brush GetBrushColor(string color)
        {
            /*
             * The string for the color will be in one of two formats; either #RRGGBB or rgb(r,g,b,a)
             */
            if (color[0] == '#')
            {
                string red = color.Substring(1, 2);
                string green = color.Substring(3, 2);
                string blue = color.Substring(5, 2);

                byte ired = (byte) int.Parse(red, System.Globalization.NumberStyles.HexNumber);
                byte igreen = (byte) int.Parse(green, System.Globalization.NumberStyles.HexNumber);
                byte iblue = (byte) int.Parse(blue, System.Globalization.NumberStyles.HexNumber);

                var converter = new System.Windows.Media.BrushConverter();
                System.Windows.Media.Brush b = (System.Windows.Media.Brush) converter.ConvertFromString(color);
                return b;
            }

            return System.Windows.Media.Brushes.Black;
        }


    }
}
