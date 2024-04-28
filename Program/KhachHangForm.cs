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
    public delegate void sendDiaChi(DiaChi diaChi);
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
            HeaderPannel.Visible = true;
            dangNhap_Button.Visible = false;
            SignUp_Button.Visible = false;
            userProfile_Button.Visible = true;
            user_DangXuat_Button.Visible = true;
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
            if (!user.kiemTraMatKhau(matKhauCu_Box.Text))
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
                pictureBox6.BackColor = Color.White;
                ten_UP_Box.BackColor = Color.White;
                suaTen_Button.Text = "Lưu";
                ten_UP_Box.ReadOnly = false;
                ten_UP_Box.Cursor = Cursors.IBeam;
            }
            else
            {
                if (ten_UP_Box.Text != "")
                {
                    pictureBox6.BackColor = Color.Gainsboro;
                    ten_UP_Box.BackColor = Color.Gainsboro;
                }
                else
                {
                    pictureBox6.BackColor = Color.White;
                    ten_UP_Box.BackColor = Color.White;
                }
                suaTen_Button.Text = "Sửa";
                ten_UP_Box.ReadOnly = true;
                ten_UP_Box.Cursor = Cursors.Default;
            }
        }

        private void suaEmail_button_Click(object sender, EventArgs e)
        {


            if (email_UP_Box.ReadOnly)
            {
                pictureBox7.BackColor = Color.White;
                email_UP_Box.BackColor = Color.White;
                suaEmail_button.Text = "Lưu";
                email_UP_Box.ReadOnly = false;
                email_UP_Box.Cursor = Cursors.IBeam;
            }
            else
            {
                if (email_UP_Box.Text != "")
                {
                    pictureBox7.BackColor = Color.Gainsboro;
                    email_UP_Box.BackColor = Color.Gainsboro;
                }
                else
                {
                    pictureBox7.BackColor = Color.White;
                    email_UP_Box.BackColor = Color.White;
                }
                suaEmail_button.Text = "Sửa";
                email_UP_Box.ReadOnly = true;
                email_UP_Box.Cursor = Cursors.Default;
            }
        }

        private void suaSDT_button_Click(object sender, EventArgs e)
        {


            if (soDT_UP_Box.ReadOnly)
            {
                pictureBox8.BackColor = Color.White;
                soDT_UP_Box.BackColor = Color.White;
                suaSDT_button.Text = "Lưu";
                soDT_UP_Box.ReadOnly = false;
                soDT_UP_Box.Cursor = Cursors.IBeam;
                
            }
            else
            {
                if (soDT_UP_Box.Text != "")
                {
                    pictureBox8.BackColor = Color.Gainsboro;
                    soDT_UP_Box.BackColor = Color.Gainsboro;
                }
                else
                {
                    pictureBox8.BackColor = Color.White;
                    soDT_UP_Box.BackColor = Color.White;
                }
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
            if (ten_UP_Box.Text == "")
            {
                pictureBox6.BackColor = Color.White;
                ten_UP_Box.BackColor = Color.White;
            }
            if (email_UP_Box.Text == "")
            {
                pictureBox7.BackColor = Color.White;
                email_UP_Box.BackColor = Color.White;
            }
            if (email_UP_Box.Text == "")

            {
                pictureBox7.BackColor = Color.White;
                email_UP_Box.BackColor = Color.White;
            }
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

        public void themDiaChi(DiaChi diaChi)
        {
            if(khachHang.diaChi == null)
            {
                khachHang.capNhatDiaChi(diaChi);
                HeThong.CapNhatDiaChiMacDinh(khachHang);
            }
            else
            {
                khachHang.themDiaChi(diaChi);
            }

            HeThong.ThemDiaChi(khachHang.maSo, diaChi);
            veLai_DiaChi();
        }

        public void capNhatDiaChi(DiaChi diaChi)
        {
            if(diaChi.maDC == khachHang.diaChi.maDC)
            {
                khachHang.capNhatDiaChi(diaChi);
            }
            else
            {
                for(int i = 0; i < khachHang.listDiaChi.Count; i++)
                {
                    if (khachHang.listDiaChi[i].maDC == diaChi.maDC)
                    {
                        khachHang.capNhatDiaChi(i, diaChi);
                        break;
                    }
                }
            }
            HeThong.CapNhatDiaChi(diaChi);
            veLai_DiaChi();
        }

        private void themDiaChi_Button_Click(object sender, EventArgs e)
        {
            DimForm dimForm = new DimForm();
            dimForm.Show();
            DiaChiForm form = new DiaChiForm(themDiaChi);
            form.ShowDialog();
            dimForm.Close();
        }

        private void diaChiUser_Button_Click(object sender, EventArgs e)
        {
            profilePanel.Visible = false;
            doiMatKhauPanel.Visible = false;
            diaChiUser_Panel.Visible = true;

            veLai_DiaChi();
        }

        private void dangNhap_Button_Click(object sender, EventArgs e)
        {
            DangNhap_Form form = new DangNhap_Form(setData);
            Hide();
            form.ShowDialog();
            Show();
        }

        private void SignUp_Button_Click(object sender, EventArgs e)
        {
            DangNhap_Form form = new DangNhap_Form(setData, false);
            Hide();
            form.ShowDialog();
            Show();
        }
        
        private void veLai_DiaChi()
        {
            if (khachHang.diaChi == null) return;
            listDiaChi_FLPanel.Controls.Clear();
            veDiaChiMacDinh();

            foreach (DiaChi diachi in khachHang.listDiaChi)
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

            khachHang.thayDoiDiaChiMacDinh(khachHang.listDiaChi[panelIndex - 1]);
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
                HeThong.XoaDiaChi(khachHang.listDiaChi[panelIndex - 1].maDC);
                khachHang.xoaDiaChi(panelIndex - 1);
            }
            veLai_DiaChi();
        }

        private void CapNhat_Button(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Panel panelToRemove = clickedButton.Parent as Panel;
            int panelIndex = listDiaChi_FLPanel.Controls.IndexOf(panelToRemove);

            DiaChi diaChi;

            if (panelIndex == 0)
                diaChi = khachHang.diaChi;
            else
                diaChi = khachHang.listDiaChi[panelIndex - 1];

            DimForm dimForm = new DimForm();
            dimForm.Show();
            DiaChiForm form = new DiaChiForm(capNhatDiaChi, diaChi);
            form.ShowDialog();
            dimForm.Close();
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
            Close();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string key = search_Text.Text;
            QLBaiDang list = HeThong.SearchBaiDang(key);

            foreach(BaiDang baiDang in list.list)
            {
                MessageBox.Show($"{baiDang.tieuDe}");
            }
        }
    }
}
