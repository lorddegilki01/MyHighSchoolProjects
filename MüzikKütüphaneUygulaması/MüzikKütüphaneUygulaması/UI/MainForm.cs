using System.ComponentModel;
using System.Drawing;
using MüzikKütüphaneUygulaması.Models;
using MüzikKütüphaneUygulaması.Services;

namespace MüzikKütüphaneUygulaması.UI;

public partial class MainForm : Form
{
    private sealed class ComboItem
    {
        public int? Id { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    private readonly AppUser _currentUser;
    private readonly LibraryService _libraryService = new LibraryService();
    private readonly UserService _userService = new UserService();
    private readonly PlaylistService _playlistService = new PlaylistService();

    private readonly List<PlaylistItem> _playlistItems = new List<PlaylistItem>();
    private List<Genre> _genres = new List<Genre>();
    private List<Artist> _artists = new List<Artist>();

    private readonly TextBox _txtSearch = new TextBox();
    private readonly ComboBox _cmbGenreFilter = new ComboBox();
    private readonly ComboBox _cmbSort = new ComboBox();
    private readonly DataGridView _dgvSongs = new DataGridView();

    private readonly DataGridView _dgvPlaylist = new DataGridView();
    private readonly TextBox _txtPlaylistName = new TextBox();
    private readonly Label _lblPlaylistCount = new Label();
    private readonly DataGridView _dgvSavedPlaylists = new DataGridView();

    private readonly TabPage _tabAdmin = new TabPage("Yönetim");
    private readonly DataGridView _dgvAdminSongs = new DataGridView();
    private readonly DataGridView _dgvAdminArtists = new DataGridView();
    private readonly DataGridView _dgvAdminGenres = new DataGridView();
    private readonly DataGridView _dgvAdminUsers = new DataGridView();

    private readonly TextBox _txtSongName = new TextBox();
    private readonly ComboBox _cmbSongArtist = new ComboBox();
    private readonly ComboBox _cmbSongGenre = new ComboBox();
    private readonly TextBox _txtSongAlbum = new TextBox();
    private readonly NumericUpDown _nudSongYear = new NumericUpDown();
    private readonly NumericUpDown _nudSongDuration = new NumericUpDown();

    private readonly TextBox _txtArtistName = new TextBox();
    private readonly TextBox _txtGenreName = new TextBox();

    public MainForm() : this(new AppUser { Isim = "Tasarım Kullanıcısı", Rol = "yonetici" })
    {
    }

    public MainForm(AppUser currentUser)
    {
        _currentUser = currentUser;
        InitializeComponent();
        InitializeUi();

        if (!IsInDesignMode())
        {
            LoadInitialData();
        }
    }

    private static bool IsInDesignMode()
    {
        return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
    }

    private void InitializeUi()
    {
        Controls.Clear();

        Text = "Müzik Kütüphane Pro";
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1300, 780);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(246, 248, 246);
        Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 66,
            BackColor = Color.FromArgb(19, 78, 65)
        };

        var lblTitle = new Label
        {
            Text = "Müzik Kütüphane Masaüstü",
            AutoSize = true,
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point),
            Left = 20,
            Top = 18
        };

        var roleText = _currentUser.IsYonetici ? "Yönetici" : "Kullanıcı";
        var lblUser = new Label
        {
            Text = $"Aktif Kullanıcı: {_currentUser.Isim} ({roleText})",
            AutoSize = true,
            ForeColor = Color.FromArgb(216, 239, 231),
            Top = 24,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };

        var btnLogout = new Button
        {
            Text = "Çıkış",
            Width = 98,
            Height = 36,
            Top = 15,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        StyleButton(btnLogout, true);
        btnLogout.Click += (_, _) => Close();

        header.Controls.Add(lblTitle);
        header.Controls.Add(lblUser);
        header.Controls.Add(btnLogout);

        header.Resize += (_, _) =>
        {
            btnLogout.Left = header.ClientSize.Width - btnLogout.Width - 20;
            lblUser.Left = btnLogout.Left - lblUser.Width - 16;
        };

        var tabs = new TabControl
        {
            Dock = DockStyle.Fill,
            Padding = new Point(16, 8),
            SizeMode = TabSizeMode.Fixed,
            ItemSize = new Size(170, 32)
        };

        tabs.TabPages.Add(CreateUserTab());

        if (_currentUser.IsYonetici)
        {
            _tabAdmin.Controls.Clear();
            _tabAdmin.BackColor = Color.FromArgb(246, 248, 246);
            _tabAdmin.Controls.Add(CreateAdminLayout());
            tabs.TabPages.Add(_tabAdmin);
        }

        var bodyPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(246, 248, 246)
        };

        bodyPanel.Controls.Add(tabs);

        Controls.Add(bodyPanel);
        Controls.Add(header);
    }

    private TabPage CreateUserTab()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.FromArgb(246, 248, 246)
        };

        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29));

        var left = new Panel { Dock = DockStyle.Fill, Padding = new Padding(14, 14, 8, 14) };
        var right = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8, 14, 14, 14) };

        left.Controls.Add(CreateSongsArea());
        right.Controls.Add(CreatePlaylistArea());

        root.Controls.Add(left, 0, 0);
        root.Controls.Add(right, 1, 0);

        var tab = new TabPage("Kullanıcı Paneli") { BackColor = Color.FromArgb(246, 248, 246) };
        tab.Controls.Add(root);
        return tab;
    }

    private Control CreateSongsArea()
    {
        var wrap = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

        var filterPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 146,
            Padding = new Padding(14, 10, 14, 10)
        };

        var filterLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 5,
            RowCount = 3
        };

        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));

        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));

        var lblSearch = new Label { Text = "Arama", Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft, AutoSize = true };
        var lblGenre = new Label { Text = "Tür", Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft, AutoSize = true };
        var lblSort = new Label { Text = "Sıralama", Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft, AutoSize = true };

        _txtSearch.Dock = DockStyle.Fill;
        _txtSearch.Margin = new Padding(0, 2, 10, 0);

        _cmbGenreFilter.Dock = DockStyle.Fill;
        _cmbGenreFilter.Margin = new Padding(0, 2, 10, 0);
        _cmbGenreFilter.DropDownStyle = ComboBoxStyle.DropDownList;

        _cmbSort.Dock = DockStyle.Fill;
        _cmbSort.Margin = new Padding(0, 2, 10, 0);
        _cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;

        var btnApply = new Button { Text = "Uygula", Dock = DockStyle.Fill, Height = 34, Margin = new Padding(0, 2, 10, 0) };
        StyleButton(btnApply);
        btnApply.Click += (_, _) => LoadSongs();

        var btnClear = new Button { Text = "Temizle", Dock = DockStyle.Fill, Height = 34, Margin = new Padding(0, 2, 0, 0) };
        StyleButton(btnClear, true);
        btnClear.Click += BtnClearFilter_Click;

        var btnAdd = new Button { Text = "Seçili Şarkıyı Listeye Ekle", Width = 240, Height = 34, Margin = new Padding(0, 0, 8, 0) };
        StyleButton(btnAdd);
        btnAdd.Click += BtnAddToPlaylist_Click;

        var btnRefresh = new Button { Text = "Şarkıları Yenile", Width = 150, Height = 34, Margin = new Padding(0) };
        StyleButton(btnRefresh, true);
        btnRefresh.Click += (_, _) => LoadSongs();

        var actionRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Margin = new Padding(0, 8, 0, 0),
            Padding = new Padding(0)
        };

        actionRow.Controls.Add(btnAdd);
        actionRow.Controls.Add(btnRefresh);

        filterLayout.Controls.Add(lblSearch, 0, 0);
        filterLayout.Controls.Add(lblGenre, 1, 0);
        filterLayout.Controls.Add(lblSort, 2, 0);

        filterLayout.Controls.Add(_txtSearch, 0, 1);
        filterLayout.Controls.Add(_cmbGenreFilter, 1, 1);
        filterLayout.Controls.Add(_cmbSort, 2, 1);
        filterLayout.Controls.Add(btnApply, 3, 1);
        filterLayout.Controls.Add(btnClear, 4, 1);

        filterLayout.Controls.Add(actionRow, 0, 2);
        filterLayout.SetColumnSpan(actionRow, 3);

        filterPanel.Controls.Add(filterLayout);

        ConfigureGrid(_dgvSongs, true);
        _dgvSongs.Dock = DockStyle.Fill;

        wrap.Controls.Add(_dgvSongs);
        wrap.Controls.Add(filterPanel);

        return wrap;
    }

    private Control CreatePlaylistArea()
    {
        var wrap = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

        var lblTop = new Label
        {
            Text = "Çalma Listesi",
            Dock = DockStyle.Top,
            Height = 40,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point),
            Padding = new Padding(12, 0, 0, 0)
        };

        var topActions = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 182,
            Padding = new Padding(12, 8, 12, 10)
        };

        var actionsLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4
        };

        actionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        actionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        actionsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        actionsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        actionsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        actionsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

        var btnRemove = new Button { Text = "Listeden Çıkar", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 10, 0) };
        StyleButton(btnRemove, true);
        btnRemove.Click += BtnRemoveFromPlaylist_Click;

        var lblPlaylistName = new Label
        {
            Text = "Liste Adı",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.BottomLeft,
            ForeColor = Color.FromArgb(33, 62, 55)
        };

        _txtPlaylistName.Dock = DockStyle.Fill;
        _txtPlaylistName.Margin = new Padding(0, 2, 0, 0);
        _txtPlaylistName.PlaceholderText = "Liste adı";

        var btnSave = new Button { Text = "Listeyi Kaydet", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 10, 0) };
        StyleButton(btnSave);
        btnSave.Click += BtnSavePlaylist_Click;

        _lblPlaylistCount.AutoSize = true;
        _lblPlaylistCount.Anchor = AnchorStyles.Left;
        _lblPlaylistCount.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
        _lblPlaylistCount.Text = "Şarkı: 0";

        actionsLayout.Controls.Add(btnRemove, 0, 0);
        actionsLayout.Controls.Add(lblPlaylistName, 0, 1);
        actionsLayout.SetColumnSpan(lblPlaylistName, 2);
        actionsLayout.Controls.Add(_txtPlaylistName, 0, 2);
        actionsLayout.SetColumnSpan(_txtPlaylistName, 2);
        actionsLayout.Controls.Add(btnSave, 0, 3);
        actionsLayout.Controls.Add(_lblPlaylistCount, 1, 3);

        topActions.Controls.Add(actionsLayout);

        ConfigureGrid(_dgvPlaylist, false);
        _dgvPlaylist.Dock = DockStyle.Fill;

        var savedWrap = new Panel { Dock = DockStyle.Bottom, Height = 190, Padding = new Padding(8, 6, 8, 8) };
        var savedTitle = new Label
        {
            Text = "Kaydedilen Listeler",
            Dock = DockStyle.Top,
            Height = 24,
            Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
        };

        ConfigureGrid(_dgvSavedPlaylists, false);
        _dgvSavedPlaylists.Dock = DockStyle.Fill;
        savedWrap.Controls.Add(_dgvSavedPlaylists);
        savedWrap.Controls.Add(savedTitle);

        wrap.Controls.Add(_dgvPlaylist);
        wrap.Controls.Add(savedWrap);
        wrap.Controls.Add(topActions);
        wrap.Controls.Add(lblTop);

        return wrap;
    }

    private Control CreateAdminLayout()
    {
        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(CreateAdminSongsTab());
        tabs.TabPages.Add(CreateAdminArtistsTab());
        tabs.TabPages.Add(CreateAdminGenresTab());
        tabs.TabPages.Add(CreateAdminUsersTab());
        return tabs;
    }

    private TabPage CreateAdminSongsTab()
    {
        var tab = new TabPage("Şarkı Yönetimi") { BackColor = Color.FromArgb(246, 248, 246) };
        var top = new Panel { Dock = DockStyle.Top, Height = 126, Padding = new Padding(10) };

        _txtSongName.SetBounds(0, 8, 210, 28);
        _txtSongName.PlaceholderText = "Şarkı adı";

        _cmbSongArtist.SetBounds(216, 8, 180, 28);
        _cmbSongArtist.DropDownStyle = ComboBoxStyle.DropDownList;

        _cmbSongGenre.SetBounds(402, 8, 180, 28);
        _cmbSongGenre.DropDownStyle = ComboBoxStyle.DropDownList;

        _txtSongAlbum.SetBounds(588, 8, 160, 28);
        _txtSongAlbum.PlaceholderText = "Albüm";

        _nudSongYear.SetBounds(754, 8, 90, 28);
        _nudSongYear.Minimum = 0;
        _nudSongYear.Maximum = 2100;

        _nudSongDuration.SetBounds(850, 8, 90, 28);
        _nudSongDuration.Minimum = 30;
        _nudSongDuration.Maximum = 900;
        _nudSongDuration.Value = 180;

        var btnAdd = new Button { Text = "Ekle", Left = 0, Top = 46, Width = 110, Height = 34 };
        StyleButton(btnAdd);
        btnAdd.Click += BtnAdminAddSong_Click;

        var btnUpdate = new Button { Text = "Güncelle", Left = 116, Top = 46, Width = 110, Height = 34 };
        StyleButton(btnUpdate, true);
        btnUpdate.Click += BtnAdminUpdateSong_Click;

        var btnDelete = new Button { Text = "Sil", Left = 232, Top = 46, Width = 90, Height = 34 };
        StyleButton(btnDelete, true);
        btnDelete.Click += BtnAdminDeleteSong_Click;

        var btnReload = new Button { Text = "Yenile", Left = 328, Top = 46, Width = 90, Height = 34 };
        StyleButton(btnReload, true);
        btnReload.Click += (_, _) => LoadAdminSongs();

        top.Controls.AddRange(new Control[]
        {
            _txtSongName, _cmbSongArtist, _cmbSongGenre, _txtSongAlbum,
            _nudSongYear, _nudSongDuration, btnAdd, btnUpdate, btnDelete, btnReload
        });

        ConfigureGrid(_dgvAdminSongs, true);
        _dgvAdminSongs.Dock = DockStyle.Fill;
        _dgvAdminSongs.SelectionChanged += DgvAdminSongs_SelectionChanged;

        tab.Controls.Add(_dgvAdminSongs);
        tab.Controls.Add(top);
        return tab;
    }

    private TabPage CreateAdminArtistsTab()
    {
        var tab = new TabPage("Sanatçı Yönetimi") { BackColor = Color.FromArgb(246, 248, 246) };
        var top = new Panel { Dock = DockStyle.Top, Height = 72, Padding = new Padding(10) };

        _txtArtistName.SetBounds(0, 14, 250, 28);
        _txtArtistName.PlaceholderText = "Sanatçı adı";

        var btnAdd = new Button { Text = "Ekle", Left = 256, Top = 12, Width = 100, Height = 32 };
        StyleButton(btnAdd);
        btnAdd.Click += BtnAdminAddArtist_Click;

        var btnUpdate = new Button { Text = "Güncelle", Left = 362, Top = 12, Width = 100, Height = 32 };
        StyleButton(btnUpdate, true);
        btnUpdate.Click += BtnAdminUpdateArtist_Click;

        var btnDelete = new Button { Text = "Sil", Left = 468, Top = 12, Width = 88, Height = 32 };
        StyleButton(btnDelete, true);
        btnDelete.Click += BtnAdminDeleteArtist_Click;

        top.Controls.AddRange(new Control[] { _txtArtistName, btnAdd, btnUpdate, btnDelete });

        ConfigureGrid(_dgvAdminArtists, true);
        _dgvAdminArtists.Dock = DockStyle.Fill;
        _dgvAdminArtists.SelectionChanged += DgvAdminArtists_SelectionChanged;

        tab.Controls.Add(_dgvAdminArtists);
        tab.Controls.Add(top);
        return tab;
    }

    private TabPage CreateAdminGenresTab()
    {
        var tab = new TabPage("Tür Yönetimi") { BackColor = Color.FromArgb(246, 248, 246) };
        var top = new Panel { Dock = DockStyle.Top, Height = 72, Padding = new Padding(10) };

        _txtGenreName.SetBounds(0, 14, 250, 28);
        _txtGenreName.PlaceholderText = "Tür adı";

        var btnAdd = new Button { Text = "Ekle", Left = 256, Top = 12, Width = 100, Height = 32 };
        StyleButton(btnAdd);
        btnAdd.Click += BtnAdminAddGenre_Click;

        var btnUpdate = new Button { Text = "Güncelle", Left = 362, Top = 12, Width = 100, Height = 32 };
        StyleButton(btnUpdate, true);
        btnUpdate.Click += BtnAdminUpdateGenre_Click;

        var btnDelete = new Button { Text = "Sil", Left = 468, Top = 12, Width = 88, Height = 32 };
        StyleButton(btnDelete, true);
        btnDelete.Click += BtnAdminDeleteGenre_Click;

        top.Controls.AddRange(new Control[] { _txtGenreName, btnAdd, btnUpdate, btnDelete });

        ConfigureGrid(_dgvAdminGenres, true);
        _dgvAdminGenres.Dock = DockStyle.Fill;
        _dgvAdminGenres.SelectionChanged += DgvAdminGenres_SelectionChanged;

        tab.Controls.Add(_dgvAdminGenres);
        tab.Controls.Add(top);
        return tab;
    }

    private TabPage CreateAdminUsersTab()
    {
        var tab = new TabPage("Kullanıcı Yönetimi") { BackColor = Color.FromArgb(246, 248, 246) };
        var top = new Panel { Dock = DockStyle.Top, Height = 62, Padding = new Padding(10) };

        var btnRefresh = new Button { Text = "Listeyi Yenile", Left = 0, Top = 12, Width = 120, Height = 32 };
        StyleButton(btnRefresh, true);
        btnRefresh.Click += (_, _) => LoadUsers();

        var btnDelete = new Button { Text = "Seçiliyi Sil", Left = 126, Top = 12, Width = 110, Height = 32 };
        StyleButton(btnDelete, true);
        btnDelete.Click += BtnAdminDeleteUser_Click;

        top.Controls.Add(btnRefresh);
        top.Controls.Add(btnDelete);

        ConfigureGrid(_dgvAdminUsers, true);
        _dgvAdminUsers.Dock = DockStyle.Fill;

        tab.Controls.Add(_dgvAdminUsers);
        tab.Controls.Add(top);
        return tab;
    }

    private static void ConfigureGrid(DataGridView grid, bool fullRow)
    {
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.MultiSelect = false;
        grid.SelectionMode = fullRow ? DataGridViewSelectionMode.FullRowSelect : DataGridViewSelectionMode.FullRowSelect;
        grid.AutoGenerateColumns = true;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BorderStyle = BorderStyle.None;
        grid.BackgroundColor = Color.White;
        grid.RowHeadersVisible = false;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(13, 74, 63);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
        grid.DefaultCellStyle.BackColor = Color.FromArgb(243, 250, 247);
        grid.DefaultCellStyle.ForeColor = Color.FromArgb(32, 52, 47);
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 119, 105);
        grid.DefaultCellStyle.SelectionForeColor = Color.White;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(232, 244, 239);
    }

    private static void StyleButton(Button button, bool secondary = false)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Font = new Font("Segoe UI Semibold", 9.6F, FontStyle.Bold, GraphicsUnit.Point);
        button.ForeColor = Color.White;
        button.BackColor = secondary ? Color.FromArgb(67, 111, 101) : Color.FromArgb(22, 101, 85);
    }
}







