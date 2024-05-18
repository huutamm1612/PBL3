using Program.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.BLL
{
    internal class BLL_KhachHang
    {
        private static BLL_KhachHang _Instance;

        public static BLL_KhachHang Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BLL_KhachHang();
                return _Instance;
            }
            private set { }
        }

        private BLL_KhachHang()
        {

        }

        public void ThemDiaChi(DiaChi diaChi, string maKH)
        {
            DAL_DiaChi.Instance.ThemDiaChi(diaChi, maKH, 1);
        }

        public KhachHang GetKhachHangFromTaiKhoan(string taiKhoan)
        {
            return DAL_KhachHang.Instance.LoadKhachHangFromTaiKhoan(taiKhoan);
        }

        public void Thich(KhachHang khachHang, BaiDang baiDang)
        {
            DAL_KhachHang.Instance.ThemThich(khachHang.maSo, baiDang.maBD);
            DAL_BaiDang.Instance.Thich(baiDang.maBD);
            baiDang.luocThich += 1;
            khachHang.Thich(baiDang.maBD);
        }

        public void HuyThich(KhachHang khachHang, BaiDang baiDang)
        {
            DAL_KhachHang.Instance.XoaThich(khachHang.maSo, baiDang.maBD);
            DAL_BaiDang.Instance.HuyThich(baiDang.maBD);
            baiDang.luocThich -= 1;
            khachHang.HuyThich(baiDang.maBD);
        }

        public void TheoDoi(KhachHang khachHang, Shop shop)
        {
            DAL_KhachHang.Instance.ThemFollow(khachHang.maSo, shop.maSo);
            shop.Follow(khachHang.maSo);
            khachHang.Follow(shop.maSo);
        }

        public void HuyTheoDoi(KhachHang khachHang, Shop shop)
        {
            DAL_KhachHang.Instance.XoaFollow(khachHang.maSo, shop.maSo);
            shop.UnFollow(khachHang.maSo);
            khachHang.UnFollow(shop.maSo);
        }

        public void HuyDonHang(DonHang donHang)
        {
            DAL_DonHang.Instance.CapNhatTinhTrangDonHang(donHang.maDH, donHang.tinhTrang, donHang.ngayGiaoHang);
        }

        public bool DaTheoDoi(List<string> listFollow, string maS)
        {
            return listFollow.Contains(maS);
        }

        public bool DaThich(List<string> listThich, string maBD)
        {
            return listThich.Contains(maBD);
        }

        public void CapNhatDiaChiMacDinh(KhachHang khachHang, DiaChi diaChi)
        {
            khachHang.thayDoiDiaChiMacDinh(diaChi);
            DAL_DiaChi.Instance.CapNhatDiaChiMacDinh(khachHang.maSo, diaChi.maDC);
        }

        public void XoaDiaChi(KhachHang khachHang, DiaChi diaChi)
        {
            DAL_DiaChi.Instance.XoaDiaChi(diaChi.maDC);
            khachHang.xoaDiaChi(diaChi);
        }

        public bool KiemTraTaoShop(string maKH)
        {
            return DAL_KhachHang.Instance.KiemTraKhachHang_Shop(maKH);
        }
    }
}
