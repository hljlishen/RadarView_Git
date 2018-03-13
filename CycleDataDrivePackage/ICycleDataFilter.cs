namespace CycleDataDrivePackage
{
    internal interface ICycleDataFilter //扫描数据滤波器接口，根据不同的规则滤除扫描数据中的杂波
    {
        AzimuthCell Process(byte[] data);
    }
}
