using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestCoba
{
    public partial class HargaModalPage : System.Web.UI.Page
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
                        // do something
                        BindRepeator();
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
                string tglTemp = "";
                DateTime tglBeliStr = DateTime.Now;
                sqlConnection.Open();
                tglTemp = tglBeliStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                string queryHrgModal = "; with list_tgl_beli as ( select MAX(CONVERT(DATETIME,b.TanggalPOPembelian,103)) as tglBeli,IdBarang from POPembelianDtl a INNER JOIN POPembelianMst b on b.IdPOPembelianMst = a.IdPoPembelianMst where CONVERT(DATETIME,b.TanggalPOPembelian,103) <= CONVERT(DATETIME,'13/07/2022',103) group by IdBarang ),list_hrg_beli as ( select  HargaPOPembelianDtl,b.IdBarang,TanggalPOPembelian,ROW_NUMBER() Over(partition by b.IdBarang order by abs(DATEDIFF(ss, CONVERT(DATETIME,TanggalPOPembelian,103), b.tglBeli)) asc) as RowNum from	POPembelianDtl a inner join list_tgl_beli b on a.IdBarang = b.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst ) select a.IdPOPenjualanMst,SupplierName,TanggalPOPenjualan,f.TanggalPOPembelian,StatusInvoicePenjualan,CustomerName,NamaBarang,QtyBarangPOPenjualanDtl,HargaPOPenjualanDtl,HargaPOPembelianDtl,HargaPOPenjualanDtl-HargaPOPembelianDtl as selisih,cast(((HargaPOPenjualanDtl-HargaPOPembelianDtl) * QtyBarangPOPenjualanDtl) as decimal(10,2)) as TotalSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN list_hrg_beli f on f.IdBarang = c.IdBarang INNER JOIN Barang g on g.IdBarang = f.IdBarang INNER JOIN Supplier h on h.IdSupplier = g.IdSupplier where SUBSTRING(a.TanggalPOPenjualan,1,2) = '13' AND SUBSTRING(a.TanggalPOPenjualan,4,2) = '07' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '2022' AND StatusInvoicePenjualan != 'RETUR' AND RowNum = 1 order by TanggalPOPenjualan";
                using (SqlCommand commLbKtrQty = new SqlCommand(queryHrgModal, sqlConnection))
                {
                    RepeaterListItm.DataSource = commLbKtrQty.ExecuteReader();
                    RepeaterListItm.DataBind();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void btnCari_Click(object sender, EventArgs e)
        {
            string tglTemp = "";
            DateTime tglBeliStr = DateTime.ParseExact(txtCldBrgBeli.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            try
            {
                tglTemp = tglBeliStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                string queryHrgModal = "; with list_tgl_beli as ( select MAX(CONVERT(DATETIME,b.TanggalPOPembelian,103)) as tglBeli,IdBarang from POPembelianDtl a INNER JOIN POPembelianMst b on b.IdPOPembelianMst = a.IdPoPembelianMst where CONVERT(DATETIME,b.TanggalPOPembelian,103) <= CONVERT(DATETIME,'" + tglTemp + "',103) group by IdBarang ),list_hrg_beli as ( select  HargaPOPembelianDtl,b.IdBarang,TanggalPOPembelian,ROW_NUMBER() Over(partition by b.IdBarang order by abs(DATEDIFF(ss, CONVERT(DATETIME,TanggalPOPembelian,103), b.tglBeli)) asc) as RowNum from	POPembelianDtl a inner join list_tgl_beli b on a.IdBarang = b.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst ) select a.IdPOPenjualanMst,SupplierName,TanggalPOPenjualan,f.TanggalPOPembelian,StatusInvoicePenjualan,CustomerName,NamaBarang,QtyBarangPOPenjualanDtl,HargaPOPenjualanDtl,HargaPOPembelianDtl,HargaPOPenjualanDtl-HargaPOPembelianDtl as selisih,CAST((QtyBarangPOPenjualanDtl * (HargaPOPenjualanDtl-HargaPOPembelianDtl))as decimal(18,2)) as TotalSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN list_hrg_beli f on f.IdBarang = c.IdBarang INNER JOIN Barang g on g.IdBarang = f.IdBarang INNER JOIN Supplier h on h.IdSupplier = g.IdSupplier where SUBSTRING(a.TanggalPOPenjualan,1,2) = '" + tglTemp.Substring(0, 2) + "' AND SUBSTRING(a.TanggalPOPenjualan,4,2) = '" + tglTemp.Substring(3, 2) + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + tglTemp.Substring(6, 4) + "' AND StatusInvoicePenjualan != 'RETUR' AND RowNum = 1 order by TanggalPOPenjualan";
                using (SqlCommand commLbKtrQty = new SqlCommand(queryHrgModal, sqlConnection))
                {
                    RepeaterListItm.DataSource = commLbKtrQty.ExecuteReader();
                    RepeaterListItm.DataBind();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
    }
}