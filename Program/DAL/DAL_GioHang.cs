using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;

namespace Program.DAL
{
    internal class DAL_GioHang
    {
        private static DAL_GioHang _Instance;
        public static DAL_GioHang Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DAL_GioHang();
                return _Instance;
            }
            private set { }
        }
        private DAL_GioHang()
        {

        }
        public void CapNhatSoLuong(string maKH, string maSP, int soLuong)
        {
            string query = "UPDATE GioHang SET soLuong = @soLuong, ngayThem = @ngayThem WHERE maKH = @maKH AND maSP = @maSP";
            SqlParameter param1 = new SqlParameter("@maKH", maKH);
            SqlParameter param2 = new SqlParameter("@maSP", maSP);
            SqlParameter param3 = new SqlParameter("@soLuong", soLuong);
            SqlParameter param4 = new SqlParameter("@ngayThem", DateTime.Now);

            Database.Instance.ExecuteNonQuery(query, param1, param2, param3, param4);
        }

        public void ThemSanPham(string maKH, string maSP, int soLuong)
        {
            string query = "INSERT INTO GioHang VALUES(@maKH, @maSP, @soLuong, @ngayThem)";
            SqlParameter param1 = new SqlParameter("@maKH", maKH);
            SqlParameter param2 = new SqlParameter("@maSP", maSP);
            SqlParameter param3 = new SqlParameter("@soLuong", soLuong);
            SqlParameter param4 = new SqlParameter("@ngayThem", DateTime.Now);

            Database.Instance.ExecuteNonQuery(query, param1, param2, param3, param4);
        }

        public void CapNhatSanPham(string maKH, string maSP, int soLuong)
        {
            string query = "UPDATE GioHang SET soLuong = @soLuong, ngayThem = @ngayThem WHERE maKH = @maKH AND maSP = @maSP";
            SqlParameter param1 = new SqlParameter("@maKH", maKH);
            SqlParameter param2 = new SqlParameter("@maSP", maSP);
            SqlParameter param3 = new SqlParameter("@soLuong", soLuong);
            SqlParameter param4 = new SqlParameter("@ngayThem", DateTime.Now);

            Database.Instance.ExecuteNonQuery(query, param1, param2, param3, param4);
        }

        public void XoaSanPham(string maKH, string maSP)
        {
            string query = "DELETE FROM GioHang WHERE maKH = @maKH AND maSP = @maSP";
            SqlParameter param1 = new SqlParameter("@maKH", maKH);
            SqlParameter param2 = new SqlParameter("@maSP", maSP);

            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public GioHang LoadGioHangFromMaKH(string maKH)
        {
            GioHang gioHang = new GioHang();

            string query = $"SELECT * FROM GioHang WHERE maKH = @maKH ORDER BY ngayThem";
            SqlParameter p = new SqlParameter("@maKH", maKH);
            DataTable table = Database.Instance.ExecuteQuery(query, p);

            foreach (DataRow row in table.Rows)
            {
                gioHang.Add(DAL_SanPham.Instance.LoadSanPhamFromMaSP(row["maSP"].ToString()));
                gioHang.list.First().soLuong = Convert.ToInt32(row["soLuong"].ToString());
                gioHang.list.First().ngayThem = Convert.ToDateTime(row["ngayThem"].ToString());
            }

            gioHang.maKH = maKH;

            return gioHang;
        }
    }
}
