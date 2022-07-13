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
    public partial class StockOpnamePage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Role"] != null)
                {
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    try
                    {
                        string com = "Select * from Inventory WHERE InvStatus = 'ACTIVE'";
                        SqlDataAdapter adpt = new SqlDataAdapter(com, sqlConnection);
                        DataTable dt = new DataTable();
                        adpt.Fill(dt);
                        ddlInventory.DataSource = dt;
                        ddlInventory.DataBind();
                        ddlInventory1.DataSource = dt;
                        ddlInventory1.DataBind();
                        BindRepeator();
                        Role r = new Role();
                        r = (Role)Session["Role"];
                        if(r.IdRole == 2)
                        {
                            thVal1.Visible = false;
                            thVal2.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    { throw ex; }
                    finally { sqlConnection.Close(); sqlConnection.Dispose(); }
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
                using (SqlCommand StrQuer = new SqlCommand(";with table1 as (select a.IdBarang,a.NamaBarang,a.BarangQuantity,COALESCE(SUM(DISTINCT b.QtyBarangOlahan),0) as BarangOlahan,SUM(c.QtyBarangPOPenjualanDtl) as Penjualan,COALESCE(SUM(DISTINCT d.QtyBarangPOPenjualanReturDtl),0) as PenjualanRetur from Barang a LEFT JOIN BarangOlahan b on b.IdBarang = a.IdBarang LEFT JOIN POPenjualanDtl c on c.IdBarang = a.IdBarang LEFT JOIN POPenjualanReturDtl d on d.IdBarang = c.IdBarang WHERE a.BarangCategory = 'PABRIKAN' group by a.IdBarang,a.NamaBarang,a.BarangQuantity,d.QtyBarangPOPenjualanReturDtl), table2 as (select a.IdBarang,a.NamaBarang,a.BarangQuantity,SUM(DISTINCT b.QtyBarangOlahan) as BarangOlahan,SUM(c.QtyBarangPOPembelianDtl) as Pembelian,COALESCE(SUM(DISTINCT d.QtyBarangPOPembelianReturDtl),0) as PembelianRetur from Barang a LEFT JOIN BarangOlahan b on b.IdBarang = a.IdBarang LEFT JOIN POPembelianDtl c on c.IdBarang = a.IdBarang LEFT JOIN POPembelianReturDtl d on d.IdBarang = a.IdBarang WHERE a.BarangCategory = 'PABRIKAN' GROUP BY a.IdBarang,a.NamaBarang,a.BarangQuantity,d.QtyBarangPOPembelianReturDtl) SELECT a.IdBarang,a.NamaBarang,a.BarangQuantity,a.BarangOlahan,Penjualan,PenjualanRetur,Pembelian,SUM(PembelianRetur) as PembelianRetur from table1 a LEFT JOIN table2 b on a.IdBarang = b.IdBarang group by a.IdBarang,a.NamaBarang,a.BarangQuantity,a.BarangOlahan,Penjualan,PenjualanRetur,Pembelian order by a.IdBarang", sqlConnection))
                {
                    //StrQuer.CommandType = CommandType.StoredProcedure;
                    RepeaterListItm.DataSource = StrQuer.ExecuteReader();
                    RepeaterListItm.DataBind();
                }
                using (SqlCommand StrQuer = new SqlCommand(";with table1 as (select a.IdBarang,a.NamaBarang,a.BarangJmlBox,COALESCE(SUM(DISTINCT b.JmlBoxBarangOlahan),0) as BarangOlahan,SUM(c.JumlahBoxPOPenjualanDtl) as Penjualan,COALESCE(SUM(DISTINCT d.JumlahBoxPOPenjualanReturDtl),0) as PenjualanRetur from Barang a LEFT JOIN BarangOlahan b on b.IdBarang = a.IdBarang LEFT JOIN POPenjualanDtl c on c.IdBarang = a.IdBarang LEFT JOIN POPenjualanReturDtl d on d.IdBarang = c.IdBarang WHERE a.BarangCategory = 'PABRIKAN' group by a.IdBarang,a.NamaBarang,a.BarangJmlBox,d.JumlahBoxPOPenjualanReturDtl), table2 as (select a.IdBarang,a.NamaBarang,a.BarangJmlBox,SUM(DISTINCT b.JmlBoxBarangOlahan) as BarangOlahan,SUM(c.JumlahBoxPOPembelianDtl) as Pembelian,COALESCE(SUM(DISTINCT d.JumlahBoxPOPembelianReturDtl),0) as PembelianRetur from Barang a LEFT JOIN BarangOlahan b on b.IdBarang = a.IdBarang LEFT JOIN POPembelianDtl c on c.IdBarang = a.IdBarang LEFT JOIN POPembelianReturDtl d on d.IdBarang = c.IdBarang WHERE a.BarangCategory = 'PABRIKAN' GROUP BY a.IdBarang,a.NamaBarang,a.BarangJmlBox,d.JumlahBoxPOPembelianReturDtl) SELECT a.IdBarang,a.NamaBarang,a.BarangJmlBox,a.BarangOlahan,Penjualan,PenjualanRetur,Pembelian,SUM(PembelianRetur) as PembelianRetur from table1 a LEFT JOIN table2 b on a.IdBarang = b.IdBarang group by a.IdBarang,a.NamaBarang,a.BarangJmlBox,a.BarangOlahan,Penjualan,PenjualanRetur,Pembelian order by a.IdBarang", sqlConnection))
                {
                    //StrQuer.CommandType = CommandType.StoredProcedure;
                    rptBoxUpdate.DataSource = StrQuer.ExecuteReader();
                    rptBoxUpdate.DataBind();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string whereInv = "";
            if (ddlInventory.SelectedValue.Trim() != "")
            {
                whereInv = " AND a.IdInventory = "+ddlInventory.SelectedValue.Trim();
            }
            string queryFilParam = ";with table1 as (select a.IdBarang,a.NamaBarang,a.BarangQuantity,COALESCE(SUM(DISTINCT b.QtyBarangOlahan),0) as BarangOlahan,SUM(c.QtyBarangPOPenjualanDtl) as Penjualan,COALESCE(SUM(DISTINCT d.QtyBarangPOPenjualanReturDtl),0) as PenjualanRetur from Barang a LEFT JOIN BarangOlahan b on b.IdBarang = a.IdBarang LEFT JOIN POPenjualanDtl c on c.IdBarang = a.IdBarang LEFT JOIN POPenjualanReturDtl d on d.IdBarang = c.IdBarang WHERE a.BarangCategory = 'PABRIKAN'" + whereInv + " group by a.IdBarang,a.NamaBarang,a.BarangQuantity,d.QtyBarangPOPenjualanReturDtl), table2 as (select a.IdBarang,a.NamaBarang,a.BarangQuantity,SUM(DISTINCT b.QtyBarangOlahan) as BarangOlahan,SUM(c.QtyBarangPOPembelianDtl) as Pembelian,COALESCE(SUM(DISTINCT d.QtyBarangPOPembelianReturDtl),0) as PembelianRetur from Barang a LEFT JOIN BarangOlahan b on b.IdBarang = a.IdBarang LEFT JOIN POPembelianDtl c on c.IdBarang = a.IdBarang LEFT JOIN POPembelianReturDtl d on d.IdBarang = c.IdBarang WHERE a.BarangCategory = 'PABRIKAN'" + whereInv + " GROUP BY a.IdBarang,a.NamaBarang,a.BarangQuantity,d.QtyBarangPOPembelianReturDtl) SELECT a.IdBarang,a.NamaBarang,a.BarangQuantity,a.BarangOlahan,Penjualan,PenjualanRetur,Pembelian,SUM(PembelianRetur) as PembelianRetur from table1 a LEFT JOIN table2 b on a.IdBarang = b.IdBarang group by a.IdBarang,a.NamaBarang,a.BarangQuantity,a.BarangOlahan,Penjualan,PenjualanRetur,Pembelian order by a.IdBarang";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                SqlCommand bindDataFilComm = new SqlCommand(queryFilParam, sqlConnection);
                SqlDataAdapter fillAdpt = new SqlDataAdapter(bindDataFilComm);
                DataTable dtFill = new DataTable();
                fillAdpt.Fill(dtFill);
                RepeaterListItm.DataSource = dtFill;
                RepeaterListItm.DataBind();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void lnkUpdate1_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string link = btn.CommandArgument;
            Response.Redirect("UpdateStockOpname.aspx?id=" + link +"&code=BOX");
        }

        protected void btnFilter1_Click(object sender, EventArgs e)
        {
            string whereInv = "";
            if (ddlInventory1.SelectedValue.Trim() != "")
            {
                whereInv = " AND a.IdInventory = " + ddlInventory1.SelectedValue.Trim();
            }
            string queryFilParam = ";with table1 as (select a.IdBarang,a.NamaBarang,a.BarangJmlBox,COALESCE(SUM(DISTINCT b.JmlBoxBarangOlahan),0) as BarangOlahan,SUM(c.JumlahBoxPOPenjualanDtl) as Penjualan,COALESCE(SUM(DISTINCT d.JumlahBoxPOPenjualanReturDtl),0) as PenjualanRetur from Barang a LEFT JOIN BarangOlahan b on b.IdBarang = a.IdBarang LEFT JOIN POPenjualanDtl c on c.IdBarang = a.IdBarang LEFT JOIN POPenjualanReturDtl d on d.IdBarang = c.IdBarang WHERE a.BarangCategory = 'PABRIKAN'" + whereInv + " group by a.IdBarang,a.NamaBarang,a.BarangJmlBox,d.JumlahBoxPOPenjualanReturDtl), table2 as (select a.IdBarang,a.NamaBarang,a.BarangJmlBox,SUM(DISTINCT b.JmlBoxBarangOlahan) as BarangOlahan,SUM(c.JumlahBoxPOPembelianDtl) as Pembelian,COALESCE(SUM(DISTINCT d.JumlahBoxPOPembelianReturDtl),0) as PembelianRetur from Barang a LEFT JOIN BarangOlahan b on b.IdBarang = a.IdBarang LEFT JOIN POPembelianDtl c on c.IdBarang = a.IdBarang LEFT JOIN POPembelianReturDtl d on d.IdBarang = c.IdBarang WHERE a.BarangCategory = 'PABRIKAN'" + whereInv + " GROUP BY a.IdBarang,a.NamaBarang,a.BarangJmlBox,d.JumlahBoxPOPembelianReturDtl) SELECT a.IdBarang,a.NamaBarang,a.BarangJmlBox,a.BarangOlahan,Penjualan,PenjualanRetur,Pembelian,SUM(PembelianRetur) as PembelianRetur from table1 a LEFT JOIN table2 b on a.IdBarang = b.IdBarang group by a.IdBarang,a.NamaBarang,a.BarangJmlBox,a.BarangOlahan,Penjualan,PenjualanRetur,Pembelian order by a.IdBarang";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                SqlCommand bindDataFilComm = new SqlCommand(queryFilParam, sqlConnection);
                SqlDataAdapter fillAdpt = new SqlDataAdapter(bindDataFilComm);
                DataTable dtFill = new DataTable();
                fillAdpt.Fill(dtFill);
                rptBoxUpdate.DataSource = dtFill;
                rptBoxUpdate.DataBind();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string link = btn.CommandArgument;
            Response.Redirect("UpdateStockOpname.aspx?id=" + link +"&code=QTY");
        }

        protected void RepeaterListItm_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Role r = new Role();
            r = (Role)Session["Role"];
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (r.IdRole == 2)
                {
                    HtmlTableCell tdValue2 = (HtmlTableCell)e.Item.FindControl("tdVal1");
                    tdValue2.Visible = false;
                }
                else
                {
                    LinkButton btn = (LinkButton)e.Item.FindControl("lnkUpdate");
                    HtmlTableCell tdValue2 = (HtmlTableCell)e.Item.FindControl("qtyTotalPembelian");
                    if (tdValue2.InnerText.Trim() == "")
                    {
                        btn.Visible = false;
                    }
                }
            }
        }

        protected void rptBoxUpdate_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Role r = new Role();
            r = (Role)Session["Role"];
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (r.IdRole == 2)
                {
                    HtmlTableCell tdValue2 = (HtmlTableCell)e.Item.FindControl("tdVal2");
                    tdValue2.Visible = false;
                }
                else
                {
                    LinkButton btn = (LinkButton)e.Item.FindControl("lnkUpdate1");
                    HtmlTableCell tdValue2 = (HtmlTableCell)e.Item.FindControl("boxTotalPembelian");
                    if (tdValue2.InnerText.Trim() == "")
                    {
                        btn.Visible = false;
                    }
                }
            }
        }
    }
}