using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Program.DAL;
using Program.GUI;

namespace Program
{
    internal class Utils
    {
        private static Utils _Instance;
        public static Utils Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new Utils();
                return _Instance;
            }
            private set { }
        }
        private Utils()
        {

        }
        public delegate bool Compare(object o1, object o2);
        public delegate bool Equal(object o1, object o2);

        public string MoTaThoiGian(DateTime dateTime)
        {
            return $"{dateTime: HH}:{dateTime: mm}, ngày{dateTime: dd} tháng{dateTime: MM} năm{dateTime: yyyy}";
        }

        public void RemoveRange<T>(List<T> list, List<string> maSo)
        {
            maSo.Sort();
            int i = 0, j = 0;

            while (j < maSo.Count && i < list.Count)
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

        public string GetImageURL(Image image)
        {
            int maAnh = int.Parse(Database.Instance.MaMoi("maAnh"));
            string url = SetPath() + $"img{maAnh}.png";

            int min = Math.Min(image.Width, image.Height);

            Bitmap bitmap = new Bitmap(GUI_Utils.Instance.Resize(image, new Size(min, min)));
            bitmap.Save(url, System.Drawing.Imaging.ImageFormat.Png);

            return url;
        }

        public string SetPath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Substring(0, path.Length - ("Program\\bin\\Debug\\").Length) + "Images\\";
            return path;
        }

        public int GiamGia(int gia, int giam)
        {
            return (int)(gia - gia * (giam / 100.0));
        }

        public static void BSearch<T>(List<T> list, Compare cmp, Equal eql)
        {

        }

        public string SetGia(int gia)
        {
            string s = "";
            while (gia / 1000 != 0)
            {
                s = "." + (gia % 1000).ToString("D3") + s;
                gia /= 1000;
            }

            s = gia.ToString() + s;

            return s;
        }

        public void Sort<T>(List<T> list, int left, int right, Compare cmp, Equal eql)
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
                    l++;
                    r--;
                }
            }

            Sort(list, left, r, cmp, eql);
            Sort(list, l, right, cmp, eql);
        }

        public List<string> GetListPage(int currPageNumber, int numPage)
        {
            List<string> list;

            if (currPageNumber <= 3)
            {
                list = new List<string> { "1", "2", "3", "4", "..." };
            }
            else if (currPageNumber < numPage - 1)
            {
                list = new List<string> { "1", "...", (currPageNumber - 1).ToString(), (currPageNumber).ToString(), (currPageNumber + 1).ToString(), "..." };
            }
            else if (currPageNumber == numPage - 1)
            {
                list = new List<string> { "1", "...", (currPageNumber - 1).ToString(), (currPageNumber).ToString(), (currPageNumber + 1).ToString() };
            }
            else
            {
                list = new List<string> { "1", "...", (currPageNumber - 1).ToString(), (currPageNumber).ToString() };
            }

            return list;
        }

        public bool KiemTraSoDT(string soDT)
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
