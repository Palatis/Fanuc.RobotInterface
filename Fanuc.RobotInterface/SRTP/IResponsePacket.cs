namespace Fanuc.RobotInterface.SRTP
{
    public interface IResponsePacket : IPacket
    {
        byte[] ActualPayload { get; }
    }
}
