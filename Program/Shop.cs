using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class Shop : Nguoi
    {
        public List<BaiDang> list { get; private set; }
        public int nFollower { get; private set; }
        public int tinhTrang { get; private set; }
        public int doanhThu { get; private set; }

        public Shop() : base()
        {
            list = new List<BaiDang>();
            nFollower = 0;
            doanhThu = 0;
            tinhTrang = 1;
        }

        public Shop(string maSo, string ten, string soDT, string email, DiaChi diaChi, int _, DateTime ngaySinh, List<BaiDang> list, int nFollower, int tinhTrang, int doanhThu) : base(maSo, ten, soDT, email, diaChi, _, ngaySinh)
        {
            this.list = list;
            this.nFollower = nFollower;
            this.doanhThu = doanhThu;
            this.tinhTrang = tinhTrang;
        }

        public Shop(string maSo) : base(maSo){ }

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

        public void themBaiDang(BaiDang baiDang)
        {
            list.Add(baiDang);
        }

        public void themBaiDang(int index, BaiDang baiDang)
        {
            list.Insert(index, baiDang);
        }

        public void xoaBaiDang(BaiDang baiDang)
        {
            list.Remove(baiDang);
        }

        public void xoaBaiDang(int index)
        {
            list.RemoveAt(index);
        }

        public void capNhatBaiDang(BaiDang baiDang)
        {
            int index = list.IndexOf(baiDang);
            list[index] = baiDang;
        }

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

        public void capNhatTinhTrang(int tinhTrang)
        {
            this.tinhTrang = tinhTrang;
        }
    }
}
