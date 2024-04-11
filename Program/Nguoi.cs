using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal abstract class Nguoi
    {
        public string maSo { get; set; }
        public string ten { get; set; }
        public string soDT { get; set; }
        public string email { get; set; }
        public DiaChi diaChi { get; set; }
        public int gioiTinh { get; set; }
        public DateTime ngaySinh { get; set; }

        public void capNhatDiaChi(DiaChi diaChiMoi)
        {
            this.diaChi = diaChiMoi;
        }
        public abstract void nhap(string ten, string email, string soDT, int gioiTinh, DateTime ngaySinh);
        public abstract void sua(string ten, string email, string soDT, int gioiTinh, DateTime ngaySinh);
    }
}
