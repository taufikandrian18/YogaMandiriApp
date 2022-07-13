using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;

namespace TestCoba
{
    public partial class CustomerPage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Role"] != null)
                {
                    BindRepeator();
                    Role r = new Role();
                    r = (Role)Session["Role"];
                    if (r.IdRole == 2)
                    {
                        btnAddVsbl.Visible = false;
                        thBtnVsbl.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }
        private void BindRepeator()
        {
            try
            {
                //connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\TAUFIK\DOCUMENTS\VISUAL STUDIO 2013\PROJECTS\TESTCOBA2\TESTCOBA\APP_DATA\TEST.MDF;User ID=''; Password='';";
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                using (SqlCommand StrQuer = new SqlCommand("spGetCustomer", sqlConnection))
                {
                    StrQuer.CommandType = CommandType.StoredProcedure;
                    RepeaterListCustomer.DataSource = StrQuer.ExecuteReader();
                    RepeaterListCustomer.DataBind();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            Button btn = (Button)(sender);
            connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\TAUFIK\DOCUMENTS\VISUAL STUDIO 2013\PROJECTS\TESTCOBA2\TESTCOBA\APP_DATA\TEST.MDF;User ID=''; Password='';";
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("update Customer set CustomerStatus=@CustomerStatus WHERE IdCustomer = @IdCustomer ", sqlConnection);
                commNama.Parameters.AddWithValue("@CustomerStatus", "INACTIVE");
                commNama.Parameters.AddWithValue("@IdCustomer", btn.CommandArgument);
                commNama.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); Response.Redirect("CustomerPage.aspx"); }
        }

        protected void RepeaterListCustomer_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Role r = new Role();
            r = (Role)Session["Role"];
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (r.IdRole == 2)
                {
                    HtmlTableCell tdValue1 = (HtmlTableCell)e.Item.FindControl("tdVal1");
                    HtmlTableCell tdValue2 = (HtmlTableCell)e.Item.FindControl("tdVal2");
                    tdValue1.Visible = false;
                    tdValue2.Visible = false;
                }
            }
        }
    }
}