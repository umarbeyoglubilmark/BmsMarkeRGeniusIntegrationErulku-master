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

    }
}
