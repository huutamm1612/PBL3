using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class Shop : Nguoi
    {
        public List<BaiDang> list { get; set; }
        public int nFollower { get; set; }
        public int tinhTrang { get; set; }
        public int doanhThu { get; set; }

        public Shop() : base()
        {
            nFollower = 0;
            doanhThu = 0;
            tinhTrang = 1;
        }

        public Shop(string soDT, string email) : base()
        {
            list = new List<BaiDang>();
            nFollower = 0;
            doanhThu = 0;
            tinhTrang = 1;
            this.soDT = soDT;
            this.email = email;
        }

        public Shop(string maSo, string ten, string soDT, string email, DiaChi diaChi, int _, DateTime ngaySinh, List<BaiDang> list, int nFollower, int tinhTrang, int doanhThu) : base(maSo, ten, soDT, email, diaChi, _, ngaySinh)
        {
            this.list = list;
            this.nFollower = nFollower;
            this.doanhThu = doanhThu;
            this.tinhTrang = tinhTrang;
        }

        public Shop(string maSo) : base(maSo) { }

        public Shop(Shop shop) : base(shop)
        {
            this.list = shop.list;
            this.nFollower = shop.nFollower;
            this.tinhTrang = shop.tinhTrang;
            this.doanhThu = shop.doanhThu;
        }

        public void setMaSo(string maSo)
        {
            this.maSo = maSo;
        }

        public void Add(BaiDang baiDang) => list.Add(baiDang);
        public void Insert(int index, BaiDang baiDang) => list.Insert(index, baiDang);
        public int IndexOf(BaiDang baiDang)
        {
            for (int index = 0; index < list.Count; index++)
            {
                if (BaiDang.EqualMaBD(list[index], baiDang))
                    return index;
            }

            return -1;
        }
        public void Remove(BaiDang baiDang)
        {
            foreach (BaiDang bd in list)
            {
                if (BaiDang.EqualMaBD(bd, baiDang))
                {
                    list.Remove(bd);
                    return;
                }
            }
        }

        public void RemoveSer(List<string> maBDs)
        {
            Utils.Sort(list, 0, list.Count - 1, BaiDang.CompareMaBD, BaiDang.EqualMaBD);
            Utils.RemoveSer(list, maBDs);
        }

        public void RemoveAt(int index) => list.RemoveAt(index);

        public void Update(BaiDang baiDang)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (BaiDang.EqualMaBD(list[i], baiDang))
                {
                    list[i] = baiDang;
                    return;
                }
            }
        }

        public QLSanPham GetAllSP()
        {
            QLSanPham qlSP = new QLSanPham();

            foreach(BaiDang baiDang in list)
            {
                qlSP.AddRange(baiDang.GetAllSP());
            }

            return qlSP;
        }

        public void setNgayTaoShop() => ngaySinh = DateTime.Now;

        public override void nhap(string ten, string email, string soDT, int _, DateTime ngaySinh)
        {
            this.ten = ten;
            this.email = email;
            this.soDT = soDT;
            this.ngaySinh = ngaySinh;
        }

        public override void sua(string ten, string email, string soDT, int _, DateTime ngaySinh)
        {
            this.ten = ten;
            this.email = email;
            this.soDT = soDT;
            this.ngaySinh = ngaySinh;
        }

        public void capNhatTinhTrang(int tinhTrang) => this.tinhTrang = tinhTrang;
    }
}
