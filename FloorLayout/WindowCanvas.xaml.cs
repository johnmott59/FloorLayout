using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace FloorLayout
{
    /// <summary>
    /// Interaction logic for CanvasWindow.xaml
    /// </summary>
    public partial class WindowCanvas : Window
    {
        ViewModelCanvas oViewModel;
        public WindowCanvas()
        {
            InitializeComponent();

            // Hook up to viewmodel, pass in the canvas
            oViewModel = new ViewModelCanvas(layoutCanvas);
          
            this.DataContext = oViewModel;
        }

 
    }
}
