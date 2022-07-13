using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;

namespace TestCoba
{
    public partial class NeracaTransaksi : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Role"] != null)
                {
                    DataTable dt1 = checkDatabaseHasData();
                    var test = (from item in dt1.AsEnumerable()
                                select item.Field<int>("IdInventory")).FirstOrDefault();
                    if (test != 0)
                    {
                        int year = DateTime.Now.Year;
                        for (int i = year - 20; i <= year + 50; i++)
                        {
                            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(i.ToString());
                            ddlYears.Items.Add(li);
                        }
                        ddlYears.Items.FindByText(year.ToString()).Selected = true;
                    }
                    else
                    {
                        Response.Redirect("Index.aspx");
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }
        private DataTable checkDatabaseHasData()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Inventory");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            BindRepeator(ddlTanggal.SelectedValue.Trim(), ddlMonths.SelectedValue.Trim(), ddlYears.SelectedValue.Trim());
        }

        private void BindRepeator(string tanggalBeli, string bulan, string tahun)
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                string queryJmlPemb = "SELECT COALESCE(SUM(CAST((c.QtyBarangPOPenjualanDtl * (c.HargaPOPenjualanDtl-f.HargaPOPembelianDtl))as decimal(18,2))),0) as TotSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN Barang d on d.IdBarang = c.IdBarang INNER JOIN Supplier e on e.IdSupplier = d.IdSupplier INNER JOIN POPembelianDtl f on f.IdBarang = d.IdBarang WHERE SUBSTRING(a.TanggalPOPenjualan,1,2) = '" + tanggalBeli + "' AND SUBSTRING(a.TanggalPOPenjualan,4,2) = '" + bulan + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + tahun + "'";
                string queryJmlQty = "SELECT COALESCE(SUM(c.QtyBarangPOPenjualanDtl),0) as TotalQTY FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN Barang d on d.IdBarang = c.IdBarang INNER JOIN Supplier e on e.IdSupplier = d.IdSupplier INNER JOIN POPembelianDtl f on f.IdBarang = d.IdBarang WHERE SUBSTRING(a.TanggalPOPenjualan,1,2) = '" + tanggalBeli + "' AND SUBSTRING(a.TanggalPOPenjualan,4,2) = '" + bulan + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + tahun + "'";
                using (SqlCommand StrQuer = new SqlCommand("SELECT a.NoInvoicePenjualan,a.TanggalPOPenjualan,e.SupplierName,d.NamaBarang,b.CustomerName,c.QtyBarangPOPenjualanDtl,c.JumlahBoxPOPenjualanDtl,c.HargaPOPenjualanDtl,f.hrg,(c.HargaPOPenjualanDtl-f.hrg) as Selisih, CAST((c.QtyBarangPOPenjualanDtl * (c.HargaPOPenjualanDtl-f.hrg))as decimal(18,2)) as TotSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN Barang d on d.IdBarang = c.IdBarang INNER JOIN Supplier e on e.IdSupplier = d.IdSupplier INNER JOIN (select ts.IdBarang, MAX(CAST(ts.PembelianDtlUpdatedDate as date)) as tgl, MAX(ts.HargaPOPembelianDtl) as hrg from POPembelianDtl ts group by IdBarang) f on f.IdBarang = d.IdBarang WHERE SUBSTRING(a.TanggalPOPenjualan,1,2) = '" + tanggalBeli + "' AND SUBSTRING(a.TanggalPOPenjualan,4,2) = '"+ bulan + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '"+ tahun + "' ORDER BY d.NamaBarang", sqlConnection))
                {
                    //StrQuer.CommandType = CommandType.StoredProcedure;
                    RepeaterListNrcTransaksi.DataSource = StrQuer.ExecuteReader();
                    RepeaterListNrcTransaksi.DataBind();
                }
                using (SqlCommand StrQuer = new SqlCommand(queryJmlPemb, sqlConnection))
                {
                    using (SqlDataReader drTotPmb = StrQuer.ExecuteReader())
                    {
                        if (drTotPmb.Read())
                        {
                            var GTotPmb = (decimal)drTotPmb["TotSelisih"];
                            GTPembelian.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPmb);
                        }
                        else
                        {
                            GTPembelian.Text = "0";
                        }
                    }
                }
                using (SqlCommand StrQuer = new SqlCommand(queryJmlQty, sqlConnection))
                {
                    using (SqlDataReader drTotQty = StrQuer.ExecuteReader())
                    {
                        if (drTotQty.Read())
                        {
                            var GTotQty = (decimal)drTotQty["TotalQTY"];
                            GTQty.Text = string.Format("{0:n}", GTotQty) + " KG";
                        }
                        else
                        {
                            GTQty.Text = "0";
                        }
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