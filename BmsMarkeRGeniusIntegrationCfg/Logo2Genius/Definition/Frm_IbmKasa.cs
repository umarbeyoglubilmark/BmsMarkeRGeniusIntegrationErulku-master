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

namespace BmsMarkeRGeniusIntegrationCfg.Logo2Genius.Definition
{
    public partial class Frm_IbmKasa : DevExpress.XtraEditors.XtraForm
    {
        CONFIG CFG;
        string TABLENAME = "Bms_XXX_MarkeRGeniusIntegration_IbmKasa";
        List<Bms_XXX_MarkeRGeniusIntegration_IbmKasa> OList = new List<Bms_XXX_MarkeRGeniusIntegration_IbmKasa>();
        public Frm_IbmKasa(string HEADERNAME)
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
            OList = HELPER.DataTableToList<Bms_XXX_MarkeRGeniusIntegration_IbmKasa>(HELPER.SqlSelectLogo($@"SELECT * FROM {TABLENAME}"));
            grc_CariSeYuzde.DataSource = OList;
            grv_CariSeYuzde.Columns["Id"].Visible = false;
            SplashScreenManager.CloseForm(false);
        }

        private void ExportToExcel(object sender, EventArgs e)
        {
            HELPER.DxExportGridToExcel(grv_CariSeYuzde, true);
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
            com = new SqlCommand($@"DELETE FROM {TABLENAME}", CON, TRANSACTION);
            com.ExecuteNonQuery();

            for (int i = 0; i < grv_CariSeYuzde.RowCount; i++)
            {
                Bms_XXX_MarkeRGeniusIntegration_IbmKasa item = (Bms_XXX_MarkeRGeniusIntegration_IbmKasa)grv_CariSeYuzde.GetRow(i);
                com = new SqlCommand($@"INSERT INTO {TABLENAME} (LogoValue,G3Value,Path,SqlServer,SqlUsername,SqlPassword,SqlDatabase) VALUES (@LogoValue,@G3Value,@Path,@SqlServer,@SqlUsername,@SqlPassword,@SqlDatabase)", CON, TRANSACTION);
                com.Parameters.AddWithValue("@LogoValue", item.LogoValue);
                com.Parameters.AddWithValue("@G3Value", item.G3Value);
                com.Parameters.AddWithValue("@Path", item.Path);
                com.Parameters.AddWithValue("@SqlServer", item.SqlServer);
                com.Parameters.AddWithValue("@SqlUsername", item.SqlUsername);
                com.Parameters.AddWithValue("@SqlPassword", item.SqlPassword);
                com.Parameters.AddWithValue("@SqlDatabase", item.SqlDatabase);
                com.ExecuteNonQuery();
            }
            TRANSACTION.Commit();
            SplashScreenManager.CloseForm(false);
            InitializeData(null, null);
            XtraMessageBox.Show("Kayıt İşlemi Tamamlandı ", "İşlem Sonucu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Frm_CRUDs_FormClosed(object sender, FormClosedEventArgs e)
        {
            grv_CariSeYuzde.SaveLayoutToRegistry(string.Format(@"{0}\{1}", Application.StartupPath, this.GetType().Name));
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
            Bms_XXX_MarkeRGeniusIntegration_IbmKasa item = (Bms_XXX_MarkeRGeniusIntegration_IbmKasa)grv_CariSeYuzde.GetFocusedRow();
            if (item.Id == 0)
                grv_CariSeYuzde.DeleteRow(grv_CariSeYuzde.FocusedRowHandle);
            else
                HELPER.SqlExecute($@"DELETE FROM {TABLENAME} WHERE Id = {item.Id}");
            InitializeData(null, null);
        }

        private void sb_GetFromMysql_Click(object sender, EventArgs e)
        {
            InitializeData(null, null);
        }

        private void sb_AddRow_Click(object sender, EventArgs e)
        {
            Bms_XXX_MarkeRGeniusIntegration_IbmKasa item = new Bms_XXX_MarkeRGeniusIntegration_IbmKasa();
            item.LogoValue = te_LogoValue.Text;
            item.G3Value = te_G3Value.Text;
            item.Path = te_DosyaYolu.Text;
            item.SqlServer = te_SqlServer.Text;
            item.SqlUsername = te_SqlUsername.Text;
            item.SqlPassword = te_SqlPassword.Text;
            item.SqlDatabase = te_SqlDatabase.Text;
            OList.Add(item);
            grv_CariSeYuzde.RefreshData();
        }

        private void sb_Path_Click(object sender, EventArgs e)
        {
            XtraFolderBrowserDialog dialog = new XtraFolderBrowserDialog();
            dialog.DialogStyle = FolderBrowserDialogStyle.Wide;
            if (te_DosyaYolu.EditValue != null)
                dialog.SelectedPath = te_DosyaYolu.EditValue.ToString();
            if (dialog.ShowDialog() == DialogResult.OK)
                te_DosyaYolu.EditValue = dialog.SelectedPath;
        }
    }
}