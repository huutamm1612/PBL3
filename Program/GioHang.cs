using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class GioHang : QLSanPham
    {
        public string maKH { get; set; }

        public GioHang()
        {
            list = new List<SanPham>();
            maKH = "";
        }

        public override void Add(object item)
        {
            SanPham sanPham = item as SanPham;
            sanPham.ngayThem = DateTime.Now;

            foreach(SanPham sp in list)
            {
                if(SanPham.EqualMaSP(sanPham, sp))
                {
                    sp.ngayThem = sanPham.ngayThem;
                    sp.soLuong += sanPham.soLuong;

                    return;
                }
            }

            list.Insert(0, sanPham);
        }

        public override void RemoveRange(params string[] maSPs)
        {
            base.RemoveRange(maSPs);
            HeThong.XoaSPKhoiGioHang(maKH, maSPs);
        }
    }
}
