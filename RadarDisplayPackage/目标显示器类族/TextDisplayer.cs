using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using RadarDisplayPackage.目标显示器类族;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RadarDisplayPackage
{
    public class TextDisplayer : GraphicTrackDisplayer
    {
        public Rect DrawRect { get; protected set; }
        public Rect DrawTargetArea { get; protected set; }
        public int ColumnWidth { get; }

        public List<RectF> HeaderRects { get; }

        private readonly string[] _columnNames = {"批号", "方位", "仰角", "距离", "速度"};

        public TextDisplayer(Panel holder) : base(holder)
        {
            holder.BackColor = Color.Black;
            DrawRect = new Rect(0,0, holder.Width, holder.Height);
            DrawTargetArea = new Rect(0, TextDisplayerBackgroud.ColumnHeigth, DrawRect.Right, DrawRect.Bottom);
            targetsManager = new TextTargetViewManager(this);
            background = CreateBackground();
            ColumnWidth = ((TextDisplayerBackgroud) background).ColumnWidth;
            HeaderRects = ((TextDisplayerBackgroud) background).ColumnRects;
        }

        internal sealed override GraphicTrackDisplayerBackground CreateBackground()
        {
            return new TextDisplayerBackgroud(this, new List<string>(_columnNames));
        }

        internal override GraphicTrackDisplayerAntenna CreateAntenna() => null;

        internal override CoordinateSystem CreateCoordinateSystem() => null;
    }
}
