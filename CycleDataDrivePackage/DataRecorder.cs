using System;
using System.IO;

namespace CycleDataDrivePackage
{
    class DataRecorder
    {
        private readonly BinaryWriter _writer;

        public DataRecorder()
        {
            string path = @"d:\RadarData\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");

            FileStream fs = new FileStream(path, FileMode.Create);
            _writer = new BinaryWriter(fs);
        }

        public void RecordBytes(byte[] data, int pos, int count)
        {
            _writer.Write(data, pos, count);
        }
    }
}
