using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.DAL
{
    internal class DAL_DonHang
    {
        private static DAL_DonHang _Instance;
        public static DAL_DonHang Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DAL_DonHang();
                return _Instance;
            }
            private set { }
        }
        private DAL_DonHang()
        {

        }

        public QLDonHang LoadAllDonHangFromMaKH(string maKH)
        {
            QLDonHang qlDonHang = new QLDonHang();

            string query = "SELECT * FROM DonHang DH JOIN DonHang_KhachHang DHKH ON DH.maDH = DHKH.maDH JOIN DonHang_Shop DHS ON DHS.maDH = DH.maDH WHERE DHKH.maKH = @maKH";
            SqlParameter param = new SqlParameter("@maKH", maKH);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            foreach(DataRow row in table.Rows)
            {
                qlDonHang.Add(LoadDonHang(row));
            }

            return qlDonHang;
        }

        public QLDonHang LoadAllDonHangFromMaS(string maS)
        {
            QLDonHang qlDonHang = new QLDonHang();

            string query = "SELECT * FROM DonHang DH JOIN DonHang_KhachHang DHKH ON DH.maDH = DHKH.maDH JOIN DonHang_Shop DHS ON DHS.maDH = DH.maDH WHERE DHS.maS = @maS";
            SqlParameter param = new SqlParameter("@maS", maS);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            foreach (DataRow row in table.Rows)
            {
                qlDonHang.Add(LoadDonHang(row));
            }

            return qlDonHang;
        }

        public void DatHang(DonHang donHang)
        {
            string query = "INSERT INTO DonHang VALUES(@maDH, @maDC, @tongTien, @tinhTrang, @ptThanhToan, @xu, @ngayDatHang, @ngayGiaoHang)";
            Database.Instance.ExecuteNonQuery(query, donHang.GetParameters().ToArray());
        }

        public void GiaoHang(string maDH, DateTime ngayGiaoHang)
        {
            string query = "UPDATE DonHang SET tinhTrang = 1, ngayGiaoHang = @ngayGiaoHang WHERE maDH = @maDH";
            SqlParameter param1 = new SqlParameter("@maDH", maDH);
            SqlParameter param2 = new SqlParameter("@ngayGiaoHang", ngayGiaoHang);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public void CapNhatTinhTrangDonHang(string maDH, int tinhTrang, DateTime ngayGiaoHang)
        {
            string query = "UPDATE DonHang SET tinhTrang = @tinhTrang, ngayGiaoHang = @ngayGiaoHang WHERE maDH = @maDH";
            SqlParameter param1 = new SqlParameter("@tinhTrang", tinhTrang);
            SqlParameter param2 = new SqlParameter("@ngayGiaoHang", ngayGiaoHang);
            SqlParameter param3 = new SqlParameter("@maDH", maDH);

            Database.Instance.ExecuteNonQuery(query, param1, param2, param3);
        }

        private DonHang LoadDonHang(DataRow row)
        {
            return new DonHang
            {
                maDH = row["maDH"].ToString(),
                maKH = row["maKH"].ToString(),
                maS = row["maS"].ToString(),
                diaChi = DAL_DiaChi.Instance.LoadDiaChiFromMaDC(row["maDC"].ToString()),
                list = DAL_SanPham.Instance.LoadAllSanPhamFromMaDH(row["maDH"].ToString()),
                tongTien = Convert.ToInt32(row["tongTien"]),
                tinhTrang = Convert.ToInt32(row["tinhTrang"]),
                ptThanhToan = Convert.ToInt32(row["ptThanhToan"]),
                xu = Convert.ToInt32(row["xu"]),
                ngayDatHang = Convert.ToDateTime(row["ngayDatHang"]),
                ngayGiaoHang = Convert.ToDateTime(row["ngayGiaoHang"])
            };
        }
    }
}
