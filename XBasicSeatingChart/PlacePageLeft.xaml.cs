using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XBasicSeatingChart;



namespace XBasicSeatingChart
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class RandomizeButtonNameConverter : IValueConverter
    {
        private static readonly CommonVM c = Application.Current.Resources["commonVM"] as CommonVM;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ? "Reset" : "Place randomly";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class PlacePageLeft : ContentView
    {
        public PlacePageLeft()
        {
            InitializeComponent();
        }
    }
}