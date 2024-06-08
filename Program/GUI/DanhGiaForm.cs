using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Program.BLL;
using Program.DTO;
using Program.Properties;

namespace Program.GUI
{
    public partial class DanhGiaForm : Form
    {
        public SendData send;
        private bool isAdd = true;
        private List<DanhGia> listDanhGia = null;

        public DanhGiaForm(SendData sender, DonHang donHang)
        {
            isAdd = true;
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

            this.send = sender;
            listDanhGia = BLL_DanhGia.Instance.TaoDGMoiTuDH(donHang);
            Init();
        }

        public DanhGiaForm(SendData sender, DanhGia danhGia)
        {
            isAdd = false;
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

            this.send = sender;
            listDanhGia = new List<DanhGia>
            {
                danhGia
            };
            ListDanhGiaFLP.Controls.Clear();
            ListDanhGiaFLP.Controls.Add(DrawDanhGiaPanel(danhGia));

            if (danhGia.sao == 1)
            {
                StarButton_Click(GUI_Utils.Instance.FindControl(ListDanhGiaFLP.Controls[0] as Panel, "star1"), null);
            }
            else if (danhGia.sao == 2)
            {
                StarButton_Click(GUI_Utils.Instance.FindControl(ListDanhGiaFLP.Controls[0] as Panel, "star2"), null);
            }
            else if (danhGia.sao == 3)
            {
                StarButton_Click(GUI_Utils.Instance.FindControl(ListDanhGiaFLP.Controls[0] as Panel, "star3"), null);
            }
            else if (danhGia.sao == 4)
            {
                StarButton_Click(GUI_Utils.Instance.FindControl(ListDanhGiaFLP.Controls[0] as Panel, "star4"), null);
            }
            else if (danhGia.sao == 5)
            {
                StarButton_Click(GUI_Utils.Instance.FindControl(ListDanhGiaFLP.Controls[0] as Panel, "star5"), null);
            }

        }

        private void Init()
        {
            ListDanhGiaFLP.Controls.Clear();
            foreach (DanhGia danhGia in listDanhGia)
            {
                ListDanhGiaFLP.Controls.Add(DrawDanhGiaPanel(danhGia));
            }
        }

        private void troVeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void hoanThanhButton_Click(object sender, EventArgs e)
        {
            if (isAdd)
            {
                for (int i = 0; i < listDanhGia.Count; i++)
                {
                    Panel danhGiaPanel = ListDanhGiaFLP.Controls[i] as Panel;

                    listDanhGia[i].ngayThem = DateTime.Now;
                    listDanhGia[i].thietKeBia = ((TextBox)GUI_Utils.Instance.FindControl(danhGiaPanel, "thietKeBia")).Text;
                    listDanhGia[i].doiTuong = ((TextBox)GUI_Utils.Instance.FindControl(danhGiaPanel, "doiTuong")).Text;
                    listDanhGia[i].noiDung = ((TextBox)GUI_Utils.Instance.FindControl(danhGiaPanel, "noiDung")).Text;

                }
                send(listDanhGia.ToArray());
            }
            else
            {
                listDanhGia[0].ngayThem = DateTime.Now;
                listDanhGia[0].thietKeBia = ((TextBox)GUI_Utils.Instance.FindControl(ListDanhGiaFLP.Controls[0] as Panel, "thietKeBia")).Text;
                listDanhGia[0].doiTuong = ((TextBox)GUI_Utils.Instance.FindControl(ListDanhGiaFLP.Controls[0] as Panel, "doiTuong")).Text;
                listDanhGia[0].noiDung = ((TextBox)GUI_Utils.Instance.FindControl(ListDanhGiaFLP.Controls[0] as Panel, "noiDung")).Text;
                send(listDanhGia.ToArray());
            }

            Close();
        }

        private Panel DrawDanhGiaPanel(DanhGia danhGia)
        {
            Panel panel = new Panel
            {
                Size = panel1.Size,
                BackColor = Color.White,
                Margin = panel1.Margin,
                Parent = ListDanhGiaFLP
            };

            using (Bitmap bmp = new Bitmap(BLL_BaiDang.Instance.GetURLFromMaBD(danhGia.maBD)))
            {
                PictureBox image = new PictureBox
                {
                    Image = GUI_Utils.Instance.Resize(bmp, pictureBox1.Size),
                    Size = pictureBox1.Size,
                    Location = pictureBox1.Location,
                    BackColor = Color.White,
                    Parent = panel
                };
            }

            TextBox tieuDeBD = new TextBox
            {
                Text = Utils.Instance.GioiHangKyTu(BLL_BaiDang.Instance.GetTieuDeFromMaBD(danhGia.maBD), 80),
                Size = textBox1.Size,
                Font = textBox1.Font,
                Location = textBox1.Location,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                BackColor = Color.White,
                Parent = panel
            };
            panel.Controls.Add(tieuDeBD);

            TextBox sanPhamDaMua = new TextBox
            {
                Text = Utils.Instance.GioiHangKyTu("Sản phẩm đã mua: " + danhGia.sanPhamDaMua, 60),
                Size = textBox2.Size,
                Font = textBox2.Font,
                Location = textBox2.Location,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.DarkGray,
                ReadOnly = true,
                BackColor = Color.White,
                Parent = panel
            };
            panel.Controls.Add(sanPhamDaMua);

            TextBox chatLuongSP = new TextBox
            {
                Text = "Chất lượng sản phẩm",
                Size = textBox3.Size,
                Font = textBox3.Font,
                Location = textBox3.Location,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                BackColor = Color.White,
                Parent = panel
            };
            panel.Controls.Add(chatLuongSP);

            Button button1 = new Button
            {
                Name = "star1",
                Size = star1.Size,
                Location = star1.Location,
                Image = Resources.star1,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.White,
                Parent = panel

            };
            button1.Click += StarButton_Click;
            button1.FlatAppearance.MouseDownBackColor = Color.White;
            button1.FlatAppearance.CheckedBackColor = Color.Transparent;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            panel.Controls.Add(button1);

            Button button2 = new Button
            {
                Name = "star2",
                Size = star2.Size,
                Location = star2.Location,
                Image = Resources.star1,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.White,
                Parent = panel

            };
            button2.Click += StarButton_Click;
            button2.FlatAppearance.MouseDownBackColor = Color.White;
            button2.FlatAppearance.CheckedBackColor = Color.Transparent;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            panel.Controls.Add(button2);

            Button button3 = new Button
            {
                Name = "star3",
                Size = star3.Size,
                Location = star3.Location,
                Image = Resources.star1,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.White,
                Parent = panel

            };
            button3.Click += StarButton_Click;
            button3.FlatAppearance.MouseDownBackColor = Color.White;
            button3.FlatAppearance.CheckedBackColor = Color.Transparent;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button3.FlatAppearance.MouseOverBackColor = Color.Transparent;
            panel.Controls.Add(button3);

            Button button4 = new Button
            {
                Name = "star4",
                Size = star4.Size,
                Location = star4.Location,
                Image = Resources.star1,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.White,
                Parent = panel

            };
            button4.Click += StarButton_Click;
            button4.FlatAppearance.MouseDownBackColor = Color.White;
            button4.FlatAppearance.CheckedBackColor = Color.Transparent;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button4.FlatAppearance.MouseOverBackColor = Color.Transparent;
            panel.Controls.Add(button4);

            Button button5 = new Button
            {
                Name = "star5",
                Size = star5.Size,
                Location = star5.Location,
                Image = Resources.star1,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.White,
                Parent = panel

            };
            button5.Click += StarButton_Click;
            button5.FlatAppearance.MouseDownBackColor = Color.White;
            button5.FlatAppearance.CheckedBackColor = Color.Transparent;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button5.FlatAppearance.MouseOverBackColor = Color.Transparent;
            panel.Controls.Add(button5);

            TextBox chatLuongSPTex = new TextBox
            {
                Name = "ChatLuongSP",
                Text = "Tuyệt vời",
                Size = textBox4.Size,
                Font = textBox4.Font,
                Location = textBox4.Location,
                ForeColor = Color.Gold,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                BackColor = Color.White,
                Parent = panel
            };
            panel.Controls.Add(chatLuongSPTex);

            Panel backPanel = new Panel
            {
                Location = panel2.Location,
                Size = panel2.Size,
                BackColor = Color.WhiteSmoke,
                Margin = panel2.Margin,
                Parent = panel
            };
            panel.Controls.Add(backPanel);

            Panel textPanel = new Panel
            {
                Location = panel3.Location,
                Size = panel3.Size,
                BackColor = Color.White,
                Margin = panel3.Margin,
                Parent = backPanel
            };
            backPanel.Controls.Add(textPanel);

            TextBox thietKeBiaLB = new TextBox
            {
                Text = "Thiết kế bìa:",
                Size = textBox5.Size,
                Font = textBox5.Font,
                Location = textBox5.Location,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.DimGray,
                ReadOnly = true,
                BackColor = Color.White,
                Parent = textPanel
            };
            textPanel.Controls.Add(thietKeBiaLB);

            TextBox thietKeBia = new TextBox
            {
                Text = danhGia.thietKeBia,
                Name = "thietKeBia",
                Size = textBox6.Size,
                Font = textBox6.Font,
                Location = textBox6.Location,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Parent = textPanel
            };
            textPanel.Controls.Add(thietKeBia);

            TextBox dtDocGiaLB = new TextBox
            {
                Text = "Đối tượng đọc giả:",
                Size = textBox7.Size,
                Font = textBox7.Font,
                Location = textBox7.Location,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.DimGray,
                ReadOnly = true,
                BackColor = Color.White,
                Parent = textPanel
            };
            textPanel.Controls.Add(dtDocGiaLB);

            TextBox dtDocGia = new TextBox
            {
                Text = danhGia.doiTuong,
                Name = "doiTuong",
                Size = textBox8.Size,
                Font = textBox8.Font,
                Location = textBox8.Location,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Parent = textPanel
            };
            textPanel.Controls.Add(dtDocGia);

            TextBox noiDungDGLB = new TextBox
            {
                Text = "Nội dung đánh giá:",
                Size = textBox9.Size,
                Font = textBox9.Font,
                Location = textBox9.Location,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.DimGray,
                ReadOnly = true,
                BackColor = Color.White,
                Parent = textPanel
            };
            textPanel.Controls.Add(noiDungDGLB);

            TextBox noiDungDG = new TextBox
            {
                Text = danhGia.noiDung,
                Name = "noiDung",
                Size = textBox10.Size,
                Font = textBox10.Font,
                Location = textBox10.Location,
                BorderStyle = BorderStyle.None,
                Multiline = true,
                BackColor = Color.White,
                Parent = textPanel
            };
            textPanel.Controls.Add(noiDungDGLB);

            ListDanhGiaFLP.Controls.Add(panel);
            return panel;
        }

        private void StarButton_Click(object sender, EventArgs e)
        {
            Panel parent = (Panel)((Control)sender).Parent;
            int soSao = int.Parse(((Control)sender).Name.Substring(4));
            int index = ListDanhGiaFLP.Controls.IndexOf(parent);

            listDanhGia[index].sao = soSao;


            for (int i = 1; i <= 5; i++)
            {
                if(i > soSao)
                    ((Button)GUI_Utils.Instance.FindControl(parent, "star" + i.ToString())).Image = Resources.star0;
                else
                    ((Button)GUI_Utils.Instance.FindControl(parent, "star" + i.ToString())).Image = Resources.star1;
            }

            TextBox txt = ((TextBox)GUI_Utils.Instance.FindControl(parent, "ChatLuongSP"));

            if (soSao == 1)
            {
                txt.Text = "Tệ";
                txt.ForeColor = Color.Black;
            }
            else if (soSao == 2)
            {
                txt.Text = "Không hài lòng";
                txt.ForeColor = Color.Black;
            }
            else if (soSao == 3)
            {
                txt.Text = "Bình thường";
                txt.ForeColor = Color.Black;
            }
            else if (soSao == 4)
            {
                txt.Text = "Hài lòng";
                txt.ForeColor = Color.Gold;
            }
            else if (soSao == 5)
            {
                txt.Text = "Tuyệt vời";
                txt.ForeColor = Color.Gold;
            }
        }
    }
}
