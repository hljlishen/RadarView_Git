namespace TargetManagerPackage
{
    class DotViewDeleter : SectorProcessor      //删除某个扇区的目标视图
    {
        public void DeleteViews( Sector s , bool deleteTracks)
        {
            NotifyDeleteSectorDot(s);

            if(deleteTracks)
            {
                NotifyDeleteSectorTrack(s);
            }
        }
    }
}
