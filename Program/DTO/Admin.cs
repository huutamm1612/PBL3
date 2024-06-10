using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.DTO
{
    public class Admin : TaiKhoan
    {
        public Admin()
        {
            taiKhoan = "";
            matKhau = "";
        }

        public List<SqlParameter> GetParameters()
        {
            return new List<SqlParameter> 
            {
                new SqlParameter("@taiKhoan", this.taiKhoan),
                new SqlParameter("@matKhau", this.matKhau)
            };
        }
        
        public override void dangNhap(string taiKhoan, string matKhau)
        {
            this.taiKhoan = taiKhoan;
            this.matKhau = matKhau;
        }

        public override void doiMatKhau(string matKhauMoi)
        {
            this.matKhau = matKhauMoi;
        }
    }
}
