namespace BmsMarkeRGeniusIntegrationCfg
{
    partial class FRM_Errors
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
            this.grc_Errors = new DevExpress.XtraGrid.GridControl();
            this.grv_Errors = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.grc_Errors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grv_Errors)).BeginInit();
            this.SuspendLayout();
            // 
            // grc_Errors
            // 
            this.grc_Errors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grc_Errors.Location = new System.Drawing.Point(0, 0);
            this.grc_Errors.MainView = this.grv_Errors;
            this.grc_Errors.Name = "grc_Errors";
            this.grc_Errors.Size = new System.Drawing.Size(800, 450);
            this.grc_Errors.TabIndex = 0;
            this.grc_Errors.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grv_Errors});
            // 
            // grv_Errors
            // 
            this.grv_Errors.GridControl = this.grc_Errors;
            this.grv_Errors.Name = "grv_Errors";
            this.grv_Errors.OptionsBehavior.Editable = false;
            this.grv_Errors.OptionsView.ShowGroupPanel = false;
            this.grv_Errors.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.grv_Errors_PopupMenuShowing);
            // 
            // FRM_Errors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grc_Errors);
            this.Name = "FRM_Errors";
            this.Text = "Hatalar";
            ((System.ComponentModel.ISupportInitialize)(this.grc_Errors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grv_Errors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grc_Errors;
        private DevExpress.XtraGrid.Views.Grid.GridView grv_Errors;
    }
}