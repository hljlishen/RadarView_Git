
namespace TargetManagerPackage
{
    public delegate void DataReceivedHandler(byte[] recvData);
    public interface IPort
    {
        event DataReceivedHandler DataReceived;

        void Open();

        bool IsOpen();

        void Close();

        void Send(byte[] sendData);
    }
}
