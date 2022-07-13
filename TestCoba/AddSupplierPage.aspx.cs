using System;
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
    public partial class AddSupplierPage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        Supplier supplierObj = new Supplier();
        List<Supplier> supplierLst = new List<Supplier>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                if (Session["Role"] != null)
                {
                    int id;
                    int.TryParse(Request.QueryString["id"], out id);
                    //connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\taufik\Documents\Visual Studio 2013\Projects\TestCoba2\TestCoba\App_Data\Test.mdf';Integrated Security=True";
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand commNama = new SqlCommand("SELECT * FROM Supplier WHERE IdSupplier = @IdSupplier ", sqlConnection);
                        commNama.Parameters.AddWithValue("@IdSupplier", id);
                        using(SqlDataReader dr = commNama.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                txtSupName.Text = dr.GetString(dr.GetOrdinal("SupplierName"));
                                txtAddr.Text = dr.GetString(dr.GetOrdinal("SupplierAddress"));
                                txtPhone.Text = dr.GetString(dr.GetOrdinal("SupplierPhone"));
                                txtEmail.Text = dr.GetString(dr.GetOrdinal("SupplierEmail"));
                                lblStatusPage.Text = "Update Supplier";
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
            vldSupName.Text = "";
            vldAddr.Text = "";
            vldEmail.Text = "";
            vldPhone.Text = "";
        }

        protected void btnSupOK_Click(object sender, EventArgs e)
        {
            bool mobileValStatus = VerifyNumericValue(txtPhone.Text.Trim());
            bool emailidValStatus = VerifyEmailID(txtEmail.Text.Trim());
            //connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\taufik\Documents\Visual Studio 2013\Projects\TestCoba2\TestCoba\App_Data\Test.mdf';Integrated Security=True;MultipleActiveResultSets=True";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            if(txtSupName.Text.Trim() != "" && txtAddr.Text.Trim() != "" && txtEmail.Text.Trim() != "" && txtPhone.Text.Trim() != "" && mobileValStatus == true && emailidValStatus == true)
            {
                try
                {
                    sqlConnection.Open();
                    int id;
                    int.TryParse(Request.QueryString["id"], out id);
                    int numRecords = 0;
                    SqlCommand commNama = new SqlCommand("SELECT COUNT(*) FROM Supplier WHERE SupplierName = @SupplierName", sqlConnection);
                    commNama.Parameters.AddWithValue("@SupplierName", txtSupName.Text.Trim());
                    numRecords = (int)commNama.ExecuteScalar();
                    if (id == 0)
                    {
                        if (numRecords == 0)
                        {
                            SqlCommand command = new SqlCommand("insert into Supplier" + "(SupplierName,SupplierAddress,SupplierPhone,SupplierEmail,SupplierStatus,SupplierCreatedBy,SupplierCreatedDate,SupplierUpdateBy,SupplierUpdateDate) values " + "('" + txtSupName.Text.ToUpper().Trim() + "','" + txtAddr.Text.Trim() + "','" + txtPhone.Text.Trim() + "','" + txtEmail.Text.Trim() + "','ACTIVE','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "')", sqlConnection);
                            command.ExecuteNonQuery();
                            sqlConnection.Close();
                            sqlConnection.Dispose();
                            Response.Redirect("SupplierPage.aspx");
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Supplier Name is in Database Already!" + "');", true);
                        }
                    }
                    else
                    {
                        if (lblStatusPage.Text.Trim() == "Update Supplier")
                        {
                            SqlCommand strQuerEditSupplier = new SqlCommand("update Supplier set SupplierName=@SupplierName,SupplierAddress=@SupplierAddress,SupplierPhone=@SupplierPhone,SupplierEmail=@SupplierEmail,SupplierUpdateBy=@SupplierUpdateBy,SupplierUpdateDate=@SupplierUpdateDate where IdSupplier=@IdSupplier", sqlConnection);
                            strQuerEditSupplier.Parameters.AddWithValue("@SupplierName", txtSupName.Text.ToUpper().Trim());
                            strQuerEditSupplier.Parameters.AddWithValue("@SupplierAddress", txtAddr.Text.Trim());
                            strQuerEditSupplier.Parameters.AddWithValue("@SupplierPhone", txtPhone.Text.Trim());
                            strQuerEditSupplier.Parameters.AddWithValue("@SupplierEmail", txtEmail.Text.Trim());
                            strQuerEditSupplier.Parameters.AddWithValue("@SupplierUpdateBy", ((Role)Session["Role"]).EmailEmp);
                            strQuerEditSupplier.Parameters.AddWithValue("@SupplierUpdateDate", DateTime.Now.Date.ToString());
                            strQuerEditSupplier.Parameters.AddWithValue("@IdSupplier", id);
                            strQuerEditSupplier.ExecuteNonQuery();
                            sqlConnection.Close();
                            sqlConnection.Dispose();
                            Response.Redirect("SupplierPage.aspx");
                        }
                    }
                }
                catch (Exception ex)
                { throw ex; }
                finally { sqlConnection.Close(); sqlConnection.Dispose(); }
            }
            else
            {
                if(txtSupName.Text.Trim() == "")
                {
                    vldSupName.Text = "Supplier Name Field Must Be Filled!";
                    txtSupName.Text = "";
                }
                if(txtAddr.Text.Trim() == "")
                {
                    vldAddr.Text = "Address Field Must Be Filled!";
                    txtAddr.Text = "";
                }
                if(txtEmail.Text.Trim() == "")
                {
                    vldEmail.Text = "Email Field Must Be Filled!";
                    txtEmail.Text = "";
                }
                if(txtPhone.Text.Trim() == "")
                {
                    vldPhone.Text = "Phone Field Must Be Filled!";
                    txtPhone.Text = "";
                }
                if (txtPhone.Text.Trim() != "" && mobileValStatus == false)
                {
                    vldPhone.Text = "Phone Does Not Contain Character!";
                    txtPhone.Text = "";
                }
                if (txtEmail.Text.Trim() != "" && emailidValStatus == false)
                {
                    vldEmail.Text = "Email Invalid!";
                    txtEmail.Text = "";
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
            if(Regex.IsMatch(ValueToCheck, expression))
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