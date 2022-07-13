using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace TestCoba
{
    public partial class index : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Role"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    DataTable dt1 = checkDatabaseHasData();
                    var test = (from item in dt1.AsEnumerable()
                                select item.Field<int>("IdInventory")).FirstOrDefault();
                    if (test != 0)
                    {
                        lblTotPmb.Text = getTotalPembelian();
                        lblTotPenj.Text = getTotalPenjualan();
                        lblTotCust.Text = getJmlCust();
                        lblTotSup.Text = getJmlSup();
                        lblTotPembKg.Text = getTotPmbKg();
                        lblTotPenjKg.Text = getTotPnjKg();
                        Role r = new Role();
                        r = (Role)Session["Role"];
                        if (r.IdRole == 4)
                        {
                            statHeader.Visible = false;
                            statSubHeader1.Visible = false;
                            statSubHeader2.Visible = false;
                            statSubHeader3.Visible = false;
                            chartHeader.Visible = false;
                            chartSubHeader1.Visible = false;
                        }
                        if (r.IdRole == 3)
                        {
                            //statHeader.Visible = false;
                            //statSubHeader1.Visible = false;
                            //statSubHeader2.Visible = false;
                            //statSubHeader3.Visible = false;
                            //chartHeader.Visible = false;
                            //chartSubHeader1.Visible = false;
                        }
                        if(r.IdRole == 5)
                        {
                            statHeader.Visible = false;
                            statSubHeader1.Visible = false;
                            statSubHeader2.Visible = false;
                            statSubHeader3.Visible = false;
                            chartHeader.Visible = false;
                            chartSubHeader1.Visible = false;
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Isi Terlebih Dahulu Seluruh Data Sebelum Melakukan Transaksi!" + "');", true);
                    }
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
        private static string FormatNumber(long n)
        {
            long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(n) - 2));
            n = n / i * i;
            if(n>=1000000000000000)
            {
                return (n / 1000000000D).ToString("Rp0.##") + "Q";
            }
            if(n>=1000000000000)
            {
                return (n / 1000000000D).ToString("Rp0.##") + "T";
            }
            if(n>=1000000000)
            {
                return (n / 1000000000D).ToString("Rp0.##") + "M";
            }
            if(n>=1000000)
            {
                return (n / 1000000D).ToString("Rp0.##") + "Jt";
            }
            if(n>=1000)
            {
                return (n / 1000D).ToString("Rp0.##") + "Rb";
            }
            //if(n<1000000)
            //{
            //    return string.Format("{0:Rp#,.#}M", n - 5000);
            //}
            //if(n<10000000)
            //{
            //    return string.Format("{0:Rp#,.#}M", n - 50000);
            //}
            //if(n<100000000)
            //{
            //    return string.Format("{0:Rp#,.#}M", n - 500000);
            //}
            //if(n<1000000000)
            //{
            //    return string.Format("{0:Rp#,.#}B", n - 5000000);
            //}
            return n.ToString("Rp#,0");
        }

        private string getTotalPembelian()
        {
            string itmCode = "0";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT COALESCE(SUM(TotalTagihanPembelian),0) as TotalPembelian FROM POPembelianMst", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        var GTotal = (decimal)dr["TotalPembelian"];
                        if (GTotal != 0)
                        {
                            //itmCode = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotal);
                            long frmt = Convert.ToInt64(GTotal);
                            itmCode = FormatNumber(frmt);
                            return itmCode;
                        }
                        else
                        {
                            return itmCode;
                        }
                    }
                    else
                    {
                        return itmCode;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        private string getTotalPenjualan()
        {
            string itmCode = "";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT COALESCE(SUM(TotalTagihanPenjualan),0) as TotalPenjualan FROM POPenjualanMst", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        var GTotal = (decimal)dr["TotalPenjualan"];
                        if (GTotal != 0)
                        {
                            //itmCode = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotal);
                            long frmt = Convert.ToInt64(GTotal);
                            itmCode = FormatNumber(frmt);
                            return itmCode;
                        }
                        else
                        {
                            return itmCode;
                        }
                    }
                    else
                    {
                        return itmCode;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        private string getJmlCust()
        {
            string itmCode = "0";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            int numRecords = 0;
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT COUNT(*) as JumlahCustomer FROM Customer", sqlConnection);
                numRecords = (int)commNama.ExecuteScalar();
                if (numRecords != 0)
                {
                    //var GTotal = (int)dr["JumlahCustomer"];
                    itmCode = Convert.ToString(numRecords);
                    return itmCode;
                }
                else
                {
                    return itmCode;
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        private string getJmlSup()
        {
            string itmCode = "";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            int numRecords = 0;
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT COUNT(*) as JumlahSupplier FROM Supplier", sqlConnection);
                numRecords = (int)commNama.ExecuteScalar();
                if (numRecords != 0)
                {
                    //var GTotal = (int)dr["JumlahSupplier"];
                    itmCode = Convert.ToString(numRecords);
                    return itmCode;
                }
                else
                {
                    return itmCode;
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        private string getTotPmbKg()
        {
            string itmCode = "0";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT COALESCE(SUM(QtyBarangPOPembelianDtl),0) as TotalBarangBeli FROM POPembelianDtl", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        var GTotal = (decimal)dr["TotalBarangBeli"];
                        if (GTotal != 0)
                        {
                            itmCode = string.Format("{0:0.00}", GTotal) + "Kg";
                            return itmCode;
                        }
                        else
                        {
                            return itmCode;
                        }
                    }
                    else
                    {
                        return itmCode;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        private string getTotPnjKg()
        {
            string itmCode = "0";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT COALESCE(SUM(QtyBarangPOPenjualanDtl),0) as TotalBarangTerjual FROM POPenjualanDtl", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        var GTotal = (decimal)dr["TotalBarangTerjual"];
                        if (GTotal != 0)
                        {
                            itmCode = string.Format("{0:0.00}", GTotal) + "Kg";
                            return itmCode;
                        }
                        else
                        {
                            return itmCode;
                        }
                    }
                    else
                    {
                        return itmCode;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }


        [WebMethod]
        public static string GetChart(string country)
        {
            Role objRole = new Role();
            string resultChart = "";
            objRole = (Role)HttpContext.Current.Session["Role"];
            if (objRole.IdRole != 4 && objRole.IdRole != 5)
            {
                string constr = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string bulan = DateTime.Now.Date.ToString().Substring(3, 2).Trim();
                    string tahun = DateTime.Now.Date.ToString().Substring(6, 4).Trim();
                    string query = string.Format("select b.NamaBarang, count(a.IdBarang) from {0} a INNER JOIN Barang b on a.IdBarang = b.IdBarang WHERE b.BarangQuantity != 0 AND b.BarangTanggalBeli IS NOT NULL AND SUBSTRING(b.BarangTanggalBeli,4,2) = '" + bulan + "' AND SUBSTRING(b.BarangTanggalBeli,7,4) = '" + tahun + "' group by b.NamaBarang", country);
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("[");
                            while (sdr.Read())
                            {
                                sb.Append("{");
                                System.Threading.Thread.Sleep(50);
                                string color = String.Format("#{0:X6}", new Random().Next(0x1000000));
                                sb.Append(string.Format("text :'{0}', value:{1}, color: '{2}'", sdr[0], sdr[1], color));
                                sb.Append("},");
                            }
                            sb = sb.Remove(sb.Length - 1, 1);
                            sb.Append("]");
                            con.Close();
                            return sb.ToString();
                        }
                    }
                }
            }
            else
            {
                return resultChart;
            }
        }
    }
}