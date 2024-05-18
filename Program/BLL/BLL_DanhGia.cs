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

        public string GetMaMoi()
        {
            return Database.Instance.MaMoi("maDG");
        }
    }
}
