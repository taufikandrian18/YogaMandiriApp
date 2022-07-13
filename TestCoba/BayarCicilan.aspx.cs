using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;

namespace TestCoba
{
    public partial class BayarCicilan : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        Cicilan cicilanObj = new Cicilan();
        List<Cicilan> cicilanList = new List<Cicilan>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                if (Session["Role"] != null)
                {
                    int id;
                    int.TryParse(Request.QueryString["id"], out id);
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    try
                    {
                        string queryJmlTotalPembayaran = "SELECT COALESCE(SUM(NilaiPembayaran),0) as TOTALBAYAR FROM Cicilan WHERE IdPOPenjualanMst = '" + id + "' ";
                        string queryPOPenjualanMst = "SELECT * FROM POPenjualanMst WHERE IdPOPenjualanMst = '" + id + "' ";
                        sqlConnection.Open();
                        using (SqlCommand commNama = new SqlCommand(queryJmlTotalPembayaran, sqlConnection))
                        {
                            using (SqlDataReader dr = commNama.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    var totalBayar = (decimal)dr["TOTALBAYAR"];
                                    using (SqlCommand commJual = new SqlCommand(queryPOPenjualanMst, sqlConnection))
                                    {
                                        using (SqlDataReader drJual = commJual.ExecuteReader())
                                        {
                                            if (dr.Read())
                                            {
                                                var totalTagihan = (decimal)dr["TotalTagihanPenjualan"];
                                                txtTerbayar.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", totalBayar);
                                                txtInvoice.Text = drJual.GetString(drJual.GetOrdinal("NoInvoicePenjualan"));
                                                txtTransaksi.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", totalTagihan);
                                            }
                                        }
                                    }
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
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        private bool VerifyNumericValue(string ValueToCheck)
        {
            //int numval;
            //bool rslt = false;

            //rslt = int.TryParse(ValueToCheck, out numval);

            //return rslt;
            string expression;
            expression = "^\\+?(\\d[\\d-. ]+)?(\\([\\d-. ]+\\))?[\\d-. ]+\\d$";
            if (Regex.IsMatch(ValueToCheck, expression))
            {
                if (Regex.Replace(ValueToCheck, expression, string.Empty).Length == 0)
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
        protected void bckBtn_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
                Response.Redirect((string)refUrl);
        }

        protected void btnIvnOK_Click(object sender, EventArgs e)
        {
            bool bayarStatusCheck = VerifyNumericValue(txtJumlahBayar.Text.Trim());
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            if (txtJumlahBayar.Text.Trim() != "" && bayarStatusCheck == true)
            {
                try
                {
                    sqlConnection.Open();
                    int id;
                    int.TryParse(Request.QueryString["id"], out id);
                    if (Convert.ToDecimal(txtJumlahBayar.Text.Trim()) + Convert.ToDecimal(txtTerbayar.Text.Trim()) <= Convert.ToDecimal(txtTransaksi.Text.Trim()))
                    {
                        insertToCicilan(id, txtTerbayar.Text.Trim());
                        sqlConnection.Close();
                        sqlConnection.Dispose();
                        //check if total terbayar sudah sama dengan total tagihan
                        string queryJmlTotalPembayaran = "SELECT COALESCE(SUM(NilaiPembayaran),0) as TOTALBAYAR FROM Cicilan WHERE IdPOPenjualanMst = '" + id + "' ";
                        string queryPOPenjualanMst = "SELECT TotalTagihanPenjualan FROM POPenjualanMst WHERE IdPOPenjualanMst = '" + id + "' ";
                        using (SqlCommand commNama = new SqlCommand(queryJmlTotalPembayaran, sqlConnection))
                        {
                            using (SqlDataReader dr = commNama.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    var totalBayar = (decimal)dr["TOTALBAYAR"];
                                    using (SqlCommand commJual = new SqlCommand(queryPOPenjualanMst, sqlConnection))
                                    {
                                        using (SqlDataReader drJual = commJual.ExecuteReader())
                                        {
                                            if (dr.Read())
                                            {
                                                var totalTagihan = (decimal)dr["TotalTagihanPenjualan"];
                                                if(totalBayar == totalTagihan)
                                                {
                                                    deleteFromCicilan(id);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        Response.Redirect("PenjualanPage.aspx");
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Pembayaran Melebihi Transaksi!" + "');", true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { sqlConnection.Close(); sqlConnection.Dispose(); }
            }
            else
            {
                if (txtJumlahBayar.Text.Trim() == "")
                {
                    vldCapacity.Text = "Jumlah Bayar Harus Di isi!";
                    txtJumlahBayar.Text = "Rp0";
                }
                if (txtJumlahBayar.Text.Trim() != "" && bayarStatusCheck == false)
                {
                    vldCapacity.Text = "Harus Di isi hanya dengan angka!";
                    txtJumlahBayar.Text = "Rp0";
                }
            }
        }
        protected void deleteFromCicilan(int id)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("DELETE FROM Cicilan WHERE IdPOPenjualanMst = '" + id + "'", sqlConnection);
                commNama.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected void insertToCicilan(int id,string jumlahBayar)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                double txtPriceItmVal = Convert.ToDouble(jumlahBayar);
                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                SqlCommand commNama = new SqlCommand("insert into Cicilan" + "(IdPOPenjualanMst,NilaiPembayaran) values " + "('" + id + "'," + txtPriceItmVal + ")", sqlConnection);
                commNama.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
    }
}