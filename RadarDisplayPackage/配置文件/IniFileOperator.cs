using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RadarDisplayPackage
{
    public class IniFileOperator
    {
        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key,string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,string def, StringBuilder retVal, int size, string filePath);

        private string fileName;

        public IniFileOperator(string fileName)
        {
            this.fileName = fileName;
        }
        public string ReadIniData(string Section, string Key)
        {
            if (!File.Exists(fileName)) throw new Exception($"<{fileName}>文件不存在");

            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, Key, "", temp, 1024, fileName);
            return temp.ToString();
        }
        public bool WriteIniData(string Section, string Key, string Value)
        {
            if (!File.Exists(fileName)) throw new Exception($"<{fileName}>文件不存在");

            long OpStation = WritePrivateProfileString(Section, Key, Value, fileName);

            return OpStation == 0 ? false : true;
        }
    }
}
