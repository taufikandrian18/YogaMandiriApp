using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DayPilot;
using DayPilot.Utils;
using DayPilot.Web;
using DayPilot.Json;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

namespace TestCoba
{
    public partial class SchedulePage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Role"] != null)
                {
                    DayPilotCalendar1.StartDate = DateTime.Now.Date;
                    MyDataTable();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void MyDataTable()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT * FROM EventDP", sqlConnection);
                SqlDataAdapter brgAdapter = new SqlDataAdapter();
                brgAdapter.SelectCommand = commNama;
                DataTable dt = new DataTable();
                brgAdapter.Fill(dt);

                DayPilotCalendar1.DataSource = dt;
                DayPilotCalendar1.DataBind();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
    }
}