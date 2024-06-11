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

        public string GetMaDHFromNoiDungTB(string noiDung)
        {
            foreach (string s in noiDung.Split())
            {
                if (s.Contains("DH"))
                    return s;
            }
            return "";
        }

        public int GetXuFromThongBao(string noiDung)
        {
            foreach(string s in noiDung.Split())
            {
                if(int.TryParse(s, out int xu)) 
                    return xu;
            }
            return 0;
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
        public string ThongBaoTinhTrangDHChoS(string noiDung)
        {
            string tinhTrangDH = "";

            if (noiDung.Contains("đã đặt"))
                tinhTrangDH = "Chờ xác nhận";
            else if (noiDung.Contains("đã bị hủy"))
                tinhTrangDH = "Đơn hàng đã bị hủy";
            else if (noiDung.Contains("giao thành công"))
                tinhTrangDH = "Đơn hàng đã được giao thành công";

            return tinhTrangDH;
        }

        public string GetMaMoi()
        {
            return Database.Instance.MaMoi("maTB");
        }
    }
}
