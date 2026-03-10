namespace KütüphaneProjesi.UI
{
    partial class LoginForm
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
            panelKart = new Panel();
            panelSag = new Panel();
            label8 = new Label();
            label7 = new Label();
            checkBoxSifreGoster = new CheckBox();
            label1 = new Label();
            label3 = new Label();
            label2 = new Label();
            button1 = new Button();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            panelSol = new Panel();
            panelVurgu = new Panel();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            panelKart.SuspendLayout();
            panelSag.SuspendLayout();
            panelSol.SuspendLayout();
            SuspendLayout();
            // 
            // panelKart
            // 
            panelKart.Anchor = AnchorStyles.None;
            panelKart.BackColor = Color.White;
            panelKart.BorderStyle = BorderStyle.FixedSingle;
            panelKart.Controls.Add(panelSag);
            panelKart.Controls.Add(panelSol);
            panelKart.Location = new Point(120, 72);
            panelKart.Name = "panelKart";
            panelKart.Size = new Size(960, 590);
            panelKart.TabIndex = 0;
            // 
            // panelSag
            // 
            panelSag.BackColor = Color.White;
            panelSag.Controls.Add(label8);
            panelSag.Controls.Add(label7);
            panelSag.Controls.Add(checkBoxSifreGoster);
            panelSag.Controls.Add(label1);
            panelSag.Controls.Add(label3);
            panelSag.Controls.Add(label2);
            panelSag.Controls.Add(button1);
            panelSag.Controls.Add(textBox2);
            panelSag.Controls.Add(textBox1);
            panelSag.Dock = DockStyle.Fill;
            panelSag.Location = new Point(330, 0);
            panelSag.Name = "panelSag";
            panelSag.Size = new Size(628, 588);
            panelSag.TabIndex = 1;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label8.ForeColor = Color.FromArgb(117, 95, 154);
            label8.Location = new Point(60, 121);
            label8.Name = "label8";
            label8.Size = new Size(212, 23);
            label8.TabIndex = 8;
            label8.Text = "Bilgilerinizi girip devam edin.";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label7.ForeColor = Color.FromArgb(58, 35, 94);
            label7.Location = new Point(52, 64);
            label7.Name = "label7";
            label7.Size = new Size(328, 54);
            label7.TabIndex = 7;
            label7.Text = "Hesabına Giriş Yap";
            // 
            // checkBoxSifreGoster
            // 
            checkBoxSifreGoster.AutoSize = true;
            checkBoxSifreGoster.ForeColor = Color.FromArgb(106, 88, 140);
            checkBoxSifreGoster.Location = new Point(60, 347);
            checkBoxSifreGoster.Name = "checkBoxSifreGoster";
            checkBoxSifreGoster.Size = new Size(114, 24);
            checkBoxSifreGoster.TabIndex = 4;
            checkBoxSifreGoster.Text = "Şifreyi göster";
            checkBoxSifreGoster.UseVisualStyleBackColor = true;
            checkBoxSifreGoster.CheckedChanged += checkBoxSifreGoster_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Underline, GraphicsUnit.Point, 162);
            label1.ForeColor = Color.FromArgb(122, 84, 198);
            label1.Location = new Point(60, 460);
            label1.Name = "label1";
            label1.Size = new Size(182, 23);
            label1.TabIndex = 6;
            label1.Text = "Hesabın yok mu? Kayıt ol";
            label1.Click += label1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label3.ForeColor = Color.FromArgb(64, 42, 102);
            label3.Location = new Point(60, 260);
            label3.Name = "label3";
            label3.Size = new Size(55, 28);
            label3.TabIndex = 2;
            label3.Text = "Şifre";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.ForeColor = Color.FromArgb(64, 42, 102);
            label2.Location = new Point(60, 170);
            label2.Name = "label2";
            label2.Size = new Size(126, 28);
            label2.TabIndex = 0;
            label2.Text = "Kullanıcı Adı";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(122, 84, 198);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            button1.ForeColor = Color.White;
            button1.Location = new Point(60, 388);
            button1.Name = "button1";
            button1.Size = new Size(510, 54);
            button1.TabIndex = 5;
            button1.Text = "Giriş Yap";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            textBox2.Location = new Point(60, 295);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(510, 34);
            textBox2.TabIndex = 3;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            textBox1.Location = new Point(60, 205);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(510, 34);
            textBox1.TabIndex = 1;
            // 
            // panelSol
            // 
            panelSol.BackColor = Color.FromArgb(61, 34, 96);
            panelSol.Controls.Add(panelVurgu);
            panelSol.Controls.Add(label6);
            panelSol.Controls.Add(label5);
            panelSol.Controls.Add(label4);
            panelSol.Dock = DockStyle.Left;
            panelSol.Location = new Point(0, 0);
            panelSol.Name = "panelSol";
            panelSol.Padding = new Padding(32, 40, 32, 40);
            panelSol.Size = new Size(330, 588);
            panelSol.TabIndex = 0;
            // 
            // panelVurgu
            // 
            panelVurgu.BackColor = Color.FromArgb(174, 142, 235);
            panelVurgu.Location = new Point(34, 222);
            panelVurgu.Name = "panelVurgu";
            panelVurgu.Size = new Size(262, 2);
            panelVurgu.TabIndex = 3;
            // 
            // label6
            // 
            label6.ForeColor = Color.FromArgb(238, 229, 255);
            label6.Location = new Point(34, 246);
            label6.Name = "label6";
            label6.Size = new Size(262, 220);
            label6.TabIndex = 2;
            label6.Text = "• Rol bazlı giriş\r\n• Kitap, yazar ve tür yönetimi\r\n• Ödünç işlemlerini takip\r\n• Güvenli şifre doğrulama";
            // 
            // label5
            // 
            label5.ForeColor = Color.FromArgb(224, 209, 249);
            label5.Location = new Point(34, 136);
            label5.Name = "label5";
            label5.Size = new Size(262, 72);
            label5.TabIndex = 1;
            label5.Text = "Dijital kütüphane yönetimini\r\ntek ekrandan kolayca yönetin.";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 27F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label4.ForeColor = Color.White;
            label4.Location = new Point(34, 56);
            label4.Name = "label4";
            label4.Size = new Size(296, 62);
            label4.TabIndex = 0;
            label4.Text = "KÜTÜPHANE";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 241, 255);
            ClientSize = new Size(1200, 740);
            Controls.Add(panelKart);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kütüphane Pro - Giriş";
            panelKart.ResumeLayout(false);
            panelSag.ResumeLayout(false);
            panelSag.PerformLayout();
            panelSol.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel panelKart;
        private Panel panelSag;
        private Label label8;
        private Label label7;
        private CheckBox checkBoxSifreGoster;
        private Label label1;
        private Label label3;
        private Label label2;
        private Button button1;
        private TextBox textBox2;
        private TextBox textBox1;
        private Panel panelSol;
        private Panel panelVurgu;
        private Label label6;
        private Label label5;
        private Label label4;
    }
}



