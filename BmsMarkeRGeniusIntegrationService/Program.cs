using BmsMarkeRGeniusIntegrationLibrary;
using BmsMarkeRGeniusIntegrationLibrary.METHODS.MODELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BmsMarkeRGeniusIntegrationService
{
    internal class Program
    {
        static CONFIG CFG;
        //static string TABLENAME_Sales = "BMSF_XXX_MarkeRGenius_Sales";
        //static string TABLENAME_SalesWithCustomer = "BMSF_XXX_MarkeRGenius_Sales_WithCustomer";
        static string QueryFile_Sales = @"Queries\Sales.sql";
        static string QueryFile_Sales_WithCustomer = @"Queries\Sales_WithCustomer.sql";
        static string QueryFile_Payments = @"Queries\Payments.sql";
        static string _directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        [STAThread]
        static void Main(string[] args)
        {
            HELPER.LOGYAZ("SERVICE STARTED!", null);
            CFG = CONFIG_HELPER.GET_CONFIG();
            //TABLENAME_Sales = TABLENAME_Sales.Replace("XXX", CFG.FIRMNR);
            //TABLENAME_SalesWithCustomer = TABLENAME_SalesWithCustomer.Replace("XXX", CFG.FIRMNR);
            if (CFG == null)
            {
                Console.WriteLine("CONFIG ERROR.");
                Console.ReadLine();
                return;
            }
            try
            {
                HELPER.LOGYAZ("Integrations Started", null);
                Console.WriteLine("Integrations Started");
                HELPER.LOBJECTSKILLER();
                DataTable branch = HELPER.SqlSelectLogo($@"SELECT NR,CAST(NR AS VARCHAR)+'-'+NAME AS NAME FROM BMS_{CFG.FIRMNR}_MarkeRGenius_Branch ORDER BY NR");
                foreach (DataRow item in branch.Rows)
                {
                    string branchNr = item["NR"].ToString();
                    string sqlFormattedDateStart = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy") + " 00:00:00";
                    string sqlFormattedDateEnd = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy") + " 23:59:59";

                    Console.WriteLine("G3 Integration Sales Started"); HELPER.LOGYAZ("G3 Integration Sales Started", null);
                    try { G3IntegrationSales(branchNr, sqlFormattedDateStart, sqlFormattedDateEnd); } catch { }
                    Console.WriteLine("G3 Integration Sales With Customer Started"); HELPER.LOGYAZ("G3 Integration Sales With Customer Started", null);
                    try { G3IntegrationSalesWithCustomer(branchNr, sqlFormattedDateStart, sqlFormattedDateEnd); } catch { }
                    Console.WriteLine("G3 Integration Payments Started"); HELPER.LOGYAZ("G3 Integration Payments Started", null);
                    try { G3IntegrationPayments(branchNr, sqlFormattedDateStart, sqlFormattedDateEnd); } catch { }
                    Console.WriteLine("G3 Integration DebtClose Started"); HELPER.LOGYAZ("G3 Integration DebtClose Started", null);
                    try { G3IntegrationDebtClose(branchNr, sqlFormattedDateStart, sqlFormattedDateEnd); } catch { }
                }
                try { HELPER.LOGO_LOGOUT(); } catch { }
                HELPER.LOGYAZ("Integrations Finished", null);
            }
            catch (Exception ex) { HELPER.LOGYAZ("HATA!", ex); }
            HELPER.LOGYAZ("SERVICE FINISHED!", null);
        }

        private static void G3IntegrationDebtClose(string branchNr, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            string strLogin = HELPER.LOGO_LOGIN(CFG.LOBJECTDEFAULTUSERNAME, CFG.LOBJECTDEFAULTPASSWORD, int.Parse(CFG.FIRMNR));
            if (strLogin != "") throw new Exception(strLogin);
            try
            {
                List<Bms_Errors> errorList = new List<Bms_Errors>();
                DebtClose(errorList, branchNr, sqlFormattedDateStart, sqlFormattedDateEnd, 1);
                DebtClose(errorList, branchNr, sqlFormattedDateStart, sqlFormattedDateEnd, 2);
                DebtClose(errorList, branchNr, sqlFormattedDateStart, sqlFormattedDateEnd, 3);
            }
            catch (Exception ex) { HELPER.LOGYAZ("HATA!", ex); }
            finally
            {
                HELPER.LOGO_LOGOUT();
            }
        }

        private static void DebtClose(List<Bms_Errors> errorList, string branch, string sqlFormattedDateStart, string sqlFormattedDateEnd, int count)
        {

            string sqlQuery = $@"SELECT PAYTRANS_INVOICE,BRANCH,DOCODE,DATE_INVOICE,CLIENTREF,SPECODE,PAYTRANS_TOTAL from BMS_{CFG.FIRMNR}_MarkeRGenius_DebtClose_Invoice where BRANCH = {branch} AND DATE_INVOICE BETWEEN '{sqlFormattedDateStart}' AND '{sqlFormattedDateEnd}' ORDER BY PAYTRANS_TOTAL DESC";
            DataTable fhl = HELPER.SqlSelectLogo(sqlQuery);
            foreach (DataRow item in fhl.Rows)
            {
                double percantage = (double)fhl.Rows.IndexOf(item) / (double)fhl.Rows.Count;
                string result = "";
                int PAYTRANS_INVOICE = int.Parse(item["PAYTRANS_INVOICE"].ToString());
                int BRANCH = int.Parse(item["BRANCH"].ToString());
                string DOCODE = item["DOCODE"].ToString();
                DateTime DATE_INVOICE = DateTime.Parse(item["DATE_INVOICE"].ToString());
                int CLIENTREF = int.Parse(item["CLIENTREF"].ToString());
                string SPECODE = item["SPECODE"].ToString();
                double PAYTRANS_TOTAL = double.Parse(item["PAYTRANS_TOTAL"].ToString());
                //get CSROLLS
                string sqlQueryCSROLLS = $@"SELECT PAYTRANS_CSROLL FROM BMS_{CFG.FIRMNR}_MarkeRGenius_DebtClose_Csroll WHERE BRANCH = {BRANCH} AND DOCODE = '{DOCODE}' AND DATE_CSROLL = '{DATE_INVOICE.ToString("yyyy-MM-dd")}' AND CLIENTREF = {CLIENTREF} AND SPECODE = '{SPECODE}' ORDER BY PAYTRANS_TOTAL DESC";
                DataTable fhlCSROLLS = HELPER.SqlSelectLogo(sqlQueryCSROLLS);
                if (fhlCSROLLS.Rows.Count > 0)
                    foreach (DataRow itemCSROLLS in fhlCSROLLS.Rows)
                    {
                        int PAYTRANS_CSROLL = int.Parse(itemCSROLLS["PAYTRANS_CSROLL"].ToString());
                        if (!HELPER.AppUnity.DebtClose(PAYTRANS_INVOICE, PAYTRANS_CSROLL))
                        {
                            if (count == 3)
                                errorList.Add(new Bms_Errors()
                                {
                                    BRANCH = BRANCH,
                                    POS = 0,
                                    FTYPE = "DebtClose",
                                    DATE_ = DATE_INVOICE,
                                    ERRORMESSAGE = $@"Borç kapatma işlemi yapılamadı.PAYTRANS_INVOICE:{PAYTRANS_INVOICE}, PAYTRANS_CSROLL:{PAYTRANS_CSROLL}, LOGOOBJECTS SONUC:" + HELPER.AppUnity.GetLastError() + "--" + HELPER.AppUnity.GetLastErrorString()
                                });
                        }
                    }
                string sqlQueryKSLINES = $@"SELECT PAYTRANS_KSLINES FROM BMS_{CFG.FIRMNR}_MarkeRGenius_DebtClose_Kslines WHERE BRANCH = {BRANCH} AND DOCODE = '{DOCODE}' AND DATE_KSLINES = '{DATE_INVOICE.ToString("yyyy-MM-dd")}' AND CLIENTREF = {CLIENTREF} AND SPECODE = '{SPECODE}' ORDER BY PAYTRANS_TOTAL DESC";
                DataTable fhlKSLINES = HELPER.SqlSelectLogo(sqlQueryKSLINES);
                if (fhlKSLINES.Rows.Count > 0)
                    foreach (DataRow itemKSLINES in fhlKSLINES.Rows)
                    {
                        int PAYTRANS_KSLINES = int.Parse(itemKSLINES["PAYTRANS_KSLINES"].ToString());
                        if (!HELPER.AppUnity.DebtClose(PAYTRANS_INVOICE, PAYTRANS_KSLINES))
                        {
                            if (count == 3)
                                errorList.Add(new Bms_Errors()
                                {
                                    BRANCH = BRANCH,
                                    POS = 0,
                                    FTYPE = "DebtClose",
                                    DATE_ = DATE_INVOICE,
                                    ERRORMESSAGE = $@"Borç kapatma işlemi yapılamadı.PAYTRANS_INVOICE:{PAYTRANS_INVOICE}, PAYTRANS_KSLINES:{PAYTRANS_KSLINES}, LOGOOBJECTS SONUC:" + HELPER.AppUnity.GetLastError() + "--" + HELPER.AppUnity.GetLastErrorString()
                                });
                        }
                    }
                GC.Collect();
            }
            if (errorList.Count > 0)
            {
                for (int i = 0; i < errorList.Count; i++)
                {
                    HELPER.LOGYAZ("G3IntegrationDebtClose ERRORS! " + errorList[i].BRANCH + " " + errorList[i].POS + " " + errorList[i].FTYPE + " " + errorList[i].DATE_ + " " + errorList[i].ERRORMESSAGE, null);
                }
            }
        }

        private static void G3IntegrationSales(string branch, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            List<Bms_Errors> errorList = new List<Bms_Errors>();
            string InvoiceClient = HELPER.SqlSelectLogo($@"SELECT NR,NAME FROM BMS_{CFG.FIRMNR}_MarkeRGenius_InvoiceClient ORDER BY NR").Rows[0][0].ToString();
            string ReturnClient = HELPER.SqlSelectLogo($@"SELECT NR,NAME FROM BMS_{CFG.FIRMNR}_MarkeRGenius_ReturnClient ORDER BY NR").Rows[0][0].ToString();
            string ipFromBranch = HELPER.SqlSelectLogo($@"SELECT TOP 1 Ip FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping WHERE LogoBranch = {branch}").Rows[0][0].ToString();
            string sqlQuery = File.ReadAllText(_directory + "\\" + QueryFile_Sales);
            //Fix for ip
            sqlQuery = sqlQuery.Replace(CFG.OTHERSERVER, ipFromBranch);
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
            try
            {
                string strLogin = HELPER.LOGO_LOGIN(CFG.LOBJECTDEFAULTUSERNAME, CFG.LOBJECTDEFAULTPASSWORD, int.Parse(CFG.FIRMNR));
                if (strLogin != "") throw new Exception(strLogin);
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
                        result = HELPER.InsertInvoice(InvoiceClient, branch, item, fdl, false, CFG.FIRMNR);
                    }
                    else if (item.FTYPE == "IADE")
                    {
                        //IADE FATURASI
                        result = HELPER.InsertReturnInvoice(ReturnClient, branch, item, fdl, false, CFG.FIRMNR,"BMS");
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
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                HELPER.LOGYAZ("HATA!", ex);
                for (int i = 0; i < errorList.Count; i++)
                {
                    HELPER.LOGYAZ("G3IntegrationSales ERRORS! " + errorList[i].BRANCH + " " + errorList[i].POS + " " + errorList[i].FTYPE + " " + errorList[i].DATE_ + " " + errorList[i].ERRORMESSAGE, null);
                }
            }
            finally
            {
                HELPER.LOGO_LOGOUT();
            }
        }
        private static void G3IntegrationSalesWithCustomer(string branch, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            List<Bms_Errors> errorList = new List<Bms_Errors>();
            string InvoiceClient = HELPER.SqlSelectLogo($@"SELECT NR,NAME FROM BMS_{CFG.FIRMNR}_MarkeRGenius_InvoiceClient ORDER BY NR").Rows[0][0].ToString();
            string ReturnClient = HELPER.SqlSelectLogo($@"SELECT NR,NAME FROM BMS_{CFG.FIRMNR}_MarkeRGenius_ReturnClient ORDER BY NR").Rows[0][0].ToString();
            string ipFromBranch = HELPER.SqlSelectLogo($@"SELECT TOP 1 Ip FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping WHERE LogoBranch = {branch}").Rows[0][0].ToString();

            string sqlQuery = File.ReadAllText(_directory + "\\" + QueryFile_Sales_WithCustomer);
            //Fix for ip
            sqlQuery = sqlQuery.Replace(CFG.OTHERSERVER, ipFromBranch);
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
            try
            {
                string strLogin = HELPER.LOGO_LOGIN(CFG.LOBJECTDEFAULTUSERNAME, CFG.LOBJECTDEFAULTPASSWORD, int.Parse(CFG.FIRMNR));
                if (strLogin != "") throw new Exception(strLogin);

                foreach (var item in fhl)
                {
                    string result = "";
                    string sqlQueryDetail = sqlQuery + $@" AND FK_POS = {item.POS} AND FTYPE = '{item.FTYPE}' and CAST(FICHE_ID AS VARCHAR) = '{item.FICHE_ID}' AND (CASE WHEN CUSTOMER_CODE='' THEN (SELECT TOP 1 DD.Value FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Default DD WHERE DD.Description='YAZARKARA FISLI CARI BOS LOGO CARISI') ELSE CUSTOMER_CODE END) = '{item.CUSTOMER_CODE}' AND DOCUMENT_NO = '{item.DOCUMENT_NO}' AND CUSTOMER_NAME = '{item.CUSTOMER_NAME}' AND G3B.TDATE = CAST('{item.DATE_.ToString("yyyyMMdd")}' AS DATE) ";

                    //string sqlQueryDetail = $@"SELECT * FROM {TABLENAME_Sales_WithCustomer} ('{sqlFormattedDateStartForDetail}','{sqlFormattedDateEndForDetail}') WHERE BRANCH = {branch} AND POS = {item.POS} AND FTYPE = '{item.FTYPE}' AND FICHE_ID='{item.FICHE_ID}' AND CUSTOMER_CODE = '{item.CUSTOMER_CODE}' AND CUSTOMER_NAME = '{item.CUSTOMER_NAME}'";
                    List<Bms_Fiche_Detail> fdl = HELPER.DataTableToList<Bms_Fiche_Detail>(HELPER.SqlSelectLogo(sqlQueryDetail));
                    //INSERT WITH LOGO
                    //string BRANCH = "0";
                    //try { BRANCH = le_Branch.EditValue.ToString(); } catch { }
                    if (item.FTYPE == "SATIS")
                    {
                        //SATIS FATURASI
                        result = HELPER.InsertInvoice(InvoiceClient, branch, item, fdl, true, CFG.FIRMNR);
                    }
                    else if (item.FTYPE == "IADE")
                    {
                        //IADE FATURASI
                        result = HELPER.InsertReturnInvoice(ReturnClient, branch, item, fdl, true, CFG.FIRMNR,"BMS");
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
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                HELPER.LOGYAZ("HATA!", ex);
                for (int i = 0; i < errorList.Count; i++)
                {
                    HELPER.LOGYAZ("G3IntegrationSalesWithCustomer ERRORS! " + errorList[i].BRANCH + " " + errorList[i].POS + " " + errorList[i].FTYPE + " " + errorList[i].DATE_ + " " + errorList[i].ERRORMESSAGE, null);
                }
            }
            finally
            {
                HELPER.LOGO_LOGOUT();
            }
        }
        private static void G3IntegrationPayments(string branch, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            List<Bms_Errors> errorList = new List<Bms_Errors>();
            string InvoiceClient = HELPER.SqlSelectLogo($@"SELECT NR,NAME FROM BMS_{CFG.FIRMNR}_MarkeRGenius_InvoiceClient ORDER BY NR").Rows[0][0].ToString();
            string ReturnClient = HELPER.SqlSelectLogo($@"SELECT NR,NAME FROM BMS_{CFG.FIRMNR}_MarkeRGenius_ReturnClient ORDER BY NR").Rows[0][0].ToString();
            string ipFromBranch = HELPER.SqlSelectLogo($@"SELECT TOP 1 Ip FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping WHERE LogoBranch = {branch}").Rows[0][0].ToString();

            string sqlQuery = File.ReadAllText(_directory + "\\" + QueryFile_Payments);
            //Fix for ip
            sqlQuery = sqlQuery.Replace(CFG.OTHERSERVER, ipFromBranch);
            //Fix for firmnr
            sqlQuery = sqlQuery.Replace("_124_", "_" + CFG.FIRMNR + "_");
            //Fix for date1
            sqlQuery = sqlQuery.Replace("@DATE1", "'" + sqlFormattedDateStart + "'");
            //Fix for date2
            sqlQuery = sqlQuery.Replace("@DATE2", "'" + sqlFormattedDateEnd + "'");
            //add branch to query
            sqlQuery = sqlQuery.Replace(" MM.Branch=0", " MM.Branch=" + branch);
            sqlQuery = sqlQuery.Replace(" MM where LogoBranch=0", " MM where LogoBranch=" + branch);

            List<Bms_Fiche_Payment> fhl = HELPER.DataTableToList<Bms_Fiche_Payment>(HELPER.SqlSelectLogo(sqlQuery));
            try
            {
                string strLogin = HELPER.LOGO_LOGIN(CFG.LOBJECTDEFAULTUSERNAME, CFG.LOBJECTDEFAULTPASSWORD, int.Parse(CFG.FIRMNR));
                if (strLogin != "") throw new Exception(strLogin);

                foreach (var item in fhl)
                {
                    if (string.IsNullOrEmpty(item.DOCUMENT_NO) && string.IsNullOrEmpty(item.CUSTOMER_CODE))
                    {
                        item.CUSTOMER_CODE = InvoiceClient;
                        item.CUSTOMER_NAME = HELPER.SqlSelectLogo($"SELECT DEFINITION_ FROM LG_{CFG.FIRMNR}_CLCARD WHERE CODE='{item.CUSTOMER_CODE}'").Rows[0][0].ToString();
                    }
                    string result = "";
                    if (string.IsNullOrEmpty(item.LOGO_FICHE_TYPE))
                    {
                        result = "LOGO_FICHE_TYPE is null or empty";
                    }
                    else if (item.LOGO_FICHE_TYPE == "Veresiye")
                    {
                        continue;
                    }
                    else if (item.LOGO_FICHE_TYPE == "Cek Girisi")
                    {
                        result = HELPER.InsertCheque(branch, item, CFG.FIRMNR);
                    }
                    else if (item.LOGO_FICHE_TYPE == "CH Kredi Karti" || item.LOGO_FICHE_TYPE == "CH Kredi Karti Iade" || item.LOGO_FICHE_TYPE == "CH Borc" || item.LOGO_FICHE_TYPE == "CH Alacak")
                    {
                        result = HELPER.InsertCHFiche(branch, item, CFG.FIRMNR);
                    }
                    else if (item.LOGO_FICHE_TYPE == "Kasa Tahsilat" || item.LOGO_FICHE_TYPE == "Kasa Odeme")
                    {
                        result = HELPER.InsertKsFiche(branch, item, CFG.FIRMNR);
                    }
                    if (result != "ok")
                    {
                        errorList.Add(new Bms_Errors()
                        {
                            BRANCH = item.LOGO_BRANCH,
                            POS = item.POS,
                            FTYPE = "Payment:" + item.LOGO_FICHE_TYPE,
                            DATE_ = item.DATE_,
                            ERRORMESSAGE = result
                        });
                    }
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                HELPER.LOGYAZ("HATA!", ex);
                for (int i = 0; i < errorList.Count; i++)
                {
                    HELPER.LOGYAZ("G3IntegrationSPayments ERRORS! " + errorList[i].BRANCH + " " + errorList[i].POS + " " + errorList[i].FTYPE + " " + errorList[i].DATE_ + " " + errorList[i].ERRORMESSAGE, null);
                }
            }
            finally
            {
                HELPER.LOGO_LOGOUT();
            }
        }
    }
}
