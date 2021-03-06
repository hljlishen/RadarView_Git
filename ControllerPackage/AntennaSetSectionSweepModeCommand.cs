﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class AntennaSetSectionSweepModeCommand : AntennaCommand
    {
        float beginAngle;
        float endAngle;

        public AntennaSetSectionSweepModeCommand(float beginAngle, float endAngle) :base()
        {
            this.beginAngle = beginAngle;
            this.endAngle = endAngle;
        }

        public override void Execute()
        {
            antenna.SetSectionSweepMode(new AngleArea(beginAngle, endAngle));
        }
    }
}
