using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace CycleDataDrivePackage
{
    class BinFileReader : CycleDataReader
    {
        BinaryReader reader;
        FileStream fs;

        public BinFileReader(): base()
        {
            //new OpenFileDialog();
            Source = "";
            LoadFile(Source);
            Interval = 50;
        }

        private void LoadFile(string fileName)
        {
            if (Source != null && File.Exists(fileName))
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new BinaryReader(fs);
            }
        }

        protected override void ReadData()
        {
            AzimuthCell cell;
            byte[] data;
            LoadFile(Source);
            int count = 0;
            while (true)
            {
                lock (this)
                {
                    try
                    {
                        data = new byte[DataMaximumLength];
                        reader.Read(data, 0, DataMaximumLength);
                        if(data[16] != 0xAA) continue;;
                        cell = new AzimuthCell(data);
                        NotifyAllObservers(cell);
                        Thread.Sleep(Interval);
                        count++;
                    }
                    catch
                    {
                        MessageBox.Show("数据读取完毕");
                        break;
                    }
                }
            }
        }

        public override string Source
        {
            get
            {
                return base.Source;
            }

            set
            {
                base.Source = value;
                reader?.Close();
                fs?.Close();
                fs?.Dispose();
                reader?.Dispose();
                LoadFile(value);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            reader?.Close();
            fs?.Close();
            fs?.Dispose();
            reader?.Dispose();
        }
    }
}
