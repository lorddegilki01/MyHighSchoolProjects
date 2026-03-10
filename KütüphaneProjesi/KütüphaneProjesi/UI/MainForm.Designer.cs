namespace KütüphaneProjesi.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            btnTureGoreAra = new Button();
            btnYazaraGoreAra = new Button();
            btnIsmeGoreAra = new Button();
            txtTur = new TextBox();
            txtYazar = new TextBox();
            txtKitapAdi = new TextBox();
            groupBox2 = new GroupBox();
            btnÖdüncKitapListele = new Button();
            btnTureGoreListele = new Button();
            btnYazaraGoreListele = new Button();
            btnTumKitaplariListele = new Button();
            textBox5 = new TextBox();
            dataGridView1 = new DataGridView();
            groupBox3 = new GroupBox();
            txtKullaniciID = new TextBox();
            txtKullaniciAdi = new TextBox();
            radioAdmin = new RadioButton();
            radioKullanici = new RadioButton();
            Sil = new Button();
            btnYetkiDuzenle = new Button();
            btnKullaniciListele = new Button();
            groupBox4 = new GroupBox();
            btnYazarSil = new Button();
            button1 = new Button();
            btnYazarEkle = new Button();
            txtYazarAdi = new TextBox();
            groupBox5 = new GroupBox();
            button3 = new Button();
            button2 = new Button();
            btnTurEkle = new Button();
            txtTurAdi = new TextBox();
            S = new GroupBox();
            txtKitapID = new TextBox();
            btnKitapGuncelle = new Button();
            btnKitapSil = new Button();
            btnKitapEkle = new Button();
            txtKitapKullaniciAdi = new TextBox();
            txtKitapBasimYili = new TextBox();
            txtKitapYayinEvi = new TextBox();
            txtKitapTur = new TextBox();
            txtKitapYazar = new TextBox();
            groupBox6 = new GroupBox();
            BtnÖdüncKitapKaydet = new Button();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtÖdüncTarih = new TextBox();
            txtÖdüncKullanıcıİd = new TextBox();
            txtÖdüncKitapİd = new TextBox();
            dataGridView2 = new DataGridView();
            groupBox7 = new GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            S.SuspendLayout();
            groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnTureGoreAra);
            groupBox1.Controls.Add(btnYazaraGoreAra);
            groupBox1.Controls.Add(btnIsmeGoreAra);
            groupBox1.Controls.Add(txtTur);
            groupBox1.Controls.Add(txtYazar);
            groupBox1.Controls.Add(txtKitapAdi);
            groupBox1.Location = new Point(21, 31);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(240, 147);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Kitap arama";
            // 
            // btnTureGoreAra
            // 
            btnTureGoreAra.Location = new Point(128, 101);
            btnTureGoreAra.Margin = new Padding(3, 4, 3, 4);
            btnTureGoreAra.Name = "btnTureGoreAra";
            btnTureGoreAra.Size = new Size(86, 31);
            btnTureGoreAra.TabIndex = 5;
            btnTureGoreAra.Text = "Ara";
            btnTureGoreAra.UseVisualStyleBackColor = true;
            btnTureGoreAra.Click += btnTureGoreAra_Click;
            // 
            // btnYazaraGoreAra
            // 
            btnYazaraGoreAra.Location = new Point(128, 67);
            btnYazaraGoreAra.Margin = new Padding(3, 4, 3, 4);
            btnYazaraGoreAra.Name = "btnYazaraGoreAra";
            btnYazaraGoreAra.Size = new Size(86, 31);
            btnYazaraGoreAra.TabIndex = 4;
            btnYazaraGoreAra.Text = "Ara";
            btnYazaraGoreAra.UseVisualStyleBackColor = true;
            btnYazaraGoreAra.Click += btnYazaraGoreAra_Click;
            // 
            // btnIsmeGoreAra
            // 
            btnIsmeGoreAra.Location = new Point(128, 29);
            btnIsmeGoreAra.Margin = new Padding(3, 4, 3, 4);
            btnIsmeGoreAra.Name = "btnIsmeGoreAra";
            btnIsmeGoreAra.Size = new Size(86, 31);
            btnIsmeGoreAra.TabIndex = 6;
            btnIsmeGoreAra.Text = "Ara";
            btnIsmeGoreAra.Click += btnIsmeGoreAra_Click_1;
            // 
            // txtTur
            // 
            txtTur.Location = new Point(7, 103);
            txtTur.Margin = new Padding(3, 4, 3, 4);
            txtTur.Name = "txtTur";
            txtTur.Size = new Size(114, 27);
            txtTur.TabIndex = 2;
            txtTur.Text = "Türe göre adama";
            // 
            // txtYazar
            // 
            txtYazar.Location = new Point(7, 68);
            txtYazar.Margin = new Padding(3, 4, 3, 4);
            txtYazar.Name = "txtYazar";
            txtYazar.Size = new Size(114, 27);
            txtYazar.TabIndex = 1;
            txtYazar.Text = "Yazara göre arama";
            // 
            // txtKitapAdi
            // 
            txtKitapAdi.Location = new Point(7, 29);
            txtKitapAdi.Margin = new Padding(3, 4, 3, 4);
            txtKitapAdi.Name = "txtKitapAdi";
            txtKitapAdi.Size = new Size(114, 27);
            txtKitapAdi.TabIndex = 0;
            txtKitapAdi.Text = "İsme göre arama";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnÖdüncKitapListele);
            groupBox2.Controls.Add(btnTureGoreListele);
            groupBox2.Controls.Add(btnYazaraGoreListele);
            groupBox2.Controls.Add(btnTumKitaplariListele);
            groupBox2.Location = new Point(21, 185);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(240, 201);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "Kitap liseteleme";
            // 
            // btnÖdüncKitapListele
            // 
            btnÖdüncKitapListele.Location = new Point(15, 144);
            btnÖdüncKitapListele.Name = "btnÖdüncKitapListele";
            btnÖdüncKitapListele.Size = new Size(207, 29);
            btnÖdüncKitapListele.TabIndex = 6;
            btnÖdüncKitapListele.Text = "Ödünc Kitap Listele";
            btnÖdüncKitapListele.UseVisualStyleBackColor = true;
            btnÖdüncKitapListele.Click += btnÖdüncKitapListele_Click;
            // 
            // btnTureGoreListele
            // 
            btnTureGoreListele.Location = new Point(15, 106);
            btnTureGoreListele.Margin = new Padding(3, 4, 3, 4);
            btnTureGoreListele.Name = "btnTureGoreListele";
            btnTureGoreListele.Size = new Size(207, 31);
            btnTureGoreListele.TabIndex = 5;
            btnTureGoreListele.Text = "Türleri listele";
            btnTureGoreListele.UseVisualStyleBackColor = true;
            btnTureGoreListele.Click += btnTureGoreListele_Click;
            // 
            // btnYazaraGoreListele
            // 
            btnYazaraGoreListele.Location = new Point(15, 67);
            btnYazaraGoreListele.Margin = new Padding(3, 4, 3, 4);
            btnYazaraGoreListele.Name = "btnYazaraGoreListele";
            btnYazaraGoreListele.Size = new Size(207, 31);
            btnYazaraGoreListele.TabIndex = 4;
            btnYazaraGoreListele.Text = "Yazarı Listele";
            btnYazaraGoreListele.UseVisualStyleBackColor = true;
            btnYazaraGoreListele.Click += btnYazaraGoreListele_Click;
            // 
            // btnTumKitaplariListele
            // 
            btnTumKitaplariListele.Location = new Point(15, 28);
            btnTumKitaplariListele.Margin = new Padding(3, 4, 3, 4);
            btnTumKitaplariListele.Name = "btnTumKitaplariListele";
            btnTumKitaplariListele.Size = new Size(207, 31);
            btnTumKitaplariListele.TabIndex = 3;
            btnTumKitaplariListele.Text = "Tüm Kitapları Listele";
            btnTumKitaplariListele.UseVisualStyleBackColor = true;
            btnTumKitaplariListele.Click += btnTumKitaplariListele_Click;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(290, 500);
            textBox5.Margin = new Padding(3, 4, 3, 4);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(114, 27);
            textBox5.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.None;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(268, 42);
            dataGridView1.Margin = new Padding(3, 4, 3, 4);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(632, 659);
            dataGridView1.TabIndex = 7;
            dataGridView1.CellClick += dataGridView1_CellClick;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(txtKullaniciID);
            groupBox3.Controls.Add(txtKullaniciAdi);
            groupBox3.Controls.Add(radioAdmin);
            groupBox3.Controls.Add(radioKullanici);
            groupBox3.Controls.Add(Sil);
            groupBox3.Controls.Add(btnYetkiDuzenle);
            groupBox3.Controls.Add(btnKullaniciListele);
            groupBox3.Location = new Point(21, 394);
            groupBox3.Margin = new Padding(3, 4, 3, 4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(3, 4, 3, 4);
            groupBox3.Size = new Size(240, 203);
            groupBox3.TabIndex = 8;
            groupBox3.TabStop = false;
            groupBox3.Text = "Kullanıcı Yönetimi";
            // 
            // txtKullaniciID
            // 
            txtKullaniciID.Location = new Point(15, 98);
            txtKullaniciID.Margin = new Padding(3, 4, 3, 4);
            txtKullaniciID.Name = "txtKullaniciID";
            txtKullaniciID.Size = new Size(97, 27);
            txtKullaniciID.TabIndex = 7;
            // 
            // txtKullaniciAdi
            // 
            txtKullaniciAdi.Location = new Point(118, 98);
            txtKullaniciAdi.Margin = new Padding(3, 4, 3, 4);
            txtKullaniciAdi.Name = "txtKullaniciAdi";
            txtKullaniciAdi.Size = new Size(96, 27);
            txtKullaniciAdi.TabIndex = 6;
            // 
            // radioAdmin
            // 
            radioAdmin.AutoSize = true;
            radioAdmin.Location = new Point(128, 61);
            radioAdmin.Margin = new Padding(3, 4, 3, 4);
            radioAdmin.Name = "radioAdmin";
            radioAdmin.Size = new Size(74, 24);
            radioAdmin.TabIndex = 5;
            radioAdmin.TabStop = true;
            radioAdmin.Text = "Admin";
            radioAdmin.UseVisualStyleBackColor = true;
            // 
            // radioKullanici
            // 
            radioKullanici.AutoSize = true;
            radioKullanici.Location = new Point(19, 61);
            radioKullanici.Margin = new Padding(3, 4, 3, 4);
            radioKullanici.Name = "radioKullanici";
            radioKullanici.Size = new Size(86, 24);
            radioKullanici.TabIndex = 4;
            radioKullanici.TabStop = true;
            radioKullanici.Text = "Kullanıcı";
            radioKullanici.UseVisualStyleBackColor = true;
            // 
            // Sil
            // 
            Sil.Location = new Point(17, 168);
            Sil.Margin = new Padding(3, 4, 3, 4);
            Sil.Name = "Sil";
            Sil.Size = new Size(197, 31);
            Sil.TabIndex = 3;
            Sil.Text = "Sil";
            Sil.UseVisualStyleBackColor = true;
            Sil.Click += Sil_Click;
            // 
            // btnYetkiDuzenle
            // 
            btnYetkiDuzenle.Location = new Point(19, 133);
            btnYetkiDuzenle.Margin = new Padding(3, 4, 3, 4);
            btnYetkiDuzenle.Name = "btnYetkiDuzenle";
            btnYetkiDuzenle.Size = new Size(194, 31);
            btnYetkiDuzenle.TabIndex = 2;
            btnYetkiDuzenle.Text = "Yetki Düzenle";
            btnYetkiDuzenle.UseVisualStyleBackColor = true;
            btnYetkiDuzenle.Click += btnYetkiDuzenle_Click;
            // 
            // btnKullaniciListele
            // 
            btnKullaniciListele.Location = new Point(17, 23);
            btnKullaniciListele.Margin = new Padding(3, 4, 3, 4);
            btnKullaniciListele.Name = "btnKullaniciListele";
            btnKullaniciListele.Size = new Size(197, 31);
            btnKullaniciListele.TabIndex = 0;
            btnKullaniciListele.Text = "Kullanıcıları Listele";
            btnKullaniciListele.UseVisualStyleBackColor = true;
            btnKullaniciListele.Click += btnKullaniciListele_Click;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(btnYazarSil);
            groupBox4.Controls.Add(button1);
            groupBox4.Controls.Add(btnYazarEkle);
            groupBox4.Controls.Add(txtYazarAdi);
            groupBox4.Location = new Point(21, 601);
            groupBox4.Margin = new Padding(3, 4, 3, 4);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(3, 4, 3, 4);
            groupBox4.Size = new Size(241, 100);
            groupBox4.TabIndex = 9;
            groupBox4.TabStop = false;
            groupBox4.Text = "Yazar Yönetimi";
            // 
            // btnYazarSil
            // 
            btnYazarSil.Location = new Point(166, 60);
            btnYazarSil.Margin = new Padding(3, 4, 3, 4);
            btnYazarSil.Name = "btnYazarSil";
            btnYazarSil.Size = new Size(68, 32);
            btnYazarSil.TabIndex = 3;
            btnYazarSil.Text = "Sil";
            btnYazarSil.UseVisualStyleBackColor = true;
            btnYazarSil.Click += btnYazarSil_Click;
            // 
            // button1
            // 
            button1.Location = new Point(15, 60);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(74, 32);
            button1.TabIndex = 2;
            button1.Text = "Listele";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnYazarEkle
            // 
            btnYazarEkle.Location = new Point(95, 60);
            btnYazarEkle.Margin = new Padding(3, 4, 3, 4);
            btnYazarEkle.Name = "btnYazarEkle";
            btnYazarEkle.Size = new Size(68, 32);
            btnYazarEkle.TabIndex = 1;
            btnYazarEkle.Text = "Ekle";
            btnYazarEkle.UseVisualStyleBackColor = true;
            btnYazarEkle.Click += btnYazarEkle_Click;
            // 
            // txtYazarAdi
            // 
            txtYazarAdi.Location = new Point(11, 25);
            txtYazarAdi.Margin = new Padding(3, 4, 3, 4);
            txtYazarAdi.Name = "txtYazarAdi";
            txtYazarAdi.Size = new Size(224, 27);
            txtYazarAdi.TabIndex = 0;
            txtYazarAdi.Text = "Yazar Adı";
            txtYazarAdi.Click += txtYazarAdi_Click;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(button3);
            groupBox5.Controls.Add(button2);
            groupBox5.Controls.Add(btnTurEkle);
            groupBox5.Controls.Add(txtTurAdi);
            groupBox5.Location = new Point(20, 702);
            groupBox5.Margin = new Padding(3, 4, 3, 4);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(3, 4, 3, 4);
            groupBox5.Size = new Size(241, 130);
            groupBox5.TabIndex = 9;
            groupBox5.TabStop = false;
            groupBox5.Text = "Tür Yönetimi";
            // 
            // button3
            // 
            button3.Location = new Point(167, 62);
            button3.Margin = new Padding(3, 4, 3, 4);
            button3.Name = "button3";
            button3.Size = new Size(68, 31);
            button3.TabIndex = 4;
            button3.Text = "Sil";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Location = new Point(15, 62);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(75, 31);
            button2.TabIndex = 3;
            button2.Text = "Listele";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnTurEkle
            // 
            btnTurEkle.Location = new Point(96, 62);
            btnTurEkle.Margin = new Padding(3, 4, 3, 4);
            btnTurEkle.Name = "btnTurEkle";
            btnTurEkle.Size = new Size(68, 31);
            btnTurEkle.TabIndex = 2;
            btnTurEkle.Text = "Ekle";
            btnTurEkle.UseVisualStyleBackColor = true;
            btnTurEkle.Click += btnTurEkle_Click;
            // 
            // txtTurAdi
            // 
            txtTurAdi.Location = new Point(12, 27);
            txtTurAdi.Margin = new Padding(3, 4, 3, 4);
            txtTurAdi.Name = "txtTurAdi";
            txtTurAdi.Size = new Size(223, 27);
            txtTurAdi.TabIndex = 0;
            txtTurAdi.Text = "Tür Adı";
            txtTurAdi.Click += txtTurAdi_Click;
            // 
            // S
            // 
            S.Controls.Add(txtKitapID);
            S.Controls.Add(btnKitapGuncelle);
            S.Controls.Add(btnKitapSil);
            S.Controls.Add(btnKitapEkle);
            S.Controls.Add(txtKitapKullaniciAdi);
            S.Controls.Add(txtKitapBasimYili);
            S.Controls.Add(txtKitapYayinEvi);
            S.Controls.Add(txtKitapTur);
            S.Controls.Add(txtKitapYazar);
            S.Location = new Point(267, 702);
            S.Margin = new Padding(3, 4, 3, 4);
            S.Name = "S";
            S.Padding = new Padding(3, 4, 3, 4);
            S.Size = new Size(633, 130);
            S.TabIndex = 10;
            S.TabStop = false;
            S.Text = "Kitap İşlemleri";
            // 
            // txtKitapID
            // 
            txtKitapID.Location = new Point(15, 24);
            txtKitapID.Margin = new Padding(3, 4, 3, 4);
            txtKitapID.Name = "txtKitapID";
            txtKitapID.Size = new Size(114, 27);
            txtKitapID.TabIndex = 8;
            txtKitapID.Text = "Kitap id";
            txtKitapID.Click += txtKitapID_Click;
            // 
            // btnKitapGuncelle
            // 
            btnKitapGuncelle.Location = new Point(509, 24);
            btnKitapGuncelle.Margin = new Padding(3, 4, 3, 4);
            btnKitapGuncelle.Name = "btnKitapGuncelle";
            btnKitapGuncelle.Size = new Size(86, 67);
            btnKitapGuncelle.TabIndex = 7;
            btnKitapGuncelle.Text = "Güncelle";
            btnKitapGuncelle.UseVisualStyleBackColor = true;
            btnKitapGuncelle.Click += btnKitapGuncelle_Click;
            // 
            // btnKitapSil
            // 
            btnKitapSil.Location = new Point(416, 24);
            btnKitapSil.Margin = new Padding(3, 4, 3, 4);
            btnKitapSil.Name = "btnKitapSil";
            btnKitapSil.Size = new Size(86, 67);
            btnKitapSil.TabIndex = 6;
            btnKitapSil.Text = "Sil";
            btnKitapSil.UseVisualStyleBackColor = true;
            btnKitapSil.Click += btnKitapSil_Click;
            // 
            // btnKitapEkle
            // 
            btnKitapEkle.Location = new Point(323, 24);
            btnKitapEkle.Margin = new Padding(3, 4, 3, 4);
            btnKitapEkle.Name = "btnKitapEkle";
            btnKitapEkle.Size = new Size(86, 66);
            btnKitapEkle.TabIndex = 5;
            btnKitapEkle.Text = "Ekle";
            btnKitapEkle.UseVisualStyleBackColor = true;
            btnKitapEkle.Click += btnKitapEkle_Click;
            // 
            // txtKitapKullaniciAdi
            // 
            txtKitapKullaniciAdi.Location = new Point(135, 24);
            txtKitapKullaniciAdi.Margin = new Padding(3, 4, 3, 4);
            txtKitapKullaniciAdi.Name = "txtKitapKullaniciAdi";
            txtKitapKullaniciAdi.Size = new Size(114, 27);
            txtKitapKullaniciAdi.TabIndex = 4;
            txtKitapKullaniciAdi.Text = "Kullanıcı adı";
            txtKitapKullaniciAdi.Click += txtKitapKullaniciAdi_Click;
            // 
            // txtKitapBasimYili
            // 
            txtKitapBasimYili.Location = new Point(135, 94);
            txtKitapBasimYili.Margin = new Padding(3, 4, 3, 4);
            txtKitapBasimYili.Name = "txtKitapBasimYili";
            txtKitapBasimYili.Size = new Size(114, 27);
            txtKitapBasimYili.TabIndex = 3;
            txtKitapBasimYili.Text = "Basım Yılını";
            txtKitapBasimYili.Click += txtKitapBasimYili_Click;
            // 
            // txtKitapYayinEvi
            // 
            txtKitapYayinEvi.Location = new Point(15, 94);
            txtKitapYayinEvi.Margin = new Padding(3, 4, 3, 4);
            txtKitapYayinEvi.Name = "txtKitapYayinEvi";
            txtKitapYayinEvi.Size = new Size(114, 27);
            txtKitapYayinEvi.TabIndex = 2;
            txtKitapYayinEvi.Text = "Yayın Evi";
            txtKitapYayinEvi.Click += txtKitapYayinEvi_Click;
            // 
            // txtKitapTur
            // 
            txtKitapTur.Location = new Point(135, 59);
            txtKitapTur.Margin = new Padding(3, 4, 3, 4);
            txtKitapTur.Name = "txtKitapTur";
            txtKitapTur.Size = new Size(114, 27);
            txtKitapTur.TabIndex = 1;
            txtKitapTur.Text = "Tür";
            txtKitapTur.Click += txtKitapTur_Click;
            // 
            // txtKitapYazar
            // 
            txtKitapYazar.Location = new Point(15, 59);
            txtKitapYazar.Margin = new Padding(3, 4, 3, 4);
            txtKitapYazar.Name = "txtKitapYazar";
            txtKitapYazar.Size = new Size(114, 27);
            txtKitapYazar.TabIndex = 0;
            txtKitapYazar.Text = "Yazar";
            txtKitapYazar.Click += txtKitapYazar_Click;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(BtnÖdüncKitapKaydet);
            groupBox6.Controls.Add(label3);
            groupBox6.Controls.Add(label2);
            groupBox6.Controls.Add(label1);
            groupBox6.Controls.Add(txtÖdüncTarih);
            groupBox6.Controls.Add(txtÖdüncKullanıcıİd);
            groupBox6.Controls.Add(txtÖdüncKitapİd);
            groupBox6.Controls.Add(dataGridView2);
            groupBox6.Location = new Point(906, 42);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(313, 344);
            groupBox6.TabIndex = 11;
            groupBox6.TabStop = false;
            groupBox6.Text = "Ödünç Kitaplar";
            // 
            // BtnÖdüncKitapKaydet
            // 
            BtnÖdüncKitapKaydet.Location = new Point(6, 131);
            BtnÖdüncKitapKaydet.Name = "BtnÖdüncKitapKaydet";
            BtnÖdüncKitapKaydet.Size = new Size(301, 29);
            BtnÖdüncKitapKaydet.TabIndex = 7;
            BtnÖdüncKitapKaydet.Text = "Kaydet";
            BtnÖdüncKitapKaydet.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(17, 101);
            label3.Name = "label3";
            label3.Size = new Size(93, 20);
            label3.TabIndex = 6;
            label3.Text = "Alınan Tarih :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 67);
            label2.Name = "label2";
            label2.Size = new Size(87, 20);
            label2.TabIndex = 5;
            label2.Text = "Kullanıcı İD:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(40, 33);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 4;
            label1.Text = "Kitap İD :";
            // 
            // txtÖdüncTarih
            // 
            txtÖdüncTarih.Location = new Point(116, 98);
            txtÖdüncTarih.Name = "txtÖdüncTarih";
            txtÖdüncTarih.Size = new Size(125, 27);
            txtÖdüncTarih.TabIndex = 3;
            // 
            // txtÖdüncKullanıcıİd
            // 
            txtÖdüncKullanıcıİd.Location = new Point(116, 64);
            txtÖdüncKullanıcıİd.Name = "txtÖdüncKullanıcıİd";
            txtÖdüncKullanıcıİd.Size = new Size(125, 27);
            txtÖdüncKullanıcıİd.TabIndex = 2;
            // 
            // txtÖdüncKitapİd
            // 
            txtÖdüncKitapİd.Location = new Point(116, 30);
            txtÖdüncKitapİd.Name = "txtÖdüncKitapİd";
            txtÖdüncKitapİd.Size = new Size(125, 27);
            txtÖdüncKitapİd.TabIndex = 1;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(6, 166);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(301, 163);
            dataGridView2.TabIndex = 0;
            // 
            // groupBox7
            // 
            groupBox7.Location = new Point(906, 392);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(313, 431);
            groupBox7.TabIndex = 12;
            groupBox7.TabStop = false;
            groupBox7.Text = "groupBox7";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1231, 833);
            Controls.Add(dataGridView1);
            Controls.Add(groupBox7);
            Controls.Add(groupBox6);
            Controls.Add(S);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(textBox5);
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kitap Arama Ve Listeleme";
            Load += MainForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            S.ResumeLayout(false);
            S.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox txtKitapAdi;
        private GroupBox groupBox2;
        private Button btnTureGoreListele;
        private Button btnYazaraGoreListele;
        private Button btnTumKitaplariListele;
        private TextBox textBox5;
        private Button btnTureGoreAra;
        private Button btnYazaraGoreAra;
        private TextBox txtTur;
        private TextBox txtYazar;
        private DataGridView dataGridView1;
        private GroupBox groupBox3;
        private Button btnYetkiDuzenle;
        private TextBox txtKitapYazar;
        private Button btnKullaniciListele;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox S;
        private Button Sil;
        private RadioButton radioAdmin;
        private RadioButton radioKullanici;
        private Button btnYazarEkle;
        private TextBox txtYazarAdi;
        private Button btnTurEkle;
        private TextBox txtTurAdi;
        private Button btnKitapGuncelle;
        private Button btnKitapSil;
        private Button btnKitapEkle;
        private TextBox txtKitapKullaniciAdi;
        private TextBox txtKitapBasimYili;
        private TextBox txtKitapYayinEvi;
        private TextBox txtKitapTur;
        private Button btnIsmeGoreAra;
        private TextBox txtKullaniciAdi;
        private Button button1;
        private Button button2;
        private TextBox txtKullaniciID;
        private TextBox txtKitapID;
        private Button btnYazarSil;
        private Button button3;
        private GroupBox groupBox6;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txtÖdüncTarih;
        private TextBox txtÖdüncKullanıcıİd;
        private TextBox txtÖdüncKitapİd;
        private DataGridView dataGridView2;
        private Button BtnÖdüncKitapKaydet;
        private Button btnÖdüncKitapListele;
        private GroupBox groupBox7;
    }
}




