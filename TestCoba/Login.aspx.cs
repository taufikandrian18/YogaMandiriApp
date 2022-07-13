using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;
namespace TestCoba
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Abandon();
                //connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\taufik\Documents\Visual Studio 2013\Projects\TestCoba2\TestCoba\App_Data\Test.mdf';Integrated Security=True";
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
            }
        }

        protected void btnLogin1_Click(object sender, EventArgs e)
        {
            DataTable dt1 = checkDatabaseHasData();
            string usernameVal = txtEmail1.Text.Trim();
            string passwordVal = txtPass1.Text.Trim();
            var test = (from item in dt1.AsEnumerable()
                        select item.Field<int>("IdEmployee")).FirstOrDefault();
            if (test != 0)
            {
                try
                {
                    //connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\taufik\Documents\Visual Studio 2013\Projects\TestCoba2\TestCoba\App_Data\Test.mdf';Integrated Security=True";
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    using (SqlCommand StrQuer = new SqlCommand("SELECT * FROM Employee WHERE EmpEmail=@namauser AND EmpPassword=@password AND EmpStatus=@status", sqlConnection))
                    {
                        StrQuer.Parameters.AddWithValue("@namauser", usernameVal.Trim());
                        StrQuer.Parameters.AddWithValue("@password", passwordVal.Trim());
                        StrQuer.Parameters.AddWithValue("@status", "ACTIVE");
                        using (SqlDataReader dr = StrQuer.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                Role r = new Role();
                                r.NameEmp = dr.GetString(dr.GetOrdinal("EmpName"));
                                r.EmailEmp = usernameVal;
                                r.PassEmp = passwordVal;
                                r.StatusEmp = "ACTIVE";
                                r.IdRole = (int)dr["IdRole"];
                                Session["Role"] = r;
                                sqlConnection.Close();
                                sqlConnection.Dispose();
                                Response.Redirect("index.aspx");
                            }
                            else
                            {
                                txtEmail1.Text = "";
                                txtPass1.Text = "";
                                //lblWarning.Text = "Login Failed Check Email and Password Once Again!";
                                sqlConnection.Close();
                                sqlConnection.Dispose();
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Login Failed Check Email and Password Once Again!" + "');", true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                { throw ex; }
            }
            else
            {
                Response.Redirect("404.aspx");
            }
        }
        private DataTable checkDatabaseHasData()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Employee");
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

        protected void btnLoginGuest_Click(object sender, EventArgs e)
        {
            DataTable dt1 = checkDatabaseHasData();
            string usernameVal = "taufikandrian18@gmail.com";
            string passwordVal = "taufikandrian";
            var test = (from item in dt1.AsEnumerable()
                        select item.Field<int>("IdEmployee")).FirstOrDefault();
            if (test != 0)
            {
                try
                {
                    //connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\taufik\Documents\Visual Studio 2013\Projects\TestCoba2\TestCoba\App_Data\Test.mdf';Integrated Security=True";
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    using (SqlCommand StrQuer = new SqlCommand("SELECT * FROM Employee WHERE EmpEmail=@namauser AND EmpPassword=@password AND EmpStatus=@status", sqlConnection))
                    {
                        StrQuer.Parameters.AddWithValue("@namauser", usernameVal.Trim());
                        StrQuer.Parameters.AddWithValue("@password", passwordVal.Trim());
                        StrQuer.Parameters.AddWithValue("@status", "ACTIVE");
                        using (SqlDataReader dr = StrQuer.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                Role r = new Role();
                                r.NameEmp = dr.GetString(dr.GetOrdinal("EmpName"));
                                r.EmailEmp = usernameVal;
                                r.PassEmp = passwordVal;
                                r.StatusEmp = "ACTIVE";
                                r.IdRole = (int)dr["IdRole"];
                                Session["Role"] = r;
                                sqlConnection.Close();
                                sqlConnection.Dispose();
                                Response.Redirect("index.aspx");
                            }
                            else
                            {
                                txtEmail1.Text = "";
                                txtPass1.Text = "";
                                //lblWarning.Text = "Login Failed Check Email and Password Once Again!";
                                sqlConnection.Close();
                                sqlConnection.Dispose();
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Login Failed Check Email and Password Once Again!" + "');", true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                { throw ex; }
            }
            else
            {
                Response.Redirect("404.aspx");
            }
        }
    }
}