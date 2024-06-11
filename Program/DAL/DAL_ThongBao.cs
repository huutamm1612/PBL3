using Program.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.DAL
{
    internal class DAL_ThongBao
    {
        private static DAL_ThongBao _Instance;
        public static DAL_ThongBao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DAL_ThongBao();
                return _Instance;
            }
            private set { }
        }
        private DAL_ThongBao()
        {

        }

        public void XoaThongBaoFromMaTB(string maTB)
        {
            string query = "DELETE FROM ThongBao WHERE maTB = @maTB";
            SqlParameter param = new SqlParameter("@maTB", maTB);
            Database.Instance.ExecuteNonQuery(query, param);
        }

        public bool IsVanChuyenDaGui(string maDH)
        {
            string query = "SELECT * FROM ThongBao WHERE _From = 'BenVanChuyen' AND dinhKem = @maDH";
            SqlParameter param = new SqlParameter("@maDH", "DH" + maDH);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            if (table.Rows.Count == 0)
                return false;

            return true;
        }

        public QLThongBao LoadAllThongBaoFromMaKH(string maKH)
        {
            QLThongBao list = new QLThongBao();
            
            string query = "SELECT * FROM ThongBao WHERE _To = @maKH ORDER BY ngayGui DESC";
            SqlParameter param = new SqlParameter("@maKH", "KH" + maKH);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            foreach(DataRow row in table.Rows)
            {
                list.Add(LoadThongBao(row));
            }

            return list;
        }

        public QLThongBao LoadAllThongBaoToHeThong()
        {
            QLThongBao list = new QLThongBao();

            string query = "SELECT * FROM ThongBao WHERE _To = 'HeThong' ORDER BY ngayGui DESC";
            DataTable table = Database.Instance.ExecuteQuery(query);

            foreach (DataRow row in table.Rows)
            {
                list.Add(LoadThongBao(row));
            }

            return list;
        }

        public void ThemThongBao(ThongBao thongBao)
        {
            string query = "INSERT INTO ThongBao VALUES(@maTB, @from, @to, @dinhKem, @noiDung, @tinhTrang, @ngayGui)";
            Database.Instance.ExecuteNonQuery(query, thongBao.GetParameters().ToArray());
        }

        public QLThongBao LoadAllThongBaoFromMaS(string maS)
        {
            QLThongBao list = new QLThongBao();

            string query = "SELECT * FROM ThongBao WHERE _To = @maS ORDER BY ngayGui DESC";
            SqlParameter param = new SqlParameter("@maS", "S" + maS);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            foreach (DataRow row in table.Rows)
            {
                list.list.Add(LoadThongBao(row));
            }

            return list;
        }

        private ThongBao LoadThongBao(DataRow row)
        {
            string dinhKem = "";
            if (!row.IsNull("dinhKem"))
                dinhKem = row["dinhKem"].ToString();

            return new ThongBao
            {
                maTB = row["maTB"].ToString(),
                from = row["_From"].ToString(),
                to = row["_To"].ToString(),
                dinhKem = dinhKem,
                noiDung = row["noiDung"].ToString(),
                tinhTrang = Convert.ToInt32(row["tinhTrang"]),
                ngayGui = Convert.ToDateTime(row["ngayGui"])
            };
        }
    }
}
