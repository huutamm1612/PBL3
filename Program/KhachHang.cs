using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class KhachHang : Nguoi
    {
        public string taiKhoan { get; private set; }
        public int nFollow { get; private set; }
        public int chiTieu { get; private set; }
        public int xu { get; private set; }
        public string avt { get; private set; }
        public List<DiaChi> diaChis { get; private set; }

        public override bool Equals(object obj)
        {
            return ((KhachHang)obj).maSo == this.maSo;
        }

        public KhachHang() : base()
        {
            this.taiKhoan = null;
            this.nFollow = 0;
            this.xu = 0;
            this.chiTieu = 0;
        }
        public KhachHang(string maSo, string taiKhoan) : base(maSo)
        {
            this.taiKhoan = taiKhoan;
        }
        public KhachHang(string maSo, string ten, string soDT, string email, DiaChi diaChi, int gioiTinh, DateTime ngaySinh, string taiKhoan, int nFollow, int chiTieu, int xu) : base(maSo, ten, soDT, email, diaChi, gioiTinh, ngaySinh)
        {
            this.taiKhoan = taiKhoan;
            this.nFollow = nFollow;
            this.chiTieu = chiTieu;
            this.xu = xu;
        }
        public KhachHang(KhachHang khachHang) : base(khachHang)
        {
            this.taiKhoan = khachHang.taiKhoan;
            this.nFollow = khachHang.nFollow;
            this.xu = khachHang.xu;
            this.chiTieu = khachHang.chiTieu;
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
            this.diaChis[index] = diaChi;
        }

        public void themDiaChi(DiaChi diaChi) // thêm địa chỉ không mặc định
        {
            this.diaChis.Insert(0, diaChi);
        }

        public void xoaDiaChi(int index) // xóa địa chỉ không mặc định
        {
            this.diaChis.RemoveAt(index);
        }

        public void xoaDiaChi(DiaChi diaChi) // xóa địa chỉ không mặc định
        {
            this.diaChis.Remove(diaChi);
        }

        public void setDiaChis(List<DiaChi> list)
        {
            this.diaChis = list;
        }

        public void thayDoiDiaChiMacDinh(DiaChi diaChi) // thay đổi địa chỉ mặc định bằng cách thêm vào một trong các địa chỉ mặc định
        {
            this.diaChis.Insert(0, this.diaChi);
            this.diaChis.Remove(diaChi);

            this.diaChi = diaChi;
        }
    }
}
