using System.Drawing;
using System.Drawing.Drawing2D;
using MüzikKütüphaneUygulaması.Services;

namespace MüzikKütüphaneUygulaması.UI;

public partial class LoginForm : Form
{
    private readonly AuthService _authService = new AuthService();

    private readonly Panel _cardPanel = new Panel();
    private readonly Panel _shadowPanel = new Panel();
    private readonly TextBox _txtEmail = new TextBox();
    private readonly TextBox _txtSifre = new TextBox();
    private readonly CheckBox _chkSifreGoster = new CheckBox();
    private readonly Button _btnGiris = new Button();

    public LoginForm()
    {
        InitializeComponent();
        InitializeUi();
    }

    private void InitializeUi()
    {
        Text = "Müzik Kütüphane Pro | Giriş";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(920, 560);
        Size = new Size(980, 620);
        BackColor = Color.FromArgb(237, 244, 241);
        Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        DoubleBuffered = true;

        Paint -= LoginForm_Paint;
        Paint += LoginForm_Paint;

        _shadowPanel.Size = new Size(760, 420);
        _shadowPanel.BackColor = Color.FromArgb(206, 223, 216);
        _shadowPanel.Enabled = false;

        _cardPanel.Size = new Size(740, 400);
        _cardPanel.BackColor = Color.White;
        _cardPanel.BorderStyle = BorderStyle.FixedSingle;
        _cardPanel.Controls.Clear();
        _cardPanel.Controls.Add(BuildLoginPanel());
        _cardPanel.Controls.Add(BuildInfoPanel());

        Controls.Add(_shadowPanel);
        Controls.Add(_cardPanel);

        Resize -= LoginForm_Resize;
        Resize += LoginForm_Resize;
        CenterCard();

        AcceptButton = _btnGiris;
    }

    private Control BuildInfoPanel()
    {
        var panel = new GradientPanel(Color.FromArgb(13, 74, 63), Color.FromArgb(29, 128, 108), 130f)
        {
            Dock = DockStyle.Left,
            Width = 280
        };

        var lblTitle = new Label
        {
            Text = "MÜZİK KÜTÜPHANE",
            AutoSize = true,
            ForeColor = Color.FromArgb(208, 245, 236),
            Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point),
            Left = 30,
            Top = 32
        };

        var lblHero = new Label
        {
            Text = "Ritmini\nYönet",
            AutoSize = true,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 32F, FontStyle.Bold, GraphicsUnit.Point),
            Left = 28,
            Top = 82
        };

        var lblText = new Label
        {
            Text = "Şarkılarınızı, sanatçıları ve\nçalma listelerinizi tek panelde yönetin.",
            AutoSize = true,
            ForeColor = Color.FromArgb(225, 248, 241),
            Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point),
            Left = 30,
            Top = 198
        };

        var lblDemo = new Label
        {
            Text = "Demo: yonetici@muzik.local / Yonetici123!\n      kullanici@muzik.local / Kullanici123!",
            AutoSize = true,
            ForeColor = Color.FromArgb(208, 245, 236),
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
            Left = 30,
            Top = 306
        };

        foreach (var control in new Control[] { lblTitle, lblHero, lblText, lblDemo })
        {
            control.BackColor = Color.Transparent;
        }

        panel.Controls.Add(lblTitle);
        panel.Controls.Add(lblHero);
        panel.Controls.Add(lblText);
        panel.Controls.Add(lblDemo);

        return panel;
    }

    private Control BuildLoginPanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(42, 30, 42, 34)
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 10
        };

        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 8));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var lblHeader = new Label
        {
            Text = "Hesabına Giriş Yap",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(18, 42, 36),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var lblSub = new Label
        {
            Text = "Rol bazlı müzik yönetim paneline devam edin.",
            Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(82, 110, 102),
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var lblEmail = new Label
        {
            Text = "E-posta",
            Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(24, 64, 55),
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point),
            TextAlign = ContentAlignment.BottomLeft
        };

        _txtEmail.Dock = DockStyle.Fill;
        _txtEmail.Margin = new Padding(0, 2, 0, 8);
        _txtEmail.BorderStyle = BorderStyle.FixedSingle;
        _txtEmail.Text = "yonetici@muzik.local";

        var lblSifre = new Label
        {
            Text = "Şifre",
            Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(24, 64, 55),
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point),
            TextAlign = ContentAlignment.BottomLeft
        };

        _txtSifre.Dock = DockStyle.Fill;
        _txtSifre.Margin = new Padding(0, 2, 0, 8);
        _txtSifre.BorderStyle = BorderStyle.FixedSingle;
        _txtSifre.UseSystemPasswordChar = true;
        _txtSifre.Text = "Yonetici123!";

        _chkSifreGoster.Text = "Şifreyi göster";
        _chkSifreGoster.AutoSize = true;
        _chkSifreGoster.ForeColor = Color.FromArgb(78, 110, 101);
        _chkSifreGoster.Margin = new Padding(0);
        _chkSifreGoster.CheckedChanged += (_, _) => _txtSifre.UseSystemPasswordChar = !_chkSifreGoster.Checked;

        _btnGiris.Dock = DockStyle.Fill;
        _btnGiris.Height = 48;
        _btnGiris.Text = "Giriş Yap";
        _btnGiris.FlatStyle = FlatStyle.Flat;
        _btnGiris.FlatAppearance.BorderSize = 0;
        _btnGiris.BackColor = Color.FromArgb(22, 101, 85);
        _btnGiris.ForeColor = Color.White;
        _btnGiris.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
        _btnGiris.Click += BtnGiris_Click;

        layout.Controls.Add(lblHeader, 0, 0);
        layout.Controls.Add(lblSub, 0, 1);
        layout.Controls.Add(lblEmail, 0, 2);
        layout.Controls.Add(_txtEmail, 0, 3);
        layout.Controls.Add(lblSifre, 0, 4);
        layout.Controls.Add(_txtSifre, 0, 5);
        layout.Controls.Add(_chkSifreGoster, 0, 6);
        layout.Controls.Add(_btnGiris, 0, 7);

        panel.Controls.Add(layout);
        return panel;
    }

    private void BtnGiris_Click(object sender, EventArgs e)
    {
        var email = _txtEmail.Text.Trim();
        var sifre = _txtSifre.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(sifre))
        {
            MessageBox.Show("E-posta ve şifre zorunludur.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var user = _authService.Login(email, sifre);
            if (user is null)
            {
                MessageBox.Show("E-posta veya şifre hatalı.", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _txtSifre.SelectAll();
                _txtSifre.Focus();
                return;
            }

            Hide();
            var main = new MainForm(user);
            main.FormClosed += (_, _) => Close();
            main.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Giriş sırasında hata oluştu:\n\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CenterCard()
    {
        var x = (ClientSize.Width - _cardPanel.Width) / 2;
        var y = (ClientSize.Height - _cardPanel.Height) / 2;

        _cardPanel.Location = new Point(Math.Max(24, x), Math.Max(24, y));
        _shadowPanel.Location = new Point(_cardPanel.Left + 8, _cardPanel.Top + 10);

        _shadowPanel.SendToBack();
        _cardPanel.BringToFront();
    }

    private void LoginForm_Resize(object sender, EventArgs e)
    {
        CenterCard();
        Invalidate();
    }

    private void LoginForm_Paint(object sender, PaintEventArgs e)
    {
        using var brush = new LinearGradientBrush(ClientRectangle, Color.FromArgb(236, 245, 241), Color.FromArgb(215, 236, 228), 95f);
        e.Graphics.FillRectangle(brush, ClientRectangle);
    }

    private sealed class GradientPanel : Panel
    {
        private readonly Color _start;
        private readonly Color _end;
        private readonly float _angle;

        public GradientPanel(Color start, Color end, float angle)
        {
            _start = start;
            _end = end;
            _angle = angle;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using var brush = new LinearGradientBrush(ClientRectangle, _start, _end, _angle);
            e.Graphics.FillRectangle(brush, ClientRectangle);
        }
    }
}







