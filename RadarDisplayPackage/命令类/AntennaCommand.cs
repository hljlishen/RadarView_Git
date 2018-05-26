using TargetManagerPackage;

namespace RadarDisplayPackage
{
    public abstract class AntennaCommand : ICommand
    {
        protected IAntennaController antenna;   //天线控制器

        public AntennaCommand()
        {
            antenna = TargetManagerFactory.CreateAntennaContoller();
        }

        public abstract void Execute();
    }
}
