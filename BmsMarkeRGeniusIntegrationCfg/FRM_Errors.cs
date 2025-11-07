using BmsMarkeRGeniusIntegrationLibrary;
using BmsMarkeRGeniusIntegrationLibrary.METHODS.MODELS;
using DevExpress.Utils.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BmsMarkeRGeniusIntegrationCfg
{
    public partial class FRM_Errors : Form
    {
        List<Bms_Errors> bmsErrors = new List<Bms_Errors>();
        public FRM_Errors(List<Bms_Errors> bms_Errors)
        {
            InitializeComponent();
            bmsErrors = bms_Errors;
            grc_Errors.DataSource = bmsErrors;
        }

        private void grv_Errors_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
            {
                DevExpress.XtraGrid.Menu.GridViewMenu menu = e.Menu;
                DXMenuItem item = new DXMenuItem("Excele Kaydet", new EventHandler(ExportToExcel));
                menu.Items.Add(item);
            }
        }

        private void ExportToExcel(object sender, EventArgs e)
        {
            HELPER.DxExportGridToExcel(grv_Errors, true);
        }
    }
}
