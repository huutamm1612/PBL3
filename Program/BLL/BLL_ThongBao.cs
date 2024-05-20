using Program.DAL;
using Program.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.BLL
{
    internal class BLL_ThongBao
    {
        private static BLL_ThongBao _Instance;
        public static BLL_ThongBao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BLL_ThongBao();
                return _Instance;
            }
            private set { }
        }
        private BLL_ThongBao()
        {

        }

        public void ThemThongBao(ThongBao thongBao)
        {
            DAL_ThongBao.Instance.ThemThongBao(thongBao);
        }

        public string GetMaMoi()
        {
            return Database.Instance.MaMoi("maTB");
        }
    }
}
