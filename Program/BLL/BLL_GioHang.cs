using Program.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
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

        public void XoaSPKhoiGioHang(GioHang gioHang, params SanPham[] list)
        {
            foreach(SanPham sanPham in list)
            {
                XoaSPKhoiGioHang(gioHang, sanPham);
            }
        }

        public void XoaSPKhoiGioHang(GioHang gioHang, SanPham sanPham)
        {
            DAL_GioHang.Instance.XoaSanPham(gioHang.maKH, sanPham.maSP);
            gioHang.Remove(sanPham);
        }

        public void XoaSPKhoiGioHang(GioHang gioHang, int index)
        {
            DAL_GioHang.Instance.XoaSanPham(gioHang.maKH, gioHang.list[index].maSP);
            gioHang.RemoveAt(index);
        }

        public void ThemSPVaoGioHang(GioHang gioHang, SanPham sanPham, int soLuong)
        {
            SanPham item = sanPham.Clone();
            item.soLuong = soLuong;
            item.ngayThem = DateTime.Now;

            if (gioHang.IsExist(sanPham))
                DAL_GioHang.Instance.CapNhatSanPham(gioHang.maKH, item.maSP, item.soLuong);
            else
                DAL_GioHang.Instance.ThemSanPham(gioHang.maKH, item.maSP, item.soLuong);

            gioHang.Add(item);
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
