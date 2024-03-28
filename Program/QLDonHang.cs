using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    internal class QLDonHang
    {
        public List<DonHang> list { get; set; }

        public QLDonHang()
        {
            list = new List<DonHang>();
        }

        public QLDonHang(QLDonHang donHang)
        {
            list = donHang.list;
        }

        public void Add(DonHang donHang)
        {
            list.Insert(0, donHang);
        }
    }
}
