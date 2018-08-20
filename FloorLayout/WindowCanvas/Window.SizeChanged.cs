using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FloorLayout
{

    public partial class WindowCanvas 
    {
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Get the grid cell that the canvas is in and resize it

            layoutCanvas.Width = MainGrid.ColumnDefinitions[1].ActualWidth;
            layoutCanvas.Height = MainGrid.RowDefinitions[1].ActualHeight;
        }
    }

}
