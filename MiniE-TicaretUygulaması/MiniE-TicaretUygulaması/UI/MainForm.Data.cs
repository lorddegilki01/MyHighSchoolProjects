using System.IO;

namespace MiniE_TicaretUygulaması.UI;

public sealed partial class MainForm
{
    private void LoadInitialData()
    {
        var errors = new List<string>();

        TryLoadStep("Kategoriler", LoadCategories, errors);
        TryLoadStep("Ürünler", LoadShopProducts, errors);
        TryLoadStep("Sepet", RefreshCartGrid, errors);
        TryLoadStep("Siparişlerim", LoadMyOrders, errors);

        if (_currentUser.IsAdmin)
        {
            TryLoadStep("Admin Ürünler", LoadAdminProducts, errors);
            TryLoadStep("Admin Kategoriler", LoadCategoryGrid, errors);
            TryLoadStep("Admin Kullanıcılar", LoadUsers, errors);
            TryLoadStep("Admin Siparişler", LoadAllOrders, errors);
        }

        if (errors.Count == 0)
        {
            return;
        }

        var joined = string.Join("\n\n", errors);
        var message = $"Veriler yüklenirken hata oluştu:\n\n{joined}";

        if (Environment.GetEnvironmentVariable("MINI_ETICARET_SILENT_LOAD_ERRORS") == "1")
        {
            return;
        }

        MessageBox.Show(message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private static void TryLoadStep(string stepName, Action action, List<string> errors)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            errors.Add($"{stepName}: {ex.Message}");
            AppendLoadError(stepName, ex);
        }
    }

    private static void AppendLoadError(string stepName, Exception ex)
    {
        try
        {
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "load-errors.log");
            var content =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {stepName}{Environment.NewLine}" +
                $"{ex}{Environment.NewLine}" +
                $"----------------------------------------{Environment.NewLine}";

            File.AppendAllText(logPath, content);
        }
        catch
        {
        }
    }

    private void LoadCategories()
    {
        _categories = _categoryService.GetAll().ToList();

        var selectedFilterId = (_cmbCategoryFilter.SelectedItem as ComboItem)?.Id;
        var filterItems = new List<ComboItem> { new() { Text = "Tüm Kategoriler", Id = null } };
        filterItems.AddRange(_categories.Select(c => new ComboItem { Id = c.KategoriId, Text = c.KategoriAdi }));

        _cmbCategoryFilter.DataSource = null;
        _cmbCategoryFilter.DataSource = filterItems;
        var filterIndex = filterItems.FindIndex(x => x.Id == selectedFilterId);
        SetComboSelectedIndexSafe(_cmbCategoryFilter, filterIndex);

        if (_currentUser.IsAdmin)
        {
            var selectedAdminCategoryId = (_cmbAdminProductCategory.SelectedItem as ComboItem)?.Id;
            var adminCategoryItems = _categories.Select(c => new ComboItem { Id = c.KategoriId, Text = c.KategoriAdi }).ToList();

            _cmbAdminProductCategory.DataSource = null;
            _cmbAdminProductCategory.DataSource = adminCategoryItems;
            var adminIndex = adminCategoryItems.FindIndex(x => x.Id == selectedAdminCategoryId);
            SetComboSelectedIndexSafe(_cmbAdminProductCategory, adminIndex);
        }
    }

    private static void SetComboSelectedIndexSafe(ComboBox comboBox, int preferredIndex)
    {
        if (comboBox.Items.Count == 0)
        {
            return;
        }

        var resolvedIndex = preferredIndex;
        if (resolvedIndex < 0 || resolvedIndex >= comboBox.Items.Count)
        {
            resolvedIndex = 0;
        }

        comboBox.SelectedIndex = resolvedIndex;
    }

    private void LoadShopProducts()
    {
        var categoryId = (_cmbCategoryFilter.SelectedItem as ComboItem)?.Id;
        var sortBy = (_cmbSort.SelectedItem as ComboItem)?.Key;

        var products = _productService.GetProducts(_txtProductSearch.Text, categoryId, sortBy).ToList();
        _dgvProducts.DataSource = products;

        ConfigureProductColumns(_dgvProducts);
    }

    private void LoadAdminProducts()
    {
        var products = _productService.GetProducts(_txtAdminProductSearch.Text, null, "AdArtan").ToList();
        _dgvAdminProducts.DataSource = products;

        ConfigureProductColumns(_dgvAdminProducts);
    }

    private void LoadCategoryGrid()
    {
        var categories = _categoryService.GetAll().ToList();
        _dgvCategories.DataSource = categories;

        if (_dgvCategories.Columns["KategoriId"] is not null)
        {
            _dgvCategories.Columns["KategoriId"].HeaderText = "Kategori ID";
            _dgvCategories.Columns["KategoriId"].FillWeight = 25;
        }

        if (_dgvCategories.Columns["KategoriAdi"] is not null)
        {
            _dgvCategories.Columns["KategoriAdi"].HeaderText = "Kategori Adı";
            _dgvCategories.Columns["KategoriAdi"].FillWeight = 75;
        }
    }

    private void LoadUsers()
    {
        var users = _userService.GetAllUsers().ToList();
        _dgvUsers.DataSource = users;

        if (_dgvUsers.Columns["KullaniciId"] is not null)
        {
            _dgvUsers.Columns["KullaniciId"].HeaderText = "ID";
            _dgvUsers.Columns["KullaniciId"].FillWeight = 22;
        }

        if (_dgvUsers.Columns["Isim"] is not null)
        {
            _dgvUsers.Columns["Isim"].HeaderText = "Ad Soyad";
        }

        if (_dgvUsers.Columns["Email"] is not null)
        {
            _dgvUsers.Columns["Email"].HeaderText = "E-posta";
        }

        if (_dgvUsers.Columns["Rol"] is not null)
        {
            _dgvUsers.Columns["Rol"].HeaderText = "Rol";
            _dgvUsers.Columns["Rol"].FillWeight = 24;
        }

        if (_dgvUsers.Columns["KayitTarihi"] is not null)
        {
            _dgvUsers.Columns["KayitTarihi"].HeaderText = "Kayıt Tarihi";
            _dgvUsers.Columns["KayitTarihi"].DefaultCellStyle.Format = "g";
            _dgvUsers.Columns["KayitTarihi"].FillWeight = 38;
        }

        if (_dgvUsers.Columns["IsAdmin"] is not null)
        {
            _dgvUsers.Columns["IsAdmin"].Visible = false;
        }
    }

    private void LoadMyOrders()
    {
        var orders = _orderService.GetOrders(_currentUser.KullaniciId).ToList();
        _dgvMyOrders.DataSource = orders;
        ConfigureOrderColumns(_dgvMyOrders, false);
    }

    private void LoadAllOrders()
    {
        var orders = _orderService.GetOrders().ToList();
        _dgvOrders.DataSource = orders;
        ConfigureOrderColumns(_dgvOrders, true);
    }

    private void RefreshCartGrid()
    {
        _dgvCart.DataSource = null;
        _dgvCart.DataSource = _cartItems.ToList();

        if (_dgvCart.Columns["UrunId"] is not null)
        {
            _dgvCart.Columns["UrunId"].HeaderText = "Ürün ID";
            _dgvCart.Columns["UrunId"].FillWeight = 22;
        }

        if (_dgvCart.Columns["UrunAdi"] is not null)
        {
            _dgvCart.Columns["UrunAdi"].HeaderText = "Ürün";
            _dgvCart.Columns["UrunAdi"].FillWeight = 42;
        }

        if (_dgvCart.Columns["BirimFiyat"] is not null)
        {
            _dgvCart.Columns["BirimFiyat"].HeaderText = "Birim Fiyat";
            _dgvCart.Columns["BirimFiyat"].DefaultCellStyle.Format = "N2";
        }

        if (_dgvCart.Columns["Adet"] is not null)
        {
            _dgvCart.Columns["Adet"].HeaderText = "Adet";
            _dgvCart.Columns["Adet"].FillWeight = 16;
        }

        if (_dgvCart.Columns["AraToplam"] is not null)
        {
            _dgvCart.Columns["AraToplam"].HeaderText = "Ara Toplam";
            _dgvCart.Columns["AraToplam"].DefaultCellStyle.Format = "N2";
        }

        if (_dgvCart.Columns["MevcutStok"] is not null)
        {
            _dgvCart.Columns["MevcutStok"].Visible = false;
        }

        var total = _cartItems.Sum(x => x.AraToplam);
        _lblCartTotal.Text = $"Toplam: {total:N2} TL";
    }

    private static void ConfigureGrid(DataGridView grid, bool readOnly)
    {
        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = readOnly;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.MultiSelect = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoGenerateColumns = true;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.RowHeadersVisible = false;
        grid.BorderStyle = BorderStyle.FixedSingle;
        grid.BackgroundColor = Color.White;
    }

    private static void StyleButton(Button button, bool secondary = false)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Cursor = Cursors.Hand;

        if (secondary)
        {
            button.BackColor = Color.FromArgb(220, 227, 238);
            button.ForeColor = Color.FromArgb(28, 39, 59);
        }
        else
        {
            button.BackColor = Color.FromArgb(38, 70, 122);
            button.ForeColor = Color.White;
        }
    }

    private static void ConfigureProductColumns(DataGridView grid)
    {
        if (grid.Columns["UrunId"] is not null)
        {
            grid.Columns["UrunId"].HeaderText = "Ürün ID";
            grid.Columns["UrunId"].FillWeight = 22;
        }

        if (grid.Columns["UrunAdi"] is not null)
        {
            grid.Columns["UrunAdi"].HeaderText = "Ürün Adı";
            grid.Columns["UrunAdi"].FillWeight = 38;
        }

        if (grid.Columns["KategoriAdi"] is not null)
        {
            grid.Columns["KategoriAdi"].HeaderText = "Kategori";
            grid.Columns["KategoriAdi"].FillWeight = 26;
        }

        if (grid.Columns["Fiyat"] is not null)
        {
            grid.Columns["Fiyat"].HeaderText = "Fiyat";
            grid.Columns["Fiyat"].DefaultCellStyle.Format = "N2";
            grid.Columns["Fiyat"].FillWeight = 20;
        }

        if (grid.Columns["Stok"] is not null)
        {
            grid.Columns["Stok"].HeaderText = "Stok";
            grid.Columns["Stok"].FillWeight = 16;
        }

        if (grid.Columns["EklenmeTarihi"] is not null)
        {
            grid.Columns["EklenmeTarihi"].HeaderText = "Eklenme";
            grid.Columns["EklenmeTarihi"].DefaultCellStyle.Format = "g";
            grid.Columns["EklenmeTarihi"].FillWeight = 24;
        }

        if (grid.Columns["KategoriId"] is not null)
        {
            grid.Columns["KategoriId"].Visible = false;
        }
    }

    private static void ConfigureOrderColumns(DataGridView grid, bool showUser)
    {
        if (grid.Columns["SiparisId"] is not null)
        {
            grid.Columns["SiparisId"].HeaderText = "Sipariş ID";
            grid.Columns["SiparisId"].FillWeight = 18;
        }

        if (grid.Columns["KullaniciId"] is not null)
        {
            grid.Columns["KullaniciId"].Visible = false;
        }

        if (grid.Columns["KullaniciAdi"] is not null)
        {
            grid.Columns["KullaniciAdi"].HeaderText = "Kullanıcı";
            grid.Columns["KullaniciAdi"].Visible = showUser;
            grid.Columns["KullaniciAdi"].FillWeight = 24;
        }

        if (grid.Columns["Tarih"] is not null)
        {
            grid.Columns["Tarih"].HeaderText = "Tarih";
            grid.Columns["Tarih"].DefaultCellStyle.Format = "g";
            grid.Columns["Tarih"].FillWeight = 26;
        }

        if (grid.Columns["ToplamTutar"] is not null)
        {
            grid.Columns["ToplamTutar"].HeaderText = "Toplam";
            grid.Columns["ToplamTutar"].DefaultCellStyle.Format = "N2";
            grid.Columns["ToplamTutar"].FillWeight = 22;
        }

        if (grid.Columns["Durum"] is not null)
        {
            grid.Columns["Durum"].HeaderText = "Durum";
            grid.Columns["Durum"].FillWeight = 20;
        }
    }

    private static decimal ClampDecimal(decimal value, decimal min, decimal max)
    {
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }
}


