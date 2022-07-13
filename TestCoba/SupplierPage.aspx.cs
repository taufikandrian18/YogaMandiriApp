﻿using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

namespace TestCoba
{
    public partial class SupplierPage : System.Web.UI.Page
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
                //connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\taufik\Documents\Visual Studio 2013\Projects\TestCoba2\TestCoba\App_Data\Test.mdf';Integrated Security=True";
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                using (SqlCommand StrQuer = new SqlCommand("spGetSupplier", sqlConnection))
                {
                    StrQuer.CommandType = CommandType.StoredProcedure;
                    RepeaterListSupplier.DataSource = StrQuer.ExecuteReader();
                    RepeaterListSupplier.DataBind();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            Button btn = (Button)(sender);
            //connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\taufik\Documents\Visual Studio 2013\Projects\TestCoba2\TestCoba\App_Data\Test.mdf';Integrated Security=True;MultipleActiveResultSets=True";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("update Supplier set SupplierStatus=@SupplierStatus WHERE IdSupplier = @IdSupplier ", sqlConnection);
                commNama.Parameters.AddWithValue("@SupplierStatus", "INACTIVE");
                commNama.Parameters.AddWithValue("@IdSupplier", btn.CommandArgument);
                commNama.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); Response.Redirect("SupplierPage.aspx"); }
        }

        protected void RepeaterListSupplier_ItemDataBound(object sender, RepeaterItemEventArgs e)
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