using System.Drawing;
using System.ComponentModel;
using MiniE_TicaretUygulaması.Models;
using MiniE_TicaretUygulaması.Services;

namespace MiniE_TicaretUygulaması.UI;

public partial class MainForm : Form
{
    private sealed class ComboItem
    {
        public int? Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;

        public override string ToString() => Text;
    }

    private readonly AppUser _currentUser;
    private readonly ProductService _productService = new ProductService();
    private readonly CategoryService _categoryService = new CategoryService();
    private readonly OrderService _orderService = new OrderService();
    private readonly UserService _userService = new UserService();

    private readonly List<CartItem> _cartItems = new List<CartItem>();
    private List<Category> _categories = new List<Category>();

    private readonly ComboBox _cmbCategoryFilter = new ComboBox();
    private readonly ComboBox _cmbSort = new ComboBox();
    private readonly TextBox _txtProductSearch = new TextBox();
    private readonly NumericUpDown _nudAddQty = new NumericUpDown();
    private readonly NumericUpDown _nudUpdateCartQty = new NumericUpDown();
    private readonly DataGridView _dgvProducts = new DataGridView();
    private readonly DataGridView _dgvCart = new DataGridView();
    private readonly DataGridView _dgvMyOrders = new DataGridView();
    private readonly Label _lblCartTotal = new Label();

    private readonly TabPage _tabAdmin = new TabPage("Yönetim");
    private readonly TextBox _txtAdminProductName = new TextBox();
    private readonly TextBox _txtAdminProductSearch = new TextBox();
    private readonly ComboBox _cmbAdminProductCategory = new ComboBox();
    private readonly NumericUpDown _nudAdminPrice = new NumericUpDown();
    private readonly NumericUpDown _nudAdminStock = new NumericUpDown();
    private readonly DataGridView _dgvAdminProducts = new DataGridView();

    private readonly TextBox _txtCategoryName = new TextBox();
    private readonly DataGridView _dgvCategories = new DataGridView();

    private readonly DataGridView _dgvUsers = new DataGridView();

    private readonly DataGridView _dgvOrders = new DataGridView();
    private readonly ComboBox _cmbOrderStatus = new ComboBox();

    public MainForm() : this(new AppUser { Isim = "Tasarım Kullanıcı", Rol = "admin" })
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
        Text = "Mini E-Ticaret Uygulaması";
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1200, 760);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(245, 247, 250);
        Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

        var headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 62,
            BackColor = Color.FromArgb(24, 43, 73)
        };

        var lblTitle = new Label
        {
            Text = "Mini E-Ticaret Masaüstü",
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point),
            Left = 18,
            Top = 17,
            AutoSize = true
        };

        var roleLabel = _currentUser.IsAdmin ? "Yönetici" : "Kullanıcı";
        var lblUser = new Label
        {
            Text = $"Aktif Kullanıcı: {_currentUser.Isim} ({roleLabel})",
            ForeColor = Color.FromArgb(224, 232, 247),
            AutoSize = true,
            Top = 22,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };

        var btnLogout = new Button
        {
            Text = "Çıkış",
            Width = 92,
            Height = 34,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        StyleButton(btnLogout, true);
        btnLogout.Top = 14;
        btnLogout.Click += (_, _) => Close();

        headerPanel.Controls.Add(lblTitle);
        headerPanel.Controls.Add(lblUser);
        headerPanel.Controls.Add(btnLogout);

        headerPanel.Resize += (_, _) =>
        {
            btnLogout.Left = headerPanel.Width - btnLogout.Width - 20;
            lblUser.Left = btnLogout.Left - lblUser.Width - 16;
        };

        var mainTabs = new TabControl
        {
            Dock = DockStyle.Fill,
            Padding = new Point(22, 8)
        };

        mainTabs.TabPages.Add(CreateShopTab());
        mainTabs.TabPages.Add(CreateAdminTab());

        if (!_currentUser.IsAdmin)
        {
            mainTabs.TabPages.Remove(_tabAdmin);
        }

        Controls.Add(mainTabs);
        Controls.Add(headerPanel);
    }

    private TabPage CreateShopTab()
    {
        var tab = new TabPage("Kullanıcı Paneli") { BackColor = Color.FromArgb(245, 247, 250) };

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            SplitterDistance = 760,
            BackColor = Color.FromArgb(245, 247, 250)
        };

        split.Panel1.Controls.Add(CreateProductsPanel());
        split.Panel2.Controls.Add(CreateCartOrdersPanel());

        tab.Controls.Add(split);
        return tab;
    }

    private Control CreateProductsPanel()
    {
        var group = new GroupBox
        {
            Text = "Ürün Listesi",
            Dock = DockStyle.Fill,
            Padding = new Padding(12)
        };

        var filterPanel = new Panel { Dock = DockStyle.Top, Height = 82 };

        var lblSearch = new Label { Text = "Arama", Left = 6, Top = 12, AutoSize = true };
        _txtProductSearch.Left = 6;
        _txtProductSearch.Top = 30;
        _txtProductSearch.Width = 220;

        var lblCategory = new Label { Text = "Kategori", Left = 240, Top = 12, AutoSize = true };
        _cmbCategoryFilter.Left = 240;
        _cmbCategoryFilter.Top = 30;
        _cmbCategoryFilter.Width = 180;
        _cmbCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;

        var lblSort = new Label { Text = "Sıralama", Left = 434, Top = 12, AutoSize = true };
        _cmbSort.Left = 434;
        _cmbSort.Top = 30;
        _cmbSort.Width = 170;
        _cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbSort.Items.AddRange(
        [
            new ComboItem { Key = "AdArtan", Text = "Ada Göre (A-Z)" },
            new ComboItem { Key = "AdAzalan", Text = "Ada Göre (Z-A)" },
            new ComboItem { Key = "FiyatArtan", Text = "Fiyata Göre (Artan)" },
            new ComboItem { Key = "FiyatAzalan", Text = "Fiyata Göre (Azalan)" }
        ]);
        _cmbSort.SelectedIndex = 0;

        var btnFilter = new Button
        {
            Text = "Uygula",
            Left = 620,
            Top = 28,
            Width = 90,
            Height = 32
        };
        StyleButton(btnFilter);
        btnFilter.Click += (_, _) => LoadShopProducts();

        var btnClear = new Button
        {
            Text = "Temizle",
            Left = 718,
            Top = 28,
            Width = 90,
            Height = 32
        };
        StyleButton(btnClear, true);
        btnClear.Click += (_, _) =>
        {
            _txtProductSearch.Clear();
            if (_cmbCategoryFilter.Items.Count > 0)
            {
                _cmbCategoryFilter.SelectedIndex = 0;
            }

            _cmbSort.SelectedIndex = 0;
            LoadShopProducts();
        };

        filterPanel.Controls.Add(lblSearch);
        filterPanel.Controls.Add(_txtProductSearch);
        filterPanel.Controls.Add(lblCategory);
        filterPanel.Controls.Add(_cmbCategoryFilter);
        filterPanel.Controls.Add(lblSort);
        filterPanel.Controls.Add(_cmbSort);
        filterPanel.Controls.Add(btnFilter);
        filterPanel.Controls.Add(btnClear);

        ConfigureGrid(_dgvProducts, true);

        var addPanel = new Panel { Dock = DockStyle.Bottom, Height = 58 };
        var lblQty = new Label { Text = "Adet", Left = 8, Top = 21, AutoSize = true };

        _nudAddQty.Left = 48;
        _nudAddQty.Top = 18;
        _nudAddQty.Width = 70;
        _nudAddQty.Minimum = 1;
        _nudAddQty.Maximum = 999;
        _nudAddQty.Value = 1;

        var btnAddToCart = new Button
        {
            Text = "Sepete Ekle",
            Left = 136,
            Top = 14,
            Width = 130,
            Height = 32
        };
        StyleButton(btnAddToCart);
        btnAddToCart.Click += BtnAddToCart_Click;

        var btnRefresh = new Button
        {
            Text = "Ürünleri Yenile",
            Left = 278,
            Top = 14,
            Width = 130,
            Height = 32
        };
        StyleButton(btnRefresh, true);
        btnRefresh.Click += (_, _) => LoadShopProducts();

        addPanel.Controls.Add(lblQty);
        addPanel.Controls.Add(_nudAddQty);
        addPanel.Controls.Add(btnAddToCart);
        addPanel.Controls.Add(btnRefresh);

        group.Controls.Add(_dgvProducts);
        group.Controls.Add(addPanel);
        group.Controls.Add(filterPanel);

        return group;
    }

    private Control CreateCartOrdersPanel()
    {
        var panel = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterDistance = 340
        };

        panel.Panel1.Controls.Add(CreateCartPanel());
        panel.Panel2.Controls.Add(CreateMyOrdersPanel());

        return panel;
    }

    private Control CreateCartPanel()
    {
        var group = new GroupBox
        {
            Text = "Sepet",
            Dock = DockStyle.Fill,
            Padding = new Padding(12)
        };

        ConfigureGrid(_dgvCart, true);

        var controlsPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 128,
            ColumnCount = 2,
            RowCount = 3,
            Padding = new Padding(0, 2, 0, 0)
        };
        controlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56F));
        controlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
        controlsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        controlsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        controlsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));

        var lblUpdateQty = new Label
        {
            Text = "Yeni Adet",
            AutoSize = true,
            Margin = new Padding(0, 8, 8, 0)
        };

        _nudUpdateCartQty.Width = 70;
        _nudUpdateCartQty.Minimum = 1;
        _nudUpdateCartQty.Maximum = 999;
        _nudUpdateCartQty.Value = 1;
        _nudUpdateCartQty.Margin = new Padding(0, 4, 8, 0);

        var btnUpdateQty = new Button
        {
            Text = "Adet Güncelle",
            Width = 120,
            Height = 32
        };
        StyleButton(btnUpdateQty, true);
        btnUpdateQty.Click += BtnUpdateCartQty_Click;
        btnUpdateQty.Margin = new Padding(0, 2, 0, 0);

        var btnRemove = new Button
        {
            Text = "Sepetten Çıkar"
        };
        StyleButton(btnRemove, true);
        btnRemove.Click += BtnRemoveFromCart_Click;
        btnRemove.Dock = DockStyle.Fill;
        btnRemove.Margin = Padding.Empty;

        var btnCreateOrder = new Button
        {
            Text = "Sipariş Oluştur",
            Width = 160,
            Height = 34
        };
        StyleButton(btnCreateOrder);
        btnCreateOrder.Click += BtnCreateOrder_Click;
        btnCreateOrder.Margin = new Padding(0, 2, 0, 0);

        _lblCartTotal.Text = "Toplam: 0 TL";
        _lblCartTotal.AutoSize = false;
        _lblCartTotal.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        _lblCartTotal.TextAlign = ContentAlignment.MiddleRight;
        _lblCartTotal.Dock = DockStyle.Fill;
        _lblCartTotal.Padding = new Padding(0, 0, 8, 0);

        var topLeft = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            AutoScroll = false,
            Padding = new Padding(6, 6, 0, 0)
        };
        topLeft.Controls.Add(lblUpdateQty);
        topLeft.Controls.Add(_nudUpdateCartQty);
        topLeft.Controls.Add(btnUpdateQty);

        var removeRow = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(6, 2, 6, 2)
        };
        removeRow.Controls.Add(btnRemove);

        var bottomLeft = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            AutoScroll = false,
            Padding = new Padding(6, 4, 0, 0)
        };
        bottomLeft.Controls.Add(btnCreateOrder);

        controlsPanel.Controls.Add(topLeft, 0, 0);
        controlsPanel.SetColumnSpan(topLeft, 2);
        controlsPanel.Controls.Add(removeRow, 0, 1);
        controlsPanel.SetColumnSpan(removeRow, 2);
        controlsPanel.Controls.Add(bottomLeft, 0, 2);
        controlsPanel.Controls.Add(_lblCartTotal, 1, 2);

        group.Controls.Add(_dgvCart);
        group.Controls.Add(controlsPanel);

        return group;
    }

    private Control CreateMyOrdersPanel()
    {
        var group = new GroupBox
        {
            Text = "Siparişlerim",
            Dock = DockStyle.Fill,
            Padding = new Padding(12)
        };

        ConfigureGrid(_dgvMyOrders, true);

        var topPanel = new Panel { Dock = DockStyle.Top, Height = 46 };
        var btnRefreshOrders = new Button
        {
            Text = "Siparişleri Yenile",
            Width = 140,
            Height = 32,
            Left = 8,
            Top = 8
        };
        StyleButton(btnRefreshOrders, true);
        btnRefreshOrders.Click += (_, _) => LoadMyOrders();

        topPanel.Controls.Add(btnRefreshOrders);

        group.Controls.Add(_dgvMyOrders);
        group.Controls.Add(topPanel);

        return group;
    }

    private TabPage CreateAdminTab()
    {
        _tabAdmin.BackColor = Color.FromArgb(245, 247, 250);

        var adminTabs = new TabControl { Dock = DockStyle.Fill, Padding = new Point(18, 6) };
        adminTabs.TabPages.Add(CreateAdminProductsTab());
        adminTabs.TabPages.Add(CreateAdminCategoriesTab());
        adminTabs.TabPages.Add(CreateAdminUsersTab());
        adminTabs.TabPages.Add(CreateAdminOrdersTab());

        _tabAdmin.Controls.Add(adminTabs);
        return _tabAdmin;
    }

    private TabPage CreateAdminProductsTab()
    {
        var tab = new TabPage("Ürün Yönetimi") { BackColor = Color.FromArgb(245, 247, 250) };

        var topPanel = new Panel { Dock = DockStyle.Top, Height = 135 };

        var lblName = new Label { Text = "Ürün Adı", Left = 8, Top = 12, AutoSize = true };
        _txtAdminProductName.Left = 8;
        _txtAdminProductName.Top = 30;
        _txtAdminProductName.Width = 240;

        var lblCategory = new Label { Text = "Kategori", Left = 258, Top = 12, AutoSize = true };
        _cmbAdminProductCategory.Left = 258;
        _cmbAdminProductCategory.Top = 30;
        _cmbAdminProductCategory.Width = 170;
        _cmbAdminProductCategory.DropDownStyle = ComboBoxStyle.DropDownList;

        var lblPrice = new Label { Text = "Fiyat", Left = 438, Top = 12, AutoSize = true };
        _nudAdminPrice.Left = 438;
        _nudAdminPrice.Top = 30;
        _nudAdminPrice.Width = 120;
        _nudAdminPrice.DecimalPlaces = 2;
        _nudAdminPrice.Maximum = 1_000_000;

        var lblStock = new Label { Text = "Stok", Left = 568, Top = 12, AutoSize = true };
        _nudAdminStock.Left = 568;
        _nudAdminStock.Top = 30;
        _nudAdminStock.Width = 100;
        _nudAdminStock.Maximum = 100_000;

        var btnAdd = new Button
        {
            Text = "Ekle",
            Left = 684,
            Top = 26,
            Width = 74,
            Height = 32
        };
        StyleButton(btnAdd);
        btnAdd.Click += BtnAdminAddProduct_Click;

        var btnUpdate = new Button
        {
            Text = "Güncelle",
            Left = 764,
            Top = 26,
            Width = 84,
            Height = 32
        };
        StyleButton(btnUpdate, true);
        btnUpdate.Click += BtnAdminUpdateProduct_Click;

        var btnDelete = new Button
        {
            Text = "Sil",
            Left = 854,
            Top = 26,
            Width = 74,
            Height = 32
        };
        StyleButton(btnDelete, true);
        btnDelete.Click += BtnAdminDeleteProduct_Click;

        var lblSearch = new Label { Text = "Arama", Left = 8, Top = 74, AutoSize = true };
        _txtAdminProductSearch.Left = 8;
        _txtAdminProductSearch.Top = 92;
        _txtAdminProductSearch.Width = 240;

        var btnSearch = new Button
        {
            Text = "Ara",
            Left = 258,
            Top = 89,
            Width = 70,
            Height = 30
        };
        StyleButton(btnSearch, true);
        btnSearch.Click += (_, _) => LoadAdminProducts();

        var btnClear = new Button
        {
            Text = "Formu Temizle",
            Left = 334,
            Top = 89,
            Width = 124,
            Height = 30
        };
        StyleButton(btnClear, true);
        btnClear.Click += (_, _) => ClearAdminProductForm();

        var btnRefresh = new Button
        {
            Text = "Listeyi Yenile",
            Left = 464,
            Top = 89,
            Width = 124,
            Height = 30
        };
        StyleButton(btnRefresh, true);
        btnRefresh.Click += (_, _) => LoadAdminProducts();

        topPanel.Controls.Add(lblName);
        topPanel.Controls.Add(_txtAdminProductName);
        topPanel.Controls.Add(lblCategory);
        topPanel.Controls.Add(_cmbAdminProductCategory);
        topPanel.Controls.Add(lblPrice);
        topPanel.Controls.Add(_nudAdminPrice);
        topPanel.Controls.Add(lblStock);
        topPanel.Controls.Add(_nudAdminStock);
        topPanel.Controls.Add(btnAdd);
        topPanel.Controls.Add(btnUpdate);
        topPanel.Controls.Add(btnDelete);
        topPanel.Controls.Add(lblSearch);
        topPanel.Controls.Add(_txtAdminProductSearch);
        topPanel.Controls.Add(btnSearch);
        topPanel.Controls.Add(btnClear);
        topPanel.Controls.Add(btnRefresh);

        ConfigureGrid(_dgvAdminProducts, true);
        _dgvAdminProducts.SelectionChanged += (_, _) => PopulateAdminProductFormFromSelection();

        tab.Controls.Add(_dgvAdminProducts);
        tab.Controls.Add(topPanel);

        return tab;
    }

    private TabPage CreateAdminCategoriesTab()
    {
        var tab = new TabPage("Kategori Yönetimi") { BackColor = Color.FromArgb(245, 247, 250) };

        var topPanel = new Panel { Dock = DockStyle.Top, Height = 68 };

        var lblCategoryName = new Label { Text = "Kategori Adı", Left = 8, Top = 12, AutoSize = true };
        _txtCategoryName.Left = 8;
        _txtCategoryName.Top = 30;
        _txtCategoryName.Width = 220;

        var btnAdd = new Button { Text = "Ekle", Left = 238, Top = 27, Width = 72, Height = 30 };
        StyleButton(btnAdd);
        btnAdd.Click += BtnCategoryAdd_Click;

        var btnUpdate = new Button { Text = "Güncelle", Left = 316, Top = 27, Width = 88, Height = 30 };
        StyleButton(btnUpdate, true);
        btnUpdate.Click += BtnCategoryUpdate_Click;

        var btnDelete = new Button { Text = "Sil", Left = 410, Top = 27, Width = 72, Height = 30 };
        StyleButton(btnDelete, true);
        btnDelete.Click += BtnCategoryDelete_Click;

        var btnRefresh = new Button { Text = "Yenile", Left = 488, Top = 27, Width = 82, Height = 30 };
        StyleButton(btnRefresh, true);
        btnRefresh.Click += (_, _) => LoadCategoryGrid();

        topPanel.Controls.Add(lblCategoryName);
        topPanel.Controls.Add(_txtCategoryName);
        topPanel.Controls.Add(btnAdd);
        topPanel.Controls.Add(btnUpdate);
        topPanel.Controls.Add(btnDelete);
        topPanel.Controls.Add(btnRefresh);

        ConfigureGrid(_dgvCategories, true);
        _dgvCategories.SelectionChanged += (_, _) =>
        {
            if (_dgvCategories.CurrentRow?.DataBoundItem is Category selected)
            {
                _txtCategoryName.Text = selected.KategoriAdi;
            }
        };

        tab.Controls.Add(_dgvCategories);
        tab.Controls.Add(topPanel);

        return tab;
    }

    private TabPage CreateAdminUsersTab()
    {
        var tab = new TabPage("Kullanıcı Yönetimi") { BackColor = Color.FromArgb(245, 247, 250) };

        var topPanel = new Panel { Dock = DockStyle.Top, Height = 54 };

        var btnRefresh = new Button { Text = "Listeyi Yenile", Left = 8, Top = 12, Width = 120, Height = 30 };
        StyleButton(btnRefresh, true);
        btnRefresh.Click += (_, _) => LoadUsers();

        var btnDelete = new Button { Text = "Kullanıcı Sil", Left = 136, Top = 12, Width = 120, Height = 30 };
        StyleButton(btnDelete, true);
        btnDelete.Click += BtnDeleteUser_Click;

        topPanel.Controls.Add(btnRefresh);
        topPanel.Controls.Add(btnDelete);

        ConfigureGrid(_dgvUsers, true);

        tab.Controls.Add(_dgvUsers);
        tab.Controls.Add(topPanel);

        return tab;
    }

    private TabPage CreateAdminOrdersTab()
    {
        var tab = new TabPage("Sipariş Yönetimi") { BackColor = Color.FromArgb(245, 247, 250) };

        var topPanel = new Panel { Dock = DockStyle.Top, Height = 54 };

        var lblStatus = new Label { Text = "Durum", Left = 8, Top = 17, AutoSize = true };

        _cmbOrderStatus.Left = 56;
        _cmbOrderStatus.Top = 13;
        _cmbOrderStatus.Width = 140;
        _cmbOrderStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbOrderStatus.Items.AddRange(OrderService.AllowedStatuses.Cast<object>().ToArray());
        _cmbOrderStatus.SelectedIndex = 0;

        var btnUpdate = new Button { Text = "Durum Güncelle", Left = 202, Top = 11, Width = 130, Height = 30 };
        StyleButton(btnUpdate);
        btnUpdate.Click += BtnUpdateOrderStatus_Click;

        var btnRefresh = new Button { Text = "Listeyi Yenile", Left = 338, Top = 11, Width = 118, Height = 30 };
        StyleButton(btnRefresh, true);
        btnRefresh.Click += (_, _) => LoadAllOrders();

        topPanel.Controls.Add(lblStatus);
        topPanel.Controls.Add(_cmbOrderStatus);
        topPanel.Controls.Add(btnUpdate);
        topPanel.Controls.Add(btnRefresh);

        ConfigureGrid(_dgvOrders, true);
        _dgvOrders.SelectionChanged += (_, _) =>
        {
            if (_dgvOrders.CurrentRow?.DataBoundItem is OrderSummary selected)
            {
                _cmbOrderStatus.SelectedItem = selected.Durum;
            }
        };

        tab.Controls.Add(_dgvOrders);
        tab.Controls.Add(topPanel);

        return tab;
    }
}



