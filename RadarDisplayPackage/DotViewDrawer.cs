using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class DotViewDrawer
    {
        protected float targetViewRadius = 3;
        public DotViewDrawer()
        {
            
        }

        public void DrawDot(GraphicTrackDisplayer displayer, GraphicTargetDotView view)
        {
            if (!displayer.coordinateSystem.PointOutOfRange(CoordinateSystem.Point2FToPoint(view.Position)))
            {
                Ellipse e = new Ellipse(view.Position, targetViewRadius, targetViewRadius);
                displayer.Canvas.FillEllipse(e, targetViewBrush);
            }
        }
    }
}
