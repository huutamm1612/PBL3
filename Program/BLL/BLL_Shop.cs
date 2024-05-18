using Program.DAL;
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
