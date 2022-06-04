using System.Runtime.CompilerServices;

namespace Fanuc.RobotInterface
{
    public static class IRobotIFExtension
    {
        public const int OFFSET_SDIO = 0;
        public const int OFFSET_RDIO = 5000;
        public const int OFFSET_UOP = 6000;
        public const int OFFSET_SOP = 7000;
        public const int OFFSET_WIO = 8000;
        public const int OFFSET_WSI = 8400;
        public const int OFFSET_PMC_K = 10000;
        public const int OFFSET_PMC_R = 11000;

        public const int OFFSET_GIO = 0;
        public const int OFFSET_AIO = 1000;
        public const int OFFSET_PMC_D = 10000;

        #region Read
        private static bool[] GuardedReadDI(this IRobotIF robot, int index, ushort count, [CallerMemberName] string caller = nameof(IRobotIF.ReadDI))
        {
            try
            {
                return robot.ReadDI(index, count);
            }
            catch (Exception ex)
            {
                throw new IOException($"{caller}(index: {index}, count: {count}) failed", ex);
            }
        }
        private static bool[] GuardedReadDO(this IRobotIF robot, int index, ushort count, [CallerMemberName] string caller = nameof(IRobotIF.ReadDO))
        {
            try
            {
                return robot.ReadDO(index, count);
            }
            catch (Exception ex)
            {
                throw new IOException($"{caller}(index: {index}, count: {count}) failed", ex);
            }
        }

        public static bool[] ReadSDI(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDI(index + OFFSET_SDIO, count);
        public static bool[] ReadSDO(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDO(index + OFFSET_SDIO, count);
        public static bool[] ReadRDI(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDI(index + OFFSET_RDIO, count);
        public static bool[] ReadRDO(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDO(index + OFFSET_RDIO, count);
        public static bool[] ReadUI(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDI(index + OFFSET_UOP, count);
        public static bool[] ReadUO(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDO(index + OFFSET_UOP, count);
        public static bool[] ReadSI(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDI(index + OFFSET_SOP, count);
        public static bool[] ReadSO(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDO(index + OFFSET_SOP, count);
        public static bool[] ReadWI(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDI(index + OFFSET_WIO, count);
        public static bool[] ReadWSI(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDI(index + OFFSET_WSI, count);
        public static bool[] ReadWO(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDO(index + OFFSET_WIO, count);
        public static bool[] ReadPmcK(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDO(index + OFFSET_PMC_K, count);
        public static bool[] ReadPmcR(this IRobotIF robot, int index, ushort count) => robot.GuardedReadDO(index + OFFSET_PMC_R, count);

        private static ushort[] _GuardedReadGI(this IRobotIF robot, int index, ushort count, [CallerMemberName] string caller = nameof(IRobotIF.ReadGI))
        {
            try
            {
                return robot.ReadGI(index, count);
            }
            catch (Exception ex)
            {
                throw new IOException($"{caller}(index: {index}, count: {count}) failed", ex);
            }
        }
        private static ushort[] _GuardedReadGO(this IRobotIF robot, int index, ushort count, [CallerMemberName] string caller = nameof(IRobotIF.ReadGO))
        {
            try
            {
                return robot.ReadGO(index, count);
            }
            catch (Exception ex)
            {
                throw new IOException($"{caller}(index: {index}, count: {count}) failed", ex);
            }
        }

        public static ushort[] ReadGI(this IRobotIF robot, int index, ushort count) => robot.ReadGI(index + OFFSET_GIO, count);
        public static ushort[] ReadGO(this IRobotIF robot, int index, ushort count) => robot.ReadGO(index + OFFSET_GIO, count);
        public static ushort[] ReadAI(this IRobotIF robot, int index, ushort count) => robot.ReadGI(index + OFFSET_AIO, count);
        public static ushort[] ReadAO(this IRobotIF robot, int index, ushort count) => robot.ReadGO(index + OFFSET_AIO, count);
        public static ushort[] ReadPmcD(this IRobotIF robot, int index, ushort count) => robot.ReadGO(index + OFFSET_PMC_D, count);
        #endregion

        #region Write
        public static void WriteSDI(this IRobotIF robot, int index, params bool[] values) => robot.WriteDI(index + OFFSET_SDIO, values);
        public static void WriteSDO(this IRobotIF robot, int index, params bool[] values) => robot.WriteDO(index + OFFSET_SDIO, values);
        public static void WriteRDI(this IRobotIF robot, int index, params bool[] values) => robot.WriteDI(index + OFFSET_RDIO, values);
        public static void WriteRDO(this IRobotIF robot, int index, params bool[] values) => robot.WriteDO(index + OFFSET_RDIO, values);
        public static void WriteUI(this IRobotIF robot, int index, params bool[] values) => robot.WriteDI(index + OFFSET_UOP, values);
        public static void WriteUO(this IRobotIF robot, int index, params bool[] values) => robot.WriteDO(index + OFFSET_UOP, values);
        public static void WriteSI(this IRobotIF robot, int index, params bool[] values) => robot.WriteDI(index + OFFSET_SOP, values);
        public static void WriteSO(this IRobotIF robot, int index, params bool[] values) => robot.WriteDO(index + OFFSET_SOP, values);
        public static void WriteWI(this IRobotIF robot, int index, params bool[] values) => robot.WriteDI(index + OFFSET_WIO, values);
        public static void WriteWSI(this IRobotIF robot, int index, params bool[] values) => robot.WriteDI(index + OFFSET_WSI, values);
        public static void WriteWO(this IRobotIF robot, int index, params bool[] values) => robot.WriteDO(index + OFFSET_WIO, values);
        public static void WritePmcK(this IRobotIF robot, int index, params bool[] values) => robot.WriteDO(index + OFFSET_PMC_K, values);
        public static void WritePmcR(this IRobotIF robot, int index, params bool[] values) => robot.WriteDO(index + OFFSET_PMC_R, values);

        public static void WriteGI(this IRobotIF robot, int index, params ushort[] values) => robot.WriteGI(index + OFFSET_GIO, values);
        public static void WriteGO(this IRobotIF robot, int index, params ushort[] values) => robot.WriteGO(index + OFFSET_GIO, values);
        public static void WriteAI(this IRobotIF robot, int index, params ushort[] values) => robot.WriteGI(index + OFFSET_AIO, values);
        public static void WriteAO(this IRobotIF robot, int index, params ushort[] values) => robot.WriteGO(index + OFFSET_AIO, values);
        public static void WritePmcD(this IRobotIF robot, int index, params ushort[] values) => robot.WriteGO(index + OFFSET_PMC_D, values);
        #endregion
    }
}