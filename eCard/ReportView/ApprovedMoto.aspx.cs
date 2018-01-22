using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using eCard.Models;

namespace eCard.ReportView
{
    public partial class ApprovedMoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnNewDates.Visible = false;
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            Panel1.Visible = btnGo.Visible = false;

            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/ApprovedMoto.rdlc");

            eCardEntities entities = new eCardEntities();

            DateTime endDate = EndDate.SelectedDate.AddHours(24);

            ReportDataSource dataSource = new ReportDataSource("DataSet1", (from moto in entities.vw_Approved
                                                                            where moto.Date >= StartDate.SelectedDate
                                                                            && moto.Date <= endDate
                                                                            select moto));

            ReportViewer1.LocalReport.DataSources.Clear();

            ReportViewer1.LocalReport.DataSources.Add(dataSource);
        }

        protected void btnNewDates_Click(object sender, EventArgs e)
        {
            Panel1.Visible = btnGo.Visible = true;

            btnNewDates.Visible = false;
        }
    }
}