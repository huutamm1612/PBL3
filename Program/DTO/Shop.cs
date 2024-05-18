using Program.BLL;
using Program.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Program
{
    internal class Shop : Nguoi
    {
        public int tinhTrang { get; set; }
        public int doanhThu { get; set; }
        public List<string> listFollower { get; set; }
        public QLDonHang listDonHang { get; set; }
        public QLBaiDang listBaiDang { get; set; }

        public Shop()
        {

            this.maSo = "";
            this.ten = "";
            this.soDT = "";
            this.email = "";
            this.avt = "";
            this.diaChi = null;
            this.ngaySinh = ngaySinh;
            this.listBaiDang = new QLBaiDang();
            this.doanhThu = 0;
            this.tinhTrang = 1;
            this.listFollower = new List<string>();
            this.listDonHang = null;
        }

        public List<SqlParameter> GetParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@maS", maSo),
                new SqlParameter("@ten", ten),
                new SqlParameter("@soDT", soDT),
                new SqlParameter("@email", email),
                new SqlParameter("@ngayTao", ngaySinh),
                new SqlParameter("@avt", avt),
                new SqlParameter("@maDC", diaChi.maDC),
                new SqlParameter("@tinhTrang", tinhTrang)

            };
        }
        public Shop(string soDT, string email)
        {
            this.maSo = "";
            this.ten = "";
            this.avt = "";
            this.soDT = soDT;
            this.email = email;
            this.diaChi = new DiaChi();
            this.ngaySinh = ngaySinh;
            this.listBaiDang = new QLBaiDang();
            this.doanhThu = 0;
            this.tinhTrang = 1;
            this.listFollower = new List<string>();
            this.listDonHang = null;
        }

        public Shop(string maSo)
        {
            this.maSo = maSo;
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
                    DAL_BaiDang.Instance.XoaBaiDang(bd);
                    return;
                }
            }
        }

        public void RemoveRange(List<string> maBDs)
        {
            Utils.Instance.Sort(listBaiDang.list, 0, listBaiDang.list.Count - 1, BaiDang.CompareMaBD, BaiDang.EqualMaBD);
            Utils.Instance.RemoveRange(listBaiDang.list, maBDs);
            foreach(string maBD in maBDs)
            {
                //HeThong.XoaBaiDang(maBD);
            }
        }

        public void RemoveAt(int index)
        {
            DAL_BaiDang.Instance.XoaBaiDang(listBaiDang.list[index]);
            listBaiDang.list.RemoveAt(index);
        }

        public void Update(BaiDang baiDang)
        {
            for (int i = 0; i < listBaiDang.list.Count; i++)
            {
                if (BaiDang.EqualMaBD(listBaiDang.list[i], baiDang))
                {
                    listBaiDang.list[i] = baiDang;
                    BLL_BaiDang.Instance.CapNhatBaiDang(baiDang);
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

        public double tinhSao()
        {
            double total = 0.0;
            int count = 0;

            foreach(BaiDang baiDang in listBaiDang.list)
            {
                foreach(DanhGia danhGia in baiDang.listDanhGia.list)
                {
                    total += danhGia.sao;
                }
                count += baiDang.listDanhGia.list.Count;
            }

            return total / count;
        }

        public SanPham searchSanPham(string maSP)
        {
            foreach (var baiDang in listBaiDang.list)
                foreach (var sanPham in baiDang.list)            
                    if (sanPham.maSP == maSP)
                        return sanPham;

            return null;
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
                donHang.capNhatTinhTrang(1);
                donHang.diDon();
                
                foreach(var sanPham in donHang.list)
                {
                    SanPham sp = searchSanPham(sanPham.maSP);
                    sp.soLuong -= sanPham.soLuong;
                    sp.luocBan += sanPham.soLuong;


                    BLL_SanPham.Instance.CapNhatSanPham(sp);
                }
            }
        }

        public void setNgayTaoShop() => ngaySinh = DateTime.Now;

        public override void Nhap(string ten, string email, string soDT, int _, DateTime ngaySinh)
        {
            this.ten = ten;
            this.email = email;
            this.soDT = soDT;
            this.ngaySinh = ngaySinh;
        }

        public override void Sua(string ten, string email, string soDT, int _, DateTime ngaySinh)
        {
            this.ten = ten;
            this.email = email;
            this.soDT = soDT;
        }

        public void capNhatTinhTrang(int tinhTrang) => this.tinhTrang = tinhTrang;

        public override void CapNhatDiaChi(DiaChi diaChiMoi)
        {
            this.diaChi = diaChiMoi;
        }

        public override void Follow(string maSo)
        {
            listFollower.Insert(0, maSo);
        }

        public override void UnFollow(string maSo)
        {
            listFollower.Remove(maSo);
        }
    }
}
