using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public BaiDang(string maBD, List<SanPham> list, string tieuDe, string moTa, int luocThich, int giamGia)
        {
            this.maBD = maBD;
            this.list = list;
            this.tieuDe = tieuDe;
            this.moTa = moTa;
            this.luocThich = luocThich;
            this.giamGia = giamGia;
        }

        public BaiDang(BaiDang baiDang)
        {
            maBD = baiDang.maBD;
            list = baiDang.list;
            tieuDe = baiDang.tieuDe;
            moTa = baiDang.moTa;
            luocThich = baiDang.luocThich;
            giamGia = baiDang.giamGia;
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
            return 0.0;
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
