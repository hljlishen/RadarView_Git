using System;
using System.Windows.Forms;
using System.IO;

namespace CycleDataDrivePackage
{
    class DataRecorder
    {
        private readonly BinaryWriter _writer;

        public DataRecorder()
        {
            string path = @"c:\RadarData\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");

            try
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                _writer = new BinaryWriter(fs);
            }
            catch
            {
                MessageBox.Show(path + " 创建失败");
            }
        }

        public void RecordBytes(byte[] data, int pos, int count)
        {
            _writer?.Write(data, pos, count);
        }
    }
}
