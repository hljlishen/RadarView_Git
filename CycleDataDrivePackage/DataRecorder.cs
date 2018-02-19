using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CycleDataDrivePackage
{
    class DataRecorder
    {
        BinaryWriter writer;

        public DataRecorder()
        {
            string path = "d:\\RadarData\\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");

            FileStream fs = new FileStream(path, FileMode.Create);
            writer = new BinaryWriter(fs);
        }

        public void RecordByte(byte data)
        {
            writer.Write(data);
        }

        public void RecordBytes(byte[] data, int pos, int count)
        {
            writer.Write(data, pos, count);
        }
    }
}
