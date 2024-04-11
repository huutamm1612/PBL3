using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    internal class Utils
    {
        public delegate bool Compare(object o1, object o2);
        public delegate bool Equal(object o1, object o2);

        public static void RemoveSer<T>(List<T> list, List<string> maSo)
        {
            maSo.Sort();
            int i = 0, j = 0;

            while (j < maSo.Count)
            {
                if (list[i].Equals(maSo[i]))
                {
                    list.Remove(list[i]);
                    j++;
                }
                else
                    i++;
            }

        }

        public static void BSearch<T>(List<T> list, Compare cmp, Equal eql)
        {

        }

        public static void Sort<T>(List<T> list, int left, int right, Compare cmp, Equal eql)
        {
            if (left >= right)
                return;

            int r = right;
            int l = left;
            object pivot = list[(r + l) / 2];

            while (l < r)
            {
                while (cmp(list[l], pivot)) l++;
                while (!cmp(list[r], pivot) && !eql(list[r], pivot)) r--;
                if (l <= r)
                {
                    (list[r], list[l]) = (list[l], list[r]);
                    l++;
                    r--;
                }
            }

            Sort(list, left, r, cmp, eql);
            Sort(list, l, right, cmp, eql);
        }

        public static void Filter()
        {

        }

        public static bool KiemTraSoDT(string soDT)
        {
            if (soDT.Length != 10 || soDT[0] != '0')
            {
                return false;
            }

            foreach (char i in soDT)
            {
                if (!char.IsDigit(i))
                {
                    return false;
                }
            }
            return true;
        }

        public static void DrawRectangle(Graphics g, RectangleF rect, Color color, float radius = 0, Pen pen = null)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.CompositingMode = CompositingMode.SourceOver;
            Brush brush = new SolidBrush(color);

            if (radius == 0)
                g.FillRectangle(brush, rect);

            else
            {
                GraphicsPath path = new GraphicsPath();
                path.StartFigure();

                path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                path.AddArc(rect.X + rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
                path.AddArc(rect.X + rect.Width - radius * 2, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(rect.X, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseFigure();

                g.FillPath(brush, path);
                if (pen == null)
                    pen = new Pen(brush);
                g.DrawPath(pen, path);

            }
        }

        public static void SetComboBox(ComboBox cbb, List<string> list)
        {
            cbb.Items.Clear();
            foreach (string item in list)
            {
                cbb.Items.Add(item);
            }
        }
    }
}
