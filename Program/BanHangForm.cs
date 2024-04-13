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
            miniConTrolPanel.Visible = false;
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
                listBaiDang = new QLBaiDang(),
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
                    QLSP = new QLSanPham();
                    SwitchPanel(themSanPhamPanel);
                    miniConTrolPanel.Visible = true;
                    miniConTrolPanel.BringToFront();
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

            Panel parentPanel = new Panel
            {
                Size = formThemSPPanel.Size,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = color1
            };

            Panel panel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1420, 585),
                BackColor = color1
            };

            TextBox anhSP = new TextBox
            {
                Location = textBox28.Location,
                Text = "Ảnh sản phẩm",
                Size = textBox28.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbTenSP = new TextBox
            {
                Location = textBox13.Location,
                Text = "Tên sản phẩm",
                Size = textBox13.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbTacGia = new TextBox
            {
                Location = textBox18.Location,
                Text = "Tác giả",
                Size = textBox18.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbDichGia = new TextBox
            {
                Location = textBox19.Location,
                Text = "Dịch giả",
                Size = textBox19.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbNhaXB = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Location = textBox27.Location,
                Text = "Nhà xuất bản",
                Size = textBox27.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbNamXB = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Location = textBox30.Location,
                Text = "Năm xuất bản",
                Size = textBox30.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbMoTa = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Location = textBox38.Location,
                Text = "Mô tả",
                Size = textBox38.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbTheLoai = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Location = textBox16.Location,
                Text = "Thể loại",
                Size = textBox16.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbLoaiBia = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Location = textBox17.Location,
                Text = "Loại bìa",
                Size = textBox17.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbNgonNgu = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Location = textBox35.Location,
                Text = "Ngôn ngữ",
                Size = textBox35.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbGia = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Location = textBox34.Location,
                Text = "Giá",
                Size = textBox34.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbSoLuong = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Location = textBox37.Location,
                Text = "Số lượng",
                Size = textBox37.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox lbSoTrang = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Location = textBox23.Location,
                Text = "Số trang",
                Size = textBox23.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };           
            TextBox txtSP = new TextBox
            {
                Location = tenSP_Text.Location,
                Text = tenSP_Text.Text,
                Size = tenSP_Text.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 0,
            };
            TextBox txtTG = new TextBox
            {
                Location = tenTacGia_Text.Location,
                Text = tenTacGia_Text.Text,
                Size = tenTacGia_Text.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 1,
            };
            TextBox txtDG = new TextBox
            {
                Location = tenDichGia_Text.Location,
                Text = tenDichGia_Text.Text,
                Size = tenDichGia_Text.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 2,
            };
            TextBox txtNXB = new TextBox
            {
                Text = nhaXuatBan_Text.Text,
                Location = nhaXuatBan_Text.Location,
                Size = nhaXuatBan_Text.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 3,
            };
            TextBox txtNam = new TextBox
            {
                Text = namXuatBan_Text.Text,
                Location = namXuatBan_Text.Location,
                Size = namXuatBan_Text.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 4,
            };
            TextBox txtPrice = new TextBox
            {
                Text = gia_Text.Text,
                Location = gia_Text.Location,
                Size = gia_Text.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 5,
            };
            TextBox txtSL = new TextBox
            {
                Text = soLuong_Text.Text,
                Location = soLuong_Text.Location,
                Size = soLuong_Text.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 6,
            };
            TextBox txtTrang = new TextBox
            {
                Text = soTrang_Text.Text,
                Location = soTrang_Text.Location,
                Size = soTrang_Text.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 7,
            };
            TextBox txtMota = new TextBox
            {
                Text = moTaSP_Text.Text,
                Location = moTaSP_Text.Location,
                Size = moTaSP_Text.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 8,
            };
            TextBox txt10 = new TextBox
            {
                Location = Count_SP.Location,
                ForeColor = Color.Silver,
                BorderStyle = BorderStyle.None,
                Text = Count_SP.Text,
                Size = Count_SP.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                Enabled = false,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 8,
            };
            TextBox txt15 = new TextBox
            {
                Location = Count_MoTa.Location,
                ForeColor = Color.Silver,
                BorderStyle = BorderStyle.None,
                Text = Count_MoTa.Text,
                Size = Count_MoTa.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                Enabled = false,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 20,
            };
            TextBox txt11 = new TextBox
            {
                Location = Count_TG.Location,
                ForeColor = Color.Silver,
                BorderStyle = BorderStyle.None,
                Text = Count_TG.Text,
                Size = Count_TG.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                Enabled = false,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 17,
            };
            TextBox txt12 = new TextBox
            {
                Location = Count_Dichgia.Location,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.Silver,
                Text = Count_Dichgia.Text,
                Size = Count_Dichgia.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                Enabled = false,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 20,
            };
            TextBox txt13 = new TextBox
            {
                Location = Count_NXB.Location,
                ForeColor = Color.Silver,
                BorderStyle = BorderStyle.None,
                Text = Count_NXB.Text,
                Size = Count_NXB.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                Enabled = false,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 26,
            };
            PictureBox picSP = new PictureBox
            {
                Location = SP_Pic.Location,
                Size = SP_Pic.Size,
                Font = font1,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = color1
            };
            PictureBox picTG = new PictureBox
            {
                Location = TG_Pic.Location,
                Size = TG_Pic.Size,
                Font = font1,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = color1
            };
            PictureBox pic3 = new PictureBox
            {
                Location = DichGia_Pic.Location,
                Size = DichGia_Pic.Size,
                Font = font1,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = color1
            };
            PictureBox pic4 = new PictureBox
            {
                Location = NXB_Pic.Location,
                Size = NXB_Pic.Size,
                Font = font1,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = color1
            };
            PictureBox pic5 = new PictureBox
            {
                Location = Nam_Pic.Location,
                Size = Nam_Pic.Size,
                Font = font1,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = color1
            };
            PictureBox pic6 = new PictureBox
            {
                Location = Gia_Pic.Location,
                Size = Gia_Pic.Size,
                Font = font1,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = color1
            };
            PictureBox pic7 = new PictureBox
            {
                Location = SoLuong_Pic.Location,
                Size = SoLuong_Pic.Size,
                BorderStyle = BorderStyle.FixedSingle,
                Font = font1,
                BackColor = color1
            };
            PictureBox pic8 = new PictureBox
            {
                Location = SoTrang_Pic.Location,
                Size = SoTrang_Pic.Size,
                BorderStyle = BorderStyle.FixedSingle,
                Font = font1,
                BackColor = color1
            };
            PictureBox pic9 = new PictureBox
            {
                Location = moTa_Pic.Location,
                Size = moTa_Pic.Size,
                BorderStyle = BorderStyle.FixedSingle,
                Font = font1,
                BackColor = color1
            };
            ComboBox cboTheLoai = new ComboBox
            {
                Location = theLoai_CBBox.Location,
                Size = theLoai_CBBox.Size,
                Font = font1,
                BackColor = color1,

            };
            Utils.SetComboBox(cboTheLoai, HeThong.LoadTheLoai());
            cboTheLoai.SelectedIndex = theLoai_CBBox.SelectedIndex;

            ComboBox cboLoaiBia = new ComboBox
            {
                DataSource = loaiBia_CBBox.DataSource,
                Location = loaiBia_CBBox.Location,
                Size = loaiBia_CBBox.Size,
                Font = font1,
                BackColor = color1,

            };
            cboLoaiBia.Items.AddRange( new object[] {"Bìa mềm", "Bìa Cứng"} );
            cboLoaiBia.SelectedIndex = loaiBia_CBBox.SelectedIndex;

            ComboBox cboLanguage = new ComboBox
            {
                DataSource = ngonNgu_CBBox.DataSource,
                Location = ngonNgu_CBBox.Location,
                Size = ngonNgu_CBBox.Size,
                Font = font1,
                BackColor = color1,

            };
            cboLanguage.Items.AddRange(new object[] 
            {
                "Tiếng Việt",
                "Tiếng Anh",
                "Tiếng Trung Quốc",
                "Tiếng Nhật Bản",
                "Tiếng Nga",
                "Khác"
            });
            cboLanguage.SelectedIndex = ngonNgu_CBBox.SelectedIndex;

            panel.Controls.Add(anhSP);
            panel.Controls.Add(lbSoTrang);
            panel.Controls.Add(txtSP);
            panel.Controls.Add(txtTG);
            panel.Controls.Add(txtDG);
            panel.Controls.Add(txtNXB);
            panel.Controls.Add(txtNam);
            panel.Controls.Add(txtPrice);
            panel.Controls.Add(txtSL);
            panel.Controls.Add(txtTrang);
            panel.Controls.Add(txtMota);
            panel.Controls.Add(txt15);
            panel.Controls.Add(txt10);
            panel.Controls.Add(txt11);
            panel.Controls.Add(txt12);
            panel.Controls.Add(txt13);
            panel.Controls.Add(picSP);
            panel.Controls.Add(picTG);
            panel.Controls.Add(pic3);
            panel.Controls.Add(pic4);
            panel.Controls.Add(pic5);
            panel.Controls.Add(pic6);
            panel.Controls.Add(pic7);
            panel.Controls.Add(pic8);
            panel.Controls.Add(pic9);
            panel.Controls.Add(cboTheLoai);
            panel.Controls.Add(cboLoaiBia);
            panel.Controls.Add(cboLanguage);
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

            Button btnCapNhat = new Button
            {
                Location = luuSPButton.Location,
                Text = "Cập nhật",
                ForeColor = Color.MistyRose,
                Size = luuSPButton.Size,
                Font = font1,
                BackColor = Color.OrangeRed,
            };
            Button btnXoa = new Button
            {
                Location = huyThemSPButton.Location,
                Text = "Xóa",
                ForeColor = Color.Black,
                Size = huyThemSPButton.Size,
                Font = font1,
                BackColor = color1,
            };

            panel.Enabled = false;

            parentPanel.Controls.Add(panel);
            parentPanel.Controls.Add(btnXoa);
            parentPanel.Controls.Add(btnCapNhat);

            btnCapNhat.Click += Update_Button;
            btnXoa.Click += Remove_Button;

            return parentPanel;
        }

        private void Remove_Button(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Panel panelToRemove = clickedButton.Parent as Panel;
            int panelIndex = listSP_FLPanel.Controls.IndexOf(panelToRemove);

            if (panelIndex != -1 && panelIndex < listSP_FLPanel.Controls.Count)
            {
                listSP_FLPanel.Controls.RemoveAt(panelIndex);
            }
            themSPButton.Location = new Point(themSPButton.Location.X, themSPButton.Location.Y - formThemSPPanel.Size.Height + 20);
        }

        private void Update_Button(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Panel panelToUpdate = clickedButton.Parent as Panel;

            foreach(Control control in panelToUpdate.Controls)
                control.Enabled = true;

        }

        private void luuSPButton_Click(object sender, EventArgs e)
        {
            // kiem tra hop le
            if (tenSP_Text.Text != "" && tenTacGia_Text.Text != "" && tenDichGia_Text.Text != "" && nhaXuatBan_Text.Text != ""
               && moTaSP_Text.Text != "" && namXuatBan_Text.Text != "" && soLuong_Text.Text != "" && ngonNgu_CBBox.SelectedIndex != -1 
               && theLoai_CBBox.SelectedIndex != -1 && loaiBia_CBBox.SelectedIndex != -1 && soTrang_Text.Text != "")
            {
                SanPham sanPham = new SanPham
                {
                    maSP = "",
                    maLoaiSP = theLoai_CBBox.SelectedIndex.ToString("D10"),
                    maBD = shop.listBaiDang.list.Last().maBD,
                    ten = tenDichGia_Text.Text,
                    tacGia = tenDichGia_Text.Text,
                    dichGia = tenDichGia_Text.Text,
                    nhaXuatBan = nhaXuatBan_Text.Text,
                    moTa = moTaSP_Text.Text,
                    namXuatBan = int.Parse(namXuatBan_Text.Text),
                    soLuong = int.Parse(soLuong_Text.Text),
                    ngonNgu = ngonNgu_CBBox.SelectedItem.ToString(),
                    loaiBia = loaiBia_CBBox.SelectedItem.ToString(),
                };
                QLSP.Add(sanPham);

                listSP_FLPanel.Size = new Size(listSP_FLPanel.Size.Width, listSP_FLPanel.Size.Height + formThemSPPanel.Size.Height + 20);
                listSP_FLPanel.Controls.Add(this.sanPhamForm());
                listSP_FLPanel.Visible = true;  
                formThemSPPanel.Visible = false;
                themSPButton.Visible = true;

                refreshThemSPForm(sender, e);   
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
            Count(moTa_txt, Count_MoTaTT, 2000);
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
            Pen pen = new Pen(color, 2);

            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

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
            moTaSP_Text.Text = "";
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

        private void capNhat_Button_Click(object sender, EventArgs e)
        {

        }

        private void luuBaiDangButton_Click(object sender, EventArgs e)
        {
            // kiem tra hop le
            if(/*hople*/ true)
            {
                BaiDang baiDang = new BaiDang
                {
                    maBD = HeThong.MaMoi("MaBD"),
                    maS = shop.maSo,
                    tieuDe = tieuDe_txt.Text,
                    moTa = moTa_txt.Text,
                    giamGia = int.Parse(giamGia_txt.Text),
                    luocThich = 0
                };

                foreach(var sanPham in QLSP.list)
                {
                    baiDang.Add(sanPham);
                }

                QLSP.Clear();
                shop.Insert(0, baiDang);
            }
            else
            {
                // bao loi
            }
        }

        private void huyThemBaiDang_Click(object sender, EventArgs e)
        {
            // refresh va ve trang chu
        }
    }
}
