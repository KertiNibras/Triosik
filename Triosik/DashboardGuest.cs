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

        readonly Color Blue = Color.FromArgb(31, 126, 224);
        readonly Color Yellow = Color.FromArgb(255, 230, 55);

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

            Image finalIcon = RecolorIcon(icon, active ? Blue : Color.White);
            AddPic(menu, finalIcon, 17, 14, 26, 26);

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

        Image RecolorIcon(Image original, Color color)
        {
            Bitmap bmp = new Bitmap(original.Width, original.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(original, 0, 0, original.Width, original.Height);
            }

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color px = bmp.GetPixel(x, y);

                    if (px.A > 20)
                    {
                        bmp.SetPixel(x, y, Color.FromArgb(px.A, color.R, color.G, color.B));
                    }
                }
            }

            return bmp;
        }

        void ShowDashboardPage()
        {
            wrapperPanel.Controls.Clear();

            Label title = new Label();
            title.Text = "Welcome, Guest!";
            title.Font = new Font("Chubby And Groovy", 42, FontStyle.Bold);
            title.ForeColor = Color.Black;
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

            Label bigLbl = new Label();
            bigLbl.Text = big;
            bigLbl.Font = new Font("Anek Devanagari", 21, FontStyle.Bold);
            bigLbl.AutoSize = true;
            bigLbl.Location = new Point(28, 92);
            card.Controls.Add(bigLbl);

            Label titleLbl = new Label();
            titleLbl.Text = title;
            titleLbl.Font = new Font("Anek Devanagari", 13, FontStyle.Bold);
            titleLbl.AutoSize = true;
            titleLbl.Location = new Point(28, 132);
            card.Controls.Add(titleLbl);

            Label descLbl = new Label();
            descLbl.Text = desc;
            descLbl.Font = new Font("Anek Devanagari", 10);
            descLbl.AutoSize = true;
            descLbl.Location = new Point(28, 162);
            card.Controls.Add(descLbl);
        }

        void AddSchedulePanel()
        {
            Panel box = RoundedPanel(650, 390, 15, Color.White);
            box.Location = new Point(15, 420);
            wrapperPanel.Controls.Add(box);

            Label title = new Label();
            title.Text = "Jadwal Hari Ini";
            title.Font = new Font("Chubby And Groovy", 18);
            title.AutoSize = true;
            title.Location = new Point(30, 22);
            box.Controls.Add(title);

            Label lihat = new Label();
            lihat.Text = "Lihat Semua";
            lihat.Font = new Font("Anek Devanagari", 13);
            lihat.ForeColor = Blue;
            lihat.AutoSize = true;
            lihat.Location = new Point(530, 25);
            lihat.Cursor = Cursors.Hand;
            lihat.Click += (s, e) =>
            {
                SetActiveMenu("Lihat Jadwal");
                ShowSchedulePage();
            };
            box.Controls.Add(lihat);

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

            Label lbl = new Label();
            lbl.Text = time;
            lbl.Font = new Font("Anek Devanagari", 14, FontStyle.Bold);
            lbl.AutoSize = true;
            lbl.Location = new Point(70, 9);
            row.Controls.Add(lbl);

            AddPic(row, Properties.Resources.arrowblack, 535, 14, 22, 18);
        }

        void AddWhyPanel()
        {
            Panel box = RoundedPanel(460, 240, 15, Color.White);
            box.Location = new Point(685, 420);
            wrapperPanel.Controls.Add(box);

            Label title = new Label();
            title.Text = "Kenapa Pilih Triosik?";
            title.Font = new Font("Chubby And Groovy", 16);
            title.AutoSize = true;
            title.Location = new Point(25, 18);
            box.Controls.Add(title);

            AddWhyItem(box, Properties.Resources.Secureicon, "Peralatan Berkualitas", "Dilengkapi alat musik dan sound system terbaik.", 62);
            AddWhyItem(box, Properties.Resources.musicpurple, "Studio Nyaman", "Ruang kedap suara dan nyaman untuk berkreasi.", 105);
            AddWhyItem(box, Properties.Resources.calendergreen, "Booking Mudah", "Proses cepat dan praktis tanpa ribet.", 145);
            AddWhyItem(box, Properties.Resources.smileyellow, "Cocok untuk Semua", "Untuk latihan pribadi, band, rekaman, dan kebutuhan lain.", 185);
        }

        void AddWhyItem(Panel p, Image icon, string title, string desc, int y)
        {
            AddPic(p, icon, 25, y, 30, 30);

            Label lbl = new Label();
            lbl.Text = title + "\n" + desc;
            lbl.Font = new Font("Anek Devanagari", 9, FontStyle.Bold);
            lbl.AutoSize = true;
            lbl.Location = new Point(70, y - 2);
            p.Controls.Add(lbl);
        }

        void AddBookingPanel()
        {
            Panel box = RoundedPanel(460, 135, 15, Color.White);
            box.Location = new Point(685, 675);
            wrapperPanel.Controls.Add(box);

            Label title = new Label();
            title.Text = "Mulai Bermusik";
            title.Font = new Font("Chubby And Groovy", 16);
            title.AutoSize = true;
            title.Location = new Point(25, 12);
            box.Controls.Add(title);

            Panel btn = RoundedPanel(410, 52, 10, Color.FromArgb(225, 239, 255));
            btn.Location = new Point(25, 55);
            btn.Cursor = Cursors.Hand;
            btn.Click += (s, e) =>
            {
                SetActiveMenu("Booking Sekarang");
                ShowBookingPage();
            };
            box.Controls.Add(btn);

            AddPic(btn, Properties.Resources.mulaibookingicon, 20, 13, 27, 27);

            Label text = new Label();
            text.Text = "Booking Studio Sekarang";
            text.Font = new Font("Anek Devanagari", 10, FontStyle.Bold);
            text.ForeColor = Blue;
            text.AutoSize = true;
            text.Location = new Point(60, 17);
            btn.Controls.Add(text);

            AddPic(btn, Properties.Resources.arrowblue, 380, 16, 22, 20);
        }

        void ShowSchedulePage()
        {
            wrapperPanel.Controls.Clear();

            Label title = new Label();
            title.Text = "Schedule";
            title.Font = new Font("Chubby And Groovy", 42, FontStyle.Bold);
            title.ForeColor = Color.Black;
            title.AutoSize = true;
            title.Location = new Point(55, 45);
            wrapperPanel.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "Lihat semua jadwal studio yang tersedia hari ini.";
            subtitle.Font = new Font("Anek Devanagari", 15);
            subtitle.AutoSize = true;
            subtitle.Location = new Point(60, 120);
            wrapperPanel.Controls.Add(subtitle);

            AddPic(wrapperPanel, Properties.Resources.Hitam, 1090, 45, 95, 95);

            Panel filterBox = RoundedPanel(1120, 105, 15, Color.White);
            filterBox.Location = new Point(45, 180);
            wrapperPanel.Controls.Add(filterBox);

            Label tgl = new Label();
            tgl.Text = "Pilih Tanggal";
            tgl.Font = new Font("Anek Devanagari", 11);
            tgl.Location = new Point(35, 18);
            tgl.AutoSize = true;
            filterBox.Controls.Add(tgl);

            TextBox inputTanggal = new TextBox();
            inputTanggal.Location = new Point(35, 50);
            inputTanggal.Size = new Size(250, 34);
            inputTanggal.Font = new Font("Anek Devanagari", 12);
            filterBox.Controls.Add(inputTanggal);

            Label studio = new Label();
            studio.Text = "Pilih Studio (Opsional)";
            studio.Font = new Font("Anek Devanagari", 11);
            studio.Location = new Point(330, 18);
            studio.AutoSize = true;
            filterBox.Controls.Add(studio);

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

            AddLegend(filterBox, Color.FromArgb(120, 200, 145), "Tersedia", 650);
            AddLegend(filterBox, Color.FromArgb(235, 120, 140), "Terbooking", 760);
            AddLegend(filterBox, Color.LightGray, "Tidak Tersedia", 880);

            Button cari = new Button();
            cari.Size = new Size(80, 38);
            cari.Location = new Point(1010, 45);
            cari.FlatStyle = FlatStyle.Flat;
            cari.BackColor = Color.White;
            filterBox.Controls.Add(cari);

            Panel table = RoundedPanel(1120, 500, 15, Color.White);
            table.Location = new Point(45, 305);
            wrapperPanel.Controls.Add(table);

            AddScheduleHeader(table);
            AddScheduleTimeRows(table);
        }

        void ShowBookingPage()
        {
            wrapperPanel.Controls.Clear();

            AddPic(wrapperPanel, Properties.Resources.Hitam, 1030, 15, 95, 95);

            Label title = new Label();
            title.Text = "Booking Form";
            title.Font = new Font("Chubby And Groovy", 42, FontStyle.Bold);
            title.ForeColor = Color.Black;
            title.AutoSize = true;
            title.Location = new Point(55, 45);
            wrapperPanel.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "Isi Form dibawah ini untuk melakukan pemesanan studio.";
            subtitle.Font = new Font("Anek Devanagari", 13);
            subtitle.AutoSize = true;
            subtitle.Location = new Point(60, 120);
            wrapperPanel.Controls.Add(subtitle);

            Panel formBox = RoundedPanel(650, 650, 15, Color.White);
            formBox.Location = new Point(40, 165);
            wrapperPanel.Controls.Add(formBox);

            Label formTitle = new Label();
            formTitle.Text = "Form Booking";
            formTitle.Font = new Font("Anek Devanagari", 13, FontStyle.Bold);
            formTitle.ForeColor = Color.FromArgb(66, 132, 245);
            formTitle.AutoSize = true;
            formTitle.Location = new Point(28, 25);
            formBox.Controls.Add(formTitle);

            AddBookingSectionTitle(formBox, Properties.Resources.Jadwalicon, "Pilih Tanggal", 72);
            DateTimePicker tanggal = new DateTimePicker();
            tanggal.Location = new Point(32, 105);
            tanggal.Size = new Size(260, 34);
            tanggal.Font = new Font("Anek Devanagari", 11);
            tanggal.Format = DateTimePickerFormat.Short;
            formBox.Controls.Add(tanggal);

            AddBookingSectionTitle(formBox, Properties.Resources.card3icon, "Pilih Studio", 155);

            AddStudioCard(formBox, 32, 188, "Studio 1", "(Regular)", Color.FromArgb(105, 198, 145), "🥁");
            AddStudioCard(formBox, 237, 188, "Studio 2", "(VIP)", Color.FromArgb(255, 205, 69), "🎤");
            AddStudioCard(formBox, 442, 188, "Studio Utama", "(VVIP)", Color.FromArgb(255, 124, 85), "🎸");

            AddBookingSectionTitle(formBox, Properties.Resources.clock, "Pilih Waktu", 315);

            Label dari = new Label();
            dari.Text = "Dari";
            dari.Font = new Font("Anek Devanagari", 9);
            dari.AutoSize = true;
            dari.Location = new Point(32, 348);
            formBox.Controls.Add(dari);

            ComboBox cmbDari = new ComboBox();
            cmbDari.Location = new Point(32, 375);
            cmbDari.Size = new Size(180, 34);
            cmbDari.Font = new Font("Anek Devanagari", 11);
            cmbDari.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDari.Items.AddRange(new object[]
            {
        "09:00", "10:00", "11:00", "12:00", "13:00", "14:00",
        "15:00", "16:00", "17:00", "18:00", "19:00", "20:00"
            });
            cmbDari.SelectedIndex = 0;
            formBox.Controls.Add(cmbDari);

            Label sampai = new Label();
            sampai.Text = "Sampai";
            sampai.Font = new Font("Anek Devanagari", 9);
            sampai.AutoSize = true;
            sampai.Location = new Point(230, 348);
            formBox.Controls.Add(sampai);

            ComboBox cmbSampai = new ComboBox();
            cmbSampai.Location = new Point(230, 375);
            cmbSampai.Size = new Size(180, 34);
            cmbSampai.Font = new Font("Anek Devanagari", 11);
            cmbSampai.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSampai.Items.AddRange(new object[]
            {
        "10:00", "11:00", "12:00", "13:00", "14:00", "15:00",
        "16:00", "17:00", "18:00", "19:00", "20:00", "21:00"
            });
            cmbSampai.SelectedIndex = 0;
            formBox.Controls.Add(cmbSampai);

            AddBookingSectionTitle(formBox, null, "Pilih Layanan Tambahan (Opsional)", 430);

            CheckBox alat = AddServiceBox(formBox, 32, 470, "Sewa Alat", "Rp 25.000");
            CheckBox rekam = AddServiceBox(formBox, 185, 470, "Recording", "Rp 50.000");
            CheckBox mixing = AddServiceBox(formBox, 338, 470, "Mixing", "Rp 75.000");

            AddBookingSectionTitle(formBox, null, "Catatan (Opsional)", 550);

            TextBox catatan = new TextBox();
            catatan.Location = new Point(32, 585);
            catatan.Size = new Size(570, 55);
            catatan.Font = new Font("Anek Devanagari", 11);
            catatan.Multiline = true;
            catatan.BorderStyle = BorderStyle.FixedSingle;
            formBox.Controls.Add(catatan);

            Button btnBooking = new Button();
            btnBooking.Text = "Booking Sekarang";
            btnBooking.Font = new Font("Anek Devanagari", 12, FontStyle.Bold);
            btnBooking.Size = new Size(570, 45);
            btnBooking.Location = new Point(32, 715);
            btnBooking.FlatStyle = FlatStyle.Flat;
            btnBooking.FlatAppearance.BorderSize = 0;
            btnBooking.BackColor = Yellow;
            btnBooking.ForeColor = Color.Black;
            btnBooking.Cursor = Cursors.Hand;
            formBox.Controls.Add(btnBooking);

            Panel rightBox = RoundedPanel(438, 650, 15, Color.White);
            rightBox.Location = new Point(710, 165);
            wrapperPanel.Controls.Add(rightBox);

            Panel summaryHead = RoundedPanel(398, 128, 15, Color.FromArgb(232, 241, 255));
            summaryHead.Location = new Point(20, 22);
            rightBox.Controls.Add(summaryHead);

            Label sumTitle = new Label();
            sumTitle.Text = "Ringkasan Booking";
            sumTitle.Font = new Font("Anek Devanagari", 13, FontStyle.Bold);
            sumTitle.ForeColor = Color.FromArgb(66, 132, 245);
            sumTitle.AutoSize = true;
            sumTitle.Location = new Point(18, 18);
            summaryHead.Controls.Add(sumTitle);

            Panel summary = RoundedPanel(398, 345, 12, Color.White);
            summary.Location = new Point(20, 150);
            rightBox.Controls.Add(summary);

            AddSummaryRow(summary, Properties.Resources.Jadwalicon, "Tanggal", "-", 25);
            AddSummaryRow(summary, Properties.Resources.clock, "Waktu", "-", 65);
            AddSummaryRow(summary, Properties.Resources.Bookingicon, "Layanan Tambahan", "-", 105);

            AddLine(summary, 0, 160, 398);

            Label subtotal = new Label();
            subtotal.Text = "Subtotal Studio";
            subtotal.Font = new Font("Anek Devanagari", 10);
            subtotal.AutoSize = true;
            subtotal.Location = new Point(25, 190);
            summary.Controls.Add(subtotal);

            Label subtotalPrice = new Label();
            subtotalPrice.Text = "Rp 0";
            subtotalPrice.Font = new Font("Anek Devanagari", 10, FontStyle.Bold);
            subtotalPrice.AutoSize = true;
            subtotalPrice.Location = new Point(295, 190);
            summary.Controls.Add(subtotalPrice);

            AddLine(summary, 0, 278, 398);

            Label total = new Label();
            total.Text = "Total";
            total.Font = new Font("Anek Devanagari", 12, FontStyle.Bold);
            total.AutoSize = true;
            total.Location = new Point(25, 300);
            summary.Controls.Add(total);

            Label totalPrice = new Label();
            totalPrice.Text = "Rp 0";
            totalPrice.Font = new Font("Anek Devanagari", 12, FontStyle.Bold);
            totalPrice.AutoSize = true;
            totalPrice.Location = new Point(295, 300);
            summary.Controls.Add(totalPrice);

            Panel infoBox = RoundedPanel(398, 123, 14, Color.FromArgb(255, 251, 235));
            infoBox.Location = new Point(20, 515);
            rightBox.Controls.Add(infoBox);

            Label infoTitle = new Label();
            infoTitle.Text = "✦  Informasi";
            infoTitle.Font = new Font("Anek Devanagari", 10, FontStyle.Bold);
            infoTitle.AutoSize = true;
            infoTitle.Location = new Point(20, 15);
            infoBox.Controls.Add(infoTitle);

            Label infoText = new Label();
            infoText.Text = "• Pembayaran harus dilakukan dalam 15 menit\n• Booking akan otomatis dibatalkan jika pembayaran\n  tidak berhasil";
            infoText.Font = new Font("Anek Devanagari", 9);
            infoText.AutoSize = true;
            infoText.Location = new Point(25, 45);
            infoBox.Controls.Add(infoText);

            Panel secureBox = RoundedPanel(398, 70, 14, Color.FromArgb(238, 250, 240));
            secureBox.Location = new Point(20, 655);
            rightBox.Controls.Add(secureBox);

            Label safeTitle = new Label();
            safeTitle.Text = "🛡  Keamanan Terjamin";
            safeTitle.Font = new Font("Anek Devanagari", 10, FontStyle.Bold);
            safeTitle.AutoSize = true;
            safeTitle.Location = new Point(18, 14);
            secureBox.Controls.Add(safeTitle);

            Label safeText = new Label();
            safeText.Text = "Data dan pembayaran kamu aman bersama kami";
            safeText.Font = new Font("Anek Devanagari", 9);
            safeText.AutoSize = true;
            safeText.Location = new Point(18, 40);
            secureBox.Controls.Add(safeText);
        }

        void AddBookingSectionTitle(Control parent, Image icon, string text, int y)
        {
            if (icon != null)
            {
                AddPic(parent, RecolorIcon(icon, Blue), 28, y, 24, 24);
            }
            else
            {
                Label plus = new Label();
                plus.Text = "+";
                plus.Font = new Font("Anek Devanagari", 18, FontStyle.Bold);
                plus.ForeColor = Blue;
                plus.AutoSize = true;
                plus.Location = new Point(30, y - 5);
                parent.Controls.Add(plus);
            }

            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Anek Devanagari", 10);
            lbl.AutoSize = true;
            lbl.Location = new Point(62, y + 3);
            parent.Controls.Add(lbl);
        }

        Panel AddStudioCard(Control parent, int x, int y, string name, string type, Color dotColor, string emoji)
        {
            Panel card = RoundedPanel(188, 112, 8, Color.White);
            card.Location = new Point(x, y);
            card.Cursor = Cursors.Hand;
            parent.Controls.Add(card);

            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(225, 225, 225), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            Panel dot = new Panel();
            dot.Size = new Size(16, 16);
            dot.Location = new Point(18, 18);
            dot.BackColor = dotColor;
            dot.Region = new Region(new GraphicsPath());
            card.Controls.Add(dot);

            dot.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Brush b = new SolidBrush(dotColor))
                    e.Graphics.FillEllipse(b, 0, 0, 15, 15);
            };

            Label title = new Label();
            title.Text = name;
            title.Font = new Font("Anek Devanagari", 10, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(42, 14);
            card.Controls.Add(title);

            Label sub = new Label();
            sub.Text = type;
            sub.Font = new Font("Anek Devanagari", 8);
            sub.AutoSize = true;
            sub.Location = new Point(42, 36);
            card.Controls.Add(sub);

            Label icon = new Label();
            icon.Text = emoji;
            icon.Font = new Font("Segoe UI Emoji", 30);
            icon.ForeColor = dotColor;
            icon.AutoSize = true;
            icon.Location = new Point(125, 54);
            card.Controls.Add(icon);

            return card;
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

        void AddSummaryRow(Control parent, Image icon, string title, string value, int y)
        {
            AddPic(parent, RecolorIcon(icon, Blue), 22, y, 20, 20);

            Label lbl = new Label();
            lbl.Text = title;
            lbl.Font = new Font("Anek Devanagari", 9);
            lbl.AutoSize = true;
            lbl.Location = new Point(50, y);
            parent.Controls.Add(lbl);

            Label val = new Label();
            val.Text = value;
            val.Font = new Font("Anek Devanagari", 9, FontStyle.Bold);
            val.AutoSize = true;
            val.Location = new Point(250, y);
            parent.Controls.Add(val);
        }

        void AddLine(Control parent, int x, int y, int w)
        {
            Panel line = new Panel();
            line.BackColor = Color.FromArgb(230, 230, 230);
            line.Size = new Size(w, 1);
            line.Location = new Point(x, y);
            parent.Controls.Add(line);
        }

        void Logout()
        {
            this.Close();
            Form1 login = new Form1();
            login.Show();
        }

        void AddLegend(Control parent, Color color, string text, int x)
        {
            Panel dot = new Panel();
            dot.Size = new Size(13, 13);
            dot.Location = new Point(x, 61);
            dot.BackColor = color;
            parent.Controls.Add(dot);

            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Anek Devanagari", 9);
            lbl.AutoSize = true;
            lbl.Location = new Point(x + 20, 55);
            parent.Controls.Add(lbl);
        }

        void AddScheduleHeader(Panel table)
        {
            AddText(table, "Waktu", 55, 30, 11, true);
            AddStudioHeader(table, Color.FromArgb(120, 200, 145), "Studio 1", "(Regular)", 270);
            AddStudioHeader(table, Color.FromArgb(255, 225, 120), "Studio 2", "(Vip)", 580);
            AddStudioHeader(table, Color.FromArgb(255, 120, 90), "Studio Utama", "(Vvip)", 900);
        }

        void AddStudioHeader(Panel table, Color color, string name, string type, int x)
        {
            Panel dot = new Panel();
            dot.Size = new Size(13, 13);
            dot.Location = new Point(x, 30);
            dot.BackColor = color;
            table.Controls.Add(dot);

            AddText(table, name, x + 20, 23, 10, true);
            AddText(table, type, x + 20, 42, 8, false);
        }

        void AddScheduleTimeRows(Panel table)
        {
            string[] times =
            {
                "09:00 - 10:00", "10:00 - 11:00", "11:00 - 12:00",
                "12:00 - 13:00", "13:00 - 14:00", "14:00 - 15:00",
                "15:00 - 16:00", "16:00 - 17:00", "17:00 - 18:00",
                "18:00 - 19:00", "19:00 - 20:00", "20:00 - 21:00"
            };

            int y = 80;
            foreach (string time in times)
            {
                AddText(table, time, 55, y, 11, false);
                y += 34;
            }
        }

        void AddText(Control parent, string text, int x, int y, int size, bool bold)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Anek Devanagari", size, bold ? FontStyle.Bold : FontStyle.Regular);
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