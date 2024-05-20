using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.DTO
{
    internal class ThongBao
    {
        public string maTB { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string dinhKem { get; set; }
        public string noiDung { get; set; }
        public int tinhTrang { get; set; }
        public DateTime ngayGui { get; set; }

        public ThongBao()
        {
            maTB = "";
            from = "";
            to = "";
            dinhKem = "";
            noiDung = "";
            tinhTrang = 0;
            ngayGui = DateTime.Now;
        }

        public List<SqlParameter> GetParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@maTB", maTB),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@dinhKem", dinhKem),
                new SqlParameter("@noiDung", noiDung),
                new SqlParameter("@tinhTrang", tinhTrang),
                new SqlParameter("@ngayGui", ngayGui)
            };
        }

        public static bool EqualMaTB(object o1, object o2) => String.Equals(((ThongBao)o1).maTB, ((ThongBao)o2).maTB);
    }
}
