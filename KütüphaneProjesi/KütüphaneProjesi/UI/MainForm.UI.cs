namespace KütüphaneProjesi.UI
{
    public partial class MainForm
    {
        private static readonly Color AnaRenk = Color.FromArgb(66, 39, 105);
        private static readonly Color VurguRenk = Color.FromArgb(142, 98, 224);
        private static readonly Color ArkaPlanRenk = Color.FromArgb(246, 242, 255);
        private static readonly Color KartRenk = Color.White;

        private void ArayuzuHazirla()
        {
            SuspendLayout();

            BackColor = ArkaPlanRenk;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 162);
            MinimumSize = new Size(1380, 860);
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            groupBox7.Visible = false;

            AramaBolumunuHazirla();
            HizliListeBolumunuHazirla();
            KullaniciYonetimiBolumunuHazirla();
            YazarYonetimiBolumunuHazirla();
            TurYonetimiBolumunuHazirla();
            KitapIslemBolumunuHazirla();
            OduncBolumunuHazirla();
            StilUygula();

            var ustPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 72,
                BackColor = AnaRenk,
                Padding = new Padding(20, 12, 20, 12)
            };

            var baslikEtiketi = new Label
            {
                AutoSize = true,
                Dock = DockStyle.Left,
                Font = new Font("Segoe UI Semibold", 17F, FontStyle.Bold, GraphicsUnit.Point, 162),
                ForeColor = Color.White,
                Text = "Kütüphane Yönetim Masası"
            };

            var cikisButonu = new Button
            {
                Dock = DockStyle.Right,
                Width = 122,
                Text = "Çıkış"
            };
            ButonStiliUygula(cikisButonu, ikincil: true);
            cikisButonu.Click += (_, _) => Close();

            _aktifKullaniciEtiketi = new Label
            {
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight,
                AutoEllipsis = true,
                Width = 420,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 162),
                ForeColor = Color.FromArgb(236, 226, 252),
                Text = $"Aktif Kullanıcı: {_kullaniciAdi} ({RolMetniGetir(_rol)})"
            };

            ustPanel.Controls.Add(cikisButonu);
            ustPanel.Controls.Add(_aktifKullaniciEtiketi);
            ustPanel.Controls.Add(baslikEtiketi);

            _anaAyrim = new SplitContainer
            {
                Dock = DockStyle.Fill,
                FixedPanel = FixedPanel.None,
                SplitterWidth = 6,
                BorderStyle = BorderStyle.None
            };

            _anaAyrim.Panel1.Padding = new Padding(16, 16, 8, 16);
            _anaAyrim.Panel2.Padding = new Padding(8, 16, 16, 16);
            _anaAyrim.Panel1.BackColor = ArkaPlanRenk;
            _anaAyrim.Panel2.BackColor = ArkaPlanRenk;

            var solYerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            solYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            solYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));

            var solUstYerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2
            };
            solUstYerlesim.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            solUstYerlesim.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            solUstYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 208F));
            solUstYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            groupBox1.Margin = new Padding(0, 0, 10, 10);
            groupBox2.Margin = new Padding(0, 0, 0, 10);
            dataGridView1.Margin = new Padding(0);

            groupBox1.Dock = DockStyle.Fill;
            groupBox2.Dock = DockStyle.Fill;
            dataGridView1.Dock = DockStyle.Fill;

            solUstYerlesim.Controls.Add(groupBox1, 0, 0);
            solUstYerlesim.Controls.Add(groupBox2, 1, 0);
            solUstYerlesim.Controls.Add(dataGridView1, 0, 1);
            solUstYerlesim.SetColumnSpan(dataGridView1, 2);

            var solAltYerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            solAltYerlesim.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            solAltYerlesim.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));

            S.Margin = new Padding(0, 10, 10, 0);
            groupBox6.Margin = new Padding(0, 10, 0, 0);

            S.Dock = DockStyle.Fill;
            groupBox6.Dock = DockStyle.Fill;

            solAltYerlesim.Controls.Add(S, 0, 0);
            solAltYerlesim.Controls.Add(groupBox6, 1, 0);

            solYerlesim.Controls.Add(solUstYerlesim, 0, 0);
            solYerlesim.Controls.Add(solAltYerlesim, 0, 1);

            _yonetimAkisPaneli = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = ArkaPlanRenk,
                Padding = new Padding(0, 0, 0, 8)
            };

            groupBox3.Margin = new Padding(0, 0, 0, 12);
            groupBox4.Margin = new Padding(0, 0, 0, 12);
            groupBox5.Margin = new Padding(0);

            _yonetimAkisPaneli.Controls.Add(groupBox3);
            _yonetimAkisPaneli.Controls.Add(groupBox4);
            _yonetimAkisPaneli.Controls.Add(groupBox5);
            _yonetimAkisPaneli.SizeChanged += (_, _) => SagPanelGenisliginiAyarla();

            _anaAyrim.Panel1.Controls.Add(solYerlesim);
            _anaAyrim.Panel2.Controls.Add(_yonetimAkisPaneli);

            Controls.Clear();
            Controls.Add(_anaAyrim);
            Controls.Add(ustPanel);

            SagPanelGenisliginiAyarla();
            AktifKullaniciEtiketiGenisligiAyarla();
            AnaYerlesimiGuncelle();

            Resize -= MainForm_Resize;
            Resize += MainForm_Resize;

            ResumeLayout(true);
        }
        private void SagPanelGenisliginiAyarla()
        {
            if (_yonetimAkisPaneli == null || (_anaAyrim != null && _anaAyrim.Panel2Collapsed))
            {
                return;
            }

            var hedefGenislik = Math.Max(280, _yonetimAkisPaneli.ClientSize.Width - _yonetimAkisPaneli.Padding.Horizontal - 8);
            foreach (var kontrol in _yonetimAkisPaneli.Controls.OfType<GroupBox>())
            {
                kontrol.Width = hedefGenislik;
            }

            YonetimPanelYukseklikleriniAyarla();
        }

        private void YonetimPanelYukseklikleriniAyarla()
        {
            groupBox3.Height = OlcekliPiksel(270);
            groupBox4.Height = OlcekliPiksel(150);
            groupBox5.Height = OlcekliPiksel(150);
        }

        private int OlcekliPiksel(int tabanDeger)
        {
            return (int)Math.Round(tabanDeger * (DeviceDpi / 96f));
        }

        private void AktifKullaniciEtiketiGenisligiAyarla()
        {
            if (_aktifKullaniciEtiketi == null)
            {
                return;
            }

            var hedefGenislik = Math.Min(560, Math.Max(260, ClientSize.Width / 3));
            _aktifKullaniciEtiketi.Width = hedefGenislik;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            AnaYerlesimiGuncelle();
            SagPanelGenisliginiAyarla();
            AktifKullaniciEtiketiGenisligiAyarla();
        }
        private void AnaYerlesimiGuncelle()
        {
            if (_anaAyrim == null || IsDisposed)
            {
                return;
            }

            if (_anaAyrim.Panel2Collapsed)
            {
                return;
            }

            var toplamGenislik = _anaAyrim.ClientSize.Width;
            var splitterGenisligi = Math.Max(1, _anaAyrim.SplitterWidth);
            var kullanilabilirGenislik = Math.Max(0, toplamGenislik - splitterGenisligi);
            if (kullanilabilirGenislik <= 0)
            {
                return;
            }

            const int IstenenSolMin = 780;
            const int IstenenSagMin = 320;
            const int MutlakAltSinir = 60;

            var sagMin = Math.Min(IstenenSagMin, Math.Max(180, kullanilabilirGenislik / 3));
            if (sagMin >= kullanilabilirGenislik)
            {
                sagMin = Math.Max(MutlakAltSinir, kullanilabilirGenislik / 3);
            }

            var solMin = Math.Min(IstenenSolMin, kullanilabilirGenislik - sagMin);
            if (solMin < 120)
            {
                solMin = Math.Max(MutlakAltSinir, kullanilabilirGenislik - sagMin);
            }

            if (solMin + sagMin > kullanilabilirGenislik)
            {
                sagMin = Math.Max(MutlakAltSinir, kullanilabilirGenislik - solMin);
            }

            if (solMin < 0)
            {
                solMin = 0;
            }

            if (sagMin < 0)
            {
                sagMin = 0;
            }

            if (solMin + sagMin > kullanilabilirGenislik)
            {
                var dengeliSol = Math.Max(MutlakAltSinir, kullanilabilirGenislik / 2);
                solMin = Math.Min(dengeliSol, kullanilabilirGenislik);
                sagMin = Math.Max(0, kullanilabilirGenislik - solMin);
            }

            _anaAyrim.Panel1MinSize = solMin;
            _anaAyrim.Panel2MinSize = sagMin;

            var hedefSol = (int)(kullanilabilirGenislik * 0.74);
            var minSol = solMin;
            var maxSol = kullanilabilirGenislik - sagMin;

            if (maxSol < minSol)
            {
                maxSol = minSol;
            }

            _anaAyrim.SplitterDistance = Math.Clamp(hedefSol, minSol, maxSol);
        }

        private static string RolMetniGetir(string rol)
        {
            return string.Equals(rol, "Yonetici", StringComparison.OrdinalIgnoreCase)
                ? "Yönetici"
                : "Kullanıcı";
        }

        private void AramaBolumunuHazirla()
        {
            groupBox1.Text = "Kitap Arama";
            txtKitapAdi.PlaceholderText = "Kitap adına göre ara";
            txtYazar.PlaceholderText = "Yazara göre ara";
            txtTur.PlaceholderText = "Türe göre ara";

            btnIsmeGoreAra.Text = "Kitap Adı";
            btnYazaraGoreAra.Text = "Yazar";
            btnTureGoreAra.Text = "Tür";

            var yerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(10)
            };
            yerlesim.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 74F));
            yerlesim.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            txtKitapAdi.Dock = DockStyle.Fill;
            txtYazar.Dock = DockStyle.Fill;
            txtTur.Dock = DockStyle.Fill;

            btnIsmeGoreAra.Dock = DockStyle.Fill;
            btnYazaraGoreAra.Dock = DockStyle.Fill;
            btnTureGoreAra.Dock = DockStyle.Fill;

            yerlesim.Controls.Add(txtKitapAdi, 0, 0);
            yerlesim.Controls.Add(btnIsmeGoreAra, 1, 0);
            yerlesim.Controls.Add(txtYazar, 0, 1);
            yerlesim.Controls.Add(btnYazaraGoreAra, 1, 1);
            yerlesim.Controls.Add(txtTur, 0, 2);
            yerlesim.Controls.Add(btnTureGoreAra, 1, 2);

            groupBox1.Controls.Clear();
            groupBox1.Controls.Add(yerlesim);
        }

        private void HizliListeBolumunuHazirla()
        {
            groupBox2.Text = "Hızlı Listeleme";
            textBox5.PlaceholderText = "Yazar veya tür filtresi";
            btnTumKitaplariListele.Text = "Tüm Kitapları Listele";
            btnYazaraGoreListele.Text = "Yazarları Listele";
            btnTureGoreListele.Text = "Türleri Listele";
            btnÖdüncKitapListele.Text = "Ödünç Kayıtlarını Listele";

            var baslikEtiketi = new Label
            {
                AutoSize = true,
                Text = "Filtre"
            };

            var yerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                Padding = new Padding(12)
            };
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

            textBox5.Dock = DockStyle.Fill;
            btnTumKitaplariListele.Dock = DockStyle.Fill;
            btnYazaraGoreListele.Dock = DockStyle.Fill;
            btnTureGoreListele.Dock = DockStyle.Fill;
            btnÖdüncKitapListele.Dock = DockStyle.Fill;

            if (!_yoneticiMi)
            {
                btnÖdüncKitapListele.Visible = false;
                btnÖdüncKitapListele.Enabled = false;
                yerlesim.RowStyles[5].Height = 0F;
            }

            yerlesim.Controls.Add(baslikEtiketi, 0, 0);
            yerlesim.Controls.Add(textBox5, 0, 1);
            yerlesim.Controls.Add(btnTumKitaplariListele, 0, 2);
            yerlesim.Controls.Add(btnYazaraGoreListele, 0, 3);
            yerlesim.Controls.Add(btnTureGoreListele, 0, 4);
            if (_yoneticiMi)
            {
                yerlesim.Controls.Add(btnÖdüncKitapListele, 0, 5);
            }

            groupBox2.Controls.Clear();
            groupBox2.Controls.Add(yerlesim);
        }
        private void KullaniciYonetimiBolumunuHazirla()
        {
            groupBox3.Text = "Kullanıcı Yönetimi";
            txtKullaniciID.PlaceholderText = "Kullanıcı ID";
            txtKullaniciAdi.PlaceholderText = "Kullanıcı adı";

            btnKullaniciListele.Text = "Kullanıcıları Listele";
            btnYetkiDuzenle.Text = "Yetkiyi Güncelle";
            Sil.Text = "Kullanıcıyı Sil";

            radioKullanici.Text = "Kullanıcı";
            radioAdmin.Text = "Yönetici";

            var idPaneli = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0)
            };
            idPaneli.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            idPaneli.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));

            txtKullaniciID.Dock = DockStyle.Fill;
            txtKullaniciAdi.Dock = DockStyle.Fill;
            txtKullaniciID.Margin = new Padding(0, 0, 6, 0);
            txtKullaniciAdi.Margin = new Padding(0);

            idPaneli.Controls.Add(txtKullaniciID, 0, 0);
            idPaneli.Controls.Add(txtKullaniciAdi, 1, 0);

            var rolPaneli = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0, 2, 0, 0),
                AutoSize = false
            };

            radioKullanici.Margin = new Padding(0, 0, 14, 0);
            radioAdmin.Margin = new Padding(0);
            rolPaneli.Controls.Add(radioKullanici);
            rolPaneli.Controls.Add(radioAdmin);

            var yerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                Padding = new Padding(12)
            };
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            btnKullaniciListele.Dock = DockStyle.Fill;
            btnYetkiDuzenle.Dock = DockStyle.Fill;
            Sil.Dock = DockStyle.Fill;

            btnKullaniciListele.Margin = new Padding(0, 0, 0, 6);
            idPaneli.Margin = new Padding(0, 0, 0, 6);
            rolPaneli.Margin = new Padding(0, 0, 0, 6);
            btnYetkiDuzenle.Margin = new Padding(0, 0, 0, 6);
            Sil.Margin = new Padding(0);

            yerlesim.Controls.Add(btnKullaniciListele, 0, 0);
            yerlesim.Controls.Add(idPaneli, 0, 1);
            yerlesim.Controls.Add(rolPaneli, 0, 2);
            yerlesim.Controls.Add(btnYetkiDuzenle, 0, 3);
            yerlesim.Controls.Add(Sil, 0, 4);

            groupBox3.Controls.Clear();
            groupBox3.Controls.Add(yerlesim);
        }
        private void YazarYonetimiBolumunuHazirla()
        {
            groupBox4.Text = "Yazar Yönetimi";
            txtYazarAdi.PlaceholderText = "Yazar adı";

            button1.Text = "Listele";
            btnYazarEkle.Text = "Ekle";
            btnYazarSil.Text = "Sil";

            var butonPaneli = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0)
            };
            butonPaneli.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            butonPaneli.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            butonPaneli.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            button1.Dock = DockStyle.Fill;
            btnYazarEkle.Dock = DockStyle.Fill;
            btnYazarSil.Dock = DockStyle.Fill;

            button1.Margin = new Padding(0, 0, 6, 0);
            btnYazarEkle.Margin = new Padding(0, 0, 6, 0);
            btnYazarSil.Margin = new Padding(0);

            butonPaneli.Controls.Add(button1, 0, 0);
            butonPaneli.Controls.Add(btnYazarEkle, 1, 0);
            butonPaneli.Controls.Add(btnYazarSil, 2, 0);

            var yerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(12)
            };
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            txtYazarAdi.Dock = DockStyle.Fill;
            txtYazarAdi.Margin = new Padding(0, 0, 0, 6);
            butonPaneli.Margin = new Padding(0);

            yerlesim.Controls.Add(txtYazarAdi, 0, 0);
            yerlesim.Controls.Add(butonPaneli, 0, 1);

            groupBox4.Controls.Clear();
            groupBox4.Controls.Add(yerlesim);
        }
        private void TurYonetimiBolumunuHazirla()
        {
            groupBox5.Text = "Tür Yönetimi";
            txtTurAdi.PlaceholderText = "Tür adı";

            button2.Text = "Listele";
            btnTurEkle.Text = "Ekle";
            button3.Text = "Sil";

            var butonPaneli = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0)
            };
            butonPaneli.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            butonPaneli.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            butonPaneli.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            button2.Dock = DockStyle.Fill;
            btnTurEkle.Dock = DockStyle.Fill;
            button3.Dock = DockStyle.Fill;

            button2.Margin = new Padding(0, 0, 6, 0);
            btnTurEkle.Margin = new Padding(0, 0, 6, 0);
            button3.Margin = new Padding(0);

            butonPaneli.Controls.Add(button2, 0, 0);
            butonPaneli.Controls.Add(btnTurEkle, 1, 0);
            butonPaneli.Controls.Add(button3, 2, 0);

            var yerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(12)
            };
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            txtTurAdi.Dock = DockStyle.Fill;
            txtTurAdi.Margin = new Padding(0, 0, 0, 6);
            butonPaneli.Margin = new Padding(0);

            yerlesim.Controls.Add(txtTurAdi, 0, 0);
            yerlesim.Controls.Add(butonPaneli, 0, 1);

            groupBox5.Controls.Clear();
            groupBox5.Controls.Add(yerlesim);
        }

        private void KitapIslemBolumunuHazirla()
        {
            S.Text = "Kitap İşlemleri";
            txtKitapID.PlaceholderText = "Kitap ID";
            txtKitapKullaniciAdi.PlaceholderText = "Kitap adı";
            txtKitapYazar.PlaceholderText = "Yazar";
            txtKitapTur.PlaceholderText = "Tür";
            txtKitapYayinEvi.PlaceholderText = "Yayınevi";
            txtKitapBasimYili.PlaceholderText = "Basım yılı";

            btnKitapEkle.Text = "Ekle";
            btnKitapSil.Text = "Sil";
            btnKitapGuncelle.Text = "Güncelle";

            var girdiYerlesimi = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2
            };
            girdiYerlesimi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            girdiYerlesimi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            girdiYerlesimi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            girdiYerlesimi.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            girdiYerlesimi.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            txtKitapID.Dock = DockStyle.Fill;
            txtKitapKullaniciAdi.Dock = DockStyle.Fill;
            txtKitapYazar.Dock = DockStyle.Fill;
            txtKitapTur.Dock = DockStyle.Fill;
            txtKitapYayinEvi.Dock = DockStyle.Fill;
            txtKitapBasimYili.Dock = DockStyle.Fill;

            girdiYerlesimi.Controls.Add(txtKitapID, 0, 0);
            girdiYerlesimi.Controls.Add(txtKitapKullaniciAdi, 1, 0);
            girdiYerlesimi.Controls.Add(txtKitapYazar, 2, 0);
            girdiYerlesimi.Controls.Add(txtKitapTur, 0, 1);
            girdiYerlesimi.Controls.Add(txtKitapYayinEvi, 1, 1);
            girdiYerlesimi.Controls.Add(txtKitapBasimYili, 2, 1);

            var butonPaneli = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1
            };
            butonPaneli.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            butonPaneli.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            butonPaneli.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            btnKitapEkle.Dock = DockStyle.Fill;
            btnKitapSil.Dock = DockStyle.Fill;
            btnKitapGuncelle.Dock = DockStyle.Fill;

            butonPaneli.Controls.Add(btnKitapEkle, 0, 0);
            butonPaneli.Controls.Add(btnKitapSil, 1, 0);
            butonPaneli.Controls.Add(btnKitapGuncelle, 2, 0);

            var anaYerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(12)
            };
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 68F));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 32F));
            anaYerlesim.Controls.Add(girdiYerlesimi, 0, 0);
            anaYerlesim.Controls.Add(butonPaneli, 0, 1);

            S.Controls.Clear();
            S.Controls.Add(anaYerlesim);
        }

        private void OduncBolumunuHazirla()
        {
            groupBox6.Text = "Ödünç İşlemleri";
            groupBox6.MinimumSize = new Size(0, OlcekliPiksel(300));
            BtnÖdüncKitapKaydet.Text = "Ödünç Kaydı Oluştur";

            if (_btnOduncKaydiGuncelle == null)
            {
                _btnOduncKaydiGuncelle = new Button
                {
                    Name = "btnOduncKaydiGuncelle"
                };
                _btnOduncKaydiGuncelle.Click += BtnÖdüncKaydiGuncelle_Click;
            }

            if (_btnOduncKaydiSil == null)
            {
                _btnOduncKaydiSil = new Button
                {
                    Name = "btnOduncKaydiSil"
                };
                _btnOduncKaydiSil.Click += BtnÖdüncKaydiSil_Click;
            }

            _btnOduncKaydiGuncelle.Text = "Ödünç Kaydı Güncelle";
            _btnOduncKaydiSil.Text = "Ödünç Kaydı Sil";

            ButonStiliUygula(BtnÖdüncKitapKaydet);
            if (_btnOduncKaydiGuncelle != null) ButonStiliUygula(_btnOduncKaydiGuncelle, ikincil: true);
            if (_btnOduncKaydiSil != null) ButonStiliUygula(_btnOduncKaydiSil, tehlikeli: true);

            label1.Text = "Kitap ID";
            label2.Text = "Kullanıcı ID";
            label3.Text = "Ödünç Tarihi";

            txtÖdüncKitapİd.PlaceholderText = "Örnek: 15";
            txtÖdüncKullanıcıİd.PlaceholderText = "Örnek: 7";
            txtÖdüncTarih.PlaceholderText = "GG.AA.YYYY";

            var alanYerlesimi = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2,
                Margin = new Padding(0)
            };
            alanYerlesimi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            alanYerlesimi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            alanYerlesimi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            alanYerlesimi.RowStyles.Add(new RowStyle(SizeType.Absolute, OlcekliPiksel(24)));
            alanYerlesimi.RowStyles.Add(new RowStyle(SizeType.Absolute, OlcekliPiksel(34)));

            label1.Dock = DockStyle.Fill;
            label2.Dock = DockStyle.Fill;
            label3.Dock = DockStyle.Fill;
            label1.TextAlign = ContentAlignment.MiddleLeft;
            label2.TextAlign = ContentAlignment.MiddleLeft;
            label3.TextAlign = ContentAlignment.MiddleLeft;

            txtÖdüncKitapİd.Dock = DockStyle.Fill;
            txtÖdüncKullanıcıİd.Dock = DockStyle.Fill;
            txtÖdüncTarih.Dock = DockStyle.Fill;

            txtÖdüncKitapİd.Margin = new Padding(0, 0, 6, 0);
            txtÖdüncKullanıcıİd.Margin = new Padding(0, 0, 6, 0);
            txtÖdüncTarih.Margin = new Padding(0);

            alanYerlesimi.Controls.Add(label1, 0, 0);
            alanYerlesimi.Controls.Add(label2, 1, 0);
            alanYerlesimi.Controls.Add(label3, 2, 0);
            alanYerlesimi.Controls.Add(txtÖdüncKitapİd, 0, 1);
            alanYerlesimi.Controls.Add(txtÖdüncKullanıcıİd, 1, 1);
            alanYerlesimi.Controls.Add(txtÖdüncTarih, 2, 1);

            var butonYerlesimi = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0)
            };
            butonYerlesimi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            butonYerlesimi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            butonYerlesimi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));

            BtnÖdüncKitapKaydet.Dock = DockStyle.Fill;
            _btnOduncKaydiGuncelle.Dock = DockStyle.Fill;
            _btnOduncKaydiSil.Dock = DockStyle.Fill;

            BtnÖdüncKitapKaydet.Margin = new Padding(0, 0, 6, 0);
            _btnOduncKaydiGuncelle.Margin = new Padding(0, 0, 6, 0);
            _btnOduncKaydiSil.Margin = new Padding(0);

            butonYerlesimi.Controls.Add(BtnÖdüncKitapKaydet, 0, 0);
            butonYerlesimi.Controls.Add(_btnOduncKaydiGuncelle, 1, 0);
            butonYerlesimi.Controls.Add(_btnOduncKaydiSil, 2, 0);

            var anaYerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(12)
            };
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, OlcekliPiksel(72)));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, OlcekliPiksel(40)));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Margin = new Padding(0);

            anaYerlesim.Controls.Add(alanYerlesimi, 0, 0);
            anaYerlesim.Controls.Add(butonYerlesimi, 0, 1);
            anaYerlesim.Controls.Add(dataGridView2, 0, 2);

            groupBox6.Controls.Clear();
            groupBox6.Controls.Add(anaYerlesim);
        }

        private void StilUygula()
        {
            GrupKutusuStili(groupBox1);
            GrupKutusuStili(groupBox2);
            GrupKutusuStili(groupBox3);
            GrupKutusuStili(groupBox4);
            GrupKutusuStili(groupBox5);
            GrupKutusuStili(groupBox6);
            GrupKutusuStili(S);

            ButonStiliUygula(btnIsmeGoreAra, ikincil: true);
            ButonStiliUygula(btnYazaraGoreAra, ikincil: true);
            ButonStiliUygula(btnTureGoreAra, ikincil: true);

            ButonStiliUygula(btnTumKitaplariListele);
            ButonStiliUygula(btnYazaraGoreListele, ikincil: true);
            ButonStiliUygula(btnTureGoreListele, ikincil: true);
            ButonStiliUygula(btnÖdüncKitapListele, ikincil: true);

            ButonStiliUygula(btnKullaniciListele);
            ButonStiliUygula(btnYetkiDuzenle, ikincil: true);
            ButonStiliUygula(Sil, tehlikeli: true);

            ButonStiliUygula(button1, ikincil: true);
            ButonStiliUygula(btnYazarEkle);
            ButonStiliUygula(btnYazarSil, tehlikeli: true);

            ButonStiliUygula(button2, ikincil: true);
            ButonStiliUygula(btnTurEkle);
            ButonStiliUygula(button3, tehlikeli: true);

            ButonStiliUygula(btnKitapEkle);
            ButonStiliUygula(btnKitapSil, tehlikeli: true);
            ButonStiliUygula(btnKitapGuncelle, ikincil: true);
            ButonStiliUygula(BtnÖdüncKitapKaydet);
            if (_btnOduncKaydiGuncelle != null) ButonStiliUygula(_btnOduncKaydiGuncelle, ikincil: true);
            if (_btnOduncKaydiSil != null) ButonStiliUygula(_btnOduncKaydiSil, tehlikeli: true);

            YaziKutusuStili(txtKitapAdi);
            YaziKutusuStili(txtYazar);
            YaziKutusuStili(txtTur);
            YaziKutusuStili(textBox5);
            YaziKutusuStili(txtKullaniciID);
            YaziKutusuStili(txtKullaniciAdi);
            YaziKutusuStili(txtYazarAdi);
            YaziKutusuStili(txtTurAdi);
            YaziKutusuStili(txtKitapID);
            YaziKutusuStili(txtKitapKullaniciAdi);
            YaziKutusuStili(txtKitapYazar);
            YaziKutusuStili(txtKitapTur);
            YaziKutusuStili(txtKitapYayinEvi);
            YaziKutusuStili(txtKitapBasimYili);
            YaziKutusuStili(txtÖdüncKitapİd);
            YaziKutusuStili(txtÖdüncKullanıcıİd);
            YaziKutusuStili(txtÖdüncTarih);

            VeriGridiStiliUygula(dataGridView1);
            VeriGridiStiliUygula(dataGridView2);
        }

        private static void GrupKutusuStili(GroupBox kutu)
        {
            kutu.BackColor = KartRenk;
            kutu.ForeColor = Color.FromArgb(64, 42, 102);
            kutu.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 162);
        }

        private static void YaziKutusuStili(TextBox kutu)
        {
            kutu.BackColor = Color.White;
            kutu.BorderStyle = BorderStyle.FixedSingle;
            kutu.ForeColor = Color.FromArgb(76, 57, 115);
            kutu.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 162);
            kutu.Margin = new Padding(3);
        }

        private static void ButonStiliUygula(Button buton, bool ikincil = false, bool tehlikeli = false)
        {
            buton.FlatStyle = FlatStyle.Flat;
            buton.FlatAppearance.BorderSize = 0;
            buton.UseVisualStyleBackColor = false;
            buton.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 162);
            buton.ForeColor = Color.White;
            buton.Height = 36;

            if (tehlikeli)
            {
                buton.BackColor = Color.FromArgb(201, 86, 76);
                return;
            }

            buton.BackColor = ikincil ? Color.FromArgb(123, 87, 189) : AnaRenk;
        }

        private static void VeriGridiStiliUygula(DataGridView tablo)
        {
            tablo.BackgroundColor = KartRenk;
            tablo.BorderStyle = BorderStyle.FixedSingle;
            tablo.EnableHeadersVisualStyles = false;
            tablo.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            tablo.GridColor = Color.FromArgb(224, 214, 243);
            tablo.RowHeadersVisible = false;

            tablo.ColumnHeadersDefaultCellStyle.BackColor = AnaRenk;
            tablo.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tablo.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 162);
            tablo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            tablo.DefaultCellStyle.BackColor = Color.White;
            tablo.DefaultCellStyle.ForeColor = Color.FromArgb(73, 56, 109);
            tablo.DefaultCellStyle.SelectionBackColor = VurguRenk;
            tablo.DefaultCellStyle.SelectionForeColor = Color.White;

            tablo.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(251, 247, 255);
            tablo.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
        }
    }
}

































