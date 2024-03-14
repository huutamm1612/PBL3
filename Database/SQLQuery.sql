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
	nFollow int DEFAULT 0,
	xu int DEFAULT 0,
	chiTieu int DEFAULT 0,
	avt varchar(255)
	
	CONSTRAINT FK_KhachHang_DiaChi FOREIGN KEY (maDC) REFERENCES DiaChi(maDC),
)

CREATE TABLE Shop (	
	maS varchar(10) PRIMARY KEY,
	ten nvarchar(50),
	soDT varchar(10),
	email varchar(50),
	maDC varchar(10),
	nFollower int default 0,
	ngayTao date,
	tinhTrang bit default 1,
	doanhThu int default 0,
	sao real default 0.0,
	avt varchar(255)

	CONSTRAINT FK_Shop_DiaChi FOREIGN KEY (maDC) REFERENCES DiaChi(maDC)
)

ALTER TABLE DiaChi
DROP CONSTRAINT FK_DiaChi_Shop

CREATE TABLE MaHienTai(
	maKH varchar(10),
	maS varchar(10),
	maDC varchar(10),
)

INSERT INTO MaHienTai VAlUES ('0000000000','0000000000','0000000000')

CREATE TABLE KhachHang_Shop (
	maKH varchar(10),
	maS varchar(10),
	PRIMARY KEY (maKH, maS),

	CONSTRAINT FK_KhachHang_Shop_KhachHang FOREIGN KEY (maKH) REFERENCES KhachHang(maKH),
	CONSTRAINT FK_KhachHang_Shop_Shop FOREIGN KEY (maS) REFERENCES Shop(maS)
)
