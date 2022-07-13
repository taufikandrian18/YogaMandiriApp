using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Globalization;

namespace TestCoba
{
    public partial class UpdateStockOpname : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        Barang brgObj = new Barang();
        List<Barang> myBrgObj = new List<Barang>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                if (Session["Role"] != null)
                {
                    int id;
                    int.TryParse(Request.QueryString["id"], out id);
                    string codePage = Request.QueryString["code"];
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang ", sqlConnection);
                        commNama.Parameters.AddWithValue("@IdBarang", id);
                        using (SqlDataReader dr = commNama.ExecuteReader())
                        {
                            string sup = "Select * from Supplier WHERE SupplierStatus = 'ACTIVE'";
                            string com = "Select * from Inventory WHERE InvStatus = 'ACTIVE'";
                            SqlDataAdapter adpt = new SqlDataAdapter(com, sqlConnection);
                            SqlDataAdapter supAdpt = new SqlDataAdapter(sup, sqlConnection);
                            DataTable dtSup = new DataTable();
                            DataTable dt = new DataTable();
                            adpt.Fill(dt);
                            supAdpt.Fill(dtSup);
                            ddlSupName.DataSource = dtSup;
                            ddlSupName.DataBind();
                            ddlSupName.DataTextField = "SupplierName";
                            ddlSupName.DataValueField = "IdSupplier";
                            ddlSupName.DataBind();
                            ddlStorage.DataSource = dt;
                            ddlStorage.DataBind();
                            ddlStorage.DataTextField = "IvnStorage";
                            ddlStorage.DataValueField = "IdInventory";
                            ddlStorage.DataBind();
                            if (dr.Read())
                            {
                                if (codePage.Trim() == "QTY")
                                {
                                    string jmlBoxStr;
                                    var strg = (int)dr["IdInventory"];
                                    var supId = (int)dr["IdSupplier"];
                                    var qtyBrg = (decimal)dr["BarangQuantity"];
                                    var jmlBox = (int)dr["BarangJmlBox"];
                                    jmlBoxStr = string.Format("{0:n}", jmlBox);
                                    var tglBeli = Convert.ToDateTime(dr["BarangTanggalBeli"]).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                    txtBrgName.Text = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                    txtBrgName.Enabled = false;
                                    txtQtyBrg.Text = qtyBrg.ToString().Replace(",", ".");
                                    ddlSupName.SelectedValue = supId.ToString();
                                    ddlSupName.Enabled = false;
                                    //txtCldBrgBeli.Text = dr.GetString(dr.GetOrdinal("BarangTanggalBeli"));
                                    ddlStorage.SelectedValue = strg.ToString();
                                    ddlStorage.Enabled = false;
                                    txtItemCode.Text = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                                    txtItemCode.Enabled = false;
                                    lblJmlBox.Visible = false;
                                    txtmlBox.Visible = false;
                                    vldJmlBox.Visible = false;
                                    //txtEmployeeUsr.Enabled = false;
                                    //txtEmail.Enabled = false;
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
                                    brgObj.BarangTglBeli = Convert.ToDateTime(dr["BarangTanggalBeli"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    brgObj.IdInventory = (int)dr["IdInventory"];
                                    brgObj.BarangJmlBox = (int)dr["BarangJmlBox"];
                                    brgObj.BarangItemCode = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                                    myBrgObj.Add(brgObj);
                                    ViewState["qtyTbl"] = txtQtyBrg.Text.Trim();
                                }
                                else
                                {
                                    string jmlBoxStr;
                                    var strg = (int)dr["IdInventory"];
                                    var supId = (int)dr["IdSupplier"];
                                    var qtyBrg = (decimal)dr["BarangQuantity"];
                                    var jmlBox = (int)dr["BarangJmlBox"];
                                    jmlBoxStr = string.Format("{0:n}", jmlBox);
                                    var tglBeli = Convert.ToDateTime(dr["BarangTanggalBeli"]).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                    txtBrgName.Text = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                    txtBrgName.Enabled = false;
                                    //txtQtyBrg.Text = qtyBrg.ToString().Replace(",", ".");
                                    ddlSupName.SelectedValue = supId.ToString();
                                    ddlSupName.Enabled = false;
                                    //txtCldBrgBeli.Text = dr.GetString(dr.GetOrdinal("BarangTanggalBeli"));
                                    ddlStorage.SelectedValue = strg.ToString();
                                    ddlStorage.Enabled = false;
                                    txtmlBox.Text = jmlBoxStr.ToString().Replace(",", ".");
                                    txtItemCode.Text = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                                    txtItemCode.Enabled = false;
                                    lblQtyBrg.Visible = false;
                                    txtQtyBrg.Visible = false;
                                    vldQtyBrg.Visible = false;
                                    //txtEmployeeUsr.Enabled = false;
                                    //txtEmail.Enabled = false;
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
                                    brgObj.BarangTglBeli = Convert.ToDateTime(dr["BarangTanggalBeli"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    brgObj.IdInventory = (int)dr["IdInventory"];
                                    brgObj.BarangJmlBox = (int)dr["BarangJmlBox"];
                                    brgObj.BarangItemCode = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                                    myBrgObj.Add(brgObj);
                                    ViewState["boxTbl"] = txtmlBox.Text.Trim();
                                }
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
            vldBrgName.Text = "";
            vldQtyBrg.Text = "";
            vldJmlBox.Text = "";
        }

        protected void bckBtn_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
                Response.Redirect((string)refUrl);
        }

        protected void btnBrgOK_Click(object sender, EventArgs e)
        {
            int id;
            int.TryParse(Request.QueryString["id"], out id);
            string codePage = Request.QueryString["code"];
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                if (codePage.Trim() == "QTY")
                {
                    if (ViewState["qtyTbl"] != null)
                    {
                        string nilaiLama = ViewState["qtyTbl"].ToString();
                        if (txtQtyBrg.Text.Trim() != "" && txtQtyBrg.Text.Trim() != "0")
                        {
                            if (txtQtyBrg.Text.Trim() != nilaiLama)
                            {
                                double txtQtyItmVal = Convert.ToDouble(txtQtyBrg.Text.Trim().Replace(".",","));
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                sqlConnection.Open();
                                SqlCommand strQuerEditBarang = new SqlCommand("update Barang set BarangQuantity=@BarangQuantity,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate where IdBarang=@IdBarang", sqlConnection);
                                strQuerEditBarang.Parameters.AddWithValue("@IdBarang", id);
                                strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                strQuerEditBarang.Parameters.AddWithValue("@BarangQuantity", Convert.ToDecimal(txtQtyItmStr));
                                strQuerEditBarang.ExecuteNonQuery();
                                SqlCommand strQuerStockOpname = new SqlCommand("insert into StockOpnameLog" + "(JenisUpdate,NilaiLama,NilaiDiubah,IdBarang,UpdatedBy,UpdatedDate) values " + "('Quantity','" + nilaiLama + "','" + txtQtyItmStr + "','"+id+"','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "')", sqlConnection);
                                strQuerStockOpname.ExecuteNonQuery();
                                sqlConnection.Close(); 
                                sqlConnection.Dispose();
                                Response.Redirect("StockOpnamePage.aspx");
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nilai Qty Tidak Boleh Sama degan Sebelumnya!" + "');", true);
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nilai Qty Tidak Boleh Kosong!" + "');", true);
                        }
                    }
                }
                else
                {
                    if (ViewState["boxTbl"] != null)
                    {
                        string nilaiLama = ViewState["boxTbl"].ToString();
                        if (txtmlBox.Text.Trim() != "" && txtmlBox.Text.Trim() != "0")
                        {
                            if (txtmlBox.Text.Trim() != nilaiLama)
                            {
                                int boxVal = Convert.ToInt32(txtmlBox.Text.Trim().Replace(".",","));
                                sqlConnection.Open();
                                SqlCommand strQuerEditBarang = new SqlCommand("update Barang set BarangJmlBox=@BarangJmlBox,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate where IdBarang=@IdBarang", sqlConnection);
                                strQuerEditBarang.Parameters.AddWithValue("@IdBarang", id);
                                strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                strQuerEditBarang.Parameters.AddWithValue("@BarangJmlBox", boxVal);
                                strQuerEditBarang.ExecuteNonQuery();
                                updateKapasitasGudang(Convert.ToInt32(ddlStorage.SelectedValue.Trim()), boxVal, Convert.ToInt32(Convert.ToDecimal(nilaiLama.Replace(".", ","))));
                                SqlCommand strQuerStockOpname = new SqlCommand("insert into StockOpnameLog" + "(JenisUpdate,NilaiLama,NilaiDiubah,IdBarang,UpdatedBy,UpdatedDate) values " + "('Box','" + nilaiLama + "','" + boxVal + "','" + id + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "')", sqlConnection);
                                strQuerStockOpname.ExecuteNonQuery();
                                sqlConnection.Close();
                                sqlConnection.Dispose();
                                Response.Redirect("StockOpnamePage.aspx");
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nilai Qty Tidak Boleh Sama degan Sebelumnya!" + "');", true);
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nilai Box Tidak Boleh Kosong!" + "');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        private void updateKapasitasGudang(int idInventory,int jmlBoxVal,int nilaiLama)
        {
            int capacityTmp = 0;
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT IvnSisaCapacity FROM Inventory WHERE IdInventory = @IdInventory ", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", idInventory);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        capacityTmp = (int)dr["IvnSisaCapacity"];
                        capacityTmp += nilaiLama;
                        capacityTmp -= jmlBoxVal;
                        SqlCommand strQuerEditInventory = new SqlCommand("update Inventory set IvnSisaCapacity=@IvnSisaCapacity,IvnUpdateBy=@IvnUpdateBy,IvnUpdateDate=@IvnUpdateDate where IdInventory=@IdInventory", sqlConnection);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnSisaCapacity", capacityTmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateBy", ((Role)Session["Role"]).EmailEmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateDate", DateTime.Now.Date.ToString());
                        strQuerEditInventory.Parameters.AddWithValue("@IdInventory", idInventory);
                        strQuerEditInventory.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}