using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FANUCRobotTest
{
    public abstract class NotifyPropertyBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static readonly Dictionary<string, PropertyChangedEventArgs> _ChangedEventArgsCache = new Dictionary<string, PropertyChangedEventArgs>();
        private static readonly Dictionary<string, PropertyChangingEventArgs> _ChangingEventArgsCache = new Dictionary<string, PropertyChangingEventArgs>();

        private static PropertyChangedEventArgs _GetChangedEventArgs(string name)
        {
            var e = _ChangedEventArgsCache.GetValueOrDefault(name, null);
            if (e == null)
            {
                e = new PropertyChangedEventArgs(name);
                _ChangedEventArgsCache.Add(name, e);
            }
            return e;
        }
        private static PropertyChangingEventArgs _GetChangingEventArgs(string name)
        {
            var e = _ChangingEventArgsCache.GetValueOrDefault(name, null);
            if (e == null)
            {
                e = new PropertyChangingEventArgs(name);
                _ChangingEventArgsCache.Add(name, e);
            }
            return e;
        }

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, _GetChangedEventArgs(name));
        protected void RaisePropertyChanging([CallerMemberName] string name = "") => PropertyChanging?.Invoke(this, _GetChangingEventArgs(name));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string name = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            RaisePropertyChanging(name);
            field = value;
            RaisePropertyChanged(name);
            return true;
        }
    }
}
