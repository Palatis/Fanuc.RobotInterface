using Fanuc.RobotInterface;
using System;
using System.Globalization;
using System.Windows.Data;

namespace FANUCRobotTest.UI
{
    public class RobotTaskTypeToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (RobotTaskType)value switch
            {
                RobotTaskType.All => "",
                RobotTaskType.IgnoreMacro => "M",
                RobotTaskType.IgnoreKarel => "K",
                RobotTaskType.IgnoreMacroKarel => "MK",
                _ => throw new NotImplementedException(),
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
