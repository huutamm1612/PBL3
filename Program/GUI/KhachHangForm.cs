using Microsoft.Win32;
using Program.BLL;
using Program.DAL;
using Program.DTO;
using Program.GUI;
using Program.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public delegate void SendData(params DanhGia[] danhGia);
    public partial class KhachHangForm : Form
    {
        private User user = null;
        private Panel currPanel = null;
        private Panel currChildPanel = null;
        private QLSanPham qlSanPham = null;
        private KhachHang khachHang = null;
        private QLBaiDang qlBaiDangTmp = null; // for search and shop
        private QLBaiDang qlBaiDang = null;
        private BaiDang currBaiDang = null;
        private SanPham currSanPham = null;
        private DiaChi currDiaChi = null;
        private Shop currShop = null;
        private string URL = null;
        private string currMaDH = null;
        private string currMaDG = null;
        private int currSao = -1;

        private const int nColBaiDang_Home = 6;
        private const int nRowBaiDang_Home = 4;
        private const int nColBaiDang_BaiDang = 6;
        private const int nRowBaiDang_BaiDang = 4;
        private const int nColBaiDang_TimKiem = 5;
        private const int nRowBaiDang_TimKiem = 4;
        private const int nColBaiDang_Shop = 5;
        private const int nRowBaiDang_Shop = 4;
        private const int nDanhGiaInBaiDang = 5;

        private delegate void ClickEvent(object sender, EventArgs e);

        public KhachHangForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.gioHangPanel.MouseWheel += GioHangPanel_MouseWheel;

            TuDongDangNhap();
            qlBaiDang = BLL_BaiDang.Instance.GetBaiDangForHomeList();
            currPanel = HomePanel;
        }
        public KhachHangForm(User user)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.gioHangPanel.MouseWheel += GioHangPanel_MouseWheel;

            this.user = user;

            if (this.user != null)
            {
                khachHang = BLL_KhachHang.Instance.GetKhachHangFromTaiKhoan(this.user.taiKhoan);
                DangNhapThanhCong();
                SetHeaderPanel();
            }

            qlBaiDang = BLL_BaiDang.Instance.GetBaiDangForHomeList();
            currPanel = HomePanel;
        }

        public void DangNhapThanhCong()
        {
            dangNhap_Button.Visible = false;
            SignUp_Button.Visible = false;
            userProfile_Button.Visible = true;
            user_DangXuat_Button.Visible = true;
            flowLayoutPanel6.Visible = true;

            try
            {
                miniAvt.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(khachHang.avt), miniAvt.Size);
            }
            catch
            {
                miniAvt.Image = null;
            }
        }

        public void SetData(string taiKhoan, string matKhau)
        {
            user = BLL_User.Instance.DangNhap(taiKhoan, matKhau);

            khachHang = BLL_KhachHang.Instance.GetKhachHangFromTaiKhoan(user.taiKhoan);
            DangNhapThanhCong();
            SetHeaderPanel();
        }

        private void HienMatKhau(CheckBox hienMK, TextBox box)
        {
            if (hienMK.Checked)
                box.UseSystemPasswordChar = false;
            else
                box.UseSystemPasswordChar = true;
        }

        private void TuDongDangNhap()
        {
            user = BLL_User.Instance.DangNhapBangCache();
            if (user != null)
            {
                khachHang = BLL_KhachHang.Instance.GetKhachHangFromTaiKhoan(user.taiKhoan);
                SetHeaderPanel();
                DangNhapThanhCong();
            }
        }

        private void RefreshCartButton()
        {
            if (khachHang == null)
            {
                cartButton.Text = "";
                return;
            }
            cartButton.Text = BLL_GioHang.Instance.GetSoInGioHangIcon(khachHang.gioHang);
        }

        private void SetHeaderPanel()
        {
            RefreshCartButton();
            if (user != null)
            {
                userProfile_Button.Text = user.taiKhoan + "◀";
            }
        }

        private void myAccount_Button_Click(object sender, EventArgs e)
        {
            SwitchPanel(ref currChildPanel, ref profilePanel);
        }

        private void userProfile_Botton_Click(object sender, EventArgs e)
        {
            SwitchPanel(ref currChildPanel, ref profilePanel);
            ChangeColorOfButtonInFLP(sender as Button);
        }

        private void hienMK_UP_Check_CheckedChanged(object sender, EventArgs e)
        {
            this.HienMatKhau(hienMK_UP_Check, matKhauCu_Box);
            this.HienMatKhau(hienMK_UP_Check, matKhauMoi1_Box);
            this.HienMatKhau(hienMK_UP_Check, matKhauMoi2_Box);
        }

        private void ChangeColorOfButtonInFLP(Button button)
        {
            foreach (Button btn in funcFLPanel.Controls)
            {
                btn.ForeColor = Color.Black;
            }
            button.ForeColor = Color.Red;
        }

        private void SpreadOutClick(object sender, EventArgs e)
        {
            Button button = sender as Button;

            foreach (Button btn in funcFLPanel.Controls)
            {
                if (btn.Image == null)
                {
                    btn.Visible = false;
                }
            }

            int index = funcFLPanel.Controls.IndexOf(button) + 1;
            bool spread = !funcFLPanel.Controls[index].Visible;


            while (funcFLPanel.Controls[index].Text != "")
            {
                funcFLPanel.Controls[index].Visible = spread;
                index++;
            }
        }

        private void UserAccountButton_Click(object sender, EventArgs e)
        {
            if (!profilePanel.Visible)
                SpreadOutClick(myAccount_Button, e);
            userProfile_Botton_Click(userProfile_Botton, e);
        }

        private void ThongBaoUserButton_Click(object sender, EventArgs e)
        {
            if (!capNhatDHPanel.Visible)
                SpreadOutClick(thongBaoUserButton, e);
            CapNhatDonHangButton_Click(capNhatDHButton, e);
        }

        private void CapNhatDonHangButton_Click(object sender, EventArgs e)
        {
            TBDonHangFLP.Controls.Clear();
            TBDonHangFLP.Height = 0;
            foreach (ThongBao thongBao in khachHang.listThongBao.list)
            {
                if (thongBao.dinhKem.Contains("DH"))
                {
                    Panel panel = DrawThongBaoDH(thongBao);
                    TBDonHangFLP.Controls.Add(panel);
                    TBDonHangFLP.Controls.SetChildIndex(panel, 0);
                }
            }
            if (TBDonHangFLP.Controls.Count == 0)
                TBDonHangFLP.Controls.Add(khongCoTBPanel);

            GUI_Utils.Instance.FitFLPHeight(TBDonHangFLP);

            capNhatDHPanel.Height = TBDonHangFLP.Height + 50;

            SwitchPanel(ref currChildPanel, ref capNhatDHPanel);
            ChangeColorOfButtonInFLP(sender as Button);

        }

        private void MouseIn(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.ForeColor != Color.Red)
                button.ForeColor = Color.Salmon;
        }
        private void MouseOut(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.ForeColor != Color.Red)
                button.ForeColor = Color.Black;
        }

        private void ChonAnhButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "File anh|*.jpg.; *.gif; *.png; |All file| *.*"
            };

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                avtInUserProfile.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(openFile.FileName), avtInUserProfile.Size);
                URL = openFile.FileName;
            }
        }
        private void userProfile_Button_Paint(object sender, PaintEventArgs e)
        {
            GUI_Utils.Instance.FitTextBox(sender as Control, 0, 0);
        }

        private void doiMK_Botton_Click(object sender, EventArgs e)
        {
            SwitchPanel(ref currChildPanel, ref doiMatKhauPanel);
            ChangeColorOfButtonInFLP(sender as Button);
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
                BLL_User.Instance.DoiMatKhau(user, matKhauMoi1_Box.Text);

                ThongBaoForm form = new ThongBaoForm("Đổi mật khẩu thành công!!");
                form.Show();

                if (saiMKC_Text.Visible)
                    saiMKC_Text.Visible = false;

                matKhauCu_Box.Clear();
                matKhauMoi1_Box.Clear();
                matKhauMoi2_Box.Clear();
            }
        }

        private void SwitchPanel(ref Panel currPanel, ref Panel newPanel)
        {
            KhachHang_Panel.Controls.Add(waittingForm);
            waittingForm.Location = new Point(0, 0);
            waittingForm.Size = KhachHang_Panel.Size;
            waittingForm.BringToFront();
            currPanel.SendToBack();
            currPanel.Visible = false;

            if (currPanel.Equals(newPanel))
                waittingForm.Size = currPanel.Size;
            else
                waittingForm.Size = KhachHang_Panel.Size;

            waittingForm.Location = newPanel.Location;
            waittingForm.Visible = true;

            currPanel.Visible = false;
            currPanel = newPanel;

            currPanel.AutoScrollPosition = new Point(0, 0);
            currPanel.Visible = true;

            waittingForm.Visible = false;
            currPanel.BringToFront();
        }

        private void home_Button_Click(object sender, EventArgs e)
        {
            SwitchPanel(ref currPanel, ref HomePanel);
        }

        private void ChangeEditButtonColor(object sender)
        {
            Button button = sender as Button;
            if(button.Text == "Sửa")
            {
                button.BackColor = Color.White;
                button.FlatAppearance.MouseOverBackColor = Color.White;
                button.FlatAppearance.MouseDownBackColor = Color.White; 
            }
            else
            {
                button.BackColor = Color.Gainsboro;
                button.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
                button.FlatAppearance.MouseDownBackColor = Color.Gainsboro;
            }
        }

        private void userProfile_Button_Click(object sender, EventArgs e)
        {
            if (currPanel == UserPanel)
                return;
            littleMenuPanel.Visible = false;
            waittingPanel.Visible = true;
            waittingPanel.BringToFront();
            currChildPanel = profilePanel;
            SwitchPanel(ref currPanel, ref UserPanel);
        }

        private void suaTen_Button_Click(object sender, EventArgs e)
        {
            ChangeEditButtonColor(sender);
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
            ChangeEditButtonColor(sender);
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
            ChangeEditButtonColor(sender);
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

            if (URL != null)
                khachHang.avt = Utils.Instance.GetImageURL(System.Drawing.Image.FromFile(URL));

            khachHang.Sua(ten_UP_Box.Text, email_UP_Box.Text, soDT_UP_Box.Text, gioiTinh, ngaySinh);
            DAL_KhachHang.Instance.CapNhatKhachHang(khachHang);
            SwitchPanel(ref currChildPanel, ref profilePanel);

            ThongBaoForm form = new ThongBaoForm("Cập nhật thông tin cá nhân hoàn thành!!");
            form.Show();
        }

        public void ThemDiaChi(DiaChi diaChi)
        {
            BLL_DiaChi.Instance.ThemDiaChi(khachHang, diaChi);
            ThongBaoForm form = new ThongBaoForm("Thêm địa chỉ thành công!!");
            form.Show();
        }

        public void CapNhatDiaChi(DiaChi diaChi)
        {
            BLL_DiaChi.Instance.CapNhatDiaChi(khachHang, diaChi);
        }

        private void themDiaChi_Button_Click(object sender, EventArgs e)
        {
            DimForm dimForm = new DimForm();
            dimForm.Show();
            DiaChiForm form = new DiaChiForm(ThemDiaChi);
            form.ShowDialog();
            dimForm.Close();
            VeLai_DiaChi();
        }

        private void diaChiUser_Button_Click(object sender, EventArgs e)
        {
            SwitchPanel(ref currChildPanel, ref diaChiUser_Panel);
            ChangeColorOfButtonInFLP(sender as Button);
        }

        private void dangNhap_Button_Click(object sender, EventArgs e)
        {
            Hide();
            Close();
            DangNhap_Form form = new DangNhap_Form(true);
            form.ShowDialog();
        }

        private void SignUp_Button_Click(object sender, EventArgs e)
        {
            Hide();
            Close();
            DangNhap_Form form = new DangNhap_Form(false);
            form.ShowDialog();
        }

        private void VeLai_DiaChi()
        {
            if (khachHang.diaChi == null) return;
            listDiaChi_FLPanel.Controls.Clear();
            VeDiaChiMacDinh();

            foreach (DiaChi diachi in khachHang.listDiaChi)
            {
                VeDiaChi(diachi);
            }

            int height = 0;
            foreach (Control control in listDiaChi_FLPanel.Controls)
            {
                height += control.Height + 6;
            }

            if (listDiaChi_FLPanel.Controls.Count == 0)
                listDiaChi_FLPanel.Controls.Add(khongCoDiaChiPanel);

            GUI_Utils.Instance.FitFLPHeight(listDiaChi_FLPanel);
            diaChiUser_Panel.Height = height + 100;
        }

        private void VeDiaChiMacDinh()
        {
            TextBox txt = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Text = khachHang.diaChi.ToString(),
                Size = new Size(606, 60),
                Location = new System.Drawing.Point(11, 27),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White
            };

            Button btn1 = new Button
            {
                Text = "Cập nhật",
                Size = new Size(120, 28),
                Cursor = Cursors.Hand,
                Location = new System.Drawing.Point(779, 10),
                BackColor = Color.White
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
                BackColor = Color.White,
            };

            panel.Controls.Add(txt);
            panel.Controls.Add(btn1);
            panel.Controls.Add(lb);
            listDiaChi_FLPanel.Controls.Add(panel);
        }

        private void VeDiaChi(DiaChi diaChi)
        {
            TextBox txt = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Text = diaChi.ToString(),
                Size = new Size(606, 83),
                Location = new System.Drawing.Point(11, 27),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
            };

            Button btn1 = new Button
            {
                Text = "Cập nhật",
                Size = new Size(120, 28),
                Cursor = Cursors.Hand,
                Location = new System.Drawing.Point(779, 35),
                BackColor = Color.White,
            };
            btn1.Click += CapNhat_Button;


            Button btn2 = new Button
            {
                Text = "Thiết lập mặc định",
                Size = new Size(239, 40),
                Cursor = Cursors.Hand,
                Location = new System.Drawing.Point(660, 70),
                BackColor = Color.White,
            };
            btn2.Click += MacDinh_Button;

            Button btn3 = new Button
            {
                Text = "Xóa",
                Size = new Size(120, 28),
                Cursor = Cursors.Hand,
                Location = new System.Drawing.Point(660, 35),
                BackColor = Color.White,
            };
            btn3.Click += xoa_Button;

            Panel panel = new Panel
            {
                Size = new Size(918, 130),
                BackColor = Color.White,
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

            BLL_KhachHang.Instance.CapNhatDiaChiMacDinh(khachHang, khachHang.listDiaChi[panelIndex - 1]);

            VeLai_DiaChi();
        }

        private void xoa_Button(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Panel panelToRemove = clickedButton.Parent as Panel;
            int panelIndex = listDiaChi_FLPanel.Controls.IndexOf(panelToRemove);

            if (panelIndex != -1 && panelIndex < listDiaChi_FLPanel.Controls.Count)
            {
                listDiaChi_FLPanel.Controls.RemoveAt(panelIndex);
                BLL_KhachHang.Instance.XoaDiaChi(khachHang, khachHang.listDiaChi[panelIndex - 1]);
            }

            VeLai_DiaChi();
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
            DiaChiForm form = new DiaChiForm(CapNhatDiaChi, diaChi);
            form.ShowDialog();
            dimForm.Close();
            VeLai_DiaChi();
        }

        private void user_DangXuat_Button_Click(object sender, EventArgs e)
        {
            khachHang = null;
            user = null;
            littleMenuPanel.Visible = false;
            flowLayoutPanel6.Visible = false;
            dangNhap_Button.Visible = true;
            SignUp_Button.Visible = true;
            dangNhap_Button.BringToFront();
            SignUp_Button.BringToFront();
            label.BringToFront();

            if (currPanel == UserPanel)
                SwitchPanel(ref currPanel, ref HomePanel);

            SetHeaderPanel();
            BLL_User.Instance.ClearCache();
        }

        private void soDT_UP_Box_TextChanged(object sender, EventArgs e)
        {
            if (soDT_UP_Box.Text == "")
                return;

            if (!Utils.Instance.KiemTraSoDT(soDT_UP_Box.Text))
            {
                SDTKhongHopLe_Label.Visible = true;
                luu_UP_Button.Enabled = false;
                if (suaSDT_button.Text == "Lưu")
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
            {
                Hide();
                DangNhap_Form form = new DangNhap_Form(null, "Shop");
                Close();
                form.ShowDialog();
            }
            else
            {
                Hide();
                BanHang_Form BHForm = new BanHang_Form();
                sendData send = new sendData(BHForm.setData);
                send(user.taiKhoan, user.matKhau);
                Close();
                BHForm.ShowDialog();
            }
        }


        private void MouseHoverPanel(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            panel.BorderStyle = BorderStyle.FixedSingle;
        }

        private void MouseLeavePanel(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            panel.BorderStyle = BorderStyle.None;
        }
        private void MouseMovePanel(object sender, MouseEventArgs e)
        {
            Panel panel = sender as Panel;
            panel.BorderStyle = BorderStyle.FixedSingle;
        }

        private void MouseHoverObjInPanel(object sender, EventArgs e)
        {
            Control con = sender as Control;

            while (con.Parent.Name != "parentPanel")
                con = con.Parent;

            MouseHoverPanel(con.Parent, e);
        }

        private void MouseMoveObjInPanel(object sender, MouseEventArgs e)
        {
            Control con = sender as Control;

            while (con.Parent.Name != "parentPanel")
                con = con.Parent;

            MouseMovePanel(con.Parent, e);
        }

        private void MouseLeaveObjInPanel(object sender, EventArgs e)
        {
            Control con = sender as Control;

            while (con.Parent.Name != "parentPanel")
                con = con.Parent;

            MouseLeavePanel(con.Parent, e);
        }

        private void SetPageFLP(FlowLayoutPanel flp, int currPageNumber, int numPage, ClickEvent func)
        {
            Font font = new Font("Gadugi", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            List<Button> buttons = new List<Button>();

            foreach (Button button in flp.Controls)
            {
                if (button.Text == ">" || button.Text == "<")
                {
                    continue;
                }
                buttons.Add(button);
            }

            foreach (Button button in buttons)
            {
                flp.Controls.Remove(button);
            }

            if (numPage < 5)
            {
                for (int i = 1; i <= numPage; i++)
                {
                    Button button = new Button
                    {
                        Font = font,
                        Cursor = Cursors.Hand,
                        Text = i.ToString(),
                        BackColor = flp.BackColor,
                        Size = new Size(45, 45),
                        FlatStyle = FlatStyle.Flat,
                        Parent = flp
                    };
                    button.FlatAppearance.BorderSize = 0;
                    button.Click += new EventHandler(func);

                    if (i == currPageNumber)
                    {
                        button.BackColor = Color.OrangeRed;
                        button.ForeColor = Color.White;
                        button.FlatAppearance.MouseDownBackColor = Color.OrangeRed;
                        button.FlatAppearance.MouseOverBackColor = Color.OrangeRed;
                    }
                    else
                    {
                        button.FlatAppearance.MouseDownBackColor = flp.BackColor;
                        button.FlatAppearance.MouseOverBackColor = flp.BackColor;
                    }

                    flp.Controls.Add(button);
                    flp.Controls.SetChildIndex(button, i);
                }
            }
            else
            {

                int index = 1;

                foreach (string i in Utils.Instance.GetListPage(currPageNumber, numPage))
                {
                    Button button = new Button
                    {
                        Font = font,
                        Cursor = Cursors.Default,
                        Text = i.ToString(),
                        BackColor = flp.BackColor,
                        Size = new Size(45, 45),
                        FlatStyle = FlatStyle.Flat,
                        Parent = flp
                    };

                    button.FlatAppearance.BorderSize = 0;
                    button.Click += new EventHandler(func);

                    if (i == currPageNumber.ToString())
                    {
                        button.BackColor = Color.OrangeRed;
                        button.ForeColor = Color.White;
                        button.FlatAppearance.MouseDownBackColor = Color.OrangeRed;
                        button.FlatAppearance.MouseOverBackColor = Color.OrangeRed;
                    }
                    else
                    {
                        button.FlatAppearance.MouseDownBackColor = flp.BackColor;
                        button.FlatAppearance.MouseOverBackColor = flp.BackColor;
                    }

                    if (i != "...")
                    {
                        button.Click += new EventHandler(func);
                        button.Cursor = Cursors.Hand;
                    }
                    flp.Controls.Add(button);
                    flp.Controls.SetChildIndex(button, index);
                    index++;
                }
            }

            flp.Size = new Size(51 * flp.Controls.Count + 6, 51);
            flp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flp.Location = new Point(flp.Parent.Width / 2 - flp.Width / 2, 66 / 2 - flp.Height / 2);
        }

        private int CurrPageNumber(FlowLayoutPanel flp)
        {
            foreach (Button button in flp.Controls)
            {
                if (button.BackColor == Color.OrangeRed)
                    return Convert.ToInt32(button.Text);
            }

            return 1;
        }

        private void ChuyenTrang(FlowLayoutPanel listBaiDangFLP, QLBaiDang listBaiDang, int page, ClickEvent func, int nRow, int nCol)
        {
            listBaiDangFLP.Controls.Clear();
            GC.Collect();

            listBaiDangFLP.Visible = false;

            int startIndex = nRow * nCol * (page - 1);

            for (int i = 0; i < nRow * nCol && (i + startIndex) < listBaiDang.list.Count; i++)
            {
                Panel panel = DrawBaiDang(listBaiDang.list[i + startIndex], listBaiDangFLP, func);
                listBaiDangFLP.Controls.Add(panel);
                listBaiDangFLP.Height = ((i - 1) / nCol + 1) * (panel.Height + panel.Margin.Top + panel.Margin.Bottom);
                listBaiDangFLP.Width = (panel.Width + panel.Margin.Left + panel.Margin.Right) * nCol;
            }

            listBaiDangFLP.Visible = true;
            GUI_Utils.Instance.FitFLPHeight(listBaiDangFLP.Parent);
        }

        private void HomePanel_Load(object sender, EventArgs e)
        {
            if (HomePanel.Visible == false)
            {
                waittingPanel.BringToFront();
                KhachHang_Panel.Controls.Add(HeaderPannel);
                HeaderPannel.Visible = true;
                HeaderPannel.BringToFront();

                waittingPanel.SendToBack();

                FLPBaiDang1.Controls.Clear();
                HeaderPannel.Width = Width;
            }
            else
            {
                waittingPanel.BringToFront();
                waittingPanel.Visible = true;
                KhachHang_Panel.Controls.Add(HeaderPannel);
                HeaderPannel.Location = new Point(0, 0);


                qlBaiDang = BLL_BaiDang.Instance.GetBaiDangForHomeList();
                HomePanel.AutoScrollPosition = new Point(0, 0);

                FLPBaiDang1.Controls.Clear();

                for (int i = 0; i < nColBaiDang_Home * nRowBaiDang_Home && i < qlBaiDang.list.Count; i++)
                {
                    Panel panel = DrawBaiDang(qlBaiDang.list[i], FLPBaiDang1, DenBaiDangTuHome);
                    FLPBaiDang1.Controls.Add(panel);
                    FLPBaiDang1.Height = ((i - 1) / nColBaiDang_Home + 1) * (panel.Height + panel.Margin.Top + panel.Margin.Bottom);
                    FLPBaiDang1.Width = (panel.Width + panel.Margin.Left + panel.Margin.Right) * nColBaiDang_Home;
                }

                GUI_Utils.Instance.FitFLPHeight(homeFLP);
                SetPageFLP(PageListFLP, 1, (qlBaiDang.list.Count - 1) / (nColBaiDang_Home * nRowBaiDang_Home) + 1, SelectPageHomeButton_Click);


                waittingPanel.SendToBack();
            }
        }

        private int DenBaiDang(object sender)
        {
            Control con = sender as Control;

            while (con.Parent.Name != "parentPanel")
                con = con.Parent;

            Panel panel = con.Parent as Panel;
            FlowLayoutPanel flp = panel.Parent as FlowLayoutPanel;

            return flp.Controls.IndexOf(panel);
        }

        private void DenBaiDangTuHome(object sender, EventArgs e)
        {
            int index = DenBaiDang(sender);
            int currPage = CurrPageNumber(PageListFLP);

            currBaiDang = qlBaiDang.list[(currPage - 1) * (nColBaiDang_Home * nRowBaiDang_Home) + index];
            currShop = DAL_Shop.Instance.LoadShopFromMaBD(currBaiDang.maBD);
            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        private void DenBaiDangTuBD(object sender, EventArgs e)
        {
            int index = DenBaiDang(sender);
            int currPage = CurrPageNumber(PageListBDFLP);

            currBaiDang = qlBaiDang.list[(currPage - 1) * (nColBaiDang_BaiDang * nRowBaiDang_BaiDang) + index];
            currShop = DAL_Shop.Instance.LoadShopFromMaBD(currBaiDang.maBD);
            currSanPham = null;
            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        private void DenBaiDangTuDaXem(object sender, EventArgs e)
        {
            int index = DenBaiDang(sender);

            currBaiDang = BLL_BaiDang.Instance.GetBaiDangFromMaBD(khachHang.listDaXem[index]);
            if (currBaiDang != null)
            {
                currShop = DAL_Shop.Instance.LoadShopFromMaBD(currBaiDang.maBD);
            }
            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        private void DenBaiDangTuDaThich(object sender, EventArgs e)
        {
            int index = DenBaiDang(sender);//listPageTimKiemFLP

            currBaiDang = BLL_BaiDang.Instance.GetBaiDangFromMaBD(khachHang.listThich[index]);
            if (currBaiDang != null)
            {
                currShop = DAL_Shop.Instance.LoadShopFromMaBD(currBaiDang.maBD);
            }

            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        private void DenBaiDangTuTimKiem(object sender, EventArgs e)
        {
            int index = DenBaiDang(sender);
            int currPage = CurrPageNumber(listPageTimKiemFLP);

            currBaiDang = qlBaiDangTmp.list[(currPage - 1) * (nColBaiDang_TimKiem * nRowBaiDang_TimKiem) + index];
            currShop = DAL_Shop.Instance.LoadShopFromMaBD(currBaiDang.maBD);
            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        private void DenBaiDangTuShop(object sender, EventArgs e)
        {
            int index = DenBaiDang(sender);
            int currPage = CurrPageNumber(listPageTimKiemFLP);

            currBaiDang = qlBaiDangTmp.list[(currPage - 1) * (nColBaiDang_Shop * nRowBaiDang_Shop) + index];
            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        private void DenBaiDangTuGioHang(object sender, EventArgs e)
        {
            string maSP = GUI_Utils.Instance.FindControl(((Control)sender).Parent as Panel, "maSP").Text;
            currBaiDang = BLL_BaiDang.Instance.GetBaiDangFromMaBD(khachHang.gioHang.GetSanPhamFromMaSP(maSP).maBD);

            if(currBaiDang != null)
            {
                currShop = DAL_Shop.Instance.LoadShopFromMaBD(currBaiDang.maBD);
                currSanPham = currBaiDang.GetSanPhamFromMaSP(maSP);
            }

            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        private void DenBaiDangTuDonHang(object sender, EventArgs e)
        {
            int index = DenBaiDang(sender) - 1;
            currBaiDang = BLL_BaiDang.Instance.GetBaiDangFromMaBD(khachHang.listDonHang.GetDonHangFromMaDH(currMaDH).list[index].maBD);

            if (currBaiDang != null)
            {
                currShop = DAL_Shop.Instance.LoadShopFromMaBD(currBaiDang.maBD);
                currSanPham = BLL_SanPham.Instance.GetSanPhamFromMaSP(khachHang.listDonHang.GetDonHangFromMaDH(currMaDH).list[index].maSP);
            }

            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        private Panel DrawBaiDang(BaiDang baiDang, FlowLayoutPanel parent, ClickEvent func)
        {
            Panel panel = new Panel
            {
                Name = "parentPanel",
                Margin = panel31.Margin,
                Size = panel31.Size,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Parent = parent,
                Cursor = Cursors.Hand
            };
            panel.MouseLeave += new EventHandler(MouseLeavePanel);
            panel.MouseHover += new EventHandler(MouseHoverPanel);
            panel.MouseMove += new MouseEventHandler(MouseMovePanel);
            //panel.Click += new EventHandler(func);
            // pictureBox25
            using (Bitmap bmp = new Bitmap(baiDang.anhBia))
            {
                PictureBox anhBia = new PictureBox
                {
                    Image = GUI_Utils.Instance.Resize(bmp, pictureBox25.Size),
                    BackColor = Color.White,
                    Size = pictureBox25.Size,
                    Location = pictureBox25.Location,
                    Cursor = Cursors.Hand,
                    BorderStyle = BorderStyle.None,
                    Parent = panel,
                    SizeMode = PictureBoxSizeMode.Zoom
                };
                anhBia.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
                anhBia.MouseHover += new EventHandler(MouseHoverObjInPanel);
                anhBia.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
                anhBia.Click += new EventHandler(func);
                panel.Controls.Add(anhBia);
            }
            // textBox72 tieuDe

            TextBox tieuDe = new TextBox
            {
                Multiline = true,
                Text = baiDang.tieuDe,
                Size = textBox72.Size,
                Location = textBox72.Location,
                BackColor = Color.White,
                Font = textBox72.Font,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.Black,
                Parent = panel,
                ReadOnly = true
            };
            tieuDe.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
            tieuDe.MouseHover += new EventHandler(MouseHoverObjInPanel);
            tieuDe.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
            tieuDe.Click += new EventHandler(func);
            panel.Controls.Add(tieuDe);

            FlowLayoutPanel giaFLP = new FlowLayoutPanel
            {
                Size = flowLayoutPanel9.Size,
                Location = flowLayoutPanel9.Location,
                Cursor = Cursors.Hand,
                BackColor = Color.White,
                Parent = panel,
            };
            giaFLP.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
            giaFLP.MouseHover += new EventHandler(MouseHoverObjInPanel);
            giaFLP.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
            giaFLP.Click += new EventHandler(func);
            panel.Controls.Add(giaFLP);

            TextBox giaBan = new TextBox
            {
                Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(baiDang.giaMin(), baiDang.giamGia)),
                Size = textBox74.Size,
                Location = textBox74.Location,
                BackColor = Color.White,
                Font = textBox74.Font,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.Red,
                Parent = giaFLP,
                ReadOnly = true
            };
            giaBan.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
            giaBan.MouseHover += new EventHandler(MouseHoverObjInPanel);
            giaBan.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
            giaBan.Click += new EventHandler(func);
            giaFLP.Controls.Add(giaBan);
            GUI_Utils.Instance.FitTextBox(giaBan, 0, 0);

            TextBox giaGoc = new TextBox
            {
                Text = "₫" + Utils.Instance.SetGia(baiDang.giaMin()),
                Size = textBox73.Size,
                Location = textBox73.Location,
                BackColor = Color.White,
                Font = textBox73.Font,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.Black,
                Parent = giaFLP,
                ReadOnly = true
            };
            GUI_Utils.Instance.FitTextBox(giaGoc, 0, 0);
            giaGoc.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
            giaGoc.MouseHover += new EventHandler(MouseHoverObjInPanel);
            giaGoc.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
            giaGoc.Click += new EventHandler(func);
            giaFLP.Controls.Add(giaGoc);

            TextBox giamGia = new TextBox
            {
                Text = "-" + baiDang.giamGia + "%",
                Size = textBox75.Size,
                Location = textBox75.Location,
                BackColor = Color.MistyRose,
                Font = textBox75.Font,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.Red,
                Parent = giaFLP,
                ReadOnly = true
            };
            GUI_Utils.Instance.FitTextBox(giamGia, 0, 0);
            giamGia.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
            giamGia.MouseHover += new EventHandler(MouseHoverObjInPanel);
            giamGia.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
            giamGia.Click += new EventHandler(func);
            giaFLP.Controls.Add(giamGia);

            PictureBox sao = new PictureBox
            {
                Image = GUI_Utils.Instance.CreateStarRatingPictureBox(baiDang.tinhSao(), 5, pictureBox26.Height, 1).Image,
                BackColor = Color.White,
                Size = pictureBox26.Size,
                Location = pictureBox26.Location,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                Parent = panel,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            sao.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
            sao.MouseHover += new EventHandler(MouseHoverObjInPanel);
            sao.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
            sao.Click += new EventHandler(func);
            panel.Controls.Add(sao);

            TextBox daBan = new TextBox
            {
                Text = "Đã bán " + baiDang.luocBan().ToString(),
                Size = textBox76.Size,
                Location = textBox76.Location,
                BackColor = Color.White,
                Font = textBox76.Font,
                Cursor = Cursors.Hand,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true
            };
            daBan.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
            daBan.MouseHover += new EventHandler(MouseHoverObjInPanel);
            daBan.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
            daBan.Click += new EventHandler(func);
            panel.Controls.Add(daBan);

            return panel;
        }

        private void NextPageHomeButton_Click(object sender, EventArgs e)
        {
            int n = CurrPageNumber(PageListFLP);
            if (n++ * (nRowBaiDang_Home * nColBaiDang_Home) >= qlBaiDang.list.Count) return;

            HomePanel.AutoScrollPosition = new Point(39, 790);
            ChuyenTrang(FLPBaiDang1, qlBaiDang, n, DenBaiDangTuHome, nRowBaiDang_Home, nColBaiDang_Home);
            SetPageFLP(PageListFLP, n, (qlBaiDang.list.Count - 1) / (nRowBaiDang_Home * nColBaiDang_Home) + 1, SelectPageHomeButton_Click);
        }

        private void PrevPageHomeButton_Click(object sender, EventArgs e)
        {
            int n = CurrPageNumber(PageListFLP);
            if (n-- == 1) return;

            HomePanel.AutoScrollPosition = new Point(39, 790);
            ChuyenTrang(FLPBaiDang1, qlBaiDang, n, DenBaiDangTuHome, nRowBaiDang_Home, nColBaiDang_Home);
            SetPageFLP(PageListFLP, n, (qlBaiDang.list.Count - 1) / (nRowBaiDang_Home * nColBaiDang_Home) + 1, SelectPageHomeButton_Click);
        }

        private void SelectPageHomeButton_Click(object sender, EventArgs e)
        {
            int selectedPage = int.Parse(((Control)sender).Text);
            if (selectedPage == CurrPageNumber(PageListFLP)) return;

            HomePanel.AutoScrollPosition = new Point(39, 790);
            ChuyenTrang(FLPBaiDang1, qlBaiDang, selectedPage, DenBaiDangTuHome, nRowBaiDang_Home, nColBaiDang_Home);
            SetPageFLP(PageListFLP, selectedPage, (qlBaiDang.list.Count - 1) / (nRowBaiDang_Home * nColBaiDang_Home) + 1, SelectPageHomeButton_Click);
        }

        private void NextPageBaiDangButton_Click(object sender, EventArgs e)
        {
            int n = CurrPageNumber(PageListBDFLP);
            if (n++ * (nRowBaiDang_BaiDang * nColBaiDang_BaiDang) >= qlBaiDang.list.Count) return;

            BaiDangPanel.AutoScrollPosition = textBox51.Location;
            ChuyenTrang(coTheBanCungThichFLP, qlBaiDang, n, DenBaiDangTuBD, nRowBaiDang_BaiDang, nColBaiDang_BaiDang);
            SetPageFLP(PageListBDFLP, n, (qlBaiDang.list.Count - 1) / (nRowBaiDang_BaiDang * nColBaiDang_BaiDang) + 1, SelectPageBaiDangButton_Click);
        }

        private void PrevPageBaiDangButton_Click(object sender, EventArgs e)
        {
            int n = CurrPageNumber(PageListBDFLP);
            if (n-- == 1) return;

            BaiDangPanel.AutoScrollPosition = textBox51.Location;
            ChuyenTrang(coTheBanCungThichFLP, qlBaiDang, n, DenBaiDangTuBD, nRowBaiDang_BaiDang, nColBaiDang_BaiDang);
            SetPageFLP(PageListBDFLP, n, (qlBaiDang.list.Count - 1) / (nRowBaiDang_BaiDang * nColBaiDang_BaiDang) + 1, SelectPageBaiDangButton_Click);
        }
        private void SelectPageBaiDangButton_Click(object sender, EventArgs e)
        {
            int selectedPage = int.Parse(((Control)sender).Text);
            if (selectedPage == CurrPageNumber(PageListBDFLP)) return;

            BaiDangPanel.AutoScrollPosition = textBox51.Location;
            ChuyenTrang(coTheBanCungThichFLP, qlBaiDang, selectedPage, DenBaiDangTuBD, nRowBaiDang_BaiDang, nColBaiDang_BaiDang);
            SetPageFLP(PageListBDFLP, selectedPage, (qlBaiDang.list.Count - 1) / (nColBaiDang_BaiDang * nColBaiDang_BaiDang) + 1, SelectPageBaiDangButton_Click);
        }

        private void NextPageTimKiemButton_Click(object sender, EventArgs e)
        {
            int n = CurrPageNumber(listPageTimKiemFLP);
            if (n++ * (nRowBaiDang_TimKiem * nColBaiDang_TimKiem) > qlBaiDang.list.Count) return;

            BaiDangPanel.AutoScrollPosition = controlTimKiemPanel.Location;
            ChuyenTrang(listBDInTimKiemFLP, qlBaiDang, n, DenBaiDangTuTimKiem, nRowBaiDang_TimKiem, nColBaiDang_TimKiem);
            SetPageFLP(listPageTimKiemFLP, n, (qlBaiDang.list.Count - 1) / (nRowBaiDang_TimKiem * nColBaiDang_TimKiem) + 1, SelectPageTimKiemButton_Click);
        }

        private void PrevPageTimKiemButton_Click(object sender, EventArgs e)
        {
            int n = CurrPageNumber(listPageTimKiemFLP);
            if (n-- == 1) return;

            timKiemPanel.AutoScrollPosition = controlTimKiemPanel.Location;
            ChuyenTrang(listBDInTimKiemFLP, qlBaiDang, n, DenBaiDangTuTimKiem, nRowBaiDang_TimKiem, nColBaiDang_TimKiem);
            SetPageFLP(listPageTimKiemFLP, n, (qlBaiDang.list.Count - 1) / (nRowBaiDang_TimKiem * nColBaiDang_TimKiem) + 1, SelectPageTimKiemButton_Click);
        }
        private void SelectPageTimKiemButton_Click(object sender, EventArgs e)
        {
            int selectedPage = int.Parse(((Control)sender).Text);
            if (selectedPage == CurrPageNumber(listPageTimKiemFLP)) return;

            timKiemPanel.AutoScrollPosition = controlTimKiemPanel.Location;
            ChuyenTrang(listBDInTimKiemFLP, qlBaiDang, selectedPage, DenBaiDangTuTimKiem, nRowBaiDang_TimKiem, nColBaiDang_TimKiem);
            SetPageFLP(listPageTimKiemFLP, selectedPage, (qlBaiDang.list.Count - 1) / (nRowBaiDang_TimKiem * nColBaiDang_TimKiem) + 1, SelectPageTimKiemButton_Click);
        }

        private void NextPageShopButton_Click(object sender, EventArgs e)
        {
            int n = CurrPageNumber(listPageInShopFLP);
            if (n++ * (nRowBaiDang_Shop * nColBaiDang_Shop) > qlBaiDang.list.Count) return;

            BaiDangPanel.AutoScrollPosition = panel34.Location;
            ChuyenTrang(listBDCuaShopFLP, qlBaiDang, n, DenBaiDangTuShop, nRowBaiDang_Shop, nColBaiDang_Shop);
            SetPageFLP(listPageInShopFLP, n, (qlBaiDang.list.Count - 1) / (nRowBaiDang_Shop * nColBaiDang_Shop) + 1, SelectPageShopButton_Click);
        }

        private void PrevPageShopButton_Click(object sender, EventArgs e)
        {
            int n = CurrPageNumber(listPageInShopFLP);
            if (n-- == 1) return;

            timKiemPanel.AutoScrollPosition = panel34.Location;
            ChuyenTrang(listBDCuaShopFLP, qlBaiDang, n, DenBaiDangTuShop, nRowBaiDang_Shop, nColBaiDang_Shop);
            SetPageFLP(listPageInShopFLP, n, (qlBaiDang.list.Count - 1) / (nRowBaiDang_Shop * nColBaiDang_Shop) + 1, SelectPageShopButton_Click);
        }
        private void SelectPageShopButton_Click(object sender, EventArgs e)
        {
            int selectedPage = int.Parse(((Control)sender).Text);
            if (selectedPage == CurrPageNumber(listPageInShopFLP)) return;

            timKiemPanel.AutoScrollPosition = panel34.Location;
            ChuyenTrang(listBDCuaShopFLP, qlBaiDang, selectedPage, DenBaiDangTuShop, nRowBaiDang_Shop, nColBaiDang_Shop);
            SetPageFLP(listPageInShopFLP, selectedPage, (qlBaiDang.list.Count - 1) / (nRowBaiDang_Shop * nColBaiDang_Shop) + 1, SelectPageShopButton_Click);
        }

        private void NextDGPageButton_Click(object sender, EventArgs e)
        {
            int n = CurrPageNumber(listPageDanhGia);
            if (n++ * nDanhGiaInBaiDang > currBaiDang.listDanhGia.SoluongDanhGia(currSao)) return;

            BaiDangPanel.AutoScrollPosition = DGPanelInBDPanel.Location;
            ChuyenTrangDanhGia(n);
            SetPageFLP(listPageDanhGia, n, (currBaiDang.listDanhGia.SoluongDanhGia(currSao) - 1) / nDanhGiaInBaiDang + 1, SelectDGPage_Click);
        }
        private void PrevDGPageButton_Click(object sender, EventArgs e)
        {
            int n = CurrPageNumber(listPageDanhGia);
            if (n-- == 1) return;

            BaiDangPanel.AutoScrollPosition = DGPanelInBDPanel.Location;
            ChuyenTrangDanhGia(n);
            SetPageFLP(listPageDanhGia, n, (currBaiDang.listDanhGia.SoluongDanhGia(currSao) - 1) / nDanhGiaInBaiDang + 1, SelectDGPage_Click);
        }

        private void SelectDGPage_Click(object sender, EventArgs e)
        {
            int selectedPage = int.Parse(((Control)sender).Text);
            if (selectedPage == CurrPageNumber(listPageDanhGia)) return;

            BaiDangPanel.AutoScrollPosition = DGPanelInBDPanel.Location;
            ChuyenTrangDanhGia(selectedPage);
            SetPageFLP(listPageDanhGia, selectedPage, (currBaiDang.listDanhGia.SoluongDanhGia(currSao) - 1) / nDanhGiaInBaiDang + 1, SelectDGPage_Click);
        }

        private void ChuyenTrangDanhGia(int page)
        {
            ListDGInBDPanel.Controls.Clear();
            ListDGInBDPanel.Visible = false;
            DGPanelInBDPanel.Height = 230;
            GC.Collect();
            int startIndex = nDanhGiaInBaiDang * (page - 1);

            for(int i = 0, j = 0, c = 0; i < currBaiDang.listDanhGia.list.Count && j < nDanhGiaInBaiDang; i++)
            {
                if (currSao == - 1 || currBaiDang.listDanhGia.list[i].sao == currSao)
                {
                    if (c == startIndex)
                    {
                        ListDGInBDPanel.Controls.Add(DrawDanhGiaInBaiDang(currBaiDang.listDanhGia.list[i]));
                        j++;
                    }
                    else
                    {
                        c++;
                    }
                }
            }

            GUI_Utils.Instance.FitFLPHeight(ListDGInBDPanel);
            GUI_Utils.Instance.FitFLPHeight(listDanhGiaFLP);
            DGPanelInBDPanel.Height = listDanhGiaFLP.Height + 230;
            ListDGInBDPanel.Visible = true;
        }

        private void increaseButton_Click(object sender, EventArgs e)
        {
            if (currSanPham != null && int.Parse(soLuongTxt.Text) < currSanPham.soLuong)
            {
                soLuongTxt.Text = (int.Parse(soLuongTxt.Text) + 1).ToString();
            }
        }

        private void decreaseButton_Click(object sender, EventArgs e)
        {
            if (int.Parse(soLuongTxt.Text) > 1/**/)
            {
                soLuongTxt.Text = (int.Parse(soLuongTxt.Text) - 1).ToString();
            }
        }

        private void RefreshSanPhamInBaiDang(SanPham sanPham)
        {
            giaGocTxt.Text = "₫" + Utils.Instance.SetGia(sanPham.gia);
            giaTxt.Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(sanPham.gia, currBaiDang.giamGia));
            GUI_Utils.Instance.FitTextBox(giaGocTxt, 20);
            GUI_Utils.Instance.FitTextBox(giaTxt, 20);
            currImage.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(sanPham.anh), new Size(450, 450));
            soLuongSanCoTxt.Text = sanPham.soLuong.ToString() + " sản phẩm có sẵn";
            soLuongTxt.Text = "1";
        }

        private void PictureBoxToCircle_Paint(object sender, PaintEventArgs e)
        {
            GUI_Utils.Instance.PictureBoxToCircle_Paint(sender, e);
        }

        private void DenBaiDangTuBDShop(object sender, EventArgs e)
        {
            int index = spKhacCuaShopFLP.Controls.IndexOf(((Control)sender).Parent);
            currBaiDang = currShop.listBaiDang.list[index];
            currSanPham = null;
            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        public void SanPham_MouseHover(object sender, EventArgs e)
        {
            Button obj = sender as Button;
            int index = listItemFLP.Controls.IndexOf(obj);

            currImage.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(currBaiDang.list[index].anh), new Size(450, 450));
            obj.FlatAppearance.BorderColor = Color.OrangeRed;
            obj.ForeColor = Color.OrangeRed;
        }

        public void SanPham_MouseMove(object sender, MouseEventArgs e)
        {
            Button obj = sender as Button;

            if (obj.ForeColor == Color.OrangeRed)
                return;

            int index = listItemFLP.Controls.IndexOf(obj);

            currImage.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(currBaiDang.list[index].anh), new Size(450, 450));
            obj.FlatAppearance.BorderColor = Color.OrangeRed;
            obj.ForeColor = Color.OrangeRed;
        }

        public void SanPham_MouseLeave(object sender, EventArgs e)
        {
            Button obj = sender as Button;
            if (currSanPham != null)
            {
                currImage.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(currSanPham.anh), new Size(450, 450));
                if (currSanPham.ten != obj.Text)
                {
                    obj.FlatAppearance.BorderColor = Color.LightGray;
                    obj.ForeColor = Color.Black;
                }
            }
            else
            {
                currImage.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(currBaiDang.anhBia), new Size(450, 450));
                obj.FlatAppearance.BorderColor = Color.LightGray;
                obj.ForeColor = Color.Black;
            }
        }

        public void SanPham_Click(object sender, EventArgs e)
        {
            Button obj = sender as Button;

            foreach (Button button in listItemFLP.Controls)
            {
                if (button.Enabled == false)
                    continue;
                button.FlatAppearance.BorderColor = Color.LightGray;
                button.ForeColor = Color.Black;
            }

            if (currSanPham != null && currSanPham.ten == obj.Text)
            {
                currImage.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(currBaiDang.anhBia), currImage.Size);
                if (currBaiDang.giaMin() != currBaiDang.giaMax())
                {
                    giaGocTxt.Text = "₫" + Utils.Instance.SetGia(currBaiDang.giaMin()) + " - ₫" + Utils.Instance.SetGia(currBaiDang.giaMax());
                    giaTxt.Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(currBaiDang.giaMin(), currBaiDang.giamGia)) + " - ₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(currBaiDang.giaMax(), currBaiDang.giamGia));
                }
                else
                {
                    giaGocTxt.Text = "₫" + Utils.Instance.SetGia(currBaiDang.giaMin());
                    giaTxt.Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(currBaiDang.giaMin(), currBaiDang.giamGia));
                }
                GUI_Utils.Instance.FitTextBox(giaGocTxt, 20);
                GUI_Utils.Instance.FitTextBox(giaTxt, 20);
                obj.FlatAppearance.BorderColor = Color.Black;
                obj.ForeColor = Color.Black;

                currSanPham = null;
            }
            else
            {
                obj.FlatAppearance.BorderColor = Color.OrangeRed;
                obj.ForeColor = Color.OrangeRed;

                foreach (SanPham item in currBaiDang.list)
                {
                    if (item.ten == obj.Text)
                    {
                        RefreshSanPhamInBaiDang(item);
                        currSanPham = item;
                        break;
                    }
                }
            }
        }

        private void soLuongTxt_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(soLuongTxt.Text, out int soLuong) && soLuongTxt.Text.Length != 0)
            {
                soLuongTxt.Text = soLuongTxt.Text.Substring(0, soLuongTxt.Text.Length - 1);
                soLuongTxt.SelectionStart = soLuongTxt.Text.Length;
                return;
            }

            if (soLuongTxt.Text.Length == 0)
                return;

            if (currSanPham != null && soLuong > currSanPham.soLuong)
            {
                soLuongTxt.Text = currSanPham.soLuong.ToString();
            }
            if (soLuong < 1)
            {
                soLuongTxt.Text = "1";
            }
            soLuongTxt.SelectionStart = soLuongTxt.Text.Length;
        }

        private void FollowButton_Click(object sender, EventArgs e)
        {
            if (khachHang == null)
            {
                GoToLoginForm();
                SwitchPanel(ref currPanel, ref BaiDangPanel);
                Show();
            }

            Button button = sender as Button;

            if (!BLL_KhachHang.Instance.DaTheoDoi(khachHang.listFollow, currShop.maSo))
            {
                BLL_KhachHang.Instance.TheoDoi(khachHang, currShop);
                button.Text = "Đang theo dõi";
            }
            else
            {
                BLL_KhachHang.Instance.HuyTheoDoi(khachHang, currShop);
                button.Text = "Theo dõi";
            }
        }

        private void FollowInBaiDang(object sender, EventArgs e)
        {
            FollowButton_Click(sender, e);
            nTheoDoiTxt.Text = currShop.listFollower.Count.ToString();
        }

        private void thichButton_Click(object sender, EventArgs e)
        {
            if (khachHang == null)
            {
                GoToLoginForm();
                SwitchPanel(ref currPanel, ref BaiDangPanel);
                Show();
            }
            Button obj = sender as Button;
            if (BLL_KhachHang.Instance.DaThich(khachHang.listThich, currBaiDang.maBD))
            {
                BLL_KhachHang.Instance.HuyThich(khachHang, currBaiDang);
                obj.Image = Resources.heart2;
            }
            else
            {
                BLL_KhachHang.Instance.Thich(khachHang, currBaiDang);
                obj.Image = Resources.heart1;
            }
            obj.Text = "Đã thích (" + currBaiDang.luocThich.ToString() + ")";
        }

        private void GoToLoginForm()
        {
            Hide();
            DangNhap_Form form = new DangNhap_Form(SetData, "KhachHang");
            form.ShowDialog();
            Show();
        }

        private void addToCartButton_Click(object sender, EventArgs e)
        {
            if (currSanPham == null)
                return;

            if (khachHang != null)
            {
                BLL_GioHang.Instance.ThemSPVaoGioHang(khachHang.gioHang, currSanPham, int.Parse(soLuongTxt.Text));
                RefreshCartButton();
                ThongBaoForm form = new ThongBaoForm("Sản phẩm đã được thêm vào giỏ hàng");
                form.Show();
            }
            else
            {
                GoToLoginForm();
                SwitchPanel(ref currPanel, ref BaiDangPanel);
                Show();
            }
        }

        private void userProfile_Button_MouseHover(object sender, EventArgs e)
        {
            littleMenuPanel.Visible = true;
            littleMenuPanel.BringToFront();
            userProfile_Button.Text = userProfile_Button.Text.Replace("◀", "◣");
        }

        private void userProfile_Button_MouseLeave(object sender, EventArgs e)
        {
            if (Cursor.Position.Y < 35 || (Cursor.Position.X < 1273 || Cursor.Position.X > 1440))
            {
                littleMenuPanel.Visible = false;
                userProfile_Button.Text = userProfile_Button.Text.Replace("◣", "◀");

            }
        }

        private void littleMenuPanel_MouseLeave(object sender, EventArgs e)
        {
            if (Cursor.Position.X > 1275 && Cursor.Position.X < 1450 && Cursor.Position.Y > 50 && Cursor.Position.Y < 185)
                return;
            littleMenuPanel.Visible = false;
            userProfile_Button.Text = userProfile_Button.Text.Replace("◣", "◀");
        }

        private void gioHangPanel_Scroll(object sender, ScrollEventArgs e)
        {
            noiDungPanel.Top = 715;
        }

        private void HomePanel_Scroll(object sender, ScrollEventArgs e)
        {
            HeaderPannel.Top = 0;
        }

        private void GioHangPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if (noiDungPanel.Top - e.Delta > panel13.Top)
                noiDungPanel.Top = panel13.Top;
            else
                noiDungPanel.Top = 715;
        }

        private void cartButton_Click(object sender, EventArgs e)
        {
            while ((khachHang == null))
            {
                GoToLoginForm();
                SwitchPanel(ref currPanel, ref BaiDangPanel);
                Show();
            }
            SwitchPanel(ref currPanel, ref gioHangPanel);
        }

        private void DrawPanelBorder(object sender, PaintEventArgs e)
        {
            Control panel = sender as Control;

            using (var pen = new Pen(Color.DarkGray, 1))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(-1, -1, panel.Width + 2, panel.Height));
            }
        }

        private Panel DrawSPTrongGioHang(SanPham sanPham)
        {
            string tinhTrangSP = BLL_SanPham.Instance.KiemTraTinhTrang(sanPham.maSP);

            int giamGia = BLL_BaiDang.Instance.GetGiamGiaFromMaSP(sanPham.maSP);
            Panel panel = new Panel
            {
                Size = spPanel.Size,
                BackColor = Color.White,
                Margin = spPanel.Margin,
                Parent = listSPTrongGHFLP
            };
            panel.Paint += DrawPanelBorder;

            Label maSP = new Label
            {
                Name = "maSP",
                Text = sanPham.maSP,
                Visible = false,
                Parent = panel
            };
            panel.Controls.Add(maSP);

            PictureBox pictureBox = new PictureBox
            {
                Name = "anhSanPham",
                Location = pictureBox9.Location,
                Size = pictureBox9.Size,
                Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(sanPham.anh), pictureBox9.Size),
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand,
                Parent = panel
            };
            pictureBox.Click += DenBaiDangTuGioHang;
            panel.Controls.Add(pictureBox);


            TextBox tenSP = new TextBox
            {
                Name = "TenSanPham",
                Text = $"Sách - {sanPham.ten}({sanPham.tacGia})",
                Multiline = true,
                Size = textBox15.Size,
                BackColor = Color.White,
                Location = textBox15.Location,
                Font = textBox15.Font,
                BorderStyle = BorderStyle.None,
                Cursor = Cursors.Hand,
                ReadOnly = true,
                Parent = panel
            };
            tenSP.Click += DenBaiDangTuGioHang;
            panel.Controls.Add(tenSP);

            TextBox giaGoc = new TextBox
            {
                Name = "giaGocSP",
                Text = "₫" + Utils.Instance.SetGia(sanPham.gia),
                BackColor = Color.White,
                ForeColor = Color.DarkGray,
                Size = textBox16.Size,
                Location = textBox16.Location,
                Font = textBox16.Font,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(giaGoc);
            
            TextBox giaBan = new TextBox
            {
                Name = "giaBan",
                Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(sanPham.gia, giamGia)),
                BackColor = Color.White,
                Size = textBox21.Size,
                Location = textBox21.Location,
                Font = textBox21.Font,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(giaBan);
            
            TextBox soLuong = new TextBox
            {
                Name = "soluongSP",
                Text = sanPham.soLuong.ToString(),
                BackColor = Color.White,
                Font = SLText.Font,
                Size = SLText.Size,
                Location = SLText.Location,
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = HorizontalAlignment.Center,
                Parent = panel
            };
            soLuong.TextChanged += SLText_TextChanged;
            panel.Controls.Add(soLuong);

            TextBox soTien = new TextBox
            {
                Name = "soTien",
                Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(sanPham.gia, giamGia) * int.Parse(soLuong.Text)),
                ForeColor = Color.OrangeRed,
                BackColor = Color.White,
                Font = textBox24.Font,
                Size = textBox24.Size,
                Location = textBox24.Location,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(soTien);
            
            Button xoa = new Button
            {
                Name = "xoa",
                Text = "Xóa",
                Font = xoaSPKhoiGHButton.Font,
                Size = xoaSPKhoiGHButton.Size,
                Location = xoaSPKhoiGHButton.Location,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Parent = panel
            };
            xoa.Click += xoaSPKhoiGHButton_Click;
            xoa.FlatAppearance.BorderSize = 0;
            xoa.FlatAppearance.MouseOverBackColor = Color.Transparent;
            xoa.FlatAppearance.MouseDownBackColor = Color.Transparent;
            panel.Controls.Add(xoa);

            if (tinhTrangSP == "TỐT")
            {
                CheckBox checkBox = new CheckBox
                {
                    Name = "ChoseCheckBox",
                    Location = checkBox1.Location,
                    Text = checkBox1.Text,
                    Size = checkBox1.Size,
                    Cursor = Cursors.Hand,
                    Parent = panel
                };
                checkBox.CheckedChanged += CB_CheckChanged;
                panel.Controls.Add(checkBox);

                Button giamButton = new Button
                {
                    Name = "giamButton",
                    Text = giamSLButton.Text,
                    Size = giamSLButton.Size,
                    Location = giamSLButton.Location,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Parent = panel
                };
                giamButton.FlatAppearance.BorderSize = 1;
                giamButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
                giamButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                giamButton.Click += giamSLButton_Click;
                panel.Controls.Add(giamButton);

                Button tangButton = new Button
                {
                    Name = "tangButton",
                    Text = tangSLButton.Text,
                    Size = tangSLButton.Size,
                    Location = tangSLButton.Location,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Parent = panel
                };
                tangButton.FlatAppearance.BorderSize = 1;
                tangButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
                tangButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                tangButton.Click += tangSLButton_Click;
                panel.Controls.Add(tangButton);
            }
            else
            {
                giaGoc.Enabled = false;
                giaBan.Enabled = false;
                soLuong.Enabled = false;
                soTien.Enabled = false;

                TextBox tinhTrang = new TextBox
                {
                    Text = tinhTrangSP,
                    Location = textBox117.Location,
                    Font = textBox117.Font,
                    Size = textBox117.Size,
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.Red,
                    BorderStyle = BorderStyle.None,
                    ReadOnly = true,
                    Parent = panel
                };
                GUI_Utils.Instance.FitTextBox(tinhTrang, 0, 0);
                panel.Controls.Add(tinhTrang);

                pictureBox.Location = new Point(pictureBox.Location.X + 50, pictureBox.Location.Y);
                tenSP.Location = new Point(tenSP.Location.X + 50, tenSP.Location.Y);
                soLuong.BorderStyle = BorderStyle.None;
            }

            return panel;
        }

        private void giamSLButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Panel panel = button.Parent as Panel;

            int soLuong = int.Parse(((TextBox)GUI_Utils.Instance.FindControl(panel, "soluongSP")).Text);
            int index = listSPTrongGHFLP.Controls.IndexOf(panel);

            if (soLuong == 1)
            {
                DialogResult result = MessageBox.Show("Bạn có muốn xóa sản phầm này khỏi giỏ hàng không?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    xoaSPKhoiGHButton_Click(GUI_Utils.Instance.FindControl(panel, "xoa"), e);
            }
            else
            {
                ((TextBox)GUI_Utils.Instance.FindControl(panel, "soluongSP")).Text = (soLuong - 1).ToString();
                khachHang.CapNhatSoLuongSPTrongGH(index, soLuong - 1);
                ((TextBox)GUI_Utils.Instance.FindControl(panel, "soTien")).Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(khachHang.gioHang.list[index].gia * (soLuong - 1), BLL_BaiDang.Instance.GetGiamGiaFromMaSP(khachHang.gioHang.list[index].maSP)));
            }

            tongTienTxt.Text = "₫" + Utils.Instance.SetGia(qlSanPham.tinhTongTien());
            tietKiemTxt.Text = "₫" + Utils.Instance.SetGia(qlSanPham.tinhTongTien(false) - qlSanPham.tinhTongTien());
            GUI_Utils.Instance.FitTextBox(tietKiemTxt, 20);
            GUI_Utils.Instance.FitTextBox(tongTienTxt, 20);
        }

        private void tangSLButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Panel panel = button.Parent as Panel;

            int soLuong = int.Parse(((TextBox)GUI_Utils.Instance.FindControl(panel, "soluongSP")).Text);
            int index = listSPTrongGHFLP.Controls.IndexOf(panel);

            if (soLuong < BLL_SanPham.Instance.GetSoLuongFromMaSP(khachHang.gioHang.list[index].maSP))
            {
                ((TextBox)GUI_Utils.Instance.FindControl(panel, "soluongSP")).Text = (soLuong + 1).ToString();
                khachHang.CapNhatSoLuongSPTrongGH(index, soLuong + 1);
                ((TextBox)GUI_Utils.Instance.FindControl(panel, "soTien")).Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(khachHang.gioHang.list[index].gia * (soLuong + 1), BLL_BaiDang.Instance.GetGiamGiaFromMaSP(khachHang.gioHang.list[index].maSP)));
            }

            tongTienTxt.Text = "₫" + Utils.Instance.SetGia(qlSanPham.tinhTongTien());
            tietKiemTxt.Text = "₫" + Utils.Instance.SetGia(qlSanPham.tinhTongTien(false) - qlSanPham.tinhTongTien());
            GUI_Utils.Instance.FitTextBox(tietKiemTxt, 20);
            GUI_Utils.Instance.FitTextBox(tongTienTxt, 20);
        }

        private void SLText_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Panel panel = textBox.Parent as Panel;
            int index = listSPTrongGHFLP.Controls.IndexOf(panel);

            if (!int.TryParse(textBox.Text, out int soLuong) && textBox.Text.Length != 0)
            {
                textBox.Text = textBox.Text.Substring(0, textBox.Text.Length - 1);
                textBox.SelectionStart = textBox.Text.Length;
                return;
            }

            int soLuongMax = BLL_SanPham.Instance.GetSoLuongFromMaSP(khachHang.gioHang.list[index].maSP);

            if (soLuong > soLuongMax)
            {
                textBox.Text = soLuongMax.ToString();
            }
            if (soLuong < 1)
            {
                textBox.Text = "1";
            }
            textBox.SelectionStart = textBox.Text.Length;
            khachHang.CapNhatSoLuongSPTrongGH(index, soLuong);
            ((TextBox)GUI_Utils.Instance.FindControl(panel, "soTien")).Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(khachHang.gioHang.list[index].gia * soLuong, BLL_BaiDang.Instance.GetGiamGiaFromMaSP(khachHang.gioHang.list[index].maSP)));

            tongTienTxt.Text = "₫" + Utils.Instance.SetGia(qlSanPham.tinhTongTien());
            tietKiemTxt.Text = "₫" + Utils.Instance.SetGia(qlSanPham.tinhTongTien(false) - qlSanPham.tinhTongTien());
            GUI_Utils.Instance.FitTextBox(tietKiemTxt, 20);
            GUI_Utils.Instance.FitTextBox(tongTienTxt, 20);
        }

        private void xoaSPKhoiGHButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Panel panel = button.Parent as Panel;

            int index = listSPTrongGHFLP.Controls.IndexOf(panel);

            if (GUI_Utils.Instance.FindControl(panel, "ChoseCheckBox") != null && ((CheckBox)GUI_Utils.Instance.FindControl(panel, "ChoseCheckBox")).Checked)
            {
                qlSanPham.Remove(khachHang.gioHang.list[index]);
            }

            BLL_GioHang.Instance.XoaSPKhoiGioHang(khachHang.gioHang, index);
            listSPTrongGHFLP.Controls.RemoveAt(index);
            tongTienTxt.Text = "₫" + Utils.Instance.SetGia(qlSanPham.tinhTongTien());
            tietKiemTxt.Text = "₫" + Utils.Instance.SetGia(qlSanPham.tinhTongTien(false) - qlSanPham.tinhTongTien());
            GUI_Utils.Instance.FitTextBox(tietKiemTxt, 20);
            GUI_Utils.Instance.FitTextBox(tongTienTxt, 20);
            RefreshCartButton();
        }

        private void CB_CheckChanged(object sender, EventArgs e)
        {
            CheckBox obj = sender as CheckBox;
            Panel panel = obj.Parent as Panel;

            int index = listSPTrongGHFLP.Controls.IndexOf(panel);

            if (obj.Checked)
            {
                qlSanPham.Add(khachHang.gioHang.list[index]);
            }
            else
            {
                qlSanPham.Remove(khachHang.gioHang.list[index]);
            }
            soLuongSPTTTxt.Text = $"Tổng thanh toán({qlSanPham.list.Count} sản phẩm)";
            tongTienTxt.Text = "₫" + Utils.Instance.SetGia(qlSanPham.tinhTongTien());
            tietKiemTxt.Text = "₫" + Utils.Instance.SetGia(qlSanPham.tinhTongTien(false) - qlSanPham.tinhTongTien());
            GUI_Utils.Instance.FitTextBox(tietKiemTxt, 20);
            GUI_Utils.Instance.FitTextBox(soLuongSPTTTxt, 20);
            GUI_Utils.Instance.FitTextBox(tongTienTxt, 20);
        }

        private void chooseAllButton_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Panel panel in listSPTrongGHFLP.Controls)
            {
                if(GUI_Utils.Instance.FindControl(panel, "ChoseCheckBox") != null)
                    ((CheckBox)GUI_Utils.Instance.FindControl(panel, "ChoseCheckBox")).Checked = chooseAllButton.Checked;
            }
        }

        private Panel DrawSanPhamDonHang(FlowLayoutPanel parent, SanPham sanPham)
        {
            Panel panel = new Panel
            {
                Size = panel17.Size,
                Margin = panel17.Margin,
                BackColor = Color.White,
                Parent = parent
            };

            PictureBox pictureBox = new PictureBox
            {
                Name = "anhSP",
                Size = pictureBox12.Size,
                Location = pictureBox12.Location,
                Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(sanPham.anh), pictureBox12.Size),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                Parent = panel
            };
            panel.Controls.Add(pictureBox);

            TextBox textBox1 = new TextBox
            {
                Text = $"Sách - {sanPham.ten}({sanPham.tacGia})",
                Multiline = true,
                Size = textBox32.Size,
                Location = textBox32.Location,
                BackColor = Color.White,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(textBox1);

            int donGia = Utils.Instance.GiamGia(sanPham.gia, BLL_BaiDang.Instance.GetGiamGiaFromMaSP(sanPham.maSP));

            TextBox textBox2 = new TextBox
            {
                Name = "donGia",
                Text = "₫" + Utils.Instance.SetGia(donGia),
                Size = textBox36.Size,
                Location = textBox36.Location,
                BackColor = Color.White,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Center,
                Parent = panel
            };
            panel.Controls.Add(textBox2);

            TextBox textBox3 = new TextBox
            {
                Name = "soLuong",
                Text = sanPham.soLuong.ToString(),
                Size = textBox37.Size,
                Location = textBox37.Location,
                BackColor = Color.White,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Right,
                Parent = panel
            };
            panel.Controls.Add(textBox3);

            TextBox textBox4 = new TextBox
            {
                Name = "tongTien",
                Text = "₫" + Utils.Instance.SetGia(donGia * sanPham.soLuong),
                Size = textBox38.Size,
                Location = textBox38.Location,
                BackColor = Color.White,
                ForeColor = Color.OrangeRed,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Right,
                Parent = panel
            };
            panel.Controls.Add(textBox4);

            return panel;
        }

        private FlowLayoutPanel DrawFLPDonHang(QLSanPham list)
        {
            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                Parent = listDonHangFLP,
                BackColor = Color.White,
                Margin = flowLayoutPanel4.Margin
            };

            Panel headerPanel = new Panel
            {
                Name = "header",
                Size = panel16.Size,
                BackColor = Color.White,
                Margin = panel16.Margin,

            };

            TextBox textBox1 = new TextBox
            {
                Name = "sanPham",
                Font = textBox31.Font,
                Location = textBox31.Location,
                Text = textBox31.Text,
                Size = textBox31.Size,
                BackColor = Color.White,
                Cursor = Cursors.IBeam,
                ReadOnly = true,
                BorderStyle = BorderStyle.None
            };
            headerPanel.Controls.Add(textBox1);

            TextBox textBox2 = new TextBox
            {
                Name = "donGia",
                Font = textBox35.Font,
                Location = textBox35.Location,
                Text = textBox35.Text,
                Size = textBox35.Size,
                BackColor = Color.White,
                Cursor = Cursors.IBeam,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Center
            };
            headerPanel.Controls.Add(textBox2);

            TextBox textBox3 = new TextBox
            {
                Name = "soLuong",
                Font = textBox34.Font,
                Location = textBox34.Location,
                Text = textBox34.Text,
                Size = textBox34.Size,
                BackColor = Color.White,
                Cursor = Cursors.IBeam,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right
            };
            headerPanel.Controls.Add(textBox3);

            TextBox textBox4 = new TextBox
            {
                Name = "thanhTien",
                Font = textBox33.Font,
                Location = textBox33.Location,
                Text = textBox33.Text,
                Size = textBox33.Size,
                BackColor = Color.White,
                Cursor = Cursors.IBeam,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right
            };
            headerPanel.Controls.Add(textBox4);

            PictureBox pictureBox = new PictureBox
            {
                Size = pictureBox13.Size,
                Location = pictureBox13.Location,
                Image = pictureBox13.Image,

            };
            headerPanel.Controls.Add(pictureBox);

            TextBox tenShop = new TextBox
            {
                Name = "tenShop",
                Font = TxtTenShopDH.Font,
                Location = TxtTenShopDH.Location,
                Text = BLL_Shop.Instance.GetTenShopFromMaS(list.list[0].maS),
                Size = TxtTenShopDH.Size,
                BackColor = Color.White,
                Cursor = Cursors.IBeam,
                ReadOnly = true,
                BorderStyle = BorderStyle.None
            };
            headerPanel.Controls.Add(tenShop);

            flp.Controls.Add(headerPanel);

            foreach (SanPham sanPham in list.list)
            {
                flp.Controls.Add(DrawSanPhamDonHang(flp, sanPham));
            }

            Panel tailPanel = new Panel
            {
                Name = "tailPanel",
                Size = panel18.Size,
                Margin = panel18.Margin,
                BackColor = panel18.BackColor,
                Parent = flp
            };

            TextBox textBox5 = new TextBox
            {
                Name = "tongSoTien",
                Size = textBox39.Size,
                Location = textBox39.Location,
                Text = $"Tổng số tiền({list.list.Count} sản phẩm):",
                TextAlign = HorizontalAlignment.Right,
                BackColor = textBox39.BackColor,
                BorderStyle = BorderStyle.None,
                Parent = tailPanel
            };
            tailPanel.Controls.Add(textBox5);

            TextBox textBox6 = new TextBox
            {
                Name = "tongSoTienThanhToan",
                Size = textBox40.Size,
                Location = textBox40.Location,
                Text = "₫" + Utils.Instance.SetGia(list.tinhTongTien()),
                TextAlign = HorizontalAlignment.Right,
                BorderStyle = BorderStyle.None,
                BackColor = textBox40.BackColor,
                ForeColor = Color.OrangeRed,
                Parent = tailPanel
            };
            tailPanel.Controls.Add(textBox6);
            flp.Controls.Add(tailPanel);

            int height = 0;
            foreach (Control control in flp.Controls)
            {
                height += control.Height;
            }
            flp.Size = new Size(flowLayoutPanel4.Width, height);
            return flp;
        }

        private void muaHangButton_Click(object sender, EventArgs e)
        {
            if (qlSanPham.list.Count > 0)
            {
                SwitchPanel(ref currPanel, ref thanhToanPanel);

            }
            else
            {

            }
        }

        private void chonPTTT_Click(object sender, EventArgs e)
        {
            foreach (Button button in listPTTTFLP.Controls)
            {
                button.FlatAppearance.BorderColor = Color.DarkGray;
                button.ForeColor = Color.Black;
            }

            Button obj = sender as Button;
            obj.FlatAppearance.BorderColor = Color.OrangeRed;
            obj.ForeColor = Color.OrangeRed;
        }

        private void Enter_Luu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                luu_UP_Button_Click(sender, e);
            }
        }

        private void Enter_DoiMK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                xacNhan_UP_Button_Click(sender, e);
            }
        }

        private void muaNgayButton_Click(object sender, EventArgs e)
        {
            if (khachHang == null)
            {
                GoToLoginForm();
                SwitchPanel(ref currPanel, ref BaiDangPanel);
                Show();
            }

            if (currSanPham != null)
            {
                qlSanPham = new QLSanPham();
                qlSanPham.Add(currSanPham.Clone());
                qlSanPham.list[0].soLuong = int.Parse(soLuongTxt.Text);
                BLL_GioHang.Instance.ThemSPVaoGioHang(khachHang.gioHang, currSanPham, int.Parse(soLuongTxt.Text));
                RefreshCartButton();

                muaHangButton_Click(sender, e);
            }
        }

        public void ThayDoiDiaChiGiaoHang(DiaChi diaChi)
        {
            currDiaChi = diaChi;
        }

        private void DCNhanHangButton_Click(object sender, EventArgs e)
        {
            DimForm dimForm = new DimForm();
            dimForm.Show();
            DiaChiForm form = new DiaChiForm(ThemDiaChi, ThayDoiDiaChiGiaoHang, khachHang.diaChi, khachHang.listDiaChi.ToArray());
            form.ShowDialog();
            dimForm.Close();
            TextBox textBox = ((Control)sender).Parent.Controls[0] as TextBox;

            if (textBox.Name == "vanChuyenTxt")
                textBox.Text = BLL_DiaChi.Instance.MoTaDiaChi(currDiaChi);
            else
                textBox.Text = currDiaChi.ToString();

            GUI_Utils.Instance.FitTextBox(textBox);
        }

        private void datHangButton_Click(object sender, EventArgs e)
        {
            int ptThanhToan = 0; // 0: 
            if (TTKhiNhanHangButton.ForeColor == Color.OrangeRed)
                ptThanhToan = 0;
            else if (CKNganHangButton.ForeColor == Color.OrangeRed)
                ptThanhToan = 1;

            BLL_KhachHang.Instance.DatHang(khachHang, qlSanPham, currDiaChi, ptThanhToan, dungXuCB.Checked);
            thanhToanPanel.Visible = false;

            userProfile_Button_Click(null, null);
            DonMuaButton_Click(DonMuaButton, null);
            ChuyenLoaiDH_Click(DHChoXNButton, null);

            RefreshCartButton();

            ThongBaoForm form = new ThongBaoForm("Cảm ơn bạn đã đặt hàng!!!");
            form.Show();
        }

        private void danhGiaTxt_Click(object sender, EventArgs e)
        {
            BaiDangPanel.AutoScrollPosition = new Point(DGPanelInBDPanel.Location.X, DGPanelInBDPanel.Location.Y);
        }

        private void gioHangPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (gioHangPanel.Visible)
            {
                waittingPanel.BringToFront();
                waittingPanel.Visible = true;
                gioHangPanel.Controls.Add(HeaderPannel);

                chooseAllButton.Checked = false;
                tongTienTxt.Text = "₫0";
                tietKiemTxt.Text = "₫0";

                currDiaChi = khachHang.diaChi;
                qlSanPham = new QLSanPham();
                listSPTrongGHFLP.Controls.Clear();
                listSPTrongGHFLP.Size = new Size(listSPTrongGHFLP.Width, 20);
                chooseAllButton.Checked = false;

                Utils.Instance.Sort(khachHang.gioHang.list, 0, khachHang.gioHang.list.Count - 1, SanPham.CompareNgayThem, SanPham.EqualNgayThem);
                khachHang.gioHang.list.Reverse();


                List<Panel> listSP = new List<Panel>();
                foreach (SanPham sanPham in khachHang.gioHang.list)
                {
                    Panel panel = DrawSPTrongGioHang(sanPham);

                    if(GUI_Utils.Instance.FindControl(panel, "soluongSP").Enabled)
                        listSPTrongGHFLP.Controls.Add(panel);
                    else 
                        listSP.Add(panel);
                }
                listSPTrongGHFLP.Controls.AddRange(listSP.ToArray());   

                int height = 0;
                foreach (Control control in listSPTrongGHFLP.Controls)
                {
                    height += control.Height;
                }
                listSPTrongGHFLP.Size = new Size(listSPTrongGHFLP.Width, 20 + height);
                panel13.Location = new Point(panel13.Location.X, listSPTrongGHFLP.Bottom + 10);

                if (panel13.Top < 596)
                {
                    noiDungPanel.Top = panel13.Top;
                }
                else
                {
                    noiDungPanel.Top = 715;
                }
                waittingPanel.SendToBack();
            }
            else
            {
                waittingPanel.BringToFront();
                KhachHang_Panel.Controls.Add(HeaderPannel);
                HeaderPannel.Visible = true;
                HeaderPannel.BringToFront();
                waittingPanel.SendToBack();
            }
        }

        private void BaiDangPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (BaiDangPanel.Visible)
            {
                waittingPanel.BringToFront();
                waittingPanel.Visible = true;
                BaiDangPanel.Controls.Add(HeaderPannel);
                HeaderPannel.Location = new Point(0, 0);

                if (currBaiDang == null)
                {
                    baiDangFLP.Visible = false;
                    pictureBox53.Visible = true;
                    label14.Visible = true;
                    waittingPanel.SendToBack();
                    return;
                }

                baiDangFLP.Visible = true;
                pictureBox53.Visible = false;
                label14.Visible = false;

                if (khachHang != null)
                {
                    currDiaChi = khachHang.diaChi;
                    vanChuyenTxt.Text = BLL_DiaChi.Instance.MoTaDiaChi(currDiaChi);
                    GUI_Utils.Instance.FitTextBox(vanChuyenTxt);
                    BLL_KhachHang.Instance.XemBaiDang(khachHang, currBaiDang.maBD);

                    if (BLL_KhachHang.Instance.DaTheoDoi(khachHang.listFollow, currShop.maSo))
                        followButton.Text = "Hủy heo dõi";
                    else
                        followButton.Text = "Theo dõi";

                    if (BLL_KhachHang.Instance.DaThich(khachHang.listThich, currBaiDang.maBD))
                        thichButton.Image = Resources.heart2;
                    else
                        thichButton.Image = Resources.heart1;
                }
                else
                {
                    thichButton.Image = Resources.heart1;
                    followButton.Text = "Theo dõi";
                    vanChuyenTxt.Text = "Vui lòng đăng nhập để có thể đặt hàng";
                    GUI_Utils.Instance.FitTextBox(vanChuyenTxt);
                }

                titleTxt.Text = currBaiDang.tieuDe;
                GUI_Utils.Instance.FitTextBoxMultiLines(titleTxt);

                currImage.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(currBaiDang.anhBia), new Size(450, 450));
                daBanTxt.Text = currBaiDang.luocBan().ToString() + " Đã bán";
                danhGiaTxt.Text = currBaiDang.listDanhGia.list.Count.ToString() + " Đánh giá";

                if (!BLL_BaiDang.Instance.IsSamePrice(currBaiDang))
                {
                    giaGocTxt.Text = "₫" + Utils.Instance.SetGia(currBaiDang.giaMin()) + " - ₫" + Utils.Instance.SetGia(currBaiDang.giaMax());
                    giaTxt.Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(currBaiDang.giaMin(), currBaiDang.giamGia)) + " - ₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(currBaiDang.giaMax(), currBaiDang.giamGia));
                }
                else
                {
                    giaGocTxt.Text = "₫" + Utils.Instance.SetGia(currBaiDang.giaMin());
                    giaTxt.Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(currBaiDang.giaMin(), currBaiDang.giamGia));
                }
                GUI_Utils.Instance.FitTextBox(giaGocTxt, 20);
                GUI_Utils.Instance.FitTextBox(giaTxt, 20);

                thichButton.Text = "Đã thích (" + currBaiDang.luocThich.ToString() + ")";
                Font font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                listItemFLP.Controls.Clear();
                moTaBaiDangTxt.Text = "";
                int index = 1;
                foreach (SanPham item in currBaiDang.list)
                {
                    Size textSize = TextRenderer.MeasureText(item.ten, font);

                    using (Bitmap bmp = new Bitmap(item.anh))
                    {
                        Button button = new Button
                        {
                            Font = font,
                            Size = new Size(textSize.Width + 50, textSize.Height + 20),
                            Padding = new Padding(3, 3, 10, 3),
                            Text = item.ten,
                            FlatStyle = FlatStyle.Flat,
                            Cursor = Cursors.Hand,
                            TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                            Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(item.anh), new Size(textSize.Height + 10, textSize.Height + 10)),
                            ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
                        };

                        if (item.soLuong == 0)
                        {
                            button.Enabled = false;
                            button.Cursor = Cursors.No;
                            button.BackColor = Color.FromArgb(244, 244, 244);
                            button.FlatAppearance.BorderColor = Color.LightGray;
                        }
                        else
                        {
                            button.FlatAppearance.MouseOverBackColor = Color.Transparent;
                            button.FlatAppearance.MouseDownBackColor = Color.Transparent;
                            button.FlatAppearance.BorderColor = Color.LightGray;
                            button.Click += new EventHandler(SanPham_Click);
                            button.MouseHover += new EventHandler(SanPham_MouseHover);
                            button.MouseLeave += new EventHandler(SanPham_MouseLeave);
                            button.MouseMove += new MouseEventHandler(SanPham_MouseMove);
                        }
                        if (currSanPham != null && SanPham.EqualMaSP(item, currSanPham))
                        {
                            currSanPham = null;
                            SanPham_Click(button, null);
                        }

                        listItemFLP.Controls.Add(button);
                    }

                    moTaBaiDangTxt.Text += index.ToString() + "." + item.ToString() + "\r\n";
                    index++;
                }

                moTaBaiDangTxt.Text += currBaiDang.moTa;
                moTaBaiDangTxt.Size = new Size(moTaBaiDangTxt.Size.Width, moTaBaiDangTxt.Lines.Length * 18);
                moTaPanel.Size = new Size(moTaPanel.Size.Width, moTaBaiDangTxt.Size.Height + 120);

                soLuongSanCoTxt.Text = currBaiDang.tongSoLuong().ToString() + " sản phẩm có sẵn";
                tenShopTxt.Text = currShop.ten;
                GUI_Utils.Instance.FitTextBox(tenShopTxt);

                nDanhGiaTxt.Text = currShop.TinhSao().ToString() + "(" + currShop.listBaiDang.SoLuongDanhGia().ToString() + " Đánh Giá)";
                nSanPhamTxt.Text = currShop.listBaiDang.SoLuongSanPham().ToString();
                nTheoDoiTxt.Text = currShop.listFollower.Count.ToString();

                try
                {
                    shopAvtInBaiDang.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(currShop.avt), shopAvtInBaiDang.Size);
                }
                catch
                {
                    shopAvtInBaiDang.Image = GUI_Utils.Instance.Resize(Resources.noPicture, shopAvtInBaiDang.Size);
                }

                currSao = -1;
                ListDGInBDPanel.Controls.Clear();
                if (currBaiDang.listDanhGia.list.Count == 0)
                {
                    ListDGInBDPanel.Controls.Add(KhongCoDanhGiaPanel);
                    ListDGInBDPanel.Height = KhongCoDanhGiaPanel.Height;
                    DGPanelInBDPanel.Height = 230 + ListDGInBDPanel.Height;

                    saoTxt.Text = "";
                    saoBD.Text = "0";

                    saoPTB.Image = GUI_Utils.Instance.CreateStarRatingPictureBox(0, 5, 16, 1).Image;
                    saoPicBD.Image = GUI_Utils.Instance.CreateStarRatingPictureBox(0, 5, 32, 1).Image;
                }
                else
                {
                    saoTxt.Text = currBaiDang.tinhSao().ToString();
                    saoBD.Text = currBaiDang.tinhSao().ToString();

                    saoPTB.Image = GUI_Utils.Instance.CreateStarRatingPictureBox(currBaiDang.tinhSao(), 5, 16, 1).Image;
                    saoPicBD.Image = GUI_Utils.Instance.CreateStarRatingPictureBox(currBaiDang.tinhSao(), 5, 32, 1).Image;


                    giamGiaTxt.Text = "GIẢM " + currBaiDang.giamGia.ToString() + "%";
                    GUI_Utils.Instance.FitTextBox(giamGiaTxt, 0, 0);

                    Sao5Button.Text = "5 Sao(" + currBaiDang.listDanhGia.SoluongDanhGia(5) + ")";
                    Sao4Button.Text = "4 Sao(" + currBaiDang.listDanhGia.SoluongDanhGia(4) + ")";
                    Sao3Button.Text = "3 Sao(" + currBaiDang.listDanhGia.SoluongDanhGia(3) + ")";
                    Sao2Button.Text = "2 Sao(" + currBaiDang.listDanhGia.SoluongDanhGia(2) + ")";
                    Sao1Button.Text = "1 Sao(" + currBaiDang.listDanhGia.SoluongDanhGia(1) + ")";

                    ChuyenTrangDanhGia(1);

                    if (currBaiDang.listDanhGia.list.Count < nDanhGiaInBaiDang)
                    {
                        listPageDanhGia.Visible = false;
                        listDanhGiaFLP.Height = ListDGInBDPanel.Height;
                    }
                    else
                    {
                        listPageDanhGia.Visible = true;
                        SetPageFLP(listPageDanhGia, 1, (currBaiDang.listDanhGia.list.Count - 1) / nDanhGiaInBaiDang + 1, SelectDGPage_Click);
                        GUI_Utils.Instance.FitFLPHeight(listDanhGiaFLP);
                    }

                    DGPanelInBDPanel.Height = listDanhGiaFLP.Height + 230;
                }

                spKhacCuaShopFLP.Controls.Clear();

                int nBaiDang = 0;

                foreach (BaiDang baiDang in currShop.listBaiDang.list)
                {
                    if (nBaiDang++ < 20)
                        spKhacCuaShopFLP.Controls.Add(DrawBaiDang(baiDang, spKhacCuaShopFLP, DenBaiDangTuBDShop));
                }

                qlBaiDang = DAL_BaiDang.Instance.LoadALLBaiDang();

                coTheBanCungThichFLP.Controls.Clear();

                for (int i = 0; i < nColBaiDang_BaiDang * nRowBaiDang_BaiDang; i++)
                {
                    if (i == qlBaiDang.list.Count)
                        break;

                    Panel panel = DrawBaiDang(qlBaiDang.list[i], coTheBanCungThichFLP, DenBaiDangTuBD);
                    coTheBanCungThichFLP.Controls.Add(panel);
                    coTheBanCungThichFLP.Height = ((i - 1) / nColBaiDang_BaiDang + 1) * (panel.Height + panel.Margin.Top + panel.Margin.Bottom);
                    coTheBanCungThichFLP.Width = (panel.Width + panel.Margin.Left + panel.Margin.Right) * nColBaiDang_BaiDang;
                }

                GUI_Utils.Instance.FitFLPHeight(baiDangFLP);
                SetPageFLP(PageListBDFLP, 1, (qlBaiDang.list.Count - 1) / (nColBaiDang_BaiDang * nRowBaiDang_BaiDang) + 1, SelectPageBaiDangButton_Click);
                waittingPanel.SendToBack();
            }
            else
            {
                coTheBanCungThichFLP.Controls.Clear();

                waittingPanel.BringToFront();
                this.Controls.Add(HeaderPannel);
                HeaderPannel.Visible = true;
                HeaderPannel.BringToFront();
                waittingPanel.SendToBack();
            }

        }

        private void thanhToanPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (thanhToanPanel.Visible && qlSanPham.list.Count > 0)
            {
                waittingPanel.BringToFront();
                waittingPanel.Visible = true;
                thanhToanPanel.Controls.Add(HeaderPannel);

                xuHienCoTxt.Text = $"Hiện có {Utils.Instance.SetGia(khachHang.xu)} xu";

                diaChiNhanHangTxt.Text = $"{currDiaChi.ten} | {currDiaChi.soDT}   |   {currDiaChi.diaChiCuThe}, {BLL_DiaChi.Instance.MoTaDiaChi(currDiaChi)}";
                GUI_Utils.Instance.FitTextBox(diaChiNhanHangTxt);
                List<QLSanPham> listQLSP = qlSanPham.PhanRaTheoShop();

                listDonHangFLP.Controls.Clear();
                foreach (QLSanPham qlSP in listQLSP)
                {
                    listDonHangFLP.Controls.Add(DrawFLPDonHang(qlSP));
                }

                int phiVanChuyen = 30000 * listQLSP.Count;
                int tongTien = qlSanPham.tinhTongTien();
                tongTienHangTxt.Text = "₫" + Utils.Instance.SetGia(tongTien);
                phiVanChuyenTxt.Text = "₫" + Utils.Instance.SetGia(phiVanChuyen);
                tongThanhToanTxt.Text = "₫" + Utils.Instance.SetGia(tongTien + phiVanChuyen);

                listDonHangFLP.Controls.Add(thongTinKhacFLP);

                int height = 0;
                foreach (Control control in listDonHangFLP.Controls)
                {
                    height += control.Height + 20;
                }
                listDonHangFLP.Size = new Size(listDonHangFLP.Width, height);
                waittingPanel.SendToBack();
            }
            else
            {
                waittingPanel.BringToFront();
                KhachHang_Panel.Controls.Add(HeaderPannel);
                HeaderPannel.Visible = true;
                HeaderPannel.BringToFront();
                waittingPanel.SendToBack();
            }
        }

        private void dungXuCB_CheckedChanged(object sender, EventArgs e)
        {
            int xu = 0;
            if (((CheckBox)sender).Checked)
            {
                daDungXuTxt.Text = "-₫" + Utils.Instance.SetGia(khachHang.xu);
                xu = khachHang.xu;
            }
            else
            {
                daDungXuTxt.Text = "-₫0";
            }
            tongThanhToanTxt.Text = "₫" + Utils.Instance.SetGia(BLL_DonHang.Instance.TongTien(qlSanPham, xu));
        }

        private void UserPanel_VisibleChanged(object sender, EventArgs e)
        {

            if (UserPanel.Visible)
            {
                waittingPanel.Visible = true;
                waittingPanel.BringToFront();
                UserPanel.Controls.Add(HeaderPannel);
                HeaderPannel.Location = new Point(0, 0);

                SpreadOutClick(myAccount_Button, e);
                UserAccountButton_Click(myAccount_Button, null);


                waittingPanel.SendToBack();
            }
            else
            {
                currChildPanel.Visible = false;
                currChildPanel = profilePanel;
                waittingPanel.BringToFront();
                KhachHang_Panel.Controls.Add(HeaderPannel);
                HeaderPannel.Visible = true;
                HeaderPannel.BringToFront();
                waittingPanel.SendToBack();
            }
        }

        private void diaChiUser_Panel_VisibleChanged(object sender, EventArgs e)
        {
            if (diaChiUser_Panel.Visible)
            {
                VeLai_DiaChi();
            }
            else
            {

            }
        }

        private void profilePanel_VisibleChanged(object sender, EventArgs e)
        {
            if (profilePanel.Visible)
            {
                try
                {
                    miniAvt.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(khachHang.avt), miniAvt.Size);
                    userImagePB.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(khachHang.avt), userImagePB.Size);
                    avtInUserProfile.Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(khachHang.avt), avtInUserProfile.Size);
                }
                catch
                {
                    userImagePB.Image = GUI_Utils.Instance.Resize(Resources.noPicture, userImagePB.Size);
                    avtInUserProfile.Image = GUI_Utils.Instance.Resize(Resources.noPicture, avtInUserProfile.Size);
                }
                userNameTxt.Text = khachHang.taiKhoan;

                suaTen_Button.Text = "Sửa";
                suaTen_Button.BackColor = Color.Gainsboro;
                suaTen_Button.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
                suaTen_Button.FlatAppearance.MouseDownBackColor = Color.Gainsboro;

                suaEmail_button.Text = "Sửa";
                suaEmail_button.BackColor = Color.Gainsboro;
                suaEmail_button.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
                suaEmail_button.FlatAppearance.MouseDownBackColor = Color.Gainsboro;

                suaSDT_button.Text = "Sửa";
                suaSDT_button.BackColor = Color.Gainsboro;
                suaSDT_button.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
                suaSDT_button.FlatAppearance.MouseDownBackColor = Color.Gainsboro;

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
                if (soDT_UP_Box.Text == "")
                {
                    pictureBox8.BackColor = Color.White;
                    soDT_UP_Box.BackColor = Color.White;
                }
                if (khachHang.ngaySinh.Year >= 1899)
                {
                    ngaySinh_CB.SelectedIndex = khachHang.ngaySinh.Day - 1;
                    thangSinh_CB.SelectedIndex = khachHang.ngaySinh.Month - 1;
                    namSinh_CB.SelectedIndex = DateTime.Now.Year - khachHang.ngaySinh.Year;
                }
            }
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

        private void DonMuaButton_Click(object sender, EventArgs e)
        {
            SpreadOutClick(sender, e);
            ChangeColorOfButtonInFLP(sender as Button);
            SwitchPanel(ref currChildPanel, ref DonMuaPanel);
            ChuyenLoaiDH_Click(tatCaDHButton, null);
        }
        
        private void DonMuaB_Click(object sender, EventArgs e)
        {
            littleMenuPanel.Visible = false;
            userProfile_Button_Click(null, null);
            DonMuaButton_Click(DonMuaButton, null);
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

            int loaiDH = -2;

            if (button.Text == "Chờ xác nhận")
                loaiDH = 0;
            else if (button.Text == "Vận chuyển")
                loaiDH = 1;
            else if (button.Text == "Hoàn thành")
                loaiDH = 2;
            else if (button.Text == "Đã hủy")
                loaiDH = -1;

            int height = 10;
            DonHangFLP.Controls.Clear();
            foreach (DonHang donHang in khachHang.listDonHang.list)
            {
                if (loaiDH == -2 || loaiDH == donHang.tinhTrang || (loaiDH == 2 && donHang.tinhTrang == 3))
                {
                    FlowLayoutPanel panel = DrawDonHangPanel(donHang, XemChiTietDonHang_Click);
                    panel.MouseMove += DonHangFLP_Enter;
                    DonHangFLP.Controls.Add(panel);
                    DonHangFLP.Controls.SetChildIndex(panel, 0);
                    height += panel.Height + panel.Margin.Bottom + panel.Margin.Top;
                }
            }
            if (height == 10)
            {
                DonHangFLP.Controls.Add(panel33);
                height += panel33.Height + 10;
            }

            DonHangFLP.Height = height;
            DonMuaPanel.Height = height + 50 + HeaderDHPanel.Height;
        }

        private Panel DrawSPPanelInDHFLP(SanPham sanPham, FlowLayoutPanel parent)
        {
            Panel panel = new Panel
            {
                Name = "parentPanel",
                Size = panel23.Size,
                Margin = panel23.Margin,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
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
                    Cursor = Cursors.Hand,
                    BorderStyle = BorderStyle.None,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.White,
                    Parent = panel
                };
                panel.Controls.Add(pictureBox);
                // pictureBox.Click += ...
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
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(ten);

            TextBox soLuong = new TextBox
            {
                Text = "x" + sanPham.soLuong.ToString(),
                Location = textBox56.Location,
                Size = textBox56.Size,
                Font = textBox56.Font,
                ReadOnly = true,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(soLuong);

            TextBox giaGoc = new TextBox
            {
                Text = "₫" + Utils.Instance.SetGia(sanPham.soLuong * sanPham.gia),
                Location = textBox55.Location,
                Size = textBox55.Size,
                Font = textBox55.Font,
                ReadOnly = true,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(giaGoc);

            TextBox gia = new TextBox
            {
                Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(sanPham.soLuong * sanPham.gia, BLL_BaiDang.Instance.GetGiamGiaFromMaSP(sanPham.maSP))),
                Location = textBox53.Location,
                Size = textBox53.Size,
                Font = textBox53.Font,
                ReadOnly = true,
                ForeColor = Color.OrangeRed,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(gia);

            return panel;
        }

        private void ChuyenSaoDGButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            foreach (Button control in listButtonSaoFLP.Controls)
            {
                if (control.Text == button.Text)
                {
                    button.ForeColor = Color.OrangeRed;
                    button.FlatAppearance.BorderColor = Color.OrangeRed;
                }
                else
                {
                    control.ForeColor = Color.Black;
                    control.FlatAppearance.BorderColor = Color.DarkGray;
                }
            }

            if (button.Text == "Tất Cả")
                currSao = -1;
            else
                currSao = int.Parse(button.Text.Substring(0, 1));

            ListDGInBDPanel.Controls.Clear();
            ListDGInBDPanel.Height = 0;


            if (currBaiDang.listDanhGia.SoluongDanhGia(currSao) == 0)
            {
                ListDGInBDPanel.Controls.Add(KhongCoDanhGiaPanel);
                ListDGInBDPanel.Height = KhongCoDanhGiaPanel.Height;
                DGPanelInBDPanel.Height = 230 + ListDGInBDPanel.Height;
            }
            else
            {
                ChuyenTrangDanhGia(1);

                if (currBaiDang.listDanhGia.SoluongDanhGia(currSao) < nDanhGiaInBaiDang)
                {
                    listPageDanhGia.Visible = false;
                    listDanhGiaFLP.Height = ListDGInBDPanel.Height;
                }
                else
                {
                    listPageDanhGia.Visible = true;
                    SetPageFLP(listPageDanhGia, 1, (currBaiDang.listDanhGia.SoluongDanhGia(currSao) - 1) / nDanhGiaInBaiDang + 1, SelectDGPage_Click);
                    GUI_Utils.Instance.FitFLPHeight(listDanhGiaFLP);
                }

                DGPanelInBDPanel.Height = listDanhGiaFLP.Height + 230;
            }
            
            GUI_Utils.Instance.FitFLPHeight(listDanhGiaFLP);
            GUI_Utils.Instance.FitFLPHeight(baiDangFLP);
        }

        private void DonHangFLP_Enter(object sender, EventArgs e)
        {
            currMaDH = GUI_Utils.Instance.FindControl((Panel)(((Control)sender).Controls[0]), "maDH").Text.Substring(15);
        }

        private FlowLayoutPanel DrawDonHangPanel(DonHang donHang, ClickEvent func)
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
                Name = "headPanel",
                Size = panel22.Size,
                BackColor = Color.White,
                Margin = panel22.Margin,
                Parent = flp
            };
            headPanel.Paint += DrawPanelBorder;

            Button xemShop = new Button
            {
                Name = "xemShop",
                Size = button10.Size,
                BackColor = Color.White,
                Font = button10.Font,
                Location = button10.Location,
                Text = "Xem Shop",
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Parent = headPanel
            };
            xemShop.Click += XemShopInDHButton_Click;
            xemShop.FlatAppearance.BorderColor = Color.Gainsboro;
            xemShop.FlatAppearance.MouseDownBackColor = Color.WhiteSmoke;
            xemShop.FlatAppearance.MouseOverBackColor = Color.WhiteSmoke;
            headPanel.Controls.Add(xemShop);

            TextBox tenShop = new TextBox
            {
                Text = BLL_Shop.Instance.GetTenShopFromMaS(donHang.maS),
                Font = textBox52.Font,
                BackColor = Color.White,
                ReadOnly = true,
                Location = textBox52.Location,
                Cursor = Cursors.IBeam,
                BorderStyle = BorderStyle.None,
                Parent = headPanel
            };
            GUI_Utils.Instance.FitTextBox(tenShop);
            headPanel.Controls.Add(tenShop);

            TextBox maDonHang = new TextBox
            {
                Name = "maDH",
                Text = "Mã đơn hàng: DH" + donHang.maDH,
                Font = textBox58.Font,
                Size = textBox58.Size,
                BackColor = Color.White,
                ReadOnly = true,
                Location = textBox58.Location,
                Cursor = Cursors.IBeam,
                TextAlign = HorizontalAlignment.Right,
                BorderStyle = BorderStyle.None,
                Parent = headPanel
            };

            TextBox tinhTrang = new TextBox
            {
                Name = "tinhTrang",
                Text = donHang.TinhTrang().ToUpper(),
                Font = textBox57.Font,
                Size = textBox57.Size,
                Location = textBox57.Location,
                TextAlign = HorizontalAlignment.Right,
                BackColor = Color.White,
                ForeColor = Color.OrangeRed,
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
                foreach (Control control in panel.Controls)
                    control.Click += new EventHandler(func);
                panel.Click += new EventHandler(func);

                flp.Controls.Add(panel);
                height += panel.Height + panel.Margin.Top + panel.Margin.Bottom;
            }

            Panel tailPanel = new Panel
            {
                Name = "tailPanel",
                Size = panel26.Size,
                Margin = panel26.Margin,
                BackColor = Color.White,
                Parent = flp
            };

            TextBox thanhTien = new TextBox
            {
                Text = "Thành Tiền:",
                Location = textBox65.Location,
                Font = textBox65.Font,
                Size = textBox65.Size,
                BackColor = Color.White,
                TextAlign = HorizontalAlignment.Right,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = tailPanel
            };
            tailPanel.Controls.Add(thanhTien);

            TextBox tongTien = new TextBox
            {
                Text = "₫" + Utils.Instance.SetGia(donHang.tongTien),
                Location = textBox63.Location,
                Font = textBox63.Font,
                Size = textBox63.Size,
                BackColor = Color.White,
                ForeColor = Color.OrangeRed,
                TextAlign = HorizontalAlignment.Right,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = tailPanel
            };
            tailPanel.Controls.Add(tongTien);

            Button button = new Button
            {
                Size = button32.Size,
                Location = button32.Location,
                ForeColor = Color.White,
                BackColor = Color.OrangeRed,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Parent = tailPanel
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.Salmon;
            button.FlatAppearance.MouseOverBackColor = Color.Salmon;


            if (donHang.tinhTrang == 0) // cho xac nhan
            {
                button.Text = "Hủy Đơn";
                button.Click += HuyDonHangButton_Click;
            }
            else if (donHang.tinhTrang == 1) // van chuyen
            {
                button.Text = "Đã nhận hàng";
                if (DateTime.Compare(DateTime.Now, donHang.ngayGiaoHang) >= 0)
                    button.Click += DaNhanHangButton_Click;
                else
                    button.Enabled = false;
            }
            else if (donHang.tinhTrang == 2)
            {
                button.Text = "Đánh giá";
                button.Click += DanhGiaButton_Click;

                Button muaLaiButton = new Button
                {
                    Text = "Mua lại",
                    Location = button33.Location,
                    Size = button33.Size,
                    ForeColor = Color.OrangeRed,
                    BackColor = Color.White,
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    Parent = tailPanel
                };
                tailPanel.Controls.Add(muaLaiButton);
                muaHangButton.Click += MuaLaiButton_Click;
                muaLaiButton.FlatAppearance.BorderSize = 1;
                muaLaiButton.FlatAppearance.BorderColor = Color.DarkGray;
                muaLaiButton.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
                muaLaiButton.FlatAppearance.MouseOverBackColor = Color.Gainsboro;

                TextBox textBox = new TextBox
                {
                    Text = "Đánh giá ngay để nhận 200 xu",
                    Location = textBox62.Location,
                    Size = textBox62.Size,
                    Font = textBox62.Font,
                    ForeColor = Color.OrangeRed,
                    BackColor = Color.White,
                    ReadOnly = true,
                    BorderStyle = BorderStyle.None,
                    Parent = tailPanel
                };
                tailPanel.Controls.Add(textBox);
            }
            else if (donHang.tinhTrang == 3 || donHang.tinhTrang == -1)
            {
                button.Text = "Mua lại";
                button.Click += MuaLaiButton_Click;
            }

            tailPanel.Controls.Add(button);

            flp.Height = height + tailPanel.Height + tailPanel.Margin.Top + tailPanel.Margin.Bottom;

            return flp;
        }

        private void HuyDonHangButton_Click(object sender, EventArgs e)
        {
            DimForm dimForm = new DimForm();
            dimForm.Show();
            ReportForm form = new ReportForm(HuyDon, "Khách hàng hủy đơn");
            form.ShowDialog();
            dimForm.Close();
        }

        private void XemShopInDHButton_Click(object sender, EventArgs e)
        {
            currShop = BLL_Shop.Instance.GetShopFromMaDH(currMaDH);
            XemShopButton_Click(null, null);
        }

        private void MuaLaiButton_Click(object sender, EventArgs e)
        {
            BLL_KhachHang.Instance.MuaLai(khachHang, currMaDH);
            SwitchPanel(ref currPanel, ref gioHangPanel);

            int i = 0;
            foreach (SanPham sanPham in khachHang.listDonHang.GetDonHangFromMaDH(currMaDH).list)
            {
                if (BLL_SanPham.Instance.GetSoLuongFromMaSP(sanPham.maSP) != 0)
                {
                    ((CheckBox)GUI_Utils.Instance.FindControl(listSPTrongGHFLP.Controls[i] as Panel, "ChoseCheckBox")).Checked = true;
                    i++;
                }
            }
            RefreshCartButton();
        }

        private void DaNhanHangButton_Click(object sender, EventArgs e)
        {
            string maDH = GUI_Utils.Instance.GetMaDHByClick(sender);
            BLL_KhachHang.Instance.NhanHang(khachHang, maDH);
            ChuyenLoaiDH_Click(DHDaHTButton, null);

            ThongBaoForm form = new ThongBaoForm("Nhận hàng thành công!!");
            form.Show();
        }

        private void DanhGiaButton_Click(object sender, EventArgs e)
        {
            DimForm dimForm = new DimForm();
            dimForm.Show();
            DanhGiaForm form = new DanhGiaForm(SetData, khachHang.listDonHang.GetDonHangFromMaDH(currMaDH));
            form.ShowDialog();
            dimForm.Close();
        }

        private void HuyDon(string lyDo)
        {
            BLL_KhachHang.Instance.HuyDonHang(khachHang, currMaDH, lyDo);
            ChuyenLoaiDH_Click(DHDaHuyButton, null);

            ThongBaoForm form = new ThongBaoForm("Hủy đơn hàng thành công!!");
            form.Show();
        }

        private void SetData(params DanhGia[] listDanhGia)
        {
            foreach (DanhGia danhGia in listDanhGia)
            {
                BLL_KhachHang.Instance.DanhGia(khachHang, danhGia, currMaDH);
            }

            ThongBaoForm form = new ThongBaoForm("Cảm ơn bạn đã đánh giá!!");
            form.Show();
        }

        private Panel DrawDanhGiaInBaiDang(DanhGia danhGia)
        {
            Panel panel = new Panel
            {
                Size = panel12.Size,
                Margin = panel12.Margin,
                BackColor = Color.White,
                Parent = ListDGInBDPanel
            };
            panel.Paint += DrawPanelBorder;
            panel.Height = 165;

            Label maDG = new Label
            {
                Name = "maDG",
                Text = danhGia.maDG,
                Visible = false,
                Parent = panel,
            };
            panel.Controls.Add(maDG);   

            PictureBox pictureBox = new PictureBox
            {
                Size = pictureBox17.Size,
                Location = pictureBox17.Location,
                BorderStyle = BorderStyle.None,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                Parent = panel
            };
            try
            {
                using (Bitmap bmp = new Bitmap(BLL_KhachHang.Instance.GetURLFromMaKH(danhGia.maKH)))
                {
                    pictureBox.Image = GUI_Utils.Instance.Resize(bmp, pictureBox.Size);
                }
            }
            catch
            {
                pictureBox.Image = GUI_Utils.Instance.Resize(Resources.noPicture, pictureBox.Size);
            }
            pictureBox.Paint += PictureBoxToCircle_Paint;

            panel.Controls.Add(pictureBox);


            TextBox ten = new TextBox
            {
                Text = BLL_KhachHang.Instance.GetTenFromMaKH(danhGia.maKH),
                Location = textBox12.Location,
                Font = textBox12.Font,
                Size = textBox12.Size,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(ten);

            TextBox sanPhamDaMua = new TextBox
            {
                Text = "Sản phẩm đã mua: " + Utils.Instance.GioiHangKyTu(danhGia.sanPhamDaMua, 60),
                Location = textBox64.Location,
                Font = textBox64.Font,
                Size = textBox64.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(sanPhamDaMua);

            for (int i = 0; i < 5; i++)
            {
                PictureBox sao = new PictureBox
                {
                    Size = pictureBox18.Size,
                    Location = new Point(pictureBox18.Location.X + i * pictureBox18.Width, pictureBox18.Location.Y),
                    BorderStyle = BorderStyle.None,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.White,
                    Parent = panel
                };

                if (i < danhGia.sao)
                    sao.Image = Resources.star3;
                else
                    sao.Image = Resources.star4;


                panel.Controls.Add(sao);
            }

            TextBox date = new TextBox
            {
                Text = Utils.Instance.MoTaThoiGian(danhGia.ngayThem),
                Location = textBox66.Location,
                Font = textBox66.Font,
                Size = textBox66.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(date);
            //textBox67

            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                Size = flowLayoutPanel7.Size,
                Location = flowLayoutPanel7.Location,
                BackColor = Color.White,
                Parent = panel
            };
            panel.Controls.Add(flp);

            if (danhGia.thietKeBia != "")
            {
                TextBox thietKeBia = new TextBox
                {
                    Text = "Thiết kế bìa: " + danhGia.thietKeBia,
                    Margin = textBox67.Margin,
                    Location = textBox67.Location,
                    Font = textBox67.Font,
                    Size = textBox67.Size,
                    BackColor = Color.White,
                    ForeColor = Color.DimGray,
                    BorderStyle = BorderStyle.None,
                    ReadOnly = true,
                    Parent = flp
                };
                flp.Controls.Add(thietKeBia);
            }

            if (danhGia.thietKeBia != "")
            {
                TextBox doiTuong = new TextBox
                {
                    Text = "Đối tượng đọc giả: " + danhGia.doiTuong,
                    Location = textBox68.Location,
                    Margin = textBox68.Margin,
                    Font = textBox68.Font,
                    Size = textBox68.Size,
                    BackColor = Color.White,
                    ForeColor = Color.DimGray,
                    BorderStyle = BorderStyle.None,
                    ReadOnly = true,
                    Parent = flp
                };
                flp.Controls.Add(doiTuong);
            }

            if (danhGia.noiDung != "")
            {
                TextBox noiDung = new TextBox
                {
                    Text = danhGia.noiDung,
                    Margin = textBox69.Margin,
                    Location = textBox69.Location,
                    Font = textBox69.Font,
                    Size = textBox69.Size,
                    BackColor = Color.White,
                    Multiline = true,
                    BorderStyle = BorderStyle.None,
                    ReadOnly = true,
                    Parent = flp
                };
                GUI_Utils.Instance.FitTextBoxMultiLines(noiDung);
                flp.Controls.Add(noiDung);
            }

            Panel tailPanel = new Panel
            {
                Size = panel41.Size,
                Margin = panel41.Margin,
                BackColor = Color.White,
                Parent = flp
            };
            flp.Controls.Add(tailPanel);

            PictureBox like = new PictureBox
            {
                Image = Resources.like0,
                Size = pictureBox23.Size,
                Location = pictureBox23.Location,
                BorderStyle = BorderStyle.None,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                Anchor = pictureBox23.Anchor,
                Cursor = Cursors.Hand,
                Parent = tailPanel
            };
            tailPanel.Controls.Add(like);

            TextBox huuIch = new TextBox
            {
                Text = "Hữu Ích?",
                Location = textBox70.Location,
                Font = textBox70.Font,
                Size = textBox70.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Anchor = textBox70.Anchor,
                Parent = tailPanel
            };
            tailPanel.Controls.Add(huuIch);

            Button baoCao = new Button
            {
                Text = "Báo cáo",
                Location = baoCaoButton.Location,
                Size = baoCaoButton.Size,
                ForeColor = Color.DarkGray,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Anchor = baoCaoButton.Anchor,
                Parent = tailPanel
            };
            baoCao.Click += BaoCaoButton_Click;
            baoCao.FlatAppearance.BorderSize = 0;
            baoCao.FlatAppearance.MouseDownBackColor = Color.Transparent;
            baoCao.FlatAppearance.MouseOverBackColor = Color.Transparent;
            tailPanel.Controls.Add(baoCao);

            GUI_Utils.Instance.FitFLPHeight(flp);
            flp.Height += 30;
            panel.Height = flp.Height + 120;

            return panel;
        }

        private Panel DrawThongBaoDH(ThongBao thongBao)
        {
            Panel panel = new Panel
            {
                Size = panel28.Size,
                Cursor = Cursors.Hand,
                BackColor = Color.White,
                Margin = panel28.Margin,
                Parent = TBDonHangFLP
            };
            panel.Paint += DrawPanelBorder;

            string maDH = thongBao.dinhKem.Substring(2);

            using (Bitmap bmp = new Bitmap(BLL_BaiDang.Instance.GetURLFromMaDH(maDH)))
            {
                PictureBox pictureBox = new PictureBox
                {
                    Image = GUI_Utils.Instance.Resize(bmp, pictureBox24.Size),
                    Size = pictureBox24.Size,
                    Location = pictureBox24.Location,
                    BorderStyle = BorderStyle.None,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.White,
                    Parent = panel
                };
                panel.Controls.Add(pictureBox);
            }

            TextBox tinhTrang = new TextBox
            {
                Text = BLL_ThongBao.Instance.ThongBaoTinhTrangDHChoKH(thongBao.noiDung),
                Location = textBox11.Location,
                Font = textBox11.Font,
                Size = textBox11.Size,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(tinhTrang);

            TextBox noiDung = new TextBox
            {
                Name = "noiDung",
                Text = thongBao.noiDung,
                Location = textBox59.Location,
                Font = textBox59.Font,
                Size = textBox59.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Multiline = true,
                Parent = panel
            };
            panel.Controls.Add(noiDung);

            TextBox thoiGian = new TextBox
            {
                Text = Utils.Instance.MoTaThoiGian(thongBao.ngayGui),
                Location = textBox71.Location,
                Font = textBox71.Font,
                Size = textBox71.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(thoiGian);

            //button30
            Button xemChiTiet = new Button
            {
                Text = "Xem Chi Tiết",
                Location = button30.Location,
                Size = button30.Size,
                ForeColor = Color.DarkGray,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Anchor = button30.Anchor,
                Parent = panel
            };
            xemChiTiet.Click += XemChiTietDonHangButton_Click;
            xemChiTiet.FlatAppearance.BorderColor = Color.DimGray;
            xemChiTiet.FlatAppearance.BorderSize = 1;
            xemChiTiet.FlatAppearance.MouseDownBackColor = Color.Transparent;
            xemChiTiet.FlatAppearance.MouseOverBackColor = Color.Transparent;
            panel.Controls.Add(xemChiTiet);


            return panel;
        }

        private void XemChiTietDonHangButton_Click(object sender, EventArgs e)
        {
            string noiDung = GUI_Utils.Instance.FindControl(((Control)sender).Parent as Panel, "noiDung").Text;
            currMaDH = noiDung.Substring(noiDung.IndexOf("DH") + 2, 10);
            capNhatDHPanel.Visible = false;
            troLaiTuChiTietDHButton.Click -= TroVeDonMuaButton_Click;
            troLaiTuChiTietDHButton.Click += TroVeThongBaoDHButotn_Click;
            DrawChiTietDonHang();
        }

        private void MouseOutPanel(object sender, EventArgs e)
        {

        }

        private void MouseInPanel(object sender, EventArgs e)
        {

        }

        private void MouseInPanel(object sender, MouseEventArgs e)
        {

        }

        private void XuButton_Click(object sender, EventArgs e)
        {
            SpreadOutClick(sender, e);
            ChangeColorOfButtonInFLP(sender as Button);
            SwitchPanel(ref currChildPanel, ref ThongBaoXuPanel);
            ThongBaoXuButton_Click(AllLichSuXuButton, null);
            xuHienCoText.Text = Utils.Instance.SetGia(khachHang.xu) + " Xu hiện có";
        }

        private Panel DrawThongBaoXu(ThongBao thongBao)
        {
            Panel panel = new Panel
            {
                Location = panel29.Location,
                Size = panel29.Size,
                Margin = panel29.Margin,
                BackColor = Color.White,
                Parent = XuFLP
            };
            panel.Paint += DrawPanelBorder;

            PictureBox pictureBox = new PictureBox
            {
                Size = pictureBox28.Size,
                Location = pictureBox28.Location,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                Parent = panel
            };
            panel.Controls.Add(pictureBox);

            TextBox textBox1 = new TextBox
            {
                Size = textBox77.Size,
                Location = textBox77.Location,
                Font = textBox77.Font,
                BackColor = Color.White,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(textBox1);

            TextBox textBox2 = new TextBox
            {
                Size = textBox78.Size,
                Location = textBox78.Location,
                Font = textBox78.Font,
                BackColor = Color.White,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(textBox2);

            TextBox textBox3 = new TextBox
            {
                Text = Utils.Instance.MoTaThoiGian(thongBao.ngayGui),
                Size = textBox79.Size,
                Location = textBox79.Location,
                Font = textBox79.Font,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(textBox3);

            TextBox textBox4 = new TextBox
            {
                Size = textBox80.Size,
                Location = textBox80.Location,
                Font = textBox80.Font,
                BackColor = Color.White,
                ForeColor = Color.Orange,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(textBox4);

            if (thongBao.noiDung.Contains("nhận được"))
            {
                pictureBox.Image = Resources.coin4;
                textBox1.Text = "Đánh giá sản phẩm";
                textBox2.Text = "Cảm ơn đánh giá của bạn";
                textBox4.Text = "+" + BLL_ThongBao.Instance.GetXuFromThongBao(thongBao.noiDung);
            }
            else
            {
                pictureBox.Image = Resources.coin5;
                textBox1.Text = "Đã dùng xu";
                textBox2.Text = "Bạn đã dùng xu cho đơn hàng " + BLL_ThongBao.Instance.GetMaDHFromNoiDungTB(thongBao.noiDung);
                textBox4.Text = "-" + BLL_ThongBao.Instance.GetXuFromThongBao(thongBao.noiDung);
            }

            return panel;
        }

        private void ThongBaoXuButton_Click(object sender, EventArgs e)
        {
            DaDungXuButton.ForeColor = Color.Black;
            DaNhanXuButton.ForeColor = Color.Black;
            AllLichSuXuButton.ForeColor = Color.Black;
            ((Button)sender).ForeColor = Color.Red;

            Button button = sender as Button;

            string loaiTB = " ";

            if (button.Text == "ĐÃ NHẬN")
            {
                loaiTB = "nhận được";
            }
            else if (button.Text == "ĐÃ DÙNG")
            {
                loaiTB = "đã dùng";
            }

            int height = 0;
            XuFLP.Controls.Clear();
            foreach (ThongBao thongBao in khachHang.listThongBao.list)
            {
                if (thongBao.from.Equals("HeThongXu") && thongBao.noiDung.Contains(loaiTB))
                {
                    Panel panel = DrawThongBaoXu(thongBao);
                    XuFLP.Controls.Add(panel);
                    XuFLP.Controls.SetChildIndex(panel, 0);
                    height += panel.Height;
                }
            }

            if (height == 0)
            {
                XuFLP.Controls.Add(panel32);
                height += panel32.Height;

            }

            XuFLP.Height = height;
            ThongBaoXuPanel.Height = height + 230;
        }

        private void DaXemButton_Click(object sender, EventArgs e)
        {
            ChangeColorOfButtonInFLP(sender as Button);
            if (khachHang.listDaXem.Count == 0)
            {
                SwitchPanel(ref currChildPanel, ref khongCoBDPanel);
                khongCoTxt.Text = "Bạn chưa xem bài đăng nào gần đây.";

                return;
            }

            daXemFLP.Controls.Clear();

            foreach (string maBD in khachHang.listDaXem)
            {
                BaiDang baiDang = BLL_BaiDang.Instance.GetBaiDangFromMaBD(maBD);
                if (baiDang != null)
                    daXemFLP.Controls.Add(DrawBaiDang(baiDang, daXemFLP, DenBaiDangTuDaXem));
            }

            daXemFLP.Height = ((khachHang.listDaXem.Count - 1) / 4 + 1) * daXemFLP.Controls[0].Height + daXemFLP.Controls[0].Margin.Top + daXemFLP.Controls[0].Margin.Bottom;
            daXemPanel.Height = daXemFLP.Height + 100;

            SwitchPanel(ref currChildPanel, ref daXemPanel);
        }

        private void KhacButton_Click(object sender, EventArgs e)
        {
            if (!daXemPanel.Visible)
                SpreadOutClick(khacButton, e);
            DaXemButton_Click(daXemButton, e);
        }

        private void DaThichButton_Click(object sender, EventArgs e)
        {
            ChangeColorOfButtonInFLP(sender as Button);
            if (khachHang.listThich.Count == 0)
            {
                khongCoTxt.Text = "Bạn chưa chọn thích sản phẩm nào.";
                SwitchPanel(ref currChildPanel, ref khongCoBDPanel);

                return;
            }

            daThichFLP.Controls.Clear();

            foreach (string maBD in khachHang.listThich)
            {
                BaiDang baiDang = BLL_BaiDang.Instance.GetBaiDangFromMaBD(maBD);
                if(baiDang != null)
                    daThichFLP.Controls.Add(DrawBaiDang(baiDang, daThichFLP, DenBaiDangTuDaThich));
            }

            daThichFLP.Height = ((khachHang.listThich.Count - 1) / 4 + 1) * daThichFLP.Controls[0].Height + daThichFLP.Controls[0].Margin.Top + daThichFLP.Controls[0].Margin.Bottom;
            daThichPanel.Height = daThichFLP.Height + 100;

            SwitchPanel(ref currChildPanel, ref daThichPanel);
        }

        private void timKiemPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (timKiemPanel.Visible)
            {
                waittingPanel.BringToFront();
                waittingPanel.Visible = true;
                timKiemPanel.Controls.Add(HeaderPannel);
                HeaderPannel.Location = new Point(0, 0);
                HeaderPannel.BringToFront();

                listBDInTimKiemFLP.Controls.Clear();

                if (qlBaiDangTmp.list.Count == 0)
                {
                    boLocTimKiemFLP.Visible = false;
                    controlTimKiemPanel.Visible = false;
                    kqTimKiemTxt.Visible = false;
                    listPageTimKiemFLP.Visible = false;
                    listBDInTimKiemFLP.Controls.Add(panel35);
                    listBDInTimKiemFLP.Height = listBDInTimKiemFLP.Controls[0].Height;
                }
                else
                {
                    boLocTimKiemFLP.Visible = true;
                    controlTimKiemPanel.Visible = true;
                    kqTimKiemTxt.Visible = true;
                    listPageTimKiemFLP.Visible = true;

                    listBDInTimKiemFLP.Controls.Clear();

                    for (int i = 0; i < nColBaiDang_TimKiem * nRowBaiDang_TimKiem && i < qlBaiDangTmp.list.Count; i++)
                    {
                        Panel panel = DrawBaiDang(qlBaiDangTmp.list[i], listBDInTimKiemFLP, DenBaiDangTuTimKiem);
                        listBDInTimKiemFLP.Controls.Add(panel);
                        listBDInTimKiemFLP.Height = ((i - 1) / nColBaiDang_TimKiem + 1) * (panel.Height + panel.Margin.Top + panel.Margin.Bottom);
                        listBDInTimKiemFLP.Width = (panel.Width + panel.Margin.Left + panel.Margin.Right) * nColBaiDang_TimKiem;
                    }

                    GUI_Utils.Instance.FitFLPHeight(timKiemFLP);
                    SetPageFLP(listPageTimKiemFLP, 1, (qlBaiDangTmp.list.Count - 1) / (nColBaiDang_TimKiem * nRowBaiDang_TimKiem) + 1, SelectPageTimKiemButton_Click);
                }
                waittingPanel.SendToBack();
            }
            else
            {
                waittingPanel.BringToFront();
                this.Controls.Add(HeaderPannel);
                HeaderPannel.Visible = true;
                HeaderPannel.BringToFront();
                waittingPanel.SendToBack();
            }
        }

        private void KhoanGiaButton_Click(object sender, EventArgs e)
        {
            int min = int.Parse(giaMinTxt.Text);
            int max = int.Parse(giaMaxTxt.Text);

            if (min <= max)
            {
                SetListBaiDangInTimKiem();
            }
        }

        private void OnlyPressNumber(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void OrderByBanChayButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button.ForeColor == Color.White)
            {
                return;
            }

            foreach (Control con in button.Parent.Controls)
            {
                if (con is Button)
                {
                    Button btn = con as Button;
                    btn.ForeColor = Color.Black;
                    btn.BackColor = Color.White;
                    btn.FlatAppearance.MouseOverBackColor = Color.White;
                    btn.FlatAppearance.MouseDownBackColor = Color.White;
                }
            }

            button.ForeColor = Color.White;
            button.BackColor = Color.OrangeRed;
            button.FlatAppearance.MouseOverBackColor = Color.OrangeRed;
            button.FlatAppearance.MouseDownBackColor = Color.OrangeRed;

            Utils.Instance.Sort(qlBaiDang.list, 0, qlBaiDang.list.Count - 1, BaiDang.CompareLuocBan, BaiDang.EqualLuocBan);
            qlBaiDang.list.Reverse();
        }

        private void SetListBaiDang(FlowLayoutPanel flpDanhMuc)
        {
            qlBaiDangTmp.list.Clear();

            bool full = true;
            foreach (CheckBox con in flpDanhMuc.Controls)
            {
                if (con.Checked)
                {
                    full = false;
                    break;
                }
            }
            if (full)
            {
                foreach (BaiDang baiDang in qlBaiDang.list)
                {
                    qlBaiDangTmp.Add(baiDang);
                }
            }
            else
            {
                foreach (BaiDang baiDang in qlBaiDang.list)
                {
                    bool exits = false;
                    foreach (SanPham sanPham in baiDang.list)
                    {
                        bool flag = false;
                        foreach (CheckBox con in flpDanhMuc.Controls)
                        {
                            if (con.Checked && con.Text.Contains(sanPham.loaiSP.tenLoaiSP))
                            {
                                exits = true;
                                flag = true;
                                break;
                            }
                        }

                        if (flag)
                            break;
                    }
                    if (exits)
                    {
                        qlBaiDangTmp.Add(baiDang);
                    }
                }
            }
        }

        private void SetListBaiDangInTimKiem()
        {
            SetListBaiDang(danhMucFLP);

            if (giaMinTxt.Text != "" && giaMaxTxt.Text != "")
            {
                int min = int.Parse(giaMinTxt.Text);
                int max = int.Parse(giaMaxTxt.Text);
                if (min <= max)
                {
                    qlBaiDangTmp = BLL_BaiDang.Instance.GetBaiDangInKhoangGia(qlBaiDangTmp, min, max);
                }
            }

            timKiemPanel.AutoScrollPosition = controlTimKiemPanel.Location;
            ChuyenTrang(listBDInTimKiemFLP, qlBaiDangTmp, 1, DenBaiDangTuTimKiem, nRowBaiDang_TimKiem, nColBaiDang_TimKiem);
            SetPageFLP(listPageTimKiemFLP, 1, (qlBaiDangTmp.list.Count - 1) / (nRowBaiDang_TimKiem * nColBaiDang_TimKiem) + 1, SelectPageTimKiemButton_Click);
        }

        private void SetListBaiDangInShop()
        {
            SetListBaiDang(danhMucSFLP);

            timKiemPanel.AutoScrollPosition = panel34.Location;
            ChuyenTrang(listBDCuaShopFLP, qlBaiDangTmp, 1, DenBaiDangTuTimKiem, nRowBaiDang_Shop, nColBaiDang_Shop);
            SetPageFLP(listPageInShopFLP, 1, (qlBaiDangTmp.list.Count - 1) / (nRowBaiDang_Shop * nColBaiDang_Shop) + 1, SelectPageTimKiemButton_Click);
        }

        private void OrderByBanChayInTimKiem(object sender, EventArgs e)
        {
            OrderByBanChayButton_Click(sender, e);
            SetListBaiDangInTimKiem();
        }

        private void OrderByBanChayInShop(object sender, EventArgs e)
        {
            OrderByBanChayButton_Click(sender, e);
            SetListBaiDangInShop();
        }

        private void OrderByGiaButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            foreach (Control con in button.Parent.Controls)
            {
                if (con is Button)
                {
                    Button btn = con as Button;
                    btn.ForeColor = Color.Black;
                    btn.BackColor = Color.White;
                    btn.FlatAppearance.MouseOverBackColor = Color.White;
                    btn.FlatAppearance.MouseDownBackColor = Color.White;
                }
            }

            button.ForeColor = Color.White;
            button.BackColor = Color.OrangeRed;
            button.FlatAppearance.MouseOverBackColor = Color.OrangeRed;
            button.FlatAppearance.MouseDownBackColor = Color.OrangeRed;

            Utils.Instance.Sort(qlBaiDang.list, 0, qlBaiDang.list.Count - 1, BaiDang.CompareGiaMin, BaiDang.EqualGiaMin);
            if (button.Text == "Giá: Thấp đến cao")
            {
                button.Text = "Giá: Cao đến thấp";
                qlBaiDang.list.Reverse();
            }
            else
            {
                button.Text = "Giá: Thấp đến cao";
            }
        }

        private void OrderByGiaInTimKiem(object sender, EventArgs e)
        {
            OrderByGiaButton_Click(sender, e);
            SetListBaiDangInTimKiem();
        }

        private void OrderByGiaInShop(object sender, EventArgs e)
        {
            OrderByGiaButton_Click(sender, e);
            SetListBaiDangInShop();
        }

        private void DanhMucCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetListBaiDangInTimKiem();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            kqTimKiemTxt.Text = $"Kết quả tìm kiếm có liên quan cho từ khóa '{search_Text.Text}'";
            qlBaiDang = BLL_BaiDang.Instance.GetBaiDangFromTextSearch(search_Text.Text);
            qlBaiDangTmp = BLL_BaiDang.Instance.GetBaiDangFromTextSearch(search_Text.Text);

            BLL_KhachHang.Instance.LuuLichSuTimKiem(khachHang, search_Text.Text);

            danhMucFLP.Controls.Clear();
            danhMucFLP.Height = 0;

            foreach (string str in BLL_BaiDang.Instance.TaoListDanhMucVaSoLuong(qlBaiDang))
            {
                CheckBox checkBox = new CheckBox
                {
                    Text = str,
                    Size = button35.Size,
                    Parent = danhMucFLP,
                    Checked = false,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(10, 0, 0, 0),
                };
                checkBox.CheckedChanged += DanhMucCheckBox_CheckedChanged;
                danhMucFLP.Height += checkBox.Height;
                danhMucFLP.Controls.Add(checkBox);
                danhMucFLP.Controls.SetChildIndex(checkBox, 0);
            }

            int height = 0;
            foreach (Control con in danhMucFLP.Controls)
            {
                height += con.Height + con.Margin.Top + con.Margin.Bottom;
            }

            danhMucFLP.Height = height + 20;

            giaMinTxt.Text = "";
            giaMaxTxt.Text = "";
            orderByGiaButton.Text = "Giá";


            SwitchPanel(ref currPanel, ref timKiemPanel);
        }

        private void TimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchButton_Click(searchButton, null);
            }
        }

        private void shopPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (shopPanel.Visible)
            {
                waittingPanel.BringToFront();
                waittingPanel.Visible = true;
                shopPanel.Controls.Add(HeaderPannel);
                HeaderPannel.Location = new Point(0, 0);
                HeaderPannel.BringToFront();

                try
                {
                    using (Bitmap bmp = new Bitmap(currShop.avt))
                    {
                        avtShop.Image = GUI_Utils.Instance.Resize(bmp, avtShop.Size);
                    }
                }
                catch
                {
                    avtShop.Image = GUI_Utils.Instance.Resize(Resources.noPicture, avtShop.Size);
                }

                nameShop.Text = currShop.ten;
                slSanPhamInSTxt.Text = currShop.listBaiDang.SoLuongSanPham().ToString();
                slDanhGiaInSTxt.Text = currShop.TinhSao().ToString() + "(" + currShop.listBaiDang.SoLuongDanhGia().ToString() + " Đánh Giá)";
                slNgTheoDoiInSTxt.Text = currShop.listFollower.Count.ToString();

                if (BLL_KhachHang.Instance.DaTheoDoi(khachHang.listFollow, currShop.maSo))
                    followButtonInS.Text = "Hủy heo dõi";
                else
                    followButtonInS.Text = "Theo dõi";

                listBDCuaShopFLP.Controls.Clear();
                for (int i = 0; i < nColBaiDang_Shop * nRowBaiDang_Shop && i < qlBaiDangTmp.list.Count; i++)
                {
                    Panel panel = DrawBaiDang(qlBaiDangTmp.list[i], listBDCuaShopFLP, DenBaiDangTuShop);
                    listBDCuaShopFLP.Controls.Add(panel);
                    listBDCuaShopFLP.Height = ((i - 1) / nColBaiDang_Shop + 1) * (panel.Height + panel.Margin.Top + panel.Margin.Bottom);
                    listBDCuaShopFLP.Width = (panel.Width + panel.Margin.Left + panel.Margin.Right) * nColBaiDang_Shop;
                }

                GUI_Utils.Instance.FitFLPHeight(shopFLP);
                SetPageFLP(listPageInShopFLP, 1, (qlBaiDangTmp.list.Count - 1) / (nColBaiDang_Shop * nRowBaiDang_Shop) + 1, SelectPageShopButton_Click);

                waittingPanel.SendToBack();
            }
            else
            {
                waittingPanel.BringToFront();
                this.Controls.Add(HeaderPannel);
                HeaderPannel.Visible = true;
                HeaderPannel.BringToFront();
                waittingPanel.SendToBack();
            }
        }

        private void XemShopButton_Click(object sender, EventArgs e)
        {
            qlBaiDang = currShop.listBaiDang;
            qlBaiDangTmp = new QLBaiDang();
            SetListBaiDangInShop();

            danhMucSFLP.Controls.Clear();
            danhMucSFLP.Height = 0;

            foreach (string str in BLL_BaiDang.Instance.TaoListDanhMucVaSoLuong(qlBaiDang))
            {
                CheckBox checkBox = new CheckBox
                {
                    Text = str,
                    Size = button35.Size,
                    Parent = danhMucFLP,
                    Checked = false,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(10, 0, 0, 0),
                };
                checkBox.CheckedChanged += DanhMucSCheckBox_CheckedChanged;
                danhMucSFLP.Height += checkBox.Height;
                danhMucSFLP.Controls.Add(checkBox);
                danhMucSFLP.Controls.SetChildIndex(checkBox, 0);
            }

            int height = 0;
            foreach (Control con in danhMucSFLP.Controls)
            {
                height += con.Height + con.Margin.Top + con.Margin.Bottom;
            }

            danhMucSFLP.Height = height + 20;

            SwitchPanel(ref currPanel, ref shopPanel);
        }

        private void DanhMucSCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetListBaiDangInShop();
        }

        private void FollowInShop(object sender, EventArgs e)
        {
            FollowButton_Click(sender, e);
            slNgTheoDoiInSTxt.Text = currShop.listFollower.Count.ToString();
        }

        private void tatCaSPShopButton_Click(object sender, EventArgs e)
        {
            foreach (CheckBox con in danhMucSFLP.Controls)
            {
                con.Checked = false;
            }
        }

        private void TimKiem_Click(object sender, EventArgs e)
        {
            if (khachHang.lichSuTimKiem.Count == 0)
            {
                lichSuTimKiemFLP.Visible = false;
                return;
            }

            string searchText = (sender as TextBox).Text;


            lichSuTimKiemFLP.Controls.Clear();
            if (searchText.Length == 0)
            {
                lichSuTimKiemFLP.Controls.Add(lichSuTimKiemLB);
                lichSuTimKiemFLP.Controls.Add(xoaLichSuButton);
            }
            HeaderPannel.Parent.Controls.Add(lichSuTimKiemFLP);
            lichSuTimKiemFLP.Location = new Point(271, 95);
            lichSuTimKiemFLP.Width = search_Text.Width + 20;
            lichSuTimKiemLB.Width = search_Text.Width - xoaLichSuButton.Width;


            foreach (string noiDung in BLL_KhachHang.Instance.GoiYTimKiem(khachHang.lichSuTimKiem, searchText))
            {
                Button button = new Button
                {
                    Text = noiDung,
                    Margin = button48.Margin,
                    Size = button48.Size,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    BackColor = Color.White,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Parent = lichSuTimKiemFLP
                };
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
                button.FlatAppearance.MouseDownBackColor = Color.FromArgb(224, 224, 224);
                button.Width = lichSuTimKiemFLP.Width;
                button.Click += TimKiemFromLichSu_Click;
                lichSuTimKiemFLP.Controls.Add(button);
            }

            GUI_Utils.Instance.FitFLPHeight(lichSuTimKiemFLP);
            if (searchText.Length == 0)
                lichSuTimKiemFLP.Height -= xoaLichSuButton.Height;

            lichSuTimKiemFLP.BringToFront();
            lichSuTimKiemFLP.Visible = true;
        }

        private void LeaveSearch(object sender, EventArgs e)
        {
            if (!GUI_Utils.Instance.IsMouseOverControl(lichSuTimKiemFLP, Cursor.Position))
                lichSuTimKiemFLP.Visible = false;
        }

        private void TimKiemFromLichSu_Click(object sender, EventArgs e)
        {
            search_Text.Text = (sender as Button).Text;
            searchButton_Click(null, null);
            lichSuTimKiemFLP.Visible = false;
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            string searchText = (sender as TextBox).Text;

            lichSuTimKiemFLP.Controls.Clear();
            if (searchText == "")
            {
                lichSuTimKiemFLP.Controls.Add(lichSuTimKiemLB);
                lichSuTimKiemFLP.Controls.Add(xoaLichSuButton);
                if (khachHang.lichSuTimKiem.Count == 0)
                {
                    lichSuTimKiemFLP.Visible = false;
                    return;
                }
            }

            foreach (string noiDung in BLL_KhachHang.Instance.GoiYTimKiem(khachHang.lichSuTimKiem, searchText))
            {
                Button button = new Button
                {
                    Text = noiDung,
                    Margin = button48.Margin,
                    Size = button48.Size,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    BackColor = Color.White,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Parent = lichSuTimKiemFLP
                };
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
                button.FlatAppearance.MouseDownBackColor = Color.FromArgb(224, 224, 224);
                button.Width = lichSuTimKiemFLP.Width;
                button.Click += TimKiemFromLichSu_Click;
                lichSuTimKiemFLP.Controls.Add(button);
            }

            GUI_Utils.Instance.FitFLPHeight(lichSuTimKiemFLP);
            if (searchText.Length == 0)
                lichSuTimKiemFLP.Height -= xoaLichSuButton.Height;
            lichSuTimKiemFLP.Visible = true;
        }

        private void XoaLichSuButton_Click(object sender, EventArgs e)
        {
            BLL_KhachHang.Instance.XoaLichSuTimKiem(khachHang);
            lichSuTimKiemFLP.Visible = false;
        }

        private void ToCaoBaiDang(string lyDo)
        {
            BLL_KhachHang.Instance.ToCaoBaiDang(currBaiDang.maBD, lyDo);

            ThongBaoForm form = new ThongBaoForm("Cảm ơn bạn đã báo cáo!!");
            form.Show();
        }

        private void ToCaoBDButton_Click(object sender, EventArgs e)
        {
            DimForm dimForm = new DimForm();
            dimForm.Show();
            ReportForm form = new ReportForm(ToCaoBaiDang, "Tố cáo bài đăng");
            form.ShowDialog();
            dimForm.Close();
        }

        private void BaoCaoDanhGia(string lyDo)
        {
            BLL_KhachHang.Instance.BaoCaoDanhGia(currMaDG, lyDo);

            ThongBaoForm form = new ThongBaoForm("Cảm ơn bạn đã báo cáo!!");
            form.Show();
        }

        private void BaoCaoButton_Click(object sender, EventArgs e)
        {
            currMaDG = GUI_Utils.Instance.FindControl(((Button)sender).Parent.Parent.Parent as Panel, "maDG").Text;

            DimForm dimForm = new DimForm();
            dimForm.Show();
            ReportForm form = new ReportForm(BaoCaoDanhGia, "Báo cáo đánh giá");
            form.ShowDialog();
            dimForm.Close();
        }

        private void DrawTinhTrangDonHang(int tinhTrang)
        {
            if (tinhTrang == 0)
            {
                bridge1.BackColor = Color.FromArgb(255, 120, 32);
                bridge2.BackColor = Color.FromArgb(160, 160, 160);
                bridge3.BackColor = Color.FromArgb(160, 160, 160);

                dhDaDatPic.Paint += PictureBoxToCircle2;
                choXacNhanPic.Image = Resources.wait;
                choXacNhanPic.Paint += PictureBoxToCircle1;
                choXacNhanTxt.Text = "Chờ Xác Nhận";

                choGiaoHangPic.Image = Resources.truct3;
                choGiaoHangPic.Paint += PictureBoxToCircle3;
                choGiaoHangTxt.Text = "Chờ Giao Hàng";

                danhGiaPic.Image = Resources.star8;
                danhGiaPic.Paint += PictureBoxToCircle3;
                danhGiaDHTxt.Text = "Đánh Giá";

                buttonFunc1.Text = "Hủy Đơn";
                buttonFunc1.Click += HuyDonHangButton_Click;
                buttonFunc2.Visible = false;
            }
            else if (tinhTrang == 1)
            {
                bridge1.BackColor = Color.FromArgb(255, 120, 32);
                bridge2.BackColor = Color.FromArgb(255, 120, 32);
                bridge3.BackColor = Color.FromArgb(160, 160, 160);

                dhDaDatPic.Paint += PictureBoxToCircle2;
                choXacNhanPic.Image = Resources.money1;
                choXacNhanPic.Paint += PictureBoxToCircle2;
                choXacNhanTxt.Text = "Đã Xác Nhận Và Giao Hàng";

                choGiaoHangPic.Image = Resources.truck4;
                choGiaoHangPic.Paint += PictureBoxToCircle1;
                choGiaoHangTxt.Text = "Chờ Nhận Hàng";

                danhGiaPic.Image = Resources.star8;
                danhGiaPic.Paint += PictureBoxToCircle3;
                danhGiaDHTxt.Text = "Đánh Giá";

                buttonFunc1.Text = "Đã Nhận Hàng";
                buttonFunc1.Click += DaNhanHangButton_Click;
                buttonFunc2.Visible = false;
            }
            else if (tinhTrang == 2)
            {
                bridge1.BackColor = Color.FromArgb(255, 120, 32);
                bridge2.BackColor = Color.FromArgb(255, 120, 32);
                bridge3.BackColor = Color.FromArgb(255, 120, 32);

                dhDaDatPic.Paint += PictureBoxToCircle2;
                choXacNhanPic.Image = Resources.money1;
                choXacNhanPic.Paint += PictureBoxToCircle2;
                choXacNhanTxt.Text = "Đã Xác Nhận Và Giao Hàng";

                choGiaoHangPic.Image = Resources.truct2;
                choGiaoHangPic.Paint += PictureBoxToCircle2;
                choGiaoHangTxt.Text = "Chờ Nhận Hàng";

                danhGiaPic.Image = Resources.star7;
                danhGiaPic.Paint += PictureBoxToCircle1;
                danhGiaDHTxt.Text = "Đánh Giá";

                buttonFunc1.Text = "Đánh Giá";
                buttonFunc1.Click += DanhGiaButton_Click;
                buttonFunc2.Visible = true;
                buttonFunc2.Text = "Mua Lại";
                buttonFunc2.Click += MuaLaiButton_Click;
            }
            else if (tinhTrang == 3)
            {
                bridge1.BackColor = Color.FromArgb(255, 120, 32);
                bridge2.BackColor = Color.FromArgb(255, 120, 32);
                bridge3.BackColor = Color.FromArgb(255, 120, 32);

                dhDaDatPic.Paint += PictureBoxToCircle2;
                choXacNhanPic.Image = Resources.money1;
                choXacNhanPic.Paint += PictureBoxToCircle2;
                choXacNhanTxt.Text = "Đã Xác Nhận Và Giao Hàng";

                choGiaoHangPic.Image = Resources.package;
                choGiaoHangPic.Paint += PictureBoxToCircle2;
                choGiaoHangTxt.Text = "Đã Nhận Hàng";

                danhGiaPic.Image = Resources.start6;
                danhGiaPic.Paint += PictureBoxToCircle2;
                danhGiaDHTxt.Text = "Đã Đánh Giá";

                buttonFunc1.Text = "Mua Lại";
                buttonFunc1.Click += MuaLaiButton_Click;
                buttonFunc2.Visible = false;
            }
        }

        private void DrawChiTietDonHang()
        {
            DonHang donHang = khachHang.listDonHang.GetDonHangFromMaDH(currMaDH);

            DrawTinhTrangDonHang(donHang.tinhTrang);
            maDHInChiTietDHTxt.Text = "MÃ ĐƠN HÀNG: ĐH" + donHang.maDH;
            ttdhInChiTietDH.Text = "TÌNH TRẠNG: " + donHang.TinhTrang().ToUpper();
            tenNhanHangTxt.Text = donHang.diaChi.ten;
            sdtNhanHangTxt.Text = donHang.diaChi.soDT;
            dcctNhanHangTxt.Text = donHang.diaChi.diaChiCuThe + ", " + BLL_DiaChi.Instance.MoTaDiaChi(donHang.diaChi);

            FlowLayoutPanel sanPhamFlp = DrawDonHangPanel(donHang, DenBaiDangTuDonHang);
            sanPhamFlp.Controls[0].Controls.RemoveByKey("maDH");
            sanPhamFlp.Controls[0].Controls.RemoveByKey("tinhTrang");
            sanPhamFlp.Controls.RemoveByKey("tailPanel");
            GUI_Utils.Instance.FitFLPHeight(sanPhamFlp);

            chiTietDonHangFLP.Controls.Clear();
            chiTietDonHangFLP.Controls.Add(panel43);
            chiTietDonHangFLP.Controls.Add(tinhTrangDHPanel);
            chiTietDonHangFLP.Controls.Add(panel42);
            chiTietDonHangFLP.Controls.Add(sanPhamFlp);
            chiTietDonHangFLP.Controls.Add(panel44);

            GUI_Utils.Instance.FitFLPHeight(chiTietDonHangFLP);
            chiTietDHPanel.Height = chiTietDonHangFLP.Height + 60;

            ttHangTxt.Text = "₫" + Utils.Instance.SetGia(donHang.tongTien - 30000 + donHang.xu);
            phiVCTxt.Text = "₫30.000";
            xuDaDungTxt.Text = "-₫" + Utils.Instance.SetGia(donHang.xu);
            ttDonHangTxt.Text = "₫" + Utils.Instance.SetGia(donHang.tongTien);
            ptttTxt.Text = donHang.PhuongThucThanhToan();

            chiTietDHPanel.Visible = true;
            chiTietDHPanel.BringToFront();
            currChildPanel = chiTietDHPanel;
        }

        private void XemChiTietDonHang_Click(object sender, EventArgs e)
        {
            DonMuaPanel.Visible = false;
            currMaDH = GUI_Utils.Instance.GetMaDHByClick(sender);
            troLaiTuChiTietDHButton.Click -= TroVeThongBaoDHButotn_Click;
            troLaiTuChiTietDHButton.Click += TroVeDonMuaButton_Click;
            DrawChiTietDonHang();
        }

        private void TroVeDonMuaButton_Click(object sender, EventArgs e)
        {
            chiTietDHPanel.Visible = false;
            DonMuaPanel.Visible = true;
            DonMuaPanel.BringToFront();
            currChildPanel = DonMuaPanel;
        }

        private void TroVeThongBaoDHButotn_Click(object sender, EventArgs e)
        {
            chiTietDHPanel.Visible = false;
            capNhatDHPanel.Visible = true;
            capNhatDHPanel.BringToFront();
            currChildPanel = capNhatDHPanel;
        }

        public void PictureBoxToCircle1(object sender, PaintEventArgs e)
        {
            using (SolidBrush br = new SolidBrush(Color.White))
            {
                e.Graphics.FillRectangle(br, ClientRectangle);
            }

            PictureBox pic = sender as PictureBox;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            SolidBrush brush = new SolidBrush(Color.FromArgb(255, 120, 32));
            e.Graphics.FillEllipse(brush, 0, 0, pic.Width - 1, pic.Height - 1);

            e.Graphics.DrawImage(GUI_Utils.Instance.Resize(pic.Image, new Size(32, 32)), new Point(16, 16));
        }

        public void PictureBoxToCircle2(object sender, PaintEventArgs e)
        {
            using (SolidBrush br = new SolidBrush(Color.White))
            {
                e.Graphics.FillRectangle(br, ClientRectangle);
            }

            PictureBox pic = sender as PictureBox;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen pen = new Pen(Color.FromArgb(255, 120, 32), 3))
            {
                e.Graphics.DrawEllipse(pen, 1, 1, pic.Width - 3, pic.Height - 3);
            }
            e.Graphics.DrawImage(GUI_Utils.Instance.Resize(pic.Image, new Size(32, 32)), new Point(16, 16));
        }

        public void PictureBoxToCircle3(object sender, PaintEventArgs e)
        {
            using (SolidBrush br = new SolidBrush(Color.White))
            {
                e.Graphics.FillRectangle(br, ClientRectangle);
            }

            PictureBox pic = sender as PictureBox;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen pen = new Pen(Color.FromArgb(160, 160, 160), 3))
            {
                e.Graphics.DrawEllipse(pen, 1, 1, pic.Width - 3, pic.Height - 3);
            }
            e.Graphics.DrawImage(GUI_Utils.Instance.Resize(pic.Image, new Size(32, 32)), new Point(16, 16));
        }

        private void ChuaDanhGiaButton_Click(object sender, EventArgs e)
        {
            foreach (Control control in DGHeaderPanel.Controls)
            {
                if (control.ForeColor == Color.Red)
                {
                    control.ForeColor = Color.Black;
                    break;
                }
            }
            Button button = sender as Button;
            button.ForeColor = Color.Red;

            danhGiaCuaToiFLP.Controls.Clear();
            foreach (DonHang donHang in khachHang.listDonHang.list)
            {
                if (donHang.tinhTrang == 2)
                {
                    Panel panel = DrawChuaDanhGia(donHang);
                    danhGiaCuaToiFLP.Controls.Add(panel);
                }
            }

            if (danhGiaCuaToiFLP.Controls.Count == 0)
                danhGiaCuaToiFLP.Controls.Add(panel46);

            GUI_Utils.Instance.FitFLPHeight(danhGiaCuaToiFLP);
            danhGiaCuaToiPanel.Height = danhGiaCuaToiFLP.Height + 100;

        }

        private void DaDanhGiaButton_Click(object sender, EventArgs e)
        {
            foreach (Control control in DGHeaderPanel.Controls)
            {
                if (control.ForeColor == Color.Red)
                {
                    control.ForeColor = Color.Black;
                    break;
                }
            }
            Button button = sender as Button;
            button.ForeColor = Color.Red;

            danhGiaCuaToiFLP.Controls.Clear();

            foreach (DanhGia danhGia in khachHang.listDanhGia.list)
            {
                Panel panel = DrawDaDanhGia(danhGia);
                danhGiaCuaToiFLP.Controls.Add(panel);
            }

            if (danhGiaCuaToiFLP.Controls.Count == 0)
                danhGiaCuaToiFLP.Controls.Add(panel46);

            GUI_Utils.Instance.FitFLPHeight(danhGiaCuaToiFLP);
            danhGiaCuaToiPanel.Height = danhGiaCuaToiFLP.Height + 100;
        }
        private void DanhGiaViPhamButton_Click(object sender, EventArgs e)
        {
            foreach (Control control in DGHeaderPanel.Controls)
            {
                if (control.ForeColor == Color.Red)
                {
                    control.ForeColor = Color.Black;
                    break;
                }
            }
            Button button = sender as Button;
            button.ForeColor = Color.Red;

            int i = 0;
            danhGiaCuaToiFLP.Controls.Clear();
            foreach (DanhGia danhGia in BLL_DanhGia.Instance.GetAllDanhGiaViPhamFromMaKH(khachHang.maSo, out List<string> listLyDo).list)
            {
                Panel panel = DrawDanhGiaViPham(danhGia, listLyDo[i++]);
                danhGiaCuaToiFLP.Controls.Add(panel);
            }
            if (danhGiaCuaToiFLP.Controls.Count == 0)
                danhGiaCuaToiFLP.Controls.Add(panel46);

            GUI_Utils.Instance.FitFLPHeight(danhGiaCuaToiFLP);
            danhGiaCuaToiPanel.Height = danhGiaCuaToiFLP.Height + 100;
        }

        public Panel DrawDanhGiaViPham(DanhGia danhGia, string lyDo)
        {
            Panel panel = DrawDaDanhGia(danhGia);
            GUI_Utils.Instance.FindControl(panel, "tenShop").Text = "Lý Do Vi Phạm: " + lyDo;
            GUI_Utils.Instance.FindControl(panel, "control").Text = "Xóa Đánh Giá";
            GUI_Utils.Instance.FindControl(panel, "control").Click -= SuaDanhGiaButton_Click;
            GUI_Utils.Instance.FindControl(panel, "control").Click += XoaDanhGia_Click;

            ((PictureBox)GUI_Utils.Instance.FindControl(panel, "icon")).Image = Resources.error1;
            return panel;
        }

        private void XoaDanhGia_Click(object sender, EventArgs e)
        {
            MessageBox.Show("as");
        }

        public Panel DrawDaDanhGia(DanhGia danhGia)
        {
            Panel panel = new Panel
            {
                Size = panel48.Size,
                Margin = panel48.Margin,
                BackColor = Color.White,
                Parent = ListDGInBDPanel
            };
            panel.Paint += DrawPanelBorder;

            PictureBox shopIcon = new PictureBox
            {
                Name = "icon",
                Size = pictureBox42.Size,
                Location = pictureBox42.Location,
                BackColor = Color.White,
                Image = Resources.shop2,
                Parent = panel
            };
            panel.Controls.Add(shopIcon);

            TextBox tenShop = new TextBox
            {
                Name = "tenShop",
                Text = BLL_Shop.Instance.GetTenShopFromMaBD(danhGia.maBD),
                Size = textBox96.Size,
                Location = textBox96.Location,
                Font = textBox96.Font,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(tenShop);

            using (Bitmap bmp = new Bitmap(BLL_BaiDang.Instance.GetURLFromMaBD(danhGia.maBD)))
            {
                PictureBox image = new PictureBox
                {
                    Image = GUI_Utils.Instance.Resize(bmp, pictureBox41.Size),
                    Size = pictureBox41.Size,
                    Location = pictureBox41.Location,
                    BackColor = Color.White,
                    Parent = panel
                };
                panel.Controls.Add(image);
            }

            TextBox sanPhamDaMua = new TextBox
            {
                Text = "Sản phẩm đã mua: " + Utils.Instance.GioiHangKyTu(danhGia.sanPhamDaMua, 60),
                Location = textBox103.Location,
                Font = textBox103.Font,
                Size = textBox103.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(sanPhamDaMua);

            for (int i = 0; i < 5; i++)
            {
                PictureBox sao = new PictureBox
                {
                    Size = pictureBox48.Size,
                    Location = new Point(pictureBox48.Location.X + i * pictureBox48.Width, pictureBox48.Location.Y),
                    BorderStyle = BorderStyle.None,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.White,
                    Parent = panel
                };

                if (i < danhGia.sao)
                    sao.Image = Resources.star3;
                else
                    sao.Image = Resources.star4;

                panel.Controls.Add(sao);
            }

            TextBox date = new TextBox
            {
                Text = Utils.Instance.MoTaThoiGian(danhGia.ngayThem),
                Location = textBox101.Location,
                Font = textBox101.Font,
                Size = textBox101.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(date);

            if (danhGia.thietKeBia != "")
            {
                TextBox thietKeBia = new TextBox
                {
                    Text = "Thiết kế bìa: " + danhGia.thietKeBia,
                    Margin = textBox97.Margin,
                    Location = textBox97.Location,
                    Font = textBox97.Font,
                    Size = textBox97.Size,
                    BackColor = Color.White,
                    ForeColor = Color.DimGray,
                    BorderStyle = BorderStyle.None,
                    ReadOnly = true,
                    Parent = panel
                };
                panel.Controls.Add(thietKeBia);
            }

            if (danhGia.thietKeBia != "")
            {
                TextBox doiTuong = new TextBox
                {
                    Text = "Đối tượng đọc giả: " + danhGia.doiTuong,
                    Location = textBox98.Location,
                    Margin = textBox98.Margin,
                    Font = textBox98.Font,
                    Size = textBox98.Size,
                    BackColor = Color.White,
                    ForeColor = Color.DimGray,
                    BorderStyle = BorderStyle.None,
                    ReadOnly = true,
                    Parent = panel
                };
                panel.Controls.Add(doiTuong);
            }

            if (danhGia.noiDung != "")
            {
                TextBox noiDung = new TextBox
                {
                    Text = danhGia.noiDung,
                    Margin = textBox99.Margin,
                    Location = textBox99.Location,
                    Font = textBox99.Font,
                    Size = textBox99.Size,
                    BackColor = Color.White,
                    Multiline = true,
                    BorderStyle = BorderStyle.None,
                    ReadOnly = true,
                    Parent = panel
                };
                GUI_Utils.Instance.FitTextBoxMultiLines(noiDung);
                panel.Controls.Add(noiDung);
                panel.Height += noiDung.Height;
            }

            Button danhGiaButton = new Button
            {
                Name = "control",
                Text = "Sửa Đánh Giá",
                Size = button52.Size,
                Location = button52.Location,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.OrangeRed,
                ForeColor = Color.White,
                Parent = panel
            };
            danhGiaButton.Click += SuaDanhGiaButton_Click;
            danhGiaButton.FlatAppearance.BorderSize = 0;
            danhGiaButton.FlatAppearance.MouseDownBackColor = Color.Salmon;
            danhGiaButton.FlatAppearance.MouseOverBackColor = Color.Salmon;
            panel.Controls.Add(danhGiaButton);

            return panel;
        }

        private void SuaDanhGia(params DanhGia[] danhGia)
        {
            BLL_KhachHang.Instance.SuaDanhGia(khachHang, danhGia[0]);
        }

        private void SuaDanhGiaButton_Click(object sender, EventArgs e)
        {
            int index = danhGiaCuaToiFLP.Controls.IndexOf(((Control)sender).Parent);

            DimForm dimForm = new DimForm();
            dimForm.Show();
            DanhGiaForm form = new DanhGiaForm(SuaDanhGia, khachHang.listDanhGia.list[index]);
            form.ShowDialog();
            dimForm.Close();
        }

        public Panel DrawChuaDanhGia(DonHang donHang)
        {
            List<DanhGia> list = BLL_DanhGia.Instance.TaoDGMoiTuDH(donHang);


            Panel panel = new Panel
            {
                Size = panel47.Size,
                Margin = panel47.Margin,
                BackColor = Color.White,
                Parent = danhGiaCuaToiFLP,
            };
            panel.Paint += DrawPanelBorder;

            PictureBox shopIcon = new PictureBox
            {
                Size = pictureBox39.Size,
                Location = pictureBox39.Location,
                BackColor = Color.White,
                Image = Resources.shop2,
                Parent = panel
            };
            panel.Controls.Add(shopIcon);

            TextBox tenShop = new TextBox
            {
                Text = BLL_Shop.Instance.GetTenShopFromMaS(donHang.maS),
                Size = textBox90.Size,
                Location = textBox90.Location,
                Font = textBox90.Font,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(tenShop);

            using (Bitmap bmp = new Bitmap(BLL_BaiDang.Instance.GetURLFromMaBD(list[0].maBD)))
            {
                PictureBox image = new PictureBox
                {
                    Image = GUI_Utils.Instance.Resize(bmp, pictureBox40.Size),
                    Size = pictureBox40.Size,
                    Location = pictureBox40.Location,
                    BackColor = Color.White,
                    Parent = panel
                };
                panel.Controls.Add(image);
            }

            TextBox maDonHang = new TextBox
            {
                Name = "maDH",
                Text = "Mã đơn hàng: DH" + donHang.maDH,
                Font = textBox100.Font,
                Size = textBox100.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                ReadOnly = true,
                Location = textBox58.Location,
                Cursor = Cursors.IBeam,
                TextAlign = HorizontalAlignment.Right,
                BorderStyle = BorderStyle.None,
                Parent = panel
            };
            panel.Controls.Add(maDonHang);

            TextBox tieuDeBD = new TextBox
            {
                Text = BLL_BaiDang.Instance.GetTieuDeFromMaBD(list[0].maBD),
                Size = textBox91.Size,
                Font = textBox91.Font,
                Location = textBox91.Location,
                Multiline = true,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                BackColor = Color.White,
                Parent = panel
            };
            GUI_Utils.Instance.FitTextBoxMultiLines(tieuDeBD);
            panel.Controls.Add(tieuDeBD);

            if(list.Count > 1)
            {
                TextBox soLuongSP = new TextBox
                {
                    Text = list.Count + " Sản Phẩm",
                    Size = textBox92.Size,
                    Location = textBox92.Location,
                    Font = textBox92.Font,
                    BackColor = Color.White,
                    ForeColor = Color.DimGray,
                    BorderStyle = BorderStyle.None,
                    ReadOnly = true,
                    Parent = panel
                };
                panel.Controls.Add(soLuongSP);
            }

            Button danhGiaButton = new Button
            {
                Text = "Đánh Giá (+" + list.Count * 200 + ")",
                Size = button50.Size,
                Location = button50.Location,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.OrangeRed,
                ForeColor = Color.White,
                Parent = panel
            };
            danhGiaButton.Click += DanhGiaInDGCTButton_Click;
            danhGiaButton.FlatAppearance.BorderSize = 0;
            danhGiaButton.FlatAppearance.MouseDownBackColor = Color.Salmon;
            danhGiaButton.FlatAppearance.MouseOverBackColor = Color.Salmon;
            
            panel.Controls.Add(danhGiaButton);

            return panel;
        }

        private void DanhGiaInDGCTButton_Click(object sender, EventArgs e)
        {
            currMaDH = GUI_Utils.Instance.FindControl(((Button)sender).Parent as Panel, "maDH").Text.Substring(15);
            DanhGiaButton_Click(null, null);
        }

        private void DanhGiaCuaToiButton_Click(object sender, EventArgs e)
        {
            SwitchPanel(ref currChildPanel, ref danhGiaCuaToiPanel);
            ChangeColorOfButtonInFLP(sender as Button);
            ChuaDanhGiaButton_Click(chuaDanhGiaButton, null);
        }

        private Panel DrawThongBaoHeThongPanel(ThongBao thongBao)
        {
            Panel panel = new Panel
            {
                Size = panel52.Size,
                BackColor = Color.White,
                Margin = panel52.Margin,
                Parent = TBDonHangFLP
            };
            panel.Paint += DrawPanelBorder;

            PictureBox pictureBox = new PictureBox
            {
                Image = Resources.error2,
                Size = pictureBox50.Size,
                Location = pictureBox50.Location,
                BorderStyle = BorderStyle.None,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                Parent = panel
            };
            panel.Controls.Add(pictureBox);

            TextBox noiDung = new TextBox
            {
                Text = thongBao.noiDung,
                Location = textBox11.Location,
                Font = textBox11.Font,
                Size = textBox11.Size,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            GUI_Utils.Instance.FitTextBox(noiDung);
            panel.Controls.Add(noiDung);

            TextBox thoiGian = new TextBox
            {
                Text = Utils.Instance.MoTaThoiGian(thongBao.ngayGui),
                Location = textBox71.Location,
                Font = textBox71.Font,
                Size = textBox71.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(thoiGian);

            return panel;
        }

        private void TBHTButton_Click(object sender, EventArgs e)
        {
            tbHeThongFLP.Controls.Clear();
            tbHeThongFLP.Height = 0;
            foreach (ThongBao thongBao in khachHang.listThongBao.list)
            {
                if (thongBao.from == "HeThong")
                {
                    Panel panel = DrawThongBaoHeThongPanel(thongBao);
                    tbHeThongFLP.Controls.Add(panel);
                    tbHeThongFLP.Controls.SetChildIndex(panel, 0);
                }
            }
            if (tbHeThongFLP.Controls.Count == 0)
                tbHeThongFLP.Controls.Add(khongCoTBPanel);

            GUI_Utils.Instance.FitFLPHeight(tbHeThongFLP);

            capNhatDHPanel.Height = tbHeThongFLP.Height + 50;

            SwitchPanel(ref currChildPanel, ref tbHeThongPanel);
            ChangeColorOfButtonInFLP(sender as Button);
        }
    }
}