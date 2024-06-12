using Program.BLL;
using Program.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.GUI
{

    public delegate void sendLyDo(string noiDung);

    public partial class ReportForm : Form
    {
        private string noiDung = "";
        private string maBC;
        private sendLyDo send;
        private SanPham currSanPham = null;
        private BaiDang currBaiDang = null;
        private DanhGia currDanhGia = null;

        public ReportForm(sendLyDo sender, string loaiBaoCao)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

            this.send = sender;
            hoanThanhButton.Enabled = false;
            if (loaiBaoCao == "Khách hàng hủy đơn")
            {
                SetFLP(BLL_DonHang.Instance.GetAllLyDoHuyDonByKhachHang());
                hoanThanhButton.Text = "HỦY ĐƠN HÀNG";
                textBox13.Text = "Lý Do Hủy";
                GUI_Utils.Instance.FitTextBox(textBox13);
            }
            else if(loaiBaoCao == "Shop hủy đơn")
            {
                SetFLP(BLL_DonHang.Instance.GetAllLyDoHuyDonByShop());
                hoanThanhButton.Text = "HỦY ĐƠN HÀNG";
                textBox13.Text = "Lý Do Hủy";
                GUI_Utils.Instance.FitTextBox(textBox13);
            }
            else if(loaiBaoCao == "Tố cáo bài đăng")
            { 
                SetFLP(BLL_BaiDang.Instance.GetAllLyDoToCaoBaiDang());
                hoanThanhButton.Text = "Gửi";
                textBox13.Text = "Lý Do Sản Phẩm Vi phạm";
                GUI_Utils.Instance.FitTextBox(textBox13);
            }
            else if(loaiBaoCao == "Báo cáo đánh giá")
            {
                SetFLP(BLL_DanhGia.Instance.GetAllLyDoBaoCaoDanhGia());
                hoanThanhButton.Text = "Gửi";
                textBox13.Text = "Lý Do Đánh Giá Vi Phạm";
                GUI_Utils.Instance.FitTextBox(textBox13);
            }
        }

        public ReportForm(BaiDang baiDang, string noiDung, string maTB, bool isVP = false)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            currBaiDang = baiDang;
            this.noiDung = noiDung;
            this.maBC = maTB;


            if (isVP)
            {
                lyDoViPhamTxt.Text = "Yêu cầu gỡ vi phạm";
                button3.Text = "GỠ VI PHẠM";
                button3.Click += GoViPhamButton_Click;
                xacNhanVPButton.Text = "KHÔNG GỠ VI PHẠM";
                xacNhanVPButton.Click += XacNhanBDViPham_Click;
            }
            else
            {
                lyDoViPhamTxt.Text = "Lý Do Vi Phạm :" + noiDung.Substring(69);
                button3.Text = "KHÔNG VI PHẠM";
                button3.Click += KhongViPham_Click;
                xacNhanVPButton.Text = "XÁC NHẬN VI PHẠM";
                xacNhanVPButton.Click += XacNhanBDViPham_Click;
            }
            SetBaoCaoBaiDang();
        }

        public ReportForm(DanhGia danhGia, string noiDung, string maTB)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            currDanhGia = danhGia;
            this.noiDung = noiDung;
            this.maBC = maTB;

            xacNhanVPButton.Click += XacNhanDGViPham_Click;


            lyDoViPhamTxt.Text = "Lý Do Vi Phạm :" + noiDung.Substring(58);
            xemBaiDangButton.Visible = true;

            SetBaoCaoDanhGia();
        }

        private void SetBaoCaoDanhGia()
        {
            this.Size = new Size(headPanel.Width, headPanel.Height + panel2.Height + tailPanel.Height);
            headPanel.Visible = true;
            panel2.Visible = true;
            danhGiaViPhamPanel.Visible = true;
            baiDangViPhamPanel.Visible = false;
            tailPanel.Visible = true;

            pictureBox1.Image = GUI_Utils.Instance.Resize(GUI_Utils.Instance.LoadImage(BLL_BaiDang.Instance.GetURLFromMaBD(currDanhGia.maBD)), pictureBox1.Size);
            tenBDTxt.Text = Utils.Instance.GioiHangKyTu(BLL_BaiDang.Instance.GetTieuDeFromMaBD(currDanhGia.maBD), 120);
            sanPhamDaMuaTxt.Text = Utils.Instance.GioiHangKyTu("Sản phẩm đã mua: " + currDanhGia.sanPhamDaMua, 100);
            thietKeBiaTxt.Text = currDanhGia.thietKeBia;
            doiTuongTxt.Text = currDanhGia.doiTuong;
            noiDungTxt.Text = currDanhGia.noiDung;
        }

        private void SetBaoCaoBaiDang()
        {
            this.Size = new Size(headPanel.Width, headPanel.Height + panel2.Height + tailPanel.Height);
            headPanel.Visible = true;
            panel2.Visible = true;
            baiDangViPhamPanel.Visible = true;
            danhGiaViPhamPanel.Visible = false;
            tailPanel.Visible = true;

            titleTxt.Text = currBaiDang.tieuDe;
            GUI_Utils.Instance.FitTextBoxMultiLines(titleTxt);
            currImage.Image = GUI_Utils.Instance.Resize(GUI_Utils.Instance.LoadImage(currBaiDang.anhBia), currImage.Size);

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
            Font font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            listItemFLP.Controls.Clear();
            moTaBaiDangTxt.Text = "";
            int index = 1;
            foreach (SanPham item in currBaiDang.list)
            {
                Size textSize = TextRenderer.MeasureText(item.ten, font);

                using (Bitmap bmp = GUI_Utils.Instance.LoadImage(item.anh))
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
                        Image = GUI_Utils.Instance.Resize(GUI_Utils.Instance.LoadImage(item.anh), new Size(textSize.Height + 10, textSize.Height + 10)),
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

                    listItemFLP.Controls.Add(button);
                }

                moTaBaiDangTxt.Text += index.ToString() + "." + item.ToString() + "\r\n";
                index++;
            }

            moTaBaiDangTxt.Text += currBaiDang.moTa;
            GUI_Utils.Instance.FitTextBoxMultiLines(moTaBaiDangTxt);
        }
        public void SanPham_MouseHover(object sender, EventArgs e)
        {
            Button obj = sender as Button;
            int index = listItemFLP.Controls.IndexOf(obj);

            currImage.Image = GUI_Utils.Instance.Resize(GUI_Utils.Instance.LoadImage(currBaiDang.list[index].anh), currImage.Size);
            obj.FlatAppearance.BorderColor = Color.OrangeRed;
            obj.ForeColor = Color.OrangeRed;
        }

        public void SanPham_MouseMove(object sender, MouseEventArgs e)
        {
            Button obj = sender as Button;

            if (obj.ForeColor == Color.OrangeRed)
                return;

            int index = listItemFLP.Controls.IndexOf(obj);

            currImage.Image = GUI_Utils.Instance.Resize(GUI_Utils.Instance.LoadImage(currBaiDang.list[index].anh), currImage.Size);
            obj.FlatAppearance.BorderColor = Color.OrangeRed;
            obj.ForeColor = Color.OrangeRed;
        }

        public void SanPham_MouseLeave(object sender, EventArgs e)
        {
            Button obj = sender as Button;
            if (currSanPham != null)
            {
                currImage.Image = GUI_Utils.Instance.Resize(GUI_Utils.Instance.LoadImage(currSanPham.anh), currImage.Size);
                if (currSanPham.ten != obj.Text)
                {
                    obj.FlatAppearance.BorderColor = Color.LightGray;
                    obj.ForeColor = Color.Black;
                }
            }
            else
            {
                currImage.Image = GUI_Utils.Instance.Resize(GUI_Utils.Instance.LoadImage(currBaiDang.anhBia), currImage.Size);
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
                currImage.Image = GUI_Utils.Instance.Resize(GUI_Utils.Instance.LoadImage(currBaiDang.anhBia), currImage.Size);
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
                        giaGocTxt.Text = "₫" + Utils.Instance.SetGia(item.gia);
                        giaTxt.Text = "₫" + Utils.Instance.SetGia(Utils.Instance.GiamGia(item.gia, currBaiDang.giamGia));
                        GUI_Utils.Instance.FitTextBox(giaGocTxt, 20);
                        GUI_Utils.Instance.FitTextBox(giaTxt, 20);
                        currImage.Image = GUI_Utils.Instance.Resize(GUI_Utils.Instance.LoadImage(item.anh), currImage.Size);
                        currSanPham = item;
                        break;
                    }
                }
            }
        }

        private void SetFLP(List<string> listLyDo)
        {
            baoCaoPanel.BringToFront();
            Size = baoCaoPanel.Size;
            baoCaoPanel.Visible = true;
            flowLayoutPanel1.Visible = true;
            flowLayoutPanel1.Controls.Clear();
            foreach (string s in listLyDo)
            {
                RadioButton rad = new RadioButton
                {
                    Text = s,
                    Size = new Size(flowLayoutPanel1.Width, 30),
                    Font = radioButton1.Font,
                    Margin = radioButton1.Margin,
                    Cursor = Cursors.Hand,
                    Parent = flowLayoutPanel1
                };
                rad.CheckedChanged += RadioButton_CheckedChanged;
                flowLayoutPanel1.Controls.Add(rad);
            }
            RadioButton khac = new RadioButton
            {
                Text = "Khác",
                Size = new Size(flowLayoutPanel1.Width, 30),
                Font = radioButton1.Font,
                Margin = radioButton1.Margin,
                Cursor = Cursors.Hand,
                Parent = flowLayoutPanel1
            };
            flowLayoutPanel1.Controls.Add(khac);
            khac.CheckedChanged += RadioButton_CheckedChanged;

            GUI_Utils.Instance.FitFLPHeight(flowLayoutPanel1);
            baoCaoPanel.Height = flowLayoutPanel1.Height + 150;
            Height = baoCaoPanel.Height;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton obj = sender as RadioButton;
            hoanThanhButton.Enabled = true;
            if (obj.Text.Equals("Khác"))    
            {
                flowLayoutPanel1.Controls.Add(panel1);
            }
            else if(noiDung == "Khác")
            {
                flowLayoutPanel1.Controls.Remove(panel1);
            }
            GUI_Utils.Instance.FitFLPHeight(flowLayoutPanel1);
            baoCaoPanel.Height = flowLayoutPanel1.Height + 150;
            Height = baoCaoPanel.Height;
            noiDung = obj.Text;
        }

        private void troVeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void hoanThanhButton_Click(object sender, EventArgs e)
        {
            send(noiDung);
            Close();
        }

        private void XacNhanBDViPham_Click(object sender, EventArgs e)
        {
            BLL_Admin.Instance.XacNhanBDViPham(maBC, currBaiDang.maBD, noiDung.Substring(69));
            Close();
        }

        private void GoViPhamButton_Click(object sender, EventArgs e)
        {
            BLL_Admin.Instance.GoViPham(currBaiDang.maBD, maBC);
            Close();
        }

        private void KhongViPham_Click(object sender, EventArgs e)
        {
            BLL_Admin.Instance.XacNhanKhongViPham(maBC);
            Close();
        }

        private void XacNhanDGViPham_Click(object sender, EventArgs e)
        {
            BLL_Admin.Instance.XacNhanDGViPham(maBC, currDanhGia.maDG, noiDung.Substring(58));
            Close();
        }

        private void TroVeDanhGiaViPham_Click(object sender, EventArgs e)
        {
            backButton.Text = "TRỞ VỀ";
            xemBaiDangButton.Visible = true;
            baiDangViPhamPanel.Visible = false;
            danhGiaViPhamPanel.Visible = true;
            backButton.Click += troVeButton_Click;
        }

        private void XemBaiDangButton_Click(object sender, EventArgs e)
        {
            ((Control)sender).Visible = false;
            if(currBaiDang == null)
            {
                currBaiDang = BLL_BaiDang.Instance.GetBaiDangFromMaBD(currDanhGia.maBD);
                SetBaoCaoBaiDang();
            }
            backButton.Text = "QUAY LẠI";
            backButton.Click -= troVeButton_Click;
            backButton.Click += TroVeDanhGiaViPham_Click;
        }
    }
}
