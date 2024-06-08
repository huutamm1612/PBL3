using Microsoft.Win32;
using Program.BLL;
using Program.DAL;
using Program.DTO;
using Program.GUI;
using Program.Properties;
using System;
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
        private QLBaiDang qlBaiDang = null;
        private BaiDang currBaiDang = null;
        private SanPham currSanPham = null;
        private DiaChi currDiaChi = null;
        private Shop currShop = null;
        private string URL = null;
        private string currMaDH = null;

        private delegate void GoToBaiDang(object sender, EventArgs e);

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

        public void DangNhapThanhCong()
        {
            KhachHang_Panel.Visible = true;
            HeaderPannel.Visible = true;
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

            }
        }

        public void SetData(string taiKhoan, string matKhau)
        {
            user = BLL_User.Instance.DangNhap(taiKhoan, matKhau);

            khachHang = BLL_KhachHang.Instance.GetKhachHangFromTaiKhoan(user.taiKhoan);
            BLL_User.Instance.LuuTaiKhoan(user);
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
                    TBDonHangFLP.Height += panel.Height + panel.Margin.Top + panel.Margin.Bottom;
                }
            }

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
                MessageBox.Show("Đổi mật khẩu thành công!!!");

                if (saiMKC_Text.Visible)
                    saiMKC_Text.Visible = false;

                matKhauCu_Box.Clear();
                matKhauMoi1_Box.Clear();
                matKhauMoi2_Box.Clear();
            }
        }

        private void SwitchPanel(ref Panel currPanel, ref Panel newPanel)
        {
            waittingForm.BringToFront();
            waittingForm.Size = currPanel.Size;
            waittingForm.Visible = true;

            currPanel.Visible = false;
            currPanel = newPanel;

            currPanel.SendToBack();
            currPanel.AutoScrollPosition = new Point(0, 0);
            currPanel.Visible = true;

            waittingForm.Visible = false;
            currPanel.BringToFront();
        }

        private void home_Button_Click(object sender, EventArgs e)
        {
            SwitchPanel(ref currPanel, ref HomePanel);
        }

        private void userProfile_Button_Click(object sender, EventArgs e)
        {
            currChildPanel = profilePanel;
            SwitchPanel(ref currPanel, ref UserPanel);
            SpreadOutClick(myAccount_Button, e);
            UserAccountButton_Click(myAccount_Button, null);

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

            if (URL != "")
                khachHang.avt = Utils.Instance.GetImageURL(System.Drawing.Image.FromFile(URL));

            khachHang.Sua(ten_UP_Box.Text, email_UP_Box.Text, soDT_UP_Box.Text, gioiTinh, ngaySinh);
            DAL_KhachHang.Instance.CapNhatKhachHang(khachHang);
            MessageBox.Show("Đã lưu thành công!!!");
            SwitchPanel(ref currChildPanel, ref profilePanel);
        }

        public void ThemDiaChi(DiaChi diaChi)
        {
            BLL_DiaChi.Instance.ThemDiaChi(khachHang, diaChi);
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
            DangNhap_Form form = new DangNhap_Form(SetData);
            Hide();
            form.ShowDialog();
            Show();
        }

        private void SignUp_Button_Click(object sender, EventArgs e)
        {
            DangNhap_Form form = new DangNhap_Form(SetData, false);
            Hide();
            form.ShowDialog();
            Show();
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

            listDiaChi_FLPanel.Height = height;
            diaChiUser_Panel.Height = height + 200;
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
            dangNhap_Button.Visible = true;
            SignUp_Button.Visible = true;
            HomePanel.Visible = true;
            UserPanel.Visible = false;
            userProfile_Button.Visible = false;
            littleMenuPanel.Visible = false;
            flowLayoutPanel6.Visible = false;
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
                return;

            Hide();
            BanHang_Form BHForm = new BanHang_Form();
            sendData send = new sendData(BHForm.setData);
            send(user.taiKhoan, user.matKhau);
            BHForm.ShowDialog();
            Close();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {

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
            Control pic = sender as Control;
            MouseHoverPanel(pic.Parent, e);
        }

        private void MouseMoveObjInPanel(object sender, MouseEventArgs e)
        {
            Control pic = sender as Control;
            MouseMovePanel(pic.Parent, e);
        }

        private void MouseLeaveObjInPanel(object sender, EventArgs e)
        {
            Control pic = sender as Control;
            MouseLeavePanel(pic.Parent, e);
        }


        private void SetPageFLP(FlowLayoutPanel flp, int currPageNumber, int numPage)
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
                    };

                    button.FlatAppearance.BorderSize = 0;

                    if (i == currPageNumber)
                    {
                        button.BackColor = Color.OrangeRed;
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
                        Cursor = Cursors.Hand,
                        Text = i.ToString(),
                        BackColor = flp.BackColor,
                        Size = new Size(45, 45),
                        FlatStyle = FlatStyle.Flat,
                    };

                    button.FlatAppearance.BorderSize = 0;

                    if (i == currPageNumber.ToString())
                    {
                        button.BackColor = Color.OrangeRed;
                        button.FlatAppearance.MouseDownBackColor = Color.OrangeRed;
                        button.FlatAppearance.MouseOverBackColor = Color.OrangeRed;
                    }
                    else
                    {
                        button.FlatAppearance.MouseDownBackColor = flp.BackColor;
                        button.FlatAppearance.MouseOverBackColor = flp.BackColor;
                    }

                    if (i == "...")
                    {

                    }
                    flp.Controls.Add(button);
                    flp.Controls.SetChildIndex(button, index);
                    index++;
                }
            }

            flp.Size = new Size(51 * flp.Controls.Count + 6, 51);
            flp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flp.Location = new Point(1326 / 2 - flp.Width / 2, 66 / 2 - flp.Height / 2);
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

        private void ChuyenTrangBaiDang(QLBaiDang qlbd, FlowLayoutPanel flpBaiDang, FlowLayoutPanel flpPage, GoToBaiDang func)
        {
            Control titlePanel = flpBaiDang.Controls[0];
            Panel pagePanel = flpBaiDang.Controls[flpBaiDang.Controls.Count - 1] as Panel;

            flpBaiDang.Controls.Clear();
            GC.Collect();
            flpBaiDang.Controls.Add(titlePanel);

            int currPage = CurrPageNumber(flpPage);
            int tmp = 0;
            for (int i = (currPage - 1) * 24; i < currPage * 24; i++)
            {
                if (i == qlbd.list.Count)
                    break;
                flpBaiDang.Controls.Add(DrawBaiDang(qlbd.list[i], flpBaiDang, func));
                tmp++;
            }

            flpBaiDang.Size = new Size(flpBaiDang.Width, 160 + 316 * ((tmp - 1) / 6 + 1));
            flpBaiDang.Controls.Add(pagePanel);
            flpBaiDang.Controls.SetChildIndex(pagePanel, flpBaiDang.Controls.Count - 1);
        }

        private void HomePanel_Load(object sender, EventArgs e)
        {
            if (HomePanel.Visible == false)
            {


                FLPBaiDang1.Controls.Clear();
                FLPBaiDang1.Controls.Add(titlePanel1);
                FLPBaiDang1.Controls.Add(PageListP);

                HeaderPannel.Width = Width;
            }
            else
            {
                qlBaiDang = BLL_BaiDang.Instance.GetBaiDangForHomeList();
                HomePanel.AutoScrollPosition = new Point(0, 0);

                SetPageFLP(PageListFLP, 1, (qlBaiDang.list.Count - 1) / 24 + 1);
                ChuyenTrangBaiDang(qlBaiDang, FLPBaiDang1, PageListFLP, DenBaiDangTuHome);
            }
        }

        private void DenBaiDangTuHome(object sender, EventArgs e)
        {
            Control obj = sender as Control;
            Panel panel = obj.Parent as Panel;
            int panelIndex = FLPBaiDang1.Controls.IndexOf(panel);
            int currPage = CurrPageNumber(PageListFLP);

            currBaiDang = qlBaiDang.list[(currPage - 1) * 24 + panelIndex - 1];
            currShop = DAL_Shop.Instance.LoadShopFromMaBD(currBaiDang.maBD);
            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        private Panel DrawBaiDang(BaiDang baiDang, FlowLayoutPanel parent, GoToBaiDang func)
        {
            Panel panel = new Panel
            {
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
                Text = "₫" + Utils.Instance.SetGia(baiDang.giaMin()),
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

        private void nextPageButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            FlowLayoutPanel flp = (FlowLayoutPanel)button.Parent;

            int n = CurrPageNumber(flp);

            HomePanel.AutoScrollPosition = new Point(39, 790);
            SetPageFLP(flp, n + 1, (qlBaiDang.list.Count - 1) / 24 + 1);
            ChuyenTrangBaiDang(qlBaiDang, FLPBaiDang1, flp, DenBaiDangTuHome);
        }

        private void prevPageButton_Click(object sender, EventArgs e)
        {
            int n = CurrPageNumber(PageListFLP);
            if (n == 1)
                return;

            HomePanel.AutoScrollPosition = new Point(39, 790);
            SetPageFLP(PageListFLP, n - 1, (qlBaiDang.list.Count - 1) / 24 + 1);
            ChuyenTrangBaiDang(qlBaiDang, FLPBaiDang1, PageListFLP, DenBaiDangTuHome);
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

        private void DenBaiDangTuBD(object sender, EventArgs e)
        {
            int index = coTheBanCungThichFLP.Controls.IndexOf(((Control)sender).Parent) - 1;
            currBaiDang = qlBaiDang.list[index];
            currShop = DAL_Shop.Instance.LoadShopFromMaBD(currBaiDang.maBD);
            currSanPham = null;
            SwitchPanel(ref currPanel, ref BaiDangPanel);
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

        private void followButton_Click(object sender, EventArgs e)
        {
            if (khachHang == null)
            {
                GoToLoginForm();
                SwitchPanel(ref currPanel, ref BaiDangPanel);
                Show();
            }

            if (BLL_KhachHang.Instance.DaTheoDoi(khachHang.listFollow, currShop.maSo))
            {
                BLL_KhachHang.Instance.TheoDoi(khachHang, currShop);
                followButton.Text = "Hủy theo dõi";
            }
            else
            {
                BLL_KhachHang.Instance.HuyTheoDoi(khachHang, currShop);
                followButton.Text = "Theo dõi";
            }
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
            DangNhap_Form form = new DangNhap_Form(SetData);
            Hide();
            form.ShowDialog();
        }

        private void addToCartButton_Click(object sender, EventArgs e)
        {
            if (currSanPham == null)
                return;

            if (khachHang != null)
            {
                BLL_GioHang.Instance.ThemSPVaoGioHang(khachHang.gioHang, currSanPham, int.Parse(soLuongTxt.Text));
                RefreshCartButton();
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
            Panel panel = sender as Panel;

            using (var pen = new Pen(Color.DarkGray, 1))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(-1, -1, panel.Width + 2, panel.Height));
            }
        }

        private void DenBaiDangTuGioHang(object sender, EventArgs e)
        {
            int index = listSPTrongGHFLP.Controls.IndexOf(((Control)sender).Parent);
            currBaiDang = BLL_BaiDang.Instance.GetBaiDangFromMaBD(khachHang.gioHang.list[index].maBD);
            currShop = DAL_Shop.Instance.LoadShopFromMaBD(currBaiDang.maBD);
            currSanPham = khachHang.gioHang.list[index];

            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        private Panel DrawSPTrongGioHang(SanPham sanPham)
        {
            int giamGia = BLL_BaiDang.Instance.GetGiamGiaFromMaSP(sanPham.maSP);
            Panel panel = new Panel
            {
                Size = spPanel.Size,
                BackColor = Color.White,
                Margin = spPanel.Margin,
                Parent = listSPTrongGHFLP
            };

            panel.Paint += DrawPanelBorder;

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

            if (((CheckBox)GUI_Utils.Instance.FindControl(panel, "ChoseCheckBox")).Checked)
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

            home_Button_Click(null, null);
            RefreshCartButton();
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

                foreach (SanPham sanPham in khachHang.gioHang.list)
                {
                    listSPTrongGHFLP.Controls.Add(DrawSPTrongGioHang(sanPham));
                }

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
                this.Controls.Add(HeaderPannel);
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
                BaiDangPanel.Controls.Add(HeaderPannel);
                HeaderPannel.Location = new Point(0, 0);
                waittingPanel.SendToBack();

                if (khachHang != null)
                {
                    currDiaChi = khachHang.diaChi;
                    vanChuyenTxt.Text = BLL_DiaChi.Instance.MoTaDiaChi(currDiaChi);
                    GUI_Utils.Instance.FitTextBox(vanChuyenTxt);

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

                nDanhGiaTxt.Text = currShop.listBaiDang.SoLuongDanhGia().ToString();
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

                    titleTxt.Text = currBaiDang.tieuDe;
                    GUI_Utils.Instance.FitTextBox(titleTxt, 20, 10);

                    titleTxt.Size = new Size(750, titleTxt.Height);
                    giamGiaTxt.Text = "GIẢM " + currBaiDang.giamGia.ToString() + "%";
                    GUI_Utils.Instance.FitTextBox(giamGiaTxt, 0, 0);

                    Sao5Button.Text = "5 Sao(" + currBaiDang.listDanhGia.SoluongDanhGia(5) + ")";
                    Sao4Button.Text = "4 Sao(" + currBaiDang.listDanhGia.SoluongDanhGia(4) + ")";
                    Sao3Button.Text = "3 Sao(" + currBaiDang.listDanhGia.SoluongDanhGia(3) + ")";
                    Sao2Button.Text = "2 Sao(" + currBaiDang.listDanhGia.SoluongDanhGia(2) + ")";
                    Sao1Button.Text = "1 Sao(" + currBaiDang.listDanhGia.SoluongDanhGia(1) + ")";

                    ListDGInBDPanel.Height = 0;
                    foreach (DanhGia danhGia in currBaiDang.listDanhGia.list)
                    {
                        Panel panel = DrawDanhGiaInBaiDang(danhGia);
                        ListDGInBDPanel.Controls.Add(panel);
                        ListDGInBDPanel.Height += panel.Height + panel.Margin.Top + panel.Margin.Bottom;
                    }
                    DGPanelInBDPanel.Height = ListDGInBDPanel.Height + 230;
                }

                spKhacCuaShopFLP.Controls.Clear();

                int i = 0;

                foreach (BaiDang baiDang in currShop.listBaiDang.list)
                {
                    if (i++ < 20)
                        spKhacCuaShopFLP.Controls.Add(DrawBaiDang(baiDang, spKhacCuaShopFLP, DenBaiDangTuBDShop));
                }

                qlBaiDang = DAL_BaiDang.Instance.LoadALLBaiDang();
                SetPageFLP(PageListBDFLP, 1, (qlBaiDang.list.Count - 1) / 24 + 1);
                ChuyenTrangBaiDang(qlBaiDang, coTheBanCungThichFLP, PageListBDFLP, DenBaiDangTuBD);

                int height = 0;
                foreach (Control control in baiDangFLP.Controls)
                {
                    height += control.Height + 20;
                }
                baiDangFLP.Size = new Size(baiDangFLP.Width, height);

                BaiDangPanel.Visible = true;
                BaiDangPanel.BringToFront();
                waittingForm.Visible = false;
            }
            else
            {
                coTheBanCungThichFLP.Controls.Clear();
                coTheBanCungThichFLP.Controls.Add(textBox51);
                coTheBanCungThichFLP.Controls.Add(PageListBDP);

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
                thanhToanPanel.Controls.Add(HeaderPannel);
                waittingPanel.SendToBack();

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
                tongTien += phiVanChuyen;
                if (dungXuCB.Checked)
                {
                    daDungXuTxt.Text = "₫" + Utils.Instance.SetGia(khachHang.xu);
                    tongTien -= khachHang.xu;
                }
                tongThanhToanTxt.Text = "₫" + Utils.Instance.SetGia(tongTien);

                listDonHangFLP.Controls.Add(thongTinKhacFLP);

                int height = 0;
                foreach (Control control in listDonHangFLP.Controls)
                {
                    height += control.Height + 20;
                }
                listDonHangFLP.Size = new Size(listDonHangFLP.Width, height);
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

        private void UserPanel_VisibleChanged(object sender, EventArgs e)
        {

            if (UserPanel.Visible)
            {
                waittingPanel.BringToFront();
                UserPanel.Controls.Add(HeaderPannel);
                waittingPanel.SendToBack();
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
            else
            {
                currChildPanel.Visible = false;
                currChildPanel = profilePanel;
                waittingPanel.BringToFront();
                this.Controls.Add(HeaderPannel);
                HeaderPannel.Visible = true;
                HeaderPannel.BringToFront();
                waittingPanel.SendToBack();
            }
        }

        private void diaChiUser_Panel_VisibleChanged(object sender, EventArgs e)
        {
            if (diaChiUser_Panel.Visible)
            {
                if (listDiaChi_FLPanel.Controls.Count == 0)
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
                    Panel panel = DrawDonHangPanel(donHang);
                    DonHangFLP.Controls.Add(panel);
                    DonHangFLP.Controls.SetChildIndex(panel, 0);
                    height += panel.Height + panel.Margin.Bottom + panel.Margin.Top;
                }
            }
            DonHangFLP.Height = height;
            DonMuaPanel.Height = height + 50 + HeaderDHPanel.Height;
        }

        private Panel DrawSPPanelInDHFLP(SanPham sanPham, FlowLayoutPanel parent)
        {
            Panel panel = new Panel
            {
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

            foreach(Button control in listButtonSaoFLP.Controls)
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

            int sao;

            if (button.Text == "Tất Cả")
                sao = 5;
            else
                sao = int.Parse(button.Text.Substring(0, 1));

            ListDGInBDPanel.Controls.Clear();
            ListDGInBDPanel.Height = 0;


            if (currBaiDang.listDanhGia.SoluongDanhGia(sao) == 0)
            {
                ListDGInBDPanel.Controls.Add(KhongCoDanhGiaPanel);
                ListDGInBDPanel.Height = KhongCoDanhGiaPanel.Height;
                DGPanelInBDPanel.Height = 230 + ListDGInBDPanel.Height;
            }
            else
            {
                foreach (DanhGia danhGia in currBaiDang.listDanhGia.list)
                {
                    if (danhGia.sao == sao)
                    {
                        Panel panel = DrawDanhGiaInBaiDang(danhGia);
                        ListDGInBDPanel.Controls.Add(panel);
                        ListDGInBDPanel.Height += panel.Height + panel.Margin.Top + panel.Margin.Bottom;
                    }
                }
                DGPanelInBDPanel.Height = ListDGInBDPanel.Height + 230;
            }

            int height = 0;
            foreach (Control control in baiDangFLP.Controls)
            {
                height += control.Height + 20;
            }
            baiDangFLP.Size = new Size(baiDangFLP.Width, height);
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
                BackColor = Color.White,
                Margin = panel22.Margin,
                Parent = flp
            };
            headPanel.Paint += DrawPanelBorder;

            Button xemShop = new Button
            {
                Size = button10.Size,
                BackColor = Color.White,
                Font = button10.Font,
                Location = button10.Location,
                Text = "Xem Shop",
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Parent = headPanel
            };
            //xemShop.Click += ...
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
                //button.Click += ...
            }
            else if (donHang.tinhTrang == 1) // van chuyen
            {
                if (DateTime.Compare(DateTime.Now, donHang.ngayGiaoHang) >= 0)
                {
                    button.Text = "Đã nhận hàng";
                    button.Click += DaNhanHangButton_Click;
                }
                else
                    button.Text = "Xem tình trạng giao hàng";
                //button.Click += ...
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

                //button.Click += ...
            }
            else if (donHang.tinhTrang == 3 || donHang.tinhTrang == -1)
            {
                button.Text = "Mua lại";
            }

            tailPanel.Controls.Add(button);

            flp.Height = height + tailPanel.Height + tailPanel.Margin.Top + tailPanel.Margin.Bottom;

            return flp;
        }

        private void DaNhanHangButton_Click(object sender, EventArgs e)
        {
            string maDH = GUI_Utils.Instance.GetMaDHByClick(sender);
            MessageBox.Show(maDH);
            BLL_KhachHang.Instance.NhanHang(khachHang, maDH);
            MessageBox.Show("Nhận hàng thành công");
        }

        private void DanhGiaButton_Click(Object sender, EventArgs e)
        {
            string maDH = GUI_Utils.Instance.GetMaDHByClick(sender);
            currMaDH = maDH;

            DimForm dimForm = new DimForm();
            dimForm.Show();
            DanhGiaForm form = new DanhGiaForm(SetData, khachHang.listDonHang.GetDonHangFromMaDH(maDH));
            form.ShowDialog();
            dimForm.Close();

        }

        private void SetData(params DanhGia[] listDanhGia)
        {
            foreach(DanhGia danhGia in listDanhGia)
            {
                BLL_KhachHang.Instance.DanhGia(khachHang, danhGia, currMaDH);
            }
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

            for(int i = 0; i < 5; i++)
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

            TextBox thietKeBia = new TextBox
            {
                Text = "Thiết kế bìa: " + danhGia.thietKeBia,
                Location = textBox67.Location,
                Font = textBox67.Font,
                Size = textBox67.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(thietKeBia);

            TextBox doiTuong = new TextBox
            {
                Text = "Đối tượng đọc giả: " + danhGia.doiTuong,
                Location = textBox68.Location,
                Font = textBox68.Font,
                Size = textBox68.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            panel.Controls.Add(doiTuong);

            TextBox noiDung = new TextBox
            {
                Text = danhGia.noiDung,
                Location = textBox69.Location,
                Font = textBox69.Font,
                Size = textBox69.Size,
                BackColor = Color.White,
                Multiline = true,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Parent = panel
            };
            Size size = TextRenderer.MeasureText(noiDung.Text, noiDung.Font, new Size(noiDung.Width, int.MaxValue), TextFormatFlags.WordBreak);
            noiDung.Height = size.Height + noiDung.Margin.Vertical;
            panel.Controls.Add(noiDung);


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
                Parent = panel
            };
            panel.Controls.Add(like);


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
                Parent = panel
            };
            panel.Controls.Add(huuIch);

            panel.Height = 265 + noiDung.Height;

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
                    Cursor = Cursors.Hand,
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
                Cursor = Cursors.Hand,
                Parent = panel
            };
            panel.Controls.Add(tinhTrang);

            TextBox noiDung = new TextBox
            {
                Text = thongBao.noiDung,
                Location = textBox59.Location,
                Font = textBox59.Font,
                Size = textBox59.Size,
                BackColor = Color.White,
                ForeColor = Color.DimGray,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Cursor = Cursors.Hand,
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
                Cursor = Cursors.Hand,
                Parent = panel
            };
            panel.Controls.Add(thoiGian);


            return panel;
        }

        private void MouseOutPanel(object sender, EventArgs e)
        {

        }

        private void MouseInPanel(object sender, EventArgs e)
        {

        }

        private void MouseIn(object sender, MouseEventArgs e)
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
            xuHienCoText.Text = Utils.Instance.SetGia(khachHang.xu) + " Xu hiện có";
        }
    }
}