using Program.DAL;
using Program.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.BLL
{
    internal class BLL_User
    {
        private static BLL_User _Instance;
        public static BLL_User Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BLL_User();
                return _Instance;
            }
            private set { }
        }
        private BLL_User()
        {

        }

        public User DangNhapBangCache()
        {
            List<string> list = DAL_User.Instance.ReadAccountCache();
            if (list == null)
                return null;

            return new User
            {
                taiKhoan = list[0],
                matKhau = list[1]
            };
        }

        public User DangNhap(string taiKhoan, string matKhau)
        {
            return DAL_User.Instance.DangNhap(taiKhoan, matKhau);
        }

        public void LuuTaiKhoan(User user)
        {
            DAL_User.Instance.WriteAccoutCache(user);
        }

        public void ClearCache()
        {
            DAL_User.Instance.ClearAccountCache();
        }

        public bool KiemTraCauHoi(string taiKhoan, int maCH, string cauTraLoi)
        {
            if (maCH != DAL_User.Instance.LoadMaCHFromTaiKhoan(taiKhoan))
                return false;
            if (cauTraLoi != DAL_User.Instance.LoadCauTraLoiFromTaiKhoan(taiKhoan))
                return false;
            return true;
        }

        public List<CBBItem> GetAllCauHoi()
        {
            List<CBBItem> list = new List<CBBItem>();
            foreach(int maCH in DAL_User.Instance.LoadAllMaCH())
            {
                list.Add(new CBBItem { Value = 1, Text = DAL_User.Instance.LoadCauHoiFromMaCH(maCH)});
            }
            return list;
        }

        public void DangKy(string taiKhoan, string matKhau, int maCH, string cauTraLoi)
        {
            DAL_User.Instance.DangKy(taiKhoan, matKhau, maCH, cauTraLoi);
            DAL_KhachHang.Instance.TaoKhachHangMoiBangTaiKhoan(taiKhoan);
        }
        
        public void DoiMatKhau(string taiKhoan, string matKhau)
        {
            DAL_User.Instance.DoiMatKhau(taiKhoan, matKhau);
        }

        public void DoiMatKhau(User user, string matKhauMoi)
        {
            user.doiMatKhau(matKhauMoi);
            DAL_User.Instance.DoiMatKhau(user.taiKhoan, user.matKhau);
        }

        public bool KiemTraTaiKhoan(string taiKhoan)
        {
            return !DAL_User.Instance.TaiKhoanDaTonTai(taiKhoan);
        }

    }
}
