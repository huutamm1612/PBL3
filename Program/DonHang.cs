using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    internal class DonHang : QLSanPham
    {
        public string maDH { get; set; }
        public string maKH { get; set; }
        public DiaChi diaChi { get; set; }
        public int tongTien { get; set; }
        public int tinhTrang { get; set; }
        public int xu { get; set; }
        public int ptThanhToan { get; set; }
        public DateTime ngayDatHang { get; set; }
        public DateTime ngayGiaoHang { get; set; }

        public DonHang()
        {
            list = null;
            maDH = "";
            maKH = "";
            tinhTrang = 0;
            ptThanhToan = 0;
            tongTien = 0;
            xu = 0;
            diaChi = new DiaChi();
            ngayDatHang = DateTime.Now;
            ngayGiaoHang = DateTime.Now;
        }

        public DonHang(QLSanPham sanPham)
        {
            list = sanPham.list;
        }

        public void capNhatTinhTrang(int tinhTrang) => this.tinhTrang = tinhTrang;

        public List<DonHang> phanRaDonHang(int n)
        {
            List<DonHang> listDonHang = new List<DonHang>();
            foreach(QLSanPham qlSP in phanRa())
            {
                listDonHang.Add(new DonHang
                {
                    list = qlSP.list,
                    maDH = HeThong.MaMoi("maDH"),
                    maKH = maDH,
                    tinhTrang = tinhTrang,
                    ptThanhToan = ptThanhToan,
                    tongTien = qlSP.tinhTongTien() + 30000,
                    diaChi = diaChi,
                    xu = xu / n,
                    ngayDatHang = ngayDatHang,
                });
            }

            return listDonHang;
        }

        public DonHang[] datHang()
        {
            List<DonHang> listDonHang = new List<DonHang>();

            int n = soLuongShop();

            if(n == 1)
            {
                maDH = HeThong.MaMoi("maDH");
                tongTien = tinhTongTien();
                listDonHang.Add(this);
            }
            else
            {
                listDonHang.AddRange(phanRaDonHang(n));
            }

            /*foreach(DonHang donHang in listDonHang) 
                HeThong.DatHang(donHang);*/

            return listDonHang.ToArray();
        }

        public void diDon()
        {
            Random random = new Random();

            int soNgay = random.Next(3, 7); 
            int soGio = random.Next(24); 
            int soPhut = random.Next(60); 
            int soGiay = random.Next(60);

            tinhTrang = 1;
            ngayGiaoHang = ngayDatHang.Add(new TimeSpan(soNgay, soGio, soPhut, soGiay));

            HeThong.DiDon(this);
        }

        public void huyDon()
        {
            ngayGiaoHang = DateTime.Now;
            HeThong.HuyDon(this);
        }

        public void nhanHang()
        {
            ngayGiaoHang = DateTime.Now;
            HeThong.NhanHang(this);
        }

        public DanhGia[] taoDanhGia()
        {
            QLDanhGia danhGia = new QLDanhGia();
            Utils.Sort(list, 0, list.Count - 1, SanPham.CompareMaBD, SanPham.EqualMaBD);

            string maBD = list[0].maBD;
            string text = "";

            foreach (SanPham sanPham in list)
            {
                if (maBD.Equals(sanPham.maBD))
                {
                    text += sanPham.ten + ", ";
                }
                else
                {
                    danhGia.Add(new DanhGia
                    {
                        maDG = HeThong.MaMoi("maDG"),
                        maKH = maKH,
                        maBD = maBD,
                        sanPhamDaMua = text,
                    }) ;
                }
            }

            danhGia.Add(new DanhGia
            {
                maDG = HeThong.MaMoi("maDG"),
                maKH = maKH,
                maBD = maBD,
                sanPhamDaMua = text,
            });


            return danhGia.list.ToArray();
        }

        /*public DanhGia[] taoDanhGia()
        {
            QLDanhGia qLDanhGia = new QLDanhGia();
            Utils.Sort(list, 0, list.Count - 1, SanPham.CompareMaS, SanPham.EqualMaS);

            List<string> tmp = new List<string>();
            string maBD = list[0].maBD;

            foreach(SanPham sanPham in list)
            {
                if(maBD == sanPham.maBD)
                {
                    tmp.Add(sanPham.maSP);
                }
                else
                {
                    qLDanhGia.Add(new DanhGia
                    {
                        maSP = tmp
                    });
                    tmp.Clear();
                }
            }

            qLDanhGia.Add(new DanhGia
            {
                maSP = tmp
            });

            return qLDanhGia.list.ToArray();
        }*/

        public static bool EqualMaDH(object o1, object o2) => String.Equals(((DonHang)o1).maDH, ((DonHang)o2).maDH);
        public override bool Equals(object obj) => ((DonHang)obj).maDH == this.maDH;
    }
}
