using BmsMarkeRGeniusIntegrationLibrary;
using BmsMarkeRGeniusIntegrationLibrary.METHODS.MODELS;
using DevExpress.Utils.CommonDialogs;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BmsMarkeRGeniusIntegrationCfg.Genius2Logo.Definition
{
    public partial class Frm_Default : DevExpress.XtraEditors.XtraForm
    {
        CONFIG CFG;
        string TABLENAME = "Bms_XXX_MarkeRGeniusIntegration_Default";
        List<Bms_XXX_MarkeRGeniusIntegration_Default> OList = new List<Bms_XXX_MarkeRGeniusIntegration_Default>();
        public Frm_Default(string HEADERNAME)
        {
            InitializeComponent();
            this.Text = HEADERNAME;
            CFG = CONFIG_HELPER.GET_CONFIG();
            TABLENAME = TABLENAME.Replace("XXX", CFG.FIRMNR);
            InitializeData(null, null);
        }

        private void InitializeData(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(PROGRESSFORM), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("LÜTFEN BEKLEYİN.");
            SplashScreenManager.Default.SetWaitFormDescription("");
            OList = HELPER.DataTableToList<Bms_XXX_MarkeRGeniusIntegration_Default>(HELPER.SqlSelectLogo($@"SELECT * FROM {TABLENAME}"));
            grc_Mapping.DataSource = OList;
            grv_Mapping.Columns["Id"].Visible = false;
            //column id is readonly
            grv_Mapping.Columns["Id"].OptionsColumn.AllowEdit = false;
            grv_Mapping.Columns["Description"].OptionsColumn.AllowEdit = false;
            SplashScreenManager.CloseForm(false);
        }

        private void ExportToExcel(object sender, EventArgs e)
        {
            HELPER.DxExportGridToExcel(grv_Mapping, true);
        }

        private void sb_SaveToBM_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(PROGRESSFORM), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("LÜTFEN BEKLEYİN.");
            SplashScreenManager.Default.SetWaitFormDescription("");
            string LGCONSTR = string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};MultipleActiveResultSets=True;", CFG.LGDBSERVER, CFG.LGDBDATABASE, CFG.LGDBUSERNAME, CFG.LGDBPASSWORD);
            SqlConnection CON = new SqlConnection(LGCONSTR);
            SqlTransaction TRANSACTION = null;
            if (CON.State != ConnectionState.Open)
                CON.Open();
            TRANSACTION = CON.BeginTransaction();
            //delete all records
            SqlCommand com = null;
            com = new SqlCommand($@"TRUNCATE TABLE {TABLENAME}", CON, TRANSACTION);
            com.ExecuteNonQuery();

            for (int i = 0; i < grv_Mapping.RowCount; i++)
            {
                Bms_XXX_MarkeRGeniusIntegration_Default item = (Bms_XXX_MarkeRGeniusIntegration_Default)grv_Mapping.GetRow(i);
                com = new SqlCommand($@"INSERT INTO {TABLENAME} (Description,Value) VALUES (@Description,@Value)", CON, TRANSACTION);
                com.Parameters.AddWithValue("@Description", item.Description);
                com.Parameters.AddWithValue("@Value", item.Value);
                com.ExecuteNonQuery();
            }
            TRANSACTION.Commit();
            SplashScreenManager.CloseForm(false);
            InitializeData(null, null);
            XtraMessageBox.Show("Kayıt İşlemi Tamamlandı ", "İşlem Sonucu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Frm_CRUDs_FormClosed(object sender, FormClosedEventArgs e)
        {
            grv_Mapping.SaveLayoutToRegistry(string.Format(@"{0}\{1}", Application.StartupPath, this.GetType().Name));
        }

        private void grv_Invoices_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
            {
                e.Menu.Items.Add(new DXMenuItem("Excele Kaydet", ExportToExcel));
                e.Menu.Items.Add(new DXMenuItem("Güncelle", InitializeData));
                e.Menu.Items.Add(new DXMenuItem("Satır Sil", DeleteRow));
            }
        }

        private void DeleteRow(object sender, EventArgs e)
        {
            //ask if sure
            if (XtraMessageBox.Show("Seçili kayıt silinecek emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            Bms_XXX_MarkeRGeniusIntegration_Default item = (Bms_XXX_MarkeRGeniusIntegration_Default)grv_Mapping.GetFocusedRow();
            if (item.Id == 0)
                grv_Mapping.DeleteRow(grv_Mapping.FocusedRowHandle);
            else
                HELPER.SqlExecute($@"DELETE FROM {TABLENAME} WHERE Id = {item.Id}");
            InitializeData(null, null);
        }

        private void sb_GetFromMysql_Click(object sender, EventArgs e)
        {
            InitializeData(null, null);
        }
    }
}