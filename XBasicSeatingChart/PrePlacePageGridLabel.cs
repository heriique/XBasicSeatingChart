using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XBasicSeatingChart
{
    internal class PrePlaceNameConverter : IValueConverter
    {
        private static readonly CommonVM c = Application.Current.Resources["commonVM"] as CommonVM;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return "artery";
            return (string)value == null ? c.NoDesk : ((string)value).Length == 0 ? c.Available : (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class PrePlacePageGridLabel : GridLabel
    {
        private static readonly PrePlaceNameConverter _prePlaceNameConverter = new PrePlaceNameConverter();
        public PrePlacePageGridLabel(int column, int row) : base(column, row)
        {
            this.SetBinding(PrePlacePageGridLabel.TextProperty, new Binding("DeskName", source: c.Classroom.DeskAt(Column, Row), converter: _prePlaceNameConverter));
            tgr.Tapped += (s, e) =>
            {
                Desk d = c.Classroom.DeskAt(column, row);
                if (d.Active)
                {
                    if (d.IsEmpty())
                    {
                        c.PrePlaceAddName(column, row);
                        //Text = string.IsNullOrEmpty(name) ? c.Available : name;
                    } else
                    {
                        c.PrePlaceRemoveName(column, row);
                        //Text = c.Available;
                    }
                    
                }             
            };
            //GestureRecognizers.Add(tgr);
        }
    }
}
