using System;
using System.Globalization;
using System.Windows.Data;

namespace FANUCRobotTest.UI
{
    public class RobotAlarmIsHistoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool)value switch
            {
                true => "E",
                false => "",
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
