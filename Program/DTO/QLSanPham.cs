using Program.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    public class QLSanPham : IQuanLy
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
            {
                if (i.Equals(item))
                {
                    list.Remove(i);
                    return;
                }
            }
        }

        public void Remove(string maSP)
        {
            foreach (SanPham item in list)
            {
                if (item.maSP.Equals(maSP))
                {
                    list.Remove(item);
                    return;
                }
            }
        }

        public void RemoveAt(int index)
        {
            if (index == -1)
                list.RemoveAt(list.Count - 1);

            list.RemoveAt(index);
        }

        public virtual void RemoveRange(params string[] maSPs)
        {
            Utils.Instance.Sort(list, 0, list.Count - 1, SanPham.CompareMaSP, SanPham.EqualMaSP);
            Utils.Instance.RemoveRange(list, maSPs.ToList());
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
                {
                    list[index] = sanPham;
                    return;
                }
            }
        }

        public SanPham[] GetAllSP()
        {
            return list.ToArray();
        }

        public SanPham GetSanPhamFromMaSP(string maSP)
        {
            foreach(SanPham sanPham in list)    
            {
                if(maSP.Equals(sanPham.maSP))
                    return sanPham;
            }

            return null;
        }

        public int tinhTongTien(bool isGiamGia = true)
        {
            int tongTien = 0;

            foreach(SanPham sanPham in list)
            {
                if (isGiamGia)
                    tongTien += Utils.Instance.GiamGia(sanPham.gia, BLL_BaiDang.Instance.GetGiamGiaFromMaSP(sanPham.maSP)) * sanPham.soLuong;
                else
                    tongTien += sanPham.soLuong * sanPham.gia;
            }

            return tongTien;
        }

        public int SoLuongBaiDang()
        {
            Utils.Instance.Sort(list, 0, list.Count - 1, SanPham.CompareMaBD, SanPham.EqualMaBD);
            int n = 1;
            string maBD = list[0].maBD;

            foreach (SanPham sanPham in list)
            {
                if (!String.Equals(maBD, sanPham.maBD))
                {
                    n++;
                    maBD = sanPham.maBD;
                }
            }

            return n;
        }

        public int SoLuongShop()
        {
            Utils.Instance.Sort(list, 0, list.Count - 1, SanPham.CompareMaS, SanPham.EqualMaS);

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

        public List<QLSanPham> PhanRaTheoBaiDang()
        {

            List<QLSanPham> listQLSP = new List<QLSanPham>();
            for (int i = 0; i < SoLuongBaiDang(); i++)
                listQLSP.Add(new QLSanPham());

            foreach (SanPham item in list)
            {
                foreach (QLSanPham qLSanPham in listQLSP)
                {
                    if (qLSanPham.list.Count == 0)
                    {
                        qLSanPham.Add(item);
                        break;
                    }
                    else if (SanPham.EqualMaBD(qLSanPham.list[0], item))
                    {
                        qLSanPham.Add(item);
                        break;
                    }
                }
            }

            return listQLSP;
        }
        
        public List<QLSanPham> PhanRaTheoShop()
        {
            List<QLSanPham> listQLSP = new List<QLSanPham> ();
            for (int i = 0; i < SoLuongShop(); i++)
                listQLSP.Add(new QLSanPham());

            foreach (SanPham item in list)
            {
                foreach(QLSanPham qLSanPham in listQLSP)
                {
                    if (qLSanPham.list.Count == 0)
                    {
                        qLSanPham.Add(item);
                        break;
                    }
                    else if (SanPham.EqualMaS(qLSanPham.list[0], item))
                    {
                        qLSanPham.Add(item);
                        break;
                    }
                }
            }

            return listQLSP;
        }
    }
}
