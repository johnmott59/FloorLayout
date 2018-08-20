using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;

namespace FloorLayout
{
    public partial class WindowCanvas
    {
        private void layoutCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Mouse up ");
            oViewModel.MouseUp(e);
        }
    }

}
