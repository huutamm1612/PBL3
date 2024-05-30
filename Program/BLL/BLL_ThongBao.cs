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
    
        public bool IsVanChuyenDaGui(string maDH)
        {
            return DAL_ThongBao.Instance.IsVanChuyenDaGui(maDH);
        }

        public string ThongBaoTinhTrangDHChoKH(string noiDung)
        {
            string tinhTrangDH = "";

            if (noiDung.Contains("gửi đi"))
                tinhTrangDH = "Đang vận chuyển";
            else if (noiDung.Contains("đã bị hủy"))
                tinhTrangDH = "Đơn hàng đã bị hủy";
            else if (noiDung.Contains("bàn giao cho bên vận chuyển"))
                tinhTrangDH = "Đơn hàng đang được vận chuyển đến bạn";
            else if (noiDung.Contains("đã đến"))
                tinhTrangDH = "Đơn hàng đã giao đến bạn";

            return tinhTrangDH;
        }

        public string GetMaMoi()
        {
            return Database.Instance.MaMoi("maTB");
        }
    }
}
