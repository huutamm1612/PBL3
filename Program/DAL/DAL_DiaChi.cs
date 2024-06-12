using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.DAL
{
    internal class DAL_DiaChi
    {
        private static DAL_DiaChi _Instance;
        public static DAL_DiaChi Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DAL_DiaChi();
                return _Instance;
            }
            private set { }
        }
        private DAL_DiaChi()
        {

        }

        public DiaChi LoadDiaChiFromMaDC(string maDC)
        {
            string query = $"SELECT * FROM DiaChi WHERE maDC = '{maDC}'";
            DataTable table = Database.Instance.ExecuteQuery(query);

            if (table.Rows.Count == 0)
                return null;

            return LoadDiaChi(table.Rows[0]);
        }

        public DiaChi LoadDiaChiFromMaS(string maS)
        {
            string query = $"SELECT * FROM DiaChi WHERE maSo = '{maS}' AND diaChiKH = 0";
            DataTable table = Database.Instance.ExecuteQuery(query);

            if (table.Rows.Count == 0)
                return null;

            return LoadDiaChi(table.Rows[0]);
        }

        public List<DiaChi> LoadAllDiaChiFromMaKH(string maKH)
        {
            List<DiaChi> list = new List<DiaChi>();

            string query = $"SELECT * FROM DiaChi WHERE maSo = {maKH} and diaChiKH = 1 AND maDC != (SELECT KhachHang.maDC FROM KhachHang WHERE maKH = maSo)";
            DataTable table = Database.Instance.ExecuteQuery(query);

            foreach (DataRow row in table.Rows)
            {
                list.Add(LoadDiaChi(row));
            }

            return list;
        }

        public string LoadTenT_TP(int maT_TP)
        {
            string query = "SELECT ten FROM Tinh_ThanhPho WHERE maT_TP = @maT_TP";
            SqlParameter param = new SqlParameter("@maT_TP", maT_TP);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return row["ten"].ToString();
        }

        public string LoadTenQH(int maQH)
        {
            string query = "SELECT ten FROM Quan_Huyen WHERE maQH = @maQH";
            SqlParameter param = new SqlParameter("@maQH", maQH);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return row["ten"].ToString();
        }

        public string LoadTenPX(int maPX)
        {
            string query = "SELECT ten FROM Phuong_Xa WHERE maPX = @maPX";
            SqlParameter param = new SqlParameter("@maPX", maPX);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return row["ten"].ToString();
        }

        public List<int> LoadAllMaT_TP()
        {
            List<int> list = new List<int>();
            string query = "SELECT * FROM Tinh_ThanhPho";

            DataTable table = Database.Instance.ExecuteQuery(query);
            foreach(DataRow row in table.Rows)
            {
                list.Add(Convert.ToInt32(row["maT_TP"]));
            }
            return list;
        }

        public List<int> LoadAllMaQHFromMaT_TP(int maT_TP)
        {
            List<int> list = new List<int>();
            string query = "SELECT * FROM Quan_Huyen WHERE maT_TP = @maT_TP";
            SqlParameter param = new SqlParameter("@maT_TP", maT_TP);

            DataTable table = Database.Instance.ExecuteQuery(query, param);
            foreach (DataRow row in table.Rows)
            {
                list.Add(Convert.ToInt32(row["maQH"]));
            }
            return list;
        }

        public List<int> LoadAllMaPXFromMaQH(int maQH)
        {
            List<int> list = new List<int>();
            string query = "SELECT * FROM Phuong_Xa WHERE maQH = @maQH";
            SqlParameter param = new SqlParameter("@maQH", maQH);

            DataTable table = Database.Instance.ExecuteQuery(query, param);
            foreach (DataRow row in table.Rows)
            {
                list.Add(Convert.ToInt32(row["maPX"]));
            }
            return list;
        }

        public void CapNhatDiaChiMacDinh(string maKH, string maDC)
        {
            string query = $"UPDATE KhachHang SET maDC = @maDC WHERE maKH = @maKH";
            SqlParameter param1 = new SqlParameter("@maDC", maDC);
            SqlParameter param2 = new SqlParameter("@maKH", maKH);

            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public void ThemDiaChi(DiaChi diaChi, string maSo, int isKhachHang)
        {
            string query = $"INSERT INTO DiaChi VALUES(@maDC, @maSo, @ten, @soDT, @maT_TP, @maQH, @maPX, @diaChiCuThe, @isDCKH)";
            SqlParameter param1 = new SqlParameter("@maSo", maSo);
            SqlParameter param2 = new SqlParameter("@isDCKH", isKhachHang);

            List<SqlParameter> listParam = diaChi.GetParameters();
            listParam.Add(param1);
            listParam.Add(param2);

            Database.Instance.ExecuteNonQuery(query, listParam.ToArray());
        }

        public bool KiemTraDCGH(string maDC)
        {
            string query = "select 1 from DiaChi DC JOIN DonHang DH ON DC.maDC = DH.maDH WHERE DH.maDC = @maDC";
            SqlParameter param = new SqlParameter("@maDC", maDC);

            DataTable table = Database.Instance.ExecuteQuery(query, param);

            return table.Rows.Count == 0;
        }

        public void CapNhatDiaChi(DiaChi diaChi)
        {
            string query = $"UPDATE DiaChi SET ten = @ten, soDT = @soDT, maT_TP = @maT_TP, maQH = @maQH, maPX = @maPX, diaChiCuThe = @diaChiCuThe WHERE maDC = maDC";
            Database.Instance.ExecuteNonQuery(query, diaChi.GetParameters().ToArray());
        }

        public void XoaDiaChi(string maDC)
        {
            string query = $"DELETE FROM DiaChi WHERE maDC = '{maDC}'";
            Database.Instance.ExecuteNonQuery(query);
        }

        public void AnDiaChi(string maDC)
        {
            string query = "UPDATE DiaChi SET maSo = '0000000000' WHERE maDC = @maDC";
            SqlParameter param = new SqlParameter("@maDC", maDC);
            Database.Instance.ExecuteNonQuery(query);
        }
        
        private DiaChi LoadDiaChi(DataRow row)
        {
            return new DiaChi
            {
                maDC = row["maDC"].ToString(),
                ten = row["ten"].ToString(),
                soDT = row["soDT"].ToString(),
                maT_TP = Convert.ToInt32(row["maT_TP"].ToString()),
                maQH = Convert.ToInt32(row["maQH"].ToString()),
                maPX = Convert.ToInt32(row["maPX"].ToString()),
                diaChiCuThe = row["diaChiCuThe"].ToString()
            };
        }
    }
}
