using System;
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
    public partial class InventoryPage : System.Web.UI.Page
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
                    if (r.IdRole == 4 || r.IdRole == 2)
                    {
                        btnAddInvVsbl.Visible = false;
                        thUpdateVisbl.Visible = false;
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
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                using (SqlCommand StrQuer = new SqlCommand("spGetInventory", sqlConnection))
                {
                    StrQuer.CommandType = CommandType.StoredProcedure;
                    RepeaterListInventory.DataSource = StrQuer.ExecuteReader();
                    RepeaterListInventory.DataBind();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            Button btn = (Button)(sender);
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();
                if (checkDeleteStorage(btn.CommandArgument, sqlConnection))
                {
                    SqlCommand commNama = new SqlCommand("update Inventory set InvStatus=@InvStatus WHERE IdInventory = @IdInventory ", sqlConnection);
                    commNama.Parameters.AddWithValue("@InvStatus", "INACTIVE");
                    commNama.Parameters.AddWithValue("@IdInventory", btn.CommandArgument);
                    commNama.ExecuteNonQuery();
                    sqlConnection.Close();
                    sqlConnection.Dispose();
                    Response.Redirect("InventoryPage.aspx");
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Gudang tidak dapat di delete karena masih di gunakan!" + "');", true);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        private bool checkDeleteStorage(string idInv, SqlConnection sqlConnection)
        {
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT * FROM Inventory WHERE IdInventory = @IdInventory ", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", idInv);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if(dr.Read())
                    {
                        var capacityStrg = (int)dr["IvnCapacity"];
                        var capacitySisaStrg = (int)dr["IvnSisaCapacity"];
                        if (capacityStrg == capacitySisaStrg)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected void RepeaterListInventory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Role r = new Role();
            r = (Role)Session["Role"];
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (r.IdRole == 4 || r.IdRole == 2)
                {
                    HtmlTableCell tdValue2 = (HtmlTableCell)e.Item.FindControl("btnUpdateVisbl");
                    tdValue2.Visible = false;
                }
            }
        }
    }
}