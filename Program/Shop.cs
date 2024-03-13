using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class Shop : Nguoi
    {
        public string maS { get; private set; }
        public int nFollower { get; private set; }
        public bool tinhTrang { get; private set; }
        public int doanhThu { get; private set; }

        public Shop() : base()
        {

        }

        public override void nhap(string ten, string email, string soDT, int _, DateTime ngaySinh)
        {
            throw new NotImplementedException();
        }

        public override void sua(string ten, string email, string soDT, int _, DateTime ngaySinh)
        {
            throw new NotImplementedException();
        }
    }
}
