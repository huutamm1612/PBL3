using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class BaiDang : QLSanPham
    {
        public string maBD { get; private set; }
        public string tieuDe { get; private set; }
        public string moTa { get; private set; }
        public int luocThich { get; private set; }
        public int giamGia { get; private set; }

        public BaiDang(string maBD)
        {
            this.maBD = maBD;
        }

        public BaiDang()
        {
            maBD = "";
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
            
            foreach(SanPham sanPham in list)
            {
                daBan += sanPham.luocBan;
            }

            return daBan;
        }

        public int giaMin()
        {
            int min = list[0].gia;

            foreach(SanPham sanPham in list)
            {
                if(sanPham.gia < min)
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
    }
}
