using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class TargetManagerAddTrackGeneratorCommand : TargetManagerCommand
    {
        private PolarCoordinate coordinate;

        public TargetManagerAddTrackGeneratorCommand(PolarCoordinate coordinate)
        {
            this.coordinate = coordinate;
        }
        public override void Execute()
        {
            targetController.AddTrackGenerator(coordinate);
        }
    }
}
