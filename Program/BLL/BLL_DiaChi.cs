using Program.DAL;
using Program.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.BLL
{
    internal class BLL_DiaChi
    {
        private static BLL_DiaChi _Instance;
        public static BLL_DiaChi Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BLL_DiaChi();
                return _Instance;
            }
            private set { }
        }
        private BLL_DiaChi()
        {

        }

        public string GetTenTTPFromMaTTP(int maTTP)
        {
            return DAL_DiaChi.Instance.LoadTenT_TP(maTTP);
        }
        public string GetTenPXFromMaPX(int maPX)
        {
            return DAL_DiaChi.Instance.LoadTenPX(maPX);
        }
        public string GetTenQHFromMaQH(int maQH)
        {
            return DAL_DiaChi.Instance.LoadTenQH(maQH);
        }
        public void CapNhatDiaChi(KhachHang khachHang, DiaChi diaChi)
        {
            if (diaChi.maDC == khachHang.diaChi.maDC)
            {
                khachHang.CapNhatDiaChi(diaChi);
            }
            else
            {
                for (int i = 0; i < khachHang.listDiaChi.Count; i++)
                {
                    if (khachHang.listDiaChi[i].maDC == diaChi.maDC)
                    {
                        khachHang.capNhatDiaChi(i, diaChi);
                        break;
                    }
                }
            }
            DAL_DiaChi.Instance.CapNhatDiaChi(diaChi);
        }

        public void ThemDiaChi(KhachHang khachHang, DiaChi diaChi)
        {
            BLL_KhachHang.Instance.ThemDiaChi(diaChi, khachHang.maSo);
            if (khachHang.diaChi == null)
            {
                khachHang.CapNhatDiaChi(diaChi);
                BLL_KhachHang.Instance.CapNhatDiaChiMacDinh(khachHang, diaChi);
            }
            else
            {
                khachHang.themDiaChi(diaChi);
            }
        }

        public string GetMaMoi()
        {
            return Database.Instance.MaMoi("MaDC");
        }

        public List<CBBItem> GetAllTinh_ThanhPho()
        {
            List<CBBItem> list = new List<CBBItem>();
            foreach(int maT_TP in DAL_DiaChi.Instance.LoadAllMaT_TP())
            {
                list.Add(new CBBItem { Value = maT_TP, Text = DAL_DiaChi.Instance.LoadTenT_TP(maT_TP)});
            }
            return list;
        }

        public List<CBBItem> GetAllQuanHuyenFromMa_TTP(int maT_TP) 
        {
            List<CBBItem> list = new List<CBBItem>();
            foreach (int maQH in DAL_DiaChi.Instance.LoadAllMaQHFromMaT_TP(maT_TP))
            {
                list.Add(new CBBItem { Value = maQH, Text = DAL_DiaChi.Instance.LoadTenQH(maQH) });
            }
            return list;
        }

        public List<CBBItem> GetAllPhuongXaFromMaQH(int maQH)
        {
            List<CBBItem> list = new List<CBBItem>();
            foreach (int maPX in DAL_DiaChi.Instance.LoadAllMaPXFromMaQH(maQH))
            {
                list.Add(new CBBItem { Value = maPX, Text = DAL_DiaChi.Instance.LoadTenPX(maPX) });
            }
            return list;
        }

        public string MoTaDiaChi(DiaChi diaChi)
        {
            return $"{DAL_DiaChi.Instance.LoadTenPX(diaChi.maPX)}, {DAL_DiaChi.Instance.LoadTenQH(diaChi.maQH)}, {DAL_DiaChi.Instance.LoadTenT_TP(diaChi.maT_TP)}";
        }
    } 
}
