﻿namespace Fanuc.RobotInterface
{
    public enum AlarmCategory : short
    {
        ACAL = 112,
        APPL = 20,
        APSH = 38,
        ARC = 53,
        ASBN = 22,
        ATZN = 138,
        BBOX = 118,
        BRCH = 144,
        CALM = 106,
        CD = 82,
        CMND = 42,
        CNTR = 73,
        COMP = 59,
        COND = 4,
        COPT = 37,
        CPMO = 114,
        CUST = 97,
        CVIS = 117,
        DIAG = 148,
        DICT = 33,
        DMDR = 84,
        DMER = 40,
        DNET = 76,
        DTBR = 149,
        DX = 72,
        ELOG = 5,
        FILE = 2,
        FLPY = 10,
        FORC = 91,
        FRSY = 85,
        FXTL = 136,
        HOST = 67,
        HRTL = 66,
        IBS = 88,
        ICRZ = 124,
        IFPN = 153,
        INTP = 12,
        IRPK = 127,
        ISD = 39,
        ISDT = 95,
        JOG = 19,
        KALM = 122,
        LANG = 21,
        LECO = 109,
        LNTK = 44,
        LSTP = 108,
        MACR = 57,
        MARL = 83,
        MASI = 147,
        MCTL = 6,
        MEMO = 7,
        MOTN = 15,
        MUPS = 48,
        OPTN = 65,
        OS = 0,
        PALL = 115,
        PALT = 26,
        PICK = 132,
        PMON = 28,
        PNT1 = 86, // 52 in chinese manual, 86 in english manual
        PNT2 = 137,
        PRIO = 13,
        PROF = 92,
        PROG = 3,
        PWD = 31,
        QMGR = 61,
        RIPE = 130,
        ROUT = 17,
        RPC = 93,
        RPM = 43,
        RTCP = 89,
        SCIO = 25,
        SDTL = 123,
        SEAL = 51,
        SENS = 58,
        SHAP = 79,
        SPOT = 23,
        SPRM = 131,
        SRIO = 1,
        SRVO = 11,
        SSPC = 69,
        SVGN = 30,
        SYST = 24,
        TAST = 47,
        TCPP = 46,
        TG = 90,
        THSR = 60,
        TJOG = 116,
        TMAT = 119,
        TOOL = 29,
        TPIF = 9,
        TRAK = 54,
        VARS = 16,
        WEAV = 45,
        WMAP = 103,
        WNDW = 18,
        XMLF = 129,
    }

    public enum AlarmSeverity : short
    {
        NONE = 128,
        WARN = 0,
        PAUSE_L = 2,
        PAUSE_G = 34,
        STOP_L = 6,
        STOP_G = 38,
        SERVO = 54,
        ABORT_L = 11,
        ABORT_G = 45,
        SERVO2 = 58,
        SYSTEM = 122,
    }

    public enum AlarmType
    {
        Active = 10,
        History = 9,
    }

    public class RobotAlarm : IEquatable<RobotAlarm>
    {
        public AlarmCategory Category { get; init; }
        public short Number { get; init; }

        public AlarmCategory CauseCategory { get; init; }
        public short CauseNumber { get; init; }

        public AlarmSeverity Severity { get; init; }

        public DateTime Time { get; init; }

        public static RobotAlarm FromBytes(byte[] bytes, int start = 0)
        {
            if (bytes == null)
                return null;
            if (bytes.Length < start + 200)
                throw new ArgumentException("need 200 bytes for alarm", nameof(bytes));

            return new RobotAlarm()
            {
                Category = (AlarmCategory)BitConverter.ToInt16(bytes, start + 0),
                Number = BitConverter.ToInt16(bytes, start + 2),
                CauseCategory = (AlarmCategory)BitConverter.ToInt16(bytes, start + 4),
                CauseNumber = BitConverter.ToInt16(bytes, start + 6),
                Severity = (AlarmSeverity)BitConverter.ToInt16(bytes, start + 8),
                Time = _GetTime(
                    BitConverter.ToInt16(bytes, start + 10), // year
                    BitConverter.ToInt16(bytes, start + 12), // month
                    BitConverter.ToInt16(bytes, start + 14), // day
                    BitConverter.ToInt16(bytes, start + 16), // hour
                    BitConverter.ToInt16(bytes, start + 18), // minute
                    BitConverter.ToInt16(bytes, start + 20)  // second
                ),
            };
        }

        private static DateTime _GetTime(short year, short month, short day, short hour, short minute, short second)
        {
            if (year == 0 && month == 0 && day == 0 && hour == 0 && minute == 0 && second == 0)
                return DateTime.UnixEpoch;
            return new DateTime(year, month, day, hour, minute, second);
        }

        public bool Equals(RobotAlarm other)
        {
            if (other == null)
                return false;
            if (Category != other.Category)
                return false;
            if (Number != other.Number)
                return false;
            if (CauseCategory != other.CauseCategory)
                return false;
            if (CauseNumber != other.CauseNumber)
                return false;
            if (Severity != other.Severity)
                return false;
            if (!Equals(Time, other.Time))
                return false;
            return true;
        }

        public override string ToString()
        {
            var msg = $"{Time:yyyy/MM/dd hh:mm:ss}: {Severity}: {Category}-{Number:000}";
            if (CauseCategory != AlarmCategory.OS && CauseNumber != 0)
                return $"{msg}, caused by {CauseCategory}-{CauseNumber:000}";
            return msg;
        }
    }
}