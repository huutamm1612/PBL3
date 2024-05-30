using Program.BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.GUI
{
    internal class GUI_Utils
    {
        private static GUI_Utils _Instance;
        public static GUI_Utils Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new GUI_Utils();
                return _Instance;
            }
            private set { }
        }
        private GUI_Utils()
        {

        }

        public string GetMaDHByClick(object sender)
        {
            Panel headPanel = (FlowLayoutPanel)(((Control)sender).Parent.Parent);
            string maDH = FindControl(headPanel, "maDH").Text.Substring(15);
            return maDH;
        }

        public PictureBox CreateStarRatingPictureBox(double rating, int starCount = 5, int starSize = 20, int padding = 5)
        {
            int width = (starSize + padding) * starCount;
            int height = starSize + padding;

            // Tạo PictureBox
            PictureBox pictureBox = new PictureBox
            {
                Width = width,
                Height = height,
                Image = new Bitmap(width, height)
            };

            // Tạo Bitmap để vẽ
            using (Graphics g = Graphics.FromImage(pictureBox.Image))
            {
                g.Clear(Color.White); // Nền trắng
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int fullStars = (int)Math.Floor(rating);
                float partialStar = (float)(rating - fullStars);

                for (int i = 0; i < starCount; i++)
                {
                    float x = i * (starSize + padding);
                    if (i < fullStars)
                    {
                        DrawStar(g, Brushes.Red, x, 0, starSize);
                    }
                    else if (i == fullStars && partialStar > 0)
                    {
                        DrawPartialStar(g, Brushes.Red, x, 0, starSize, partialStar);
                    }
                    else
                    {
                        DrawStar(g, Brushes.Transparent, x, 0, starSize);
                    }

                    g.DrawPolygon(Pens.Red, CreateStarPoints(x, 0, starSize));
                }
            }

            return pictureBox;
        }

        private void DrawStar(Graphics g, Brush brush, float x, float y, int size)
        {
            var starPoints = CreateStarPoints(x, y, size);
            g.FillPolygon(brush, starPoints);
        }

        private void DrawPartialStar(Graphics g, Brush brush, float x, float y, int size, float fillPercentage)
        {
            var starPoints = CreateStarPoints(x, y, size);
            Region starRegion = new Region(new GraphicsPath(starPoints, Enumerable.Repeat((byte)PathPointType.Line, starPoints.Length).ToArray()));
            RectangleF clipRect = new RectangleF(x, y, size * fillPercentage, size);
            starRegion.Intersect(clipRect);
            g.FillRegion(brush, starRegion);
        }

        private PointF[] CreateStarPoints(float x, float y, int size)
        {
            PointF[] points = new PointF[10];
            double angle = -Math.PI / 2;
            double increment = Math.PI / 5;

            for (int i = 0; i < 10; i++)
            {
                float length = i % 2 == 0 ? size / 2f : size / 4f;
                points[i] = new PointF(
                    x + (float)(Math.Cos(angle) * length + size / 2),
                    y + (float)(Math.Sin(angle) * length + size / 2));
                angle += increment;
            }
            return points;
        }

        public void FitTextBox(Control textBox, int w = 10, int h = 10)
        {
            Size textSize = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            //MessageBox.Show(textBox.Width.ToString());
            textBox.Height = textSize.Height * (1 + textSize.Width / textBox.Width) + h;
            textBox.Width = textSize.Width + w;
        }
        public Image Resize(Image image, Size newSize)
        {
            int nw = newSize.Width; int nh = newSize.Height;
            int iw = image.Width; int ih = image.Height;

            if (iw > ih)
            {
                nh = (int)(ih * (double)nw / (double)iw);
            }
            else
            {
                nw = (int)(iw * (double)nh / (double)ih);
            }

            Bitmap resizedBitmap = new Bitmap(image, new Size(nw, nh));
            Image resizedImage = (Image)resizedBitmap;

            return resizedImage;
        }
        public Control FindControl(Panel panel, string name)
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

        public void DrawRectangle(Graphics g, RectangleF rect, Color color, float radius = 0, Pen pen = null)
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

        public void PictureBoxToCircle_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pic = sender as PictureBox;

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, pic.Width, pic.Height);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.CompositingMode = CompositingMode.SourceOver;


            using (Pen pen = new Pen(pic.Parent.BackColor, 2))
            {
                e.Graphics.DrawEllipse(pen, 1, 1, pic.Width - 1, pic.Height - 1);
            }

            pic.Region = new Region(path);
        }
    }
}
