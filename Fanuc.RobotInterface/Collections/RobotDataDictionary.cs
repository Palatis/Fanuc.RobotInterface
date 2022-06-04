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
    public interface IRobotDataDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        TValue GetOrAdd(TKey key);
        bool Remove(TKey key);
        void Clear();
    }

    internal partial class RobotDataDictionary<TKey, TValue> : IRobotDataDictionary<TKey, TValue>
    {
        private readonly ObservableConcurrentDictionary<TKey, TValue> _Items = new();

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

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _Items.TryGetValue(key, out value);
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

    //internal class RobotDigitalSignalDictionary : RobotDataDictionary<int, ExRobotIF.RobotSignalHolderBase<bool>>
    //{
    //    private readonly Dictionary<int, int> _Ranges = new();

    //    private readonly Func<int, int, bool[]> _ValueReader;

    //    public RobotDigitalSignalDictionary(Func<int, ExRobotIF.RobotSignalHolderBase<bool>> factory, Func<int, int, bool[]> reader) :
    //        base(factory)
    //    {
    //        _ValueReader = reader;
    //    }

    //    protected override ExRobotIF.RobotSignalHolderBase<bool> CreateValue(int key)
    //    {
    //        var holder = base.CreateValue(key);
    //        Repack();
    //        return holder;
    //    }

    //    private void Repack()
    //    {
    //        _Ranges.Clear();
    //        if (Count == 0)
    //            return;

    //        var offsets = Values.Select(v => v.Offset + v.Index - 1).OrderBy(v => v);
    //        var first = offsets.First();
    //        var last = offsets.Last();

    //        if (first < 0)
    //            throw new ArgumentOutOfRangeException(nameof(first), "First index cannot be < 0");

    //        var count = last - first + 1;

    //        int f = first / 8 * 8;
    //        int l = (last + 7) / 8 * 8;
    //        int len = last - first;

    //        var bools = new bool[len];

    //        foreach (var offset in offsets)
    //            bools[offset - f] = true;

    //        for (int i = 0; i < len;)
    //        {
    //            if (!bools[i])
    //            {
    //                ++i;
    //                continue;
    //            }

    //            int j = i;
    //            for (; bools[j]; ++j) ;
    //            _Ranges[f + i + 1] = j - i;
    //        }
    //    }

    //    public void PullValues()
    //    {
    //        // values are pushed one-by-one to avoid intefer with other bits in the same byte chunk.
    //        foreach (var v in Values)
    //            v.PushValue();

    //        foreach (var range in _Ranges)
    //        {
    //            var values = _ValueReader.Invoke(range.Key, range.Value);
    //            for (int i = range.Key;i < range.Key + range.Value; ++i)
    //            {

    //            }
    //        }
    //    }
    //}
}
