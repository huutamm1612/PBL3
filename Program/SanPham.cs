using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class SanPham
    {
        public string maSP { get; set; }
        public string maLoaiSP { get; set; }
        public string maS { get; set; }
        public string maBD { get; set; }
        public string ten { get; set; }
        public int gia { get; set; }
        public int soLuong { get; set; }
        public int luocBan { get; set; }
        public string moTa { get; set; }
        public string tacGia { get; set; }
        public string dichGia { get; set; }
        public string ngonNgu { get; set; }
        public int soTrang { get; set; }
        public int namXuatBan { get; set; }
        public string nhaXuatBan { get; set; }
        public string loaiBia { get; set; }
        public DateTime ngayThem { get; set; }
        public SanPham()
        {
            maSP = "";
            maLoaiSP = "";
            maS = "";
            maBD = "";
            ten = "";
            gia = 0;
            soLuong = 0;
            luocBan = 0;
            tacGia = "";
            dichGia = "";
            ngonNgu = "";
            soTrang = 0;
            namXuatBan = 0;
            nhaXuatBan = "";
            loaiBia = "";
            moTa = "";
            ngayThem = DateTime.Now;
        }

        public SanPham(string maSP)
        {
            this.maSP = maSP;
        }

        public SanPham(SanPham sanPham)
        {
            maSP = sanPham.maSP;
            maLoaiSP = sanPham.maLoaiSP;
            ten = sanPham.ten;
            gia = sanPham.gia;
            soLuong = sanPham.soLuong;
            luocBan = sanPham.luocBan;
            tacGia = sanPham.tacGia;
            dichGia = sanPham.dichGia;
            ngonNgu = sanPham.ngonNgu;
            soTrang = sanPham.soTrang;
            namXuatBan = sanPham.namXuatBan;
            nhaXuatBan = sanPham.nhaXuatBan;
            loaiBia = sanPham.loaiBia;
            moTa = sanPham.moTa;
            ngayThem = sanPham.ngayThem;
        }

        public static bool CompareGia(object o1, object o2) => ((SanPham)o1).gia < ((SanPham)o2).gia;
        public static bool CompareTen(object o1, object o2) => String.Compare(((SanPham)o1).ten, ((SanPham)o2).ten) < 0;
        public static bool CompareSoLuong(object o1, object o2) => ((SanPham) o1).soLuong < ((SanPham) o2).soLuong;
        public static bool CompareLuocBan(object o1, object o2) => ((SanPham)o1).luocBan < ((SanPham)o2).luocBan;
        public static bool CompareMaSP(object o1, object o2) => String.Compare(((SanPham)o1).maSP, ((SanPham)o2).maSP) < 0;
        public static bool CompareNgayThem(object o1, object o2) => DateTime.Compare(((SanPham)o1).ngayThem, ((SanPham)o2).ngayThem) < 0;
        public static bool CompareMaS(object o1, object o2) => String.Compare(((SanPham)o1).maS, ((SanPham)o2).maS) < 0;

        public static bool EqualGia(object o1, object o2) => ((SanPham) o1).gia == ((SanPham) o2).gia;       
        public static bool EqualTen(object o1, object o2) => String.Equals(((SanPham)o1).ten, ((SanPham)o2).ten);
        public static bool EqualSoLuong(object o1, object o2) => ((SanPham)o1).soLuong == ((SanPham)o2).soLuong;
        public static bool EqualLuocBan(object o1, object o2) => ((SanPham)o1).luocBan == ((SanPham)o2).luocBan;
        public static bool EqualMaSP(object o1, object o2) => String.Equals(((SanPham)o1).maSP, ((SanPham)o2).maSP);
        public static bool EqualMaLoaiSP(object o1, object o2) => String.Equals(((SanPham)o1).maLoaiSP, ((SanPham)o2).maLoaiSP);
        public static bool EqualLoaiBia(object o1, object o2) => String.Equals(((SanPham)o1).loaiBia, ((SanPham)o2).loaiBia);
        public static bool EqualNgayThem(object o1, object o2) => DateTime.Equals(((SanPham)o1).ngayThem, ((SanPham)o2).ngayThem);
        public static bool EqualMaS(object o1, object o2) => String.Equals(((SanPham)o1).maS, ((SanPham)o2).maS);
    }
}
