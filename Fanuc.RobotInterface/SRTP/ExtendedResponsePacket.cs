﻿using System.Linq;

namespace Fanuc.RobotInterface.SRTP
{
    public class ExtendedResponsePacket : ExtendedPacketBase, IResponsePacket
    {
        public ExtendedResponsePacket(byte[] buffer)
        {
            Header = buffer.Take(56).ToArray();
            ExtraPayload = buffer.Skip(56).ToArray();
        }
    }
}
