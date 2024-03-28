using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Program
{
    public delegate void sendData(string taiKhoan, string matKhau);
    public partial class KhachHangForm : Form
    {
        private User user = null;
        private KhachHang khachHang = null;

        public KhachHangForm(bool nonStart = true)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            if (!nonStart) 
                tuDongDangNhap();
        }

        public void setData(string taiKhoan, string matKhau)
        {
            user = new User();
            user.dangNhap(taiKhoan, matKhau);
            khachHang = HeThong.DangNhap(user);
            HeThong.WriteAccoutCache(user);

            KhachHang_Panel.Visible = true;
            HomePanel.Visible = true;
            HeaderPannel.Visible = true;
            dangNhap_Button.Visible = false;
            SignUp_Button.Visible = false;
            userProfile_Button.Visible = true;
            user_DangXuat_Button.Visible = true;
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
            diaChiCuThe_Box.Text = "...";
            diaChiCuThe_Box.Enabled = false;
            datDCMacDinh_check.Checked = false;
        }

        private void refreshCapNhatDiaChi_Panel(DiaChi diaChi)
        {
            hoVaTen_Box.Text = diaChi.ten;
            soDienThoai_Box.Text = diaChi.soDT;
            TTP_ComboBox.SelectedIndex = diaChi.maT_TP;
            QH_ComboBox.SelectedIndex = diaChi.maQH % 100;
            PX_ComboBox.SelectedIndex = diaChi.maPX % 100;
            diaChiCuThe_Box.Text = diaChi.diaChiCuThe;
            HTThemDiaChi_Button.Visible = false;
            HTCapNhatDC_Button.Visible = true;
        }

        private void hienMatKhau(CheckBox hienMK, TextBox box)
        {
            if (hienMK.Checked)
            {
                box.UseSystemPasswordChar = false;
            }
            else
            {
                box.UseSystemPasswordChar = true;
            }
        }

        private void tuDongDangNhap()
        {
            List<string> list = HeThong.ReadAccountCache();
            if (list != null)
            {
                user = new User();
                user.dangNhap(list[0], list[1]);

                KhachHang_Panel.Visible = true;
                HomePanel.Visible = true;
                HeaderPannel.Visible = true;
                dangNhap_Button.Visible = false;
                SignUp_Button.Visible = false;
                userProfile_Button.Visible = true;
                user_DangXuat_Button.Visible = true;

                khachHang = HeThong.DangNhap(user);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            refreshUserProfile_Panel();
            profilePanel.Visible = true;
            profilePanel.BringToFront();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            refreshUserProfile_Panel();
            doiMatKhauPanel.Visible = false;
            diaChiUser_Panel.Visible = false;
            profilePanel.Visible = true;
        }

        private void hienMK_UP_Check_CheckedChanged(object sender, EventArgs e)
        {
            this.hienMatKhau(hienMK_UP_Check, matKhauCu_Box);
            this.hienMatKhau(hienMK_UP_Check, matKhauMoi1_Box);
            this.hienMatKhau(hienMK_UP_Check, matKhauMoi2_Box);
        }

        private void doiMK_Botton_Click(object sender, EventArgs e)
        {
            doiMatKhauPanel.Visible = true;
            diaChiUser_Panel.Visible = false;
            profilePanel.Visible = false;
        }

        private void matKhauMoi2_Box_TextChanged(object sender, EventArgs e)
        {
            if (matKhauMoi1_Box.Text != matKhauMoi2_Box.Text && !matKhauKhongKhop_UP_Text.Visible)
                matKhauKhongKhop_UP_Text.Visible = true;
            else if (matKhauKhongKhop_UP_Text.Visible && matKhauMoi1_Box.Text == matKhauMoi2_Box.Text)
                matKhauKhongKhop_UP_Text.Visible = false;
        }

        private void xacNhan_UP_Button_Click(object sender, EventArgs e)
        {
            if (!HeThong.KiemTraMatKhau(user, matKhauCu_Box.Text))
                saiMKC_Text.Visible = true;
            else
            {
                user.doiMatKhau(matKhauMoi1_Box.Text);
                HeThong.CapNhatMatKhau(user);

                MessageBox.Show("Đổi mật khẩu thành công!!!");

                if (saiMKC_Text.Visible)
                    saiMKC_Text.Visible = false;

                matKhauCu_Box.Clear();
                matKhauMoi1_Box.Clear();
                matKhauMoi2_Box.Clear();
            }  
        }

        private void home_Button_Click(object sender, EventArgs e)
        {
            UserPanel.Visible = false;
            HomePanel.Visible = true;
        }

        private void userProfile_Button_Click(object sender, EventArgs e)
        {
            refreshUserProfile_Panel();
            HomePanel.Visible = false;
            UserPanel.Visible = true;
            profilePanel.Visible = true;
        }


        private void suaTen_Button_Click(object sender, EventArgs e)
        {
            if (ten_UP_Box.ReadOnly)
            {
                suaTen_Button.Text = "Lưu";
                ten_UP_Box.ReadOnly = false;
                ten_UP_Box.Cursor = Cursors.IBeam;
            }
            else
            {
                suaTen_Button.Text = "Sửa";
                ten_UP_Box.ReadOnly = true;
                ten_UP_Box.Cursor = Cursors.Default;
            }
        }

        private void suaEmail_button_Click(object sender, EventArgs e)
        {
            if (email_UP_Box.ReadOnly)
            {
                suaEmail_button.Text = "Lưu";
                email_UP_Box.ReadOnly = false;
                email_UP_Box.Cursor = Cursors.IBeam;
            }
            else
            {
                suaEmail_button.Text = "Sửa";
                email_UP_Box.ReadOnly = true;
                email_UP_Box.Cursor = Cursors.Default;
            }
        }

        private void suaSDT_button_Click(object sender, EventArgs e)
        {
            if (soDT_UP_Box.ReadOnly)
            {
                suaSDT_button.Text = "Lưu";
                soDT_UP_Box.ReadOnly = false;
                soDT_UP_Box.Cursor = Cursors.IBeam;
            }
            else
            {
                suaSDT_button.Text = "Sửa";
                soDT_UP_Box.ReadOnly = true;
                soDT_UP_Box.Cursor = Cursors.Default;
            }
        }

        private void refreshUserProfile_Panel()
        {
            suaTen_Button.Text = "Sửa";
            suaEmail_button.Text = "Sửa";
            suaSDT_button.Text = "Sửa";
            ten_UP_Box.ReadOnly = true;
            email_UP_Box.ReadOnly = true;
            soDT_UP_Box.ReadOnly = true;
            loiNgaySinh_Text.Visible = false;

            taiKhoan_UP_Text.Text = khachHang.taiKhoan;
            ten_UP_Box.Text = khachHang.ten;
            email_UP_Box.Text = khachHang.email;
            soDT_UP_Box.Text = khachHang.soDT;

            if (khachHang.gioiTinh == 0)
                nam_RB.Checked = true;
            else if (khachHang.gioiTinh == 1)
                nu_RB.Checked = true;
            else if (khachHang.gioiTinh == 2)
                khac_RB.Checked = true;

            if (khachHang.ngaySinh.Year >= 1899)
            {
                ngaySinh_CB.SelectedIndex = khachHang.ngaySinh.Day - 1;
                thangSinh_CB.SelectedIndex = khachHang.ngaySinh.Month - 1;
                namSinh_CB.SelectedIndex = DateTime.Now.Year - khachHang.ngaySinh.Year;
            }
        }
        private void luu_UP_Button_Click(object sender, EventArgs e)
        {
            DateTime ngaySinh;
            try
            {
                ngaySinh = new DateTime(int.Parse(namSinh_CB.SelectedItem.ToString()), thangSinh_CB.SelectedIndex + 1, ngaySinh_CB.SelectedIndex + 1);
                loiNgaySinh_Text.Visible = false;
            }
            catch
            {
                loiNgaySinh_Text.Visible = true;
                return;
            }
            int gioiTinh = 2;
            if (nam_RB.Checked)
                gioiTinh = 0;
            else if (nu_RB.Checked)
                gioiTinh = 1;
            else if (khac_RB.Checked)
                gioiTinh = 2;

            khachHang.sua(ten_UP_Box.Text, email_UP_Box.Text, soDT_UP_Box.Text, gioiTinh, ngaySinh);
            HeThong.CapNhatThongTinCaNhan(khachHang);
            MessageBox.Show("Đã lưu thành công!!!");
            this.refreshUserProfile_Panel();
        }

        private void themDiaChi_Button_Click(object sender, EventArgs e)
        {
            txtDiaChi.Text = "Địa chỉ mới";
            themDiaChi_Panel.Visible = true;
            themDiaChi_Panel.BringToFront();
            if (khachHang.diaChi == null)
            {
                datDCMacDinh_check.Checked = true;
                datDCMacDinh_check.Enabled = false;
            }
            if (TTP_ComboBox.Items.Count == 0)
                TTP_ComboBox.DataSource = HeThong.LoadTinh_ThanhPho();

        }

        private void diaChiUser_Button_Click(object sender, EventArgs e)
        {
            profilePanel.Visible = false;
            doiMatKhauPanel.Visible = false;
            diaChiUser_Panel.Visible = true;

            veLai_DiaChi();


        }

        private void backDiaChi_Button_Click(object sender, EventArgs e)
        {
            themDiaChi_Panel.Visible = false;
            refreshThemDiaChi_Panel();
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
                HTThemDiaChi_Button.Enabled = false;
            }
            else
            {
                diaChiCuThe_Box.Enabled = true;
                HTThemDiaChi_Button.Enabled = true;
                HTCapNhatDC_Button.Enabled = true;
            }
        }

        private void HTThemDiaChi_Button_Click(object sender, EventArgs e)
        {
            string maDC = HeThong.MaMoi("maDC");
            int maT_TP = TTP_ComboBox.SelectedIndex;
            int maQH = maT_TP * 100 + QH_ComboBox.SelectedIndex;
            int maPX = maQH * 100 + PX_ComboBox.SelectedIndex;
                DiaChi diaChi = new DiaChi(maDC, hoVaTen_Box.Text, soDienThoai_Box.Text, maT_TP, maQH, maPX, diaChiCuThe_Box.Text);
                HeThong.ThemDiaChi(khachHang, diaChi);

            if (datDCMacDinh_check.Checked)
            {
                if (khachHang.diaChi != null)
                {
                    khachHang.themDiaChi(khachHang.diaChi);
                }

                khachHang.capNhatDiaChi(diaChi);
                HeThong.CapNhatDiaChiMacDinh(khachHang);
            }
            else
            {
                khachHang.themDiaChi(diaChi);
            }

            themDiaChi_Panel.Visible = false;
            refreshThemDiaChi_Panel();
            veLai_DiaChi();
        }

        private void dangNhap_Button_Click(object sender, EventArgs e)
        {
            DangNhap_Form DNForm = new DangNhap_Form();
            DNForm.Show();
            this.Hide();
        }

        private void SignUp_Button_Click(object sender, EventArgs e)
        {
            DangNhap_Form DNForm = new DangNhap_Form(false);
            DNForm.Show();
            this.Hide();
        }

        private void troVe_button_Click(object sender, EventArgs e)
        {
            //LoginPanel.Visible = false;
            KhachHang_Panel.Visible = true;
        }
        private void veLai_DiaChi()
        {
            if (khachHang.diaChi == null) return;
            listDiaChi_FLPanel.Controls.Clear();
            veDiaChiMacDinh();

            foreach (DiaChi diachi in khachHang.diaChis)
            {
                veDiaChi(diachi);
            }

        }

        private void veDiaChiMacDinh()
        {
            TextBox txt = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Text = khachHang.diaChi.ToString(),
                Size = new Size(606, 60),
                Location = new System.Drawing.Point(11, 27),
                BorderStyle = BorderStyle.None,
                BackColor = Color.Snow
            };

            Button btn1 = new Button
            {
                Text = "Cập nhật",
                Size = new Size(120, 28),
                Location = new System.Drawing.Point(779, 10),
                BackColor = Color.Snow
            };
            btn1.Click += CapNhat_Button;

            Label lb = new Label
            {
                Text = "Mặc Định",
                Size = new Size(100, 25),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new System.Drawing.Point(11, 95),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                ForeColor = Color.Red
            };

            Panel panel = new Panel
            {
                Size = new Size(918, 130),
                BackColor = Color.Snow,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(txt);
            panel.Controls.Add(btn1);
            panel.Controls.Add(lb);
            listDiaChi_FLPanel.Controls.Add(panel);

        }

        private void veDiaChi(DiaChi diaChi)
        {
            TextBox txt = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Text = diaChi.ToString(),
                Size = new Size(606, 83),
                Location = new System.Drawing.Point(11, 27),
                BorderStyle = BorderStyle.None,
                BackColor = Color.Snow
            };

            Button btn1 = new Button
            {
                Text = "Cập nhật",
                Size = new Size(120, 28),
                Location = new System.Drawing.Point(779, 35),
                BackColor = Color.Snow
            };
            btn1.Click += CapNhat_Button;


            Button btn2 = new Button
            {
                Text = "Thiết lập mặc định",
                Size = new Size(239, 40),
                Location = new System.Drawing.Point(660, 70),
                BackColor = Color.Snow
            };
            btn2.Click += MacDinh_Button;

            Button btn3 = new Button
            {
                Text = "Xóa",
                Size = new Size(120, 28),
                Location = new System.Drawing.Point(660, 35),
                BackColor = Color.Snow
            };
            btn3.Click += xoa_Button;

            Panel panel = new Panel
            {
                Size = new Size(918, 130),
                BackColor = Color.Snow,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(txt);
            panel.Controls.Add(btn1);
            panel.Controls.Add(btn2);
            panel.Controls.Add(btn3);

            listDiaChi_FLPanel.Controls.Add(panel);

        }

        private void MacDinh_Button(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Panel panelToRemove = clickedButton.Parent as Panel;
            int panelIndex = listDiaChi_FLPanel.Controls.IndexOf(panelToRemove);

            khachHang.thayDoiDiaChiMacDinh(khachHang.diaChis[panelIndex - 1]);
            HeThong.CapNhatDiaChiMacDinh(khachHang);


            veLai_DiaChi();
        }

        private void xoa_Button(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Panel panelToRemove = clickedButton.Parent as Panel;
            int panelIndex = listDiaChi_FLPanel.Controls.IndexOf(panelToRemove);

            if (panelIndex != -1 && panelIndex < listDiaChi_FLPanel.Controls.Count)
            {
                listDiaChi_FLPanel.Controls.RemoveAt(panelIndex);
                HeThong.XoaDiaChi(khachHang.diaChis[panelIndex - 1]);
                khachHang.xoaDiaChi(panelIndex - 1);
            }
            veLai_DiaChi();
        }

        private void CapNhat_Button(object sender, EventArgs e)
        {
            themDiaChi_Button_Click(sender, e);
            txtDiaChi.Text = "Cập nhật địa chỉ";

            Button clickedButton = sender as Button;
            Panel panelToRemove = clickedButton.Parent as Panel;
            int panelIndex = listDiaChi_FLPanel.Controls.IndexOf(panelToRemove);

            indexOfDiaChi.Text = panelIndex.ToString();
            datDCMacDinh_check.Visible = false;

            if (panelIndex == 0)
                refreshCapNhatDiaChi_Panel(khachHang.diaChi);
            else
                refreshCapNhatDiaChi_Panel(khachHang.diaChis[panelIndex - 1]);
        }

        private void HTCapNhatDC_Button_Click(object sender, EventArgs e)
        {
            int index = int.Parse(indexOfDiaChi.Text);
            int maT_TP = TTP_ComboBox.SelectedIndex;
            int maQH = maT_TP * 100 + QH_ComboBox.SelectedIndex;
            int maPX = maQH * 100 + PX_ComboBox.SelectedIndex;
            DiaChi diaChi;

            if (index == 0)
            {
                diaChi = khachHang.diaChi;
                diaChi.capNhat(hoVaTen_Box.Text, soDienThoai_Box.Text, maT_TP, maQH, maPX, diaChiCuThe_Box.Text);
                khachHang.capNhatDiaChi(diaChi);
            }
            else
            {
                diaChi = khachHang.diaChis[index - 1];
                diaChi.capNhat(hoVaTen_Box.Text, soDienThoai_Box.Text, maT_TP, maQH, maPX, diaChiCuThe_Box.Text);
                khachHang.capNhatDiaChi(index - 1, diaChi);
            }
            HeThong.CapNhatDiaChi(diaChi);
            refreshThemDiaChi_Panel();
            themDiaChi_Panel.Visible = false;
            veLai_DiaChi();
        }

        private void user_DangXuat_Button_Click(object sender, EventArgs e)
        {
            khachHang = null;
            user = null;
            dangNhap_Button.Visible = true;
            SignUp_Button.Visible = true;
            user_DangXuat_Button.Visible = false;
            HomePanel.Visible = true;
            UserPanel.Visible = false;
            userProfile_Button.Visible = false;
            HeThong.ClearAccountCache();
        }

        private void soDienThoai_Box_TextChanged(object sender, EventArgs e)
        {
            if (soDienThoai_Box.Text == "...")
                return;

            if (!Utils.KiemTraSoDT(soDienThoai_Box.Text))
            {
                soDTKhongHopLe_Label.Visible = true;
                HTCapNhatDC_Button.Enabled = false;
                HTThemDiaChi_Button.Enabled = false;
            }
            else
            {
                soDTKhongHopLe_Label.Visible = false;
                if(diaChiCuThe_Box.Enabled)
                {
                    HTCapNhatDC_Button.Enabled = true;
                }
            }
        }

        private void soDT_UP_Box_TextChanged(object sender, EventArgs e)
        {
            if (soDT_UP_Box.Text == "")
                return;

            if (!Utils.KiemTraSoDT(soDT_UP_Box.Text))
            {
                SDTKhongHopLe_Label.Visible = true;
                luu_UP_Button.Enabled = false;
                if(suaSDT_button.Text == "Lưu")
                    suaSDT_button.Enabled = false;
            }
            else
            {
                SDTKhongHopLe_Label.Visible = false;
                luu_UP_Button.Enabled = true;
                if (suaSDT_button.Text == "Lưu")
                    suaSDT_button.Enabled = true;
            }
        }

        private void KenhNguoiBan_button_Click(object sender, EventArgs e)
        {
            if (user == null)
                return;

            Hide();
            BanHang_Form BHForm = new BanHang_Form(khachHang.daTaoShop());
            sendData send = new sendData(BHForm.setData);
            send(user.taiKhoan, user.matKhau);
            BHForm.ShowDialog();
        }

    }
}
