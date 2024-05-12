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

        public static void RemoveRange<T>(List<T> list, List<string> maSo)
        {
            maSo.Sort();
            int i = 0, j = 0;

            while (j < maSo.Count && i < list.Count)
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

        public static string SetPath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Substring(0, path.Length - ("Program\\bin\\Debug\\").Length) + "Images\\";
            return path;
        }

        public static Image Resize(Image image, Size newSize)
        {
            int nw = newSize.Width; int nh = newSize.Height;
            int iw = image.Width; int ih = image.Height;

            if(iw > ih)
            {
                nh = (int)(ih * (double) nw / (double) iw);
            }
            else
            {
                nw = (int)(iw * (double) nh / (double) ih);
            }

            Bitmap resizedBitmap = new Bitmap(image, new Size(nw, nh));
            Image resizedImage = (Image)resizedBitmap;

            return resizedImage;
        }

        public static int GiamGia(int gia, int giam)
        {
            return (int)(gia - gia * (giam / 100.0));
        }

        public static void FitTextBox(TextBox textBox, int w = 10, int h = 10)
        {
            Size textSize = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            //MessageBox.Show(textBox.Width.ToString());
            textBox.Height = textSize.Height * (1 + textSize.Width / textBox.Width) + h;
            textBox.Width = textSize.Width + w;
        }
        public static void BSearch<T>(List<T> list, Compare cmp, Equal eql)
        {

        }

        public static string SetGia(int gia)
        {
            string s = "";
            while (gia / 1000 != 0)
            {
                s = "." + (gia % 1000).ToString("D3") + s;
                gia /= 1000;
            }

            s = gia.ToString() + s;

            return s;
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

        public static Control FindControl(Panel panel, string name)
        {
            foreach (Control control in panel.Controls)
            {
                if (control.Name == name)
                    return control;


                if (control.HasChildren)
                {
                    Control foundControl = FindControl(control as Panel, name);
                    if (foundControl != null)
                        return foundControl;
                }
            }
            return null;
        }

        public static SanPham[] LoadListSanPham(params string[] list)
        {
            QLSanPham listSP = new QLSanPham();

            foreach(string maSP in list)
            {
                listSP.Add(HeThong.LoadSanPham(maSP));
            }

            return listSP.ToArray();
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
