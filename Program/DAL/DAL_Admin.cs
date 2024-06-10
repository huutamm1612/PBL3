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
    internal class DAL_Admin
    {
        private static DAL_Admin _Instance;
        public static DAL_Admin Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DAL_Admin();
                return _Instance;
            }
            private set { }
        }
        private DAL_Admin()
        {

        }

        public void DoiMatKhau(Admin admin)
        {
            string query = "UPDATE AdminAccount SET matKhau = @matKhau WHERE taiKhoan = @taiKhoan";
            Database.Instance.ExecuteNonQuery(query, admin.GetParameters().ToArray());
        }

        public Admin DangNhap(string taiKhoan, string matKhau)
        {
            string query = "SELECT * FROM AdminAccount WHERE taiKhoan = @taiKhoan AND matKhau = @matKhau";
            SqlParameter param1 = new SqlParameter("@taiKhoan", taiKhoan);
            SqlParameter param2 = new SqlParameter("@matKhau", matKhau);
            DataTable table = Database.Instance.ExecuteQuery(query, param1, param2);

            if (table.Rows.Count == 0)
                return null;

            return LoadAdmin(table.Rows[0]);
        }

        public void XacNhanBDViPham(string maTB, string maBD)
        {
            string query = "UPDATE ThongBao SET _From = 'HeThong', _To = @maS, ngayGui = @ngayGui WHERE maTB = @maTB";
            SqlParameter param1 = new SqlParameter("@maS", "S" + DAL_Shop.Instance.LoadMaSFromMaBD(maBD));
            SqlParameter param2 = new SqlParameter("@ngayGui", DateTime.Now);
            SqlParameter param3 = new SqlParameter("@maTB", maTB);
        
            Database.Instance.ExecuteNonQuery(query, param1, param2, param3);
        }
        public void XacNhanDGViPham(string maTB, string maDG)
        {
            string query = "UPDATE ThongBao SET _From = 'HeThong', _To = @maKH, ngayGui = @ngayGui WHERE maTB = @maTB";
            SqlParameter param1 = new SqlParameter("@maKH", "KH" + DAL_KhachHang.Instance.LoadMaKHFromMaDG(maDG));
            SqlParameter param2 = new SqlParameter("@ngayGui", DateTime.Now);
            SqlParameter param3 = new SqlParameter("@maTB", maTB);

            Database.Instance.ExecuteNonQuery(query, param1, param2, param3);
        }

        private Admin LoadAdmin(DataRow row)
        {
            return new Admin
            {
                taiKhoan = row["taiKhoan"].ToString(),
                matKhau = row["matKhau"].ToString()
            };
        }
    }
}
