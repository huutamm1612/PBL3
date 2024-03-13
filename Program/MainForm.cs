using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Program
{
    public partial class MainForm : Form
    {
        private User user = null;
        private KhachHang khachHang = null;
        public MainForm()
        { 
            InitializeComponent();
        }
        private void refreshDangNhap_Panel()
        {
            taiKhoan_DN_Box.Text = "Tài Khoản";
            matKhau_DN_Box.Text = "Mật khẩu";
            matKhau_DN_Box.UseSystemPasswordChar = false;
            hienMK_DN_Check.Checked = false;
        }

        private void refreshDangKy_Panel()
        {
            if (cauHoi_CB.Items.Count == 0)
                this.setCauHoi(cauHoi_CB);
            taiKhoan_DK_Box.Text = "Tài khoản";
            cauTraLoi_Box.Text = "Câu trả lời";
            matKhau1_DK_Box.Text = "Mật khẩu";
            matKhau2_DK_Box.Text = "Nhập lại mật khẩu";
            matKhau1_DK_Box.UseSystemPasswordChar = false;
            matKhau2_DK_Box.UseSystemPasswordChar = false;
            taiKhoanSai_DK_Text.Visible = false;
            matKhauKhongKhop_DK_Text.Visible = false;
            hienMK_DK_Check.Checked = false;
            cauHoi_CB.SelectedIndex = 0;
        }

        private void refreshQuenMatKhau_Panel()
        {
            taiKhoan_QMK_Box.Text = "Tài khoản";
            cauTraLoi_QML_Box.Text = "Câu trả lời";
            matKhau1_QMK_Box.Text = "Mật khẩu";
            matKhau2_QMK_Box.Text = "Nhập lại mật khẩu";
            matKhau1_QMK_Box.UseSystemPasswordChar = false;
            matKhau2_QMK_Box.UseSystemPasswordChar = false;
            cauHoiQMK_CB.SelectedIndex = 0;
            saiMK_Text.Visible = false;
            thongBao_Text.Visible = false;
            hienMK_QMK_Check.Checked = false;
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

        private void hienMKCheck_CheckedChanged(object sender, EventArgs e)
        {
            this.hienMatKhau(hienMK_DN_Check, matKhau_DN_Box);
        }

        private void setCauHoi(ComboBox comboBox)
        {
            List<string> listCauHoi = HeThong.LoadCauHoi();
            comboBox.DataSource = listCauHoi;
        }

        private void dangKyBox_Click(object sender, EventArgs e)
        {
            
            this.refreshDangKy_Panel();
            LoginPanel.Visible = false;
            Signup_Panel.Visible = true;

        }

        private void hienMK_DK_Check_CheckedChanged(object sender, EventArgs e)
        {
            this.hienMatKhau(hienMK_DK_Check, matKhau1_DK_Box);
            this.hienMatKhau(hienMK_DK_Check, matKhau2_DK_Box);
        }

        private void troVe_DK_Button_Click(object sender, EventArgs e)
        {
            
            Signup_Panel.Visible = false;
            KhachHang_Panel.Visible = true;
        }

        private void matKhau_DN_Box_TextChanged(object sender, EventArgs e)
        {
            if (matKhau_DN_Box.Text.Length <= 1 && !hienMK_DN_Check.Checked)
                matKhau_DN_Box.UseSystemPasswordChar = true;
        }

        private void dangNhap_DN_Button_Click(object sender, EventArgs e)
        {
            string taiKhoan = taiKhoan_DN_Box.Text;
            string matKhau = matKhau_DN_Box.Text;
            if (HeThong.DangNhap(taiKhoan, matKhau))
            {
                LoginError.Visible = false;
                user = new User();
                user.dangNhap(taiKhoan, matKhau);
                LoginPanel.Visible = false;
                KhachHang_Panel.Visible = true;
                HomePanel.Visible = true;
                HeaderPannel.Visible = true;
                dangNhap_Button.Visible = false;
                SignUp_Button.Visible = false;
                userProfile_Button.Visible = true;
                khachHang = HeThong.DangNhap(user);
            }
            else
            {
                LoginError.Visible = true;
            }
        }

        private void dangKy_Botton_Click(object sender, EventArgs e)
        {
            if (matKhauKhongKhop_DK_Text.Visible)
                return;

            string taiKhoan = taiKhoan_DK_Box.Text;
            bool canCreate = HeThong.KiemTraTaiKhoan(taiKhoan);

            if (!canCreate)
                taiKhoanSai_DK_Text.Visible = true;

            if (!matKhauKhongKhop_DK_Text.Visible && canCreate)
            {
                string matKhau = matKhau1_DK_Box.Text;
                int maCH = cauHoi_CB.SelectedIndex - 1;
                string cauTraLoi = cauTraLoi_Box.Text;

                HeThong.DangKy(taiKhoan, matKhau, maCH, cauTraLoi);
                MessageBox.Show("Đăng ký thành công!!!");

                Signup_Panel.Visible = false;
                LoginPanel.Visible = true;
            }
        }

        private void matKhau1_DK_Box_TextChanged(object sender, EventArgs e)
        {
            if (matKhau1_DK_Box.Text.Length <= 1 && !hienMK_DK_Check.Checked)
                matKhau1_DK_Box.UseSystemPasswordChar = true;
        }

        private void matKhau2_DK_Box_TextChanged(object sender, EventArgs e)
        {
            if (matKhau2_DK_Box.Text.Length <= 1 && !hienMK_DK_Check.Checked)
                matKhau2_DK_Box.UseSystemPasswordChar = true;
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

        private void matKhau2_DK_Box_TextChanged_1(object sender, EventArgs e)
        {
            if (matKhau2_DK_Box.Text.Length <= 1 && !hienMK_DK_Check.Checked)
                matKhau2_DK_Box.UseSystemPasswordChar = true;

            if (matKhau1_DK_Box.Text != matKhau2_DK_Box.Text && !matKhauKhongKhop_DK_Text.Visible)
                matKhauKhongKhop_DK_Text.Visible = true;
            else if (matKhau1_DK_Box.Text == matKhau2_DK_Box.Text && matKhauKhongKhop_DK_Text.Visible)
                matKhauKhongKhop_DK_Text.Visible = false;
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

        private void quenMK_Button_Click(object sender, EventArgs e)
        {
            if (cauHoiQMK_CB.Items.Count == 0)
                this.setCauHoi(cauHoiQMK_CB);
            this.refreshQuenMatKhau_Panel();

            LoginPanel.Visible = false;
            quenMK_Panel.Visible = true;
        }

        private void hienMK_QMK_Check_CheckedChanged(object sender, EventArgs e)
        {
            this.hienMatKhau(hienMK_QMK_Check, matKhau1_QMK_Box);
            this.hienMatKhau(hienMK_QMK_Check, matKhau2_QMK_Box);
        }

        private void matKhau1_QMK_Box_TextChanged(object sender, EventArgs e)
        {
            if (matKhau1_QMK_Box.Text.Length <= 1 && !hienMK_DK_Check.Checked)
                matKhau1_QMK_Box.UseSystemPasswordChar = true;

        }

        private void matKhau2_QMK_Box_TextChanged(object sender, EventArgs e)
        {
            if (matKhau2_QMK_Box.Text.Length <= 1 && !hienMK_DK_Check.Checked)
                matKhau2_QMK_Box.UseSystemPasswordChar = true;

            if (matKhau1_QMK_Box.Text != matKhau2_QMK_Box.Text && !saiMK_Text.Visible)
                saiMK_Text.Visible = true;
            else if (matKhau1_QMK_Box.Text == matKhau2_QMK_Box.Text && saiMK_Text.Visible)
                saiMK_Text.Visible = false;
        }

        private void troVe_QMK_Button_Click(object sender, EventArgs e)
        {
            this.refreshDangNhap_Panel();
            quenMK_Panel.Visible = false;
            LoginPanel.Visible = true;
        }

        private void xacNhan_QMK_Button_Click(object sender, EventArgs e)
        {
            if (saiMK_Text.Visible)
                return;

            if (HeThong.KiemTraTaiKhoan(taiKhoan_QMK_Box.Text))
            {
                thongBao_Text.Text = "Tài khoản không tồn tại";
                thongBao_Text.Visible = true;
                return;
            }
            else if (thongBao_Text.Visible)
            {
                thongBao_Text.Visible = false;
            }

            if (HeThong.KiemTraCauHoi(taiKhoan_QMK_Box.Text, cauHoiQMK_CB.SelectedIndex - 1, cauTraLoi_QML_Box.Text))
            {
                HeThong.CapNhatMatKhau(taiKhoan_QMK_Box.Text, matKhau1_QMK_Box.Text);

                MessageBox.Show("Đổi mật khẩu thành công");

                this.refreshDangNhap_Panel();
                quenMK_Panel.Visible = false;
                LoginPanel.Visible = true;
            }
            else
            {
                thongBao_Text.Text = "Câu hỏi hoặc câu trả lời không chính xác";
                thongBao_Text.Visible = true;
            }
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

            if (khachHang.ngaySinh.Year != 1899)
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
            ve();
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
            if (QH_ComboBox.SelectedIndex != 0)//937, 208
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
            if (PX_ComboBox.SelectedIndex != 0)
            {
                diaChiCuThe_Box.Enabled = true;
            }
            else
            {
                diaChiCuThe_Box.Enabled = false;
            }
        }

        private void HTThemDiaChi_Button_Click(object sender, EventArgs e)
        {
            string maDC = HeThong.MaMoi("maDC");
            int maT_TP = TTP_ComboBox.SelectedIndex;
            int maQH = maT_TP * 100 + QH_ComboBox.SelectedIndex;
            int maPX = maQH * 100 + PX_ComboBox.SelectedIndex;
            DiaChi diaChi = new DiaChi(maDC, hoVaTen_Box.Text, soDienThoai_Box.Text, maT_TP, maQH, maPX, diaChiCuThe_Box.Text);

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

            HeThong.ThemDiaChi(khachHang, diaChi);
            themDiaChi_Panel.Visible = false;
            refreshThemDiaChi_Panel();
        }

        private void dangNhap_Button_Click(object sender, EventArgs e)
        {
            KhachHang_Panel.Visible = false;
            LoginPanel.Visible = true;
            Signup_Panel.Visible = false;
        }

        private void SignUp_Button_Click(object sender, EventArgs e)
        {
            refreshDangKy_Panel();
            KhachHang_Panel.Visible = false;
            Signup_Panel.Visible = true;
        }

        private void troVe_button_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = false;
            KhachHang_Panel.Visible = true;
        }

        private void ve()
        {  
            
            Panel panel = new Panel();
            panel.Size = new Size(918, 130);
            Button btn1 = new Button();
            panel.BackColor = Color.Snow;
            btn1.Text = "Cập nhật";
            btn1.Size = new Size(120, 28);
            btn1.Location = new System.Drawing.Point(779, 35);
            btn1.BackColor = Color.Snow;
            Button btn2 = new Button();
            btn2.Text = "Thiết lập mặc định";
            btn2.Size = new Size(239, 40);
            btn2.Location = new System.Drawing.Point(660, 70);
            btn2.BackColor = Color.Snow;
            panel.Controls.Add(btn1);
            panel.Controls.Add(btn2);  
            listDiaChi_FLPanel.Controls.Add(panel);

        }

    }
}
