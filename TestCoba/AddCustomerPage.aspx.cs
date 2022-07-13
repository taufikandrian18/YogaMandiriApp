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
    public partial class AddCustomerPage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        Customer customerObj = new Customer();
        List<Customer> customerLst = new List<Customer>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                if (Session["Role"] != null)
                {
                    int id;
                    int.TryParse(Request.QueryString["id"], out id);
                    //connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\TAUFIK\DOCUMENTS\VISUAL STUDIO 2013\PROJECTS\TESTCOBA2\TESTCOBA\APP_DATA\TEST.MDF;User ID=''; Password='';";
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand commNama = new SqlCommand("SELECT * FROM Customer WHERE IdCustomer = @IdCustomer ", sqlConnection);
                        commNama.Parameters.AddWithValue("@IdCustomer", id);
                        using (SqlDataReader dr = commNama.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                txtCusName.Text = dr.GetString(dr.GetOrdinal("CustomerName"));
                                txtCusAddr.Text = dr.GetString(dr.GetOrdinal("CustomerAddress"));
                                txtCusPhone.Text = dr.GetString(dr.GetOrdinal("CustomerPhone"));
                                txtCusEmail.Text = dr.GetString(dr.GetOrdinal("CustomerEmail"));
                                lblStatusPage.Text = "Update Customer";
                                //txtSupName.Enabled = false;
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
            vldCusName.Text = "";
            vldCusAddr.Text = "";
            vldCusEmail.Text = "";
            vldCusPhone.Text = "";
        }

        protected void btnCusOK_Click(object sender, EventArgs e)
        {
            bool mobileValStatus = VerifyNumericValue(txtCusPhone.Text.Trim());
            bool emailidValStatus = VerifyEmailID(txtCusEmail.Text.Trim());
            //connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\TAUFIK\DOCUMENTS\VISUAL STUDIO 2013\PROJECTS\TESTCOBA2\TESTCOBA\APP_DATA\TEST.MDF;User ID=''; Password='';";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            if (txtCusName.Text.Trim() != "" && txtCusAddr.Text.Trim() != "" && txtCusEmail.Text.Trim() != "" && txtCusPhone.Text.Trim() != "" && mobileValStatus == true && emailidValStatus == true)
            {
                try
                {
                    sqlConnection.Open();
                    int id;
                    int.TryParse(Request.QueryString["id"], out id);
                    int numRecords = 0;
                    SqlCommand commNama = new SqlCommand("SELECT COUNT(*) FROM Customer WHERE CustomerName = @CustomerName", sqlConnection);
                    commNama.Parameters.AddWithValue("@CustomerName", txtCusName.Text.Trim());
                    numRecords = (int)commNama.ExecuteScalar();
                    if (id == 0)
                    {
                        if (numRecords == 0)
                        {
                            SqlCommand command = new SqlCommand("insert into Customer" + "(CustomerName,CustomerAddress,CustomerPhone,CustomerEmail,CustomerStatus,CustomerCreatedBy,CustomerCreatedDate,CustomerUpdatedBy,CustomerUpdatedDate) values " + "('" + txtCusName.Text.Trim() + "','" + txtCusAddr.Text.Trim() + "','" + txtCusPhone.Text.Trim() + "','" + txtCusEmail.Text.Trim() + "','ACTIVE','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "')", sqlConnection);
                            command.ExecuteNonQuery();
                            sqlConnection.Close();
                            sqlConnection.Dispose();
                            Response.Redirect("CustomerPage.aspx");
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Customer Name is in Database Already!" + "');", true);
                        }
                    }
                    else
                    {
                        if (lblStatusPage.Text.Trim() == "Update Customer")
                        {
                            SqlCommand strQuerEditSupplier = new SqlCommand("update Customer set CustomerName=@CustomerName,CustomerAddress=@CustomerAddress,CustomerPhone=@CustomerPhone,CustomerEmail=@CustomerEmail,CustomerUpdatedBy=@CustomerUpdatedBy,CustomerUpdatedDate=@CustomerUpdatedDate where IdCustomer=@IdCustomer", sqlConnection);
                            strQuerEditSupplier.Parameters.AddWithValue("@CustomerName", txtCusName.Text.Trim());
                            strQuerEditSupplier.Parameters.AddWithValue("@CustomerAddress", txtCusAddr.Text.Trim());
                            strQuerEditSupplier.Parameters.AddWithValue("@CustomerPhone", txtCusPhone.Text.Trim());
                            strQuerEditSupplier.Parameters.AddWithValue("@CustomerEmail", txtCusEmail.Text.Trim());
                            strQuerEditSupplier.Parameters.AddWithValue("@CustomerUpdatedBy", ((Role)Session["Role"]).EmailEmp);
                            strQuerEditSupplier.Parameters.AddWithValue("@CustomerUpdatedDate", DateTime.Now.Date.ToString());
                            strQuerEditSupplier.Parameters.AddWithValue("@IdCustomer", id);
                            strQuerEditSupplier.ExecuteNonQuery();
                            sqlConnection.Close();
                            sqlConnection.Dispose();
                            Response.Redirect("CustomerPage.aspx");
                        }
                    }
                }
                catch (Exception ex)
                { throw ex; }
                finally { sqlConnection.Close(); sqlConnection.Dispose(); }
            }
            else
            {
                if (txtCusName.Text.Trim() == "")
                {
                    vldCusName.Text = "Customer Name Field Must Be Filled!";
                    txtCusName.Text = "";
                }
                if (txtCusAddr.Text.Trim() == "")
                {
                    vldCusAddr.Text = "Address Field Must Be Filled!";
                    txtCusAddr.Text = "";
                }
                if (txtCusEmail.Text.Trim() == "")
                {
                    vldCusEmail.Text = "Email Field Must Be Filled!";
                    txtCusEmail.Text = "";
                }
                if (txtCusPhone.Text.Trim() == "")
                {
                    vldCusPhone.Text = "Phone Field Must Be Filled!";
                    txtCusPhone.Text = "";
                }
                if (txtCusPhone.Text.Trim() != "" && mobileValStatus == false)
                {
                    vldCusPhone.Text = "Phone Does Not Contain Character!";
                    txtCusPhone.Text = "";
                }
                if (txtCusEmail.Text.Trim() != "" && emailidValStatus == false)
                {
                    vldCusEmail.Text = "Email Invalid!";
                    txtCusEmail.Text = "";
                }

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
        private static bool VerifyEmailID(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
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
    }
}