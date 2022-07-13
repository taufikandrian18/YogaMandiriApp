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
    public partial class AddBarangOlahan : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        Barang brgObj;
        List<Barang> lstBrg;
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
                        SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang ", sqlConnection);
                        commNama.Parameters.AddWithValue("@IdBarang", id);
                        using (SqlDataReader dr = commNama.ExecuteReader())
                        {
                            string sup = "Select * from Supplier WHERE SupplierStatus = 'ACTIVE'";
                            string com = "Select * from Inventory WHERE InvStatus = 'ACTIVE'";
                            SqlDataAdapter supAdpt = new SqlDataAdapter(sup, sqlConnection);
                            SqlDataAdapter adpt = new SqlDataAdapter(com, sqlConnection);
                            DataTable dt = new DataTable();
                            DataTable dtSup = new DataTable();
                            adpt.Fill(dt);
                            supAdpt.Fill(dtSup);
                            ddlNamaGudang.DataSource = dt;
                            ddlNamaGudang.DataBind();
                            ddlNamaGudang.DataTextField = "IvnStorage";
                            ddlNamaGudang.DataValueField = "IdInventory";
                            ddlNamaGudang.DataBind();
                            ddlSupName.DataSource = dtSup;
                            ddlSupName.DataBind();
                            ddlSupName.DataTextField = "SupplierName";
                            ddlSupName.DataValueField = "IdSupplier";
                            ddlSupName.DataBind();
                            if (dr.Read())
                            {
                                brgObj = new Barang();
                                brgObj.IdBarang = id;
                                var supId = (int)dr["IdSupplier"];
                                brgObj.IdSupplier = supId;
                                var strg = (int)dr["IdInventory"];
                                brgObj.IdInventory = strg;
                                lblHeaderPage.Text = "Update Barang Olahan";
                                lblStatusPage.Text = "Update Barang Olahan";
                                DateTime tglBeliStr = DateTime.ParseExact(dr.GetString(dr.GetOrdinal("BarangTanggalBeli")), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                txtNamaBrgOlahan.Text = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                txtNamaBrgOlahan.Enabled = false;
                                brgObj.NamaBarang = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                brgObj.BarangStatus = dr.GetString(dr.GetOrdinal("BarangStatus"));
                                brgObj.BarangCategory = dr.GetString(dr.GetOrdinal("BarangCategory"));
                                txtItemCode.Text = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                                txtItemCode.Enabled = false;
                                brgObj.BarangItemCode = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                                ddlSupName.SelectedValue = supId.ToString();
                                ddlSupName.Enabled = false;
                                ddlNamaGudang.SelectedValue = strg.ToString();
                                ddlNamaGudang.Enabled = false;
                                getDtlBrgOlahan(id);
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
                                BindRepeater();
                                BindDDLBarang(0, 0);
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

        protected void getDtlBrgOlahan(int id)
        {
            DataTable counter = new DataTable();
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT * FROM BarangOlahan WHERE IdBrgOlahan = @IdBrgOlahan", sqlConnection);
                commNama.Parameters.AddWithValue("@IdBrgOlahan", id);
                SqlDataAdapter pmbDtlAdapter = new SqlDataAdapter();
                pmbDtlAdapter.SelectCommand = commNama;
                pmbDtlAdapter.Fill(counter);
                List<DataRow> dtRowList = counter.AsEnumerable().ToList();
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        DataTable pmbDtl = new DataTable();
                        pmbDtl.Clear();
                        //var brgId = (int)dr["IdBarang"];
                        //pmbDtl.Columns.Add(brgId.ToString(), typeof(int));
                        //var itmQty = (decimal)dr["QtyBarangPOPembelianDtl"];
                        //pmbDtl.Columns.Add(itmQty.ToString(), typeof(string));
                        //var itmPrice = (decimal)dr["HargaPOPembelianDtl"];
                        //pmbDtl.Columns.Add(itmPrice.ToString(), typeof(string));
                        //var itmBox = (int)dr["JumlahBoxPOPembelianDtl"];
                        //pmbDtl.Columns.Add(itmBox.ToString(), typeof(string));
                        //var itmTotal = (decimal)dr["TotalHargaPOPembelianDtl"];
                        //pmbDtl.Columns.Add(itmTotal.ToString(), typeof(string));
                        pmbDtl.Columns.Add("ddlNamaBarang", typeof(int));
                        pmbDtl.Columns.Add("txtItemQty", typeof(string));
                        pmbDtl.Columns.Add("txtItemBox", typeof(string));
                        for (int i = 0; i < counter.Rows.Count; i++)
                        {
                            DataRow drPmbDtl = pmbDtl.NewRow();
                            drPmbDtl["ddlNamaBarang"] = dtRowList[i]["IdBarang"];
                            drPmbDtl["txtItemQty"] = 0;
                            drPmbDtl["txtItemBox"] = 0;
                            //drPmbDtl["txtItemQty"] = dtRowList[i]["QtyBarangOlahan"];
                            //drPmbDtl["txtItemBox"] = dtRowList[i]["JmlBoxBarangOlahan"];
                            pmbDtl.Rows.Add(drPmbDtl);
                        }
                        ViewState["Curtbl"] = pmbDtl;
                        //ViewState["ReturTbl"] = pmbDtl;
                        rptBrgOlahan.DataSource = pmbDtl;
                        rptBrgOlahan.DataBind();
                    }
                }
                SetOldDataUpdate();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        private void SetOldDataUpdate()
        {
            int rowIndex = 0;
            if (ViewState["Curtbl"] != null)
            {
                DataTable dt = (DataTable)ViewState["Curtbl"];
                //BindDDLBarang(dt.Rows.Count);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //TextBox txtItemCode = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemCode");
                        //TextBox txtItemName = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemName");
                        DropDownList ddlNamaBarang = (DropDownList)rptBrgOlahan.Items[rowIndex].FindControl("ddlNamaBarang");
                        TextBox txtItemQty = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemQty");
                        TextBox txtItemBox = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemBox");
                        LinkButton lnkBtnAdd = (LinkButton)rptBrgOlahan.Items[rowIndex].FindControl("btnadd");
                        LinkButton lnkBtnDel = (LinkButton)rptBrgOlahan.Items[rowIndex].FindControl("btndel");
                        //txtItemCode.Text = (Convert.ToInt32(GetItemCode()) + i).ToString();
                        //txtItemName.Text = dt.Rows[i]["txtItemName"].ToString();
                        //ddlNamaBarang.SelectedIndex = (Convert.ToInt32(dt.Rows[i]["txtItemName"].ToString()));
                        BindDDLBarangUpdate(i, (Convert.ToInt32(dt.Rows[i]["ddlNamaBarang"].ToString())));
                        ddlNamaBarang.Enabled = false;
                        txtItemQty.Text = dt.Rows[i]["txtItemQty"].ToString();
                        txtItemBox.Text = dt.Rows[i]["txtItemBox"].ToString();
                        //txtItemBox.Enabled = false;
                        lnkBtnAdd.Visible = false;
                        lnkBtnDel.Visible = false;
                        rowIndex++;
                    }
                }
            }
        }

        protected void BindDDLBarangUpdate(int numRow, int slctdIndx)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                DropDownList ddlNamaBarang = (DropDownList)rptBrgOlahan.Items[numRow].FindControl("ddlNamaBarang");
                SqlCommand commNama = new SqlCommand("SELECT NamaBarang FROM Barang WHERE IdBarang=@IdBarang AND IdInventory = @IdInventory AND IdSupplier= @IdSupplier AND BarangStatus = 'ACTIVE' AND BarangCategory = 'PABRIKAN' ORDER BY NamaBarang", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", ddlNamaGudang.SelectedValue.Trim());
                commNama.Parameters.AddWithValue("@IdSupplier", ddlSupName.SelectedValue.Trim());
                commNama.Parameters.AddWithValue("@IdBarang", slctdIndx);
                SqlDataAdapter brgAdapter = new SqlDataAdapter();
                brgAdapter.SelectCommand = commNama;
                DataTable dtBrg = new DataTable();
                brgAdapter.Fill(dtBrg);
                if (dtBrg.Rows.Count != 0)
                {
                    ddlNamaBarang.DataSource = dtBrg;
                    ddlNamaBarang.DataBind();
                    ddlNamaBarang.DataTextField = "NamaBarang";
                    ddlNamaBarang.DataValueField = "NamaBarang";
                    ddlNamaBarang.DataBind();
                    ddlNamaBarang.SelectedIndex = slctdIndx;
                    ddlNamaBarang.Enabled = false;
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void BindDDLBarang(int numRow, int slctdIndx)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                DropDownList ddlNamaBarang = (DropDownList)rptBrgOlahan.Items[numRow].FindControl("ddlNamaBarang");
                SqlCommand commNama = new SqlCommand("SELECT NamaBarang FROM Barang WHERE IdInventory = @IdInventory AND IdSupplier= @IdSupplier AND BarangStatus = 'ACTIVE' AND BarangCategory = 'PABRIKAN' ORDER BY NamaBarang", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", ddlNamaGudang.SelectedValue.Trim());
                commNama.Parameters.AddWithValue("@IdSupplier", ddlSupName.SelectedValue.Trim());
                SqlDataAdapter brgAdapter = new SqlDataAdapter();
                brgAdapter.SelectCommand = commNama;
                DataTable dtBrg = new DataTable();
                brgAdapter.Fill(dtBrg);
                if (dtBrg.Rows.Count != 0)
                {
                    TextBox txtItemQty = (TextBox)rptBrgOlahan.Items[numRow].FindControl("txtItemQty");
                    TextBox txtItemBox = (TextBox)rptBrgOlahan.Items[numRow].FindControl("txtItemBox");
                    ddlNamaBarang.DataSource = dtBrg;
                    ddlNamaBarang.DataBind();
                    ddlNamaBarang.Enabled = true;
                    txtItemQty.Enabled = true;
                    txtItemBox.Enabled = true;
                    ddlNamaBarang.DataTextField = "NamaBarang";
                    ddlNamaBarang.DataValueField = "NamaBarang";
                    ddlNamaBarang.DataBind();
                    ddlNamaBarang.SelectedIndex = slctdIndx;

                }
                else
                {
                    BindRepeater();
                    DropDownList ddlNewBarang = (DropDownList)rptBrgOlahan.Items[0].FindControl("ddlNamaBarang");
                    TextBox txtItemQty = (TextBox)rptBrgOlahan.Items[0].FindControl("txtItemQty");
                    TextBox txtItemBox = (TextBox)rptBrgOlahan.Items[0].FindControl("txtItemBox");
                    ddlNewBarang.DataSource = dtBrg;
                    ddlNewBarang.DataBind();
                    ddlNewBarang.Items.Insert(0, new ListItem("Data Kosong", "Data Kosong"));
                    ddlNewBarang.Enabled = false;
                    txtItemQty.Enabled = false;
                    txtItemBox.Enabled = false;
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void BindRepeater()
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add("txtItemCode", typeof(string));
            //dt.Columns.Add("txtItemName", typeof(string));
            dt.Columns.Add("ddlNamaBarang", typeof(int));
            dt.Columns.Add("txtItemQty", typeof(string));
            dt.Columns.Add("txtItemBox", typeof(string));
            DataRow dr = dt.NewRow();
            //dr["txtItemCode"] = string.Empty;
            //dr["txtItemName"] = string.Empty;
            dr["ddlNamaBarang"] = 0;
            dr["txtItemQty"] = string.Empty;
            dr["txtItemBox"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["Curtbl"] = dt;
            rptBrgOlahan.DataSource = dt;
            rptBrgOlahan.DataBind();

        }

        protected void bckBtn_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
                Response.Redirect((string)refUrl);
        }

        protected void ddlNamaGudang_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowIndex = 0;
            if (ViewState["Curtbl"] != null)
            {
                DataTable dt = (DataTable)ViewState["Curtbl"];
                //BindDDLBarang(dt.Rows.Count);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataTable checkData = (DataTable)ViewState["Curtbl"];
                        //TextBox txtItemCode = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemCode");
                        //TextBox txtItemName = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemName");
                        if (checkData.Rows.Count == dt.Rows.Count)
                        {
                            DropDownList ddlNamaBarang = (DropDownList)rptBrgOlahan.Items[rowIndex].FindControl("ddlNamaBarang");
                            TextBox txtItemQty = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemQty");
                            TextBox txtItemBox = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemBox");
                            //txtItemCode.Text = (Convert.ToInt32(GetItemCode()) + i).ToString();
                            //txtItemName.Text = dt.Rows[i]["txtItemName"].ToString();
                            BindDDLBarang(i, 0);
                            txtItemQty.Text = dt.Rows[i]["txtItemQty"].ToString();
                            txtItemBox.Text = dt.Rows[i]["txtItemBox"].ToString();
                            rowIndex++;
                        }
                    }
                }
            }
        }

        protected void ddlSupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowIndex = 0;
            if (ViewState["Curtbl"] != null)
            {
                DataTable dt = (DataTable)ViewState["Curtbl"];
                //BindDDLBarang(dt.Rows.Count);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataTable checkData = (DataTable)ViewState["Curtbl"];
                        //TextBox txtItemCode = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemCode");
                        //TextBox txtItemName = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemName");
                        if (checkData.Rows.Count == dt.Rows.Count)
                        {
                            DropDownList ddlNamaBarang = (DropDownList)rptBrgOlahan.Items[rowIndex].FindControl("ddlNamaBarang");
                            TextBox txtItemQty = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemQty");
                            TextBox txtItemBox = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemBox");
                            //txtItemCode.Text = (Convert.ToInt32(GetItemCode()) + i).ToString();
                            //txtItemName.Text = dt.Rows[i]["txtItemName"].ToString();
                            BindDDLBarang(i, 0);
                            txtItemQty.Text = dt.Rows[i]["txtItemQty"].ToString();
                            txtItemBox.Text = dt.Rows[i]["txtItemBox"].ToString();
                            rowIndex++;
                        }
                    }
                }
            }
        }

        private void SetOldData()
        {
            int rowIndex = 0;
            if (ViewState["Curtbl"] != null)
            {
                DataTable dt = (DataTable)ViewState["Curtbl"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //TextBox txtItemCode = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemCode");
                        //TextBox txtItemName = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemName");
                        DropDownList ddlNamaBarang = (DropDownList)rptBrgOlahan.Items[rowIndex].FindControl("ddlNamaBarang");
                        TextBox txtItemQty = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemQty");
                        TextBox txtItemBox = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemBox");
                        //txtItemCode.Text = dt.Rows[i]["txtItemCode"].ToString();
                        //txtItemName.Text = dt.Rows[i]["txtItemName"].ToString();
                        if (dt.Rows[i]["ddlNamaBarang"].ToString().Trim() == "")
                        {
                            BindDDLBarang(i, 0);
                        }
                        else
                        {
                            BindDDLBarang(i, (Convert.ToInt32(dt.Rows[i]["ddlNamaBarang"].ToString())));
                        }
                        txtItemQty.Text = dt.Rows[i]["txtItemQty"].ToString();
                        txtItemBox.Text = dt.Rows[i]["txtItemBox"].ToString();
                        rowIndex++;
                    }
                }
            }
        }
        protected void btnadd_Click(object sender, EventArgs e)
        {
            DropDownList ddlNamaBarangCheck = (DropDownList)rptBrgOlahan.Items[0].FindControl("ddlNamaBarang");
            LinkButton btn = (sender as LinkButton);
            RepeaterItem itemCheck = btn.NamingContainer as RepeaterItem;
            int indexCheck = itemCheck.ItemIndex;
            TextBox txtItemQtyCheck = (TextBox)itemCheck.FindControl("txtItemQty");
            TextBox txtItemBoxCheck = (TextBox)itemCheck.FindControl("txtItemBox");
            DropDownList ddlNamaBarangRpt = (DropDownList)itemCheck.FindControl("ddlNamaBarang");
            if (((DropDownList)rptBrgOlahan.Items[indexCheck].FindControl("ddlNamaBarang")).Enabled == true)
            {
                if (txtItemQtyCheck.Text.Trim() != "" && txtItemQtyCheck.Text.Trim() != "0" && txtItemBoxCheck.Text.Trim() != "" && txtItemBoxCheck.Text.Trim() != "0")
                {
                    if (checkQtyItem(ddlNamaBarangRpt.SelectedValue, Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()), Convert.ToInt32(ddlSupName.SelectedValue.Trim()), decimal.Parse(txtItemQtyCheck.Text.Trim())))
                    {
                        if (checkBoxItem(ddlNamaBarangRpt.SelectedValue, Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()), Convert.ToInt32(ddlSupName.SelectedValue.Trim()), Convert.ToInt32(txtItemBoxCheck.Text.Trim())))
                        {
                            if (ddlNamaBarangCheck.Items.Count > rptBrgOlahan.Items.Count)
                            {
                                int rowIndex = 0;

                                if (ViewState["Curtbl"] != null)
                                {
                                    DataTable dt = (DataTable)ViewState["Curtbl"];
                                    DataRow drCurrentRow = null;
                                    if (dt.Rows.Count > 0)
                                    {
                                        for (int i = 1; i <= dt.Rows.Count; i++)
                                        {
                                            //TextBox txtItemCode = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemCode");
                                            //TextBox txtItemName = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemName");
                                            DropDownList ddlNamaBarang = (DropDownList)rptBrgOlahan.Items[rowIndex].FindControl("ddlNamaBarang");
                                            TextBox txtItemQty = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemQty");
                                            TextBox txtItemBox = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemBox");
                                            drCurrentRow = dt.NewRow();
                                            //dt.Rows[i - 1]["txtItemCode"] = txtItemCode.Text;
                                            //dt.Rows[i - 1]["txtItemName"] = txtItemName.Text;
                                            dt.Rows[i - 1]["ddlNamaBarang"] = ddlNamaBarang.SelectedIndex.ToString();
                                            dt.Rows[i - 1]["txtItemQty"] = txtItemQty.Text;
                                            dt.Rows[i - 1]["txtItemBox"] = txtItemBox.Text;
                                            rowIndex++;
                                        }
                                        dt.Rows.Add(drCurrentRow);
                                        ViewState["Curtbl"] = dt;
                                        rptBrgOlahan.DataSource = dt;
                                        rptBrgOlahan.DataBind();
                                    }
                                }
                                else
                                {
                                    Response.Write("ViewState Value is Null");
                                }
                                SetOldData();
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Jumlah Barang Sudah Tidak Bisa Dipilih!" + "');", true);
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Box Melebihi Stock yang ada di Gudang!" + "');", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Stock yang ada di Gudang!" + "');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Seluruh Data Harus Terpenuhi Dahulu!" + "');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Data Kosong Tidak Dapat Menambah Item!" + "');", true);
            }
        }

        private bool checkBoxItem(string namaBrg, int idInventory, int idSupplier, int JmlBox)
        {
            bool setBoxItem = false;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT BarangJmlBox FROM Barang WHERE IdInventory = @IdInventory AND IdSupplier = @IdSupplier AND NamaBarang = @NamaBarang", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", idInventory);
                commNama.Parameters.AddWithValue("@IdSupplier", idSupplier);
                commNama.Parameters.AddWithValue("@NamaBarang", namaBrg);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        var tmpJmlBox = (int)dr["BarangJmlBox"];
                        if (JmlBox <= tmpJmlBox)
                        {
                            setBoxItem = true;
                            return setBoxItem;
                        }
                        else
                        {
                            return setBoxItem;
                        }
                    }
                    else
                    {
                        return setBoxItem;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        private bool checkQtyItem(string namaBrg, int idInventory, int idSupplier, decimal qtyBrg)
        {
            bool setBoxItem = false;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT BarangQuantity FROM Barang WHERE IdInventory = @IdInventory AND IdSupplier = @IdSupplier AND NamaBarang = @NamaBarang", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", idInventory);
                commNama.Parameters.AddWithValue("@IdSupplier", idSupplier);
                commNama.Parameters.AddWithValue("@NamaBarang", namaBrg);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        var tmpQtyBox = (decimal)dr["BarangQuantity"];
                        if (qtyBrg <= tmpQtyBox)
                        {
                            setBoxItem = true;
                            return setBoxItem;
                        }
                        else
                        {
                            return setBoxItem;
                        }
                    }
                    else
                    {
                        return setBoxItem;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void btndel_Click(object sender, EventArgs e)
        {
            RepeaterItem item = (sender as LinkButton).NamingContainer as RepeaterItem;
            LinkButton lnkButton = sender as LinkButton;
            int RepeaterItemIndex = ((RepeaterItem)lnkButton.NamingContainer).ItemIndex;
            TextBox txtItemQtyCheck = (TextBox)rptBrgOlahan.Items[RepeaterItemIndex - 1].FindControl("txtItemQty");
            TextBox txtItemBoxCheck = (TextBox)rptBrgOlahan.Items[RepeaterItemIndex - 1].FindControl("txtItemBox");
            DropDownList ddlNamaBarangRpt = (DropDownList)rptBrgOlahan.Items[RepeaterItemIndex - 1].FindControl("ddlNamaBarang");
            int rowIndex = 0;
            int rowID = item.ItemIndex;
            if (ViewState["Curtbl"] != null)
            {
                DataTable dt = (DataTable)ViewState["Curtbl"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 1; i <= rptBrgOlahan.Items.Count; i++)
                    {
                        //TextBox txtItemCode = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemCode");
                        //TextBox txtItemName = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemName");
                        DropDownList ddlNamaBarang = (DropDownList)rptBrgOlahan.Items[rowIndex].FindControl("ddlNamaBarang");
                        TextBox txtItemQty = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemQty");
                        TextBox txtItemBox = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemBox");
                        //dt.Rows[i - 1]["txtItemCode"] = txtItemCode.Text;
                        //dt.Rows[i - 1]["txtItemName"] = txtItemName.Text;
                        dt.Rows[i - 1]["ddlNamaBarang"] = ddlNamaBarang.SelectedIndex.ToString();
                        dt.Rows[i - 1]["txtItemQty"] = txtItemQty.Text;
                        dt.Rows[i - 1]["txtItemBox"] = txtItemBox.Text;
                        rowIndex++;
                    }
                    if (dt.Rows.Count - 1 != 0)
                    {
                        if (dt.Rows.Count - 1 <= item.ItemIndex)
                        {
                            dt.Rows.Remove(dt.Rows[rowID]);
                        }
                    }
                }

                ViewState["Curtbl"] = dt;
                rptBrgOlahan.DataSource = dt;
                rptBrgOlahan.DataBind();
            }

            SetOldData();
        }

        protected void txtItemQty_TextChanged(object sender, EventArgs e)
        {
            TextBox tb1 = ((TextBox)(sender));

            RepeaterItem rp1 = ((RepeaterItem)(tb1.NamingContainer));

            TextBox tb3 = (TextBox)rp1.FindControl("txtPrice");
            DropDownList tb4 = (DropDownList)rp1.FindControl("ddlNamaBarang");
            if (tb1.Text.Trim() != "")
            {
                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                if (!checkQtyItem(tb4.SelectedValue, Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()), Convert.ToInt32(ddlSupName.SelectedValue.Trim()), decimal.Parse(txtQtyItmStr)))
                {
                    tb1.Text = "0";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Stock yang ada di Gudang!" + "');", true);
                }
            }
            else
            {
                tb1.Text = "0";
            }
        }

        protected void txtItemBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb1 = ((TextBox)(sender));
            RepeaterItem rp1 = ((RepeaterItem)(tb1.NamingContainer));
            DropDownList tb2 = (DropDownList)rp1.FindControl("ddlNamaBarang");
            if (tb1.Text.Trim() != "")
            {
                if (!checkBoxItem(tb2.SelectedValue, Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()), Convert.ToInt32(ddlSupName.SelectedValue.Trim()), Convert.ToInt32(tb1.Text.Trim())))
                {
                    tb1.Text = "0";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Box Melebihi Stock yang ada di Gudang!" + "');", true);
                }
            }
            else
            {
                tb1.Text = "0";
            }
        }

        protected bool checkControlDetail()
        {
            int rowIndex = 0;
            int checkDtl = 0;
            DataTable dt = (DataTable)ViewState["Curtbl"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DropDownList ddlNamaBarang = (DropDownList)rptBrgOlahan.Items[rowIndex].FindControl("ddlNamaBarang");
                    TextBox txtItemQty = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemQty");
                    TextBox txtItemBox = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemBox");
                    if (txtItemQty.Text.Trim() != "0" && txtItemBox.Text.Trim() != "0")
                    {
                        checkDtl = 1;
                    }
                    else
                    {
                        checkDtl = 0;
                        break;
                    }
                    rowIndex++;
                }
                if (checkDtl == 1)
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

        protected bool checkNamaBrg(string namaBrg)
        {
            bool isResult = false;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE NamaBarang = '"+namaBrg+"'", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return isResult;
                    }
                    else
                    {
                        isResult = true;
                        return isResult;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected decimal getQtyUpdate(int id)
        {
            decimal isResult = 0;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT BarangQuantity FROM Barang WHERE IdBarang = '" + id + "'", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        isResult = (decimal)dr["BarangQuantity"];
                        return isResult;
                    }
                    else
                    {
                        return isResult;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected int getJmlBox(int id)
        {
            int isResult = 0;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT BarangJmlBox FROM Barang WHERE IdBarang = '" + id + "'", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        isResult = (int)dr["BarangJmlBox"];
                        return isResult;
                    }
                    else
                    {
                        return isResult;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (lblStatusPage.Text.Trim() != "Update Barang Olahan" && lblHeaderPage.Text.Trim() != "Update Barang Olahan")
            {
                DropDownList ddlNamaBarangCheck = (DropDownList)rptBrgOlahan.Items[0].FindControl("ddlNamaBarang");
                brgObj = new Barang();
                lstBrg = new List<Barang>();
                int counterJmlBox = 0;
                decimal counterQty = 0;
                if (ddlNamaBarangCheck.Enabled == true)
                {
                    int rowIndex = 0;
                    if (ViewState["Curtbl"] != null)
                    {
                        DataTable dt = (DataTable)ViewState["Curtbl"];
                        if (checkControlDetail())
                        {
                            if (checkNamaBrg(txtNamaBrgOlahan.Text.ToUpper().Trim()))
                            {
                                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DropDownList ddlNamaBarang = (DropDownList)rptBrgOlahan.Items[rowIndex].FindControl("ddlNamaBarang");
                                    TextBox txtItemQty = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemQty");
                                    TextBox txtItemBox = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemBox");
                                    sqlConnection = new SqlConnection(connectionString);
                                    try
                                    {
                                        sqlConnection.Open();
                                        SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE NamaBarang = @NamaBarang AND IdSupplier = @IdSupplier AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                        commNama.Parameters.AddWithValue("@NamaBarang", ddlNamaBarang.SelectedValue.Trim());
                                        commNama.Parameters.AddWithValue("@IdSupplier", Convert.ToInt32(ddlSupName.SelectedValue.Trim()));
                                        commNama.Parameters.AddWithValue("@IdInventory", Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()));
                                        using (SqlDataReader dr = commNama.ExecuteReader())
                                        {
                                            if (dr.Read())
                                            {
                                                double txtQtyItmVal = Convert.ToDouble(txtItemQty.Text.Trim());
                                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                                Barang brgNewObj = new Barang();
                                                brgNewObj.IdBarang = (int)dr["IdBarang"];
                                                brgNewObj.NamaBarang = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                                brgNewObj.IdInventory = Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()); //harus 2 angka dibelakang koma
                                                brgNewObj.IdSupplier = Convert.ToInt32(ddlSupName.SelectedValue.Trim());
                                                brgNewObj.BarangJmlBox = Convert.ToInt32(txtItemBox.Text.Trim());
                                                brgNewObj.BarangQty = Convert.ToDecimal(txtQtyItmStr); //harus 2 angka dibelakang koma
                                                lstBrg.Add(brgNewObj);
                                                counterJmlBox += brgNewObj.BarangJmlBox;
                                                counterQty += brgNewObj.BarangQty;
                                                //tambah proses ngecek kapasitas gudang
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    { throw ex; }
                                    finally { sqlConnection.Close(); sqlConnection.Dispose(); }
                                    rowIndex++;
                                }
                                if (!lstBrg.GroupBy(a => a.NamaBarang).Any(c => c.Count() > 1))
                                {
                                    sqlConnection = new SqlConnection(connectionString);
                                    try
                                    {
                                        Barang brgCounter = new Barang();
                                        sqlConnection.Open();
                                        for (int i = 0; i < lstBrg.Count; i++)
                                        {
                                            SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang AND IdSupplier = @IdSupplier AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                            commNama.Parameters.AddWithValue("@IdBarang", lstBrg[i].IdBarang);
                                            commNama.Parameters.AddWithValue("@IdSupplier", lstBrg[i].IdSupplier);
                                            commNama.Parameters.AddWithValue("@IdInventory", lstBrg[i].IdInventory);
                                            SqlDataReader dr = commNama.ExecuteReader();
                                            if (dr.Read())
                                            {
                                                brgCounter.IdBarang = (int)dr["IdBarang"];
                                                brgCounter.NamaBarang = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                                brgCounter.BarangQty = (decimal)dr["BarangQuantity"];
                                                brgCounter.BarangCreatedBy = dr.GetString(dr.GetOrdinal("BarangCreatedBy"));
                                                brgCounter.BarangCreatedDate = dr.GetString(dr.GetOrdinal("BarangCreatedDate"));
                                                brgCounter.BarangUpdateBy = dr.GetString(dr.GetOrdinal("BarangUpdateBy"));
                                                brgCounter.BarangUpdateDate = dr.GetString(dr.GetOrdinal("BarangUpdateDate"));
                                                //brgCounter.BarangHargaJual = (decimal)dr["BarangHargaJual"];
                                                brgCounter.BarangSatuan = dr.GetString(dr.GetOrdinal("BarangSatuan"));
                                                brgCounter.IdSupplier = (int)dr["IdSupplier"];
                                                brgCounter.BarangStatus = dr.GetString(dr.GetOrdinal("BarangStatus"));
                                                //brgCounter.BarangTglBeli = dr.GetString(dr.GetOrdinal("BarangTanggalBeli"));
                                                brgCounter.IdInventory = (int)dr["IdInventory"];
                                                brgCounter.BarangJmlBox = (int)dr["BarangJmlBox"];
                                                brgCounter.BarangItemCode = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                                                dr.Close();
                                                dr.Dispose();
                                            }
                                            SqlCommand strQuerEditBarang = new SqlCommand("update Barang set BarangQuantity=@BarangQuantity,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate,BarangJmlBox=@BarangJmlBox where IdBarang=@IdBarang AND IdInventory=@IdInventory AND IdSupplier=@IdSupplier", sqlConnection);
                                            strQuerEditBarang.Parameters.AddWithValue("@IdBarang", lstBrg[i].IdBarang);
                                            strQuerEditBarang.Parameters.AddWithValue("@IdInventory", lstBrg[i].IdInventory);
                                            strQuerEditBarang.Parameters.AddWithValue("@IdSupplier", lstBrg[i].IdSupplier);
                                            strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                            strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                            strQuerEditBarang.Parameters.AddWithValue("@BarangQuantity", brgCounter.BarangQty - lstBrg[i].BarangQty);
                                            strQuerEditBarang.Parameters.AddWithValue("@BarangJmlBox", brgCounter.BarangJmlBox - lstBrg[i].BarangJmlBox);
                                            strQuerEditBarang.ExecuteNonQuery();
                                        }
                                        string valQtyBrg = string.Format("{0:0.00}", counterQty);
                                        SqlCommand cmdTambahBrg = new SqlCommand("insert into Barang" + "(NamaBarang,BarangQuantity,BarangTanggalBeli,BarangCreatedBy,BarangCreatedDate,BarangUpdateBy,BarangUpdateDate,BarangSatuan,IdSupplier,BarangStatus,IdInventory,BarangJmlBox,BarangItemCode,BarangCategory) values " + "('" + txtNamaBrgOlahan.Text.Trim().ToUpper() + "'," + valQtyBrg.Replace(",", ".") + ",'" + DateTime.Now.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','KG','" + ddlSupName.SelectedValue.Trim() + "','ACTIVE','" + ddlNamaGudang.SelectedValue.Trim() + "'," + counterJmlBox + ",'" + txtItemCode.Text.Trim() + "','OLAHAN')", sqlConnection);
                                        cmdTambahBrg.ExecuteNonQuery();
                                        sqlConnection.Close();
                                        sqlConnection.Dispose();
                                        insertToBrgOlahan(txtItemCode.Text.Trim(), lstBrg);
                                        Response.Redirect("ItemPage.aspx", false);
                                    }
                                    catch (Exception ex)
                                    { throw ex; }
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nama Barang Tidak Boleh Barang Yang Sama Dalam 1 Campuran!" + "');", true);
                                }
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nama Barang Olahan Sudah Ada di Database!" + "');", true);
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Seluruh Data Harus Terpenuhi Dahulu!" + "');", true);
                        }
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Data Kosong Tidak Dapat Menambah Item!" + "');", true);
                }
            }
            else
            {
                int id;
                int.TryParse(Request.QueryString["id"], out id);
                DropDownList ddlNamaBarangCheck = (DropDownList)rptBrgOlahan.Items[0].FindControl("ddlNamaBarang");
                brgObj = new Barang();
                lstBrg = new List<Barang>();
                int counterJmlBox = 0;
                decimal counterQty = 0;
                int rowIndex = 0;
                if (ViewState["Curtbl"] != null)
                {
                    DataTable dt = (DataTable)ViewState["Curtbl"];
                    if (checkControlDetail())
                    {
                        connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddlNamaBarang = (DropDownList)rptBrgOlahan.Items[rowIndex].FindControl("ddlNamaBarang");
                            TextBox txtItemQty = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemQty");
                            TextBox txtItemBox = (TextBox)rptBrgOlahan.Items[rowIndex].FindControl("txtItemBox");
                            sqlConnection = new SqlConnection(connectionString);
                            try
                            {
                                sqlConnection.Open();
                                SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE NamaBarang = @NamaBarang AND IdSupplier = @IdSupplier AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                commNama.Parameters.AddWithValue("@NamaBarang", ddlNamaBarang.SelectedValue.Trim());
                                commNama.Parameters.AddWithValue("@IdSupplier", Convert.ToInt32(ddlSupName.SelectedValue.Trim()));
                                commNama.Parameters.AddWithValue("@IdInventory", Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()));
                                using (SqlDataReader dr = commNama.ExecuteReader())
                                {
                                    if (dr.Read())
                                    {
                                        double txtQtyItmVal = Convert.ToDouble(txtItemQty.Text.Trim());
                                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                        Barang brgNewObj = new Barang();
                                        brgNewObj.IdBarang = (int)dr["IdBarang"];
                                        brgNewObj.NamaBarang = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                        brgNewObj.IdInventory = Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()); //harus 2 angka dibelakang koma
                                        brgNewObj.IdSupplier = Convert.ToInt32(ddlSupName.SelectedValue.Trim());
                                        brgNewObj.BarangJmlBox = Convert.ToInt32(txtItemBox.Text.Trim());
                                        brgNewObj.BarangQty = Convert.ToDecimal(txtQtyItmStr); //harus 2 angka dibelakang koma
                                        lstBrg.Add(brgNewObj);
                                        counterJmlBox += brgNewObj.BarangJmlBox;
                                        counterQty += brgNewObj.BarangQty;
                                        //tambah proses ngecek kapasitas gudang
                                    }
                                }
                            }
                            catch (Exception ex)
                            { throw ex; }
                            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
                            rowIndex++;
                        }
                        if (!lstBrg.GroupBy(a => a.NamaBarang).Any(c => c.Count() > 1))
                        {
                            try
                            {
                                Barang brgCounter = new Barang();
                                for (int i = 0; i < lstBrg.Count; i++)
                                {
                                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                                    sqlConnection = new SqlConnection(connectionString);
                                    sqlConnection.Open();
                                    SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang AND IdSupplier = @IdSupplier AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                    commNama.Parameters.AddWithValue("@IdBarang", lstBrg[i].IdBarang);
                                    commNama.Parameters.AddWithValue("@IdSupplier", lstBrg[i].IdSupplier);
                                    commNama.Parameters.AddWithValue("@IdInventory", lstBrg[i].IdInventory);
                                    SqlDataReader dr = commNama.ExecuteReader();
                                    if (dr.Read())
                                    {
                                        brgCounter.IdBarang = (int)dr["IdBarang"];
                                        brgCounter.NamaBarang = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                        brgCounter.BarangQty = (decimal)dr["BarangQuantity"];
                                        brgCounter.BarangCreatedBy = dr.GetString(dr.GetOrdinal("BarangCreatedBy"));
                                        brgCounter.BarangCreatedDate = dr.GetString(dr.GetOrdinal("BarangCreatedDate"));
                                        brgCounter.BarangUpdateBy = dr.GetString(dr.GetOrdinal("BarangUpdateBy"));
                                        brgCounter.BarangUpdateDate = dr.GetString(dr.GetOrdinal("BarangUpdateDate"));
                                        //brgCounter.BarangHargaJual = (decimal)dr["BarangHargaJual"];
                                        brgCounter.BarangSatuan = dr.GetString(dr.GetOrdinal("BarangSatuan"));
                                        brgCounter.IdSupplier = (int)dr["IdSupplier"];
                                        brgCounter.BarangStatus = dr.GetString(dr.GetOrdinal("BarangStatus"));
                                        //brgCounter.BarangTglBeli = dr.GetString(dr.GetOrdinal("BarangTanggalBeli"));
                                        brgCounter.IdInventory = (int)dr["IdInventory"];
                                        brgCounter.BarangJmlBox = (int)dr["BarangJmlBox"];
                                        brgCounter.BarangItemCode = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                                        dr.Close();
                                        dr.Dispose();
                                    }
                                    SqlCommand strQuerEditBarang = new SqlCommand("update Barang set BarangQuantity=@BarangQuantity,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate,BarangJmlBox=@BarangJmlBox where IdBarang=@IdBarang AND IdInventory=@IdInventory AND IdSupplier=@IdSupplier", sqlConnection);
                                    strQuerEditBarang.Parameters.AddWithValue("@IdBarang", lstBrg[i].IdBarang);
                                    strQuerEditBarang.Parameters.AddWithValue("@IdInventory", lstBrg[i].IdInventory);
                                    strQuerEditBarang.Parameters.AddWithValue("@IdSupplier", lstBrg[i].IdSupplier);
                                    strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                    strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                    strQuerEditBarang.Parameters.AddWithValue("@BarangQuantity", brgCounter.BarangQty - lstBrg[i].BarangQty);
                                    strQuerEditBarang.Parameters.AddWithValue("@BarangJmlBox", brgCounter.BarangJmlBox - lstBrg[i].BarangJmlBox);
                                    strQuerEditBarang.ExecuteNonQuery();
                                    //updateToBrgOlahan(txtItemCode.Text.Trim(), lstBrg);
                                }
                                string valQtyBrg = string.Format("{0:0.00}", counterQty + getQtyUpdate(id));
                                int valJmlBoxBrg = counterJmlBox + getJmlBox(id);
                                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                                sqlConnection = new SqlConnection(connectionString);
                                sqlConnection.Open();
                                SqlCommand cmdTambahBrg = new SqlCommand("update Barang set BarangQuantity=@BarangQuantity,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate,BarangJmlBox=@BarangJmlBox where IdBarang=@IdBarang AND IdInventory=@IdInventory AND IdSupplier=@IdSupplier", sqlConnection);
                                cmdTambahBrg.Parameters.AddWithValue("@BarangQuantity", valQtyBrg.Replace(",", "."));
                                cmdTambahBrg.Parameters.AddWithValue("@IdBarang", id);
                                cmdTambahBrg.Parameters.AddWithValue("@IdInventory", Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()));
                                cmdTambahBrg.Parameters.AddWithValue("@IdSupplier", Convert.ToInt32(ddlSupName.SelectedValue.Trim()));
                                cmdTambahBrg.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                cmdTambahBrg.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                cmdTambahBrg.Parameters.AddWithValue("@BarangJmlBox", valJmlBoxBrg);
                                cmdTambahBrg.ExecuteNonQuery();
                                sqlConnection.Close();
                                sqlConnection.Dispose();
                                updateToBrgOlahan(txtItemCode.Text.Trim(), lstBrg);
                                Response.Redirect("ItemPage.aspx", false);
                            }
                            catch (Exception ex)
                            { throw ex; }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nama Barang Tidak Boleh Barang Yang Sama Dalam 1 Campuran!" + "');", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Seluruh Data Harus Terpenuhi Dahulu!" + "');", true);
                    }
                }
            }
        }

        protected void insertToBrgOlahan(string itmCode, List<Barang>myListBrg)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT IdBarang FROM Barang WHERE BarangItemCode=@BarangItemCode AND BarangStatus = 'ACTIVE' ", sqlConnection);
                commNama.Parameters.AddWithValue("@BarangItemCode", itmCode);
                SqlDataReader dr = commNama.ExecuteReader();
                if (dr.Read())
                {
                    var idBrg = (int)dr["IdBarang"];
                    for(int i = 0;i < myListBrg.Count;i++)
                    {
                        SqlCommand strInsertBrgOlahan = new SqlCommand("insert into BarangOlahan" + "(IdBrgOlahan,IdBarang,QtyBarangOlahan,JmlBoxBarangOlahan,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate) values " + "(" + idBrg + "," + myListBrg[i].IdBarang + "," + myListBrg[i].BarangQty.ToString().Replace(",", ".") + "," + myListBrg[i].BarangJmlBox + ",'" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "')", sqlConnection);
                        strInsertBrgOlahan.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void updateToBrgOlahan(string itmCode, List<Barang>myListBrg)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT IdBarang FROM Barang WHERE BarangItemCode=@BarangItemCode AND BarangStatus = 'ACTIVE' ", sqlConnection);
                commNama.Parameters.AddWithValue("@BarangItemCode", itmCode);
                SqlDataReader dr = commNama.ExecuteReader();
                if (dr.Read())
                {
                    var idBrg = (int)dr["IdBarang"];
                    for (int i = 0; i < myListBrg.Count; i++)
                    {
                        SqlCommand slcOlahan = new SqlCommand("SELECT * FROM BarangOlahan WHERE IdBarang=@IdBarang", sqlConnection);
                        slcOlahan.Parameters.AddWithValue("@IdBarang", myListBrg[i].IdBarang);
                        SqlDataReader drOlahan = slcOlahan.ExecuteReader();
                        if (drOlahan.Read())
                        {
                            var qtyBrg = (decimal)drOlahan["QtyBarangOlahan"];
                            var jmlBrg = (int)drOlahan["JmlBoxBarangOlahan"];
                            string qtyValStr = string.Format("{0:0.00}", myListBrg[i].BarangQty + qtyBrg);
                            string jmlValStr = Convert.ToString(myListBrg[i].BarangJmlBox + jmlBrg);
                            SqlCommand strInsertBrgOlahan = new SqlCommand("update BarangOlahan set QtyBarangOlahan=@QtyBarangOlahan,JmlBoxBarangOlahan=@JmlBoxBarangOlahan,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate where IdBarang=@IdBarang", sqlConnection);
                            strInsertBrgOlahan.Parameters.AddWithValue("@QtyBarangOlahan", qtyValStr.Replace(",", "."));
                            strInsertBrgOlahan.Parameters.AddWithValue("@IdBarang", myListBrg[i].IdBarang);
                            strInsertBrgOlahan.Parameters.AddWithValue("@UpdatedBy", ((Role)Session["Role"]).EmailEmp);
                            strInsertBrgOlahan.Parameters.AddWithValue("@UpdatedDate", DateTime.Now.Date.ToString());
                            strInsertBrgOlahan.Parameters.AddWithValue("@JmlBoxBarangOlahan", jmlValStr);
                            strInsertBrgOlahan.ExecuteNonQuery();
                            drOlahan.Close();
                            drOlahan.Dispose();
                        }
                    }
                    dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
    }
}