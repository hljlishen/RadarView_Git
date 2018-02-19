using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;

namespace RadarDisplayPackage
{
    class OverViewDisplayerOffsetCommand : OverViewDisplayerCommand
    {
        readonly Point2F mouseDoubleClickPos;
        public OverViewDisplayerOffsetCommand(OverViewDisplayer ovd, Point p) : base(ovd)
        {
            mouseDoubleClickPos = CoordinateSystem.PointToPoint2F(p);
        }

        public override void Execute()
        {
            ovd.OffsetDisplayerToPoint(mouseDoubleClickPos);
        }
    }
}
