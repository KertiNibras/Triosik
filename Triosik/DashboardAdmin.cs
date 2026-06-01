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
            this.BackColor = Color.FromArgb(239, 247, 255);
            this.DoubleBuffered = true;

            sidebar = new Panel();
            sidebar.Width = 280;
            sidebar.Dock = DockStyle.Left;
            sidebar.BackColor = Color.FromArgb(31, 126, 224);
            this.Controls.Add(sidebar);

            contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.FromArgb(239, 247, 255);
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

            Panel menu = RoundedPanel(240, text.Contains("\n") ? 64 : 52, 10,
                active ? Color.FromArgb(255, 230, 55) : Color.FromArgb(31, 126, 224));

            menu.Location = new Point(20, y);
            menu.Cursor = Cursors.Hand;
            sidebar.Controls.Add(menu);

            AddPic(menu, icon, 17, 15, 26, 26);

            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Anek Devanagari", 12, FontStyle.Bold);
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

            Label title = new Label();
            title.Text = "Selamat datang, Admin! 👋";
            title.Font = new Font("Segoe UI", 25, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(50, 55);
            contentPanel.Controls.Add(title);

            Label sub = new Label();
            sub.Text = "Kelola studio Triosic Music Studio dengan mudah.";
            sub.Font = new Font("Segoe UI", 12);
            sub.ForeColor = Color.FromArgb(45, 55, 75);
            sub.AutoSize = true;
            sub.Location = new Point(53, 105);
            contentPanel.Controls.Add(sub);

            AddPic(contentPanel, Properties.Resources.Hitam, 1030, 50, 55, 55);

            Label admin = new Label();
            admin.Text = "Admin\nAdministrator";
            admin.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            admin.AutoSize = true;
            admin.Location = new Point(1090, 56);
            contentPanel.Controls.Add(admin);

            AddStatCard(50, 170, Color.White, Properties.Resources.card1icon, "Total Studio", "3", "Studio Aktif");
            AddStatCard(305, 170, Color.FromArgb(242, 253, 246), Properties.Resources.calendergreen, "Total Booking Masuk", "24", "Booking");
            AddStatCard(560, 170, Color.FromArgb(255, 249, 236), Properties.Resources.card2icon, "Booking Menunggu", "5", "Menunggu Konfirmasi");
            AddStatCard(815, 170, Color.FromArgb(250, 241, 255), Properties.Resources.card4icon, "Total Pendapatan", "Rp7.250.000", "Bulan Ini");

            AddRecentBookingPanel();
            AddQuickButtonPanel();
        }

        void ShowStudioPage()
        {
            contentPanel.Controls.Clear();

            Label title = new Label();
            title.Text = "Kelola Studio";
            title.Font = new Font("Segoe UI", 25, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(50, 65);
            contentPanel.Controls.Add(title);

            Label sub = new Label();
            sub.Text = "Kelola semua studio yang tersedia di Triosic Music Studio.";
            sub.Font = new Font("Segoe UI", 12);
            sub.ForeColor = Color.FromArgb(45, 55, 75);
            sub.AutoSize = true;
            sub.Location = new Point(52, 115);
            contentPanel.Controls.Add(sub);

            Button add = new Button();
            add.Text = "+  Tambah Studio";
            add.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            add.ForeColor = Color.White;
            add.BackColor = Color.FromArgb(0, 100, 230);
            add.FlatStyle = FlatStyle.Flat;
            add.FlatAppearance.BorderSize = 0;
            add.Size = new Size(210, 55);
            add.Location = new Point(1010, 65);
            contentPanel.Controls.Add(add);
            MakeRounded(add, 10);

            Panel table = RoundedPanel(1165, 520, 12, Color.White);
            table.Location = new Point(50, 180);
            contentPanel.Controls.Add(table);

            AddStudioHeader(table);
            AddStudioRow(table, 1, Properties.Resources.calendergreen, "Studio A", "Band Room", "Band Room", "Rp200.000", "6 Orang", 85);
            AddStudioRow(table, 2, Properties.Resources.mulaibookingicon, "Studio B", "Vocal Room", "Vocal Room", "Rp150.000", "3 Orang", 215);
            AddStudioRow(table, 3, Properties.Resources.musicpurple, "Studio C", "Instrument Room", "Instrument Room", "Rp125.000", "4 Orang", 345);

            Label page = new Label();
            page.Text = "‹    1    ›";
            page.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            page.ForeColor = Color.FromArgb(0, 100, 230);
            page.AutoSize = true;
            page.Location = new Point(525, 470);
            table.Controls.Add(page);
        }

        void AddStudioHeader(Panel table)
        {
            string[] headers = { "No", "Nama Studio", "Jenis Studio", "Harga / Jam", "Kapasitas", "Status", "Aksi" };
            int[] xs = { 35, 160, 360, 540, 720, 880, 1040 };

            for (int i = 0; i < headers.Length; i++)
            {
                Label h = new Label();
                h.Text = headers[i];
                h.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                h.AutoSize = true;
                h.Location = new Point(xs[i], 35);
                table.Controls.Add(h);
            }

            AddLine(table, 0, 75, 1165);
        }

        void AddStudioRow(Panel table, int no, Image icon, string nama, string subNama, string jenis, string harga, string kapasitas, int y)
        {
            Label noLbl = new Label();
            noLbl.Text = no.ToString();
            noLbl.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            noLbl.AutoSize = true;
            noLbl.Location = new Point(35, y + 35);
            table.Controls.Add(noLbl);

            AddPic(table, icon, 90, y + 20, 55, 55);

            Label namaLbl = new Label();
            namaLbl.Text = nama + "\n" + subNama;
            namaLbl.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            namaLbl.AutoSize = true;
            namaLbl.Location = new Point(160, y + 22);
            table.Controls.Add(namaLbl);

            AddText(table, jenis, 360, y + 35, 11, false);
            AddText(table, harga, 540, y + 35, 11, false);
            AddText(table, kapasitas, 720, y + 35, 11, false);

            Label status = new Label();
            status.Text = "Aktif";
            status.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            status.ForeColor = Color.FromArgb(0, 135, 65);
            status.BackColor = Color.FromArgb(222, 248, 230);
            status.TextAlign = ContentAlignment.MiddleCenter;
            status.Size = new Size(75, 35);
            status.Location = new Point(880, y + 28);
            table.Controls.Add(status);
            MakeRounded(status, 8);

            Button edit = SmallActionButton("✎", Color.FromArgb(0, 100, 230));
            edit.Location = new Point(1035, y + 25);
            table.Controls.Add(edit);

            Button del = SmallActionButton("🗑", Color.Red);
            del.Location = new Point(1100, y + 25);
            table.Controls.Add(del);

            AddLine(table, 0, y + 105, 1165);
        }

        Button SmallActionButton(string text, Color color)
        {
            Button b = new Button();
            b.Text = text;
            b.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            b.ForeColor = color;
            b.BackColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderColor = Color.FromArgb(215, 225, 240);
            b.Size = new Size(48, 45);
            return b;
        }

        void AddStatCard(int x, int y, Color bg, Image icon, string title, string value, string desc)
        {
            Panel card = RoundedPanel(230, 160, 12, bg);
            card.Location = new Point(x, y);
            contentPanel.Controls.Add(card);

            AddPic(card, icon, 25, 25, 45, 45);
            AddText(card, title, 85, 35, 10, true);
            AddText(card, value, 25, 85, value.Length > 8 ? 19 : 24, true);
            AddText(card, desc, 25, 125, 10, false);
        }

        void AddRecentBookingPanel()
        {
            Panel box = RoundedPanel(735, 375, 12, Color.White);
            box.Location = new Point(50, 360);
            contentPanel.Controls.Add(box);

            AddText(box, "Ringkasan Booking Terbaru", 25, 20, 13, true);

            Label lihat = new Label();
            lihat.Text = "Lihat Semua";
            lihat.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lihat.ForeColor = Color.FromArgb(0, 100, 230);
            lihat.AutoSize = true;
            lihat.Location = new Point(625, 23);
            box.Controls.Add(lihat);

            AddBookingRow(box, Properties.Resources.calendergreen, "Studio A", "Band Room", "29 Mei 2025 • 13:00 - 15:00 • 2 Jam", "Menunggu", Color.Orange, 70);
            AddBookingRow(box, Properties.Resources.mulaibookingicon, "Studio B", "Vocal Room", "29 Mei 2025 • 15:00 - 17:00 • 2 Jam", "Menunggu", Color.Orange, 145);
            AddBookingRow(box, Properties.Resources.musicpurple, "Studio C", "Instrument Room", "28 Mei 2025 • 10:00 - 12:00 • 2 Jam", "Dikonfirmasi", Color.Green, 220);
            AddBookingRow(box, Properties.Resources.calendergreen, "Studio A", "Band Room", "27 Mei 2025 • 19:00 - 22:00 • 3 Jam", "Selesai", Color.Green, 295);
        }

        void AddBookingRow(Panel parent, Image icon, string studio, string room, string info, string status, Color statusColor, int y)
        {
            AddPic(parent, icon, 25, y, 45, 45);

            AddText(parent, studio + " ", 90, y, 12, true);
            AddText(parent, "(" + room + ")", 165, y + 2, 10, false);
            AddText(parent, info, 90, y + 28, 10, false);

            Label st = new Label();
            st.Text = status;
            st.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            st.ForeColor = statusColor;
            st.BackColor = statusColor == Color.Orange ? Color.FromArgb(255, 244, 225) : Color.FromArgb(225, 247, 235);
            st.TextAlign = ContentAlignment.MiddleCenter;
            st.Size = new Size(110, 30);
            st.Location = new Point(600, y + 10);
            parent.Controls.Add(st);
            MakeRounded(st, 12);
        }

        void AddQuickButtonPanel()
        {
            Panel box = RoundedPanel(430, 375, 12, Color.White);
            box.Location = new Point(815, 360);
            contentPanel.Controls.Add(box);

            AddText(box, "Tombol Cepat", 25, 20, 13, true);
            AddQuickButton(box, "+", "Tambah Studio", "Tambah studio baru", 70, Color.FromArgb(0, 100, 230));
            AddQuickButton(box, "▣", "Atur Jadwal Operasional", "Kelola jam operasional studio", 155, Color.FromArgb(0, 100, 230));
            AddQuickButton(box, "▣", "Cek Booking", "Lihat & konfirmasi booking masuk", 240, Color.FromArgb(0, 150, 80));
        }

        void AddQuickButton(Panel parent, string icon, string title, string desc, int y, Color color)
        {
            Panel btn = RoundedPanel(380, 70, 10, Color.FromArgb(240, 248, 255));
            btn.Location = new Point(25, y);
            parent.Controls.Add(btn);

            AddText(btn, icon, 25, 12, 26, true, color);
            AddText(btn, title, 85, 14, 12, true, color);
            AddText(btn, desc, 85, 40, 10, false);
        }

        void ShowJadwalOperasionalPage()
        {
            contentPanel.Controls.Clear();
            AddPageTitle("Kelola Jadwal Operasional", "Atur jam operasional studio.");
        }

        void ShowKonfirmasiPage()
        {
            contentPanel.Controls.Clear();
            AddPageTitle("Konfirmasi Booking", "Lihat dan konfirmasi booking masuk.");
        }

        void ShowLaporanPage()
        {
            contentPanel.Controls.Clear();
            AddPageTitle("Laporan Pemesanan", "Lihat laporan pemesanan studio.");
        }

        void AddPageTitle(string title, string subtitle)
        {
            AddText(contentPanel, title, 50, 70, 26, true);
            AddText(contentPanel, subtitle, 53, 125, 12, false);
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