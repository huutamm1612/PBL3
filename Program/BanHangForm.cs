using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            shop = HeThong.LoadShop(user);

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
            shop = new Shop(HeThong.MaMoi("maS"), tenShop_DK_Text.Text, soDT_DK_Text.Text, email_DK_Text.Text, shop.diaChi, -1, DateTime.Now, new List<BaiDang>(), 0, 1, 0);
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
            TTBH_Panel.Size = new Size(TTBH_Panel.Width, TTBH_Panel.Height + formThemSPPanel.Size.Height + 20);
            panel8.Location = new Point(panel8.Location.X, panel8.Location.Y + formThemSPPanel.Size.Height + 20);
            formThemSPPanel.Location = themSPButton.Location;
            themSPButton.Location = new Point(themSPButton.Location.X, themSPButton.Location.Y + formThemSPPanel.Size.Height + 20);
            formThemSPPanel.Visible = true;
        }

        private void huyThemSPButton_Click(object sender, EventArgs e)
        {
            themSPButton.Location = formThemSPPanel.Location;
            TTBH_Panel.Size = new Size(TTBH_Panel.Width, TTBH_Panel.Height - (formThemSPPanel.Size.Height + 20));
            panel8.Location = new Point(panel8.Location.X, panel8.Location.Y - (formThemSPPanel.Size.Height + 20));
            formThemSPPanel.Visible = false;
        }

        private void luuSPButton_Click(object sender, EventArgs e)
        {
            // kiem tra hop le
            if (tenSP_Check.Visible == true || dichGia_check.Visible == true || tacGia_Check.Visible == true || NXB_Check.Visible == true || nam_Check.Visible == true || soTrang_Check.Visible == true
                || theLoai_check.Visible == true || bia_Check.Visible == true  || language_Check.Visible == true || Price_Check.Visible == true || soLuong_check.Visible == true )
            {
                luuSPButton.Enabled = false;
            }
            else
            {
                luuSPButton.Enabled = true;
            }

            // dua du lieu vao QLSP     

            // copy roi them vao FlowLayoutPanel listSP_FLPanel

            // chinh sua lai kich thuoc

            // lam moi va an panel
        }

        private void refreshThemSPForm_Button_Click(object sender, EventArgs e)
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
            tenSP_Check.Visible = false;
            dichGia_check.Visible = false;
            tacGia_Check.Visible = false;
            NXB_Check.Visible = false;
            soTrang_Check.Visible = false;
            theLoai_check.Visible = false;
            bia_Check.Visible = false;
            language_Check.Visible = false;
            Price_Check.Visible = false;
            soLuong_check.Visible = false;
            nam_Check.Visible = false;  


        } 

        private void tenSP_Text_TextChanged(object sender, EventArgs e)
        {
            Count(tenSP_Text, Count_SP, 50);
            if (tenSP_Text.Text == "")
                tenSP_Check.Visible = true;
        }

        private void tenTacGia_Text_TextChanged(object sender, EventArgs e)
        {
            Count(tenTacGia_Text, Count_TG, 50);
            if (tenTacGia_Text.Text == "")
                tacGia_Check.Visible = true;
        }

        private void tenDichGia_Text_TextChanged(object sender, EventArgs e)
        {
            Count(tenDichGia_Text, Count_Dichgia, 50);
            if (tenDichGia_Text.Text == "")
                dichGia_check.Visible = true;
        }

        private void nhaXuatBan_Text_TextChanged(object sender, EventArgs e)
        {
            Count(nhaXuatBan_Text, Count_NXB, 50);
            if (nhaXuatBan_Text.Text == "")
                NXB_Check.Visible = true;
        }

        private void moTaSP_Text_TextChanged(object sender, EventArgs e)
        {
            Count(moTaSP_Text, Count_MoTa, 1000);
        }

        private void moTaTT_txt_TextChanged(object sender, EventArgs e)
        {
            Count(moTaTT_txt, Count_MoTaTT, 2000);
        }
        private void tieuDe_txt_TextChanged(object sender, EventArgs e)
        {
            Count(tieuDe_txt, Count_Tieude, 120);
        }
        void Count(TextBox txt, TextBox dem, int max)
        {
            if (txt.Text.Length > max)
            {
                txt.Text = txt.Text.Substring(0, max);
                txt.SelectionStart = max;
                txt.SelectionLength = 0;
            }
            dem.Text = txt.Text.Length + "/" + max;
        }

        private void namXuatBan_Text_TextChanged(object sender, EventArgs e)
        {
            if (namXuatBan_Text.Text == "")
                nam_Check.Visible = true;
        }

        private void soLuong_Text_TextChanged(object sender, EventArgs e)
        {
            if (soLuong_Text.Text == "")
                soLuong_check.Visible = true;
        }

        private void gia_Text_TextChanged(object sender, EventArgs e)
        {
            if (gia_Text.Text == "")
                Price_Check.Visible = true;
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

        private void soTrang_Text_TextChanged(object sender, EventArgs e)
        {
            if (soTrang_Text.Text == "")
                soTrang_Check.Visible = true;
        }
    }
}
