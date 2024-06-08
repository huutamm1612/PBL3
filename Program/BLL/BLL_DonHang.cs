using Program.DAL;
using Program.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.BLL
{
    internal class BLL_DonHang
    {
        private static BLL_DonHang _Instance;
        public static BLL_DonHang Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BLL_DonHang();
                return _Instance;
            }
            private set { }
        }
        private BLL_DonHang()
        {

        }

        public string GetMaMoi()
        {
            return Database.Instance.MaMoi("maDH");
        }

        public List<string> GetAllLyDoHuyDonByKhachHang()
        {
            return DAL_DonHang.Instance.LoadAllLyDoHuyDonByKhachHang();
        }

        public List<DonHang> PhanRaDonHang(QLSanPham qLSanPham)
        {
            List<DonHang> list = new List<DonHang>();

            foreach(QLSanPham listSP in qLSanPham.PhanRaTheoShop())
            {
                list.Add(new DonHang
                {
                    list = listSP.list,
                    tongTien = listSP.tinhTongTien() + 30000,
                    tinhTrang = 0,
                    ngayDatHang = DateTime.Now,
                    ngayGiaoHang = DateTime.Now
                });
            }

            return list;
        }

        public int TongTien(QLSanPham qLSanPham, int xu)
        {
            return qLSanPham.tinhTongTien() + 30000 * qLSanPham.SoLuongShop() - xu;
        }

        public void DatHang(DonHang donHang)
        {
            DAL_DonHang.Instance.DatHang(donHang);
            DAL_KhachHang.Instance.DatHang(donHang.maKH, donHang.maDH);
            DAL_Shop.Instance.DatHang(donHang.list[0].maS, donHang.maDH);

            foreach(SanPham sanPham in donHang.list)
            {
                DAL_SanPham.Instance.DatHang(sanPham.maSP, donHang.maDH, sanPham.soLuong);
            }
        }

        public void KiemTraDHLaDaDen(DonHang donHang)
        {
            if (donHang.tinhTrang == 1 && DateTime.Compare(DateTime.Now, donHang.ngayGiaoHang) >= 0 && !BLL_ThongBao.Instance.IsVanChuyenDaGui(donHang.maDH))
            { 
                ThongBao thongBao = new ThongBao
                {
                    maTB = BLL_ThongBao.Instance.GetMaMoi(),
                    from = "BenVanChuyen",
                    to = "KH" + donHang.maKH,
                    dinhKem = "DH" + donHang.maDH,
                    noiDung = $"Đơn hàng (DH{donHang.maDH}) của bạn được bàn giao cho bên vận chuyển và sẽ được giao trong thời gian sắp tới, hãy chú ý điện thoại từ shipper. Vui lòng bỏ qua thông báo này nếu bạn đã nhận được hàng!",
                    ngayGui = donHang.ngayGiaoHang,
                    tinhTrang = 0
                };

                DAL_ThongBao.Instance.ThemThongBao(thongBao);
            }
        }

        public void GiaoHang(string maDH, DateTime ngayGiaoHang)
        {
            DAL_DonHang.Instance.GiaoHang(maDH, ngayGiaoHang);
        }

        public void NhanHang(DonHang donHang)
        {
            DAL_DonHang.Instance.CapNhatTinhTrangDonHang(donHang.maDH, donHang.tinhTrang, donHang.ngayGiaoHang);
            DAL_KhachHang.Instance.NhanHang(donHang.maKH, donHang.tongTien);
            DAL_Shop.Instance.GiaoHangThanhCong(donHang.list[0].maS, donHang.tongTien + donHang.xu);
        }
        public int DemSoLuong(QLDonHang donHangList, int tinhTrang) // =0
        {
            int count = 0;

            foreach(DonHang donHang in donHangList.list)
            {
                if (donHang.tinhTrang == tinhTrang)
                    count++;
            }

            return count;
        }
    }
}
