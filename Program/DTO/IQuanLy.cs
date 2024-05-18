using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal interface IQuanLy
    {
        void Add(object item);
        int IndexOf(object item);
        void RemoveAt(int index);
        void Remove(object item);
        void Clear();
    }
}
