using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Fanuc.RobotInterface
{
    public abstract class NotifyPropertyBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private static readonly ObservableConcurrentDictionary<string, PropertyChangedEventArgs> _ChangedEventArgsCache = new ObservableConcurrentDictionary<string, PropertyChangedEventArgs>();
        private static readonly ObservableConcurrentDictionary<string, PropertyChangingEventArgs> _ChangingEventArgsCache = new ObservableConcurrentDictionary<string, PropertyChangingEventArgs>();

        private static PropertyChangedEventArgs _GetChangedEventArgs(string name)
        {
            var e = _ChangedEventArgsCache.ContainsKey(name) ? _ChangedEventArgsCache[name] : null;
            if (e == null)
            {
                e = new PropertyChangedEventArgs(name);
                _ChangedEventArgsCache.Add(name, e);
            }
            return e;
        }
        private static PropertyChangingEventArgs _GetChangingEventArgs(string name)
        {
            var e = _ChangingEventArgsCache.ContainsKey(name) ? _ChangingEventArgsCache[name] : null;
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