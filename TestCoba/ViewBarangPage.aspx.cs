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
    public partial class ViewBarangPage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        Barang brgObj = new Barang();
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
                    SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang ", sqlConnection);
                    commNama.Parameters.AddWithValue("@IdBarang", id);
                    using (SqlDataReader dr = commNama.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            brgObj = new Barang();
                            brgObj.IdBarang = id;
                            brgObj.NamaBarang = dr.GetString(dr.GetOrdinal("NamaBarang"));
                            brgObj.BarangQty = (decimal)dr["BarangQuantity"];
                            brgObj.BarangCreatedBy = dr.GetString(dr.GetOrdinal("BarangCreatedBy"));
                            brgObj.BarangCreatedDate = dr.GetString(dr.GetOrdinal("BarangCreatedDate"));
                            brgObj.BarangUpdateBy = dr.GetString(dr.GetOrdinal("BarangUpdateBy"));
                            brgObj.BarangUpdateDate = dr.GetString(dr.GetOrdinal("BarangUpdateDate"));
                            //brgObj.BarangHargaJual = (decimal)dr["BarangHargaJual"];
                            brgObj.BarangSatuan = dr.GetString(dr.GetOrdinal("BarangSatuan"));
                            brgObj.IdSupplier = (int)dr["IdSupplier"];
                            brgObj.BarangStatus = dr.GetString(dr.GetOrdinal("BarangStatus"));
                            //brgObj.BarangTglBeli = Convert.ToDateTime(dr["BarangTanggalBeli"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            brgObj.IdInventory = (int)dr["IdInventory"];
                            brgObj.BarangJmlBox = (int)dr["BarangJmlBox"];
                            brgObj.BarangItemCode = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                            valNoInv.Text = brgObj.NamaBarang;
                            lblValNoSup.Text = getSupName(brgObj.IdSupplier);
                            lblInvVal.Text = getInvName(brgObj.IdInventory);
                            //lblGTVal.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", pmbObj.TotalTagihanPembelian);
                            valQty.Text = string.Format("{0:n}", brgObj.BarangQty) + " KG";
                            valBox.Text = string.Format("{0:n}", brgObj.BarangJmlBox) + " BOX";
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
                if (!checkDataPenjualan(id))
                {
                    table_box_bootstrapJual.Visible = false;
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    using (SqlCommand StrQuer = new SqlCommand("SELECT c.TanggalPOPembelian,c.NoInvoicePembelian,b.NamaBarang,a.QtyBarangPOPembelianDtl,a.JumlahBoxPOPembelianDtl FROM POPembelianDtl a INNER JOIN Barang b on a.IdBarang = b.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst WHERE b.IdBarang = '"+id+"'", sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        RepeaterListItm.DataSource = StrQuer.ExecuteReader();
                        RepeaterListItm.DataBind();
                    }
                }
                else
                {
                    lblNormal.Text = "Data Pembelian";
                    lblNormal.Visible = true;
                    lblPenjualan.Text = "Data Penjualan";
                    lblPenjualan.Visible = true;
                    table_box_bootstrapJual.Visible = true;
                    backButton.Visible = false;
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    using (SqlCommand StrQuer = new SqlCommand("SELECT c.TanggalPOPembelian,c.NoInvoicePembelian,b.NamaBarang,a.QtyBarangPOPembelianDtl,a.JumlahBoxPOPembelianDtl FROM POPembelianDtl a INNER JOIN Barang b on a.IdBarang = b.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst WHERE b.IdBarang = '" + id + "'", sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        RepeaterListItm.DataSource = StrQuer.ExecuteReader();
                        RepeaterListItm.DataBind();
                    }
                    using (SqlCommand StrQuer = new SqlCommand("SELECT c.TanggalPOPenjualan,c.NoInvoicePenjualan,b.NamaBarang,a.QtyBarangPOPenjualanDtl,a.JumlahBoxPOPenjualanDtl FROM POPenjualanDtl a INNER JOIN Barang b on a.IdBarang = b.IdBarang INNER JOIN POPenjualanMst c on c.IdPOPenjualanMst = a.IdPOPenjualanMst WHERE b.IdBarang = '"+id+"'", sqlConnection))
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
        private bool checkDataPenjualan(int id)
        {
            bool isJual = false;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                int numRecords = 0;
                SqlCommand commNama = new SqlCommand("SELECT COUNT(*) from POPenjualanDtl where IdBarang = @IdBarang", sqlConnection);
                commNama.Parameters.AddWithValue("@IdBarang", id);
                numRecords = (int)commNama.ExecuteScalar();
                if (id != 0)
                {
                    if (numRecords == 0)
                    {
                        return isJual;
                    }
                    else
                    {
                        isJual = true;
                        return isJual;
                    }
                }
                else
                {
                    return isJual;
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
    }
}