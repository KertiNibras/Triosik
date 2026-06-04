using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Triosik
{
    public partial class DashboardAdmin : Form
    {
        Panel sidebar;
        Panel contentPanel;
        string activeMenu = "Dashboard";

        public DashboardAdmin()
        {
            InitializeComponent();
            BuildUI();
        }

        private void DashboardAdmin_Load(object sender, EventArgs e) { }

        void BuildUI()
        {
            this.Text = "Dashboard Admin";
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(248, 251, 255);
            this.DoubleBuffered = true;

            sidebar = new Panel();
            sidebar.Width = 280;
            sidebar.Dock = DockStyle.Left;
            sidebar.BackColor = Color.FromArgb(31, 126, 224);
            this.Controls.Add(sidebar);

            contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.FromArgb(248, 251, 255);
            this.Controls.Add(contentPanel);

            SetActiveMenu("Dashboard");
            ShowDashboardPage();
        }

        void SetActiveMenu(string menuName)
        {
            activeMenu = menuName;
            sidebar.Controls.Clear();

            AddPic(sidebar, Properties.Resources.Hitam, 35, 55, 70, 70);
            AddPic(sidebar, Properties.Resources.Kuning, 85, 40, 85, 85);
            AddPic(sidebar, Properties.Resources.Pink, 155, 58, 70, 70);

            Label logo1 = new Label();
            logo1.Text = "Triosic";
            logo1.Font = new Font("Chubby And Groovy", 34, FontStyle.Bold);
            logo1.ForeColor = Color.White;
            logo1.AutoSize = true;
            logo1.Location = new Point(58, 135);
            sidebar.Controls.Add(logo1);

            Label logo2 = new Label();
            logo2.Text = "music studio";
            logo2.Font = new Font("Ground Castle DEMO", 24, FontStyle.Bold);
            logo2.ForeColor = Color.White;
            logo2.AutoSize = true;
            logo2.Location = new Point(50, 185);
            sidebar.Controls.Add(logo2);

            AddMenu(Properties.Resources.HomeIcon, "Dashboard", 280, ShowDashboardPage);
            AddMenu(Properties.Resources.Jadwalicon, "Kelola Studio", 345, ShowStudioPage);
            AddMenu(Properties.Resources.mulaibookingicon, "Kelola Jadwal\nOperasional", 410, ShowJadwalOperasionalPage);
            AddMenu(Properties.Resources.Logouticon, "Konfirmasi Booking", 490, ShowKonfirmasiPage);
            AddMenu(Properties.Resources.Logouticon, "Laporan Pemesanan", 555, ShowLaporanPage);
            AddMenu(Properties.Resources.Logouticon, "Logout", 620, Logout);
        }

        void AddMenu(Image icon, string text, int y, Action action)
        {
            bool active = activeMenu == text.Replace("\n", " ");

            Panel menu = RoundedPanel(240, text.Contains("\n") ? 64 : 52, 14,
                active ? Color.FromArgb(255, 230, 55) : Color.FromArgb(31, 126, 224));

            menu.Location = new Point(20, y);
            menu.Cursor = Cursors.Hand;
            sidebar.Controls.Add(menu);

            AddPic(menu, icon, 17, 15, 26, 26);

            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lbl.ForeColor = active ? Color.Black : Color.White;
            lbl.AutoSize = true;
            lbl.Location = new Point(60, text.Contains("\n") ? 9 : 15);
            lbl.Cursor = Cursors.Hand;
            menu.Controls.Add(lbl);

            menu.Click += (s, e) =>
            {
                SetActiveMenu(text.Replace("\n", " "));
                action();
            };

            lbl.Click += (s, e) =>
            {
                SetActiveMenu(text.Replace("\n", " "));
                action();
            };
        }

        void ShowDashboardPage()
        {
            contentPanel.Controls.Clear();

            AddText(contentPanel, "Selamat datang, Admin! 👋", 50, 45, 24, true);
            AddText(contentPanel, "Kelola studio Triosic Music Studio dengan mudah.", 52, 90, 11, false, Color.FromArgb(100, 116, 139));

            AddPic(contentPanel, Properties.Resources.Hitam, 980, 42, 55, 55);
            AddText(contentPanel, "Admin", 1045, 50, 11, true);
            AddText(contentPanel, "Administrator", 1045, 75, 9, false, Color.FromArgb(100, 116, 139));
            AddText(contentPanel, "⌄", 1145, 54, 15, true, Color.FromArgb(37, 99, 235));

            AddStatCard(50, 145, Color.FromArgb(245, 249, 255), Properties.Resources.card1icon, "Total Studio", "3", "Studio Aktif");
            AddStatCard(300, 145, Color.FromArgb(246, 255, 249), Properties.Resources.calendergreen, "Total Booking Masuk", "24", "Booking");
            AddStatCard(550, 145, Color.FromArgb(255, 251, 242), Properties.Resources.card2icon, "Booking Menunggu", "5", "Menunggu Konfirmasi");
            AddStatCard(800, 145, Color.FromArgb(252, 246, 255), Properties.Resources.card4icon, "Total Pendapatan", "Rp7.250.000", "Bulan Ini");

            AddRecentBookingPanel();
            AddQuickButtonPanel();
        }

        void AddStatCard(int x, int y, Color bg, Image icon, string title, string value, string desc)
        {
            Panel card = RoundedPanel(230, 135, 16, bg);
            card.Location = new Point(x, y);
            contentPanel.Controls.Add(card);

            AddPic(card, icon, 22, 22, 36, 36);
            AddText(card, title, 75, 29, 10, true, Color.FromArgb(51, 65, 85));
            AddText(card, value, 22, 70, value.Length > 8 ? 18 : 22, true, Color.Black);
            AddText(card, desc, 22, 105, 9, false, Color.FromArgb(100, 116, 139));
        }

        void AddRecentBookingPanel()
        {
            Panel box = RoundedPanel(735, 380, 16, Color.White);
            box.Location = new Point(50, 310);
            contentPanel.Controls.Add(box);

            AddText(box, "Ringkasan Booking Terbaru", 22, 18, 12, true);

            Label lihat = new Label();
            lihat.Text = "Lihat Semua";
            lihat.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lihat.ForeColor = Color.FromArgb(37, 99, 235);
            lihat.AutoSize = true;
            lihat.Location = new Point(635, 20);
            box.Controls.Add(lihat);

            AddLine(box, 0, 55, 735);

            AddBookingRow(box, Properties.Resources.calendergreen, "Studio A", "Band Room", "29 Mei 2025 • 13:00 - 15:00 • 2 Jam", "Menunggu", Color.Orange, 80);
            AddBookingRow(box, Properties.Resources.mulaibookingicon, "Studio B", "Vocal Room", "29 Mei 2025 • 15:00 - 17:00 • 2 Jam", "Menunggu", Color.Orange, 155);
            AddBookingRow(box, Properties.Resources.musicpurple, "Studio C", "Instrument Room", "28 Mei 2025 • 10:00 - 12:00 • 2 Jam", "Dikonfirmasi", Color.Green, 230);
            AddBookingRow(box, Properties.Resources.calendergreen, "Studio A", "Band Room", "27 Mei 2025 • 19:00 - 22:00 • 3 Jam", "Selesai", Color.Green, 305);
        }

        void AddBookingRow(Panel parent, Image icon, string studio, string room, string info, string status, Color statusColor, int y)
        {
            AddPic(parent, icon, 28, y, 42, 42);
            AddText(parent, studio, 92, y - 2, 11, true);
            AddText(parent, "(" + room + ")", 165, y, 9, false, Color.FromArgb(100, 116, 139));
            AddText(parent, info, 92, y + 25, 9, false, Color.FromArgb(71, 85, 105));

            Label st = new Label();
            st.Text = status;
            st.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            st.ForeColor = statusColor == Color.Orange ? Color.FromArgb(245, 158, 11) : Color.FromArgb(22, 163, 74);
            st.BackColor = statusColor == Color.Orange ? Color.FromArgb(255, 247, 237) : Color.FromArgb(220, 252, 231);
            st.TextAlign = ContentAlignment.MiddleCenter;
            st.Size = new Size(95, 28);
            st.Location = new Point(610, y + 6);
            parent.Controls.Add(st);
            MakeRounded(st, 12);
        }

        void AddQuickButtonPanel()
        {
            Panel box = RoundedPanel(430, 380, 16, Color.White);
            box.Location = new Point(815, 310);
            contentPanel.Controls.Add(box);

            AddText(box, "Tombol Cepat", 22, 18, 12, true);

            AddQuickButton(box, "+", "Tambah Studio", "Tambah studio baru", 70, Color.FromArgb(37, 99, 235), Color.FromArgb(239, 246, 255));
            AddQuickButton(box, "▣", "Atur Jadwal Operasional", "Kelola jam operasional studio", 155, Color.FromArgb(37, 99, 235), Color.FromArgb(239, 246, 255));
            AddQuickButton(box, "▣", "Cek Booking", "Lihat & konfirmasi booking masuk", 240, Color.FromArgb(22, 163, 74), Color.FromArgb(240, 253, 244));
        }

        void AddQuickButton(Panel parent, string icon, string title, string desc, int y, Color color, Color bg)
        {
            Panel btn = RoundedPanel(380, 72, 14, bg);
            btn.Location = new Point(25, y);
            btn.Cursor = Cursors.Hand;
            parent.Controls.Add(btn);

            AddText(btn, icon, 25, 12, 26, true, color);
            AddText(btn, title, 85, 14, 11, true, color);
            AddText(btn, desc, 85, 40, 9, false, Color.FromArgb(71, 85, 105));

            if (title == "Tambah Studio")
                btn.Click += (s, e) => { SetActiveMenu("Kelola Studio"); ShowStudioPage(); };

            if (title == "Atur Jadwal Operasional")
                btn.Click += (s, e) => { SetActiveMenu("Kelola Jadwal Operasional"); ShowJadwalOperasionalPage(); };

            if (title == "Cek Booking")
                btn.Click += (s, e) => { SetActiveMenu("Konfirmasi Booking"); ShowKonfirmasiPage(); };
        }

        void ShowStudioPage()
        {
            contentPanel.Controls.Clear();

            AddText(contentPanel, "Kelola Studio", 50, 45, 22, true);
            AddText(contentPanel, "Kelola semua studio yang tersedia di Triosic Music Studio.", 52, 85, 10, false, Color.FromArgb(100, 116, 139));

            Button add = new Button();
            add.Text = "+  Tambah Studio";
            add.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            add.ForeColor = Color.White;
            add.BackColor = Color.FromArgb(37, 99, 235);
            add.FlatStyle = FlatStyle.Flat;
            add.FlatAppearance.BorderSize = 0;
            add.Size = new Size(160, 44);
            add.Location = new Point(1000, 45);
            contentPanel.Controls.Add(add);
            MakeRounded(add, 8);

            Panel table = RoundedPanel(1110, 520, 12, Color.White);
            table.Location = new Point(50, 130);
            contentPanel.Controls.Add(table);

            AddStudioHeader(table);
            AddStudioRow(table, 1, Properties.Resources.calendergreen, "Studio A", "Band Room", "Band Room", "Rp200.000", "6 Orang", 85);
            AddStudioRow(table, 2, Properties.Resources.mulaibookingicon, "Studio B", "Vocal Room", "Vocal Room", "Rp150.000", "3 Orang", 215);
            AddStudioRow(table, 3, Properties.Resources.musicpurple, "Studio C", "Instrument Room", "Instrument Room", "Rp125.000", "4 Orang", 345);
        }

        void AddStudioHeader(Panel table)
        {
            string[] headers = { "No", "Nama Studio", "Jenis Studio", "Harga / Jam", "Kapasitas", "Status", "Aksi" };
            int[] xs = { 35, 160, 360, 540, 710, 860, 1000 };

            for (int i = 0; i < headers.Length; i++)
                AddText(table, headers[i], xs[i], 35, 10, true);

            AddLine(table, 0, 75, 1110);
        }

        void AddStudioRow(Panel table, int no, Image icon, string nama, string subNama, string jenis, string harga, string kapasitas, int y)
        {
            AddText(table, no.ToString(), 35, y + 35, 12, true);
            AddPic(table, icon, 90, y + 20, 55, 55);

            AddText(table, nama, 160, y + 22, 12, true);
            AddText(table, subNama, 160, y + 50, 9, false, Color.FromArgb(100, 116, 139));

            AddText(table, jenis, 360, y + 35, 10, false);
            AddText(table, harga, 540, y + 35, 10, false);
            AddText(table, kapasitas, 710, y + 35, 10, false);

            Label status = new Label();
            status.Text = "Aktif";
            status.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            status.ForeColor = Color.FromArgb(22, 163, 74);
            status.BackColor = Color.FromArgb(220, 252, 231);
            status.TextAlign = ContentAlignment.MiddleCenter;
            status.Size = new Size(70, 30);
            status.Location = new Point(860, y + 30);
            table.Controls.Add(status);
            MakeRounded(status, 8);

            Button edit = SmallActionButton("✎", Color.FromArgb(37, 99, 235));
            edit.Location = new Point(990, y + 25);
            table.Controls.Add(edit);

            Button del = SmallActionButton("🗑", Color.Red);
            del.Location = new Point(1045, y + 25);
            table.Controls.Add(del);

            AddLine(table, 0, y + 105, 1110);
        }

        Button SmallActionButton(string text, Color color)
        {
            Button b = new Button();
            b.Text = text;
            b.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            b.ForeColor = color;
            b.BackColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderColor = Color.FromArgb(215, 225, 240);
            b.Size = new Size(42, 40);
            return b;
        }

        void ShowJadwalOperasionalPage()
        {
            contentPanel.Controls.Clear();

            AddText(contentPanel, "Kelola Jadwal Operasional", 50, 45, 20, true);
            AddText(contentPanel, "Atur jam operasional studio dan slot waktu yang tersedia.", 52, 82, 10, false, Color.FromArgb(100, 116, 139));

            Button add = new Button();
            add.Text = "+  Atur Jadwal";
            add.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            add.ForeColor = Color.White;
            add.BackColor = Color.FromArgb(37, 99, 235);
            add.FlatStyle = FlatStyle.Flat;
            add.FlatAppearance.BorderSize = 0;
            add.Size = new Size(120, 42);
            add.Location = new Point(1030, 38);
            contentPanel.Controls.Add(add);
            MakeRounded(add, 8);

            Panel table = RoundedPanel(720, 370, 10, Color.White);
            table.Location = new Point(50, 115);
            contentPanel.Controls.Add(table);

            AddText(table, "Hari", 25, 22, 9, true);
            AddText(table, "Status", 165, 22, 9, true);
            AddText(table, "Jam Operasional", 300, 22, 9, true);
            AddText(table, "Slot Waktu", 505, 22, 9, true);
            AddText(table, "Aksi", 650, 22, 9, true);

            AddLine(table, 0, 55, 720);

            AddJadwalRow(table, "Senin", "Buka", "09:00 - 22:00", "1 Jam / slot", 70, true);
            AddJadwalRow(table, "Selasa", "Buka", "09:00 - 22:00", "1 Jam / slot", 110, true);
            AddJadwalRow(table, "Rabu", "Buka", "09:00 - 23:00", "1 Jam / slot", 150, true);
            AddJadwalRow(table, "Kamis", "Buka", "09:00 - 22:00", "1 Jam / slot", 190, true);
            AddJadwalRow(table, "Jumat", "Buka", "09:00 - 23:00", "1 Jam / slot", 230, true);
            AddJadwalRow(table, "Sabtu", "Buka", "08:00 - 23:00", "1 Jam / slot", 270, true);
            AddJadwalRow(table, "Minggu", "Tutup", "-", "-", 310, false);

            Panel info = RoundedPanel(220, 155, 10, Color.FromArgb(240, 247, 255));
            info.Location = new Point(800, 115);
            contentPanel.Controls.Add(info);

            AddText(info, "Informasi", 20, 18, 11, true);
            AddText(info, "Slot waktu adalah\ninterval waktu minimal\nuntuk setiap booking.", 20, 50, 9, false, Color.FromArgb(71, 85, 105));
            AddText(info, "Saat ini: 1 Jam / slot", 20, 120, 9, false, Color.FromArgb(71, 85, 105));

            Panel note = RoundedPanel(220, 150, 10, Color.FromArgb(255, 250, 235));
            note.Location = new Point(800, 295);
            contentPanel.Controls.Add(note);

            AddText(note, "Catatan", 20, 18, 11, true);
            AddText(note, "Perubahan jadwal\nakan langsung\nberlaku untuk semua\npengguna.", 20, 52, 9, false, Color.FromArgb(71, 85, 105));
        }

        void AddJadwalRow(Panel parent, string hari, string status, string jam, string slot, int y, bool buka)
        {
            AddText(parent, hari, 25, y + 8, 9, true);

            Label st = new Label();
            st.Text = status;
            st.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            st.ForeColor = buka ? Color.FromArgb(22, 163, 74) : Color.FromArgb(239, 68, 68);
            st.BackColor = buka ? Color.FromArgb(220, 252, 231) : Color.FromArgb(254, 226, 226);
            st.TextAlign = ContentAlignment.MiddleCenter;
            st.Size = new Size(55, 24);
            st.Location = new Point(160, y + 6);
            parent.Controls.Add(st);
            MakeRounded(st, 8);

            AddText(parent, jam, 300, y + 8, 9, false);
            AddText(parent, slot, 505, y + 8, 9, false);

            Button edit = new Button();
            edit.Text = "✎";
            edit.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            edit.ForeColor = Color.FromArgb(37, 99, 235);
            edit.BackColor = Color.FromArgb(239, 246, 255);
            edit.FlatStyle = FlatStyle.Flat;
            edit.FlatAppearance.BorderColor = Color.FromArgb(191, 219, 254);
            edit.Size = new Size(32, 30);
            edit.Location = new Point(645, y + 3);
            parent.Controls.Add(edit);
            MakeRounded(edit, 7);

            AddLine(parent, 0, y + 38, 720);
        }

        void ShowKonfirmasiPage()
        {
            contentPanel.Controls.Clear();

            AddText(contentPanel, "Konfirmasi Booking", 50, 45, 22, true);
            AddText(contentPanel, "Review dan konfirmasi booking yang masuk dari pengguna.", 52, 85, 11, false, Color.FromArgb(100, 116, 139));

            AddFilterButton("Semua (5)", 50, 135, false);
            AddFilterButton("Menunggu (5)", 175, 135, true);
            AddFilterButton("Dikonfirmasi (0)", 325, 135, false);
            AddFilterButton("Ditolak (0)", 500, 135, false);

            Button date = new Button();
            date.Text = "Pilih Tanggal      📅";
            date.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            date.ForeColor = Color.FromArgb(100, 116, 139);
            date.BackColor = Color.White;
            date.FlatStyle = FlatStyle.Flat;
            date.FlatAppearance.BorderColor = Color.FromArgb(220, 230, 245);
            date.Size = new Size(190, 45);
            date.Location = new Point(1015, 135);
            contentPanel.Controls.Add(date);
            MakeRounded(date, 8);

            Panel table = RoundedPanel(1155, 390, 12, Color.White);
            table.Location = new Point(50, 205);
            contentPanel.Controls.Add(table);

            AddText(table, "No", 25, 25, 10, true);
            AddText(table, "Detail Booking", 85, 25, 10, true);
            AddText(table, "Studio", 315, 25, 10, true);
            AddText(table, "Tanggal & Waktu", 515, 25, 10, true);
            AddText(table, "Durasi", 675, 25, 10, true);
            AddText(table, "Total", 780, 25, 10, true);
            AddText(table, "Pembayaran", 895, 25, 10, true);
            AddText(table, "Status", 1040, 25, 10, true);
            AddText(table, "Aksi", 1110, 25, 10, true);

            AddLine(table, 0, 60, 1155);

            AddKonfirmasiRow(table, "1", "Budi Santoso", "budi@email.com", "0812-xxxx-xxxx",
                Properties.Resources.calendergreen, "Studio A", "Band Room",
                "29 Mei 2025\n13:00 - 15:00", "2 Jam", "Rp400.000",
                "Transfer Bank\nBCA", 75);

            AddKonfirmasiRow(table, "2", "Siti Rahma", "siti@email.com", "0813-xxxx-xxxx",
                Properties.Resources.mulaibookingicon, "Studio B", "Vocal Room",
                "29 Mei 2025\n15:00 - 17:00", "2 Jam", "Rp300.000",
                "E-Wallet\nOVO", 175);

            AddKonfirmasiRow(table, "3", "Andi Wijaya", "andi@email.com", "0814-xxxx-xxxx",
                Properties.Resources.musicpurple, "Studio C", "Instrument Room",
                "30 Mei 2025\n10:00 - 12:00", "2 Jam", "Rp250.000",
                "Transfer Bank\nMandiri", 275);
        }

        void AddFilterButton(string text, int x, int y, bool active)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.ForeColor = active ? Color.FromArgb(37, 99, 235) : Color.FromArgb(80, 90, 110);
            btn.BackColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = active ? Color.FromArgb(37, 99, 235) : Color.FromArgb(220, 230, 245);
            btn.FlatAppearance.BorderSize = active ? 2 : 1;
            btn.Size = new Size(120, 45);
            btn.Location = new Point(x, y);
            contentPanel.Controls.Add(btn);
            MakeRounded(btn, 8);
        }

        void AddKonfirmasiRow(
            Panel table,
            string no,
            string nama,
            string email,
            string hp,
            Image studioIcon,
            string studio,
            string jenis,
            string tanggalJam,
            string durasi,
            string total,
            string pembayaran,
            int y)
        {
            AddText(table, no, 25, y + 35, 10, false);

            AddText(table, nama, 85, y + 15, 10, true);
            AddText(table, email, 85, y + 40, 9, false, Color.FromArgb(100, 116, 139));
            AddText(table, hp, 85, y + 62, 9, false, Color.FromArgb(100, 116, 139));

            AddPic(table, studioIcon, 315, y + 25, 45, 45);
            AddText(table, studio, 370, y + 25, 10, true);
            AddText(table, "(" + jenis + ")", 370, y + 50, 9, false, Color.FromArgb(100, 116, 139));

            AddText(table, tanggalJam, 515, y + 25, 9, false);
            AddText(table, durasi, 675, y + 35, 9, false);
            AddText(table, total, 780, y + 35, 10, false);
            AddText(table, pembayaran, 895, y + 25, 9, false);

            Label status = new Label();
            status.Text = "Menunggu";
            status.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            status.ForeColor = Color.FromArgb(245, 158, 11);
            status.BackColor = Color.FromArgb(255, 247, 237);
            status.TextAlign = ContentAlignment.MiddleCenter;
            status.Size = new Size(90, 30);
            status.Location = new Point(1030, y + 30);
            table.Controls.Add(status);
            MakeRounded(status, 10);

            Button acc = new Button();
            acc.Text = "✓";
            acc.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            acc.ForeColor = Color.White;
            acc.BackColor = Color.FromArgb(22, 163, 74);
            acc.FlatStyle = FlatStyle.Flat;
            acc.FlatAppearance.BorderSize = 0;
            acc.Size = new Size(38, 38);
            acc.Location = new Point(1125, y + 25);
            table.Controls.Add(acc);
            MakeRounded(acc, 8);

            Button reject = new Button();
            reject.Text = "×";
            reject.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            reject.ForeColor = Color.White;
            reject.BackColor = Color.FromArgb(239, 68, 68);
            reject.FlatStyle = FlatStyle.Flat;
            reject.FlatAppearance.BorderSize = 0;
            reject.Size = new Size(38, 38);
            reject.Location = new Point(1170, y + 25);
            table.Controls.Add(reject);
            MakeRounded(reject, 8);

            AddLine(table, 0, y + 95, 1155);
        }

        void ShowLaporanPage()
        {
            contentPanel.Controls.Clear();

            AddText(contentPanel, "Laporan / Riwayat Booking", 50, 35, 22, true);
            AddText(contentPanel, "Lihat semua riwayat booking dan laporan pendapatan.", 52, 75, 10, false, Color.FromArgb(100, 116, 139));

            Button download = new Button();
            download.Text = "↓  Download Laporan";
            download.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            download.ForeColor = Color.FromArgb(37, 99, 235);
            download.BackColor = Color.White;
            download.FlatStyle = FlatStyle.Flat;
            download.FlatAppearance.BorderColor = Color.FromArgb(37, 99, 235);
            download.Size = new Size(190, 45);
            download.Location = new Point(1010, 35);
            contentPanel.Controls.Add(download);
            MakeRounded(download, 8);

            AddFilterBox("01 Mei 2025      📅", 50, 105, 170);
            AddText(contentPanel, "s/d", 230, 117, 9, true, Color.FromArgb(71, 85, 105));
            AddFilterBox("31 Mei 2025      📅", 270, 105, 170);
            AddFilterBox("Semua Studio  ⌄", 470, 105, 170);
            AddFilterBox("Semua Status  ⌄", 660, 105, 170);

            Button filter = new Button();
            filter.Text = "Filter";
            filter.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            filter.ForeColor = Color.White;
            filter.BackColor = Color.FromArgb(37, 99, 235);
            filter.FlatStyle = FlatStyle.Flat;
            filter.FlatAppearance.BorderSize = 0;
            filter.Size = new Size(75, 42);
            filter.Location = new Point(850, 105);
            contentPanel.Controls.Add(filter);
            MakeRounded(filter, 8);

            AddLaporanStatCard(50, 170, Properties.Resources.card1icon, "Total Booking", "24", "Transaksi", Color.FromArgb(239, 246, 255), Color.FromArgb(37, 99, 235));
            AddLaporanStatCard(255, 170, Properties.Resources.calendergreen, "Selesai", "18", "75%", Color.FromArgb(240, 253, 244), Color.FromArgb(22, 163, 74));
            AddLaporanStatCard(460, 170, Properties.Resources.card2icon, "Dibatalkan", "3", "12.5%", Color.FromArgb(255, 251, 235), Color.FromArgb(245, 158, 11));
            AddLaporanStatCard(665, 170, Properties.Resources.Logouticon, "Ditolak", "3", "12.5%", Color.FromArgb(254, 242, 242), Color.FromArgb(239, 68, 68));
            AddLaporanStatCard(870, 170, Properties.Resources.card4icon, "Total Pendapatan", "Rp7.250.000", "", Color.FromArgb(250, 245, 255), Color.FromArgb(147, 51, 234));

            Panel table = RoundedPanel(1155, 300, 10, Color.White);
            table.Location = new Point(50, 265);
            contentPanel.Controls.Add(table);

            AddText(table, "No", 25, 22, 9, true);
            AddText(table, "Tanggal", 95, 22, 9, true);
            AddText(table, "Studio", 245, 22, 9, true);
            AddText(table, "Penyewa", 365, 22, 9, true);
            AddText(table, "Waktu", 525, 22, 9, true);
            AddText(table, "Durasi", 665, 22, 9, true);
            AddText(table, "Total", 775, 22, 9, true);
            AddText(table, "Status", 905, 22, 9, true);
            AddText(table, "Pembayaran", 1030, 22, 9, true);

            AddLine(table, 0, 55, 1155);

            AddLaporanRow(table, "1", "29 Mei 2025", "Studio A", "Budi Santoso", "13:00 - 15:00", "2 Jam", "Rp400.000", "Menunggu", "Transfer Bank\nBCA", 70, false);
            AddLaporanRow(table, "2", "29 Mei 2025", "Studio B", "Siti Rahma", "15:00 - 17:00", "2 Jam", "Rp300.000", "Menunggu", "E-Wallet\nOVO", 120, false);
            AddLaporanRow(table, "3", "28 Mei 2025", "Studio C", "Rizky Pratama", "10:00 - 12:00", "2 Jam", "Rp250.000", "Selesai", "Transfer Bank\nMandiri", 170, true);
            AddLaporanRow(table, "4", "27 Mei 2025", "Studio A", "Dewi Lestari", "19:00 - 22:00", "3 Jam", "Rp600.000", "Selesai", "Transfer Bank\nBCA", 220, true);

            AddText(contentPanel, "‹", 520, 590, 18, true, Color.FromArgb(148, 163, 184));

            Button p1 = new Button();
            p1.Text = "1";
            p1.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            p1.ForeColor = Color.White;
            p1.BackColor = Color.FromArgb(37, 99, 235);
            p1.FlatStyle = FlatStyle.Flat;
            p1.FlatAppearance.BorderSize = 0;
            p1.Size = new Size(38, 38);
            p1.Location = new Point(560, 585);
            contentPanel.Controls.Add(p1);
            MakeRounded(p1, 7);

            AddText(contentPanel, "2", 615, 593, 10, true, Color.FromArgb(71, 85, 105));
            AddText(contentPanel, "3", 655, 593, 10, true, Color.FromArgb(71, 85, 105));
            AddText(contentPanel, "›", 700, 590, 18, true, Color.FromArgb(37, 99, 235));
        }

        void AddFilterBox(string text, int x, int y, int w)
        {
            Button box = new Button();
            box.Text = text;
            box.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            box.ForeColor = Color.FromArgb(71, 85, 105);
            box.BackColor = Color.White;
            box.FlatStyle = FlatStyle.Flat;
            box.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            box.Size = new Size(w, 42);
            box.Location = new Point(x, y);
            contentPanel.Controls.Add(box);
            MakeRounded(box, 8);
        }

        void AddLaporanStatCard(int x, int y, Image icon, string title, string value, string desc, Color bg, Color color)
        {
            Panel card = RoundedPanel(180, 90, 10, bg);
            card.Location = new Point(x, y);
            contentPanel.Controls.Add(card);

            AddPic(card, icon, 18, 18, 24, 24);
            AddText(card, title, 50, 18, 8, true, color);
            AddText(card, value, 50, 40, value.Length > 5 ? 14 : 18, true);
            if (desc != "")
                AddText(card, desc, 50, 67, 8, false);
        }

        void AddLaporanRow(Panel table, string no, string tanggal, string studio, string penyewa, string waktu, string durasi, string total, string status, string pembayaran, int y, bool selesai)
        {
            AddText(table, no, 25, y + 10, 9, false);
            AddText(table, tanggal, 95, y + 10, 9, false);
            AddText(table, studio, 245, y + 10, 9, true);
            AddText(table, penyewa, 365, y + 10, 9, false);
            AddText(table, waktu, 525, y + 10, 9, false);
            AddText(table, durasi, 665, y + 10, 9, false);
            AddText(table, total, 775, y + 10, 9, false);

            Label st = new Label();
            st.Text = status;
            st.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            st.ForeColor = selesai ? Color.FromArgb(22, 163, 74) : Color.FromArgb(245, 158, 11);
            st.BackColor = selesai ? Color.FromArgb(220, 252, 231) : Color.FromArgb(255, 247, 237);
            st.TextAlign = ContentAlignment.MiddleCenter;
            st.Size = new Size(80, 25);
            st.Location = new Point(895, y + 5);
            table.Controls.Add(st);
            MakeRounded(st, 8);

            AddText(table, pembayaran, 1030, y + 3, 8, false);

            AddLine(table, 0, y + 45, 1155);
        }

        void AddPageTitle(string title, string subtitle)
        {
            AddText(contentPanel, title, 50, 70, 26, true);
            AddText(contentPanel, subtitle, 53, 125, 12, false, Color.FromArgb(100, 116, 139));
        }

        void Logout()
        {
            this.Close();
            Form1 login = new Form1();
            login.Show();
        }

        void AddText(Control parent, string text, int x, int y, int size, bool bold, Color? color = null)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular);
            lbl.ForeColor = color ?? Color.FromArgb(15, 23, 42);
            lbl.AutoSize = true;
            lbl.Location = new Point(x, y);
            lbl.BackColor = Color.Transparent;
            parent.Controls.Add(lbl);
        }

        void AddPic(Control parent, Image img, int x, int y, int w, int h)
        {
            PictureBox pic = new PictureBox();
            pic.Image = img;
            pic.Location = new Point(x, y);
            pic.Size = new Size(w, h);
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.BackColor = Color.Transparent;
            parent.Controls.Add(pic);
            pic.BringToFront();
        }

        void AddLine(Control parent, int x, int y, int w)
        {
            Panel line = new Panel();
            line.BackColor = Color.FromArgb(225, 232, 240);
            line.Size = new Size(w, 1);
            line.Location = new Point(x, y);
            parent.Controls.Add(line);
        }

        Panel RoundedPanel(int w, int h, int r, Color color)
        {
            Panel p = new Panel();
            p.Size = new Size(w, h);
            p.BackColor = color;

            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, r * 2, r * 2, 180, 90);
            path.AddArc(w - r * 2, 0, r * 2, r * 2, 270, 90);
            path.AddArc(w - r * 2, h - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(0, h - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();

            p.Region = new Region(path);
            return p;
        }

        void MakeRounded(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            path.AddArc(control.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
            path.AddArc(control.Width - radius * 2, control.Height - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(0, control.Height - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }
    }
}
