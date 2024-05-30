
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class DanhGia
    {
        public string maDG { get; set; }
        public string maKH { get; set; }
        public string maBD { get; set; }
        public string sanPhamDaMua { get; set; }
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
            sanPhamDaMua = "";
            doiTuong = "";
            thietKeBia = "";
            noiDung = "";
            sao = 5;
            luocThich = 0;
            ngayThem = DateTime.Now;
        }

        public List<SqlParameter> GetParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@maDG", maDG),
                new SqlParameter("@sanPhamDaMua", sanPhamDaMua),
                new SqlParameter("@doiTuong", doiTuong),
                new SqlParameter("@thietKeBia", thietKeBia),
                new SqlParameter("@noiDung", noiDung),
                new SqlParameter("@sao", sao),
                new SqlParameter("@luocThich", luocThich),
                new SqlParameter("@ngayThem", ngayThem)
            };
        }


        public static bool CompareSao(object o1, object o2) => ((DanhGia)o1).sao < ((DanhGia)o2).sao;
        public static bool EqualMaDH(object o1, object o2) => String.Equals(((DanhGia)o1).maDG, ((DanhGia)o2).maDG);

    }
}
