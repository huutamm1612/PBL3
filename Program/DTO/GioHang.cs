using Program.BLL;
using Program.DAL;
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
            SanPham sanPham = (item as SanPham).Clone();
            sanPham.ngayThem = DateTime.Now;

            foreach(SanPham sp in list)
            {
                if(SanPham.EqualMaSP(sanPham, sp))
                {
                    sanPham.soLuong += sp.soLuong;
                    list.Remove(sp);
                    break;
                }
            }

            list.Insert(0, sanPham);
        }

        public void Update(SanPham sanPham)
        {
            foreach (SanPham sp in list)
            {
                if (SanPham.EqualMaSP(sp, sanPham))
                {
                    sp.soLuong = sanPham.soLuong;
                    sp.ngayThem = sanPham.ngayThem;
                    return;
                }
            }
        }

        public void Remove(string maSP)
        {
            foreach(SanPham sanPham in list)
            {
                if (maSP.Equals(sanPham.maSP))
                {
                    list.Remove(sanPham);
                    return;
                }
            }
        }

        public override void RemoveRange(params string[] maSPs)
        {
            base.RemoveRange(maSPs);
            foreach(string masp in maSPs)
            {
                DAL_GioHang.Instance.XoaSanPham(maKH, masp);
            }
        }

        public static bool CompareNgayThem(object o1, object o2) => ((SanPham)o1).ngayThem < ((SanPham)o2).ngayThem;

        public static bool EqualNgayThem(object o1, object o2) => ((SanPham)o1).ngayThem == ((SanPham)o2).ngayThem; 
    }
}
