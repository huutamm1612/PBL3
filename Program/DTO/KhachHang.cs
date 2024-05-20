using Program.BLL;
using Program.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            this.avt = "";
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

        public List<SqlParameter> GetParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@maKH", maSo),
                new SqlParameter("@taiKhoan", taiKhoan),
                new SqlParameter("@ten", ten),
                new SqlParameter("@soDT", soDT),
                new SqlParameter("@email", email),
                new SqlParameter("@ngaySinh", ngaySinh),
                new SqlParameter("@gioiTinh", gioiTinh),
                new SqlParameter("@avt", avt),
                new SqlParameter("@maDC", diaChi.maDC)
            };
        }

        public KhachHang(string maSo, string taiKhoan)
        {
            this.maSo = maSo;
            this.taiKhoan = taiKhoan;
        }

        public override void Nhap(string ten, string email, string soDT, int gioiTinh, DateTime ngaySinh)
        {
            this.ten = ten;
            this.email = email;
            this.soDT = soDT;
            this.gioiTinh = gioiTinh;
            this.ngaySinh = ngaySinh;
        }

        public override void Sua(string ten, string email, string soDT, int gioiTinh, DateTime ngaySinh)
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

        public void xemBaiDang(string maBD)
        {
            listDaXem.Insert(0, maBD);
            if (listDaXem.Count == 20)
            {
                listDaXem.RemoveAt(19);
            }
        }

        public void XoaSPKhoiGioHang(int index)
        {
            gioHang.Remove(gioHang.list[index]);
        }

        public void ThemVaoGioHang(SanPham sanPham)
        {
            gioHang.Add(sanPham);
        }

        public void CapNhatSoLuongSPTrongGH(int index, int newSoLuong)
        {
            SanPham sanPham = gioHang.list[index].Clone();
            sanPham.soLuong = newSoLuong;
            sanPham.ngayThem = DateTime.Now;

            gioHang.Update(sanPham);
            BLL_GioHang.Instance.CapNhatSoLuong(maSo, sanPham.maSP, newSoLuong);
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

        public override void Follow(string maS)
        {
            listFollow.Insert(0, maS);
        }

        public override void UnFollow(string maS)
        {
            listFollow.Remove(maS);
        }

        public void Thich(string maBD)
        {
            listThich.Insert(0, maBD);
        }

        public void HuyThich(string maBD)
        {
            listThich.Remove(maBD);
        }

        public void DatHang(DonHang donHang)
        {
            listDonHang.Add(donHang);
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

        public override void CapNhatDiaChi(DiaChi diaChiMoi)
        {
            this.diaChi = diaChiMoi;
        }
    }
}
