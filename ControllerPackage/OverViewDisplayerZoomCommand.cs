using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class OverViewDisplayerZoomCommand : OverViewDisplayerCommand
    {
        Rect rect;
        public OverViewDisplayerZoomCommand (OverViewDisplayer ovd, Rect zoomRect) : base(ovd)
        {
            rect = zoomRect;
        }

        public override void Execute()
        {
            ovd.ZoomArea(rect);
        }
    }
}
