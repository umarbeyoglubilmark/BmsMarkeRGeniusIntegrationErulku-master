using BmsMarkeRGeniusIntegrationCfg;

namespace Integration.BmsMarkeRGeniusIntegrationCfg.Genius2Logo.Integration
{
    partial class Frm_PosEODCancel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_PosEODCancel));
            this.p_Top = new System.Windows.Forms.Panel();
            this.gb_OrderTypes = new System.Windows.Forms.GroupBox();
            this.ce_DontRollbackDebtClose = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.le_ReturnClient = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.le_InvoiceClient = new DevExpress.XtraEditors.LookUpEdit();
            this.ce_OnlyPayments = new DevExpress.XtraEditors.CheckEdit();
            this.ce_OnlySalesWithCustomer = new DevExpress.XtraEditors.CheckEdit();
            this.ce_AllPos = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.le_Pos = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.le_Branch = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.de_Date = new DevExpress.XtraEditors.DateEdit();
            this.p_Bottom = new System.Windows.Forms.Panel();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.sb_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.sb_SaveToBm = new DevExpress.XtraEditors.SimpleButton();
            this.p_Center = new System.Windows.Forms.Panel();
            this.p_Top.SuspendLayout();
            this.gb_OrderTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ce_DontRollbackDebtClose.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_ReturnClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_InvoiceClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_OnlyPayments.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_OnlySalesWithCustomer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_AllPos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_Pos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_Branch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_Date.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_Date.Properties.CalendarTimeProperties)).BeginInit();
            this.p_Bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // p_Top
            // 
            this.p_Top.Controls.Add(this.gb_OrderTypes);
            this.p_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.p_Top.Location = new System.Drawing.Point(0, 0);
            this.p_Top.Margin = new System.Windows.Forms.Padding(5);
            this.p_Top.Name = "p_Top";
            this.p_Top.Size = new System.Drawing.Size(872, 465);
            this.p_Top.TabIndex = 0;
            // 
            // gb_OrderTypes
            // 
            this.gb_OrderTypes.Controls.Add(this.ce_DontRollbackDebtClose);
            this.gb_OrderTypes.Controls.Add(this.labelControl5);
            this.gb_OrderTypes.Controls.Add(this.le_ReturnClient);
            this.gb_OrderTypes.Controls.Add(this.labelControl4);
            this.gb_OrderTypes.Controls.Add(this.le_InvoiceClient);
            this.gb_OrderTypes.Controls.Add(this.ce_OnlyPayments);
            this.gb_OrderTypes.Controls.Add(this.ce_OnlySalesWithCustomer);
            this.gb_OrderTypes.Controls.Add(this.ce_AllPos);
            this.gb_OrderTypes.Controls.Add(this.labelControl2);
            this.gb_OrderTypes.Controls.Add(this.le_Pos);
            this.gb_OrderTypes.Controls.Add(this.labelControl3);
            this.gb_OrderTypes.Controls.Add(this.le_Branch);
            this.gb_OrderTypes.Controls.Add(this.labelControl1);
            this.gb_OrderTypes.Controls.Add(this.de_Date);
            this.gb_OrderTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_OrderTypes.Location = new System.Drawing.Point(0, 0);
            this.gb_OrderTypes.Margin = new System.Windows.Forms.Padding(5);
            this.gb_OrderTypes.Name = "gb_OrderTypes";
            this.gb_OrderTypes.Padding = new System.Windows.Forms.Padding(5);
            this.gb_OrderTypes.Size = new System.Drawing.Size(872, 465);
            this.gb_OrderTypes.TabIndex = 0;
            this.gb_OrderTypes.TabStop = false;
            // 
            // ce_DontRollbackDebtClose
            // 
            this.ce_DontRollbackDebtClose.Location = new System.Drawing.Point(232, 419);
            this.ce_DontRollbackDebtClose.Margin = new System.Windows.Forms.Padding(5);
            this.ce_DontRollbackDebtClose.Name = "ce_DontRollbackDebtClose";
            this.ce_DontRollbackDebtClose.Properties.Caption = "Borç Takip Yapılanlar Geri Alınmasın";
            this.ce_DontRollbackDebtClose.Size = new System.Drawing.Size(620, 35);
            this.ce_DontRollbackDebtClose.TabIndex = 22;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(5, 202);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(5);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(139, 40);
            this.labelControl5.TabIndex = 21;
            this.labelControl5.Text = "İade Cari";
            // 
            // le_ReturnClient
            // 
            this.le_ReturnClient.Location = new System.Drawing.Point(232, 196);
            this.le_ReturnClient.Margin = new System.Windows.Forms.Padding(5);
            this.le_ReturnClient.Name = "le_ReturnClient";
            this.le_ReturnClient.Properties.Appearance.BorderColor = System.Drawing.Color.Blue;
            this.le_ReturnClient.Properties.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.le_ReturnClient.Properties.Appearance.Options.UseBorderColor = true;
            this.le_ReturnClient.Properties.Appearance.Options.UseFont = true;
            this.le_ReturnClient.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.le_ReturnClient.Properties.NullText = "[Cari Seçiniz]";
            this.le_ReturnClient.Size = new System.Drawing.Size(377, 54);
            this.le_ReturnClient.TabIndex = 19;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(5, 138);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(5);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(171, 40);
            this.labelControl4.TabIndex = 20;
            this.labelControl4.Text = "Fatura Cari";
            // 
            // le_InvoiceClient
            // 
            this.le_InvoiceClient.Location = new System.Drawing.Point(232, 133);
            this.le_InvoiceClient.Margin = new System.Windows.Forms.Padding(5);
            this.le_InvoiceClient.Name = "le_InvoiceClient";
            this.le_InvoiceClient.Properties.Appearance.BorderColor = System.Drawing.Color.Blue;
            this.le_InvoiceClient.Properties.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.le_InvoiceClient.Properties.Appearance.Options.UseBorderColor = true;
            this.le_InvoiceClient.Properties.Appearance.Options.UseFont = true;
            this.le_InvoiceClient.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.le_InvoiceClient.Properties.NullText = "[Cari Seçiniz]";
            this.le_InvoiceClient.Size = new System.Drawing.Size(377, 54);
            this.le_InvoiceClient.TabIndex = 18;
            // 
            // ce_OnlyPayments
            // 
            this.ce_OnlyPayments.Location = new System.Drawing.Point(232, 372);
            this.ce_OnlyPayments.Margin = new System.Windows.Forms.Padding(5);
            this.ce_OnlyPayments.Name = "ce_OnlyPayments";
            this.ce_OnlyPayments.Properties.Caption = "Sadece Tahsilatlar";
            this.ce_OnlyPayments.Size = new System.Drawing.Size(620, 35);
            this.ce_OnlyPayments.TabIndex = 17;
            this.ce_OnlyPayments.CheckedChanged += new System.EventHandler(this.ce_OnlyPayments_CheckedChanged);
            // 
            // ce_OnlySalesWithCustomer
            // 
            this.ce_OnlySalesWithCustomer.Location = new System.Drawing.Point(232, 326);
            this.ce_OnlySalesWithCustomer.Margin = new System.Windows.Forms.Padding(5);
            this.ce_OnlySalesWithCustomer.Name = "ce_OnlySalesWithCustomer";
            this.ce_OnlySalesWithCustomer.Properties.Caption = "Sadece Carili Satışlar/Fatura Cari ve Iade Cari Dışındakileri Silmek İçin.";
            this.ce_OnlySalesWithCustomer.Size = new System.Drawing.Size(620, 35);
            this.ce_OnlySalesWithCustomer.TabIndex = 16;
            this.ce_OnlySalesWithCustomer.CheckedChanged += new System.EventHandler(this.ce_OnlySalesWithCustomer_CheckedChanged);
            // 
            // ce_AllPos
            // 
            this.ce_AllPos.EditValue = true;
            this.ce_AllPos.Location = new System.Drawing.Point(618, 272);
            this.ce_AllPos.Margin = new System.Windows.Forms.Padding(5);
            this.ce_AllPos.Name = "ce_AllPos";
            this.ce_AllPos.Properties.Caption = "Tüm Kasalar";
            this.ce_AllPos.Size = new System.Drawing.Size(145, 35);
            this.ce_AllPos.TabIndex = 15;
            this.ce_AllPos.Visible = false;
            this.ce_AllPos.CheckedChanged += new System.EventHandler(this.ce_AllPos_CheckedChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(5, 265);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(5);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(78, 40);
            this.labelControl2.TabIndex = 14;
            this.labelControl2.Text = "Kasa";
            this.labelControl2.Visible = false;
            // 
            // le_Pos
            // 
            this.le_Pos.Enabled = false;
            this.le_Pos.Location = new System.Drawing.Point(232, 260);
            this.le_Pos.Margin = new System.Windows.Forms.Padding(5);
            this.le_Pos.Name = "le_Pos";
            this.le_Pos.Properties.Appearance.BorderColor = System.Drawing.Color.Blue;
            this.le_Pos.Properties.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.le_Pos.Properties.Appearance.Options.UseBorderColor = true;
            this.le_Pos.Properties.Appearance.Options.UseFont = true;
            this.le_Pos.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.le_Pos.Properties.NullText = "[Kasa Seçiniz]";
            this.le_Pos.Size = new System.Drawing.Size(377, 54);
            this.le_Pos.TabIndex = 13;
            this.le_Pos.Visible = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(5, 74);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(5);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(85, 40);
            this.labelControl3.TabIndex = 10;
            this.labelControl3.Text = "İşyeri";
            // 
            // le_Branch
            // 
            this.le_Branch.Location = new System.Drawing.Point(232, 69);
            this.le_Branch.Margin = new System.Windows.Forms.Padding(5);
            this.le_Branch.Name = "le_Branch";
            this.le_Branch.Properties.Appearance.BorderColor = System.Drawing.Color.Blue;
            this.le_Branch.Properties.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.le_Branch.Properties.Appearance.Options.UseBorderColor = true;
            this.le_Branch.Properties.Appearance.Options.UseFont = true;
            this.le_Branch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.le_Branch.Properties.NullText = "[İşyeri Seçiniz]";
            this.le_Branch.Size = new System.Drawing.Size(377, 54);
            this.le_Branch.TabIndex = 4;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(5, 18);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(79, 40);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Tarih";
            // 
            // de_Date
            // 
            this.de_Date.EditValue = null;
            this.de_Date.Location = new System.Drawing.Point(232, 5);
            this.de_Date.Margin = new System.Windows.Forms.Padding(5);
            this.de_Date.Name = "de_Date";
            this.de_Date.Properties.Appearance.BorderColor = System.Drawing.Color.Blue;
            this.de_Date.Properties.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.de_Date.Properties.Appearance.Options.UseBorderColor = true;
            this.de_Date.Properties.Appearance.Options.UseFont = true;
            this.de_Date.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.de_Date.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.de_Date.Size = new System.Drawing.Size(377, 54);
            this.de_Date.TabIndex = 0;
            // 
            // p_Bottom
            // 
            this.p_Bottom.Controls.Add(this.simpleButton1);
            this.p_Bottom.Controls.Add(this.sb_Cancel);
            this.p_Bottom.Controls.Add(this.sb_SaveToBm);
            this.p_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.p_Bottom.Location = new System.Drawing.Point(0, 532);
            this.p_Bottom.Margin = new System.Windows.Forms.Padding(5);
            this.p_Bottom.Name = "p_Bottom";
            this.p_Bottom.Size = new System.Drawing.Size(872, 154);
            this.p_Bottom.TabIndex = 1;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(23, 0);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(5);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(283, 154);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "NCR Sil";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // sb_Cancel
            // 
            this.sb_Cancel.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.sb_Cancel.Appearance.Options.UseFont = true;
            this.sb_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sb_Cancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.sb_Cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("sb_Cancel.ImageOptions.Image")));
            this.sb_Cancel.Location = new System.Drawing.Point(306, 0);
            this.sb_Cancel.Margin = new System.Windows.Forms.Padding(5);
            this.sb_Cancel.Name = "sb_Cancel";
            this.sb_Cancel.Size = new System.Drawing.Size(283, 154);
            this.sb_Cancel.TabIndex = 0;
            this.sb_Cancel.Text = "Sil";
            this.sb_Cancel.Click += new System.EventHandler(this.sb_Cancel_Click);
            // 
            // sb_SaveToBm
            // 
            this.sb_SaveToBm.Appearance.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.sb_SaveToBm.Appearance.Options.UseFont = true;
            this.sb_SaveToBm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sb_SaveToBm.Dock = System.Windows.Forms.DockStyle.Right;
            this.sb_SaveToBm.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("sb_SaveToBm.ImageOptions.Image")));
            this.sb_SaveToBm.Location = new System.Drawing.Point(589, 0);
            this.sb_SaveToBm.Margin = new System.Windows.Forms.Padding(5);
            this.sb_SaveToBm.Name = "sb_SaveToBm";
            this.sb_SaveToBm.Size = new System.Drawing.Size(283, 154);
            this.sb_SaveToBm.TabIndex = 1;
            this.sb_SaveToBm.Text = "Vazgeç";
            this.sb_SaveToBm.Click += new System.EventHandler(this.sb_SaveToBm_Click);
            // 
            // p_Center
            // 
            this.p_Center.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_Center.Location = new System.Drawing.Point(0, 465);
            this.p_Center.Margin = new System.Windows.Forms.Padding(5);
            this.p_Center.Name = "p_Center";
            this.p_Center.Size = new System.Drawing.Size(872, 67);
            this.p_Center.TabIndex = 1;
            // 
            // Frm_PosEODCancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 686);
            this.Controls.Add(this.p_Center);
            this.Controls.Add(this.p_Bottom);
            this.Controls.Add(this.p_Top);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Frm_PosEODCancel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.p_Top.ResumeLayout(false);
            this.gb_OrderTypes.ResumeLayout(false);
            this.gb_OrderTypes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ce_DontRollbackDebtClose.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_ReturnClient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_InvoiceClient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_OnlyPayments.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_OnlySalesWithCustomer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_AllPos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_Pos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_Branch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_Date.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_Date.Properties)).EndInit();
            this.p_Bottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel p_Top;
        private System.Windows.Forms.Panel p_Bottom;
        private System.Windows.Forms.GroupBox gb_OrderTypes;
        private System.Windows.Forms.Panel p_Center;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit de_Date;
        private DevExpress.XtraEditors.SimpleButton sb_SaveToBm;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LookUpEdit le_Branch;
        private DevExpress.XtraEditors.SimpleButton sb_Cancel;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LookUpEdit le_Pos;
        private DevExpress.XtraEditors.CheckEdit ce_AllPos;
        private DevExpress.XtraEditors.CheckEdit ce_OnlyPayments;
        private DevExpress.XtraEditors.CheckEdit ce_OnlySalesWithCustomer;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LookUpEdit le_ReturnClient;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LookUpEdit le_InvoiceClient;
        private DevExpress.XtraEditors.CheckEdit ce_DontRollbackDebtClose;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}