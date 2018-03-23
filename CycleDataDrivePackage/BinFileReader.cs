using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CycleDataDrivePackage
{
    internal class BinFileReader : CycleDataReader
    {
        private BinaryReader _reader;

        public BinFileReader()
        {
            Source = "";
            _reader = LoadFile(Source);
            Interval = 1;
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
            while (true)
            {
                lock (this)
                {
                    try
                    {
                        var data = new byte[1472*4];
                        _reader.Read(data, 0, 1472*4);
                        if(data[16] != 0xAA) continue;
                        NotifyAllObservers(data);
                        Thread.Sleep(Interval);
                    }
                    catch
                    {
                        MessageBox.Show("数据读取完毕");
                        break;
                    }
                }
            }
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
