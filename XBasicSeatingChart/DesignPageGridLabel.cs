using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XBasicSeatingChart
{
    internal class DesignNameConverter : IValueConverter
    {
        private static readonly CommonVM c = Application.Current.Resources["commonVM"] as CommonVM;
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == null ? c.NoDesk : c.Available;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class DesignPageGridLabel : GridLabel
    {
        private static readonly DesignNameConverter _designNameConverter = new DesignNameConverter();
        public DesignPageGridLabel(int column, int row) : base(column, row)
        {
            this.SetBinding(GridLabel.TextProperty, new Binding("DeskName", source: c.Classroom.DeskAt(Column, Row), converter: _designNameConverter));

            tgr.Tapped += (s, e) => c.SwapActive(column, row);;
        }
    }
}
