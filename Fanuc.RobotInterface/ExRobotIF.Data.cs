using Fanuc.RobotInterface.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Fanuc.RobotInterface
{
    public partial class ExRobotIF
    {
        public abstract class RobotDataHolderBase<T> : NotifyPropertyBase, IValueHolder<T>, IRobotValueHolder
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private T _RemoteValue;
            public T Value => _RemoteValue;
            object IValueHolder.Value => Value;

            protected IExRobotIF Robot { get; }

            public RobotDataHolderBase(IExRobotIF robot)
            {
                Robot = robot;
            }

            protected virtual void RemoteUpdate(T value) => SetField(ref _RemoteValue, value, nameof(Value));

            public void PullValue() => RemoteUpdate(ReadValue());

            protected abstract T ReadValue();
        }

        public abstract class RobotWritableDataHolderBase<T> : RobotDataHolderBase<T>, IWritableValueHolder<T>, IRobotWritableValueHolder
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private T _LocalValue;
            public new virtual T Value { get => base.Value; set => _LocalUpdate(value); }
            T IWritableValueHolder<T>.Value { get => Value; set => Value = value; }
            object IWritableValueHolder.Value { get => Value; set => Value = (T)value; }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool _IsDirty;
            public bool IsDirty { get => _IsDirty; }

            public RobotWritableDataHolderBase(IExRobotIF robot) :
                base(robot)
            {
            }

            protected void _LocalUpdate(T value)
            {
                _LocalValue = value;
                SetField(ref _IsDirty, !Equals(_LocalValue, Value), nameof(IsDirty));
            }

            protected override void RemoteUpdate(T value)
            {
                if (!IsDirty)
                    _LocalValue = value;
                base.RemoteUpdate(value);
            }

            public void PushValue()
            {
                if (IsDirty)
                {
                    WriteValue(_LocalValue);
                    SetField(ref _IsDirty, false, nameof(IsDirty));
                }
            }

            protected abstract void WriteValue(T value);
        }

        internal int AssignmentOffset { get; set; } = 1;
        public Task<int> AsyncWriteAssignment(int length, string target) =>
            _InvokeIfConnected(() =>
            {
                _Robot.WriteCommand($"SETASG {AssignmentOffset} {length} {target}");
                var n = AssignmentOffset;
                AssignmentOffset += length;
                return n;
            });

        public int WriteAssignment(int length, string target) => AsyncWriteAssignment(length, target).Result;

        public interface IRobotComplexDataHolder
        {
            void WriteAssignment();
        }

        public abstract class RobotComplexDataHolderBase<T> : RobotDataHolderBase<T>, IRobotComplexDataHolder
        {
            protected int Offset { get; set; } = -1;

            public RobotComplexDataHolderBase(IExRobotIF robot) :
                base(robot)
            {
            }

            public abstract void WriteAssignment();
        }

        public abstract class RobotWritableComplexDataHolderBase<T> : RobotWritableDataHolderBase<T>, IRobotComplexDataHolder
        {
            protected int Offset { get; set; } = -1;

            protected RobotWritableComplexDataHolderBase(IExRobotIF robot) :
                base(robot)
            {
            }

            public abstract void WriteAssignment();
        }

        private IEnumerable<IRobotWritableValueHolder> AllWritableSignals
        {
            get
            {
                foreach (var v in SDI) yield return v.Value;
                foreach (var v in SDO) yield return v.Value;
                foreach (var v in RDI) yield return v.Value;
                foreach (var v in RDO) yield return v.Value;
                foreach (var v in UI) yield return v.Value;
                foreach (var v in UO) yield return v.Value;
                foreach (var v in SI) yield return v.Value;
                foreach (var v in SO) yield return v.Value;
                foreach (var v in WI) yield return v.Value;
                foreach (var v in WO) yield return v.Value;
                foreach (var v in WSI) yield return v.Value;
                foreach (var v in PMC_K) yield return v.Value;
                foreach (var v in PMC_R) yield return v.Value;

                foreach (var v in GI) yield return v.Value;
                foreach (var v in GO) yield return v.Value;
                foreach (var v in AI) yield return v.Value;
                foreach (var v in AO) yield return v.Value;
                foreach (var v in PMC_D) yield return v.Value;

                foreach (var v in R) yield return v.Value;
                foreach (var v in PR) yield return v.Value;
                foreach (var v in SR) yield return v.Value;

                foreach (var v in SysVarI) yield return v.Value;
                foreach (var v in SysVarS) yield return v.Value;
                foreach (var v in SysVarP) yield return v.Value;
            }
        }

        private IEnumerable<IRobotValueHolder> AllReadableSignals
        {
            get
            {
                foreach (var v in AllWritableSignals) yield return v;
                foreach (var v in POS) yield return v.Value;
                foreach (var v in PRG) yield return v.Value;
                foreach (var v in ALM) yield return v.Value;
            }
        }
    }
}