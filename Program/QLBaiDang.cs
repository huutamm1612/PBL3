using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class QLBaiDang
    {
        public List<BaiDang> list { get; set; }

        public QLBaiDang()
        {
            list = new List<BaiDang>();
        }

        public void Add(BaiDang baiDang)
        {
            foreach(BaiDang item in list)
            {
                if(BaiDang.EqualMaBD(item, baiDang))
                {
                    return;
                }
            }
            list.Add(baiDang);
        }

        public void Add(int index, SanPham sanPham)
        {
            if(index == -1)
                list.Last().Add(sanPham);
            else
                list[index].Add(sanPham);
        }
    }
}
