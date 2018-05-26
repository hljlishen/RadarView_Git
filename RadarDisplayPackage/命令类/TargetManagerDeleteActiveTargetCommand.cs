namespace RadarDisplayPackage
{
    public class TargetManagerDeleteActiveTargetCommand : TargetManagerCommand
    {
        public override void Execute() => targetController.DeleteActiveTarget();
    }
}
