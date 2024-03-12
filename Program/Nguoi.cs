using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal abstract class Nguoi
    {
        public string maSo { get; protected set; }
        public string ten { get; protected set; }
        public string soDT { get; protected set; }
        public string email { get; protected set; }
        public DiaChi diaChi { get; protected set; }
        public int gioiTinh { get; protected set; }
        public DateTime ngaySinh { get; set; }

        public Nguoi()
        {
            this.maSo = "";
            this.ten = "";
            this.soDT = "";
            this.email = "";
            this.diaChi = null;
            this.gioiTinh = 2;
            this.ngaySinh = new DateTime();
        }

        public Nguoi(string maSo)
        {
            this.maSo = maSo;
            this.ten = "";
            this.soDT = "";
            this.email = "";
            this.diaChi = null;
            this.gioiTinh = 2;
            this.ngaySinh = new DateTime(1899, 10, 10);
        }

        public Nguoi(string maSo, string ten, string soDT, string email, DiaChi diaChi, int gioiTinh, DateTime ngaySinh)
        {
            this.maSo = maSo;
            this.ten = ten;
            this.soDT = soDT;
            this.email = email;
            this.diaChi = diaChi;
            this.gioiTinh = gioiTinh;
            this.ngaySinh = ngaySinh;
        }

        public Nguoi(Nguoi nguoi)
        {
            this.maSo = nguoi.maSo;
            this.ten = nguoi.ten;
            this.soDT = nguoi.soDT;
            this.email = nguoi.email;
            this.diaChi = nguoi.diaChi;
            this.ngaySinh = nguoi.ngaySinh;
        }

        public abstract void nhap(string ten, string email, string soDT, int gioiTinh, DateTime ngaySinh);
        public abstract void sua(string ten, string email, string soDT, int gioiTinh, DateTime ngaySinh);
    }
}
