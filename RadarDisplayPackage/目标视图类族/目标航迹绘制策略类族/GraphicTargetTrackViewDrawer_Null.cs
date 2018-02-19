using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class GraphicTargetTrackViewDrawer_Null : GraphicTargetTrackViewDrawer  //NullObject模式，应用于超出显示范围的目标，绘制目标时什么都不做
    {
        public GraphicTargetTrackViewDrawer_Null(GraphicTargetTrackView view) : base(view)
        {
        }

        protected override PathGeometry BuildTriangle()
        {
            return null;
        }

        protected override RoundedRectangleGeometry BuildIDTag()
        {
            return null;
        }

        public override void Draw()
        {
        }
    }
}
