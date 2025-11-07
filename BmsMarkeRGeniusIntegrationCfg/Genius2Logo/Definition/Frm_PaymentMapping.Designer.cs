namespace BmsMarkeRGeniusIntegrationCfg.Genius2Logo.Definition
{
    partial class Frm_PaymentMapping
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_PaymentMapping));
            this.p_Top = new System.Windows.Forms.Panel();
            this.gb_OrderTypes = new System.Windows.Forms.GroupBox();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.te_Saleman = new DevExpress.XtraEditors.TextEdit();
            this.lc_KKBKK = new DevExpress.XtraEditors.LabelControl();
            this.te_BankOrKsCode = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.sb_AddRow = new DevExpress.XtraEditors.SimpleButton();
            this.l_LogoField = new DevExpress.XtraEditors.LabelControl();
            this.te_IntegrationCode = new DevExpress.XtraEditors.TextEdit();
            this.cbe_LogoFicheType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbe_Currency = new DevExpress.XtraEditors.ComboBoxEdit();
            this.p_Bottom = new System.Windows.Forms.Panel();
            this.sb_SaveToBM = new DevExpress.XtraEditors.SimpleButton();
            this.p_Center = new System.Windows.Forms.Panel();
            this.grc_Mapping = new DevExpress.XtraGrid.GridControl();
            this.grv_Mapping = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.p_Top.SuspendLayout();
            this.gb_OrderTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.te_Saleman.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_BankOrKsCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_IntegrationCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbe_LogoFicheType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbe_Currency.Properties)).BeginInit();
            this.p_Bottom.SuspendLayout();
            this.p_Center.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grc_Mapping)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grv_Mapping)).BeginInit();
            this.SuspendLayout();
            // 
            // p_Top
            // 
            this.p_Top.Controls.Add(this.gb_OrderTypes);
            this.p_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.p_Top.Location = new System.Drawing.Point(0, 0);
            this.p_Top.Name = "p_Top";
            this.p_Top.Size = new System.Drawing.Size(763, 180);
            this.p_Top.TabIndex = 0;
            // 
            // gb_OrderTypes
            // 
            this.gb_OrderTypes.Controls.Add(this.labelControl4);
            this.gb_OrderTypes.Controls.Add(this.te_Saleman);
            this.gb_OrderTypes.Controls.Add(this.lc_KKBKK);
            this.gb_OrderTypes.Controls.Add(this.te_BankOrKsCode);
            this.gb_OrderTypes.Controls.Add(this.labelControl2);
            this.gb_OrderTypes.Controls.Add(this.labelControl1);
            this.gb_OrderTypes.Controls.Add(this.sb_AddRow);
            this.gb_OrderTypes.Controls.Add(this.l_LogoField);
            this.gb_OrderTypes.Controls.Add(this.te_IntegrationCode);
            this.gb_OrderTypes.Controls.Add(this.cbe_LogoFicheType);
            this.gb_OrderTypes.Controls.Add(this.cbe_Currency);
            this.gb_OrderTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_OrderTypes.Location = new System.Drawing.Point(0, 0);
            this.gb_OrderTypes.Name = "gb_OrderTypes";
            this.gb_OrderTypes.Size = new System.Drawing.Size(763, 180);
            this.gb_OrderTypes.TabIndex = 0;
            this.gb_OrderTypes.TabStop = false;
            this.gb_OrderTypes.Text = "Tahsilat Entegrasyon Eşleştirme";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(6, 55);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(93, 13);
            this.labelControl4.TabIndex = 37;
            this.labelControl4.Text = "Satış Elemanı Kodu:";
            // 
            // te_Saleman
            // 
            this.te_Saleman.EditValue = "";
            this.te_Saleman.Location = new System.Drawing.Point(182, 49);
            this.te_Saleman.Name = "te_Saleman";
            this.te_Saleman.Properties.NullText = "[EditValue is null]";
            this.te_Saleman.Size = new System.Drawing.Size(138, 20);
            this.te_Saleman.TabIndex = 1;
            // 
            // lc_KKBKK
            // 
            this.lc_KKBKK.Location = new System.Drawing.Point(6, 119);
            this.lc_KKBKK.Name = "lc_KKBKK";
            this.lc_KKBKK.Size = new System.Drawing.Size(173, 13);
            this.lc_KKBKK.TabIndex = 35;
            this.lc_KKBKK.Text = "Kredi Karti Hesap Kodu / Kasa Kodu:";
            // 
            // te_BankOrKsCode
            // 
            this.te_BankOrKsCode.EditValue = "";
            this.te_BankOrKsCode.Location = new System.Drawing.Point(182, 115);
            this.te_BankOrKsCode.Name = "te_BankOrKsCode";
            this.te_BankOrKsCode.Properties.NullText = "[EditValue is null]";
            this.te_BankOrKsCode.Properties.ReadOnly = true;
            this.te_BankOrKsCode.Size = new System.Drawing.Size(138, 20);
            this.te_BankOrKsCode.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(6, 77);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(135, 13);
            this.labelControl2.TabIndex = 33;
            this.labelControl2.Text = "Logoya Aktarılacak Fiş Türü:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(6, 97);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(55, 13);
            this.labelControl1.TabIndex = 26;
            this.labelControl1.Text = "Döviz Cinsi:";
            // 
            // sb_AddRow
            // 
            this.sb_AddRow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sb_AddRow.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("sb_AddRow.ImageOptions.Image")));
            this.sb_AddRow.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.sb_AddRow.Location = new System.Drawing.Point(182, 141);
            this.sb_AddRow.Name = "sb_AddRow";
            this.sb_AddRow.Size = new System.Drawing.Size(138, 29);
            this.sb_AddRow.TabIndex = 5;
            this.sb_AddRow.Text = "Ekle";
            this.sb_AddRow.Click += new System.EventHandler(this.sb_AddRow_Click);
            // 
            // l_LogoField
            // 
            this.l_LogoField.Location = new System.Drawing.Point(6, 33);
            this.l_LogoField.Name = "l_LogoField";
            this.l_LogoField.Size = new System.Drawing.Size(92, 13);
            this.l_LogoField.TabIndex = 20;
            this.l_LogoField.Text = "Entegrasyon Kodu:";
            // 
            // te_IntegrationCode
            // 
            this.te_IntegrationCode.EditValue = "";
            this.te_IntegrationCode.Location = new System.Drawing.Point(182, 27);
            this.te_IntegrationCode.Name = "te_IntegrationCode";
            this.te_IntegrationCode.Properties.NullText = "[EditValue is null]";
            this.te_IntegrationCode.Size = new System.Drawing.Size(138, 20);
            this.te_IntegrationCode.TabIndex = 0;
            // 
            // cbe_LogoFicheType
            // 
            this.cbe_LogoFicheType.EditValue = "";
            this.cbe_LogoFicheType.Location = new System.Drawing.Point(182, 71);
            this.cbe_LogoFicheType.Name = "cbe_LogoFicheType";
            this.cbe_LogoFicheType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbe_LogoFicheType.Properties.Items.AddRange(new object[] {
            "Cek Girisi",
            "CH Kredi Karti",
            "CH Kredi Karti Iade",
            "CH Borc",
            "CH Alacak",
            "Kasa Tahsilat",
            "Kasa Odeme",
            "Veresiye"});
            this.cbe_LogoFicheType.Properties.NullText = "[EditValue is null]";
            this.cbe_LogoFicheType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbe_LogoFicheType.Size = new System.Drawing.Size(138, 20);
            this.cbe_LogoFicheType.TabIndex = 2;
            this.cbe_LogoFicheType.SelectedIndexChanged += new System.EventHandler(this.te_LogoFicheType_SelectedIndexChanged);
            // 
            // cbe_Currency
            // 
            this.cbe_Currency.EditValue = "";
            this.cbe_Currency.Location = new System.Drawing.Point(182, 93);
            this.cbe_Currency.Name = "cbe_Currency";
            this.cbe_Currency.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbe_Currency.Properties.Items.AddRange(new object[] {
            "TL",
            "EUR",
            "GBP",
            "USD"});
            this.cbe_Currency.Properties.NullText = "[EditValue is null]";
            this.cbe_Currency.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbe_Currency.Size = new System.Drawing.Size(138, 20);
            this.cbe_Currency.TabIndex = 3;
            // 
            // p_Bottom
            // 
            this.p_Bottom.Controls.Add(this.sb_SaveToBM);
            this.p_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.p_Bottom.Location = new System.Drawing.Point(0, 516);
            this.p_Bottom.Name = "p_Bottom";
            this.p_Bottom.Size = new System.Drawing.Size(763, 100);
            this.p_Bottom.TabIndex = 1;
            // 
            // sb_SaveToBM
            // 
            this.sb_SaveToBM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sb_SaveToBM.Dock = System.Windows.Forms.DockStyle.Right;
            this.sb_SaveToBM.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.sb_SaveToBM.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sb_SaveToBM.ImageOptions.SvgImage")));
            this.sb_SaveToBM.Location = new System.Drawing.Point(612, 0);
            this.sb_SaveToBM.Name = "sb_SaveToBM";
            this.sb_SaveToBM.Size = new System.Drawing.Size(151, 100);
            this.sb_SaveToBM.TabIndex = 0;
            this.sb_SaveToBM.Text = "Kaydet";
            this.sb_SaveToBM.Click += new System.EventHandler(this.sb_SaveToBM_Click);
            // 
            // p_Center
            // 
            this.p_Center.Controls.Add(this.grc_Mapping);
            this.p_Center.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_Center.Location = new System.Drawing.Point(0, 180);
            this.p_Center.Name = "p_Center";
            this.p_Center.Size = new System.Drawing.Size(763, 336);
            this.p_Center.TabIndex = 1;
            // 
            // grc_Mapping
            // 
            this.grc_Mapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grc_Mapping.Location = new System.Drawing.Point(0, 0);
            this.grc_Mapping.MainView = this.grv_Mapping;
            this.grc_Mapping.Name = "grc_Mapping";
            this.grc_Mapping.Size = new System.Drawing.Size(763, 336);
            this.grc_Mapping.TabIndex = 0;
            this.grc_Mapping.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grv_Mapping});
            // 
            // grv_Mapping
            // 
            this.grv_Mapping.GridControl = this.grc_Mapping;
            this.grv_Mapping.Name = "grv_Mapping";
            this.grv_Mapping.OptionsBehavior.Editable = false;
            this.grv_Mapping.OptionsSelection.MultiSelect = true;
            this.grv_Mapping.OptionsView.ColumnAutoWidth = false;
            this.grv_Mapping.OptionsView.ShowFooter = true;
            this.grv_Mapping.OptionsView.ShowGroupPanel = false;
            this.grv_Mapping.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.grv_Invoices_PopupMenuShowing);
            // 
            // Frm_PaymentMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 616);
            this.Controls.Add(this.p_Center);
            this.Controls.Add(this.p_Bottom);
            this.Controls.Add(this.p_Top);
            this.Name = "Frm_PaymentMapping";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Frm_CRUDs_FormClosed);
            this.p_Top.ResumeLayout(false);
            this.gb_OrderTypes.ResumeLayout(false);
            this.gb_OrderTypes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.te_Saleman.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_BankOrKsCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_IntegrationCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbe_LogoFicheType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbe_Currency.Properties)).EndInit();
            this.p_Bottom.ResumeLayout(false);
            this.p_Center.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grc_Mapping)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grv_Mapping)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel p_Top;
        private System.Windows.Forms.Panel p_Bottom;
        private System.Windows.Forms.GroupBox gb_OrderTypes;
        private System.Windows.Forms.Panel p_Center;
        private DevExpress.XtraGrid.GridControl grc_Mapping;
        private DevExpress.XtraGrid.Views.Grid.GridView grv_Mapping;
        private DevExpress.XtraEditors.LabelControl l_LogoField;
        private DevExpress.XtraEditors.SimpleButton sb_SaveToBM;
        private DevExpress.XtraEditors.SimpleButton sb_AddRow;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit te_IntegrationCode;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl lc_KKBKK;
        private DevExpress.XtraEditors.TextEdit te_BankOrKsCode;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit te_Saleman;
        private DevExpress.XtraEditors.ComboBoxEdit cbe_LogoFicheType;
        private DevExpress.XtraEditors.ComboBoxEdit cbe_Currency;
    }
}