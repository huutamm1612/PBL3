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
using System.IO;
using System.Security.Cryptography;

namespace Program
{
    internal class HeThong
    {
        private static readonly string strCon = @"Data Source=ASUS\HUUTAM;Initial Catalog=PBL3_Database;Integrated Security=True;MultipleActiveResultSets=true;";
        private static SqlConnection sqlCon;

        private static SqlCommand Query(string noiDung)
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

        public static void ExecuteNonQuery(string query)
        {
            Query(query).ExecuteNonQuery();
        }

        public static SqlDataReader ExecuteQuery(string query)
        {
            return Query(query).ExecuteReader();
        }

        public static void WriteAccoutCache(User user) // Lưu lại user đăng nhập hiện tại khi đăng nhập
        {
            StreamWriter writer = new StreamWriter(@"Cache.txt");
            writer.WriteLine(user.taiKhoan);
            writer.WriteLine(user.matKhau);
            writer.Close();
        }

        public static void ClearAccountCache() // xóa khi đăng xuất
        {
            StreamWriter writer = new StreamWriter(@"Cache.txt", false);
            writer.Close();
        }

        public static List<string> ReadAccountCache()
        {
            FileInfo sourceFile = new FileInfo(@"Cache.txt");
            StreamReader reader = sourceFile.OpenText();

            string taiKhoan = reader.ReadLine();
            if (taiKhoan == null)
            {
                reader.Close();    
                return null;
            }
            if (KiemTraTaiKhoan(taiKhoan))
                return null;

            List<string> list = new List<string>
            {
                taiKhoan,
                reader.ReadLine()
            };

            reader.Close();
            return list;
        }

        public static string MaMoi(string loaiMa)
        {
            string query = $"SELECT {loaiMa} FROM maHienTai";

            SqlDataReader reader = ExecuteQuery(query);

            reader.Read();
            string ma = reader.GetString(0);
            string maMoi = (int.Parse(ma) + 1).ToString("D10");
            reader.Close();

            query = $"UPDATE maHienTai SET {loaiMa} = '{maMoi}'";
            ExecuteNonQuery(query);

            return maMoi;
        }

        public static bool DangNhap(string taiKhoan, string matKhau, bool userState = true)
        {
            string table = userState ? "UserAccount" : "Admin"; 
            string query = "SELECT * from " + table + " WHERE taiKhoan = '" + taiKhoan + "'";

            SqlDataReader reader = ExecuteQuery(query);

            bool ketQua = false;
            if (reader.Read() && matKhau == reader.GetString(1))
                ketQua = true;

            reader.Close();
            return ketQua;
        }

        public static void DangKy(string taiKhoan, string matKhau, int maCH, string cauTraLoi)
        {
            string query = $"INSERT INTO UserAccount VALUES('{taiKhoan}', '{matKhau}', '{maCH}', N'{cauTraLoi}', 0)";
            ExecuteNonQuery(query);
            TaoKhachHangMoi(taiKhoan);
        }

        public static void TaoKhachHangMoi(string taiKhoan)
        {
            string query = $"INSERT INTO KhachHang(maKH, taiKhoan) VALUES('{MaMoi("maKH")}', '{taiKhoan}')";
            ExecuteNonQuery(query);
        }

        public static bool KiemTraTaiKhoan(string taiKhoan) // trả về false nếu taiKhoan đã tồn tại
        {
            string query = $"SELECT * FROM UserAccount WHERE taiKhoan = '{taiKhoan}'";
            SqlDataReader reader =  ExecuteQuery(query);

            bool result = true;
            if (reader.Read())
                result = false;
            reader.Close();
            return result;
        }

        public static bool KiemTraTaoShop(string taiKhoan) // trả về true nếu khách hàng đã tạo shop
        {
            //string query = $"SELECT KhachHang_Shop.maKH FROM KhachHang_Shop INNER JOIN KhachHang ON KhachHang_Shop.maKH = KhachHang.maKH INNER JOIN UserAccount ON KhachHang.taiKhoan = UserAccount.taiKhoan WHERE UserAccount.taiKhoan = '{taiKhoan}'";
            string query = $"SELECT KhachHang_Shop.* FROM KhachHang_Shop INNER JOIN KhachHang ON KhachHang.maKH = KhachHang_Shop.maKH WHERE KhachHang.taiKhoan = '{taiKhoan}'";
            SqlDataReader reader = ExecuteQuery(query);

            bool result = false;
            if (reader.Read())
                result = true;
            reader.Close();
            return result;
        }

        public static void CapNhatMatKhau(TaiKhoan account, bool userState = true)
        {
            string table = userState ? "UserAccount" : "Admin";
            string query = $"UPDATE {table} SET matKhau = '{account.matKhau}' WHERE taiKhoan = '{account.taiKhoan}'";

            ExecuteNonQuery(query);
        }

        public static void CapNhatMatKhau(string taiKhoan, string matKhauMoi)
        {
            string query = $"UPDATE UserAccount SET matKhau = '{matKhauMoi}' WHERE taiKhoan = '{taiKhoan}'";
            ExecuteNonQuery(query);
        }

        private static void SetDiaChis(KhachHang khachHang)
        {
            string query = $"SELECT * FROM DiaChi WHERE maSo = '{khachHang.maSo}' and diaChiKH = 1";
            SqlDataReader reader = ExecuteQuery(query);

            List<DiaChi> listDiaChi = new List<DiaChi>();
            while (reader.Read())
            {
                if (reader.GetString(0) == khachHang.diaChi.maDC)
                    continue;

                DiaChi diaChi = new DiaChi
                {
                    maDC = reader.GetString(0),
                    ten = reader.GetString(2),
                    soDT = reader.GetString(3),
                    maT_TP = reader.GetInt32(4),
                    maQH = reader.GetInt32(5),
                    maPX = reader.GetInt32(6),
                    diaChiCuThe = reader.GetString(7)
                };

                listDiaChi.Add(diaChi);
            }
            reader.Close();
            khachHang.setDiaChis(listDiaChi);
        }

        public static KhachHang DangNhap(User user)
        {
            string query = $"SELECT * FROM KhachHang WHERE taiKhoan = '{user.taiKhoan}'";
            SqlDataReader reader = ExecuteQuery(query);
            reader.Read();

            KhachHang khachHang;

            QLDonHang listDonHang = LoadDonHang(reader.GetString(0));
            GioHang gioHang = LoadGioHang(reader.GetString(0));

            if (reader.IsDBNull(6))
            {
                khachHang = new KhachHang
                {
                    maSo = reader.GetString(0),
                    taiKhoan = reader.GetString(1),
                    gioHang = gioHang,
                    listDonHang = listDonHang
                };

                reader.Close();
                return khachHang;
            }

            khachHang = new KhachHang
            {
                maSo = reader.GetString(0),
                taiKhoan = reader.GetString(1),
                ten = reader.GetString(2),
                soDT = reader.GetString(3),
                email = reader.GetString(4),
                gioiTinh = reader.GetInt32(6),
                ngaySinh = reader.GetDateTime(7),
                nFollow = reader.GetInt32(8),
                chiTieu = reader.GetInt32(9),
                xu = reader.GetInt32(10),
                gioHang = gioHang,
                listDonHang = listDonHang
            };

            if (!reader.IsDBNull(5))
            {
                string maDC = reader.GetString(5);
                reader.Close();

                khachHang.capNhatDiaChi(LoadDiaChi(maDC));
                SetDiaChis(khachHang);
            }

            if (!reader.IsClosed)
                reader.Close();
            return khachHang;
        }

        public static List<string> LoadTheLoai()
        {
            string query = $"SELECT ten FROM LoaiSanPham";
            SqlDataReader reader = ExecuteQuery(query);

            List<string> list = new List<string>();
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }

            reader.Close();
            return list;
        }

        public static List<string> LoadCauHoi()
        {
            string query = $"SELECT cauHoi FROM CauHoi";
            SqlDataReader reader = ExecuteQuery(query);

            List<string> list = new List<string>
            {
                "Câu hỏi bảo mật"
            };
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }

            reader.Close();
            return list;
        }

        public static bool KiemTraCauHoi(string taiKhoan, int maCH, string cauTraLoi)
        {
            string query = $"SELECT maCH, cauTraLoi FROM UserAccount WHERE taiKhoan = '{taiKhoan}'";
            SqlDataReader reader = ExecuteQuery(query);

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
            string query;

            if (bang == "KhachHang")
            {
                query = $"UPDATE {bang} SET ten = N'{nguoi.ten}', soDT = '{nguoi.soDT}', email = '{nguoi.email}', gioiTinh = '{nguoi.gioiTinh}', ngaySinh = '{nguoi.ngaySinh.Date:MM/dd/yyyy}' WHERE {loaiMa} = '{nguoi.maSo}'";
            }
            else
            {
                query = $"UPDATE {bang} SET ten = N'{nguoi.ten}', soDT = '{nguoi.soDT}', email = '{nguoi.email}' WHERE {loaiMa} = '{nguoi.maSo}'";
            }
            
            ExecuteNonQuery(query);
        }

        public static List<string> LoadTinh_ThanhPho()
        {
            string query = $"SELECT ten FROM Tinh_ThanhPho";
            SqlDataReader reader = ExecuteQuery(query);

            List<string> list = new List<string>
            {
                "Tỉnh/Thành Phố"
            };
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }
            reader.Close();

            return list;
        }

        public static List<string> LoadQuan_Huyen(int maT_TP)
        {
            string query = $"SELECT ten FROM Quan_Huyen WHERE maT_TP = {maT_TP}";
            SqlDataReader reader = ExecuteQuery(query);

            List<string> list = new List<string>
            {
                "Quận/Huyện"
            };
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
            string query = $"SELECT ten FROM Phuong_Xa WHERE maQH = {maQH}";
            SqlDataReader reader = ExecuteQuery(query);

            List<string> list = new List<string>
            {
                "Phường/Xã"
            };
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }
            reader.Close();

            return list;
        }
        
        public static string MoTaDiaChi(int maPX)
        {
            string query = $"SELECT Phuong_Xa.ten, Quan_Huyen.ten, Tinh_ThanhPho.ten FROM Tinh_ThanhPho INNER JOIN Quan_Huyen ON Tinh_ThanhPho.maT_TP = Quan_Huyen.maT_TP INNER JOIN Phuong_Xa ON Phuong_Xa.maQH = Quan_Huyen.maQH WHERE Phuong_Xa.maPX = {maPX}";
            SqlDataReader reader = ExecuteQuery(query);

            reader.Read();
            string s = $"{reader.GetString(0)}, {reader.GetString(1)}, {reader.GetString(2)}";
            reader.Close();

            return s;
        }

        public static void ThemDiaChi(string maSo, DiaChi diaChi, bool dcKhachHang = true)
        {
            int state = dcKhachHang == true ? 1 : 0;
            string query = $"INSERT INTO DiaChi VALUES('{diaChi.maDC}', '{maSo}', N'{diaChi.ten}', '{diaChi.soDT}',{diaChi.maT_TP}, {diaChi.maQH}, {diaChi.maPX}, N'{diaChi.diaChiCuThe}', {state})";
            ExecuteNonQuery(query);
        }

        public static void XoaDiaChi(string maDC)
        {
            string query = $"DELETE FROM DiaChi WHERE maDC = {maDC}";
            ExecuteNonQuery(query);
        }

        public static void CapNhatDiaChi(DiaChi diaChi)
        {
            string query = $"UPDATE DiaChi SET ten = N'{diaChi.ten}', soDT = '{diaChi.soDT}', maT_TP = {diaChi.maT_TP}, maQH = {diaChi.maQH}, maPX = {diaChi.maPX}, diaChiCuThe = N'{diaChi.diaChiCuThe}' WHERE maDC = '{diaChi.maDC}'";
            ExecuteNonQuery(query);
        }
      
        public static DiaChi LoadDiaChi(string maDC)
        {
            string query = $"SELECT * FROM DiaChi WHERE maDC = '{maDC}'";
            SqlDataReader reader = ExecuteQuery(query);

            reader.Read();

            DiaChi diaChi = new DiaChi
            {
                maDC = reader.GetString(0),
                ten = reader.GetString(2),
                soDT = reader.GetString(3),
                maT_TP = reader.GetInt32(4),
                maQH = reader.GetInt32(5),
                maPX = reader.GetInt32(6),
                diaChiCuThe = reader.GetString(7)
            };

            reader.Close();

            return diaChi;
        }

        public static void CapNhatDiaChiMacDinh(KhachHang khachHang)
        {
            string query = $"UPDATE KhachHang SET maDC = '{khachHang.diaChi.maDC}' WHERE maKH = '{khachHang.maSo}'";
            ExecuteNonQuery(query);
        }

        public static bool KiemTraTaoShop(KhachHang khachHang) // trả về true nếu khách hàng đã tạo shop
        {
            string query = $"SELECT * FROM KhachHang_Shop WHERE maKH = '{khachHang.maSo}'";
            SqlDataReader reader = ExecuteQuery(query);

            bool result = false;
            if (reader.Read())
                result = true;
            reader.Close();
            return result;
        }

        public static void DangKy(User user, Shop shop)
        {
            string query = $"SELECT maKH FROM KhachHang WHERE taiKhoan = '{user.taiKhoan}'";
            SqlDataReader reader = ExecuteQuery(query);

            reader.Read();
            string maKH = reader.GetString(0);
            reader.Close();

            ThemDiaChi(shop.maSo, shop.diaChi, false);

            query = $"INSERT INTO Shop(maS, ten, soDT, email, maDC, ngayTao, tinhTrang) VALUES('{shop.maSo}', N'{shop.ten}', '{shop.soDT}', '{shop.email}', '{shop.diaChi.maDC}', '{shop.ngaySinh.Date:MM/dd/yyyy}', 1)";
            ExecuteNonQuery(query);

            query = $"INSERT INTO KhachHang_Shop VALUES ('{maKH}', '{shop.maSo}')";
            ExecuteNonQuery(query);

            query = $"UPDATE KhachHang SET soDT = '{shop.soDT}', email = '{shop.email}' WHERE maKH = '{maKH}'";
            ExecuteNonQuery(query);
        }

        public static bool KiemTraTaoShop(User user) //trả về true nếu user đã tạo shop
        {
            string query = $"SELECT KhachHang_Shop.* FROM KhachHang_Shop INNER JOIN KhachHang ON KhachHang.maKH = KhachHang_Shop.maKH WHERE KhachHang.taiKhoan = '{user.taiKhoan}'";

            SqlDataReader reader = ExecuteQuery(query);

            bool result = false;
            if (reader.Read())
                result = true;
            reader.Close();
            return result;
        }

        public static Shop LoadShop(string taiKhoan)
        {
            Shop shop;

            if (KiemTraTaoShop(taiKhoan))
            {
                string query = $"SELECT Shop.* FROM Shop INNER JOIN KhachHang_Shop on KhachHang_Shop.maS = Shop.maS INNER JOIN KhachHang on KhachHang_Shop.maKH = KhachHang.maKH WHERE KhachHang.taiKhoan = '{taiKhoan}'";

                SqlDataReader reader = ExecuteQuery(query);
                reader.Read();
                shop = new Shop
                {
                    maSo = reader.GetString(0),
                    ten = reader.GetString(1),
                    soDT = reader.GetString(2),
                    email = reader.GetString(3),
                    diaChi = LoadDiaChi(reader.GetString(4)),
                    nFollower = reader.GetInt32(5),
                    ngaySinh = reader.GetDateTime(6),
                    tinhTrang = reader.GetInt32(7),
                    doanhThu = reader.GetInt32(8),
                    listBaiDang = new QLBaiDang(),
                    listDonHang = LoadDonHang(reader.GetString(0), false)
                };
                reader.Close();

                query = $"SELECT maBD FROM BaiDang_Shop WHERE maS = '{shop.maSo}'";
                reader = ExecuteQuery(query);

                while (reader.Read())
                {
                    shop.Add(LoadBaiDang(reader.GetString(0)));
                }

                reader.Close();

                /*foreach(BaiDang baiDang in shop.listBaiDang)
                {
                    foreach(SanPham sanPham in baiDang.list)
                    {
                        MessageBox.Show(sanPham.maSP);
                    }
                }*/
            }
            else
            {
                string query = $"SELECT * FROM KhachHang WHERE taiKhoan = '{taiKhoan}'";
                SqlDataReader reader = ExecuteQuery(query);

                reader.Read();

                string soDT = "", email = "";
                if(!reader.IsDBNull(3))
                    soDT = reader.GetString(3);
                if(!reader.IsDBNull(4))
                    email = reader.GetString(4);

                shop = new Shop
                {
                    soDT = soDT,
                    email = email
                };
                reader.Close();
            }

            return shop;
        }
        
        public static void ThemSanPham(string maBD, SanPham sanPham)
        {
            string query = $"INSERT INTO SanPham VALUES('{sanPham.maSP}', '{sanPham.maLoaiSP}', N'{sanPham.ten}', {sanPham.gia}, {sanPham.soLuong}, N'{sanPham.tacGia}', N'{sanPham.dichGia}', N'{sanPham.ngonNgu}', {sanPham.soTrang}, {sanPham.namXuatBan}, N'{sanPham.nhaXuatBan}', N'{sanPham.loaiBia}', N'{sanPham.moTa}', {sanPham.luocBan}, null)";
            ExecuteNonQuery(query);

            query = $"INSERT INTO SanPham_BaiDang VALUES('{sanPham.maSP}', '{maBD}')";
            ExecuteNonQuery(query);
        }

        public static void ThemBaiDang(string maS, BaiDang baiDang)
        {
            foreach (SanPham sanPham in baiDang.list)
                ThemSanPham(baiDang.maBD, sanPham);

            string query = $"INSERT INTO BaiDang VALUES('{baiDang.maBD}', N'{baiDang.tieuDe}', N'{baiDang.moTa}', {baiDang.luocThich}, {baiDang.giamGia})";
            ExecuteNonQuery(query);

            query = $"INSERT INTO BaiDang_Shop VALUES('{baiDang.maBD}', '{maS}')";
            ExecuteNonQuery(query);
        }

        public static BaiDang LoadBaiDang(string maBD)
        {
            string query = $"SELECT BaiDang_Shop.maS, BaiDang.* FROM BaiDang, BaiDang_Shop WHERE BaiDang.maBD = '{maBD}' AND BaiDang_Shop.maBD = '{maBD}'";
            SqlDataReader reader = ExecuteQuery(query);
            reader.Read();

            BaiDang baiDang = new BaiDang
            {
                maBD = maBD,
                maS = reader.GetString(0),
                tieuDe = reader.GetString(2),
                moTa = reader.GetString(3),
                luocThich = reader.GetInt32(4),
                giamGia = reader.GetInt32(5)
            };

            reader.Close();

            query = $"SELECT maSP FROM SanPham_BaiDang WHERE maBD = '{maBD}'";
            reader = ExecuteQuery(query);

            while (reader.Read())
            {
                baiDang.Add(LoadSanPham(reader.GetString(0)));
            }
            reader.Close();
            return baiDang;
        }

        public static SanPham LoadSanPham(string maSP)
        {
            string query = $"SELECT maBD FROM SanPham_BaiDang WHERE maSP = '{maSP}'";
            SqlDataReader reader = ExecuteQuery(query);

            reader.Read();
            string maBD = reader.GetString(0);
            reader.Close();

            query = $"SELECT maS FROM BaiDang_Shop WHERE maBD = '{maBD}'";
            reader = ExecuteQuery(query);

            reader.Read();
            string maS = reader.GetString(0);
            reader.Close();

            query = $"SELECT * FROM SanPham WHERE maSP = '{maSP}'";
            reader = ExecuteQuery(query);

            reader.Read();
            SanPham sanPham = new SanPham
            {
                maSP = reader.GetString(0),
                maLoaiSP = reader.GetString(1),
                ten = reader.GetString(2),
                gia = reader.GetInt32(3),
                soLuong = reader.GetInt32(4),
                tacGia = reader.GetString(5),
                dichGia = reader.GetString(6),
                ngonNgu = reader.GetString(7),
                soTrang = reader.GetInt32(8),
                namXuatBan = reader.GetInt32(9),
                nhaXuatBan = reader.GetString(10),
                loaiBia = reader.GetString(11),
                moTa = reader.GetString(12),
                luocBan = reader.GetInt32(13),
                maS = maS,
                maBD = maBD
            };
            reader.Close();

            return sanPham;
        }

        public static void CapNhatSanPham(SanPham sanPham)
        {
            string query = $"UPDATE SanPham SET maLoaiSP = '{sanPham.maLoaiSP}', ten = N'{sanPham.ten}', gia = {sanPham.gia}, soLuong = {sanPham.soLuong}, tacGia = N'{sanPham.tacGia}', ngonNgu = N'{sanPham.ngonNgu}', soTrang = {sanPham.soTrang}, namXuaBan = {sanPham.namXuatBan}, nhaXuatBan = N'{sanPham.nhaXuatBan}', loaiBia = N'{sanPham.loaiBia}', moTa = N'{sanPham.moTa}', luocBan = {sanPham.luocBan} WHERE maSP = '{sanPham.maSP}'";
            ExecuteNonQuery(query);
        }

        public static void CapNhatBaiDang(BaiDang baiDang)
        {
            foreach (SanPham sanPham in baiDang.list)
                CapNhatSanPham(sanPham);

            string query = $"UPDATE BaiDang SET tieuDe = '{baiDang.tieuDe}', moTa = '{baiDang.moTa}', luocThich = {baiDang.luocThich}, giamGia = {baiDang.giamGia} WHERE maBD = '{baiDang.maBD}'";
            ExecuteNonQuery(query);
        }

        public static void XoaBaiDang(string maBD)
        {
            string query = $"DELETE FROM BaiDang_Shop WHERE maBD = '{maBD}'";
            ExecuteNonQuery(query);

            query = $"DELETE FROM SanPham_BaiDang WHERE maBD = '{maBD}'";
            ExecuteNonQuery(query);

            query = $"DELETE FROM BaiDang WHERE maBD = '{maBD}'";
            ExecuteNonQuery(query);
        }

        public static void CapNhatTinhTrangShop(string maS, int tinhTrang)
        {
            string query = $"UPDATE Shop Set tinhTrang = {tinhTrang} WHERE maS = '{maS}'";
            ExecuteNonQuery(query);
        }

        public static void DatHang(DonHang donHang)
        {
            string query = $"INSERT INTO DonHang VALUES('{donHang.maDH}', '{donHang.diaChi.maDC}', {donHang.tongTien}, {donHang.ptThanhToan}, {donHang.xu}, '{donHang.ngayDatHang.Date:MM/dd/yyyy}', '{donHang.ngayGiaoHang.Date:MM/dd/yyyy}')";
            ExecuteNonQuery(query);

            foreach(SanPham sanPham in donHang.list)
            {
                query = $"INSERT INTO DonHang_SanPham VALUES('{donHang.maDH}', '{sanPham.maSP}')";
                ExecuteNonQuery(query);
            }

            query = $"INSERT INTO DonHang_KhachHang VALUES('{donHang.maDH}', '{donHang.maKH}')";
            ExecuteNonQuery(query);

            query = $"INSERT INTO DonHang_Shop VALUES('{donHang.maDH}', '{donHang.list[0].maS}')";
            ExecuteNonQuery(query);
        }

        public static void DiDon(DonHang donHang)
        {
            string query = $"UPDATE DonHang SET tinhTrang = {donHang.tinhTrang}, ngayGiaoHang = '{donHang.ngayGiaoHang.Date:MM/dd/yyyy}' WHERE maDH = '{donHang.maDH}'";
            ExecuteNonQuery(query);
        }

        public static void HuyDon(DonHang donHang)
        {
            string query = $"UPDATE DonHang SET tinhTrang = {donHang.tinhTrang}, ngayGiaoHang = '{donHang.ngayGiaoHang.Date:MM/dd/yyyy}' WHERE maDH = '{donHang.maDH}'";
            ExecuteNonQuery(query);
        }

        public static void NhanHang(DonHang donHang, string maKH)
        {
            string query = $"UPDATE DonHang SET tinhTrang = {donHang.tinhTrang}, ngayGiaoHang = '{donHang.ngayGiaoHang.Date:MM/dd/yyyy}' WHERE maDH = '{donHang.maDH}'";
            ExecuteNonQuery(query);

            query = $"SELECT chiTieu FROM KhachHang WHERE maKH = '{maKH}'";
            SqlDataReader reader = ExecuteQuery(query);
            reader.Read();
            int chiTieu = reader.GetInt32(0) + donHang.tongTien - donHang.xu;
            reader.Close();
            query = $"UPDATE KhachHang SET chiTieu = {chiTieu} WHERE maKH = '{maKH}'";
            ExecuteNonQuery(query);

            query = $"SELECT doanhThu FROM Shop WHERE maS = '{donHang.list[0].maS}'";
            reader = ExecuteQuery(query);
            reader.Read();
            int doanhThu = reader.GetInt32(0) + donHang.tongTien;
            reader.Close();
            query = $"UPDATE Shop SET doanhThu = {doanhThu} WHERE maS = '{donHang.list[0].maS}'";
            ExecuteNonQuery(query);
        }

        public static QLDonHang LoadDonHang(string maSo, bool khachHang = true)
        {
            QLDonHang listDonHang = new QLDonHang();
            string query;

            if (khachHang)
                query = $"SELECT DonHang.* FROM DonHang INNER JOIN DonHang_KhachHang ON DonHang_KhachHang.maDH = DonHang.maDH WHERE DonHang_KhachHang.maKH = '{maSo}'";
            else
                query = $"SELECT DonHang.* FROM DonHang INNER JOIN DonHang_Shop ON DonHang_Shop.maDH = DonHang.maDH WHERE DonHang_Shop.maS = '{maSo}'";

            SqlDataReader reader = ExecuteQuery(query);

            while (reader.Read())
            {
                listDonHang.Add(new DonHang
                {
                    list = new List<SanPham>(),
                    maDH = reader.GetString(0),
                    tongTien = reader.GetInt32(2),
                    tinhTrang = reader.GetInt32(3),
                    ptThanhToan = reader.GetInt32(4),
                    ngayDatHang = reader.GetDateTime(5),
                    ngayGiaoHang = reader.GetDateTime(6),
                    diaChi = LoadDiaChi(reader.GetString(1))
                });
            }
            reader.Close();

            foreach(DonHang donHang in listDonHang.list)
            {
                query = $"SELECT maSP, soLuong FORM DonHang_SanPham WHERE maDH = '{donHang.maDH}'";
                reader = ExecuteQuery(query);
                
                while(reader.Read())
                {
                    donHang.Add(LoadSanPham(reader.GetString(0)));
                    donHang.list.Last().soLuong = reader.GetInt32(1);
                }
                reader.Close();
            }

            return listDonHang;
        }

        public static double GetGiamGia(string maSP)
        {
            string query = $"SELECT BD.giamGia FROM BaiDang BD INNER JOIN SanPham_BaiDang SPBD ON SPBD.maBD = BD.maBD WHERE SPBD.maSP = '{maSP}'";
            SqlDataReader reader = ExecuteQuery(query);

            reader.Read();
            double giamGia = reader.GetInt32(0) / 100;
            reader.Close();

            return giamGia;
        }

        public static void ThemVaoGioHang(SanPham sanPham, string maKH, bool exist = false)
        {
            string query;
            if(exist)
            {
                query = $"UPDATE GioHang SET soLuong = {sanPham.soLuong}, ngayThem = '{sanPham.ngayThem.Date:MM/dd/yyyy}' WHERE maKH = {maKH} AND maSP = {sanPham.maSP}";
            }
            else
            {
                query = $"INSERT INTO GioHang VALUES('{maKH}', '{sanPham.maSP}', {sanPham.soLuong}, '{sanPham.ngayThem.Date:MM/dd/yyyy}')";
            }
            ExecuteNonQuery(query);
        }

        public static void XoaSPKhoiGioHang(string maKH, params string[] list)
        {
            foreach (string maSP in list)
            {
                string query = $"DELETE FROM GioHang WHERE maKH = '{maKH}' AND maSP = '{maSP}'";
                ExecuteNonQuery(query);
            }
        } 

        public static GioHang LoadGioHang(string maKH)
        {
            GioHang gioHang = new GioHang();

            string query = $"SELECT maSP, soLuong, ngayThem FROM GioHang WHERE maKH = '{maKH}'";
            SqlDataReader reader = ExecuteQuery(query);

            while (reader.Read())
            {
                gioHang.Add(LoadSanPham(reader.GetString(0)));
                gioHang.list.Last().soLuong = reader.GetInt32(1);
                gioHang.list.Last().ngayThem = reader.GetDateTime(2);
            }
            reader.Close();

            gioHang.maKH = maKH;

            return gioHang;
        }

        public static QLBaiDang SearchBaiDang(string key)
        {
            QLBaiDang list = new QLBaiDang();

            string query = $"SELECT BDS.maS, BD.* FROM BaiDang BD INNER JOIN BaiDang_Shop BDS ON BDS.maBD = BD.maBD WHERE BD.tieuDe LIKE N'%{key}%'";
            SqlDataReader reader = ExecuteQuery(query);

            while(reader.Read())
            {
                list.Add(new BaiDang
                {
                    maS = reader.GetString(0),
                    maBD = reader.GetString(1),
                    tieuDe = reader.GetString(2),
                    moTa = reader.GetString(3),
                    luocThich = reader.GetInt32(4),
                    giamGia = reader.GetInt32(5)
                });
            }
            reader.Close();

            query = $"SELECT SPBD.maBD FROM SanPham_BaiDang SPBD INNER JOIN SanPham SP ON SP.maSP = SPBD.maBD WHERE SP.ten LIKE N'%{key}%'";
            reader = ExecuteQuery(query);

            while (reader.Read())
            {
                list.Add(LoadBaiDang(reader.GetString(0)));
            }
            reader.Close();

            return list;
        }
    }
}
