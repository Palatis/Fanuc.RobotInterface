using Fanuc.RobotInterface;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;

namespace FANUCRobotTest.UI
{
    public class PositionConverter : IValueConverter
    {
        private static readonly Regex sJointPattern = new Regex(@"\((?<J1>.*),(?<J2>.*),(?<J3>.*),(?<J4>.*),(?<J5>.*),(?<J6>.*)\)\s*\[(?<J7>.*),(?<J8>.*),(?<J9>.*)\]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex sCartisianPattern = new Regex(@"\((?<X>.*),(?<Y>.*),(?<Z>.*),(?<W>.*),(?<P>.*),(?<R>.*)\)\s*\[(?<E1>.*),(?<E2>.*),(?<E3>.*)\]\s*\{\s*(?<CONFIG>[NF][RL][DU][BT])\s*,(?<T1>.*),(?<T2>.*),(?<T3>.*)\}", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            $"{value}";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = $"{value}";
            if (string.IsNullOrWhiteSpace(str))
                return null;

            switch ($"{parameter}")
            {
                case "J":
                    {
                        var m = sJointPattern.Match(str);
                        if (!m.Success)
                            return new ValidationResult(false, $"Cannot convert to Joint position.");

                        var pj = new PositionJoint();
                        try { pj.J1 = System.Convert.ToSingle(m.Groups["J1"].Value); } catch { }
                        try { pj.J2 = System.Convert.ToSingle(m.Groups["J2"].Value); } catch { }
                        try { pj.J3 = System.Convert.ToSingle(m.Groups["J3"].Value); } catch { }
                        try { pj.J4 = System.Convert.ToSingle(m.Groups["J4"].Value); } catch { }
                        try { pj.J5 = System.Convert.ToSingle(m.Groups["J5"].Value); } catch { }
                        try { pj.J6 = System.Convert.ToSingle(m.Groups["J6"].Value); } catch { }
                        try { pj.J7 = System.Convert.ToSingle(m.Groups["J7"].Value); } catch { }
                        try { pj.J8 = System.Convert.ToSingle(m.Groups["J8"].Value); } catch { }
                        try { pj.J9 = System.Convert.ToSingle(m.Groups["J9"].Value); } catch { }
                        return pj;
                    }
                case "C":
                    {
                        var m = sCartisianPattern.Match(str);
                        if (!m.Success)
                            return new ValidationResult(false, $"Cannot convert to Cartisian position.");

                        var pc = new PositionCartisian();
                        try { pc.X = System.Convert.ToSingle(m.Groups["X"].Value); } catch { }
                        try { pc.Y = System.Convert.ToSingle(m.Groups["Y"].Value); } catch { }
                        try { pc.Z = System.Convert.ToSingle(m.Groups["Z"].Value); } catch { }
                        try { pc.W = System.Convert.ToSingle(m.Groups["W"].Value); } catch { }
                        try { pc.P = System.Convert.ToSingle(m.Groups["P"].Value); } catch { }
                        try { pc.R = System.Convert.ToSingle(m.Groups["R"].Value); } catch { }
                        try { pc.E1 = System.Convert.ToSingle(m.Groups["E1"].Value); } catch { }
                        try { pc.E2 = System.Convert.ToSingle(m.Groups["E2"].Value); } catch { }
                        try { pc.E3 = System.Convert.ToSingle(m.Groups["E3"].Value); } catch { }
                        try { pc.Config = (CartisianConfig)Enum.Parse(typeof(CartisianConfig), m.Groups["CONFIG"].Value); } catch { }
                        try { pc.T4 = System.Convert.ToInt16(m.Groups["T1"].Value); } catch { }
                        try { pc.T5 = System.Convert.ToInt16(m.Groups["T2"].Value); } catch { }
                        try { pc.T6 = System.Convert.ToInt16(m.Groups["T3"].Value); } catch { }
                        return pc;
                    }
            }
            throw new NotImplementedException();
        }
    }
}
