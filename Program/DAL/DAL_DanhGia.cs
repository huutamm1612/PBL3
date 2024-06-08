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
    internal class DAL_DanhGia
    {
        private static DAL_DanhGia _Instance;
        public static DAL_DanhGia Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DAL_DanhGia();
                return _Instance;
            }
            private set { }
        }
        private DAL_DanhGia()
        {

        }

        public void SuaDanhGia(DanhGia danhGia)
        {
            string query = "UPDATE DanhGia SET doiTuong = @doiTuong, thietKeBia = @thietKeBia, noiDung = @noiDung, sao = @sao, ngayThem = @ngayThem WHERE maDG = @maDG";
            Database.Instance.ExecuteNonQuery(query, danhGia.GetParameters().ToArray());
        }

        public List<string> LoadAllLyDoBaoCaoDanhGia()
        {
            string query = "SELECT * FROM LyDo WHERE loaiLyDo = 3";

            List<string> list = new List<string>();

            DataTable table = Database.Instance.ExecuteQuery(query);
            foreach (DataRow row in table.Rows)
            {
                list.Add(row["noiDung"].ToString());
            }

            return list;
        }

        public QLDanhGia LoadAllDanhGiaFromMaKH(string maKH)
        {
            QLDanhGia qLDanhGia = new QLDanhGia();

            string query = "SELECT * FROM DanhGia DG JOIN DanhGia_KhachHang DGKH ON DG.maDG = DGKH.maDG JOIN DanhGia_BaiDang DGBD ON DGBD.maDG = DG.maDG WHERE DGKH.maKH = @maKH ORDER BY ngayThem DESC";
            SqlParameter param = new SqlParameter("@maKH", maKH);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            foreach(DataRow row in table.Rows)
            {
                qLDanhGia.Add(LoadDanhGia(row));
            }

            return qLDanhGia;
        }

        public QLDanhGia LoadAllDanhGiaFromMaBD(string maBD)
        {
            QLDanhGia qLDanhGia = new QLDanhGia();

            string query = "SELECT * FROM DanhGia DG JOIN DanhGia_KhachHang DGKH ON DG.maDG = DGKH.maDG JOIN DanhGia_BaiDang DGBD ON DGBD.maDG = DG.maDG WHERE DGBD.maBD = @maBD ORDER BY ngayThem DESC";
            SqlParameter param = new SqlParameter("@maBD", maBD);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            foreach (DataRow row in table.Rows)
            {
                qLDanhGia.Add(LoadDanhGia(row));
            }

            return qLDanhGia;
        }

        public void ThemDanhGia(DanhGia danhGia)
        {
            string query = "INSERT INTO DanhGia VALUES(@maDG, @sanPhamDaMua, @doiTuong, @thietKeBia, @noiDung, @sao, @luocThich, @ngayThem)";
            Database.Instance.ExecuteNonQuery(query, danhGia.GetParameters().ToArray());

            query = "INSERT INTO DanhGia_BaiDang VALUES(@maDG, @maBD)";
            SqlParameter param1 = new SqlParameter("@maDG", danhGia.maDG);
            SqlParameter param2 = new SqlParameter("@maBD", danhGia.maBD);
            Database.Instance.ExecuteNonQuery(query, param1, param2);

            query = "INSERT INTO DanhGia_KhachHang VALUES(@maKH, @maDG)";
            SqlParameter param3 = new SqlParameter("@maKH", danhGia.maKH);
            SqlParameter param4 = new SqlParameter("@maDG", danhGia.maDG);
            Database.Instance.ExecuteNonQuery(query, param3, param4);
        }

        public void CapNhatDanhGia(DanhGia danhGia)
        {
            string query = "UPDATE DanhGia SET doiTuong = @doiTuong, thietKeBia = @thietKeBia, noiDung = @noiDung, luocThich = @luocThich, ngayThem = @ngayThem WHERE maDG = @maDG";
            Database.Instance.ExecuteNonQuery(query, danhGia.GetParameters().ToArray());
        }

        public void XoaDanhGia(string maDG)
        {
            string query = "DELETE FROM DanhGia_KhachHang WHERE maDG = @maDG; DELETE FROM DanhGia_BaiDang WHERE maDG = @maDG; DELETE FROM DanhGia WHERE maDG = @maDG";
            SqlParameter param = new SqlParameter("@maDG", maDG);
            Database.Instance.ExecuteNonQuery(query, param);
        }

        private DanhGia LoadDanhGia(DataRow row)
        {
            return new DanhGia
            {
                maDG = row["maDG"].ToString(),
                maBD = row["maBD"].ToString(),
                maKH = row["maKH"].ToString(),
                sanPhamDaMua = row["sanPhamDaMua"].ToString(),
                doiTuong = row["doiTuong"].ToString(),
                thietKeBia = row["thietKeBia"].ToString(),
                noiDung = row["noiDung"].ToString(),
                sao = Convert.ToInt32(row["sao"]),
                luocThich = Convert.ToInt32(row["luocThich"]),
                ngayThem = Convert.ToDateTime(row["ngayThem"]),
            };
        }
    }
}
