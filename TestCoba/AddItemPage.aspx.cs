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
    public partial class AddItemPage : System.Web.UI.Page
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
                    //connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\taufik\Documents\Visual Studio 2013\Projects\TestCoba2\TestCoba\App_Data\Test.mdf';Integrated Security=True;MultipleActiveResultSets=True";
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
                            ddlSatuanBrg.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                            ddlSatuanBrg.Items.Insert(1, new ListItem("-------------------", String.Empty));
                            ddlSatuanBrg.Items.Insert(2, "KG");
                            ddlSatuanBrg.Items.Insert(3, "GR");
                            if (dr.Read())
                            {
                                string jmlBoxStr;
                                var strg = (int)dr["IdInventory"];
                                var supId = (int)dr["IdSupplier"];
                                //var hargaJl = (decimal)dr["BarangHargaJual"];
                                var qtyBrg = (decimal)dr["BarangQuantity"];
                                var jmlBox = (int)dr["BarangJmlBox"];
                                jmlBoxStr = string.Format("{0:n}", jmlBox);
                                //var tglBeli = Convert.ToDateTime(dr["BarangTanggalBeli"]).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                txtBrgName.Text = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                txtQtyBrg.Text = qtyBrg.ToString().Replace(",",".");
                                txtHargaJual.Text = "1";
                                //txtCldBrgBeli.Text = Convert.ToDateTime(dr["BarangTanggalBeli"]).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                ddlSatuanBrg.SelectedValue = dr.GetString(dr.GetOrdinal("BarangSatuan"));
                                ddlSupName.SelectedValue = supId.ToString();
                                txtCldBrgBeli.Text = DateTime.Now.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                ddlStorage.SelectedValue = strg.ToString();
                                lblStatusPage.Text = "Update Barang";
                                txtmlBox.Text = jmlBoxStr.ToString().Replace(",", ".");
                                txtItemCode.Text = dr.GetString(dr.GetOrdinal("BarangItemCode"));
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
                                //brgObj.BarangTglBeli = Convert.ToDateTime(dr["BarangTanggalBeli"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                brgObj.IdInventory = (int)dr["IdInventory"];
                                brgObj.BarangJmlBox = (int)dr["BarangJmlBox"];
                                brgObj.BarangItemCode = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                                detailBarangList.Add(brgObj);
                                lblTglBeli.Visible = false;
                                txtCldBrgBeli.Visible = false;
                                lblQtyBrg.Visible = false;
                                txtQtyBrg.Visible = false;
                                lblHargaJual.Visible = false;
                                txtHargaJual.Visible = false;
                                lblJmlBox.Visible = false;
                                txtmlBox.Visible = false;
                                ddlSupName.Enabled = false;
                                ddlStorage.Enabled = false;
                            }
                            else
                            {
                                if (GetItemCode().Trim() == "")
                                {
                                    txtItemCode.Text = "100676";
                                }
                                else
                                {
                                    txtItemCode.Text = GetItemCode();
                                }
                                lblTglBeli.Visible = false;
                                lblQtyBrg.Visible = false;
                                lblHargaJual.Visible = false;
                                lblJmlBox.Visible = false;
                                txtCldBrgBeli.Visible = false;
                                txtHargaJual.Visible = false;
                                txtmlBox.Visible = false;
                                txtQtyBrg.Visible = false;
                                vldTglBeli.Visible = false;
                                vldQtyBrg.Visible = false;
                                vldSatuanBrg.Visible = false;
                                vldHargaJual.Visible = false;
                                vldJmlBox.Visible = false;
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
            vldHargaJual.Text = "";
            vldJmlBox.Text = "";
            vldQtyBrg.Text = "";
            vldTglBeli.Text = "";
            vldSatuanBrg.Text = "";
        }
        const string BarangState = "DetailBrg_cnst";
        public List<Barang> detailBarangList
        {
            get
            {
                if (!(ViewState[BarangState] is List<Barang>))
                {
                    ViewState[BarangState] = new List<Barang>();
                }

                return (List<Barang>)ViewState[BarangState];
            }
        }
        private bool checkCapacityStorageNew(SqlConnection sqlConnection, List<Barang> dtlBarang)
        {
            int capacityTmp = 0;
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT * FROM Inventory WHERE IdInventory = @IdInventory ", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", dtlBarang[0].IdInventory);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        capacityTmp = (int)dr["IvnSisaCapacity"];
                        capacityTmp -= dtlBarang[0].BarangJmlBox;
                        if (capacityTmp < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        private bool checkCapacityStorage(SqlConnection sqlConnection, int JmlBox, List<Barang> dtlBarang)
        {
            int capacityTmp = 0;
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT * FROM Inventory WHERE IdInventory = @IdInventory ", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", dtlBarang[0].IdInventory);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        capacityTmp = (int)dr["IvnSisaCapacity"];
                        capacityTmp += dtlBarang[0].BarangJmlBox;
                        capacityTmp -= JmlBox;
                        if(capacityTmp < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        private void updateKapasitasGudangNew(List<Barang> dtlBarang,SqlConnection sqlConnection)
        {
            int capacityTmp = 0;
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT IvnSisaCapacity FROM Inventory WHERE IdInventory = @IdInventory ", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", dtlBarang[0].IdInventory);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        capacityTmp = (int)dr["IvnSisaCapacity"];
                        capacityTmp -= dtlBarang[0].BarangJmlBox;
                        SqlCommand strQuerEditInventory = new SqlCommand("update Inventory set IvnSisaCapacity=@IvnSisaCapacity,IvnUpdateBy=@IvnUpdateBy,IvnUpdateDate=@IvnUpdateDate where IdInventory=@IdInventory", sqlConnection);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnSisaCapacity", capacityTmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateBy", ((Role)Session["Role"]).EmailEmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateDate", DateTime.Now.Date.ToString());
                        strQuerEditInventory.Parameters.AddWithValue("@IdInventory", dtlBarang[0].IdInventory);
                        strQuerEditInventory.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        private void updateKapasitasGudang(List<Barang> dtlBarang, int jumlahBox, SqlConnection sqlConnection)
        {
            int capacityTmp = 0;
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT IvnSisaCapacity FROM Inventory WHERE IdInventory = @IdInventory ", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", dtlBarang[0].IdInventory);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        capacityTmp = (int)dr["IvnSisaCapacity"];
                        capacityTmp += dtlBarang[0].BarangJmlBox;
                        capacityTmp -= jumlahBox;
                        SqlCommand strQuerEditInventory = new SqlCommand("update Inventory set IvnSisaCapacity=@IvnSisaCapacity,IvnUpdateBy=@IvnUpdateBy,IvnUpdateDate=@IvnUpdateDate where IdInventory=@IdInventory", sqlConnection);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnSisaCapacity", capacityTmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateBy", ((Role)Session["Role"]).EmailEmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateDate", DateTime.Now.Date.ToString());
                        strQuerEditInventory.Parameters.AddWithValue("@IdInventory", dtlBarang[0].IdInventory);
                        strQuerEditInventory.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        private string getNewId(SqlConnection sqlConnection)
        {
            string itmId = "";
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT TOP 1 * FROM Barang ORDER BY IdBarang DESC", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        var brgItemId = (int)dr["IdBarang"];
                        itmId = (brgItemId + 1).ToString();
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
        private string GetItemCode()
        {
            string itmCode = "";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT TOP 1 * FROM Barang ORDER BY IdBarang DESC", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        var brgItemCode = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                        itmCode = (Convert.ToInt32(brgItemCode) + 1).ToString();
                        return itmCode;
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
        private bool VerifyNumericValue(string ValueToCheck)
        {
            //int numval;
            //bool rslt = false;

            //rslt = int.TryParse(ValueToCheck, out numval);

            //return rslt;
            string expression;
            expression = "^[0-9,.]+$";
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
        private bool VerifyJustNumericAboveZero(string ValueToCheck)
        {
            string expression;
            expression = "^[1-9]\\d*\\.?[0]*$";
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
        private bool VerifyDecimalValue(string ValueToCheck)
        {
            string expression;
            expression = "(^\\d*\\.\\d{2}$)";
            if(Regex.IsMatch(ValueToCheck,expression))
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
        protected void btnBrgOK_Click(object sender, EventArgs e)
        {
            int Checkid;
            int.TryParse(Request.QueryString["id"], out Checkid);
            if (Checkid != 0)
            {
                string[] formats = { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy" };
                //DateTime tglBeliVar;
                DateTime tglBeliVar = DateTime.Now.Date;
                if (txtCldBrgBeli.Text.Trim() != "mm/dd/yyyy")
                {
                    tglBeliVar = DateTime.ParseExact(txtCldBrgBeli.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                string test = tglBeliVar.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //decimal qtyDec;
                //decimal hargaJlDec;
                //decimal jmlBoxInt;
                bool jmlBoxValStatus = VerifyDecimalValue(txtmlBox.Text.Trim());
                bool QtyBrgValStatus = VerifyDecimalValue(txtQtyBrg.Text.Trim());
                bool hargaJualValStatus = VerifyDecimalValue(txtHargaJual.Text.Trim());
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                if (txtBrgName.Text.Trim() != "")
                {
                    try
                    {
                        sqlConnection.Open();
                        int id;
                        int.TryParse(Request.QueryString["id"], out id);
                        int numRecords = 0;
                        SqlCommand commNama = new SqlCommand("SELECT COUNT(*) FROM Barang WHERE NamaBarang = @NamaBarang AND IdSupplier = @IdSupplier AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                        commNama.Parameters.AddWithValue("@NamaBarang", txtBrgName.Text.Trim());
                        commNama.Parameters.AddWithValue("@IdSupplier", Convert.ToInt32(ddlSupName.SelectedValue.Trim()));
                        commNama.Parameters.AddWithValue("@IdInventory", Convert.ToInt32(ddlStorage.SelectedValue.Trim()));
                        numRecords = (int)commNama.ExecuteScalar();
                        if (id == 0)
                        {
                            if (numRecords == 0)
                            {
                                brgObj = new Barang();
                                brgObj.IdBarang = Convert.ToInt32(getNewId(sqlConnection));
                                brgObj.NamaBarang = txtBrgName.Text.Trim();
                                brgObj.BarangQty = Convert.ToDecimal(txtQtyBrg.Text.Trim().Replace(".", ","));
                                brgObj.BarangCreatedBy = ((Role)Session["Role"]).EmailEmp;
                                brgObj.BarangCreatedDate = DateTime.Now.Date.ToString();
                                brgObj.BarangUpdateBy = ((Role)Session["Role"]).EmailEmp;
                                brgObj.BarangUpdateDate = DateTime.Now.Date.ToString();
                                //brgObj.BarangHargaJual = Convert.ToDecimal(txtHargaJual.Text.Trim().Replace(".", ","));
                                brgObj.BarangSatuan = ddlSatuanBrg.SelectedValue.Trim();
                                brgObj.IdSupplier = Convert.ToInt32(ddlSupName.SelectedValue.Trim());
                                brgObj.BarangStatus = "ACTIVE";
                                brgObj.BarangTglBeli = tglBeliVar.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                brgObj.IdInventory = Convert.ToInt32(ddlStorage.SelectedValue.Trim());
                                //brgObj.BarangJmlBox = Convert.ToInt32(jmlBoxInt);
                                brgObj.BarangItemCode = txtItemCode.Text.Trim();
                                detailBarangList.Add(brgObj);
                                //if (checkCapacityStorageNew(sqlConnection, detailBarangList))
                                //{
                                //    SqlCommand command = new SqlCommand("insert into Barang" + "(NamaBarang,BarangQuantity,BarangCreatedBy,BarangCreatedDate,BarangUpdateBy,BarangUpdateDate,BarangHargaJual,BarangSatuan,IdSupplier,BarangStatus,BarangTanggalBeli,IdInventory,BarangJmlBox,BarangItemCode) values " + "('" + txtBrgName.Text.Trim().ToUpper() + "'," + txtQtyBrg.Text.Trim() + ",'" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "'," + txtHargaJual.Text.Trim() + ",'KG','" + ddlSupName.SelectedValue.Trim() + "','ACTIVE','" + tglBeliVar.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "','" + ddlStorage.SelectedValue.Trim() + "'," + Convert.ToInt32(jmlBoxInt) + ",'" + txtItemCode.Text.Trim() + "')", sqlConnection);
                                //    command.ExecuteNonQuery();
                                //    updateKapasitasGudangNew(detailBarangList, sqlConnection);
                                //    sqlConnection.Close();
                                //    sqlConnection.Dispose();
                                //    Response.Redirect("ItemPage.aspx", false);
                                //}
                                //else
                                //{
                                //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Kapasitas di storage overload, harus diperbaharui terlebih dahulu!" + "');", true);
                                //}
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nama Barang Sudah Ada di Database!" + "');", true);
                            }
                        }
                        else
                        {
                            if (lblStatusPage.Text.Trim() == "Update Barang")
                            {
                                //if (checkCapacityStorage(sqlConnection, Convert.ToInt32(jmlBoxInt), detailBarangList))
                                //{
                                //    SqlCommand strQuerEditSupplier = new SqlCommand("update Barang set NamaBarang=@NamaBarang,BarangQuantity=@BarangQuantity,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate,BarangHargaJual=@BarangHargaJual,IdSupplier=@IdSupplier,BarangTanggalBeli=@BarangTanggalBeli,IdInventory=@IdInventory,BarangJmlBox=@BarangJmlBox where IdBarang=@IdBarang", sqlConnection);
                                //    strQuerEditSupplier.Parameters.AddWithValue("@NamaBarang", txtBrgName.Text.Trim().ToUpper());
                                //    strQuerEditSupplier.Parameters.AddWithValue("@BarangQuantity", txtQtyBrg.Text.Trim());
                                //    strQuerEditSupplier.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                //    strQuerEditSupplier.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                //    strQuerEditSupplier.Parameters.AddWithValue("@BarangHargaJual", txtHargaJual.Text.Trim());
                                //    //strQuerEditSupplier.Parameters.AddWithValue("@BarangSatuan", ddlSatuanBrg.SelectedValue.Trim());
                                //    strQuerEditSupplier.Parameters.AddWithValue("@IdSupplier", ddlSupName.SelectedValue.Trim());
                                //    strQuerEditSupplier.Parameters.AddWithValue("@BarangTanggalBeli", tglBeliVar.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                                //    strQuerEditSupplier.Parameters.AddWithValue("@IdInventory", ddlStorage.SelectedValue.Trim());
                                //    strQuerEditSupplier.Parameters.AddWithValue("@BarangJmlBox", Convert.ToInt32(jmlBoxInt));
                                //    strQuerEditSupplier.Parameters.AddWithValue("@IdBarang", id);
                                //    strQuerEditSupplier.ExecuteNonQuery();
                                //    updateKapasitasGudang(detailBarangList, Convert.ToInt32(jmlBoxInt), sqlConnection);
                                //    sqlConnection.Close();
                                //    sqlConnection.Dispose();
                                //    Response.Redirect("ItemPage.aspx", false);
                                //}
                                //else
                                //{
                                //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Kapasitas di storage overload, harus diperbaharui terlebih dahulu!" + "');", true);
                                //}
                                SqlCommand strQuerEditSupplier = new SqlCommand("update Barang set NamaBarang=@NamaBarang,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate where IdBarang=@IdBarang", sqlConnection);
                                strQuerEditSupplier.Parameters.AddWithValue("@NamaBarang", txtBrgName.Text.Trim().ToUpper());
                                strQuerEditSupplier.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                strQuerEditSupplier.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                strQuerEditSupplier.Parameters.AddWithValue("@IdBarang", id);
                                strQuerEditSupplier.ExecuteNonQuery();
                                //updateKapasitasGudang(detailBarangList, Convert.ToInt32(jmlBoxInt), sqlConnection);
                                sqlConnection.Close();
                                sqlConnection.Dispose();
                                Response.Redirect("ItemPage.aspx", false);
                            }
                        }
                    }
                    catch (Exception ex)
                    { throw ex; }
                    finally { sqlConnection.Close(); sqlConnection.Dispose(); }
                }
                else
                {
                    if (txtBrgName.Text.Trim() == "")
                    {
                        vldBrgName.Text = "Nama Barang Tidak Boleh Kosong!";
                        txtBrgName.Text = "";
                    }
                    if (txtCldBrgBeli.Text.Trim() == "mm/dd/yyyy")
                    {
                        vldTglBeli.Text = "Tanggal Beli Tidak Boleh Kosong!";
                        txtCldBrgBeli.Text = "mm/dd/yyyy";
                    }
                    if (txtQtyBrg.Text.Trim() == "")
                    {
                        vldQtyBrg.Text = "Qty Tidak Boleh Kosong!";
                        txtQtyBrg.Text = "";
                    }
                    //if (txtQtyBrg.Text.Trim() != "" && !VerifyDecimalValue(txtQtyBrg.Text.Trim()))
                    //{
                    //    if (txtQtyBrg.Text.Trim() != "" && !VerifyNumericValue(txtQtyBrg.Text.Trim()))
                    //    {
                    //        if (txtQtyBrg.Text.Trim() != "" && !VerifyJustNumericAboveZero(txtQtyBrg.Text.Trim()))
                    //        {
                    //            vldQtyBrg.Text = "Qty Barang Tidak Mengandung Karakter, Hanya Angka!";
                    //            txtQtyBrg.Text = "";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (txtQtyBrg.Text.Trim() != "" && !VerifyJustNumericAboveZero(txtQtyBrg.Text.Trim()))
                    //        {
                    //            if (txtQtyBrg.Text.Trim() != "" && Convert.ToDecimal(txtQtyBrg.Text.Trim()) <= 0)
                    //            {
                    //                vldQtyBrg.Text = "Nilai Qty Tidak Boleh Kurang Dari 0!";
                    //                txtQtyBrg.Text = "";
                    //            }
                    //            else
                    //            {
                    //                vldQtyBrg.Text = "Format Qty untuk koma(,) Memakai Titik(.) dan Hanya 2 angka dibelakang koma!";
                    //                txtQtyBrg.Text = "";
                    //            }
                    //        }
                    //        else
                    //        {
                    //            vldQtyBrg.Text = "Format Qty untuk koma(,) Memakai Titik(.) dan Hanya 2 angka dibelakang koma!";
                    //            txtQtyBrg.Text = "";
                    //        }
                    //    }
                    //}
                    if (txtHargaJual.Text.Trim() == "")
                    {
                        vldHargaJual.Text = "Harga Jual Tidak Boleh Kosong!";
                        txtHargaJual.Text = "";
                    }
                    //if (txtHargaJual.Text.Trim() != "" && !VerifyDecimalValue(txtHargaJual.Text.Trim()))
                    //{
                    //    if (txtHargaJual.Text.Trim() != "" && !VerifyNumericValue(txtHargaJual.Text.Trim()))
                    //    {
                    //        if (txtHargaJual.Text.Trim() != "" && !VerifyJustNumericAboveZero(txtHargaJual.Text.Trim()))
                    //        {
                    //            vldHargaJual.Text = "Harga Jual Tidak Mengandung Karakter, Hanya Angka!";
                    //            txtHargaJual.Text = "";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (txtHargaJual.Text.Trim() != "" && !VerifyJustNumericAboveZero(txtHargaJual.Text.Trim()))
                    //        {
                    //            if (txtHargaJual.Text.Trim() != "" && Convert.ToDecimal(txtHargaJual.Text.Trim()) <= 0)
                    //            {
                    //                vldHargaJual.Text = "Nilai Qty Tidak Boleh Kurang Dari 0!";
                    //                txtHargaJual.Text = "";
                    //            }
                    //            else
                    //            {
                    //                vldHargaJual.Text = "Format Qty untuk koma(,) Memakai Titik(.) dan Hanya 2 angka dibelakang koma!";
                    //                txtHargaJual.Text = "";
                    //            }
                    //        }
                    //        else
                    //        {
                    //            vldHargaJual.Text = "Format Qty untuk koma(,) Memakai Titik(.) dan Hanya 2 angka dibelakang koma!";
                    //            txtHargaJual.Text = "";
                    //        }
                    //    }
                    //}
                    if (txtmlBox.Text.Trim() == "")
                    {
                        vldJmlBox.Text = "Jumlah Box Tidak Boleh Kosong!";
                        txtmlBox.Text = "";
                    }
                    if (txtmlBox.Text.Trim() != "" && !VerifyDecimalValue(txtmlBox.Text.Trim()))
                    {
                        if (txtmlBox.Text.Trim() != "" && !VerifyNumericValue(txtmlBox.Text.Trim()))
                        {
                            if (txtmlBox.Text.Trim() != "" && !VerifyJustNumericAboveZero(txtmlBox.Text.Trim()))
                            {
                                vldJmlBox.Text = "Jumlah Box Tidak Mengandung Karakter, Hanya Angka!";
                                txtmlBox.Text = "";
                            }
                        }
                        else
                        {
                            if (txtmlBox.Text.Trim() != "" && !VerifyJustNumericAboveZero(txtmlBox.Text.Trim()))
                            {
                                if (txtmlBox.Text.Trim() != "" && Convert.ToDecimal(txtmlBox.Text.Trim()) <= 0)
                                {
                                    vldJmlBox.Text = "Jumlah Box Tidak Boleh Kurang Dari 0!";
                                    txtmlBox.Text = "";
                                }
                                else
                                {
                                    vldJmlBox.Text = "Jumlah Box Tidak Mengandung Karakter, Hanya Angka!";
                                    txtmlBox.Text = "";
                                }
                            }
                            else
                            {
                                vldJmlBox.Text = "Jumlah Box Tidak Mengandung Karakter, Hanya Angka!";
                                txtmlBox.Text = "";
                            }
                        }
                    }
                    if (ddlSatuanBrg.SelectedIndex == 0 || ddlSatuanBrg.SelectedIndex == 1)
                    {
                        vldSatuanBrg.Text = "Satuan Barang Tidak Boleh Kosong";
                        ddlSatuanBrg.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                if (txtBrgName.Text.Trim() != "")
                {
                    try
                    {
                        sqlConnection.Open();
                        int numRecords = 0;
                        SqlCommand commNama = new SqlCommand("SELECT COUNT(*) FROM Barang WHERE NamaBarang = @NamaBarang AND IdSupplier = @IdSupplier AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                        commNama.Parameters.AddWithValue("@NamaBarang", txtBrgName.Text.Trim());
                        commNama.Parameters.AddWithValue("@IdSupplier", Convert.ToInt32(ddlSupName.SelectedValue.Trim()));
                        commNama.Parameters.AddWithValue("@IdInventory", Convert.ToInt32(ddlStorage.SelectedValue.Trim()));
                        numRecords = (int)commNama.ExecuteScalar();
                        if (numRecords == 0)
                        {
                            brgObj = new Barang();
                            //brgObj.IdBarang = Convert.ToInt32(getNewId(sqlConnection));
                            brgObj.NamaBarang = txtBrgName.Text.Trim();
                            brgObj.BarangQty = 0;
                            brgObj.BarangCreatedBy = ((Role)Session["Role"]).EmailEmp;
                            brgObj.BarangCreatedDate = DateTime.Now.Date.ToString();
                            brgObj.BarangUpdateBy = ((Role)Session["Role"]).EmailEmp;
                            brgObj.BarangUpdateDate = DateTime.Now.Date.ToString();
                            //brgObj.BarangHargaJual = 0;
                            brgObj.BarangSatuan = "KG";
                            brgObj.IdSupplier = Convert.ToInt32(ddlSupName.SelectedValue.Trim());
                            brgObj.BarangStatus = "ACTIVE";
                            brgObj.IdInventory = Convert.ToInt32(ddlStorage.SelectedValue.Trim());
                            brgObj.BarangJmlBox = 0;
                            brgObj.BarangItemCode = txtItemCode.Text.Trim();
                            detailBarangList.Add(brgObj);
                            SqlCommand command = new SqlCommand("insert into Barang" + "(NamaBarang,BarangQuantity,BarangCreatedBy,BarangCreatedDate,BarangUpdateBy,BarangUpdateDate,BarangSatuan,IdSupplier,BarangStatus,IdInventory,BarangJmlBox,BarangItemCode,BarangCategory) values " + "('" + txtBrgName.Text.Trim() + "'," + brgObj.BarangQty + ",'" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','KG','" + ddlSupName.SelectedValue.Trim() + "','ACTIVE','" + ddlStorage.SelectedValue.Trim() + "'," + brgObj.BarangJmlBox + ",'" + txtItemCode.Text.Trim() + "','PABRIKAN')", sqlConnection);
                            command.ExecuteNonQuery();
                            //updateKapasitasGudangNew(detailBarangList, sqlConnection);
                            sqlConnection.Close();
                            sqlConnection.Dispose();
                            Response.Redirect("ItemPage.aspx", false);
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nama Barang Sudah Ada di Database!" + "');", true);
                        }
                    }
                    catch (Exception ex)
                    { throw ex; }
                    finally { sqlConnection.Close(); sqlConnection.Dispose(); }
                }
                else
                {
                    if (txtBrgName.Text.Trim() == "")
                    {
                        vldBrgName.Text = "Nama Barang Tidak Boleh Kosong!";
                        txtBrgName.Text = "";
                    }
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