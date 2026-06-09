using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Triosik
{
    public partial class DashboardAdmin : Form
    {
        Panel sidebar, contentPanel;
        string activeMenu = "Dashboard";

        readonly Color Blue = Color.FromArgb(0, 92, 220);
        readonly Color DarkBlue = Color.FromArgb(0, 48, 120);
        readonly Color Bg = Color.FromArgb(245, 249, 255);
        readonly Color Card = Color.White;
        readonly Color TextDark = Color.FromArgb(15, 23, 42);

        const int SidebarW = 250;

        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Triosik;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public DashboardAdmin()
        {
            InitializeComponent();
            BuildUI();
        }

        private void DashboardAdmin_Load(object sender, EventArgs e) { }

        void BuildUI()
        {
            Text = "Triosik Admin";
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
            AddLabel(sidebar, "Admin Panel", 86, 65, 10, false, Color.White);

            AddMenu("⌂", "Dashboard", 125, ShowDashboard);
            AddMenu("▣", "Kelola Booking", 200, ShowBooking);
            AddMenu("♫", "Kelola Studio", 275, ShowStudio);
            AddMenu("+", "Kelola Layanan", 350, ShowLayanan);
            AddMenu("▤", "Laporan Pembayaran", 425, ShowPembayaran);

            AddLine(sidebar, 25, 510, 200);
            AddMenu("↪", "Logout", 550, Logout);
        }

        void AddMenu(string icon, string text, int y, Action action)
        {
            bool active = activeMenu == text;
            Panel menu = RoundedPanel(215, 52, 12, active ? Color.FromArgb(60, 145, 255) : Blue);
            menu.Location = new Point(17, y);
            menu.Cursor = Cursors.Hand;
            sidebar.Controls.Add(menu);

            AddLabel(menu, icon, 24, 11, 19, true, Color.White);
            AddLabel(menu, text, 68, 15, 10, true, Color.White);

            menu.Click += (s, e) => { SetActiveMenu(text); action(); };
            foreach (Control c in menu.Controls)
                c.Click += (s, e) => { SetActiveMenu(text); action(); };
        }

        void TopBar(string title, string subtitle)
        {
            contentPanel.Controls.Clear();
            AddLabel(contentPanel, title, 30, 35, 24, true, TextDark);
            AddLabel(contentPanel, subtitle, 32, 75, 10, false, Color.FromArgb(90, 103, 125));
            AddLabel(contentPanel, "▣  " + DateTime.Now.ToString("dd MMMM yyyy"), 560, 35, 9, true, TextDark);
            AddLabel(contentPanel, "●  Admin⌄", 770, 35, 9, true, TextDark);
        }

        void ShowDashboard()
        {
            TopBar("Welcome Admin! 👋", "Kelola studio musik dengan mudah.");

            AddStatCard(30, 120, "▣", "Total Booking", CountDb("SELECT COUNT(*) FROM Booking").ToString(), "Semua Data", Color.FromArgb(235, 245, 255), Blue);
            AddStatCard(290, 120, "✓", "Booking Selesai", CountDb("SELECT COUNT(*) FROM Booking WHERE status_booking='Selesai'").ToString(), "Selesai", Color.FromArgb(235, 255, 242), Color.Green);
            AddStatCard(550, 120, "◷", "Booking Pending", CountDb("SELECT COUNT(*) FROM Booking WHERE status_pembayaran='Belum Lunas'").ToString(), "Menunggu Kasir", Color.FromArgb(255, 246, 225), Color.Orange);
            AddStatCard(810, 120, "Rp", "Total Pendapatan", FormatRupiah(CountDb("SELECT ISNULL(SUM(jumlah_bayar),0) FROM Pembayaran")), "Semua", Color.FromArgb(245, 240, 255), Color.FromArgb(90, 70, 220));

            Panel quick = RoundedPanel(1020, 330, 18, Card);
            quick.Location = new Point(30, 285);
            contentPanel.Controls.Add(quick);

            AddLabel(quick, "Akses Cepat", 30, 25, 15, true, TextDark);
            AddQuickCard(quick, 35, 85, "▣", "Kelola Booking", "Lihat booking dan hapus data.", ShowBooking);
            AddQuickCard(quick, 285, 85, "♫", "Kelola Studio", "Tambah, ubah, hapus studio.", ShowStudio);
            AddQuickCard(quick, 535, 85, "+", "Kelola Layanan", "Tambah dan kelola layanan.", ShowLayanan);
            AddQuickCard(quick, 785, 85, "▤", "Laporan Pembayaran", "Lihat pembayaran kasir.", ShowPembayaran);
        }

        void ShowBooking()
        {
            TopBar("Kelola Booking", "Kelola semua data booking studio.");
            AddButton(contentPanel, "Refresh", 860, 105, 160, 42, Blue, Color.White, (s, e) => ShowBooking());

            Panel box = RoundedPanel(1020, 515, 18, Card);
            box.Location = new Point(30, 165);
            contentPanel.Controls.Add(box);

            string[] h = { "ID", "Nama", "Studio", "Tanggal", "Jam", "Total", "Booking", "Bayar", "Aksi" };
            int[] x = { 25, 110, 245, 350, 465, 570, 675, 790, 910 };
            AddTableHeader(box, h, x, 35);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string q = @"
                    SELECT TOP 8
                        b.id_booking, b.nama_pemesan, s.nama_studio, b.tanggal,
                        CONVERT(VARCHAR(5), b.jam_mulai, 108) AS jam,
                        b.total_harga, b.status_booking, b.status_pembayaran
                    FROM Booking b
                    INNER JOIN Studio s ON b.id_studio = s.id_studio
                    ORDER BY b.id_booking DESC";

                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader r = cmd.ExecuteReader();

                int y = 95;
                while (r.Read())
                {
                    int id = Convert.ToInt32(r["id_booking"]);
                    AddBookingRow(box, id, "BK" + id, r["nama_pemesan"].ToString(), r["nama_studio"].ToString(),
                        Convert.ToDateTime(r["tanggal"]).ToString("dd/MM/yy"), r["jam"].ToString(),
                        FormatRupiah(Convert.ToInt32(r["total_harga"])), r["status_booking"].ToString(),
                        r["status_pembayaran"].ToString(), y);
                    y += 55;
                }

                r.Close();
                if (y == 95) AddLabel(box, "Belum ada data booking.", 25, 105, 10, false, Color.Gray);
            }
        }

        void ShowStudio()
        {
            TopBar("Kelola Studio", "Kelola semua studio dan harga sewa.");
            AddButton(contentPanel, "+ Tambah Studio", 840, 105, 180, 42, Blue, Color.White, (s, e) => ShowStudioForm(0, "", "", 0, "", "Aktif"));

            AddStatCard(30, 120, "▦", "Total Studio", CountDb("SELECT COUNT(*) FROM Studio").ToString(), "Semua Studio", Color.FromArgb(240, 245, 255), Blue);
            AddStatCard(290, 120, "✓", "Studio Aktif", CountDb("SELECT COUNT(*) FROM Studio WHERE status='Aktif'").ToString(), "Ditampilkan", Color.FromArgb(235, 255, 242), Color.Green);
            AddStatCard(550, 120, "□", "Studio Nonaktif", CountDb("SELECT COUNT(*) FROM Studio WHERE status<>'Aktif'").ToString(), "Disembunyikan", Color.FromArgb(255, 246, 225), Color.Orange);

            Panel box = RoundedPanel(1020, 420, 18, Card);
            box.Location = new Point(30, 285);
            contentPanel.Controls.Add(box);

            AddLabel(box, "Daftar Studio", 25, 20, 14, true, TextDark);
            string[] h = { "ID", "Nama Studio", "Jenis", "Harga / Jam", "Kapasitas", "Status", "Aksi" };
            int[] x = { 25, 140, 310, 465, 610, 760, 890 };
            AddTableHeader(box, h, x, 75);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Studio ORDER BY id_studio", conn);
                SqlDataReader r = cmd.ExecuteReader();

                int y = 130;
                while (r.Read())
                {
                    int id = Convert.ToInt32(r["id_studio"]);
                    AddStudioRow(box, id, "ST" + id.ToString("000"), r["nama_studio"].ToString(),
                        r["jenis_studio"].ToString(), FormatRupiah(Convert.ToInt32(r["harga_per_jam"])),
                        r["kapasitas"].ToString(), r["status"].ToString(), y);
                    y += 65;
                }

                r.Close();
                if (y == 130) AddLabel(box, "Belum ada data studio.", 25, 140, 10, false, Color.Gray);
            }
        }

        void ShowLayanan()
        {
            TopBar("Kelola Layanan", "Kelola layanan tambahan yang tersedia.");
            AddButton(contentPanel, "+ Tambah Layanan", 825, 105, 195, 42, Blue, Color.White, (s, e) => ShowLayananForm(0, "", 0, "Aktif"));

            AddStatCard(30, 120, "≡", "Total Layanan", CountDb("SELECT COUNT(*) FROM Layanan").ToString(), "Semua Layanan", Color.FromArgb(240, 245, 255), Blue);
            AddStatCard(290, 120, "✓", "Layanan Aktif", CountDb("SELECT COUNT(*) FROM Layanan WHERE status='Aktif'").ToString(), "Ditampilkan", Color.FromArgb(235, 255, 242), Color.Green);
            AddStatCard(550, 120, "□", "Layanan Nonaktif", CountDb("SELECT COUNT(*) FROM Layanan WHERE status<>'Aktif'").ToString(), "Disembunyikan", Color.FromArgb(255, 246, 225), Color.Orange);

            Panel box = RoundedPanel(1020, 420, 18, Card);
            box.Location = new Point(30, 285);
            contentPanel.Controls.Add(box);

            string[] h = { "ID", "Nama Layanan", "Harga", "Status", "Aksi" };
            int[] x = { 25, 170, 420, 610, 790 };
            AddTableHeader(box, h, x, 75);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Layanan ORDER BY id_layanan", conn);
                SqlDataReader r = cmd.ExecuteReader();

                int y = 130;
                while (r.Read())
                {
                    int id = Convert.ToInt32(r["id_layanan"]);
                    AddLayananRow(box, id, "LY" + id.ToString("000"), r["nama_layanan"].ToString(),
                        FormatRupiah(Convert.ToInt32(r["harga"])), r["status"].ToString(), y);
                    y += 65;
                }

                r.Close();
                if (y == 130) AddLabel(box, "Belum ada data layanan.", 25, 140, 10, false, Color.Gray);
            }
        }

        void ShowPembayaran()
        {
            TopBar("Laporan Pembayaran", "Lihat pembayaran yang sudah dikonfirmasi kasir.");
            AddButton(contentPanel, "Refresh", 860, 105, 160, 42, Blue, Color.White, (s, e) => ShowPembayaran());

            AddStatCard(30, 120, "▦", "Total Transaksi", CountDb("SELECT COUNT(*) FROM Pembayaran").ToString(), "Transaksi", Color.FromArgb(240, 245, 255), Blue);
            AddStatCard(290, 120, "Rp", "Total Pendapatan", FormatRupiah(CountDb("SELECT ISNULL(SUM(jumlah_bayar),0) FROM Pembayaran")), "Total", Color.FromArgb(235, 255, 242), Color.Green);
            AddStatCard(550, 120, "◷", "Menunggu Kasir", CountDb("SELECT COUNT(*) FROM Booking WHERE status_pembayaran='Belum Lunas'").ToString(), "Transaksi", Color.FromArgb(255, 246, 225), Color.Orange);

            Panel box = RoundedPanel(1020, 420, 18, Card);
            box.Location = new Point(30, 285);
            contentPanel.Controls.Add(box);

            string[] h = { "ID", "Nama", "Studio", "Tanggal", "Total", "Metode", "Status", "Kasir" };
            int[] x = { 25, 120, 270, 390, 520, 650, 770, 900 };
            AddTableHeader(box, h, x, 75);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string q = @"
                    SELECT TOP 6
                        b.id_booking, b.nama_pemesan, s.nama_studio, p.tanggal_bayar,
                        p.jumlah_bayar, p.metode_pembayaran, p.status_pembayaran
                    FROM Pembayaran p
                    INNER JOIN Booking b ON p.id_booking = b.id_booking
                    INNER JOIN Studio s ON b.id_studio = s.id_studio
                    ORDER BY p.tanggal_bayar DESC";

                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader r = cmd.ExecuteReader();

                int y = 130;
                while (r.Read())
                {
                    AddBayarRow(box, "BK" + r["id_booking"], r["nama_pemesan"].ToString(),
                        r["nama_studio"].ToString(), Convert.ToDateTime(r["tanggal_bayar"]).ToString("dd/MM/yy"),
                        FormatRupiah(Convert.ToInt32(r["jumlah_bayar"])), r["metode_pembayaran"].ToString(),
                        r["status_pembayaran"].ToString(), "Kasir", y);
                    y += 65;
                }

                r.Close();
                if (y == 130) AddLabel(box, "Belum ada laporan pembayaran.", 25, 140, 10, false, Color.Gray);
            }
        }

        void ShowStudioForm(int id, string nama, string jenis, int harga, string kapasitas, string status)
        {
            Form f = new Form();
            f.Text = id == 0 ? "Tambah Studio" : "Edit Studio";
            f.Size = new Size(420, 390);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.FormBorderStyle = FormBorderStyle.FixedDialog;
            f.MaximizeBox = false;

            TextBox txtNama = MakeTextBox(f, "Nama Studio", nama, 30, 35);
            TextBox txtJenis = MakeTextBox(f, "Jenis Studio", jenis, 30, 95);
            TextBox txtHarga = MakeTextBox(f, "Harga / Jam", harga == 0 ? "" : harga.ToString(), 30, 155);
            TextBox txtKapasitas = MakeTextBox(f, "Kapasitas", kapasitas, 30, 215);

            ComboBox cbStatus = new ComboBox();
            cbStatus.Items.AddRange(new string[] { "Aktif", "Nonaktif" });
            cbStatus.Text = status == "" ? "Aktif" : status;
            cbStatus.Location = new Point(160, 275);
            cbStatus.Size = new Size(200, 28);
            f.Controls.Add(new Label() { Text = "Status", Location = new Point(30, 278), AutoSize = true });
            f.Controls.Add(cbStatus);

            Button save = new Button();
            save.Text = "Simpan";
            save.Location = new Point(250, 320);
            save.Size = new Size(110, 35);
            save.Click += (s, e) =>
            {
                int hargaValue;
                if (txtNama.Text.Trim() == "" || !int.TryParse(txtHarga.Text.Trim(), out hargaValue))
                {
                    MessageBox.Show("Nama wajib diisi dan harga harus angka.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string q = id == 0
                        ? @"INSERT INTO Studio (nama_studio, jenis_studio, harga_per_jam, kapasitas, status)
                            VALUES (@nama, @jenis, @harga, @kapasitas, @status)"
                        : @"UPDATE Studio SET nama_studio=@nama, jenis_studio=@jenis, harga_per_jam=@harga, kapasitas=@kapasitas, status=@status
                            WHERE id_studio=@id";

                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@jenis", txtJenis.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", hargaValue);
                    cmd.Parameters.AddWithValue("@kapasitas", txtKapasitas.Text.Trim());
                    cmd.Parameters.AddWithValue("@status", cbStatus.Text);
                    if (id != 0) cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                f.Close();
                ShowStudio();
            };

            f.Controls.Add(save);
            f.ShowDialog();
        }

        void ShowLayananForm(int id, string nama, int harga, string status)
        {
            Form f = new Form();
            f.Text = id == 0 ? "Tambah Layanan" : "Edit Layanan";
            f.Size = new Size(420, 310);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.FormBorderStyle = FormBorderStyle.FixedDialog;
            f.MaximizeBox = false;

            TextBox txtNama = MakeTextBox(f, "Nama Layanan", nama, 30, 45);
            TextBox txtHarga = MakeTextBox(f, "Harga", harga == 0 ? "" : harga.ToString(), 30, 115);

            ComboBox cbStatus = new ComboBox();
            cbStatus.Items.AddRange(new string[] { "Aktif", "Nonaktif" });
            cbStatus.Text = status == "" ? "Aktif" : status;
            cbStatus.Location = new Point(160, 185);
            cbStatus.Size = new Size(200, 28);
            f.Controls.Add(new Label() { Text = "Status", Location = new Point(30, 188), AutoSize = true });
            f.Controls.Add(cbStatus);

            Button save = new Button();
            save.Text = "Simpan";
            save.Location = new Point(250, 230);
            save.Size = new Size(110, 35);
            save.Click += (s, e) =>
            {
                int hargaValue;
                if (txtNama.Text.Trim() == "" || !int.TryParse(txtHarga.Text.Trim(), out hargaValue))
                {
                    MessageBox.Show("Nama wajib diisi dan harga harus angka.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string q = id == 0
                        ? @"INSERT INTO Layanan (nama_layanan, harga, status) VALUES (@nama, @harga, @status)"
                        : @"UPDATE Layanan SET nama_layanan=@nama, harga=@harga, status=@status WHERE id_layanan=@id";

                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", hargaValue);
                    cmd.Parameters.AddWithValue("@status", cbStatus.Text);
                    if (id != 0) cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                f.Close();
                ShowLayanan();
            };

            f.Controls.Add(save);
            f.ShowDialog();
        }

        TextBox MakeTextBox(Form f, string label, string value, int x, int y)
        {
            Label l = new Label();
            l.Text = label;
            l.Location = new Point(x, y);
            l.AutoSize = true;
            f.Controls.Add(l);

            TextBox t = new TextBox();
            t.Text = value;
            t.Location = new Point(x + 130, y - 3);
            t.Size = new Size(200, 28);
            f.Controls.Add(t);

            return t;
        }

        void DeleteStudio(int id)
        {
            if (MessageBox.Show("Yakin hapus studio ini?", "Hapus Studio", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Studio WHERE id_studio=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                ShowStudio();
            }
            catch
            {
                MessageBox.Show("Studio tidak bisa dihapus karena sudah dipakai booking.\nUbah status jadi Nonaktif aja.");
            }
        }

        void DeleteLayanan(int id)
        {
            if (MessageBox.Show("Yakin hapus layanan ini?", "Hapus Layanan", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Layanan WHERE id_layanan=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                ShowLayanan();
            }
            catch
            {
                MessageBox.Show("Layanan tidak bisa dihapus karena sudah dipakai booking.\nUbah status jadi Nonaktif aja.");
            }
        }

        void DeleteBooking(int id)
        {
            if (MessageBox.Show("Yakin hapus booking ini?", "Hapus Booking", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand c1 = new SqlCommand("DELETE FROM Booking_Layanan WHERE id_booking=@id", conn);
                    c1.Parameters.AddWithValue("@id", id);
                    c1.ExecuteNonQuery();

                    SqlCommand c2 = new SqlCommand("DELETE FROM Pembayaran WHERE id_booking=@id", conn);
                    c2.Parameters.AddWithValue("@id", id);
                    c2.ExecuteNonQuery();

                    SqlCommand c3 = new SqlCommand("DELETE FROM Booking WHERE id_booking=@id", conn);
                    c3.Parameters.AddWithValue("@id", id);
                    c3.ExecuteNonQuery();
                }
                ShowBooking();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal hapus booking:\n" + ex.Message);
            }
        }

        int AmbilHargaStudio(int id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT harga_per_jam FROM Studio WHERE id_studio=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        int AmbilHargaLayanan(int id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT harga FROM Layanan WHERE id_layanan=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        int CountDb(string query)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        string FormatRupiah(int value)
        {
            return "Rp " + value.ToString("N0").Replace(",", ".");
        }

        void AddBookingRow(Control p, int realId, string id, string nama, string studio, string tgl, string jam, string total, string status, string bayar, int y)
        {
            AddLabel(p, id, 25, y, 8, false, TextDark);
            AddLabel(p, nama, 110, y, 8, false, TextDark);
            AddLabel(p, studio, 245, y, 8, false, TextDark);
            AddLabel(p, tgl, 350, y, 8, false, TextDark);
            AddLabel(p, jam, 465, y, 8, false, TextDark);
            AddLabel(p, total, 570, y, 8, false, TextDark);
            AddBadge(p, status, 675, y - 5, status == "Selesai" ? Color.Green : status == "Dibooking" ? Blue : Color.Orange);
            AddBadge(p, bayar, 790, y - 5, bayar == "Lunas" ? Color.Green : Color.Red);
            AddMiniButton(p, "🗑", 930, y - 7, Color.Red, (s, e) => DeleteBooking(realId));
            AddLine(p, 20, y + 40, 980);
        }

        void AddStudioRow(Control p, int realId, string id, string nama, string jenis, string harga, string kapasitas, string status, int y)
        {
            AddLabel(p, id, 25, y, 8, false, TextDark);
            AddLabel(p, nama, 140, y, 8, true, TextDark);
            AddLabel(p, jenis, 310, y, 8, false, TextDark);
            AddLabel(p, harga, 465, y, 8, false, TextDark);
            AddLabel(p, kapasitas, 610, y, 8, false, TextDark);
            AddBadge(p, status, 760, y - 5, status == "Aktif" ? Color.Green : Color.Orange);
            AddMiniButton(p, "✎", 890, y - 7, Color.Orange, (s, e) => ShowStudioForm(realId, nama, jenis, AmbilHargaStudio(realId), kapasitas, status));
            AddMiniButton(p, "🗑", 930, y - 7, Color.Red, (s, e) => DeleteStudio(realId));
            AddLine(p, 20, y + 45, 980);
        }

        void AddLayananRow(Control p, int realId, string id, string nama, string harga, string status, int y)
        {
            AddLabel(p, id, 25, y, 8, false, TextDark);
            AddLabel(p, nama, 170, y, 8, true, TextDark);
            AddLabel(p, harga, 420, y, 8, false, TextDark);
            AddBadge(p, status, 610, y - 5, status == "Aktif" ? Color.Green : Color.Orange);
            AddMiniButton(p, "✎", 790, y - 7, Color.Orange, (s, e) => ShowLayananForm(realId, nama, AmbilHargaLayanan(realId), status));
            AddMiniButton(p, "🗑", 830, y - 7, Color.Red, (s, e) => DeleteLayanan(realId));
            AddLine(p, 20, y + 45, 980);
        }

        void AddBayarRow(Control p, string id, string nama, string studio, string tgl, string total, string metode, string status, string kasir, int y)
        {
            AddLabel(p, id, 25, y, 8, false, TextDark);
            AddLabel(p, nama, 120, y, 8, false, TextDark);
            AddLabel(p, studio, 270, y, 8, false, TextDark);
            AddLabel(p, tgl, 390, y, 8, false, TextDark);
            AddLabel(p, total, 520, y, 8, false, TextDark);
            AddLabel(p, metode, 650, y, 8, false, TextDark);
            AddBadge(p, status, 770, y - 5, status == "Lunas" ? Color.Green : Color.Orange);
            AddLabel(p, kasir, 900, y, 8, false, TextDark);
            AddLine(p, 20, y + 45, 980);
        }

        void AddQuickCard(Control parent, int x, int y, string icon, string title, string desc, Action action)
        {
            Panel card = RoundedPanel(215, 200, 15, Color.White);
            card.Location = new Point(x, y);
            card.Cursor = Cursors.Hand;
            parent.Controls.Add(card);
            AddLabel(card, icon, 88, 25, 32, true, Blue);
            AddLabel(card, title, 35, 85, 12, true, TextDark);
            AddLabel(card, desc, 28, 120, 9, false, Color.FromArgb(70, 85, 110));
            AddLabel(card, "→", 175, 155, 22, true, Blue);
            card.Click += (s, e) => { SetActiveMenu(title); action(); };
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

        void AddTableHeader(Control p, string[] headers, int[] xs, int y)
        {
            Panel head = new Panel();
            head.BackColor = Color.FromArgb(245, 249, 255);
            head.Location = new Point(20, y);
            head.Size = new Size(980, 45);
            p.Controls.Add(head);

            for (int i = 0; i < headers.Length; i++)
                AddLabel(head, headers[i], xs[i] - 20, 13, 8, true, TextDark);
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
            b.Size = new Size(90, 27);
            p.Controls.Add(b);
            MakeRounded(b, 8);
        }

        void AddMiniButton(Control p, string text, int x, int y, Color color, EventHandler click)
        {
            Button b = new Button();
            b.Text = text;
            b.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            b.ForeColor = color;
            b.BackColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderColor = color;
            b.Size = new Size(30, 30);
            b.Location = new Point(x, y);
            if (click != null) b.Click += click;
            p.Controls.Add(b);
            MakeRounded(b, 7);
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
            MakeRounded(b, 9);
        }

        void Logout()
        {
            Close();
            Form1 login = new Form1();
            login.Show();
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
