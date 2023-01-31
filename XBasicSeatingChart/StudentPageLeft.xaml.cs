using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XBasicSeatingChart
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentPageLeft : ContentView
    {
        public StudentPageLeft()
        {
            InitializeComponent();
            entryName.Completed += (sender, e) => EntryCompleted(sender, e);
            
        }

        void EntryCompleted(object sender, EventArgs e)
        {
            if (((CommonVM)BindingContext).AddName.CanExecute(sender))
                ((CommonVM)BindingContext).AddName.Execute(sender);
        }
    }  
}