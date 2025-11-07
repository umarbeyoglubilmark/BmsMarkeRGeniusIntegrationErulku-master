namespace BmsMarkeRGeniusIntegrationCfg
{
    partial class FRM_MAIN
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRM_MAIN));
            this.accordionControl1 = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.accordionControlElement1 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Admin = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_DBSettings = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_CreateUpdateTableView = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Genius2Logo = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Genius2Logo_Definition = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_LogoPosBranchMapping = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_PaymentMapping = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Default = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Genius2Logo_Integration = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_PosEODIntegration = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_PosEODCancel = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Genius2Logo_Control = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Control_Client = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Control_Items = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Control_Salesman = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Logo2Genius = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Logo2Genius_Definition = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Logo2Genius_Definition_IbmKasa = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Logo2Genius_Integration = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Logo2Genius_Integration_Material = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ace_Logo2Genius_Integration_Client = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.fluentDesignFormControl1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl();
            this.spdbi_ThemePallete = new DevExpress.XtraBars.SkinPaletteDropDownButtonItem();
            this.sbsi_Theme = new DevExpress.XtraBars.SkinBarSubItem();
            this.fluentFormDefaultManager1 = new DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager(this.components);
            this.xtmm_Mdi = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtmm_Mdi)).BeginInit();
            this.SuspendLayout();
            // 
            // accordionControl1
            // 
            this.accordionControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.accordionControl1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlElement1});
            this.accordionControl1.Location = new System.Drawing.Point(0, 55);
            this.accordionControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.accordionControl1.Name = "accordionControl1";
            this.accordionControl1.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Touch;
            this.accordionControl1.Size = new System.Drawing.Size(493, 1051);
            this.accordionControl1.TabIndex = 1;
            this.accordionControl1.ViewType = DevExpress.XtraBars.Navigation.AccordionControlViewType.HamburgerMenu;
            // 
            // accordionControlElement1
            // 
            this.accordionControlElement1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.ace_Admin,
            this.ace_Genius2Logo,
            this.ace_Logo2Genius});
            this.accordionControlElement1.Expanded = true;
            this.accordionControlElement1.Name = "accordionControlElement1";
            this.accordionControlElement1.Text = "Menu";
            // 
            // ace_Admin
            // 
            this.ace_Admin.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.ace_DBSettings,
            this.ace_CreateUpdateTableView});
            this.ace_Admin.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Admin.ImageOptions.Image")));
            this.ace_Admin.Name = "ace_Admin";
            this.ace_Admin.Text = "Sistem";
            // 
            // ace_DBSettings
            // 
            this.ace_DBSettings.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ace_DBSettings.ImageOptions.SvgImage")));
            this.ace_DBSettings.Name = "ace_DBSettings";
            this.ace_DBSettings.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_DBSettings.Text = "Veritabanı Ayarları";
            this.ace_DBSettings.Click += new System.EventHandler(this.ace_DBSettings_Click);
            // 
            // ace_CreateUpdateTableView
            // 
            this.ace_CreateUpdateTableView.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ace_CreateUpdateTableView.ImageOptions.SvgImage")));
            this.ace_CreateUpdateTableView.Name = "ace_CreateUpdateTableView";
            this.ace_CreateUpdateTableView.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_CreateUpdateTableView.Text = "Tabloları Oluştur/Güncelle";
            this.ace_CreateUpdateTableView.Click += new System.EventHandler(this.ace_CreateUpdateTableView_Click);
            // 
            // ace_Genius2Logo
            // 
            this.ace_Genius2Logo.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.ace_Genius2Logo_Definition,
            this.ace_Genius2Logo_Integration,
            this.ace_Genius2Logo_Control});
            this.ace_Genius2Logo.Expanded = true;
            this.ace_Genius2Logo.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Genius2Logo.ImageOptions.Image")));
            this.ace_Genius2Logo.Name = "ace_Genius2Logo";
            this.ace_Genius2Logo.Text = "Logoya Aktarım";
            // 
            // ace_Genius2Logo_Definition
            // 
            this.ace_Genius2Logo_Definition.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.ace_LogoPosBranchMapping,
            this.ace_PaymentMapping,
            this.ace_Default});
            this.ace_Genius2Logo_Definition.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Genius2Logo_Definition.ImageOptions.Image")));
            this.ace_Genius2Logo_Definition.Name = "ace_Genius2Logo_Definition";
            this.ace_Genius2Logo_Definition.Text = "Tanımlamalar";
            // 
            // ace_LogoPosBranchMapping
            // 
            this.ace_LogoPosBranchMapping.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_LogoPosBranchMapping.ImageOptions.Image")));
            this.ace_LogoPosBranchMapping.Name = "ace_LogoPosBranchMapping";
            this.ace_LogoPosBranchMapping.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_LogoPosBranchMapping.Text = "Logo Pos İşyeri Tanımlamaları";
            this.ace_LogoPosBranchMapping.Click += new System.EventHandler(this.ace_LogoPosBranchMapping_Click);
            // 
            // ace_PaymentMapping
            // 
            this.ace_PaymentMapping.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_PaymentMapping.ImageOptions.Image")));
            this.ace_PaymentMapping.Name = "ace_PaymentMapping";
            this.ace_PaymentMapping.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_PaymentMapping.Text = "Tahsilat Eşleştirme Tanımlamaları";
            this.ace_PaymentMapping.Click += new System.EventHandler(this.ace_PaymentMapping_Click);
            // 
            // ace_Default
            // 
            this.ace_Default.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Default.ImageOptions.Image")));
            this.ace_Default.Name = "ace_Default";
            this.ace_Default.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_Default.Text = "Varsayılanlar";
            this.ace_Default.Click += new System.EventHandler(this.ace_Default_Click);
            // 
            // ace_Genius2Logo_Integration
            // 
            this.ace_Genius2Logo_Integration.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.ace_PosEODIntegration,
            this.ace_PosEODCancel});
            this.ace_Genius2Logo_Integration.Expanded = true;
            this.ace_Genius2Logo_Integration.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Genius2Logo_Integration.ImageOptions.Image")));
            this.ace_Genius2Logo_Integration.Name = "ace_Genius2Logo_Integration";
            this.ace_Genius2Logo_Integration.Text = "Entegrasyon";
            // 
            // ace_PosEODIntegration
            // 
            this.ace_PosEODIntegration.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_PosEODIntegration.ImageOptions.Image")));
            this.ace_PosEODIntegration.Name = "ace_PosEODIntegration";
            this.ace_PosEODIntegration.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_PosEODIntegration.Text = "POS Günsonu İşlemi";
            this.ace_PosEODIntegration.Click += new System.EventHandler(this.ace_PosEODIntegration_Click);
            // 
            // ace_PosEODCancel
            // 
            this.ace_PosEODCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_PosEODCancel.ImageOptions.Image")));
            this.ace_PosEODCancel.Name = "ace_PosEODCancel";
            this.ace_PosEODCancel.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_PosEODCancel.Text = "POS Günsonu İptal İşlemi";
            this.ace_PosEODCancel.Click += new System.EventHandler(this.ace_PosEODCancel_Click);
            // 
            // ace_Genius2Logo_Control
            // 
            this.ace_Genius2Logo_Control.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.ace_Control_Client,
            this.ace_Control_Items,
            this.ace_Control_Salesman});
            this.ace_Genius2Logo_Control.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Genius2Logo_Control.ImageOptions.Image")));
            this.ace_Genius2Logo_Control.Name = "ace_Genius2Logo_Control";
            this.ace_Genius2Logo_Control.Text = "Kontroller";
            // 
            // ace_Control_Client
            // 
            this.ace_Control_Client.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Control_Client.ImageOptions.Image")));
            this.ace_Control_Client.Name = "ace_Control_Client";
            this.ace_Control_Client.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_Control_Client.Text = "Cariler-Logoda Olmayanlar";
            this.ace_Control_Client.Click += new System.EventHandler(this.ace_Control_Client_Click);
            // 
            // ace_Control_Items
            // 
            this.ace_Control_Items.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Control_Items.ImageOptions.Image")));
            this.ace_Control_Items.Name = "ace_Control_Items";
            this.ace_Control_Items.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_Control_Items.Text = "Urunler-Logoda Olmayanlar";
            this.ace_Control_Items.Click += new System.EventHandler(this.ace_Control_Items_Click);
            // 
            // ace_Control_Salesman
            // 
            this.ace_Control_Salesman.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Control_Salesman.ImageOptions.Image")));
            this.ace_Control_Salesman.Name = "ace_Control_Salesman";
            this.ace_Control_Salesman.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_Control_Salesman.Text = "Satış Elemanları-Logoda Olmayanlar";
            this.ace_Control_Salesman.Click += new System.EventHandler(this.ace_Control_Salesman_Click);
            // 
            // ace_Logo2Genius
            // 
            this.ace_Logo2Genius.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.ace_Logo2Genius_Definition,
            this.ace_Logo2Genius_Integration});
            this.ace_Logo2Genius.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Logo2Genius.ImageOptions.Image")));
            this.ace_Logo2Genius.Name = "ace_Logo2Genius";
            this.ace_Logo2Genius.Text = "IbmGeniusa Aktarım";
            // 
            // ace_Logo2Genius_Definition
            // 
            this.ace_Logo2Genius_Definition.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.ace_Logo2Genius_Definition_IbmKasa});
            this.ace_Logo2Genius_Definition.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Logo2Genius_Definition.ImageOptions.Image")));
            this.ace_Logo2Genius_Definition.Name = "ace_Logo2Genius_Definition";
            this.ace_Logo2Genius_Definition.Text = "Tanımlamalar";
            // 
            // ace_Logo2Genius_Definition_IbmKasa
            // 
            this.ace_Logo2Genius_Definition_IbmKasa.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Logo2Genius_Definition_IbmKasa.ImageOptions.Image")));
            this.ace_Logo2Genius_Definition_IbmKasa.Name = "ace_Logo2Genius_Definition_IbmKasa";
            this.ace_Logo2Genius_Definition_IbmKasa.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_Logo2Genius_Definition_IbmKasa.Text = "Ibm Kasa";
            this.ace_Logo2Genius_Definition_IbmKasa.Click += new System.EventHandler(this.ace_Logo2Genius_Definition_IbmKasa_Click);
            // 
            // ace_Logo2Genius_Integration
            // 
            this.ace_Logo2Genius_Integration.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.ace_Logo2Genius_Integration_Material,
            this.ace_Logo2Genius_Integration_Client});
            this.ace_Logo2Genius_Integration.Expanded = true;
            this.ace_Logo2Genius_Integration.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Logo2Genius_Integration.ImageOptions.Image")));
            this.ace_Logo2Genius_Integration.Name = "ace_Logo2Genius_Integration";
            this.ace_Logo2Genius_Integration.Text = "Aktarım";
            // 
            // ace_Logo2Genius_Integration_Material
            // 
            this.ace_Logo2Genius_Integration_Material.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Logo2Genius_Integration_Material.ImageOptions.Image")));
            this.ace_Logo2Genius_Integration_Material.Name = "ace_Logo2Genius_Integration_Material";
            this.ace_Logo2Genius_Integration_Material.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_Logo2Genius_Integration_Material.Text = "Malzeme";
            this.ace_Logo2Genius_Integration_Material.Click += new System.EventHandler(this.ace_Logo2Genius_Integration_Material_Click);
            // 
            // ace_Logo2Genius_Integration_Client
            // 
            this.ace_Logo2Genius_Integration_Client.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ace_Logo2Genius_Integration_Client.ImageOptions.Image")));
            this.ace_Logo2Genius_Integration_Client.Name = "ace_Logo2Genius_Integration_Client";
            this.ace_Logo2Genius_Integration_Client.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ace_Logo2Genius_Integration_Client.Text = "Cari";
            this.ace_Logo2Genius_Integration_Client.Click += new System.EventHandler(this.ace_Logo2Genius_Integration_Client_Click);
            // 
            // fluentDesignFormControl1
            // 
            this.fluentDesignFormControl1.FluentDesignForm = this;
            this.fluentDesignFormControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.spdbi_ThemePallete,
            this.sbsi_Theme});
            this.fluentDesignFormControl1.Location = new System.Drawing.Point(0, 0);
            this.fluentDesignFormControl1.Manager = this.fluentFormDefaultManager1;
            this.fluentDesignFormControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fluentDesignFormControl1.Name = "fluentDesignFormControl1";
            this.fluentDesignFormControl1.Size = new System.Drawing.Size(1640, 55);
            this.fluentDesignFormControl1.TabIndex = 2;
            this.fluentDesignFormControl1.TabStop = false;
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.sbsi_Theme);
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.spdbi_ThemePallete);
            // 
            // spdbi_ThemePallete
            // 
            this.spdbi_ThemePallete.Id = 0;
            this.spdbi_ThemePallete.Name = "spdbi_ThemePallete";
            // 
            // sbsi_Theme
            // 
            this.sbsi_Theme.Caption = "Tema";
            this.sbsi_Theme.Id = 1;
            this.sbsi_Theme.Name = "sbsi_Theme";
            // 
            // fluentFormDefaultManager1
            // 
            this.fluentFormDefaultManager1.Form = this;
            this.fluentFormDefaultManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.spdbi_ThemePallete,
            this.sbsi_Theme});
            this.fluentFormDefaultManager1.MaxItemId = 2;
            // 
            // xtmm_Mdi
            // 
            this.xtmm_Mdi.MdiParent = this;
            // 
            // FRM_MAIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1640, 1106);
            this.Controls.Add(this.accordionControl1);
            this.Controls.Add(this.fluentDesignFormControl1);
            this.FluentDesignFormControl = this.fluentDesignFormControl1;
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("FRM_MAIN.IconOptions.SvgImage")));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "FRM_MAIN";
            this.NavigationControl = this.accordionControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.SurfaceMaterial = DevExpress.XtraEditors.SurfaceMaterial.None;
            this.Text = "BmsMarkeRGeniusIntegration";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FRM_MAIN_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtmm_Mdi)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.Navigation.AccordionControl accordionControl1;
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl fluentDesignFormControl1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement1;
        private DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager fluentFormDefaultManager1;
        private DevExpress.XtraBars.SkinPaletteDropDownButtonItem spdbi_ThemePallete;
        private DevExpress.XtraBars.SkinBarSubItem sbsi_Theme;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_DBSettings;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_CreateUpdateTableView;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtmm_Mdi;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Admin;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Genius2Logo_Integration;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_PosEODIntegration;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_PosEODCancel;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Genius2Logo_Definition;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_LogoPosBranchMapping;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Default;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_PaymentMapping;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Genius2Logo_Control;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Control_Client;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Control_Items;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Control_Salesman;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Genius2Logo;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Logo2Genius;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Logo2Genius_Definition;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Logo2Genius_Definition_IbmKasa;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Logo2Genius_Integration;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Logo2Genius_Integration_Material;
        private DevExpress.XtraBars.Navigation.AccordionControlElement ace_Logo2Genius_Integration_Client;
    }
}

