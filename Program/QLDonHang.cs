using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    internal class QLDonHang : IQuanLy
    {
        public List<DonHang> list { get; set; }

        public QLDonHang()
        {
            list = new List<DonHang>();
        }

        public void Add(object item)
        {
            foreach(DonHang donHang in list)
            {
                if (DonHang.EqualMaDH(item, donHang))
                    return;
            }
            list.Insert(0, item as DonHang);
        }
        public int IndexOf(object item)
        {
            for (int i = 0; i < list.Count; i++)
                if (DonHang.EqualMaDH(list[i], item))
                    return i;

            return -1;
        }
        public void Remove(object item)
        {
            foreach (var i in list)
                if (i.Equals(item))
                    list.Remove(i);
        }

        public void RemoveAt(int index)
        {
            if (index == -1)
                list.RemoveAt(list.Count - 1);

            list.RemoveAt(index);
        }

        public QLDonHang(QLDonHang donHang)
        {
            list = donHang.list;
        }

        public void AddRange(params DonHang[] listDonHang)
        {
            foreach(DonHang donHang in listDonHang)
            {
                Add(donHang);
            }
        }
    }
}
