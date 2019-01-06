using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CycleDataDrivePackage
{
    internal class BinFileReader : CycleDataReader
    {
        private BinaryReader _reader;
        private int _readLength = 1472;

        public BinFileReader()
        {
            Source = "";
            _reader = LoadFile(Source);
            Interval = 2;
        }

        private BinaryReader LoadFile(string fileName)
        {
            if (Source == null || !File.Exists(fileName)) return null;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader reader = new BinaryReader(fs);
            return reader;
        }

        protected override void ReadData()
        {
            _reader = LoadFile(Source);
            int bytesRead = 0;
            lock (this)
            {
                while ( _reader.BaseStream!= null && bytesRead < _reader.BaseStream.Length)
                {
                    var data = new byte[_readLength];
                    _reader.Read(data, 0, _readLength);
                    if (data[16] != 0xAA) continue;
                    NotifyAllObservers(data);
                    bytesRead += _readLength;
                    Thread.Sleep(Interval);
                }
            }

            MessageBox.Show("读取完毕");
        }

        public sealed override string Source
        {
            get => base.Source;

            set
            {
                base.Source = value;
                _reader?.Close();
                _reader?.Dispose();
                _reader = LoadFile(value);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _reader?.Close();
            _reader?.Dispose();
        }
    }
}
