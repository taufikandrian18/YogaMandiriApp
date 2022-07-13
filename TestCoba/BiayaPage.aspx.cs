using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace TestCoba
{
    public partial class BiayaPage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Role"] != null)
                {
                    int year = DateTime.Now.Year;
                    for (int i = year - 20; i <= year + 50; i++)
                    {
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(i.ToString());
                        ddlYears.Items.Add(li);
                    }
                    ddlYears.Items.FindByText(year.ToString()).Selected = true;
                    //Role r = new Role();
                    //r = (Role)Session["Role"];
                    //if (r.IdRole == 2)
                    //{
                    //    btnValVsbl.Visible = false;
                    //    thVal.ColSpan = 0;
                    //    //thVal.Visible = false;
                    //}
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            BindRepeator(ddlMonths.SelectedValue.Trim(), ddlYears.SelectedValue.Trim());
        }

        private void BindRepeator(string p1, string p2)
        {
            string queryFilParam = "SELECT * FROM TblBiayaMst WHERE SUBSTRING(TanggalBiayaMst,4,2) = '" + p1 + "' AND SUBSTRING(TanggalBiayaMst,7,4) = '" + p2 + "' ";
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                if (ddlJenisBiaya.SelectedValue.Trim() != "" && ddlSortBy.SelectedValue.Trim() != "")
                {
                    string queryParam = "AND JenisBiayaMst = '" + ddlJenisBiaya.SelectedValue.Trim() + "'";
                    string queryOrderPrm = " ORDER BY " + ddlSortBy.SelectedValue.Trim() + "";
                    using (SqlCommand StrQuer = new SqlCommand(queryFilParam + queryParam + queryOrderPrm, sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        rptListPembiayaan.DataSource = StrQuer.ExecuteReader();
                        rptListPembiayaan.DataBind();
                    }
                }
                else
                {
                    string queryParam = "";
                    string queryOrderPrm = " ORDER BY IdBiayaMst";
                    if (ddlJenisBiaya.SelectedValue.Trim() != "")
                    {
                        queryParam += " AND JenisBiayaMst = '" + ddlJenisBiaya.SelectedValue + "'";
                    }
                    if (ddlSortBy.SelectedValue.Trim() != "")
                    {
                        queryOrderPrm = " ORDER BY " + ddlSortBy.SelectedValue.Trim() + "";
                    }
                    string finalFill = queryFilParam + queryParam + queryOrderPrm;
                    using (SqlCommand StrQuer = new SqlCommand(finalFill, sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        rptListPembiayaan.DataSource = StrQuer.ExecuteReader();
                        rptListPembiayaan.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
    }
}