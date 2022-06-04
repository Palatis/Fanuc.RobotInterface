using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Fanuc.RobotInterface.Collections
{
    internal partial class RobotDataDictionary<TKey, TValue> : INotifyPropertyChanged, INotifyCollectionChanged
    {
        private static readonly Dictionary<string, PropertyChangedEventArgs> _ChangedEventArgsCache = new();

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, _GetChangedEventArgs(name));
        protected void RaisePropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string name = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            RaisePropertyChanged(name);
            return true;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);
    }
}
