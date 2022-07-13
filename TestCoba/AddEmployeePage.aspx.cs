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
    public partial class AddEmployeePage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                if (Session["Role"] != null)
                {
                    int id;
                    int.TryParse(Request.QueryString["id"], out id);
                    //connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\taufik\Documents\Visual Studio 2013\Projects\TestCoba2\TestCoba\App_Data\Test.mdf';Integrated Security=True;MultipleActiveResultSets=True";
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand commNama = new SqlCommand("SELECT * FROM Employee WHERE IdEmployee = @IdEmployee ", sqlConnection);
                        commNama.Parameters.AddWithValue("@IdEmployee", id);
                        using (SqlDataReader dr = commNama.ExecuteReader())
                        {
                            string com = "Select * from Roles WHERE StatusRole = 'ACTIVE'";
                            SqlDataAdapter adpt = new SqlDataAdapter(com, sqlConnection);
                            DataTable dt = new DataTable();
                            adpt.Fill(dt);
                            ddlRole.DataSource = dt;
                            ddlRole.DataBind();
                            ddlRole.DataTextField = "NameRole";
                            ddlRole.DataValueField = "IdRole";
                            ddlRole.DataBind();
                            if (dr.Read())
                            {
                                var role = (int)dr["IdRole"];

                                txtEmployeeNm.Text = dr.GetString(dr.GetOrdinal("EmpName"));
                                txtEmployeeUsr.Text = dr.GetString(dr.GetOrdinal("EmpUsername"));
                                txtAddr.Text = dr.GetString(dr.GetOrdinal("EmpAddress"));
                                txtPhone.Text = dr.GetString(dr.GetOrdinal("EmpPhone"));
                                txtEmail.Text = dr.GetString(dr.GetOrdinal("EmpEmail"));
                                ddlRole.SelectedValue = role.ToString();
                                lblStatusPage.Text = "Update Employee";
                                txtEmployeeUsr.Enabled = false;
                                txtEmail.Enabled = false;
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
            vldEmployeeNm.Text = "";
            vldEmployeePass.Text = "";
            vldEmployeePassCnfrm.Text = "";
            vldEmployeeUsr.Text = "";
            vldAddr.Text = "";
            vldPhone.Text = "";
            vldEmail.Text = "";
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

        protected void btnEmpOK_Click(object sender, EventArgs e)
        {
            bool mobileValStatus = VerifyNumericValue(txtPhone.Text.Trim());
            bool emailidValStatus = VerifyEmailID(txtEmail.Text.Trim());
            //connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\taufik\Documents\Visual Studio 2013\Projects\TestCoba2\TestCoba\App_Data\Test.mdf';Integrated Security=True;MultipleActiveResultSets=True";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            if (txtEmployeeNm.Text.Trim() != "" && txtEmployeeUsr.Text.Trim() != "" && txtEmployeePass.Text.Trim() != "" && txtEmployeePassCnfrm.Text.Trim() != "" && txtAddr.Text.Trim() != "" && txtEmail.Text.Trim() != "" && txtPhone.Text.Trim() != "" && mobileValStatus == true && emailidValStatus == true)
            {
                if (txtEmployeePassCnfrm.Text.Trim() == txtEmployeePass.Text.Trim())
                {
                    try
                    {
                        sqlConnection.Open();
                        int id;
                        int.TryParse(Request.QueryString["id"], out id);
                        int numRecords = 0;
                        SqlCommand commNama = new SqlCommand("SELECT COUNT(*) FROM Employee WHERE EmpEmail = @EmpEmail OR EmpUsername = @EmpUsername", sqlConnection);
                        commNama.Parameters.AddWithValue("@EmpEmail", txtEmail.Text.Trim());
                        commNama.Parameters.AddWithValue("@EmpUsername", txtEmployeeUsr.Text.Trim());
                        numRecords = (int)commNama.ExecuteScalar();
                        if (id == 0)
                        {
                            if (numRecords == 0)
                            {
                                SqlCommand command = new SqlCommand("insert into Employee" + "(EmpName,EmpUsername,EmpPassword,EmpAddress,EmpPhone,EmpEmail,EmpStatus,EmpCreateBy,EmpCreateDate,EmpUpdateBy,EmpUpdateDate,IdRole) values " + "('" + txtEmployeeNm.Text.Trim() + "','" + txtEmployeeUsr.Text.Trim() + "','" + txtEmployeePassCnfrm.Text.Trim() + "','" + txtAddr.Text.Trim() + "','" + txtPhone.Text.Trim() + "','" + txtEmail.Text.Trim() + "','ACTIVE','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ddlRole.SelectedValue.Trim() + "')", sqlConnection);
                                command.ExecuteNonQuery();
                                sqlConnection.Close(); 
                                sqlConnection.Dispose(); 
                                Response.Redirect("EmployeePage.aspx");
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Employee Email is in Database Already!" + "');", true);
                                //throw new Exception("Employee Email is in Database Already!");
                            }
                        }
                        else
                        {
                            if (lblStatusPage.Text.Trim() == "Update Employee")
                            {
                                SqlCommand strQuerEditSupplier = new SqlCommand("update Employee set EmpName=@EmpName,EmpUsername=@EmpUsername,EmpPassword=@EmpPassword,EmpAddress=@EmpAddress,EmpPhone=@EmpPhone,EmpEmail=@EmpEmail,EmpUpdateBy=@EmpUpdateBy,EmpUpdateDate=@EmpUpdateDate,IdRole=@IdRole where IdEmployee=@IdEmployee", sqlConnection);
                                strQuerEditSupplier.Parameters.AddWithValue("@EmpName", txtEmployeeNm.Text.Trim());
                                strQuerEditSupplier.Parameters.AddWithValue("@EmpUsername", txtEmployeeUsr.Text.Trim());
                                strQuerEditSupplier.Parameters.AddWithValue("@EmpPassword", txtEmployeePassCnfrm.Text.Trim());
                                strQuerEditSupplier.Parameters.AddWithValue("@EmpAddress", txtAddr.Text.Trim());
                                strQuerEditSupplier.Parameters.AddWithValue("@EmpPhone", txtPhone.Text.Trim());
                                strQuerEditSupplier.Parameters.AddWithValue("@EmpEmail", txtEmail.Text.Trim());
                                strQuerEditSupplier.Parameters.AddWithValue("@EmpUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                strQuerEditSupplier.Parameters.AddWithValue("@EmpUpdateDate", DateTime.Now.Date.ToString());
                                strQuerEditSupplier.Parameters.AddWithValue("@IdRole", ddlRole.SelectedValue.Trim());
                                strQuerEditSupplier.Parameters.AddWithValue("@IdEmployee", id);
                                strQuerEditSupplier.ExecuteNonQuery();
                                sqlConnection.Close();
                                sqlConnection.Dispose();
                                Response.Redirect("EmployeePage.aspx");
                            }
                        }
                    }
                    catch (Exception ex)
                    { 
                        throw ex;
                        //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + ex + "');", true);
                    }
                    finally { sqlConnection.Close(); sqlConnection.Dispose();}
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Password is not Match!" + "');", true);
                }
            }
            else
            {
                if (txtEmployeeNm.Text.Trim() == "")
                {
                    vldEmployeeNm.Text = "Employee Name Field Must Be Filled!";
                    txtEmployeeNm.Text = "";
                }
                if(txtEmployeeUsr.Text.Trim() == "")
                {
                    vldEmployeeUsr.Text = "Username Field Must Be Filled!";
                    txtEmployeeUsr.Text = "";
                }
                if (txtEmployeePass.Text.Trim() == "")
                {
                    vldEmployeePass.Text = "Password Field Must Be Filled!";
                    txtEmployeePass.Text = "";
                }
                if (txtEmployeePassCnfrm.Text.Trim() == "")
                {
                    vldEmployeePassCnfrm.Text = "Confirm Password Field Must Be Filled!";
                    txtEmployeePassCnfrm.Text = "";
                }
                if (txtAddr.Text.Trim() == "")
                {
                    vldAddr.Text = "Address Field Must Be Filled!";
                    txtAddr.Text = "";
                }
                if (txtEmail.Text.Trim() == "")
                {
                    vldEmail.Text = "Email Field Must Be Filled!";
                    txtEmail.Text = "";
                }
                if (txtPhone.Text.Trim() == "")
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

        protected void bckBtn_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
                Response.Redirect((string)refUrl);
        }
    }
}