using Program.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.BLL
{
    internal class BLL_BaiDang
    {
        private static BLL_BaiDang _Instance;
        public static BLL_BaiDang Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BLL_BaiDang();
                return _Instance;
            }
            private set { }
        }
        private BLL_BaiDang()
        {

        }

        public List<string> GetAllLyDoToCaoBaiDang()
        {
            return DAL_BaiDang.Instance.LoadLyDoToCaoBaiDang();
        }
        public int GetSoLuongBaiDangViPhamFromMaS(string maS) => DAL_BaiDang.Instance.LoadSoLuongBaiDangViPhamFromMaS(maS);

        public QLBaiDang GetAllBaiDangViPhamFromMaS(string maS) => DAL_BaiDang.Instance.LoadAllBaiDangViPhamFromMaS(maS);

        public QLBaiDang GetAllBaiDangDaAnFromMaS(string maS) => DAL_BaiDang.Instance.LoadAllBaiDangDaAnFromMaS(maS);

        public QLBaiDang GetBaiDangFromTextSearch(string text)
        {
            text = Utils.Instance.XoaDau(text);

            QLBaiDang qLBaiDang = new QLBaiDang();

            string query = $"SELECT DISTINCT BD.maBD FROM BaiDang BD JOIN SanPham_BaiDang SPBD ON SPBD.maBD = BD.maBD JOIN SanPham SP ON SP.maSP = SPBD.maSP WHERE (SP.ten LIKE N'%{text}%' OR BD.tieuDe LIKE '%{text}%' OR SP.tacGia LIKE '%{text}%') AND BD.maBD NOT IN (SELECT maBD FROM BaiDangViPham)";

            foreach (string maLoaiSP in DAL_SanPham.Instance.LoadMaLoaiSPFromText(text))
            {
                query += $" OR maLoaiSP = '{maLoaiSP}'";
            }

            foreach (string maBD in DAL_BaiDang.Instance.LoadMaBDWithQuery(query))
            {
                BaiDang baiDang = DAL_BaiDang.Instance.LoadBaiDangFromMaBD(maBD);
                qLBaiDang.Add(baiDang);
            }

            return qLBaiDang;
        }

        public List<string> TaoListDanhMucVaSoLuong(QLBaiDang qLBaiDang)
        {
            Dictionary<string, int> soLuongMap = new Dictionary<string, int>();

            foreach(BaiDang baiDang in qLBaiDang.list)
            {
                foreach(SanPham sanPham in baiDang.list)
                {
                    if (!soLuongMap.ContainsKey(sanPham.loaiSP.tenLoaiSP))
                    {
                        soLuongMap.Add(sanPham.loaiSP.tenLoaiSP, 1);
                    }
                }
            }

            List<string> list = new List<string>();

            foreach (var item in soLuongMap.OrderBy(kv => kv.Value).ToList())
            {
                list.Add(item.Key);
            }

            return list;
        }

        public QLBaiDang GetBaiDangInKhoangGia(QLBaiDang qLBaiDang, int min, int max)
        {
            QLBaiDang newQLBD = new QLBaiDang();
            
            foreach(BaiDang baiDang in qLBaiDang.list)
            {
                int gia = Utils.Instance.GiamGia(baiDang.giaMin(), baiDang.giamGia);
                //MessageBox.Show(gia.ToString());
                if (min < gia && gia < max)
                    newQLBD.Add(baiDang);
            }

            return newQLBD;
        }

        public string GetURLFromMaDH(string maDH)
        {
            return DAL_BaiDang.Instance.Load1URLFromMaDH(maDH);
        }

        public string GetTieuDeFromMaBD(string maBD)
        {
            return DAL_BaiDang.Instance.LoadTieuDeFromMaBD(maBD);
        }

        public string GetURLFromMaBD(string maBD)
        {
            return DAL_BaiDang.Instance.LoadURLFromMaBD(maBD);
        }

        public void XoaBaiDang(BaiDang baiDang)
        {
            DAL_BaiDang.Instance.XoaBaiDang(baiDang);
        }

        public void ThemBaiDang(BaiDang baiDang)
        {
            DAL_BaiDang.Instance.ThemBaiDang(baiDang);
        }

        public void CapNhatBaiDang(BaiDang baiDang)
        {
            DAL_BaiDang.Instance.CapNhatBaiDang(baiDang);
        }

        public BaiDang GetBaiDangFromMaBD(string maBD)
        {
            return DAL_BaiDang.Instance.LoadBaiDangFromMaBD(maBD);
        }

        public bool IsSamePrice(BaiDang baiDang)
        {
            return baiDang.giaMax() == baiDang.giaMin();
        }

        public QLBaiDang GetBaiDangForHomeList()
        {
            return DAL_BaiDang.Instance.LoadALLBaiDang();
        }

        public int GetGiamGiaFromMaSP(string maSP)
        {
            return DAL_BaiDang.Instance.LoadGiamGiaFromMaSP(maSP);
        }

        public string GetMaMoi()
        {
            return Database.Instance.MaMoi("maBD");
        }
    }
}
