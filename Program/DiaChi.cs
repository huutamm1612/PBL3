using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class DiaChi
    {
        public string maDC { get; private set; }
        public string ten { get; private set; }
        public string soDT { get; private set; }
        public int maT_TP { get; private set; }
        public int maQH { get; private set; }
        public int maPX { get; private set; }
        public string diaChiCuThe { get; private set; }
        public DiaChi(string maDC, string ten, string soDT, int maT_TP, int maQH, int maPX, string diaChiCuThe)
        {
            this.maDC = maDC;
            this.ten = ten;
            this.soDT = soDT;
            this.maT_TP = maT_TP;
            this.maQH = maQH;
            this.maPX = maPX;
            this.diaChiCuThe = diaChiCuThe;
        }
        public void capNhat(string ten, string soDT, int maT_TP, int maQH, int maPX, string diaChiCuThe)
        {
            this.ten = ten;
            this.soDT = soDT;
            this.maT_TP = maT_TP;
            this.maQH = maQH;
            this.maPX = maPX;
            this.diaChiCuThe = diaChiCuThe;
        }
        public override bool Equals(object obj)
        {
            return this.maDC == ((DiaChi)obj).maDC;
        }

        public override string ToString()
        {
            return $"{ten} | {soDT}\r\n{diaChiCuThe}\r\n{HeThong.MoTaDiaChi(maPX)}";
        }
    }
}
