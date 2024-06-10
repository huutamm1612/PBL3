using Program.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.DAL
{
    internal class DAL_User
    {
        private static DAL_User _Instance;
        public static DAL_User Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DAL_User();
                return _Instance;
            }
            private set { }
        }
        private DAL_User()
        {
            
        }

        public void WriteAccoutCache(User user) // Lưu lại user đăng nhập hiện tại khi đăng nhập
        {
            StreamWriter writer = new StreamWriter(@"Cache.txt");
            writer.WriteLine(user.taiKhoan);
            writer.WriteLine(user.matKhau);
            writer.Close();
        }

        public void ClearAccountCache() // xóa khi đăng xuất
        {
            StreamWriter writer = new StreamWriter(@"Cache.txt", false);
            writer.Close();
        }

        public List<string> ReadAccountCache()
        {
            FileInfo sourceFile = new FileInfo(@"Cache.txt");
            StreamReader reader = sourceFile.OpenText();

            string taiKhoan = reader.ReadLine();
            if (taiKhoan == null)
            {
                reader.Close();
                return null;
            }
            if (BLL_User.Instance.KiemTraTaiKhoan(taiKhoan))
                return null;

            List<string> list = new List<string>
            {
                taiKhoan,
                reader.ReadLine()
            };

            reader.Close();
            return list;
        }

        public void WriteCache(string query)
        {
            using (StreamWriter writer = new StreamWriter("DBCache.txt", true))
            {
                writer.WriteLine(query);
                writer.Close();
            }

        }

        public int LoadMaCHFromTaiKhoan(string taiKhoan)
        {
            string query = "SELECT * FROM UserAccount WHERE taiKhoan = @taiKhoan";
            SqlParameter param = new SqlParameter("@taiKhoan", taiKhoan);

            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return Convert.ToInt32(row["maCH"]);
        }

        public string LoadCauTraLoiFromTaiKhoan(string taiKhoan)
        {
            string query = "SELECT * FROM UserAccount WHERE taiKhoan = @taiKhoan";
            SqlParameter param = new SqlParameter("@taiKhoan", taiKhoan);

            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return row["cauTraLoi"].ToString();
        }

        public string LoadCauHoiFromMaCH(int maCH)
        {
            string query = "SELECT * FROM CauHoi WHERE maCH = @maCH";
            SqlParameter param = new SqlParameter("@maCH", maCH);

            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return row["cauHoi"].ToString();
        }

        public List<int> LoadAllMaCH()
        {
            List<int> list = new List<int>();

            string query = "SELECT * FROM CauHoi";
            DataTable table = Database.Instance.ExecuteQuery(query);

            foreach (DataRow row in table.Rows)
            {
                list.Add(Convert.ToInt32(row["maCH"]));
            }
            return list;
        }

        public void DoiMatKhau(string taiKhoan, string matKhau)
        {
            string query = "UPDATE UserAccount SET matKhau = @matKhau WHERE taiKhoan = @taiKhoan";
            SqlParameter param1 = new SqlParameter("@taiKhoan", taiKhoan);
            SqlParameter param2 = new SqlParameter("@matKhau", matKhau);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public void DoiMatKhau(User user)
        {
            string query = "UPDATE UserAccount SET matKhau = @matKhau WHERE taiKhoan = @taiKhoan";
            Database.Instance.ExecuteNonQuery(query, user.GetParameters().ToArray());
        }

        public void DangKy(string taiKhoan, string matKhau, int maCH, string cauTraLoi)
        {
            string query = $"INSERT INTO UserAccount VALUES(@taiKhoan, @matKhau, @maCH, @cauTraLoi, 0)";
            SqlParameter param1 = new SqlParameter("@taiKhoan", taiKhoan);
            SqlParameter param2 = new SqlParameter("@matKhau", matKhau);
            SqlParameter param3 = new SqlParameter("@maCH", maCH);
            SqlParameter param4 = new SqlParameter("@cauTraLoi", cauTraLoi);

            Database.Instance.ExecuteNonQuery(query, param1, param2, param3, param4);
        }

        public bool TaiKhoanDaTonTai(string taiKhoan)
        {
            string query = "SELECT * FROM UserAccount WHERE taiKhoan = @taiKhoan";
            SqlParameter param = new SqlParameter("@taiKhoan", taiKhoan);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            if (table.Rows.Count == 0)
                return false;

            return true;
        }

        public User DangNhap(string taiKhoan, string matKhau)
        {
            string query = "SELECT * FROM UserAccount WHERE taiKhoan = @taiKhoan AND matKhau = @matKhau";
            SqlParameter param1 = new SqlParameter("@taiKhoan", taiKhoan);
            SqlParameter param2 = new SqlParameter("@matKhau", matKhau);
            DataTable table = Database.Instance.ExecuteQuery(query, param1, param2);

            if (table.Rows.Count == 0)
                return null;

            return LoadUser(table.Rows[0]);
        }
        public User DangNhapAsShop(string taiKhoan, string matKhau)
        {
            string query = "SELECT UA.* From UserAccount UA JOIN KhachHang KH ON UA.taiKhoan = KH.taiKhoan JOIN KhachHang_Shop KHS ON KHS.maKH = KH.maKH WHERE UA.taiKhoan = @taiKhoan AND UA.matKhau = @matKhau";
            SqlParameter param1 = new SqlParameter("@taiKhoan", taiKhoan);
            SqlParameter param2 = new SqlParameter("@matKhau", matKhau);
            DataTable table = Database.Instance.ExecuteQuery(query, param1, param2);

            if (table.Rows.Count == 0)
                return null;

            return LoadUser(table.Rows[0]);
        }

        private User LoadUser(DataRow row)
        {
            return new User
            {
                taiKhoan = row["taiKhoan"].ToString(),
                matKhau = row["matKhau"].ToString()
            };
        }
    }
}
