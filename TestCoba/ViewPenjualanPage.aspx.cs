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
    public partial class ViewPenjualanPage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        PenjualanMst pnjMst = new PenjualanMst();
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
                    SqlCommand commNama = new SqlCommand("SELECT * FROM POPenjualanMst WHERE IdPOPenjualanMst = @IdPOPenjualanMst ", sqlConnection);
                    commNama.Parameters.AddWithValue("@IdPOPenjualanMst", id);
                    using (SqlDataReader dr = commNama.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            pnjMst = new PenjualanMst();
                            pnjMst.IdCustomer = (int)dr["IdCustomer"];
                            pnjMst.NoInvoicePenjualan = dr.GetString(dr.GetOrdinal("NoInvoicePenjualan"));
                            pnjMst.TanggalPOPenjualan = Convert.ToDateTime(dr["TanggalPOPenjualan"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            pnjMst.JenisTagihan = dr.GetString(dr.GetOrdinal("JenisTagihan"));
                            pnjMst.TanggalPOJatuhTmpPenjualan = Convert.ToDateTime(dr["TanggalPOJatuhTmpPenjualan"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            pnjMst.TotalTagihanPenjualan = (decimal)dr["TotalTagihanPenjualan"];
                            pnjMst.NoRekeningPembayaran = dr.GetString(dr.GetOrdinal("NoRekeningPembayaran"));
                            pnjMst.StatusInvoicePenjualan = dr.GetString(dr.GetOrdinal("StatusInvoicePenjualan"));
                            pnjMst.IdInventory = (int)dr["IdInventory"];
                            valNoInv.Text = pnjMst.NoInvoicePenjualan;
                            lblValNoSup.Text = getCustName(pnjMst.IdCustomer);
                            lblTglInvVal.Text = pnjMst.TanggalPOPenjualan;
                            lblJnisTagihanVal.Text = pnjMst.JenisTagihan;
                            lblJthTempVal.Text = pnjMst.TanggalPOJatuhTmpPenjualan;
                            lblNmBankVal.Text = pnjMst.NoRekeningPembayaran;
                            lblInvVal.Text = getInvName(pnjMst.IdInventory);
                            valStatusMst.Text = pnjMst.StatusInvoicePenjualan;
                            lblGTVal.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", pnjMst.TotalTagihanPenjualan);
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
                    using (SqlCommand StrQuer = new SqlCommand("SELECT * FROM POPenjualanDtl a INNER JOIN Barang b on a.IdBarang = b.IdBarang WHERE IdPOPenjualanMst =" + id + " ", sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        RepeaterListItm.DataSource = StrQuer.ExecuteReader();
                        RepeaterListItm.DataBind();
                    }
                }
                else
                {
                    lblNormal.Text = "Penjualan Detail";
                    lblNormal.Visible = true;
                    lblRetur.Text = "Penjualan Retur";
                    lblRetur.Visible = true;
                    table_box_bootstrapRetur.Visible = true;
                    backButton.Visible = false;
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    using (SqlCommand StrQuer = new SqlCommand("SELECT * FROM POPenjualanDtl a INNER JOIN Barang b on a.IdBarang = b.IdBarang WHERE IdPOPenjualanMst =" + id + " ", sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        RepeaterListItm.DataSource = StrQuer.ExecuteReader();
                        RepeaterListItm.DataBind();
                    }
                    using (SqlCommand StrQuer = new SqlCommand("SELECT * FROM POPenjualanReturDtl a INNER JOIN Barang b on a.IdBarang = b.IdBarang WHERE IdPOPenjualanMst =" + id + " ", sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        Repeater1.DataSource = StrQuer.ExecuteReader();
                        Repeater1.DataBind();
                        Label2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", getJumlahRetur(id));
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        private string getCustName(int id)
        {
            string itmId = "";
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT * FROM Customer WHERE IdCustomer = @IdCustomer", sqlConnection);
                commNama.Parameters.AddWithValue("@IdCustomer", id);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        itmId = dr.GetString(dr.GetOrdinal("CustomerName"));
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

        private string checkStatusInvoice(int id)
        {
            string statusInvoice = "NORMAL";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT StatusInvoicePenjualan FROM POPenjualanMst WHERE IdPOPenjualanMst = '"+id+"'", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        statusInvoice = dr.GetString(dr.GetOrdinal("StatusInvoicePenjualan"));
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
                SqlCommand commNama = new SqlCommand("SELECT SUM(TotalHargaPOPenjualanReturDtl) as TOTALHARGA from POPenjualanReturDtl where IdPOPenjualanMst = '" + id + "'", sqlConnection);
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