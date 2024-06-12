using Program.DAL;
using Program.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Program.BLL
{
    internal class BLL_KhachHang
    {
        private static BLL_KhachHang _Instance;

        public static BLL_KhachHang Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BLL_KhachHang();
                return _Instance;
            }
            private set { }
        }

        private BLL_KhachHang()
        {

        }

        public void XoaDanhGia(string maDG) => DAL_DanhGia.Instance.XoaDanhGia(maDG);

        public void SuaDanhGia(KhachHang khachHang, DanhGia danhGia)
        {
            khachHang.listDanhGia.list[khachHang.listDanhGia.IndexOf(danhGia)] = danhGia;
            BLL_DanhGia.Instance.SuaDanhGia(danhGia);
        }

        public void BaoCaoDanhGia(string maDG, string lyDo)
        {
            ThongBao thongBao = new ThongBao
            {
                maTB = BLL_ThongBao.Instance.GetMaMoi(),
                from = "AnDanh",
                to = "HeThong",
                dinhKem = "DG" + maDG,
                noiDung = $"Đánh giá của một khách hàng bị báo cáo vi phạm với lý do: {lyDo}",
                ngayGui = DateTime.Now,
                tinhTrang = 0
            };
            DAL_ThongBao.Instance.ThemThongBao(thongBao);
        }

        public void ToCaoBaiDang(string maBD, string lyDo)
        {
            ThongBao thongBao = new ThongBao
            {
                maTB = BLL_ThongBao.Instance.GetMaMoi(),
                from = "AnDanh",
                to = "HeThong",
                dinhKem = "BD" + maBD,
                noiDung = $"Bài đăng BĐ{maBD} được một khách hàng báo cáo vi phạm với lý do: {lyDo}",
                ngayGui = DateTime.Now,
                tinhTrang = 0
            };
            DAL_ThongBao.Instance.ThemThongBao(thongBao);
        }

        public void HuyDonHang(KhachHang khachHang, string maDH, string lyDo)
        {
            khachHang.listDonHang.GetDonHangFromMaDH(maDH).tinhTrang = -1;

            int xu = DAL_DonHang.Instance.LoadXuFromMaDH(maDH);
            khachHang.xu += xu;
            DAL_KhachHang.Instance.HoanTraXu(khachHang.maSo, xu);

            ThongBao thongBao = new ThongBao
            {
                maTB = BLL_ThongBao.Instance.GetMaMoi(),
                from = "KH" + khachHang.maSo,
                to = "S" + DAL_Shop.Instance.LoadMaSFromMaDH(maDH),
                dinhKem = "DH" + maDH,
                noiDung = $"Đơn hàng ĐH{maDH} của bạn đã bị hủy vào lúc {Utils.Instance.MoTaThoiGian(DateTime.Now)} với lý do: {lyDo}",
                ngayGui = DateTime.Now,
                tinhTrang = 0
            };

            DAL_DonHang.Instance.KhachHangHuyHang(maDH, lyDo);
            DAL_ThongBao.Instance.ThemThongBao(thongBao);
        }

        public List<string> GoiYTimKiem(List<string> lichSuTimKiem, string noiDung)
        {
            if (noiDung == "") return lichSuTimKiem;

            List<string> list = new List<string>();

            foreach(string s in lichSuTimKiem)
            {
                try
                {
                    if (noiDung.ToLower().Equals(s.Substring(0, noiDung.Length).ToLower()))
                        list.Add(s);
                }
                catch { }
            }

            List<string> tmp = new List<string>();
            tmp.AddRange(DAL_SanPham.Instance.LoadGoiYTimKiemTheLoai(noiDung).ToArray());
            tmp.AddRange(DAL_SanPham.Instance.LoadGoiYTimKiemSanPham(noiDung).ToArray());
            tmp.AddRange(DAL_SanPham.Instance.LoadGoiYTimKiemTacGia(noiDung).ToArray());
            tmp.AddRange(DAL_BaiDang.Instance.LoadGoiYTimKiemBaiDang(noiDung).ToArray());

            tmp.Sort();

            foreach(string goiY in tmp)
            {
                if(!list.Contains(goiY))
                    list.Add(goiY);

                if (list.Count >= 10)
                    break;
            }

            return list;
        }

        public void LuuLichSuTimKiem(KhachHang khachHang, string noiDung)
        {
            if(khachHang.lichSuTimKiem.Count >= 10)
            {
                DAL_KhachHang.Instance.XoaMotLichSuTimKiem(khachHang.maSo, khachHang.lichSuTimKiem.Last());
                khachHang.lichSuTimKiem.Remove(khachHang.lichSuTimKiem.Last());
            }

            if (khachHang.lichSuTimKiem.Contains(noiDung))
            {
                khachHang.lichSuTimKiem.Remove(noiDung);
                khachHang.lichSuTimKiem.Insert(0, noiDung);

                DAL_KhachHang.Instance.CapNhatLichSuTimKiem(khachHang.maSo, noiDung);
            }
            else
            {
                khachHang.lichSuTimKiem.Insert(0, noiDung);
                DAL_KhachHang.Instance.LuuLichSuTimKiem(khachHang.maSo, noiDung);
            }
        }

        public void XoaLichSuTimKiem(KhachHang khachHang)
        {
            khachHang.lichSuTimKiem.Clear();
            DAL_KhachHang.Instance.XoaTatCaLichSu(khachHang.maSo);
        }

        public void MuaLai(KhachHang khachHang, string maDH)
        {
            foreach(SanPham sanPham in khachHang.listDonHang.GetDonHangFromMaDH(maDH).list)
            {
                if(BLL_SanPham.Instance.GetSoLuongFromMaSP(sanPham.maSP) != 0)
                    khachHang.gioHang.Add(sanPham);
            }
        }

        public void DanhGia(KhachHang khachHang, DanhGia danhGia, string maDH)
        {
            khachHang.listDanhGia.Add(danhGia);
            khachHang.listDonHang.GetDonHangFromMaDH(maDH).tinhTrang = 3;
            khachHang.xu += 200;
            danhGia.maDG = BLL_DanhGia.Instance.GetMaMoi();

            ThongBao thongBao1 = new ThongBao
            {
                maTB = BLL_ThongBao.Instance.GetMaMoi(),
                from = "KH" + khachHang.maSo,
                to = "S" + DAL_Shop.Instance.LoadMaSFromMaBD(danhGia.maBD),
                dinhKem = "BD" + danhGia.maBD,
                noiDung = $"Bài đăng BD{danhGia.maBD} của bạn đã được khách hàng {khachHang.taiKhoan} đánh giá {danhGia.sao} sao vào lúc {Utils.Instance.MoTaThoiGian(danhGia.ngayThem)}.",
                ngayGui = DateTime.Now,
                tinhTrang = 0
            };

            ThongBao thongBao2 = new ThongBao
            {
                maTB = BLL_ThongBao.Instance.GetMaMoi(),
                from = "HeThongXu",
                to = "KH" + khachHang.maSo,
                noiDung = $"Chúc mừng bạn đã nhận được 200 xu từ việc đánh giá sản phẩm.",
                ngayGui = DateTime.Now,
                tinhTrang = 0
            };

            khachHang.listThongBao.Add(thongBao2);
            DAL_DanhGia.Instance.ThemDanhGia(danhGia);

            DAL_KhachHang.Instance.ThemXu(khachHang.maSo, 200);
            DAL_DonHang.Instance.CapNhatTinhTrangDonHang(maDH, 3, khachHang.listDonHang.GetDonHangFromMaDH(maDH).ngayGiaoHang);
            DAL_ThongBao.Instance.ThemThongBao(thongBao1);
            DAL_ThongBao.Instance.ThemThongBao(thongBao2);
        }

        public void ThemDiaChi(DiaChi diaChi, string maKH) => DAL_DiaChi.Instance.ThemDiaChi(diaChi, maKH, 1);

        public KhachHang GetKhachHangFromTaiKhoan(string taiKhoan) => DAL_KhachHang.Instance.LoadKhachHangFromTaiKhoan(taiKhoan);
            
        public void Thich(KhachHang khachHang, BaiDang baiDang)
        {
            DAL_KhachHang.Instance.ThemThich(khachHang.maSo, baiDang.maBD);
            DAL_BaiDang.Instance.Thich(baiDang.maBD);
            baiDang.luocThich += 1;
            khachHang.Thich(baiDang.maBD);
        }

        public void XemBaiDang(KhachHang khachHang, string maBD)
        {
            if (khachHang.listDaXem.Contains(maBD)){
                return;
            }

            khachHang.listDaXem.Insert(0, maBD);
            DAL_KhachHang.Instance.ThemDaXem(khachHang.maSo, maBD);
            if (khachHang.listDaXem.Count == 20)
            {
                DAL_KhachHang.Instance.XoaDaXem(khachHang.maSo, khachHang.listDaXem.Last());
                khachHang.listDaXem.Remove(khachHang.listDaXem.Last());
            }
        }

        public void HuyThich(KhachHang khachHang, BaiDang baiDang)
        {
            DAL_KhachHang.Instance.XoaThich(khachHang.maSo, baiDang.maBD);
            DAL_BaiDang.Instance.HuyThich(baiDang.maBD);
            baiDang.luocThich -= 1;
            khachHang.HuyThich(baiDang.maBD);
        }

        public void TheoDoi(KhachHang khachHang, Shop shop)
        {
            DAL_KhachHang.Instance.ThemFollow(khachHang.maSo, shop.maSo);
            shop.Follow(khachHang.maSo);
            khachHang.Follow(shop.maSo);
        }

        public void HuyTheoDoi(KhachHang khachHang, Shop shop)
        {
            DAL_KhachHang.Instance.XoaFollow(khachHang.maSo, shop.maSo);
            shop.UnFollow(khachHang.maSo);
            khachHang.UnFollow(shop.maSo);
        }

        public void DatHang(KhachHang khachHang, QLSanPham listSanPham, DiaChi diaChi, int ptThanhToan, bool dungXu)
        {
            int soLuongShop = listSanPham.SoLuongShop();
            int xu = 0;
            if (dungXu)
            {
                int xuConLai = Math.Max(0, khachHang.xu - (listSanPham.tinhTongTien() + soLuongShop * 30000));
                xu = khachHang.xu - xuConLai;
                khachHang.xu = xuConLai;
                DAL_KhachHang.Instance.DungXu(khachHang.maSo, xuConLai);    
            }

            foreach(DonHang donHang in BLL_DonHang.Instance.PhanRaDonHang(listSanPham))
            {
                string maS = donHang.list[0].maS;

                donHang.maDH = BLL_DonHang.Instance.GetMaMoi();
                donHang.maKH = khachHang.maSo;
                donHang.ptThanhToan = ptThanhToan;

                if(donHang.tongTien <= xu / soLuongShop)
                {
                    donHang.xu = donHang.tongTien;
                    donHang.tongTien = 0;
                    xu -= donHang.tongTien;
                    soLuongShop--;
                }
                else
                {
                    donHang.xu = xu / soLuongShop;
                    donHang.tongTien -= donHang.xu;
                }

                donHang.maS = maS;
                donHang.diaChi = diaChi;

                khachHang.listDonHang.Add(donHang);
                BLL_DonHang.Instance.DatHang(donHang);

                ThongBao thongBao = new ThongBao
                {
                    maTB = BLL_ThongBao.Instance.GetMaMoi(),
                    from = "KH" + khachHang.maSo,
                    to = "S" + maS,
                    dinhKem = "DH" + donHang.maDH,
                    noiDung = $"Khách hàng {khachHang.taiKhoan} đã đặt đơn hàng DH{donHang.maDH} vào lúc {Utils.Instance.MoTaThoiGian(DateTime.Now)}",
                    ngayGui = DateTime.Now,
                    tinhTrang = 0
                };

                BLL_ThongBao.Instance.ThemThongBao(thongBao);
            }

            BLL_GioHang.Instance.XoaSPKhoiGioHang(khachHang.gioHang, listSanPham.ToArray());
        }

        public void NhanHang(KhachHang khachHang, string maDH)
        {
            int index = khachHang.listDonHang.IndexOf(maDH);
            khachHang.listDonHang.list[index].ngayGiaoHang = DateTime.Now;
            khachHang.listDonHang.list[index].tinhTrang = 2;

            ThongBao thongBao = new ThongBao
            {
                maTB = BLL_ThongBao.Instance.GetMaMoi(),
                from = "KH" + khachHang.maSo,
                to = "S" + khachHang.listDonHang.list[index].maS,
                dinhKem = "DH" + khachHang.listDonHang.list[index].maDH,
                noiDung = $"Đơn hàng DH{khachHang.listDonHang.list[index].maDH} đã được giao thành công vào lúc {Utils.Instance.MoTaThoiGian(DateTime.Now)}. Doanh thu của bạn đã tăng thêm ₫{Utils.Instance.SetGia(khachHang.listDonHang.list[index].tongTien + khachHang.listDonHang.list[index].xu)}.",
                ngayGui = DateTime.Now,
                tinhTrang = 0
            };

            foreach(SanPham sanPham in khachHang.listDonHang.list[index].list)
            {
                DAL_SanPham.Instance.NhanHang(sanPham.maSP);
            }

            DAL_ThongBao.Instance.ThemThongBao(thongBao);
            DAL_DonHang.Instance.CapNhatTinhTrangDonHang(khachHang.listDonHang.list[index].maDH, khachHang.listDonHang.list[index].tinhTrang, khachHang.listDonHang.list[index].ngayGiaoHang);
            DAL_KhachHang.Instance.NhanHang(khachHang.maSo, khachHang.listDonHang.list[index].tongTien);
            DAL_Shop.Instance.GiaoHangThanhCong(khachHang.listDonHang.list[index].maS, khachHang.listDonHang.list[index].tongTien + khachHang.listDonHang.list[index].xu);
        }

        public void CapNhatDiaChiMacDinh(KhachHang khachHang, DiaChi diaChi)
        {
            khachHang.thayDoiDiaChiMacDinh(diaChi);
            DAL_DiaChi.Instance.CapNhatDiaChiMacDinh(khachHang.maSo, diaChi.maDC);
        }

        public void XoaDiaChi(KhachHang khachHang, DiaChi diaChi)
        {
            BLL_DiaChi.Instance.XoaDiaChi(diaChi.maDC);
            khachHang.xoaDiaChi(diaChi);
        }

        public string GetURLFromMaKH(string maKH) => DAL_KhachHang.Instance.LoadURLFromMaKH(maKH);

        public string GetTenFromMaKH(string maKH) => DAL_KhachHang.Instance.LoadTenFromMaKH(maKH);

        public bool DaTheoDoi(List<string> listFollow, string maS) => listFollow.Contains(maS);

        public bool DaThich(List<string> listThich, string maBD) => listThich.Contains(maBD);

        public bool KiemTraTaoShop(string maKH) => DAL_KhachHang.Instance.KiemTraKhachHang_Shop(maKH);
    }
}
