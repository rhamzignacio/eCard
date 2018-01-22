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
    public partial class AllMoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Button2.Visible = false;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;

            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/AllMotoRequest.rdlc");

            eCardEntities entities = new eCardEntities();

            DateTime endDate = Calendar2.SelectedDate.AddHours(24);

            ReportDataSource dataSource = new ReportDataSource("DataSet1", (from customer in entities.v_MotoRequest where customer.Date >= Calendar1.SelectedDate
                                                                           && customer.Date <= endDate select customer));
            ReportViewer1.LocalReport.DataSources.Clear();

            ReportViewer1.LocalReport.DataSources.Add(dataSource);

            Panel1.Visible = Button1.Visible = false;

            Button2.Visible = true;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Panel1.Visible = Button1.Visible = true;

            Button2.Visible = false;
        }
    }
}