using BmsMarkeRGeniusIntegrationCfg;
using BmsMarkeRGeniusIntegrationLibrary;
using BmsMarkeRGeniusIntegrationLibrary;
using BmsMarkeRGeniusIntegrationLibrary.METHODS.MODELS;
using BmsMarkeRGeniusIntegrationLibrary.METHODS.MODELS;
using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.Office.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Preview;
using DevExpress.XtraSplashScreen;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly;
using Polly;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Contrib.WaitAndRetry;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
// If you use AddPolicyHandler:
using Polly.Extensions.Http;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Data;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Globalization;
using System.Globalization;
using System.IO;
using System.IO;
using System.Linq;
using System.Linq;
using System.Net;                 // DecompressionMethods
using System.Net;
using System.Net;
using System.Net;
using System.Net.Http;
using System.Net.Http;            // HttpClientHandler
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Net.Http.Headers;
// SSL bypass için gerekirse:
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;
using UnityObjects;
using static BmsMarkeRGeniusIntegrationLibrary.HELPER;
using static BmsMarkeRGeniusIntegrationLibrary.HELPER;
using Application = System.Windows.Forms.Application;
using static Integration.BmsMarkeRGeniusIntegrationCfg.Genius2Logo.Integration.Frm_PosEOD;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Linq.Expressions;

namespace Integration.BmsMarkeRGeniusIntegrationCfg.Genius2Logo.Integration
{
    public partial class Frm_PosEOD : DevExpress.XtraEditors.XtraForm
    {
        string POS_GLOBAL = "";
        public static CONFIG CFG { get; set; } = new CONFIG();
        //string TABLENAME_Sales = "BMSF_XXX_MarkeRGenius_Sales";
        //string TABLENAME_Sales_WithCustomer = "BMSF_XXX_MarkeRGenius_Sales_WithCustomer";
        string QueryFile_Sales = @"Queries\Sales.sql";
        string QueryFile_Sales_WithCustomer = @"Queries\Sales_WithCustomer.sql";
        string QueryFile_Payments = @"Queries\Payments.sql";
        public Frm_PosEOD(string HEADERNAME, bool isAdmin)
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
            ce_DontDebtClose.Checked = CFG.DODEBTCLOSE == "1" ? false : true;
            ce_DontDebtClose.Visible = CFG.DODEBTCLOSE == "1" ? true : false;
            ce_WithoutControl.Visible = isAdmin;
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

            if (!ce_OnlyPayments.Checked)
                if (ce_WithoutControl.Checked == false)
                    existenceController(checkedBranches, sqlFormattedDateStart, sqlFormattedDateEnd);

            try
            {
                string strLogin = HELPER.LOGO_LOGIN(CFG.LOBJECTDEFAULTUSERNAME, CFG.LOBJECTDEFAULTPASSWORD, int.Parse(CFG.FIRMNR));
                if (strLogin != "") throw new Exception(strLogin);
                foreach (string branch in checkedBranches)
                {
                    if (ce_OnlySalesWithCustomer.Checked)
                        Sales_WithCustomer(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
            
                    else if (ce_OnlyPayments.Checked)
                        Payments(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
                    else
                    {
                        Sales(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
                        Sales_WithCustomer(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
                        Payments(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
                    }
                    if (!ce_DontDebtClose.Checked)
                    {
                        DebtClose(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd, 1);
                        DebtClose(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd, 2);
                        DebtClose(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd, 3);
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

        private static readonly JsonSerializerOptions _jsonOpts = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
        };




        private IReadOnlyList<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data> PullSalesFromApi()
        {
            var (list, diag) = PullSalesFromApiAsync(CancellationToken.None)
                               .GetAwaiter().GetResult();

            // İstersen diagnostikleri burada logla
            if (diag != null && (diag.Count is null || diag.Count == 0))
                HELPER.LOGYAZ("Sales API DIAG:\n" + diag, null);

            return list ?? Array.Empty<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>();
        }

        private async Task<(IReadOnlyList<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data> List, ApiDiag Diag)>
    PullSalesFromApiAsyncOld(CancellationToken ct)
{
    var diag = new ApiDiag();

    // 1) Tarih (UTC 00:00)
    var d = de_DateStart.DateTime.Date;
    var utcMidnight = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0, DateTimeKind.Utc);
    string dateUtcString = utcMidnight.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);

    // 2) Parametreler (gerekirse CFG)
    string storeCode = "MGZ1";
    string posCode   = "9";
    int    salesType = 0;
    bool   excludeCancelledLines = true;

    // 3) Uçlar
    var baseUrl       = "http://192.168.3.10:9996/";
    var salesEndpoint = "api/Reports/sales";

    // Proxy kapalı
    ServicePointManager.SecurityProtocol =
        SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
    var handler = new HttpClientHandler { UseProxy = false, Proxy = null };

    // 4) Token
    string token = await AuthApi.GetTokenAsync(
        storeId: 0, posId: 0, cashierId: 0,
        username: "kasa",
        password: "81dc9bdb52d04dc20036dbd8313ed055",
        timeout: TimeSpan.FromSeconds(30)
    ).ConfigureAwait(false);

    // 5) Body
    var bodyObj = new
    {
        date = dateUtcString,
        excludeCancelledLines,
        storeCode,
        salesType,
        posCode
    };
    var json = Newtonsoft.Json.JsonConvert.SerializeObject(bodyObj);
    diag.RequestBody = json;

    // 6) POST
     var http = new HttpClient(handler) { BaseAddress = new Uri(baseUrl), Timeout = TimeSpan.FromSeconds(60) };
    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    http.DefaultRequestHeaders.Accept.Clear();
    http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var fullUri = new Uri(http.BaseAddress, salesEndpoint);
    diag.Endpoint = fullUri.ToString();

     var content = new StringContent(json, Encoding.UTF8, "application/json");
     var resp = await http.PostAsync(salesEndpoint, content, ct).ConfigureAwait(false);

    var respText = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
    diag.StatusCode = (int)resp.StatusCode;
    diag.Reason     = resp.ReasonPhrase ?? "";
    diag.RawBodySnippet = respText;

    if (!resp.IsSuccessStatusCode)
        return (Array.Empty<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>(), diag);


    try
    {
                
        var root = JToken.Parse(respText);
        if (root is JArray arrRoot)
        {
            var list = arrRoot.ToObject<List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>>();
            diag.Count = list.Count;
            diag.EnvelopeKeys = "array";
            return (list, diag);
        }

        if (root is JObject obj)
        {
            diag.EnvelopeKeys = string.Join(",", obj.Properties().Select(p => p.Name));

            var successTok = obj["success"];
            if (successTok != null && successTok.Type == JTokenType.Boolean && !successTok.Value<bool>())
            {
                diag.Message = obj["message"]?.ToString() ?? obj["errorMessage"]?.ToString() ?? obj["error"]?.ToString();
                return (Array.Empty<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>(), diag);
            }

            var node =
                  obj["datas"] ?? obj["Datas"]
               ?? obj["data"]  ?? obj["Data"]
               ?? obj.SelectToken("result.datas")   ?? obj.SelectToken("result.data")
               ?? obj.SelectToken("payload.datas")  ?? obj.SelectToken("payload.data")
               ?? obj.SelectToken("response.datas") ?? obj.SelectToken("response.data")
               ?? obj.SelectToken("Result.Datas")   ?? obj.SelectToken("Result.Data");

            if (node is JArray arr)
            {
                var list = arr.ToObject<List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>>();
                diag.Count = list.Count;
                return (list, diag);
            }
            if (node is JObject objNode)
            {
                var items = (objNode["items"] ?? objNode["Items"]) as JArray;
                if (items != null)
                {
                    var list = items.ToObject<List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>>() ;
                    diag.Count = list.Count;
                    return (list, diag);
                }
                var single = objNode.ToObject<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>();
                if (single != null)
                {
                    var list = new List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data> { single };
                    diag.Count = 1;
                    return (list, diag);
                }
            }
        }

        // JSON var ama beklenen düğüm yok
        return (Array.Empty<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>(), diag);
    }
    catch (Exception ex)
    {
        diag.Message = "JSON parse exception: " + ex.Message;
        return (Array.Empty<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>(), diag);
    }
}

        private async Task<(IReadOnlyList<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data> List, ApiDiag Diag)>
    PullSalesFromApiAsync(CancellationToken ct)
        {
            var diag = new ApiDiag();
            var allResults = new List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>();

            // 1) Tarih (UTC 00:00)
            var d = de_DateStart.DateTime.Date;
            var utcMidnight = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0, DateTimeKind.Utc);
            string dateUtcString = utcMidnight.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);

            // Parametreler
            string storeCode = "MGZ1";
            int salesType = 0;
            bool excludeCancelledLines = true;

            // Uçlar
            var baseUrl = "http://192.168.3.10:9996/";
            var salesEndpoint = "api/Reports/sales";

            // Proxy kapalı
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var handler = new HttpClientHandler { UseProxy = false, Proxy = null };

            // Token
            string token = await AuthApi.GetTokenAsync(
                storeId: 0, posId: 0, cashierId: 0,
                username: "kasa",
                password: "81dc9bdb52d04dc20036dbd8313ed055",
                timeout: TimeSpan.FromSeconds(30)
            ).ConfigureAwait(false);

            var http = new HttpClient(handler) { BaseAddress = new Uri(baseUrl), Timeout = TimeSpan.FromSeconds(60) };
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Birden fazla posCode için dön
            string[] posCodes = { "4","8", "9", "10", "12" };
            foreach (var posCode in posCodes)
            {
                POS_GLOBAL = posCode;
                var bodyObj = new
                {
                    date = dateUtcString,
                    excludeCancelledLines,
                    storeCode,
                    salesType,
                    posCode
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(bodyObj);
                diag.RequestBody = json;

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await http.PostAsync(salesEndpoint, content, ct).ConfigureAwait(false);
                var respText = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!resp.IsSuccessStatusCode)
                    continue;

                try
                {
                    var root = JToken.Parse(respText);
                    if (root is JArray arrRoot)
                    {
                        allResults.AddRange(arrRoot.ToObject<List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>>());
                        continue;
                    }
                    if (root is JObject obj)
                    {
                        var node =
                              obj["datas"] ?? obj["Datas"]
                           ?? obj["data"] ?? obj["Data"]
                           ?? obj.SelectToken("result.datas") ?? obj.SelectToken("result.data")
                           ?? obj.SelectToken("payload.datas") ?? obj.SelectToken("payload.data")
                           ?? obj.SelectToken("response.datas") ?? obj.SelectToken("response.data")
                           ?? obj.SelectToken("Result.Datas") ?? obj.SelectToken("Result.Data");

                        if (node is JArray arr)
                        {
                            allResults.AddRange(arr.ToObject<List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>>());
                        }
                    }
                }
                catch (Exception ex)
                {
                    diag.Message = "JSON parse exception for posCode " + posCode + ": " + ex.Message;
                }
            }

            diag.Count = allResults.Count;
            return (allResults, diag);
        }

        // Yanıtı güvenli şekilde çözen yardımcı
        private static IReadOnlyList<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data> ParseSalesResponse(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return new List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>();

                var root = JToken.Parse(json);

                // 1) Düz dizi: [ {...}, {...} ]
                if (root is JArray arrRoot)
                    return arrRoot.ToObject<List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>>();

                // 2) Zarf senaryoları
                if (root is JObject obj)
                {
                    // success=false ise mesajla fail et
                    var successTok = obj["success"];
                    if (successTok != null && successTok.Type == JTokenType.Boolean && !successTok.Value<bool>())
                    {
                        var msg = obj["message"]?.ToString()
                              ?? obj["errorMessage"]?.ToString()
                              ?? obj["error"]?.ToString()
                              ?? "Request failed";
                        throw new InvalidOperationException($"API response says success=false. Message: {msg}\nBody: {json}");
                    }

                    // En yaygın düğümler: datas/Data(s) → data/Data; ayrıca result.* / payload.*
                    JToken node =
                          obj["datas"] ?? obj["Datas"]
                       ?? obj["data"] ?? obj["Data"]
                       ?? obj.SelectToken("result.datas") ?? obj.SelectToken("result.data")
                       ?? obj.SelectToken("payload.datas") ?? obj.SelectToken("payload.data")
                       ?? obj.SelectToken("response.datas") ?? obj.SelectToken("response.data")
                       ?? obj.SelectToken("Result.Datas") ?? obj.SelectToken("Result.Data");

                    if (node is JArray arr)
                        return arr.ToObject<List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>>();

                    if (node is JObject objNode)
                    {
                        // data: { items:[...] } / datas: { items:[...] }
                        var items = (objNode["items"] ?? objNode["Items"]) as JArray;
                        if (items != null)
                            return items.ToObject<List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>>();

                        // Tek nesne fallback
                        var single = objNode.ToObject<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>();
                        if (single != null)
                            return new List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data> { single };
                    }
     
                }

                // 3) Hâlâ bulunamadıysa üst seviye anahtarları raporla
                var keys = (root as JObject)?.Properties().Select(p => p.Name) ?? Enumerable.Empty<string>();
                throw new InvalidOperationException("Unexpected JSON shape. Keys: " + string.Join(",", keys));
            }
            catch (Exception ex) {
                HELPER.LOGYAZ(ex.ToString(), null);

            }
            return new List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>();
        }


        // Yanıtı güvenli şekilde çözen yardımcı
        private static IReadOnlyList<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data> ParseSalesResponse2(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>();

            var root = JToken.Parse(json);

            // Düz dizi: [ {...}, {...} ]
            if (root is JArray arrRoot)
                return arrRoot.ToObject<List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>>();

            // Zarf: { success, data } / { result: { data } } / { data: { items } } / tek nesne
            JToken node = null;
            if (root is JObject obj)
            {
                // success=false ise hata mesajını taşı
                var success = obj["success"]?.ToObject<bool?>();
                if (success == false)
                {
                    var msg = obj["message"]?.ToString() ?? obj["error"]?.ToString() ?? "Request failed";
                    throw new InvalidOperationException($"API response says success=false. Message: {msg}\nBody: {json}");
                }

                node = obj["data"] ?? obj["Data"]
                    ?? obj.SelectToken("result.data")
                    ?? obj.SelectToken("payload.data")
                    ?? obj.SelectToken("response.data")
                    ?? obj.SelectToken("Result.Data");
            }

            if (node is JArray arr) // data: [...]
                return arr.ToObject<List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>>();

            if (node is JObject objNode) // data: { items:[...] } veya tek nesne
            {
                var items = (objNode["items"] ?? objNode["Items"]) as JArray;
                if (items != null)
                    return items.ToObject<List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>>();

                var single = objNode.ToObject<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>();
                if (single != null)
                    return new List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data> { single };
            }

            // Son çare: en üstteki anahtarları bildir
            var keys = (root as JObject)?.Properties();
            throw new InvalidOperationException("Unexpected JSON shape. Keys: " + string.Join(",", keys ?? Array.Empty<JProperty>()));
        }


        // Map storeCode -> LogoBranch to compare with 'branch' parameter
        private string ResolveLogoBranch(string storeCode)
        {
            try
            {
                var dt = HELPER.SqlSelectLogo(
                    $@"SELECT TOP 1 LogoBranch FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping WHERE StoreCode='{storeCode}'");
                if (dt.Rows.Count > 0) return Convert.ToString(dt.Rows[0][0], CultureInfo.InvariantCulture);
            }
            catch { }
            return "1";
        }

        private string ResolveDefaultCustomer(BmsMarkeRGeniusIntegrationLibrary.HELPER.Data sale)
        {
            var code = sale?.customerData?.code;
            if (!string.IsNullOrWhiteSpace(code)) return code.TrimStart('0');

            try
            {
                var tbl = (sale.total < 0 || sale.documentType == 2)
                    ? $"BMS_{CFG.FIRMNR}_MarkeRGenius_ReturnClient"
                    : $"BMS_{CFG.FIRMNR}_MarkeRGenius_InvoiceClient";
                var dt = HELPER.SqlSelectLogo($@"SELECT TOP 1 NR FROM {tbl} ORDER BY NR");
                if (dt.Rows.Count > 0) return Convert.ToString(dt.Rows[0][0], CultureInfo.InvariantCulture);
            }
            catch { }
            return "120.YKF";
        }

        // Push a single sale into Logo (keeps your existing helper semantics)
        private void PostSaleToLogo(BmsMarkeRGeniusIntegrationLibrary.HELPER.Data sale, string branch, bool withCustomer)
        {
            try
            {

                var header = new Bms_Fiche_Header
                {
                    FICHE_ID = sale.posDocumentId.ToString(),
                    DATE_ = sale.date,
                    BRANCH = int.TryParse(branch, out var b) ? b : 1,
                    POS = int.TryParse(sale.posCode, out var p) ? p : 0,
                    DOCUMENT_NO = !string.IsNullOrWhiteSpace(sale.documentNo) ? sale.documentNo : sale.receiptNo,
                    CUSTOMER_CODE = withCustomer ? ResolveDefaultCustomer(sale) : null,
                    FTYPE = (sale.documentType == 2 || sale.total < 0) ? "IADE" : "SATIS",
                
                };
               
                var details = new List<Bms_Fiche_Detail>();
                foreach (var l in sale.lines ?? new List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Line>())
                {
            
                   
                    var lineTotal = l.TotalPrice;
                    var price = lineTotal;

                    details.Add(new Bms_Fiche_Detail
                    {
                      
                        ITEMCODE = l.productCode,
                        ITEMUNIT = string.IsNullOrWhiteSpace(l.productUnit) ? "ADET" : l.productUnit,
                        QUANTITY = l.amount,
                        PRICE = price,
                        LINETOTAL = lineTotal,
                        DISCOUNT_TOTAL = Convert.ToDecimal(l.discountTotal),
                        SALESMAN = l.salesmanCode
                    });
                }

                if (sale.posCode == "4") {
                    var res2 = HELPER.InsertReturnInvoice("120.YKF", branch, header, details, withCustomer: withCustomer, FIRMNR: CFG.FIRMNR, "BMS-NCR");
                    if (!string.Equals(res2, "ok", StringComparison.OrdinalIgnoreCase))
                        HELPER.LOGYAZ($"Logo post failed for {header.DOCUMENT_NO}: {res2}", null);
                    return;
                }
                var res = header.FTYPE == "SATIS"
                    ? InsertInvoice2("120.YKF", branch, header, details, withCustomer: withCustomer, FIRMNR: CFG.FIRMNR)
                    : HELPER.InsertReturnInvoice("120.YKF", branch, header, details, withCustomer: withCustomer, FIRMNR: CFG.FIRMNR,"BMS-NCR");

                if (!string.Equals(res, "ok", StringComparison.OrdinalIgnoreCase))
                    HELPER.LOGYAZ($"Logo post failed for {header.DOCUMENT_NO}: {res}", null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                HELPER.LOGYAZ(ex.ToString(), null);
            }
        }

        // Payment helpers
        private static string ClassifyPayment(BmsMarkeRGeniusIntegrationLibrary.HELPER.Payment p, BmsMarkeRGeniusIntegrationLibrary.HELPER.Data sale)
        {
            var name = (p.paymentName ?? p.name ?? p.paymentCode ?? p.code ?? string.Empty).ToLowerInvariant();
            var isReturn = sale.total < 0 || (sale.documentType == 2);
            if (name.Contains("cek") || name.Contains("cheque")) return "Cek Girisi";
            if (name.Contains("nakit") || name.Contains("cash")) return isReturn ? "Kasa Odeme" : "Kasa Tahsilat";
            if (name.Contains("kk") || name.Contains("kredi") || name.Contains("card") || p.creditCardDetails != null)
                return isReturn ? "CH Kredi Karti Iade" : "CH Kredi Karti";
            return isReturn ? "CH Borc" : "CH Alacak";
        }



        private void SalesNCR(List<Bms_Errors> errorList, string branch, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            try
            {
                var start = DateTime.Parse(sqlFormattedDateStart, CultureInfo.InvariantCulture);
                var end = DateTime.Parse(sqlFormattedDateEnd, CultureInfo.InvariantCulture);

                var all = PullSalesFromApi();
                var filtered = all
                    .Where(s => s.date >= start && s.date <= end)
                    .Where(s => ResolveLogoBranch(s.storeCode) == branch)
                    // plain retail: no customer code
                    .Where(s => string.IsNullOrWhiteSpace(s.customerData?.code))
                    .ToList();

                foreach (var sale in filtered)
                    PostSaleToLogo(sale, branch, withCustomer: false);

            }
            catch (Exception ex)
            {
                HELPER.LOGYAZ(ex.ToString(), null);
            }
        }


        private void Sales_WithCustomerNCR(List<Bms_Errors> errorList, string branch, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            try
            {
               
                var start = DateTime.Parse(sqlFormattedDateStart, CultureInfo.InvariantCulture);
                var end = DateTime.Parse(sqlFormattedDateEnd, CultureInfo.InvariantCulture);

                var all = PullSalesFromApi();
                var filtered = all
                  //  .Where(s => s.date >= start && s.date <= end)
                  //  .Where(s => ResolveLogoBranch(s.storeCode) == branch)
                    // customer-present documents
                   // .Where(s => !string.IsNullOrWhiteSpace(s.customerData?.code))
                    .ToList();

                foreach (var sale in all)
                    PostSaleToLogo(sale, branch, withCustomer: true);

            }
            catch(Exception ex) {
                HELPER.LOGYAZ(ex.ToString(), null);
            
            }
        }

        private void PaymentsNCR(List<Bms_Errors> errorList, string branch, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            try
            {
                var start = DateTime.Parse(sqlFormattedDateStart, CultureInfo.InvariantCulture);
                var end = DateTime.Parse(sqlFormattedDateEnd, CultureInfo.InvariantCulture);

                var all = PullSalesFromApi();
                var filtered = all
                      //  .Where(s => s.date >= start && s.date <= end)
                      //  .Where(s => ResolveLogoBranch(s.storeCode) == branch)
                      // customer-present documents
                      // .Where(s => !string.IsNullOrWhiteSpace(s.customerData?.code))
                      .ToList();

                foreach (var sale in all)
                {
                    var customerCode = ResolveDefaultCustomer(sale);
                    foreach (var p in sale.payments)
                    {
                        var ficheType = ClassifyPayment(p, sale);
                        var total = Convert.ToDecimal(p.amount) / 100m;
                        var paymentDoc = string.IsNullOrWhiteSpace(sale.documentNo) ? sale.receiptNo : sale.documentNo;

                        var dto = new Bms_Fiche_Payment
                        {
                            LOGO_FICHE_TYPE = ficheType,
                            CUSTOMER_CODE = customerCode,
                            POS = int.TryParse(sale.posCode, out var pos) ? pos : 0,
                            DATE_ = sale.date,
                            DOCUMENT_NO = paymentDoc,
                            PAYMENT_TOTAL = total
                        };

                        string result =
                            ficheType.Equals("Cek Girisi", StringComparison.OrdinalIgnoreCase) ? HELPER.InsertCheque(branch, dto, CFG.FIRMNR) :
                            ficheType.StartsWith("Kasa", StringComparison.OrdinalIgnoreCase) ? HELPER.InsertKsFiche(branch, dto, CFG.FIRMNR) :
                                                                                                    HELPER.InsertCHFiche(branch, dto, CFG.FIRMNR);

                        if (!string.Equals(result, "ok", StringComparison.OrdinalIgnoreCase))
                            HELPER.LOGYAZ(result, null);

                    }
                }
            }
            catch (Exception ex)
            {
                HELPER.LOGYAZ(ex.ToString(), null);
            }
        }


        private static void DebtClose(List<Bms_Errors> errorList, string branch, string sqlFormattedDateStart, string sqlFormattedDateEnd, int count)
        {

            string sqlQuery = $@"SELECT PAYTRANS_INVOICE,BRANCH,DOCODE,DATE_INVOICE,CLIENTREF,SPECODE,PAYTRANS_TOTAL from BMS_{CFG.FIRMNR}_MarkeRGenius_DebtClose_Invoice where BRANCH = {branch} AND DATE_INVOICE BETWEEN '{sqlFormattedDateStart}' AND '{sqlFormattedDateEnd}' ORDER BY PAYTRANS_TOTAL DESC";
            DataTable fhl = HELPER.SqlSelectLogo(sqlQuery);
            foreach (DataRow item in fhl.Rows)
            {
                double percantage = (double)fhl.Rows.IndexOf(item) / (double)fhl.Rows.Count;
                SplashScreenManager.Default.SetWaitFormDescription($"Borç Kapatma. " + (percantage * 100).ToString("0.00") + "%");
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
                    {
                        string sqlQueryHeader = $@"SELECT FICHENO FROM LG_{CFG.FIRMNR}_01_INVOICE II WHERE II.TIME_=0 AND II.POSTRANSFERINFO=1 AND II.CYPHCODE='BMS' AND II.DATE_ = '{sqlFormattedDate}' AND II.BRANCH = {branch}";
                        DataTable fhl = HELPER.SqlSelectLogo(sqlQueryHeader);

                        if (fhl.Rows.Count > 0)
                        {
                            var ficheNos = fhl.AsEnumerable().Select(r => r.Field<string>("FICHENO")).ToList();
                            errorList.Add(new Bms_Errors()
                            {
                                BRANCH = int.Parse(branch),
                                POS = 0,
                                FTYPE = "",
                                DATE_ = date,
                                ERRORMESSAGE = "Bu tarih için daha önce günsonu yapılmıştır. Logo Fatura Noları : " + string.Join(",", ficheNos)
                            });
                        }
                    }
                    {
                        string wherePos = $@"(SELECT DISTINCT CAST(GP.NR AS VARCHAR) AS NR FROM BMS_{CFG.FIRMNR}_MarkeRGenius_GeniusPos GP WITH(NOLOCK) )";
                        string sqlQueryPayments = $@"SELECT TOP 1 LOGICALREF FROM LG_{CFG.FIRMNR}_01_CSROLL WITH(NOLOCK)  WHERE CYPHCODE='BMS' AND DATE_ = '{sqlFormattedDate}' AND BRANCH = {branch} AND SPECODE IN ({wherePos})
UNION ALL
SELECT LOGICALREF FROM LG_{CFG.FIRMNR}_01_CLFICHE WITH(NOLOCK)  WHERE CYPHCODE='BMS' AND DATE_ = '{sqlFormattedDate}' AND BRANCH = {branch} AND SPECCODE IN ({wherePos})
UNION ALL
SELECT LOGICALREF FROM LG_{CFG.FIRMNR}_01_KSLINES  WITH(NOLOCK) WHERE CYPHCODE='BMS' AND DATE_ = '{sqlFormattedDate}' AND BRANCH = {branch} AND SPECODE IN ({wherePos})";
                        DataTable fhl = HELPER.SqlSelectLogo(sqlQueryPayments);


                        if (fhl.Rows.Count > 0)
                        {
                            var logicalRefs = fhl.AsEnumerable().Select(r => r.Field<int>("LOGICALREF")).ToList();
                            errorList.Add(new Bms_Errors()
                            {
                                BRANCH = int.Parse(branch),
                                POS = 0,
                                FTYPE = "",
                                DATE_ = date,
                                ERRORMESSAGE = "Bu tarih için daha önce tahsilat yapılmıştır. Logo Referans Noları : " + string.Join(",", logicalRefs)
                            });
                        }
                    }
                    {//GENIUS CARI KONTROL
                        string viewName = $@"Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Control_Client";
                        string sqlQuery = $@"SELECT KOD,AD FROM {viewName}";
                        DataTable fhl = HELPER.SqlSelectLogo(sqlQuery);
                        if (fhl.Rows.Count == 0)
                        {
                            foreach (DataRow item in fhl.Rows)
                            {
                                string KOD = item["KOD"].ToString();
                                string AD = item["AD"].ToString();
                                errorList.Add(new Bms_Errors()
                                {
                                    BRANCH = int.Parse(branch),
                                    POS = 0,
                                    FTYPE = "",
                                    DATE_ = date,
                                    ERRORMESSAGE = "Cari Logoda Bulunamadı : " + KOD + " - " + AD
                                });
                            }
                        }

                    }
                    {//GENIUS URUN KONTROL
                        string viewName = $@"Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Control_Items";
                        string sqlQuery = $@"SELECT KOD,AD FROM {viewName}";
                        DataTable fhl = HELPER.SqlSelectLogo(sqlQuery);
                        if (fhl.Rows.Count == 0)
                        {
                            foreach (DataRow item in fhl.Rows)
                            {
                                string KOD = item["KOD"].ToString();
                                string AD = item["AD"].ToString();
                                errorList.Add(new Bms_Errors()
                                {
                                    BRANCH = int.Parse(branch),
                                    POS = 0,
                                    FTYPE = "",
                                    DATE_ = date,
                                    ERRORMESSAGE = "Ürün Logoda Bulunamadı : " + KOD + " - " + AD
                                });
                            }
                        }
                    }
                    {//GENIUS SATISELEMANI KONTROL
                        string viewName = $@"Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Control_Salesman";
                        string sqlQuery = $@"SELECT KOD,AD FROM {viewName}";
                        DataTable fhl = HELPER.SqlSelectLogo(sqlQuery);
                        if (fhl.Rows.Count == 0)
                        {
                            foreach (DataRow item in fhl.Rows)
                            {
                                string KOD = item["KOD"].ToString();
                                string AD = item["AD"].ToString();
                                errorList.Add(new Bms_Errors()
                                {
                                    BRANCH = int.Parse(branch),
                                    POS = 0,
                                    FTYPE = "",
                                    DATE_ = date,
                                    ERRORMESSAGE = "Satış Elemanı Logoda Bulunamadı : " + KOD + " - " + AD
                                });
                            }
                        }

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
            foreach (var item in fhl)
            {
                double percantage = (double)fhl.IndexOf(item) / (double)fhl.Count;
                SplashScreenManager.Default.SetWaitFormDescription("(1/3)Kasa Satışlar(İşyeri " + branch.ToString() + "). " + (percantage * 100).ToString("0.00") + "%");
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
                    result = HELPER.InsertReturnInvoice(le_ReturnClient.EditValue.ToString(), branch, item, fdl, false, CFG.FIRMNR,"BMS");
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
        private void Payments(List<Bms_Errors> errorList, string branch, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            string ipFromBranch = HELPER.SqlSelectLogo($@"SELECT TOP 1 Ip FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping WHERE LogoBranch = {branch}").Rows[0][0].ToString();
            string sqlQuery = File.ReadAllText(QueryFile_Payments);
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
            foreach (var item in fhl)
            {
                double percantage = (double)fhl.IndexOf(item) / (double)fhl.Count;
                SplashScreenManager.Default.SetWaitFormDescription("(3/3)Tahsilarlar(İşyeri " + branch.ToString() + "). " + (percantage * 100).ToString("0.00") + "%");
                if (string.IsNullOrEmpty(item.DOCUMENT_NO) && string.IsNullOrEmpty(item.CUSTOMER_CODE))
                {
                    item.CUSTOMER_CODE = le_InvoiceClient.EditValue.ToString();
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
        private void Sales_WithCustomer(List<Bms_Errors> errorList, string branch, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            string ipFromBranch = HELPER.SqlSelectLogo($@"SELECT TOP 1 Ip FROM Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping WHERE LogoBranch = {branch}").Rows[0][0].ToString();
            string sqlQuery = File.ReadAllText(QueryFile_Sales_WithCustomer);
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
            HELPER.LOGYAZ(sqlQueryHeader, null);
            foreach (var item in fhl)
            {
                double percantage = (double)fhl.IndexOf(item) / (double)fhl.Count;
                SplashScreenManager.Default.SetWaitFormDescription("(2/3)Kasa Satışlar-Carili(İşyeri " + branch.ToString() + "). " + (percantage * 100).ToString("0.00") + "%");
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
                    result = HELPER.InsertInvoice(le_InvoiceClient.EditValue.ToString(), branch, item, fdl, true, CFG.FIRMNR);
                }
                else if (item.FTYPE == "IADE")
                {
                    //IADE FATURASI
                    result = HELPER.InsertReturnInvoice(le_ReturnClient.EditValue.ToString(), branch, item, fdl, true, CFG.FIRMNR,"BMS");
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

        private void ce_OnlySalesWithCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (ce_OnlySalesWithCustomer.Checked)
                ce_OnlyPayments.Enabled = false;
            else
                ce_OnlyPayments.Enabled = true;
        }

        private void ce_OnlyPayments_CheckedChanged(object sender, EventArgs e)
        {
            if (ce_OnlyPayments.Checked)
                ce_OnlySalesWithCustomer.Enabled = false;
            else
                ce_OnlySalesWithCustomer.Enabled = true;
        }

        public sealed class SalesApiOptions
        {
            public string BaseUrl { get; set; } = default;
            public string Endpoint { get; set; } = default;
            public string BearerToken { get; set; }
            public int PollIntervalSeconds { get; set; } = 60;
            public bool IgnoreSslErrors { get; set; } = false;
            public int WindowMinutes { get; set; } = 120;
        }

        // ---- Envelope matching your API ----
        // success/httpStatusCode/statusCode + datas[]
        // (You already have the inner sale model 'Data' in your project.)  // :contentReference[oaicite:2]{index=2}
        public sealed class ApiEnvelope<T>
        {
            public bool success { get; set; }
            public int httpStatusCode { get; set; }
            public int statusCode { get; set; }
            public string message { get; set; }
            public string errorMessage { get; set; }
            public List<T> datas { get; set; }
        }

        public sealed class LogoInvoiceHandler : ISalesHandler
        {
            private readonly ILogger<LogoInvoiceHandler> _log;
            private readonly string _firmNr;
            private readonly string _defaultArpForCash;

            public LogoInvoiceHandler(ILogger<LogoInvoiceHandler> log, IOptions<SalesApiOptions> opt)
            {
                _log = log;
                _firmNr = /* CFG.FIRMNR */ "125";
                _defaultArpForCash = /* config/DB */ "120.YKF";
            }

            public async Task HandleAsync(BmsMarkeRGeniusIntegrationLibrary.HELPER.Data sale, CancellationToken ct)
            {
                // --- HEADER MAP ---
                try
                {
                    var h = new Bms_Fiche_Header
                    {
                        FICHE_ID = sale.posDocumentId.ToString(),
                        DATE_ = sale.date,                              // DateTime
                        BRANCH = int.Parse(ResolveBranch(sale.storeCode)),          // storeCode -> BRANCH
                        POS = int.Parse(sale.posCode),                           // string; InsertInvoice ToString() çağırıyor
                        DOCUMENT_NO = !string.IsNullOrWhiteSpace(sale.documentNo) ? sale.documentNo : sale.receiptNo,
                        CUSTOMER_CODE = sale.customerData?.code,
                        FTYPE = IsReturn(sale) ? "IADE" : "SATIS"
                    };

                    // --- LINES MAP ---
                    var details = new List<Bms_Fiche_Detail>();
                    var lines = sale.lines ?? new List<BmsMarkeRGeniusIntegrationLibrary.HELPER.Line>();

                    foreach (var l in lines)
                    {
                        // Tipleriniz int görünüyor; Logo tarafı double/decimal istiyor olabilir -> güvenli cast
                        var qty = Convert.ToDecimal(l.quantity);
                        var lineTotal = Convert.ToDecimal(l.TotalPrice);        // satır toplamı
                        var unitPrice = l.amount > 0 ? Convert.ToDecimal(l.amount)
                                                     : (qty != 0 ? lineTotal / qty : 0m);

                        details.Add(new Bms_Fiche_Detail
                        {
                            ITEMCODE = l.productCode,
                            ITEMUNIT = string.IsNullOrWhiteSpace(l.productUnit) ? "ADET" : l.productUnit,
                            QUANTITY = (double)qty,
                            PRICE = unitPrice,
                            LINETOTAL = lineTotal,
                            DISCOUNT_TOTAL = Convert.ToDecimal(l.discountTotal),
                            SALESMAN = l.salesmanCode
                        });
                    }

                    // --- LOGO CALL ---
                    string branchStr = h.BRANCH.ToString();
                    string res = (h.FTYPE == "SATIS")
                        ? InsertInvoice2(_defaultArpForCash, branchStr, h, details, withCustomer: !string.IsNullOrEmpty(h.CUSTOMER_CODE), FIRMNR: _firmNr)
                        : HELPER.InsertReturnInvoice(_defaultArpForCash, branchStr, h, details, withCustomer: !string.IsNullOrEmpty(h.CUSTOMER_CODE), FIRMNR: _firmNr, "BMS-NCR");

                    if (!string.Equals(res, "ok", StringComparison.OrdinalIgnoreCase))
                        _log.LogError("Logo post failed for {DocNo}: {Err}", h.DOCUMENT_NO, res);

                    await Task.CompletedTask;
                }
                catch (Exception ex) {
                    HELPER.LOGYAZ(ex.ToString(), null);
                }
            }

            // Satış/iade ayrımı: Projenizdeki enum değerine göre uyarlayın.
            private static bool IsReturn(BmsMarkeRGeniusIntegrationLibrary.HELPER.Data s)
            {
                // Varsayımlar: documentType == 2 iade; yoksa tutar işaretine bak.
                return s.documentType == 2 || s.total < 0;
            }

            // storeCode -> BRANCH eşlemesi (var olan mapping tablonuzu kullanır)
            private static string ResolveBranch(string storeCode)
            {
                try
                {
                    var sql = $"SELECT TOP 1 PosBranch FROM Bms_125_MarkeRGeniusIntegration_Mapping WHERE StoreCode='{storeCode}'";
                    var dt = HELPER.SqlSelectLogo(sql);
                    if (dt.Rows.Count > 0)
                        return Convert.ToString(dt.Rows[0][0]);
                }
                catch { /* fallback */ }
                return "1"; // varsayılan şube
            }
        }

        public static string InsertInvoice2(string CARI_KOD, string BRANCH, Bms_Fiche_Header _BASLIK, List<Bms_Fiche_Detail> _DETAILS, bool withCustomer, string FIRMNR)
        {
            bool isCustomerExist = false;
            try { isCustomerExist = Convert.ToBoolean(SqlSelectLogo($"SELECT COUNT(*) FROM LG_{FIRMNR}_CLCARD WHERE CODE='{_BASLIK.CUSTOMER_CODE}'").Rows[0][0]); } catch (Exception ex) { MessageBox.Show("Müşteri hatası!"); }
            if (!isCustomerExist)
                _BASLIK.CUSTOMER_CODE = _BASLIK.CUSTOMER_CODE.TrimStart('0');
         
            //TRCODE TYPE 7 PERAKENDE SATIS FATURASI
            try
            {
                //    LogField("TYPE", 7);
                //      LogField("NUMBER", "~");
                //      LogField("DATE", _BASLIK.DATE_);
                //      LogField("AUXIL_CODE", _BASLIK.POS);
                //      LogField("DOC_NUMBER", _BASLIK.DOCUMENT_NO);
                //       LogField("DOC_TRACK_NR", _BASLIK.POS);
                //       LogField("NOTES6", withCustomer ? _BASLIK.FICHE_ID : "");
                //       LogField("AUTH_CODE", "BMS");
                //       LogField("ARP_CODE", withCustomer ? _BASLIK.CUSTOMER_CODE : CARI_KOD);
                //       LogField("POST_FLAGS", 243);
                //       LogField("CURRSEL_TOTALS", 1);
                //       LogField("DEDUCTIONPART1", 2);
                //       LogField("DEDUCTIONPART2", 3);
                //       LogField("POS_TRANSFER_INFO", 1);
                //       LogField("DOC_DATE", _BASLIK.DATE_);
                //       LogField("EBOOK_DOCDATE", _BASLIK.DATE_);
                //       LogField("EBOOK_DOCTYPE", 6);
                //       LogField("EBOOK_EXPLAIN", "Z Raporu");
                //       LogField("EBOOK_NOPAY", 1);
                //       LogField("DIVISION", BRANCH);

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
                invoice.DataFields.FieldByName("AUTH_CODE").Value = "BMS-NCR";
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
                    HELPER.LOGYAZ(line.ITEMCODE, null);
                    if (string.IsNullOrEmpty(line.ITEMCODE))
                        throw new Exception("Ürün kodu bulunamadı tarih:" + _BASLIK.DATE_.ToString() + " pos:" + _BASLIK.POS.ToString());
                    transactions_lines.AppendLine();
                    double VatRate = 0;
                    try { VatRate = double.Parse(HELPER.SqlSelectLogo($"SELECT VAT FROM LG_{FIRMNR}_ITEMS WITH(NOLOCK) WHERE CODE='" + line.ITEMCODE + "'").Rows[0][0].ToString()); } catch (Exception ex) { MessageBox.Show("VAT HATASI"); }


                    double priceFromDecmailToDouble = 0;
                    try { priceFromDecmailToDouble = Convert.ToDouble(line.PRICE.ToString().Replace(".", ",")); } catch { }

                    double linetotalFromDecmailToDouble = 0;
                    try { linetotalFromDecmailToDouble = Convert.ToDouble(line.LINETOTAL.ToString().Replace(".", ",")); } catch { }

                    //    double calculatedPrice = 1;



                    //     if (Math.Abs(line.DISCOUNT_TOTAL) > 0)
                    //     {
                    //      calculatedPrice = priceFromDecmailToDouble;
                    //     }
                    //     else if (line.QUANTITY > 0)
                    //          calculatedPrice = linetotalFromDecmailToDouble / line.QUANTITY;
                    //      else



                    transactions_lines[transactions_lines.Count - 1].FieldByName("TYPE").Value = 0;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("MASTER_CODE").Value = line.ITEMCODE;
                    transactions_lines[transactions_lines.Count - 1].FieldByName("QUANTITY").Value = line.QUANTITY == 0 ? 1 : line.QUANTITY;

                    double lqcalc = line.QUANTITY == 0 ? 1 : line.QUANTITY;

                    if (Math.Abs(line.DISCOUNT_TOTAL) > 0)
                        transactions_lines[transactions_lines.Count - 1].FieldByName("PRICE").Value = (double)line.PRICE / lqcalc;
                    else
                        transactions_lines[transactions_lines.Count - 1].FieldByName("PRICE").Value = (double)line.PRICE / lqcalc;



                    //     LogField("MASTER_CODE", line.ITEMCODE);
                    //      LogField("QUANTITY", line.QUANTITY);
                    //      LogField("PRICE", calculatedPrice);
                    //      LogField("UNIT_CODE", line.ITEMUNIT);
                    //      LogField("UNIT_CONV1", 1);
                    //      LogField("UNIT_CONV2", 1);
                    //      LogField("VAT_INCLUDED", 1);
                    //      LogField("VAT_RATE", VatRate);
                    //      LogField("BILLED", 1);
                    //      LogField("EDT_CURR", 160);
                    //      LogField("SALEMANCODE", line.SALESMAN);
                    //      LogField("MONTH", _BASLIK.DATE_.Month);
                    //      LogField("YEAR", _BASLIK.DATE_.Year);
                    //      LogField("AFFECT_RISK", 1);
                    //    LogField("BARCODE", line.ITEMCODE);


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
                        //     LogField("DISCEXP_CALC", 1);
                        //     LogField("TOTAL", discountFromDecmailToDouble);
                        //     LogField("BILLED", 1);
                        //   LogField("MONTH", _BASLIK.DATE_.Month);
                        //    LogField("YEAR", _BASLIK.DATE_.Year);
                        //    LogField("AFFECT_RISK", 1);
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

        public interface IPaymentPostingService
        {
            Task PostAsync(HELPER.Data sale, CancellationToken ct);
        }

        public sealed class PaymentPostingService : IPaymentPostingService
        {
            private readonly ILogger<PaymentPostingService> _log;
            private readonly IBranchResolver _branches;
            private readonly ICustomerResolver _customers;
            private readonly IBankCashResolver _bankCash;

            public PaymentPostingService(
                ILogger<PaymentPostingService> log,
                IBranchResolver branches,
                ICustomerResolver customers,
                IBankCashResolver bankCash)
            {
                _log = log;
                _branches = branches;
                _customers = customers;
                _bankCash = bankCash;
            }

            public async Task PostAsync(HELPER.Data sale, CancellationToken ct)
            {
                try
                {
                    if (sale.payments == null || sale.payments.Count == 0) return;

                    var branch = _branches.ResolveLogoBranchCode(sale.storeCode); // your mapping table
                    var customerCode = _customers.ResolveForSale(sale);           // Invoice/Return client selection

                    foreach (var p in sale.payments)
                    {
                        ct.ThrowIfCancellationRequested();

                        var ficheType = Classify(p, sale); // "CH Kredi Karti", "Kasa Tahsilat", "Cek Girisi", returns => "... Iade" / "Kasa Odeme"
                        var bankOrKsCode = _bankCash.ResolveLogoCode(ficheType, sale, p); // Logo BANKACC_CODE or KS/Cash code

                        // POS totals are int; convert to decimal TL
                        var total = Convert.ToDecimal(p.amount) / 100m;

                        var paymentDocNo = string.IsNullOrWhiteSpace(sale.documentNo) ? sale.receiptNo : sale.documentNo;

                        var dto = new Bms_Fiche_Payment
                        {
                            LOGO_FICHE_TYPE = ficheType,          // drives debit/credit direction in InsertCHFiche lines
                            CUSTOMER_CODE = customerCode,       // maps to CL_CARD.CODE / PAYTRANS.ARP_CODE
                            LOGO_BANK_OR_KS_CODE = bankOrKsCode,  // BANKACC_CODE for card or KS code for cash
                            POS = SafeInt(sale.posCode),
                            DATE_ = sale.date,           // PAYTRANS.DATE_ / CLFICHE.DATE_
                            DOCUMENT_NO = paymentDocNo,        // BNFLINE.DOC_NUMBER / CLFICHE.DOC_NUMBER
                            PAYMENT_TOTAL = total
                        };

                        string result;
                        if (ficheType.Equals("Cek Girisi", StringComparison.OrdinalIgnoreCase))
                        {
                            result = HELPER.InsertCheque(branch, dto, "125"); // cheque in
                        }
                        else if (ficheType.StartsWith("Kasa", StringComparison.OrdinalIgnoreCase))
                        {
                            result = HELPER.InsertKsFiche(branch, dto, "125"); // cash receipt/payment
                        }
                        else
                        {
                            result = HELPER.InsertCHFiche(branch, dto, "125"); // bank / AR/AP voucher
                        }

                        if (!string.Equals(result, "ok", StringComparison.OrdinalIgnoreCase))
                            _log.LogWarning("Payment post returned: {Result} (type={Type}, doc={Doc})", result, ficheType, paymentDocNo);
                        else
                            _log.LogInformation("Payment posted: type={Type} amount={Amt} doc={Doc}", ficheType, total, paymentDocNo);
                    }

                    await Task.CompletedTask;
                }
                catch (Exception ex) {
                    HELPER.LOGYAZ(ex.ToString(), null);
                }
            }

            private static int SafeInt(string s) =>
                int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0;

            private static string Classify(HELPER.Payment p, HELPER.Data sale)
            {
                // Very pragmatic classifier; align with your mapping table semantics.
                try
                {
                    var name = (p.paymentName ?? p.name ?? p.paymentCode ?? p.code ?? string.Empty).ToLowerInvariant();

                    var isReturn = sale.total < 0 || (sale.documentTypeName ?? string.Empty).Contains("refund");

                    if (name.Contains("cek") || name.Contains("cheque")) return "Cek Girisi";

                    if (name.Contains("nakit") || name.Contains("cash"))
                        return isReturn ? "Kasa Odeme" : "Kasa Tahsilat";

                    if (name.Contains("kk") || name.Contains("kredi") || name.Contains("card") || p.creditCardDetails != null)
                        return isReturn ? "CH Kredi Karti Iade" : "CH Kredi Karti";

                    // Default to AR/AP generic voucher
                    return isReturn ? "CH Borc" : "CH Alacak";
                }
                catch (Exception ex) {
                    HELPER.LOGYAZ(ex.ToString(), null);
                    return "";
                }

            }
        }

        public static void LogField(string fieldName, object value)
        {
            string val;
            if (value == null)
                val = "NULL";
            else if (value is string s && string.IsNullOrWhiteSpace(s))
                val = "EMPTY";
            else
                val = value.ToString();

            HELPER.LOGYAZ($"{fieldName}: {val}", null);
        }

        // ---- API client ----
        public interface ISalesApiClient
        {
            Task<IReadOnlyList<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>> PullAsync(CancellationToken ct);
        }

        public sealed class SalesApiClient : ISalesApiClient
        {
            private readonly HttpClient _http;
            private readonly SalesApiOptions _opt;
            private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };

            public SalesApiClient(HttpClient http, IOptions<SalesApiOptions> opt)
            {
                _http = http;
                _opt = opt.Value;
                _http.BaseAddress = new Uri(_opt.BaseUrl);
                _http.Timeout = TimeSpan.FromSeconds(60);
                _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrWhiteSpace(_opt.BearerToken))
                    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _opt.BearerToken);
            }

            public async Task<IReadOnlyList<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>> PullAsync(CancellationToken ct)
            {
                // Basic GET. If your server supports query filters (from/to/page), append them here.
                try
                {
                    var req = new HttpRequestMessage(HttpMethod.Get, _opt.Endpoint);

                    var res = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
                    res.EnsureSuccessStatusCode();

                    var stream = await res.Content.ReadAsStreamAsync();
                    var envelope = await JsonSerializer.DeserializeAsync<ApiEnvelope<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>>(stream, JsonOpts, ct);
                    if (envelope is null || envelope.datas is null)
                        return Array.Empty<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>();

                    return envelope.datas;
                }
                catch(Exception ex)
                {
                    HELPER.LOGYAZ(ex.ToString(), null);
                    return Array.Empty<BmsMarkeRGeniusIntegrationLibrary.HELPER.Data>();
                }
            }
        }

        // ---- Polling worker with high-water mark ----
        public sealed class SalesPoller : BackgroundService
        {
            private readonly ILogger<SalesPoller> _log;
            private readonly ISalesApiClient _api;
            private readonly ISalesHandler _handler;
            private readonly SalesApiOptions _opt;

            // High-water marks (persist to DB/redis in real deployments)
            private DateTime _lastSeenUtc = DateTime.UtcNow.AddMinutes(-120);
            private long _lastPosDocumentId = 0;

            public SalesPoller(ILogger<SalesPoller> log, ISalesApiClient api, ISalesHandler handler, IOptions<SalesApiOptions> opt)
            {
                _log = log; _api = api; _handler = handler; _opt = opt.Value;
                _lastSeenUtc = DateTime.UtcNow.AddMinutes(-_opt.WindowMinutes);
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                _log.LogInformation("SalesPoller started; endpoint: {Endpoint}", _opt.Endpoint);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var batch = await _api.PullAsync(stoppingToken);

                        // Filter new items client-side (if server lacks date/page filters).
                        // Uses your Data fields: date/startDate/posDocumentId for idempotency.  // :contentReference[oaicite:4]{index=4}
                        var newOnes = batch
                            .Where(d => d.date.ToUniversalTime() >= _lastSeenUtc)
                            .OrderBy(d => d.date)
                            .ThenBy(d => d.posDocumentId)
                            .ToList();

                        foreach (var sale in newOnes)
                        {
                            await _handler.HandleAsync(sale, stoppingToken);

                            // advance high-water mark
                            var dUtc = sale.date.ToUniversalTime();
                            if (dUtc > _lastSeenUtc) _lastSeenUtc = dUtc;
                            if (sale.posDocumentId > _lastPosDocumentId) _lastPosDocumentId = sale.posDocumentId;
                        }

                        _log.LogInformation("Pulled {Count} sale(s). HighWater: {When:o} / {Id}", newOnes.Count, _lastSeenUtc, _lastPosDocumentId);
                    }
                    catch (Exception ex)
                    {
                        _log.LogError(ex, "Sales pull failed");
                    }

                    await Task.Delay(TimeSpan.FromSeconds(_opt.PollIntervalSeconds), stoppingToken);
                }
            }
        }

        // ---- Your business hook ----
        public interface ISalesHandler
        {
            Task HandleAsync(BmsMarkeRGeniusIntegrationLibrary.HELPER.Data sale, CancellationToken ct);
        }

        public sealed class SalesHandler : ISalesHandler
        {
            private readonly ILogger<SalesHandler> _log;
            private readonly IPaymentPostingService _payments;
            private readonly IDebtCloser _debtCloser;

            public SalesHandler(ILogger<SalesHandler> log, IPaymentPostingService payments, IDebtCloser debtCloser)
            { _log = log; _payments = payments; _debtCloser = debtCloser; }

            public async Task HandleAsync(HELPER.Data sale, CancellationToken ct)
            {
                try
                {
                    await _payments.PostAsync(sale, ct);

                    // 3) Try to reconcile (DebtClose) for the same branch/day
                    await _debtCloser.TryCloseAsync(sale, ct);

                    _log.LogInformation("Handled sale {Receipt} / {DocNo} total={Total} items={ItemCount}",
                        sale.receiptNo, sale.documentNo, sale.total, sale.lines?.Count ?? 0);


                }
                catch (Exception ex)
                {
                    HELPER.LOGYAZ(ex.ToString(), null);
                }
            }
           
            
        }

        public interface IDebtCloser
        {
            Task TryCloseAsync(HELPER.Data sale, CancellationToken ct);
        }

        public interface IBranchResolver
        {
            string ResolveLogoBranchCode(string storeCode);
        }

        public interface ICustomerResolver
        {
            /// Satışta müşteri varsa onu, yoksa konfigürasyondaki default cariyi döndürür.
            string ResolveForSale(HELPER.Data sale);
        }

        public interface IBankCashResolver
        {
            /// Ödeme fişi tipine göre BANKACC_CODE veya KASA (SD_CODE) değerini döndürür.
            string ResolveLogoCode(string ficheType, HELPER.Data sale, HELPER.Payment payment);
        }

        public sealed class SqlBranchResolver : IBranchResolver
        {
            public string ResolveLogoBranchCode(string storeCode)
            {
                try
                {
                    // Öncelik: LogoBranch
                    var dt = HELPER.SqlSelectLogo(
                        $@"SELECT TOP 1 LogoBranch 
                   FROM Bms_125_MarkeRGeniusIntegration_Mapping 
                   WHERE StoreCode='{storeCode}'");
                    if (dt.Rows.Count > 0) return Convert.ToString(dt.Rows[0][0], CultureInfo.InvariantCulture);

                    // Alternatif kolon: PosBranch (bazı şemalarda böyle)
                    dt = HELPER.SqlSelectLogo(
                        $@"SELECT TOP 1 PosBranch 
                   FROM Bms_125_MarkeRGeniusIntegration_Mapping 
                   WHERE StoreCode='{storeCode}'");
                    if (dt.Rows.Count > 0) return Convert.ToString(dt.Rows[0][0], CultureInfo.InvariantCulture);
                }
                catch { /* fallback */ }
                return "1"; // güvenli varsayılan
            }
        }

        public sealed class SqlCustomerResolver : ICustomerResolver
        {
            public string ResolveForSale(HELPER.Data sale)
            {
      

                try
                {
                    var code = sale?.customerData?.code;
                    if (!string.IsNullOrWhiteSpace(code))
                        return code.TrimStart('0');

                    // Aksi halde, iade/satış durumuna göre default cari
                    var table = (sale != null && (sale.total < 0 || sale.documentType == 2))
                        ? $"BMS_125_MarkeRGenius_ReturnClient"
                        : $"BMS_125_MarkeRGenius_InvoiceClient";

                    var dt = HELPER.SqlSelectLogo($@"SELECT TOP 1 NR FROM {table} ORDER BY NR");
                    if (dt.Rows.Count > 0) return Convert.ToString(dt.Rows[0][0], CultureInfo.InvariantCulture);
                }
                catch { /* fallback */ }

                // Son çare: kurum içi varsayılan
                return "120.YKF";
            }
        }



        public sealed class SqlBankCashResolver : IBankCashResolver
        {
            private readonly IBranchResolver _branches;
            public SqlBankCashResolver(IBranchResolver branches) => _branches = branches;

            public string ResolveLogoCode(string ficheType, HELPER.Data sale, HELPER.Payment payment)
            {
     
                try
                {
                    var branch = _branches.ResolveLogoBranchCode(sale?.storeCode);

                    // Mapping tablosunda tutulduğunu varsayan yol
                    var column = ficheType.StartsWith("Kasa", StringComparison.OrdinalIgnoreCase)
                        ? "KsCode"               // kasa kodu kolonu
                        : "BankAccCode";         // banka hesabı kolonu

                    var dt = HELPER.SqlSelectLogo(
                        $@"SELECT TOP 1 {column} 
                   FROM Bms_125_MarkeRGeniusIntegration_Mapping 
                   WHERE LogoBranch = {branch}");
                    if (dt.Rows.Count > 0) return Convert.ToString(dt.Rows[0][0], CultureInfo.InvariantCulture);
                }
                catch(Exception ex) {
                    HELPER.LOGYAZ(ex.ToString(), null);
                }

                // Entegre defaultlar tablosu üzerinden fall-back
                try
                {
                    var desc = ficheType.StartsWith("Kasa", StringComparison.OrdinalIgnoreCase)
                        ? "DEFAULT_KASA_CODE"
                        : "DEFAULT_BANKACC_CODE";

                    var dt = HELPER.SqlSelectLogo(
                        $@"SELECT TOP 1 Value 
                   FROM Bms_125_MarkeRGeniusIntegration_Default 
                   WHERE Description='{desc}'");
                    if (dt.Rows.Count > 0) return Convert.ToString(dt.Rows[0][0], CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    HELPER.LOGYAZ(ex.ToString(), null);
                }

                return string.Empty; // helper, boş gelirse hata mesajı yazacaktır
            }
        }

        public  class DebtCloser : IDebtCloser
        {
            private readonly ILogger<DebtCloser> _log;
            private readonly IBranchResolver _branches;

            public DebtCloser(ILogger<DebtCloser> log, IBranchResolver branches)
            { _log = log; _branches = branches; }

            public Task TryCloseAsync(HELPER.Data sale, CancellationToken ct)
            {
                try
                {
                    var branch = _branches.ResolveLogoBranchCode(sale.storeCode);

                    var start = sale.date.Date;
                    var end = sale.date.Date.AddDays(1).AddTicks(-1);

                    // Reuse your existing logic (same three-type pass inside).
                    // If G3IntegrationDebtClose is private, expose a public facade or copy its content here.
                    RunDebtCloseForRange(branch, start, end); // thin wrapper around your function

                    _log.LogInformation("DebtClose attempted for branch {Branch} between {Start} and {End}", branch, start, end);
                    return Task.CompletedTask;
                }
                catch (Exception ex)
                {
                    HELPER.LOGYAZ(ex.ToString(), null);
                    return Task.CompletedTask;
                }
            }
        }

        public static void RunDebtCloseForRange(string branch, DateTime start, DateTime end)
        {
            var d1 = start.ToString("yyyy-MM-dd HH:mm:ss");
            var d2 = end.ToString("yyyy-MM-dd HH:mm:ss");
            // G3IntegrationDebtClose da static olmalı
            G3IntegrationDebtClose(branch, d1, d2);
        }
        public static void G3IntegrationDebtClose(string branchNr, string sqlFormattedDateStart, string sqlFormattedDateEnd)
        {
            // HATA #2: Aşağıdaki satırı SİLİN: "CONFIG CFG;"
            

        
            var loginErr = HELPER.LOGO_LOGIN(CFG.LOBJECTDEFAULTUSERNAME, CFG.LOBJECTDEFAULTPASSWORD, int.Parse(CFG.FIRMNR));
            if (!string.IsNullOrEmpty(loginErr))
               throw new Exception(loginErr);

            try
            {
                var errorList = new List<Bms_Errors>();
                // HATA #1: DebtClose’ı static yapacağız ve doğrudan çağıracağız
                DebtClose(errorList, branchNr, sqlFormattedDateStart, sqlFormattedDateEnd, 1);
                DebtClose(errorList, branchNr, sqlFormattedDateStart, sqlFormattedDateEnd, 2);
                DebtClose(errorList, branchNr, sqlFormattedDateStart, sqlFormattedDateEnd, 3);
            }
            catch (Exception ex)
            {
                HELPER.LOGYAZ("HATA!", ex);
            }
            finally
            {
                HELPER.LOGO_LOGOUT();
            }

        }



        private (List<Bms_Errors> Errors, RunSummary Summary) RunSalesPaymentsDebtClose(
            string sqlFormattedDateStart,
            string sqlFormattedDateEnd,
            List<object> branches,
            bool onlyPayments,
            bool onlySalesWithCustomer,
            bool dontDebtClose,
            bool withoutControl)
        {
            var errorList = new List<Bms_Errors>();
            int postedSales = 0, postedSalesWithCustomer = 0, postedPayments = 0, debtCloseRuns = 0;

            if (!onlyPayments && !withoutControl)
            {
                try { existenceController(branches, sqlFormattedDateStart, sqlFormattedDateEnd); } catch { /* pre-check hatası süreci bloklamasın */ }
            }

       
            var sum = new RunSummary();

            var loginErr = HELPER.LOGO_LOGIN(CFG.LOBJECTDEFAULTUSERNAME, CFG.LOBJECTDEFAULTPASSWORD, int.Parse(CFG.FIRMNR));
            if (!string.IsNullOrEmpty(loginErr)) throw new Exception(loginErr);

            try
            {
                foreach (var branch in branches.Cast<string>())
                {
                    if (onlyPayments)
                    {
                        PaymentsNCR(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
                        postedPayments++; // kaba sayaç; gerçek insert’lerde artırmak istiyorsan PaymentsNCR içinde artır
                    }
                    else if (onlySalesWithCustomer)
                    {
                        Sales_WithCustomerNCR(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
                        postedSalesWithCustomer++;
                    }
                    else
                    {
                        SalesNCR(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
                        Sales_WithCustomerNCR(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
                        PaymentsNCR(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd);
                        postedSales++; postedSalesWithCustomer++; postedPayments++;
                    }

                    if (!dontDebtClose)
                    {
                        DebtClose(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd, 1);
                        DebtClose(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd, 2);
                        DebtClose(errorList, branch, sqlFormattedDateStart, sqlFormattedDateEnd, 3);
                        debtCloseRuns += 3;
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogDeep(ex);
                return (errorList, sum);
            }
            finally
            {
                HELPER.LOGO_LOGOUT();
            }

            // >>> Eski davranışa geri dön: hata varsa formu aç, yoksa başarı göster
            SplashScreenManager.CloseForm(false);
            if (errorList.Count > 0)
            {
                XtraMessageBox.Show("İşlem hatalarla tamamlandı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (errorList, sum);
            }
            else
            {
                XtraMessageBox.Show(
                    $"İşlem tamamlandı.\n" +
                    $"Satış: {postedSales}, Cari Satış: {postedSalesWithCustomer}, Tahsilat: {postedPayments}, DebtClose Adedi: {debtCloseRuns}",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return (errorList, sum);
        }
        public sealed class RunSummary
        {
            public int Sales { get; set; }
            public int SalesWithCustomer { get; set; }
            public int Payments { get; set; }
            public int DebtCloseRuns { get; set; }
        }

        public sealed class ApiDiag
        {
            public string Endpoint { get; set; }
            public string RequestBody { get; set; }
            public int StatusCode { get; set; }
            public string Reason { get; set; }
            public string RawBodySnippet { get; set; }   // İlk 2 KB
            public string EnvelopeKeys { get; set; }     // success, data/datas vb.
            public string Message { get; set; }          // success=false ise mesaj
            public int? Count { get; set; }              // çözülen kayıt adedi
            public override string ToString() =>
                $"POST {Endpoint}\nHTTP {(StatusCode)} {Reason}\n" +
                $"Message: {Message}\nKeys: {EnvelopeKeys}\nCount: {Count}\nBody: {RawBodySnippet}";
        }

        public sealed class SalesProbeRow
        {
            public DateTime Date { get; set; }
            public string Variant { get; set; }      // "UTC" or "Local"
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public string Keys { get; set; }
            public int Count { get; set; }
        }

        private async Task<List<SalesProbeRow>> ProbeSalesBetweenSep1To8Async(
            string storeCode, string posCode, int salesType, bool excludeCancelled, CancellationToken ct)
        {
            var baseUrl = "http://192.168.3.10:9996/";
            var salesEndpoint = "api/Reports/sales";

            // Reuse one token & one client
            var token = await AuthApi.GetTokenAsync(
                storeId: 0, posId: 0, cashierId: 0,
                username: "kasa",
                password: "81dc9bdb52d04dc20036dbd8313ed055",
                timeout: TimeSpan.FromSeconds(30)
            ).ConfigureAwait(false);

            var handler = new HttpClientHandler { UseProxy = false, Proxy = null };
             var http = new HttpClient(handler) { BaseAddress = new Uri(baseUrl), Timeout = TimeSpan.FromSeconds(60) };
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var rows = new List<SalesProbeRow>();

            // Pick the year from your UI start date to avoid hardcoding
            var y = de_DateStart.DateTime.Year;
            var m = 9; // September
            for (int day = 1; day <= 8; day++)
            {
                var date = new DateTime(y, m, day);

                // Try both variants: UTC midnight (Z) and Local midnight (no Z)
                foreach (var asUtc in new[] { true, false })
                {
                    string bodyDate;
                    if (asUtc)
                    {
                        var utcMidnight = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
                        bodyDate = utcMidnight.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        var localMidnight = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Local);
                        bodyDate = localMidnight.ToString("yyyy-MM-dd'T'HH:mm:ss.fff", CultureInfo.InvariantCulture); // no Z
                    }

                    var bodyObj = new
                    {
                        date = bodyDate,
                        excludeCancelledLines = excludeCancelled,
                        storeCode,
                        salesType,
                        posCode
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(bodyObj);

                     var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = null;
                    string respText = "";
                    int status = 0;
                    string reason = "", message = "", keys = "";
                    int count = 0;

                    try
                    {
                        resp = await http.PostAsync(salesEndpoint, content, ct).ConfigureAwait(false);
                        status = (int)resp.StatusCode;
                        reason = resp.ReasonPhrase ?? "";
                        respText = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                        // Parse minimally
                        var root = JToken.Parse(string.IsNullOrWhiteSpace(respText) ? "{}" : respText);

                        if (root is JObject obj)
                        {
                            message = obj["message"]?.ToString()
                                   ?? obj["errorMessage"]?.ToString()
                                   ?? obj["statusCodeMessage"]?.ToString()
                                   ?? "";

                            keys = string.Join(",", obj.Properties().Select(p => p.Name));

                            var node = obj["datas"] ?? obj["Datas"]
                                    ?? obj["data"] ?? obj["Data"]
                                    ?? obj.SelectToken("result.datas") ?? obj.SelectToken("result.data");

                            if (node is JArray arr) count = arr.Count;
                        }
                        else if (root is JArray arrRoot)
                        {
                            keys = "array";
                            count = arrRoot.Count;
                        }
                    }
                    catch (Exception ex)
                    {
                        message = "EX: " + ex.Message;
                    }

                    rows.Add(new SalesProbeRow
                    {
                        Date = date,
                        Variant = asUtc ? "UTC" : "Local",
                        StatusCode = status,
                        Message = string.IsNullOrWhiteSpace(message) ? reason : message,
                        Keys = keys,
                        Count = count
                    });
                }
            }

            // Write CSV for quick inspection
            var csvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"sales-probe-{y}-09-01_08.csv");
            using (var sw = new StreamWriter(csvPath, false, Encoding.UTF8))
            {
                sw.WriteLine("Date,Variant,StatusCode,Count,Keys,Message");
                foreach (var r in rows)
                {
                    // naive CSV escaping
                    string esc(string s) => "\"" + (s ?? "").Replace("\"", "\"\"") + "\"";
                    sw.WriteLine($"{r.Date:yyyy-MM-dd},{r.Variant},{r.StatusCode},{r.Count},{esc(r.Keys)},{esc(r.Message)}");
                }
            }

            // Open in Notepad for immediate viewing
            try { System.Diagnostics.Process.Start("notepad.exe", csvPath); } catch { /* ignore */ }

            return rows;
        }

        private async Task<List<SalesProbeRow>> ProbeSep1to8_ShowEmptiesAsync(
    string storeCode, string posCode, int salesType, bool excludeCancelled, CancellationToken ct)
        {
            // Mevcut probe fonksiyonunu kullanıyoruz:
            // var all = await ProbeSalesBetweenSep1To8Async(...);
            var all = await ProbeSalesBetweenSep1To8Async(storeCode, posCode, salesType, excludeCancelled, ct);

            var empties = all.Where(r => r.Count == 0).OrderBy(r => r.Date).ThenBy(r => r.Variant).ToList();
            var nonEmpty = all.Where(r => r.Count > 0).OrderBy(r => r.Date).ThenBy(r => r.Variant).ToList();

            // CSV: sadece boş dönenler
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sales-probe-empties.csv");
            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.WriteLine("Date,Variant,StatusCode,Count,Keys,Message");
                foreach (var r in empties)
                {
                    string esc(string s) => "\"" + (s ?? "").Replace("\"", "\"\"") + "\"";
                    sw.WriteLine($"{r.Date:yyyy-MM-dd},{r.Variant},{r.StatusCode},{r.Count},{esc(r.Keys)},{esc(r.Message)}");
                }
            }

            try { System.Diagnostics.Process.Start("notepad.exe", path); } catch { /* ignore */ }

            // Kısa özet
            var msg =
                $"Boş dönen satır sayısı: {empties.Count}\n" +
                (nonEmpty.Count == 0
                    ? "Hiç dolu sonuç yok."
                    : "Dolu günler: " + string.Join(", ",
                        nonEmpty.GroupBy(x => x.Date).Select(g => $"{g.Key:yyyy-MM-dd} [{string.Join("/", g.Select(v => v.Variant))}] → {g.Sum(x => x.Count)} kayıt")));
            XtraMessageBox.Show(this, msg, "Probe Result (Empty datas)", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return empties;
        }


        private static void LogDeep(Exception ex)
        {
            var sb = new System.Text.StringBuilder();
            Exception cur = ex;
            while (cur != null)
            {
                sb.AppendLine(cur.GetType().FullName);
                sb.AppendLine(cur.Message);
                sb.AppendLine(cur.StackTrace);
                sb.AppendLine("----");
                cur = cur.InnerException;
            }
            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "crash.log");
            HELPER.LOGYAZ(sb.ToString(), null);
            System.Windows.Forms.MessageBox.Show(sb.ToString(), "Exception", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }
        // 2) Your button (simpleButton2) -> runs everything without hassle
        private async void simpleButton2_ClickAsync2(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(PROGRESSFORM), true, true, false);
                await ProbeSep1to8_ShowEmptiesAsync(
                    storeCode: "MGZ1",
                    posCode: "9",
                    salesType: 0,
                    excludeCancelled: true,
                    ct: CancellationToken.None);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this, ex.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }
        private async void simpleButton2_ClickAsync(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(PROGRESSFORM), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("LÜTFEN BEKLEYİN.");
            SplashScreenManager.Default.SetWaitFormDescription("");

            var sqlStart = de_DateStart.DateTime.ToString("MM/dd/yyyy") + " 00:00:00";
            var sqlEnd = de_DateEnd.DateTime.ToString("MM/dd/yyyy") + " 23:59:59";
            var branches = ccbe_Branch.Properties.Items.GetCheckedValues().ToList();

            bool onlyPayments = ce_OnlyPayments.Checked;
            bool onlySalesWithCustomer = ce_OnlySalesWithCustomer.Checked;
            bool dontDebtClose = ce_DontDebtClose.Checked;
            bool withoutControl = ce_WithoutControl.Checked;

            try
            {
                var (errors, summary) = await Task.Run(() =>
                    RunSalesPaymentsDebtClose(sqlStart, sqlEnd, branches,
                                              onlyPayments, onlySalesWithCustomer, dontDebtClose, withoutControl));

                SplashScreenManager.CloseForm(false);

                if (errors.Count > 0)
                {
                    var dlg = new FRM_Errors(errors) { StartPosition = FormStartPosition.CenterParent };
                    dlg.ShowDialog(this);
                }
            
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(this, ex.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            // <<<< BURADA EK MESAJ GÖSTERME, orchestrator kendisi gösteriyor
        }



    }
}