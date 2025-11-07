using BmsMarkeRGeniusIntegrationCfg;

namespace Integration.BmsMarkeRGeniusIntegrationCfg.Genius2Logo.Integration
{
    partial class Frm_PosEOD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_PosEOD));
            this.p_Top = new System.Windows.Forms.Panel();
            this.gb_OrderTypes = new System.Windows.Forms.GroupBox();
            this.ce_WithoutControl = new DevExpress.XtraEditors.CheckEdit();
            this.ce_DontDebtClose = new DevExpress.XtraEditors.CheckEdit();
            this.ce_OnlyPayments = new DevExpress.XtraEditors.CheckEdit();
            this.ccbe_Branch = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.ce_OnlySalesWithCustomer = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.le_ReturnClient = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.le_InvoiceClient = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.de_DateEnd = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.de_DateStart = new DevExpress.XtraEditors.DateEdit();
            this.p_Bottom = new System.Windows.Forms.Panel();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.sb_SaveToBm = new DevExpress.XtraEditors.SimpleButton();
            this.p_Center = new System.Windows.Forms.Panel();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.p_Top.SuspendLayout();
            this.gb_OrderTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ce_WithoutControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_DontDebtClose.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_OnlyPayments.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ccbe_Branch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_OnlySalesWithCustomer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_ReturnClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_InvoiceClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_DateEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_DateEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_DateStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_DateStart.Properties.CalendarTimeProperties)).BeginInit();
            this.p_Bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // p_Top
            // 
            this.p_Top.Controls.Add(this.gb_OrderTypes);
            this.p_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.p_Top.Location = new System.Drawing.Point(0, 0);
            this.p_Top.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.p_Top.Name = "p_Top";
            this.p_Top.Size = new System.Drawing.Size(872, 414);
            this.p_Top.TabIndex = 0;
            // 
            // gb_OrderTypes
            // 
            this.gb_OrderTypes.Controls.Add(this.ce_WithoutControl);
            this.gb_OrderTypes.Controls.Add(this.ce_DontDebtClose);
            this.gb_OrderTypes.Controls.Add(this.ce_OnlyPayments);
            this.gb_OrderTypes.Controls.Add(this.ccbe_Branch);
            this.gb_OrderTypes.Controls.Add(this.ce_OnlySalesWithCustomer);
            this.gb_OrderTypes.Controls.Add(this.labelControl5);
            this.gb_OrderTypes.Controls.Add(this.le_ReturnClient);
            this.gb_OrderTypes.Controls.Add(this.labelControl4);
            this.gb_OrderTypes.Controls.Add(this.le_InvoiceClient);
            this.gb_OrderTypes.Controls.Add(this.labelControl3);
            this.gb_OrderTypes.Controls.Add(this.labelControl2);
            this.gb_OrderTypes.Controls.Add(this.de_DateEnd);
            this.gb_OrderTypes.Controls.Add(this.labelControl1);
            this.gb_OrderTypes.Controls.Add(this.de_DateStart);
            this.gb_OrderTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_OrderTypes.Location = new System.Drawing.Point(0, 0);
            this.gb_OrderTypes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gb_OrderTypes.Name = "gb_OrderTypes";
            this.gb_OrderTypes.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gb_OrderTypes.Size = new System.Drawing.Size(872, 414);
            this.gb_OrderTypes.TabIndex = 0;
            this.gb_OrderTypes.TabStop = false;
            // 
            // ce_WithoutControl
            // 
            this.ce_WithoutControl.Location = new System.Drawing.Point(208, 384);
            this.ce_WithoutControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ce_WithoutControl.Name = "ce_WithoutControl";
            this.ce_WithoutControl.Properties.Caption = "Kontrolsüz At";
            this.ce_WithoutControl.Size = new System.Drawing.Size(339, 27);
            this.ce_WithoutControl.TabIndex = 18;
            this.ce_WithoutControl.Visible = false;
            // 
            // ce_DontDebtClose
            // 
            this.ce_DontDebtClose.Location = new System.Drawing.Point(208, 345);
            this.ce_DontDebtClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ce_DontDebtClose.Name = "ce_DontDebtClose";
            this.ce_DontDebtClose.Properties.Caption = "Borç Takip Yapılmasın";
            this.ce_DontDebtClose.Size = new System.Drawing.Size(339, 27);
            this.ce_DontDebtClose.TabIndex = 17;
            // 
            // ce_OnlyPayments
            // 
            this.ce_OnlyPayments.Location = new System.Drawing.Point(208, 307);
            this.ce_OnlyPayments.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ce_OnlyPayments.Name = "ce_OnlyPayments";
            this.ce_OnlyPayments.Properties.Caption = "Sadece Tahsilatlar";
            this.ce_OnlyPayments.Size = new System.Drawing.Size(339, 27);
            this.ce_OnlyPayments.TabIndex = 15;
            this.ce_OnlyPayments.CheckedChanged += new System.EventHandler(this.ce_OnlyPayments_CheckedChanged);
            // 
            // ccbe_Branch
            // 
            this.ccbe_Branch.Location = new System.Drawing.Point(208, 210);
            this.ccbe_Branch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ccbe_Branch.Name = "ccbe_Branch";
            this.ccbe_Branch.Properties.AllowMultiSelect = true;
            this.ccbe_Branch.Properties.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ccbe_Branch.Properties.Appearance.Options.UseFont = true;
            this.ccbe_Branch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ccbe_Branch.Properties.NullText = "[İşyeri Seçiniz]";
            this.ccbe_Branch.Size = new System.Drawing.Size(339, 40);
            this.ccbe_Branch.TabIndex = 4;
            // 
            // ce_OnlySalesWithCustomer
            // 
            this.ce_OnlySalesWithCustomer.Location = new System.Drawing.Point(208, 269);
            this.ce_OnlySalesWithCustomer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ce_OnlySalesWithCustomer.Name = "ce_OnlySalesWithCustomer";
            this.ce_OnlySalesWithCustomer.Properties.Caption = "Sadece Carili Satışlar";
            this.ce_OnlySalesWithCustomer.Size = new System.Drawing.Size(339, 27);
            this.ce_OnlySalesWithCustomer.TabIndex = 5;
            this.ce_OnlySalesWithCustomer.CheckedChanged += new System.EventHandler(this.ce_OnlySalesWithCustomer_CheckedChanged);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(4, 167);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(119, 33);
            this.labelControl5.TabIndex = 14;
            this.labelControl5.Text = "İade Cari";
            // 
            // le_ReturnClient
            // 
            this.le_ReturnClient.Location = new System.Drawing.Point(208, 162);
            this.le_ReturnClient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.le_ReturnClient.Name = "le_ReturnClient";
            this.le_ReturnClient.Properties.Appearance.BorderColor = System.Drawing.Color.Blue;
            this.le_ReturnClient.Properties.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.le_ReturnClient.Properties.Appearance.Options.UseBorderColor = true;
            this.le_ReturnClient.Properties.Appearance.Options.UseFont = true;
            this.le_ReturnClient.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.le_ReturnClient.Properties.NullText = "[Cari Seçiniz]";
            this.le_ReturnClient.Size = new System.Drawing.Size(339, 40);
            this.le_ReturnClient.TabIndex = 3;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(4, 114);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(148, 33);
            this.labelControl4.TabIndex = 12;
            this.labelControl4.Text = "Fatura Cari";
            // 
            // le_InvoiceClient
            // 
            this.le_InvoiceClient.Location = new System.Drawing.Point(208, 110);
            this.le_InvoiceClient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.le_InvoiceClient.Name = "le_InvoiceClient";
            this.le_InvoiceClient.Properties.Appearance.BorderColor = System.Drawing.Color.Blue;
            this.le_InvoiceClient.Properties.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.le_InvoiceClient.Properties.Appearance.Options.UseBorderColor = true;
            this.le_InvoiceClient.Properties.Appearance.Options.UseFont = true;
            this.le_InvoiceClient.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.le_InvoiceClient.Properties.NullText = "[Cari Seçiniz]";
            this.le_InvoiceClient.Size = new System.Drawing.Size(339, 40);
            this.le_InvoiceClient.TabIndex = 2;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(4, 221);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(74, 33);
            this.labelControl3.TabIndex = 10;
            this.labelControl3.Text = "İşyeri";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(4, 67);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(125, 33);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "Tarih Son";
            // 
            // de_DateEnd
            // 
            this.de_DateEnd.EditValue = null;
            this.de_DateEnd.Location = new System.Drawing.Point(208, 57);
            this.de_DateEnd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.de_DateEnd.Name = "de_DateEnd";
            this.de_DateEnd.Properties.Appearance.BorderColor = System.Drawing.Color.Blue;
            this.de_DateEnd.Properties.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.de_DateEnd.Properties.Appearance.Options.UseBorderColor = true;
            this.de_DateEnd.Properties.Appearance.Options.UseFont = true;
            this.de_DateEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.de_DateEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.de_DateEnd.Size = new System.Drawing.Size(339, 40);
            this.de_DateEnd.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(4, 15);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(126, 33);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Tarih Baş";
            // 
            // de_DateStart
            // 
            this.de_DateStart.EditValue = null;
            this.de_DateStart.Location = new System.Drawing.Point(208, 4);
            this.de_DateStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.de_DateStart.Name = "de_DateStart";
            this.de_DateStart.Properties.Appearance.BorderColor = System.Drawing.Color.Blue;
            this.de_DateStart.Properties.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.de_DateStart.Properties.Appearance.Options.UseBorderColor = true;
            this.de_DateStart.Properties.Appearance.Options.UseFont = true;
            this.de_DateStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.de_DateStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.de_DateStart.Size = new System.Drawing.Size(339, 40);
            this.de_DateStart.TabIndex = 0;
            // 
            // p_Bottom
            // 
            this.p_Bottom.Controls.Add(this.simpleButton2);
            this.p_Bottom.Controls.Add(this.simpleButton1);
            this.p_Bottom.Controls.Add(this.sb_SaveToBm);
            this.p_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.p_Bottom.Location = new System.Drawing.Point(0, 440);
            this.p_Bottom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.p_Bottom.Name = "p_Bottom";
            this.p_Bottom.Size = new System.Drawing.Size(872, 127);
            this.p_Bottom.TabIndex = 1;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(362, 0);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(255, 127);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = " Kaydet";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // sb_SaveToBm
            // 
            this.sb_SaveToBm.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.sb_SaveToBm.Appearance.Options.UseFont = true;
            this.sb_SaveToBm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sb_SaveToBm.Dock = System.Windows.Forms.DockStyle.Right;
            this.sb_SaveToBm.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("sb_SaveToBm.ImageOptions.Image")));
            this.sb_SaveToBm.Location = new System.Drawing.Point(617, 0);
            this.sb_SaveToBm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sb_SaveToBm.Name = "sb_SaveToBm";
            this.sb_SaveToBm.Size = new System.Drawing.Size(255, 127);
            this.sb_SaveToBm.TabIndex = 1;
            this.sb_SaveToBm.Text = "Vazgeç";
            this.sb_SaveToBm.Click += new System.EventHandler(this.sb_SaveToBm_Click);
            // 
            // p_Center
            // 
            this.p_Center.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_Center.Location = new System.Drawing.Point(0, 414);
            this.p_Center.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.p_Center.Name = "p_Center";
            this.p_Center.Size = new System.Drawing.Size(872, 26);
            this.p_Center.TabIndex = 1;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.ImageOptions.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(107, 0);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(255, 127);
            this.simpleButton2.TabIndex = 2;
            this.simpleButton2.Text = "NCR Kaydet";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_ClickAsync);
            // 
            // Frm_PosEOD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 567);
            this.Controls.Add(this.p_Center);
            this.Controls.Add(this.p_Bottom);
            this.Controls.Add(this.p_Top);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Frm_PosEOD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.p_Top.ResumeLayout(false);
            this.gb_OrderTypes.ResumeLayout(false);
            this.gb_OrderTypes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ce_WithoutControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_DontDebtClose.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_OnlyPayments.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ccbe_Branch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_OnlySalesWithCustomer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_ReturnClient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_InvoiceClient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_DateEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_DateEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_DateStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_DateStart.Properties)).EndInit();
            this.p_Bottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel p_Top;
        private System.Windows.Forms.Panel p_Bottom;
        private System.Windows.Forms.GroupBox gb_OrderTypes;
        private System.Windows.Forms.Panel p_Center;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit de_DateStart;
        private DevExpress.XtraEditors.SimpleButton sb_SaveToBm;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit de_DateEnd;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.CheckEdit ce_OnlySalesWithCustomer;
        private DevExpress.XtraEditors.CheckedComboBoxEdit ccbe_Branch;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LookUpEdit le_ReturnClient;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LookUpEdit le_InvoiceClient;
        private DevExpress.XtraEditors.CheckEdit ce_OnlyPayments;
        private DevExpress.XtraEditors.CheckEdit ce_DontDebtClose;
        private DevExpress.XtraEditors.CheckEdit ce_WithoutControl;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}