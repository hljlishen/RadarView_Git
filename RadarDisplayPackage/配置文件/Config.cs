using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarDisplayPackage
{
    class Config
    {
        private IniFileOperator ini;
        private string sectionName;
        private string speedMinKeyName = "speedMin";
        private string speedMaxKeyName = "speedMax";
        private string amMinKeyName = "amMin";
        private string heightMinKeyName = "HeightMin";
        private string azAdjustmentKeyName = "azAdjustment";
        private string elAdjustmentKeyName = "elAdjustment";
        private string antennaStopDegreeKeyName = "antennaStopDegree";

        public Config()
        {
            ini = new IniFileOperator(@".\config.ini");
            sectionName = "radarConfig";
        }

        public void SetSpeedMin(int speedMin) => SetValue(speedMinKeyName, speedMin.ToString());
        private void SetValue(string key, string value) => ini.WriteIniData(sectionName, key, value);
        public int GetSpeedMin() => int.Parse(GetValue(speedMinKeyName));
        private string GetValue(string keyName) => ini.ReadIniData(sectionName, keyName);

        public void SetSpeedMax(int speedMax) => SetValue(speedMaxKeyName, speedMax.ToString());
        public int GetSpeedMax() => int.Parse(GetValue(speedMaxKeyName));

        public void SetAmMin(int amMin) => SetValue(amMinKeyName, amMin.ToString());
        public int GetAmMin() => int.Parse(GetValue(amMinKeyName));

        public void SetHeightMin(int heightMin) => SetValue(heightMinKeyName, heightMin.ToString());
        public int GetHeightMin() => int.Parse(GetValue(heightMinKeyName));

        public void SetAzAdjustment(float adjustment) => ini.WriteIniData(sectionName, azAdjustmentKeyName, adjustment.ToString("0.00"));

        public float GetAzAdjustment() => float.Parse(ini.ReadIniData(sectionName, azAdjustmentKeyName));

        public void SetElAdjustment(float adjustment) => ini.WriteIniData(sectionName, elAdjustmentKeyName, adjustment.ToString("0.00"));

        public float GetElAdjustment() => float.Parse(ini.ReadIniData(sectionName, elAdjustmentKeyName));

        public void SetStopDegree(float degree) => ini.WriteIniData(sectionName, antennaStopDegreeKeyName, degree.ToString("0.00"));

        public float GetStopDegree() => float.Parse(ini.ReadIniData(sectionName, antennaStopDegreeKeyName));
    }
}
