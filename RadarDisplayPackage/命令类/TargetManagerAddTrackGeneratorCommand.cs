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
        public override void Execute() => targetController.AddTrackGenerator(coordinate);
    }
}
