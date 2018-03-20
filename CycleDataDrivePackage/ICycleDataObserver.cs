namespace CycleDataDrivePackage
{
    public interface ICycleDataObserver
    {
        //void NotifyNewCycleData(AzimuthCell data);

        void NotifyNewCycleData(byte[] rawData);
    }
}
