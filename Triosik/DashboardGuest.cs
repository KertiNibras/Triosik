using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Triosik
{
    public partial class DashboardGuest : Form
    {
        Panel sidebar;
        Panel contentPanel;
        Panel wrapperPanel;
        string activeMenu = "Dashboard";

        public DashboardGuest()
        {
            InitializeComponent();
            BuildUI();
        }

        private void DashboardGuest_Load(object sender, EventArgs e) { }

        void BuildUI()
        {
            this.Text = "Dashboard Guest";
            this.FormBorderStyle = FormBorderStyle.None; // Kiosk mode
            this.WindowState = FormWindowState.Maximized; // Fullscreen
            this.BackColor = Color.FromArgb(200, 224, 249);
            this.DoubleBuffered = true;

            // 1. Setup Sidebar
    sidebar = new Panel();
    sidebar.Width = 280;
    sidebar.Dock = DockStyle.Left;
    sidebar.BackColor = Color.FromArgb(31, 126, 224);
    this.Controls.Add(sidebar);

    // 2. Setup Content Panel (Ini JANGAN diganti namanya, tetap contentPanel)
    contentPanel = new Panel();
    contentPanel.Dock = DockStyle.Fill; // Mengisi sisa layar penuh
    contentPanel.BackColor = Color.FromArgb(200, 224, 249);
    this.Controls.Add(contentPanel);

    // 3. Setup Wrapper Panel (Ini baru bungkus desainmu)
    wrapperPanel = new Panel();
    wrapperPanel.Size = new Size(1086, 768); 
    wrapperPanel.BackColor = Color.Transparent;
    contentPanel.Controls.Add(wrapperPanel); // <--- Masukkan wrapper ke dalam content

    // 4. Trik biar wrapper selalu di tengah contentPanel
    contentPanel.Resize += (s, ev) =>
    {
        wrapperPanel.Location = new Point(
            (contentPanel.Width - wrapperPanel.Width) / 2,
            (contentPanel.Height - wrapperPanel.Height) / 2
        );
    };
            // --- Sidebar design tetep masuk ke sidebar ---
            AddPic(sidebar, Properties.Resources.Hitam, 50, 95, 55, 55);
            AddPic(sidebar, Properties.Resources.Kuning, 105, 65, 70, 70);
            AddPic(sidebar, Properties.Resources.Pink, 168, 95, 60, 60);

            Label logoTriosic = new Label();
            logoTriosic.Text = "Triosic";
            logoTriosic.Font = new Font("Chubby And Groovy", 30, FontStyle.Bold);
            logoTriosic.ForeColor = Color.White;
            logoTriosic.AutoSize = true;
            logoTriosic.Location = new Point(70, 155);
            sidebar.Controls.Add(logoTriosic);

            Label logoStudio = new Label();
            logoStudio.Text = "music studio";
            logoStudio.Font = new Font("Ground Castle DEMO", 23, FontStyle.Bold);
            logoStudio.ForeColor = Color.White;
            logoStudio.AutoSize = true;
            logoStudio.Location = new Point(40, 190);
            sidebar.Controls.Add(logoStudio);

            SetActiveMenu("Dashboard");
            ShowDashboardPage();
        }
        void ShowDashboardPage()
        {
            wrapperPanel.Controls.Clear();

            Label title = new Label();
            title.Text = "Welcome, Guest!";
            title.Font = new Font("Chubby And Groovy", 30, FontStyle.Bold);
            title.ForeColor = Color.Black;
            title.AutoSize = true;
            title.Location = new Point(85, 70);
            wrapperPanel.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "Selamat datang di Triosic Music Studio.\nYuk, buat pengalaman musikmu jadi lebih seru!";
            subtitle.Font = new Font("Anek Devanagari", 11);
            subtitle.AutoSize = true;
            subtitle.Location = new Point(90, 125);
            wrapperPanel.Controls.Add(subtitle);

            AddPic(wrapperPanel, Properties.Resources.Hitam, 935, 45, 80, 80);

            AddTopCard(90, 190, Color.FromArgb(235, 246, 255), Properties.Resources.card1icon, "0", "Jadwal Hari Ini", "Lihat jadwal studio hari ini.");
            AddTopCard(325, 190, Color.FromArgb(255, 249, 231), Properties.Resources.card2icon, "09:00 - 22:00", "Jam Operasional", "Kami siap melayani kamu.");
            AddTopCard(565, 190, Color.FromArgb(238, 250, 230), Properties.Resources.card3icon, "Studio Utama", "Studio Populer", "Paling banyak dibooking.");
            AddTopCard(805, 190, Color.FromArgb(248, 235, 255), Properties.Resources.card4icon, "Akses Cepat", "Booking Tanpa Login", "Booking tanpa login.");

            AddSchedulePanel();
            AddWhyPanel();
            AddBookingPanel();
        }

        void ShowSchedulePage()
        {
            wrapperPanel.Controls.Clear();

            Label title = new Label();
            title.Text = "Schedule";
            title.Font = new Font("Chubby And Groovy", 36, FontStyle.Bold);
            title.ForeColor = Color.Black;
            title.AutoSize = true;
            title.Location = new Point(85, 70);
            wrapperPanel.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "Lihat semua jadwal studio yang tersedia hari ini.";
            subtitle.Font = new Font("Anek Devanagari", 13);
            subtitle.AutoSize = true;
            subtitle.Location = new Point(90, 125);
            wrapperPanel.Controls.Add(subtitle);

            AddPic(wrapperPanel, Properties.Resources.Hitam, 935, 45, 80, 80);

            Panel filterBox = RoundedPanel(990, 95, 15, Color.White);
            filterBox.Location = new Point(90, 200);
            wrapperPanel.Controls.Add(filterBox);

            Label tgl = new Label();
            tgl.Text = "Pilih Tanggal";
            tgl.Font = new Font("Anek Devanagari", 10);
            tgl.Location = new Point(30, 20);
            tgl.AutoSize = true;
            filterBox.Controls.Add(tgl);

            TextBox inputTanggal = new TextBox();
            inputTanggal.Location = new Point(30, 45);
            inputTanggal.Size = new Size(250, 32);
            inputTanggal.Font = new Font("Anek Devanagari", 12);
            filterBox.Controls.Add(inputTanggal);

            AddPic(filterBox, Properties.Resources.card1icon, 35, 50, 22, 22);

            Label studio = new Label();
            studio.Text = "Pilih Studio (Opsional)";
            studio.Font = new Font("Anek Devanagari", 10);
            studio.Location = new Point(320, 20);
            studio.AutoSize = true;
            filterBox.Controls.Add(studio);

            ComboBox cmbStudio = new ComboBox();
            cmbStudio.Location = new Point(320, 45);
            cmbStudio.Size = new Size(270, 32);
            cmbStudio.Font = new Font("Anek Devanagari", 11);
            cmbStudio.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStudio.Items.Add("Semua Studio");
            cmbStudio.Items.Add("Studio 1");
            cmbStudio.Items.Add("Studio 2");
            cmbStudio.Items.Add("Studio Utama");
            cmbStudio.SelectedIndex = 0;
            filterBox.Controls.Add(cmbStudio);

            AddLegend(filterBox, Color.FromArgb(120, 200, 145), "Tersedia", 610);
            AddLegend(filterBox, Color.FromArgb(235, 120, 140), "Terbooking", 693);
            AddLegend(filterBox, Color.LightGray, "Tidak Tersedia", 790);

            Button cari = new Button();
            cari.Size = new Size(70, 40);
            cari.Location = new Point(900, 40);
            cari.FlatStyle = FlatStyle.Flat;
            cari.BackColor = Color.White;
            filterBox.Controls.Add(cari);

            Panel table = RoundedPanel(1040, 430, 15, Color.White);
            table.Location = new Point(25, 320);
            wrapperPanel.Controls.Add(table);

            AddScheduleHeader(table);
            AddScheduleTimeRows(table);
        }

        void SetActiveMenu(string menuName)
        {
            activeMenu = menuName;

            sidebar.Controls.Clear();

            AddPic(sidebar, Properties.Resources.Hitam, 50, 95, 55, 55);
            AddPic(sidebar, Properties.Resources.Kuning, 105, 65, 70, 70);
            AddPic(sidebar, Properties.Resources.Pink, 168, 95, 60, 60);

            Label logoTriosic = new Label();
            logoTriosic.Text = "Triosic";
            logoTriosic.Font = new Font("Chubby And Groovy", 30, FontStyle.Bold);
            logoTriosic.ForeColor = Color.White;
            logoTriosic.AutoSize = true;
            logoTriosic.Location = new Point(70, 155);
            sidebar.Controls.Add(logoTriosic);

            Label logoStudio = new Label();
            logoStudio.Text = "music studio";
            logoStudio.Font = new Font("Ground Castle DEMO", 23, FontStyle.Bold);
            logoStudio.ForeColor = Color.White;
            logoStudio.AutoSize = true;
            logoStudio.Location = new Point(40, 190);
            sidebar.Controls.Add(logoStudio);

            AddMenu(sidebar, Properties.Resources.HomeIcon, "Dashboard", 260, ShowDashboardPage);

            AddMenu(sidebar, Properties.Resources.Jadwalicon, "Lihat Jadwal", 325, ShowSchedulePage);

            AddMenu(sidebar, Properties.Resources.Bookingicon, "Booking Sekarang", 380, ShowBookingPage);

            AddMenu(sidebar, Properties.Resources.Logouticon, "Logout", 435, Logout);
        }

        void ShowBookingPage()
        {
            wrapperPanel.Controls.Clear();

            Label title = new Label();
            title.Text = "Booking Sekarang";
            title.Font = new Font("Chubby And Groovy", 36, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(45, 70);
            wrapperPanel.Controls.Add(title);

            Label info = new Label();
            info.Text = "Halaman form booking nanti taruh di sini.";
            info.Font = new Font("Anek Devanagari", 14);
            info.AutoSize = true;
            info.Location = new Point(50, 140);
            wrapperPanel.Controls.Add(info);
        }

        void Logout()
        {
            Dispose();
            Form1 login = new Form1();
            login.Show();
        }

        void AddMenu(Panel parent, Image icon, string text, int y, Action clickAction)
        {
            bool active = activeMenu == text;

            Panel menu = RoundedPanel(
                210,
                48,
                10,
                active
                ? Color.FromArgb(255, 230, 55)
                : Color.FromArgb(31, 126, 224)
            );

            menu.Location = new Point(35, y);
            menu.Cursor = Cursors.Hand;
            parent.Controls.Add(menu);

            AddPic(menu, icon, 15, 12, 24, 24);

            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Anek Devanagari", 12, FontStyle.Bold);
            lbl.ForeColor = active ? Color.Black : Color.White;
            lbl.AutoSize = true;
            lbl.Location = new Point(55, 13);
            lbl.Cursor = Cursors.Hand;
            menu.Controls.Add(lbl);

            menu.Click += (s, e) =>
            {
                SetActiveMenu(text);
                clickAction();
            };

            lbl.Click += (s, e) =>
            {
                SetActiveMenu(text);
                clickAction();
            };
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

        void AddTopCard(int x, int y, Color bg, Image icon, string big, string title, string desc)
        {
            Panel card = RoundedPanel(210, 180, 15, bg);
            card.Location = new Point(x, y);
            wrapperPanel.Controls.Add(card);

            AddPic(card, icon, 25, 25, 45, 45);

            Label bigLbl = new Label();
            bigLbl.Text = big;
            bigLbl.Font = new Font("Anek Devanagari", 17, FontStyle.Bold);
            bigLbl.AutoSize = true;
            bigLbl.Location = new Point(25, 85);
            card.Controls.Add(bigLbl);

            Label titleLbl = new Label();
            titleLbl.Text = title;
            titleLbl.Font = new Font("Anek Devanagari", 11, FontStyle.Bold);
            titleLbl.AutoSize = true;
            titleLbl.Location = new Point(25, 118);
            card.Controls.Add(titleLbl);

            Label descLbl = new Label();
            descLbl.Text = desc;
            descLbl.Font = new Font("Anek Devanagari", 9);
            descLbl.AutoSize = true;
            descLbl.Location = new Point(25, 145);
            card.Controls.Add(descLbl);
        }

        void AddSchedulePanel()
        {
            Panel box = RoundedPanel(560, 360, 15, Color.White);
            box.Location = new Point(55, 400);
            wrapperPanel.Controls.Add(box);

            Label title = new Label();
            title.Text = "Jadwal Hari Ini";
            title.Font = new Font("Chubby And Groovy", 16);
            title.AutoSize = true;
            title.Location = new Point(25, 20);
            box.Controls.Add(title);

            Label lihat = new Label();
            lihat.Text = "Lihat Semua";
            lihat.Font = new Font("Anek Devanagari", 11);
            lihat.ForeColor = Color.FromArgb(31, 126, 224);
            lihat.AutoSize = true;
            lihat.Location = new Point(450, 25);
            lihat.Cursor = Cursors.Hand;
            lihat.Click += (s, e) => ShowSchedulePage();
            box.Controls.Add(lihat);

            AddScheduleRow(box, "09:00 - 10:00", 70);
            AddScheduleRow(box, "10:00 - 11:00", 125);
            AddScheduleRow(box, "11:00 - 12:00", 180);
            AddScheduleRow(box, "12:00 - 13:00", 235);
            AddScheduleRow(box, "13:00 - 14:00", 290);
        }

        void AddScheduleRow(Panel parent, string time, int y)
        {
            Panel row = RoundedPanel(500, 45, 10, Color.White);
            row.Location = new Point(30, y);
            parent.Controls.Add(row);

            row.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.LightGray, 2))
                    e.Graphics.DrawRectangle(pen, 0, 0, row.Width - 1, row.Height - 1);
            };

            AddPic(row, Properties.Resources.clock, 18, 10, 25, 25);

            Label lbl = new Label();
            lbl.Text = time;
            lbl.Font = new Font("Anek Devanagari", 13, FontStyle.Bold);
            lbl.AutoSize = true;
            lbl.Location = new Point(60, 11);
            row.Controls.Add(lbl);

            AddPic(row, Properties.Resources.arrowblack, 455, 14, 22, 18);
        }

        void AddWhyPanel()
        {
            Panel box = RoundedPanel(420, 230, 15, Color.White);
            box.Location = new Point(635, 400);
            wrapperPanel.Controls.Add(box);

            Label title = new Label();
            title.Text = "Kenapa Pilih Triosik?";
            title.Font = new Font("Chubby And Groovy", 14);
            title.AutoSize = true;
            title.Location = new Point(20, 18);
            box.Controls.Add(title);

            AddWhyItem(box, Properties.Resources.Secureicon, "Peralatan Berkualitas", "Dilengkapi alat musik dan sound system terbaik.", 60);
            AddWhyItem(box, Properties.Resources.musicpurple, "Studio Nyaman", "Ruang kedap suara dan nyaman untuk berkreasi.", 105);
            AddWhyItem(box, Properties.Resources.calendergreen, "Booking Mudah", "Proses cepat dan praktis tanpa ribet.", 150);
            AddWhyItem(box, Properties.Resources.smileyellow, "Cocok untuk Semua", "Untuk latihan pribadi, band, rekaman, dan kebutuhan lain.", 190);
        }

        void AddWhyItem(Panel p, Image icon, string title, string desc, int y)
        {
            AddPic(p, icon, 25, y, 28, 28);

            Label lbl = new Label();
            lbl.Text = title + "\n" + desc;
            lbl.Font = new Font("Anek Devanagari", 8, FontStyle.Bold);
            lbl.AutoSize = true;
            lbl.Location = new Point(65, y - 1);
            p.Controls.Add(lbl);
        }

        void AddBookingPanel()
        {
            Panel box = RoundedPanel(420, 118, 15, Color.White);
            box.Location = new Point(635, 640);
            wrapperPanel.Controls.Add(box);

            Label title = new Label();
            title.Text = "Mulai Bermusik";
            title.Font = new Font("Chubby And Groovy", 14);
            title.AutoSize = true;
            title.Location = new Point(20, 12);
            box.Controls.Add(title);

            Panel btn = RoundedPanel(375, 50, 10, Color.FromArgb(225, 239, 255));
            btn.Location = new Point(23, 42);
            btn.Cursor = Cursors.Hand;
            btn.Click += (s, e) => ShowBookingPage();
            box.Controls.Add(btn);

            AddPic(btn, Properties.Resources.mulaibookingicon, 18, 15, 24, 24);

            Label text = new Label();
            text.Text = "Booking Studio Sekarang";
            text.Font = new Font("Anek Devanagari", 9, FontStyle.Bold);
            text.ForeColor = Color.FromArgb(31, 126, 224);
            text.AutoSize = true;
            text.Location = new Point(55, 18);
            text.Cursor = Cursors.Hand;
            text.Click += (s, e) => ShowBookingPage();
            btn.Controls.Add(text);

            AddPic(btn, Properties.Resources.arrowblue, 340, 16, 20, 18);
        }

        void AddLegend(Control parent, Color color, string text, int x)
        {
            Panel dot = new Panel();
            dot.Size = new Size(12, 12);
            dot.Location = new Point(x, 55);
            dot.BackColor = color;
            parent.Controls.Add(dot);

            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Anek Devanagari", 9);
            lbl.AutoSize = true;
            lbl.Location = new Point(x + 20, 50);
            parent.Controls.Add(lbl);
        }

        void AddScheduleHeader(Panel table)
        {
            Label waktu = new Label();
            waktu.Text = "Waktu";
            waktu.Font = new Font("Anek Devanagari", 11, FontStyle.Bold);
            waktu.Location = new Point(55, 30);
            waktu.AutoSize = true;
            table.Controls.Add(waktu);

            AddStudioHeader(table, Color.FromArgb(120, 200, 145), "Studio 1", "(Regular)", 260);
            AddStudioHeader(table, Color.FromArgb(255, 225, 120), "Studio 2", "(Vip)", 550);
            AddStudioHeader(table, Color.FromArgb(255, 120, 90), "Studio Utama", "(Vvip)", 850);
        }

        void AddStudioHeader(Panel table, Color color, string name, string type, int x)
        {
            Panel dot = new Panel();
            dot.Size = new Size(12, 12);
            dot.Location = new Point(x, 28);
            dot.BackColor = color;
            table.Controls.Add(dot);

            Label lbl = new Label();
            lbl.Text = name + "\n" + type;
            lbl.Font = new Font("Anek Devanagari", 9, FontStyle.Bold);
            lbl.AutoSize = true;
            lbl.Location = new Point(x + 20, 22);
            table.Controls.Add(lbl);
        }

        void AddScheduleTimeRows(Panel table)
        {
            string[] times =
            {
                "09:00 - 10:00",
                "10:00 - 11:00",
                "11:00 - 12:00",
                "12:00 - 13:00",
                "13:00 - 14:00",
                "14:00 - 15:00",
                "15:00 - 16:00",
                "16:00 - 17:00",
                "17:00 - 18:00",
                "18:00 - 19:00",
                "19:00 - 20:00",
                "20:00 - 21:00"
            };

            int y = 85;

            foreach (string time in times)
            {
                Label lbl = new Label();
                lbl.Text = time;
                lbl.Font = new Font("Anek Devanagari", 11);
                lbl.AutoSize = true;
                lbl.Location = new Point(55, y);
                table.Controls.Add(lbl);
                y += 34;
            }
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
    }
}