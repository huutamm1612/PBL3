CREATE DATABASE PBL3_Database
USE PBL3_Database

CREATE TABLE Tinh_ThanhPho (
	maT_TP int PRIMARY KEY,
	ten nvarchar(50)
)

CREATE TABLE Quan_Huyen (
	maQH int PRIMARY KEY,
	maT_TP int,
	ten nvarchar(50)
	CONSTRAINT FK_Quan_Huyen_Tinh_ThanhPho FOREIGN KEY (maT_TP) REFERENCES Tinh_ThanhPho(maT_TP)
)

CREATE TABLE Phuong_Xa (
	maPX int PRIMARY KEY,
	maQH int,
	ten nvarchar(50)
	CONSTRAINT FK_Phuong_Xa_Quan_Huyen FOREIGN KEY (maQH) REFERENCES Quan_Huyen(maQH)
)

CREATE TABLE CauHoi(
	maCH int PRIMARY KEY,
	cauHoi nvarchar(50)
)

CREATE TABLE UserAccount(
	taiKhoan varchar(50) PRIMARY KEY,
	matKhau varchar(50),
	maCH int,
	cauTraLoi nvarchar(50),
	biKhoa bit DEFAULT 0,

	CONSTRAINT FK_UserAccount_CauHoi FOREIGN KEY (maCH) REFERENCES CauHoi(maCH)
)

CREATE TABLE AdminAccount(
	taiKhoan varchar(50) PRIMARY KEY,
	matKhau varchar(50)
)

INSERT INTO AdminAccount VALUES('admin1', 'Admin@123')

CREATE TABLE DiaChi (
	maDC varchar(10) PRIMARY KEY,
	maSo varchar(10),
	ten nvarchar(50),
	soDT varchar(10),
	maT_TP int,
	maQH int,
	maPX int,
	diaChiCuThe nvarchar(50),
	diaChiKH bit default 1

	CONSTRAINT FK_DiaChi_Tinh_ThanhPho FOREIGN KEY (maT_TP) REFERENCES Tinh_ThanhPho(maT_TP),
	CONSTRAINT FK_DiaChi_Quan_Huyen FOREIGN KEY (maQH) REFERENCES Quan_Huyen(maQH),
	CONSTRAINT FK_DiaChi_Phuong_Xa FOREIGN KEY (maPX) REFERENCES Phuong_Xa(maPX),
)

CREATE TABLE KhachHang (
	maKH varchar(10) PRIMARY KEY,
	taiKhoan varchar(50),
	ten nvarchar(50) NULL,
	soDT varchar(10) NULL,
	email varchar(50) NULL,
	maDC varchar(10) NULL,
	gioiTinh int NULL,
	ngaySinh date NULL,
	xu int DEFAULT 0,
	chiTieu int DEFAULT 0,
	avt varchar(255),
	
	CONSTRAINT FK_KhachHang_DiaChi FOREIGN KEY (maDC) REFERENCES DiaChi(maDC),
	CONSTRAINT FK_KhachHang_UserAccount FOREIGN KEY (taiKhoan) REFERENCES UserAccount(taiKhoan)
)

CREATE TABLE Shop (	
	maS varchar(10) PRIMARY KEY,
	ten nvarchar(50) COLLATE Vietnamese_CI_AI,
	soDT varchar(10),
	email varchar(50),
	maDC varchar(10),
	ngayTao date,
	tinhTrang int default 1,
	doanhThu int default 0,
	sao float(1) default 0.0,
	avt varchar(255),

	CONSTRAINT FK_Shop_DiaChi FOREIGN KEY (maDC) REFERENCES DiaChi(maDC)
)

CREATE TABLE Follow(
	maKH varchar(10),
	maS varchar(10),
	PRIMARY KEY(maKH, maS),

	CONSTRAINT FK_Follow_KhachHang FOREIGN KEY (maKH) REFERENCES KhachHang(maKH),
	CONSTRAINT FK_Follow_Shop FOREIGN KEY (maS) REFERENCES Shop(maS)
)

CREATE TABLE MaHienTai(
	maKH varchar(10),
	maS varchar(10),
	maDC varchar(10),
	maLoaiSP varchar(10),
	maSP varchar(10),
	maBD varchar(10),
	maDH varchar(10),
	maDG varchar(10),
	maTB varchar(10)
)

INSERT INTO MaHienTai VAlUES ('0000000000','0000000000','0000000000', '0000000000','0000000000','0000000000','0000000000','0000000000','0000000000')

CREATE TABLE KhachHang_Shop (
	maKH varchar(10),
	maS varchar(10),
	PRIMARY KEY (maKH, maS),

	CONSTRAINT FK_KhachHang_Shop_KhachHang FOREIGN KEY (maKH) REFERENCES KhachHang(maKH),
	CONSTRAINT FK_KhachHang_Shop_Shop FOREIGN KEY (maS) REFERENCES Shop(maS)
)

CREATE TABLE LoaiSanPham (
	maLoaiSP varchar(10) PRIMARY KEY COLLATE Vietnamese_CI_AI,
	tenLoaiSP nvarchar(50)
)

CREATE TABLE SanPham(
	maSP varchar(10) PRIMARY KEY,
	maLoaiSP varchar(10),
	ten nvarchar(100) COLLATE Vietnamese_CI_AI,
	gia int,
	soLuong int,
	tacGia nvarchar(50),
	dichGia nvarchar(50),
	ngonNgu nvarchar(20),
	soTrang int,
	namXuatBan int,
	nhaXuatBan nvarchar(50),
	loaiBia nvarchar(10),
	moTa nvarchar(500),
	luocBan int DEFAULT 0,
	anh varchar(255) NULL,

	CONSTRAINT FK_SanPham_LoaiSanPham FOREIGN KEY (maLoaiSP) REFERENCES LoaiSanPham(maLoaiSP)
)

CREATE TABLE BaiDang(
	maBD varchar(10) PRIMARY KEY,
	tieuDe nvarchar(120) COLLATE Vietnamese_CI_AI,
	moTa nvarchar(1000),
	luocThich int DEFAULT 0,
	giamGia int DEFAULT 0,
	anh varchar(255) null
)

CREATE TABLE BaiDang_Anh(
	maBD varchar(10),
	anh varchar(255),
	PRIMARY KEY (maBD, anh),

	CONSTRAINT FK_BaiDang_Anh_BaiDang FOREIGN KEY (maBD) REFERENCES BaiDang(maBD)
)

CREATE TABLE Thich(
	maKH varchar(10),
	maBD varchar(10),
	PRIMARY KEY (maKH, maBD),


	CONSTRAINT FK_Thich_KhachHang FOREIGN KEY (maKH) REFERENCES KhachHang(maKH),
	CONSTRAINT FK_Thich_BaiDang FOREIGN KEY (maBD) REFERENCES BaiDang(maBD)
)

CREATE TABLE SanPhamDaXoa(
	maSP varchar(10) PRIMARY KEY,
	maS varchar(10)

	CONSTRAINT FK_SanPhamDaXoa_SanPham FOREIGN KEY (maSP) REFERENCES SanPham(maSP),
	CONSTRAINT FK_SanPhamDaXoa_Shop FOREIGN KEY (maS) REFERENCES Shop(maS)
)

CREATE TABLE DaXemGanDay(
	maBD varchar(10),
	maKH varchar(10),
	PRIMARY KEY (maBD, maKH),

	CONSTRAINT FK_DaXemGanDay_BaiDang FOREIGN KEY (maBD) REFERENCES BaiDang(maBD),
	CONSTRAINT FK_DaXemGanDay_KhachHang FOREIGN KEY (maKH) REFERENCES KhachHang(maKH)
)

CREATE TABLE SanPham_BaiDang(
	maSP varchar(10),
	maBD varchar(10),
	PRIMARY KEY (maSP, maBD),

	CONSTRAINT FK_SanPham_BaiDang_SanPham FOREIGN KEY (maSP) REFERENCES SanPham(maSP),
	CONSTRAINT FK_SanPham_BaiDang_BaiDang FOREIGN KEY (maBD) REFERENCES BaiDang(maBD)
)

CREATE TABLE BaiDang_Shop(
	maBD varchar(10),
	maS varchar(10),
	PRIMARY KEY (maBD, maS),

	CONSTRAINT FK_BaiDang_Shop_BaiDang FOREIGN KEY (maBD) REFERENCES BaiDang(maBD),
	CONSTRAINT FK_BaiDang_Shop_Shop FOREIGN KEY (maS) REFERENCES Shop(maS)
)

CREATE TABLE DonHang(
	maDH varchar(10) PRIMARY KEY,
	maDC varchar(10),
	tongTien int,
	tinhTrang int,
	ptThanhToan int,
	xu int,
	ngayDatHang date,
	ngayGiaoHang date,

	CONSTRAINT FK_DonHang_DiaChi FOREIGN KEY (maDC) REFERENCES DiaChi(maDC)
)

CREATE TABLE DonHang_SanPham(
	maDH varchar(10),
	maSP varchar(10),
	soLuong int,
	PRIMARY KEY(maDH, maSP),
	
	CONSTRAINT FK_DonHang_SanPham_DonHang FOREIGN KEY (maDH) REFERENCES DonHang(maDH),
	CONSTRAINT FK_DonHang_SanPham_SanPham FOREIGN KEY (maSP) REFERENCES SanPham(maSP)
)

CREATE TABLE DonHang_KhachHang(
	maDH varchar(10),
	maKH varchar(10),
	PRIMARY KEY (maKH, maDH),

	CONSTRAINT FK_DonHang_KhachHang_DonHang FOREIGN KEY (maDH) REFERENCES DonHang(maDH),
	CONSTRAINT FK_DonHang_KhachHang_KhachHang FOREIGN KEY (maKH) REFERENCES KhachHang(maKH),
)


CREATE TABLE DonHang_Shop(
	maDH varchar(10),
	maS varchar(10),
	PRIMARY KEY (maS, maDH),

	CONSTRAINT FK_DonHang_Shop_DonHang FOREIGN KEY (maDH) REFERENCES DonHang(maDH),
	CONSTRAINT FK_DonHang_Shop_Shop FOREIGN KEY (maS) REFERENCES Shop(maS),
)

CREATE TABLE GioHang(
	maKH varchar(10),
	maSP varchar(10),
	soLuong int,
	ngayThem date,
	PRIMARY KEY (maKH, maSP),	
	
	CONSTRAINT FK_GioHang_KhachHang FOREIGN KEY (maKH) REFERENCES KhachHang(maKH),
	CONSTRAINT FK_GioHang_SanPham FOREIGN KEY (maSP) REFERENCES SanPham(maSP)
)

CREATE TABLE DanhGia(
	maDG varchar(10),
	sanPhamDaMua nvarchar(100),
	doiTuong nvarchar(50),
	thietKeBia nvarchar(50),
	noiDung nvarchar(200),
	sao int,
	luocThich int,
	ngayThem date,

	PRIMARY KEY(maDG)
)

CREATE TABLE DanhGia_KhachHang(
	maKH varchar(10),
	maDG varchar(10),
	PRIMARY KEY (maKH, maDG),

	CONSTRAINT FK_DanhGia_KhachHang_KhachHang FOREIGN KEY (maKH) REFERENCES KhachHang(maKH),
	CONSTRAINT FK_DanhGia_KhachHang_DanhGia FOREIGN KEY (maDG) REFERENCES DanhGia(maDG)
)

CREATE TABLE DanhGia_BaiDang(
	maDG varchar(10),
	maBD varchar(10),
	PRIMARY KEY (maBD, maDG),

	CONSTRAINT FK_DanhGia_BaiDang_BaiDang FOREIGN KEY (maBD) REFERENCES BaiDang(maBD),
	CONSTRAINT FK_DanhGia_BaiDang_DanhGia FOREIGN KEY (maDG) REFERENCES DanhGia(maDG)
)

CREATE TABLE ThongBao
(
	maTB varchar(10) PRIMARY KEY,
	_From varchar(12),
	_To varchar(12),
	dinhKem varchar(12) null,
	noiDung nvarchar(500),
	tinhTrang int,
	ngayGui datetime
)

CREATE TABLE LichSuTimKiem
(
	maKH varchar(10),
	noiDung nvarchar(120),
	ngayTim datetime,
	
	CONSTRAINT FK_LichSuTimKiem_KhachHang FOREIGN KEY (maKH) REFERENCES KhachHang(maKH),
)

CREATE TABLE LyDo
(
	loaiLyDo int,
	noiDung nvarchar(120),
)

CREATE TABLE DonHangBiHuy
(
	maDH varchar(10) PRIMARY KEY,
	lyDo nvarchar(120),
	isKhachHang bit,
	ngayHuy datetime,

	CONSTRAINT FK_DonHangBiHuy_DonHang FOREIGN KEY (maDH) REFERENCES DonHang(maDH),
)

CREATE TABLE BaiDangViPham
(
	maBD varchar(10) PRIMARY KEY,
	lyDo nvarchar(120),

	CONSTRAINT FK_BaiDangViPham_BaiDang FOREIGN KEY (maBD) REFERENCES BaiDang(maBD)
)