using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class KhachHang : Nguoi
    {
        public string taiKhoan { get; set; }
        public int chiTieu { get; set; }
        public int xu { get; set; }
        public string avt { get; set; }
        public List<string> listFollow { get; set; }
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
            this.maSo = "";
            this.ten = "";
            this.soDT = "";
            this.email = "";
            this.diaChi = null;
            this.ngaySinh = ngaySinh;
            this.taiKhoan = null;
            this.xu = 0;
            this.chiTieu = 0;
            this.listFollow = new List<string>();
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
            this.listFollow = listFollow;
            this.chiTieu = chiTieu;
            this.xu = xu;
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

        public void themVaoGioHang(SanPham sanPham, int soLuong)
        {
            SanPham item = sanPham.Clone();
            item.themVaoGioHang(soLuong);
            gioHang.Add(item);
            HeThong.ThemVaoGioHang(item, maSo, gioHang.IsExist(item));
        }

        public void xoaKhoiGioHang(string[] list)
        {
            gioHang.RemoveRange(list);
            HeThong.XoaSPKhoiGioHang(maSo, list);
            Utils.Sort(gioHang.list, 0, gioHang.list.Count - 1, GioHang.CompareNgayThem, GioHang.EqualNgayThem);
        }

        public void muaHang(string[] list)
        {

        }

        public void follow(string maS)
        {
            listFollow.Insert(0, maS);
            HeThong.Follow(maSo, maS);
        }

        public void unFollow(string maS)
        {
            listFollow.Remove(maS);
            HeThong.UnFollow(maSo, maS);
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

        public override void capNhatDiaChi(DiaChi diaChiMoi)
        {
            this.diaChi = diaChiMoi;
        }
    }
}
