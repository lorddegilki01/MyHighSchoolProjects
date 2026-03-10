namespace KütüphaneProjesi.UI
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            checkBoxSifreGoster = new CheckBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            btnKaydol = new Button();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtSifre = new TextBox();
            txtKullaniciAdi = new TextBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.White;
            groupBox1.Controls.Add(checkBoxSifreGoster);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(btnKaydol);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtSifre);
            groupBox1.Controls.Add(txtKullaniciAdi);
            groupBox1.ForeColor = Color.FromArgb(64, 42, 102);
            groupBox1.Location = new Point(62, 34);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(24, 20, 24, 24);
            groupBox1.Size = new Size(676, 428);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Kayıt İşlemleri";
            // 
            // checkBoxSifreGoster
            // 
            checkBoxSifreGoster.AutoSize = true;
            checkBoxSifreGoster.ForeColor = Color.FromArgb(106, 88, 140);
            checkBoxSifreGoster.Location = new Point(273, 201);
            checkBoxSifreGoster.Name = "checkBoxSifreGoster";
            checkBoxSifreGoster.Size = new Size(114, 24);
            checkBoxSifreGoster.TabIndex = 4;
            checkBoxSifreGoster.Text = "Şifreyi göster";
            checkBoxSifreGoster.UseVisualStyleBackColor = true;
            checkBoxSifreGoster.CheckedChanged += checkBoxSifreGoster_CheckedChanged;
            // 
            // label6
            // 
            label6.ForeColor = Color.FromArgb(130, 109, 170);
            label6.Location = new Point(39, 360);
            label6.Name = "label6";
            label6.Size = new Size(597, 45);
            label6.TabIndex = 11;
            label6.Text = "Kullanıcı kaydı tamamlandığında giriş ekranına otomatik dönebilirsiniz.";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.FromArgb(130, 109, 170);
            label5.Location = new Point(42, 86);
            label5.Name = "label5";
            label5.Size = new Size(271, 20);
            label5.TabIndex = 10;
            label5.Text = "Yeni kullanıcı oluşturmak için bilgileri girin.";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label4.ForeColor = Color.FromArgb(64, 42, 102);
            label4.Location = new Point(37, 46);
            label4.Name = "label4";
            label4.Size = new Size(218, 41);
            label4.TabIndex = 9;
            label4.Text = "Yeni Kayıt Aç";
            // 
            // btnKaydol
            // 
            btnKaydol.BackColor = Color.FromArgb(122, 84, 198);
            btnKaydol.FlatAppearance.BorderSize = 0;
            btnKaydol.FlatStyle = FlatStyle.Flat;
            btnKaydol.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnKaydol.ForeColor = Color.White;
            btnKaydol.Location = new Point(39, 292);
            btnKaydol.Name = "btnKaydol";
            btnKaydol.Size = new Size(597, 52);
            btnKaydol.TabIndex = 7;
            btnKaydol.Text = "Kaydı Tamamla";
            btnKaydol.UseVisualStyleBackColor = false;
            btnKaydol.Click += btnKaydol_Click;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.ForeColor = Color.FromArgb(98, 76, 138);
            radioButton2.Location = new Point(421, 252);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(84, 24);
            radioButton2.TabIndex = 6;
            radioButton2.TabStop = true;
            radioButton2.Text = "Yönetici";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.ForeColor = Color.FromArgb(98, 76, 138);
            radioButton1.Location = new Point(273, 252);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(86, 24);
            radioButton1.TabIndex = 5;
            radioButton1.TabStop = true;
            radioButton1.Text = "Kullanıcı";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label3.ForeColor = Color.FromArgb(64, 42, 102);
            label3.Location = new Point(40, 248);
            label3.Name = "label3";
            label3.Size = new Size(43, 28);
            label3.TabIndex = 4;
            label3.Text = "Rol";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.ForeColor = Color.FromArgb(64, 42, 102);
            label2.Location = new Point(40, 165);
            label2.Name = "label2";
            label2.Size = new Size(55, 28);
            label2.TabIndex = 3;
            label2.Text = "Şifre";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.ForeColor = Color.FromArgb(64, 42, 102);
            label1.Location = new Point(39, 124);
            label1.Name = "label1";
            label1.Size = new Size(126, 28);
            label1.TabIndex = 2;
            label1.Text = "Kullanıcı Adı";
            // 
            // txtSifre
            // 
            txtSifre.BorderStyle = BorderStyle.FixedSingle;
            txtSifre.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtSifre.Location = new Point(273, 160);
            txtSifre.Name = "txtSifre";
            txtSifre.Size = new Size(363, 34);
            txtSifre.TabIndex = 3;
            // 
            // txtKullaniciAdi
            // 
            txtKullaniciAdi.BorderStyle = BorderStyle.FixedSingle;
            txtKullaniciAdi.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtKullaniciAdi.Location = new Point(273, 119);
            txtKullaniciAdi.Name = "txtKullaniciAdi";
            txtKullaniciAdi.Size = new Size(363, 34);
            txtKullaniciAdi.TabIndex = 2;
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 241, 255);
            ClientSize = new Size(800, 500);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RegisterForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Kütüphane Pro - Kayıt";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        private GroupBox groupBox1;
        private CheckBox checkBoxSifreGoster;
        private Label label6;
        private Label label5;
        private Label label4;
        private Button btnKaydol;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txtSifre;
        private TextBox txtKullaniciAdi;
    }
}






