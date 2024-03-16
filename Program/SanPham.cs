using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class SanPham
    {
        public string maSP { get; private set; }
        public string maLoaiSP { get; private set; }
        public string ten { get; private set; }
        public int gia { get; private set; }
        public int soLuong { get; private set; }
        public int luocBan { get; private set; }
        public string moTa { get; private set; }
        public string tacGia { get; private set; }
        public string dichGia { get; private set; }
        public string ngonNgu { get; private set; }
        public int soTrang { get; private set; }
        public int namXuatBan { get; private set; }
        public string nhaXuatBan { get; private set; }
        public string loaiBia { get; private set; }
        SanPham()
        {
            maSP = "";
            maLoaiSP = "";
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
        }

        SanPham(string maSP)
        {
            this.maSP = maSP;
        }


        SanPham(string maSP, string maLoaiSP, string ten, int gia, int soLuong, int luocBan, string tacGia, string dichGia, string ngonNgu, int soTrang, int namXuatBan, string nhaXuatBan, string loaiBia, string moTa)
        {
            this.maSP = maSP;
            this.maLoaiSP = maLoaiSP;
            this.ten = ten;
            this.gia = gia;
            this.soLuong = soLuong;
            this.luocBan = luocBan;
            this.tacGia = tacGia;
            this.dichGia = dichGia;
            this.ngonNgu = ngonNgu;
            this.soTrang = soTrang;
            this.namXuatBan = namXuatBan;
            this.nhaXuatBan = nhaXuatBan;
            this.loaiBia = loaiBia;
            this.moTa = moTa;
        }

        SanPham(SanPham sanPham)
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
        }
    }
}
