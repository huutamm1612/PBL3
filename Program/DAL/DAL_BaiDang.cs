﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.DAL
{
    internal class DAL_BaiDang
    {
        private static DAL_BaiDang _Instance;
        public static DAL_BaiDang Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DAL_BaiDang();
                return _Instance;
            }
            private set { }
        }
        private DAL_BaiDang()
        {

        }

        public int LoadSoLuongBaiDangViPhamFromMaS(string maS)
        {
            string query = $"SELECT 1 FROM BaiDang BD JOIN BaiDang_Shop BDS ON BDS.maBD = BD.maBD WHERE BDS.maS = @maS AND BD.tinhTrang = 1";
            SqlParameter param = new SqlParameter("@maS", maS);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            return table.Rows.Count;
        }

        public void ThemBaiDangViPham(string maBD, string lyDo)
        {
            string query = "INSERT INTO BaiDangViPham VALUES(@maBD, @lyDo)";
            SqlParameter param1 = new SqlParameter("@maBD", maBD);
            SqlParameter param2 = new SqlParameter("@lyDo", lyDo);
            Database.Instance.ExecuteNonQuery(query, param1, param2);

            query = "UPDATE BaiDang SET tinhTrang = 1 WHERE maBD = @maBD";
            SqlParameter param3 = new SqlParameter("@maBD", maBD);
            Database.Instance.ExecuteNonQuery(query, param3);
        }

        public void GoBaiDangViPham(string maBD)
        {
            string query = "DELETE From BaiDangViPham WHERE maBD = @maBD";
            SqlParameter param1 = new SqlParameter("@maBD", maBD);
            Database.Instance.ExecuteNonQuery(query, param1);

            query = "UPDATE BaiDang SET tinhTrang = 0 WHERE maBD = @maBD";
            SqlParameter param2 = new SqlParameter("@maBD", maBD);
            Database.Instance.ExecuteNonQuery(query, param2);
        }

        public void AnBaiDang(string maBD)
        {
            string query = "UPDATE BaiDang SET tinhTrang = 2 WHERE maBD = @maBD";
            SqlParameter param = new SqlParameter("@maBD", maBD);
            Database.Instance.ExecuteNonQuery(query, param);
        }

        public void HoanTacBaiDang(string maBD)
        {
            string query = "UPDATE BaiDang SET tinhTrang = 0 WHERE maBD = @maBD";
            SqlParameter param = new SqlParameter("@maBD", maBD);
            Database.Instance.ExecuteNonQuery(query, param);
        }

        public List<string> LoadLyDoToCaoBaiDang()
        {
            string query = "SELECT * FROM LyDo WHERE loaiLyDo = 2";

            List<string> list = new List<string>();

            DataTable table = Database.Instance.ExecuteQuery(query);
            foreach (DataRow row in table.Rows)
            {
                list.Add(row["noiDung"].ToString());
            }

            return list;
        }

        public List<string> LoadGoiYTimKiemBaiDang(string noiDung)
        {
            string query = $"SELECT DISTINCT TOP 10 tieuDe FROM BaiDang WHERE tieuDe LIKE '{noiDung}%'";
            DataTable table = Database.Instance.ExecuteQuery(query);

            List<string> list = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(row["tieuDe"].ToString());
            }

            return list;
        }

        public List<string> LoadMaBDWithQuery(string query)
        {
            DataTable table = Database.Instance.ExecuteQuery(query);

            List<string> list = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(row["maBD"].ToString());
            }

            return list;
        }

        public string Load1URLFromMaDH(string maDH)
        {
            string query = "SELECT BD.anh FROM BaiDang BD JOIN SanPham_BaiDang SPBD ON SPBD.maBD = BD.maBD JOIN DonHang_SanPham DHSP ON SPBD.maSP = DHSP.maSP WHERE maDH = @maDH";
            SqlParameter param = new SqlParameter("@maDH", maDH);
            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];
        
            return row["anh"].ToString();
        }

        public void ThemDanhGia(string maDG, string maBD)
        {
            string query = "INSERT INTO DanhGia_BaiDang VALUES(@maDG, @maBD)";
            SqlParameter param1 = new SqlParameter("@maDG", maDG);
            SqlParameter param2 = new SqlParameter("@maBD", maBD);
            Database.Instance.ExecuteNonQuery(query, param1, param2);
        }

        public string LoadTieuDeFromMaBD(string maBD)
        {
            string query = "SELECT BaiDang.tieuDe FROM BaiDang WHERE maBD = @maBD";
            SqlParameter param = new SqlParameter("@maBD", maBD);

            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];
            return row["tieuDe"].ToString();
        }

        public string LoadURLFromMaBD(string maBD)
        {
            string query = "SELECT BaiDang.anh FROM BaiDang WHERE maBD = @maBD";
            SqlParameter param = new SqlParameter("@maBD", maBD);

            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];
            return row["anh"].ToString();
        }

        public void Thich(string maBD)
        {
            string query = "UPDATE BaiDang SET luocThich = luocThich + 1 WHERE maBD = @maBD";
            SqlParameter param = new SqlParameter("@maBD", maBD);
            Database.Instance.ExecuteNonQuery(query, param);
        }

        public void HuyThich(string maBD)
        {
            string query = "UPDATE BaiDang SET luocThich = luocThich - 1 WHERE maBD = @maBD";
            SqlParameter param = new SqlParameter("@maBD", maBD);
            Database.Instance.ExecuteNonQuery(query, param);
        }

        public int LoadGiamGiaFromMaSP(string maSP)
        {
            string query = "SELECT BD.giamGia FROM BaiDang BD JOIN SanPham_BaiDang SPBD ON SPBD.maBD = BD.maBD WHERE SPBD.maSP = @maSP";
            SqlParameter param = new SqlParameter("@maSP", maSP);

            DataRow row = Database.Instance.ExecuteQuery(query, param).Rows[0];

            return Convert.ToInt32(row["giamGia"]);
        }

        public QLBaiDang LoadALLBaiDang()
        {
            QLBaiDang list = new QLBaiDang();
            string query = "SELECT * FROM BaiDang BD JOIN BaiDang_Shop BDS ON BDS.maBD = BD.maBD AND BD.tinhTrang = 0";
            DataTable table = Database.Instance.ExecuteQuery(query);

            foreach (DataRow row in table.Rows)
            {
                list.Add(LoadBaiDang(row));
            }

            return list;
        }

        public QLBaiDang LoadAllBaiDangFromMaS(string maS)
        {
            QLBaiDang list = new QLBaiDang();
            string query = $"SELECT * FROM BaiDang BD JOIN BaiDang_Shop BDS ON BDS.maBD = BD.maBD WHERE BDS.maS = @maS AND BD.tinhTrang = 0";
            SqlParameter param = new SqlParameter("@maS", maS);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            foreach(DataRow row in table.Rows)
            {
                list.Add(LoadBaiDang(row));
            }

            return list;
        }

        public BaiDang LoadBaiDangHoatDongFromMaBD(string maBD)
        {
            string query = $"SELECT * FROM BaiDang BD JOIN BaiDang_Shop BDS ON BDS.maBD = BD.maBD WHERE BD.maBD = @maBD AND BD.tinhTrang = 0";
            SqlParameter param = new SqlParameter("@maBD", maBD);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            if(table.Rows.Count != 0)
                return LoadBaiDang(table.Rows[0]);
            return null;
        }

        public BaiDang LoadBaiDangFromMaBD(string maBD)
        {
            string query = $"SELECT * FROM BaiDang BD JOIN BaiDang_Shop BDS ON BDS.maBD = BD.maBD WHERE BD.maBD = @maBD";
            SqlParameter param = new SqlParameter("@maBD", maBD);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            if (table.Rows.Count != 0)
                return LoadBaiDang(table.Rows[0]);
            return null;
        }

        public BaiDang LoadBaiDangViPhamFromMaBD(string maBD)
        {
            string query = $"SELECT * FROM BaiDang BD JOIN BaiDang_Shop BDS ON BDS.maBD = BD.maBD WHERE BD.maBD = @maBD AND BD.tinhTrang = 1";
            SqlParameter param = new SqlParameter("@maBD", maBD);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            if (table.Rows.Count != 0)
                return LoadBaiDang(table.Rows[0]);
            return null;
        }

        public QLBaiDang LoadAllBaiDangDaAnFromMaS(string maS)
        {
            QLBaiDang list = new QLBaiDang();
            string query = $"SELECT * FROM BaiDang BD JOIN BaiDang_Shop BDS ON BDS.maBD = BD.maBD WHERE BDS.maS = @maS AND BD.tinhTrang = 2";
            SqlParameter param = new SqlParameter("@maS", maS);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            foreach (DataRow row in table.Rows)
            {
                list.Add(LoadBaiDang(row));
            }

            return list;
        }

        public QLBaiDang LoadAllBaiDangViPhamFromMaS(string maS)
        {
            QLBaiDang list = new QLBaiDang();
            string query = $"SELECT * FROM BaiDang BD JOIN BaiDang_Shop BDS ON BDS.maBD = BD.maBD WHERE BDS.maS = @maS AND BD.tinhTrang = 1";
            SqlParameter param = new SqlParameter("@maS", maS);
            DataTable table = Database.Instance.ExecuteQuery(query, param);

            foreach (DataRow row in table.Rows)
            {
                list.Add(LoadBaiDang(row));
            }

            return list;
        }

        public void CapNhatBaiDang(BaiDang baiDang)
        {
            string query = "UPDATE BaiDang SET tieuDe = @tieuDe, moTa = @moTa, luocThich = @luocThich, giamGia = @giamGia, anh = @anhBia WHERE maBD = @maBD";
            Database.Instance.ExecuteNonQuery(query, baiDang.GetParameters().ToArray());
        }

        public void ThemBaiDang(BaiDang baiDang)
        {
            string query = $"INSERT INTO BaiDang VALUES(@maBD, @tieuDe, @moTa, @luocThich, @giamGia, @anhBia, 0)";
            Database.Instance.ExecuteNonQuery(query, baiDang.GetParameters().ToArray());

            query = $"INSERT INTO BaiDang_Shop VALUES(@maBD, @maS)";
            Database.Instance.ExecuteNonQuery(query, baiDang.GetParameters().ToArray());

            
        }

        public void XoaBaiDang(BaiDang baiDang)
        {
            string query = "DELETE FROM BaiDang_Shop Where maBD = @maBD; DELETE FROM SanPham_BaiDang WHERE maBD = @maBD; DELETE FROM Thich WHERE maBD = @maBD; DELETE FROM DaXemGanDay WHERE maBD = @maBD; DELETE FROM BaiDang WHERE maBD = @maBD";
            SqlParameter param = new SqlParameter("@maBD", baiDang.maBD);
            Database.Instance.ExecuteNonQuery(query, param);

            foreach (SanPham sanPham in baiDang.list)
                DAL_SanPham.Instance.ThemVaoSanPhamDaXoa(sanPham.maSP, sanPham.maS);
        }

        private BaiDang LoadBaiDang(DataRow row)
        {
            return new BaiDang
            {
                maBD = row["maBD"].ToString(),
                maS = row["maS"].ToString(),
                tieuDe = row["tieuDe"].ToString(),
                moTa = row["moTa"].ToString(),
                luocThich = Convert.ToInt32(row["luocThich"]),
                giamGia = Convert.ToInt32(row["giamGia"]),
                anhBia = row["anh"].ToString(),
                list = DAL_SanPham.Instance.LoadAllSanPhamFromMaBD(row["maBD"].ToString()),
                listDanhGia = DAL_DanhGia.Instance.LoadAllDanhGiaFromMaBD(row["maBD"].ToString())
            };
        }
    }
}
