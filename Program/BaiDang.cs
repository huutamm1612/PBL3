using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class BaiDang : QLSanPham
    {
        public string maBD { get; set; }
        public string maS { get; set; }
        public string tieuDe { get; set; }
        public string moTa { get; set; }
        public int luocThich { get; set; }
        public int giamGia { get; set; }
        public string anhBia { get; set; }
        public QLDanhGia listDanhGia { get; set; }

        public BaiDang(string maBD)
        {
            this.maBD = maBD;
        }

        public BaiDang()
        {
            maBD = "";
            maS = "";
            list = new List<SanPham>();
            tieuDe = "";
            moTa = "";
            luocThich = 0;
            giamGia = 0;
            anhBia = "";
            listDanhGia = new QLDanhGia();
        }

        public override void Add(object item)
        {
            SanPham sanPham = item as SanPham;
            if (sanPham.maSP == "")
                sanPham.maSP = HeThong.MaMoi("maSP");
            sanPham.maBD = maBD;
            sanPham.maS = maS;
            base.Add(sanPham);
        }

        public int luocBan()
        {
            int daBan = 0;

            foreach (SanPham sanPham in list)
            {
                daBan += sanPham.luocBan;
            }

            return daBan;
        }

        public int giaMin()
        {
            int min = list[0].gia;

            foreach (SanPham sanPham in list)
            {
                if (sanPham.gia < min)
                    min = sanPham.gia;
                System.Console.WriteLine(sanPham.gia);
            }
            return min;
        }

        public int giaMax()
        {
            int max = list[0].gia;

            foreach (SanPham sanPham in list)
            {
                if (sanPham.gia > max)
                    max = sanPham.gia;
            }

            return max;
        }

        public double tinhSao()
        {
            return listDanhGia.tinhSao();
        }

        public int doanhThu()
        {
            int doanhThu = 0;
            foreach(SanPham sanPham in list)
            {
                doanhThu += sanPham.luocBan * sanPham.gia;
            }
            return doanhThu;
        }

        public int tongSoLuong()
        {
            int sum = 0;
            foreach(SanPham sanPham in list)
            {
                sum += sanPham.soLuong;
            }

            return sum;
        }

        public static bool CompareLuocBan(object o1 ,object o2) => ((BaiDang)o1).luocBan() < ((BaiDang)o2).luocBan();
        public static bool CompareLuocThich(object o1 ,object o2) => ((BaiDang)o1).luocThich < ((BaiDang)o2).luocThich;
        public static bool CompareMaBD(object o1, object o2) => String.Compare(((BaiDang)o1).maBD, ((BaiDang)o2).maBD) <= 0;

        public static bool CompareSao(object o1,object o2) => ((BaiDang)o1).tinhSao() < ((BaiDang)o2).tinhSao();
        public static bool ComareGiaMin(object o1,object o2) => ((BaiDang)o1).tinhSao() < ((BaiDang)o2).tinhSao();
        public static bool ComareGiaMax(object o1, object o2) => ((BaiDang)o1).giaMax() < ((BaiDang)o2).giaMax();
        public static bool EqualMaBD(object o1, object o2) => String.Equals(((BaiDang)o1).maBD, ((BaiDang)o2).maBD);

        public bool Equals(string obj) => maBD == obj;
        public override bool Equals(object obj) => ((BaiDang)obj).maBD == this.maBD;
    }
}
