using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class QLSanPham
    {
        public List<SanPham> list { get; set; }

        public QLSanPham() { }

        public QLSanPham(List<SanPham> list)
        {
            this.list = list;
        }
        
        public QLSanPham(QLSanPham sanPham)
        {
            this.list = sanPham.list;
        }

        public int IndexOf(SanPham sanPham)
        {
            for(int index = 0; index < list.Count ; index++)
            {
                if (SanPham.EqualMaSP(list[index], sanPham))
                    return index;
            }

            return -1;
        }

        public void RemoveSer(List<string> maSPs)
        {
            Utils.Sort(list, 0, list.Count - 1, SanPham.CompareMaSP, SanPham.EqualMaSP);
            Utils.RemoveSer(list, maSPs);
        }

        public void RemoveAt(int index) => list.RemoveAt(index);

        public void Remove(SanPham sanPham)
        {
            foreach(SanPham sp in list)
            {
                if (SanPham.EqualMaSP(sp, sanPham))
                {
                    list.Remove(sp);
                    return;
                }
            }
        }

        public void AddRange(QLSanPham QLSP)
        {
            foreach(SanPham sanPham in QLSP.list)
            {
                Add(sanPham);
            }
        }

        public void Add(SanPham sanPham)
        {
            list.Add(sanPham);
        }

        public void Update(SanPham sanPham)
        {
            for (int index = 0; index < list.Count; index++)
            {
                if (SanPham.EqualMaSP(list[index], sanPham))
                    list[index] = sanPham;
            }
        }

        public QLSanPham GetAllSP()
        {
            return this;
        }

        public int tinhTongTien()
        {
            int tongTien = 0;

            foreach(SanPham sanPham in list)
            {
                tongTien += sanPham.gia * sanPham.soLuong;
            }

            return tongTien;
        }

        public int soLuongShop()
        {
            Utils.Sort(list, 0, list.Count - 1, SanPham.CompareMaS, SanPham.EqualMaS);

            int n = 1;
            string mas = list[0].maS;

            foreach (SanPham sanPham in list)
            {
                if (!String.Equals(mas, sanPham.maS))
                {
                    n++;
                    mas = sanPham.maS;
                }
            }

            return n;
        }
    }
}
