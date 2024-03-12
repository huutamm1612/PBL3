using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class User : TaiKhoan
    {
        public bool biKhoa { get; private set; } = false;
        public override void dangNhap(string taiKhoan, string matKhau)
        {
            this.taiKhoan = taiKhoan;
            this.matKhau = matKhau;
        }
        public override void doiMatKhau(string matKhauMoi)
        {
            this.matKhau = matKhauMoi;
        }
        public void dangKy(string taiKhoan, string matKhau)
        {
            this.taiKhoan = taiKhoan;
            this.matKhau = matKhau;
        }

        public bool kiemTraMatKhau(string matKhau)
        {
            return this.matKhau == matKhau;
        }
    }
}
