using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    internal class HeThong
    {
        private static readonly string strCon = @"Data Source=ASUS\HUUTAM;Initial Catalog=PBL3_Database;Integrated Security=True;";
        private static SqlConnection sqlCon;

        private static HeThong _System;
        public static HeThong System
        {
            get
            {
                if (_System == null)
                    _System = new HeThong();
                return _System;
            }
            private set { }
        }

        private HeThong()
        {
            sqlCon = null;
        }

        public static SqlCommand TruyVan(string noiDung)
        {
            if (sqlCon == null)
                sqlCon = new SqlConnection(strCon);
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();

            SqlCommand sqlCmd = new SqlCommand()
            {
                CommandType = CommandType.Text,
                CommandText = noiDung,

                Connection = sqlCon
            };
            return sqlCmd;
        }

        public static string MaMoi(string loaiMa)
        {
            string noiDung = $"SELECT {loaiMa} FROM maHienTai";

            SqlCommand sqlCmd = TruyVan(noiDung);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            reader.Read();
            string ma = reader.GetString(0);
            string maMoi = (int.Parse(ma) + 1).ToString("D10");
            reader.Close();

            noiDung = $"UPDATE maHienTai SET {loaiMa} = '{maMoi}'";
            sqlCmd = TruyVan(noiDung);
            sqlCmd.ExecuteNonQuery();

            return maMoi;
        }

        public static bool DangNhap(string taiKhoan, string matKhau, bool userState = true)
        {
            string table = userState ? "UserAccount" : "Admin";
            string noiDung = "SELECT * from " + table + " WHERE taiKhoan = '" + taiKhoan + "'";

            SqlCommand sqlCmd = TruyVan(noiDung);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            bool ketQua = false;
            if (reader.Read() && matKhau == reader.GetString(1))
                ketQua = true;

            reader.Close();
            return ketQua;
        }

        public static void DangKy(string taiKhoan, string matKhau, int maCH, string cauTraLoi)
        {
            string noiDung = $"INSERT INTO UserAccount VALUES('{taiKhoan}', '{matKhau}', '{maCH}', N'{cauTraLoi}', 0)";
            SqlCommand sqlCmd = TruyVan(noiDung);
            sqlCmd.ExecuteNonQuery();

            TaoKhachHangMoi(taiKhoan);
        }

        public static void TaoKhachHangMoi(string taiKhoan)
        {
            string noiDung = $"INSERT INTO KhachHang(maKH, taiKhoan) VALUES('{MaMoi("maKH")}', '{taiKhoan}')";
            SqlCommand sqlCmd = TruyVan(noiDung);
            sqlCmd.ExecuteNonQuery();
        }

        public static bool KiemTraMatKhau(TaiKhoan account, string matKhau)
        {
            return account.matKhau == matKhau;
        }


        public static bool KiemTraTaiKhoan(string taiKhoan) // trả về false nếu taiKhoan đã tồn tại
        {
            string noiDung = $"SELECT * FROM UserAccount WHERE taiKhoan = '{taiKhoan}'";
            SqlCommand sqlCmd = TruyVan(noiDung);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            bool result = true;
            if (reader.Read())
                result = false;
            reader.Close();
            return result;
        }

        public static void CapNhatMatKhau(TaiKhoan account, bool userState = true)
        {
            string table = userState ? "UserAccount" : "Admin";
            string noiDung = $"UPDATE {table} SET matKhau = '{account.matKhau}' WHERE taiKhoan = '{account.taiKhoan}'";

            SqlCommand sqlCmd = TruyVan(noiDung);
            sqlCmd.ExecuteNonQuery();
        }

        public static void CapNhatMatKhau(string taiKhoan, string matKhauMoi)
        {
            string noiDung = $"UPDATE UserAccount SET matKhau = '{matKhauMoi}' WHERE taiKhoan = '{taiKhoan}'";
            SqlCommand sqlCmd = TruyVan(noiDung);
            sqlCmd.ExecuteNonQuery();
        }

        private static void SetDiaChis(KhachHang khachHang)
        {
            string noiDung = $"SELECT * FROM DiaChi WHERE maSo = '{khachHang.maSo}' and diaChiKH = 1";
            SqlCommand sqlCmd = TruyVan(noiDung);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            List<DiaChi> diaChis = new List<DiaChi>();
            while (reader.Read())
            {
                if (reader.GetString(0) == khachHang.diaChi.maDC)
                    continue;

                diaChis.Add(new DiaChi(reader.GetString(0), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6), reader.GetString(7)));
            }
            reader.Close();
            khachHang.setDiaChis(diaChis);
        }

        public static KhachHang DangNhap(User user)
        {
            string noiDung = $"SELECT * FROM KhachHang WHERE taiKhoan = '{user.taiKhoan}'";
            SqlCommand sqlCmd = TruyVan(noiDung);
            SqlDataReader reader = sqlCmd.ExecuteReader();
            reader.Read();

            KhachHang khachHang;

            if (reader.IsDBNull(6))
                khachHang = new KhachHang(reader.GetString(0), reader.GetString(1));
            else if (reader.IsDBNull(5))
                khachHang = new KhachHang(reader.GetString(0), reader.GetString(2), reader.GetString(3), reader.GetString(4), null, reader.GetInt32(6), reader.GetDateTime(7), reader.GetString(1), reader.GetInt32(8), reader.GetInt32(9), reader.GetInt32(10));
            else
            {
                khachHang = new KhachHang(reader.GetString(0), reader.GetString(2), reader.GetString(3), reader.GetString(4), null, reader.GetInt32(6), reader.GetDateTime(7), reader.GetString(1), reader.GetInt32(8), reader.GetInt32(9), reader.GetInt32(10));

                string maDC = reader.GetString(5);
                reader.Close();

                khachHang.capNhatDiaChi(LoadDiaChi(maDC));
                SetDiaChis(khachHang);
            }
            if (!reader.IsClosed)
                reader.Close();
            return khachHang;
        }

        public static List<string> LoadCauHoi()
        {
            string noiDung = $"SELECT cauHoi FROM CauHoi";

            SqlCommand sqlCmd = TruyVan(noiDung);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            List<string> cauHoi = new List<string>();
            cauHoi.Add("Câu hỏi bảo mật");
            while (reader.Read())
            {
                cauHoi.Add(reader.GetString(0));
            }

            reader.Close();
            return cauHoi;
        }

        public static bool KiemTraCauHoi(string taiKhoan, int maCH, string cauTraLoi)
        {
            string noiDung = $"SELECT maCH, cauTraLoi FROM UserAccount WHERE taiKhoan = '{taiKhoan}'";
            SqlCommand sqlCmd = TruyVan(noiDung);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            reader.Read();

            bool result = false;
            if (reader.GetInt32(0) == maCH && reader.GetString(1) == cauTraLoi)
                result = true;

            reader.Close();
            return result;
        }

        public static void CapNhatThongTinCaNhan(Nguoi nguoi, string bang = "KhachHang")
        {
            string loaiMa = bang == "KhachHang" ? "maKH" : "maS";
            string noiDung = $"UPDATE {bang} SET ten = N'{nguoi.ten}', soDT = '{nguoi.soDT}', email = '{nguoi.email}', gioiTinh = '{nguoi.gioiTinh}', ngaySinh = '{nguoi.ngaySinh.Date.ToString("MM/dd/yyyy")}' WHERE {loaiMa} = '{nguoi.maSo}'";
            SqlCommand sqlCmd = TruyVan(noiDung);
            sqlCmd.ExecuteNonQuery();
        }

        public static List<string> LoadTinh_ThanhPho()
        {
            string noiDung = $"SELECT ten FROM Tinh_ThanhPho";
            SqlCommand sqlCmd = TruyVan(noiDung);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            List<string> list = new List<string>();
            list.Add("Tỉnh/Thành Phố");
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }
            reader.Close();

            return list;
        }

        public static List<string> LoadQuan_Huyen(int maT_TP)
        {
            string noiDung = $"SELECT ten FROM Quan_Huyen WHERE maT_TP = {maT_TP}";
            SqlCommand sqlCmd = TruyVan(noiDung);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            List<string> list = new List<string>();
            list.Add("Quận/Huyện");
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }
            reader.Close();

            return list;
        }

        public static List<string> LoadPhuong_Xa(int maT_TP, int indexQ_H)
        {
            string maQH = maT_TP.ToString() + indexQ_H.ToString("D2");
            string noiDung = $"SELECT ten FROM Phuong_Xa WHERE maQH = {maQH}";
            SqlCommand sqlCmd = TruyVan(noiDung);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            List<string> list = new List<string>();
            list.Add("Phường/Xã");
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }
            reader.Close();

            return list;
        }
        public static string MoTaDiaChi(int maPX)
        {
            string noiDung = $"select Phuong_Xa.ten, Quan_Huyen.ten, Tinh_ThanhPho.ten from Tinh_ThanhPho inner join Quan_Huyen on Tinh_ThanhPho.maT_TP = Quan_Huyen.maT_TP inner join Phuong_Xa on Phuong_Xa.maQH = Quan_Huyen.maQH where Phuong_Xa.maPX = {maPX}";
            SqlCommand sqlCmd = TruyVan(noiDung);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            reader.Read();
            string s = $"{reader.GetString(0)}, {reader.GetString(1)}, {reader.GetString(2)}";
            reader.Close();

            return s;
        }

        public static void ThemDiaChi(Nguoi obj, DiaChi diaChi, bool dcKhachHang = true)
        {
            int state = dcKhachHang == true ? 1 : 0;
            string noiDung = $"INSERT INTO DiaChi VALUES('{diaChi.maDC}', '{obj.maSo}', N'{diaChi.ten}', '{diaChi.soDT}',{diaChi.maT_TP}, {diaChi.maQH}, {diaChi.maPX}, N'{diaChi.diaChiCuThe}', {state})";
            SqlCommand sqlCmd = TruyVan(noiDung);
            sqlCmd.ExecuteNonQuery();
        }

        public static DiaChi LoadDiaChi(string maDC)
        {
            string noiDung = $"SELECT * FROM DiaChi WHERE maDC = '{maDC}'";
            SqlCommand sqlCmd = TruyVan(noiDung);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            reader.Read();
            DiaChi diaChi = new DiaChi(reader.GetString(0), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6), reader.GetString(7));
            reader.Close();

            return diaChi;
        }
        public static void CapNhatDiaChiMacDinh(KhachHang khachHang)
        {
            string noiDung = $"UPDATE KhachHang SET maDC = '{khachHang.diaChi.maDC}' WHERE maKH = '{khachHang.maSo}'";
            SqlCommand sqlCmd = TruyVan(noiDung);
            sqlCmd.ExecuteNonQuery();
        }

        public static void TaoShop()
        {
        }
    }
}
