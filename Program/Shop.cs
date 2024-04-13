using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Program
{
    internal class Shop : Nguoi
    {
        public int nFollower { get; set; }
        public int tinhTrang { get; set; }
        public int doanhThu { get; set; }
        public QLDonHang listDonHang { get; set; }
        public QLBaiDang listBaiDang { get; set; }

        public Shop() : base()
        {
            listBaiDang = new QLBaiDang();
            nFollower = 0;
            doanhThu = 0;
            tinhTrang = 1;
            listDonHang = null;
        }

        public Shop(string soDT, string email) : base()
        {
            listBaiDang = new QLBaiDang();
            nFollower = 0;
            doanhThu = 0;
            tinhTrang = 1;
            this.soDT = soDT;
            this.email = email;
            listDonHang = null;
        }

        public Shop(string maSo, string ten, string soDT, string email, DiaChi diaChi, DateTime ngaySinh, List<BaiDang> list, int nFollower, int tinhTrang, int doanhThu)
        {
            this.maSo = maSo;
            this.ten = ten;
            this.soDT = soDT;
            this.email = email;
            this.diaChi = diaChi;
            this.ngaySinh = ngaySinh;
            this.listBaiDang = listBaiDang;
            this.nFollower = nFollower;
            this.doanhThu = doanhThu;
            this.tinhTrang = tinhTrang;
        }

        public Shop(string maSo)
        {
            this.maSo = maSo;
        }

        public Shop(Shop shop)
        {
            this.maSo = shop.maSo;
            this.ten = shop.ten;
            this.soDT = shop.soDT;
            this.email = shop.email;
            this.diaChi = shop.diaChi;
            this.ngaySinh = shop.ngaySinh;
            this.listBaiDang = shop.listBaiDang;
            this.nFollower = shop.nFollower;
            this.tinhTrang = shop.tinhTrang;
            this.doanhThu = shop.doanhThu;
            this.listDonHang = shop.listDonHang;
        }

        public void Add(BaiDang baiDang)
        {
            listBaiDang.Add(baiDang);
        }
        public void Insert(int index, BaiDang baiDang)
        {
            listBaiDang.list.Insert(index, baiDang);
        }

        public int IndexOf(BaiDang baiDang)
        {
            for (int index = 0; index < listBaiDang.list.Count; index++)
            {
                if (BaiDang.EqualMaBD(listBaiDang.list[index], baiDang))
                    return index;
            }

            return -1;
        }
        public void Remove(BaiDang baiDang)
        {
            foreach (BaiDang bd in listBaiDang.list)
            {
                if (BaiDang.EqualMaBD(bd, baiDang))
                {
                    listBaiDang.list.Remove(bd);
                    HeThong.XoaBaiDang(baiDang.maBD);
                    return;
                }
            }
        }

        public void RemoveRange(List<string> maBDs)
        {
            Utils.Sort(listBaiDang.list, 0, listBaiDang.list.Count - 1, BaiDang.CompareMaBD, BaiDang.EqualMaBD);
            Utils.RemoveRange(listBaiDang.list, maBDs);
            foreach(string maBD in maBDs)
            {
                HeThong.XoaBaiDang(maBD);
            }
        }

        public void RemoveAt(int index)
        {
            HeThong.XoaBaiDang(listBaiDang.list[index].maBD);
            listBaiDang.list.RemoveAt(index);
        }

        public void Update(BaiDang baiDang)
        {
            for (int i = 0; i < listBaiDang.list.Count; i++)
            {
                if (BaiDang.EqualMaBD(listBaiDang.list[i], baiDang))
                {
                    listBaiDang.list[i] = baiDang;
                    HeThong.CapNhatBaiDang(baiDang);
                    return;
                }
            }
        }

        public SanPham[] GetAllSP()
        {
            QLSanPham qlSP = new QLSanPham();

            foreach(BaiDang baiDang in listBaiDang.list)
            {
                qlSP.AddRange(baiDang.GetAllSP());
            }

            return qlSP.list.ToArray();
        }

        public SanPham searchSanPham(string maSP, string maBD)
        {
            foreach(var baiDang in listBaiDang.list)
                if(baiDang.maBD == maBD)
                    foreach(var sanPham in baiDang.list)
                        if (sanPham.maSP == maSP)
                            return sanPham;

            return null;
        }

        public void diDon(params DonHang[] list) 
        {
            foreach(DonHang donHang in list)
            {
                donHang.capNhatTinhTrang(2);
                donHang.diDon();
                
                foreach(var sanPham in donHang.list)
                {
                    SanPham sp = searchSanPham(sanPham.maSP, sanPham.maBD);
                    sp.soLuong -= sanPham.soLuong;
                    sp.luocBan += sanPham.soLuong;

                    HeThong.CapNhatSanPham(sp);
                }
            }
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
