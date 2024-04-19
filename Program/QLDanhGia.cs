﻿using System;
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
                list.Insert(0, danhGia);
            }
        }

        public void Insert(int index, object item)
        {
            list.Insert(index, item as DanhGia);
        }

        public double tinhSao()
        {
            double total = 0.0;
            foreach(DanhGia danhGia in list)
            {
                total += danhGia.sao;
            }

            return total / list.Count;
        }
    }
}