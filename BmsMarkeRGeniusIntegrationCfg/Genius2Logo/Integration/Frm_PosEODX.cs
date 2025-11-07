using BmsMarkeRGeniusIntegrationCfg;
using BmsMarkeRGeniusIntegrationLibrary;
using BmsMarkeRGeniusIntegrationLibrary.METHODS.MODELS;
using BmsMarkeRGeniusIntegrationCfg;
using BmsMarkeRGeniusIntegrationLibrary.METHODS.MODELS;
using BmsMarkeRGeniusIntegrationLibrary;
using DevExpress.Office.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Integration.BmsMarkeRGeniusIntegrationCfg
{
    public partial class Frm_PosEOD : DevExpress.XtraEditors.XtraForm
    {
        CONFIG CFG;
        //string TABLENAME_Sales = "BMSF_XXX_MarkeRGenius_Sales";
        //string TABLENAME_Sales_WithCustomer = "BMSF_XXX_MarkeRGenius_Sales_WithCustomer";
        string QueryFile_Sales = @"Queries\Sales.sql";
        string QueryFile_Sales_WithCustomer = @"Queries\Sales_WithCustomer.sql";
        public Frm_PosEOD(string HEADERNAME)
        {
            InitializeComponent();
            this.Text = HEADERNAME;
            CFG = CONFIG_HELPER.GET_CONFIG();
            //TABLENAME_Sales = TABLENAME_Sales.Replace("XXX", CFG.FIRMNR);
            //TABLENAME_Sales_WithCustomer = TABLENAME_Sales_WithCustomer.Replace("XXX", CFG.FIRMNR);
            de_DateStart.DateTime = DateTime.Now.Date;
            de_DateEnd.DateTime = DateTime.Now.Date;
            //de_DateStart.DateTime = new DateTime(2023, 12, 26);
            //de_DateEnd.DateTime = new DateTime(2023, 12, 26);
            loadLookupEdits();
            loadCheckedComboBoxEdit();
        }

        private void loadLookupEdits()
        {
            //le_InvoiceClient
            le_InvoiceClient.Properties.DataSource = HELPER.SqlSelectLogo($@"SELECT NR,NAME FROM BMS_{CFG.FIRMNR}_MarkeRGenius_InvoiceClient ORDER BY NR");
            le_InvoiceClient.Properties.ValueMember = "NR";
            le_InvoiceClient.Properties.DisplayMember = "NAME";
            le_InvoiceClient.Properties.PopulateColumns();

            //le_ReturnClient
            le_ReturnClient.Properties.DataSource = HELPER.SqlSelectLogo($@"SELECT NR,NAME FROM BMS_{CFG.FIRMNR}_MarkeRGenius_ReturnClient ORDER BY NR");
            le_ReturnClient.Properties.ValueMember = "NR";
            le_ReturnClient.Properties.DisplayMember = "NAME";
            le_ReturnClient.Properties.PopulateColumns();



            //object valueOfIc = HELPER.SqlSelectLogo($@"SELECT TOP 1 NR FROM BMS_{CFG.FIRMNR}_MarkeRGenius_InvoiceClient ORDER BY NR").Rows[0][0].ToString();
            //le_InvoiceClient.ItemIndex = le_InvoiceClient.Properties.GetDataSourceRowIndex("NR", valueOfIc);
            //object valueOfRc = HELPER.SqlSelectLogo($@"SELECT TOP 1 NR FROM BMS_{CFG.FIRMNR}_MarkeRGenius_ReturnClient ORDER BY NR").Rows[0][0].ToString();
            //le_ReturnClient.ItemIndex = le_ReturnClient.Properties.GetDataSourceRowIndex("NR", valueOfRc);
            //object valueOfB = HELPER.SqlSelectLogo($@"SELECT TOP 1 NR FROM BMS_{CFG.FIRMNR}_MarkeRGenius_Branch ORDER BY NR").Rows[0][0].ToString();
            //le_Branch.ItemIndex = le_Branch.Properties.GetDataSourceRowIndex("NR", valueOfB);
        }
        private void loadCheckedComboBoxEdit()
        {
            DataTable branch = HELPER.SqlSelectLogo($@"SELECT NR,CAST(NR AS VARCHAR)+'-'+NAME AS NAME FROM BMS_{CFG.FIRMNR}_MarkeRGenius_Branch ORDER BY NR");
            foreach (DataRow item in branch.Rows)
                ccbe_Branch.Properties.Items.Add(item["NR"].ToString(), item["NAME"].ToString());
            ccbe_Branch.CheckAll();
        }
        private void InitializeData(object sender, EventArgs e)
        {
            List<Bms_Errors> errorList = new List<Bms_Errors>();
            SplashScreenManager.ShowForm(this, typeof(PROGRESSFORM), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("LÜTFEN BEKLEYİN.");
            SplashScreenManager.Default.SetWaitFormDescription("");
            string sqlFormattedDateStart = de_DateStart.DateTime.ToString("MM/dd/yyyy") + " 00:00:00";
            string sqlFormattedDateEnd = de_DateEnd.DateTime.ToString("MM/dd/yyyy") + " 23:59:59";

            var checkedBranches = ccbe_Branch.Properties.Items.GetCheckedValues().ToList();

            existenceController(checkedBranches, sqlFormattedDateStart, sqlFormattedDateEnd);

            try
            {
                string strLogin = HELPER.LOGO_LOGIN(CFG.LOBJECTDEFAULTUSERNAME, CFG.LOBJECTDEFAULTPASSWORD, int.Parse(CFG.FIRMNR));
                if (strLogin != "") throw new Exception(strLogin);
                foreach (string branch in checkedBranches)
                {
                    if (ce_OnlySalesWithCustomer.Checked)
                        Sales_WithCustomer(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
                    else
                    {
                        Sales(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
                        Sales_WithCustomer(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HELPER.LOGO_LOGOUT();
            }

            if (errorList.Count > 0)
            {
                SplashScreenManager.CloseForm(false);
                string errorText = "İşlem Hatalarla Tamamlandı.";
                XtraMessageBox.Show(errorText, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FRM_Errors frm = new FRM_Errors(errorList);
                frm.Show();
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("İşlem tamamlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void existenceController(List<object> checkedBranches, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            List<Bms_Errors> errorList = new List<Bms_Errors>();
            foreach (string branch in checkedBranches)
            {
                //from sqlFormattedDateStart to sqlFormattedDateEnd control if exists
                for (DateTime date = de_DateStart.DateTime; date <= de_DateEnd.DateTime; date = date.AddDays(1))
                {
                    string sqlFormattedDate = date.ToString("MM/dd/yyyy") + " 00:00:00";
                    string sqlQueryHeader = $@"SELECT TOP 1 FICHENO FROM LG_{CFG.FIRMNR}_01_INVOICE II WHERE II.TIME_=0 AND II.POSTRANSFERINFO=1 AND II.CYPHCODE='BMS' AND II.DATE_ = '{sqlFormattedDate}' AND II.BRANCH = {branch}";
                    DataTable fhl = HELPER.SqlSelectLogo(sqlQueryHeader);
                    if (fhl.Rows.Count > 0)
                    {
                        errorList.Add(new Bms_Errors()
                        {
                            BRANCH = int.Parse(branch),
                            POS = 0,
                            FTYPE = "",
                            DATE_ = date,
                            ERRORMESSAGE = "Bu tarih için daha önce günsonu yapılmıştır. Logo Fatura No : " + fhl.Rows[0][0].ToString()
                        });
                    }
                }
            }
            if (errorList.Count > 0)
            {
                SplashScreenManager.CloseForm(false);
                string errorText = "İşlem Hatalarla Tamamlandı.";
                //XtraMessageBox.Show(errorText, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FRM_Errors frm = new FRM_Errors(errorList);
                frm.Show();
                throw new Exception(errorText);
            }
        }

        private void Sales(List<Bms_Errors> errorList, string branch, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            string ipFromBranch = HELPER.SqlSelectLogo($@"SELECT TOP 1 Ip FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping WHERE LogoBranch = {branch}").Rows[0][0].ToString();
            string sqlQuery = File.ReadAllText(QueryFile_Sales);
            //Fix for ip
            sqlQuery = sqlQuery.Replace("192.168.5.103", ipFromBranch);
            //Fix for firmnr
            sqlQuery = sqlQuery.Replace("_124_", "_" + CFG.FIRMNR + "_");
            //Fix for date1
            sqlQuery = sqlQuery.Replace("@DATE1", "'" + sqlFormattedDateStart + "'");
            //Fix for date2
            sqlQuery = sqlQuery.Replace("@DATE2", "'" + sqlFormattedDateEnd + "'");
            //add branch to query
            sqlQuery = sqlQuery + $@" AND FK_STORE = (SELECT top 1 MM.PosBranch FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping MM where LogoBranch={branch}) ";
            //Fix for distinct
            string sqlQueryHeader = Regex.Replace(sqlQuery, @"\/\*REPLACE BEGIN FOR DISTINCT\*\/.*?\/\*REPLACE END FOR DISTINCT\*\/", $" DISTINCT DATE_=G3B.TDATE,BRANCH=(SELECT MM.LogoBranch FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping MM where PosBranch=FK_STORE),POS=FK_POS,FTYPE ", RegexOptions.Singleline);
            /*string sqlQueryHeader = $@"SELECT DISTINCT DATE_,BRANCH,POS,FTYPE FROM {TABLENAME_Sales}('{sqlFormattedDateStart}','{sqlFormattedDateEnd}') WHERE BRANCH = {branch}";*/
            List<Bms_Fiche_Header> fhl = HELPER.DataTableToList<Bms_Fiche_Header>(HELPER.SqlSelectLogo(sqlQueryHeader));
            foreach (var item in fhl)
            {
                string result = "";
                string sqlQueryDetail = sqlQuery + $@" AND FK_POS = {item.POS} AND FTYPE = '{item.FTYPE}' and G3B.TDATE = CAST('{item.DATE_.ToString("yyyyMMdd")}' AS DATE) ";

                //string sqlQueryDetail = $@"SELECT * FROM {TABLENAME_Sales} ('{sqlFormattedDateStartForDetail}','{sqlFormattedDateEndForDetail}') WHERE BRANCH = {branch} AND POS = {item.POS} AND FTYPE = '{item.FTYPE}'";
                List<Bms_Fiche_Detail> fdl = HELPER.DataTableToList<Bms_Fiche_Detail>(HELPER.SqlSelectLogo(sqlQueryDetail));
                //INSERT WITH LOGO
                //string BRANCH = "0";
                //try { BRANCH = le_Branch.EditValue.ToString(); } catch { }
                if (item.FTYPE == "SATIS")
                {
                    //SATIS FATURASI
                    result = HELPER.InsertInvoice(le_InvoiceClient.EditValue.ToString(), branch, item, fdl, false, CFG.FIRMNR);
                }
                else if (item.FTYPE == "IADE")
                {
                    //IADE FATURASI
                    result = HELPER.InsertReturnInvoice(le_ReturnClient.EditValue.ToString(), branch, item, fdl, false, CFG.FIRMNR);
                }
                if (result != "ok")
                {
                    errorList.Add(new Bms_Errors()
                    {
                        BRANCH = item.BRANCH,
                        POS = item.POS,
                        FTYPE = item.FTYPE,
                        DATE_ = item.DATE_,
                        ERRORMESSAGE = result
                    });
                }
            }
        }
        private void Sales_WithCustomer(List<Bms_Errors> errorList, string branch, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            string ipFromBranch = HELPER.SqlSelectLogo($@"SELECT TOP 1 Ip FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping WHERE LogoBranch = {branch}").Rows[0][0].ToString();
            string sqlQuery = File.ReadAllText(QueryFile_Sales_WithCustomer);
            //Fix for ip
            sqlQuery = sqlQuery.Replace("192.168.5.103", ipFromBranch);
            //Fix for firmnr
            sqlQuery = sqlQuery.Replace("_124_", "_" + CFG.FIRMNR + "_");
            //Fix for date1
            sqlQuery = sqlQuery.Replace("@DATE1", "'" + sqlFormattedDateStart + "'");
            //Fix for date2
            sqlQuery = sqlQuery.Replace("@DATE2", "'" + sqlFormattedDateEnd + "'");
            //add branch to query
            sqlQuery = sqlQuery + $@" AND FK_STORE = (SELECT top 1 MM.PosBranch FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping MM where LogoBranch={branch}) ";
            //Fix for distinct
            string sqlQueryHeader = Regex.Replace(sqlQuery, @"\/\*REPLACE BEGIN FOR DISTINCT\*\/.*?\/\*REPLACE END FOR DISTINCT\*\/", $" DISTINCT FICHE_ID=CAST(FICHE_ID AS VARCHAR),DATE_=G3B.TDATE,BRANCH=(SELECT MM.LogoBranch FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping MM where PosBranch=FK_STORE),POS=FK_POS,FTYPE,DOCUMENT_NO,CUSTOMER_CODE=CASE WHEN CUSTOMER_CODE='' THEN (SELECT TOP 1 DD.Value FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Default DD WHERE DD.Description='YAZARKARA FISLI CARI BOS LOGO CARISI') ELSE CUSTOMER_CODE END,CUSTOMER_NAME ", RegexOptions.Singleline);
            //string sqlQueryHeader = $@"SELECT DISTINCT FICHE_ID,DATE_,BRANCH,POS,FTYPE,CUSTOMER_CODE,CUSTOMER_NAME FROM {TABLENAME_Sales_WithCustomer}('{sqlFormattedDateStart}','{sqlFormattedDateEnd}') WHERE BRANCH = {branch}";
            List<Bms_Fiche_Header> fhl = HELPER.DataTableToList<Bms_Fiche_Header>(HELPER.SqlSelectLogo(sqlQueryHeader));
            foreach (var item in fhl)
            {
                string result = "";
                string sqlQueryDetail = sqlQuery + $@" AND FK_POS = {item.POS} AND FTYPE = '{item.FTYPE}' and CAST(FICHE_ID AS VARCHAR) = '{item.FICHE_ID}' AND DOCUMENT_NO = '{item.DOCUMENT_NO}' AND (CASE WHEN CUSTOMER_CODE='' THEN (SELECT TOP 1 DD.Value FROM Bms_{CFG.FIRMNR}_ErdenerGeniusIntegration_Default DD WHERE DD.Description='YAZARKARA FISLI CARI BOS LOGO CARISI') ELSE CUSTOMER_CODE END) = '{item.CUSTOMER_CODE}' AND CUSTOMER_NAME = '{item.CUSTOMER_NAME}' AND G3B.TDATE = CAST('{item.DATE_.ToString("yyyyMMdd")}' AS DATE) ";

                //string sqlQueryDetail = $@"SELECT * FROM {TABLENAME_Sales_WithCustomer} ('{sqlFormattedDateStartForDetail}','{sqlFormattedDateEndForDetail}') WHERE BRANCH = {branch} AND POS = {item.POS} AND FTYPE = '{item.FTYPE}' AND FICHE_ID='{item.FICHE_ID}' AND CUSTOMER_CODE = '{item.CUSTOMER_CODE}' AND CUSTOMER_NAME = '{item.CUSTOMER_NAME}'";
                List<Bms_Fiche_Detail> fdl = HELPER.DataTableToList<Bms_Fiche_Detail>(HELPER.SqlSelectLogo(sqlQueryDetail));
                //INSERT WITH LOGO
                //string BRANCH = "0";
                //try { BRANCH = le_Branch.EditValue.ToString(); } catch { }
                if (item.FTYPE == "SATIS")
                {
                    //SATIS FATURASI
                    result = HELPER.InsertInvoice(le_InvoiceClient.EditValue.ToString(), branch, item, fdl, true, CFG.FIRMNR);
                }
                else if (item.FTYPE == "IADE")
                {
                    //IADE FATURASI
                    result = HELPER.InsertReturnInvoice(le_ReturnClient.EditValue.ToString(), branch, item, fdl, true, CFG.FIRMNR);
                }
                if (result != "ok")
                {
                    errorList.Add(new Bms_Errors()
                    {
                        BRANCH = item.BRANCH,
                        POS = item.POS,
                        FTYPE = item.FTYPE,
                        DATE_ = item.DATE_,
                        ERRORMESSAGE = result
                    });
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //if le_Branch.EditValue == null or le_InvoiceClient.EditValue == null or le_ReturnClient.EditValue == null
            if (le_InvoiceClient.EditValue == null || le_ReturnClient.EditValue == null || (ccbe_Branch.EditValue == null || ccbe_Branch.Properties.Items.GetCheckedValues().Count == 0))
            {
                XtraMessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            InitializeData(null, null);
        }

        private void sb_SaveToBm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}