
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class DanhGia
    {
        public string maDG { get; set; }
        public string maKH { get; set; }
        public string maBD { get; set; }
        public List<string> maSP { get; set; }
        public string doiTuong { get; set; }
        public string thietKeBia { get; set; }
        public string noiDung { get; set; }
        public int sao { get; set; } 
        public int luocThich { get; set; }
        public DateTime ngayThem { get; set; }

        public DanhGia()
        {
            maDG = "";
            maKH = "";
            maBD = "";
            maSP = new List<string>();
            doiTuong = "";
            thietKeBia = "";
            noiDung = "";
            sao = 0;
            ngayThem = DateTime.Now;
        }

        public bool daDanhGia()
        {
            return sao != 0;
        }

        public static bool CompareSao(object o1, object o2) => ((DanhGia)o1).sao < ((DanhGia)o2).sao;
        public static bool EqualMaDH(object o1, object o2) => String.Equals(((DanhGia)o1).maDG, ((DanhGia)o2).maDG);

    }
}
