using Program.BLL;
using Program.GUI;
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
    public partial class SanPhamForm : Form
    {
        private QLSanPham QLSP = null;
        private string url = null;
        public SendSanPham send;
        private string maSP = null;
        public SanPhamForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

        }

        public SanPhamForm(SendSanPham sender, bool isAdd = true)
        {
            InitializeComponent();
            theLoai_CBBox.Items.AddRange(BLL_SanPham.Instance.GetAllLoaiSanPham().ToArray());
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

            this.send = sender;
        }

        public void setSanPham(SanPham sanPham)
        {
            maSP = sanPham.maSP;
            url = sanPham.anh;
            picImage.Image = GUI_Utils.Instance.Resize(Image.FromFile(sanPham.anh), picImage.Size);
            tenSP_Text.Text = sanPham.ten;
            tenTacGia_Text.Text = sanPham.tacGia;
            tenDichGia_Text.Text = sanPham.dichGia;
            nhaXuatBan_Text.Text = sanPham.nhaXuatBan;
            namXuatBan_Text.Text = sanPham.namXuatBan.ToString();
            moTaSP_Text.Text = sanPham.moTa;
            theLoai_CBBox.SelectedIndex = int.Parse(sanPham.loaiSP.maLoaiSP);
            loaiBia_CBBox.SelectedItem = sanPham.loaiBia;
            ngonNgu_CBBox.SelectedItem = sanPham.ngonNgu;
            gia_Text.Text = sanPham.gia.ToString();
            soLuong_Text.Text = sanPham.soLuong.ToString();
            soTrang_Text.Text = sanPham.soTrang.ToString();
        }


        private void huyBoButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void luuButton_Click(object sender, EventArgs e)
        {
            if (tenSP_Text.Text != "" && tenTacGia_Text.Text != "" && tenDichGia_Text.Text != "" && nhaXuatBan_Text.Text != ""
             && moTaSP_Text.Text != "" && namXuatBan_Text.Text != "" && soLuong_Text.Text != "" && ngonNgu_CBBox.SelectedIndex != -1
             && theLoai_CBBox.SelectedIndex != -1 && loaiBia_CBBox.SelectedIndex != -1 && soTrang_Text.Text != "")
            {
                this.send(new SanPham
                {
                    maSP = maSP,
                    loaiSP = new LoaiSanPham { maLoaiSP = theLoai_CBBox.SelectedIndex.ToString("D10"), tenLoaiSP = theLoai_CBBox.SelectedItem.ToString() },
                    ten = tenSP_Text.Text,
                    tacGia = tenTacGia_Text.Text,
                    dichGia = tenDichGia_Text.Text,
                    nhaXuatBan = nhaXuatBan_Text.Text,
                    moTa = moTaSP_Text.Text,
                    gia = int.Parse(gia_Text.Text),
                    namXuatBan = int.Parse(namXuatBan_Text.Text),
                    soLuong = int.Parse(soLuong_Text.Text),
                    soTrang = int.Parse(soTrang_Text.Text),
                    ngonNgu = ngonNgu_CBBox.SelectedItem.ToString(),
                    loaiBia = loaiBia_CBBox.SelectedItem.ToString(),
                    luocBan = 0,
                    ngayThem = DateTime.Now,
                    anh = url
                });
                Close();
            }
            else
            {
                Check(tenSP_Text, SP_Pic);
                Check(tenTacGia_Text, TG_Pic);
                Check(tenDichGia_Text, DichGia_Pic);
                Check(nhaXuatBan_Text, NXB_Pic);
                Check(namXuatBan_Text, Nam_Pic);
                Check(soLuong_Text, SoLuong_Pic);
                Check(gia_Text, Gia_Pic);
                Check(soTrang_Text, SoTrang_Pic);
                Check(moTaSP_Text, moTa_Pic);
                if (ngonNgu_CBBox.SelectedIndex != -1)
                    language_Check.Visible = false;
                else
                    language_Check.Visible = true;
                if (loaiBia_CBBox.SelectedIndex != -1)
                    bia_Check.Visible = false;
                else
                    bia_Check.Visible = true;
                if (theLoai_CBBox.SelectedIndex != -1)
                    theLoai_check.Visible = false;
                else
                    theLoai_check.Visible = true;
            }       
        }
        private void Check(TextBox txt, PictureBox pic)
        {
            // pic.BorderStyle = BorderStyle.FixedSingle;
            if (txt.Text == "")
            {
                pic.BorderStyle = BorderStyle.None;
                AddBorderToPictureBox(pic, Color.Red);
            }
            else
            {
                pic.BorderStyle = BorderStyle.FixedSingle;
                AddBorderToPictureBox(pic, Color.White);
            }
        }
        private void AddBorderToPictureBox(PictureBox pictureBox, Color color)
        {
            Pen pen = new Pen(color, 2);

            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

            using (Graphics g = pictureBox.Parent.CreateGraphics())
            {
                g.DrawRectangle(pen, new Rectangle(pictureBox.Location, pictureBox.Size));
            }
        }

        private void tenSP_Text_TextChanged(object sender, EventArgs e)
        {
            Check(tenSP_Text, SP_Pic);
            demKyTu(tenSP_Text, 50, Count_SP);
        }

        private void demKyTu(TextBox txt, int maxLength, TextBox count)
        {
            if (txt.Text.Length <= maxLength)
            {
                count.Text = txt.Text.Length + "/" + maxLength;
            }
            else
            {
                txt.Text = txt.Text.Substring(0, maxLength);
                txt.SelectionStart = txt.Text.Length;
                count.Text = maxLength + "/" + maxLength;
            }
        }


        private void tenTacGia_Text_TextChanged(object sender, EventArgs e)
        {
            Check(tenTacGia_Text, TG_Pic);
            demKyTu(tenTacGia_Text, 50, Count_TG);
        }

        private void tenDichGia_Text_TextChanged(object sender, EventArgs e)
        {
            Check(tenDichGia_Text, DichGia_Pic);
            demKyTu(tenDichGia_Text, 50, Count_Dichgia);
        }

        private void nhaXuatBan_Text_TextChanged(object sender, EventArgs e)
        {
            Check(nhaXuatBan_Text, NXB_Pic);
            demKyTu(nhaXuatBan_Text, 50, Count_NXB);
        }

        private void moTaSP_Text_TextChanged(object sender, EventArgs e)
        {
            demKyTu(moTaSP_Text, 1000, Count_MoTa);
            Check(moTaSP_Text, moTa_Pic);
        }
        private void namXuatBan_Text_TextChanged(object sender, EventArgs e)
        {
            Check(namXuatBan_Text, Nam_Pic);

        }

        private void gia_Text_TextChanged(object sender, EventArgs e)
        {
            Check(gia_Text, Gia_Pic);
        }

        private void soLuong_Text_TextChanged(object sender, EventArgs e)
        {
            Check(soLuong_Text, SoLuong_Pic);
        }

        private void soTrang_Text_TextChanged(object sender, EventArgs e)
        {
            Check(soTrang_Text, SoTrang_Pic);
        }
        private void refreshThemSPForm()
        {
            // refresh form
            tenSP_Text.Text = "";
            theLoai_CBBox.SelectedIndex = -1;
            tenTacGia_Text.Text = "";
            tenDichGia_Text.Text = "";
            nhaXuatBan_Text.Text = "";
            namXuatBan_Text.Text = "";
            loaiBia_CBBox.SelectedIndex = -1;
            ngonNgu_CBBox.SelectedIndex = -1;
            gia_Text.Text = "";
            soLuong_Text.Text = "";
            soTrang_Text.Text = "";
            theLoai_check.Visible = false;
            bia_Check.Visible = false;
            language_Check.Visible = false;
            moTaSP_Text.Text = "";
            AddBorderToPictureBox(SP_Pic, Color.White);
            SP_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(TG_Pic, Color.White);
            TG_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(DichGia_Pic, Color.White);
            DichGia_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(DichGia_Pic, Color.White);
            DichGia_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(NXB_Pic, Color.White);
            NXB_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(Nam_Pic, Color.White);
            Nam_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(SoTrang_Pic, Color.White);
            SoTrang_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(SoLuong_Pic, Color.White);
            SoLuong_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(Gia_Pic, Color.White);
            Gia_Pic.BorderStyle = BorderStyle.FixedSingle;
            AddBorderToPictureBox(moTa_Pic, Color.White);
            moTa_Pic.BorderStyle = BorderStyle.FixedSingle;

        }

        private void refreshThemSPForm_Button_Click(object sender, EventArgs e)
        {
            refreshThemSPForm();
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "File anh|*.jpg.; *.gif; *.png; |All file| *.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                picImage.Image = GUI_Utils.Instance.Resize(Image.FromFile(openFile.FileName), picImage.Size);
                url = openFile.FileName;
            }
        }

        private void picImage_Click(object sender, EventArgs e)
        {

        }

        private void Enter_Esc_SanPham_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                luuButton_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void theLoai_CBBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (theLoai_CBBox.SelectedIndex != -1)
            {
                theLoai_check.Visible = false;
            }
            else
            {
                theLoai_CBBox.Visible = true;
            }
        }

        private void loaiBia_CBBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaiBia_CBBox.SelectedIndex != -1)
            {
                bia_Check.Visible = false;
            }
            else
            {
                bia_Check.Visible = true;
            }
        }

        private void ngonNgu_CBBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ngonNgu_CBBox.SelectedIndex != -1)
            {
                language_Check.Visible = false;
            }
            else
            {
                language_Check.Visible = true;
            }
        }
    }
}
