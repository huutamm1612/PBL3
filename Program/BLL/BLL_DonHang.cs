using Program.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void DatHang(DonHang donHang)
        {
            DAL_DonHang.Instance.DatHang(donHang);
            DAL_KhachHang.Instance.DatHang(donHang.maKH, donHang.maDH);
            DAL_Shop.Instance.DatHang(donHang.list[0].maS, donHang.maDH);

            foreach(SanPham sanPham in donHang.list)
            {
                DAL_SanPham.Instance.DatHang(sanPham.maSP, donHang.maDH);
            }
        }

        public void NhanHang(DonHang donHang)
        {
            DAL_DonHang.Instance.CapNhatTinhTrangDonHang(donHang.maDH, donHang.tinhTrang, donHang.ngayGiaoHang);
            DAL_KhachHang.Instance.NhanHang(donHang.maKH, donHang.tongTien);
            DAL_Shop.Instance.GiaoHangThanhCong(donHang.list[0].maS, donHang.tongTien + donHang.xu);
        }

    }
}
