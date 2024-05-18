using Program.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.BLL
{
    internal class BLL_GioHang
    {
        private static BLL_GioHang _Instance;
        public static BLL_GioHang Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BLL_GioHang();
                return _Instance;
            }
            private set { }
        }

        private BLL_GioHang()
        {
            
        }

        public void XoaSPKhoiGioHang(KhachHang khachHang, int index)
        {
            DAL_GioHang.Instance.XoaSanPham(khachHang.maSo, khachHang.gioHang.list[index].maSP);
            khachHang.XoaSPKhoiGioHang(index);
        }

        public void ThemSPVaoGioHang(KhachHang khachHang, SanPham sanPham, int soLuong)
        {
            SanPham item = sanPham.Clone();
            item.soLuong = soLuong;
            item.ngayThem = DateTime.Now;

            if (khachHang.gioHang.IsExist(sanPham))
                DAL_GioHang.Instance.CapNhatSanPham(khachHang.maSo, item.maSP, item.soLuong);
            else
                DAL_GioHang.Instance.ThemSanPham(khachHang.maSo, item.maSP, item.soLuong);

            khachHang.ThemVaoGioHang(item);
        }

        public string GetSoInGioHangIcon(GioHang gioHang)
        {
            int n = gioHang.list.Count;

            if (n > 99)
                return "   99+";
            else
                return "  " + n.ToString();
        }

        public void CapNhatSoLuong(string maKH, string maSP, int soLuong)
        {
            DAL_GioHang.Instance.CapNhatSanPham(maKH, maSP, soLuong);
        }
    }
}
