using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanuc.RobotInterface.Collections
{
    internal partial class RobotDataDictionary<TKey, TValue> : IRobotDataDictionary<TKey, TValue>
    {
        private readonly ObservableConcurrentDictionary<TKey, TValue> _Items = new ObservableConcurrentDictionary<TKey, TValue>();

        public TValue this[TKey key] { get => GetOrAdd(key); }

        public IEnumerable<TKey> Keys => _Items.Keys;
        public IEnumerable<TValue> Values => _Items.Values;
        public int Count { get; }

        private readonly Func<TKey, TValue> _Factory;

        public RobotDataDictionary(Func<TKey, TValue> factory)
        {
            _Factory = factory;

            _Items.CollectionChanged += _Items_CollectionChanged;
            _Items.PropertyChanged += _Items_PropertyChanged;
        }

        private void _Items_PropertyChanged(object sender, PropertyChangedEventArgs e) => RaisePropertyChanged(e);
        private void _Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => RaiseCollectionChanged(e);

        public bool ContainsKey(TKey key) => _Items.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value) => _Items.TryGetValue(key, out value);
        public TValue GetOrAdd(TKey key)
        {
            if (_Items.ContainsKey(key))
                return _Items[key];
            lock (_Items)
            {
                if (_Items.ContainsKey(key))
                    return _Items[key];

                var v = CreateValue(key);
                _Items[key] = v;

                return v;
            }
        }
        public bool Remove(TKey key) => _Items.Remove(key);
        public void Clear()
        {
            foreach (var key in _Items.Keys)
                _Items.Remove(key);
        }

        protected virtual TValue CreateValue(TKey key) => _Factory.Invoke(key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _Items.ToList().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
