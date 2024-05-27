using Program.BLL;
using Program.DAL;
using Program.DTO;
using Program.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace Program
{
    public delegate void SendBaiDang(BaiDang baiDang);
    public delegate void SendSanPham(SanPham sanPham);

    public partial class BanHang_Form : Form
    {
        private int index = -1;
        private int loaiDH = -2;
        private User user;
        private Shop shop = null;
        private Button currTab = null;
        private Panel currPanel = null;
        private QLSanPham QLSP = null;
        private QLBaiDang QLBD = null;
        private SanPham currSanPham = null;
        private DiaChi diaChi = null;
        public BanHang_Form()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void SwitchPanel(Panel newPanel)
        {
            currPanel.Visible = false;
            currPanel = newPanel;
            currPanel.Visible = true;
            currPanel.AutoScrollPosition = new Point(0, 0);
            miniConTrolPanel.Visible = false;
        }

        private void RefreshHomePanel()
        {
            choice_Panel.Visible = true;
            screen_Panel.Visible = true;
            currPanel = trangChuPanel;
            currPanel.Visible = true;
        }

        private void RefreshDangKyShopPanel()
        {
            dangKyPanel.Visible = true;
        }

        public void setData(string taiKhoan, string matKhau)
        {
            user = new User();
            user.dangNhap(taiKhoan, matKhau);
            shop = BLL_Shop.Instance.GetShopFromTaiKhoan(taiKhoan);

            if (shop != null)
                RefreshHomePanel();
            else
            {
                RefreshDangKyShopPanel();
                shop = new Shop();
            }
        }

        public void ThemDiaChi(DiaChi diaChi)
        {
            this.diaChi = diaChi;
        }

        private void taoDiaChi_button_Click(object sender, EventArgs e)
        {
            DimForm dimForm = new DimForm();
            dimForm.Show();
            DiaChiForm form = new DiaChiForm(ThemDiaChi);
            form.ShowDialog();
            dimForm.Close();

            if(diaChi != null)
            {
                diaChiT_panel.Visible = true;
                diaChi_Text.Text = diaChi.ToString();
                taoDiaChi_button.Visible = false;
            }
        }
        private void suaDiaChi_button_Click(object sender, EventArgs e)
        {
            DimForm dimForm = new DimForm();
            dimForm.Show();
            DiaChiForm form = new DiaChiForm(ThemDiaChi, diaChi);
            form.ShowDialog();
            dimForm.Close();

            if (diaChi != null) 
                diaChi_Text.Text = diaChi.ToString();
        }

        private void soDT_DK_Text_TextChanged(object sender, EventArgs e)
        {
            if (Utils.Instance.KiemTraSoDT(soDT_DK_Text.Text))
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
            diaChi.setMaDC(BLL_DiaChi.Instance.GetMaMoi());
            shop = new Shop
            {
                maSo = BLL_Shop.Instance.GetMaMoi(),
                ten = tenShop_DK_Text.Text,
                soDT = soDT_DK_Text.Text,
                email = email_DK_Text.Text,
                diaChi = diaChi,
                ngaySinh = DateTime.Now,
                listBaiDang = new QLBaiDang(),
                listFollower = new List<string>(),
                tinhTrang = 1,
                doanhThu = 0,
            };

            BLL_Shop.Instance.TaoShopByTaiKhoan(user.taiKhoan, shop);

            dangKyPanel.Visible = false;
            RefreshHomePanel();
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

        private void layTCSP()
        {
            flowLayoutPanel.Controls.Clear();
            foreach(BaiDang baiDang in shop.listBaiDang.list)
            {
                foreach(SanPham sanPham in baiDang.GetAllSP())
                {
                    ThemThongTinSanPham(sanPham);
                }
            }
        }
        private void layTCBD()
        {
            FLLayout_TatCaBaiDang.Controls.Clear();
            foreach (BaiDang baiDang in shop.listBaiDang.list)
            {
                 ThemThongTinBaiDang(baiDang);    
            }
        }

        private void themSanPhamPanel_Scroll(object sender, ScrollEventArgs e)
        {
            miniConTrolPanel.Top = 732;
        }

        private void ThemSPP_MouseWheel(object sender, MouseEventArgs e)
        {
            miniConTrolPanel.Top = 732;
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
                    SwitchPanel(TatCaDHPanel);
                    ChuyenLoaiDH_Click(tatCaDHButton, null);
                    break;

                case "Thêm Sản Phẩm":
                    RefreshThemSPPanel();
                    miniConTrolPanel.Top = 732;
                    QLSP = new QLSanPham();
                    SwitchPanel(themSanPhamPanel);
                    themSanPhamPanel.MouseWheel += ThemSPP_MouseWheel;
                    break;

                case "Tất Cả Sản Phẩm":
                    QLSP = new QLSanPham();
                    SwitchPanel(tatCaSanPhamPanel);
                    layTCSP();
                    break;

                case "Hồ Sơ Shop":
                    SwitchPanel(hoSoShop_Panel);
                    hoSoShop();
                    break;
                case "Thiết Lập Shop":
                    DiaChi diachi = new DiaChi();
                    SwitchPanel(thietlapShop_panel);
                    thietlapShop(diachi);
                    break;
                case "Tất Cả Bài Đăng":
                    QLSP = new QLSanPham();
                    QLBD = new QLBaiDang();
                    SwitchPanel(tatCaBaiDang_Panel);
                    layTCBD();
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

        private void thietlapShop(DiaChi diachi)
        {
            txtHoSoShop.Text = shop.ten;
            sdtShop_Txt.Text = shop.soDT;
            emaiShop_Txt.Text = shop.email;
            diaChicuTheShop_txt.Text = shop.diaChi.diaChiCuThe + ", "  + BLL_DiaChi.Instance.MoTaDiaChi(shop.diaChi);
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

        private void ThemSanPham(SanPham sanPham)
        {
            miniConTrolPanel.Top = 732;
            QLSP.list.Add(sanPham);

            TTBH_Panel.Size = new Size(TTBH_Panel.Width, TTBH_Panel.Height + formThemSPPanel.Size.Height + 20);
            panel8.Location = new Point(panel8.Location.X, panel8.Location.Y + formThemSPPanel.Size.Height + 20);
            formThemSPPanel.Location = themSPButton.Location;
            themSPButton.Location = new Point(themSPButton.Location.X, themSPButton.Location.Y + formThemSPPanel.Size.Height + 20);

            listSP_FLPanel.Size = new Size(listSP_FLPanel.Size.Width, listSP_FLPanel.Size.Height + formThemSPPanel.Size.Height + 20);
            listSP_FLPanel.Controls.Add(this.sanPhamForm(sanPham));
            listSP_FLPanel.Visible = true;
        }

        private Panel sanPhamForm(SanPham sanPham)
        {
            Color color1 = Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            Font font1 = new Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Panel panel = new Panel
            {
                Location = new Point(0, 0),
                Size = formThemSPPanel.Size,
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
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            PictureBox picImage = new PictureBox
            {
                Location = new Point(SP_Pic.Location.X, 12),
                Size = new Size(100, 100),
                Font = font1,
                Name = "picImage",
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = color1,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(sanPham.anh), new Size(100, 100))
            };

            TextBox lbTenSP = new TextBox
            {
                Location = textBox13.Location,
                Text = "Tên sản phẩm",
                Size = textBox13.Size,
                Font = font1,
                BackColor = color1,
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
                Location = textBox24.Location,
                Text = "Ngôn ngữ",
                Size = textBox24.Size,
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
                Location = textBox31.Location,
                Text = "Số trang",
                Size = textBox31.Size,
                Font = font1,
                BackColor = color1,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                TabIndex = 0,
            };
            TextBox txtSP = new TextBox
            {
                Location = tenSP_Text.Location,
                Text = sanPham.ten,
                Size = tenSP_Text.Size,
                Name = "txtTenSP",
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 0,
                ReadOnly = true,
            };
            TextBox txtTG = new TextBox
            {
                Location = tenTacGia_Text.Location,
                Text = sanPham.tacGia,
                Name = "txtTacGia",
                Size = tenTacGia_Text.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 1,
                ReadOnly = true,
            };
            TextBox txtDG = new TextBox
            {
                Location = tenDichGia_Text.Location,
                Text = sanPham.dichGia,
                Name = "txtDichGia",
                Size = tenDichGia_Text.Size,
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 2,
                ReadOnly = true,
            };
            TextBox txtNXB = new TextBox
            {
                Location = nhaXuatBan_Text.Location,
                Text = sanPham.nhaXuatBan,
                Size = nhaXuatBan_Text.Size,
                Name = "txtNhaXuatBan",
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 3,
                ReadOnly = true,
            };
            TextBox txtNam = new TextBox
            {
                Text = sanPham.namXuatBan.ToString(),
                Location = namXuatBan_Text.Location,
                Size = namXuatBan_Text.Size,
                Name = "txtNamXuatBan",
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 4,
                ReadOnly = true,
            };
            TextBox txtPrice = new TextBox
            {
                Text = sanPham.gia.ToString(),
                Location = gia_Text.Location,
                Size = gia_Text.Size,
                Name = "txtGia",
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 5,
                ReadOnly = true,
            };
            TextBox txtSL = new TextBox
            {
                Text = sanPham.soLuong.ToString(),
                Location = soLuong_Text.Location,
                Size = soLuong_Text.Size,
                Name = "txtSoLuong",
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 6,
                ReadOnly = true,
            };
            TextBox txtTrang = new TextBox
            {
                Text = sanPham.soTrang.ToString(),
                Location = soTrang_Text.Location,
                Size = soTrang_Text.Size,
                Name = "txtSoTrang",
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 7,
                ReadOnly = true,
            };
            TextBox txtMota = new TextBox
            {
                Text = sanPham.moTa,
                Location = moTaSP_Text.Location,
                Size = moTaSP_Text.Size,
                Name = "txtMoTa1",
                Font = font1,
                BackColor = color1,
                BorderStyle = BorderStyle.None,
                TabIndex = 8,
                ReadOnly = true,
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
            PictureBox pic11 = new PictureBox
            {
                Location = pictureBox18.Location,
                Size = pictureBox18.Size,
                Font = font1,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = color1
            };
            PictureBox pic12 = new PictureBox
            {
                Location = pictureBox19.Location,
                Size = pictureBox19.Size,
                Font = font1,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = color1
            };
            PictureBox pic13 = new PictureBox
            {
                Location = pictureBox20.Location,
                Size = pictureBox20.Size,
                Font = font1,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = color1
            };

            TextBox TheLoaitxt = new TextBox
            {
                Location = txtTheLoai.Location,
                BorderStyle = BorderStyle.None,
                //Text = theLoai_CBBox.Items[int.Parse(sanPham.maLoaiSP)].ToString(),
                Text = sanPham.loaiSP.tenLoaiSP,
                Size = txtTheLoai.Size,
                Font = font1,
                Name = "txtTheLoai",
                BackColor = color1,
                ReadOnly = true
            };
            TextBox LoaiBiatxt = new TextBox
            {
                Location = txtLoaiBia.Location,
                BorderStyle = BorderStyle.None,
                Text = sanPham.loaiBia,
                Size = txtLoaiBia.Size,
                Font = font1,
                Name = "txtLoaiBia",
                BackColor = color1,
                ReadOnly = true
            };
            TextBox Languagetxt = new TextBox
            {
                Location = txtNgonNgu.Location,
                BorderStyle = BorderStyle.None,
                Size = txtNgonNgu.Size,
                Text = sanPham.ngonNgu,
                Font = font1,
                Name = "txtLanguage",
                BackColor = color1,
                ReadOnly = true
            };
            PictureBox picTheLoai = new PictureBox
            {
                Location = SoLuong_Pic.Location,
                Size = SoLuong_Pic.Size,
                BorderStyle = BorderStyle.FixedSingle,
                Font = font1,
                BackColor = color1
            };

            panel.Controls.Add(anhSP);
            panel.Controls.Add(picImage);
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
            panel.Controls.Add(picSP);
            panel.Controls.Add(picTG);
            panel.Controls.Add(pic3);
            panel.Controls.Add(pic4);
            panel.Controls.Add(pic5);
            panel.Controls.Add(pic6);
            panel.Controls.Add(pic7);
            panel.Controls.Add(pic8);
            panel.Controls.Add(pic9);
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
            panel.Controls.Add(TheLoaitxt);
            panel.Controls.Add(LoaiBiatxt);
            panel.Controls.Add(Languagetxt);
            panel.Controls.Add(pic11);
            panel.Controls.Add(pic12);
            panel.Controls.Add(pic13);

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

            panel.Controls.Add(btnXoa);
            panel.Controls.Add(btnCapNhat);

            btnCapNhat.Click += Update_Button;
            btnXoa.Click += Remove_Button;

            miniConTrolPanel.Top = 732;
            return panel;
        }

        private void themSPButton_Click(object sender, EventArgs e)//1575, 140
        {
            DimForm dimForm = new DimForm();
            dimForm.Show();
            SanPhamForm form = new SanPhamForm(ThemSanPham);
            form.ShowDialog();
            dimForm.Close();
            miniConTrolPanel.Top = 732;
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
        private void UpdateSanPham(SanPham sanPham)
        {
            QLSP.list[index] = sanPham;
            BLL_Shop.Instance.CapNhatSanPham(shop, sanPham);
        }

        private void Update_Button(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Panel panelToUpdate = clickedButton.Parent as Panel;
            index = listSP_FLPanel.Controls.IndexOf(panelToUpdate);

            DimForm dimForm = new DimForm();
            dimForm.Show();

            SanPhamForm form = new SanPhamForm(UpdateSanPham);
            SendSanPham send = new SendSanPham(form.setSanPham);
            send(QLSP.list[index]);

            form.ShowDialog();
            dimForm.Close();

            ((PictureBox)GUI_Utils.Instance.FindControl(panelToUpdate, "picImage")).Image = System.Drawing.Image.FromFile(QLSP.list[index].anh);
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtTenSP")).Text = QLSP.list[index].ten;
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtTacGia")).Text = QLSP.list[index].tacGia;
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtDichGia")).Text = QLSP.list[index].dichGia;
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtNhaXuatBan")).Text = QLSP.list[index].nhaXuatBan;
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtNamXuatBan")).Text = QLSP.list[index].namXuatBan.ToString();
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtMoTa1")).Text = QLSP.list[index].moTa;
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtGia")).Text = QLSP.list[index].gia.ToString();
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtSoLuong")).Text = QLSP.list[index].soLuong.ToString();
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtSoTrang")).Text = QLSP.list[index].soTrang.ToString();
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtLoaiBia")).Text = QLSP.list[index].loaiBia.ToString();
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtTheLoai")).Text = QLSP.list[index].loaiSP.tenLoaiSP.ToString();
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtLanguage")).Text = QLSP.list[index].ngonNgu.ToString();
        }

        private void luuBaiDangButton_Click(object sender, EventArgs e)
        {

            if (picCoverImage != null && tieuDe_txt.Text != "" && giamGia_txt.Text != "" && moTa_txt.Text != "")
            {
                BaiDang baiDang = new BaiDang
                {
                    maBD = BLL_BaiDang.Instance.GetMaMoi(),
                    maS = shop.maSo,
                    tieuDe = tieuDe_txt.Text,
                    moTa = moTa_txt.Text,
                    giamGia = int.Parse(giamGia_txt.Text),
                    luocThich = 0,
                    anhBia = Utils.Instance.GetImageURL(picCoverImage.Image)
                };

                foreach (var sanPham in QLSP.list)
                {
                    sanPham.anh = Utils.Instance.GetImageURL(System.Drawing.Image.FromFile(sanPham.anh));
                    baiDang.Add(sanPham);
                }

                QLSP.Clear();
                index = -1;
                shop.Insert(0, baiDang);
                shop.Add(baiDang);
                BLL_BaiDang.Instance.ThemBaiDang(baiDang);

                trangChuPanel.Visible = true;
                themSanPhamPanel.Visible = false;
            }
            else
            {
                MessageBox.Show("Thêm sản phẩm không thành công, vui lòng kiểm tra lại.");
            }
        }

        private void RefreshThemSPPanel()
        {
            picCoverImage.Image = null;
            btnCoverImage.Visible = true;
            tieuDe_txt.Text = "";
            giamGia_txt.Text = "";
            moTa_txt.Text = "";
            listSP_FLPanel.Controls.Clear();
            listSP_FLPanel.Size = new Size(listSP_FLPanel.Width, 0);
            TTBH_Panel.Size = new Size(TTBH_Panel.Width, 150);
            themSPButton.Location = listSP_FLPanel.Location;
            QLSP = null;

        }

        private void huyThemBaiDang_Click(object sender, EventArgs e)
        {
            tabClick(trangChuButton, null);
        }


        private void btnCoverImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "File anh|*.jpg.; *.gif; *.png; |All file| *.*";
            
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                picCoverImage.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(openFile.FileName), picCoverImage.Size);
                btnCoverImage.Visible = false;
                picCoverImage.MouseHover += new EventHandler(Edit_Image);
                picCoverImage.MouseLeave += new EventHandler(Leave_Image);
            }
        }

        private void Edit_Image(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            btnSuaCoverImage.Visible = true;
            btnXoaCoverImage.Visible = true;
            btnXoaCoverImage.Location = new Point(40 + pic.Location.X, pic.Location.Y + 55);
            btnSuaCoverImage.Location = new Point(5 + pic.Location.X, pic.Location.Y + 55);
        }

        private void Leave_Image(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            Point mousePosition = Cursor.Position;
            if ((mousePosition.X <= pic.Location.X) && (mousePosition.X >= 328) && (mousePosition.Y <= pic.Location.Y) && (mousePosition.Y >= 143))
            {
                btnSuaCoverImage.Visible = false;
                btnXoaCoverImage.Visible = false;
            }

        }

        private void btnSuaImage_Click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "File anh|*.jpg.; *.gif; *.png; |All file| *.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pic.Image = System.Drawing.Image.FromFile(openFile.FileName);
            }
        }

        private Panel themCTSP(SanPham sanPham)
        {
            Font font1 = new Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Panel p = new Panel
            {
                Location = new Point(0,0),
                Size = chiTietSP_Panel.Size,
                BackColor = Color.White
            };
            PictureBox picImage = new PictureBox
            {
                Location = picAnhTCSP.Location,
                Size = picAnhTCSP.Size,
                Font = font1,
                Name = "picImage",
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = System.Drawing.Image.FromFile(sanPham.anh)
            };

            TextBox txtTenSP = new TextBox
            {
                Location = txtTenTCSP.Location,
                Text = sanPham.ten,
                Size = txtTenTCSP.Size,
                Name = "txtTenSP",
                Font = font1,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                TabIndex = 0,
                Multiline = true,
                ReadOnly = true,
            };
         
            TextBox txtDonGia = new TextBox
            {
                Location = txtDonGiaTCSP.Location,
                Text = sanPham.gia.ToString(),
                Size = txtDonGiaTCSP.Size,
                Name = "txtDonGia",
                Font = font1,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                TabIndex = 0,
                ReadOnly = true,
            };
            TextBox txtSoLuong = new TextBox
            {
                Location = txtSoLuongTCSP.Location,
                Text = sanPham.soLuong.ToString(),
                Size = txtSoLuongTCSP.Size,
                Name = "txtSoLuong",
                Font = font1,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                TabIndex = 0,
                ReadOnly = true,
            };
            TextBox txtLuocBan = new TextBox
            {
                Location = txtLuocBanTCSP.Location,
                Text = sanPham.luocBan.ToString(),
                Size = txtLuocBanTCSP.Size,
                Name = "txtLuocBan",
                Font = font1,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                TabIndex = 0,
                ReadOnly = true,
            };
            Button btnTTSP = new Button
            {
                Location = btnThongTinSanPham.Location,
                Text = "Thông tin sản phẩm",
                ForeColor = Color.MistyRose,
                Size = btnThongTinSanPham.Size,
                Font = font1,
                BackColor = Color.OrangeRed,
            };
          
            p.Controls.Add(txtTenSP);
            p.Controls.Add(txtDonGia);
            p.Controls.Add(txtSoLuong);
            p.Controls.Add(txtLuocBan);
            p.Controls.Add(btnTTSP);

            p.Controls.Add(picImage);
            btnTTSP.Click += TTSP_Button;
            return p;
        }

        private void TTSP_Button(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Panel panelToUpdate = clickedButton.Parent as Panel;
            index = flowLayoutPanel.Controls.IndexOf(panelToUpdate);

            DimForm dimForm = new DimForm();
            dimForm.Show();

            SanPhamForm form = new SanPhamForm(this.UpdateSanPham);
            SendSanPham send = new SendSanPham(form.setSanPham);
            send(QLSP.list[index]);

            form.ShowDialog();
            dimForm.Close();
            ((PictureBox)GUI_Utils.Instance.FindControl(panelToUpdate, "picImage")).Image = System.Drawing.Image.FromFile(QLSP.list[index].anh);
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtTenSP")).Text = QLSP.list[index].ten;
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtDonGia")).Text = QLSP.list[index].gia.ToString();
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtSoLuong")).Text = QLSP.list[index].soLuong.ToString();
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtLuocBan")).Text = QLSP.list[index].luocBan.ToString();
        }
        private void ThemThongTinSanPham(SanPham sanPham)
        {
            QLSP.list.Add(sanPham);
            flowLayoutPanel.Size = new Size(flowLayoutPanel.Size.Width, flowLayoutPanel.Size.Height + chiTietSP_Panel.Size.Height + 20);
            flowLayoutPanel.Controls.Add(this.themCTSP(sanPham));
            flowLayoutPanel.Visible = true;
        }
        private void ThemThongTinBaiDang(BaiDang baiDang)
        {
            QLBD.list.Add(baiDang);
            FLLayout_TatCaBaiDang.Size = new Size(FLLayout_TatCaBaiDang.Size.Width, FLLayout_TatCaBaiDang.Size.Height + chiTietBaiDang_Panel.Size.Height + 20);
            FLLayout_TatCaBaiDang.Controls.Add(this.themCTBD(baiDang));
            FLLayout_TatCaBaiDang.Visible = true;
        }

        private Panel themCTBD(BaiDang baiDang)
        {
            Font font1 = new Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Panel p = new Panel
            {
                Location = new Point(0, 0),
                Size = chiTietSP_Panel.Size,
                BackColor = Color.White
            };
            PictureBox picImage = new PictureBox
            {
                Location = picAnh.Location,
                Size = picAnh.Size,
                Font = font1,
                Name = "picAnh",
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = System.Drawing.Image.FromFile(baiDang.anhBia)
            };

            TextBox txtTieuDe1 = new TextBox
            {
                Location = txtTieuDe.Location,
                Text = baiDang.tieuDe,
                Size = txtTieuDe.Size,
                Name = "txtTieuDe",
                Font = font1,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                TabIndex = 0,
                Multiline = true,
                ReadOnly = true,
            };

            TextBox txtGiamGia1 = new TextBox
            {
                Location = txtGiamGia.Location,
                Text = baiDang.giamGia.ToString(),
                Size = txtGiamGia.Size,
                Name = "txtGiamGia",
                Font = font1,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                TabIndex = 0,
                ReadOnly = true,
            };
            TextBox txtMoTa = new TextBox
            {
                Location = txtMoTa1.Location,
                Text = baiDang.moTa,
                Size = txtMoTa1.Size,
                Name = "txtMoTa",
                Font = font1,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                TabIndex = 0,
                ReadOnly = true,
            };
            TextBox txtLuocThich1 = new TextBox
            {
                Location = txtLuotThich.Location,
                Text = baiDang.luocThich.ToString(),
                Size = txtLuotThich.Size,
                Name = "txtLuocThich",
                Font = font1,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                TabIndex = 0,
                ReadOnly = true,
            };
            Button btnTTSP1 = new Button
            {
                Location = btnTTSP.Location,
                Text = "Thông tin chi tiết",
                ForeColor = Color.MistyRose,
                Size = btnThongTinSanPham.Size,
                Font = font1,
                BackColor = Color.OrangeRed,
            };

            Button btnXoa1 = new Button
            {
                Location = btnXoa.Location,
                Text = "Xóa",
                ForeColor = Color.Black,
                Size = btnXoa.Size,
                Font = font1,
                BackColor = btnXoa.BackColor,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            p.Controls.Add(txtTieuDe1);
            p.Controls.Add(txtGiamGia1);
            p.Controls.Add(txtMoTa);
            p.Controls.Add(txtLuocThich1);
            p.Controls.Add(btnTTSP1);
            p.Controls.Add(btnXoa1);
            p.Controls.Add(picImage);

            btnTTSP1.Click += TTBD_Button;
            btnXoa1.Click += Xoa_Button;
            return p;
        }

        private void Xoa_Button(object sender, EventArgs e)
        {
            
            Button clickedButton = sender as Button;
            Panel panelToRemove = clickedButton.Parent as Panel;
            int panelIndex = FLLayout_TatCaBaiDang.Controls.IndexOf(panelToRemove);
         
            if (panelIndex != -1 && panelIndex < FLLayout_TatCaBaiDang.Controls.Count)
            {
                FLLayout_TatCaBaiDang.Controls.RemoveAt(panelIndex);
                BLL_BaiDang.Instance.XoaBaiDang(QLBD.list[index]);        
            }
        }

        private void XoaBaiDang()
        {
            throw new NotImplementedException();
        }

        private void UpdateBaiDang(BaiDang baiDang)
        {
            QLBD.list[index] = baiDang;
            BLL_Shop.Instance.CapNhatBaiDang(shop, baiDang);
        }
        private void TTBD_Button(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Panel panelToUpdate = clickedButton.Parent as Panel;
            index = FLLayout_TatCaBaiDang.Controls.IndexOf(panelToUpdate);

            DimForm dimForm = new DimForm();
            dimForm.Show();

            BaiDangForm form = new BaiDangForm(this.UpdateBaiDang);
            SendBaiDang send = new SendBaiDang(form.SetBaiDang);
            send(QLBD.list[index]);

            form.ShowDialog();
            dimForm.Close();
            ((PictureBox)GUI_Utils.Instance.FindControl(panelToUpdate, "picAnh")).Image = System.Drawing.Image.FromFile(QLBD.list[index].anhBia);
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtTieuDe")).Text = QLBD.list[index].tieuDe;
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtGiamGia")).Text = QLBD.list[index].giamGia.ToString();
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtMoTa")).Text = QLBD.list[index].moTa;
            ((TextBox)GUI_Utils.Instance.FindControl(panelToUpdate, "txtLuocThich")).Text = QLBD.list[index].luocThich.ToString();
        }
        private void hoSoShop()
        {
            txtTenShop.Text = shop.ten;
            txtEmailShop.Text = shop.email;
            txtSdtShop.Text = shop.soDT;
            if (shop.avt != "")
                picLogo.Image = System.Drawing.Image.FromFile(shop.avt);
        }

        private void btnEditShop_Click(object sender, EventArgs e)
        {
            txtTenShop.ReadOnly = false;
            txtTenShop.BackColor = Color.White;
            picTenShop.BackColor = Color.White;
            txtEmailShop.ReadOnly = false;
            txtEmailShop.BackColor = Color.White;
            picEmailShop.BackColor = Color.White;
            txtSdtShop.ReadOnly = false;
            txtSdtShop.BackColor = Color.White;
            picSdtShop.BackColor = Color.White;
            txtMoTaShop.ReadOnly = false;
            txtMoTaShop.BackColor = Color.White;
            picMotaShop.BackColor = Color.White;
            if (shop.avt != "")
            {
                picLogo.Image = System.Drawing.Image.FromFile(shop.avt);
            }
            btnLuuShop.Visible = true;
            btnThoatShop.Visible = true;
            btnXemShop.Visible = false;
            btnEditShop.Visible = false;
            btnThemAnhShop.Visible = true;
        }

        private void btnLuuShop_Click(object sender, EventArgs e)
        {
            if(shop.avt != "")
                shop.avt = Utils.Instance.GetImageURL(System.Drawing.Image.FromFile(shop.avt));

            shop.Sua(txtTenShop.Text, txtEmailShop.Text, txtSdtShop.Text, 0, DateTime.Now);
            BLL_Shop.Instance.CapNhatThongTin(shop);

            txtTenShop.ReadOnly = true;
            txtTenShop.BackColor = Color.Gainsboro;
            picTenShop.BackColor = Color.Gainsboro;
            txtEmailShop.ReadOnly = true;
            txtEmailShop.BackColor = Color.Gainsboro;
            picEmailShop.BackColor = Color.Gainsboro;
            txtSdtShop.ReadOnly = true;
            txtSdtShop.BackColor = Color.Gainsboro;
            picSdtShop.BackColor = Color.Gainsboro;
            txtMoTaShop.ReadOnly = true;
            txtMoTaShop.BackColor = Color.Gainsboro;
            picMotaShop.BackColor = Color.Gainsboro;
            btnLuuShop.Visible = false;
            btnThoatShop.Visible = false;
            btnXemShop.Visible = true;
            btnEditShop.Visible = true;
            btnThemAnhShop.Visible = false;
        }
        private void btnThemAnhShop_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "File anh|*.jpg.; *.gif; *.png; |All file| *.*"
            };

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                picLogo.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(openFile.FileName), picLogo.Size);
                shop.avt = openFile.FileName;
            }

            picLogo.Visible = true;
        }

        private void btnThoatShop_Click(object sender, EventArgs e)
        {
            txtTenShop.ReadOnly = true;
            txtTenShop.BackColor = Color.Gainsboro;
            picTenShop.BackColor = Color.Gainsboro;
            txtEmailShop.ReadOnly = true;
            txtEmailShop.BackColor = Color.Gainsboro;
            picEmailShop.BackColor = Color.Gainsboro;
            txtSdtShop.ReadOnly = true;
            txtSdtShop.BackColor = Color.Gainsboro;
            picSdtShop.BackColor = Color.Gainsboro;
            txtMoTaShop.ReadOnly = true;
            txtMoTaShop.BackColor = Color.Gainsboro;
            picMotaShop.BackColor = Color.Gainsboro;
            btnLuuShop.Visible = false;
            btnThoatShop.Visible = false;
            btnXemShop.Visible = true;
            btnEditShop.Visible = true;
            btnThemAnhShop.Visible = false;
        }

        private void DrawPanelBorder(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;

            using (var pen = new Pen(Color.DarkGray, 1))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(-1, -1, panel.Width + 2, panel.Height));
            }
        }

        private void ChuyenLoaiDH_Click(object sender, EventArgs e)
        {
            foreach (Control control in HeaderDHPanel.Controls)
            {
                if (control.ForeColor == Color.Red)
                {
                    control.ForeColor = Color.Black;
                    break;
                }
            }
            Button button = sender as Button;
            button.ForeColor = Color.Red;

            if (button.Text == "Tất cả")
                loaiDH = -2;
            if (button.Text == "Chờ xác nhận")
                loaiDH = 0;
            else if (button.Text == "Đang giao")
                loaiDH = 1;
            else if (button.Text == "Hoàn thành")
                loaiDH = 2;
            else if (button.Text == "Đơn hủy")
                loaiDH = -1;

            int n = 0;
            int height = 10;
            DonHangFLP.Controls.Clear();
            foreach (DonHang donHang in shop.listDonHang.list)
            {
                if (loaiDH == -2 || loaiDH == donHang.tinhTrang)
                {
                    Panel panel = DrawDonHangPanel(donHang);
                    DonHangFLP.Controls.Add(panel);
                    height += panel.Height + panel.Margin.Bottom + panel.Margin.Top;
                    n++;
                }
            }
            soLuongDHTxt.Text = n.ToString() + " Đơn hàng";

            DonHangFLP.Height = height + 200;
            TatCaDHP.Height = height + 200;

        }

        private Panel DrawSPPanelInDHFLP(SanPham sanPham, FlowLayoutPanel parent)
        {
            Panel panel = new Panel
            {
                Size = panel23.Size,
                Margin = panel23.Margin,
                BackColor = Color.White,
                Parent = parent
            };
            panel.Paint += DrawPanelBorder;

            using (Bitmap bmp = new Bitmap(sanPham.anh))
            {
                PictureBox pictureBox = new PictureBox
                {
                    Location = pictureBox16.Location,
                    Size = pictureBox16.Size,
                    Image = GUI_Utils.Instance.Resize(bmp, pictureBox16.Size),
                    BorderStyle = BorderStyle.None,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.White,
                    Parent = panel
                };
                panel.Controls.Add(pictureBox);
            }

            TextBox ten = new TextBox
            {
                Text = sanPham.ten,
                Location = textBox54.Location,
                Size = textBox54.Size,
                Font = textBox54.Font,
                Multiline = true,
                ReadOnly = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(ten);

            TextBox soLuong = new TextBox
            {
                Text = sanPham.soLuong.ToString(),
                Location = textBox56.Location,
                Size = textBox56.Size,
                Font = textBox56.Font,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(soLuong);

            TextBox soLuongGoc = new TextBox
            {
                Text = BLL_Shop.Instance.GetSoLuongFromMaSP(shop.listBaiDang, sanPham.maSP).ToString(),
                Location = textBox35.Location,
                Size = textBox35.Size,
                Font = textBox35.Font,
                ReadOnly = true,
                BackColor = Color.White,
                TextAlign = HorizontalAlignment.Right,
                ForeColor = Color.DarkGray,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(soLuong);

            return panel;
        }

        private FlowLayoutPanel DrawDonHangPanel(DonHang donHang)
        {
            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                Size = flowLayoutPanel8.Size,
                BackColor = Color.White,
                Margin = flowLayoutPanel8.Margin,
                Parent = DonHangFLP
            };

            Panel headPanel = new Panel
            {
                Size = panel22.Size,
                BackColor = Color.WhiteSmoke,
                Margin = panel22.Margin,
                Parent = flp
            };

            TextBox maDH = new TextBox
            {
                Name = "maDH",
                Text = "Mã đơn hàng: DH" + donHang.maDH,
                Font = textBox32.Font,
                Size = textBox32.Size,
                BackColor = Color.WhiteSmoke,
                ReadOnly = true,
                Location = textBox32.Location,
                Cursor = Cursors.IBeam,
                BorderStyle = BorderStyle.None,
                Parent = headPanel
            };
            headPanel.Controls.Add(maDH);

            TextBox tinhTrang = new TextBox
            {
                Text = donHang.TinhTrang().ToUpper(),
                Font = textBox57.Font,
                Size = textBox57.Size,
                Location = textBox57.Location,
                TextAlign = HorizontalAlignment.Right,
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.DarkGray,
                ReadOnly = true,
                Cursor = Cursors.IBeam,
                BorderStyle = BorderStyle.None,
                Parent = headPanel
            };
            headPanel.Controls.Add(tinhTrang);
            flp.Controls.Add(headPanel);

            int height = headPanel.Height + headPanel.Margin.Top + headPanel.Margin.Bottom;
            foreach (SanPham sanPham in donHang.list)
            {
                Panel panel = DrawSPPanelInDHFLP(sanPham, flp);
                flp.Controls.Add(panel);
                height += panel.Height + panel.Margin.Top + panel.Margin.Bottom;
            }

            Panel tailPanel = new Panel
            {
                Size = panel26.Size,
                Margin = panel26.Margin,
                BackColor = Color.White,
                Parent = flp
            };

            Button button = new Button
            {
                Text = "Xem chi tiết đơn hàng",
                Size = button32.Size,
                Location = button32.Location,
                ForeColor = Color.White,
                BackColor = Color.DarkGray,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Parent = tailPanel
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.Silver;
            button.FlatAppearance.MouseOverBackColor = Color.Silver;
            tailPanel.Controls.Add(button);

            if(donHang.tinhTrang == 0)
            {
                Button button1 = new Button
                {
                    Text = "Giao hàng", //button24
                    Size = button33.Size,
                    Location = button33.Location,
                    ForeColor = Color.DarkGray,
                    BackColor = Color.White,
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    Parent = tailPanel
                };
                button1.Click += GiaoHangButton_Click;
                button1.FlatAppearance.BorderColor = Color.DarkGray;
                button1.FlatAppearance.BorderSize = 1;
                button1.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
                button1.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
                tailPanel.Controls.Add(button1);

                Button button2 = new Button
                {
                    Text = "Hủy đơn", //button24
                    Size = button24.Size,
                    Location = button24.Location,
                    ForeColor = Color.DarkGray,
                    BackColor = Color.White,
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    Parent = tailPanel
                };
                button2.FlatAppearance.BorderColor = Color.DarkGray;
                button2.FlatAppearance.BorderSize = 1;
                button2.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
                button2.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
                tailPanel.Controls.Add(button2);
            }
            //tailPanel.Paint += DrawPanelBorder;
            flp.Controls.Add (tailPanel);

            flp.Height = height + tailPanel.Height + tailPanel.Margin.Top + tailPanel.Margin.Bottom;
            return flp;
        }

        private void GiaoHangButton_Click(object sender, EventArgs e)
        {
            string maDH = GUI_Utils.Instance.GetMaDHByClick(sender);

            BLL_Shop.Instance.GiaoHang(shop, maDH);

            MessageBox.Show("Giao hàng thành công");

            if (loaiDH == -2)
                ChuyenLoaiDH_Click(tatCaButton, null);

        }

        private void DHMouseIn(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.ForeColor != Color.Red)
                button.ForeColor = Color.OrangeRed;
        }

        private void DHMouseOut(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button.ForeColor != Color.Red)
                button.ForeColor = Color.Black;

        }


        private void btnSuaShop_Click_1(object sender, EventArgs e)
        {
            DimForm dimForm = new DimForm();
            dimForm.Show();
            DiaChiForm form = new DiaChiForm(ThemDiaChi, diaChi);
            form.ShowDialog();
            dimForm.Close();

            if (diaChi != null)
                diaChicuTheShop_txt.Text = diaChi.ToString();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            pic_HoSo.BackColor = Color.White;
            txtHoSoShop.BackColor = Color.White;
            txtHoSoShop.ReadOnly = false;
            btnSuaTenShop.Visible = false;
            btnLuuTenShop.Visible = true;
        }

        private void btnLuuTenShop_Click(object sender, EventArgs e)
        {
            pic_HoSo.BackColor = Color.Gainsboro;
            txtHoSoShop.BackColor = Color.Gainsboro;
            txtHoSoShop.ReadOnly = true;
            btnLuuTenShop.Visible = false;
            btnSuaTenShop.Visible = true;
        }

        private void btnSuaSdtShop_Click(object sender, EventArgs e)
        {
            pic_SDT.BackColor = Color.White;
            sdtShop_Txt.BackColor = Color.White;
            sdtShop_Txt.ReadOnly = false;
            btnSuaSdtShop.Visible = false;
            btnLuusdtShop.Visible = true;
        }

        private void btnLuusdtShop_Click(object sender, EventArgs e)
        {
            pic_SDT.BackColor = Color.Gainsboro;
            sdtShop_Txt.BackColor = Color.Gainsboro;
            sdtShop_Txt.ReadOnly = true;
            btnLuusdtShop.Visible = false;
            btnSuaSdtShop.Visible = true;
        }

        private void btnSuaEmailShop_Click(object sender, EventArgs e)
        {
            pic_Email.BackColor = Color.White;
            emaiShop_Txt.BackColor = Color.White;
            emaiShop_Txt.ReadOnly = false;
            btnSuaEmailShop.Visible = false;
            btnLuuEmailShop.Visible = true;
        }

        private void btnLuuEmailShop_Click(object sender, EventArgs e)
        {
            pic_Email.BackColor = Color.Gainsboro;
            emaiShop_Txt.BackColor = Color.Gainsboro;
            emaiShop_Txt.ReadOnly = true;
            btnLuuEmailShop.Visible = false;
            btnSuaEmailShop.Visible = true;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            shop.ten = txtHoSoShop.Text;
            shop.email = emaiShop_Txt.Text;
            shop.soDT = sdtShop_Txt.Text;
            BLL_Shop.Instance.CapNhatThongTin(shop);
            MessageBox.Show("Lưu thông tin thành công !");
        }

        private void btnTTSP_Click(object sender, EventArgs e)
        {

        }
    }
}
