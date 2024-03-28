using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class DonHang : QLSanPham
    {
        public string maDH { get; set; }
        public string maKH { get; set; }
        public DiaChi diaChi { get; set; }
        public int tongTien { get; set; }
        public int tinhTrang { get; set; }
        public int ptThanhToan { get; set; }
        public DateTime ngayDatHang { get; set; }
        public DateTime ngayGiaoHang { get; set; }

        public DonHang()
        {
            list = null;
            maDH = "";
            maKH = "";
            tinhTrang = 0;
            ptThanhToan = 0;
            tongTien = 0;
            diaChi = new DiaChi();
            ngayDatHang = DateTime.Now;
            ngayGiaoHang = DateTime.Now;
        }

        public DonHang(string maDH, string maKH, int tinhTrang, int ptThanhToan, int tongTien, DiaChi diaChi, DateTime ngayDatHang)
        {
            this.maDH = maDH;
            this.maKH = maKH;
            this.tinhTrang = tinhTrang;
            this.ptThanhToan = ptThanhToan;
            this.tongTien = tongTien;
            this.diaChi = diaChi;
            this.ngayDatHang = ngayDatHang;
        }

        public DonHang(QLSanPham sanPham)
        {
            list = sanPham.list;
        }

        public DonHang(DonHang donHang)
        {
            maDH = donHang.maDH;
            maKH= donHang.maKH;
            tinhTrang = donHang.tinhTrang;
            ptThanhToan = donHang.ptThanhToan;
            tongTien = donHang.tongTien;
            diaChi = donHang.diaChi;
            ngayDatHang = donHang.ngayDatHang;
            ngayGiaoHang = donHang.ngayGiaoHang;
        }

        public void diDon()
        {
            Random random = new Random();

            int soNgay = random.Next(3, 7); 
            int soGio = random.Next(24); 
            int soPhut = random.Next(60);
            int soGiay = random.Next(60);

            tinhTrang = 1;
            ngayGiaoHang = ngayDatHang.Add(new TimeSpan(soNgay, soGio, soPhut, soGiay));
        }

        public void datHang(KhachHang khachHang, DiaChi diaChi, int ptThanhToan)
        {
            maDH = HeThong.MaMoi("MaDH");
            maKH = khachHang.maSo;
            tongTien = tinhTongTien();
            this.diaChi = diaChi;
            this.ptThanhToan = ptThanhToan;

            ngayGiaoHang = DateTime.Now;
            tinhTrang = 0;
        }

        public void nhanHang()
        {

        }
    }
}
