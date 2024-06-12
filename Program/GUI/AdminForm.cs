using Program.BLL;
using Program.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.GUI
{
    public partial class AdminForm : Form
    {
        private List<string> listStr = null;
        private List<string> listMaTB = null;
        private List<string> listNoiDung = null;

        private Admin admin;
        public AdminForm(Admin admin)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;

            BDBiToCaoButton_Click(bdBiToCaoButton, null);

            this.admin = admin;
        }

        private void DrawPanelBorder(object sender, PaintEventArgs e)
        {
            Control panel = sender as Control;

            using (var pen = new Pen(Color.DarkGray, 1))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(-1, -1, panel.Width + 2, panel.Height));
            }
        }

        private void MouseIn(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.ForeColor != Color.Red)
                button.ForeColor = Color.Salmon;
        }
        private void MouseOut(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.ForeColor != Color.Red)
                button.ForeColor = Color.Black;
        }

        private void BDBiToCaoButton_Click(object sender, EventArgs e)
        {
            label1.Text = "Bài Đăng Bị Tố Cáo";
            baoCaoPanel.Visible = true;
            doiMatKhauPanel.Visible = false;

            foreach (Control con in funcFLPanel.Controls)
                con.ForeColor = Color.Black;

            Button button = sender as Button;
            button.ForeColor = Color.Red;

            listStr = new List<string>();
            listNoiDung = new List<string>();
            baoCaoFLP.Controls.Clear();

            BLL_Admin.Instance.SetListBDViPham(ref listStr, ref listNoiDung, ref listMaTB);

            for(int i = 0; i < listStr.Count; i++)
            {
                baoCaoFLP.Controls.Add(DrawBaoCaoBDPanel(listStr[i], listNoiDung[i]));
            }
            if (baoCaoFLP.Controls.Count == 0)
                baoCaoFLP.Controls.Add(khongCoTBPanel);
            GUI_Utils.Instance.FitFLPHeight(baoCaoFLP);

        }

        private void DGBiBaoCaoButton_Click(object sender, EventArgs e)
        {
            label1.Text = "Đánh Giá Bị Báo Cáo";
            baoCaoPanel.Visible = true;
            doiMatKhauPanel.Visible = false;

            foreach (Control con in funcFLPanel.Controls)
                con.ForeColor = Color.Black;

            Button button = sender as Button;
            button.ForeColor = Color.Red;

            BLL_Admin.Instance.SetListDGViPham(ref listStr, ref listNoiDung, ref listMaTB);

            baoCaoFLP.Controls.Clear();
            for (int i = 0; i < listStr.Count; i++)
            {
                baoCaoFLP.Controls.Add(DrawBaoCaoDGPanel(listStr[i], listNoiDung[i]));
            }
            if (baoCaoFLP.Controls.Count == 0)
                baoCaoFLP.Controls.Add(khongCoTBPanel);

            GUI_Utils.Instance.FitFLPHeight(baoCaoFLP);
        }

        private void YCGoBDViPhamButton_Click(object sender, EventArgs e)
        {
            label1.Text = "Yêu Cầu Gỡ Bài Đăng Vi Phạm";
            baoCaoPanel.Visible = true;
            doiMatKhauPanel.Visible = false;

            foreach (Control con in funcFLPanel.Controls)
                con.ForeColor = Color.Black;

            Button button = sender as Button;
            button.ForeColor = Color.Red;

            BLL_Admin.Instance.SetListYCGoViPhamBD(ref listStr, ref listNoiDung, ref listMaTB);

            baoCaoFLP.Controls.Clear();
            for (int i = 0; i < listStr.Count; i++)
            {
                baoCaoFLP.Controls.Add(DrawYCGoViPhamBDPane(listStr[i], listNoiDung[i]));
            }
            if (baoCaoFLP.Controls.Count == 0)
                baoCaoFLP.Controls.Add(khongCoTBPanel);
            GUI_Utils.Instance.FitFLPHeight(baoCaoFLP);
        }

        private Panel DrawYCGoViPhamBDPane(string maBD, string noiDungBC)
        {
            Panel panel = new Panel
            {
                BackColor = Color.White,
                Margin = panel5.Margin,
                Size = panel5.Size,
                Parent = baoCaoFLP,
            };
            panel.Paint += DrawPanelBorder;

            using (Bitmap bmp = GUI_Utils.Instance.LoadImage(BLL_BaiDang.Instance.GetURLFromMaBD(maBD)))
            {
                PictureBox pictureBox = new PictureBox
                {
                    Image = GUI_Utils.Instance.Resize(bmp, pictureBox1.Size),
                    Size = pictureBox1.Size,
                    Location = pictureBox1.Location,
                    BorderStyle = BorderStyle.None,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.White,
                    Parent = panel
                };
                panel.Controls.Add(pictureBox);
            }

            TextBox tieuDe = new TextBox
            {
                Text = BLL_BaiDang.Instance.GetTieuDeFromMaBD(maBD),
                Location = textBox1.Location,
                Font = textBox1.Font,
                Size = textBox1.Size,
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Multiline = true,
                Parent = panel
            };
            GUI_Utils.Instance.FitTextBoxMultiLines(tieuDe);
            panel.Controls.Add(tieuDe);


            TextBox noiDung = new TextBox
            {
                Text = noiDungBC,
                Location = textBox2.Location,
                Font = textBox2.Font,
                Size = textBox2.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            GUI_Utils.Instance.FitTextBox(noiDung);
            panel.Controls.Add(noiDung);

            Button xemChiTiet = new Button
            {
                Text = "Xem Chi Tiết",
                Location = button3.Location,
                Size = button3.Size,
                ForeColor = Color.DarkGray,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Anchor = button3.Anchor,
                Parent = panel
            };
            xemChiTiet.Click += XemChiTiet1BDButton_Click;
            xemChiTiet.FlatAppearance.BorderSize = 1;
            xemChiTiet.FlatAppearance.MouseDownBackColor = Color.Transparent;
            xemChiTiet.FlatAppearance.MouseOverBackColor = Color.Transparent;

            return panel;
        }

        private Panel DrawBaoCaoDGPanel(string maDG, string noiDungBC)
        {
            DanhGia danhGia = BLL_DanhGia.Instance.GetDanhGiaFromMaDG(maDG);

            Panel panel = new Panel
            {
                BackColor = Color.White,
                Margin = panel5.Margin,
                Size = panel5.Size,
                Parent = baoCaoFLP,
            };
            panel.Paint += DrawPanelBorder;

            using (Bitmap bmp = GUI_Utils.Instance.LoadImage(BLL_BaiDang.Instance.GetURLFromMaBD(danhGia.maBD)))
            {
                PictureBox pictureBox = new PictureBox
                {
                    Image = GUI_Utils.Instance.Resize(bmp, pictureBox2.Size),
                    Size = pictureBox2.Size,
                    Location = pictureBox2.Location,
                    BorderStyle = BorderStyle.None,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.White,
                    Parent = panel
                };
                panel.Controls.Add(pictureBox);
            }

            TextBox tieuDe = new TextBox
            {
                Text = danhGia.noiDung,
                Location = textBox4.Location,
                Font = textBox4.Font,
                Size = textBox4.Size,
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Multiline = true,
                Parent = panel
            };
            panel.Controls.Add(tieuDe);


            TextBox lyDo = new TextBox
            {
                Text = noiDungBC,
                Location = textBox5.Location,
                Font = textBox5.Font,
                Size = textBox5.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            GUI_Utils.Instance.FitTextBox(lyDo);
            panel.Controls.Add(lyDo);

            Button xemChiTiet = new Button
            {
                Text = "Xem Chi Tiết",
                Location = button5.Location,
                Size = button5.Size,
                ForeColor = Color.DarkGray,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Anchor = button5.Anchor,
                Parent = panel
            };
            xemChiTiet.Click += XemChiTietDGButton_Click;
            xemChiTiet.FlatAppearance.BorderSize = 1;
            xemChiTiet.FlatAppearance.MouseDownBackColor = Color.Transparent;
            xemChiTiet.FlatAppearance.MouseOverBackColor = Color.Transparent;

            return panel;
        }

        private Panel DrawBaoCaoBDPanel(string maBD, string noiDungBC)
        {
            Panel panel = new Panel
            {
                BackColor = Color.White,
                Margin = panel5.Margin,
                Size = panel5.Size,
                Parent = baoCaoFLP,
            };
            panel.Paint += DrawPanelBorder;

            using (Bitmap bmp = GUI_Utils.Instance.LoadImage(BLL_BaiDang.Instance.GetURLFromMaBD(maBD)))
            {
                PictureBox pictureBox = new PictureBox
                {
                    Image = GUI_Utils.Instance.Resize(bmp, pictureBox1.Size),
                    Size = pictureBox1.Size,
                    Location = pictureBox1.Location,
                    BorderStyle = BorderStyle.None,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.White,
                    Parent = panel
                };
                panel.Controls.Add(pictureBox);
            }

            TextBox tieuDe = new TextBox
            {
                Text = BLL_BaiDang.Instance.GetTieuDeFromMaBD(maBD),
                Location = textBox1.Location,
                Font = textBox1.Font,
                Size = textBox1.Size,
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Multiline = true,
                Parent = panel
            };
            GUI_Utils.Instance.FitTextBoxMultiLines(tieuDe);
            panel.Controls.Add(tieuDe);


            TextBox noiDung = new TextBox
            {
                Text = noiDungBC,
                Location = textBox2.Location,
                Font = textBox2.Font,
                Size = textBox2.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            GUI_Utils.Instance.FitTextBox(noiDung);
            panel.Controls.Add(noiDung);

            Button xemChiTiet = new Button
            {
                Text = "Xem Chi Tiết",
                Location = button3.Location,
                Size = button3.Size,
                ForeColor = Color.DarkGray,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Anchor = button3.Anchor,
                Parent = panel
            };
            xemChiTiet.Click += XemChiTietBDButton_Click;
            xemChiTiet.FlatAppearance.BorderSize = 1;
            xemChiTiet.FlatAppearance.MouseDownBackColor = Color.Transparent;
            xemChiTiet.FlatAppearance.MouseOverBackColor = Color.Transparent;

            return panel;
        }

        private void XemChiTiet1BDButton_Click(object sender, EventArgs e)
        {
            int index = baoCaoFLP.Controls.IndexOf(((Button)sender).Parent);

            DimForm dim = new DimForm();
            dim.Show();
            ReportForm form = new ReportForm(BLL_BaiDang.Instance.GetBaiDangViPhamFromMaBD(listStr[index]), listNoiDung[index], listMaTB[index], true);
            form.ShowDialog();
            dim.Close();

            YCGoBDViPhamButton_Click(button4, null);
        }

        private void XemChiTietBDButton_Click(object sender, EventArgs e)
        {
            int index = baoCaoFLP.Controls.IndexOf(((Button)sender).Parent);

            DimForm dim = new DimForm();
            dim.Show();
            ReportForm form = new ReportForm(BLL_BaiDang.Instance.GetBaiDangDangHoatDongFromMaBD(listStr[index]), listNoiDung[index], listMaTB[index]);
            form.ShowDialog();
            dim.Close();

            BDBiToCaoButton_Click(bdBiToCaoButton, null);
        }

        private void XemChiTietDGButton_Click(object sender, EventArgs e)
        {
            int index = baoCaoFLP.Controls.IndexOf(((Button)sender).Parent);

            DimForm dim = new DimForm();
            dim.Show();
            ReportForm form = new ReportForm(BLL_DanhGia.Instance.GetDanhGiaFromMaDG(listStr[index]), listNoiDung[index], listMaTB[index]);
            form.ShowDialog();
            dim.Close();

            DGBiBaoCaoButton_Click(dgBiBaoCaoButton, null);
        }

        private void DangXuatButton_Click(object sender, EventArgs e)
        {
            Hide();
            Dispose();
            DangNhap_Form form = new DangNhap_Form(true);
            form.ShowDialog();
        }

        private void DoiMatKhauButton_Click(object sender, EventArgs e)
        {
            foreach (Control con in funcFLPanel.Controls)
                con.ForeColor = Color.Black;

            Button button = sender as Button;
            button.ForeColor = Color.Red;

            baoCaoPanel.Visible = false;
            doiMatKhauPanel.Visible = true;


        }

        private void xacNhan_UP_Button_Click(object sender, EventArgs e)
        {
            if (!admin.matKhau.Equals(matKhauCu_Box.Text))
                saiMKC_Text.Visible = true;
            else
            {
                BLL_Admin.Instance.DoiMatKhau(admin, matKhauMoi1_Box.Text);

                ThongBaoForm form = new ThongBaoForm("Đổi mật khẩu thành công!!");
                form.Show();

                if (saiMKC_Text.Visible)
                    saiMKC_Text.Visible = false;

                matKhauCu_Box.Clear();
                matKhauMoi1_Box.Clear();
                matKhauMoi2_Box.Clear();
            }
        }
    }
}   
