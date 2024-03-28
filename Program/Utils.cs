using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class Utils
    {
        public delegate bool Compare(object o1, object o2);
        public delegate bool Equal(object o1, object o2);

        public static void RemoveSer<T>(List<T> list, List<string> maSo)
        {
            maSo.Sort();
            int i = 0, j = 0;

            while (j < maSo.Count)
            {
                if (list[i].Equals(maSo[i]))
                {
                    list.Remove(list[i]);
                    j++;
                }
                else
                    i++;
            }

        }

        public static void Sort<T>(List<T> list, int left, int right, Compare cmp, Equal eql)
        {
            if (left >= right)
                return;

            int r = right;
            int l = left;
            object pivot = list[(r + l) / 2];

            while (l < r)
            {
                while (cmp(list[l], pivot)) l++;
                while (!cmp(list[r], pivot) && !eql(list[r], pivot)) r--;
                if (l <= r)
                {
                    (list[r], list[l]) = (list[l], list[r]);
                    l--;
                    r++;
                }
            }

            Sort(list, left, r, cmp, eql);
            Sort(list, l, right, cmp, eql);
        }

        public static void Filter()
        {

        }

        public static bool KiemTraSoDT(string soDT)
        {
            if (soDT.Length != 10 || soDT[0] != '0')
            {
                return false;
            }

            foreach (char i in soDT)
            {
                if (!char.IsDigit(i))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
