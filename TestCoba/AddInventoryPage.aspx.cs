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
    public partial class AddInventoryPage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        Inventory inventoryObj = new Inventory();
        List<Inventory> inventoryLst = new List<Inventory>();
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
                        sqlConnection.Open();
                        SqlCommand commNama = new SqlCommand("SELECT * FROM Inventory WHERE IdInventory = @IdInventory ", sqlConnection);
                        commNama.Parameters.AddWithValue("@IdInventory", id);
                        using (SqlDataReader dr = commNama.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                var capacity = (int)dr["IvnCapacity"];
                                txtStorage.Text = dr.GetString(dr.GetOrdinal("IvnStorage"));
                                txtCapacity.Text = capacity.ToString();
                                lblStatusPage.Text = "Update Inventory";
                                //txtStorage.Enabled = false;
                                //txtSupName.Enabled = false;
                            }
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
                    Response.Redirect("Login.aspx");
                }
            }
            vldStorage.Text = "";
            vldCapacity.Text = "";
        }
        protected void btnIvnOK_Click(object sender, EventArgs e)
        {
            bool capacityStatus = VerifyNumericValue(txtCapacity.Text.Trim());
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            if (txtStorage.Text.Trim() != "" && txtCapacity.Text.Trim() != "" && capacityStatus == true)
            {
                try
                {
                    sqlConnection.Open();
                    int id;
                    int.TryParse(Request.QueryString["id"], out id);
                    int numRecords = 0;
                    SqlCommand commNama = new SqlCommand("SELECT COUNT(*) FROM Inventory WHERE IvnStorage = @IvnStorage", sqlConnection);
                    commNama.Parameters.AddWithValue("@IvnStorage", txtStorage.Text.Trim());
                    numRecords = (int)commNama.ExecuteScalar();
                    if (id == 0)
                    {
                        if (numRecords == 0)
                        {
                            SqlCommand command = new SqlCommand("insert into Inventory" + "(IvnStorage,IvnCapacity,InvStatus,IvnCreatedBy,IvnCreatedDate,IvnUpdateBy,IvnUpdateDate,IvnSisaCapacity) values " + "('" + txtStorage.Text.Trim().ToUpper() + "','" + txtCapacity.Text.Trim() + "','ACTIVE','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + txtCapacity.Text.Trim() + "')", sqlConnection);
                            command.ExecuteNonQuery();
                            sqlConnection.Close();
                            sqlConnection.Dispose();
                            Response.Redirect("InventoryPage.aspx");
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nama Gudang Sudah Ada Dalam Data Base!" + "');", true);
                        }
                    }
                    else
                    {
                        if (lblStatusPage.Text.Trim() == "Update Inventory")
                        {
                            int totalbrgStrg = checkCapacityTerpakai(id,sqlConnection);
                            if (Convert.ToInt32(txtCapacity.Text.Trim()) >= totalbrgStrg)
                            {
                                SqlCommand strQuerEditSupplier = new SqlCommand("update Inventory set IvnStorage=@IvnStorage,IvnCapacity=@IvnCapacity,IvnUpdateBy=@IvnUpdateBy,IvnUpdateDate=@IvnUpdateDate,IvnSisaCapacity=@IvnSisaCapacity where IdInventory=@IdInventory", sqlConnection);
                                strQuerEditSupplier.Parameters.AddWithValue("@IvnStorage", txtStorage.Text.Trim().ToUpper());
                                strQuerEditSupplier.Parameters.AddWithValue("@IvnCapacity", txtCapacity.Text.Trim());
                                strQuerEditSupplier.Parameters.AddWithValue("@IvnUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                strQuerEditSupplier.Parameters.AddWithValue("@IvnUpdateDate", DateTime.Now.Date.ToString());
                                strQuerEditSupplier.Parameters.AddWithValue("@IvnSisaCapacity", (Convert.ToInt32(txtCapacity.Text.Trim())-totalbrgStrg));
                                strQuerEditSupplier.Parameters.AddWithValue("@IdInventory", id);
                                strQuerEditSupplier.ExecuteNonQuery();
                                sqlConnection.Close();
                                sqlConnection.Dispose();
                                Response.Redirect("InventoryPage.aspx", false);
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Kapasitas Gudang " + txtStorage.Text.Trim().ToUpper() + " tidak boleh kurang dari total box barang!" + "');", true);
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
            else
            {
                if (txtStorage.Text.Trim() == "")
                {
                    vldStorage.Text = "Nama Gudang Harus Di isi!";
                    txtStorage.Text = "";
                }
                if (txtCapacity.Text.Trim() == "")
                {
                    vldCapacity.Text = "Kapasitas Gudang Harus Di isi!";
                    txtCapacity.Text = "";
                }
                if (txtCapacity.Text.Trim() != "" && capacityStatus == false)
                {
                    vldCapacity.Text = "Harus Di isi hanya dengan angka!";
                    txtCapacity.Text = "";
                }
            }
        }
        private int checkCapacityTerpakai(int idStorage, SqlConnection sqlConnection)
        {
            int strgTerpakai = 0;
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT SUM(BarangJmlBox) as TOTALBOX FROM Barang WHERE IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", idStorage);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        strgTerpakai = (int)dr["TOTALBOX"];
                        return strgTerpakai;
                    }
                    else
                    {
                        strgTerpakai = 0;
                        return strgTerpakai;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
    }
}