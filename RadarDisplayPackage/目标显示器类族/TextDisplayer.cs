using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using RadarDisplayPackage.目标显示器类族;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

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

        protected override void OtherDrawing()
        {
            //Canvas.DrawEllipse(new Ellipse(new Point2F(100,100), 50, 50 ), Canvas.CreateSolidColorBrush(new ColorF(1,0,0)), 3 );
        }
    }

    class TextDisplayerBackgroud : GraphicTrackDisplayerBackground
    {
        private readonly List<string> _columnNames;
        public List<RectF> ColumnRects { get;}
        public int ColumnWidth { get; }
        public const int ColumnHeigth = 30;
        private readonly TextDisplayer _displayer;

        private readonly Brush _borderBrush;
        private readonly TextFormat _headerFormat;
        private readonly Brush _headerBrush;
        public TextDisplayerBackgroud(TextDisplayer displayer, List<string> columnNames) : base(displayer.Canvas, displayer.Factory, displayer.coordinateSystem)
        {
            _columnNames = columnNames;
            this._displayer = displayer;
            ColumnWidth = CalculateColumnWidth();
            ColumnRects = CalculateColumnRects();

            _borderBrush = canvas.CreateSolidColorBrush(new ColorF(1, 1, 1));

            DWriteFactory dw = DWriteFactory.CreateFactory();
            _headerFormat = dw.CreateTextFormat("宋体", 20);
            _headerFormat.TextAlignment = TextAlignment.Center;
            _headerBrush = canvas.CreateSolidColorBrush(new ColorF(0, 1, 1));
        }

        private int CalculateColumnWidth()
        {
            int count = _columnNames.Count;
            int areaWidth = _displayer.DrawRect.Width;

            return areaWidth / count;
        }

        private List<RectF> CalculateColumnRects()
        {
            List<RectF> ret = new List<RectF>();
            for (int i = 0; i < _columnNames.Count; i++)
            {
                float left = ColumnWidth * i;
                float top = 0;
                float right = left + ColumnWidth;
                float bottom = top + ColumnHeigth;
                ret.Add(new RectF(left, top, right, bottom));
            }

            return ret;
        }

        public override void Draw()
        {

            for(int i =0; i < _columnNames.Count; i++)
            {
                canvas.DrawRectangle(ColumnRects[i], _borderBrush, 2);
                canvas.DrawText(_columnNames[i], _headerFormat, ColumnRects[i], _headerBrush);
            }
        }
    }
}
