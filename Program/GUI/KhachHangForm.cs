using Program.BLL;
using Program.DAL;
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

        }

        private void hienMK_UP_Check_CheckedChanged(object sender, EventArgs e)
        {
            this.HienMatKhau(hienMK_UP_Check, matKhauCu_Box);
            this.HienMatKhau(hienMK_UP_Check, matKhauMoi1_Box);
            this.HienMatKhau(hienMK_UP_Check, matKhauMoi2_Box);
        }

        private void SpreadOutClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int index = funcFLPanel.Controls.IndexOf(button) + 1;

            bool spread = !funcFLPanel.Controls[index].Visible;


            while (funcFLPanel.Controls[index].Text != "")
            {
                funcFLPanel.Controls[index].Visible = spread;
                index++;
            }
        }

        private void MouseIn(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (!button.Font.Bold)
                button.ForeColor = Color.Salmon;
        }
        private void MouseOut(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (!button.Font.Bold)
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
            SwitchPanel(ref currChildPanel, ref profilePanel);
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

            if(URL != "") 
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
            foreach(Control control in listDiaChi_FLPanel.Controls)
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
            flpBaiDang.Controls.Add(titlePanel);

            int currPage = CurrPageNumber(flpPage);
            int tmp = 0;
            for (int i = (currPage - 1) * 24; i < currPage * 24; i++)
            {
                if (i == qlbd.list.Count)
                    break;
                flpBaiDang.Controls.Add(drawBaiDang(qlbd.list[i], func));
                tmp++;
            }

            flpBaiDang.Size = new Size(1335, 160 + 316 * ((tmp - 1) / 6 + 1));
            flpBaiDang.Controls.Add(pagePanel);
            flpBaiDang.Controls.SetChildIndex(pagePanel, flpBaiDang.Controls.Count - 1);
        }

        private void HomePanel_Load(object sender, EventArgs e)
        {
            if (HomePanel.Visible == false)
            {

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
            Panel panel = obj.Parent.Parent as Panel;
            int panelIndex = FLPBaiDang1.Controls.IndexOf(panel);
            int currPage = CurrPageNumber(PageListFLP);

            currBaiDang = qlBaiDang.list[(currPage - 1) * 24 + panelIndex - 1];
            currShop = DAL_Shop.Instance.LoadShopFromMaBD(currBaiDang.maBD);
            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        private Panel drawBaiDang(BaiDang baiDang, GoToBaiDang func)
        {
            Panel backPanel = new Panel
            {
                Name = "backPanel",
                BackColor = Color.WhiteSmoke,
                Size = new Size(216, 310),
                BorderStyle = BorderStyle.None,
            };

            Panel parentPanel = new Panel
            {
                Name = "parentPanel",
                BackColor = Color.White,
                Size = new Size(206, 300),
                Location = new Point(5, 5),
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None
            };
            parentPanel.MouseLeave += new EventHandler(MouseLeavePanel);
            parentPanel.MouseHover += new EventHandler(MouseHoverPanel);
            parentPanel.MouseMove += new MouseEventHandler(MouseMovePanel);
            backPanel.Controls.Add(parentPanel);

            PictureBox image = new PictureBox
            {
                Image = GUI_Utils.Instance.Resize(System.Drawing.Image.FromFile(baiDang.anhBia), new Size(206, 206)),
                BackColor = Color.White,
                Size = new Size(206, 206),
                Name = "image",
                Location = new Point(0, 0),
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                SizeMode = PictureBoxSizeMode.Zoom

            };
            image.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
            image.MouseHover += new EventHandler(MouseHoverObjInPanel);
            image.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
            image.Click += new EventHandler(func);
            parentPanel.Controls.Add(image);

            TextBox title = new TextBox
            {
                Name = "title",
                Multiline = true,
                Text = baiDang.tieuDe,
                Size = new Size(196, 55),
                Location = new Point(5, 215),
                BackColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.Black,
                ReadOnly = true
            };
            title.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
            title.MouseHover += new EventHandler(MouseHoverObjInPanel);
            title.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
            title.Click += new EventHandler(func);
            parentPanel.Controls.Add(title);

            TextBox price = new TextBox
            {
                Name = "price",
                Text = "₫" + Utils.Instance.SetGia(baiDang.giaMin()),
                Size = new Size(100, 23),
                Location = new Point(5, 272),
                BackColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.Red,
                ReadOnly = true
            };
            price.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
            price.MouseHover += new EventHandler(MouseHoverObjInPanel);
            price.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
            price.Click += new EventHandler(func);
            parentPanel.Controls.Add(price);

            TextBox sold = new TextBox
            {
                Name = "sold",
                Text = "Đã bán " + baiDang.luocBan().ToString(),
                Size = new Size(90, 17),
                Location = new Point(110, 273),
                BackColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Cursor = Cursors.Hand,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true
            };
            sold.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
            sold.MouseHover += new EventHandler(MouseHoverObjInPanel);
            sold.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
            sold.Click += new EventHandler(func);
            parentPanel.Controls.Add(sold);

            TextBox discount = new TextBox
            {
                Name = "discount",
                Text = "-" + baiDang.giamGia.ToString() + "%",
                Size = new Size(38, 17),
                Location = new Point(168, 0),
                BackColor = Color.Gold,
                Font = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Cursor = Cursors.Hand,
                ForeColor = Color.Red,
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true
            };
            discount.MouseLeave += new EventHandler(MouseLeaveObjInPanel);
            discount.MouseHover += new EventHandler(MouseHoverObjInPanel);
            discount.MouseMove += new MouseEventHandler(MouseMoveObjInPanel);
            discount.Click += new EventHandler(func);
            parentPanel.Controls.Add(discount);

            return backPanel;
        }

        private void nextPageButton_Click(object sender, EventArgs e)
        {
            int n = CurrPageNumber(PageListFLP);
            if (n == 10)
                return;

            HomePanel.AutoScrollPosition = new Point(39, 790);
            SetPageFLP(PageListFLP, n + 1, (qlBaiDang.list.Count - 1) / 24 + 1);
            ChuyenTrangBaiDang(qlBaiDang, FLPBaiDang1, PageListFLP, DenBaiDangTuHome);
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
            int index = coTheBanCungThichFLP.Controls.IndexOf(((Control)sender).Parent.Parent) - 1;
            currBaiDang = qlBaiDang.list[index];
            currShop = DAL_Shop.Instance.LoadShopFromMaBD(currBaiDang.maBD);
            currSanPham = null;
            SwitchPanel(ref currPanel, ref BaiDangPanel);
        }

        private void DenBaiDangTuBDShop(object sender, EventArgs e)
        {
            int index = spKhacCuaShopFLP.Controls.IndexOf(((Control)sender).Parent.Parent);
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
            if(khachHang == null)
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

            if(khachHang != null)
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
            if(Cursor.Position.Y < 35 || (Cursor.Position.X < 1273 || Cursor.Position.X > 1440))
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
            noiDungPanel.Top = 596;
        }

        private void GioHangPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if (noiDungPanel.Top - e.Delta > panel13.Top)
                noiDungPanel.Top = panel13.Top;
            else
                noiDungPanel.Top = 596;
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

        private void DenBaiDangTuGioHang (object sender, EventArgs e)
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
                if(result == DialogResult.Yes)
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

            if(((CheckBox)GUI_Utils.Instance.FindControl(panel, "ChoseCheckBox")).Checked)
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
            foreach(Panel panel in listSPTrongGHFLP.Controls)
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
                Text = DAL_Shop.Instance.GetTenShopFromMaSP(list.list[0].maSP),
                Size = TxtTenShopDH.Size,
                BackColor = Color.White,
                Cursor = Cursors.IBeam,
                ReadOnly = true,
                BorderStyle = BorderStyle.None
            };
            headerPanel.Controls.Add(tenShop);

            flp.Controls.Add(headerPanel);

            foreach(SanPham sanPham in list.list)
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
            if(qlSanPham.list.Count > 0)
            {
                SwitchPanel(ref currPanel, ref thanhToanPanel);
            }
            else
            {

            }
        }

        private void chonPTTT_Click(object sender, EventArgs e)
        {
            foreach(Button button in listPTTTFLP.Controls)
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

            if(currSanPham != null)
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
            else if(CKNganHangButton.ForeColor == Color.OrangeRed)
                ptThanhToan = 1;

            BLL_KhachHang.Instance.DatHang(khachHang, qlSanPham, currDiaChi, ptThanhToan, dungXuCB.Checked);
            thanhToanPanel.Visible = false;

            home_Button_Click(null, null);
            RefreshCartButton();
        }

        private void gioHangPanel_VisibleChanged(object sender, EventArgs e)
        {
            if(gioHangPanel.Visible)
            {
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
                    noiDungPanel.Top = 596;
                }
            }
            else
            {

            }
        }

        private void BaiDangPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (BaiDangPanel.Visible)
            {
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
                daBanTxt.Text = "Đã bán " + currBaiDang.luocBan().ToString();
                titleTxt.Text = currBaiDang.tieuDe;
                GUI_Utils.Instance.FitTextBox(titleTxt, 20, 10);
                titleTxt.Size = new Size(750, titleTxt.Height);
                giamGiaTxt.Text = "GIẢM " + currBaiDang.giamGia.ToString() + "%";
                GUI_Utils.Instance.FitTextBox(giamGiaTxt, 0, 0);

                if (BLL_BaiDang.Instance.IsSamePrice(currBaiDang))
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

                spKhacCuaShopFLP.Controls.Clear();
                foreach (BaiDang baiDang in currShop.listBaiDang.list)
                {
                    spKhacCuaShopFLP.Controls.Add(drawBaiDang(baiDang, DenBaiDangTuBDShop));
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

            }
        
        }

        private void thanhToanPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (thanhToanPanel.Visible && qlSanPham.list.Count > 0)
            {
                xuHienCoTxt.Text = $"Hiện có {Utils.Instance.SetGia(khachHang.xu)} xu";

                diaChiNhanHangTxt.Text = $"{currDiaChi.ten} | {currDiaChi.soDT}   |   {currDiaChi.diaChiCuThe}, {BLL_DiaChi.Instance.MoTaDiaChi(currDiaChi)}";
                GUI_Utils.Instance.FitTextBox(diaChiNhanHangTxt);
                List<QLSanPham> listQLSP = qlSanPham.phanRa();

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

            }
        }

        private void UserPanel_VisibleChanged(object sender, EventArgs e)
        {
            if(UserPanel.Visible)
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

        private void diaChiUser_Panel_VisibleChanged(object sender, EventArgs e)
        {
            if(diaChiUser_Panel.Visible)
            {
                if(listDiaChi_FLPanel.Controls.Count == 0)
                    VeLai_DiaChi();
            }
            else
            {

            }
        }

        private void profilePanel_VisibleChanged(object sender, EventArgs e)
        {
            if(profilePanel.Visible)
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
    }
}
