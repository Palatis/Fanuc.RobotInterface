using System;
using System.Collections.Generic;

namespace Fanuc.RobotInterface
{
    public interface IPosition { }

    public class PositionJoint : IPosition, IEquatable<PositionJoint>
    {
        public float J1 { get; set; }
        public float J2 { get; set; }
        public float J3 { get; set; }
        public float J4 { get; set; }
        public float J5 { get; set; }
        public float J6 { get; set; }

        public float J7 { get; set; }
        public float J8 { get; set; }
        public float J9 { get; set; }

        public bool Equals(PositionJoint other)
        {
            if (other == null)
                return false;
            if (J1 != other.J1) return false;
            if (J2 != other.J2) return false;
            if (J3 != other.J3) return false;
            if (J4 != other.J4) return false;
            if (J5 != other.J5) return false;
            if (J6 != other.J6) return false;
            if (J7 != other.J7) return false;
            if (J8 != other.J8) return false;
            if (J9 != other.J9) return false;
            return true;
        }

        public override string ToString() =>
            $"({J1:0.00}, {J2:0.00}, {J3:0.00}, {J4:0.00}, {J5:0.00}, {J6:0.00}) [{J7:0.00}, {J8:0.00}, {J9:0.00}]";
    }

    public enum CartisianConfig : byte
    {
        Unknown = 0xff,

        Flip = 0b00000001,
        Left = 0b00000010,
        Up = 0b00000100,
        Front = 0b00001000,
        None = 0b00000000,

        NRDB = 0b00000000,
        FRDB = 0b00000001,
        NLDB = 0b00000010,
        FLDB = 0b00000011,
        NRUB = 0b00000100,
        FRUB = 0b00000101,
        NLUB = 0b00000110,
        FLUB = 0b00000111,
        NRDT = 0b00001000,
        FRDT = 0b00001001,
        NLDT = 0b00001010,
        FLDT = 0b00001011,
        NRUT = 0b00001100,
        FRUT = 0b00001101,
        NLUT = 0b00001110,
        FLUT = 0b00001111,
    }

    public class PositionCartisian : IPosition, IEquatable<PositionCartisian>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }
        public float P { get; set; }
        public float R { get; set; }
        public float E1 { get; set; }
        public float E2 { get; set; }
        public float E3 { get; set; }

        public CartisianConfig Config { get; set; } = CartisianConfig.Unknown;

        public short T4 { get; set; }
        public short T5 { get; set; }
        public short T6 { get; set; }

        public bool Equals(PositionCartisian other)
        {
            if (other == null)
                return false;
            if (X != other.X) return false;
            if (Y != other.Y) return false;
            if (Z != other.Z) return false;
            if (W != other.W) return false;
            if (P != other.P) return false;
            if (R != other.R) return false;
            if (E1 != other.E1) return false;
            if (E2 != other.E2) return false;
            if (E3 != other.E3) return false;
            if (Config != other.Config) return false;
            if (T4 != other.T4) return false;
            if (T5 != other.T5) return false;
            if (T6 != other.T6) return false;
            return true;
        }

        public override string ToString() =>
            $"({X:0.00}, {Y:0.00}, {Z:0.00}, {W:0.00}, {P:0.00}, {R:0.00}) [{E1:0.00}, {E2:0.00}, {E3:0.00}] {{{Config}, {T4}, {T5}, {T6}}}";
    }

    public class Position : IPosition, IEquatable<Position>
    {
        public short UserFrame { get; set; }
        public short UserTool { get; set; }
        public PositionJoint Joint { get; set; }
        public PositionCartisian Cartisian { get; set; }

        public bool Equals(Position other)
        {
            if (other == null)
                return false;
            if (UserFrame != other.UserFrame)
                return false;
            if (UserTool != other.UserTool)
                return false;
            if (!EqualityComparer<PositionJoint>.Default.Equals(Joint, other.Joint))
                return false;
            if (!EqualityComparer<PositionCartisian>.Default.Equals(Cartisian, other.Cartisian))
                return false;
            return true;
        }
    }

    public static class PositionConverter
    {
        public static Position ToPosition(byte[] bytes, int start = 0)
        {
            if (bytes == null)
                return null;
            if (bytes.Length < start + 100)
                throw new ArgumentException($"need 100 bytes to convert a position, got only {bytes.Length - start}", nameof(bytes));

            return new Position()
            {
                UserFrame = BitConverter.ToInt16(bytes, start + 90),
                UserTool = BitConverter.ToInt16(bytes, start + 92),
                Cartisian = BitConverter.ToInt16(bytes, start + 50) == 0 ?
                    null :
                    new PositionCartisian()
                    {
                        X = BitConverter.ToSingle(bytes, start + 0),
                        Y = BitConverter.ToSingle(bytes, start + 4),
                        Z = BitConverter.ToSingle(bytes, start + 8),
                        W = BitConverter.ToSingle(bytes, start + 12),
                        P = BitConverter.ToSingle(bytes, start + 16),
                        R = BitConverter.ToSingle(bytes, start + 20),
                        E1 = BitConverter.ToSingle(bytes, start + 24),
                        E2 = BitConverter.ToSingle(bytes, start + 28),
                        E3 = BitConverter.ToSingle(bytes, start + 32),

                        T4 = BitConverter.ToInt16(bytes, start + 44),
                        T5 = BitConverter.ToInt16(bytes, start + 46),
                        T6 = BitConverter.ToInt16(bytes, start + 48),

                        Config =
                            (BitConverter.ToUInt16(bytes, start + 36) != 0 ? CartisianConfig.Flip : CartisianConfig.None) |
                            (BitConverter.ToUInt16(bytes, start + 38) != 0 ? CartisianConfig.Left : CartisianConfig.None) |
                            (BitConverter.ToUInt16(bytes, start + 40) != 0 ? CartisianConfig.Up : CartisianConfig.None) |
                            (BitConverter.ToUInt16(bytes, start + 42) != 0 ? CartisianConfig.Front : CartisianConfig.None),
                    },
                Joint = BitConverter.ToInt16(bytes, start + 88) == 0 ?
                    null :
                    new PositionJoint()
                    {
                        J1 = BitConverter.ToSingle(bytes, start + 52),
                        J2 = BitConverter.ToSingle(bytes, start + 56),
                        J3 = BitConverter.ToSingle(bytes, start + 60),
                        J4 = BitConverter.ToSingle(bytes, start + 64),
                        J5 = BitConverter.ToSingle(bytes, start + 68),
                        J6 = BitConverter.ToSingle(bytes, start + 72),
                        J7 = BitConverter.ToSingle(bytes, start + 76),
                        J8 = BitConverter.ToSingle(bytes, start + 80),
                        J9 = BitConverter.ToSingle(bytes, start + 84),
                    },
            };
        }

        public static byte[] GetBytes(Position pos)
        {
            if (pos.Cartisian != null)
            {
                var bytes = new byte[52];
                Array.Copy(BitConverter.GetBytes(pos.Cartisian.X), 0, bytes, 0, 4);
                Array.Copy(BitConverter.GetBytes(pos.Cartisian.Y), 0, bytes, 4, 4);
                Array.Copy(BitConverter.GetBytes(pos.Cartisian.Z), 0, bytes, 8, 4);
                Array.Copy(BitConverter.GetBytes(pos.Cartisian.W), 0, bytes, 12, 4);
                Array.Copy(BitConverter.GetBytes(pos.Cartisian.P), 0, bytes, 16, 4);
                Array.Copy(BitConverter.GetBytes(pos.Cartisian.R), 0, bytes, 20, 4);
                Array.Copy(BitConverter.GetBytes(pos.Cartisian.E1), 0, bytes, 24, 4);
                Array.Copy(BitConverter.GetBytes(pos.Cartisian.E2), 0, bytes, 28, 4);
                Array.Copy(BitConverter.GetBytes(pos.Cartisian.E3), 0, bytes, 32, 4);
                Array.Copy(BitConverter.GetBytes((short)(pos.Cartisian.Config.HasFlag(CartisianConfig.Flip) ? 0x0001 : 0x0000)), 0, bytes, 36, 2);
                Array.Copy(BitConverter.GetBytes((short)(pos.Cartisian.Config.HasFlag(CartisianConfig.Left) ? 0x0001 : 0x0000)), 0, bytes, 38, 2);
                Array.Copy(BitConverter.GetBytes((short)(pos.Cartisian.Config.HasFlag(CartisianConfig.Up) ? 0x0001 : 0x0000)), 0, bytes, 40, 2);
                Array.Copy(BitConverter.GetBytes((short)(pos.Cartisian.Config.HasFlag(CartisianConfig.Front) ? 0x0001 : 0x0000)), 0, bytes, 42, 2);
                Array.Copy(BitConverter.GetBytes(pos.Cartisian.T4), 0, bytes, 44, 2);
                Array.Copy(BitConverter.GetBytes(pos.Cartisian.T5), 0, bytes, 46, 2);
                Array.Copy(BitConverter.GetBytes(pos.Cartisian.T6), 0, bytes, 48, 2);
                //Array.Copy(BitConverter.GetBytes((short)0x0001), 0, bytes, 50, 2);
                return bytes;
            }
            if (pos.Joint != null)
            {
                var bytes = new byte[38];
                Array.Copy(BitConverter.GetBytes(pos.Joint.J1), 0, bytes, 0, 4);
                Array.Copy(BitConverter.GetBytes(pos.Joint.J2), 0, bytes, 4, 4);
                Array.Copy(BitConverter.GetBytes(pos.Joint.J3), 0, bytes, 8, 4);
                Array.Copy(BitConverter.GetBytes(pos.Joint.J4), 0, bytes, 12, 4);
                Array.Copy(BitConverter.GetBytes(pos.Joint.J5), 0, bytes, 16, 4);
                Array.Copy(BitConverter.GetBytes(pos.Joint.J6), 0, bytes, 20, 4);
                Array.Copy(BitConverter.GetBytes(pos.Joint.J7), 0, bytes, 24, 4);
                Array.Copy(BitConverter.GetBytes(pos.Joint.J8), 0, bytes, 28, 4);
                Array.Copy(BitConverter.GetBytes(pos.Joint.J9), 0, bytes, 32, 4);
                //Array.Copy(BitConverter.GetBytes((short)0x0001), 0, bytes, 36, 2);
                return bytes;
            }
            throw new NotImplementedException();
        }
    }
}