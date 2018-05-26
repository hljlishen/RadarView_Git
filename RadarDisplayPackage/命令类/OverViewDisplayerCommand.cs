namespace RadarDisplayPackage
{
    public abstract class OverViewDisplayerCommand : ICommand
    {
        protected OverViewDisplayer ovd;

        protected OverViewDisplayerCommand(OverViewDisplayer ovd)
        {
            this.ovd = ovd;
        }

        public abstract void Execute();
    }
}
