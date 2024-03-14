using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class Shop : Nguoi
    {
        public int nFollower { get; private set; }
        public int tinhTrang { get; private set; }
        public int doanhThu { get; private set; }

        public Shop() : base()
        {
            nFollower = 0;
            doanhThu = 0;
            tinhTrang = 1;
        }

        public Shop(string maSo, string ten, string soDT, string email, DiaChi diaChi, int _, DateTime ngaySinh, int nFollower, int tinhTrang, int doanhThu) : base(maSo, ten, soDT, email, diaChi, _, ngaySinh)
        {
            this.nFollower = nFollower;
            this.doanhThu = doanhThu;
            this.tinhTrang = tinhTrang;
        }

        public Shop(string maSo) : base(maSo){ }

        public override void nhap(string ten, string email, string soDT, int _, DateTime ngaySinh)
        {
            this.ten = ten;
            this.email = email;
            this.soDT = soDT;
            this.ngaySinh = ngaySinh;
        }

        public override void sua(string ten, string email, string soDT, int _, DateTime ngaySinh)
        {
            this.ten = ten;
            this.email = email;
            this.soDT = soDT;
            this.ngaySinh = ngaySinh;
        }

        public void capNhatTinhTrang(int tinhTrang)
        {
            this.tinhTrang = tinhTrang;
        }
    }
}
