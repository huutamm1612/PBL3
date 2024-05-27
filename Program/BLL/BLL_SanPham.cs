using Program.DAL;
using Program.DTO;
using System;
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

        public void CapNhatSanPham(SanPham sanPham)
        {
            DAL_SanPham.Instance.CapNhatSanPham(sanPham);
        }


        public string GetMaMoi()
        {
            return Database.Instance.MaMoi("maSP");
        }

        public int GetSoLuongFromMaSP(string maSP)
        {
            return DAL_SanPham.Instance.LoadSoLuongFromMaSP(maSP);
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
