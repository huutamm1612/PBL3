INSERT INTO UserAccount VALUES('useraccount1', 'useraccount', 1, N'None', 0)
INSERT INTO KhachHang VALUES('1000000000', 'useraccount1', N'Default Name', '0123456789', 'email@gmail.com', null, null, null, 0, 0, null)
INSERT INTO DiaChi VALUES('1000000000', '1000000000', N'useraccount', '0123456678', 15, 1505, 150503, '17 Đồng Kè', 0)
INSERT INTO Shop VALUES('1000000000', N'AZ Việt Nam', '0123456789', 'az@gmail.com', '1000000000', '2024/04/24', 1, 0, 0, null)
INSERT INTO KhachHang_Shop VALUES('1000000000', '1000000000')
INSERT INTO BaiDang	VALUES		('1000000000', N'Sách - Cái Chết Của Những Xác Sống', '', 0, 20, null)
INSERT INTO BaiDang_Shop VALUES	('1000000000', '1000000000')
INSERT INTO SanPham VALUES			('1000000000', '0000000002', N'Tập 1', 139000, 100, N'Masaya Yamaguchi ', N'Trịnh Thanh Tâm', N'Tiếng Việt', 376, 2023, N'AZ Việt Nam', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000000', '1000000000')
INSERT INTO SanPham VALUES			('1000000001', '0000000002', N'Tập 2', 119000, 100, N'Masaya Yamaguchi ', N'Võ Phương Ngân', N'Tiếng Việt', 320, 2023, N'AZ Việt Nam', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000001', '1000000000')

UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img0.png' WHERE maBD = '1000000000'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img1.png' WHERE maSP = '1000000000'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img2.png' WHERE maSP = '1000000001'

INSERT INTO BaiDang	VALUES		('1000000001', N'Combo 2 Cuốn: Thế Giới Bên Trong Cái Ác + Hai Mươi Dáng Hình Tội Ác', '', 0, 18, null)
INSERT INTO BaiDang_Shop VALUES	('1000000001', '1000000000')
INSERT INTO SanPham VALUES			('1000000002', '0000000002', N'Thế Giới Bên Trong Cái Ác', 135000, 200, N'Trương Úy', N'Tú Phương', N'Tiếng Việt', 440, 2023, N'AZ Việt Nam', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000002', '1000000001')
INSERT INTO SanPham VALUES			('1000000003', '0000000002', N'Hai Mươi Dáng Hình Tội Ác', 129000, 200, N'Trương Úy ', N'Dư Uyển', N'Tiếng Việt', 368, 2023, N'AZ Việt Nam', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000003', '1000000001')

UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img3.png' WHERE maBD = '1000000001'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img4.png' WHERE maSP = '1000000002'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img5.png' WHERE maSP = '1000000003'

INSERT INTO BaiDang	VALUES		('1000000002', N'Sách - Bàn Tròn Trí Mạng', '', 0, 27, null)
INSERT INTO BaiDang_Shop VALUES	('1000000002', '1000000000')
INSERT INTO SanPham VALUES			('1000000004', '0000000002', N'Tập 1', 139000, 300, N'Tiếu Thanh Tranh', N'Đặng Hồng Quân', N'Tiếng Việt', 356, 2023, N'AZ Việt Nam', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000004', '1000000002')
INSERT INTO SanPham VALUES			('1000000005', '0000000002', N'Tập 2', 149000, 300, N'Tiếu Thanh Tranh', N'Đặng Hồng Quân', N'Tiếng Việt', 404, 2023, N'AZ Việt Nam', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000005', '1000000002')

UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img6.png' WHERE maBD = '1000000002'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img7.png' WHERE maSP = '1000000004'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img8.png' WHERE maSP = '1000000005'

INSERT INTO BaiDang	VALUES		('1000000003', N'Sách Bloom Books - TLH Giải Mã Tình Yêu + Thuật Thao Túng + Hành Tinh Của Một Kẻ Nghĩ Nhiều + Tôi Vỡ Tan', '', 0, 27, null)
INSERT INTO BaiDang_Shop VALUES	('1000000003', '1000000000')
INSERT INTO SanPham VALUES			('1000000006', '0000000006', N'Tâm Lý Học Giải Mã Tình Yêu', 119000, 123, N'Logan Ury', N'Trang Hoàng', N'Tiếng Việt', 216, 2022, N'AZ Việt Nam', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000006', '1000000003')
INSERT INTO SanPham VALUES			('1000000007', '0000000006', N'Thuật Thao Túng', 139000, 312, N'Wladislaw Jachtchenko', N'Vũ Trung Phi Yến', N'Tiếng Việt', 344, 2022, N'AZ Việt Nam', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000007', '1000000003')
INSERT INTO SanPham VALUES			('1000000008', '0000000006', N'Hành Tinh Của Một Kẻ Nghĩ Nhiều', 86000, 300, N'Nguyễn Đoàn Minh Thư', N'Không', N'Tiếng Việt', 184, 2022, N'AZ Việt Nam', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000008', '1000000003')
INSERT INTO SanPham VALUES			('1000000009', '0000000006', N'Tôi Vỡ Tan', 99000, 300, N'Bianca Sparacino', N'Yuki', N'Tiếng Việt', 192, 2022, N'AZ Việt Nam', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000009', '1000000003')

UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img9.png' WHERE maBD = '1000000003'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img10.png' WHERE maSP = '1000000006'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img13.png' WHERE maSP = '1000000007'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img11.png' WHERE maSP = '1000000008'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img12.png' WHERE maSP = '1000000009'

INSERT INTO UserAccount VALUES('useraccount2', 'useraccount', 1, N'None', 0)
INSERT INTO KhachHang VALUES('1000000001', 'useraccount2', N'Default Name', '0123456789', 'email@gmail.com', null, null, null, 0, 0, null)
INSERT INTO DiaChi VALUES('1000000001', '1000000001', N'useraccount', '0123456678', 15, 1505, 150503, '17 Đồng Kè', 0)
INSERT INTO Shop VALUES('1000000001', N'Nhã Nam', '0123456789', 'nhanam@gmail.com', '1000000001', '2024/04/24', 1, 0, 0, null)
INSERT INTO KhachHang_Shop VALUES('1000000001', '1000000001')

INSERT INTO BaiDang	VALUES		('1100000000', N'Sách - Series văn học trinh thám của tác giả Higashino Keigo', '', 0, 22, null)
INSERT INTO BaiDang_Shop VALUES	('1100000000', '1000000001')
INSERT INTO SanPham VALUES			('1100000000', '0000000002', N'Cánh cổng sát nhân', 249000, 321, N'Higashino Keigo', N'Nguyễn Hải Anh', N'Tiếng Việt', 529, 2022, N'Hội Nhà Văn', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000000', '1100000000')
INSERT INTO SanPham VALUES			('1100000001', '0000000002', N'Thanh gươm do dự', 199000, 20, N'Higashino Keigo', N'Mộc Miên', N'Tiếng Việt', 475, 2022, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000001', '1100000000')
INSERT INTO SanPham VALUES			('1100000002', '0000000002', N'Bạch Dạ Hành', 209000, 0, N'Higashino Keigo', N'Nguyễn Hải Anh', N'Tiếng Việt', 550, 2021, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000002', '1100000000')
INSERT INTO SanPham VALUES			('1100000003', '0000000002', N'Hoa mộng ảo', 129000, 120, N'Higashino Keigo', N'Vương Hải Yến', N'Tiếng Việt', 321, 2022, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000003', '1100000000')
INSERT INTO SanPham VALUES			('1100000004', '0000000002', N'Khách sạn mặt nạ', 300000, 100, N'Higashino Keigo', N'Nguyễn Hải Anh', N'Tiếng Việt', 729, 2022, N'Hội Nhà Văn', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000004', '1100000000')
INSERT INTO SanPham VALUES			('1100000005', '0000000002', N'Nhà ảo thuật đen', 199000, 430, N'Higashino Keigo', N'Mộc Miên', N'Tiếng Việt', 455, 2022, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000005', '1100000000')
INSERT INTO SanPham VALUES			('1100000006', '0000000002', N'Vụ án mạng ở nhà khách núi Hakuba', 138000, 120, N'Higashino Keigo', N'Nguyễn Hải Anh', N'Tiếng Việt', 421, 2022, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000006', '1100000000')
INSERT INTO SanPham VALUES			('1100000007', '0000000002', N'Trái tim của Brutus', 138000, 132, N'Higashino Keigo', N'Vương Hải Yến', N'Tiếng Việt', 351, 2022, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000007', '1100000000')
INSERT INTO SanPham VALUES			('1100000008', '0000000002', N'Vụ án mạng ở lữ quán Kairotei', 123000, 32, N'Higashino Keigo', N'Nguyễn Hải Anh', N'Tiếng Việt', 315, 2022, N'Hội Nhà Văn', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000008', '1100000000')
INSERT INTO SanPham VALUES			('1100000009', '0000000002', N'Án mạng mười một chữ', 110000, 11, N'Higashino Keigo', N'Mộc Miên', N'Tiếng Việt', 255, 2022, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000009', '1100000000')
INSERT INTO SanPham VALUES			('1100000010', '0000000002', N'Phía sau nghi can X', 129000, 43, N'Higashino Keigo', N'Nguyễn Hải Anh', N'Tiếng Việt', 392, 2022, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000010', '1100000000')
INSERT INTO SanPham VALUES			('1100000011', '0000000002', N'Sự cứu rỗi của thánh nữ', 128000, 12, N'Higashino Keigo', N'Vương Hải Yến', N'Tiếng Việt', 351, 2022, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000011', '1100000000')
INSERT INTO SanPham VALUES			('1100000012', '0000000002', N'Phương trình hạ chí', 159000, 4, N'Higashino Keigo', N'Nguyễn Hải Anh', N'Tiếng Việt', 415, 2022, N'Hội Nhà Văn', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000012', '1100000000')
INSERT INTO SanPham VALUES			('1100000013', '0000000002', N'Điều kỳ diệu của tiệm tạp hóa Namiya', 105000, 332, N'Higashino Keigo', N'Mộc Miên', N'Tiếng Việt', 255, 2022, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000013', '1100000000')
INSERT INTO SanPham VALUES			('1100000014', '0000000002', N'Thiên nga và dơi', 269000, 122, N'Higashino Keigo', N'Nguyễn Hải Anh', N'Tiếng Việt', 692, 2022, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000014', '1100000000')
INSERT INTO SanPham VALUES			('1100000015', '0000000002', N'Phố tan màn đêm', 179000, 32, N'Higashino Keigo', N'Vương Hải Yến', N'Tiếng Việt', 451, 2022, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000015', '1100000000')


UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img14.png' WHERE maBD = '1100000000'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img15.png' WHERE maSP = '1100000000'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img16.png' WHERE maSP = '1100000001'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img30.png' WHERE maSP = '1100000002'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img17.png' WHERE maSP = '1100000003'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img18.png' WHERE maSP = '1100000004'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img19.png' WHERE maSP = '1100000005'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img20.png' WHERE maSP = '1100000006'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img21.png' WHERE maSP = '1100000007'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img22.png' WHERE maSP = '1100000008'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img23.png' WHERE maSP = '1100000009'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img24.png' WHERE maSP = '1100000010'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img25.png' WHERE maSP = '1100000011'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img26.png' WHERE maSP = '1100000012'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img27.png' WHERE maSP = '1100000013'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img28.png' WHERE maSP = '1100000014'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img29.png' WHERE maSP = '1100000015'

select * from LoaiSanPham

select * from SanPham
select * from SanPham_BaiDang

delete from SanPham_BaiDang where maBD = '1100000000'
delete from SanPham where maSP like '11%'

select * from GioHang