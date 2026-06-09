SET IDENTITY_INSERT [dbo].[Studio] ON
INSERT INTO [dbo].[Studio] ([id_studio], [nama_studio], [jenis_studio], [harga_per_jam], [kapasitas], [status]) VALUES (1, N'Studio 1', N'Regular', 50000, N'4 - 6 Orang', N'Aktif')
INSERT INTO [dbo].[Studio] ([id_studio], [nama_studio], [jenis_studio], [harga_per_jam], [kapasitas], [status]) VALUES (2, N'Studio 2', N'Regular', 60000, N'6 - 8 Orang', N'Aktif')
INSERT INTO [dbo].[Studio] ([id_studio], [nama_studio], [jenis_studio], [harga_per_jam], [kapasitas], [status]) VALUES (3, N'Studio Utama', N'Premium', 70000, N'8 - 12 Orang', N'Aktif')
SET IDENTITY_INSERT [dbo].[Studio] OFF
