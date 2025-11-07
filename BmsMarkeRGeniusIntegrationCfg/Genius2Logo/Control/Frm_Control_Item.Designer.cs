namespace BmsMarkeRGeniusIntegrationCfg.Genius2Logo.Control
{
    partial class Frm_Control_Item
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
            this.p_Top = new System.Windows.Forms.Panel();
            this.gb_OrderTypes = new System.Windows.Forms.GroupBox();
            this.p_Bottom = new System.Windows.Forms.Panel();
            this.p_Center = new System.Windows.Forms.Panel();
            this.grc_Mapping = new DevExpress.XtraGrid.GridControl();
            this.grv_Mapping = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.p_Top.SuspendLayout();
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
            this.p_Top.Size = new System.Drawing.Size(763, 138);
            this.p_Top.TabIndex = 0;
            // 
            // gb_OrderTypes
            // 
            this.gb_OrderTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_OrderTypes.Location = new System.Drawing.Point(0, 0);
            this.gb_OrderTypes.Name = "gb_OrderTypes";
            this.gb_OrderTypes.Size = new System.Drawing.Size(763, 138);
            this.gb_OrderTypes.TabIndex = 0;
            this.gb_OrderTypes.TabStop = false;
            // 
            // p_Bottom
            // 
            this.p_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.p_Bottom.Location = new System.Drawing.Point(0, 516);
            this.p_Bottom.Name = "p_Bottom";
            this.p_Bottom.Size = new System.Drawing.Size(763, 100);
            this.p_Bottom.TabIndex = 1;
            // 
            // p_Center
            // 
            this.p_Center.Controls.Add(this.grc_Mapping);
            this.p_Center.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_Center.Location = new System.Drawing.Point(0, 138);
            this.p_Center.Name = "p_Center";
            this.p_Center.Size = new System.Drawing.Size(763, 378);
            this.p_Center.TabIndex = 1;
            // 
            // grc_Mapping
            // 
            this.grc_Mapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grc_Mapping.Location = new System.Drawing.Point(0, 0);
            this.grc_Mapping.MainView = this.grv_Mapping;
            this.grc_Mapping.Name = "grc_Mapping";
            this.grc_Mapping.Size = new System.Drawing.Size(763, 378);
            this.grc_Mapping.TabIndex = 0;
            this.grc_Mapping.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grv_Mapping});
            // 
            // grv_Mapping
            // 
            this.grv_Mapping.GridControl = this.grc_Mapping;
            this.grv_Mapping.Name = "grv_Mapping";
            this.grv_Mapping.OptionsSelection.MultiSelect = true;
            this.grv_Mapping.OptionsView.ColumnAutoWidth = false;
            this.grv_Mapping.OptionsView.ShowFooter = true;
            this.grv_Mapping.OptionsView.ShowGroupPanel = false;
            this.grv_Mapping.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.grv_Invoices_PopupMenuShowing);
            // 
            // Frm_Control_Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 616);
            this.Controls.Add(this.p_Center);
            this.Controls.Add(this.p_Bottom);
            this.Controls.Add(this.p_Top);
            this.Name = "Frm_Control_Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Frm_CRUDs_FormClosed);
            this.p_Top.ResumeLayout(false);
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
    }
}