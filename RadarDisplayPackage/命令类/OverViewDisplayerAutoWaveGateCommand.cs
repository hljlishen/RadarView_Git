namespace RadarDisplayPackage
{
    public class OverViewDisplayerAutoWaveGateCommand : OverViewDisplayerCommand
    {
        /// <inheritdoc />
        public OverViewDisplayerAutoWaveGateCommand(OverViewDisplayer ovd) : base(ovd)
        {

        }

        public override void Execute() => ovd.SwitchState(OverViewState.AutoWaveGate);
    }
}
