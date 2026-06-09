using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Triosik
{
    public partial class DashboardGuest : Form
    {
        Panel wrapperPanel;

        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Triosik;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        int selectedStudioId = 1;
        string selectedStudioName = "Studio 1";
        string selectedStudioJenis = "Regular";
        int selectedStudioHarga = 75000;

        List<int> selectedServiceIds = new List<int>();
        Dictionary<string, int> serviceIdByName = new Dictionary<string, int>();
        Dictionary<string, int> servicePriceByName = new Dictionary<string, int>();

        readonly Color BlueTop = Color.FromArgb(25, 121, 221);
        readonly Color BlueBottom = Color.FromArgb(235, 244, 255);
        readonly Color CardBlue = Color.FromArgb(32, 123, 235);
        readonly Color Yellow = Color.FromArgb(255, 226, 62);
        readonly Color DarkBlue = Color.FromArgb(0, 58, 140);
        readonly Color SelectedGray = Color.FromArgb(170, 170, 170);
        readonly Color DisabledGray = Color.FromArgb(200, 200, 200);

        List<string> selectedTimes = new List<string>();
        List<string> selectedServices = new List<string>();
        string selectedNote = "";

        TextBox noteBoxInput;
        Panel buatPesananButton;
        Label buatPesananText;

        public DashboardGuest()
        {
            InitializeComponent();
            LoadMasterData();
            BuildUI();
        }

        private void DashboardGuest_Load(object sender, EventArgs e) { }

        void BuildUI()
        {
            Text = "Dashboard Guest";
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            BackColor = BlueBottom;
            DoubleBuffered = true;

            Controls.Clear();

            wrapperPanel = new Panel();
            wrapperPanel.Dock = DockStyle.Fill;
            wrapperPanel.BackColor = Color.Transparent;
            Controls.Add(wrapperPanel);

            ShowDashboardAwal();
        }

        void LoadMasterData()
        {
            serviceIdByName.Clear();
            servicePriceByName.Clear();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand studioCmd = new SqlCommand(
                    "SELECT TOP 1 * FROM Studio WHERE nama_studio = 'Studio 1' AND status = 'Aktif'",
                    conn
                );

                SqlDataReader studioReader = studioCmd.ExecuteReader();

                if (studioReader.Read())
                {
                    selectedStudioId = Convert.ToInt32(studioReader["id_studio"]);
                    selectedStudioName = studioReader["nama_studio"].ToString();
                    selectedStudioJenis = studioReader["jenis_studio"].ToString();
                    selectedStudioHarga = Convert.ToInt32(studioReader["harga_per_jam"]);
                }

                studioReader.Close();

                SqlCommand layananCmd = new SqlCommand(
                    "SELECT * FROM Layanan WHERE status = 'Aktif'",
                    conn
                );

                SqlDataReader layananReader = layananCmd.ExecuteReader();

                while (layananReader.Read())
                {
                    string nama = layananReader["nama_layanan"].ToString();
                    int id = Convert.ToInt32(layananReader["id_layanan"]);
                    int harga = Convert.ToInt32(layananReader["harga"]);

                    serviceIdByName[nama] = id;
                    servicePriceByName[nama] = harga;
                }

                layananReader.Close();
            }
        }

        void ShowDashboardAwal()
        {
            wrapperPanel.Controls.Clear();

            Panel bg = CreateGradientBackground();
            wrapperPanel.Controls.Add(bg);

            Label back = CreateBackButton();
            back.Click += (s, e) => Logout();
            bg.Controls.Add(back);

            AddSparkle(bg, 555, 105, 60);
            AddSparkle(bg, 1270, 310, 60);

            AddPic(bg, Properties.Resources.Hitam, 605, 190, 190, 190);
            AddPic(bg, Properties.Resources.Kuning, 825, 70, 270, 270);
            AddPic(bg, Properties.Resources.Pink, 1100, 190, 190, 190);

            Label title = new Label();
            title.Text = "Welcome To Our\nMusic Studio";
            title.Font = SafeFont("Chubby And Groovy", 76, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.BackColor = Color.Transparent;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Size = new Size(800, 180);
            title.Location = new Point(570, 400);
            bg.Controls.Add(title);

            Panel btn = RoundedPanel(430, 95, 24, Yellow);
            btn.Location = new Point(745, 650);
            btn.Cursor = Cursors.Hand;
            btn.Click += (s, e) => ShowPilihStudio();
            bg.Controls.Add(btn);

            Label btnText = new Label();
            btnText.Text = "Booking Sekarang";
            btnText.Font = SafeFont("Chubby And Groovy", 34, FontStyle.Bold);
            btnText.ForeColor = DarkBlue;
            btnText.BackColor = Color.Transparent;
            btnText.TextAlign = ContentAlignment.MiddleCenter;
            btnText.Dock = DockStyle.Fill;
            btnText.Cursor = Cursors.Hand;
            btnText.Click += (s, e) => ShowPilihStudio();
            btn.Controls.Add(btnText);
        }

        void ShowPilihStudio()
        {
            wrapperPanel.Controls.Clear();

            Panel bg = CreateGradientBackground();
            wrapperPanel.Controls.Add(bg);

            Label back = CreateBackButton();
            back.Click += (s, e) => ShowDashboardAwal();
            bg.Controls.Add(back);

            Label title = new Label();
            title.Text = "Pilih Studio";
            title.Font = SafeFont("Chubby And Groovy", 70, FontStyle.Bold);
            title.ForeColor = DarkBlue;
            title.BackColor = Color.Transparent;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Size = new Size(700, 110);
            title.Location = new Point(610, 45);
            bg.Controls.Add(title);

            AddStudioCard(bg, 185, 190, 410, 700, "Studio 1");
            AddStudioCard(bg, 755, 190, 410, 700, "Studio 2");
            AddStudioCard(bg, 1325, 190, 410, 700, "Studio\nUtama");
        }

        void AddStudioCard(Control parent, int x, int y, int w, int h, string studioName)
        {
            Panel shadow = RoundedPanel(w, h, 35, Color.White);
            shadow.Location = new Point(x - 12, y + 12);
            parent.Controls.Add(shadow);

            Panel card = RoundedPanel(w, h, 35, CardBlue);
            card.Location = new Point(x, y);
            card.Cursor = Cursors.Hand;
            card.Click += (s, e) => PilihStudio(studioName);
            parent.Controls.Add(card);
            card.BringToFront();

            Panel imgBox = RoundedPanel(360, 455, 28, Color.FromArgb(30, 30, 30));
            imgBox.Location = new Point(25, 28);
            imgBox.Cursor = Cursors.Hand;
            imgBox.Click += (s, e) => PilihStudio(studioName);
            card.Controls.Add(imgBox);

            Label imgText = new Label();
            imgText.Text = "Asset\nGambar\nNanti";
            imgText.Font = new Font("Arial", 22, FontStyle.Bold);
            imgText.ForeColor = Color.White;
            imgText.BackColor = Color.Transparent;
            imgText.TextAlign = ContentAlignment.MiddleCenter;
            imgText.Dock = DockStyle.Fill;
            imgText.Cursor = Cursors.Hand;
            imgText.Click += (s, e) => PilihStudio(studioName);
            imgBox.Controls.Add(imgText);

            Label name = new Label();
            name.Text = studioName;
            name.Font = SafeFont("Chubby And Groovy", 58, FontStyle.Bold);
            name.ForeColor = Color.White;
            name.BackColor = Color.Transparent;
            name.TextAlign = ContentAlignment.MiddleCenter;
            name.Size = new Size(370, 170);
            name.Location = new Point(20, 510);
            name.Cursor = Cursors.Hand;
            name.Click += (s, e) => PilihStudio(studioName);
            card.Controls.Add(name);
        }

        void PilihStudio(string studioName)
        {
            string cleanName = studioName.Replace("\n", " ");

            if (cleanName == "Studio 1")
                ShowStudio1();
            else
                MessageBox.Show(cleanName + " belum dibuat halamannya.");
        }

        void ShowStudio1()
        {
            wrapperPanel.Controls.Clear();

            Panel bg = CreateGradientBackground();
            wrapperPanel.Controls.Add(bg);

            Label back = CreateBackButton();
            back.ForeColor = Color.White;
            back.Click += (s, e) => ShowPilihStudio();
            bg.Controls.Add(back);

            AddHeaderStudio1(bg);

            Panel formBox = RoundedPanel(1450, 590, 55, Color.FromArgb(214, 231, 250));
            formBox.Location = new Point(220, 310);
            bg.Controls.Add(formBox);

            AddInputRow(formBox, 60, 50, "Today\n" + GetTodayDayOnly(), "Tanggal", false, null);
            AddInputRow(formBox, 60, 190, "◷", "Pilih Waktu", true, ShowStudio1PilihWaktu);
            AddInputRow(formBox, 60, 330, "+", "Pilih Layanan Tambahan (Opsional)", true, ShowStudio1PilihLayanan);
            AddInputRow(formBox, 60, 470, "✎", "Catatan (Opsional)", true, ShowStudio1Catatan);

            AddBuatPesananButton(bg);
        }

        void ShowStudio1PilihWaktu(object sender, EventArgs e)
        {
            wrapperPanel.Controls.Clear();

            Panel bg = CreateGradientBackground();
            wrapperPanel.Controls.Add(bg);

            Label back = CreateBackButton();
            back.ForeColor = Color.White;
            back.Click += (s, ev) => ShowStudio1();
            bg.Controls.Add(back);

            AddHeaderStudio1(bg);

            Panel formBox = RoundedPanel(1450, 590, 55, Color.FromArgb(214, 231, 250));
            formBox.Location = new Point(220, 310);
            bg.Controls.Add(formBox);

            AddInputRow(formBox, 60, 50, "Today\n" + GetTodayDayOnly(), "Tanggal", false, null);

            Panel waktuBox = RoundedPanel(1310, 370, 30, Color.WhiteSmoke);
            waktuBox.Location = new Point(60, 190);
            formBox.Controls.Add(waktuBox);

            AddDropdownHeader(waktuBox, "◷", "Pilih Waktu");

            AddTimeButton(waktuBox, "09:00", 95, 125);
            AddTimeButton(waktuBox, "10:00", 370, 125);
            AddTimeButton(waktuBox, "11:00", 645, 125);
            AddTimeButton(waktuBox, "12:00", 920, 125);
            AddTimeButton(waktuBox, "13:00", 95, 190);
            AddTimeButton(waktuBox, "14:00", 370, 190);
            AddTimeButton(waktuBox, "15:00", 645, 190);
            AddTimeButton(waktuBox, "16:00", 920, 190);
            AddTimeButton(waktuBox, "17:00", 95, 255);
            AddTimeButton(waktuBox, "18:00", 370, 255);
            AddTimeButton(waktuBox, "19:00", 645, 255);
            AddTimeButton(waktuBox, "20:00", 920, 255);
            AddTimeButton(waktuBox, "21:00", 95, 320);

            AddSimpanButton(bg);
        }

        void ShowStudio1PilihLayanan(object sender, EventArgs e)
        {
            wrapperPanel.Controls.Clear();

            Panel bg = CreateGradientBackground();
            wrapperPanel.Controls.Add(bg);

            Label back = CreateBackButton();
            back.ForeColor = Color.White;
            back.Click += (s, ev) => ShowStudio1();
            bg.Controls.Add(back);

            AddHeaderStudio1(bg);

            Panel formBox = RoundedPanel(1450, 590, 55, Color.FromArgb(214, 231, 250));
            formBox.Location = new Point(220, 310);
            bg.Controls.Add(formBox);

            AddInputRow(formBox, 60, 50, "Today\n04", "Tanggal", false, null);
            AddInputRow(formBox, 60, 190, "◷", "Pilih Waktu", true, ShowStudio1PilihWaktu);

            Panel layananBox = RoundedPanel(1310, 240, 30, Color.WhiteSmoke);
            layananBox.Location = new Point(60, 330);
            formBox.Controls.Add(layananBox);

            AddDropdownHeader(layananBox, "+", "Pilih Layanan Tambahan (Opsional)");

            AddServiceButton(layananBox, "Extra Mic", "Rp. 5.000/jam", 95, 125);
            AddServiceButton(layananBox, "Keyboard", "Rp. 20.000/jam", 475, 125);
            AddServiceButton(layananBox, "Recording", "Rp. 150.000/jam", 855, 125);

            AddSimpanButton(bg);
        }

        void ShowStudio1Catatan(object sender, EventArgs e)
        {
            wrapperPanel.Controls.Clear();

            Panel bg = CreateGradientBackground();
            wrapperPanel.Controls.Add(bg);

            Label back = CreateBackButton();
            back.ForeColor = Color.White;
            back.Click += (s, ev) => ShowStudio1();
            bg.Controls.Add(back);

            AddHeaderStudio1(bg);

            Panel formBox = RoundedPanel(1450, 590, 55, Color.FromArgb(214, 231, 250));
            formBox.Location = new Point(220, 310);
            bg.Controls.Add(formBox);

            Panel catatanBox = RoundedPanel(1310, 500, 30, Color.WhiteSmoke);
            catatanBox.Location = new Point(60, 50);
            formBox.Controls.Add(catatanBox);

            AddDropdownHeader(catatanBox, "✎", "Catatan (Opsional)");

            Panel inputBorder = RoundedPanel(1200, 250, 25, Color.FromArgb(125, 170, 240));
            inputBorder.Location = new Point(55, 115);
            catatanBox.Controls.Add(inputBorder);

            noteBoxInput = new TextBox();
            noteBoxInput.Text = selectedNote == "" ? "Tinggalkan pesan" : selectedNote;
            noteBoxInput.Font = new Font("Arial", 22, FontStyle.Bold);
            noteBoxInput.ForeColor = DarkBlue;
            noteBoxInput.BackColor = Color.WhiteSmoke;
            noteBoxInput.BorderStyle = BorderStyle.None;
            noteBoxInput.Multiline = true;
            noteBoxInput.Size = new Size(1120, 200);
            noteBoxInput.Location = new Point(40, 30);
            noteBoxInput.GotFocus += (s, ev) =>
            {
                if (noteBoxInput.Text == "Tinggalkan pesan")
                    noteBoxInput.Text = "";
            };
            noteBoxInput.LostFocus += (s, ev) =>
            {
                if (noteBoxInput.Text.Trim() == "")
                    noteBoxInput.Text = "Tinggalkan pesan";
            };
            inputBorder.Controls.Add(noteBoxInput);

            Panel confirmBtn = RoundedPanel(1200, 90, 25, CardBlue);
            confirmBtn.Location = new Point(55, 395);
            confirmBtn.Cursor = Cursors.Hand;
            confirmBtn.Click += (s, ev) => SimpanCatatan();
            catatanBox.Controls.Add(confirmBtn);

            Label confirmText = new Label();
            confirmText.Text = "Konfirmasi";
            confirmText.Font = new Font("Arial", 24, FontStyle.Bold);
            confirmText.ForeColor = Color.White;
            confirmText.BackColor = Color.Transparent;
            confirmText.TextAlign = ContentAlignment.MiddleCenter;
            confirmText.Dock = DockStyle.Fill;
            confirmText.Cursor = Cursors.Hand;
            confirmText.Click += (s, ev) => SimpanCatatan();
            confirmBtn.Controls.Add(confirmText);

            AddSimpanButton(bg);
        }

        void AddHeaderStudio1(Control bg)
        {
            Panel iconBox = RoundedPanel(220, 190, 45, DarkBlue);
            iconBox.Location = new Point(220, 90);
            bg.Controls.Add(iconBox);

            Label drumIcon = new Label();
            drumIcon.Text = "🥁";
            drumIcon.Font = new Font("Segoe UI Emoji", 86, FontStyle.Regular);
            drumIcon.ForeColor = Color.White;
            drumIcon.BackColor = Color.Transparent;
            drumIcon.TextAlign = ContentAlignment.MiddleCenter;
            drumIcon.Dock = DockStyle.Fill;
            iconBox.Controls.Add(drumIcon);

            Label title = new Label();
            title.Text = "Studio 1";
            title.Font = SafeFont("Chubby And Groovy", 72, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.BackColor = Color.Transparent;
            title.Size = new Size(550, 90);
            title.Location = new Point(470, 105);
            bg.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "(Regular)";
            subtitle.Font = new Font("Georgia", 28, FontStyle.Bold);
            subtitle.ForeColor = Color.White;
            subtitle.BackColor = Color.Transparent;
            subtitle.Size = new Size(350, 50);
            subtitle.Location = new Point(480, 190);
            bg.Controls.Add(subtitle);

            AddPic(bg, Properties.Resources.Pink, 1420, 90, 190, 190);
        }

        void AddDropdownHeader(Control parent, string iconText, string text)
        {
            Label icon = new Label();
            icon.Text = iconText;
            icon.Font = new Font("Arial", 36, FontStyle.Bold);
            icon.ForeColor = CardBlue;
            icon.BackColor = Color.Transparent;
            icon.TextAlign = ContentAlignment.MiddleCenter;
            icon.Size = new Size(110, 80);
            icon.Location = new Point(35, 20);
            parent.Controls.Add(icon);

            Label label = new Label();
            label.Text = text;
            label.Font = new Font("Arial", 28, FontStyle.Bold);
            label.ForeColor = DarkBlue;
            label.BackColor = Color.Transparent;
            label.Size = new Size(850, 70);
            label.Location = new Point(180, 28);
            parent.Controls.Add(label);

            Label arrow = new Label();
            arrow.Text = "⌄";
            arrow.Font = new Font("Arial", 44, FontStyle.Bold);
            arrow.ForeColor = DarkBlue;
            arrow.BackColor = Color.Transparent;
            arrow.TextAlign = ContentAlignment.MiddleCenter;
            arrow.Size = new Size(80, 80);
            arrow.Location = new Point(1190, 20);
            parent.Controls.Add(arrow);
        }

        void AddInputRow(Control parent, int x, int y, string iconText, string text, bool dropdown, EventHandler clickEvent)
        {
            Panel rowShadow = RoundedPanel(1310, 110, 30, Color.FromArgb(135, 185, 245));
            rowShadow.Location = new Point(x, y + 8);
            parent.Controls.Add(rowShadow);

            Panel row = RoundedPanel(1310, 110, 30, Color.WhiteSmoke);
            row.Location = new Point(x, y);
            row.Cursor = Cursors.Hand;
            parent.Controls.Add(row);
            row.BringToFront();

            if (clickEvent != null)
                row.Click += clickEvent;

            Label icon = new Label();
            icon.Text = iconText;
            icon.Font = new Font("Arial", 24, FontStyle.Bold);
            icon.ForeColor = CardBlue;
            icon.BackColor = Color.Transparent;
            icon.TextAlign = ContentAlignment.MiddleCenter;
            icon.Size = new Size(110, 80);
            icon.Location = new Point(35, 15);
            icon.Cursor = Cursors.Hand;
            row.Controls.Add(icon);

            if (clickEvent != null)
                icon.Click += clickEvent;

            Label label = new Label();
            label.Text = text;
            label.Font = new Font("Arial", 28, FontStyle.Bold);
            label.ForeColor = DarkBlue;
            label.BackColor = Color.Transparent;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Size = new Size(900, 80);
            label.Location = new Point(180, 16);
            label.Cursor = Cursors.Hand;
            row.Controls.Add(label);

            if (clickEvent != null)
                label.Click += clickEvent;

            if (dropdown)
            {
                Label arrow = new Label();
                arrow.Text = "⌄";
                arrow.Font = new Font("Arial", 44, FontStyle.Bold);
                arrow.ForeColor = DarkBlue;
                arrow.BackColor = Color.Transparent;
                arrow.TextAlign = ContentAlignment.MiddleCenter;
                arrow.Size = new Size(80, 80);
                arrow.Location = new Point(1190, 10);
                arrow.Cursor = Cursors.Hand;
                row.Controls.Add(arrow);

                if (clickEvent != null)
                    arrow.Click += clickEvent;
            }
        }

        void AddSimpanButton(Control parent)
        {
            Panel simpanBtn = RoundedPanel(430, 95, 30, Yellow);
            simpanBtn.Location = new Point(700, 920);
            simpanBtn.Cursor = Cursors.Hand;
            simpanBtn.Click += (s, e) => ShowStudio1();
            parent.Controls.Add(simpanBtn);

            Label simpanText = new Label();
            simpanText.Text = "Simpan";
            simpanText.Font = new Font("Arial", 28, FontStyle.Bold);
            simpanText.ForeColor = DarkBlue;
            simpanText.BackColor = Color.Transparent;
            simpanText.TextAlign = ContentAlignment.MiddleCenter;
            simpanText.Dock = DockStyle.Fill;
            simpanText.Cursor = Cursors.Hand;
            simpanText.Click += (s, e) => ShowStudio1();
            simpanBtn.Controls.Add(simpanText);
        }

        void AddTimeButton(Control parent, string time, int x, int y)
        {
            Panel timeBtn = RoundedPanel(245, 48, 20,
                selectedTimes.Contains(time) ? SelectedGray : CardBlue);

            timeBtn.Location = new Point(x, y);
            timeBtn.Cursor = Cursors.Hand;
            parent.Controls.Add(timeBtn);

            Label timeText = new Label();
            timeText.Text = time;
            timeText.Font = new Font("Arial", 24, FontStyle.Bold);
            timeText.ForeColor = Color.White;
            timeText.BackColor = Color.Transparent;
            timeText.TextAlign = ContentAlignment.MiddleCenter;
            timeText.Dock = DockStyle.Fill;
            timeText.Cursor = Cursors.Hand;
            timeBtn.Controls.Add(timeText);

            EventHandler clickAction = (s, e) =>
            {
                if (selectedTimes.Contains(time))
                {
                    selectedTimes.Remove(time);
                    timeBtn.BackColor = CardBlue;
                }
                else
                {
                    if (selectedTimes.Count >= 3)
                    {
                        MessageBox.Show("Maksimal pilih 3 jam aja.");
                        return;
                    }

                    selectedTimes.Add(time);
                    timeBtn.BackColor = SelectedGray;
                }

                selectedTimes.Sort();
                UpdateBuatPesananButton();
            };

            timeBtn.Click += clickAction;
            timeText.Click += clickAction;
        }

       

        void AddServiceButton(Control parent, string serviceName, string price, int x, int y)
        {
            Panel serviceBtn = RoundedPanel(345, 105, 25,
                selectedServices.Contains(serviceName) ? SelectedGray : CardBlue);

            serviceBtn.Location = new Point(x, y);
            serviceBtn.Cursor = Cursors.Hand;
            parent.Controls.Add(serviceBtn);

            Label name = new Label();
            name.Text = serviceName;
            name.Font = new Font("Arial", 28, FontStyle.Bold);
            name.ForeColor = Color.White;
            name.BackColor = Color.Transparent;
            name.TextAlign = ContentAlignment.MiddleCenter;
            name.Size = new Size(345, 55);
            name.Location = new Point(0, 12);
            name.Cursor = Cursors.Hand;
            serviceBtn.Controls.Add(name);

            Label priceLbl = new Label();
            priceLbl.Text = price;
            priceLbl.Font = new Font("Arial", 16, FontStyle.Regular);
            priceLbl.ForeColor = Color.White;
            priceLbl.BackColor = Color.Transparent;
            priceLbl.TextAlign = ContentAlignment.MiddleCenter;
            priceLbl.Size = new Size(345, 35);
            priceLbl.Location = new Point(0, 60);
            priceLbl.Cursor = Cursors.Hand;
            serviceBtn.Controls.Add(priceLbl);

            EventHandler clickAction = (s, e) =>
            {
                if (selectedServices.Contains(serviceName))
                {
                    selectedServices.Remove(serviceName);
                    serviceBtn.BackColor = CardBlue;
                }
                else
                {
                    selectedServices.Add(serviceName);
                    serviceBtn.BackColor = SelectedGray;
                }
            };

            serviceBtn.Click += clickAction;
            name.Click += clickAction;
            priceLbl.Click += clickAction;
        }

        void AddBuatPesananButton(Control parent)
        {
            buatPesananButton = RoundedPanel(430, 95, 30, DisabledGray);
            buatPesananButton.Location = new Point(700, 920);
            buatPesananButton.Cursor = Cursors.Hand;
            buatPesananButton.Click += (s, e) => BuatPesananStudio1();
            parent.Controls.Add(buatPesananButton);

            buatPesananText = new Label();
            buatPesananText.Text = "Buat Pesanan";
            buatPesananText.Font = new Font("Arial", 28, FontStyle.Bold);
            buatPesananText.ForeColor = Color.White;
            buatPesananText.BackColor = Color.Transparent;
            buatPesananText.TextAlign = ContentAlignment.MiddleCenter;
            buatPesananText.Dock = DockStyle.Fill;
            buatPesananText.Cursor = Cursors.Hand;
            buatPesananText.Click += (s, e) => BuatPesananStudio1();
            buatPesananButton.Controls.Add(buatPesananText);

            UpdateBuatPesananButton();
        }

        void UpdateBuatPesananButton()
        {
            if (buatPesananButton == null || buatPesananText == null) return;

            if (selectedTimes.Count > 0)
            {
                buatPesananButton.BackColor = Yellow;
                buatPesananText.ForeColor = DarkBlue;
            }
            else
            {
                buatPesananButton.BackColor = DisabledGray;
                buatPesananText.ForeColor = Color.White;
            }
        }

        void SimpanCatatan()
        {
            if (noteBoxInput.Text.Trim() == "Tinggalkan pesan")
                selectedNote = "";
            else
                selectedNote = noteBoxInput.Text.Trim();

            MessageBox.Show("Catatan disimpan.");
            ShowStudio1();
        }

        void BuatPesananStudio1()
        {
            if (selectedTimes.Count == 0)
            {
                MessageBox.Show("Pilih waktu dulu, nge.");
                return;
            }

            ShowRincianBookingStudio1();
        }

        void ShowRincianBookingStudio1()
        {
            wrapperPanel.Controls.Clear();

            int hargaStudioPerJam = selectedStudioHarga;
            selectedTimes.Sort();
            int durasi = selectedTimes.Count;
            int subtotalStudio = hargaStudioPerJam * durasi;

            int totalLayanan = 0;

            foreach (string layanan in selectedServices)
            {
                if (servicePriceByName.ContainsKey(layanan))
                    totalLayanan += servicePriceByName[layanan] * durasi;
            }

            int total = subtotalStudio + totalLayanan;

            string waktuText = string.Join(", ", selectedTimes);
            string layananText = selectedServices.Count == 0 ? "-" : string.Join(", ", selectedServices);

            Panel bg = CreateGradientBackground();
            wrapperPanel.Controls.Add(bg);

            Label back = CreateBackButton();
            back.ForeColor = DarkBlue;
            back.Click += (s, e) => ShowStudio1();
            bg.Controls.Add(back);

            Label title = new Label();
            title.Text = "Rincian Booking";
            title.Font = SafeFont("Chubby And Groovy", 72, FontStyle.Bold);
            title.ForeColor = DarkBlue;
            title.BackColor = Color.Transparent;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Size = new Size(900, 120);
            title.Location = new Point(510, 55);
            bg.Controls.Add(title);

            Panel outerBox = RoundedPanel(1660, 710, 60, Color.FromArgb(220, 235, 252));
            outerBox.Location = new Point(130, 195);
            bg.Controls.Add(outerBox);

            Panel innerBox = RoundedPanel(1570, 625, 55, Color.WhiteSmoke);
            innerBox.Location = new Point(50, 40);
            outerBox.Controls.Add(innerBox);

            AddRincianRow(innerBox, "▣", "Tanggal", GetTodayDateText(), 120);
            AddRincianRow(innerBox, "◷", "Waktu", waktuText, 220);
            AddRincianRow(innerBox, "♫", "Studio", "Studio 1 (Regular)", 320);
            AddRincianRow(innerBox, "+", "Layanan Tambahan", layananText, 420);

            AddRincianLine(innerBox, 90, 510, 1390);

            AddRincianText(innerBox, "Subtotal Studio", FormatRupiah(subtotalStudio), 580, true);

            AddRincianLine(innerBox, 90, 650, 1390);

            AddRincianText(innerBox, "Total", FormatRupiah(total), 720, true);

            Panel bayarBtn = RoundedPanel(430, 95, 30, Yellow);
            bayarBtn.Location = new Point(745, 955);
            bayarBtn.Cursor = Cursors.Hand;
            bayarBtn.Click += (s, e) =>
            {
                KonfirmasiBayar();
            };
            bg.Controls.Add(bayarBtn);

            Label bayarText = new Label();
            bayarText.Text = "Bayar";
            bayarText.Font = new Font("Arial", 30, FontStyle.Bold);
            bayarText.ForeColor = DarkBlue;
            bayarText.BackColor = Color.Transparent;
            bayarText.TextAlign = ContentAlignment.MiddleCenter;
            bayarText.Dock = DockStyle.Fill;
            bayarText.Cursor = Cursors.Hand;
            bayarText.Click += (s, e) =>
            {
                KonfirmasiBayar();
            };
            bayarBtn.Controls.Add(bayarText);
        }

        void AddRincianRow(Control parent, string iconText, string labelText, string valueText, int y)
        {
            Label icon = new Label();
            icon.Text = iconText;
            icon.Font = new Font("Arial", 34, FontStyle.Bold);
            icon.ForeColor = CardBlue;
            icon.BackColor = Color.Transparent;
            icon.TextAlign = ContentAlignment.MiddleCenter;
            icon.Size = new Size(90, 70);
            icon.Location = new Point(120, y);
            parent.Controls.Add(icon);

            Label label = new Label();
            label.Text = labelText;
            label.Font = new Font("Arial", 24, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.BackColor = Color.Transparent;
            label.Size = new Size(420, 60);
            label.Location = new Point(260, y + 8);
            parent.Controls.Add(label);

            Label titik = new Label();
            titik.Text = ":";
            titik.Font = new Font("Arial", 24, FontStyle.Bold);
            titik.ForeColor = Color.Black;
            titik.BackColor = Color.Transparent;
            titik.Size = new Size(50, 60);
            titik.Location = new Point(1080, y + 8);
            parent.Controls.Add(titik);

            Label value = new Label();
            value.Text = valueText;
            value.Font = new Font("Arial", 22, FontStyle.Bold);
            value.ForeColor = Color.Black;
            value.BackColor = Color.Transparent;
            value.Size = new Size(420, 60);
            value.Location = new Point(1130, y + 8);
            parent.Controls.Add(value);
        }

        void AddRincianText(Control parent, string labelText, string valueText, int y, bool bold)
        {
            Label label = new Label();
            label.Text = labelText;
            label.Font = new Font("Arial", 24, bold ? FontStyle.Bold : FontStyle.Regular);
            label.ForeColor = Color.Black;
            label.BackColor = Color.Transparent;
            label.Size = new Size(500, 60);
            label.Location = new Point(120, y);
            parent.Controls.Add(label);

            Label titik = new Label();
            titik.Text = ":";
            titik.Font = new Font("Arial", 24, FontStyle.Bold);
            titik.ForeColor = Color.Black;
            titik.BackColor = Color.Transparent;
            titik.Size = new Size(50, 60);
            titik.Location = new Point(1080, y);
            parent.Controls.Add(titik);

            Label value = new Label();
            value.Text = valueText;
            value.Font = new Font("Arial", 24, FontStyle.Bold);
            value.ForeColor = Color.Black;
            value.BackColor = Color.Transparent;
            value.Size = new Size(420, 60);
            value.Location = new Point(1130, y);
            parent.Controls.Add(value);
        }

        void AddRincianLine(Control parent, int x, int y, int w)
        {
            Panel line = new Panel();
            line.BackColor = Color.FromArgb(220, 205, 175);
            line.Location = new Point(x, y);
            line.Size = new Size(w, 3);
            parent.Controls.Add(line);
        }

        string FormatRupiah(int value)
        {
            return "Rp " + value.ToString("N0").Replace(",", ".");
        }

        string GetTodayDateText()
        {
            return DateTime.Now.ToString("dd/MM/yyyy");
        }

        string GetTodayDayOnly()
        {
            return DateTime.Now.ToString("dd");
        }

        void KonfirmasiBayar()
        {
            DialogResult result = MessageBox.Show(
                "Yakin mau buat booking sekarang?",
                "Konfirmasi Booking",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                SimpanBookingKeDatabase();

                MessageBox.Show(
                    "Booking berhasil dibuat.\nSilahkan menuju ke kasir.",
                    "Berhasil",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                selectedTimes.Clear();
                selectedServices.Clear();
                selectedServiceIds.Clear();
                selectedNote = "";

                ShowDashboardAwal();
            }
        }

        void SimpanBookingKeDatabase()
        {
            selectedTimes.Sort();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                foreach (string jam in selectedTimes)
                {
                    TimeSpan jamMulai = TimeSpan.Parse(jam);
                    TimeSpan jamSelesai = jamMulai.Add(TimeSpan.FromHours(1));

                    int durasi = 1;
                    int subtotalStudio = selectedStudioHarga;
                    int totalLayanan = HitungTotalLayananPerJam();
                    int totalHarga = subtotalStudio + totalLayanan;

                    string insertBooking = @"
                INSERT INTO Booking
                (nama_pemesan, no_hp, id_studio, tanggal, jam_mulai, jam_selesai, durasi,
                 subtotal_studio, total_layanan, total_harga, status_booking, status_pembayaran,
                 metode_pembayaran, catatan)
                OUTPUT INSERTED.id_booking
                VALUES
                (@nama, @nohp, @idstudio, @tanggal, @jammulai, @jamselesai, @durasi,
                 @subtotal, @totallayanan, @totalharga, 'Dibooking', 'Belum Lunas',
                 'Cash', @catatan)";

                    SqlCommand cmd = new SqlCommand(insertBooking, conn);
                    cmd.Parameters.AddWithValue("@nama", "Guest");
                    cmd.Parameters.AddWithValue("@nohp", "-");
                    cmd.Parameters.AddWithValue("@idstudio", selectedStudioId);
                    cmd.Parameters.AddWithValue("@tanggal", DateTime.Today);
                    cmd.Parameters.AddWithValue("@jammulai", jamMulai);
                    cmd.Parameters.AddWithValue("@jamselesai", jamSelesai);
                    cmd.Parameters.AddWithValue("@durasi", durasi);
                    cmd.Parameters.AddWithValue("@subtotal", subtotalStudio);
                    cmd.Parameters.AddWithValue("@totallayanan", totalLayanan);
                    cmd.Parameters.AddWithValue("@totalharga", totalHarga);
                    cmd.Parameters.AddWithValue("@catatan", selectedNote == "" ? (object)DBNull.Value : selectedNote);

                    int idBooking = Convert.ToInt32(cmd.ExecuteScalar());

                    foreach (string layanan in selectedServices)
                    {
                        if (!serviceIdByName.ContainsKey(layanan)) continue;

                        string insertLayanan = @"
                    INSERT INTO Booking_Layanan
                    (id_booking, id_layanan, harga_layanan)
                    VALUES
                    (@idbooking, @idlayanan, @harga)";

                        SqlCommand cmdLayanan = new SqlCommand(insertLayanan, conn);
                        cmdLayanan.Parameters.AddWithValue("@idbooking", idBooking);
                        cmdLayanan.Parameters.AddWithValue("@idlayanan", serviceIdByName[layanan]);
                        cmdLayanan.Parameters.AddWithValue("@harga", servicePriceByName[layanan]);
                        cmdLayanan.ExecuteNonQuery();
                    }
                }
            }
        }

        int HitungTotalLayananPerJam()
        {
            int total = 0;

            foreach (string layanan in selectedServices)
            {
                if (servicePriceByName.ContainsKey(layanan))
                    total += servicePriceByName[layanan];
            }

            return total;
        }

        void Logout()
        {
            Close();
            Form1 login = new Form1();
            login.Show();
        }

        Panel CreateGradientBackground()
        {
            Panel bg = new Panel();
            bg.Dock = DockStyle.Fill;
            bg.BackColor = BlueTop;

            bg.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    bg.ClientRectangle,
                    BlueBottom,
                    BlueTop,
                    LinearGradientMode.ForwardDiagonal))
                {
                    e.Graphics.FillRectangle(brush, bg.ClientRectangle);
                }
            };

            return bg;
        }

        Label CreateBackButton()
        {
            Label back = new Label();
            back.Text = "‹";
            back.Font = new Font("Arial", 48, FontStyle.Regular);
            back.ForeColor = DarkBlue;
            back.AutoSize = true;
            back.Location = new Point(35, 25);
            back.Cursor = Cursors.Hand;
            back.BackColor = Color.Transparent;
            return back;
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

        void AddSparkle(Control parent, int x, int y, int size)
        {
            Label sparkle = new Label();
            sparkle.Text = "✦";
            sparkle.Font = new Font("Arial", size, FontStyle.Bold);
            sparkle.ForeColor = Color.Black;
            sparkle.BackColor = Color.Transparent;
            sparkle.AutoSize = true;
            sparkle.Location = new Point(x, y);
            parent.Controls.Add(sparkle);
            sparkle.BringToFront();
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

        Font SafeFont(string fontName, int size, FontStyle style)
        {
            try
            {
                return new Font(fontName, size, style);
            }
            catch
            {
                return new Font("Arial", size, style);
            }
        }
    }
}