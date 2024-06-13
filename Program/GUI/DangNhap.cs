using Program.BLL;
using Program.DTO;
using Program.GUI;
using Program.Properties;
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
        private string typeLog = "All";
        public sendData send = null;
        private Timer timer;

        public DangNhap_Form()
        {
            KhachHangForm form = new KhachHangForm(BLL_User.Instance.DangNhapBangCache());
            form.ShowDialog();
            timer = new Timer();
            timer.Interval = 1;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            this.Close();
        }

        public DangNhap_Form(bool dangNhapState)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            state(dangNhapState);
        }

        public DangNhap_Form(sendData sender, string typeLog, bool isDangNhap = true)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.typeLog = typeLog;
            
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
            cauHoi_CB.Items.Clear();    
            cauHoi_CB.Items.Add(new CBBItem { Value = 0, Text = "Câu hỏi bảo mật" });
            cauHoi_CB.Items.AddRange(BLL_User.Instance.GetAllCauHoi().ToArray());

            cauHoi_CB.Cursor = Cursors.Hand;
            Signup_Panel.Visible = true;
            quenMK_Panel.Visible = false;
            LoginPanel.Visible = false;
        }

        private void dangNhap()
        {
            if(typeLog == "All")
            {
                typeLogInCBB.SelectedIndex = 0;
                typeLogInCBB.Enabled = true;
            }
            else if(typeLog == "KhachHang")
            {
                typeLogInCBB.SelectedIndex = 0;
                typeLogInCBB.Visible = false;
                label1.Text += " User";
            }
            else if(typeLog == "Shop")
            {
                typeLogInCBB.SelectedIndex = 1;
                typeLogInCBB.Visible = false;
                label1.Text += " Shop";
            }

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
            cauHoi_CB.Items.Clear();
            cauHoi_CB.Items.Add(new CBBItem { Value = 0, Text = "Câu hỏi bảo mật" });
            cauHoi_CB.Items.AddRange(BLL_User.Instance.GetAllCauHoi().ToArray());

            taiKhoan_DK_Box.Text = "";
            matKhau1_DK_Box.Text = "";
            cauTraLoi_Box.Text = "";
            showPasswordButton.Image = Resources.eyeClose;
            matKhau1_DK_Box.UseSystemPasswordChar = true;
            taiKhoanSai_DK_Text.Visible = false;
            cauHoi_CB.SelectedIndex = 0;
        }

        private void dangNhap_DN_Button_Click(object sender, EventArgs e)
        { 
            if(taiKhoan_DN_Box.Text == "" || matKhau_DN_Box.Text == "")
            {
                LoginError.Text = "You must fill out all box!!!";
                LoginError.Visible = true;
                return;
            }

            string taiKhoan = taiKhoan_DN_Box.Text;
            string matKhau = matKhau_DN_Box.Text;

            if(typeLogInCBB.SelectedIndex == 0)
            {
                if (BLL_User.Instance.DangNhap(taiKhoan, matKhau) != null)
                {
                    if (rememberMeCB.Checked)
                        BLL_User.Instance.LuuTaiKhoan(new User { taiKhoan = taiKhoan, matKhau = matKhau });

                    if(send != null)
                    {
                        Hide();
                        Dispose();
                        send(taiKhoan, matKhau);
                    }
                    else
                    {
                        Hide();
                        Dispose();
                        KhachHangForm form = new KhachHangForm(new User { taiKhoan = taiKhoan, matKhau = matKhau });
                        form.ShowDialog();
                    }
                }
                else
                {
                    LoginError.Text = "Username or passwork is not correct!!";
                    LoginError.Visible = true;
                }
            }
            else if (typeLogInCBB.SelectedIndex == 1)
            {
                if(BLL_User.Instance.DangNhapAsShop(taiKhoan, matKhau) != null)
                {
                    Hide();
                    Close();
                    BanHang_Form form = new BanHang_Form();
                    sendData send = new sendData(form.setData);
                    send(taiKhoan, matKhau);
                    form.ShowDialog();
                }
                else
                {
                    LoginError.Text = "Username or passwork is not correct!!";
                    LoginError.Visible = true;
                }
            }
            else if (typeLogInCBB.SelectedIndex == 2)
            {
                if(BLL_Admin.Instance.DangNhap(taiKhoan, matKhau) != null)
                {
                    Hide();
                    Close();
                    AdminForm form = new AdminForm(new Admin { taiKhoan = taiKhoan, matKhau = matKhau });
                    form.ShowDialog();
                }
                else
                {
                    LoginError.Text = "Username or passwork is not correct!!";
                    LoginError.Visible = true;
                }
            }

        }

        private void quenMK_Button_Click(object sender, EventArgs e)
        {
            if (cauHoiQMK_CB.Items.Count == 0)
            {
                cauHoiQMK_CB.Items.Add(new CBBItem { Value = 0, Text = "Câu hỏi bảo mật" });
                cauHoiQMK_CB.Items.AddRange(BLL_User.Instance.GetAllCauHoi().ToArray());
            }

            this.refreshQuenMatKhau_Panel();

            LoginPanel.Visible = false;
            quenMK_Panel.Visible = true;
        }
        
        private void refreshQuenMatKhau_Panel()
        {
            matKhau1_QMK_Box.UseSystemPasswordChar = false;
        
            cauHoiQMK_CB.SelectedIndex = 0;
            thongBao_Text.Visible = false;
        }

        private void dangKy_Button_Click(object sender, EventArgs e)
        {
            this.refreshDangKy_Panel();
            LoginPanel.Visible = false;
            Signup_Panel.Visible = true;
        }

        private void troVe_button_Click(object sender, EventArgs e)
        {
            Hide();
            Dispose();
            if (send == null)
            {
                KhachHangForm form = new KhachHangForm(null);
                form.ShowDialog();
            }
        }

        private void dangKy_Botton_Click(object sender, EventArgs e)
        {
            string taiKhoan = taiKhoan_DK_Box.Text;

            if (!BLL_User.Instance.KiemTraTaiKhoan(taiKhoan))
            {
                taiKhoanSai_DK_Text.Visible = true;
                return;
            }

            string matKhau = matKhau1_DK_Box.Text;
            int maCH = cauHoi_CB.SelectedIndex - 1;
            string cauTraLoi = cauTraLoi_Box.Text;

            BLL_User.Instance.DangKy(taiKhoan, matKhau, maCH, cauTraLoi);
            ThongBaoForm form = new ThongBaoForm("Đăng ký thành công!!");
            form.Show();
            refreshDangKy_Panel();
            button1_Click(button1, null);
        }

        private void refreshDangNhap_Panel()
        {
            taiKhoan_DN_Box.Text = "";
            matKhau_DN_Box.Text = "";
            LoginError.Visible = false;
            typeLogInCBB.SelectedIndex = 0;

            matKhau_DN_Box.UseSystemPasswordChar = true;
            showPasswordButton.Image = Resources.eyeClose;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = true;
            Signup_Panel.Visible = false;
            refreshDangNhap_Panel();
        }

        private void troVe_DK_Button_Click(object sender, EventArgs e)
        {
            if(dangNhapF == true)
            {
                LoginPanel.Visible = true;
                Signup_Panel.Visible = false;
                quenMK_Panel.Visible = false;
            }
            else
            {
                KhachHangForm KHForm = new KhachHangForm(null);
                Hide();
                Close();
                KHForm.ShowDialog();
            }

        }

        private void matKhau_DN_Box_TextChangedmatKhau_DN_Box_TextChanged(object sender, EventArgs e)
        {
            if (matKhau_DN_Box.BackColor != Color.FromArgb(236, 230, 255))
            {
                Graphics g = LoginPanel.CreateGraphics();
                matKhau_DN_Box.BackColor = Color.FromArgb(236, 230, 255);
                GUI_Utils.Instance.DrawRectangle(g, new RectangleF(40, 290, 350, 50), Color.FromArgb(236, 230, 255), 7);
            }

            if (LoginError.Text == "You must fill out all box!!!" && taiKhoan_DN_Box.Text != "")
            {
                LoginError.Visible = false;
            }
        }

        private void cauHoi_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cauHoi_CB.SelectedIndex != 0)
                cauTraLoi_Box.Enabled = true;
            else
                cauTraLoi_Box.Enabled = false;
        }

        private void taiKhoan_DN_Box_TextChanged(object sender, EventArgs e)
        {
            if(taiKhoan_DN_Box.BackColor != Color.FromArgb(236, 230, 255))
            {
                Graphics g = LoginPanel.CreateGraphics();
                taiKhoan_DN_Box.BackColor = Color.FromArgb(236, 230, 255);
                GUI_Utils.Instance.DrawRectangle(g, new RectangleF(40, 210, 350, 50), Color.FromArgb(236, 230, 255), 7);
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

            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(430, 0, 1000, 570), Color.FromArgb(107, 0, 227), 170);
            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(140, 425, 180, 40), Color.FromArgb(107, 0, 227), 7);
            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(40, 210, 350, 50), Color.FromArgb(236, 230, 255), 7);
            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(40, 290, 350, 50), Color.FromArgb(236, 230, 255), 7);
        }

        private void Signup_Panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(-540, 0, 1000, 570), Color.FromArgb(107, 0, 227), 170);
            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(580, 440, 180, 40), Color.FromArgb(107, 0, 227), 7);
            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(500, 130, 350, 50), Color.FromArgb(236, 230, 255), 7);
            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(500, 200, 350, 50), Color.FromArgb(236, 230, 255), 7);
            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(500, 360, 350, 50), Color.FromArgb(236, 230, 255), 7);    
        }

        private void QMK_Panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(-540, 0, 1000, 570), Color.FromArgb(107, 0, 227), 170);
            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(472, 160, 412, 48), Color.FromArgb(236, 230, 255), 7);
            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(472, 281, 412, 48), Color.FromArgb(236, 230, 255), 7);
            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(472, 359, 412, 48), Color.FromArgb(236, 230, 255), 7);
            GUI_Utils.Instance.DrawRectangle(g, new RectangleF(518, 462, 314, 60), Color.FromArgb(107, 0, 227), 7);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = true;
            Signup_Panel.Visible = false;
            refreshDangNhap_Panel();
        }

        private void xacNhan_QMK_Button_Click_1(object sender, EventArgs e)
        {
            if (BLL_User.Instance.KiemTraTaiKhoan(taiKhoan_QMK_Box.Text))
            {
                thongBao_Text.Text = "Username is not exits!!";
                thongBao_Text.Visible = true;
                return;
            }
            else if (thongBao_Text.Visible)
            {
                thongBao_Text.Visible = false;
            }

            if (BLL_User.Instance.KiemTraCauHoi(taiKhoan_QMK_Box.Text, cauHoiQMK_CB.SelectedIndex - 1, cauTraLoi_QML_Box.Text))
            {
                BLL_User.Instance.DoiMatKhau(taiKhoan_QMK_Box.Text, matKhau1_QMK_Box.Text);

                ThongBaoForm form = new ThongBaoForm("Đổi mật khẩu thành công!!");
                form.Show();

                this.refreshDangNhap_Panel();
                quenMK_Panel.Visible = false;
                LoginPanel.Visible = true;
            }
            else
            {
                thongBao_Text.Visible = true;
                thongBao_Text.Text = "Infomation incorrect, please try again!!";
            }
        }

        private void matKhau1_QMK_Box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                xacNhan_QMK_Button_Click_1(sender, e);

        }

        private void cauTraLoi_Box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                dangKy_Botton_Click(sender, e);
        }

        private void ShowPasswordButton(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if(matKhau_DN_Box.UseSystemPasswordChar)
            {
                button.Image = Resources.eye;
            }
            else
            {
                button.Image = Resources.eyeClose;
            }
            matKhau_DN_Box.UseSystemPasswordChar = !matKhau_DN_Box.UseSystemPasswordChar;
        }

        private void ShowPasswordButton1_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (matKhau1_DK_Box.UseSystemPasswordChar)
            {
                button.Image = Resources.eye;
            }
            else
            {
                button.Image = Resources.eyeClose;
            }
            matKhau1_DK_Box.UseSystemPasswordChar = !matKhau1_DK_Box.UseSystemPasswordChar;
        }

        private void ShowPasswordButton2_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (matKhau1_QMK_Box.UseSystemPasswordChar)
            {
                button.Image = Resources.eye;
            }
            else
            {
                button.Image = Resources.eyeClose;
            }
            matKhau1_QMK_Box.UseSystemPasswordChar = !matKhau1_QMK_Box.UseSystemPasswordChar;
        }

        private void typeLogInCBB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(typeLogInCBB.SelectedIndex == 0)
            {
                rememberMeCB.Visible = true;
                rememberMeCB.Checked = true;
            }
            else
            {
                rememberMeCB.Visible = false;
                rememberMeCB.Checked = false;
            }
        }
    }
}
