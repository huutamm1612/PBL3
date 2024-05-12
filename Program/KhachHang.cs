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
        public List<string> listFollow { get; set; }
        public List<string> listThich { get; set; }
        public List<string> listDaXem { get; set; }
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
            this.listThich = new List<string>();
            this.listDaXem = new List<string>();
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

        public void xemBaiDang(string maBD)
        {
            listDaXem.Insert(0, maBD);
            HeThong.ThemDaXem(maSo, maBD);
            if (listDaXem.Count == 20)
            {
                HeThong.XoaDaXem(maSo, listDaXem.Last());
                listDaXem.RemoveAt(19);
            }
        }

        public void themVaoGioHang(SanPham sanPham, int soLuong)
        {
            SanPham item = sanPham.Clone();
            item.themVaoGioHang(soLuong);
            HeThong.ThemVaoGioHang(item, maSo, gioHang.IsExist(item));
            gioHang.Add(item);
        }

        public void xoaKhoiGioHang(params string[] list)
        {
            gioHang.RemoveRange(list);
            HeThong.XoaSPKhoiGioHang(maSo, list);
            Utils.Sort(gioHang.list, 0, gioHang.list.Count - 1, GioHang.CompareNgayThem, GioHang.EqualNgayThem);
        }

        public SanPham searchSanPhamTrongGioHang(string tenSanPham)
        {
            foreach(SanPham sanPham in gioHang.list)
            {
                if (sanPham.ten.ToLower().Contains(tenSanPham.ToLower()))
                    return sanPham;
            }
            return null;
        }

        public override void follow(string maS)
        {
            listFollow.Insert(0, maS);
            HeThong.Follow(maSo, maS);
        }

        public override void unFollow(string maS)
        {
            listFollow.Remove(maS);
            HeThong.UnFollow(maSo, maS);
        }

        public void thich(string maBD)
        {
            listThich.Insert(0, maBD);
            HeThong.Thich(maSo, maBD);
        }

        public void huyThich(string maBD)
        {
            listThich.Remove(maBD);
            HeThong.HuyThich(maSo, maBD);
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
