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
                ten = tenDichGia_Text.Text,
                tacGia = tenDichGia_Text.Text,
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
    }
}
