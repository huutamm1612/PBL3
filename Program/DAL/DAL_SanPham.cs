using Program.DTO;
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
    internal class DAL_SanPham
    {
        private static DAL_SanPham _Instance;
        public static DAL_SanPham Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DAL_SanPham();
                return _Instance;
            }
            private set { }
        }
        private DAL_SanPham()
        {

        }

        public bool KiemTraViPham(string maSP)
        {
            string query = "SELECT * FROM SanPham SP JOIN SanPham_BaiDang SPBD ON SP.maSP = SPBD.maSP WHERE SP.maSP = @maSP AND SPBD.maBD NOT IN (SELECT maBD FROM BaiDangViPham)";
            SqlParameter param = new SqlParameter("@maSP", maSP);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            return table.Rows.Count == 0;
        }

        public List<string> LoadMaLoaiSPFromText(string text)
        {
            string query = $"SELECT * FROM LoaiSanPham WHERE tenLoaiSP LIKE N'%{text}%'";
            DataTable table = Database.Instance.ExecuteQuery(query);

            List<string> list = new List<string>();
            foreach(DataRow row in table.Rows)
            {
                list.Add(row["maLoaiSP"].ToString());
            }

            return list;
        }

        public List<string> LoadGoiYTimKiemTheLoai(string noiDung)
        {
            string query = $"SELECT DISTINCT TOP 10 tenLoaiSP FROM LoaiSanPham WHERE tenLoaiSP LIKE '{noiDung}%'";
            DataTable table = Database.Instance.ExecuteQuery(query);


            List<string> list = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(row["tenLoaiSP"].ToString());
            }

            return list;
        }

        public List<string> LoadGoiYTimKiemTacGia(string noiDung)
        {
            string query = $"SELECT DISTINCT TOP 10 tacGia from SanPham WHERE tacGia LIKE '{noiDung}%'";
            DataTable table = Database.Instance.ExecuteQuery(query);

            List<string> list = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(row["tacGia"].ToString());
            }

            return list;
        }

        public List<string> LoadGoiYTimKiemSanPham(string noiDung)
        {
            string query = $"SELECT DISTINCT TOP 10 ten from SanPham WHERE ten LIKE '{noiDung}%'";
            DataTable table = Database.Instance.ExecuteQuery(query);

            List<string> list = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(row["ten"].ToString());
            }

            return list;
        }
        
        public void GiaoHang(string maSP, int soLuong)
        {
            string query = "UPDATE SanPham SET soLuong = soLuong - @soLuong WHERE maSP = @maSP";
            SqlParameter param1 = new SqlParameter("@soLuong", soLuong);
            SqlParameter param2 = new SqlParameter("@maSP", maSP);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public void DatHang(string maSP, string maDH, int soLuong)
        {
            string query = "INSERT INTO DonHang_SanPham VALUES(@maDH, @maSP, @soLuong)";
            SqlParameter param1 = new SqlParameter("@maDH", maDH);
            SqlParameter param2 = new SqlParameter("@maSP", maSP);
            SqlParameter param3 = new SqlParameter("@soLuong", soLuong);
            Database.Instance.ExecuteNonQuery(query, param1, param2, param3);
        }

        public void NhanHang(string maSP)
        {
            string query = "UPDATE SanPham SET luocBan = luocBan + 1 WHERE maSP = @maSP";
            SqlParameter param = new SqlParameter("@maSP", maSP);
            Database.Instance.ExecuteNonQuery(query, param);
        }

        public int LoadSoLuongFromMaSP(string maSP)
        {
            string query = "SELECT soLuong FROM SanPham WHERE maSP = @maSP";
            SqlParameter param = new SqlParameter("@maSP", maSP);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return Convert.ToInt32(row["soLuong"]);
        }

        public SanPham LoadSanPhamFromMaSP(string maSP)
        {
            string query = "SELECT * FROM SanPham SP JOIN LoaiSanPham LSP ON LSP.maLoaiSP = SP.maLoaiSP JOIN SanPham_BaiDang SPBD ON SPBD.maSP = SP.maSP JOIN BaiDang_Shop BDS ON BDS.maBD = SPBD.maBD WHERE SP.maSP = '" + maSP + "'";
            DataTable table = Database.Instance.ExecuteQuery(query);
            return LoadSanPham(table.Rows[0]);
        }

        public void ThemVaoSanPhamDaXoa(string maSP, string maS)
        {
            string query = "INSERT INTO SanPhamDaXoa VALUES(@maSP, @maS)";
            SqlParameter param1 = new SqlParameter("@maSP", maSP);
            SqlParameter param2 = new SqlParameter("@maS", maS);

            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public List<LoaiSanPham> LoadAllLoaiSanPham()
        {
            List<LoaiSanPham> list = new List<LoaiSanPham> ();

            string query = "SELECT * FROM LoaiSanPham";
            DataTable table = Database.Instance.ExecuteQuery(query);

            foreach(DataRow row in table.Rows)
            {
                list.Add(new LoaiSanPham
                {
                    maLoaiSP = row["maLoaiSP"].ToString(),
                    tenLoaiSP = row["tenLoaiSP"].ToString()
                });
            }

            return list;
        }

        public LoaiSanPham LoadLoaiSPFromMaSP(string maSP)
        {
            string query = $"SELECT LSP.* FROM LoaiSanPham LSP JOIN SanPham SP ON LSP.maLoaiSP = SP.maLoaiSP WHERE maSP = '{maSP}'";
            DataRow row = Database.Instance.ExecuteQuery(query).Rows[0];

            return new LoaiSanPham
            {
                maLoaiSP = row["maLoaiSP"].ToString(),
                tenLoaiSP = row["tenLoaiSP"].ToString()
            };
        }

        public List<SanPham> LoadAllSanPhamFromMaDH(string maDH)
        {
            List<SanPham> list = new List<SanPham>();

            string query = "SELECT * FROM DonHang_SanPham WHERE maDH = @maDH";
            SqlParameter param = new SqlParameter("@maDH", maDH);

            DataTable table = Database.Instance.ExecuteQuery(query, param);

            foreach(DataRow row in table.Rows)
            {
                list.Add(LoadSanPhamFromMaSP(row["maSP"].ToString()));
                list.Last().soLuong = Convert.ToInt32(row["soLuong"]);
            }

            return list;
        }

        public List<SanPham> LoadAllSanPhamFromMaBD(string maBD)
        {
            List<SanPham> list = new List<SanPham>();

            string query = $"SELECT * FROM SanPham SP JOIN LoaiSanPham LSP ON LSP.maLoaiSP = SP.maLoaiSP JOIN SanPham_BaiDang SPBD ON SPBD.maSP = SP.maSP JOIN BaiDang_Shop BDS ON BDS.maBD = SPBD.maBD WHERE SPBD.maBD = '{maBD}'";
            DataTable table = Database.Instance.ExecuteQuery(query);

            foreach(DataRow row in table.Rows)
            {
                list.Add(LoadSanPham(row));
            }

            return list;
        }

        public void ThemSanPham(SanPham sanPham)
        {
            string query = "INSERT INTO SanPham VALUES(@maSP, @maLoaiSP, @ten, @gia, @soLuong, @tacGia, @dichGia, @ngonNgu, @soTrang, @namXuatBan, @nhaXuatBan, @loaiBia, @moTa, 0, @anh)";
            Database.Instance.ExecuteNonQuery(query, sanPham.GetParameters().ToArray());

            query = $"INSERT INTO SanPham_BaiDang VALUES(@maSP, @maBD)";
            Database.Instance.ExecuteNonQuery(query, sanPham.GetParameters().ToArray());
        }


        public void CapNhatSanPham(SanPham sanPham)
        {
            string query = $"UPDATE SanPham SET maLoaiSP = @maLoaiSP, ten = @ten, gia = @gia, soLuong = @soLuong, tacGia = @tacGia, ngonNgu = @ngonNgu, soTrang = @soTrang, namXuatBan = @namXuatBan, nhaXuatBan = @nhaXuatBan, loaiBia = @loaiBia, moTa = @moTa WHERE maSP = @maSP";
            Database.Instance.ExecuteNonQuery(query, sanPham.GetParameters().ToArray());
        }

        private SanPham LoadSanPham(DataRow row)
        {
            return new SanPham
            {
                maSP = row["maSP"].ToString(),
                loaiSP = new LoaiSanPham { maLoaiSP = row["maLoaiSP"].ToString(), tenLoaiSP = row["tenLoaiSP"].ToString() },
                maS = row["maS"].ToString(),
                maBD = row["maBD"].ToString(),
                ten = row["ten"].ToString(),
                gia = Convert.ToInt32(row["gia"]),
                soLuong = Convert.ToInt32(row["soLuong"]),
                soTrang = Convert.ToInt32(row["soTrang"]),
                luocBan = Convert.ToInt32(row["luocBan"]),
                tacGia = row["tacGia"].ToString(),
                dichGia = row["dichGia"].ToString(),
                ngonNgu = row["ngonNgu"].ToString(),
                namXuatBan = Convert.ToInt32(row["namXuatBan"]),
                nhaXuatBan = row["nhaXuatBan"].ToString(),
                loaiBia = row["loaiBia"].ToString(),
                moTa = row["moTa"].ToString(),
                anh = row["anh"].ToString()
            };
        }
    }
}
