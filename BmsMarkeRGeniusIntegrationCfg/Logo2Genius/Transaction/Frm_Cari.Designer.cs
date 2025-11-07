namespace BmsMarkeRGeniusIntegrationCfg.Logo2Genius.Transaction
{
    partial class Frm_Cari
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Cari));
            this.p_Top = new System.Windows.Forms.Panel();
            this.gb_OrderTypes = new System.Windows.Forms.GroupBox();
            this.gb_Branch = new System.Windows.Forms.GroupBox();
            this.gle_Value = new DevExpress.XtraEditors.GridLookUpEdit();
            this.sb_Load = new DevExpress.XtraEditors.SimpleButton();
            this.rb_ChangedClients = new System.Windows.Forms.RadioButton();
            this.rb_AllClients = new System.Windows.Forms.RadioButton();
            this.p_Date = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.de_StartDate = new DevExpress.XtraEditors.DateEdit();
            this.de_EndDate = new DevExpress.XtraEditors.DateEdit();
            this.p_Bottom = new System.Windows.Forms.Panel();
            this.sb_SaveFile = new DevExpress.XtraEditors.SimpleButton();
            this.l_FilePath = new DevExpress.XtraEditors.LabelControl();
            this.p_Center = new System.Windows.Forms.Panel();
            this.grc_Malzeme = new DevExpress.XtraGrid.GridControl();
            this.grv_Malzeme = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.p_Top.SuspendLayout();
            this.gb_OrderTypes.SuspendLayout();
            this.gb_Branch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gle_Value.Properties)).BeginInit();
            this.p_Date.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.de_StartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_StartDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_EndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_EndDate.Properties.CalendarTimeProperties)).BeginInit();
            this.p_Bottom.SuspendLayout();
            this.p_Center.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grc_Malzeme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grv_Malzeme)).BeginInit();
            this.SuspendLayout();
            // 
            // p_Top
            // 
            this.p_Top.Controls.Add(this.gb_OrderTypes);
            this.p_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.p_Top.Location = new System.Drawing.Point(0, 0);
            this.p_Top.Margin = new System.Windows.Forms.Padding(4);
            this.p_Top.Name = "p_Top";
            this.p_Top.Size = new System.Drawing.Size(1227, 98);
            this.p_Top.TabIndex = 0;
            // 
            // gb_OrderTypes
            // 
            this.gb_OrderTypes.Controls.Add(this.gb_Branch);
            this.gb_OrderTypes.Controls.Add(this.sb_Load);
            this.gb_OrderTypes.Controls.Add(this.rb_ChangedClients);
            this.gb_OrderTypes.Controls.Add(this.rb_AllClients);
            this.gb_OrderTypes.Controls.Add(this.p_Date);
            this.gb_OrderTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_OrderTypes.Location = new System.Drawing.Point(0, 0);
            this.gb_OrderTypes.Margin = new System.Windows.Forms.Padding(4);
            this.gb_OrderTypes.Name = "gb_OrderTypes";
            this.gb_OrderTypes.Padding = new System.Windows.Forms.Padding(4);
            this.gb_OrderTypes.Size = new System.Drawing.Size(1227, 98);
            this.gb_OrderTypes.TabIndex = 0;
            this.gb_OrderTypes.TabStop = false;
            // 
            // gb_Branch
            // 
            this.gb_Branch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gb_Branch.Controls.Add(this.gle_Value);
            this.gb_Branch.Dock = System.Windows.Forms.DockStyle.Left;
            this.gb_Branch.Location = new System.Drawing.Point(4, 24);
            this.gb_Branch.Margin = new System.Windows.Forms.Padding(4);
            this.gb_Branch.Name = "gb_Branch";
            this.gb_Branch.Padding = new System.Windows.Forms.Padding(4);
            this.gb_Branch.Size = new System.Drawing.Size(300, 70);
            this.gb_Branch.TabIndex = 25;
            this.gb_Branch.TabStop = false;
            this.gb_Branch.Text = "Mağaza";
            // 
            // gle_Value
            // 
            this.gle_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gle_Value.Location = new System.Drawing.Point(4, 24);
            this.gle_Value.Margin = new System.Windows.Forms.Padding(4);
            this.gle_Value.Name = "gle_Value";
            this.gle_Value.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gle_Value.Size = new System.Drawing.Size(292, 26);
            this.gle_Value.TabIndex = 31;
            this.gle_Value.EditValueChanged += new System.EventHandler(this.gle_Value_EditValueChanged);
            // 
            // sb_Load
            // 
            this.sb_Load.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sb_Load.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sb_Load.ImageOptions.SvgImage")));
            this.sb_Load.Location = new System.Drawing.Point(843, 12);
            this.sb_Load.Margin = new System.Windows.Forms.Padding(4);
            this.sb_Load.Name = "sb_Load";
            this.sb_Load.Size = new System.Drawing.Size(166, 77);
            this.sb_Load.TabIndex = 22;
            this.sb_Load.Text = "Getir";
            this.sb_Load.Click += new System.EventHandler(this.sb_Load_Click);
            // 
            // rb_ChangedClients
            // 
            this.rb_ChangedClients.AutoSize = true;
            this.rb_ChangedClients.Checked = true;
            this.rb_ChangedClients.Location = new System.Drawing.Point(310, 66);
            this.rb_ChangedClients.Margin = new System.Windows.Forms.Padding(4);
            this.rb_ChangedClients.Name = "rb_ChangedClients";
            this.rb_ChangedClients.Size = new System.Drawing.Size(141, 23);
            this.rb_ChangedClients.TabIndex = 19;
            this.rb_ChangedClients.TabStop = true;
            this.rb_ChangedClients.Text = "Değişen Cariler";
            this.rb_ChangedClients.UseVisualStyleBackColor = true;
            // 
            // rb_AllClients
            // 
            this.rb_AllClients.AutoSize = true;
            this.rb_AllClients.Location = new System.Drawing.Point(508, 66);
            this.rb_AllClients.Margin = new System.Windows.Forms.Padding(4);
            this.rb_AllClients.Name = "rb_AllClients";
            this.rb_AllClients.Size = new System.Drawing.Size(118, 23);
            this.rb_AllClients.TabIndex = 20;
            this.rb_AllClients.Text = "Tüm Cariler";
            this.rb_AllClients.UseVisualStyleBackColor = true;
            // 
            // p_Date
            // 
            this.p_Date.Controls.Add(this.label3);
            this.p_Date.Controls.Add(this.label4);
            this.p_Date.Controls.Add(this.de_StartDate);
            this.p_Date.Controls.Add(this.de_EndDate);
            this.p_Date.Location = new System.Drawing.Point(315, 12);
            this.p_Date.Margin = new System.Windows.Forms.Padding(4);
            this.p_Date.Name = "p_Date";
            this.p_Date.Size = new System.Drawing.Size(519, 51);
            this.p_Date.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 16);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 19);
            this.label3.TabIndex = 11;
            this.label3.Text = "Baş. Tarih:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(260, 16);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 19);
            this.label4.TabIndex = 12;
            this.label4.Text = "Bit. Tarih:";
            // 
            // de_StartDate
            // 
            this.de_StartDate.EditValue = null;
            this.de_StartDate.Location = new System.Drawing.Point(100, 12);
            this.de_StartDate.Margin = new System.Windows.Forms.Padding(4);
            this.de_StartDate.Name = "de_StartDate";
            this.de_StartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.de_StartDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.de_StartDate.Properties.MaskSettings.Set("useAdvancingCaret", true);
            this.de_StartDate.Properties.UseMaskAsDisplayFormat = true;
            this.de_StartDate.Size = new System.Drawing.Size(150, 26);
            this.de_StartDate.TabIndex = 9;
            // 
            // de_EndDate
            // 
            this.de_EndDate.EditValue = null;
            this.de_EndDate.Location = new System.Drawing.Point(346, 12);
            this.de_EndDate.Margin = new System.Windows.Forms.Padding(4);
            this.de_EndDate.Name = "de_EndDate";
            this.de_EndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.de_EndDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.de_EndDate.Properties.MaskSettings.Set("useAdvancingCaret", true);
            this.de_EndDate.Properties.UseMaskAsDisplayFormat = true;
            this.de_EndDate.Size = new System.Drawing.Size(150, 26);
            this.de_EndDate.TabIndex = 10;
            // 
            // p_Bottom
            // 
            this.p_Bottom.Controls.Add(this.sb_SaveFile);
            this.p_Bottom.Controls.Add(this.l_FilePath);
            this.p_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.p_Bottom.Location = new System.Drawing.Point(0, 792);
            this.p_Bottom.Margin = new System.Windows.Forms.Padding(4);
            this.p_Bottom.Name = "p_Bottom";
            this.p_Bottom.Size = new System.Drawing.Size(1227, 108);
            this.p_Bottom.TabIndex = 1;
            // 
            // sb_SaveFile
            // 
            this.sb_SaveFile.Appearance.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.sb_SaveFile.Appearance.Options.UseFont = true;
            this.sb_SaveFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sb_SaveFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sb_SaveFile.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("sb_SaveFile.ImageOptions.Image")));
            this.sb_SaveFile.Location = new System.Drawing.Point(0, 19);
            this.sb_SaveFile.Margin = new System.Windows.Forms.Padding(4);
            this.sb_SaveFile.Name = "sb_SaveFile";
            this.sb_SaveFile.Size = new System.Drawing.Size(1227, 89);
            this.sb_SaveFile.TabIndex = 23;
            this.sb_SaveFile.Text = "Kaydet";
            this.sb_SaveFile.Click += new System.EventHandler(this.sb_Save2Destination_Click);
            // 
            // l_FilePath
            // 
            this.l_FilePath.Dock = System.Windows.Forms.DockStyle.Top;
            this.l_FilePath.Location = new System.Drawing.Point(0, 0);
            this.l_FilePath.Margin = new System.Windows.Forms.Padding(4);
            this.l_FilePath.Name = "l_FilePath";
            this.l_FilePath.Size = new System.Drawing.Size(75, 19);
            this.l_FilePath.TabIndex = 24;
            this.l_FilePath.Text = "DosyaYolu";
            // 
            // p_Center
            // 
            this.p_Center.Controls.Add(this.grc_Malzeme);
            this.p_Center.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_Center.Location = new System.Drawing.Point(0, 98);
            this.p_Center.Margin = new System.Windows.Forms.Padding(4);
            this.p_Center.Name = "p_Center";
            this.p_Center.Size = new System.Drawing.Size(1227, 694);
            this.p_Center.TabIndex = 1;
            // 
            // grc_Malzeme
            // 
            this.grc_Malzeme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grc_Malzeme.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grc_Malzeme.Location = new System.Drawing.Point(0, 0);
            this.grc_Malzeme.MainView = this.grv_Malzeme;
            this.grc_Malzeme.Margin = new System.Windows.Forms.Padding(4);
            this.grc_Malzeme.Name = "grc_Malzeme";
            this.grc_Malzeme.Size = new System.Drawing.Size(1227, 694);
            this.grc_Malzeme.TabIndex = 0;
            this.grc_Malzeme.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grv_Malzeme});
            // 
            // grv_Malzeme
            // 
            this.grv_Malzeme.DetailHeight = 512;
            this.grv_Malzeme.GridControl = this.grc_Malzeme;
            this.grv_Malzeme.Name = "grv_Malzeme";
            this.grv_Malzeme.OptionsBehavior.Editable = false;
            this.grv_Malzeme.OptionsSelection.MultiSelect = true;
            this.grv_Malzeme.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.grv_Malzeme.OptionsView.ColumnAutoWidth = false;
            this.grv_Malzeme.OptionsView.ShowFooter = true;
            this.grv_Malzeme.OptionsView.ShowGroupPanel = false;
            this.grv_Malzeme.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.grv_Invoices_PopupMenuShowing);
            // 
            // Frm_Cari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1227, 900);
            this.Controls.Add(this.p_Center);
            this.Controls.Add(this.p_Bottom);
            this.Controls.Add(this.p_Top);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Frm_Cari";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Frm_CRUDs_FormClosed);
            this.p_Top.ResumeLayout(false);
            this.gb_OrderTypes.ResumeLayout(false);
            this.gb_OrderTypes.PerformLayout();
            this.gb_Branch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gle_Value.Properties)).EndInit();
            this.p_Date.ResumeLayout(false);
            this.p_Date.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.de_StartDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_StartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_EndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.de_EndDate.Properties)).EndInit();
            this.p_Bottom.ResumeLayout(false);
            this.p_Bottom.PerformLayout();
            this.p_Center.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grc_Malzeme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grv_Malzeme)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel p_Top;
        private System.Windows.Forms.Panel p_Bottom;
        private System.Windows.Forms.GroupBox gb_OrderTypes;
        private System.Windows.Forms.Panel p_Center;
        private DevExpress.XtraGrid.GridControl grc_Malzeme;
        private DevExpress.XtraGrid.Views.Grid.GridView grv_Malzeme;
        private DevExpress.XtraEditors.SimpleButton sb_Load;
        private System.Windows.Forms.RadioButton rb_ChangedClients;
        private System.Windows.Forms.RadioButton rb_AllClients;
        private System.Windows.Forms.Panel p_Date;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.DateEdit de_StartDate;
        private DevExpress.XtraEditors.DateEdit de_EndDate;
        private DevExpress.XtraEditors.SimpleButton sb_SaveFile;
        private System.Windows.Forms.GroupBox gb_Branch;
        private DevExpress.XtraEditors.LabelControl l_FilePath;
        private DevExpress.XtraEditors.GridLookUpEdit gle_Value;
    }
}