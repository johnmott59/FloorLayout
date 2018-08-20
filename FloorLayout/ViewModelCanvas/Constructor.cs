using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using Edit2DLib;
using System.Xml.Linq;

namespace FloorLayout
{
    /// <summary>
    /// ViewModel
    /// </summary>
    public partial class ViewModelCanvas : ViewModelRoot
    {

        public ViewModelCanvas(Canvas oCanvas)
        {
            // Save the canvas
            this.oCanvas = oCanvas;
            oDrawPrimitives = new DrawPrimitives() { oCanvas = oCanvas };
        }


        private string _SomeText = "";
        public string SomeText
        {
            get
            {
                return _SomeText;
            }
            set
            {
                _SomeText = value;
                RaisePropertyChangedEvent("SomeText");
            }
        }


    }

}


#if false

        // binding to a button takes 4 steps:

    1. Create a class containing the routines TheCommandBinder and TheCommand, specified below. This class
        becomes the 'ViewModel' class. Set an instance of this class to the DataContext of the window where
        the buttons go

        this.DataContext = new oViewModel();

    2.  Declare the binding in the button definition to a function declared in the view model class

        <Button Name="btnLoad" Command="{Binding TheCommandBinder}" Margin="5">Load Floor</Button>
        
    3. Create the function that the button 'binds' to. This is called only once during initialization to get the 
            address of the function to call when the button is pressed. DeleteCommand is a class to wrap calling
            the work function.

        public ICommand TheCommandBinder
        {
            get { return new DelegateCommand(TheCommand); }
        }

    4. Create the routine that will do the work. This routine is called when the button is pressed. In the old
            style this would be the event handler.

        // Called by the form when the button is pressed
        private void TheCommand()
        {
            // do something
        }

#endif