using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class QLDanhGia : IQuanLy
    {
        public List<DanhGia> list;

        public void Add(object item)
        {
            list.Add(item as DanhGia);
        }

        public void Clear()
        {
            list.Clear();
        }

        public int IndexOf(object item)
        {
            for(int i = 0; i < list.Count; i++)
                if(DanhGia.EqualMaDH(item, list[i]))
                    return i;

            return -1;
        }

        public void Remove(object item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void AddRange(params object[] item)
        {
            foreach(DanhGia danhGia in item)
            {
                list.Add(danhGia);
            }
        }

        public void Insert(int index, object item)
        {
            list.Insert(index, item as DanhGia);
        }
    }
}
