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
    public partial class ViewDetailBiaya : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        BiayaMst biayaObj = new BiayaMst();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
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
                        SqlCommand commNama = new SqlCommand("SELECT * FROM TblBiayaMst WHERE IdBiayaMst = @IdBiayaMst ", sqlConnection);
                        commNama.Parameters.AddWithValue("@IdBiayaMst", id);
                        using (SqlDataReader dr = commNama.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                biayaObj = new BiayaMst();
                                biayaObj.IdBiayaMst = id;
                                biayaObj.JenisBiayaMst = dr.GetString(dr.GetOrdinal("JenisBiayaMst"));
                                biayaObj.TanggalBiayaMst = Convert.ToDateTime(dr["TanggalBiayaMst"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                biayaObj.AkunBiayaMst = dr.GetString(dr.GetOrdinal("AkunBiayaMst"));
                                biayaObj.TotalBiayaMst = (decimal)dr["TotalBiayaMst"];
                                biayaObj.CreatedBy = dr.GetString(dr.GetOrdinal("CreatedBy"));
                                biayaObj.CreatedDate = dr.GetString(dr.GetOrdinal("CreatedDate"));
                                biayaObj.UpdatedBy = dr.GetString(dr.GetOrdinal("UpdatedBy"));
                                biayaObj.UpdatedDate = dr.GetString(dr.GetOrdinal("UpdatedDate"));
                                txtJenisBiaya.Text = biayaObj.JenisBiayaMst;
                                txtTanggalInvoice.Text = biayaObj.TanggalBiayaMst;
                                lblGTVal.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", biayaObj.TotalBiayaMst);
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
        }

        private void BindRepeator(int id)
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                using (SqlCommand StrQuer = new SqlCommand("SELECT * FROM TblBiayaDtl WHERE IdBiayaMst =" + id + " ", sqlConnection))
                {
                    //StrQuer.CommandType = CommandType.StoredProcedure;
                    rptPembiayaan.DataSource = StrQuer.ExecuteReader();
                    rptPembiayaan.DataBind();
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