using MiniE_TicaretUygulaması.Models;

namespace MiniE_TicaretUygulaması.UI;

public sealed partial class MainForm
{
    private void BtnAddToCart_Click(object sender, EventArgs e)
    {
        try
        {
            if (_dgvProducts.CurrentRow?.DataBoundItem is not Product selected)
            {
                MessageBox.Show("Sepete eklemek için ürün seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var qty = Convert.ToInt32(_nudAddQty.Value);
            if (qty <= 0)
            {
                MessageBox.Show("Adet en az 1 olmalı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selected.Stok < qty)
            {
                MessageBox.Show("Seçilen adet, stok miktarından fazla.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existing = _cartItems.FirstOrDefault(x => x.UrunId == selected.UrunId);
            if (existing is null)
            {
                _cartItems.Add(new CartItem
                {
                    UrunId = selected.UrunId,
                    UrunAdi = selected.UrunAdi,
                    BirimFiyat = selected.Fiyat,
                    Adet = qty,
                    MevcutStok = selected.Stok
                });
            }
            else
            {
                if (existing.Adet + qty > selected.Stok)
                {
                    MessageBox.Show("Bu üründen sepete daha fazla eklenemez, stok aşılıyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                existing.Adet += qty;
            }

            RefreshCartGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Sepete ekleme hatası:\n\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnUpdateCartQty_Click(object sender, EventArgs e)
    {
        if (_dgvCart.CurrentRow?.DataBoundItem is not CartItem selectedRow)
        {
            MessageBox.Show("Güncellemek için sepette ürün seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var newQty = Convert.ToInt32(_nudUpdateCartQty.Value);
        var currentItem = _cartItems.FirstOrDefault(x => x.UrunId == selectedRow.UrunId);

        if (currentItem is null)
        {
            return;
        }

        if (newQty > currentItem.MevcutStok)
        {
            MessageBox.Show("Yeni adet stoktan fazla olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        currentItem.Adet = newQty;
        RefreshCartGrid();
    }

    private void BtnRemoveFromCart_Click(object sender, EventArgs e)
    {
        if (_dgvCart.CurrentRow?.DataBoundItem is not CartItem selectedRow)
        {
            MessageBox.Show("Çıkarmak için sepette ürün seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var currentItem = _cartItems.FirstOrDefault(x => x.UrunId == selectedRow.UrunId);
        if (currentItem is not null)
        {
            _cartItems.Remove(currentItem);
            RefreshCartGrid();
        }
    }

    private void BtnCreateOrder_Click(object sender, EventArgs e)
    {
        if (_cartItems.Count == 0)
        {
            MessageBox.Show("Sepet boş. Önce ürün ekleyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var orderId = _orderService.CreateOrder(_currentUser.KullaniciId, _cartItems);
            _cartItems.Clear();
            RefreshCartGrid();
            LoadShopProducts();
            LoadMyOrders();

            if (_currentUser.IsAdmin)
            {
                LoadAdminProducts();
                LoadAllOrders();
            }

            MessageBox.Show($"Sipariş başarıyla oluşturuldu. Sipariş No: {orderId}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Sipariş oluşturulamadı:\n\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdminAddProduct_Click(object sender, EventArgs e)
    {
        try
        {
            var product = BuildProductFromForm(requireSelection: false);
            _productService.Add(product);

            LoadAdminProducts();
            LoadShopProducts();
            ClearAdminProductForm();
            MessageBox.Show("Ürün eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ürün eklenemedi:\n\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdminUpdateProduct_Click(object sender, EventArgs e)
    {
        try
        {
            var product = BuildProductFromForm(requireSelection: true);
            _productService.Update(product);

            LoadAdminProducts();
            LoadShopProducts();
            MessageBox.Show("Ürün güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ürün güncellenemedi:\n\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdminDeleteProduct_Click(object sender, EventArgs e)
    {
        if (_dgvAdminProducts.CurrentRow?.DataBoundItem is not Product selected)
        {
            MessageBox.Show("Silmek için ürün seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            $"'{selected.UrunAdi}' ürününü silmek istediğinize emin misiniz?",
            "Onay",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        try
        {
            _productService.Delete(selected.UrunId);
            LoadAdminProducts();
            LoadShopProducts();
            ClearAdminProductForm();
            MessageBox.Show("Ürün silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ürün silinemedi:\n\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCategoryAdd_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtCategoryName.Text))
        {
            MessageBox.Show("Kategori adı boş olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _categoryService.Add(_txtCategoryName.Text);
            _txtCategoryName.Clear();
            LoadCategoryGrid();
            LoadCategories();
            LoadShopProducts();
            MessageBox.Show("Kategori eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kategori eklenemedi:\n\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCategoryUpdate_Click(object sender, EventArgs e)
    {
        if (_dgvCategories.CurrentRow?.DataBoundItem is not Category selected)
        {
            MessageBox.Show("Güncellemek için kategori seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_txtCategoryName.Text))
        {
            MessageBox.Show("Kategori adı boş olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _categoryService.Update(selected.KategoriId, _txtCategoryName.Text);
            LoadCategoryGrid();
            LoadCategories();
            LoadShopProducts();
            LoadAdminProducts();
            MessageBox.Show("Kategori güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kategori güncellenemedi:\n\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCategoryDelete_Click(object sender, EventArgs e)
    {
        if (_dgvCategories.CurrentRow?.DataBoundItem is not Category selected)
        {
            MessageBox.Show("Silmek için kategori seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            $"'{selected.KategoriAdi}' kategorisini silmek istediğinize emin misiniz?",
            "Onay",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        try
        {
            _categoryService.Delete(selected.KategoriId);
            _txtCategoryName.Clear();
            LoadCategoryGrid();
            LoadCategories();
            LoadShopProducts();
            LoadAdminProducts();
            MessageBox.Show("Kategori silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kategori silinemedi:\n\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnDeleteUser_Click(object sender, EventArgs e)
    {
        if (_dgvUsers.CurrentRow?.DataBoundItem is not AppUser selected)
        {
            MessageBox.Show("Silmek için kullanıcı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            $"'{selected.Isim}' kullanıcısını silmek istediğinize emin misiniz?",
            "Onay",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        try
        {
            _userService.DeleteUser(selected.KullaniciId, _currentUser.KullaniciId);
            LoadUsers();
            LoadAllOrders();
            MessageBox.Show("Kullanıcı silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kullanıcı silinemedi:\n\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnUpdateOrderStatus_Click(object sender, EventArgs e)
    {
        if (_dgvOrders.CurrentRow?.DataBoundItem is not OrderSummary selected)
        {
            MessageBox.Show("Durum güncellemek için sipariş seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbOrderStatus.SelectedItem is not string status)
        {
            MessageBox.Show("Lütfen geçerli bir durum seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _orderService.UpdateStatus(selected.SiparisId, status);
            LoadAllOrders();
            LoadMyOrders();
            MessageBox.Show("Sipariş durumu güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Sipariş durumu güncellenemedi:\n\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void PopulateAdminProductFormFromSelection()
    {
        if (_dgvAdminProducts.CurrentRow?.DataBoundItem is not Product selected)
        {
            return;
        }

        _txtAdminProductName.Text = selected.UrunAdi;
        _nudAdminPrice.Value = ClampDecimal(selected.Fiyat, _nudAdminPrice.Minimum, _nudAdminPrice.Maximum);
        _nudAdminStock.Value = ClampDecimal(selected.Stok, _nudAdminStock.Minimum, _nudAdminStock.Maximum);

        if (_cmbAdminProductCategory.DataSource is List<ComboItem> items)
        {
            var index = items.FindIndex(x => x.Id == selected.KategoriId);
            if (index >= 0)
            {
                _cmbAdminProductCategory.SelectedIndex = index;
            }
        }
    }

    private Product BuildProductFromForm(bool requireSelection)
    {
        if (string.IsNullOrWhiteSpace(_txtAdminProductName.Text))
        {
            throw new InvalidOperationException("Ürün adı boş olamaz.");
        }

        if (_cmbAdminProductCategory.SelectedItem is not ComboItem categoryItem || !categoryItem.Id.HasValue)
        {
            throw new InvalidOperationException("Lütfen kategori seçin.");
        }

        var product = new Product
        {
            UrunAdi = _txtAdminProductName.Text.Trim(),
            KategoriId = categoryItem.Id.Value,
            Fiyat = _nudAdminPrice.Value,
            Stok = Decimal.ToInt32(_nudAdminStock.Value)
        };

        if (requireSelection)
        {
            if (_dgvAdminProducts.CurrentRow?.DataBoundItem is not Product selected)
            {
                throw new InvalidOperationException("Güncellemek için listeden ürün seçin.");
            }

            product.UrunId = selected.UrunId;
        }

        return product;
    }

    private void ClearAdminProductForm()
    {
        _txtAdminProductName.Clear();
        _nudAdminPrice.Value = 0;
        _nudAdminStock.Value = 0;
        if (_cmbAdminProductCategory.Items.Count > 0)
        {
            _cmbAdminProductCategory.SelectedIndex = 0;
        }
    }
}

