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
        public SendSanPham send;
        public SanPhamForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

        }

        public SanPhamForm(SendSanPham sender, bool isAdd = true)
        {
            InitializeComponent();
            Utils.SetComboBox(theLoai_CBBox, HeThong.LoadTheLoai());
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

            this.send = sender;
        }

        public void setSanPham(SanPham sanPham)
        {
            tenSP_Text.Text = sanPham.ten;
            tenTacGia_Text.Text = sanPham.tacGia;
            tenDichGia_Text.Text = sanPham.dichGia;
            nhaXuatBan_Text.Text = sanPham.nhaXuatBan;
            namXuatBan_Text.Text = sanPham.namXuatBan.ToString();
        }


        private void huyBoButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void luuButton_Click(object sender, EventArgs e)
        {
            // kiem tra dieu kien
            this.send(new SanPham
            {
                maSP = "",
                maLoaiSP = theLoai_CBBox.SelectedIndex.ToString("D10"),
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
                ngayThem = DateTime.Now
            }) ;

            Close();
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

        private void SP_Pic_Click(object sender, EventArgs e)
        {
           
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
    }
}
