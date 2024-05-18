using Program.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
