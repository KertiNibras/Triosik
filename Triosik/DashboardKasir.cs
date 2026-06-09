using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Triosik
{
    public partial class DashboardKasir : Form
    {
        Panel sidebar, contentPanel;
        string activeMenu = "Dashboard";

        readonly Color Blue = Color.FromArgb(0, 92, 220);
        readonly Color Bg = Color.FromArgb(245, 249, 255);
        readonly Color Card = Color.White;
        readonly Color TextDark = Color.FromArgb(15, 23, 42);
        readonly Color DarkBlue = Color.FromArgb(0, 48, 120);

        const int SidebarW = 250;

        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Triosik;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        int selectedBookingId = 0;
        int selectedTotal = 0;
        string selectedNama = "-";
        string selectedStudio = "-";
        string selectedTanggal = "-";
        string selectedJam = "-";

        public DashboardKasir()
        {
            InitializeComponent();
            BuildUI();
        }

        private void DashboardKasir_Load(object sender, EventArgs e) { }

        void BuildUI()
        {
            Text = "Triosik Kasir";
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            BackColor = Bg;
            DoubleBuffered = true;
            Controls.Clear();

            sidebar = new Panel();
            sidebar.Location = new Point(0, 0);
            sidebar.Size = new Size(SidebarW, ClientSize.Height);
            sidebar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            sidebar.BackColor = Blue;
            Controls.Add(sidebar);

            contentPanel = new Panel();
            contentPanel.Location = new Point(SidebarW, 0);
            contentPanel.Size = new Size(ClientSize.Width - SidebarW, ClientSize.Height);
            contentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            contentPanel.BackColor = Bg;
            Controls.Add(contentPanel);

            Resize += (s, e) =>
            {
                sidebar.Height = ClientSize.Height;
                contentPanel.Location = new Point(SidebarW, 0);
                contentPanel.Size = new Size(ClientSize.Width - SidebarW, ClientSize.Height);
            };

            SetActiveMenu("Dashboard");
            ShowDashboard();
        }

        void SetActiveMenu(string menu)
        {
            activeMenu = menu;
            sidebar.Controls.Clear();

            AddLabel(sidebar, "♪", 35, 30, 34, true, Color.White);
            AddLabel(sidebar, "TRIOSIK", 85, 35, 18, true, Color.White);
            AddLabel(sidebar, "Kasir Panel", 86, 65, 10, false, Color.White);

            AddMenu("⌂", "Dashboard", 145, ShowDashboard);
            AddMenu("▣", "Konfirmasi Pembayaran", 225, ShowKonfirmasiPembayaran);
            AddMenu("▤", "Riwayat Pembayaran", 305, ShowRiwayatPembayaran);

            AddLine(sidebar, 25, 415, 200);
            AddMenu("↪", "Logout", 455, Logout);
        }

        void AddMenu(string icon, string text, int y, Action action)
        {
            bool active = activeMenu == text;

            Panel menu = RoundedPanel(215, 55, 12, active ? Color.FromArgb(60, 145, 255) : Blue);
            menu.Location = new Point(17, y);
            menu.Cursor = Cursors.Hand;
            sidebar.Controls.Add(menu);

            AddLabel(menu, icon, 24, 12, 19, true, Color.White);
            AddLabel(menu, text, 68, 16, 10, true, Color.White);

            menu.Click += (s, e) => { SetActiveMenu(text); action(); };

            foreach (Control c in menu.Controls)
                c.Click += (s, e) => { SetActiveMenu(text); action(); };
        }

        void TopBar(string title, string subtitle)
        {
            contentPanel.Controls.Clear();

            AddLabel(contentPanel, title, 30, 35, 24, true, TextDark);
            AddLabel(contentPanel, subtitle, 32, 75, 10, false, Color.FromArgb(90, 103, 125));

            AddLabel(contentPanel, "▣  " + DateTime.Now.ToString("dd MMMM yyyy"), 740, 35, 9, true, TextDark);
            AddLabel(contentPanel, "●  Kasir⌄", 950, 35, 9, true, TextDark);
        }

        void ShowDashboard()
        {
            TopBar("Selamat datang, Kasir! 👋", "Kelola pembayaran dan konfirmasi booking dengan mudah.");

            AddStatCard(30, 140, "▣", "Total Booking Hari Ini", CountTodayBooking().ToString(), "Booking", Color.FromArgb(235, 245, 255), Blue);
            AddStatCard(290, 140, "⌛", "Menunggu Pembayaran", CountBelumLunas().ToString(), "Perlu Konfirmasi", Color.FromArgb(255, 246, 225), Color.Orange);
            AddStatCard(550, 140, "✓", "Pembayaran Lunas", CountLunasToday().ToString(), "Hari Ini", Color.FromArgb(235, 255, 242), Color.Green);
            AddStatCard(810, 140, "Rp", "Total Pendapatan", FormatRupiah(GetPendapatanToday()), "Hari Ini", Color.FromArgb(245, 240, 255), Color.FromArgb(90, 70, 220));

            AddLabel(contentPanel, "Fitur Kasir", 35, 315, 16, true, TextDark);

            AddFeatureCard(30, 365, "▣", "Konfirmasi Pembayaran", "Konfirmasi pembayaran booking\ndan update status menjadi lunas.", ShowKonfirmasiPembayaran);
            AddFeatureCard(390, 365, "▤", "Riwayat Pembayaran", "Lihat pembayaran yang sudah\ndikonfirmasi.", ShowRiwayatPembayaran);
        }

        void ShowKonfirmasiPembayaran()
        {
            TopBar("Konfirmasi Pembayaran", "Konfirmasi pembayaran booking dan update status menjadi lunas.");

            AddStatCard(30, 120, "⌛", "Menunggu Konfirmasi", CountBelumLunas().ToString(), "Booking", Color.FromArgb(255, 246, 225), Color.Orange);
            AddStatCard(290, 120, "✓", "Pembayaran Lunas", CountLunasToday().ToString(), "Hari Ini", Color.FromArgb(235, 255, 242), Color.Green);
            AddStatCard(550, 120, "Rp", "Total Pendapatan", FormatRupiah(GetPendapatanToday()), "Hari Ini", Color.FromArgb(245, 240, 255), Color.FromArgb(90, 70, 220));

            Panel tableBox = RoundedPanel(690, 500, 18, Card);
            tableBox.Location = new Point(30, 285);
            contentPanel.Controls.Add(tableBox);

            AddLabel(tableBox, "Daftar Pembayaran Menunggu", 25, 20, 14, true, TextDark);

            string[] h = { "ID", "Nama", "Studio", "Jam", "Total", "Metode", "Aksi" };
            int[] x = { 25, 105, 225, 335, 430, 525, 610 };
            AddTableHeader(tableBox, h, x, 75, 640);

            LoadBookingBelumLunas(tableBox);

            AddDetailPanel();
        }

        void LoadBookingBelumLunas(Control parent)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        b.id_booking,
                        b.nama_pemesan,
                        s.nama_studio,
                        CONVERT(VARCHAR(5), b.jam_mulai, 108) AS jam,
                        b.total_harga,
                        b.metode_pembayaran,
                        b.tanggal
                    FROM Booking b
                    INNER JOIN Studio s ON b.id_studio = s.id_studio
                    WHERE b.status_pembayaran = 'Belum Lunas'
                    ORDER BY b.id_booking DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                int y = 130;

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id_booking"]);
                    string nama = reader["nama_pemesan"].ToString();
                    string studio = reader["nama_studio"].ToString();
                    string jam = reader["jam"].ToString();
                    int total = Convert.ToInt32(reader["total_harga"]);
                    string metode = reader["metode_pembayaran"].ToString();
                    string tanggal = Convert.ToDateTime(reader["tanggal"]).ToString("dd/MM/yyyy");

                    AddKonfirmasiRow(parent, id, nama, studio, jam, total, metode, tanggal, y);
                    y += 60;
                }

                reader.Close();

                if (y == 130)
                    AddLabel(parent, "Belum ada pembayaran yang menunggu.", 25, 135, 10, false, Color.Gray);
            }
        }

        void AddKonfirmasiRow(Control p, int id, string nama, string studio, string jam, int total, string metode, string tanggal, int y)
        {
            AddLabel(p, "BK" + id, 25, y, 8, false, TextDark);
            AddLabel(p, nama, 105, y, 8, false, TextDark);
            AddLabel(p, studio, 225, y, 8, false, TextDark);
            AddLabel(p, jam, 335, y, 8, false, TextDark);
            AddLabel(p, FormatRupiah(total), 430, y, 8, false, TextDark);
            AddLabel(p, metode, 525, y, 8, false, TextDark);

            AddButton(p, "Pilih", 600, y - 8, 70, 30, Blue, Color.White, (s, e) =>
            {
                selectedBookingId = id;
                selectedNama = nama;
                selectedStudio = studio;
                selectedTanggal = tanggal;
                selectedJam = jam;
                selectedTotal = total;
                ShowKonfirmasiPembayaran();
            });

            AddLine(p, 20, y + 38, 650);
        }

        void AddDetailPanel()
        {
            Panel detail = RoundedPanel(360, 500, 18, Card);
            detail.Location = new Point(750, 285);
            contentPanel.Controls.Add(detail);

            AddLabel(detail, "Detail Booking", 25, 20, 14, true, TextDark);

            if (selectedBookingId == 0)
            {
                Panel info = RoundedPanel(310, 65, 10, Color.FromArgb(225, 238, 255));
                info.Location = new Point(25, 60);
                detail.Controls.Add(info);
                AddLabel(info, "ⓘ Pilih booking dari tabel untuk konfirmasi.", 15, 22, 8, false, DarkBlue);
                return;
            }

            AddDetailText(detail, "ID Booking", "BK" + selectedBookingId, 80);
            AddDetailText(detail, "Nama Pemesan", selectedNama, 115);
            AddDetailText(detail, "Studio", selectedStudio, 150);
            AddDetailText(detail, "Tanggal", selectedTanggal, 185);
            AddDetailText(detail, "Jam", selectedJam, 220);
            AddDetailText(detail, "Total", FormatRupiah(selectedTotal), 275);
            AddDetailText(detail, "Metode", "Cash", 310);

            AddButton(detail, "✓ Konfirmasi & Update Lunas", 25, 400, 310, 50, Color.Green, Color.White, (s, e) =>
            {
                KonfirmasiBookingLunas();
            });
        }

        void KonfirmasiBookingLunas()
        {
            if (selectedBookingId == 0)
            {
                MessageBox.Show("Pilih booking dulu.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Yakin pembayaran booking BK" + selectedBookingId + " sudah lunas?",
                "Konfirmasi Pembayaran",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes) return;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string updateBooking = @"
                        UPDATE Booking
                        SET status_pembayaran = 'Lunas',
                            status_booking = 'Selesai'
                        WHERE id_booking = @id";

                    SqlCommand cmdUpdate = new SqlCommand(updateBooking, conn, trans);
                    cmdUpdate.Parameters.AddWithValue("@id", selectedBookingId);
                    cmdUpdate.ExecuteNonQuery();

                    string insertPembayaran = @"
                        INSERT INTO Pembayaran
                        (id_booking, jumlah_bayar, metode_pembayaran, status_pembayaran, id_kasir)
                        VALUES
                        (@idbooking, @jumlah, 'Cash', 'Lunas', NULL)";

                    SqlCommand cmdPay = new SqlCommand(insertPembayaran, conn, trans);
                    cmdPay.Parameters.AddWithValue("@idbooking", selectedBookingId);
                    cmdPay.Parameters.AddWithValue("@jumlah", selectedTotal);
                    cmdPay.ExecuteNonQuery();

                    trans.Commit();

                    MessageBox.Show("Pembayaran berhasil dikonfirmasi.\nStatus berubah jadi Lunas.");

                    selectedBookingId = 0;
                    selectedTotal = 0;
                    ShowKonfirmasiPembayaran();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Gagal konfirmasi pembayaran:\n" + ex.Message);
                }
            }
        }

        void ShowRiwayatPembayaran()
        {
            TopBar("Riwayat Pembayaran", "Lihat semua pembayaran yang sudah dikonfirmasi.");

            AddStatCard(30, 120, "▦", "Total Transaksi", CountAllPayment().ToString(), "Transaksi", Color.FromArgb(235, 245, 255), Blue);
            AddStatCard(290, 120, "✓", "Pembayaran Lunas", CountAllPayment().ToString(), "Transaksi", Color.FromArgb(235, 255, 242), Color.Green);
            AddStatCard(550, 120, "Rp", "Total Pendapatan", FormatRupiah(GetAllPendapatan()), "Total", Color.FromArgb(245, 240, 255), Color.FromArgb(90, 70, 220));

            Panel box = RoundedPanel(920, 500, 18, Card);
            box.Location = new Point(30, 285);
            contentPanel.Controls.Add(box);

            string[] h = { "ID", "Nama", "Studio", "Tanggal", "Total", "Metode", "Status", "Kasir" };
            int[] x = { 25, 110, 230, 350, 475, 600, 700, 795 };
            AddTableHeader(box, h, x, 40, 870);

            LoadRiwayatPembayaran(box);
        }

        void LoadRiwayatPembayaran(Control parent)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        b.id_booking,
                        b.nama_pemesan,
                        s.nama_studio,
                        p.tanggal_bayar,
                        p.jumlah_bayar,
                        p.metode_pembayaran,
                        p.status_pembayaran
                    FROM Pembayaran p
                    INNER JOIN Booking b ON p.id_booking = b.id_booking
                    INNER JOIN Studio s ON b.id_studio = s.id_studio
                    ORDER BY p.tanggal_bayar DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                int y = 95;

                while (reader.Read())
                {
                    AddRiwayatRow(
                        parent,
                        "BK" + reader["id_booking"].ToString(),
                        reader["nama_pemesan"].ToString(),
                        reader["nama_studio"].ToString(),
                        Convert.ToDateTime(reader["tanggal_bayar"]).ToString("dd/MM/yy"),
                        FormatRupiah(Convert.ToInt32(reader["jumlah_bayar"])),
                        reader["metode_pembayaran"].ToString(),
                        reader["status_pembayaran"].ToString(),
                        "Kasir",
                        y
                    );

                    y += 60;
                }

                reader.Close();
            }
        }

        int CountTodayBooking()
        {
            return ExecuteScalarInt("SELECT COUNT(*) FROM Booking WHERE tanggal = CAST(GETDATE() AS DATE)");
        }

        int CountBelumLunas()
        {
            return ExecuteScalarInt("SELECT COUNT(*) FROM Booking WHERE status_pembayaran = 'Belum Lunas'");
        }

        int CountLunasToday()
        {
            return ExecuteScalarInt("SELECT COUNT(*) FROM Pembayaran WHERE CAST(tanggal_bayar AS DATE) = CAST(GETDATE() AS DATE)");
        }

        int CountAllPayment()
        {
            return ExecuteScalarInt("SELECT COUNT(*) FROM Pembayaran");
        }

        int GetPendapatanToday()
        {
            return ExecuteScalarInt("SELECT ISNULL(SUM(jumlah_bayar),0) FROM Pembayaran WHERE CAST(tanggal_bayar AS DATE) = CAST(GETDATE() AS DATE)");
        }

        int GetAllPendapatan()
        {
            return ExecuteScalarInt("SELECT ISNULL(SUM(jumlah_bayar),0) FROM Pembayaran");
        }

        int ExecuteScalarInt(string query)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        void AddFeatureCard(int x, int y, string icon, string title, string desc, Action action)
        {
            Panel card = RoundedPanel(330, 130, 15, Card);
            card.Location = new Point(x, y);
            card.Cursor = Cursors.Hand;
            contentPanel.Controls.Add(card);

            Panel iconBox = RoundedPanel(65, 65, 14, Color.FromArgb(235, 245, 255));
            iconBox.Location = new Point(25, 32);
            card.Controls.Add(iconBox);

            AddLabel(iconBox, icon, 17, 11, 24, true, Blue);
            AddLabel(card, title, 110, 30, 12, true, TextDark);
            AddLabel(card, desc, 110, 62, 9, false, Color.FromArgb(70, 85, 110));
            AddLabel(card, "→", 295, 78, 22, true, Blue);

            card.Click += (s, e) =>
            {
                SetActiveMenu(title);
                action();
            };
        }

        void AddStatCard(int x, int y, string icon, string title, string value, string desc, Color bg, Color color)
        {
            Panel card = RoundedPanel(235, 110, 15, Card);
            card.Location = new Point(x, y);
            contentPanel.Controls.Add(card);

            Panel iconBox = RoundedPanel(60, 60, 14, bg);
            iconBox.Location = new Point(18, 25);
            card.Controls.Add(iconBox);

            AddLabel(iconBox, icon, 16, 10, 22, true, color);
            AddLabel(card, title, 95, 25, 9, true, TextDark);
            AddLabel(card, value, 95, 50, value.Length > 6 ? 14 : 20, true, color);
            AddLabel(card, desc, 95, 82, 8, false, Color.FromArgb(90, 103, 125));
        }

        void AddRiwayatRow(Control p, string id, string nama, string studio, string tanggal, string total, string metode, string status, string kasir, int y)
        {
            AddLabel(p, id, 25, y, 8, false, TextDark);
            AddLabel(p, nama, 110, y, 8, false, TextDark);
            AddLabel(p, studio, 230, y, 8, false, TextDark);
            AddLabel(p, tanggal, 350, y, 8, false, TextDark);
            AddLabel(p, total, 475, y, 8, false, TextDark);
            AddLabel(p, metode, 600, y, 8, false, TextDark);
            AddBadge(p, status, 700, y - 5, Color.Green);
            AddLabel(p, kasir, 795, y, 8, false, TextDark);
            AddLine(p, 20, y + 38, 870);
        }

        void AddDetailText(Control p, string left, string right, int y)
        {
            AddLabel(p, left, 25, y, 9, true, Color.FromArgb(70, 85, 110));
            AddLabel(p, ":", 155, y, 9, true, TextDark);
            AddLabel(p, right, 175, y, 9, false, TextDark);
        }

        void AddTableHeader(Control p, string[] headers, int[] xs, int y, int width)
        {
            Panel head = new Panel();
            head.BackColor = Color.FromArgb(245, 249, 255);
            head.Location = new Point(20, y);
            head.Size = new Size(width, 42);
            p.Controls.Add(head);

            for (int i = 0; i < headers.Length; i++)
                AddLabel(head, headers[i], xs[i] - 20, 12, 8, true, TextDark);
        }

        void AddBadge(Control p, string text, int x, int y, Color color)
        {
            Label b = new Label();
            b.Text = text;
            b.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            b.ForeColor = color;
            b.BackColor = Color.FromArgb(235, 245, 240);
            b.TextAlign = ContentAlignment.MiddleCenter;
            b.Location = new Point(x, y);
            b.Size = new Size(80, 27);
            p.Controls.Add(b);
            MakeRounded(b, 8);
        }

        void AddButton(Control p, string text, int x, int y, int w, int h, Color bg, Color fg, EventHandler click)
        {
            Button b = new Button();
            b.Text = text;
            b.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            b.ForeColor = fg;
            b.BackColor = bg;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Size = new Size(w, h);
            b.Location = new Point(x, y);
            if (click != null) b.Click += click;
            p.Controls.Add(b);
            MakeRounded(b, 8);
        }

        void Logout()
        {
            Close();
            Form1 login = new Form1();
            login.Show();
        }

        string FormatRupiah(int value)
        {
            return "Rp " + value.ToString("N0").Replace(",", ".");
        }

        void AddLabel(Control p, string text, int x, int y, int size, bool bold, Color color)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular);
            lbl.ForeColor = color;
            lbl.AutoSize = true;
            lbl.BackColor = Color.Transparent;
            lbl.Location = new Point(x, y);
            p.Controls.Add(lbl);
        }

        void AddLine(Control p, int x, int y, int w)
        {
            Panel line = new Panel();
            line.BackColor = Color.FromArgb(220, 228, 240);
            line.Location = new Point(x, y);
            line.Size = new Size(w, 1);
            p.Controls.Add(line);
        }

        Panel RoundedPanel(int w, int h, int r, Color color)
        {
            Panel p = new Panel();
            p.Size = new Size(w, h);
            p.BackColor = color;
            MakeRounded(p, r);
            return p;
        }

        void MakeRounded(Control c, int r)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, r * 2, r * 2, 180, 90);
            path.AddArc(c.Width - r * 2, 0, r * 2, r * 2, 270, 90);
            path.AddArc(c.Width - r * 2, c.Height - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(0, c.Height - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            c.Region = new Region(path);
        }
    }
}