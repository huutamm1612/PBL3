using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public abstract class TaiKhoan
    {
        public string taiKhoan { get; set; }
        public string matKhau { get; set; }
        public abstract void dangNhap(string taiKhoan, string matKhau);
        public abstract void doiMatKhau(string matKhauMoi);
    }
}
