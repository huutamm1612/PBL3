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
            
            if(daTaoShop)
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
            if(currTab != null && currTab != trangChuButton)
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
    }
}
