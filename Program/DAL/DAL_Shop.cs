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
    internal class DAL_Shop
    {
        private static DAL_Shop _Instance;
        public static DAL_Shop Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DAL_Shop();
                return _Instance;
            }
            private set { }
        }
        private DAL_Shop()
        {

        }

        public string LoadMaSFromMaDH(string maDH)
        {
            string query = "SELECT maS FROM DonHang_Shop WHERE maDH = @maDH";
            SqlParameter param = new SqlParameter("@maDH", maDH);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return row["maS"].ToString();
        }

        public void GiaoHangThanhCong(string maS, int giaTriDH)
        {
            string query = "UPDATE Shop SET doanhThu = doanhThu + @giaTriDH WHERE maS = @maS";
            SqlParameter param1 = new SqlParameter("@giaTriDH", giaTriDH);
            SqlParameter param2 = new SqlParameter("@maS", maS);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public void DatHang(string maS, string maDH)
        {
            string query = "INSERT INTO DonHang_Shop VALUES(@maDH, @maS)";
            SqlParameter param1 = new SqlParameter("@maDH", maDH);
            SqlParameter param2 = new SqlParameter("@maS", maS);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public List<string> LoadListFollowFromMaS(string maS)
        {
            List<string> list = new List<string>();
            string query = $"SELECT maKH FROM Follow WHERE maS = {maS}";
            DataTable table = Database.Instance.ExecuteQuery(query);

            foreach (DataRow row in table.Rows)
            {
                list.Add(row["maKH"].ToString());
            }

            return list;
        }
        
        public string GetTenShopFromMaSP(string maSP)
        {
            string query = "SELECT S.ten FROM Shop S INNER JOIN BaiDang_Shop BDS ON BDS.maS = S.maS INNER JOIN SanPham_BaiDang SPBD ON SPBD.maBD = BDS.maBD WHERE maSP = @maSP";
            SqlParameter param = new SqlParameter("@maSP", maSP);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return row["ten"].ToString();
        }

        public string LoadMaSFromMaBD(string maBD)
        {
            string query = "SELECT maS FROM BaiDang_Shop WHERE maBD = @maBD";
            SqlParameter param = new SqlParameter("@maBD", maBD);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return row["maS"].ToString();
        }

        public string LoadTenShopFromMaBD(string maBD)
        {
            string query = "SELECT S.ten FROM Shop S JOIN BaiDang_Shop BDS ON BDS.maS = S.maS WHERE maBD = @maBD";
            SqlParameter param = new SqlParameter("@maBD", maBD);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return row["ten"].ToString();
        }

        public string LoadTenShopFromMaS(string maS)
        {
            string query = "SELECT S.ten FROM Shop S WHERE maS = @maS";
            SqlParameter param = new SqlParameter("@maS", maS);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return row["ten"].ToString();
        }

        public void TaoShop(Shop shop)
        {
            string query = "INSERT INTO Shop VALUES(@maS, @ten, @soDT, @email, @maDC, @ngayTao, @tinhTrang, 0, 0, null)";
            Database.Instance.ExecuteNonQuery(query, shop.GetParameters().ToArray());
        }

        public Shop LoadShopFromMaBD(string maBD)
        {
            string query = "SELECT * FROM Shop S JOIN BaiDang_Shop BDS ON BDS.maS = S.maS WHERE BDS.maBD = @maBD";
            SqlParameter param = new SqlParameter("@maBD", maBD);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return new Shop
            {
                maSo = row["maS"].ToString(),
                ten = row["ten"].ToString(),
                soDT = row["soDT"].ToString(),
                email = row["email"].ToString(),
                avt = row["avt"].ToString(),
                diaChi = DAL_DiaChi.Instance.LoadDiaChiFromMaS(row["maDC"].ToString()),
                tinhTrang = Convert.ToInt32(row["tinhTrang"].ToString()),
                ngaySinh = Convert.ToDateTime(row["ngayTao"].ToString()),
                listFollower = LoadListFollowFromMaS(row["maS"].ToString()),
                listBaiDang = DAL_BaiDang.Instance.LoadAllBaiDangFromMaS(row["maS"].ToString()),
                listDonHang = null,
            };
        }
        public Shop LoadShopFromMaDH(string maDH)
        {
            string query = "SELECT * FROM Shop S JOIN DonHang_Shop DHS ON DHS.maS = S.maS WHERE DHS.maDH = @maDH";
            SqlParameter param = new SqlParameter("@maDH", maDH);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return new Shop
            {
                maSo = row["maS"].ToString(),
                ten = row["ten"].ToString(),
                soDT = row["soDT"].ToString(),
                email = row["email"].ToString(),
                avt = row["avt"].ToString(),
                diaChi = DAL_DiaChi.Instance.LoadDiaChiFromMaS(row["maDC"].ToString()),
                tinhTrang = Convert.ToInt32(row["tinhTrang"].ToString()),
                ngaySinh = Convert.ToDateTime(row["ngayTao"].ToString()),
                listFollower = LoadListFollowFromMaS(row["maS"].ToString()),
                listBaiDang = DAL_BaiDang.Instance.LoadAllBaiDangFromMaS(row["maS"].ToString()),
                listDonHang = null,
            };
        }

        public void CapNhatThongTin(Shop shop)
        {
            string query = "UPDATE Shop SET ten = @ten, soDT = @soDT, email = @email, avt = @avt WHERE maS = @maS";
            Database.Instance.ExecuteNonQuery(query, shop.GetParameters().ToArray());
        }

        public Shop LoadShopFromTaiKhoan(string taiKhoan)
        {
            string query = $"SELECT * FROM Shop S JOIN KhachHang_Shop KHS ON KHS.maS = S.maS JOIN KhachHang KH ON KH.maKH = KHS.maKH WHERE KH.taiKhoan = '{taiKhoan}'";
            DataRow row = Database.Instance.ExecuteQuery(query).Rows[0];
            return new Shop
            {
                maSo = row["maS"].ToString(),
                ten = row["ten"].ToString(),
                soDT = row["soDT"].ToString(),
                email = row["email"].ToString(),
                avt = row["avt"].ToString(),
                diaChi = DAL_DiaChi.Instance.LoadDiaChiFromMaS(row["maS"].ToString()),
                tinhTrang = Convert.ToInt32(row["tinhTrang"].ToString()),
                ngaySinh = Convert.ToDateTime(row["ngayTao"].ToString()),
                doanhThu = Convert.ToInt32(row["doanhThu"].ToString()),
                listFollower = LoadListFollowFromMaS(row["maS"].ToString()),
                listBaiDang = DAL_BaiDang.Instance.LoadAllBaiDangFromMaS(row["maS"].ToString()),
                listDonHang = DAL_DonHang.Instance.LoadAllDonHangFromMaS(row["maS"].ToString()),
                listThongBao = DAL_ThongBao.Instance.LoadAllThongBaoFromMaS(row["maS"].ToString())
            };
        }
    }
}
