using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Triosik
{
    public partial class Form1 : Form
    {
        TextBox txtUsername, txtPassword;
        ComboBox cmbRole;

        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Triosik;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public Form1()
        {
            InitializeComponent();
            BuildUI();
        }

        private void Form1_Load(object sender, EventArgs e) { }

        void BuildUI()
        {
            this.Text = "Triosic Login";

            // --- PENGATURAN FULLSCREEN KIOSK MODE ---
            this.FormBorderStyle = FormBorderStyle.None; // Hilangkan border dan tombol close/min/max
            this.WindowState = FormWindowState.Maximized; // Penuhi seluruh layar
            this.TopMost = true; // (Opsional) Selalu di depan taskbar atau aplikasi lain

            this.BackColor = Color.FromArgb(31, 126, 224);
            this.DoubleBuffered = true;

            // Blob background akan melar otomatis ke bawah karena Anchoring & Resize event
            Panel blob = new Panel();
            blob.Size = new Size(720, Screen.PrimaryScreen.Bounds.Height); // Ambil tinggi layar langsung
            blob.Location = new Point(0, 0);
            blob.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            blob.BackColor = Color.Transparent;
            blob.Paint += Blob_Paint;
            this.Controls.Add(blob);

            this.Resize += (s, e) =>
            {
                blob.Height = this.ClientSize.Height;
                blob.Invalidate();
            };

            // --- ELEMEN LOGIN ---
            Label logo = new Label();
            logo.Text = "Triosic";
            logo.Font = new Font("Chubby And Groovy", 80, FontStyle.Bold);
            logo.ForeColor = Color.White;
            logo.AutoSize = true;
            logo.BackColor = Color.Transparent;
            logo.Location = new Point(880, 60);
            this.Controls.Add(logo);

            Label studio = new Label();
            studio.Text = "Music Studio";
            studio.Font = new Font("Ground Castle DEMO", 40, FontStyle.Bold);
            studio.ForeColor = Color.White;
            studio.AutoSize = true;
            studio.BackColor = Color.Transparent;
            studio.Location = new Point(890, 160);
            this.Controls.Add(studio);

            AddLabel("Username", 760, 245);
            txtUsername = AddTextBox(760, 275);

            AddLabel("Password", 760, 365);
            txtPassword = AddTextBox(760, 395);
            txtPassword.UseSystemPasswordChar = true;

            AddLabel("Role", 760, 485);
            cmbRole = AddComboBox(760, 515);

            Button login = new Button();
            login.Text = "Login";
            login.Size = new Size(250, 62);
            login.Location = new Point(925, 610);
            login.BackColor = Color.FromArgb(255, 230, 55);
            login.ForeColor = Color.FromArgb(31, 126, 224);
            login.FlatStyle = FlatStyle.Flat;
            login.FlatAppearance.BorderSize = 0;
            login.Font = new Font("Chubby And Groovy", 40, FontStyle.Bold);
            login.Cursor = Cursors.Hand;

            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, 25, 25, 180, 90);
            path.AddArc(login.Width - 25, 0, 25, 25, 270, 90);
            path.AddArc(login.Width - 25, login.Height - 25, 25, 25, 0, 90);
            path.AddArc(0, login.Height - 25, 25, 25, 90, 90);
            path.CloseAllFigures();

            login.Region = new Region(path);

            login.Click += BtnLogin_Click;
            this.Controls.Add(login);

            Label guest = new Label();
            guest.Text = "Masuk sebagai Guest/User?";
            guest.Font = new Font("Anek Devanagari", 18);
            guest.ForeColor = Color.FromArgb(245, 220, 240);
            guest.AutoSize = true;
            guest.BackColor = Color.Transparent;
            guest.Cursor = Cursors.Hand;
            guest.Location = new Point(895, 690);

            guest.Click += (s, e) =>
            {
                DashboardGuest guestForm = new DashboardGuest();
                guestForm.Show();
                this.Hide();
            };

            this.Controls.Add(guest);
            guest.BringToFront();
        }

        void AddLabel(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            lbl.Font = new Font("Anek Devanagari", 15);
            lbl.ForeColor = Color.FromArgb(245, 220, 240);
            lbl.BackColor = Color.Transparent;
            this.Controls.Add(lbl);
        }

        TextBox AddTextBox(int x, int y)
        {
            Panel border = new Panel();
            border.Location = new Point(x, y);
            border.Size = new Size(545, 50);
            border.BackColor = Color.FromArgb(31, 126, 224);
            border.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(225, 170, 255), 3))
                {
                    e.Graphics.DrawRectangle(pen, 1, 1, border.Width - 3, border.Height - 3);
                }
            };
            this.Controls.Add(border);

            TextBox txt = new TextBox();
            txt.Location = new Point(10, 9);
            txt.Size = new Size(525, 35);
            txt.Font = new Font("Anek Devanagari", 17);
            txt.BackColor = Color.FromArgb(31, 126, 224);
            txt.ForeColor = Color.White;
            txt.BorderStyle = BorderStyle.None;

            border.Controls.Add(txt);
            border.BringToFront();

            return txt;
        }

        ComboBox AddComboBox(int x, int y)
        {
            ComboBox cmb = new ComboBox();
            cmb.Location = new Point(x, y);
            cmb.Size = new Size(545, 50);
            cmb.Font = new Font("Anek Devanagari", 17);
            cmb.BackColor = Color.FromArgb(31, 126, 224);
            cmb.ForeColor = Color.White;
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;

            cmb.Items.Add("Pilih");
            cmb.Items.Add("Admin");
            cmb.Items.Add("Kasir");
            cmb.SelectedIndex = 0;

            this.Controls.Add(cmb);
            return cmb;
        }

        void BtnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "" || txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("Username dan password wajib diisi!");
                return;
            }

            if (cmbRole.Text == "Pilih")
            {
                MessageBox.Show("Pilih role dulu!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query = @"SELECT COUNT(*) FROM Users 
                             WHERE username = @username 
                             AND password = @password 
                             AND role = @role";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());
                        cmd.Parameters.AddWithValue("@role", cmbRole.Text);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            if (cmbRole.Text == "Admin")
                            {
                                DashboardAdmin adminForm = new DashboardAdmin();
                                adminForm.Show();
                                this.Hide();
                                MessageBox.Show("Login berhasil sebagai Admin!");
                            }
                            else if (cmbRole.Text == "Kasir")
                            {
                                /*DashboardKasir kasirForm = new DashboardKasir();
                                kasirForm.Show();
                                this.Hide();*/
                            }
                        }
                        else
                        {
                            MessageBox.Show("Username, password, atau role salah!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi database gagal:\n" + ex.Message);
            }
        }

        void Blob_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            int h = p.Height;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            GraphicsPath path = new GraphicsPath();
            path.AddBezier(0, 0, 650, 0, 760, h * 25 / 100, 690, h * 45 / 100);
            path.AddBezier(690, h * 45 / 100, 610, h * 58 / 100, 600, h * 78 / 100, 500, h * 88 / 100);
            path.AddBezier(500, h * 88 / 100, 350, h, 180, h, 0, h);
            path.AddLine(0, h, 0, 0);
            path.CloseFigure();

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(95, 166, 235)))
            {
                e.Graphics.FillPath(brush, path);
            }

            e.Graphics.DrawImage(Properties.Resources.Kuning, new Rectangle(30, 105, 340, 300));
            e.Graphics.DrawImage(Properties.Resources.Pink, new Rectangle(330, 240, 310, 250));
            e.Graphics.DrawImage(Properties.Resources.Hitam, new Rectangle(80, h - 315, 330, 260));
        }
    }
}