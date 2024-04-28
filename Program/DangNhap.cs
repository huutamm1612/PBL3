using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    public partial class DangNhap_Form : Form
    {
        public bool dangNhapF;
        public sendData send;

        public DangNhap_Form(bool dangNhapState = true)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            state(dangNhapState);
        }

        public DangNhap_Form(sendData sender, bool isDangNhap = true)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            
            this.send = sender;
            state(isDangNhap);
        }

        private void state(bool dangNhapState)
        {
            if (dangNhapState)
                dangNhap();
            else
                dangKy();
        }

        private void dangKy()
        {
            dangNhapF = false;
            if (cauHoi_CB.Items.Count == 0)
                Utils.SetComboBox(cauHoi_CB, HeThong.LoadCauHoi());
            cauHoi_CB.Cursor = Cursors.Hand;
            Signup_Panel.Visible = true;
            quenMK_Panel.Visible = false;
            LoginPanel.Visible = false;
        }

        private void dangNhap()
        {
            dangNhapF = true;
            LoginPanel.Visible = true;
            Signup_Panel.Visible = false;
            quenMK_Panel.Visible = false;
        }

        private void SignUp_Button_Click(object sender, EventArgs e)
        {
            refreshDangKy_Panel();
            Signup_Panel.Visible = true;
        }

        private void refreshDangKy_Panel()
        {
            if (cauHoi_CB.Items.Count == 0)
                Utils.SetComboBox(cauHoi_CB, HeThong.LoadCauHoi());
            taiKhoan_DK_Box.Text = "";
            matKhau1_DK_Box.Text = "";
            cauTraLoi_Box.Text = "";
            matKhau1_DK_Box.UseSystemPasswordChar = false;
            taiKhoanSai_DK_Text.Visible = false;
            hienMK_DK_Check.Checked = false;
            cauHoi_CB.SelectedIndex = 0;
        }

        private void dangNhap_DN_Button_Click(object sender, EventArgs e)
        {
            if(taiKhoan_DN_Box.Text == "" || matKhau_DN_Box.Text == "")
            {
                Graphics g = LoginPanel.CreateGraphics();
                Color color = Color.FromArgb(255, 153, 153);
                if (taiKhoan_DN_Box.Text == "")
                {
                    Utils.DrawRectangle(g, new RectangleF(40, 210, 350, 50), color, 7);
                    taiKhoan_DN_Box.BackColor = color;
                }

                if (matKhau_DN_Box.Text == "")
                {
                    Utils.DrawRectangle(g, new RectangleF(40, 290, 350, 50), color, 7);
                    matKhau_DN_Box.BackColor = color;
                }

                LoginError.Text = "You must fill out all box!!!";
                LoginError.Visible = true;
                return;
            }

            string taiKhoan = taiKhoan_DN_Box.Text;
            string matKhau = matKhau_DN_Box.Text;
            if (HeThong.DangNhap(taiKhoan, matKhau))
            {
                LoginError.Visible = false;
                this.send(taiKhoan, matKhau);
                Close();
            }
            else
            {
                LoginError.Text = "Username or passwork is not correct!!";
                LoginError.Visible = true;
            }
        }

        private void quenMK_Button_Click(object sender, EventArgs e)
        {
            if (cauHoiQMK_CB.Items.Count == 0)
                Utils.SetComboBox(cauHoiQMK_CB, HeThong.LoadCauHoi());

            this.refreshQuenMatKhau_Panel();

            LoginPanel.Visible = false;
            quenMK_Panel.Visible = true;
        }
        
        private void refreshQuenMatKhau_Panel()
        {
            matKhau1_QMK_Box.UseSystemPasswordChar = false;
        
            cauHoiQMK_CB.SelectedIndex = 0;
            thongBao_Text.Visible = false;
            hienMK_QMK_Check.Checked = false;
        }

        private void dangKy_Button_Click(object sender, EventArgs e)
        {
            this.refreshDangKy_Panel();
            LoginPanel.Visible = false;
            Signup_Panel.Visible = true;
        }

        private void troVe_button_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = false;

            KhachHangForm KHForm = new KhachHangForm();
            this.Hide();
            KHForm.Show();
            Dispose();
        }

        private void dangKy_Botton_Click(object sender, EventArgs e)
        {
            string taiKhoan = taiKhoan_DK_Box.Text;
            bool canCreate = HeThong.KiemTraTaiKhoan(taiKhoan);

            if (!canCreate)
            {
                taiKhoanSai_DK_Text.Visible = true;
                return;
            }

            string matKhau = matKhau1_DK_Box.Text;
            int maCH = cauHoi_CB.SelectedIndex - 1;
            string cauTraLoi = cauTraLoi_Box.Text;

            HeThong.DangKy(taiKhoan, matKhau, maCH, cauTraLoi);
            MessageBox.Show("Đăng ký thành công!!!");
            refreshDangKy_Panel();
            Signup_Panel.Visible = false;
            LoginPanel.Visible = true;
        }

        private void refreshDangNhap_Panel()
        {
            taiKhoan_DN_Box.Text = "Tài Khoản";
            matKhau_DN_Box.Text = "Mật khẩu";
            matKhau_DN_Box.UseSystemPasswordChar = false;
            hienMK_DN_Check.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = true;
            Signup_Panel.Visible = false;
        }

        private void troVe_DK_Button_Click(object sender, EventArgs e)
        {
            if(this.dangNhapF == true)
            {
                LoginPanel.Visible = true;
                Signup_Panel.Visible = false;
                quenMK_Panel.Visible = false;
            }
            else
            {
                KhachHangForm KHForm = new KhachHangForm();
                this.Hide();
                KHForm.Show();
                Dispose();
            }

        }

        private void hienMK_QMK_Check_CheckedChanged(object sender, EventArgs e)
        {
            this.hienMatKhau(hienMK_DK_Check, matKhau1_DK_Box);
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
            if (matKhau_DN_Box.BackColor != Color.FromArgb(236, 230, 255))
            {
                Graphics g = LoginPanel.CreateGraphics();
                matKhau_DN_Box.BackColor = Color.FromArgb(236, 230, 255);
                Utils.DrawRectangle(g, new RectangleF(40, 290, 350, 50), Color.FromArgb(236, 230, 255), 7);
            }

            if (LoginError.Text == "You must fill out all box!!!" && taiKhoan_DN_Box.Text != "")
            {
                LoginError.Visible = false;
            }
        }


        private bool kiemTra()
        {
            return !(taiKhoan_DK_Box.Text == "" || matKhau1_DK_Box.Text == "" || cauHoi_CB.SelectedIndex == 0 || cauTraLoi_Box.Text == "");
        }

        private void taiKhoan_DK_Box_TextChanged(object sender, EventArgs e)
        {
            dangKy_Botton.Enabled = kiemTra();
        }

        private void matKhau1_DK_Box_TextChanged(object sender, EventArgs e)
        {
            dangKy_Botton.Enabled = kiemTra();
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
            this.Size = new System.Drawing.Size(892, 563);
        }

        private void hienMK_DK_Check_CheckedChanged(object sender, EventArgs e)
        {
            if (hienMK_DK_Check.Checked == true)
            {
                matKhau1_DK_Box.UseSystemPasswordChar = false;
            }
            else
            {
                matKhau1_DK_Box.UseSystemPasswordChar = true;
            }
        }

        private void taiKhoan_DN_Box_TextChanged(object sender, EventArgs e)
        {
            if(taiKhoan_DN_Box.BackColor != Color.FromArgb(236, 230, 255))
            {
                Graphics g = LoginPanel.CreateGraphics();
                taiKhoan_DN_Box.BackColor = Color.FromArgb(236, 230, 255);
                Utils.DrawRectangle(g, new RectangleF(40, 210, 350, 50), Color.FromArgb(236, 230, 255), 7);
            }

            if (LoginError.Text == "You must fill out all box!!!" && matKhau_DN_Box.Text != "")
            {
                LoginError.Visible = false;
            }
        }

        public bool KTra_QuenMK()
        {
            return !(taiKhoan_QMK_Box.Text == "" || cauHoiQMK_CB.SelectedIndex == 0 || cauTraLoi_QML_Box.Text == "" || matKhau1_QMK_Box.Text == "" );
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
        }


        private void Enter_DangNhap(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dangNhap_DN_Button_Click(sender, e);
            }
        }

        private void LoginPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;//910, 610

            Utils.DrawRectangle(g, new RectangleF(430, 0, 1000, 570), Color.FromArgb(107, 0, 227), 170);
            Utils.DrawRectangle(g, new RectangleF(140, 425, 180, 40), Color.FromArgb(107, 0, 227), 7);
            Utils.DrawRectangle(g, new RectangleF(40, 210, 350, 50), Color.FromArgb(236, 230, 255), 7);
            Utils.DrawRectangle(g, new RectangleF(40, 290, 350, 50), Color.FromArgb(236, 230, 255), 7);
        }

        private void Signup_Panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Utils.DrawRectangle(g, new RectangleF(-540, 0, 1000, 570), Color.FromArgb(107, 0, 227), 170);
            Utils.DrawRectangle(g, new RectangleF(580, 440, 180, 40), Color.FromArgb(107, 0, 227), 7);
            Utils.DrawRectangle(g, new RectangleF(500, 130, 350, 50), Color.FromArgb(236, 230, 255), 7);
            Utils.DrawRectangle(g, new RectangleF(500, 200, 350, 50), Color.FromArgb(236, 230, 255), 7);
            Utils.DrawRectangle(g, new RectangleF(500, 360, 350, 50), Color.FromArgb(236, 230, 255), 7);    
        }

        private void QMK_Panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Utils.DrawRectangle(g, new RectangleF(-540, 0, 1000, 570), Color.FromArgb(107, 0, 227), 170);
            Utils.DrawRectangle(g, new RectangleF(472, 160, 412, 48), Color.FromArgb(236, 230, 255), 7);
            Utils.DrawRectangle(g, new RectangleF(472, 281, 412, 48), Color.FromArgb(236, 230, 255), 7);
            Utils.DrawRectangle(g, new RectangleF(472, 359, 412, 48), Color.FromArgb(236, 230, 255), 7);
            Utils.DrawRectangle(g, new RectangleF(518, 462, 314, 60), Color.FromArgb(107, 0, 227), 7);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = true;
            Signup_Panel.Visible = false;
        }

        private void matKhau1_QMK_Box_TextChanged_1(object sender, EventArgs e)
        {
            if (hienMK_DN_Check.Checked)
                matKhau_DN_Box.UseSystemPasswordChar = false;
            else
                matKhau_DN_Box.UseSystemPasswordChar = true;
        }

        private void xacNhan_QMK_Button_Click_1(object sender, EventArgs e)
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
                MessageBox.Show("Sai câu hỏi bảo mật,vui lòng thử lại");  
            }
        }

        private void hienMK_QMK_Check_CheckedChanged_1(object sender, EventArgs e)
        {
            if (hienMK_DN_Check.Checked)
                matKhau_DN_Box.UseSystemPasswordChar = false;
            else
                matKhau_DN_Box.UseSystemPasswordChar = true;
        }


    }
}
