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
    public partial class DangNhap_Form : Form
    {
        private User user = null;
        private KhachHang khachHang = null;
        private bool dangNhapF;
        public DangNhap_Form(bool dangNhapF = true)
        {
            this.dangNhapF = dangNhapF;
            InitializeComponent();
            if (dangNhapF )
            {
                dangNhap();
            }
            else
            {
                dangKy();
            }
        }

        private void dangKy()
        {
            Signup_Panel.Visible = true;
            quenMK_Panel.Visible = false;
            LoginPanel.Visible = false;
            this.Size = new System.Drawing.Size(510, 729);
        }

        private void dangNhap()
        {
            LoginPanel.Visible = true;
            this.Size = new System.Drawing.Size(510, 570);
            Signup_Panel.Visible = false;
            quenMK_Panel.Visible = false;
        }

        private void SignUp_Button_Click(object sender, EventArgs e)
        {
            refreshDangKy_Panel();
            Signup_Panel.Visible = true;
        }
        private void setCauHoi(ComboBox comboBox)
        {
            List<string> listCauHoi = HeThong.LoadCauHoi();
            comboBox.DataSource = listCauHoi;
        }
        private void refreshDangKy_Panel()
        {
            if (cauHoi_CB.Items.Count == 0)
                setCauHoi(cauHoi_CB);
            matKhau1_DK_Box.UseSystemPasswordChar = false;
            matKhau2_DK_Box.UseSystemPasswordChar = false;
            taiKhoanSai_DK_Text.Visible = false;
            matKhauKhongKhop_DK_Text.Visible = false;
            hienMK_DK_Check.Checked = false;
            cauHoi_CB.SelectedIndex = 0;

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
                khachHang = HeThong.DangNhap(user);
                HeThong.WriteAccoutCache(user);
                this.Hide();
                MainForm main = new MainForm();
                DialogResult result = main.ShowDialog();
            }
            else
            {
                LoginError.Visible = true;
            }
        }

        private void quenMK_Button_Click(object sender, EventArgs e)
        {
            if (cauHoiQMK_CB.Items.Count == 0)
                this.setCauHoi(cauHoiQMK_CB);
            this.refreshQuenMatKhau_Panel();

            LoginPanel.Visible = false;
            quenMK_Panel.Visible = true;
            this.Size = new System.Drawing.Size(510, 692);
        }
        
        private void refreshQuenMatKhau_Panel()
        {
            matKhau1_QMK_Box.UseSystemPasswordChar = false;
            matKhau2_QMK_Box.UseSystemPasswordChar = false;
            cauHoiQMK_CB.SelectedIndex = 0;
            saiMK_Text.Visible = false;
            thongBao_Text.Visible = false;
            hienMK_QMK_Check.Checked = false;
        }

        private void dangKy_Button_Click(object sender, EventArgs e)
        {
            this.refreshDangKy_Panel();
            LoginPanel.Visible = false;
            Signup_Panel.Visible = true;
            this.Size = new System.Drawing.Size(510, 729); 
        }

        private void troVe_button_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = false;
            this.Hide();
            MainForm mainForm = new MainForm();
            DialogResult result = mainForm.ShowDialog();
        }

        private void dangKy_Botton_Click(object sender, EventArgs e)
        {
            
            if (matKhauKhongKhop_DK_Text.Visible)
                return;

            string taiKhoan = taiKhoan_DK_Box.Text;
            bool canCreate = HeThong.KiemTraTaiKhoan(taiKhoan);

            if (!canCreate)
            {
                taiKhoanSai_DK_Text.Visible = true;
             
            }

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


        private void refreshDangNhap_Panel()
        {
            taiKhoan_DN_Box.Text = "Tài Khoản";
            matKhau_DN_Box.Text = "Mật khẩu";
            matKhau_DN_Box.UseSystemPasswordChar = false;
            hienMK_DN_Check.Checked = false;
        }
        private void xacNhan_QMK_Button_Click(object sender, EventArgs e)
        {

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

        private void button1_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = true;
            Signup_Panel.Visible = false;
            this.Size = new System.Drawing.Size(510, 570);
        }

        private void troVe_DK_Button_Click(object sender, EventArgs e)
        {
            if(this.dangNhapF == true)
            {
                LoginPanel.Visible = true;
                Signup_Panel.Visible = false;
                this.Size = new System.Drawing.Size(510, 570);
            }
            else
            {
                this.Hide();
                MainForm main = new MainForm();
                DialogResult result = main.ShowDialog();
            }

        }

        private void hienMK_QMK_Check_CheckedChanged(object sender, EventArgs e)
        {
            this.hienMatKhau(hienMK_DK_Check, matKhau1_DK_Box);
            this.hienMatKhau(hienMK_DK_Check, matKhau2_DK_Box);
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

        private void matKhau_DN_Box_TextChanged(object sender, EventArgs e)
        {
            if (taiKhoan_DN_Box.Text == "" || matKhau_DN_Box.Text == "")
                dangNhap_DN_Button.Enabled = false;
            else
                dangNhap_DN_Button.Enabled = true;
        }

        private void hienMK_DN_Check_CheckedChanged(object sender, EventArgs e)
        {
            if (hienMK_DN_Check.Checked)
                matKhau_DN_Box.UseSystemPasswordChar = false;
            else
                matKhau_DN_Box.UseSystemPasswordChar = true;
        }

        private bool kiemTra()
        {
            
            return !(taiKhoan_DK_Box.Text == "" || matKhau1_DK_Box.Text == "" || matKhau2_DK_Box.Text == "" || cauHoi_CB.SelectedIndex == 0 || cauTraLoi_Box.Text == "" || matKhauKhongKhop_DK_Text.Visible == true);
        }

        private void taiKhoan_DK_Box_TextChanged(object sender, EventArgs e)
        {
            dangKy_Botton.Enabled = kiemTra();
        }

        private void matKhau1_DK_Box_TextChanged(object sender, EventArgs e)
        {
           
            dangKy_Botton.Enabled = kiemTra();

            if (matKhau2_DK_Box.Text != matKhau1_DK_Box.Text)
            {
                matKhauKhongKhop_DK_Text.Visible = true;
                dangKy_Botton.Enabled = false;
            }
            else
            {
                matKhauKhongKhop_DK_Text.Visible = false;
            }

        }

        private void matKhau2_DK_Box_TextChanged(object sender, EventArgs e)
        {
            dangKy_Botton.Enabled = kiemTra();
            if (matKhau2_DK_Box.Text != matKhau1_DK_Box.Text)
            {
                matKhauKhongKhop_DK_Text.Visible = true;
                dangKy_Botton.Enabled = false;
            }
            else
            {
                matKhauKhongKhop_DK_Text.Visible = false;
            }
                
        }

        private void cauHoi_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            dangKy_Botton.Enabled = kiemTra();
            if (cauHoi_CB.SelectedIndex != 0)
                cauTraLoi_Box.Enabled = true;
            else
                cauTraLoi_Box.Enabled = false;
        }

        private void cauTraLoi_Box_TextChanged(object sender, EventArgs e)
        {
           
            dangKy_Botton.Enabled = kiemTra();
        }



        private void troVe_QMK_Button_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = true;
            quenMK_Panel.Visible = false;
            this.Size = new System.Drawing.Size(510, 570);
        }

        private void hienMK_DK_Check_CheckedChanged(object sender, EventArgs e)
        {
            if (hienMK_DK_Check.Checked == true)
            {
                matKhau1_DK_Box.UseSystemPasswordChar = false;
                matKhau2_DK_Box.UseSystemPasswordChar = false;
            }
            else
            {
                matKhau1_DK_Box.UseSystemPasswordChar = true;
                matKhau2_DK_Box.UseSystemPasswordChar = true;
            }
        }

        private void taiKhoan_DN_Box_TextChanged(object sender, EventArgs e)
        {
           if (taiKhoan_DN_Box.Text == "" || matKhau_DN_Box.Text == "")
                dangNhap_DN_Button.Enabled = false;
           else
                dangNhap_DN_Button.Enabled = true;
        }

        public bool KTra_QuenMK()
        {
            return !(taiKhoan_QMK_Box.Text == "" || cauHoiQMK_CB.SelectedIndex == 0 || cauTraLoi_QML_Box.Text == "" || matKhau1_QMK_Box.Text == "" || matKhau2_QMK_Box.Text == "" || saiMK_Text.Visible == true);
        }
        private void taiKhoan_QMK_Box_TextChanged(object sender, EventArgs e)
        {
            xacNhan_QMK_Button.Enabled = KTra_QuenMK();
        }

        private void cauHoiQMK_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            xacNhan_QMK_Button.Enabled = KTra_QuenMK();
            if (cauHoiQMK_CB.SelectedIndex == 0)
                cauTraLoi_QML_Box.Enabled = false;           
            else
                cauTraLoi_QML_Box.Enabled = true;
        }

        private void cauTraLoi_QML_Box_TextChanged(object sender, EventArgs e)
        {
            xacNhan_QMK_Button.Enabled = KTra_QuenMK();
        }

        private void matKhau1_QMK_Box_TextChanged(object sender, EventArgs e)
        {
            xacNhan_QMK_Button.Enabled = KTra_QuenMK();
            if (matKhau1_QMK_Box.Text != matKhau2_QMK_Box.Text)
            {
                saiMK_Text.Visible = true;
                xacNhan_QMK_Button.Enabled = false;
            }
            else
            {
                saiMK_Text.Visible = false;
            }
        }

        private void matKhau2_QMK_Box_TextChanged(object sender, EventArgs e)
        {
            xacNhan_QMK_Button.Enabled = KTra_QuenMK();
            if (matKhau1_QMK_Box.Text != matKhau2_QMK_Box.Text)
            {
                saiMK_Text.Visible = true;
                xacNhan_QMK_Button.Enabled = false;
            }
            else
            {
                saiMK_Text.Visible = false;
            }
        }
    }
}
