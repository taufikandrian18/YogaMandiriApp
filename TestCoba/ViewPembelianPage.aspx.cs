using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

namespace TestCoba
{
    public partial class ViewPembelianPage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        PembelianMst pmbObj = new PembelianMst();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] != null)
            {
                int id;
                int.TryParse(Request.QueryString["id"], out id);
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                try
                {
                    sqlConnection.Open();
                    SqlCommand commNama = new SqlCommand("SELECT * FROM POPembelianMst WHERE IdPOPembelianMst = @IdPOPembelianMst ", sqlConnection);
                    commNama.Parameters.AddWithValue("@IdPOPembelianMst", id);
                    using (SqlDataReader dr = commNama.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            pmbObj = new PembelianMst();
                            pmbObj.IdSupplier = (int)dr["IdSupplier"];
                            pmbObj.NoInvoicePembelian = dr.GetString(dr.GetOrdinal("NoInvoicePembelian"));
                            pmbObj.TglPembelian = Convert.ToDateTime(dr["TanggalPOPembelian"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //pmbObj.TglPembelian = dr.GetString(dr.GetOrdinal("TanggalPOPembelian"));
                            pmbObj.JenisTagihan = dr.GetString(dr.GetOrdinal("JenisTagihan"));
                            pmbObj.TglJatuhTmpPembelian = Convert.ToDateTime(dr["TanggalPOJatuhTmpPembelian"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //pmbObj.TglJatuhTmpPembelian = dr.GetString(dr.GetOrdinal("TanggalPOJatuhTmpPembelian"));
                            pmbObj.TotalTagihanPembelian = (decimal)dr["TotalTagihanPembelian"];
                            pmbObj.NoRekPembayaran = dr.GetString(dr.GetOrdinal("NoRekeningPembayaran"));
                            pmbObj.IdInventory = (int)dr["IdInventory"];
                            pmbObj.StatusInvoice = dr.GetString(dr.GetOrdinal("StatusInvoicePembelianMst"));
                            valNoInv.Text = pmbObj.NoInvoicePembelian;
                            lblValNoSup.Text = getSupName(pmbObj.IdSupplier);
                            lblTglInvVal.Text = pmbObj.TglPembelian;
                            lblJnisTagihanVal.Text = pmbObj.JenisTagihan;
                            lblJthTempVal.Text = pmbObj.TglJatuhTmpPembelian;
                            lblNmBankVal.Text = pmbObj.NoRekPembayaran;
                            lblInvVal.Text = getInvName(pmbObj.IdInventory);
                            valStatusMst.Text = pmbObj.StatusInvoice;
                            lblGTVal.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", pmbObj.TotalTagihanPembelian);
                            BindRepeator(id);
                        }
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
        private void BindRepeator(int id)
        {
            try
            {
                if (checkStatusInvoice(id) != "RETUR")
                {
                    table_box_bootstrapRetur.Visible = false;
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    using (SqlCommand StrQuer = new SqlCommand("SELECT * FROM POPembelianDtl a INNER JOIN Barang b on a.IdBarang = b.IdBarang WHERE IdPOPembelianMst =" + id + " ", sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        RepeaterListItm.DataSource = StrQuer.ExecuteReader();
                        RepeaterListItm.DataBind();
                    }
                }
                else
                {
                    lblNormal.Text = "Pembelian Detail";
                    lblNormal.Visible = true;
                    lblRetur.Text = "Pembelian Retur";
                    lblRetur.Visible = true;
                    table_box_bootstrapRetur.Visible = true;
                    backButton.Visible = false;
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    using (SqlCommand StrQuer = new SqlCommand("SELECT * FROM POPembelianDtl a INNER JOIN Barang b on a.IdBarang = b.IdBarang WHERE IdPOPembelianMst =" + id + " ", sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        RepeaterListItm.DataSource = StrQuer.ExecuteReader();
                        RepeaterListItm.DataBind();
                    }
                    using (SqlCommand StrQuer = new SqlCommand("SELECT * FROM POPembelianReturDtl a INNER JOIN Barang b on a.IdBarang = b.IdBarang WHERE IdPOPembelianMst =" + id + " ", sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        Repeater1.DataSource = StrQuer.ExecuteReader();
                        Repeater1.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        private string getInvName(int id)
        {
            string itmId = "";
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT * FROM Inventory WHERE IdInventory = @IdInventory", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", id);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        itmId = dr.GetString(dr.GetOrdinal("IvnStorage"));
                        return itmId;
                    }
                    else
                    {
                        return itmId;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        private string getSupName(int id)
        {
            string itmId = "";
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT * FROM Supplier WHERE IdSupplier = @IdSupplier", sqlConnection);
                commNama.Parameters.AddWithValue("@IdSupplier", id);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        itmId = dr.GetString(dr.GetOrdinal("SupplierName"));
                        return itmId;
                    }
                    else
                    {
                        return itmId;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        private string checkStatusInvoice(int id)
        {
            string statusInvoice = "NORMAL";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT StatusInvoicePembelianMst FROM POPembelianMst WHERE IdPOPembelianMst = '" + id + "'", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        statusInvoice = dr.GetString(dr.GetOrdinal("StatusInvoicePembelianMst"));
                        return statusInvoice;
                    }
                    else
                    {
                        return statusInvoice;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        private decimal getJumlahRetur(int id)
        {
            decimal totInvoice = 0;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT SUM(TotalHargaPOPembelianReturDtl) as TOTALHARGA from POPembelianReturDtl where IdPOPembelianMst = '" + id + "'", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        totInvoice = (decimal)dr["TOTALHARGA"];
                        return totInvoice;
                    }
                    else
                    {
                        return totInvoice;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
    }
}