using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class QLBaiDang : IQuanLy
    {
        public List<BaiDang> list { get; set; }

        public QLBaiDang()
        {
            list = new List<BaiDang>();
        }

        public void Add(object item)
        {
            foreach (BaiDang baiDang in list)
            {
                if (BaiDang.EqualMaBD(item, baiDang))
                {
                    return;
                }
            }
            list.Add(item as BaiDang);
        }

        public int IndexOf(object item)
        {
            for (int i = 0; i < list.Count; i++)
                if (BaiDang.EqualMaBD(list[i], item))
                    return i;

            return -1;
        }

        public void Remove(object item)
        {
            foreach(var i in list)
                if(i.Equals(item))
                    list.Remove(i);
        }

        public void Remove(string maBD)
        {
            foreach (BaiDang baiDang in list)
            {
                if (baiDang.maBD.Equals(maBD))
                {
                    list.Remove(baiDang);
                    return;
                }
            }
        }

        public void RemoveAt(int index)
        {
            if(index == -1)
                list.RemoveAt(list.Count - 1);
                
            list.RemoveAt(index);
        }

        public void RemoveSanPhamFromMaSP(string maSP)
        {
            foreach(BaiDang baiDang in list)
            {
                baiDang.Remove(maSP);
            }
        }

        public SanPham GetSanPhamFromMaSP(string maSP)
        {
            foreach(BaiDang baiDang in list)
            {
                if (baiDang.GetSanPhamFromMaSP(maSP) != null)
                    return baiDang.GetSanPhamFromMaSP(maSP);
            }

            return null;
        }
        public BaiDang GetBaiDangFromMaBD(string maBD)
        {
            foreach(BaiDang baiDang in list)
            {
                if (baiDang.maBD == maBD) {
                    return baiDang;
                }
            }
            return null;
        }

        public void Clear() => list.Clear();

        public BaiDang Last() => list.Last();

        public void Add(int index, SanPham sanPham)
        {
            if(index == -1)
                list.Last().Add(sanPham);
            else
                list[index].Add(sanPham);
        }
        public int SoLuongSanPham()
        {
            int n = 0;
            foreach (BaiDang baiDang in list)
            {
                n += baiDang.list.Count;
            }
            return n;
        }

        public int SoLuongDanhGia()
        {
            int n = 0;
            foreach(BaiDang baiDang in list)
            {
                n += baiDang.listDanhGia.list.Count;
            }
            return n;
        }
    }
}
