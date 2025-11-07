namespace BmsMarkeRGeniusIntegrationCfg
{
    partial class FRM_LOGIN
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRM_LOGIN));
            this.pe_Logo = new DevExpress.XtraEditors.PictureEdit();
            this.te_Username = new DevExpress.XtraEditors.TextEdit();
            this.te_Password = new DevExpress.XtraEditors.TextEdit();
            this.sb_Login = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pe_Logo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_Username.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_Password.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pe_Logo
            // 
            this.pe_Logo.Location = new System.Drawing.Point(12, 12);
            this.pe_Logo.Name = "pe_Logo";
            this.pe_Logo.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pe_Logo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pe_Logo.Size = new System.Drawing.Size(169, 164);
            this.pe_Logo.TabIndex = 0;
            // 
            // te_Username
            // 
            this.te_Username.Location = new System.Drawing.Point(277, 41);
            this.te_Username.Name = "te_Username";
            this.te_Username.Size = new System.Drawing.Size(159, 20);
            this.te_Username.TabIndex = 1;
            // 
            // te_Password
            // 
            this.te_Password.Location = new System.Drawing.Point(277, 67);
            this.te_Password.Name = "te_Password";
            this.te_Password.Properties.UseSystemPasswordChar = true;
            this.te_Password.Size = new System.Drawing.Size(159, 20);
            this.te_Password.TabIndex = 2;
            // 
            // sb_Login
            // 
            this.sb_Login.Location = new System.Drawing.Point(277, 119);
            this.sb_Login.Name = "sb_Login";
            this.sb_Login.Size = new System.Drawing.Size(159, 23);
            this.sb_Login.TabIndex = 4;
            this.sb_Login.Text = "Giriş";
            this.sb_Login.Click += new System.EventHandler(this.sb_Login_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Parola";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(209, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Kullanıcı Adı";
            // 
            // FRM_LOGIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 188);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sb_Login);
            this.Controls.Add(this.te_Password);
            this.Controls.Add(this.te_Username);
            this.Controls.Add(this.pe_Logo);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("FRM_LOGIN.IconOptions.SvgImage")));
            this.Name = "FRM_LOGIN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GİRİŞ";
            ((System.ComponentModel.ISupportInitialize)(this.pe_Logo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_Username.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_Password.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pe_Logo;
        private DevExpress.XtraEditors.TextEdit te_Username;
        private DevExpress.XtraEditors.TextEdit te_Password;
        private DevExpress.XtraEditors.SimpleButton sb_Login;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}