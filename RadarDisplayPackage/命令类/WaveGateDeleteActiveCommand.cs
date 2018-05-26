namespace RadarDisplayPackage
{
    class WaveGateDeleteActiveCommand : WaveGateCommand
    {
        public override void Execute() => waveGateController.DeleteActiveWaveGate();
    }
}
