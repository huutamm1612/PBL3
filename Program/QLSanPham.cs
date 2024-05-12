using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class QLSanPham : IQuanLy
    {
        public List<SanPham> list { get; set; }

        public QLSanPham()
        {
            list = new List<SanPham>();
        }

        public QLSanPham(List<SanPham> list)
        {
            this.list = list;
        }
        
        public QLSanPham(QLSanPham sanPham)
        {
            this.list = sanPham.list;
        }

        public SanPham[] ToArray()
        {
            return list.ToArray();
        }

        public virtual void Add(object item)
        {
            foreach(SanPham sanPham in list)
            {
                if(SanPham.EqualMaSP(sanPham, item))
                {
                    sanPham.soLuong += ((SanPham)item).soLuong;
                    return;
                }
            }
            list.Add((SanPham)item);
        }

        public bool IsExist(SanPham sanPham)
        {
            foreach (SanPham item in list)
            {
                if (SanPham.EqualMaSP(item, sanPham))
                    return true;
            }
            return false;
        }

        public void Clear() => list.Clear();

        public int IndexOf(object item)
        {
            for (int i = 0; i < list.Count; i++)
                if (SanPham.EqualMaSP(list[i], item))
                    return i;

            return -1;
        }

        public void Remove(object item)
        {
            foreach (SanPham i in list)
                if (i.Equals(item))
                    list.Remove(i);
        }

        public void RemoveAt(int index)
        {
            if (index == -1)
                list.RemoveAt(list.Count - 1);

            list.RemoveAt(index);
        }

        public virtual void RemoveRange(params string[] maSPs)
        {
            Utils.Sort(list, 0, list.Count - 1, SanPham.CompareMaSP, SanPham.EqualMaSP);
            Utils.RemoveRange(list, maSPs.ToList());
        }

        public void AddRange(params SanPham[] listSanPham)
        {
            foreach(SanPham sanPham in listSanPham)
            {
                Add(sanPham);
            }
        }

        public void Update(SanPham sanPham)
        {
            for (int index = 0; index < list.Count; index++)
            {
                if (SanPham.EqualMaSP(list[index], sanPham))
                    list[index] = sanPham;
            }
        }

        public SanPham[] GetAllSP()
        {
            return list.ToArray();
        }

        public int tinhTongTien()
        {
            int tongTien = 0;

            foreach(SanPham sanPham in list)
            {
                tongTien += (int)(sanPham.gia * sanPham.soLuong * HeThong.GetGiamGia(sanPham.maSP));
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
