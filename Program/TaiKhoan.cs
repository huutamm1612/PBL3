using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal abstract class TaiKhoan
    {
        public string taiKhoan { get; protected set; }
        public string matKhau { get; protected set; }
        public abstract void dangNhap(string taiKhoan, string matKhau);
        public abstract void doiMatKhau(string matKhauMoi);
    }
}
