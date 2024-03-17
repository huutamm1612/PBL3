using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal abstract class QLSanPham
    {
        public List<SanPham> list { get; protected set; }

        public void themSanPham(SanPham sanPham)
        {
            list.Add(sanPham);
        }

        public void themSanPham(int index, SanPham sanPham)
        {
            list.Insert(index, sanPham);
        }

        public void xoaSanPham(SanPham sanPham)
        {
            list.Remove(sanPham);
        }

        public void xoaSanPham(int index)
        {
            list.RemoveAt(index);
        }

        public void capNhatSanPham(SanPham sanPham)
        {
            int index = list.IndexOf(sanPham);
            list[index] = sanPham;
        }
    }
}
