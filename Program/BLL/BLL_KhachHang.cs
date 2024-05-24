using Program.DAL;
using Program.DTO;
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

        public void DatHang(KhachHang khachHang, QLSanPham listSanPham, DiaChi diaChi, int ptThanhToan, bool dungXu)
        {
            int soLuongShop = listSanPham.soLuongShop();
            int xu = 0;
            if (dungXu)
            {
                xu = khachHang.xu;
                khachHang.xu = 0;
                DAL_KhachHang.Instance.DungXu(khachHang.maSo);
            }

            foreach(DonHang donHang in BLL_DonHang.Instance.PhanRaDonHang(listSanPham))
            {
                string maS = donHang.list[0].maS;

                donHang.maDH = BLL_DonHang.Instance.GetMaMoi();
                donHang.maKH = khachHang.maSo;
                donHang.ptThanhToan = ptThanhToan;
                donHang.xu = xu / soLuongShop;
                donHang.maS = maS;
                donHang.diaChi = diaChi;

                khachHang.listDonHang.Add(donHang);
                BLL_DonHang.Instance.DatHang(donHang);

                ThongBao thongBao = new ThongBao
                {
                    maTB = BLL_ThongBao.Instance.GetMaMoi(),
                    from = "KH" + khachHang.maSo,
                    to = "S" + maS,
                    dinhKem = "DH" + donHang.maDH,
                    noiDung = $"Khách hàng {khachHang.taiKhoan} đã đặt đơn hàng DH{donHang.maDH} vào lúc {Utils.Instance.MoTaThoiGian(DateTime.Now)}",
                    ngayGui = DateTime.Now,
                    tinhTrang = 0
                };

                BLL_ThongBao.Instance.ThemThongBao(thongBao);
            }

            BLL_GioHang.Instance.XoaSPKhoiGioHang(khachHang.gioHang, listSanPham.ToArray());
        }

        public void NhanHang(KhachHang khachHang, string maDH)
        {
            int index = khachHang.listDonHang.IndexOf(maDH);
            khachHang.listDonHang.list[index].ngayGiaoHang = DateTime.Now;
            khachHang.listDonHang.list[index].tinhTrang = 2;

            ThongBao thongBao = new ThongBao
            {
                maTB = BLL_ThongBao.Instance.GetMaMoi(),
                from = "KH" + khachHang.maSo,
                to = "S" + khachHang.listDonHang.list[index].maS,
                dinhKem = "DH" + khachHang.listDonHang.list[index].maDH,
                noiDung = $"Đơn hàng DH{khachHang.listDonHang.list[index].maDH} đã được giao thành công vào lúc {Utils.Instance.MoTaThoiGian(DateTime.Now)}. Doanh thu của bạn đã tăng thêm ₫{Utils.Instance.SetGia(khachHang.listDonHang.list[index].tongTien + khachHang.listDonHang.list[index].xu)}.",
                ngayGui = DateTime.Now,
                tinhTrang = 0
            };

            DAL_ThongBao.Instance.ThemThongBao(thongBao);
            DAL_DonHang.Instance.CapNhatTinhTrangDonHang(khachHang.listDonHang.list[index].maDH, khachHang.listDonHang.list[index].tinhTrang, khachHang.listDonHang.list[index].ngayGiaoHang);
            DAL_KhachHang.Instance.NhanHang(khachHang.maSo, khachHang.listDonHang.list[index].tongTien);
            DAL_Shop.Instance.GiaoHangThanhCong(khachHang.listDonHang.list[index].maS, khachHang.listDonHang.list[index].tongTien + khachHang.listDonHang.list[index].xu);
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
