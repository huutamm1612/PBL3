INSERT INTO UserAccount VALUES('useraccount1', 'useraccount', 1, N'None', 0)
INSERT INTO KhachHang VALUES('1000000000', 'useraccount1', N'Default Name', '0123456789', 'email@gmail.com', null, null, null, 0, 0, null)
INSERT INTO DiaChi VALUES('1000000000', '1000000000', N'useraccount', '0123456678', 15, 1505, 150503, '17 ??ng K�', 0)
INSERT INTO Shop VALUES('1000000000', N'AZ Vi?t Nam', '0123456789', 'az@gmail.com', '1000000000', '2024/04/24', 1, 0, 0, null)
INSERT INTO KhachHang_Shop VALUES('1000000000', '1000000000')
INSERT INTO BaiDang	VALUES		('1000000000', N'S�ch - C�i Ch?t C?a Nh?ng X�c S?ng', '', 0, 20, null)
INSERT INTO BaiDang_Shop VALUES	('1000000000', '1000000000')
INSERT INTO SanPham VALUES			('1000000000', '0000000002', N'T?p 1', 139000, 100, N'Masaya Yamaguchi ', N'Tr?nh Thanh T�m', N'Ti?ng Vi?t', 376, 2023, N'AZ Vi?t Nam', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000000', '1000000000')
INSERT INTO SanPham VALUES			('1000000001', '0000000002', N'T?p 2', 119000, 100, N'Masaya Yamaguchi ', N'V� Ph??ng Ng�n', N'Ti?ng Vi?t', 320, 2023, N'AZ Vi?t Nam', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000001', '1000000000')

INSERT INTO BaiDang	VALUES		('1000000001', N'Combo 2 Cu?n: Th? Gi?i B�n Trong C�i �c + Hai M??i D�ng H�nh T?i �c', '', 0, 18, null)
INSERT INTO BaiDang_Shop VALUES	('1000000001', '1000000000')
INSERT INTO SanPham VALUES			('1000000002', '0000000002', N'Th? Gi?i B�n Trong C�i �c', 135000, 200, N'Tr??ng �y', N'T� Ph??ng', N'Ti?ng Vi?t', 440, 2023, N'AZ Vi?t Nam', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000002', '1000000001')
INSERT INTO SanPham VALUES			('1000000003', '0000000002', N'Hai M??i D�ng H�nh T?i �c', 129000, 200, N'Tr??ng �y ', N'D? Uy?n', N'Ti?ng Vi?t', 368, 2023, N'AZ Vi?t Nam', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000003', '1000000001')

INSERT INTO BaiDang	VALUES		('1000000002', N'S�ch - B�n Tr�n Tr� M?ng', '', 0, 27, null)
INSERT INTO BaiDang_Shop VALUES	('1000000002', '1000000000')
INSERT INTO SanPham VALUES			('1000000004', '0000000002', N'T?p 1', 139000, 300, N'Ti?u Thanh Tranh', N'??ng H?ng Qu�n', N'Ti?ng Vi?t', 356, 2023, N'AZ Vi?t Nam', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000004', '1000000002')
INSERT INTO SanPham VALUES			('1000000005', '0000000002', N'T?p 2', 149000, 300, N'Ti?u Thanh Tranh', N'??ng H?ng Qu�n', N'Ti?ng Vi?t', 404, 2023, N'AZ Vi?t Nam', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000005', '1000000002')

INSERT INTO BaiDang	VALUES		('1000000003', N'S�ch Bloom Books - TLH Gi?i M� T�nh Y�u + Thu?t Thao T�ng + H�nh Tinh C?a M?t K? Ngh? Nhi?u + T�i V? Tan', '', 0, 27, null)
INSERT INTO BaiDang_Shop VALUES	('1000000003', '1000000000')
INSERT INTO SanPham VALUES			('1000000006', '0000000006', N'T�m L� H?c Gi?i M� T�nh Y�u', 119000, 123, N'Logan Ury', N'Trang Ho�ng', N'Ti?ng Vi?t', 216, 2022, N'AZ Vi?t Nam', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000006', '1000000003')
INSERT INTO SanPham VALUES			('1000000007', '0000000006', N'Thu?t Thao T�ng', 139000, 312, N'Wladislaw Jachtchenko', N'V? Trung Phi Y?n', N'Ti?ng Vi?t', 344, 2022, N'AZ Vi?t Nam', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000007', '1000000003')
INSERT INTO SanPham VALUES			('1000000008', '0000000006', N'H�nh Tinh C?a M?t K? Ngh? Nhi?u', 86000, 300, N'Nguy?n ?o�n Minh Th?', N'Kh�ng', N'Ti?ng Vi?t', 184, 2022, N'AZ Vi?t Nam', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000008', '1000000003')
INSERT INTO SanPham VALUES			('1000000009', '0000000006', N'T�i V? Tan', 99000, 300, N'Bianca Sparacino', N'Yuki', N'Ti?ng Vi?t', 192, 2022, N'AZ Vi?t Nam', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1000000009', '1000000003')


INSERT INTO UserAccount VALUES('useraccount2', 'useraccount', 1, N'None', 0)
INSERT INTO KhachHang VALUES('1000000001', 'useraccount2', N'Default Name', '0123456789', 'email@gmail.com', null, null, null, 0, 0, null)
INSERT INTO DiaChi VALUES('1000000001', '1000000001', N'useraccount', '0123456678', 15, 1505, 150503, '17 ??ng K�', 0)
INSERT INTO Shop VALUES('1000000001', N'Nh� Nam', '0123456789', 'nhanam@gmail.com', '1000000001', '2024/04/24', 1, 0, 0, null)
INSERT INTO KhachHang_Shop VALUES('1000000001', '1000000001')

INSERT INTO BaiDang	VALUES		('1100000000', N'S�ch - Series v?n h?c trinh th�m c?a t�c gi? Higashino Keigo', '', 0, 22, null)
INSERT INTO BaiDang_Shop VALUES	('1100000000', '1000000001')
INSERT INTO SanPham VALUES			('1100000000', '0000000002', N'C�nh c?ng s�t nh�n', 249000, 100, N'Higashino Keigo', N'Nguy?n H?i Anh', N'Ti?ng Vi?t', 529, 2022, N'H?i Nh� V?n', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000000', '1100000000')
INSERT INTO SanPham VALUES			('1100000001', '0000000002', N'Thanh g??m do d?', 199000, 100, N'Higashino Keigo', N'M?c Mi�n', N'Ti?ng Vi?t', 475, 2022, N'H� N?i', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000001', '1100000000')
INSERT INTO SanPham VALUES			('1100000002', '0000000002', N'C�nh c?ng s�t nh�n', 249000, 100, N'Higashino Keigo', N'Nguy?n H?i Anh', N'Ti?ng Vi?t', 532, 2022, N'H� N?i', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000002', '1100000000')
INSERT INTO SanPham VALUES			('1100000003', '0000000002', N'Hoa m?ng ?o', 129000, 100, N'Higashino Keigo', N'V??ng H?i Y?n', N'Ti?ng Vi?t', 321, 2022, N'H� N?i', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000003', '1100000000')
INSERT INTO SanPham VALUES			('1100000004', '0000000002', N'Kh�ch s?n m?t n?', 300000, 100, N'Higashino Keigo', N'Nguy?n H?i Anh', N'Ti?ng Vi?t', 729, 2022, N'H?i Nh� V?n', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000004', '1100000000')
INSERT INTO SanPham VALUES			('1100000005', '0000000002', N'Nh� ?o thu?t ?en', 199000, 100, N'Higashino Keigo', N'M?c Mi�n', N'Ti?ng Vi?t', 455, 2022, N'H� N?i', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000005', '1100000000')
INSERT INTO SanPham VALUES			('1100000006', '0000000002', N'V? �n m?ng ? nh� kh�ch n�i Hakuba', 138000, 100, N'Higashino Keigo', N'Nguy?n H?i Anh', N'Ti?ng Vi?t', 421, 2022, N'H� N?i', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000007', '1100000000')
INSERT INTO SanPham VALUES			('1100000008', '0000000002', N'Tr�i tim c?a Brutus', 138000, 100, N'Higashino Keigo', N'V??ng H?i Y?n', N'Ti?ng Vi?t', 351, 2022, N'H� N?i', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000008', '1100000000')
INSERT INTO SanPham VALUES			('1100000009', '0000000002', N'V? �n m?ng ? l? qu�n Kairotei', 123000, 100, N'Higashino Keigo', N'Nguy?n H?i Anh', N'Ti?ng Vi?t', 315, 2022, N'H?i Nh� V?n', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000009', '1100000000')
INSERT INTO SanPham VALUES			('1100000010', '0000000002', N'�n m?ng m??i m?t ch?', 110000, 100, N'Higashino Keigo', N'M?c Mi�n', N'Ti?ng Vi?t', 255, 2022, N'H� N?i', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000010', '1100000000')
INSERT INTO SanPham VALUES			('1100000011', '0000000002', N'Ph�a sau nghi can X', 129000, 100, N'Higashino Keigo', N'Nguy?n H?i Anh', N'Ti?ng Vi?t', 392, 2022, N'H� N?i', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000011', '1100000000')
INSERT INTO SanPham VALUES			('1100000012', '0000000002', N'S? c?u r?i c?a th�nh n?', 128000, 100, N'Higashino Keigo', N'V??ng H?i Y?n', N'Ti?ng Vi?t', 351, 2022, N'H� N?i', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000012', '1100000000')
INSERT INTO SanPham VALUES			('1100000009', '0000000002', N'Ph??ng tr�nh h? ch�', 159000, 100, N'Higashino Keigo', N'Nguy?n H?i Anh', N'Ti?ng Vi?t', 415, 2022, N'H?i Nh� V?n', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000009', '1100000000')
INSERT INTO SanPham VALUES			('1100000010', '0000000002', N'?i?u k? di?u c?a ti?m t?p h�a Namiya', 105000, 100, N'Higashino Keigo', N'M?c Mi�n', N'Ti?ng Vi?t', 255, 2022, N'H� N?i', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000010', '1100000000')
INSERT INTO SanPham VALUES			('1100000011', '0000000002', N'Thi�n nga v� d?i', 269000, 100, N'Higashino Keigo', N'Nguy?n H?i Anh', N'Ti?ng Vi?t', 692, 2022, N'H� N?i', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000011', '1100000000')
INSERT INTO SanPham VALUES			('1100000012', '0000000002', N'Ph? tan m�n ?�m', 179000, 100, N'Higashino Keigo', N'V??ng H?i Y?n', N'Ti?ng Vi?t', 451, 2022, N'H� N?i', N'B�a m?m', '', 0, null)
INSERT INTO SanPham_BaiDang VALUES	('1100000012', '1100000000')

select * from LoaiSanPham
