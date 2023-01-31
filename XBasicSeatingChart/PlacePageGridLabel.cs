using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XBasicSeatingChart
{
    internal class PlaceNameConverter : IValueConverter
    {
        private static readonly CommonVM c = Application.Current.Resources["commonVM"] as CommonVM;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == null ? String.Empty : ((string)value).Length == 0 ? c.Available : (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class PlacePageGridLabel : GridLabel
    {
        private static readonly PlaceNameConverter _placeNameConverter = new PlaceNameConverter();
        public PlacePageGridLabel(int column, int row) : base(column, row)
        {
            this.SetBinding(PlacePageGridLabel.TextProperty, new Binding("DeskName", source: c.Classroom.DeskAt(Column, Row), converter: _placeNameConverter));
        }
    }
}
