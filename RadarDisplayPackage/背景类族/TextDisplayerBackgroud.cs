using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;

namespace RadarDisplayPackage
{
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