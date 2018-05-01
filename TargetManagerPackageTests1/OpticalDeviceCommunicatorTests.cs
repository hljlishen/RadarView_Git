using Microsoft.VisualStudio.TestTools.UnitTesting;
using TargetManagerPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage.Tests
{
    [TestClass()]
    public class OpticalDeviceCommunicatorTests
    {
        [TestMethod()]
        public void CRC16Test()
        {
            //byte[] expected = {0x5b, 0x3e};
            //byte[] data = { 0x7E, 0x00, 0x05, 0x60, 0x31, 0x32, 0x33 };
            //byte crch = OpticalDeviceCommunicator.CRC16(data)[0];
            //byte crcl = OpticalDeviceCommunicator.CRC16(data)[1];
            //Assert.AreEqual(expected[0], OpticalDeviceCommunicator.CRC16(data)[0]);
            //Assert.AreEqual(expected[1], OpticalDeviceCommunicator.CRC16(data)[1]);
        }

        [TestMethod()]
        public void CRC16Test1()
        {
            //byte[] expected = { 0x5b, 0x3e };
            //byte[] data = { 0x7E, 0x00, 0x05, 0x60, 0x31, 0x32, 0x33 };
            //byte crch = OpticalDeviceCommunicator.Crc16(data)[0];
            //byte crcl = OpticalDeviceCommunicator.CRC16(data)[1];
            //Assert.AreEqual(expected[0], OpticalDeviceCommunicator.CRC16(data)[0]);
            //Assert.AreEqual(expected[1], OpticalDeviceCommunicator.CRC16(data)[1]);
        }
    }
}