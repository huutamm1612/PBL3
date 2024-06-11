using Program.DAL;
using Program.DTO;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.BLL
{
    internal class BLL_SanPham
    {
        private static BLL_SanPham _Instance;
        public static BLL_SanPham Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BLL_SanPham();
                return _Instance;
            }
            private set { }
        }
        private BLL_SanPham()
        {

        }

        public QLSanPham GetAllSanPhamDaAnFromMaS(string maS) => new QLSanPham { list = DAL_SanPham.Instance.LoadAllSanPhamDaAnFromMaS(maS) };
        public QLSanPham GetAllSanPhamViPhamFromMaS(string maS) => new QLSanPham { list = DAL_SanPham.Instance.LoadAllSanPhamViPhamFromMaS(maS) };

        public string KiemTraTinhTrang(string maSP)
        {
            if (GetSoLuongFromMaSP(maSP) == 0)
                return "HẾT HÀNG";
            if (DAL_SanPham.Instance.KiemTraViPham(maSP))
                return "KHÔNG HIỆU LỰC";

            return "TỐT";
        }

        public void CapNhatSanPham(SanPham sanPham) => DAL_SanPham.Instance.CapNhatSanPham(sanPham); 

        public bool KiemTraBaiDangDaAn(string maSP) => DAL_SanPham.Instance.KiemTraBaiDangDaAn(maSP);// true if bd da an

        public string GetMaMoi()
        {
            return Database.Instance.MaMoi("maSP");
        }

        public int GetSoLuongFromMaSP(string maSP)
        {
            return DAL_SanPham.Instance.LoadSoLuongFromMaSP(maSP);
        }

        public SanPham GetSanPhamFromMaSP(string maSP)
        {
            return DAL_SanPham.Instance.LoadSanPhamFromMaSP(maSP);
        }

        public List<CBBItem> GetAllLoaiSanPham()
        {
            List<CBBItem> list = new List<CBBItem>();
            foreach(LoaiSanPham lsp in DAL_SanPham.Instance.LoadAllLoaiSanPham())
            {
                list.Add(new CBBItem { Value = int.Parse(lsp.maLoaiSP), Text = lsp.tenLoaiSP });
            }
            return list;
        }
    }
}
