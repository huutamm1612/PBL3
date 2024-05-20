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
    internal class DAL_KhachHang
    {
        private static DAL_KhachHang _Instance;
        public static DAL_KhachHang Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DAL_KhachHang();
                return _Instance;
            }
            private set { }
        }
        private DAL_KhachHang()
        {

        }

        public void NhanHang(string maKH, int giaTriDH)
        {
            string query = "UPDATE KhachHang SET chiTieu = chiTieu + @giaTriDH WHERE maKH = @maKH";
            SqlParameter param1 = new SqlParameter("@giaTriDH", giaTriDH);
            SqlParameter param2 = new SqlParameter("@maKH", maKH);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public void DatHang(string maKH, string maDH)
        {
            string query = "INSERT INTO DonHang_KhachHang VALUES(@maDH, @maKH)";
            SqlParameter param1 = new SqlParameter("@maDH", maDH);
            SqlParameter param2 = new SqlParameter("@maKH", maKH);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public string LoadMaKHFromTaiKhoan(string taiKhoan)
        {
            string query = "SELECT maKH FROM KhachHang WHERE taiKhoan = @taiKhoan";
            SqlParameter param = new SqlParameter("@taiKhoan", taiKhoan);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return row["taiKhoan"].ToString();
        }

        public void DungXu(string maKH)
        {
            string query = "UPDATE KhachHang SET xu = 0 WHERE maKH = @maKH";
            SqlParameter param = new SqlParameter("@maKH", maKH);
            Database.Instance.ExecuteNonQuery(query, param);   
        }

        public void DangKyTaoShop(string maKH, string maS)
        {
            string query = "INSERT INTO KhachHang_Shop VALUES(@maKH, @maS)";
            SqlParameter param1 = new SqlParameter("@maKH", maKH);
            SqlParameter param2 = new SqlParameter("@maS", maS);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public bool KiemTraKhachHang_Shop(string taiKhoan)
        {
            string query = "SELECT * FROM KhachHang_Shop KHS JOIN KhachHang KH ON KH.maKH = KHS.maKH WHERE KH.taiKhoan = @taiKhoan";
            SqlParameter param = new SqlParameter("@taiKhoan", taiKhoan);
            DataTable table = Database.Instance.ExecuteQuery(query, param);
            if (table.Rows.Count == 0)
                return false;

            return true;
        }

        public void TaoKhachHangMoiBangTaiKhoan(string taiKhoan)
        {
            string query = "INSERT INTO KhachHang(maKH, taiKhoan) VALUES(@maKH, @taiKhoan)";
            SqlParameter param1 = new SqlParameter("@maKH", Database.Instance.MaMoi("maKH"));
            SqlParameter param2 = new SqlParameter("@taiKhoan", taiKhoan);

            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public KhachHang LoadKhachHangFromTaiKhoan(string taiKhoan)
        {
            string query = $"SELECT * FROM KhachHang WHERE taiKhoan = '{taiKhoan}'";
            DataTable table = Database.Instance.ExecuteQuery(query);

            return LoadKhachHang(table.Rows[0]);
        }

        public List<string> LoadListFollowFromMaKH(string maKH)
        {
            List<string> list = new List<string>();
            string query = $"SELECT maS FROM Follow WHERE maKH = {maKH}";
            DataTable table = Database.Instance.ExecuteQuery(query);

            foreach (DataRow row in table.Rows)
            {
                list.Add(row["maS"].ToString());
            }

            return list;
        }

        public void ThemFollow(string maKH, string maS)
        {
            string query = "INSERT INTO Follow VALUES(@maKH, @maS)";
            SqlParameter param1 = new SqlParameter("@maKH", maKH);
            SqlParameter param2 = new SqlParameter("@maS", maS);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public void XoaFollow(string maKH, string maS)
        {
            string query = "DELETE FROM Follow WHERE maKH = @maKH AND maS = @maS";
            SqlParameter param1 = new SqlParameter("@maKH", maKH);
            SqlParameter param2 = new SqlParameter("@maS", maS);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public List<string> LoadListDaThichFromMaKH(string maKH)
        {
            List<string> list = new List<string>();
            string query = $"SELECT maBD FROM Thich WHERE maKH = @maKH";
            SqlParameter param = new SqlParameter("@maKH", maKH);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            foreach (DataRow row in table.Rows)
            {
                list.Add(row["maBD"].ToString());
            }

            return list;
        }

        public void ThemThich(string maKH, string maBD)
        {
            string query = "INSERT INTO Thich VALUES(@maKH, @maBD)";
            SqlParameter param1 = new SqlParameter("@maKH", maKH);
            SqlParameter param2 = new SqlParameter("@maBD", maBD);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public void XoaThich(string maKH, string maBD)
        {
            string query = "DELETE FROM Thich WHERE maKH = @maKH AND maBD = @maBD";
            SqlParameter param1 = new SqlParameter("@maKH", maKH);
            SqlParameter param2 = new SqlParameter("@maBD", maBD);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }


        public List<string> LoadListDaXemFromMaKH(string maKH)
        {
            List<string> list = new List<string>();
            string query = $"SELECT maBD FROM Thich WHERE maKH = @maKH";
            SqlParameter param = new SqlParameter("@maKH", maKH);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            foreach (DataRow row in table.Rows)
            {
                list.Add(row["maBD"].ToString());
            }
            return list;
        }

        public void ThemDaXem(string maKH, string maBD)
        {
            string query = "INSERT INTO DaXemGanDay VALUES(@maKH, @maBD)";
            SqlParameter param1 = new SqlParameter("@maKH", maKH);
            SqlParameter param2 = new SqlParameter("@maBD", maBD);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public void XoaDaXem(string maKH, string maBD)
        {
            string query = "DELETE FROM DaXemGanDay WHERE maKH = @maKH AND maBD = @maBD";
            SqlParameter param1 = new SqlParameter("@maKH", maKH);
            SqlParameter param2 = new SqlParameter("@maBD", maBD);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public void CapNhatKhachHang(KhachHang khachHang)
        {
            string query = "UPDATE KhachHang Set ten = @ten, soDT = @soDT, email = @email, gioiTinh = @gioiTinh, ngaySinh = @ngaySinh, avt = @avt WHERE maKH = @maKH";
            Database.Instance.ExecuteNonQuery(query, khachHang.GetParameters().ToArray());
        }

        private KhachHang LoadKhachHang(DataRow row)
        {
            DiaChi diaChi = null;
            int gioiTinh = 2;
            string avt = "";
            DateTime ngaySinh = new DateTime();

            if (!row.IsNull("maDC"))
                diaChi = DAL_DiaChi.Instance.LoadDiaChiFromMaDC(row["maDC"].ToString());
            if (!row.IsNull("gioiTinh"))
                gioiTinh = Convert.ToInt32(row["gioiTinh"]);
            if (!row.IsNull("ngaySinh"))
                ngaySinh = Convert.ToDateTime(row["ngaySinh"]);
            if (!row.IsNull("avt"))
                avt = row["avt"].ToString();

            return new KhachHang
            {
                maSo = row["maKH"].ToString(),
                taiKhoan = row["taiKhoan"].ToString(),
                ten = row["ten"].ToString(),
                soDT = row["soDT"].ToString(),
                email = row["email"].ToString(),
                diaChi = diaChi,
                gioiTinh = gioiTinh,
                ngaySinh = ngaySinh,
                xu = Convert.ToInt32(row["xu"]),
                chiTieu = Convert.ToInt32(row["chiTieu"]),
                avt = avt,
                gioHang = DAL_GioHang.Instance.LoadGioHangFromMaKH(row["maKH"].ToString()),
                listDiaChi = DAL_DiaChi.Instance.LoadAllDiaChiFromMaKH(row["maKH"].ToString()),
                listDonHang = DAL_DonHang.Instance.LoadAllDonHangFromMaKH(row["maKH"].ToString()),
                listDanhGia = DAL_DanhGia.Instance.LoadAllDanhGiaFromMaKH(row["maKH"].ToString()),
                listFollow = LoadListFollowFromMaKH(row["maKH"].ToString()),
                listThich = LoadListDaThichFromMaKH(row["maKH"].ToString()),
                listDaXem = LoadListDaXemFromMaKH(row["maKH"].ToString())
            };
        }
    }
}
