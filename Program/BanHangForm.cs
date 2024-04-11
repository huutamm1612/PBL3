using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    public partial class BanHang_Form : Form
    {
        private User user;
        private Shop shop = null;
        private Button currTab = null;
        private Panel currPanel = null;
        private QLSanPham QLSP = null;
        public BanHang_Form()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public BanHang_Form(bool daTaoShop)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;

            if (daTaoShop)
            {
                shop = new Shop();
            }
        }

        private void SwitchPanel(Panel newPanel)
        {
            currPanel.Visible = false;
            currPanel = newPanel;
            currPanel.Visible = true;
        }

        private void refreshHomePanel()
        {
            choice_Panel.Visible = true;
            screen_Panel.Visible = true;
            currPanel = trangChuPanel;
            currPanel.Visible = true;
        }

        private void refreshDangKyShopPanel()
        {
            soDT_DK_Text.Text = shop.soDT;
            email_DK_Text.Text = shop.email;
            dangKyPanel.Visible = true;

            if (soDT_DK_Text.Text == "")
            {
                soDT_DK_Text.ReadOnly = false;
                borderSoDT.BackColor = Color.White;
            }
            if (email_DK_Text.Text == "")
            {
                email_DK_Text.ReadOnly = false;
                borderEmail.BackColor = Color.White;
            }

        }

        public void setData(string taiKhoan, string matKhau)
        {
            bool daTaoShop = shop != null ? true : false;

            user = new User();
            user.dangNhap(taiKhoan, matKhau);
            shop = HeThong.LoadShop(taiKhoan);

            if (daTaoShop)
            {
                refreshHomePanel();
            }
            else
            {
                refreshDangKyShopPanel();
            }
        }

        private void taoDiaChi_button_Click(object sender, EventArgs e)
        {
            themDiaChi_Panel.Visible = true;
            if (TTP_ComboBox.Items.Count == 0)
                TTP_ComboBox.DataSource = HeThong.LoadTinh_ThanhPho();
            refreshThemDiaChi_Panel();
        }

        private void soDienThoai_Box_TextChanged(object sender, EventArgs e)
        {
            if (soDienThoai_Box.Text == "...")
                return;

            if (!Utils.KiemTraSoDT(soDienThoai_Box.Text))
            {
                soDTKhongHopLe_Label.Visible = true;
                HTCapNhatDC_Button.Enabled = false;
            }
            else
            {
                soDTKhongHopLe_Label.Visible = false;
                if (diaChiCuThe_Box.Enabled)
                {
                    HTCapNhatDC_Button.Enabled = true;
                }
            }
        }

        private void TTP_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TTP_ComboBox.SelectedIndex != 0)
            {
                QH_ComboBox.Enabled = true;
                QH_ComboBox.DataSource = HeThong.LoadQuan_Huyen(TTP_ComboBox.SelectedIndex);
            }
            else
            {
                QH_ComboBox.Enabled = false;
                QH_ComboBox.DataSource = null;
            }

        }

        private void QH_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QH_ComboBox.SelectedIndex != 0)
            {
                PX_ComboBox.Enabled = true;
                PX_ComboBox.DataSource = HeThong.LoadPhuong_Xa(TTP_ComboBox.SelectedIndex, QH_ComboBox.SelectedIndex);
            }
            else
            {
                PX_ComboBox.Enabled = false;
                PX_ComboBox.DataSource = null;
            }
        }

        private void PX_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PX_ComboBox.SelectedIndex == 0 || PX_ComboBox.Enabled == false)
            {
                diaChiCuThe_Box.Text = "";
                diaChiCuThe_Box.Enabled = false;
                HTCapNhatDC_Button.Enabled = false;
            }
            else
            {
                diaChiCuThe_Box.Enabled = true;
                HTCapNhatDC_Button.Enabled = true;
            }
        }

        private void HTCapNhatDC_Button_Click(object sender, EventArgs e)
        {
            int maT_TP = TTP_ComboBox.SelectedIndex;
            int maQH = maT_TP * 100 + QH_ComboBox.SelectedIndex;
            int maPX = maQH * 100 + PX_ComboBox.SelectedIndex;
            DiaChi diaChi = new DiaChi("", hoVaTen_Box.Text, soDienThoai_Box.Text, maT_TP, maQH, maPX, diaChiCuThe_Box.Text);

            shop.capNhatDiaChi(diaChi);
            diaChi_Text.Text = shop.diaChi.ToString();
            diaChi_Text.ReadOnly = true;
            taoDiaChi_button.Visible = false;
            themDiaChi_Panel.Visible = false;

            diaChiT_panel.Visible = true;
        }

        private void refreshThemDiaChi_Panel()
        {
            hoVaTen_Box.Text = "...";
            soDienThoai_Box.Text = "...";
            TTP_ComboBox.SelectedIndex = 0;
            QH_ComboBox.DataSource = null;
            QH_ComboBox.Enabled = false;
            PX_ComboBox.Enabled = false;
            PX_ComboBox.DataSource = null;
            diaChiCuThe_Box.Enabled = false;
        }
        private void refreshCapNhatDiaChi_Panel(DiaChi diaChi)
        {
            hoVaTen_Box.Text = diaChi.ten;
            soDienThoai_Box.Text = diaChi.soDT;
            TTP_ComboBox.SelectedIndex = diaChi.maT_TP;
            QH_ComboBox.SelectedIndex = diaChi.maQH % 100;
            PX_ComboBox.SelectedIndex = diaChi.maPX % 100;
            diaChiCuThe_Box.Text = diaChi.diaChiCuThe;
            HTCapNhatDC_Button.Visible = true;
        }

        private void backDiaChi_Button_Click(object sender, EventArgs e)
        {
            themDiaChi_Panel.Visible = false;
        }

        private void suaDiaChi_button_Click(object sender, EventArgs e)
        {
            refreshCapNhatDiaChi_Panel(shop.diaChi);
            themDiaChi_Panel.Visible = true;
        }

        private void soDT_DK_Text_TextChanged(object sender, EventArgs e)
        {
            if (Utils.KiemTraSoDT(soDT_DK_Text.Text))
            {
                saiSDT_label.Visible = false;
            }
            else
            {
                saiSDT_label.Visible = true;
            }
        }

        private void HTTaoShop_Click(object sender, EventArgs e)
        {
            shop.diaChi.setMaDC(HeThong.MaMoi("maDC"));
            shop = new Shop
            {
                maSo = HeThong.MaMoi("maS"),
                ten = tenShop_DK_Text.Text,
                soDT = soDT_DK_Text.Text,
                email = email_DK_Text.Text,
                diaChi = shop.diaChi,
                ngaySinh = DateTime.Now,
                listBaiDang = new List<BaiDang>(),
                nFollower = 0,
                tinhTrang = 1,
                doanhThu = 0,
            };

            HeThong.DangKy(user, shop);

            dangKyPanel.Visible = false;
            refreshHomePanel();
        }

        private void hoverMouse(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (!button.Font.Bold)
                button.ForeColor = Color.Salmon;
        }

        private void leaveMouse(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (!button.Font.Bold)
                button.ForeColor = Color.Black;
        }

        private void tabClick(object sender, EventArgs e)
        {
            if (currTab != null && currTab != trangChuButton)
            {
                currTab.Font = new Font(currTab.Font, FontStyle.Regular);
                currTab.ForeColor = Color.Black;
            }

            currTab = sender as Button;

            switch (currTab.Text)
            {
                case "Trang Chủ":
                    SwitchPanel(trangChuPanel);
                    break;

                case "Tất Cả":
                    SwitchPanel(tatCaPanel);
                    break;

                case "Thêm Sản Phẩm":
                    SwitchPanel(themSanPhamPanel);
                    break;

                case "Tất Cả Sản Phẩm":
                    SwitchPanel(tatCaSanPhamPanel);
                    break;
            }

            if (currTab == trangChuButton)
            {
                cloneButton.Text = "";
                cloneButton.Visible = false;
                arrowLabel.Visible = false;
                return;
            }

            cloneButton.Text = currTab.Text;
            cloneButton.Visible = true;
            arrowLabel.Visible = true;
            currTab.Font = new Font(currTab.Font, FontStyle.Bold);
            currTab.ForeColor = Color.OrangeRed;
        }

        private void spreadOutClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int index = funcFLPanel.Controls.IndexOf(button) + 1;

            bool spread = button.Text[0] == '▶';
            button.Text = (spread == true ? "◢" : "▶") + button.Text.Substring(1);

            while (funcFLPanel.Controls[index].Text != "")
            {
                funcFLPanel.Controls[index].Visible = spread;
                index++;
            }

        }

        private void themSPButton_Click(object sender, EventArgs e)//1575, 140
        {
            if (theLoai_CBBox.Items.Count == 0)
                Utils.SetComboBox(theLoai_CBBox, HeThong.LoadTheLoai());

            TTBH_Panel.Size = new Size(TTBH_Panel.Width, TTBH_Panel.Height + formThemSPPanel.Size.Height + 20);
            panel8.Location = new Point(panel8.Location.X, panel8.Location.Y + formThemSPPanel.Size.Height + 20);
            formThemSPPanel.Location = themSPButton.Location;
            themSPButton.Location = new Point(themSPButton.Location.X, themSPButton.Location.Y + formThemSPPanel.Size.Height + 20);
            formThemSPPanel.Visible = true;
            themSPButton.Visible = false;
        }

        private void huyThemSPButton_Click(object sender, EventArgs e)
        {
            refreshThemSPForm(sender, e);
            themSPButton.Location = formThemSPPanel.Location;
            TTBH_Panel.Size = new Size(TTBH_Panel.Width, TTBH_Panel.Height - (formThemSPPanel.Size.Height + 20));
            panel8.Location = new Point(panel8.Location.X, panel8.Location.Y - (formThemSPPanel.Size.Height + 20));
            formThemSPPanel.Visible = false;
            themSPButton.Visible = true;
        }

        private Panel sanPhamForm()
        {
            Color color1 = Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            Font font1 = new Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Panel panel = new Panel
            {
                Size = new Size(1420, 800),
            };

            TextBox lbTenSP = new TextBox
            {
                Location = new Point(54, 170),
                Text = "Tên sản phẩm",
                Size = new Size(122, 19),
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbTacGia = new TextBox
            {
                Location = new Point(54, 240),
                Text = "Tác giả",
                Size = new Size(122, 19),
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbDichGia = new TextBox
            {
                Location = new Point(54, 310),
                Text = "Dịch giả",
                Size = new Size(122, 19),
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbNhaXB = new TextBox
            {
                Location = new Point(54, 380),
                Text = "Nhà xuất bản",
                Size = new Size(122, 19),
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbNamXB = new TextBox
            {
                Location = new Point(54, 450),
                Text = "Năm xuất bản",
                Size = new Size(122, 19),
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbMoTa = new TextBox
            {
                Location = new Point(54, 520),
                Text = "Mô tả",
                Size = new Size(122, 19),
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbTheLoai = new TextBox
            {
                Location = new Point(727, 170),
                Text = "Thể loại",
                Size = new Size(122, 19),
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbLoaiBia = new TextBox
            {
                Location = new Point(727, 240),
                Text = "Loại bìa",
                Size = new Size(122, 19),
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbNgonNgu = new TextBox
            {
                Location = new Point(727, 310),
                Text = "Ngôn ngữ",
                Size = new Size(122, 19),
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbGia = new TextBox
            {
                Location = new Point(727, 380),
                Text = "Giá",
                Size = new Size(122, 19),
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbSoLuong = new TextBox
            {
                Location = new Point(727, 450),
                Text = "Số lượng",
                Size = new Size(122, 19),
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };


            panel.Controls.Add(lbTenSP);
            panel.Controls.Add(lbTacGia);
            panel.Controls.Add(lbDichGia);
            panel.Controls.Add(lbNhaXB);
            panel.Controls.Add(lbNamXB);
            panel.Controls.Add(lbMoTa);
            panel.Controls.Add(lbTheLoai);
            panel.Controls.Add(lbLoaiBia);
            panel.Controls.Add(lbNgonNgu);
            panel.Controls.Add(lbGia);
            panel.Controls.Add(lbSoLuong);




            return panel;
        }

        private void luuSPButton_Click(object sender, EventArgs e)
        {
            // kiem tra hop le
            if (tenSP_Text.Text != "" && tenTacGia_Text.Text != "" && tenDichGia_Text.Text != "" && nhaXuatBan_Text.Text != "")
             //   && moTaSP_Text.Text != "" && namXuatBan_Text.Text != "" && soLuong_Text.Text != "" && ngonNgu_CBBox.SelectedIndex != -1 
            //    && theLoai_CBBox.SelectedIndex != -1 && loaiBia_CBBox.SelectedIndex != -1 && soTrang_Text.Text != "")
            {
                listSP_FLPanel.Controls.Add(this.sanPhamForm());
            }
            else
            {
                Check(tenSP_Text, SP_Pic);
                Check(tenTacGia_Text, TG_Pic);
                Check(tenDichGia_Text, DichGia_Pic);
                Check(nhaXuatBan_Text, NXB_Pic);
                Check(namXuatBan_Text, Nam_Pic);
                Check(soLuong_Text, SoLuong_Pic);
                Check(gia_Text, Gia_Pic);
                Check(soTrang_Text, SoTrang_Pic);
                Check(moTaSP_Text, moTa_Pic);
                if (ngonNgu_CBBox.SelectedIndex != -1)
                    language_Check.Visible = false;
                else
                    language_Check.Visible = true;
                if (loaiBia_CBBox.SelectedIndex != -1)
                    bia_Check.Visible = false;
                else
                    bia_Check.Visible = true;
                if (theLoai_CBBox.SelectedIndex != -1)
                    theLoai_check.Visible = false;
                else
                    theLoai_check.Visible = true;
            }
            // dua du lieu vao QLSP     

                // copy roi them vao FlowLayoutPanel listSP_FLPanel

                // chinh sua lai kich thuoc

                // lam moi va an panel
        }

        private void refreshThemSPForm_Button_Click(object sender, EventArgs e)
        {
            // refresh form
            refreshThemSPForm(sender, e);   
        } 

        private void tenSP_Text_TextChanged(object sender, EventArgs e)
        {
            Count(tenSP_Text, Count_SP, 50);
           // luuSPButton_Click(sender, e);
            Check(tenSP_Text, SP_Pic);

        }

        private void tenTacGia_Text_TextChanged(object sender, EventArgs e)
        {
            Count(tenTacGia_Text, Count_TG, 50);
            //luuSPButton_Click(sender, e);
            Check(tenTacGia_Text, TG_Pic);
        }

        private void tenDichGia_Text_TextChanged(object sender, EventArgs e)
        {
            Count(tenDichGia_Text, Count_Dichgia, 50);
            //luuSPButton_Click(sender, e); Check(tenDichGia_Text, DichGia_Pic);
            Check(tenDichGia_Text, DichGia_Pic);
        }

        private void nhaXuatBan_Text_TextChanged(object sender, EventArgs e)
        {
            Count(nhaXuatBan_Text, Count_NXB, 50);
            //luuSPButton_Click(sender, e);
            Check(nhaXuatBan_Text, NXB_Pic);
        }

        private void moTaSP_Text_TextChanged(object sender, EventArgs e)
        {
            Count(moTaSP_Text, Count_MoTa, 1000);
            //luuSPButton_Click(sender, e);
            Check(moTaSP_Text, moTa_Pic);
        }

        private void moTaTT_txt_TextChanged(object sender, EventArgs e)
        {
            Count(moTaTT_txt, Count_MoTaTT, 2000);
        }
        private void tieuDe_txt_TextChanged(object sender, EventArgs e)
        {
            Count(tieuDe_txt, Count_Tieude, 120);
        }

        private void Nam_Pic_Click(object sender, EventArgs e)
        {
            //luuSPButton_Click(sender, e);
            Check(namXuatBan_Text, Nam_Pic);
        }

        private void Gia_Pic_Click(object sender, EventArgs e)
        {
            //luuSPButton_Click(sender, e);
            Check(gia_Text, Gia_Pic);
        }

        private void SoLuong_Pic_Click(object sender, EventArgs e)
        {
            //luuSPButton_Click(sender, e);
            Check(soLuong_Text, SoLuong_Pic);
        }
        private void SoTrang_Pic_Click(object sender, EventArgs e)
        {
           // luuSPButton_Click(sender, e);
            Check(soTrang_Text, SoTrang_Pic);
        }
        private void Count(TextBox txt, TextBox dem, int max)
        {
            if (txt.Text.Length > max)
            {
                txt.Text = txt.Text.Substring(0, max);
                txt.SelectionStart = max;
                txt.SelectionLength = 0;
            }
            dem.Text = txt.Text.Length + "/" + max;
        }

        private void ngonNgu_CBBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ngonNgu_CBBox.SelectedIndex != -1)
                language_Check.Visible = false;
            else
                language_Check.Visible = true;
        }

        private void theLoai_CBBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (theLoai_CBBox.SelectedIndex != -1)
                theLoai_check.Visible = false;
            else
                theLoai_check.Visible = true;
        }

        private void loaiBia_CBBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaiBia_CBBox.SelectedIndex != -1)
                bia_Check.Visible = false;
            else 
                bia_Check.Visible = true;
        }
        private void AddBorderToPictureBox(PictureBox pictureBox, Color color)
        {
            Pen pen = new Pen(color, 2); // Độ dày của viền là 3 pixel

            // Đặt thuộc tính SizeMode của PictureBox về AutoSize để nó có thể thay đổi kích thước tự động
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

            // Vẽ một hình chữ nhật bao quanh toàn bộ PictureBox để tạo viền
            using (Graphics g = pictureBox.Parent.CreateGraphics())
            {
                g.DrawRectangle(pen, new Rectangle(pictureBox.Location, pictureBox.Size));
            }
        }
        private void Check(TextBox txt, PictureBox pic)
        {
           // pic.BorderStyle = BorderStyle.FixedSingle;
            if (txt.Text == "")
            {
                pic.BorderStyle = BorderStyle.None;
                AddBorderToPictureBox(pic, Color.Red);
            }
            else
            {
                pic.BorderStyle = BorderStyle.FixedSingle;
                AddBorderToPictureBox(pic, Color.White);
            }
        }
        private void refreshThemSPForm(object sender, EventArgs e)
        {
            // refresh form
            tenSP_Text.Text = "";
            theLoai_CBBox.SelectedIndex = -1;
            tenTacGia_Text.Text = "";
            tenDichGia_Text.Text = "";
            nhaXuatBan_Text.Text = "";
            namXuatBan_Text.Text = "";
            loaiBia_CBBox.SelectedIndex = -1;
            ngonNgu_CBBox.SelectedIndex = -1;
            gia_Text.Text = "";
            soLuong_Text.Text = "";
            soTrang_Text.Text = "";
            theLoai_check.Visible = false;
            bia_Check.Visible = false;
            language_Check.Visible = false;

            AddBorderToPictureBox(SP_Pic, Color.White);
            SP_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(TG_Pic, Color.White);
            TG_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(DichGia_Pic, Color.White);
            DichGia_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(DichGia_Pic, Color.White);
            DichGia_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(NXB_Pic, Color.White);
            NXB_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(Nam_Pic, Color.White);
            Nam_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(SoTrang_Pic, Color.White);
            SoTrang_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(SoLuong_Pic, Color.White);
            SoLuong_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(Gia_Pic, Color.White);
            Gia_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(moTa_Pic, Color.White);
            moTa_Pic.BorderStyle = BorderStyle.FixedSingle;

        }

        private void formThemSPPanel_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
