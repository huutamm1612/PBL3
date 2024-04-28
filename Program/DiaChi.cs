using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class DiaChi
    {
        public string maDC { get; set; }
        public string ten { get; set; }
        public string soDT { get; set; }
        public int maT_TP { get; set; }
        public int maQH { get; set; }
        public int maPX { get; set; }
        public string diaChiCuThe { get; set; }
        public DiaChi()
        {
            maDC = "";
            ten = "";
            soDT = "";
            maT_TP = -1;
            maQH = -1;
            maPX = -1;
            diaChiCuThe = "";
        }
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

        public void setMaDC(string maDC)
        {
            this.maDC = maDC;
        }
        public override bool Equals(object obj)
        {
            return maDC == ((DiaChi)obj).maDC;
        }

        public override string ToString()
        {
            return $"{ten} | {soDT}\r\n{diaChiCuThe}\r\n{HeThong.MoTaDiaChi(maPX)}";
        }
    }
}
