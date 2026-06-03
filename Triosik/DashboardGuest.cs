using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Triosik
{
    public partial class DashboardGuest : Form
    {
        Panel sidebar, contentPanel, wrapperPanel;
        string activeMenu = "Dashboard";

        readonly Color Blue = Color.FromArgb(31, 126, 224);
        readonly Color Yellow = Color.FromArgb(255, 230, 55);
        readonly Color Green = Color.FromArgb(120, 200, 145);
        readonly Color Red = Color.FromArgb(235, 120, 140);
        readonly Color Gray = Color.LightGray;

        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Triosik;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        int selectedStudioId = 1;
        int selectedHargaPerJam = 75000;
        string selectedStudioName = "Studio 1";
        Panel selectedStudioCard = null;

        public DashboardGuest()
        {
            InitializeComponent();
            BuildUI();
        }

        private void DashboardGuest_Load(object sender, EventArgs e) { }

        void BuildUI()
        {
            this.Text = "Dashboard Guest";
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(200, 224, 249);
            this.DoubleBuffered = true;

            sidebar = new Panel();
            sidebar.Width = 280;
            sidebar.Dock = DockStyle.Left;
            sidebar.BackColor = Blue;
            this.Controls.Add(sidebar);

            contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.FromArgb(200, 224, 249);
            this.Controls.Add(contentPanel);

            wrapperPanel = new Panel();
            wrapperPanel.Size = new Size(1280, 820);
            wrapperPanel.BackColor = Color.Transparent;
            contentPanel.Controls.Add(wrapperPanel);

            contentPanel.Resize += (s, e) =>
            {
                wrapperPanel.Location = new Point(
                    Math.Max(190, (contentPanel.Width - wrapperPanel.Width) / 2 + 180),
                    Math.Max(15, (contentPanel.Height - wrapperPanel.Height) / 2)
                );
            };

            wrapperPanel.Location = new Point(
                Math.Max(190, (contentPanel.Width - wrapperPanel.Width) / 2 + 180),
                Math.Max(15, (contentPanel.Height - wrapperPanel.Height) / 2)
            );

            SetActiveMenu("Dashboard");
            ShowDashboardPage();
        }

        void SetActiveMenu(string menuName)
        {
            activeMenu = menuName;
            sidebar.Controls.Clear();

            AddPic(sidebar, Properties.Resources.Hitam, 40, 85, 65, 65);
            AddPic(sidebar, Properties.Resources.Kuning, 105, 55, 85, 85);
            AddPic(sidebar, Properties.Resources.Pink, 180, 95, 65, 65);

            Label logoTriosic = new Label();
            logoTriosic.Text = "Triosic";
            logoTriosic.Font = new Font("Chubby And Groovy", 32, FontStyle.Bold);
            logoTriosic.ForeColor = Color.White;
            logoTriosic.AutoSize = true;
            logoTriosic.Location = new Point(65, 155);
            sidebar.Controls.Add(logoTriosic);

            Label logoStudio = new Label();
            logoStudio.Text = "music studio";
            logoStudio.Font = new Font("Ground Castle DEMO", 24, FontStyle.Bold);
            logoStudio.ForeColor = Color.White;
            logoStudio.AutoSize = true;
            logoStudio.Location = new Point(42, 198);
            sidebar.Controls.Add(logoStudio);

            AddMenu(Properties.Resources.HomeIcon, "Dashboard", 285, ShowDashboardPage);
            AddMenu(Properties.Resources.Jadwalicon, "Lihat Jadwal", 355, ShowSchedulePage);
            AddMenu(Properties.Resources.Bookingicon, "Booking Form", 425, ShowBookingPage);
            AddMenu(Properties.Resources.Logouticon, "Logout", 495, Logout);
        }

        void AddMenu(Image icon, string text, int y, Action clickAction)
        {
            bool active = activeMenu == text;

            Panel menu = RoundedPanel(230, 54, 10, active ? Yellow : Blue);
            menu.Location = new Point(25, y);
            menu.Cursor = Cursors.Hand;
            sidebar.Controls.Add(menu);

            AddPic(menu, RecolorIcon(icon, active ? Blue : Color.White), 17, 14, 26, 26);

            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Anek Devanagari", 13, FontStyle.Bold);
            lbl.ForeColor = active ? Color.Black : Color.White;
            lbl.AutoSize = true;
            lbl.Location = new Point(62, 14);
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

        void ShowDashboardPage()
        {
            wrapperPanel.Controls.Clear();

            Label title = new Label();
            title.Text = "Welcome, Guest!";
            title.Font = new Font("Chubby And Groovy", 42, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(55, 45);
            wrapperPanel.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "Selamat datang di Triosic Music Studio.\nYuk, buat pengalaman musikmu jadi lebih seru!";
            subtitle.Font = new Font("Anek Devanagari", 15);
            subtitle.AutoSize = true;
            subtitle.Location = new Point(60, 120);
            wrapperPanel.Controls.Add(subtitle);

            AddPic(wrapperPanel, Properties.Resources.Hitam, 1090, 45, 95, 95);

            AddTopCard(55, 200, Color.FromArgb(235, 246, 255), Properties.Resources.card1icon, "0", "Jadwal Hari Ini", "Lihat jadwal studio hari ini.");
            AddTopCard(320, 200, Color.FromArgb(255, 249, 231), Properties.Resources.card2icon, "09:00 - 22:00", "Jam Operasional", "Kami siap melayani kamu.");
            AddTopCard(585, 200, Color.FromArgb(238, 250, 230), Properties.Resources.card3icon, "Studio Utama", "Studio Populer", "Paling banyak dibooking.");
            AddTopCard(850, 200, Color.FromArgb(248, 235, 255), Properties.Resources.card4icon, "Akses Cepat", "Booking Tanpa Login", "Booking tanpa login.");

            AddSchedulePanel();
            AddWhyPanel();
            AddBookingPanel();
        }

        void AddTopCard(int x, int y, Color bg, Image icon, string big, string title, string desc)
        {
            Panel card = RoundedPanel(235, 190, 15, bg);
            card.Location = new Point(x, y);
            wrapperPanel.Controls.Add(card);

            AddPic(card, icon, 28, 25, 55, 55);

            AddLabel(card, big, 28, 92, 21, true, Color.Black);
            AddLabel(card, title, 28, 132, 13, true, Color.Black);
            AddLabel(card, desc, 28, 162, 10, false, Color.Black);
        }

        void AddSchedulePanel()
        {
            Panel box = RoundedPanel(650, 390, 15, Color.White);
            box.Location = new Point(15, 420);
            wrapperPanel.Controls.Add(box);

            AddLabel(box, "Jadwal Hari Ini", 30, 22, 18, true, Color.Black, "Chubby And Groovy");

            Label lihat = AddLabel(box, "Lihat Semua", 530, 25, 13, false, Blue);
            lihat.Cursor = Cursors.Hand;
            lihat.Click += (s, e) =>
            {
                SetActiveMenu("Lihat Jadwal");
                ShowSchedulePage();
            };

            AddScheduleRow(box, "09:00 - 10:00", 80);
            AddScheduleRow(box, "10:00 - 11:00", 138);
            AddScheduleRow(box, "11:00 - 12:00", 196);
            AddScheduleRow(box, "12:00 - 13:00", 254);
            AddScheduleRow(box, "13:00 - 14:00", 312);
        }

        void AddScheduleRow(Panel parent, string time, int y)
        {
            Panel row = RoundedPanel(585, 45, 10, Color.White);
            row.Location = new Point(32, y);
            parent.Controls.Add(row);

            row.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.LightGray, 2))
                    e.Graphics.DrawRectangle(pen, 0, 0, row.Width - 1, row.Height - 1);
            };

            AddPic(row, Properties.Resources.clock, 20, 10, 25, 25);
            AddLabel(row, time, 70, 9, 14, true, Color.Black);
            AddPic(row, Properties.Resources.arrowblack, 535, 14, 22, 18);
        }

        void AddWhyPanel()
        {
            Panel box = RoundedPanel(460, 240, 15, Color.White);
            box.Location = new Point(685, 420);
            wrapperPanel.Controls.Add(box);

            AddLabel(box, "Kenapa Pilih Triosik?", 25, 18, 16, true, Color.Black, "Chubby And Groovy");

            AddWhyItem(box, Properties.Resources.Secureicon, "Peralatan Berkualitas", "Dilengkapi alat musik dan sound system terbaik.", 62);
            AddWhyItem(box, Properties.Resources.musicpurple, "Studio Nyaman", "Ruang kedap suara dan nyaman untuk berkreasi.", 105);
            AddWhyItem(box, Properties.Resources.calendergreen, "Booking Mudah", "Proses cepat dan praktis tanpa ribet.", 145);
            AddWhyItem(box, Properties.Resources.smileyellow, "Cocok untuk Semua", "Untuk latihan pribadi, band, rekaman, dan kebutuhan lain.", 185);
        }

        void AddWhyItem(Panel p, Image icon, string title, string desc, int y)
        {
            AddPic(p, icon, 25, y, 30, 30);
            AddLabel(p, title + "\n" + desc, 70, y - 2, 9, true, Color.Black);
        }

        void AddBookingPanel()
        {
            Panel box = RoundedPanel(460, 135, 15, Color.White);
            box.Location = new Point(685, 675);
            wrapperPanel.Controls.Add(box);

            AddLabel(box, "Mulai Bermusik", 25, 12, 16, true, Color.Black, "Chubby And Groovy");

            Panel btn = RoundedPanel(410, 52, 10, Color.FromArgb(225, 239, 255));
            btn.Location = new Point(25, 55);
            btn.Cursor = Cursors.Hand;
            btn.Click += (s, e) =>
            {
                SetActiveMenu("Booking Form");
                ShowBookingPage();
            };
            box.Controls.Add(btn);

            AddPic(btn, Properties.Resources.mulaibookingicon, 20, 13, 27, 27);
            AddLabel(btn, "Booking Studio Sekarang", 60, 17, 10, true, Blue);
            AddPic(btn, Properties.Resources.arrowblue, 380, 16, 22, 20);
        }

        void ShowSchedulePage()
        {
            wrapperPanel.Controls.Clear();

            AddLabel(wrapperPanel, "Schedule", 55, 45, 42, true, Color.Black, "Chubby And Groovy");
            AddLabel(wrapperPanel, "Lihat semua jadwal studio yang tersedia hari ini.", 60, 120, 15, false, Color.Black);
            AddPic(wrapperPanel, Properties.Resources.Hitam, 1090, 45, 95, 95);

            Panel filterBox = RoundedPanel(1120, 105, 15, Color.White);
            filterBox.Location = new Point(45, 170);
            wrapperPanel.Controls.Add(filterBox);

            AddLabel(filterBox, "Pilih Tanggal", 35, 18, 11, false, Color.Black);

            DateTimePicker inputTanggal = new DateTimePicker();
            inputTanggal.Location = new Point(35, 50);
            inputTanggal.Size = new Size(250, 34);
            inputTanggal.Font = new Font("Anek Devanagari", 12);
            inputTanggal.Format = DateTimePickerFormat.Short;
            filterBox.Controls.Add(inputTanggal);

            AddLabel(filterBox, "Pilih Studio (Opsional)", 330, 18, 11, false, Color.Black);

            ComboBox cmbStudio = new ComboBox();
            cmbStudio.Location = new Point(330, 50);
            cmbStudio.Size = new Size(270, 34);
            cmbStudio.Font = new Font("Anek Devanagari", 11);
            cmbStudio.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStudio.Items.Add("Semua Studio");
            cmbStudio.Items.Add("Studio 1");
            cmbStudio.Items.Add("Studio 2");
            cmbStudio.Items.Add("Studio Utama");
            cmbStudio.SelectedIndex = 0;
            filterBox.Controls.Add(cmbStudio);

            AddLegend(filterBox, Green, "Tersedia", 650);
            AddLegend(filterBox, Red, "Dibooking", 760);
            AddLegend(filterBox, Gray, "Tidak Tersedia", 870);

            Button cari = new Button();
            cari.Text = "Cari";
            cari.Size = new Size(80, 38);
            cari.Location = new Point(1010, 45);
            cari.FlatStyle = FlatStyle.Flat;
            cari.BackColor = Color.White;
            cari.Cursor = Cursors.Hand;
            filterBox.Controls.Add(cari);

            Panel table = RoundedPanel(1120, 525, 15, Color.White);
            table.Location = new Point(45, 290);
            wrapperPanel.Controls.Add(table);

            Action load = () =>
            {
                table.Controls.Clear();
                AddScheduleHeader(table);
                AddScheduleTimeRowsFromBooking(table, inputTanggal.Value.Date, cmbStudio.Text);
            };

            cari.Click += (s, e) => load();
            load();
        }

        void AddScheduleHeader(Panel table)
        {
            AddLabel(table, "Waktu", 55, 30, 11, true, Color.Black);
            AddStudioHeader(table, Green, "Studio 1", "(Regular)", 270);
            AddStudioHeader(table, Color.FromArgb(255, 225, 120), "Studio 2", "(VIP)", 580);
            AddStudioHeader(table, Color.FromArgb(255, 120, 90), "Studio Utama", "(VVIP)", 900);
        }

        void AddStudioHeader(Panel table, Color color, string name, string type, int x)
        {
            Panel dot = new Panel();
            dot.Size = new Size(13, 13);
            dot.Location = new Point(x, 30);
            dot.BackColor = color;
            table.Controls.Add(dot);

            AddLabel(table, name, x + 20, 23, 10, true, Color.Black);
            AddLabel(table, type, x + 20, 42, 8, false, Color.Black);
        }

        void AddScheduleTimeRowsFromBooking(Panel table, DateTime tanggal, string studioFilter)
        {
            string[] times =
            {
                "09:00 - 10:00", "10:00 - 11:00", "11:00 - 12:00",
                "12:00 - 13:00", "13:00 - 14:00", "14:00 - 15:00",
                "15:00 - 16:00", "16:00 - 17:00", "17:00 - 18:00",
                "18:00 - 19:00", "19:00 - 20:00", "20:00 - 21:00",
                "21:00 - 22:00"
            };

            int y = 80;

            foreach (string time in times)
            {
                string[] split = time.Split('-');
                TimeSpan mulai = TimeSpan.Parse(split[0].Trim());
                TimeSpan selesai = TimeSpan.Parse(split[1].Trim());

                AddLabel(table, time, 55, y, 11, false, Color.Black);

                AddStatusCard(table, 245, y - 5, CekStatusStudio(1, tanggal, mulai, selesai, studioFilter));
                AddStatusCard(table, 555, y - 5, CekStatusStudio(2, tanggal, mulai, selesai, studioFilter));
                AddStatusCard(table, 875, y - 5, CekStatusStudio(3, tanggal, mulai, selesai, studioFilter));

                y += 34;
            }
        }

        string CekStatusStudio(int idStudio, DateTime tanggal, TimeSpan mulai, TimeSpan selesai, string studioFilter)
        {
            if (studioFilter == "Studio 1" && idStudio != 1) return "Tidak Tersedia";
            if (studioFilter == "Studio 2" && idStudio != 2) return "Tidak Tersedia";
            if (studioFilter == "Studio Utama" && idStudio != 3) return "Tidak Tersedia";

            if (mulai < TimeSpan.Parse("09:00") || selesai > TimeSpan.Parse("22:00"))
                return "Tidak Tersedia";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string statusStudioQuery = "SELECT status FROM dbo.Studio WHERE id_studio = @id_studio";
                    SqlCommand statusCmd = new SqlCommand(statusStudioQuery, conn);
                    statusCmd.Parameters.AddWithValue("@id_studio", idStudio);

                    object statusObj = statusCmd.ExecuteScalar();
                    if (statusObj == null || statusObj.ToString() != "Aktif")
                        return "Tidak Tersedia";

                    string query = @"
                    SELECT COUNT(*)
                    FROM dbo.Booking
                    WHERE id_studio = @id_studio
                    AND tanggal = @tanggal
                    AND status_booking <> 'Dibatalkan'
                    AND (
                        @mulai < jam_selesai
                        AND @selesai > jam_mulai
                    )";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_studio", idStudio);
                    cmd.Parameters.AddWithValue("@tanggal", tanggal.Date);
                    cmd.Parameters.AddWithValue("@mulai", mulai);
                    cmd.Parameters.AddWithValue("@selesai", selesai);

                    int count = (int)cmd.ExecuteScalar();

                    return count > 0 ? "Dibooking" : "Tersedia";
                }
            }
            catch
            {
                return "Tidak Tersedia";
            }
        }

        void AddStatusCard(Control parent, int x, int y, string status)
        {
            Color bg = Green;
            Color fg = Color.White;

            if (status == "Dibooking") bg = Red;
            if (status == "Tidak Tersedia")
            {
                bg = Gray;
                fg = Color.Black;
            }

            Panel card = RoundedPanel(145, 27, 9, bg);
            card.Location = new Point(x, y);
            parent.Controls.Add(card);

            Label lbl = new Label();
            lbl.Text = status;
            lbl.Font = new Font("Anek Devanagari", 8, FontStyle.Bold);
            lbl.ForeColor = fg;
            lbl.AutoSize = false;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Dock = DockStyle.Fill;
            card.Controls.Add(lbl);
        }

        void ShowBookingPage()
        {
            wrapperPanel.Controls.Clear();
            selectedStudioCard = null;

            AddPic(wrapperPanel, Properties.Resources.Hitam, 1030, 15, 95, 95);

            AddLabel(wrapperPanel, "Booking Form", 55, 45, 42, true, Color.Black, "Chubby And Groovy");
            AddLabel(wrapperPanel, "Isi Form dibawah ini untuk melakukan pemesanan studio.", 60, 120, 13, false, Color.Black);

            Panel formBox = RoundedPanel(650, 650, 15, Color.White);
            formBox.Location = new Point(40, 165);
            wrapperPanel.Controls.Add(formBox);

            AddLabel(formBox, "Form Booking", 28, 20, 13, true, Color.FromArgb(66, 132, 245));

            AddSmallLabel(formBox, "Nama Pemesan", 32, 52);
            TextBox txtNama = new TextBox();
            txtNama.Location = new Point(32, 77);
            txtNama.Size = new Size(260, 32);
            txtNama.Font = new Font("Anek Devanagari", 10);
            formBox.Controls.Add(txtNama);

            AddSmallLabel(formBox, "No HP", 320, 52);
            TextBox txtNoHp = new TextBox();
            txtNoHp.Location = new Point(320, 77);
            txtNoHp.Size = new Size(260, 32);
            txtNoHp.Font = new Font("Anek Devanagari", 10);
            formBox.Controls.Add(txtNoHp);

            AddBookingSectionTitle(formBox, Properties.Resources.Jadwalicon, "Pilih Tanggal", 125);
            DateTimePicker tanggal = new DateTimePicker();
            tanggal.Location = new Point(32, 158);
            tanggal.Size = new Size(260, 32);
            tanggal.Font = new Font("Anek Devanagari", 10);
            tanggal.Format = DateTimePickerFormat.Short;
            formBox.Controls.Add(tanggal);

            AddBookingSectionTitle(formBox, Properties.Resources.card3icon, "Pilih Studio", 205);

            Panel card1 = AddStudioCard(formBox, 32, 238, 1, "Studio 1", "(Regular)", 50000, Green, "🥁");
            Panel card2 = AddStudioCard(formBox, 237, 238, 2, "Studio 2", "(VIP)", 60000, Color.FromArgb(255, 205, 69), "🎤");
            Panel card3 = AddStudioCard(formBox, 442, 238, 3, "Studio Utama", "(VVIP)", 70000, Color.FromArgb(255, 124, 85), "🎸");

            AddBookingSectionTitle(formBox, Properties.Resources.clock, "Pilih Waktu", 365);

            AddSmallLabel(formBox, "Dari", 32, 398);
            ComboBox cmbDari = new ComboBox();
            cmbDari.Location = new Point(32, 425);
            cmbDari.Size = new Size(180, 32);
            cmbDari.Font = new Font("Anek Devanagari", 10);
            cmbDari.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDari.Items.AddRange(new object[]
            {
                "09:00", "10:00", "11:00", "12:00", "13:00", "14:00",
                "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00"
            });
            cmbDari.SelectedIndex = 0;
            formBox.Controls.Add(cmbDari);

            AddSmallLabel(formBox, "Sampai", 230, 398);
            ComboBox cmbSampai = new ComboBox();
            cmbSampai.Location = new Point(230, 425);
            cmbSampai.Size = new Size(180, 32);
            cmbSampai.Font = new Font("Anek Devanagari", 10);
            cmbSampai.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSampai.Items.AddRange(new object[]
            {
                "10:00", "11:00", "12:00", "13:00", "14:00", "15:00",
                "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00"
            });
            cmbSampai.SelectedIndex = 0;
            formBox.Controls.Add(cmbSampai);

            AddBookingSectionTitle(formBox, null, "Pilih Layanan Tambahan (Opsional)", 470);

            CheckBox alat = AddServiceBox(formBox, 32, 510, "Extra Mic", "Rp 5.000");
            CheckBox rekam = AddServiceBox(formBox, 185, 510, "Keyboard", "Rp 10.000");
            CheckBox mixing = AddServiceBox(formBox, 338, 510, "Recording", "Rp 75.000");

            AddBookingSectionTitle(formBox, null, "Catatan (Opsional)", 585);

            TextBox catatan = new TextBox();
            catatan.Location = new Point(32, 615);
            catatan.Size = new Size(570, 22);
            catatan.Font = new Font("Anek Devanagari", 10);
            catatan.BorderStyle = BorderStyle.FixedSingle;
            formBox.Controls.Add(catatan);

            Panel rightBox = RoundedPanel(438, 650, 15, Color.White);
            rightBox.Location = new Point(710, 165);
            wrapperPanel.Controls.Add(rightBox);

            Panel summaryHead = RoundedPanel(398, 98, 15, Color.FromArgb(232, 241, 255));
            summaryHead.Location = new Point(20, 22);
            rightBox.Controls.Add(summaryHead);

            AddLabel(summaryHead, "Ringkasan Booking", 105, 18, 13, true, Color.FromArgb(66, 132, 245));
            AddLabel(summaryHead, "Studio dan total harga akan berubah otomatis.", 65, 55, 9, false, Color.Black);

            Panel summary = RoundedPanel(398, 280, 12, Color.White);
            summary.Location = new Point(20, 150);
            rightBox.Controls.Add(summary);

            Label valStudio = AddSummaryRow(summary, Properties.Resources.card3icon, "Studio", "-", 0);
            Label valTanggal = AddSummaryRow(summary, Properties.Resources.Jadwalicon, "Tanggal", "-", 35);
            Label valWaktu = AddSummaryRow(summary, Properties.Resources.clock, "Waktu", "-", 70);
            Label valLayanan = AddSummaryRow(summary, Properties.Resources.Bookingicon, "Tambahan", "-", 105);

            AddLine(summary, 0, 150, 398);

            AddLabel(summary, "Subtotal Studio", 25, 175, 10, false, Color.Black);
            Label subtotalPrice = AddLabel(summary, "Rp 0", 285, 175, 10, true, Color.Black);

            AddLine(summary, 0, 215, 398);

            AddLabel(summary, "Total", 25, 235, 12, true, Color.Black);
            Label totalPrice = AddLabel(summary, "Rp 0", 285, 235, 12, true, Color.Black);

            AddLine(summary, 0, 275, 398);

            AddLabel(rightBox, "Metode Pembayaran", 28, 445, 11, true, Color.Black);

            ComboBox cmbBayar = new ComboBox();
            cmbBayar.Location = new Point(28, 475);
            cmbBayar.Size = new Size(180, 54);
            cmbBayar.Font = new Font("Anek Devanagari", 10);
            cmbBayar.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBayar.Items.Add("Cash");
            cmbBayar.Items.Add("QRIS");
            cmbBayar.SelectedIndex = 0;
            rightBox.Controls.Add(cmbBayar);

            Button btnBooking = new Button();
            btnBooking.Text = "Bayar";
            btnBooking.Font = new Font("Anek Devanagari", 11, FontStyle.Bold);
            btnBooking.Size = new Size(398, 45);
            btnBooking.Location = new Point(20, 520);
            btnBooking.FlatStyle = FlatStyle.Flat;
            btnBooking.FlatAppearance.BorderSize = 0;
            btnBooking.BackColor = Yellow;
            btnBooking.ForeColor = Color.Black;
            btnBooking.Cursor = Cursors.Hand;
            rightBox.Controls.Add(btnBooking);

            Panel infoBox = RoundedPanel(398, 45, 14, Color.FromArgb(255, 251, 235));
            infoBox.Location = new Point(20, 585);
            rightBox.Controls.Add(infoBox);

            AddLabel(infoBox, "✦ Silahkan Isi Form Booking Dengan Lengkap.", 18, 13, 9, true, Color.Black);

            Action updateSummary = () =>
            {
                TimeSpan mulai = TimeSpan.Parse(cmbDari.Text);
                TimeSpan selesai = TimeSpan.Parse(cmbSampai.Text);

                int durasi = selesai > mulai ? (int)(selesai - mulai).TotalHours : 0;
                int subtotalStudio = durasi * selectedHargaPerJam;
                int totalHarga = subtotalStudio;

                string layanan = "";
                if (alat.Checked)
                {
                    totalHarga += 5000;
                    layanan += "Mic, ";
                }
                if (rekam.Checked)
                {
                    totalHarga += 10000;
                    layanan += "Keyboard, ";
                }
                if (mixing.Checked)
                {
                    totalHarga += 75000;
                    layanan += "Recording, ";
                }

                if (layanan.EndsWith(", "))
                    layanan = layanan.Substring(0, layanan.Length - 2);

                valStudio.Text = selectedStudioName;
                valTanggal.Text = tanggal.Value.ToString("dd/MM/yyyy");
                valWaktu.Text = cmbDari.Text + " - " + cmbSampai.Text;
                valLayanan.Text = layanan == "" ? "-" : layanan;
                subtotalPrice.Text = FormatRupiah(subtotalStudio);
                totalPrice.Text = FormatRupiah(totalHarga);
            };

            SelectStudioCard(card1, 1, "Studio 1", 50000);
            updateSummary();

            tanggal.ValueChanged += (s, e) => updateSummary();
            cmbDari.SelectedIndexChanged += (s, e) => updateSummary();
            cmbSampai.SelectedIndexChanged += (s, e) => updateSummary();
            alat.CheckedChanged += (s, e) => updateSummary();
            rekam.CheckedChanged += (s, e) => updateSummary();
            mixing.CheckedChanged += (s, e) => updateSummary();

            card1.Click += (s, e) => { AnimateClick(card1); SelectStudioCard(card1, 1, "Studio 1", 50000); updateSummary(); };
            card2.Click += (s, e) => { AnimateClick(card2); SelectStudioCard(card2, 2, "Studio 2", 60000); updateSummary(); };
            card3.Click += (s, e) => { AnimateClick(card3); SelectStudioCard(card3, 3, "Studio Utama", 70000); updateSummary(); };

            btnBooking.Click += (s, e) =>
            {
                if (txtNama.Text.Trim() == "" || txtNoHp.Text.Trim() == "")
                {
                    MessageBox.Show("Nama pemesan dan No HP wajib diisi.");
                    return;
                }

                TimeSpan jamMulai = TimeSpan.Parse(cmbDari.Text);
                TimeSpan jamSelesai = TimeSpan.Parse(cmbSampai.Text);

                if (jamSelesai <= jamMulai)
                {
                    MessageBox.Show("Jam selesai harus lebih besar dari jam mulai.");
                    return;
                }

                int durasi = (int)(jamSelesai - jamMulai).TotalHours;
                int totalHarga = HitungTotal(jamMulai, jamSelesai, alat.Checked, rekam.Checked, mixing.Checked);

                string metode = cmbBayar.Text;

                bool yakin = ShowConfirmNotif(
    "Konfirmasi Booking",
    "Studio: " + selectedStudioName +
    "\nTanggal: " + tanggal.Value.ToString("dd/MM/yyyy") +
    "\nJam: " + cmbDari.Text + " - " + cmbSampai.Text +
    "\nTotal: " + FormatRupiah(totalHarga) +
    "\nMetode: " + metode
);

                if (!yakin)
                    return;

                bool sukses = SimpanBooking(
                    txtNama.Text.Trim(),
                    txtNoHp.Text.Trim(),
                    selectedStudioId,
                    tanggal.Value.Date,
                    jamMulai,
                    jamSelesai,
                    durasi,
                    totalHarga,
                    metode
                );

                if (sukses)
                {
                    if (metode == "QRIS")
                        ShowQRISBill(txtNama.Text.Trim(), selectedStudioName, tanggal.Value.Date, cmbDari.Text + " - " + cmbSampai.Text, totalHarga);
                    else
                        ShowCashBill(txtNama.Text.Trim(), selectedStudioName, tanggal.Value.Date, cmbDari.Text + " - " + cmbSampai.Text, totalHarga);
                }
            };
        }

        int HitungTotal(TimeSpan mulai, TimeSpan selesai, bool alat, bool rekam, bool mixing)
        {
            int durasi = selesai > mulai ? (int)(selesai - mulai).TotalHours : 0;
            int total = durasi * selectedHargaPerJam;

            if (alat) total += 5000;
            if (rekam) total += 10000;
            if (mixing) total += 75000;

            return total;
        }

        bool SimpanBooking(string nama, string noHp, int idStudio, DateTime tanggal,
                   TimeSpan jamMulai, TimeSpan jamSelesai, int durasi, int totalHarga, string metode)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string cekQuery = @"
                    SELECT COUNT(*) 
                    FROM dbo.Booking
                    WHERE id_studio = @id_studio
                    AND tanggal = @tanggal
                    AND status_booking <> 'Dibatalkan'
                    AND (
                        @jam_mulai < jam_selesai 
                        AND @jam_selesai > jam_mulai
                    )";

                    SqlCommand cekCmd = new SqlCommand(cekQuery, conn);
                    cekCmd.Parameters.AddWithValue("@id_studio", idStudio);
                    cekCmd.Parameters.AddWithValue("@tanggal", tanggal.Date);
                    cekCmd.Parameters.AddWithValue("@jam_mulai", jamMulai);
                    cekCmd.Parameters.AddWithValue("@jam_selesai", jamSelesai);

                    int bentrok = (int)cekCmd.ExecuteScalar();

                    if (bentrok > 0)
                    {
                        MessageBox.Show("Jadwal sudah dibooking. Pilih jam lain.");
                        return false;
                    }

                    string insertQuery = @"
                    INSERT INTO dbo.Booking
                    (nama_pemesan, no_hp, id_studio, tanggal, jam_mulai, jam_selesai, durasi, total_harga, metode_pembayaran)
                    VALUES
                    (@nama, @no_hp, @id_studio, @tanggal, @jam_mulai, @jam_selesai, @durasi, @total_harga, @metode_pembayaran)";

                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@nama", nama);
                    cmd.Parameters.AddWithValue("@no_hp", noHp);
                    cmd.Parameters.AddWithValue("@id_studio", idStudio);
                    cmd.Parameters.AddWithValue("@tanggal", tanggal.Date);
                    cmd.Parameters.AddWithValue("@jam_mulai", jamMulai);
                    cmd.Parameters.AddWithValue("@jam_selesai", jamSelesai);
                    cmd.Parameters.AddWithValue("@durasi", durasi);
                    cmd.Parameters.AddWithValue("@total_harga", totalHarga);
                    cmd.Parameters.AddWithValue("@metode_pembayaran", metode);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan booking:\n" + ex.Message);
                return false;
            }
        }

        void ShowCashBill(string nama, string studio, DateTime tanggal, string waktu, int total)
        {
            MessageBox.Show(
                "BOOKING BERHASIL\n\n" +
                "Nama: " + nama +
                "\nStudio: " + studio +
                "\nTanggal: " + tanggal.ToString("dd/MM/yyyy") +
                "\nWaktu: " + waktu +
                "\nTotal: " + FormatRupiah(total) +
                "\n\nSilakan bayar ke kasir studio.",
                "Bill Cash",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        void ShowQRISBill(string nama, string studio, DateTime tanggal, string waktu, int total)
        {
            Form qrForm = new Form();
            qrForm.Text = "Pembayaran QRIS";
            qrForm.Size = new Size(420, 560);
            qrForm.StartPosition = FormStartPosition.CenterScreen;
            qrForm.BackColor = Color.White;

            Label title = new Label();
            title.Text = "Pembayaran QRIS";
            title.Font = new Font("Anek Devanagari", 18, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(110, 25);
            qrForm.Controls.Add(title);

            Panel qrBox = RoundedPanel(250, 250, 15, Color.FromArgb(245, 245, 245));
            qrBox.Location = new Point(82, 85);
            qrForm.Controls.Add(qrBox);

            Label qr = new Label();
            qr.Text = "QRIS";
            qr.Font = new Font("Anek Devanagari", 42, FontStyle.Bold);
            qr.ForeColor = Color.Black;
            qr.AutoSize = false;
            qr.TextAlign = ContentAlignment.MiddleCenter;
            qr.Dock = DockStyle.Fill;
            qrBox.Controls.Add(qr);

            Label detail = new Label();
            detail.Text =
                "Nama: " + nama +
                "\nStudio: " + studio +
                "\nTanggal: " + tanggal.ToString("dd/MM/yyyy") +
                "\nWaktu: " + waktu +
                "\nTotal: " + FormatRupiah(total) +
                "\n\nScan QR ini untuk melakukan pembayaran.";
            detail.Font = new Font("Anek Devanagari", 10);
            detail.AutoSize = false;
            detail.Size = new Size(350, 130);
            detail.Location = new Point(35, 355);
            qrForm.Controls.Add(detail);

            Button ok = new Button();
            ok.Text = "Selesai";
            ok.Font = new Font("Anek Devanagari", 11, FontStyle.Bold);
            ok.Size = new Size(330, 38);
            ok.Location = new Point(42, 475);
            ok.BackColor = Yellow;
            ok.FlatStyle = FlatStyle.Flat;
            ok.FlatAppearance.BorderSize = 0;
            ok.Click += (s, e) => qrForm.Close();
            qrForm.Controls.Add(ok);

            qrForm.ShowDialog();
        }

        void SelectStudioCard(Panel card, int id, string name, int harga)
        {
            if (selectedStudioCard != null)
                selectedStudioCard.BackColor = Color.White;

            selectedStudioCard = card;
            selectedStudioCard.BackColor = Color.FromArgb(232, 241, 255);

            selectedStudioId = id;
            selectedStudioName = name;
            selectedHargaPerJam = harga;
        }

        Panel AddStudioCard(Control parent, int x, int y, int idStudio, string name, string type, int harga, Color dotColor, string emoji)
        {
            Panel card = RoundedPanel(188, 112, 8, Color.White);
            card.Location = new Point(x, y);
            card.Cursor = Cursors.Hand;
            parent.Controls.Add(card);

            Color normalColor = Color.White;
            Color hoverColor = Color.FromArgb(245, 249, 255);

            card.MouseEnter += (s, e) =>
            {
                if (card != selectedStudioCard)
                    card.BackColor = hoverColor;
            };

            card.MouseLeave += (s, e) =>
            {
                if (card != selectedStudioCard)
                    card.BackColor = normalColor;
            };

            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(225, 225, 225), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            Panel dot = new Panel();
            dot.Size = new Size(16, 16);
            dot.Location = new Point(18, 18);
            dot.BackColor = dotColor;
            card.Controls.Add(dot);

            Label title = AddLabel(card, name, 42, 14, 10, true, Color.Black);
            Label sub = AddLabel(card, type + "\n" + FormatRupiah(harga) + "/jam", 42, 36, 8, false, Color.Black);

            Label icon = new Label();
            icon.Text = emoji;
            icon.Font = new Font("Segoe UI Emoji", 28);
            icon.ForeColor = dotColor;
            icon.AutoSize = true;
            icon.Location = new Point(125, 58);
            icon.Cursor = Cursors.Hand;
            card.Controls.Add(icon);

            title.Cursor = Cursors.Hand;
            sub.Cursor = Cursors.Hand;

            title.Click += (s, e) => TriggerPanelClick(card);
            sub.Click += (s, e) => TriggerPanelClick(card);
            icon.Click += (s, e) => TriggerPanelClick(card);

            void TriggerPanelClick(Panel panel)
            {
                panel.GetType()
                    .GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    .Invoke(panel, new object[] { EventArgs.Empty });
            }

            return card;
        }

        void AddBookingSectionTitle(Control parent, Image icon, string text, int y)
        {
            if (icon != null)
                AddPic(parent, RecolorIcon(icon, Blue), 28, y, 24, 24);
            else
                AddLabel(parent, "+", 30, y - 5, 18, true, Blue);

            AddLabel(parent, text, 62, y + 3, 10, false, Color.Black);
        }

        CheckBox AddServiceBox(Control parent, int x, int y, string title, string price)
        {
            Panel box = RoundedPanel(143, 58, 8, Color.White);
            box.Location = new Point(x, y);
            parent.Controls.Add(box);

            box.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(225, 225, 225), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, box.Width - 1, box.Height - 1);
            };

            CheckBox cb = new CheckBox();
            cb.Text = title + "\n" + price;
            cb.Font = new Font("Anek Devanagari", 8, FontStyle.Bold);
            cb.AutoSize = true;
            cb.Location = new Point(12, 12);
            cb.Cursor = Cursors.Hand;
            box.Controls.Add(cb);

            return cb;
        }

        Label AddSummaryRow(Control parent, Image icon, string title, string value, int y)
        {
            AddPic(parent, RecolorIcon(icon, Blue), 22, y, 20, 20);
            AddLabel(parent, title, 50, y, 9, false, Color.Black);

            Label val = AddLabel(parent, value, 205, y, 9, true, Color.Black);
            return val;
        }

        void AddSmallLabel(Control parent, string text, int x, int y)
        {
            AddLabel(parent, text, x, y, 9, false, Color.Black);
        }

        void AddLegend(Control parent, Color color, string text, int x)
        {
            Panel dot = new Panel();
            dot.Size = new Size(13, 13);
            dot.Location = new Point(x, 61);
            dot.BackColor = color;
            parent.Controls.Add(dot);

            AddLabel(parent, text, x + 20, 55, 9, false, Color.Black);
        }

        Label AddLabel(Control parent, string text, int x, int y, int size, bool bold, Color color, string fontName = "Anek Devanagari")
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font(fontName, size, bold ? FontStyle.Bold : FontStyle.Regular);
            lbl.ForeColor = color;
            lbl.AutoSize = true;
            lbl.Location = new Point(x, y);
            parent.Controls.Add(lbl);
            return lbl;
        }

        void AddLine(Control parent, int x, int y, int w)
        {
            Panel line = new Panel();
            line.BackColor = Color.FromArgb(230, 230, 230);
            line.Size = new Size(w, 1);
            line.Location = new Point(x, y);
            parent.Controls.Add(line);
        }

        void AddText(Control parent, string text, int x, int y, int size, bool bold)
        {
            AddLabel(parent, text, x, y, size, bold, Color.Black);
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

        string FormatRupiah(int value)
        {
            return "Rp " + value.ToString("N0").Replace(",", ".");
        }

        Image RecolorIcon(Image original, Color color)
        {
            Bitmap bmp = new Bitmap(original.Width, original.Height);

            using (Graphics g = Graphics.FromImage(bmp))
                g.DrawImage(original, 0, 0, original.Width, original.Height);

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color px = bmp.GetPixel(x, y);
                    if (px.A > 20)
                        bmp.SetPixel(x, y, Color.FromArgb(px.A, color.R, color.G, color.B));
                }
            }

            return bmp;
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

        void Logout()
        {
            this.Close();
            Form1 login = new Form1();
            login.Show();
        }

        void AnimateClick(Control control)
        {
            int originalW = control.Width;
            int originalH = control.Height;
            int originalX = control.Left;
            int originalY = control.Top;

            control.Width -= 4;
            control.Height -= 4;
            control.Left += 2;
            control.Top += 2;

            Timer timer = new Timer();
            timer.Interval = 80;
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                timer.Dispose();

                control.Width = originalW;
                control.Height = originalH;
                control.Left = originalX;
                control.Top = originalY;
            };

            timer.Start();
        }

        void ShowCustomNotif(string title, string message, Color accentColor)
        {
            Form notif = new Form();
            notif.FormBorderStyle = FormBorderStyle.None;
            notif.StartPosition = FormStartPosition.CenterScreen;
            notif.Size = new Size(420, 220);
            notif.BackColor = Color.White;
            notif.TopMost = true;

            Panel accent = new Panel();
            accent.BackColor = accentColor;
            accent.Dock = DockStyle.Top;
            accent.Height = 8;
            notif.Controls.Add(accent);

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Anek Devanagari", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.Black;
            lblTitle.AutoSize = false;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Location = new Point(20, 35);
            lblTitle.Size = new Size(380, 40);
            notif.Controls.Add(lblTitle);

            Label lblMsg = new Label();
            lblMsg.Text = message;
            lblMsg.Font = new Font("Anek Devanagari", 10);
            lblMsg.ForeColor = Color.Black;
            lblMsg.AutoSize = false;
            lblMsg.TextAlign = ContentAlignment.MiddleCenter;
            lblMsg.Location = new Point(35, 85);
            lblMsg.Size = new Size(350, 60);
            notif.Controls.Add(lblMsg);

            Button ok = new Button();
            ok.Text = "Oke";
            ok.Font = new Font("Anek Devanagari", 10, FontStyle.Bold);
            ok.Size = new Size(150, 38);
            ok.Location = new Point(135, 160);
            ok.BackColor = Yellow;
            ok.FlatStyle = FlatStyle.Flat;
            ok.FlatAppearance.BorderSize = 0;
            ok.Cursor = Cursors.Hand;
            ok.Click += (s, e) => notif.Close();
            notif.Controls.Add(ok);

            notif.ShowDialog();
        }

        bool ShowConfirmNotif(string title, string message)
        {
            bool result = false;

            Form notif = new Form();
            notif.FormBorderStyle = FormBorderStyle.None;
            notif.StartPosition = FormStartPosition.CenterScreen;
            notif.Size = new Size(460, 250);
            notif.BackColor = Color.White;
            notif.TopMost = true;

            Panel accent = new Panel();
            accent.BackColor = Yellow;
            accent.Dock = DockStyle.Top;
            accent.Height = 8;
            notif.Controls.Add(accent);

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Anek Devanagari", 18, FontStyle.Bold);
            lblTitle.AutoSize = false;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Location = new Point(20, 35);
            lblTitle.Size = new Size(420, 40);
            notif.Controls.Add(lblTitle);

            Label lblMsg = new Label();
            lblMsg.Text = message;
            lblMsg.Font = new Font("Anek Devanagari", 10);
            lblMsg.AutoSize = false;
            lblMsg.TextAlign = ContentAlignment.MiddleCenter;
            lblMsg.Location = new Point(40, 85);
            lblMsg.Size = new Size(380, 80);
            notif.Controls.Add(lblMsg);

            Button batal = new Button();
            batal.Text = "Batal";
            batal.Font = new Font("Anek Devanagari", 10, FontStyle.Bold);
            batal.Size = new Size(130, 38);
            batal.Location = new Point(85, 180);
            batal.BackColor = Color.FromArgb(230, 230, 230);
            batal.FlatStyle = FlatStyle.Flat;
            batal.FlatAppearance.BorderSize = 0;
            batal.Click += (s, e) =>
            {
                result = false;
                notif.Close();
            };
            notif.Controls.Add(batal);

            Button yakin = new Button();
            yakin.Text = "Yakin";
            yakin.Font = new Font("Anek Devanagari", 10, FontStyle.Bold);
            yakin.Size = new Size(130, 38);
            yakin.Location = new Point(245, 180);
            yakin.BackColor = Yellow;
            yakin.FlatStyle = FlatStyle.Flat;
            yakin.FlatAppearance.BorderSize = 0;
            yakin.Click += (s, e) =>
            {
                result = true;
                notif.Close();
            };
            notif.Controls.Add(yakin);

            notif.ShowDialog();
            return result;
        }
    }
}