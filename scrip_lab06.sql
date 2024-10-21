CREATE DATABASE QUANLYSACH
GO 
USE QUANLYSACH
GO

CREATE TABLE LoaiSach 
( 
	MaLoai INT PRIMARY KEY, 
	TenLoai nvarchar(50) 
)
GO

CREATE TABLE Sach
( 
	MaSach char(6) PRIMARY KEY, 
	TenSach nvarchar(150), 
	NamXB INT, 
	MaLoai INT,
	FOREIGN KEY (MaLoai) REFERENCES LoaiSach(MaLoai)
)
GO 

INSERT INTO LoaiSach VALUES (1, N'Khoa Học')
INSERT INTO LoaiSach VALUES (2, N'Đời Sống')
INSERT INTO LoaiSach VALUES (3, N'Y Học')
GO

INSERT INTO Sach VALUES ('KH001', N'Khám Phá Sự Sống', 2018, 1)
INSERT INTO Sach VALUES ('KH002', N'Hải Dương Học', 2018, 1)
INSERT INTO Sach VALUES ('YH001', N'Chuẩn Đoán Và Điều Trị', 2020, 3)
GO