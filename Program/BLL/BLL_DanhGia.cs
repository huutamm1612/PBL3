using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.BLL
{
    internal class BLL_DanhGia
    {
        private static BLL_DanhGia _Instance;
        public static BLL_DanhGia Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BLL_DanhGia();
                return _Instance;
            }
            private set { }
        }
        private BLL_DanhGia()
        {

        }

        public List<DanhGia> TaoDGMoiTuDH(DonHang donHang)
        {
            List<DanhGia> list = new List<DanhGia>();

            foreach(QLSanPham qLSanPham in donHang.PhanRaTheoBaiDang())
            {
                string sanPhamDaMua = "";
                foreach(SanPham sanPham in qLSanPham.list)
                {
                    sanPhamDaMua += sanPham.ten + ",";
                }
                sanPhamDaMua = sanPhamDaMua.Substring(0, sanPhamDaMua.Length - 1);  

                list.Add(new DanhGia
                {
                    maKH = donHang.maKH,
                    maBD = qLSanPham.list[0].maBD,
                    sanPhamDaMua = sanPhamDaMua,
                });
            }

            return list;
        }

        public string GetMaMoi()
        {
            return Database.Instance.MaMoi("maDG");
        }
    }
}
