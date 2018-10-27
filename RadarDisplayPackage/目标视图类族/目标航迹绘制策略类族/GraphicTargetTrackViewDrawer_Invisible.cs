using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    abstract class GraphicTargetTrackViewDrawer_Invisible : GraphicTargetTrackViewDrawer //北方超出边界
    {
        protected Point2F projectivePosition;     //在可视区域边缘的投影坐标
        public GraphicTargetTrackViewDrawer_Invisible(CoordinateTargetTrackView view) : base(view)
        {
            projectivePosition = GetProjectivePosition();
            tagBrush.Opacity = 0.5f;
            idBrush.Opacity = 1f;
        }

        protected abstract Point2F GetProjectivePosition(); //计算view.position在可视区域边缘的投影坐标
    }
}
