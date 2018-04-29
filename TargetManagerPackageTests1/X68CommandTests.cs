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
    public class X68CommandTests
    {
        [TestMethod()]
        public void SerializeTest()
        {
            //TargetTrack t = new TargetTrack
            //{
            //    trackID = 10,
            //    AZ = 2,
            //    EL = 102,
            //    Dis = 57
            //};

            //X68Command x68 = new X68Command(t);
            //byte[] a1 = x68.Serialize();
            //byte[] a2 = OpticalDeviceCommunicator.GetCmdBytes(t);

            //Assert.IsTrue(ByteArrayAreEqual(a1, a2));
        }

        private bool ByteArrayAreEqual(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int index = 0; index < a1.Length; index++)
            {
                if (a1[index] != a2[index])
                {
                    Console.WriteLine(index);
                    return false;
                }
            }

            return true;
        }
    }
}