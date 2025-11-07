using BmsMarkeRGeniusIntegrationCfg.Genius2Logo.Control;
using BmsMarkeRGeniusIntegrationCfg.Genius2Logo.Definition;
using BmsMarkeRGeniusIntegrationCfg.Logo2Genius.Definition;
using BmsMarkeRGeniusIntegrationCfg.Logo2Genius.Transaction;
using BmsMarkeRGeniusIntegrationLibrary;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using Integration.BmsMarkeRGeniusIntegrationCfg;
using Integration.BmsMarkeRGeniusIntegrationCfg.Genius2Logo.Integration;
using System;
using System.Windows.Forms;

namespace BmsMarkeRGeniusIntegrationCfg
{
    public partial class FRM_MAIN : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        CONFIG CFG;
        bool isAdmin;
        public FRM_MAIN(bool _isAdmin)
        {
            InitializeComponent();
            CFG = CONFIG_HELPER.GET_CONFIG();
            isAdmin = _isAdmin;
            if (!isAdmin)
                ace_Admin.Visible = false;
        }

        private void FRM_MAIN_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ace_DBSettings_Click(object sender, EventArgs e)
        {

            using (FRM_DBSETTINGS dBSETTINGS = new FRM_DBSETTINGS())
            {
                if (dBSETTINGS.ShowDialog() == DialogResult.OK)
                {
                    CFG = CONFIG_HELPER.GET_CONFIG();
                }
            }

        }

        private void ace_CreateUpdateTableView_Click(object sender, EventArgs e)
        {
            string[] LG_TABLES =  {
$@"
CREATE VIEW BMS_{CFG.FIRMNR}_MarkeRGenius_Branch as 
select NR,NAME from L_CAPIDIV WHERE FIRMNR={CFG.FIRMNR}
",

$@"
CREATE VIEW BMS_{CFG.FIRMNR}_MarkeRGenius_InvoiceClient as 
select CODE AS NR, DEFINITION_ AS NAME from LG_{CFG.FIRMNR}_CLCARD",

$@"
CREATE VIEW BMS_{CFG.FIRMNR}_MarkeRGenius_ReturnClient as 
select CODE AS NR, DEFINITION_ AS NAME from LG_{CFG.FIRMNR}_CLCARD",

$@"
CREATE VIEW BMS_{CFG.FIRMNR}_MarkeRGenius_GeniusPos as 
select DISTINCT NUM AS NR from [{CFG.OTHERSERVER}].[Genius3].[GENIUS3].POS",

$@"
CREATE TABLE [dbo].[Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LogoBranch] [nvarchar](50) NULL,
	[PosBranch] [nvarchar](50) NULL,
	[Ip] [nvarchar](50) NULL,
 CONSTRAINT [PK_Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Mapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
",

@"
        CREATE FUNCTION [dbo].[BMS_FNC_MarkeRGeniusIntegration_CharacterFix]( @text NVARCHAR(100) ) RETURNS NVARCHAR(100)
        AS
        BEGIN
        DECLARE @return NVARCHAR(100)
          set @return = @text
          set @return = REPLACE(@return,'İ','I')
          set @return = REPLACE(@return,'ı','I')
          set @return = REPLACE(@return,'Ş','S')
          set @return = REPLACE(@return,'Ç','C')
          set @return = REPLACE(@return,'Ö','O')
          set @return = REPLACE(@return,'Ğ','G')
          set @return = REPLACE(@return,'Ü','U')
          set @return = REPLACE(@return,'ş','S')
          set @return = REPLACE(@return,'ç','C')
          set @return = REPLACE(@return,'ö','O')
          set @return = REPLACE(@return,'ğ','G')
          set @return = REPLACE(@return,'ü','U')
          set @return = REPLACE(@return,'™',' ')
         return (@return)
        END
",

$@"
CREATE TABLE [dbo].[Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Default](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NULL,
	[Value] [nvarchar](50) NULL,
 CONSTRAINT [PK_Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Default] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]",

$@"INSERT INTO [dbo].[Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Default] ([Description],[Value]) VALUES ('YAZARKARA FISLI CARI BOS LOGO CARISI','Z.002')
",

$@"CREATE TABLE [dbo].[Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_PaymentMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
    [Branch] [int] NULL,
	[Saleman] [nvarchar](50) NULL,
	[IntegrationCode] [nvarchar](50) NULL,
	[LogoFicheType] [nvarchar](50) NULL,
	[Currency] [nvarchar](50) NULL,
	[BankOrKsCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_PaymentMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]",

$@"CREATE VIEW Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Control_Client AS					
SELECT DISTINCT 
	KOD=CUSTOMER_CODE 
	,AD=NAME_ON_DOC
FROM 
	[{CFG.OTHERSERVER}].Genius3.GENIUS3.TRANSACTION_HEADER
WHERE 
	(SELECT TOP 1 1 FROM LG_{CFG.FIRMNR}_CLCARD C WITH(NOLOCK) WHERE C.CODE=CUSTOMER_CODE) IS NULL AND CUSTOMER_CODE<>''",

$@"CREATE VIEW Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Control_Salesman AS
SELECT * FROM (
SELECT DISTINCT 
	KOD=(SELECT DISTINCT GUT.CODE FROM [{CFG.OTHERSERVER}].[Genius3].[GENIUS3].[USERS] GUT WITH(NOLOCK)  WHERE GUT.ID=FK_USER)
	,AD=(SELECT DISTINCT GUT.NAME FROM [{CFG.OTHERSERVER}].[Genius3].[GENIUS3].[USERS] GUT WITH(NOLOCK)  WHERE GUT.ID=FK_USER)
FROM 
	[{CFG.OTHERSERVER}].Genius3.GENIUS3.TRANSACTION_HEADER
) AS TF 
WHERE 
	KOD NOT IN (SELECT S.CODE FROM LG_SLSMAN S WITH(NOLOCK) WHERE S.FIRMNR={CFG.FIRMNR})",

$@"CREATE VIEW Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Control_Items AS
SELECT DISTINCT 
	KOD=BARCODE
	,AD=(SELECT TOP 1 DESCRIPTION  FROM [{CFG.OTHERSERVER}].Genius3.GENIUS3.STOCK_CARD SK WITH(NOLOCK) WHERE SK.ID=B.FK_STOCK_CARD)
FROM 
	[{CFG.OTHERSERVER}].Genius3.GENIUS3.STOCK_BARCODE B WITH(NOLOCK) 
WHERE 
	BARCODE NOT IN (SELECT UB.BARCODE FROM LG_{CFG.FIRMNR}_UNITBARCODE UB WITH(NOLOCK))
",


$@"create view BMS_{CFG.FIRMNR}_MarkeRGenius_DebtClose_Invoice  as 
SELECT 
	PAYTRANS_INVOICE=	PT.LOGICALREF
	,BRANCH=			I.BRANCH
	,DOCODE=			I.DOCODE
	,DATE_INVOICE=		I.DATE_
	,CLIENTREF=			PT.CARDREF
	,I.SPECODE
	,PAYTRANS_TOTAL=	PT.TOTAL 
FROM 
	LG_{CFG.FIRMNR}_01_PAYTRANS PT LEFT JOIN LG_{CFG.FIRMNR}_01_INVOICE I ON I.LOGICALREF=PT.FICHEREF
WHERE
	 PT.MODULENR=4 AND PT.TRCODE IN (7) AND PT.PAID=0 AND I.CYPHCODE='BMS' AND I.POSTRANSFERINFO=1 AND I.CAPIBLOCK_CREATEDBY=1 AND PT.CARDREF NOT IN (SELECT C.LOGICALREF FROM LG_{CFG.FIRMNR}_CLCARD C WHERE C.CODE   IN ('Z.001','Z.002') ) ",

$@"create view BMS_{CFG.FIRMNR}_MarkeRGenius_DebtClose_Csroll  as 
SELECT 
	PAYTRANS_CSROLL=	PT.LOGICALREF
	,BRANCH=			I.BRANCH
	,DOCODE=			I.DOCODE
	,DATE_CSROLL=		I.DATE_
	,CLIENTREF=			PT.CARDREF
	,I.SPECODE
	,PAYTRANS_TOTAL=	PT.TOTAL 
FROM 
	LG_{CFG.FIRMNR}_01_PAYTRANS PT LEFT JOIN LG_{CFG.FIRMNR}_01_CSROLL I ON I.LOGICALREF=PT.FICHEREF
WHERE
	 PT.MODULENR=6 AND PT.TRCODE IN (1) AND PT.PAID=0 AND I.CYPHCODE='BMS' AND I.CAPIBLOCK_CREATEDBY=1 AND PT.CARDREF NOT IN (SELECT C.LOGICALREF FROM LG_{CFG.FIRMNR}_CLCARD C WHERE C.CODE   IN ('Z.001','Z.002') )",

$@"create view BMS_{CFG.FIRMNR}_MarkeRGenius_DebtClose_Kslines  as 
SELECT 
	PAYTRANS_KSLINES=	PT.LOGICALREF
	,BRANCH=			I.BRANCH
	,DOCODE=			I.DOCODE
	,DATE_KSLINES=		I.DATE_
	,CLIENTREF=			PT.CARDREF
	,I.SPECODE
	,PAYTRANS_TOTAL=	PT.TOTAL 
FROM 
	LG_{CFG.FIRMNR}_01_PAYTRANS PT LEFT JOIN LG_{CFG.FIRMNR}_01_KSLINES I ON I.LOGICALREF=PT.FICHEREF
WHERE
	 PT.MODULENR=10 AND PT.TRCODE IN (1) AND PT.PAID=0 AND I.CYPHCODE='BMS' AND I.CAPIBLOCK_CREATEDBY=1 AND PT.CARDREF NOT IN (SELECT C.LOGICALREF FROM LG_{CFG.FIRMNR}_CLCARD C WHERE C.CODE   IN ('Z.001','Z.002') )",

#region Logo2Genius

$@"CREATE TABLE [dbo].[Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_IbmKasa](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LogoValue] [nvarchar](199) NULL,
	[G3Value] [nvarchar](199) NULL,
	[Path] [nvarchar](max) NULL,
	[SqlServer] [nvarchar](199) NULL,
	[SqlUsername] [nvarchar](199) NULL,
	[SqlPassword] [nvarchar](199) NULL,
	[SqlDatabase] [nvarchar](199) NULL,
 CONSTRAINT [PK_Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_IbmKasa] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]",

$@"INSERT [dbo].[Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_IbmKasa] ([LogoValue], [G3Value], [Path], [SqlServer], [SqlUsername], [SqlPassword], [SqlDatabase]) VALUES (N'0', N'1', N'\\192.168.5.103\Inbox\000\Ready', N'192.168.5.103', N'GENIUS3', N'GENIUSOPEN', N'Genius3')
INSERT [dbo].[Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_IbmKasa] ([LogoValue], [G3Value], [Path], [SqlServer], [SqlUsername], [SqlPassword], [SqlDatabase]) VALUES (N'1', N'2', N'\\192.168.5.105\Inbox\000\Ready', N'192.168.5.105', N'GENIUS3', N'GENIUSOPEN', N'Genius3')
INSERT [dbo].[Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_IbmKasa] ([LogoValue], [G3Value], [Path], [SqlServer], [SqlUsername], [SqlPassword], [SqlDatabase]) VALUES (N'2', N'3', N'\\192.168.5.106\Inbox\000\Ready', N'192.168.5.106', N'GENIUS3', N'GENIUSOPEN', N'Genius3')",

$@"CREATE VIEW [dbo].[Bms_{CFG.FIRMNR}_MarkeRGeniusIntegration_Cariler] as
SELECT 
TARIH=CAST(CASE WHEN ISNULL(I.CAPIBLOCK_MODIFIEDDATE,I.CAPIBLOCK_CREADEDDATE) > I.CAPIBLOCK_CREADEDDATE THEN I.CAPIBLOCK_MODIFIEDDATE ELSE I.CAPIBLOCK_CREADEDDATE END  AS DATE)
	,CARDREF=		I.LOGICALREF
	,I.CODE
	--,B.BARCODE
	,EXPLANATION=	DBO.BMS_FNC_MarkeRGeniusIntegration_CharacterFix(I.DEFINITION_ )
	,TELNR=	LEFT(TELNRS1+' '+TELNRS2,20)
	,SPECODE5 AS INDIRIM
FROM 
	LG_{CFG.FIRMNR}_CLCARD I WITH(NOLOCK)  
WHERE
	ACTIVE=0 AND LOGICALREF>1",

$@"CREATE FUNCTION [dbo].[Bmsf_{CFG.FIRMNR}_MarkeRGeniusIntegration_Malzemeler] (
    @WHOUSENR int
)
RETURNS TABLE
AS
RETURN 
SELECT 
	* 
FROM (
SELECT
TARIH=cast(CASE	WHEN (SELECT TOP 1 ISNULL(PL.CHANGEDATE,PL.RECDATE) FROM LK_{CFG.FIRMNR}_PRCLIST PL WITH(NOLOCK) WHERE PL.LOGICALREF=LK_PRCLISTREF)>
			ITEMDATE
				THEN (SELECT TOP 1 ISNULL(PL.CHANGEDATE,PL.RECDATE) FROM LK_{CFG.FIRMNR}_PRCLIST PL WITH(NOLOCK) WHERE PL.LOGICALREF=LK_PRCLISTREF) ELSE
			ITEMDATE
		END as date), 
IBMGENIUSAMBAR
,CODE
,BARCODE
,EXPLANATION=dbo.BMS_FNC_MarkeRGeniusIntegration_CharacterFix(EXPLANATION)
,UNIT1
,UNIT1IBM=(CASE UNIT1 WHEN 'AD' THEN '1' WHEN 'M' THEN '100' WHEN 'KG' THEN '1000' END)
,SELLING_PRICE1=	(SELECT TOP 1 PL.BUYPRICE FROM LK_{CFG.FIRMNR}_PRCLIST PL WITH(NOLOCK) WHERE PL.LOGICALREF=LK_PRCLISTREF)
,VAT_RATE
,VAT_CODE
,SPECODE
,SPECODE2
,SPECODE3
,SPECODE4
,SPECODE5
 FROM (
SELECT 
	IBMGENIUSAMBAR=(SELECT TOP 1 KDD.OFFICECODE FROM  LK_{CFG.FIRMNR}_DIVDEFAULTS KDD WITH(NOLOCK) WHERE KDD.WHOUSENR=@WHOUSENR)
	,ITEMREF=		I.LOGICALREF
	,ITEMDATE=		CASE WHEN ISNULL(I.CAPIBLOCK_MODIFIEDDATE,CONVERT(DATETIME, 0))>I.CAPIBLOCK_CREADEDDATE THEN I.CAPIBLOCK_MODIFIEDDATE ELSE I.CAPIBLOCK_CREADEDDATE END
	,I.CODE
	,B.BARCODE
	,EXPLANATION=	LEFT(I.NAME,20)
	,UNIT1=			(SELECT TOP 1 UL.CODE FROM LG_{CFG.FIRMNR}_UNITSETL UL  WITH(NOLOCK) WHERE UL.MAINUNIT=1 AND UL.UNITSETREF=I.UNITSETREF)
	,LK_PRCLISTREF=	ISNULL((SELECT TOP 1 KF.LOGICALREF FROM 
						LK_{CFG.FIRMNR}_PRCLIST KF WITH(NOLOCK)  WHERE KF.OFFICECODE=(SELECT KDD.OFFICECODE FROM 
							LK_{CFG.FIRMNR}_DIVDEFAULTS KDD  WITH(NOLOCK) WHERE KDD.WHOUSENR=@WHOUSENR) AND KF.STREF=I.LOGICALREF AND KF.VARIANTREF=0 
								ORDER BY KF.LOGICALREF DESC),0)
	,VAT_RATE=I.SELLPRVAT
	,VAT_CODE=(SELECT TOP 1 M.KDVDEPNR FROM  LG_{CFG.FIRMNR}_MARKET M WITH(NOLOCK)  WHERE M.ITEMREF=I.LOGICALREF)
	,SPECODE
	,SPECODE2
	,/*SPECODE3*/STGRPCODE AS SPECODE3
	,SPECODE4
	,SPECODE3 SPECODE5
FROM 
	LG_{CFG.FIRMNR}_UNITBARCODE B  WITH(NOLOCK) LEFT JOIN LG_{CFG.FIRMNR}_ITEMS I WITH(NOLOCK)  ON I.LOGICALREF = B.ITEMREF 
) AS TT
) AS TF
WHERE 
	TF.SELLING_PRICE1>0 AND YEAR(TF.TARIH)>=2023",

                #endregion

            };
            HELPER.SqlCreateDbTables(LG_TABLES, false, null);
        }

        private void ace_PosEODIntegration_Click(object sender, EventArgs e)
        {
            XtraForm child = null;
            foreach (Form f in MdiChildren)
            {
                if (f is Frm_PosEOD)
                {
                    child = f as Frm_PosEOD;
                    break;
                }
            }
            if (child == null)
            {
                child = new Frm_PosEOD((sender as AccordionControlElement).Text, isAdmin);
                child.MdiParent = this;
                child.Show();
            }
            else
                child.Activate();
        }

        private void ace_PosEODCancel_Click(object sender, EventArgs e)
        {
            XtraForm child = null;
            foreach (Form f in MdiChildren)
            {
                if (f is Frm_PosEODCancel)
                {
                    child = f as Frm_PosEODCancel;
                    break;
                }
            }
            if (child == null)
            {
                child = new Frm_PosEODCancel((sender as AccordionControlElement).Text);
                child.MdiParent = this;
                child.Show();
            }
            else
                child.Activate();
        }

        private void ace_LogoPosBranchMapping_Click(object sender, EventArgs e)
        {
            XtraForm child = null;
            foreach (Form f in MdiChildren)
            {
                if (f is Frm_Mapping)
                {
                    child = f as Frm_Mapping;
                    break;
                }
            }
            if (child == null)
            {
                child = new Frm_Mapping((sender as AccordionControlElement).Text);
                child.MdiParent = this;
                child.Show();
            }
            else
                child.Activate();
        }

        private void ace_Default_Click(object sender, EventArgs e)
        {
            XtraForm child = null;
            foreach (Form f in MdiChildren)
            {
                if (f is Frm_Default)
                {
                    child = f as Frm_Default;
                    break;
                }
            }
            if (child == null)
            {
                child = new Frm_Default((sender as AccordionControlElement).Text);
                child.MdiParent = this;
                child.Show();
            }
            else
                child.Activate();
        }

        private void ace_PaymentMapping_Click(object sender, EventArgs e)
        {
            XtraForm child = null;
            foreach (Form f in MdiChildren)
            {
                if (f is Frm_PaymentMapping)
                {
                    child = f as Frm_PaymentMapping;
                    break;
                }
            }
            if (child == null)
            {
                child = new Frm_PaymentMapping((sender as AccordionControlElement).Text);
                child.MdiParent = this;
                child.Show();
            }
            else
                child.Activate();
        }

        private void ace_Control_Client_Click(object sender, EventArgs e)
        {
            XtraForm child = null;
            foreach (Form f in MdiChildren)
            {
                if (f is Frm_Control_Client)
                {
                    child = f as Frm_Control_Client;
                    break;
                }
            }
            if (child == null)
            {
                child = new Frm_Control_Client((sender as AccordionControlElement).Text);
                child.MdiParent = this;
                child.Show();
            }
            else
                child.Activate();
        }

        private void ace_Control_Items_Click(object sender, EventArgs e)
        {
            XtraForm child = null;
            foreach (Form f in MdiChildren)
            {
                if (f is Frm_Control_Item)
                {
                    child = f as Frm_Control_Item;
                    break;
                }
            }
            if (child == null)
            {
                child = new Frm_Control_Item((sender as AccordionControlElement).Text);
                child.MdiParent = this;
                child.Show();
            }
            else
                child.Activate();
        }

        private void ace_Control_Salesman_Click(object sender, EventArgs e)
        {
            XtraForm child = null;
            foreach (Form f in MdiChildren)
            {
                if (f is Frm_Control_Salesman)
                {
                    child = f as Frm_Control_Salesman;
                    break;
                }
            }
            if (child == null)
            {
                child = new Frm_Control_Salesman((sender as AccordionControlElement).Text);
                child.MdiParent = this;
                child.Show();
            }
            else
                child.Activate();
        }

        private void ace_Logo2Genius_Definition_IbmKasa_Click(object sender, EventArgs e)
        {
            XtraForm child = null;
            foreach (Form f in MdiChildren)
            {
                if (f is Frm_IbmKasa)
                {
                    child = f as Frm_IbmKasa;
                    break;
                }
            }
            if (child == null)
            {
                child = new Frm_IbmKasa((sender as AccordionControlElement).Text);
                child.MdiParent = this;
                child.Show();
            }
            else
                child.Activate();
        }

        private void ace_Logo2Genius_Integration_Material_Click(object sender, EventArgs e)
        {
            XtraForm child = null;
            foreach (Form f in MdiChildren)
            {
                if (f is Frm_Malzeme)
                {
                    child = f as Frm_Malzeme;
                    break;
                }
            }
            if (child == null)
            {
                child = new Frm_Malzeme((sender as AccordionControlElement).Text);
                child.MdiParent = this;
                child.Show();
            }
            else
                child.Activate();
        }

        private void ace_Logo2Genius_Integration_Client_Click(object sender, EventArgs e)
        {
            XtraForm child = null;
            foreach (Form f in MdiChildren)
            {
                if (f is Frm_Cari)
                {
                    child = f as Frm_Cari;
                    break;
                }
            }
            if (child == null)
            {
                child = new Frm_Cari((sender as AccordionControlElement).Text);
                child.MdiParent = this;
                child.Show();
            }
            else
                child.Activate();
        }
    }
}
