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
        public string avt {  get; set; }
        public DateTime ngaySinh { get; set; }

        public abstract void CapNhatDiaChi(DiaChi diaChiMoi);
        public abstract void Follow(string maSo);
        public abstract void UnFollow(string maSo);
        public abstract void Nhap(string ten, string email, string soDT, int gioiTinh, DateTime ngaySinh);
        public abstract void Sua(string ten, string email, string soDT, int gioiTinh, DateTime ngaySinh);
    }
}
