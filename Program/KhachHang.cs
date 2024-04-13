using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class KhachHang : Nguoi
    {
        public string taiKhoan { get; set; }
        public int nFollow { get; set; }
        public int chiTieu { get; set; }
        public int xu { get; set; }
        public string avt { get; set; }
        public List<DiaChi> listDiaChi { get; set; }
        public QLDonHang listDonHang { get; set; }
        public GioHang gioHang { get; set; }
        public QLDanhGia listDanhGia { get; set; }

        public override bool Equals(object obj)
        {
            return ((KhachHang)obj).maSo == this.maSo;
        }

        public KhachHang()
        {
            this.maSo = maSo;
            this.ten = ten;
            this.soDT = soDT;
            this.email = email;
            this.diaChi = diaChi;
            this.ngaySinh = ngaySinh;
            this.taiKhoan = null;
            this.nFollow = 0;
            this.xu = 0;
            this.chiTieu = 0;
            this.listDiaChi = new List<DiaChi>();
            this.listDonHang = new QLDonHang();
            this.gioHang = new GioHang();
            this.listDanhGia = new QLDanhGia();
        }
        public KhachHang(string maSo, string taiKhoan)
        {
            this.maSo = maSo;
            this.taiKhoan = taiKhoan;
        }
        public KhachHang(string maSo, string ten, string soDT, string email, DiaChi diaChi, int gioiTinh, DateTime ngaySinh, string taiKhoan, int nFollow, int chiTieu, int xu)
        {
            this.maSo = maSo;
            this.taiKhoan = taiKhoan;
            this.ten = ten;
            this.soDT = soDT;
            this.email = email;
            this.diaChi = diaChi;
            this.gioiTinh = gioiTinh;
            this.ngaySinh = ngaySinh;
            this.nFollow = nFollow;
            this.chiTieu = chiTieu;
            this.xu = xu;
        }
        public KhachHang(KhachHang khachHang)
        {
            this.maSo = khachHang.maSo;
            this.taiKhoan = khachHang.taiKhoan;
            this.ten = khachHang.ten;
            this.soDT = khachHang.soDT;
            this.email = khachHang.email;
            this.diaChi = khachHang.diaChi;
            this.ngaySinh = khachHang.ngaySinh;
            this.nFollow = khachHang.nFollow;
            this.chiTieu = khachHang.chiTieu;
            this.xu = khachHang.xu;
            this.listDiaChi = khachHang.listDiaChi;
            this.listDonHang = khachHang.listDonHang;
        }

        public override void nhap(string ten, string email, string soDT, int gioiTinh, DateTime ngaySinh)
        {
            this.ten = ten;
            this.email = email;
            this.soDT = soDT;
            this.gioiTinh = gioiTinh;
            this.ngaySinh = ngaySinh;
        }

        public override void sua(string ten, string email, string soDT, int gioiTinh, DateTime ngaySinh)
        {
            this.ten = ten;
            this.email = email;
            this.soDT = soDT;
            this.gioiTinh = gioiTinh;
            this.ngaySinh = ngaySinh;
        }

        public void capNhatDiaChi(int index, DiaChi diaChi) //update địa chỉ không mặc định
        {
            this.listDiaChi[index] = diaChi;
        }

        public void themDiaChi(DiaChi diaChi) // thêm địa chỉ không mặc định
        {
            this.listDiaChi.Insert(0, diaChi);
        }

        public void xoaDiaChi(int index) // xóa địa chỉ không mặc định
        {
            this.listDiaChi.RemoveAt(index);
        }

        public void xoaDiaChi(DiaChi diaChi) // xóa địa chỉ không mặc định
        {
            this.listDiaChi.Remove(diaChi);
        }

        public void setDiaChis(List<DiaChi> list)
        {
            this.listDiaChi = list;
        }

        public void thayDoiDiaChiMacDinh(DiaChi diaChi) // thay đổi địa chỉ mặc định bằng cách thêm vào một trong các địa chỉ mặc định
        {
            this.listDiaChi.Insert(0, this.diaChi);
            this.listDiaChi.Remove(diaChi);

            this.diaChi = diaChi;
        }

        public bool daTaoShop() // trả về true nếu khách hàng đã tạo shop
        {
            return HeThong.KiemTraTaoShop(this);
        }

        public void datHang(QLSanPham listSanPham, DiaChi diaChi, int ptThanhToan, int xu)
        {
            DonHang donHang = new DonHang
            {
                list = listSanPham.list,
                maKH = maSo,
                tinhTrang = 0,
                ptThanhToan = ptThanhToan,
                diaChi = diaChi,
                xu = xu,
                ngayDatHang = DateTime.Now
            };
            this.xu -= xu;
            listDonHang.AddRange(donHang.datHang());
        }

        public void huyDon(DonHang donHang)
        {
            donHang.capNhatTinhTrang(-1);
            donHang.huyDon();
        }

        public void nhanHang(DonHang donHang)
        {
            donHang.capNhatTinhTrang(2);
            chiTieu += donHang.tongTien - donHang.xu;
            donHang.nhanHang();

            listDanhGia.AddRange(donHang.taoDanhGia());
        }
    }
}
