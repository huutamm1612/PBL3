using Program.DAL;
using Program.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.BLL
{
    internal class BLL_Admin
    {
        private static BLL_Admin _Instance;
        public static BLL_Admin Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BLL_Admin();
                return _Instance;
            }
            private set { }
        }
        private BLL_Admin()
        {

        }

        public Admin DangNhap(string username, string password)
        {
            return DAL_Admin.Instance.DangNhap(username, password);
        }

        public void SetListBDViPham(ref List<string> listMaBD, ref List<string> listNoiDung, ref List<string> listMaTB)
        {
            listMaBD = new List<string>();
            listNoiDung = new List<string>();
            listMaTB = new List<string>();

            QLThongBao listThongBao = DAL_ThongBao.Instance.LoadAllThongBaoToHeThong();
            foreach(ThongBao thongBao in listThongBao.list)
            {
                if (thongBao.from == "AnDanh" && thongBao.dinhKem.Contains("BD"))
                {
                    listMaBD.Add(thongBao.dinhKem.Substring(2));
                    listNoiDung.Add(thongBao.noiDung);
                    listMaTB.Add(thongBao.maTB);
                }
            }
        }

        public void SetListDGViPham(ref List<string> listMaBD, ref List<string> listNoiDung, ref List<string> listMaTB)
        {
            listMaBD = new List<string>();
            listNoiDung = new List<string>();
            listMaTB = new List<string>();

            QLThongBao listThongBao = DAL_ThongBao.Instance.LoadAllThongBaoToHeThong();
            foreach (ThongBao thongBao in listThongBao.list)
            {
                if (thongBao.dinhKem.Contains("DG"))
                {
                    listMaBD.Add(thongBao.dinhKem.Substring(2));
                    listNoiDung.Add(thongBao.noiDung);
                    listMaTB.Add(thongBao.maTB);
                }
            }
        }

        public void SetListYCGoViPhamBD(ref List<string> listMaBD, ref List<string> listNoiDung, ref List<string> listMaTB)
        {
            listMaBD = new List<string>();
            listNoiDung = new List<string>();
            listMaTB = new List<string>();

            QLThongBao listThongBao = DAL_ThongBao.Instance.LoadAllThongBaoToHeThong();
            foreach (ThongBao thongBao in listThongBao.list)
            {
                if (thongBao.from.Contains("ShopAnDanh"))
                {
                    listMaBD.Add(thongBao.dinhKem.Substring(2));
                    listNoiDung.Add(thongBao.noiDung);
                    listMaTB.Add(thongBao.maTB);
                }
            }
        }

        public void XacNhanBDViPham(string maTB, string maBD, string lyDo)
        {
            DAL_Admin.Instance.XacNhanBDViPham(maTB, maBD);
            DAL_BaiDang.Instance.ThemBaiDangViPham(maBD, lyDo);
            DAL_SanPham.Instance.XacNhanSanPhamViPham(maBD);
        }
        public void XacNhanDGViPham(string maTB, string maDG, string lyDo)
        {
            DAL_Admin.Instance.XacNhanDGViPham(maTB, maDG);
            DAL_DanhGia.Instance.ThemDanhGiaViPham(maDG, lyDo);
        }

        public void GoViPham(string maBD, string maTB)
        {
            DAL_BaiDang.Instance.GoBaiDangViPham(maBD);
            DAL_SanPham.Instance.GoSanPhamViPham(maBD);
            DAL_ThongBao.Instance.XoaThongBaoFromMaTB(maTB);

            ThongBao thongBao = new ThongBao
            {
                maTB = BLL_ThongBao.Instance.GetMaMoi(),
                from = "HeThong",
                to = "S" + DAL_Shop.Instance.LoadMaSFromMaBD(maBD),
                dinhKem = "BD" + maBD,
                noiDung = $"Bài Đăng {maBD} của bạn đã được quản trị viên gỡ vi phạm.",
                ngayGui = DateTime.Now,
                tinhTrang = 0
            };
            DAL_ThongBao.Instance.ThemThongBao(thongBao);
        }

        public void XacNhanKhongViPham(string maTB)
        {
            DAL_ThongBao.Instance.XoaThongBaoFromMaTB(maTB);
        }
    }
}
