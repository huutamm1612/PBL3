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


INSERT INTO UserAccount VALUES('useraccount3', 'useraccount', 1, N'None', 0)
INSERT INTO KhachHang VALUES('1000000002', 'useraccount3', N'Default Name', '0123456789', 'email@gmail.com', null, null, null, 0, 0, null)
INSERT INTO DiaChi VALUES('1000000002', '1000000002', N'useraccount', '0123456678', 15, 1505, 150503, '17 Đồng Kè', 0)
INSERT INTO Shop VALUES('1000000002', N'IPM Việt Nam', '0123456789', 'nhanam@gmail.com', '1000000002', '2024/04/24', 1, 0, 0, null)
INSERT INTO KhachHang_Shop VALUES('1000000002', '1000000002')

UPDATE Shop Set avt = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img63.png' WHERE maS = '1000000002'

INSERT INTO BaiDang	VALUES		('1200000000', N'Sách - Series 13 tập Death Note', '', 0, 15, null)
INSERT INTO BaiDang_Shop VALUES	('1200000000', '1000000002')
INSERT INTO SanPham VALUES			('1200000000', '0000000000', N'Death Note - 1', 45000, 321, N'Tsugumi Ohba', N'Ngọc Quang', N'Tiếng Việt', 192, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000000', '1200000000')
INSERT INTO SanPham VALUES			('1200000001', '0000000000', N'Death Note - 2', 45000, 321, N'Tsugumi Ohba', N'Ngọc Quang', N'Tiếng Việt', 193, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000001', '1200000000')
INSERT INTO SanPham VALUES			('1200000002', '0000000000', N'Death Note - 3', 45000, 321, N'Tsugumi Ohba', N'Ngọc Quang', N'Tiếng Việt', 191, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000002', '1200000000')
INSERT INTO SanPham VALUES			('1200000003', '0000000000', N'Death Note - 4', 45000, 321, N'Tsugumi Ohba', N'Ngọc Quang', N'Tiếng Việt', 196, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000003', '1200000000')
INSERT INTO SanPham VALUES			('1200000004', '0000000000', N'Death Note - 5', 45000, 321, N'Tsugumi Ohba', N'Ngọc Quang', N'Tiếng Việt', 193, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000004', '1200000000')
INSERT INTO SanPham VALUES			('1200000005', '0000000000', N'Death Note - 6', 45000, 321, N'Tsugumi Ohba', N'Ngọc Quang', N'Tiếng Việt', 188, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000005', '1200000000')
INSERT INTO SanPham VALUES			('1200000006', '0000000000', N'Death Note - 7', 45000, 321, N'Tsugumi Ohba', N'Ngọc Quang', N'Tiếng Việt', 195, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000006', '1200000000')
INSERT INTO SanPham VALUES			('1200000007', '0000000000', N'Death Note - 8', 45000, 321, N'Tsugumi Ohba', N'Ngọc Quang', N'Tiếng Việt', 198, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000007', '1200000000')
INSERT INTO SanPham VALUES			('1200000008', '0000000000', N'Death Note - 9', 45000, 321, N'Tsugumi Ohba', N'Nguyễn Ánh', N'Tiếng Việt', 191, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000008', '1200000000')
INSERT INTO SanPham VALUES			('1200000009', '0000000000', N'Death Note - 10', 45000, 321, N'Tsugumi Ohba', N'Nguyễn Ánh', N'Tiếng Việt', 191, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000009', '1200000000')
INSERT INTO SanPham VALUES			('1200000010', '0000000000', N'Death Note - 11', 45000, 321, N'Tsugumi Ohba', N'Nguyễn Ánh', N'Tiếng Việt', 199, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000010', '1200000000')
INSERT INTO SanPham VALUES			('1200000011', '0000000000', N'Death Note - 12', 45000, 321, N'Tsugumi Ohba', N'Nguyễn Ánh', N'Tiếng Việt', 193, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000011', '1200000000')
INSERT INTO SanPham VALUES			('1200000012', '0000000000', N'Death Note - 13', 80000, 321, N'Tsugumi Ohba', N'Nguyễn Ánh', N'Tiếng Việt', 196, 2022, N'IPM', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000012', '1200000000')

UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img49.png' WHERE maBD = '1200000000'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img50.png' WHERE maSP = '1200000000'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img51.png' WHERE maSP = '1200000001'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img52.png' WHERE maSP = '1200000002'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img53.png' WHERE maSP = '1200000003'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img54.png' WHERE maSP = '1200000004'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img55.png' WHERE maSP = '1200000005'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img56.png' WHERE maSP = '1200000006'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img57.png' WHERE maSP = '1200000007'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img58.png' WHERE maSP = '1200000008'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img59.png' WHERE maSP = '1200000009'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img60.png' WHERE maSP = '1200000010'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img61.png' WHERE maSP = '1200000011'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img62.png' WHERE maSP = '1200000012'


INSERT INTO BaiDang	VALUES		('1200000001', N'Sách - ZOO (Kinh Dị)', '', 0, 20, null)
INSERT INTO BaiDang_Shop VALUES	('1200000001', '1000000002')
INSERT INTO SanPham VALUES			('1200000013', '0000000000', N'Zoo', 100000, 12, N'Otsuichi', N'Ngọc Quang', N'Tiếng Việt', 352, 2016, N'Hồng Đức', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000013', '1200000001')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img64.png' WHERE maBD = '1200000001'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img65.png' WHERE maSP = '1200000013'


INSERT INTO BaiDang	VALUES		('1200000002', N'Sách - Sau Giờ Học (Trinh Thám)', '', 0, 20, null)
INSERT INTO BaiDang_Shop VALUES	('1200000002', '1000000002')
INSERT INTO SanPham VALUES			('1200000014', '0000000001', N'Sau Giờ Học', 100000, 12, N'Keigo Higashino', N'Ngọc Quang', N'Tiếng Việt', 320, 2022, N'Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000014', '1200000002')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img66.png' WHERE maBD = '1200000002'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img67.png' WHERE maSP = '1200000014'


INSERT INTO BaiDang	VALUES		('1200000003', N'Sách - Ác Ý (Trinh Thám)', '', 0, 20, null)
INSERT INTO BaiDang_Shop VALUES	('1200000003', '1000000002')
INSERT INTO SanPham VALUES			('1200000015', '0000000001', N'Ác Ý', 98000, 32, N'Keigo Higashino', N'Ngọc Quang', N'Tiếng Việt', 320, 2022, N'NXB Hồng Đức', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000015', '1200000003')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img68.png' WHERE maBD = '1200000003'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img69.png' WHERE maSP = '1200000015'


INSERT INTO BaiDang	VALUES		('1200000004', N'Sách - Thư (Trinh Thám)', '', 0, 20, null)
INSERT INTO BaiDang_Shop VALUES	('1200000004', '1000000002')
INSERT INTO SanPham VALUES			('1200000016', '0000000001', N'Thư', 140000, 32, N'Keigo Higashino', N'Thu Hiền', N'Tiếng Việt', 408, 2019, N'NXB Hồng Đức', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000016', '1200000004')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img70.png' WHERE maBD = '1200000004'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img71.png' WHERE maSP = '1200000016'


INSERT INTO BaiDang	VALUES		('1200000005', N'Sách - Biến Thân (Trinh Thám)', '', 0, 20, null)
INSERT INTO BaiDang_Shop VALUES	('1200000005', '1000000002')
INSERT INTO SanPham VALUES			('1200000017', '0000000001', N'Biến Thân', 115000, 22, N'Keigo Higashino', N'Thu Hiền', N'Tiếng Việt', 384, 2020, N'NXB Hồng Đức', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000017', '1200000005')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img72.png' WHERE maBD = '1200000005'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img73.png' WHERE maSP = '1200000017'


INSERT INTO BaiDang	VALUES		('1200000006', N'Sách - Mùa Hè, Pháo Hoa Và Xác Chết Của Tôi', '', 0, 20, null)
INSERT INTO BaiDang_Shop VALUES	('1200000006', '1000000002')
INSERT INTO SanPham VALUES			('1200000018', '0000000005', N'Mùa Hè, Pháo Hoa Và Xác Chết Của Tôi', 55000, 22, N'Otsuichi', N'Thu Hiền', N'Tiếng Việt', 156, 2020, N'NXB Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000018', '1200000006')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img74.png' WHERE maBD = '1200000006'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img75.png' WHERE maSP = '1200000018'


INSERT INTO BaiDang	VALUES		('1200000007', N'Sách - Sĩ Số Lớp Vắng 0', '', 0, 20, null)
INSERT INTO BaiDang_Shop VALUES	('1200000007', '1000000002')
INSERT INTO SanPham VALUES			('1200000019', '0000000002', N'Sĩ Số Lớp Vắng 0', 102000, 22, N'Emma Hạ My', N'Không', N'Tiếng Việt', 264, 2023, N'NXB Hà Nội', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000019', '1200000007')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img76.png' WHERE maBD = '1200000007'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img77.png' WHERE maSP = '1200000019'


INSERT INTO BaiDang	VALUES		('1200000008', N'Sách - Kỳ Án Ánh Trăng', '', 0, 20, null)
INSERT INTO BaiDang_Shop VALUES	('1200000008', '1000000002')
INSERT INTO SanPham VALUES			('1200000020', '0000000002', N'Kỳ Án Ánh Trăng', 180000, 22, N'Quỷ Cổ Nữ', N'Không', N'Tiếng Việt', 500, 2015, N'NXB Hồng Đức', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1200000020', '1200000008')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img78.png' WHERE maBD = '1200000008'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img79.png' WHERE maSP = '1200000020'


INSERT INTO BaiDang	VALUES		('1100000001', N'Sách - Goth: Những Kẻ Hắc Ám', '', 0, 22, null)
INSERT INTO BaiDang_Shop VALUES	('1100000001', '1000000001')
INSERT INTO SanPham VALUES			('1100000016', '0000000002', N'Goth: Những Kẻ Hắc Ám', 108000, 22, N'Otsuichi', N'H.M', N'Tiếng Việt', 357, 2015, N'NXB Hà Nội	', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000016', '1100000001')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img80.png' WHERE maBD = '1100000001'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img81.png' WHERE maSP = '1100000016'


INSERT INTO BaiDang	VALUES		('1100000002', N'Sách - Chuộc tội', '', 0, 22, null)
INSERT INTO BaiDang_Shop VALUES	('1100000002', '1000000001')
INSERT INTO SanPham VALUES			('1100000017', '0000000002', N'Chuộc tội', 108000, 22, N'Minato Kanae', N'Vương Hải Yến', N'Tiếng Việt', 357, 2015, N'NXB Hà Nội	', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000017', '1100000002')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img82.png' WHERE maBD = '1100000002'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img83.png' WHERE maSP = '1100000017'


INSERT INTO BaiDang	VALUES		('1100000003', N'Sách - Thú tội', '', 0, 22, null)
INSERT INTO BaiDang_Shop VALUES	('1100000003', '1000000001')
INSERT INTO SanPham VALUES			('1100000018', '0000000002', N'Thú tội', 108000, 22, N'Minato Kanae', N'Vương Hải Yến', N'Tiếng Việt', 274, 2015, N'NXB Hà Nội	', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000018', '1100000003')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img84.png' WHERE maBD = '1100000003'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img85.png' WHERE maSP = '1100000018'


INSERT INTO BaiDang	VALUES		('1100000004', N'Sách - Người bóng bay', '', 0, 25, null)
INSERT INTO BaiDang_Shop VALUES	('1100000004', '1000000001')
INSERT INTO SanPham VALUES			('1100000019', '0000000002', N'Người bóng bay', 189000, 12, N'Chan Ho-Kei', N'Nguyễn Tú Uyên', N'Tiếng Việt', 484, 2022, N'NXB Hà Nội	', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000019', '1100000004')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img86.png' WHERE maBD = '1100000004'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img87.png' WHERE maSP = '1100000019'


INSERT INTO BaiDang	VALUES		('1100000005', N'Sách - Combo Hội chứng E - Mã gien tội ác (Franck Thilliez)', '', 0, 25, null)
INSERT INTO BaiDang_Shop VALUES	('1100000005', '1000000001')
INSERT INTO SanPham VALUES			('1100000020', '0000000002', N'chứng E', 199000, 41, N'Franck Thilliez', N'Nguyễn Thị Tươi', N'Tiếng Việt', 484, 2024, N'NXB Văn Học', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000020', '1100000005')
INSERT INTO SanPham VALUES			('1100000021', '0000000002', N'Mã gien tội ác', 249000, 41, N'Franck Thilliez', N'Nguyễn Thị Tươi', N'Tiếng Việt', 576, 2024, N'NXB Văn Học', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000021', '1100000005')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img88.png' WHERE maBD = '1100000005'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img89.png' WHERE maSP = '1100000020'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img90.png' WHERE maSP = '1100000021'

INSERT INTO BaiDang	VALUES		('1100000006', N'Sách - Ngày mai, ngày mai, và ngày mai nữa', '', 0, 22, null)
INSERT INTO BaiDang_Shop VALUES	('1100000006', '1000000001')
INSERT INTO SanPham VALUES			('1100000022', '0000000003', N'Ngày mai, ngày mai, và ngày mai nữa', 259000, 123, N'Gabrielle Zevin', N'KURO', N'Tiếng Việt', 576, 2015, N'NXB Phụ Nữ', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000022', '1100000006')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img91.png' WHERE maBD = '1100000006'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img92.png' WHERE maSP = '1100000022'


INSERT INTO BaiDang	VALUES		('1100000007', N'Sách - Rừng Na Uy', '', 0, 22, null)
INSERT INTO BaiDang_Shop VALUES	('1100000007', '1000000001')
INSERT INTO SanPham VALUES			('1100000023', '0000000005', N'Rừng Na Uy', 150000, 123, N'Haruki Murakami', N'Trịnh Lữ', N'Tiếng Việt', 556, 2021, N'NXB Hội nhà văn', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000023', '1100000007')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img93.png' WHERE maBD = '1100000007'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img94.png' WHERE maSP = '1100000023'


INSERT INTO BaiDang	VALUES		('1100000008', N'Sách - Series Hannibal (Thomas Harris)', '', 0, 22, null)
INSERT INTO BaiDang_Shop VALUES	('1100000008', '1000000001')
INSERT INTO SanPham VALUES			('1100000024', '0000000004', N'SỰ IM LẶNG CỦA BẦY CỪU', 115000, 135, N'Thomas Harris', N'Phạm Hồng Anh', N'Tiếng Việt', 359, 2023, N'NXB Hội nhà văn', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000024', '1100000008')
INSERT INTO SanPham VALUES			('1100000025', '0000000004', N'HANNIBAL', 145000, 135, N'Thomas Harris', N'Thu Lê', N'Tiếng Việt', 432, 2023, N'NXB Hội nhà văn', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000025', '1100000008')
INSERT INTO SanPham VALUES			('1100000026', '0000000004', N'RỒNG ĐỎ', 135000, 135, N'Thomas Harris', N'Meil. G', N'Tiếng Việt', 404, 2023, N'NXB Hội nhà văn', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000026', '1100000008')
INSERT INTO SanPham VALUES			('1100000027', '0000000004', N'HANNIBAL TRỖI DẬY', 138000, 135, N'Thomas Harris', N'Huyền Vũ', N'Tiếng Việt', 312, 2023, N'NXB Hội nhà văn', N'Bìa mềm', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000027', '1100000008')
UPDATE BaiDang SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img95.png' WHERE maBD = '1100000008'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img96.png' WHERE maSP = '1100000024'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img97.png' WHERE maSP = '1100000025'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img98.png' WHERE maSP = '1100000026'
UPDATE SanPham SET anh = 'D:\_DUT\Nam 2(2023 - 2024)\PBL3\Code\dotnet\Images\img99.png' WHERE maSP = '1100000027'









select * from SanPham
select * from Shop
select * from LoaiSanPham
select * from BaiDang join BaiDang_Shop on BaiDang_Shop.maBD = BaiDang.maBD where maS = 1000000001