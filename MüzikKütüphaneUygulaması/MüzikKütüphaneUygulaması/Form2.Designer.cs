namespace MüzikKütüphaneUygulaması
{
    partial class Form2
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
            this.dgv = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAra = new System.Windows.Forms.Button();
            this.cmbTur = new System.Windows.Forms.ComboBox();
            this.txtSanatciAdi = new System.Windows.Forms.TextBox();
            this.txtSarkiAdi = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnTureGoreListele = new System.Windows.Forms.Button();
            this.btnSanatcilariListele = new System.Windows.Forms.Button();
            this.btnTumSarkilar = new System.Windows.Forms.Button();
            this.btnTemizle = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(265, 1);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersWidth = 51;
            this.dgv.RowTemplate.Height = 24;
            this.dgv.Size = new System.Drawing.Size(536, 529);
            this.dgv.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnAra);
            this.groupBox1.Controls.Add(this.cmbTur);
            this.groupBox1.Controls.Add(this.txtSanatciAdi);
            this.groupBox1.Controls.Add(this.txtSarkiAdi);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(247, 153);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Şarkı Arama";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Tür :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Sanatçı Adı :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Şarkı Adı:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnAra
            // 
            this.btnAra.Location = new System.Drawing.Point(65, 115);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(175, 23);
            this.btnAra.TabIndex = 5;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // cmbTur
            // 
            this.cmbTur.FormattingEnabled = true;
            this.cmbTur.Location = new System.Drawing.Point(65, 85);
            this.cmbTur.Name = "cmbTur";
            this.cmbTur.Size = new System.Drawing.Size(176, 24);
            this.cmbTur.TabIndex = 4;
            // 
            // txtSanatciAdi
            // 
            this.txtSanatciAdi.Location = new System.Drawing.Point(105, 58);
            this.txtSanatciAdi.Name = "txtSanatciAdi";
            this.txtSanatciAdi.Size = new System.Drawing.Size(136, 22);
            this.txtSanatciAdi.TabIndex = 1;
            // 
            // txtSarkiAdi
            // 
            this.txtSarkiAdi.Location = new System.Drawing.Point(90, 30);
            this.txtSarkiAdi.Name = "txtSarkiAdi";
            this.txtSarkiAdi.Size = new System.Drawing.Size(151, 22);
            this.txtSarkiAdi.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnTureGoreListele);
            this.groupBox2.Controls.Add(this.btnSanatcilariListele);
            this.groupBox2.Controls.Add(this.btnTumSarkilar);
            this.groupBox2.Location = new System.Drawing.Point(13, 171);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 127);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Şarkı Listeleme";
            // 
            // btnTureGoreListele
            // 
            this.btnTureGoreListele.Location = new System.Drawing.Point(12, 91);
            this.btnTureGoreListele.Name = "btnTureGoreListele";
            this.btnTureGoreListele.Size = new System.Drawing.Size(176, 30);
            this.btnTureGoreListele.TabIndex = 2;
            this.btnTureGoreListele.Text = "Tüm Sanatçıları Listele";
            this.btnTureGoreListele.UseVisualStyleBackColor = true;
            this.btnTureGoreListele.Click += new System.EventHandler(this.btnTureGoreListele_Click);
            // 
            // btnSanatcilariListele
            // 
            this.btnSanatcilariListele.Location = new System.Drawing.Point(12, 55);
            this.btnSanatcilariListele.Name = "btnSanatcilariListele";
            this.btnSanatcilariListele.Size = new System.Drawing.Size(176, 30);
            this.btnSanatcilariListele.TabIndex = 1;
            this.btnSanatcilariListele.Text = "Tüm Türleri Listele";
            this.btnSanatcilariListele.UseVisualStyleBackColor = true;
            this.btnSanatcilariListele.Click += new System.EventHandler(this.btnSanatcilariListele_Click);
            // 
            // btnTumSarkilar
            // 
            this.btnTumSarkilar.Location = new System.Drawing.Point(12, 21);
            this.btnTumSarkilar.Name = "btnTumSarkilar";
            this.btnTumSarkilar.Size = new System.Drawing.Size(176, 30);
            this.btnTumSarkilar.TabIndex = 0;
            this.btnTumSarkilar.Text = "Tüm Şarkıları Listele";
            this.btnTumSarkilar.UseVisualStyleBackColor = true;
            this.btnTumSarkilar.Click += new System.EventHandler(this.btnTumSarkilar_Click);
            // 
            // btnTemizle
            // 
            this.btnTemizle.Location = new System.Drawing.Point(13, 496);
            this.btnTemizle.Name = "btnTemizle";
            this.btnTemizle.Size = new System.Drawing.Size(201, 23);
            this.btnTemizle.TabIndex = 6;
            this.btnTemizle.Text = "Temizle";
            this.btnTemizle.UseVisualStyleBackColor = true;
            this.btnTemizle.Click += new System.EventHandler(this.btnTemizle_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 531);
            this.Controls.Add(this.btnTemizle);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgv);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kullanıcı Arayüzü";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSanatciAdi;
        private System.Windows.Forms.TextBox txtSarkiAdi;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnTureGoreListele;
        private System.Windows.Forms.Button btnSanatcilariListele;
        private System.Windows.Forms.Button btnTumSarkilar;
        private System.Windows.Forms.ComboBox cmbTur;
        private System.Windows.Forms.Button btnTemizle;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}