namespace RadarDisplayPackage
{
    class SetAntennaToDegreeCommand : AntennaCommand
    {
        private float stopDegree;
        public SetAntennaToDegreeCommand(float degree)
        {
            this.stopDegree = degree;
        }
        public override void Execute()
        {
            antenna.SetRotateDirection(TargetManagerPackage.RotateDirection.ClockWise);
            antenna.SetAntennaToZeroDegree(stopDegree);
        }
    }
}
