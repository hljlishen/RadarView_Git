using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RadarDisplayPackage
{
    internal abstract class ZeroLine
    {
        Pen zeroDegreeLinePen;

        public ZeroLine()
        {
            ZeroDegreeLinePen = new Pen(Color.Blue, 2);
        }

        public Pen ZeroDegreeLinePen
        {
            get
            {
                return zeroDegreeLinePen;
            }

            set
            {
                zeroDegreeLinePen = value;
            }
        }

        public abstract void Draw(Bitmap bmp, Rectangle rect, float beginAngle, PointF originalPoint);
    }

    internal class OverViewZeroLine : ZeroLine
    {
        public override void Draw(Bitmap bmp, Rectangle displayArea, float beginAngle, PointF originalPoint)
        {
            Graphics drawer = Graphics.FromImage(bmp);
            //0度线
            float degree = -beginAngle;
            degree = degree % 360;
            float x = originalPoint.X + ((float)displayArea.Width / 2) * (float)Math.Sin(GraphicTrackDisplayerBackground.DegreeToRadian(degree));
            float y = originalPoint.Y - ((float)displayArea.Height / 2) * (float)Math.Cos(GraphicTrackDisplayerBackground.DegreeToRadian(degree));
            drawer.DrawLine(ZeroDegreeLinePen, originalPoint, new Point((int)x, (int)y));    //画线
        }
    }

    internal class SideViewZeroLine : ZeroLine
    {
        public override void Draw(Bitmap bmp, Rectangle displayArea, float beginAngle, PointF originalPoint)
        {
            Graphics drawer = Graphics.FromImage(bmp);

            int tmp = (int)((360 - beginAngle) * displayArea.Width / 360);
            tmp %= displayArea.Width;
            //画360度线
            float x1 = originalPoint.X + tmp;
            drawer.DrawLine(ZeroDegreeLinePen, new Point((int)x1, displayArea.Bottom), new Point((int)x1, displayArea.Top));
        }
    }
}
