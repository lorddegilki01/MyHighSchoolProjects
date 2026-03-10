using System.Drawing;
using System.Drawing.Drawing2D;
using MiniE_TicaretUygulaması.Services;

namespace MiniE_TicaretUygulaması.UI;

public partial class LoginForm : Form
{
    private readonly AuthService _authService = new AuthService();

    private readonly TextBox _txtKullaniciAdi = new TextBox();
    private readonly TextBox _txtPassword = new TextBox();
    private readonly Button _btnLogin = new Button();
    private readonly CheckBox _chkShowPassword = new CheckBox();
    private readonly Panel _cardPanel = new Panel();
    private readonly Panel _cardShadow = new Panel();

    public LoginForm()
    {
        InitializeComponent();
        InitializeUi();
    }


    private void InitializeUi()
    {
        Controls.Clear();

        Text = "Mini E-Ticaret - Giriş";
        StartPosition = FormStartPosition.CenterScreen;
        Width = 980;
        Height = 620;
        MinimumSize = new Size(900, 560);
        BackColor = Color.FromArgb(233, 240, 250);
        Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        DoubleBuffered = true;

        Paint -= LoginForm_Paint;
        Paint += LoginForm_Paint;

        _cardShadow.Size = new Size(780, 440);
        _cardShadow.BackColor = Color.FromArgb(211, 221, 235);
        _cardShadow.Enabled = false;

        _cardPanel.Size = new Size(760, 420);
        _cardPanel.BackColor = Color.White;
        _cardPanel.BorderStyle = BorderStyle.FixedSingle;

        _cardPanel.Controls.Clear();
        _cardPanel.Controls.Add(BuildLoginPanel());
        _cardPanel.Controls.Add(BuildBrandPanel());

        Controls.Add(_cardShadow);
        Controls.Add(_cardPanel);
        _cardShadow.SendToBack();
        _cardPanel.BringToFront();

        Resize -= LoginForm_Resize;
        Resize += LoginForm_Resize;
        CenterLoginCard();

        AcceptButton = _btnLogin;
    }

    private Panel BuildBrandPanel()
    {
        var brandPanel = new GradientPanel(
            Color.FromArgb(24, 43, 73),
            Color.FromArgb(33, 77, 143),
            132f)
        {
            Dock = DockStyle.Left,
            Width = 280
        };

        var lblProduct = new Label
        {
            Text = "MINI E-TICARET",
            AutoSize = true,
            ForeColor = Color.FromArgb(226, 236, 252),
            Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point),
            Left = 28,
            Top = 34
        };

        var lblTitle = new Label
        {
            Text = "Hoş geldiniz",
            AutoSize = true,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 25F, FontStyle.Bold, GraphicsUnit.Point),
            Left = 28,
            Top = 80
        };

        var lblSubtitle = new Label
        {
            Text = "Siparişlerinizi güvenli bir şekilde\nyönetin.",
            AutoSize = true,
            ForeColor = Color.FromArgb(224, 235, 252),
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
            Left = 30,
            Top = 148
        };

        var line = new Panel
        {
            Left = 30,
            Top = 220,
            Width = 220,
            Height = 1,
            BackColor = Color.FromArgb(110, 153, 209)
        };

        var lblInfo1 = new Label
        {
            Text = "Rol bazlı erişim",
            AutoSize = true,
            ForeColor = Color.FromArgb(234, 242, 252),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
            Left = 30,
            Top = 240
        };

        var lblInfo2 = new Label
        {
            Text = "Güvenli şifre doğrulama",
            AutoSize = true,
            ForeColor = Color.FromArgb(234, 242, 252),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
            Left = 30,
            Top = 266
        };

        var lblInfo3 = new Label
        {
            Text = "Hızlı ürün ve sipariş akışı",
            AutoSize = true,
            ForeColor = Color.FromArgb(234, 242, 252),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
            Left = 30,
            Top = 292
        };

        foreach (var control in new Control[] { lblProduct, lblTitle, lblSubtitle, lblInfo1, lblInfo2, lblInfo3 })
        {
            control.BackColor = Color.Transparent;
        }

        brandPanel.Controls.Add(lblProduct);
        brandPanel.Controls.Add(lblTitle);
        brandPanel.Controls.Add(lblSubtitle);
        brandPanel.Controls.Add(line);
        brandPanel.Controls.Add(lblInfo1);
        brandPanel.Controls.Add(lblInfo2);
        brandPanel.Controls.Add(lblInfo3);

        return brandPanel;
    }

    private Panel BuildLoginPanel()
    {
        var loginPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(42, 30, 42, 28)
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 9
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var lblHeader = new Label
        {
            Text = "Hesabına Giriş Yap",
            AutoSize = true,
            ForeColor = Color.FromArgb(34, 52, 82),
            Font = new Font("Segoe UI Semibold", 19F, FontStyle.Bold, GraphicsUnit.Point),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        var lblSub = new Label
        {
            Text = "Bilgilerinizi girerek devam edin.",
            AutoSize = true,
            ForeColor = Color.FromArgb(108, 122, 146),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.TopLeft
        };

        var lblKullaniciAdi = new Label
        {
            Text = "Kullanıcı Adı",
            AutoSize = true,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.BottomLeft,
            ForeColor = Color.FromArgb(40, 58, 86),
            Font = new Font("Segoe UI Semibold", 9.2F, FontStyle.Bold, GraphicsUnit.Point)
        };

        _txtKullaniciAdi.Dock = DockStyle.Fill;
        _txtKullaniciAdi.Margin = new Padding(0, 4, 0, 8);
        _txtKullaniciAdi.BorderStyle = BorderStyle.FixedSingle;
        _txtKullaniciAdi.Font = new Font("Segoe UI", 10.3F, FontStyle.Regular, GraphicsUnit.Point);
        _txtKullaniciAdi.Text = "Sistem Yöneticisi";

        var lblPassword = new Label
        {
            Text = "Şifre",
            AutoSize = true,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.BottomLeft,
            ForeColor = Color.FromArgb(40, 58, 86),
            Font = new Font("Segoe UI Semibold", 9.2F, FontStyle.Bold, GraphicsUnit.Point)
        };

        _txtPassword.Dock = DockStyle.Fill;
        _txtPassword.Margin = new Padding(0, 4, 0, 8);
        _txtPassword.BorderStyle = BorderStyle.FixedSingle;
        _txtPassword.UseSystemPasswordChar = true;
        _txtPassword.Font = new Font("Segoe UI", 10.3F, FontStyle.Regular, GraphicsUnit.Point);
        _txtPassword.Text = "Admin123!";

        _chkShowPassword.Text = "Şifreyi göster";
        _chkShowPassword.AutoSize = true;
        _chkShowPassword.ForeColor = Color.FromArgb(78, 96, 122);
        _chkShowPassword.Dock = DockStyle.Fill;
        _chkShowPassword.Margin = new Padding(0, 2, 0, 0);
        _chkShowPassword.CheckedChanged += (_, _) =>
        {
            _txtPassword.UseSystemPasswordChar = !_chkShowPassword.Checked;
        };

        _btnLogin.Text = "Giriş Yap";
        _btnLogin.Dock = DockStyle.Fill;
        _btnLogin.Height = 44;
        _btnLogin.FlatStyle = FlatStyle.Flat;
        _btnLogin.FlatAppearance.BorderSize = 0;
        _btnLogin.BackColor = Color.FromArgb(33, 77, 143);
        _btnLogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(29, 69, 130);
        _btnLogin.ForeColor = Color.White;
        _btnLogin.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
        _btnLogin.Cursor = Cursors.Hand;
        _btnLogin.Click += BtnLogin_Click;

        var demoPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(245, 248, 253),
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(12)
        };

        var demoTitle = new Label
        {
            Text = "Demo Hesaplar",
            Dock = DockStyle.Top,
            AutoSize = false,
            Height = 22,
            ForeColor = Color.FromArgb(46, 66, 98),
            Font = new Font("Segoe UI Semibold", 9.3F, FontStyle.Bold, GraphicsUnit.Point)
        };

        var demoText = new Label
        {
            Text = "Admin: Sistem Yöneticisi / Admin123!\nKullanıcı: Demo Kullanıcı / Kullanici123!",
            Dock = DockStyle.Fill,
            AutoSize = false,
            ForeColor = Color.FromArgb(84, 102, 129),
            Font = new Font("Segoe UI", 9.1F, FontStyle.Regular, GraphicsUnit.Point)
        };

        demoPanel.Controls.Add(demoText);
        demoPanel.Controls.Add(demoTitle);

        layout.Controls.Add(lblHeader, 0, 0);
        layout.Controls.Add(lblSub, 0, 1);
        layout.Controls.Add(lblKullaniciAdi, 0, 2);
        layout.Controls.Add(_txtKullaniciAdi, 0, 3);
        layout.Controls.Add(lblPassword, 0, 4);
        layout.Controls.Add(_txtPassword, 0, 5);
        layout.Controls.Add(_chkShowPassword, 0, 6);
        layout.Controls.Add(_btnLogin, 0, 7);
        layout.Controls.Add(demoPanel, 0, 8);

        loginPanel.Controls.Add(layout);
        return loginPanel;
    }

    private void LoginForm_Resize(object sender, EventArgs e)
    {
        CenterLoginCard();
        Invalidate();
    }

    private void CenterLoginCard()
    {
        var x = (ClientSize.Width - _cardPanel.Width) / 2;
        var y = (ClientSize.Height - _cardPanel.Height) / 2;

        if (y < 16)
        {
            y = 16;
        }

        _cardShadow.Location = new Point(x + 8, y + 10);
        _cardPanel.Location = new Point(x, y);
        _cardShadow.SendToBack();
        _cardPanel.BringToFront();
    }

    private void LoginForm_Paint(object sender, PaintEventArgs e)
    {
        using var brush = new LinearGradientBrush(
            ClientRectangle,
            Color.FromArgb(233, 240, 250),
            Color.FromArgb(214, 225, 241),
            125f);

        e.Graphics.FillRectangle(brush, ClientRectangle);
    }

    private void BtnLogin_Click(object sender, EventArgs e)
    {
        var kullaniciAdi = _txtKullaniciAdi.Text.Trim();
        var password = _txtPassword.Text;

        if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Kullanıcı adı ve şifre zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _btnLogin.Enabled = false;
            var user = _authService.Login(kullaniciAdi, password);

            if (user is null)
            {
                MessageBox.Show("Hatalı giriş. Bilgilerinizi kontrol edin.", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var mainForm = new MainForm(user);
            mainForm.FormClosed += (_, _) =>
            {
                _txtPassword.Clear();
                Show();
            };

            Hide();
            mainForm.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Giriş sırasında hata oluştu:\n\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _btnLogin.Enabled = true;
        }
    }

    private sealed class GradientPanel(Color colorStart, Color colorEnd, float angle) : Panel
    {
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using var brush = new LinearGradientBrush(ClientRectangle, colorStart, colorEnd, angle);
            e.Graphics.FillRectangle(brush, ClientRectangle);
        }
    }
}





