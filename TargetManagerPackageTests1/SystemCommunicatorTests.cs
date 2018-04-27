using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TargetManagerPackage.Tests
{
    [TestClass()]
    public class SystemCommunicatorTests
    {
        [TestMethod()]
        public void IntToByteLsbTest()
        {
            int data = 0xaabb;
            byte[] expected = new byte[] { 0xbb, 0xaa };
            Assert.AreEqual(SystemCommunicator.IntToByteLsb(data, 2)[0], expected[0]);
            Assert.AreEqual(SystemCommunicator.IntToByteLsb(data, 2)[1], expected[1]);

            data = 0xff;
            expected = new byte[] { 0xff, 0x00 };
            Assert.AreEqual(SystemCommunicator.IntToByteLsb(data, 2)[1], expected[1]);

            data = 0xa1bdbf;
            expected = new byte[] { 0xbf, 0xbd, 0xa1, 0, 0, 0 };

            Assert.AreEqual(SystemCommunicator.IntToByteLsb(data, 3)[0], expected[0]);
            Assert.AreEqual(SystemCommunicator.IntToByteLsb(data, 3)[1], expected[1]);
            Assert.AreEqual(SystemCommunicator.IntToByteLsb(data, 3)[2], expected[2]);
            Assert.AreEqual(SystemCommunicator.IntToByteLsb(data, 6)[3], expected[3]);
            Assert.AreEqual(SystemCommunicator.IntToByteLsb(data, 6)[4], expected[4]);
            Assert.AreEqual(SystemCommunicator.IntToByteLsb(data, 6)[5], expected[5]);
            Assert.AreEqual(SystemCommunicator.IntToByteLsb(data, 6).Length, 6);

            //data = 0xff;
            //expected = new byte[] { 0xff, 0x00 };
            //Assert.AreEqual(SystemCommunicator.IntToByteLsb(data, 2)[0], expected[0]);
            //Assert.AreEqual(SystemCommunicator.IntToByteLsb(data, 2)[1], expected[1]);
        }

        [TestMethod()]
        public void Send0X80CmdTest()
        {
            TargetTrack t = new TargetTrack
            {
                trackID = 155,
                AZ = 256.3f,
                EL = 15.4f,
                Dis = 2500f
            };

            SystemCommunicator.Send0X80Cmd(t);
        }
    }
}