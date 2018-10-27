using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    public class SideViewDisplayer : GraphicTrackDisplayer
    {
        public SideViewDisplayer(Panel h) : base(h)
        {
            targetsManager = new CoordinateTargetViewManager(this);
            //Distance = 3000;
            //targetsManager = new CoordinateTargetViewManager(this);
        }

        public override float Distance
        {
            get => base.Distance;

            set
            {
                base.Distance = value;
                background.Distance = value;
                targetsManager?.Dispose();
                targetsManager = new CoordinateTargetViewManager(this);
            }
        }

        internal override GraphicTrackDisplayerBackground CreateBackground()
        {
            SideViewDisplayerBackground svdb = new SideViewDisplayerBackground(Canvas, Factory, coordinateSystem);

            //创建角度分划线虚线风格
            StrokeStyleProperties ssp = new StrokeStyleProperties {DashStyle = DashStyle.DashDot};
            svdb.DashLineStrokeStyle = Factory.CreateStrokeStyle(ssp);

            return svdb;
        }

        internal override GraphicTrackDisplayerAntenna CreateAntenna()
        {
            SideViewDisplayerAntenna svda = new SideViewDisplayerAntenna(Canvas, Factory, coordinateSystem);
            return svda;
        }

        internal override CoordinateSystem CreateCoordinateSystem()
        {
            return new CoordinateSystemOfRetangular(drawArea, distance, Factory);
        }
    }
}