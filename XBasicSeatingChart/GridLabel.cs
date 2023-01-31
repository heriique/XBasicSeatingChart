using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XBasicSeatingChart
{
    internal class OpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1.0 : 0.5; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class GridLabel : Label
    {
        public int Column;
        public int Row;
        protected static readonly CommonVM c = Application.Current.Resources["commonVM"] as CommonVM;
        private static readonly OpacityConverter _opacityConverter = new OpacityConverter();

        protected TapGestureRecognizer tgr;

        public GridLabel(int column, int row)
        {
            Column = column;
            Row = row;
            this.SetBinding(BackgroundProperty, "ButtonColorRight");
            this.SetBinding(TextColorProperty, "FontColor");
            this.SetBinding(GridLabel.OpacityProperty, new Binding("Active", source: GetDesk(), converter: _opacityConverter));
            VerticalTextAlignment = TextAlignment.Center;
            HorizontalTextAlignment = TextAlignment.Center;
            tgr = new TapGestureRecognizer();
            GestureRecognizers.Add(tgr);
        }

        public Desk GetDesk()
        {
            return c.Classroom.DeskAt(Column, Row);
        }
    }
}
