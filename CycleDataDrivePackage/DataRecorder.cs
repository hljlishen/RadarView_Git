using System;
using System.Windows.Forms;
using System.IO;

namespace CycleDataDrivePackage
{
    class DataRecorder : IDisposable
    {
        private readonly BinaryWriter _writer;
        private string _filePath;

        public DataRecorder()
        {
            string filePath = @"c:\RadarData\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");

            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                _writer = new BinaryWriter(fs);
            }
            catch
            {
                MessageBox.Show(filePath + " 创建失败");
            }
        }

        public void RecordBytes(byte[] data, int pos, int count) => _writer?.Write(data, pos, count);

        private bool IsEmptyFile(string file)
        {
            if (_filePath == null)
            {
                return false;
            }
            FileInfo fileInfo = new FileInfo(file);
            return fileInfo.Length == 0;
        }

        public void Dispose()
        {
            _writer?.Close();
            _writer?.Dispose();

            if(IsEmptyFile(_filePath))
                File.Delete(_filePath);
        }
    }
}
