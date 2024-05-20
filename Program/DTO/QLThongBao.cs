using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.DTO
{
    internal class QLThongBao : IQuanLy
    {
        public List<ThongBao> list { get; set; }

        public QLThongBao()
        {
            list = new List<ThongBao>();
        }

        public void Add(object item)
        {
            foreach (ThongBao thongBao in list)
            {
                if (ThongBao.EqualMaTB(item, thongBao))
                    return;
            }
            list.Insert(0, item as ThongBao);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object item)
        {
            throw new NotImplementedException();
        }

        public void Remove(object item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
    }
}
