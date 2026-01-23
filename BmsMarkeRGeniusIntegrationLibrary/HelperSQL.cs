using BmsMarkeRGeniusIntegrationLibrary.METHODS.MODELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BmsMarkeRGeniusIntegrationLibrary.METHODS.MODELS;
using DevExpress.XtraGrid.Views.Grid;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using UnityObjects;
using static BmsMarkeRGeniusIntegrationLibrary.HELPER;
using System.Threading.Tasks;

namespace BmsMarkeRGeniusIntegrationLibrary
{
    internal class HelperSql
    {
        public static string InsertInvoice(string CARI_KOD, string BRANCH, Bms_Fiche_Header _BASLIK, List<Bms_Fiche_Detail> _DETAILS, bool withCustomer, string FIRMNR)
        {
            bool isCustomerExist = false;
            try { isCustomerExist = Convert.ToBoolean(SqlSelectLogo($"SELECT COUNT(*) FROM LG_{FIRMNR}_CLCARD WHERE CODE='{_BASLIK.CUSTOMER_CODE}'").Rows[0][0]); }
            catch (Exception ex)

            {
                LOGYAZ($"Müşteri Hatası \n Ürün: {_BASLIK.CUSTOMER_CODE} \n Ex: {ex.Message.ToString()}", null);
                isCustomerExist = false;
            }
            if (!isCustomerExist)
                _BASLIK.CUSTOMER_CODE = _BASLIK.CUSTOMER_CODE.TrimStart('0');
            HELPER.LOGYAZ(_BASLIK.DOCUMENT_NO.ToString(), null);
            //TRCODE TYPE 7 PERAKENDE SATIS FATURASI
            try
            {
                UnityObjects.Data invoice = NewObjectData(UnityObjects.DataObjectType.doSalesInvoice);
                invoice.New();
                invoice.DataFields.FieldByName("TYPE").Value = 7;
                invoice.DataFields.FieldByName("NUMBER").Value = "~";
                invoice.DataFields.FieldByName("DATE").Value = _BASLIK.DATE_;
                invoice.DataFields.FieldByName("AUXIL_CODE").Value = _BASLIK.POS.ToString();
                invoice.DataFields.FieldByName("DOC_NUMBER").Value = _BASLIK.DOCUMENT_NO.ToString();
                invoice.DataFields.FieldByName("DOC_TRACK_NR").Value = _BASLIK.POS.ToString();
                invoice.DataFields.FieldByName("NOTES6").Value = withCustomer ? _BASLIK.FICHE_ID : "";
                //invoice.DataFields.FieldByName("AUXIL_CODE").Value = "0";
                invoice.DataFields.FieldByName("AUTH_CODE").Value = "BMS";
                if (withCustomer)
                {
                    invoice.DataFields.FieldByName("ARP_CODE").Value = _BASLIK.CUSTOMER_CODE;
                    invoice.DataFields.FieldByName("DOC_NUMBER").Value = _BASLIK.DOCUMENT_NO.ToString();
                }
                else
                    invoice.DataFields.FieldByName("ARP_CODE").Value = CARI_KOD;
                invoice.DataFields.FieldByName("POST_FLAGS").Value = 243;
                //invoice.DataFields.FieldByName("RC_RATE").Value = getRateFromDB(20, FATURA_TARIHI, FIRMNR);
                //invoice.DataFields.FieldByName("PAYMENT_CODE").Value = _DETAILS.FirstOrDefault().TAKSITPLAN_KODU;
                invoice.DataFields.FieldByName("CURRSEL_TOTALS").Value = 1;
                invoice.DataFields.FieldByName("DEDUCTIONPART1").Value = 2;
                invoice.DataFields.FieldByName("DEDUCTIONPART2").Value = 3;
                invoice.DataFields.FieldByName("POS_TRANSFER_INFO").Value = 1;
                invoice.DataFields.FieldByName("DOC_DATE").Value = _BASLIK.DATE_;
                invoice.DataFields.FieldByName("EBOOK_DOCDATE").Value = _BASLIK.DATE_;
                invoice.DataFields.FieldByName("EBOOK_DOCTYPE").Value = 6;
                invoice.DataFields.FieldByName("EBOOK_EXPLAIN").Value = "Z Raporu";
                invoice.DataFields.FieldByName("EBOOK_NOPAY").Value = 1;
                invoice.DataFields.FieldByName("DIVISION").Value = BRANCH;
                UnityObjects.Lines transactions_lines = invoice.DataFields.FieldByName("TRANSACTIONS").Lines;
                foreach (var line in _DETAILS)
                {
                    if (string.IsNullOrEmpty(line.ITEMCODE))
                    {
                        LOGYAZ($"Boş ITEMCODE atlandı - Tarih: {_BASLIK.DATE_:yyyy-MM-dd}, POS: {_BASLIK.POS}, Ürün: {line.ITEMNAME}, Tutar: {line.LINETOTAL}, Miktar: {line.QUANTITY}", null);
                        continue;
                    }
                    transactions_lines.AppendLine();
                    double VatRate = 0;
                    try { VatRate = double.Parse(HELPER.SqlSelectLogo($"SELECT VAT FROM LG_{FIRMNR}_ITEMS WITH(NOLOCK) WHERE CODE='" + line.ITEMCODE + "'").Rows[0][0].ToString()); }
                    catch (Exception ex)
                    {
                        LOGYAZ($"Vat Hatası \n Ürün: {line.ITEMCODE} \n Ex: {ex.Message.ToString()}", null);
                        VatRate = 0;
                    }


                    double priceFromDecmailToDouble = 0;
                    try { priceFromDecmailToDouble = Convert.ToDouble(line.PRICE.ToString().Replace(".", ",")); } catch { }

                    double linetotalFromDecmailToDouble = 0;
                    try { linetotalFromDecmailToDouble = Convert.ToDouble(line.LINETOTAL.ToString().Replace(".", ",")); } catch { }

                    transactions_lines[transactions_lines.Count - 1].FieldByName("TYPE").Value = 0;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("MASTER_CODE").Value = line.ITEMCODE;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("QUANTITY").Value = line.QUANTITY;

                    if (Math.Abs(line.DISCOUNT_TOTAL) > 0)
                        transactions_lines[transactions_lines.Count - 1].FieldByName("PRICE").Value = priceFromDecmailToDouble;
                    else
                        transactions_lines[transactions_lines.Count - 1].FieldByName("PRICE").Value = linetotalFromDecmailToDouble / line.QUANTITY;

                    //transactions_lines[transactions_lines.Count - 1].FieldByName("TOTAL").Value = linetotalFromDecmailToDouble;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("UNIT_CODE").Value = line.ITEMUNIT;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("UNIT_CONV1").Value = 1;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("UNIT_CONV2").Value = 1;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("VAT_INCLUDED").Value = 1;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("VAT_RATE").Value = VatRate;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("BILLED").Value = 1;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("EDT_CURR").Value = 160;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("SALEMANCODE").Value = line.SALESMAN;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("MONTH").Value = _BASLIK.DATE_.Month;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("YEAR").Value = _BASLIK.DATE_.Year;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("AFFECT_RISK").Value = 1;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("BARCODE").Value = line.ITEMCODE;
                    if (Math.Abs(line.DISCOUNT_TOTAL) > 0)
                    {
                        //double dividationOfG3Bug = 1.00;
                        //if (line.DISCOUNT_TOTAL == line.CAMPAIGN_DISCOUNT)
                        //    dividationOfG3Bug = 2.00;
                        double discountFromDecmailToDouble = 0;
                        try { discountFromDecmailToDouble = Math.Abs(Convert.ToDouble(line.DISCOUNT_TOTAL.ToString().Replace(".", ","))); } catch { }

                        //vatRateFixed 

                        //discountFromDecmailToDouble = discountFromDecmailToDouble * (VatRate / 100 + 1);
                        discountFromDecmailToDouble = discountFromDecmailToDouble / ((100 + VatRate) / 100);
                        transactions_lines.AppendLine();
                        transactions_lines[transactions_lines.Count - 1].FieldByName("TYPE").Value = 2;
                        //transactions_lines[transactions_lines.Count - 1].FieldByName("DETAIL_LEVEL").Value = 1;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("DISCEXP_CALC").Value = 1;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("TOTAL").Value = discountFromDecmailToDouble /*/ dividationOfG3Bug*/;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("BILLED").Value = 1;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("MONTH").Value = _BASLIK.DATE_.Month;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("YEAR").Value = _BASLIK.DATE_.Year;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("AFFECT_RISK").Value = 1;
                    }
                }

                //invoice.FillAccCodes();
                invoice.ReCalculate();

                if (!invoice.Post())
                    throw new Exception(GetLastError(invoice));
                int LOGOLREF = Convert.ToInt32(invoice.DataFields.DBFieldByName("LOGICALREF").Value);
                DateTime LOGOINSERTDATE = DateTime.Now;
                if (LOGOLREF > 0)
                    return "ok";
                else return "notok";
            }
            catch (Exception E)
            {
                LOGYAZ("InsertInvoice", E);
                return E.Message;
            }
        }
        public static string InsertReturnInvoice(string CARI_KOD, string BRANCH, Bms_Fiche_Header _BASLIK, List<Bms_Fiche_Detail> _DETAILS, bool withCustomer, string FIRMNR, string AUTHCODE2)
        {
            //TRCODE TYPE 7 PERAKENDE SATIS FATURASI
            try
            {
                UnityObjects.Data invoice = NewObjectData(UnityObjects.DataObjectType.doSalesInvoice);
                invoice.New();
                invoice.DataFields.FieldByName("TYPE").Value = 2;
                invoice.DataFields.FieldByName("NUMBER").Value = "~";
                invoice.DataFields.FieldByName("DATE").Value = _BASLIK.DATE_;
                invoice.DataFields.FieldByName("AUXIL_CODE").Value = _BASLIK.POS.ToString();
                invoice.DataFields.FieldByName("DOC_NUMBER").Value = _BASLIK.FICHE_ID.ToString(); //DÜZELT
                invoice.DataFields.FieldByName("DOC_TRACK_NR").Value = _BASLIK.POS.ToString();
                invoice.DataFields.FieldByName("NOTES6").Value = withCustomer ? _BASLIK.FICHE_ID : "";
                //invoice.DataFields.FieldByName("AUXIL_CODE").Value = "0";
                invoice.DataFields.FieldByName("AUTH_CODE").Value = AUTHCODE2;
                if (withCustomer)
                {
                    invoice.DataFields.FieldByName("ARP_CODE").Value = _BASLIK.CUSTOMER_CODE;
                    invoice.DataFields.FieldByName("DOC_NUMBER").Value = _BASLIK.FICHE_ID.ToString(); //DÜZELT
                }
                else
                    invoice.DataFields.FieldByName("ARP_CODE").Value = CARI_KOD;
                invoice.DataFields.FieldByName("POST_FLAGS").Value = 243;
                //invoice.DataFields.FieldByName("RC_RATE").Value = getRateFromDB(20, FATURA_TARIHI, FIRMNR);
                //invoice.DataFields.FieldByName("PAYMENT_CODE").Value = _DETAILS.FirstOrDefault().TAKSITPLAN_KODU;
                invoice.DataFields.FieldByName("CURRSEL_TOTALS").Value = 1;
                invoice.DataFields.FieldByName("DEDUCTIONPART1").Value = 2;
                invoice.DataFields.FieldByName("DEDUCTIONPART2").Value = 3;
                invoice.DataFields.FieldByName("POS_TRANSFER_INFO").Value = 1;
                invoice.DataFields.FieldByName("DOC_DATE").Value = _BASLIK.DATE_;
                invoice.DataFields.FieldByName("EBOOK_DOCDATE").Value = _BASLIK.DATE_;
                invoice.DataFields.FieldByName("EBOOK_DOCTYPE").Value = 6;
                invoice.DataFields.FieldByName("EBOOK_EXPLAIN").Value = "Z Raporu";
                invoice.DataFields.FieldByName("EBOOK_NOPAY").Value = 1;
                invoice.DataFields.FieldByName("DIVISION").Value = BRANCH;
                UnityObjects.Lines transactions_lines = invoice.DataFields.FieldByName("TRANSACTIONS").Lines;
                foreach (var line in _DETAILS)
                {
                    if (string.IsNullOrEmpty(line.ITEMCODE))
                    {
                        LOGYAZ($"Boş ITEMCODE atlandı - Tarih: {_BASLIK.DATE_:yyyy-MM-dd}, POS: {_BASLIK.POS}, Ürün: {line.ITEMNAME}, Tutar: {line.LINETOTAL}, Miktar: {line.QUANTITY}", null);
                        continue;
                    }
                    transactions_lines.AppendLine();
                    double VatRate = 0;
                    try { VatRate = double.Parse(HELPER.SqlSelectLogo($"SELECT VAT FROM LG_{FIRMNR}_ITEMS WITH(NOLOCK) WHERE CODE='" + line.ITEMCODE + "'").Rows[0][0].ToString()); } catch { }

                    double priceFromDecmailToDouble = 0;
                    try { priceFromDecmailToDouble = Convert.ToDouble(line.PRICE.ToString().Replace(".", ",")); } catch { }

                    double linetotalFromDecmailToDouble = 0;
                    try { linetotalFromDecmailToDouble = Convert.ToDouble(line.LINETOTAL.ToString().Replace(".", ",")); } catch { }

                    transactions_lines[transactions_lines.Count - 1].FieldByName("TYPE").Value = 0;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("MASTER_CODE").Value = line.ITEMCODE;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("QUANTITY").Value = line.QUANTITY;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("PRICE").Value = linetotalFromDecmailToDouble / line.QUANTITY;
                    //transactions_lines[transactions_lines.Count - 1].FieldByName("TOTAL").Value = linetotalFromDecmailToDouble;
                    //transactions_lines[transactions_lines.Count - 1].FieldByName("PRICE").Value = priceFromDecmailToDouble;
                    //transactions_lines[transactions_lines.Count - 1].FieldByName("TOTAL").Value = linetotalFromDecmailToDouble;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("UNIT_CODE").Value = line.ITEMUNIT;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("UNIT_CONV1").Value = 1;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("UNIT_CONV2").Value = 1;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("VAT_INCLUDED").Value = 1;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("VAT_RATE").Value = VatRate;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("BILLED").Value = 1;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("EDT_CURR").Value = 160;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("SALEMANCODE").Value = line.SALESMAN;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("MONTH").Value = _BASLIK.DATE_.Month;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("YEAR").Value = _BASLIK.DATE_.Year;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("AFFECT_RISK").Value = 1;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("BARCODE").Value = line.ITEMCODE;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("RET_COST_TYPE").Value = 1;
                    if (Math.Abs(line.DISCOUNT_TOTAL) > 0)
                    {
                        //double dividationOfG3Bug = 1.00;
                        //if (line.DISCOUNT_TOTAL == line.CAMPAIGN_DISCOUNT)
                        //    dividationOfG3Bug = 2.00;
                        double discountFromDecmailToDouble = 0;
                        try { discountFromDecmailToDouble = Math.Abs(Convert.ToDouble(line.DISCOUNT_TOTAL.ToString().Replace(".", ","))); } catch { }
                        discountFromDecmailToDouble = discountFromDecmailToDouble / ((100 + VatRate) / 100);
                        transactions_lines.AppendLine();
                        transactions_lines[transactions_lines.Count - 1].FieldByName("TYPE").Value = 2;
                        //transactions_lines[transactions_lines.Count - 1].FieldByName("DETAIL_LEVEL").Value = 1;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("DISCEXP_CALC").Value = 1;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("TOTAL").Value = discountFromDecmailToDouble /*/ dividationOfG3Bug*/;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("BILLED").Value = 1;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("MONTH").Value = _BASLIK.DATE_.Month;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("YEAR").Value = _BASLIK.DATE_.Year;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("AFFECT_RISK").Value = 1;
                    }
                }

                //invoice.FillAccCodes();
                invoice.ReCalculate();

                if (!invoice.Post())
                    throw new Exception(GetLastError(invoice));
                int LOGOLREF = Convert.ToInt32(invoice.DataFields.DBFieldByName("LOGICALREF").Value);
                DateTime LOGOINSERTDATE = DateTime.Now;
                if (LOGOLREF > 0)
                    return "ok";
                else return "notok";
            }
            catch (Exception E)
            {
                LOGYAZ("InsertReturnInvoice", E);
                return E.Message;
            }
        }

        public static string rollBackDebtClose(int REF)
        {
            try
            {

                if (!AppUnity.RollBackDebtClose(REF))
                {

                    LOGYAZ("rollBackDebtClose " + REF + "", null);
                    throw new Exception(AppUnity.GetLastError() + " " + AppUnity.GetLastErrorString());
                }

                else return "ok";
            }
            catch (Exception E)
            {
                LOGYAZ("rollBackDebtClose", E);
                return E.Message;
            }
        }
        private static double getRateFromDB(int? dOVIZ_TIPI, DateTime? sIPARIS_TARIHI, string fIRMNR)
        {
            double rate = 1;
            string dateSqlFormat = DateTime.Now.ToString("yyyyMMdd");
            try { dateSqlFormat = sIPARIS_TARIHI.Value.ToString("yyyyMMdd"); } catch { }
            if (dOVIZ_TIPI == 0 || dOVIZ_TIPI == 160)
                return rate = 1;
            else
                return rate = double.Parse(HELPER.SqlSelectLogo($"SELECT TOP 1 RATES1 FROM BMS_{fIRMNR}_EXCHANGE  WHERE CRTYPE='" + dOVIZ_TIPI + "' AND EDATE<='" + dateSqlFormat + "' ORDER BY EDATE DESC").Rows[0][0].ToString());
        }

        private static int getOldDepartmentCodeFromXt(string STUDENT_NO, string FIRMNR)
        {
            int result = 0;
            try
            {
                DataTable dt = SqlSelectLogo($@"SELECT DEPARTMENT_CODE FROM LG_XT051_{FIRMNR} WHERE STUDENT_NO='{STUDENT_NO}'");
                if (dt.Rows.Count > 0)
                    result = Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception E)
            {
                LOGYAZ("getOldDepartmentCodeFromXt", E);
            }
            return result;
        }

        private static string getOldFacultyCodeFromXt(string STUDENT_NO, string FIRMNR)
        {
            string result = "";
            try
            {
                DataTable dt = SqlSelectLogo($@"SELECT FACULTY_CODE FROM LG_XT051_{FIRMNR} WHERE STUDENT_NO='{STUDENT_NO}'");
                if (dt.Rows.Count > 0)
                    result = dt.Rows[0][0].ToString();
            }
            catch (Exception E)
            {
                LOGYAZ("getOldFacultyCodeFromXt", E);
            }
            return result;
        }




        private static dynamic getVatRateFromSrvcard(string CODE, string FIRMNR)
        {
            double vatRate = 0;
            try { vatRate = double.Parse(HELPER.SqlSelectLogo($"SELECT VAT FROM LG_{FIRMNR}_ITEMS WHERE CODE='" + CODE + "'").Rows[0][0].ToString()); } catch { }
            return vatRate;
        }

        private static dynamic get4LogoMasrafMerkezi(string STUDENT_NO, string FIRMNR)
        {
            string MapType = "Masraf Merkezi - Bölüm Kodu"; /*Bölüm - Egitim Seviyesi*/ /*Masraf Merkezi - Bölüm Kodu*/
            string logoMasrafMerkezi = "";

            string XT_DEPARTMENT_CODE = "";
            try { XT_DEPARTMENT_CODE = SqlSelectLogo($"SELECT TOP 1 DEPARTMENT_CODE FROM LG_XT051_{FIRMNR} WHERE STUDENT_NO='{STUDENT_NO}'").Rows[0][0].ToString(); } catch { }

            DataTable dt = SqlSelectLogo($@"SELECT TOP 1 LOGOFIELD FROM BMS_{FIRMNR}_WPU_MAP WHERE TYPE='{MapType}' and MAPFIELD='{XT_DEPARTMENT_CODE}'");
            if (dt.Rows.Count > 0)
                logoMasrafMerkezi = dt.Rows[0][0].ToString();
            return logoMasrafMerkezi;
        }

        private static dynamic get4LogoDepartment(string STUDENT_NO, string FIRMNR)
        {
            string MapType = "Bölüm - Egitim Seviyesi"; /*Bölüm - Egitim Seviyesi*/ /*Masraf Merkezi - Bölüm Kodu*/
            int logoDepartment = 0;

            string XT_PROGRAM_LEVEL = "";
            try { XT_PROGRAM_LEVEL = SqlSelectLogo($"SELECT TOP 1 PROGRAM_LEVEL FROM LG_XT051_{FIRMNR} WHERE STUDENT_NO='{STUDENT_NO}'").Rows[0][0].ToString(); } catch { }

            DataTable dt = SqlSelectLogo($@"SELECT TOP 1 LOGOFIELD FROM BMS_{FIRMNR}_WPU_MAP WHERE TYPE='{MapType}' and MAPFIELD='{XT_PROGRAM_LEVEL}'");
            if (dt.Rows.Count > 0)
                logoDepartment = Convert.ToInt32(dt.Rows[0][0]);
            return logoDepartment;
        }

        private static dynamic get4LogoDivision(string STUDENT_NO, string FIRMNR)
        {
            string MapType = "Isyeri - Fakülte Kodu"; /*Bölüm - Egitim Seviyesi*/ /*Masraf Merkezi - Bölüm Kodu*/
            int logoDivision = 0;

            string XT_FACULTY_CODE = "";
            try { XT_FACULTY_CODE = SqlSelectLogo($"SELECT TOP 1 FACULTY_CODE FROM LG_XT051_{FIRMNR} WHERE STUDENT_NO='{STUDENT_NO}'").Rows[0][0].ToString(); } catch { }

            DataTable dt = SqlSelectLogo($@"SELECT TOP 1 LOGOFIELD FROM BMS_{FIRMNR}_WPU_MAP WHERE TYPE='{MapType}' and MAPFIELD='{XT_FACULTY_CODE}'");
            if (dt.Rows.Count > 0)
                logoDivision = Convert.ToInt32(dt.Rows[0][0]);
            return logoDivision;
        }

        private static void createAccountingFiche(/*BMS_XXX_ORDER ORDER,*/ int lOGOLREF, string FIRMNR)
        {
            DateTime ORDER_SIPARIS_TARIHI = new DateTime();
            try { ORDER_SIPARIS_TARIHI = Convert.ToDateTime(SqlSelectLogo($"SELECT DATE_ FROM LG_{FIRMNR}_01_ORFICHE WHERE LOGICALREF={lOGOLREF}").Rows[0][0]); } catch { }
            string ORDER_SIPARIS_NO = "";
            try { ORDER_SIPARIS_NO = SqlSelectLogo($"SELECT FICHENO FROM LG_{FIRMNR}_01_ORFICHE WHERE LOGICALREF={lOGOLREF}").Rows[0][0].ToString(); } catch { }

            string ORDER_CARI_KOD = "";
            try { ORDER_CARI_KOD = SqlSelectLogo($"SELECT (SELECT TOP 1 CODE FROM LG_{FIRMNR}_CLCARD C WHERE C.LOGICALREF=CLIENTREF) FROM LG_{FIRMNR}_01_ORFICHE WHERE LOGICALREF={lOGOLREF}").Rows[0][0].ToString(); } catch { }

            int ORDER_DOVIZ_TIPI = 0;
            try { ORDER_DOVIZ_TIPI = Convert.ToInt32(SqlSelectLogo($"SELECT TRCURR FROM LG_{FIRMNR}_01_ORFICHE WHERE LOGICALREF={lOGOLREF}").Rows[0][0]); } catch { }
            try
            {
                double raporlamaDoviziRate = getRateFromDB(20, ORDER_SIPARIS_TARIHI.Date, FIRMNR);
                double? islemDoviziRate = getRateFromDB(ORDER_DOVIZ_TIPI, ORDER_SIPARIS_TARIHI.Date, FIRMNR);
                int OrficheAccountref = 0;      //120.01.001.03.01.03
                string OrficheAccountCode = ""; //120.01.001.03.01.03 
                double OrficheNettotal = 0.00;
                try { OrficheAccountref = Convert.ToInt32(SqlSelectLogo($@"SELECT TOP 1 ACCOUNTREF FROM LG_{FIRMNR}_01_ORFICHE WHERE LOGICALREF={lOGOLREF}").Rows[0][0]); } catch { }

                try { OrficheNettotal = Convert.ToDouble(SqlSelectLogo($@"SELECT TOP 1 NETTOTAL FROM LG_{FIRMNR}_01_ORFICHE WHERE LOGICALREF={lOGOLREF}").Rows[0][0]); } catch { }
                if (OrficheAccountref > 0)
                    OrficheAccountCode = SqlSelectLogo($@"SELECT TOP 1 CODE FROM LG_{FIRMNR}_EMUHACC WHERE LOGICALREF={OrficheAccountref}").Rows[0][0].ToString();


                UnityObjects.Data glvoucher = NewObjectData(UnityObjects.DataObjectType.doGLVoucher);
                glvoucher.New();
                glvoucher.DataFields.FieldByName("TYPE").Value = 4;
                glvoucher.DataFields.FieldByName("NUMBER").Value = "~";
                glvoucher.DataFields.FieldByName("AUTH_CODE").Value = "BMS";
                glvoucher.DataFields.FieldByName("DATE").Value = ORDER_SIPARIS_TARIHI.Date;
                glvoucher.DataFields.FieldByName("NOTES4").Value = "SIPARISNO:" + ORDER_SIPARIS_NO;
                glvoucher.DataFields.FieldByName("CURRSEL_TOTALS").Value = 1;
                glvoucher.DataFields.FieldByName("CURRSEL_DETAILS").Value = 2;
                glvoucher.DataFields.FieldByName("DOC_DATE").Value = ORDER_SIPARIS_TARIHI.Date;
                glvoucher.DataFields.FieldByName("DIVISION").Value = get4LogoDivision(ORDER_CARI_KOD, FIRMNR); /*ISYERI*/
                glvoucher.DataFields.FieldByName("DEPARTMENT").Value = get4LogoDepartment(ORDER_CARI_KOD, FIRMNR); /*BOLUM*/
                UnityObjects.Lines transactions_lines = glvoucher.DataFields.FieldByName("TRANSACTIONS").Lines;
                transactions_lines.AppendLine();
                transactions_lines[transactions_lines.Count - 1].FieldByName("GL_CODE").Value = OrficheAccountCode;
                transactions_lines[transactions_lines.Count - 1].FieldByName("PARENT_GLCODE").Value = OrficheAccountCode.Substring(0, 3);
                //transactions_lines[transactions_lines.Count - 1].FieldByName("DEBIT").Value = OrficheNettotal;
                transactions_lines[transactions_lines.Count - 1].FieldByName("DEBIT").Value = Math.Round(OrficheNettotal, 2);
                transactions_lines[transactions_lines.Count - 1].FieldByName("LINENO").Value = 1;
                transactions_lines[transactions_lines.Count - 1].FieldByName("DESCRIPTION").Value = ORDER_CARI_KOD;
                transactions_lines[transactions_lines.Count - 1].FieldByName("CURR_TRANS").Value = ORDER_DOVIZ_TIPI;
                transactions_lines[transactions_lines.Count - 1].FieldByName("RC_XRATE").Value = raporlamaDoviziRate;
                //transactions_lines[transactions_lines.Count - 1].FieldByName("RC_AMOUNT").Value = OrficheNettotal / raporlamaDoviziRate;
                transactions_lines[transactions_lines.Count - 1].FieldByName("RC_AMOUNT").Value = Math.Round(OrficheNettotal / raporlamaDoviziRate, 2);
                transactions_lines[transactions_lines.Count - 1].FieldByName("TC_XRATE").Value = islemDoviziRate;
                transactions_lines[transactions_lines.Count - 1].FieldByName("TC_AMOUNT").Value = Math.Round((double)(OrficheNettotal / islemDoviziRate), 2);
                transactions_lines[transactions_lines.Count - 1].FieldByName("QUANTITY").Value = 0;
                //transactions_lines[transactions_lines.Count - 1].FieldByName("EURO_DEBIT").Value = OrficheNettotal / raporlamaDoviziRate;
                transactions_lines[transactions_lines.Count - 1].FieldByName("EURO_DEBIT").Value = Math.Round(OrficheNettotal / raporlamaDoviziRate, 2);
                transactions_lines[transactions_lines.Count - 1].FieldByName("CURRSEL_TRANS").Value = 2;
                transactions_lines[transactions_lines.Count - 1].FieldByName("MONTH").Value = ORDER_SIPARIS_TARIHI.Date.Month;
                transactions_lines[transactions_lines.Count - 1].FieldByName("YEAR").Value = ORDER_SIPARIS_TARIHI.Date.Year;
                transactions_lines[transactions_lines.Count - 1].FieldByName("DOC_DATE").Value = ORDER_SIPARIS_TARIHI.Date;
                transactions_lines[transactions_lines.Count - 1].FieldByName("OHP_CODE").Value = get4LogoMasrafMerkezi(ORDER_CARI_KOD, FIRMNR); /*MM*/
                transactions_lines[transactions_lines.Count - 1].FieldByName("DEPARTMENT").Value = get4LogoDepartment(ORDER_CARI_KOD, FIRMNR); /*BOLUM*/

                DataTable dataTable = SqlSelectLogo($@"SELECT * FROM LG_{FIRMNR}_01_ORFLINE WHERE ORDFICHEREF={lOGOLREF}");
                int lineNo = 2;
                foreach (DataRow dr in dataTable.Rows)
                {
                    int OrflineAccountref = 0;      //380.01.001.03.01.03
                    string OrflineAccountCode = ""; //380.01.001.03.01.03
                    int OrflineVatAccref = 0;       //391.01.001.04.01.03
                    string OrflineVatAccCode = "";  //391.01.001.04.01.03
                    int OrflineVat = 0;
                    double OflineLineNet = 0.00;
                    double OrflineVatamnt = 0.00;
                    try { OrflineAccountref = Convert.ToInt32(dr["ACCOUNTREF"]); } catch { }
                    try { OrflineVatAccref = Convert.ToInt32(dr["VATACCREF"]); } catch { }
                    try { OrflineVat = Convert.ToInt32(dr["VAT"]); } catch { }
                    try { OflineLineNet = Math.Round(Convert.ToDouble(dr["LINENET"]) /*/ Convert.ToDouble(islemDoviziRate)*/, 2); } catch { }
                    try { OrflineVatamnt = Math.Round(Convert.ToDouble(dr["VATAMNT"]) /*/ Convert.ToDouble(islemDoviziRate)*/, 2); } catch { }
                    if (OrflineAccountref > 0)
                        OrflineAccountCode = SqlSelectLogo($@"SELECT TOP 1 CODE FROM LG_{FIRMNR}_EMUHACC WHERE LOGICALREF={OrflineAccountref}").Rows[0][0].ToString();
                    if (OrflineVatAccref > 0)
                        OrflineVatAccCode = SqlSelectLogo($@"SELECT TOP 1 CODE FROM LG_{FIRMNR}_EMUHACC WHERE LOGICALREF={OrflineVatAccref}").Rows[0][0].ToString();

                    string ORDERLINE_STOK_KODU = "";
                    string ORDER_STOK_REF = dr["STOCKREF"].ToString();
                    try { ORDERLINE_STOK_KODU = SqlSelectLogo($@"SELECT TOP 1 CODE FROM LG_{FIRMNR}_SRVCARD WHERE LOGICALREF={ORDER_STOK_REF}").Rows[0][0].ToString(); } catch { }

                    int ORDERLINE_DOVIZ_TIPI = 0;
                    try { ORDERLINE_DOVIZ_TIPI = Convert.ToInt32(dr["TRCURR"]); } catch { }

                    transactions_lines.AppendLine();
                    transactions_lines[transactions_lines.Count - 1].FieldByName("SIGN").Value = 1;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("GL_CODE").Value = OrflineAccountCode;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("PARENT_GLCODE").Value = OrflineAccountCode.Substring(0, 3);
                    transactions_lines[transactions_lines.Count - 1].FieldByName("CREDIT").Value = Math.Round(OflineLineNet, 2);
                    transactions_lines[transactions_lines.Count - 1].FieldByName("LINENO").Value = lineNo++;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("DESCRIPTION").Value = ORDERLINE_STOK_KODU;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("CURR_TRANS").Value = ORDERLINE_DOVIZ_TIPI;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("RC_XRATE").Value = raporlamaDoviziRate;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("RC_AMOUNT").Value = Math.Round(OflineLineNet / raporlamaDoviziRate, 2);
                    transactions_lines[transactions_lines.Count - 1].FieldByName("TC_XRATE").Value = islemDoviziRate;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("TC_AMOUNT").Value = Math.Round((double)(OflineLineNet / islemDoviziRate), 2);
                    transactions_lines[transactions_lines.Count - 1].FieldByName("QUANTITY").Value = 0;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("EURO_CREDIT").Value = Math.Round(OflineLineNet / raporlamaDoviziRate, 2);
                    transactions_lines[transactions_lines.Count - 1].FieldByName("CURRSEL_TRANS").Value = 2;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("DATA_REFERENCE").Value = 5;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("MONTH").Value = ORDER_SIPARIS_TARIHI.Date.Month;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("YEAR").Value = ORDER_SIPARIS_TARIHI.Date.Year;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("DOC_DATE").Value = ORDER_SIPARIS_TARIHI.Date;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("OHP_CODE").Value = get4LogoMasrafMerkezi(ORDER_CARI_KOD, FIRMNR); /*MM*/
                    transactions_lines[transactions_lines.Count - 1].FieldByName("DEPARTMENT").Value = get4LogoDepartment(ORDER_CARI_KOD, FIRMNR); /*BOLUM*/
                    if (OrflineVat > 0)
                    {
                        transactions_lines.AppendLine();
                        transactions_lines[transactions_lines.Count - 1].FieldByName("SIGN").Value = 1;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("GL_CODE").Value = OrflineVatAccCode;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("PARENT_GLCODE").Value = OrflineVatAccCode.Substring(0, 3);
                        transactions_lines[transactions_lines.Count - 1].FieldByName("CREDIT").Value = Math.Round(OrflineVatamnt, 2);
                        transactions_lines[transactions_lines.Count - 1].FieldByName("LINENO").Value = lineNo++;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("DESCRIPTION").Value = "KDV % " + OrflineVat.ToString();
                        transactions_lines[transactions_lines.Count - 1].FieldByName("CURR_TRANS").Value = ORDERLINE_DOVIZ_TIPI;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("RC_XRATE").Value = raporlamaDoviziRate;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("RC_AMOUNT").Value = Math.Round(OrflineVatamnt / raporlamaDoviziRate, 2);
                        transactions_lines[transactions_lines.Count - 1].FieldByName("TC_XRATE").Value = islemDoviziRate;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("TC_AMOUNT").Value = Math.Round((double)(OrflineVatamnt / islemDoviziRate), 2);
                        transactions_lines[transactions_lines.Count - 1].FieldByName("QUANTITY").Value = 0;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("EURO_CREDIT").Value = Math.Round(OrflineVatamnt / raporlamaDoviziRate, 2);
                        transactions_lines[transactions_lines.Count - 1].FieldByName("CURRSEL_TRANS").Value = 2;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("MONTH").Value = ORDER_SIPARIS_TARIHI.Date.Month;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("YEAR").Value = ORDER_SIPARIS_TARIHI.Date.Year;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("DOC_DATE").Value = ORDER_SIPARIS_TARIHI.Date;
                        transactions_lines[transactions_lines.Count - 1].FieldByName("OHP_CODE").Value = get4LogoMasrafMerkezi(ORDER_CARI_KOD, FIRMNR); /*MM*/
                        transactions_lines[transactions_lines.Count - 1].FieldByName("DEPARTMENT").Value = get4LogoDepartment(ORDER_CARI_KOD, FIRMNR); /*BOLUM*/
                    }
                }


                string FISNO = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
                glvoucher.ExportToXML("GL_VOUCHERS", FISNO + ".xml");
                if (!glvoucher.Post())
                    throw new Exception(GetLastError(glvoucher));
            }
            catch (Exception E)
            {
                LOGYAZ("createAccountingFiche " + ORDER_SIPARIS_NO, E);
            }
        }

        private static void RevisetInvoiceForOrderToInvoiceFromPaymentIntegration(int LOGOLREF, string FIRMNR)
        {
            bool hasKayitUcretGeliri = false;
            bool hasEgitimOgretimGeliri = false;

            int AccFicheref = 0;
            try { AccFicheref = Convert.ToInt32(SqlSelectLogo($"SELECT TOP 1 ACCFICHEREF FROM LG_{FIRMNR}_01_INVOICE WHERE LOGICALREF={LOGOLREF}").Rows[0][0]); } catch { }
            if (AccFicheref > 0)
            {
                DataRow Dr120AkaOldLine = null;
                try { Dr120AkaOldLine = SqlSelectLogo($"SELECT TOP 1 * FROM LG_{FIRMNR}_01_EMFLINE WHERE ACCFICHEREF={AccFicheref} AND ACCOUNTCODE LIKE '120.01.%' ORDER BY LOGICALREF ASC").Rows[0]; } catch { }

                DataRow Dr600AkaCopyFrom = null;
                try { Dr600AkaCopyFrom = SqlSelectLogo($"SELECT TOP 1 * FROM LG_{FIRMNR}_01_EMFLINE WHERE ACCFICHEREF={AccFicheref} AND ACCOUNTCODE LIKE '600.01.%' ORDER BY LOGICALREF ASC").Rows[0]; } catch { }
                DataRow Dr600Sums = null;
                try { Dr600Sums = SqlSelectLogo($"SELECT SUM(CREDIT) CREDIT, SUM(REPORTNET) REPORTNET, SUM(TRNET) TRNET, SUM(EMUDEBIT) EMUDEBIT FROM LG_{FIRMNR}_01_EMFLINE WHERE ACCFICHEREF={AccFicheref} AND ACCOUNTCODE LIKE '600.01.%'").Rows[0]; } catch { }

                DataRow Dr391AkaCopyFrom = null;
                try { Dr391AkaCopyFrom = SqlSelectLogo($"SELECT TOP 1 * FROM LG_{FIRMNR}_01_EMFLINE WHERE ACCFICHEREF={AccFicheref} AND ACCOUNTCODE LIKE '391.01.%' ORDER BY LOGICALREF ASC").Rows[0]; } catch { }
                DataRow Dr391Sums = null;
                try { Dr391Sums = SqlSelectLogo($"SELECT SUM(CREDIT) CREDIT, SUM(REPORTNET) REPORTNET, SUM(TRNET) TRNET, SUM(EMUDEBIT) EMUDEBIT FROM LG_{FIRMNR}_01_EMFLINE WHERE ACCFICHEREF={AccFicheref} AND ACCOUNTCODE LIKE '391.01.%'").Rows[0]; } catch { }

                #region Update380 
                string newAccountCode = Dr120AkaOldLine["ACCOUNTCODE"].ToString().Replace("120.01.", "380.01.");
                int newAccountref = Convert.ToInt32(SqlSelectLogo($"SELECT TOP 1 LOGICALREF FROM LG_{FIRMNR}_EMUHACC WHERE CODE='{newAccountCode}'").Rows[0][0]);
                string newKebirCode = newAccountCode.Substring(0, 3);
                double newDebit = Convert.ToDouble(Dr600Sums["CREDIT"]);
                double newReportnet = Convert.ToDouble(Dr600Sums["REPORTNET"]);
                double newTrnet = Convert.ToDouble(Dr600Sums["TRNET"]);
                double newEmuDebit = Convert.ToDouble(Dr600Sums["EMUDEBIT"]);
                //update sql 
                SqlExecute($"UPDATE LG_{FIRMNR}_01_EMFLINE SET ACCOUNTREF={newAccountref}, ACCOUNTCODE='{newAccountCode}', KEBIRCODE='{newKebirCode}', DEBIT={newDebit.ToString().Replace(",", ".")}, REPORTNET={newReportnet.ToString().Replace(",", ".")}, TRNET={newTrnet.ToString().Replace(",", ".")}, EMUDEBIT={newEmuDebit.ToString().Replace(",", ".")} WHERE LOGICALREF={Dr120AkaOldLine["LOGICALREF"]}");
                #endregion
                #region AppendNewLine390
                UnityObjects.Data F = NewObjectData(UnityObjects.DataObjectType.doGLVoucher);
                F.Read(AccFicheref);
                UnityObjects.Lines transactions_lines = F.DataFields.FieldByName("TRANSACTIONS").Lines;
                transactions_lines.AppendLine();
                //GL_CODE
                transactions_lines[transactions_lines.Count - 1].FieldByName("GL_CODE").Value = "390.01.001"; //Satis Siparisi KDV
                transactions_lines[transactions_lines.Count - 1].FieldByName("OHP_CODE").Value = getEmcodeFromLogicalref(Dr391AkaCopyFrom["CENTERREF"], FIRMNR);
                transactions_lines[transactions_lines.Count - 1].FieldByName("PARENT_GLCODE").Value = "390.01";
                transactions_lines[transactions_lines.Count - 1].FieldByName("LINENO").Value = transactions_lines.Count + 1;
                transactions_lines[transactions_lines.Count - 1].FieldByName("CURR_TRANS").Value = Convert.ToInt32(Dr391AkaCopyFrom["TRCURR"]);
                transactions_lines[transactions_lines.Count - 1].FieldByName("RC_XRATE").Value = Convert.ToDouble(Dr391AkaCopyFrom["REPORTRATE"]);
                transactions_lines[transactions_lines.Count - 1].FieldByName("RC_AMOUNT").Value = Convert.ToDouble(Dr391Sums["REPORTNET"]);
                transactions_lines[transactions_lines.Count - 1].FieldByName("TC_XRATE").Value = Convert.ToDouble(Dr391AkaCopyFrom["TRRATE"]);
                transactions_lines[transactions_lines.Count - 1].FieldByName("TC_AMOUNT").Value = Convert.ToDouble(Dr391Sums["TRNET"]);
                transactions_lines[transactions_lines.Count - 1].FieldByName("CURRSEL_TRANS").Value = 2;
                transactions_lines[transactions_lines.Count - 1].FieldByName("DEPARTMENT").Value = Convert.ToInt32(Dr391AkaCopyFrom["DEPARTMENT"]);
                transactions_lines[transactions_lines.Count - 1].FieldByName("DOC_DATE").Value = Convert.ToDateTime(Dr391AkaCopyFrom["DOCDATE"]);
                transactions_lines[transactions_lines.Count - 1].FieldByName("MONTH").Value = Convert.ToInt32(Dr391AkaCopyFrom["MONTH_"]);
                transactions_lines[transactions_lines.Count - 1].FieldByName("DESCRIPTION").Value = Dr391AkaCopyFrom["LINEEXP"].ToString();
                transactions_lines[transactions_lines.Count - 1].FieldByName("DEBIT").Value = Convert.ToDouble(Dr391Sums["CREDIT"]);
                transactions_lines[transactions_lines.Count - 1].FieldByName("EURO_DEBIT").Value = Convert.ToDouble(Dr391Sums["EMUDEBIT"]);
                if (!F.Post())
                    throw new Exception(GetLastError(F));
                int LOGOLEFREF = Convert.ToInt32(F.DataFields.DBFieldByName("LOGICALREF").Value);
                #endregion
            }
        }

        private static dynamic getEmcodeFromLogicalref(object LOGICALREF, string FIRMNR)
        {
            string emcode = "";
            try { emcode = SqlSelectLogo($"SELECT TOP 1 CODE FROM LG_{FIRMNR}_EMCENTER WHERE LOGICALREF={LOGICALREF}").Rows[0][0].ToString(); } catch { }
            return emcode;
        }

        public static string InsertCheque(string BRANCH, Bms_Fiche_Payment _PAYMENT, string FIRMNR)
        {
            bool isCustomerExist = false;
            try { isCustomerExist = Convert.ToBoolean(SqlSelectLogo($"SELECT COUNT(*) FROM LG_{FIRMNR}_CLCARD WHERE CODE='{_PAYMENT.CUSTOMER_CODE}'").Rows[0][0]); } catch { }
            if (!isCustomerExist)
                _PAYMENT.CUSTOMER_CODE = _PAYMENT.CUSTOMER_CODE.TrimStart('0');
            try
            {
                UnityObjects.Data rolls = NewObjectData(UnityObjects.DataObjectType.doCQPnRoll);
                rolls.New();
                rolls.DataFields.FieldByName("TYPE").Value = 1;
                rolls.DataFields.FieldByName("NUMBER").Value = "~";
                rolls.DataFields.FieldByName("DOC_NUMBER").Value = _PAYMENT.DOCUMENT_NO.ToString();
                rolls.DataFields.FieldByName("MASTER_MODULE").Value = 5;
                rolls.DataFields.FieldByName("MASTER_CODE").Value = _PAYMENT.CUSTOMER_CODE;
                rolls.DataFields.FieldByName("AUXIL_CODE").Value = _PAYMENT.POS.ToString();
                rolls.DataFields.FieldByName("AUTH_CODE").Value = "BMS";
                rolls.DataFields.FieldByName("DATE").Value = _PAYMENT.DATE_.Date;
                rolls.DataFields.FieldByName("DIVISION").Value = BRANCH;
                //rolls.DataFields.FieldByName("AVERAGE_AGE").Value = 234;
                rolls.DataFields.FieldByName("DOCUMENT_COUNT").Value = 1;
                rolls.DataFields.FieldByName("TOTAL").Value = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));
                //rolls.DataFields.FieldByName("TC_XRATE").Value = 1;
                //rolls.DataFields.FieldByName("TC_TOTAL").Value = 1234;
                //rolls.DataFields.FieldByName("RC_XRATE").Value = 21.45340751;
                //rolls.DataFields.FieldByName("RC_TOTAL").Value = 57.52;
                //rolls.DataFields.FieldByName("NOTES1").Value = BORDROACIKLAMA;
                //rolls.DataFields.FieldByName("ACCFICHEREF").Value = 89525;
                //rolls.DataFields.FieldByName("GL_CODE").Value = 320.10.01.001;
                rolls.DataFields.FieldByName("CURRSEL_TOTALS").Value = 1;
                rolls.DataFields.FieldByName("CURRSEL_DETAILS").Value = 2;

                UnityObjects.Lines transactions_lines = rolls.DataFields.FieldByName("TRANSACTIONS").Lines;
                transactions_lines.AppendLine();
                transactions_lines[transactions_lines.Count - 1].FieldByName("TYPE").Value = 1;
                transactions_lines[transactions_lines.Count - 1].FieldByName("CURRENT_STATUS").Value = 1;
                transactions_lines[transactions_lines.Count - 1].FieldByName("NUMBER").Value = "~";
                transactions_lines[transactions_lines.Count - 1].FieldByName("AUXIL_CODE").Value = _PAYMENT.POS.ToString();
                transactions_lines[transactions_lines.Count - 1].FieldByName("AUTH_CODE").Value = "BMS";
                transactions_lines[transactions_lines.Count - 1].FieldByName("OWING").Value = _PAYMENT.CUSTOMER_NAME;
                transactions_lines[transactions_lines.Count - 1].FieldByName("DIVISION").Value = BRANCH;
                transactions_lines[transactions_lines.Count - 1].FieldByName("DUE_DATE").Value = _PAYMENT.DATE_.Date;
                transactions_lines[transactions_lines.Count - 1].FieldByName("DATE").Value = _PAYMENT.DATE_.Date;
                transactions_lines[transactions_lines.Count - 1].FieldByName("AMOUNT").Value = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));
                //transactions_lines[transactions_lines.Count - 1].FieldByName("TC_XRATE").Value = 1;
                //transactions_lines[transactions_lines.Count - 1].FieldByName("TC_AMOUNT").Value = 1234;
                //transactions_lines[transactions_lines.Count - 1].FieldByName("RC_XRATE").Value = 21.4534;
                //transactions_lines[transactions_lines.Count - 1].FieldByName("RC_AMOUNT").Value = 57.52;
                transactions_lines[transactions_lines.Count - 1].FieldByName("TRANS_STATUS").Value = 1;
                transactions_lines[transactions_lines.Count - 1].FieldByName("STATUS_ORDER").Value = 1;
                transactions_lines[transactions_lines.Count - 1].FieldByName("SERIAL_NR").Value = _PAYMENT.SERIAL_NO;
                transactions_lines[transactions_lines.Count - 1].FieldByName("XML_ATTRIBUTE1").Value = 1;
                transactions_lines[transactions_lines.Count - 1].FieldByName("AFFECT_RISK").Value = 1;

                rolls.ReCalculate();

                if (!rolls.Post())
                    throw new Exception(GetLastError(rolls));
                int LOGOLREF = Convert.ToInt32(rolls.DataFields.DBFieldByName("LOGICALREF").Value);
                DateTime LOGOINSERTDATE = DateTime.Now;
                if (LOGOLREF > 0)
                    return "ok";
                else return "notok";
            }
            catch (Exception E)
            {
                LOGYAZ("InsertCheque", E);
                return E.Message;
            }
        }
        public static string InsertCHFiche(string BRANCH, Bms_Fiche_Payment _PAYMENT, string FIRMNR)
        {
            LOGYAZ($"InsertCHFiche BASLADI - BRANCH:{BRANCH}, FIRMNR:{FIRMNR}, CUSTOMER_CODE:{_PAYMENT?.CUSTOMER_CODE}, LOGO_FICHE_TYPE:{_PAYMENT?.LOGO_FICHE_TYPE}, PAYMENT_TOTAL:{_PAYMENT?.PAYMENT_TOTAL}, DATE:{_PAYMENT?.DATE_}, DOCUMENT_NO:{_PAYMENT?.DOCUMENT_NO}", null);

            // Parametre kontrolleri
            if (_PAYMENT == null)
            {
                LOGYAZ("InsertCHFiche", new Exception("_PAYMENT parametresi null"));
                return "_PAYMENT parametresi null";
            }
            if (string.IsNullOrEmpty(BRANCH))
            {
                LOGYAZ("InsertCHFiche", new Exception("BRANCH parametresi boş"));
                return "BRANCH parametresi boş";
            }
            if (string.IsNullOrEmpty(FIRMNR))
            {
                LOGYAZ("InsertCHFiche", new Exception("FIRMNR parametresi boş"));
                return "FIRMNR parametresi boş";
            }
            if (string.IsNullOrEmpty(_PAYMENT.CUSTOMER_CODE))
            {
                LOGYAZ("InsertCHFiche", new Exception("CUSTOMER_CODE boş"));
                return "CUSTOMER_CODE boş";
            }
            if (string.IsNullOrEmpty(_PAYMENT.LOGO_FICHE_TYPE))
            {
                LOGYAZ("InsertCHFiche", new Exception("LOGO_FICHE_TYPE boş"));
                return "LOGO_FICHE_TYPE boş";
            }

            bool isCustomerExist = false;
            try
            {
                LOGYAZ($"InsertCHFiche - Müşteri sorgusu yapılıyor: LG_{FIRMNR}_CLCARD, CODE={_PAYMENT.CUSTOMER_CODE}", null);
                isCustomerExist = Convert.ToBoolean(SqlSelectLogo($"SELECT COUNT(*) FROM LG_{FIRMNR}_CLCARD WHERE CODE='{_PAYMENT.CUSTOMER_CODE}'").Rows[0][0]);
                LOGYAZ($"InsertCHFiche - Müşteri sorgusu sonucu: isCustomerExist={isCustomerExist}", null);
            }
            catch (Exception custEx)
            {
                LOGYAZ($"InsertCHFiche - Müşteri sorgusu hatası", custEx);
            }

            if (!isCustomerExist)
            {
                string originalCode = _PAYMENT.CUSTOMER_CODE;
                _PAYMENT.CUSTOMER_CODE = _PAYMENT.CUSTOMER_CODE.TrimStart('0');
                LOGYAZ($"InsertCHFiche - Müşteri bulunamadı, kod düzeltildi: {originalCode} -> {_PAYMENT.CUSTOMER_CODE}", null);
            }

            try
            {
                LOGYAZ("InsertCHFiche - NewObjectData çağrılıyor (doARAPVoucher)", null);
                UnityObjects.Data arpvoucher = NewObjectData(UnityObjects.DataObjectType.doARAPVoucher);
                LOGYAZ("InsertCHFiche - NewObjectData başarılı, arpvoucher.New() çağrılıyor", null);
                arpvoucher.New();
                LOGYAZ("InsertCHFiche - arpvoucher.New() başarılı, field'lar ayarlanıyor", null);

                arpvoucher.DataFields.FieldByName("NUMBER").Value = "~";
                arpvoucher.DataFields.FieldByName("DATE").Value = _PAYMENT.DATE_.Date;
                if (_PAYMENT.LOGO_FICHE_TYPE == "CH Kredi Karti" || _PAYMENT.LOGO_FICHE_TYPE == "CH Kredi Karti Iade" || _PAYMENT.LOGO_FICHE_TYPE == "CH Borc" || _PAYMENT.LOGO_FICHE_TYPE == "CH Alacak")
                    arpvoucher.DataFields.FieldByName("TYPE").Value = 70;
                if (_PAYMENT.LOGO_FICHE_TYPE == "CH Kredi Karti Iade")
                    arpvoucher.DataFields.FieldByName("TYPE").Value = 71;
                if (_PAYMENT.LOGO_FICHE_TYPE == "CH Borc")
                    arpvoucher.DataFields.FieldByName("TYPE").Value = 3;
                if (_PAYMENT.LOGO_FICHE_TYPE == "CH Alacak")
                    arpvoucher.DataFields.FieldByName("TYPE").Value = 4;
                arpvoucher.DataFields.FieldByName("AUXIL_CODE").Value = _PAYMENT.POS.ToString();
                arpvoucher.DataFields.FieldByName("AUTH_CODE").Value = "BMS";
                arpvoucher.DataFields.FieldByName("DIVISION").Value = BRANCH;
                arpvoucher.DataFields.FieldByName("CURRSEL_TOTALS").Value = 1;

                LOGYAZ("InsertCHFiche - Header field'ları ayarlandı, satırlar ekleniyor", null);

                UnityObjects.Lines transactions_lines = arpvoucher.DataFields.FieldByName("TRANSACTIONS").Lines;
                transactions_lines.AppendLine();
                transactions_lines[transactions_lines.Count - 1].FieldByName("ARP_CODE").Value = _PAYMENT.CUSTOMER_CODE;
                transactions_lines[transactions_lines.Count - 1].FieldByName("AUXIL_CODE").Value = _PAYMENT.POS.ToString();
                transactions_lines[transactions_lines.Count - 1].FieldByName("AUTH_CODE").Value = "BMS";
                transactions_lines[transactions_lines.Count - 1].FieldByName("TRANNO").Value = "~";
                transactions_lines[transactions_lines.Count - 1].FieldByName("DOC_NUMBER").Value = _PAYMENT.DOCUMENT_NO.ToString();
                if (_PAYMENT.LOGO_FICHE_TYPE == "CH Kredi Karti" || _PAYMENT.LOGO_FICHE_TYPE == "CH Alacak")
                    transactions_lines[transactions_lines.Count - 1].FieldByName("CREDIT").Value = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));
                if (_PAYMENT.LOGO_FICHE_TYPE == "CH Kredi Karti Iade" || _PAYMENT.LOGO_FICHE_TYPE == "CH Borc")
                    transactions_lines[transactions_lines.Count - 1].FieldByName("DEBIT").Value = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));
                if (_PAYMENT.LOGO_FICHE_TYPE == "CH Kredi Karti" || _PAYMENT.LOGO_FICHE_TYPE == "CH Kredi Karti Iade")
                    transactions_lines[transactions_lines.Count - 1].FieldByName("BANKACC_CODE").Value = _PAYMENT.LOGO_BANK_OR_KS_CODE;

                transactions_lines[transactions_lines.Count - 1].FieldByName("TC_XRATE").Value = 1;
                transactions_lines[transactions_lines.Count - 1].FieldByName("TC_AMOUNT").Value = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));
                transactions_lines[transactions_lines.Count - 1].FieldByName("BNLN_TC_XRATE").Value = 1;
                transactions_lines[transactions_lines.Count - 1].FieldByName("BNLN_TC_AMOUNT").Value = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));
                transactions_lines[transactions_lines.Count - 1].FieldByName("MONTH").Value = _PAYMENT.DATE_.Month;
                transactions_lines[transactions_lines.Count - 1].FieldByName("YEAR").Value = _PAYMENT.DATE_.Year;

                LOGYAZ("InsertCHFiche - Satırlar eklendi, ReCalculate çağrılıyor", null);
                arpvoucher.ReCalculate();

                LOGYAZ("InsertCHFiche - ReCalculate tamamlandı, Post çağrılıyor", null);
                if (!arpvoucher.Post())
                {
                    string postError = GetLastError(arpvoucher);
                    LOGYAZ($"InsertCHFiche - Post BAŞARISIZ: {postError}", null);
                    throw new Exception(postError);
                }

                int LOGOLREF = Convert.ToInt32(arpvoucher.DataFields.DBFieldByName("LOGICALREF").Value);
                LOGYAZ($"InsertCHFiche - Post BAŞARILI, LOGICALREF={LOGOLREF}", null);

                if (LOGOLREF > 0)
                    return "ok";
                else return "notok";
            }
            catch (Exception E)
            {
                LOGYAZ($"InsertCHFiche HATA - BRANCH:{BRANCH}, CUSTOMER_CODE:{_PAYMENT?.CUSTOMER_CODE}, LOGO_FICHE_TYPE:{_PAYMENT?.LOGO_FICHE_TYPE}", E);
                return E.Message;
            }
        }

        public static string InsertKsFiche(string BRANCH, Bms_Fiche_Payment _PAYMENT, string FIRMNR)
        {
            bool isCustomerExist = false;
            try
            {
                var query = $"SELECT COUNT(*) FROM LG_{FIRMNR}_CLCARD WHERE CODE='{_PAYMENT.CUSTOMER_CODE}'";
                LOGYAZ($"InsertKsFiche - Sorgu: {query}", null);
                var result = SqlSelectLogo(query).Rows[0][0];
                LOGYAZ($"InsertKsFiche - Sonuç: {result}", null);
                isCustomerExist = Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                LOGYAZ($"InsertKsFiche - Müşteri sorgusu HATA: {ex.Message}", ex);
            }
            if (!isCustomerExist)
                _PAYMENT.CUSTOMER_CODE = _PAYMENT.CUSTOMER_CODE.TrimStart('0');
            try
            {
                UnityObjects.Data sd_trans = NewObjectData(UnityObjects.DataObjectType.doSafeDepositTrans);
                sd_trans.New();

                if (_PAYMENT.FTYPE == "SATIS")
                    sd_trans.DataFields.FieldByName("TYPE").Value = 11;
                else if (_PAYMENT.FTYPE == "IADE")
                    sd_trans.DataFields.FieldByName("TYPE").Value = 12;
                sd_trans.DataFields.FieldByName("SD_CODE").Value = _PAYMENT.LOGO_BANK_OR_KS_CODE;
                sd_trans.DataFields.FieldByName("DATE").Value = _PAYMENT.DATE_.Date;
                sd_trans.DataFields.FieldByName("DIVISION").Value = BRANCH;
                sd_trans.DataFields.FieldByName("AUXIL_CODE").Value = _PAYMENT.POS.ToString();
                sd_trans.DataFields.FieldByName("AUTH_CODE").Value = "BMS";
                sd_trans.DataFields.FieldByName("NUMBER").Value = "~";
                sd_trans.DataFields.FieldByName("MASTER_TITLE").Value = _PAYMENT.CUSTOMER_NAME;
                //sd_trans.DataFields.FieldByName("DESCRIPTION").Value = KASAACIKLAMASI;
                sd_trans.DataFields.FieldByName("AMOUNT").Value = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));
                sd_trans.DataFields.FieldByName("TC_XRATE").Value = 1;
                sd_trans.DataFields.FieldByName("TC_AMOUNT").Value = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));

                UnityObjects.Lines attachment_arp_lines = sd_trans.DataFields.FieldByName("ATTACHMENT_ARP").Lines;
                attachment_arp_lines.AppendLine();
                attachment_arp_lines[attachment_arp_lines.Count - 1].FieldByName("ARP_CODE").Value = _PAYMENT.CUSTOMER_CODE;
                //attachment_arp_lines[attachment_arp_lines.Count - 1].FieldByName("GL_CODE2").Value = 100;
                attachment_arp_lines[attachment_arp_lines.Count - 1].FieldByName("AUXIL_CODE").Value = _PAYMENT.POS.ToString();
                attachment_arp_lines[attachment_arp_lines.Count - 1].FieldByName("AUTH_CODE").Value = "BMS";
                attachment_arp_lines[attachment_arp_lines.Count - 1].FieldByName("TRANNO").Value = "~";
                attachment_arp_lines[attachment_arp_lines.Count - 1].FieldByName("DOC_NUMBER").Value = _PAYMENT.DOCUMENT_NO.ToString();
                //attachment_arp_lines[attachment_arp_lines.Count - 1].FieldByName("DESCRIPTION").Value = KASAACIKLAMASI;
                if (_PAYMENT.FTYPE == "SATIS")
                    attachment_arp_lines[attachment_arp_lines.Count - 1].FieldByName("CREDIT").Value = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));
                else if (_PAYMENT.FTYPE == "IADE")
                    attachment_arp_lines[attachment_arp_lines.Count - 1].FieldByName("DEBIT").Value = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));
                attachment_arp_lines[attachment_arp_lines.Count - 1].FieldByName("TC_XRATE").Value = 1;
                attachment_arp_lines[attachment_arp_lines.Count - 1].FieldByName("TC_AMOUNT").Value = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));

                sd_trans.ReCalculate();
                if (!sd_trans.Post())
                    throw new Exception(GetLastError(sd_trans));
                int LOGOLREF = Convert.ToInt32(sd_trans.DataFields.DBFieldByName("LOGICALREF").Value);
                DateTime LOGOINSERTDATE = DateTime.Now;
                if (LOGOLREF > 0)
                    return "ok";
                else return "notok";
            }
            catch (Exception E)
            {
                LOGYAZ("InsertKsFiche", E);
                return E.Message;
            }
        }

        #region SQL Insert Methods - Direct SQL Versions

        /// <summary>
        /// SQL ile doğrudan satış faturası kaydı oluşturur
        /// TRCODE 7 = Perakende Satış Faturası
        /// </summary>
        public static string InsertInvoiceSQL(string CARI_KOD, string BRANCH, Bms_Fiche_Header _BASLIK, List<Bms_Fiche_Detail> _DETAILS, bool withCustomer, string FIRMNR)
        {
            try
            {
                // Müşteri kontrolü
                string customerCode = withCustomer ? _BASLIK.CUSTOMER_CODE : CARI_KOD;
                bool isCustomerExist = false;
                try { isCustomerExist = Convert.ToInt32(SqlSelectLogo($"SELECT COUNT(*) FROM LG_{FIRMNR}_CLCARD WHERE CODE='{customerCode}'").Rows[0][0]) > 0; }
                catch { }
                if (!isCustomerExist)
                    customerCode = customerCode.TrimStart('0');

                // Müşteri LOGICALREF'i al
                int clientRef = 0;
                try { clientRef = Convert.ToInt32(SqlSelectLogo($"SELECT LOGICALREF FROM LG_{FIRMNR}_CLCARD WHERE CODE='{customerCode}'").Rows[0][0]); }
                catch { }

                // Yeni LOGICALREF ve FICHENO al
                int invoiceLogicalRef = GetNextLogicalRef($"LG_{FIRMNR}_01_INVOICE", FIRMNR);
                string ficheNo = GetNextFicheNo($"LG_{FIRMNR}_01_INVOICE", "FICHENO", 7, FIRMNR);

                // Tarih formatları
                string dateStr = _BASLIK.DATE_.ToString("yyyy-MM-dd");
                int timeVal = _BASLIK.DATE_.Hour * 256 * 256 + _BASLIK.DATE_.Minute * 256 + _BASLIK.DATE_.Second;

                // Header INSERT - LG_{FIRMNR}_01_INVOICE
                string headerSql = $@"
                INSERT INTO LG_{FIRMNR}_01_INVOICE (
                    LOGICALREF, TRCODE, FICHENO, DATE_, TIME_, DOCODE, SPECODE, CYPHCODE,
                    CLIENTREF, DOCTRACKINGNR, GENEXP6, ACCOUNTED, GENEXCTYP,
                    DETEFLAG, DECESSION, POSTTRANSFER, DOCDATE, EINVOICE, EDEFFLAG, ABORESSION, EWORKNOPAY,
                    BRANCH, SITEID, SOURCEINDEX, SOURCECOSTGRP, DESTINDEX, DESTCOSTGRP,
                    FACTORYNR, GROESSION, GRESSION, ADESSION, ADDESSION, FROMKASA
                ) VALUES (
                    {invoiceLogicalRef}, 7, '{ficheNo}', '{dateStr}', {timeVal}, '{_BASLIK.DOCUMENT_NO}', '{_BASLIK.POS}', 'BMS',
                    {clientRef}, '{_BASLIK.POS}', '{(withCustomer ? _BASLIK.FICHE_ID : "")}', 0, 1,
                    2, 3, 243, '{dateStr}', 6, 6, 6, 1,
                    {BRANCH}, 0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0, 0
                )";

                SqlExecute(headerSql);
                LOGYAZ($"InsertInvoiceSQL Header OK - LOGICALREF: {invoiceLogicalRef}", null);

                // Lines INSERT - LG_{FIRMNR}_01_STLINE
                int lineNo = 0;
                foreach (var line in _DETAILS)
                {
                    if (string.IsNullOrEmpty(line.ITEMCODE))
                    {
                        LOGYAZ($"Boş ITEMCODE atlandı - Tarih: {_BASLIK.DATE_:yyyy-MM-dd}, POS: {_BASLIK.POS}, Ürün: {line.ITEMNAME}", null);
                        continue;
                    }

                    // Ürün bilgilerini al
                    int stockRef = 0;
                    double vatRate = 0;
                    int unitRef = 0;
                    try
                    {
                        DataTable dtItem = SqlSelectLogo($"SELECT LOGICALREF, VAT FROM LG_{FIRMNR}_ITEMS WHERE CODE='{line.ITEMCODE}'");
                        if (dtItem.Rows.Count > 0)
                        {
                            stockRef = Convert.ToInt32(dtItem.Rows[0]["LOGICALREF"]);
                            vatRate = Convert.ToDouble(dtItem.Rows[0]["VAT"]);
                        }
                    }
                    catch { }

                    // Birim REF al
                    try { unitRef = Convert.ToInt32(SqlSelectLogo($"SELECT LOGICALREF FROM LG_{FIRMNR}_UNITSETL WHERE CODE='{line.ITEMUNIT}'").Rows[0][0]); }
                    catch { }

                    // Satıcı REF al
                    int salesmanRef = 0;
                    try { salesmanRef = Convert.ToInt32(SqlSelectLogo($"SELECT LOGICALREF FROM LG_{FIRMNR}_SLSMAN WHERE CODE='{line.SALESMAN}'").Rows[0][0]); }
                    catch { }

                    double priceVal = Convert.ToDouble(line.PRICE.ToString().Replace(".", ","));
                    double lineTotalVal = Convert.ToDouble(line.LINETOTAL.ToString().Replace(".", ","));
                    double unitPrice = Math.Abs(line.DISCOUNT_TOTAL) > 0 ? priceVal : lineTotalVal / line.QUANTITY;

                    int stLineRef = GetNextLogicalRef($"LG_{FIRMNR}_01_STLINE", FIRMNR);

                    string lineSql = $@"
                    INSERT INTO LG_{FIRMNR}_01_STLINE (
                        LOGICALREF, INVOICEREF, STOCKREF, LINETYPE, PREVLINEREF, PREVLINENO,
                        DETLINE, AMOUNT, PRICE, TOTAL, SHIPPEDAMOUNT,
                        UNITREF, UINFO1, UINFO2, VATINC, VAT, VATAMNT, BILLED,
                        SALESMANREF, MONTH_, YEAR_, AFFECTRISK, BARCODE, STFICHELNNO,
                        SITEID, TRCODE, DATE_, FTIME, GLOBESSION, CANCELLED, LINENO,
                        CPSTFLAG, SOURCEINDEX, SOURCECOSTGRP, DESTINDEX, DESTCOSTGRP, PESSION
                    ) VALUES (
                        {stLineRef}, {invoiceLogicalRef}, {stockRef}, 0, 0, 0,
                        0, {line.QUANTITY.ToString().Replace(",", ".")}, {unitPrice.ToString().Replace(",", ".")}, {lineTotalVal.ToString().Replace(",", ".")}, 0,
                        {unitRef}, 1, 1, 1, {vatRate.ToString().Replace(",", ".")}, 0, 1,
                        {salesmanRef}, {_BASLIK.DATE_.Month}, {_BASLIK.DATE_.Year}, 1, '{line.ITEMCODE}', {lineNo},
                        0, 7, '{dateStr}', {timeVal}, 0, 0, {lineNo},
                        1, 0, 0, 0, 0, 0
                    )";

                    SqlExecute(lineSql);
                    lineNo++;

                    // İndirim satırı varsa ekle
                    if (Math.Abs(line.DISCOUNT_TOTAL) > 0)
                    {
                        double discountVal = Math.Abs(Convert.ToDouble(line.DISCOUNT_TOTAL.ToString().Replace(".", ",")));
                        discountVal = discountVal / ((100 + vatRate) / 100);

                        int discLineRef = GetNextLogicalRef($"LG_{FIRMNR}_01_STLINE", FIRMNR);

                        string discLineSql = $@"
                        INSERT INTO LG_{FIRMNR}_01_STLINE (
                            LOGICALREF, INVOICEREF, STOCKREF, LINETYPE, PREVLINEREF, PREVLINENO,
                            DETLINE, AMOUNT, PRICE, TOTAL, DISCEXP,
                            VATINC, BILLED, MONTH_, YEAR_, AFFECTRISK, STFICHELNNO,
                            SITEID, TRCODE, DATE_, FTIME, GLOBESSION, CANCELLED, LINENO,
                            CPSTFLAG, SOURCEINDEX, SOURCECOSTGRP, DESTINDEX, DESTCOSTGRP, PESSION
                        ) VALUES (
                            {discLineRef}, {invoiceLogicalRef}, 0, 2, 0, 0,
                            1, 0, 0, {discountVal.ToString().Replace(",", ".")}, 1,
                            0, 1, {_BASLIK.DATE_.Month}, {_BASLIK.DATE_.Year}, 1, {lineNo},
                            0, 7, '{dateStr}', {timeVal}, 0, 0, {lineNo},
                            1, 0, 0, 0, 0, 0
                        )";

                        SqlExecute(discLineSql);
                        lineNo++;
                    }
                }

                LOGYAZ($"InsertInvoiceSQL Lines OK - {lineNo} satır eklendi", null);
                return "ok";
            }
            catch (Exception E)
            {
                LOGYAZ("InsertInvoiceSQL", E);
                return E.Message;
            }
        }

        /// <summary>
        /// SQL ile doğrudan iade faturası kaydı oluşturur
        /// TRCODE 2 = İade Faturası
        /// </summary>
        public static string InsertReturnInvoiceSQL(string CARI_KOD, string BRANCH, Bms_Fiche_Header _BASLIK, List<Bms_Fiche_Detail> _DETAILS, bool withCustomer, string FIRMNR, string AUTHCODE2)
        {
            try
            {
                // Müşteri kontrolü
                string customerCode = withCustomer ? _BASLIK.CUSTOMER_CODE : CARI_KOD;
                bool isCustomerExist = false;
                try { isCustomerExist = Convert.ToInt32(SqlSelectLogo($"SELECT COUNT(*) FROM LG_{FIRMNR}_CLCARD WHERE CODE='{customerCode}'").Rows[0][0]) > 0; }
                catch { }
                if (!isCustomerExist)
                    customerCode = customerCode.TrimStart('0');

                // Müşteri LOGICALREF'i al
                int clientRef = 0;
                try { clientRef = Convert.ToInt32(SqlSelectLogo($"SELECT LOGICALREF FROM LG_{FIRMNR}_CLCARD WHERE CODE='{customerCode}'").Rows[0][0]); }
                catch { }

                // Yeni LOGICALREF ve FICHENO al
                int invoiceLogicalRef = GetNextLogicalRef($"LG_{FIRMNR}_01_INVOICE", FIRMNR);
                string ficheNo = GetNextFicheNo($"LG_{FIRMNR}_01_INVOICE", "FICHENO", 2, FIRMNR);

                // Tarih formatları
                string dateStr = _BASLIK.DATE_.ToString("yyyy-MM-dd");
                int timeVal = _BASLIK.DATE_.Hour * 256 * 256 + _BASLIK.DATE_.Minute * 256 + _BASLIK.DATE_.Second;

                // Header INSERT - LG_{FIRMNR}_01_INVOICE
                string headerSql = $@"
                INSERT INTO LG_{FIRMNR}_01_INVOICE (
                    LOGICALREF, TRCODE, FICHENO, DATE_, TIME_, DOCODE, SPECODE, CYPHCODE,
                    CLIENTREF, DOCTRACKINGNR, GENEXP6, ACCOUNTED, GENEXCTYP,
                    DETEFLAG, DECESSION, POSTTRANSFER, DOCDATE, EINVOICE, EDEFFLAG, ABORESSION, EWORKNOPAY,
                    BRANCH, SITEID, SOURCEINDEX, SOURCECOSTGRP, DESTINDEX, DESTCOSTGRP,
                    FACTORYNR, GROESSION, GRESSION, ADESSION, ADDESSION, FROMKASA
                ) VALUES (
                    {invoiceLogicalRef}, 2, '{ficheNo}', '{dateStr}', {timeVal}, '{_BASLIK.FICHE_ID}', '{_BASLIK.POS}', '{AUTHCODE2}',
                    {clientRef}, '{_BASLIK.POS}', '{(withCustomer ? _BASLIK.FICHE_ID : "")}', 0, 1,
                    2, 3, 243, '{dateStr}', 6, 6, 6, 1,
                    {BRANCH}, 0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0, 0
                )";

                SqlExecute(headerSql);
                LOGYAZ($"InsertReturnInvoiceSQL Header OK - LOGICALREF: {invoiceLogicalRef}", null);

                // Lines INSERT - LG_{FIRMNR}_01_STLINE
                int lineNo = 0;
                foreach (var line in _DETAILS)
                {
                    if (string.IsNullOrEmpty(line.ITEMCODE))
                    {
                        LOGYAZ($"Boş ITEMCODE atlandı - Tarih: {_BASLIK.DATE_:yyyy-MM-dd}, POS: {_BASLIK.POS}", null);
                        continue;
                    }

                    // Ürün bilgilerini al
                    int stockRef = 0;
                    double vatRate = 0;
                    int unitRef = 0;
                    try
                    {
                        DataTable dtItem = SqlSelectLogo($"SELECT LOGICALREF, VAT FROM LG_{FIRMNR}_ITEMS WHERE CODE='{line.ITEMCODE}'");
                        if (dtItem.Rows.Count > 0)
                        {
                            stockRef = Convert.ToInt32(dtItem.Rows[0]["LOGICALREF"]);
                            vatRate = Convert.ToDouble(dtItem.Rows[0]["VAT"]);
                        }
                    }
                    catch { }

                    // Birim REF al
                    try { unitRef = Convert.ToInt32(SqlSelectLogo($"SELECT LOGICALREF FROM LG_{FIRMNR}_UNITSETL WHERE CODE='{line.ITEMUNIT}'").Rows[0][0]); }
                    catch { }

                    // Satıcı REF al
                    int salesmanRef = 0;
                    try { salesmanRef = Convert.ToInt32(SqlSelectLogo($"SELECT LOGICALREF FROM LG_{FIRMNR}_SLSMAN WHERE CODE='{line.SALESMAN}'").Rows[0][0]); }
                    catch { }

                    double lineTotalVal = Convert.ToDouble(line.LINETOTAL.ToString().Replace(".", ","));
                    double unitPrice = lineTotalVal / line.QUANTITY;

                    int stLineRef = GetNextLogicalRef($"LG_{FIRMNR}_01_STLINE", FIRMNR);

                    string lineSql = $@"
                    INSERT INTO LG_{FIRMNR}_01_STLINE (
                        LOGICALREF, INVOICEREF, STOCKREF, LINETYPE, PREVLINEREF, PREVLINENO,
                        DETLINE, AMOUNT, PRICE, TOTAL, SHIPPEDAMOUNT,
                        UNITREF, UINFO1, UINFO2, VATINC, VAT, VATAMNT, BILLED,
                        SALESMANREF, MONTH_, YEAR_, AFFECTRISK, BARCODE, STFICHELNNO,
                        SITEID, TRCODE, DATE_, FTIME, GLOBESSION, CANCELLED, LINENO,
                        CPSTFLAG, SOURCEINDEX, SOURCECOSTGRP, DESTINDEX, DESTCOSTGRP, PESSION, RETCOSTTYPE
                    ) VALUES (
                        {stLineRef}, {invoiceLogicalRef}, {stockRef}, 0, 0, 0,
                        0, {line.QUANTITY.ToString().Replace(",", ".")}, {unitPrice.ToString().Replace(",", ".")}, {lineTotalVal.ToString().Replace(",", ".")}, 0,
                        {unitRef}, 1, 1, 1, {vatRate.ToString().Replace(",", ".")}, 0, 1,
                        {salesmanRef}, {_BASLIK.DATE_.Month}, {_BASLIK.DATE_.Year}, 1, '{line.ITEMCODE}', {lineNo},
                        0, 2, '{dateStr}', {timeVal}, 0, 0, {lineNo},
                        1, 0, 0, 0, 0, 0, 1
                    )";

                    SqlExecute(lineSql);
                    lineNo++;

                    // İndirim satırı varsa ekle
                    if (Math.Abs(line.DISCOUNT_TOTAL) > 0)
                    {
                        double discountVal = Math.Abs(Convert.ToDouble(line.DISCOUNT_TOTAL.ToString().Replace(".", ",")));
                        discountVal = discountVal / ((100 + vatRate) / 100);

                        int discLineRef = GetNextLogicalRef($"LG_{FIRMNR}_01_STLINE", FIRMNR);

                        string discLineSql = $@"
                        INSERT INTO LG_{FIRMNR}_01_STLINE (
                            LOGICALREF, INVOICEREF, STOCKREF, LINETYPE, PREVLINEREF, PREVLINENO,
                            DETLINE, AMOUNT, PRICE, TOTAL, DISCEXP,
                            VATINC, BILLED, MONTH_, YEAR_, AFFECTRISK, STFICHELNNO,
                            SITEID, TRCODE, DATE_, FTIME, GLOBESSION, CANCELLED, LINENO,
                            CPSTFLAG, SOURCEINDEX, SOURCECOSTGRP, DESTINDEX, DESTCOSTGRP, PESSION
                        ) VALUES (
                            {discLineRef}, {invoiceLogicalRef}, 0, 2, 0, 0,
                            1, 0, 0, {discountVal.ToString().Replace(",", ".")}, 1,
                            0, 1, {_BASLIK.DATE_.Month}, {_BASLIK.DATE_.Year}, 1, {lineNo},
                            0, 2, '{dateStr}', {timeVal}, 0, 0, {lineNo},
                            1, 0, 0, 0, 0, 0
                        )";

                        SqlExecute(discLineSql);
                        lineNo++;
                    }
                }

                LOGYAZ($"InsertReturnInvoiceSQL Lines OK - {lineNo} satır eklendi", null);
                return "ok";
            }
            catch (Exception E)
            {
                LOGYAZ("InsertReturnInvoiceSQL", E);
                return E.Message;
            }
        }

        /// <summary>
        /// SQL ile doğrudan çek/senet bordrosu kaydı oluşturur
        /// TRCODE 1 = Müşteri Çeki Giriş Bordrosu
        /// </summary>
        public static string InsertChequeSQL(string BRANCH, Bms_Fiche_Payment _PAYMENT, string FIRMNR)
        {
            try
            {
                // Müşteri kontrolü
                bool isCustomerExist = false;
                try { isCustomerExist = Convert.ToInt32(SqlSelectLogo($"SELECT COUNT(*) FROM LG_{FIRMNR}_CLCARD WHERE CODE='{_PAYMENT.CUSTOMER_CODE}'").Rows[0][0]) > 0; }
                catch { }
                if (!isCustomerExist)
                    _PAYMENT.CUSTOMER_CODE = _PAYMENT.CUSTOMER_CODE.TrimStart('0');

                // Müşteri LOGICALREF'i al
                int clCardRef = 0;
                try { clCardRef = Convert.ToInt32(SqlSelectLogo($"SELECT LOGICALREF FROM LG_{FIRMNR}_CLCARD WHERE CODE='{_PAYMENT.CUSTOMER_CODE}'").Rows[0][0]); }
                catch { }

                // Yeni LOGICALREF ve FICHENO al
                int rollLogicalRef = GetNextLogicalRef($"LG_{FIRMNR}_01_CSROLL", FIRMNR);
                string ficheNo = GetNextFicheNo($"LG_{FIRMNR}_01_CSROLL", "FICHENO", 1, FIRMNR);

                // Tarih formatları
                string dateStr = _PAYMENT.DATE_.ToString("yyyy-MM-dd");
                int timeVal = _PAYMENT.DATE_.Hour * 256 * 256 + _PAYMENT.DATE_.Minute * 256 + _PAYMENT.DATE_.Second;

                double paymentTotal = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));

                // Header INSERT - LG_{FIRMNR}_01_CSROLL (Çek/Senet Bordrosu Header)
                string headerSql = $@"
                INSERT INTO LG_{FIRMNR}_01_CSROLL (
                    LOGICALREF, TRCODE, FICHENO, DATE_, TIME_, DOCODE,
                    MODULENR, CARDREF, SPECODE, CYPHCODE,
                    TOTAL, DOCTOTAL, AVESSION, BRANCH, SITEID,
                    GENEXP1, GENEXP2, GENEXP3, GENEXP4, GENEXP5, GENEXP6,
                    PRINTDATE, ACCOUNTED, CANCELLED
                ) VALUES (
                    {rollLogicalRef}, 1, '{ficheNo}', '{dateStr}', {timeVal}, '{_PAYMENT.DOCUMENT_NO}',
                    5, {clCardRef}, '{_PAYMENT.POS}', 'BMS',
                    {paymentTotal.ToString().Replace(",", ".")}, 1, 0, {BRANCH}, 0,
                    '', '', '', '', '', '',
                    '{dateStr}', 0, 0
                )";

                SqlExecute(headerSql);
                LOGYAZ($"InsertChequeSQL Header OK - LOGICALREF: {rollLogicalRef}", null);

                // Line INSERT - LG_{FIRMNR}_01_CSCARD (Çek/Senet Kartı)
                int csCardRef = GetNextLogicalRef($"LG_{FIRMNR}_01_CSCARD", FIRMNR);
                string cardSerialNo = GetNextFicheNo($"LG_{FIRMNR}_01_CSCARD", "SERIALNR", 1, FIRMNR);

                string lineSql = $@"
                INSERT INTO LG_{FIRMNR}_01_CSCARD (
                    LOGICALREF, CARDTYPE, CURRSTATUS, SERIALNR, OWING,
                    SPECODE, CYPHCODE, SETDATE, DESSION, DUEDATE,
                    AMOUNT, PORTFOYREF, BRANCH, SITEID,
                    AFFECTRISK, STATUSORD, CURRSEL, XMLATTR1,
                    CANCELLED, PRINTDATE
                ) VALUES (
                    {csCardRef}, 1, 1, '{(string.IsNullOrEmpty(_PAYMENT.SERIAL_NO) ? cardSerialNo : _PAYMENT.SERIAL_NO)}', '{_PAYMENT.CUSTOMER_NAME?.Replace("'", "''")}',
                    '{_PAYMENT.POS}', 'BMS', '{dateStr}', {rollLogicalRef}, '{dateStr}',
                    {paymentTotal.ToString().Replace(",", ".")}, {rollLogicalRef}, {BRANCH}, 0,
                    1, 1, 1, 1,
                    0, '{dateStr}'
                )";

                SqlExecute(lineSql);
                LOGYAZ($"InsertChequeSQL Card OK - LOGICALREF: {csCardRef}", null);

                return "ok";
            }
            catch (Exception E)
            {
                LOGYAZ("InsertChequeSQL", E);
                return E.Message;
            }
        }

        /// <summary>
        /// SQL ile doğrudan cari hesap fişi kaydı oluşturur
        /// TRCODE 70 = Kredi Kartı, 71 = Kredi Kartı İade, 3 = Borç, 4 = Alacak
        /// </summary>
        public static string InsertCHFicheSQL(string BRANCH, Bms_Fiche_Payment _PAYMENT, string FIRMNR)
        {
            try
            {
                LOGYAZ($"InsertCHFicheSQL BASLADI - BRANCH:{BRANCH}, FIRMNR:{FIRMNR}, CUSTOMER_CODE:{_PAYMENT?.CUSTOMER_CODE}", null);

                // Parametre kontrolleri
                if (_PAYMENT == null) return "_PAYMENT parametresi null";
                if (string.IsNullOrEmpty(BRANCH)) return "BRANCH parametresi boş";
                if (string.IsNullOrEmpty(FIRMNR)) return "FIRMNR parametresi boş";
                if (string.IsNullOrEmpty(_PAYMENT.CUSTOMER_CODE)) return "CUSTOMER_CODE boş";
                if (string.IsNullOrEmpty(_PAYMENT.LOGO_FICHE_TYPE)) return "LOGO_FICHE_TYPE boş";

                // Müşteri kontrolü
                bool isCustomerExist = false;
                try { isCustomerExist = Convert.ToInt32(SqlSelectLogo($"SELECT COUNT(*) FROM LG_{FIRMNR}_CLCARD WHERE CODE='{_PAYMENT.CUSTOMER_CODE}'").Rows[0][0]) > 0; }
                catch { }
                if (!isCustomerExist)
                    _PAYMENT.CUSTOMER_CODE = _PAYMENT.CUSTOMER_CODE.TrimStart('0');

                // Müşteri LOGICALREF'i al
                int clCardRef = 0;
                try { clCardRef = Convert.ToInt32(SqlSelectLogo($"SELECT LOGICALREF FROM LG_{FIRMNR}_CLCARD WHERE CODE='{_PAYMENT.CUSTOMER_CODE}'").Rows[0][0]); }
                catch { }

                // TRCODE belirle
                int trCode = 70; // Default: Kredi Kartı
                if (_PAYMENT.LOGO_FICHE_TYPE == "CH Kredi Karti Iade") trCode = 71;
                else if (_PAYMENT.LOGO_FICHE_TYPE == "CH Borc") trCode = 3;
                else if (_PAYMENT.LOGO_FICHE_TYPE == "CH Alacak") trCode = 4;

                // Yeni LOGICALREF ve FICHENO al
                int ficheLogicalRef = GetNextLogicalRef($"LG_{FIRMNR}_01_CLFICHE", FIRMNR);
                string ficheNo = GetNextFicheNo($"LG_{FIRMNR}_01_CLFICHE", "FICHENO", trCode, FIRMNR);

                // Tarih formatları
                string dateStr = _PAYMENT.DATE_.ToString("yyyy-MM-dd");
                int timeVal = _PAYMENT.DATE_.Hour * 256 * 256 + _PAYMENT.DATE_.Minute * 256 + _PAYMENT.DATE_.Second;

                double paymentTotal = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));

                // Banka hesabı REF al (kredi kartı işlemleri için)
                int bankAccRef = 0;
                if (trCode == 70 || trCode == 71)
                {
                    try { bankAccRef = Convert.ToInt32(SqlSelectLogo($"SELECT LOGICALREF FROM LG_{FIRMNR}_BANKACC WHERE CODE='{_PAYMENT.LOGO_BANK_OR_KS_CODE}'").Rows[0][0]); }
                    catch { }
                }

                // Header INSERT - LG_{FIRMNR}_01_CLFICHE (Cari Hesap Fişi Header)
                string headerSql = $@"
                INSERT INTO LG_{FIRMNR}_01_CLFICHE (
                    LOGICALREF, TRCODE, FICHENO, DATE_, TIME_,
                    SPECODE, CYPHCODE, BRANCH, SITEID,
                    PRINTDATE, ACCOUNTED, CANCELLED, GENEXCTYP,
                    DETEFLAG, DECESSION, DEESSION, CURRSEL
                ) VALUES (
                    {ficheLogicalRef}, {trCode}, '{ficheNo}', '{dateStr}', {timeVal},
                    '{_PAYMENT.POS}', 'BMS', {BRANCH}, 0,
                    '{dateStr}', 0, 0, 1,
                    0, 0, 0, 1
                )";

                SqlExecute(headerSql);
                LOGYAZ($"InsertCHFicheSQL Header OK - LOGICALREF: {ficheLogicalRef}", null);

                // Line INSERT - LG_{FIRMNR}_01_CLFLINE (Cari Hesap Fişi Satırı)
                int lineLogicalRef = GetNextLogicalRef($"LG_{FIRMNR}_01_CLFLINE", FIRMNR);
                string tranNo = GetNextFicheNo($"LG_{FIRMNR}_01_CLFLINE", "TRANNO", trCode, FIRMNR);

                // DEBIT/CREDIT belirleme
                string debitCredit = "";
                if (_PAYMENT.LOGO_FICHE_TYPE == "CH Kredi Karti" || _PAYMENT.LOGO_FICHE_TYPE == "CH Alacak")
                    debitCredit = $"DEBIT = 0, CREDIT = {paymentTotal.ToString().Replace(",", ".")}";
                else
                    debitCredit = $"DEBIT = {paymentTotal.ToString().Replace(",", ".")}, CREDIT = 0";

                string lineSql = $@"
                INSERT INTO LG_{FIRMNR}_01_CLFLINE (
                    LOGICALREF, CLIENTREF, TRCODE, FICHESSION, TRANNO,
                    DATE_, TIME_, DOCODE, SPECODE, CYPHCODE,
                    {(debitCredit.Contains("CREDIT =") && debitCredit.Contains("DEBIT =") ? "DEBIT, CREDIT," : (debitCredit.Contains("CREDIT =") ? "CREDIT," : "DEBIT,"))}
                    TRRATE, TRNET, REPORTRATE, REPORTNET,
                    MONTH_, YEAR_, BRANCH, SITEID,
                    ACCOUNTED, CANCELLED, AFFECTRISK,
                    {(bankAccRef > 0 ? "BANKACCREF," : "")} CURRSEL
                ) VALUES (
                    {lineLogicalRef}, {clCardRef}, {trCode}, {ficheLogicalRef}, '{tranNo}',
                    '{dateStr}', {timeVal}, '{_PAYMENT.DOCUMENT_NO}', '{_PAYMENT.POS}', 'BMS',
                    {paymentTotal.ToString().Replace(",", ".")},
                    1, {paymentTotal.ToString().Replace(",", ".")}, 1, {paymentTotal.ToString().Replace(",", ".")},
                    {_PAYMENT.DATE_.Month}, {_PAYMENT.DATE_.Year}, {BRANCH}, 0,
                    0, 0, 1,
                    {(bankAccRef > 0 ? $"{bankAccRef}," : "")} 1
                )";

                // Düzeltilmiş line SQL
                lineSql = $@"
                INSERT INTO LG_{FIRMNR}_01_CLFLINE (
                    LOGICALREF, CLIENTREF, TRCODE, FICHESSION, TRANNO,
                    DATE_, TIME_, DOCODE, SPECODE, CYPHCODE,
                    DEBIT, CREDIT,
                    TRRATE, TRNET, REPORTRATE, REPORTNET,
                    MONTH_, YEAR_, BRANCH, SITEID,
                    ACCOUNTED, CANCELLED, AFFECTRISK, CURRSEL
                    {(bankAccRef > 0 ? ", BANKACCREF" : "")}
                ) VALUES (
                    {lineLogicalRef}, {clCardRef}, {trCode}, {ficheLogicalRef}, '{tranNo}',
                    '{dateStr}', {timeVal}, '{_PAYMENT.DOCUMENT_NO}', '{_PAYMENT.POS}', 'BMS',
                    {(_PAYMENT.LOGO_FICHE_TYPE == "CH Kredi Karti Iade" || _PAYMENT.LOGO_FICHE_TYPE == "CH Borc" ? paymentTotal.ToString().Replace(",", ".") : "0")},
                    {(_PAYMENT.LOGO_FICHE_TYPE == "CH Kredi Karti" || _PAYMENT.LOGO_FICHE_TYPE == "CH Alacak" ? paymentTotal.ToString().Replace(",", ".") : "0")},
                    1, {paymentTotal.ToString().Replace(",", ".")}, 1, {paymentTotal.ToString().Replace(",", ".")},
                    {_PAYMENT.DATE_.Month}, {_PAYMENT.DATE_.Year}, {BRANCH}, 0,
                    0, 0, 1, 1
                    {(bankAccRef > 0 ? $", {bankAccRef}" : "")}
                )";

                SqlExecute(lineSql);
                LOGYAZ($"InsertCHFicheSQL Line OK - LOGICALREF: {lineLogicalRef}", null);

                return "ok";
            }
            catch (Exception E)
            {
                LOGYAZ("InsertCHFicheSQL", E);
                return E.Message;
            }
        }

        /// <summary>
        /// SQL ile doğrudan kasa fişi kaydı oluşturur
        /// TRCODE 11 = Nakit Tahsilat, 12 = Nakit Ödeme
        /// </summary>
        public static string InsertKsFicheSQL(string BRANCH, Bms_Fiche_Payment _PAYMENT, string FIRMNR)
        {
            try
            {
                // Müşteri kontrolü
                bool isCustomerExist = false;
                try { isCustomerExist = Convert.ToInt32(SqlSelectLogo($"SELECT COUNT(*) FROM LG_{FIRMNR}_CLCARD WHERE CODE='{_PAYMENT.CUSTOMER_CODE}'").Rows[0][0]) > 0; }
                catch { }
                if (!isCustomerExist)
                    _PAYMENT.CUSTOMER_CODE = _PAYMENT.CUSTOMER_CODE.TrimStart('0');

                // Müşteri LOGICALREF'i al
                int clCardRef = 0;
                try { clCardRef = Convert.ToInt32(SqlSelectLogo($"SELECT LOGICALREF FROM LG_{FIRMNR}_CLCARD WHERE CODE='{_PAYMENT.CUSTOMER_CODE}'").Rows[0][0]); }
                catch { }

                // Kasa REF al
                int ksCardRef = 0;
                try { ksCardRef = Convert.ToInt32(SqlSelectLogo($"SELECT LOGICALREF FROM LG_{FIRMNR}_KSCARD WHERE CODE='{_PAYMENT.LOGO_BANK_OR_KS_CODE}'").Rows[0][0]); }
                catch { }

                // TRCODE belirle
                int trCode = _PAYMENT.FTYPE == "SATIS" ? 11 : 12;

                // Yeni LOGICALREF ve FICHENO al
                int ksLineRef = GetNextLogicalRef($"LG_{FIRMNR}_01_KSLINES", FIRMNR);
                string ficheNo = GetNextFicheNo($"LG_{FIRMNR}_01_KSLINES", "FICHENO", trCode, FIRMNR);

                // Tarih formatları
                string dateStr = _PAYMENT.DATE_.ToString("yyyy-MM-dd");
                int timeVal = _PAYMENT.DATE_.Hour * 256 * 256 + _PAYMENT.DATE_.Minute * 256 + _PAYMENT.DATE_.Second;

                double paymentTotal = Convert.ToDouble(_PAYMENT.PAYMENT_TOTAL.ToString().Replace(".", ","));

                // Kasa İşlem Satırı INSERT - LG_{FIRMNR}_01_KSLINES
                string ksSql = $@"
                INSERT INTO LG_{FIRMNR}_01_KSLINES (
                    LOGICALREF, TRCODE, FICHENO, DATE_, TIME_,
                    CARDREF, AMOUNT, TRRATE, TRNET, REPORTRATE, REPORTNET,
                    SPECODE, CYPHCODE, MASESSION, BRANCH, SITEID,
                    ACCOUNTED, CANCELLED, CURRSEL, LINEEXP
                ) VALUES (
                    {ksLineRef}, {trCode}, '{ficheNo}', '{dateStr}', {timeVal},
                    {ksCardRef}, {paymentTotal.ToString().Replace(",", ".")}, 1, {paymentTotal.ToString().Replace(",", ".")}, 1, {paymentTotal.ToString().Replace(",", ".")},
                    '{_PAYMENT.POS}', 'BMS', '{_PAYMENT.CUSTOMER_NAME?.Replace("'", "''")}', {BRANCH}, 0,
                    0, 0, 1, '{_PAYMENT.CUSTOMER_NAME?.Replace("'", "''")}'
                )";

                SqlExecute(ksSql);
                LOGYAZ($"InsertKsFicheSQL KS Line OK - LOGICALREF: {ksLineRef}", null);

                // Cari Hesap Satırı INSERT - LG_{FIRMNR}_01_CLFLINE
                int clfLineRef = GetNextLogicalRef($"LG_{FIRMNR}_01_CLFLINE", FIRMNR);
                string tranNo = GetNextFicheNo($"LG_{FIRMNR}_01_CLFLINE", "TRANNO", trCode, FIRMNR);

                string clfSql = $@"
                INSERT INTO LG_{FIRMNR}_01_CLFLINE (
                    LOGICALREF, CLIENTREF, TRCODE, TRANNO,
                    DATE_, TIME_, DOCODE, SPECODE, CYPHCODE,
                    DEBIT, CREDIT,
                    TRRATE, TRNET, REPORTRATE, REPORTNET,
                    MONTH_, YEAR_, BRANCH, SITEID,
                    ACCOUNTED, CANCELLED, AFFECTRISK, CURRSEL, ABORESSION
                ) VALUES (
                    {clfLineRef}, {clCardRef}, {trCode}, '{tranNo}',
                    '{dateStr}', {timeVal}, '{_PAYMENT.DOCUMENT_NO}', '{_PAYMENT.POS}', 'BMS',
                    {(_PAYMENT.FTYPE == "IADE" ? paymentTotal.ToString().Replace(",", ".") : "0")},
                    {(_PAYMENT.FTYPE == "SATIS" ? paymentTotal.ToString().Replace(",", ".") : "0")},
                    1, {paymentTotal.ToString().Replace(",", ".")}, 1, {paymentTotal.ToString().Replace(",", ".")},
                    {_PAYMENT.DATE_.Month}, {_PAYMENT.DATE_.Year}, {BRANCH}, 0,
                    0, 0, 1, 1, {ksLineRef}
                )";

                SqlExecute(clfSql);
                LOGYAZ($"InsertKsFicheSQL CLF Line OK - LOGICALREF: {clfLineRef}", null);

                return "ok";
            }
            catch (Exception E)
            {
                LOGYAZ("InsertKsFicheSQL", E);
                return E.Message;
            }
        }

        /// <summary>
        /// SQL ile doğrudan muhasebe fişi kaydı oluşturur
        /// TRCODE 4 = Mahsup Fişi
        /// </summary>
        public static string CreateAccountingFicheSQL(int lOGOLREF, string FIRMNR)
        {
            try
            {
                // Sipariş bilgilerini al
                DateTime ORDER_SIPARIS_TARIHI = new DateTime();
                try { ORDER_SIPARIS_TARIHI = Convert.ToDateTime(SqlSelectLogo($"SELECT DATE_ FROM LG_{FIRMNR}_01_ORFICHE WHERE LOGICALREF={lOGOLREF}").Rows[0][0]); }
                catch { }

                string ORDER_SIPARIS_NO = "";
                try { ORDER_SIPARIS_NO = SqlSelectLogo($"SELECT FICHENO FROM LG_{FIRMNR}_01_ORFICHE WHERE LOGICALREF={lOGOLREF}").Rows[0][0].ToString(); }
                catch { }

                string ORDER_CARI_KOD = "";
                try { ORDER_CARI_KOD = SqlSelectLogo($"SELECT (SELECT TOP 1 CODE FROM LG_{FIRMNR}_CLCARD C WHERE C.LOGICALREF=CLIENTREF) FROM LG_{FIRMNR}_01_ORFICHE WHERE LOGICALREF={lOGOLREF}").Rows[0][0].ToString(); }
                catch { }

                int ORDER_DOVIZ_TIPI = 0;
                try { ORDER_DOVIZ_TIPI = Convert.ToInt32(SqlSelectLogo($"SELECT TRCURR FROM LG_{FIRMNR}_01_ORFICHE WHERE LOGICALREF={lOGOLREF}").Rows[0][0]); }
                catch { }

                double raporlamaDoviziRate = getRateFromDB(20, ORDER_SIPARIS_TARIHI.Date, FIRMNR);
                double islemDoviziRate = getRateFromDB(ORDER_DOVIZ_TIPI, ORDER_SIPARIS_TARIHI.Date, FIRMNR);

                int OrficheAccountref = 0;
                string OrficheAccountCode = "";
                double OrficheNettotal = 0.00;
                try { OrficheAccountref = Convert.ToInt32(SqlSelectLogo($@"SELECT TOP 1 ACCOUNTREF FROM LG_{FIRMNR}_01_ORFICHE WHERE LOGICALREF={lOGOLREF}").Rows[0][0]); }
                catch { }
                try { OrficheNettotal = Convert.ToDouble(SqlSelectLogo($@"SELECT TOP 1 NETTOTAL FROM LG_{FIRMNR}_01_ORFICHE WHERE LOGICALREF={lOGOLREF}").Rows[0][0]); }
                catch { }
                if (OrficheAccountref > 0)
                    OrficheAccountCode = SqlSelectLogo($@"SELECT TOP 1 CODE FROM LG_{FIRMNR}_EMUHACC WHERE LOGICALREF={OrficheAccountref}").Rows[0][0].ToString();

                // Masraf merkezi ve bölüm bilgilerini al
                string masrafMerkezi = get4LogoMasrafMerkezi(ORDER_CARI_KOD, FIRMNR)?.ToString() ?? "";
                int department = Convert.ToInt32(get4LogoDepartment(ORDER_CARI_KOD, FIRMNR) ?? 0);
                int division = Convert.ToInt32(get4LogoDivision(ORDER_CARI_KOD, FIRMNR) ?? 0);

                // Yeni LOGICALREF ve FICHENO al
                int emFicheRef = GetNextLogicalRef($"LG_{FIRMNR}_01_EMFICHE", FIRMNR);
                string ficheNo = GetNextFicheNo($"LG_{FIRMNR}_01_EMFICHE", "FICHENO", 4, FIRMNR);

                // Tarih formatları
                string dateStr = ORDER_SIPARIS_TARIHI.ToString("yyyy-MM-dd");
                int timeVal = ORDER_SIPARIS_TARIHI.Hour * 256 * 256 + ORDER_SIPARIS_TARIHI.Minute * 256 + ORDER_SIPARIS_TARIHI.Second;

                // Header INSERT - LG_{FIRMNR}_01_EMFICHE (Muhasebe Fişi Header)
                string headerSql = $@"
                INSERT INTO LG_{FIRMNR}_01_EMFICHE (
                    LOGICALREF, TRCODE, FICHENO, DATE_, TIME_,
                    CYPHCODE, BRANCH, DEPARTMENT, SITEID,
                    GENEXP4, CURRSEL, DESSION, DEESSION,
                    ACCOUNTED, CANCELLED, DOCDATE, PRINTDATE
                ) VALUES (
                    {emFicheRef}, 4, '{ficheNo}', '{dateStr}', {timeVal},
                    'BMS', {division}, {department}, 0,
                    'SIPARISNO:{ORDER_SIPARIS_NO}', 1, 2, 0,
                    0, 0, '{dateStr}', '{dateStr}'
                )";

                SqlExecute(headerSql);
                LOGYAZ($"CreateAccountingFicheSQL Header OK - LOGICALREF: {emFicheRef}", null);

                // İlk satır - Borç (120.xx hesabı)
                int emfLineRef = GetNextLogicalRef($"LG_{FIRMNR}_01_EMFLINE", FIRMNR);

                // Masraf merkezi ref al
                int centerRef = 0;
                if (!string.IsNullOrEmpty(masrafMerkezi))
                {
                    try { centerRef = Convert.ToInt32(SqlSelectLogo($"SELECT LOGICALREF FROM LG_{FIRMNR}_EMCENTER WHERE CODE='{masrafMerkezi}'").Rows[0][0]); }
                    catch { }
                }

                string kebirCode = OrficheAccountCode.Length >= 3 ? OrficheAccountCode.Substring(0, 3) : OrficheAccountCode;

                string firstLineSql = $@"
                INSERT INTO LG_{FIRMNR}_01_EMFLINE (
                    LOGICALREF, ACCFICHEREF, ACCOUNTREF, ACCOUNTCODE, KEBIRCODE,
                    DEBIT, CREDIT, TRRATE, TRNET, REPORTRATE, REPORTNET,
                    LINENO, LINEEXP, TRCURR, CENTERREF, DEPARTMENT,
                    MONTH_, YEAR_, BRANCH, SITEID, DOCDATE,
                    EMUDEBIT, EMUCREDIT, CURRSEL
                ) VALUES (
                    {emfLineRef}, {emFicheRef}, {OrficheAccountref}, '{OrficheAccountCode}', '{kebirCode}',
                    {Math.Round(OrficheNettotal, 2).ToString().Replace(",", ".")}, 0,
                    {islemDoviziRate.ToString().Replace(",", ".")}, {Math.Round(OrficheNettotal / islemDoviziRate, 2).ToString().Replace(",", ".")},
                    {raporlamaDoviziRate.ToString().Replace(",", ".")}, {Math.Round(OrficheNettotal / raporlamaDoviziRate, 2).ToString().Replace(",", ".")},
                    1, '{ORDER_CARI_KOD}', {ORDER_DOVIZ_TIPI}, {centerRef}, {department},
                    {ORDER_SIPARIS_TARIHI.Month}, {ORDER_SIPARIS_TARIHI.Year}, {division}, 0, '{dateStr}',
                    {Math.Round(OrficheNettotal / raporlamaDoviziRate, 2).ToString().Replace(",", ".")}, 0, 2
                )";

                SqlExecute(firstLineSql);
                LOGYAZ($"CreateAccountingFicheSQL First Line OK - LOGICALREF: {emfLineRef}", null);

                // Sipariş satırlarından alacak kayıtları oluştur
                DataTable dataTable = SqlSelectLogo($@"SELECT * FROM LG_{FIRMNR}_01_ORFLINE WHERE ORDFICHEREF={lOGOLREF}");
                int lineNo = 2;

                foreach (DataRow dr in dataTable.Rows)
                {
                    int OrflineAccountref = 0;
                    string OrflineAccountCode = "";
                    int OrflineVatAccref = 0;
                    string OrflineVatAccCode = "";
                    int OrflineVat = 0;
                    double OflineLineNet = 0.00;
                    double OrflineVatamnt = 0.00;
                    int ORDERLINE_DOVIZ_TIPI = 0;

                    try { OrflineAccountref = Convert.ToInt32(dr["ACCOUNTREF"]); } catch { }
                    try { OrflineVatAccref = Convert.ToInt32(dr["VATACCREF"]); } catch { }
                    try { OrflineVat = Convert.ToInt32(dr["VAT"]); } catch { }
                    try { OflineLineNet = Math.Round(Convert.ToDouble(dr["LINENET"]), 2); } catch { }
                    try { OrflineVatamnt = Math.Round(Convert.ToDouble(dr["VATAMNT"]), 2); } catch { }
                    try { ORDERLINE_DOVIZ_TIPI = Convert.ToInt32(dr["TRCURR"]); } catch { }

                    if (OrflineAccountref > 0)
                        OrflineAccountCode = SqlSelectLogo($@"SELECT TOP 1 CODE FROM LG_{FIRMNR}_EMUHACC WHERE LOGICALREF={OrflineAccountref}").Rows[0][0].ToString();
                    if (OrflineVatAccref > 0)
                        OrflineVatAccCode = SqlSelectLogo($@"SELECT TOP 1 CODE FROM LG_{FIRMNR}_EMUHACC WHERE LOGICALREF={OrflineVatAccref}").Rows[0][0].ToString();

                    string ORDERLINE_STOK_KODU = "";
                    string ORDER_STOK_REF = dr["STOCKREF"].ToString();
                    try { ORDERLINE_STOK_KODU = SqlSelectLogo($@"SELECT TOP 1 CODE FROM LG_{FIRMNR}_SRVCARD WHERE LOGICALREF={ORDER_STOK_REF}").Rows[0][0].ToString(); } catch { }

                    // Gelir satırı
                    int lineRef = GetNextLogicalRef($"LG_{FIRMNR}_01_EMFLINE", FIRMNR);
                    string lineKebirCode = OrflineAccountCode.Length >= 3 ? OrflineAccountCode.Substring(0, 3) : OrflineAccountCode;

                    string lineSql = $@"
                    INSERT INTO LG_{FIRMNR}_01_EMFLINE (
                        LOGICALREF, ACCFICHEREF, ACCOUNTREF, ACCOUNTCODE, KEBIRCODE,
                        DEBIT, CREDIT, TRRATE, TRNET, REPORTRATE, REPORTNET,
                        LINENO, LINEEXP, TRCURR, CENTERREF, DEPARTMENT, SIGN,
                        MONTH_, YEAR_, BRANCH, SITEID, DOCDATE,
                        EMUDEBIT, EMUCREDIT, CURRSEL, DATAREF
                    ) VALUES (
                        {lineRef}, {emFicheRef}, {OrflineAccountref}, '{OrflineAccountCode}', '{lineKebirCode}',
                        0, {OflineLineNet.ToString().Replace(",", ".")},
                        {islemDoviziRate.ToString().Replace(",", ".")}, {Math.Round(OflineLineNet / islemDoviziRate, 2).ToString().Replace(",", ".")},
                        {raporlamaDoviziRate.ToString().Replace(",", ".")}, {Math.Round(OflineLineNet / raporlamaDoviziRate, 2).ToString().Replace(",", ".")},
                        {lineNo++}, '{ORDERLINE_STOK_KODU}', {ORDERLINE_DOVIZ_TIPI}, {centerRef}, {department}, 1,
                        {ORDER_SIPARIS_TARIHI.Month}, {ORDER_SIPARIS_TARIHI.Year}, {division}, 0, '{dateStr}',
                        0, {Math.Round(OflineLineNet / raporlamaDoviziRate, 2).ToString().Replace(",", ".")}, 2, 5
                    )";

                    SqlExecute(lineSql);

                    // KDV satırı
                    if (OrflineVat > 0 && OrflineVatamnt > 0)
                    {
                        int vatLineRef = GetNextLogicalRef($"LG_{FIRMNR}_01_EMFLINE", FIRMNR);
                        string vatKebirCode = OrflineVatAccCode.Length >= 3 ? OrflineVatAccCode.Substring(0, 3) : OrflineVatAccCode;

                        string vatLineSql = $@"
                        INSERT INTO LG_{FIRMNR}_01_EMFLINE (
                            LOGICALREF, ACCFICHEREF, ACCOUNTREF, ACCOUNTCODE, KEBIRCODE,
                            DEBIT, CREDIT, TRRATE, TRNET, REPORTRATE, REPORTNET,
                            LINENO, LINEEXP, TRCURR, CENTERREF, DEPARTMENT, SIGN,
                            MONTH_, YEAR_, BRANCH, SITEID, DOCDATE,
                            EMUDEBIT, EMUCREDIT, CURRSEL
                        ) VALUES (
                            {vatLineRef}, {emFicheRef}, {OrflineVatAccref}, '{OrflineVatAccCode}', '{vatKebirCode}',
                            0, {OrflineVatamnt.ToString().Replace(",", ".")},
                            {islemDoviziRate.ToString().Replace(",", ".")}, {Math.Round(OrflineVatamnt / islemDoviziRate, 2).ToString().Replace(",", ".")},
                            {raporlamaDoviziRate.ToString().Replace(",", ".")}, {Math.Round(OrflineVatamnt / raporlamaDoviziRate, 2).ToString().Replace(",", ".")},
                            {lineNo++}, 'KDV % {OrflineVat}', {ORDERLINE_DOVIZ_TIPI}, {centerRef}, {department}, 1,
                            {ORDER_SIPARIS_TARIHI.Month}, {ORDER_SIPARIS_TARIHI.Year}, {division}, 0, '{dateStr}',
                            0, {Math.Round(OrflineVatamnt / raporlamaDoviziRate, 2).ToString().Replace(",", ".")}, 2
                        )";

                        SqlExecute(vatLineSql);
                    }
                }

                LOGYAZ($"CreateAccountingFicheSQL Lines OK - {lineNo - 1} satır eklendi", null);
                return "ok";
            }
            catch (Exception E)
            {
                LOGYAZ("CreateAccountingFicheSQL", E);
                return E.Message;
            }
        }

        /// <summary>
        /// SQL ile sipariş faturası muhasebe düzeltmesi yapar
        /// 120 hesabını 380'e, 600 ve 391 hesaplarını 390'a dönüştürür
        /// </summary>
        public static string RevisetInvoiceForOrderToInvoiceFromPaymentIntegrationSQL(int LOGOLREF, string FIRMNR)
        {
            try
            {
                int AccFicheref = 0;
                try { AccFicheref = Convert.ToInt32(SqlSelectLogo($"SELECT TOP 1 ACCFICHEREF FROM LG_{FIRMNR}_01_INVOICE WHERE LOGICALREF={LOGOLREF}").Rows[0][0]); }
                catch { }

                if (AccFicheref > 0)
                {
                    // 120 hesabını bul
                    DataRow Dr120AkaOldLine = null;
                    try { Dr120AkaOldLine = SqlSelectLogo($"SELECT TOP 1 * FROM LG_{FIRMNR}_01_EMFLINE WHERE ACCFICHEREF={AccFicheref} AND ACCOUNTCODE LIKE '120.01.%' ORDER BY LOGICALREF ASC").Rows[0]; }
                    catch { }

                    // 600 hesap toplamları
                    DataRow Dr600Sums = null;
                    try { Dr600Sums = SqlSelectLogo($"SELECT SUM(CREDIT) CREDIT, SUM(REPORTNET) REPORTNET, SUM(TRNET) TRNET, SUM(EMUDEBIT) EMUDEBIT FROM LG_{FIRMNR}_01_EMFLINE WHERE ACCFICHEREF={AccFicheref} AND ACCOUNTCODE LIKE '600.01.%'").Rows[0]; }
                    catch { }

                    // 391 hesap bilgileri
                    DataRow Dr391AkaCopyFrom = null;
                    try { Dr391AkaCopyFrom = SqlSelectLogo($"SELECT TOP 1 * FROM LG_{FIRMNR}_01_EMFLINE WHERE ACCFICHEREF={AccFicheref} AND ACCOUNTCODE LIKE '391.01.%' ORDER BY LOGICALREF ASC").Rows[0]; }
                    catch { }

                    DataRow Dr391Sums = null;
                    try { Dr391Sums = SqlSelectLogo($"SELECT SUM(CREDIT) CREDIT, SUM(REPORTNET) REPORTNET, SUM(TRNET) TRNET, SUM(EMUDEBIT) EMUDEBIT FROM LG_{FIRMNR}_01_EMFLINE WHERE ACCFICHEREF={AccFicheref} AND ACCOUNTCODE LIKE '391.01.%'").Rows[0]; }
                    catch { }

                    if (Dr120AkaOldLine != null && Dr600Sums != null)
                    {
                        // 120 hesabını 380'e güncelle
                        string newAccountCode = Dr120AkaOldLine["ACCOUNTCODE"].ToString().Replace("120.01.", "380.01.");
                        int newAccountref = 0;
                        try { newAccountref = Convert.ToInt32(SqlSelectLogo($"SELECT TOP 1 LOGICALREF FROM LG_{FIRMNR}_EMUHACC WHERE CODE='{newAccountCode}'").Rows[0][0]); }
                        catch { }

                        string newKebirCode = newAccountCode.Substring(0, 3);
                        double newDebit = Convert.ToDouble(Dr600Sums["CREDIT"]);
                        double newReportnet = Convert.ToDouble(Dr600Sums["REPORTNET"]);
                        double newTrnet = Convert.ToDouble(Dr600Sums["TRNET"]);
                        double newEmuDebit = Convert.ToDouble(Dr600Sums["EMUDEBIT"]);

                        string updateSql = $@"UPDATE LG_{FIRMNR}_01_EMFLINE
                            SET ACCOUNTREF={newAccountref}, ACCOUNTCODE='{newAccountCode}', KEBIRCODE='{newKebirCode}',
                                DEBIT={newDebit.ToString().Replace(",", ".")},
                                REPORTNET={newReportnet.ToString().Replace(",", ".")},
                                TRNET={newTrnet.ToString().Replace(",", ".")},
                                EMUDEBIT={newEmuDebit.ToString().Replace(",", ".")}
                            WHERE LOGICALREF={Dr120AkaOldLine["LOGICALREF"]}";

                        SqlExecute(updateSql);
                        LOGYAZ($"RevisetInvoiceForOrderToInvoiceFromPaymentIntegrationSQL 380 Update OK", null);

                        // 390 satırı ekle (391 toplamları ile)
                        if (Dr391AkaCopyFrom != null && Dr391Sums != null)
                        {
                            int newLineRef = GetNextLogicalRef($"LG_{FIRMNR}_01_EMFLINE", FIRMNR);
                            int maxLineNo = 0;
                            try { maxLineNo = Convert.ToInt32(SqlSelectLogo($"SELECT MAX(LINENO) FROM LG_{FIRMNR}_01_EMFLINE WHERE ACCFICHEREF={AccFicheref}").Rows[0][0]); }
                            catch { }

                            // 390.01.001 hesabının ref'ini al
                            int account390Ref = 0;
                            try { account390Ref = Convert.ToInt32(SqlSelectLogo($"SELECT TOP 1 LOGICALREF FROM LG_{FIRMNR}_EMUHACC WHERE CODE='390.01.001'").Rows[0][0]); }
                            catch { }

                            // Masraf merkezi bilgisini al
                            int centerRef = 0;
                            try { centerRef = Convert.ToInt32(Dr391AkaCopyFrom["CENTERREF"]); } catch { }

                            string insertSql = $@"
                            INSERT INTO LG_{FIRMNR}_01_EMFLINE (
                                LOGICALREF, ACCFICHEREF, ACCOUNTREF, ACCOUNTCODE, KEBIRCODE,
                                DEBIT, CREDIT, TRRATE, TRNET, REPORTRATE, REPORTNET,
                                LINENO, LINEEXP, TRCURR, CENTERREF, DEPARTMENT, SIGN,
                                MONTH_, YEAR_, BRANCH, SITEID, DOCDATE,
                                EMUDEBIT, EMUCREDIT, CURRSEL
                            ) VALUES (
                                {newLineRef}, {AccFicheref}, {account390Ref}, '390.01.001', '390',
                                {Convert.ToDouble(Dr391Sums["CREDIT"]).ToString().Replace(",", ".")}, 0,
                                {Convert.ToDouble(Dr391AkaCopyFrom["TRRATE"]).ToString().Replace(",", ".")},
                                {Convert.ToDouble(Dr391Sums["TRNET"]).ToString().Replace(",", ".")},
                                {Convert.ToDouble(Dr391AkaCopyFrom["REPORTRATE"]).ToString().Replace(",", ".")},
                                {Convert.ToDouble(Dr391Sums["REPORTNET"]).ToString().Replace(",", ".")},
                                {maxLineNo + 1}, '{Dr391AkaCopyFrom["LINEEXP"]}', {Convert.ToInt32(Dr391AkaCopyFrom["TRCURR"])},
                                {centerRef}, {Convert.ToInt32(Dr391AkaCopyFrom["DEPARTMENT"])}, 0,
                                {Convert.ToInt32(Dr391AkaCopyFrom["MONTH_"])}, {Convert.ToInt32(Dr391AkaCopyFrom["YEAR_"])},
                                {Convert.ToInt32(Dr391AkaCopyFrom["BRANCH"])}, 0,
                                '{Convert.ToDateTime(Dr391AkaCopyFrom["DOCDATE"]):yyyy-MM-dd}',
                                {Convert.ToDouble(Dr391Sums["EMUDEBIT"]).ToString().Replace(",", ".")}, 0, 2
                            )";

                            SqlExecute(insertSql);
                            LOGYAZ($"RevisetInvoiceForOrderToInvoiceFromPaymentIntegrationSQL 390 Insert OK", null);
                        }
                    }
                }

                return "ok";
            }
            catch (Exception E)
            {
                LOGYAZ("RevisetInvoiceForOrderToInvoiceFromPaymentIntegrationSQL", E);
                return E.Message;
            }
        }

        /// <summary>
        /// SQL ile borç kapatma işlemini geri alır
        /// </summary>
        public static string RollBackDebtCloseSQL(int REF, string FIRMNR)
        {
            try
            {
                // Borç kapatma kaydını sil
                string deleteSql = $"DELETE FROM LG_{FIRMNR}_01_CLFLINE WHERE LOGICALREF={REF}";
                SqlExecute(deleteSql);
                LOGYAZ($"RollBackDebtCloseSQL OK - REF: {REF} silindi", null);
                return "ok";
            }
            catch (Exception E)
            {
                LOGYAZ("RollBackDebtCloseSQL", E);
                return E.Message;
            }
        }

        #endregion

        #region SQL Helper Methods

        /// <summary>
        /// Belirtilen tablo için bir sonraki LOGICALREF değerini döndürür
        /// </summary>
        private static int GetNextLogicalRef(string tableName, string FIRMNR)
        {
            int maxRef = 0;
            try
            {
                DataTable dt = SqlSelectLogo($"SELECT ISNULL(MAX(LOGICALREF), 0) AS MAXREF FROM {tableName}");
                if (dt.Rows.Count > 0)
                    maxRef = Convert.ToInt32(dt.Rows[0]["MAXREF"]);
            }
            catch (Exception ex)
            {
                LOGYAZ($"GetNextLogicalRef Error - Table: {tableName}", ex);
            }
            return maxRef + 1;
        }

        /// <summary>
        /// Belirtilen tablo ve TRCODE için bir sonraki fiş numarasını döndürür
        /// </summary>
        private static string GetNextFicheNo(string tableName, string ficheNoColumn, int trCode, string FIRMNR)
        {
            string prefix = "";
            int maxNo = 0;

            // TRCODE'a göre prefix belirle
            switch (trCode)
            {
                case 2: prefix = "IR"; break;  // İade
                case 3: prefix = "BC"; break;  // Borç
                case 4: prefix = "AC"; break;  // Alacak / Mahsup
                case 7: prefix = "PS"; break;  // Perakende Satış
                case 11: prefix = "NT"; break; // Nakit Tahsilat
                case 12: prefix = "NO"; break; // Nakit Ödeme
                case 70: prefix = "KK"; break; // Kredi Kartı
                case 71: prefix = "KI"; break; // Kredi Kartı İade
                case 1: prefix = "CK"; break;  // Çek Girişi
                default: prefix = "XX"; break;
            }

            try
            {
                // TRCODE'a göre en son numarayı bul
                DataTable dt = SqlSelectLogo($@"
                    SELECT TOP 1 {ficheNoColumn} FROM {tableName}
                    WHERE {ficheNoColumn} LIKE '{prefix}%' AND TRCODE={trCode}
                    ORDER BY {ficheNoColumn} DESC");

                if (dt.Rows.Count > 0)
                {
                    string lastNo = dt.Rows[0][0].ToString();
                    if (lastNo.Length > prefix.Length)
                    {
                        int.TryParse(lastNo.Substring(prefix.Length), out maxNo);
                    }
                }
            }
            catch { }

            return $"{prefix}{(maxNo + 1).ToString("D10")}";
        }

        #endregion

    }
}
