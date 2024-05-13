using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class SanPham
    {
        public string maSP { get; set; }
        public LoaiSanPham loaiSP { get; set; }
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
        public string anh { get; set; }
        public SanPham()
        {
            maSP = "";
            loaiSP = new LoaiSanPham();
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
            anh = "";
            ngayThem = DateTime.Now;
        }

        public SanPham(string maSP)
        {
            this.maSP = maSP;
        }

        public SanPham Clone()
        {
            SanPham sanPham = new SanPham
            {
                maSP = this.maSP,
                loaiSP = this.loaiSP,
                maBD = this.maBD,
                maS = this.maS,
                ten = this.ten,
                gia = this.gia,
                soLuong = this.soLuong,
                luocBan = this.luocBan,
                tacGia = this.tacGia,
                dichGia = this.dichGia,
                ngonNgu = this.ngonNgu,
                soTrang = this.soTrang,
                namXuatBan = this.namXuatBan,
                nhaXuatBan = this.nhaXuatBan,
                loaiBia = this.loaiBia,
                moTa = this.moTa,
                ngayThem = this.ngayThem,
                anh = this.anh
            };
            return sanPham;
        }

        public void themVaoGioHang(int soLuong)
        {
            this.soLuong = soLuong;
            this.ngayThem = DateTime.Now;
        }

        public override string ToString()
        {
            return ten + "\r\nTác giả: " + tacGia + "\r\nNgười dịch: " + dichGia + "\r\nNhà xuất bản: " + nhaXuatBan + "\r\nNăm xuất bản: " + namXuatBan + "\r\nNgôn ngữ: " + ngonNgu + "\r\nLoại bìa: " + loaiBia + "\r\nMô tả sản phẩm: " + moTa + "\r\n";
        }

        public static bool CompareGia(object o1, object o2) => ((SanPham)o1).gia < ((SanPham)o2).gia;
        public static bool CompareTen(object o1, object o2) => String.Compare(((SanPham)o1).ten, ((SanPham)o2).ten) < 0;
        public static bool CompareSoLuong(object o1, object o2) => ((SanPham) o1).soLuong < ((SanPham) o2).soLuong;
        public static bool CompareLuocBan(object o1, object o2) => ((SanPham)o1).luocBan < ((SanPham)o2).luocBan;
        public static bool CompareMaSP(object o1, object o2) => String.Compare(((SanPham)o1).maSP, ((SanPham)o2).maSP) < 0;
        public static bool CompareNgayThem(object o1, object o2) => DateTime.Compare(((SanPham)o1).ngayThem, ((SanPham)o2).ngayThem) < 0;
        public static bool CompareMaS(object o1, object o2) => String.Compare(((SanPham)o1).maS, ((SanPham)o2).maS) < 0;
        public static bool CompareMaBD(object o1, object o2) => String.Compare(((SanPham)o1).maBD, ((SanPham)o2).maBD) < 0;

        public static bool EqualGia(object o1, object o2) => ((SanPham) o1).gia == ((SanPham) o2).gia;       
        public static bool EqualTen(object o1, object o2) => String.Equals(((SanPham)o1).ten, ((SanPham)o2).ten);
        public static bool EqualSoLuong(object o1, object o2) => ((SanPham)o1).soLuong == ((SanPham)o2).soLuong;
        public static bool EqualLuocBan(object o1, object o2) => ((SanPham)o1).luocBan == ((SanPham)o2).luocBan;
        public static bool EqualMaSP(object o1, object o2) => String.Equals(((SanPham)o1).maSP, ((SanPham)o2).maSP);
        public static bool EqualMaLoaiSP(object o1, object o2) => String.Equals(((SanPham)o1).loaiSP.maLoaiSP, ((SanPham)o2).loaiSP.maLoaiSP);
        public static bool EqualLoaiBia(object o1, object o2) => String.Equals(((SanPham)o1).loaiBia, ((SanPham)o2).loaiBia);
        public static bool EqualNgayThem(object o1, object o2) => DateTime.Equals(((SanPham)o1).ngayThem, ((SanPham)o2).ngayThem);
        public static bool EqualMaS(object o1, object o2) => String.Equals(((SanPham)o1).maS, ((SanPham)o2).maS);
        public static bool EqualMaBD(object o1, object o2) => String.Equals(((SanPham)o1).maBD, ((SanPham)o2).maBD);
        public override bool Equals(object obj) => ((SanPham)obj).maSP == this.maSP;
    }
}
