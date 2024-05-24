﻿using Program.DAL;
using Program.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.BLL
{
    internal class BLL_Shop
    {
        private static BLL_Shop _Instance;
        public static BLL_Shop Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BLL_Shop();
                return _Instance;
            }
            private set { }
        }
        private BLL_Shop()
        {

        }

        public void CapNhatThongTin(Shop Shop)
        {
            DAL_Shop.Instance.CapNhatThongTin(Shop);
        }

        public void CapNhatDiaChi(Shop shop, DiaChi diaChi)
        {
            if (shop.diaChi == null)
                ThemDiaChi(diaChi, shop.maSo);
            else
                DAL_DiaChi.Instance.CapNhatDiaChi(diaChi);

            shop.CapNhatDiaChi(diaChi);
        }

        public int GetSoLuongFromMaSP(QLBaiDang listBaiDang, string maSP)
        {
            foreach(BaiDang baiDang in listBaiDang.list)
            {
                foreach(SanPham sanPham in baiDang.list)
                {
                    if (maSP.Equals(sanPham.maSP))
                    {
                        return sanPham.soLuong;
                    }
                }
            }
            return -1;
        }

        public void GiaoHang(Shop shop, string maDH)
        {
            int index = shop.listDonHang.IndexOf(maDH);
            shop.listDonHang.list[index].ngayGiaoHang = DateTime.Now; // another
            shop.listDonHang.list[index].tinhTrang = 1;

            foreach (SanPham sanPham in shop.listDonHang.list[index].list)
            {
                shop.listBaiDang.GetSanPhamFromMaSP(sanPham.maSP).soLuong -= sanPham.soLuong;
                DAL_SanPham.Instance.GiaoHang(sanPham.maSP, sanPham.soLuong);
            }

            ThongBao thongBao = new ThongBao
            {
                maTB = BLL_ThongBao.Instance.GetMaMoi(),
                from = "S" + shop.maSo,
                to = "KH" + shop.listDonHang.list[index].maKH,
                dinhKem = "DH" + shop.listDonHang.list[index].maDH,
                noiDung = $"Đơn hàng (DH{shop.listDonHang.list[index].maDH}) của bạn đã được gửi đi vào lúc {Utils.Instance.MoTaThoiGian(DateTime.Now)} và dự kiến sẽ đến vào lúc {Utils.Instance.MoTaThoiGian(shop.listDonHang.list[index].ngayGiaoHang)}",
                ngayGui = DateTime.Now,
                tinhTrang = 0
            };

            BLL_ThongBao.Instance.ThemThongBao(thongBao);
            BLL_DonHang.Instance.GiaoHang(shop.listDonHang.list[index].maDH, shop.listDonHang.list[index].ngayDatHang);

        }

        public string GetTenShopFromMaS(string maS)
        {
            return DAL_Shop.Instance.LoadTenShopFromMaS(maS);
        }

        public void ThemDiaChi(DiaChi diaChi, string maS)
        {
            DAL_DiaChi.Instance.ThemDiaChi(diaChi, maS, 0);
        }

        public string GetMaMoi()
        {
            return Database.Instance.MaMoi("maS");
        }

        public void giaoHang(DonHang donHang)
        {
            DAL_DonHang.Instance.CapNhatTinhTrangDonHang(donHang.maDH, donHang.tinhTrang, donHang.ngayGiaoHang);

            foreach (SanPham sanPham in donHang.list)
            {
                DAL_SanPham.Instance.GiaoHang(sanPham.maSP, sanPham.soLuong);
            }
        }

        public void TaoShopByTaiKhoan(string taiKhoan, Shop shop)
        {
            string maKH = DAL_KhachHang.Instance.LoadMaKHFromTaiKhoan(taiKhoan);
            DAL_Shop.Instance.TaoShop(shop);
            DAL_KhachHang.Instance.DangKyTaoShop(maKH, shop.maSo);
        }

        public Shop GetShopFromTaiKhoan(string taiKhoan)
        {
            if (DAL_KhachHang.Instance.KiemTraKhachHang_Shop(taiKhoan)){
                return DAL_Shop.Instance.LoadShopFromTaiKhoan(taiKhoan);
            }
            else
            {
                return null;
            }
        }
    }
}
