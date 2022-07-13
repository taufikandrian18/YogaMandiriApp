using System;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Globalization;
using Hangfire;
using Hangfire.SqlServer;

namespace TestCoba
{
    public partial class AddPembelianPage : System.Web.UI.Page
    {
        private BackgroundJobServer _backgroundJobServer;
        PembelianMst pmbObj;
        List<PembelianDtl> pmDtlList;
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
                    string editText = "";
                    if(Request.QueryString.AllKeys.Count() > 1)
                    {
                        editText = Request.QueryString["status"];
                    }
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand commNama = new SqlCommand("SELECT * FROM POPembelianMst WHERE IdPOPembelianMst = @IdPOPembelianMst ", sqlConnection);
                        commNama.Parameters.AddWithValue("@IdPOPembelianMst", id);
                        using (SqlDataReader dr = commNama.ExecuteReader())
                        {
                            string sup = "Select * from Supplier WHERE SupplierStatus = 'ACTIVE'";
                            string com = "Select * from Inventory WHERE InvStatus = 'ACTIVE'";
                            SqlDataAdapter supAdpt = new SqlDataAdapter(sup, sqlConnection);
                            SqlDataAdapter adpt = new SqlDataAdapter(com, sqlConnection);
                            DataTable dtSup = new DataTable();
                            DataTable dt = new DataTable();
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
                                pmbObj = new PembelianMst();
                                pmbObj.IdPembelian = id;
                                var supId = (int)dr["IdSupplier"];
                                pmbObj.IdSupplier = supId;
                                var strg = (int)dr["IdInventory"];
                                pmbObj.IdInventory = strg;
                                var GTotal = (decimal)dr["TotalTagihanPembelian"];
                                pmbObj.TotalTagihanPembelian = GTotal;
                                DateTime tglBeliStr = DateTime.ParseExact(dr.GetString(dr.GetOrdinal("TanggalPOPembelian")), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                DateTime tglJthTmpStr = DateTime.ParseExact(dr.GetString(dr.GetOrdinal("TanggalPOJatuhTmpPembelian")), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                txtNoInvoice.Text = dr.GetString(dr.GetOrdinal("NoInvoicePembelian"));
                                pmbObj.NoInvoicePembelian = dr.GetString(dr.GetOrdinal("NoInvoicePembelian"));
                                ddlSupName.SelectedValue = supId.ToString();
                                txtCldBrgBeli.Text = tglBeliStr.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                pmbObj.TglPembelian = dr.GetString(dr.GetOrdinal("TanggalPOPembelian"));
                                ddlJenisTagihan.SelectedValue = dr.GetString(dr.GetOrdinal("JenisTagihan"));
                                pmbObj.JenisTagihan = dr.GetString(dr.GetOrdinal("JenisTagihan"));
                                txtCldJtuhTempo.Text = tglJthTmpStr.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                pmbObj.TglJatuhTmpPembelian = dr.GetString(dr.GetOrdinal("TanggalPOJatuhTmpPembelian"));
                                txtBrgNameBank.Text = dr.GetString(dr.GetOrdinal("NoRekeningPembayaran"));
                                pmbObj.NoRekPembayaran = dr.GetString(dr.GetOrdinal("NoRekeningPembayaran"));
                                ddlNamaGudang.SelectedValue = strg.ToString();
                                txtGrandTotal.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotal);
                                if (editText.Trim() != "")
                                {
                                    lblHeaderPage.Text = "Edit Pembelian";
                                    lblStatusPage.Text = "Edit Pembelian";
                                }
                                else
                                {
                                    lblHeaderPage.Text = "Retur Pembelian";
                                    lblStatusPage.Text = "Retur Pembelian";
                                    ddlJenisTagihan.Enabled = false;
                                    txtCldJtuhTempo.Enabled = false;
                                    txtBrgNameBank.Enabled = false;
                                    ddlNamaGudang.Enabled = false;
                                    txtCldBrgBeli.Enabled = false;
                                    ddlSupName.Enabled = false;
                                    txtNoInvoice.Enabled = false;
                                    txtGrandTotal.Enabled = false;
                                }
                                txtSisaKapasitasGudang.Text = GetSisaKapasitasInv(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()));
                                getDtlPembelian(id,editText.Trim());
                            }
                            else
                            {
                                txtSisaKapasitasGudang.Text = GetSisaKapasitasInv(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()));
                                BindRepeater();
                                BindDDLBarang(0, 0);
                            }
                        }
                        //BindDDLBarang(0);
                        //if (GetItemCode().Trim() == "")
                        //{
                        //   TextBox txtItemCode = (TextBox)rptPembelianDtl.Items[0].FindControl("txtItemCode");
                        //   txtItemCode.Text = "100676";
                        //}
                        //else
                        //{
                        //    TextBox txtItemCode = (TextBox)rptPembelianDtl.Items[0].FindControl("txtItemCode");
                        //    txtItemCode.Text = GetItemCode();
                        //}
                        
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
        protected void getDtlPembelian(int id, string qsText)
        {
            DataTable counter = new DataTable();
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT * FROM POPembelianDtl WHERE IdPOPembelianMst = @IdPOPembelianMst", sqlConnection);
                commNama.Parameters.AddWithValue("@IdPOPembelianMst", id);
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
                        pmbDtl.Columns.Add("txtPrice", typeof(string));
                        pmbDtl.Columns.Add("txtItemBox", typeof(string));
                        pmbDtl.Columns.Add("txtTotalPrice", typeof(string));
                        for (int i = 0; i < counter.Rows.Count; i++)
                        {
                            DataRow drPmbDtl = pmbDtl.NewRow();
                            drPmbDtl["ddlNamaBarang"] = dtRowList[i]["IdBarang"];
                            drPmbDtl["txtItemQty"] = dtRowList[i]["QtyBarangPOPembelianDtl"];
                            drPmbDtl["txtPrice"] = Convert.ToInt32(dtRowList[i]["HargaPOPembelianDtl"]).ToString();
                            drPmbDtl["txtItemBox"] = dtRowList[i]["JumlahBoxPOPembelianDtl"];
                            drPmbDtl["txtTotalPrice"] = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", dtRowList[i]["TotalHargaPOPembelianDtl"]);
                            pmbDtl.Rows.Add(drPmbDtl);
                        }
                        ViewState["Curtbl"] = pmbDtl;
                        ViewState["Rtrtbl"] = pmbDtl;
                        //ViewState["ReturTbl"] = pmbDtl;
                        rptPembelianDtl.DataSource = pmbDtl;
                        rptPembelianDtl.DataBind();
                    }
                }
                SetOldDataUpdate(id);
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected void BindDDLBarangUpdate(int numRow, int slctdIndx,int id)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                if (lblStatusPage.Text.Trim() != "Edit Pembelian" && lblHeaderPage.Text.Trim() != "Edit Pembelian")
                {
                    sqlConnection.Open();
                    DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[numRow].FindControl("ddlNamaBarang");
                    SqlCommand commNama = new SqlCommand("SELECT a.IdBarang as ID, a.NamaBarang + '-(' + b.SupplierName + ')' as TEXT FROM Barang a INNER JOIN Supplier b on a.IdSupplier = b.IdSupplier INNER JOIN POPembelianDtl c on c.IdBarang = a.IdBarang  WHERE IdPOPembelianMst=@IdPOPembelianMst AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE'", sqlConnection);
                    commNama.Parameters.AddWithValue("@IdInventory", ddlNamaGudang.SelectedValue.Trim());
                    //commNama.Parameters.AddWithValue("@IdSupplier", ddlSupName.SelectedValue.Trim());
                    //commNama.Parameters.AddWithValue("@IdBarang", slctdIndx);
                    commNama.Parameters.AddWithValue("@IdPOPembelianMst", id);
                    SqlDataAdapter brgAdapter = new SqlDataAdapter();
                    brgAdapter.SelectCommand = commNama;
                    DataTable dtBrg = new DataTable();
                    brgAdapter.Fill(dtBrg);
                    if (dtBrg.Rows.Count != 0)
                    {
                        ddlNamaBarang.DataSource = dtBrg;
                        ddlNamaBarang.DataBind();
                        ddlNamaBarang.DataTextField = "TEXT";
                        ddlNamaBarang.DataValueField = "ID";
                        ddlNamaBarang.DataBind();
                        if (slctdIndx != 0)
                        {
                            ddlNamaBarang.SelectedValue = slctdIndx.ToString();
                        }
                    }
                }
                else
                {
                    sqlConnection.Open();
                    DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[numRow].FindControl("ddlNamaBarang");
                    SqlCommand commNama = new SqlCommand("SELECT a.IdBarang as ID, a.NamaBarang + '-(' + b.SupplierName + ')' as TEXT FROM Barang a INNER JOIN Supplier b on a.IdSupplier = b.IdSupplier WHERE IdInventory = @IdInventory AND b.IdSupplier = @IdSupplier AND BarangStatus = 'ACTIVE'", sqlConnection);
                    commNama.Parameters.AddWithValue("@IdInventory", ddlNamaGudang.SelectedValue.Trim());
                    commNama.Parameters.AddWithValue("@IdSupplier", ddlSupName.SelectedValue.Trim());
                    //commNama.Parameters.AddWithValue("@IdBarang", slctdIndx);
                    //commNama.Parameters.AddWithValue("@IdPOPembelianMst", id);
                    SqlDataAdapter brgAdapter = new SqlDataAdapter();
                    brgAdapter.SelectCommand = commNama;
                    DataTable dtBrg = new DataTable();
                    brgAdapter.Fill(dtBrg);
                    if (dtBrg.Rows.Count != 0)
                    {
                        ddlNamaBarang.DataSource = dtBrg;
                        ddlNamaBarang.DataBind();
                        ddlNamaBarang.DataTextField = "TEXT";
                        ddlNamaBarang.DataValueField = "ID";
                        ddlNamaBarang.DataBind();
                        if (slctdIndx != 0)
                        {
                            ddlNamaBarang.SelectedValue = slctdIndx.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected void BindDDLBarang(int numRow,int slctdIndx)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[numRow].FindControl("ddlNamaBarang");
                SqlCommand commNama = new SqlCommand("SELECT NamaBarang FROM Barang WHERE IdInventory = @IdInventory AND IdSupplier= @IdSupplier AND BarangStatus = 'ACTIVE' ORDER BY NamaBarang", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", ddlNamaGudang.SelectedValue.Trim());
                commNama.Parameters.AddWithValue("@IdSupplier", ddlSupName.SelectedValue.Trim());
                SqlDataAdapter brgAdapter = new SqlDataAdapter();
                brgAdapter.SelectCommand = commNama;
                DataTable dtBrg = new DataTable();
                brgAdapter.Fill(dtBrg);
                if (dtBrg.Rows.Count != 0)
                {
                    TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[0].FindControl("txtItemQty");
                    TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[0].FindControl("txtPrice");
                    TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[0].FindControl("txtItemBox");
                    ddlNamaBarang.DataSource = dtBrg;
                    ddlNamaBarang.DataBind();
                    ddlNamaBarang.Enabled = true;
                    txtItemPrice.Enabled = true;
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
                    DropDownList ddlNewBarang = (DropDownList)rptPembelianDtl.Items[0].FindControl("ddlNamaBarang");
                    TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[0].FindControl("txtItemQty");
                    TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[0].FindControl("txtPrice");
                    TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[0].FindControl("txtItemBox");
                    ddlNewBarang.DataSource = dtBrg;
                    ddlNewBarang.DataBind();
                    ddlNewBarang.Items.Insert(0, new ListItem("Data Kosong", "Data Kosong"));
                    ddlNewBarang.Enabled = false;
                    txtItemPrice.Enabled = false;
                    txtItemQty.Enabled = false;
                    txtItemBox.Enabled = false;
                    txtGrandTotal.Text = "0";
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
            dt.Columns.Add("txtPrice", typeof(string));
            dt.Columns.Add("txtItemBox", typeof(string));
            dt.Columns.Add("txtTotalPrice", typeof(string));
            DataRow dr = dt.NewRow();
            //dr["txtItemCode"] = string.Empty;
            //dr["txtItemName"] = string.Empty;
            dr["ddlNamaBarang"] = 0;
            dr["txtItemQty"] = string.Empty;
            dr["txtPrice"] = string.Empty;
            dr["txtItemBox"] = string.Empty;
            dr["txtTotalPrice"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["Curtbl"] = dt;
            rptPembelianDtl.DataSource = dt;
            rptPembelianDtl.DataBind();

        }
        private void SetOldDataUpdate(int id)
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
                        DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                        TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                        TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                        TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                        TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                        LinkButton lnkBtnAdd = (LinkButton)rptPembelianDtl.Items[rowIndex].FindControl("btnadd");
                        LinkButton lnkBtnDel = (LinkButton)rptPembelianDtl.Items[rowIndex].FindControl("btndel");
                        //txtItemCode.Text = (Convert.ToInt32(GetItemCode()) + i).ToString();
                        //txtItemName.Text = dt.Rows[i]["txtItemName"].ToString();
                        //ddlNamaBarang.SelectedIndex = (Convert.ToInt32(dt.Rows[i]["txtItemName"].ToString()));
                        if (dt.Rows[i]["ddlNamaBarang"].ToString().Trim() == "")
                        {
                            BindDDLBarangUpdate(i, 0, id);
                        }
                        else
                        {
                            BindDDLBarangUpdate(i, (Convert.ToInt32(dt.Rows[i]["ddlNamaBarang"].ToString())),id);
                        }
                        //ddlNamaBarang.Enabled = false;
                        txtItemQty.Text = dt.Rows[i]["txtItemQty"].ToString();
                        txtItemPrice.Text = dt.Rows[i]["txtPrice"].ToString();
                        if (lblStatusPage.Text.Trim() == "Retur Pembelian" && lblHeaderPage.Text.Trim() == "Retur Pembelian")
                        {
                            txtItemPrice.Enabled = false;
                        }
                        txtItemBox.Text = dt.Rows[i]["txtItemBox"].ToString();
                        //txtItemBox.Enabled = false;
                        txtItemTotalPrice.Text = dt.Rows[i]["txtTotalPrice"].ToString();
                        //lnkBtnAdd.Visible = false;
                        //lnkBtnDel.Visible = false;
                        rowIndex++;
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
                //BindDDLBarang(dt.Rows.Count);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //TextBox txtItemCode = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemCode");
                        //TextBox txtItemName = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemName");
                        DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                        TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                        TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                        TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                        TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                        //txtItemCode.Text = (Convert.ToInt32(GetItemCode()) + i).ToString();
                        //txtItemName.Text = dt.Rows[i]["txtItemName"].ToString();
                        //ddlNamaBarang.SelectedIndex = (Convert.ToInt32(dt.Rows[i]["txtItemName"].ToString()));
                        if (dt.Rows[i]["ddlNamaBarang"].ToString().Trim() == "")
                        {
                            BindDDLBarang(i, 0);
                        }
                        else
                        {
                            BindDDLBarang(i, (Convert.ToInt32(dt.Rows[i]["ddlNamaBarang"].ToString())));
                        }
                        txtItemQty.Text = dt.Rows[i]["txtItemQty"].ToString();
                        txtItemPrice.Text = dt.Rows[i]["txtPrice"].ToString();
                        txtItemBox.Text = dt.Rows[i]["txtItemBox"].ToString();
                        txtItemTotalPrice.Text = dt.Rows[i]["txtTotalPrice"].ToString();
                        rowIndex++;
                    }
                }
            }
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            DropDownList ddlNamaBarangCheck = (DropDownList)rptPembelianDtl.Items[0].FindControl("ddlNamaBarang");
            LinkButton btn = (sender as LinkButton);
            RepeaterItem itemCheck = btn.NamingContainer as RepeaterItem;
            int indexCheck = itemCheck.ItemIndex;
            TextBox txtItemQtyCheck = (TextBox)itemCheck.FindControl("txtItemQty");
            TextBox txtItemPriceCheck = (TextBox)itemCheck.FindControl("txtPrice");
            TextBox txtItemBoxCheck = (TextBox)itemCheck.FindControl("txtItemBox");
            if (ddlNamaBarangCheck.Enabled == true)
            {
                if (txtItemQtyCheck.Text.Trim() != "" && txtItemQtyCheck.Text.Trim() != "0" && txtItemPriceCheck.Text.Trim() != "" && txtItemPriceCheck.Text.Trim() != "0" && txtItemBoxCheck.Text.Trim() != "" && txtItemBoxCheck.Text.Trim() != "0")
                {
                    if (lblStatusPage.Text.Trim() != "Retur Pembelian" && lblHeaderPage.Text.Trim() != "Retur Pembelian")
                    {
                        if (lblStatusPage.Text.Trim() != "Edit Pembelian" && lblHeaderPage.Text.Trim() != "Edit Pembelian")
                        {
                            int rowIndex = 0;

                            if (ViewState["Curtbl"] != null)
                            {
                                DataTable dt = (DataTable)ViewState["Curtbl"];
                                //BindDDLBarang(dt.Rows.Count);
                                DataRow drCurrentRow = null;
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 1; i <= dt.Rows.Count; i++)
                                    {
                                        //TextBox txtItemCode = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemCode");
                                        //TextBox txtItemName = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemName");
                                        DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                        TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                                        TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                                        TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                                        TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                                        drCurrentRow = dt.NewRow();
                                        //dt.Rows[i - 1]["txtItemCode"] = txtItemCode.Text;
                                        //dt.Rows[i - 1]["txtItemName"] = txtItemName.Text;
                                        dt.Rows[i - 1]["ddlNamaBarang"] = ddlNamaBarang.SelectedIndex.ToString();
                                        dt.Rows[i - 1]["txtItemQty"] = txtItemQty.Text;
                                        dt.Rows[i - 1]["txtPrice"] = txtItemPrice.Text;
                                        dt.Rows[i - 1]["txtItemBox"] = txtItemBox.Text;
                                        dt.Rows[i - 1]["txtTotalPrice"] = txtItemTotalPrice.Text;
                                        rowIndex++;
                                    }
                                    dt.Rows.Add(drCurrentRow);
                                    ViewState["Curtbl"] = dt;
                                    rptPembelianDtl.DataSource = dt;
                                    rptPembelianDtl.DataBind();
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
                            int rowIndex = 0;
                            int id;
                            int.TryParse(Request.QueryString["id"], out id);
                            if (ViewState["Curtbl"] != null)
                            {
                                DataTable dt = (DataTable)ViewState["Curtbl"];
                                //BindDDLBarang(dt.Rows.Count);
                                DataRow drCurrentRow = null;
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 1; i <= dt.Rows.Count; i++)
                                    {
                                        //TextBox txtItemCode = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemCode");
                                        //TextBox txtItemName = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemName");
                                        DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                        TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                                        TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                                        TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                                        TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                                        drCurrentRow = dt.NewRow();
                                        //dt.Rows[i - 1]["txtItemCode"] = txtItemCode.Text;
                                        //dt.Rows[i - 1]["txtItemName"] = txtItemName.Text;
                                        dt.Rows[i - 1]["ddlNamaBarang"] = ddlNamaBarang.SelectedValue.ToString();
                                        dt.Rows[i - 1]["txtItemQty"] = txtItemQty.Text;
                                        dt.Rows[i - 1]["txtPrice"] = txtItemPrice.Text;
                                        dt.Rows[i - 1]["txtItemBox"] = txtItemBox.Text;
                                        dt.Rows[i - 1]["txtTotalPrice"] = txtItemTotalPrice.Text;
                                        rowIndex++;
                                    }
                                    dt.Rows.Add(drCurrentRow);
                                    ViewState["Curtbl"] = dt;
                                    rptPembelianDtl.DataSource = dt;
                                    rptPembelianDtl.DataBind();
                                }
                            }
                            else
                            {
                                Response.Write("ViewState Value is Null");
                            }
                            SetOldDataUpdate(id);
                        }
                    }
                    else
                    {
                        int rowIndex = 0;
                        int id;
                        int.TryParse(Request.QueryString["id"], out id);
                        if (ViewState["Curtbl"] != null)
                        {
                            DataTable dt = (DataTable)ViewState["Curtbl"];
                            //BindDDLBarang(dt.Rows.Count);
                            DataRow drCurrentRow = null;
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 1; i <= dt.Rows.Count; i++)
                                {
                                    //TextBox txtItemCode = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemCode");
                                    //TextBox txtItemName = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemName");
                                    DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                    TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                                    TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                                    TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                                    TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                                    drCurrentRow = dt.NewRow();
                                    //dt.Rows[i - 1]["txtItemCode"] = txtItemCode.Text;
                                    //dt.Rows[i - 1]["txtItemName"] = txtItemName.Text;
                                    dt.Rows[i - 1]["ddlNamaBarang"] = ddlNamaBarang.SelectedIndex.ToString();
                                    dt.Rows[i - 1]["txtItemQty"] = txtItemQty.Text;
                                    dt.Rows[i - 1]["txtPrice"] = txtItemPrice.Text;
                                    dt.Rows[i - 1]["txtItemBox"] = txtItemBox.Text;
                                    dt.Rows[i - 1]["txtTotalPrice"] = txtItemTotalPrice.Text;
                                    rowIndex++;
                                }
                                dt.Rows.Add(drCurrentRow);
                                ViewState["Curtbl"] = dt;
                                rptPembelianDtl.DataSource = dt;
                                rptPembelianDtl.DataBind();
                            }
                        }
                        else
                        {
                            Response.Write("ViewState Value is Null");
                        }
                        SetOldDataUpdate(id);
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

        protected void btndel_Click(object sender, EventArgs e)
        {
            RepeaterItem item = (sender as LinkButton).NamingContainer as RepeaterItem;

            int rowIndex = 0;
            int rowID = item.ItemIndex;
            int id;
            int.TryParse(Request.QueryString["id"], out id);
            if (lblStatusPage.Text.Trim() != "Retur Pembelian" && lblHeaderPage.Text.Trim() != "Retur Pembelian")
            {
                if (lblStatusPage.Text.Trim() != "Edit Pembelian" && lblHeaderPage.Text.Trim() != "Edit Pembelian")
                {
                    if (ViewState["Curtbl"] != null)
                    {
                        DataTable dt = (DataTable)ViewState["Curtbl"];
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 1; i <= rptPembelianDtl.Items.Count; i++)
                            {
                                //TextBox txtItemCode = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemCode");
                                //TextBox txtItemName = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemName");
                                DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                                TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                                TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                                TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                                //dt.Rows[i - 1]["txtItemCode"] = txtItemCode.Text;
                                //dt.Rows[i - 1]["txtItemName"] = txtItemName.Text;
                                dt.Rows[i - 1]["ddlNamaBarang"] = ddlNamaBarang.SelectedIndex.ToString();
                                dt.Rows[i - 1]["txtItemQty"] = txtItemQty.Text;
                                dt.Rows[i - 1]["txtPrice"] = txtItemPrice.Text;
                                dt.Rows[i - 1]["txtItemBox"] = txtItemBox.Text;
                                dt.Rows[i - 1]["txtTotalPrice"] = txtItemTotalPrice.Text;
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
                        rptPembelianDtl.DataSource = dt;
                        rptPembelianDtl.DataBind();
                    }

                    SetOldData();
                    txtTotalPrice_TextChanged(sender, e);
                }
                else
                {
                    if (ViewState["Curtbl"] != null)
                    {
                        DataTable dt = (DataTable)ViewState["Curtbl"];
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 1; i <= rptPembelianDtl.Items.Count; i++)
                            {
                                //TextBox txtItemCode = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemCode");
                                //TextBox txtItemName = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemName");
                                DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                                TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                                TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                                TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                                //dt.Rows[i - 1]["txtItemCode"] = txtItemCode.Text;
                                //dt.Rows[i - 1]["txtItemName"] = txtItemName.Text;
                                dt.Rows[i - 1]["ddlNamaBarang"] = ddlNamaBarang.SelectedValue.ToString();
                                dt.Rows[i - 1]["txtItemQty"] = txtItemQty.Text;
                                dt.Rows[i - 1]["txtPrice"] = txtItemPrice.Text;
                                dt.Rows[i - 1]["txtItemBox"] = txtItemBox.Text;
                                dt.Rows[i - 1]["txtTotalPrice"] = txtItemTotalPrice.Text;
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
                        rptPembelianDtl.DataSource = dt;
                        rptPembelianDtl.DataBind();
                    }
                    SetOldDataUpdate(id);
                    txtTotalPrice_TextChanged(sender, e);
                }
            }
            else
            {
                if (ViewState["Curtbl"] != null)
                {
                    DataTable dt = (DataTable)ViewState["Curtbl"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 1; i <= rptPembelianDtl.Items.Count; i++)
                        {
                            //TextBox txtItemCode = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemCode");
                            //TextBox txtItemName = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemName");
                            DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                            TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                            TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                            TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                            TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                            //dt.Rows[i - 1]["txtItemCode"] = txtItemCode.Text;
                            //dt.Rows[i - 1]["txtItemName"] = txtItemName.Text;
                            dt.Rows[i - 1]["ddlNamaBarang"] = ddlNamaBarang.SelectedIndex.ToString();
                            dt.Rows[i - 1]["txtItemQty"] = txtItemQty.Text;
                            dt.Rows[i - 1]["txtPrice"] = txtItemPrice.Text;
                            dt.Rows[i - 1]["txtItemBox"] = txtItemBox.Text;
                            dt.Rows[i - 1]["txtTotalPrice"] = txtItemTotalPrice.Text;
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
                    rptPembelianDtl.DataSource = dt;
                    rptPembelianDtl.DataBind();
                }
                SetOldDataUpdate(id);
                txtTotalPrice_TextChanged(sender, e);
            }
        }
        private string GetSisaKapasitasInv(int idInv)
        {
            string sisaCapacity = "";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT IvnSisaCapacity FROM Inventory WHERE IdInventory = @IdInventory", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", idInv);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        var sisaCpt = (int)dr["IvnSisaCapacity"];
                        sisaCapacity = sisaCpt.ToString();
                        return sisaCapacity;
                    }
                    else
                    {
                        return sisaCapacity;
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
        private int GetIDPmbMst()
        {
            int idPmbMst = 0;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT TOP 1 * FROM POPembelianMst ORDER BY IdPOPembelianMst DESC", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        idPmbMst = (int)dr["IdPOPembelianMst"];
                        return idPmbMst;
                    }
                    else
                    {
                        return idPmbMst;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void txtItemQty_TextChanged(object sender, EventArgs e)
        {

            TextBox tb1 = ((TextBox)(sender));

            RepeaterItem rp1 = ((RepeaterItem)(tb1.NamingContainer));


            TextBox tb2 = (TextBox)rp1.FindControl("txtTotalPrice");

            TextBox tb3 = (TextBox)rp1.FindControl("txtPrice");
            DropDownList tb4 = (DropDownList)rp1.FindControl("ddlNamaBarang");
            if (lblStatusPage.Text != "Retur Pembelian" && lblHeaderPage.Text != "Retur Pembelian")
            {
                if (tb1.Text.Trim() == "")
                {
                    tb1.Text = "0";
                    if (tb3.Text.Trim() == "")
                    {
                        tb3.Text = "0";
                        if (tb1.Text.Trim() == "0")
                        {
                            double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                            double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                            tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                        }
                        else
                        {
                            double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                            double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                            tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                        }
                    }
                    else
                    {
                        if (tb1.Text.Trim() == "0")
                        {
                            double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                            double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                            tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                        }
                        else
                        {
                            double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                            double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                            tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                        }
                    }
                }
                else
                {
                    if (tb3.Text.Trim() == "")
                    {
                        tb3.Text = "0";
                        if (tb1.Text.Trim() == "0")
                        {
                            double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                            double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                            tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                        }
                        else
                        {
                            double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                            double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                            tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                        }
                    }
                    else
                    {
                        if (tb1.Text.Trim() == "0")
                        {
                            double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                            double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                            tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                        }
                        else
                        {
                            double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                            double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                            tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                        }
                    }
                }
            }
            else
            {
                if (ViewState["Curtbl"] != null)
                {
                    DataTable dt = (DataTable)ViewState["Curtbl"];
                    DataTable dt1 = (DataTable)ViewState["Rtrtbl"];
                    if (tb1.Text.Trim() == "")
                    {
                        tb1.Text = "0";
                        if (tb3.Text.Trim() == "")
                        {
                            tb3.Text = "0";
                            if (tb1.Text.Trim() == "0")
                            {
                                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                            }
                            else
                            {
                                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                                var test = (from item in dt1.AsEnumerable()
                                            where item.Field<int>("ddlNamaBarang") == Convert.ToInt32(tb4.SelectedValue.Trim())
                                            select item.Field<string>("txtItemQty")).FirstOrDefault();
                                if (Convert.ToDecimal(test) < Convert.ToDecimal(tb1.Text.Trim()))
                                {
                                    tb1.Text = "0";
                                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Nota Pembelian!" + "');", true);
                                }
                                else
                                {
                                    if (!checkQtyItem(Convert.ToInt32(tb4.SelectedValue.Trim()), Convert.ToDecimal(tb1.Text.Trim())))
                                    {
                                        tb1.Text = "0";
                                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Stock Barang!" + "');", true);
                                    }
                                    else
                                    {
                                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (tb1.Text.Trim() == "0")
                            {
                                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                            }
                            else
                            {
                                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                                var test = (from item in dt1.AsEnumerable()
                                            where item.Field<int>("ddlNamaBarang") == Convert.ToInt32(tb4.SelectedValue.Trim())
                                            select item.Field<string>("txtItemQty")).FirstOrDefault();
                                if (Convert.ToDecimal(test) < Convert.ToDecimal(tb1.Text.Trim()))
                                {
                                    tb1.Text = "0";
                                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Nota Pembelian!" + "');", true);
                                }
                                else
                                {
                                    if (!checkQtyItem(Convert.ToInt32(tb4.SelectedValue.Trim()), Convert.ToDecimal(tb1.Text.Trim())))
                                    {
                                        tb1.Text = "0";
                                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Stock Barang!" + "');", true);
                                    }
                                    else
                                    {
                                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (tb3.Text.Trim() == "")
                        {
                            tb3.Text = "0";
                            if (tb1.Text.Trim() == "0")
                            {
                                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                            }
                            else
                            {
                                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                                var test = (from item in dt1.AsEnumerable()
                                            where item.Field<int>("ddlNamaBarang") == Convert.ToInt32(tb4.SelectedValue.Trim())
                                            select item.Field<string>("txtItemQty")).FirstOrDefault();
                                if (Convert.ToDecimal(test) < Convert.ToDecimal(tb1.Text.Trim()))
                                {
                                    tb1.Text = "0";
                                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Nota Pembelian!" + "');", true);
                                }
                                else
                                {
                                    if (!checkQtyItem(Convert.ToInt32(tb4.SelectedValue.Trim()), Convert.ToDecimal(tb1.Text.Trim())))
                                    {
                                        tb1.Text = "0";
                                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Stock Barang!" + "');", true);
                                    }
                                    else
                                    {
                                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (tb1.Text.Trim() == "0")
                            {
                                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                            }
                            else
                            {
                                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                                var test = (from item in dt1.AsEnumerable()
                                            where item.Field<int>("ddlNamaBarang") == Convert.ToInt32(tb4.SelectedValue.Trim())
                                            select item.Field<string>("txtItemQty")).FirstOrDefault();
                                if (Convert.ToDecimal(test) < Convert.ToDecimal(tb1.Text.Trim()))
                                {
                                    tb1.Text = "0";
                                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Nota Pembelian!" + "');", true);
                                }
                                else
                                {
                                    if (!checkQtyItem(Convert.ToInt32(tb4.SelectedValue.Trim()), Convert.ToDecimal(tb1.Text.Trim())))
                                    {
                                        tb1.Text = "0";
                                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Stock Barang!" + "');", true);
                                    }
                                    else
                                    {
                                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (tb2.Text.Trim() != "Rp0")
            {
                txtTotalPrice_TextChanged(sender, e);
            }
            else
            {
                txtGrandTotal.Text = "0";
                tb1.Text = "0";
            }
        }
        private bool checkQtyItem(int idBarang, decimal qtyBrg)
        {
            bool setBoxItem = false;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT BarangQuantity FROM Barang WHERE IdBarang = @IdBarang", sqlConnection);
                commNama.Parameters.AddWithValue("@IdBarang", idBarang);
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

        protected void txtPrice_TextChanged(object sender, EventArgs e)
        {
            TextBox tb1 = ((TextBox)(sender));
            RepeaterItem rp1 = ((RepeaterItem)(tb1.NamingContainer));

            TextBox tb2 = (TextBox)rp1.FindControl("txtTotalPrice");

            TextBox tb3 = (TextBox)rp1.FindControl("txtItemQty");
            if (tb1.Text.Trim() == "")
            {
                tb1.Text = "0";
                if (tb3.Text.Trim() == "")
                {
                    tb3.Text = "0";
                    if (tb1.Text.Trim() == "0")
                    {
                        double txtQtyItmVal = Convert.ToDouble(tb3.Text.Trim());
                        double txtPriceItmVal = Convert.ToDouble(tb1.Text.Trim());
                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                        string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                    }
                    else
                    {
                        double txtQtyItmVal = Convert.ToDouble(tb3.Text.Trim());
                        double txtPriceItmVal = Convert.ToDouble(tb1.Text.Trim());
                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                        string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                    }
                }
                else
                {
                    if (tb1.Text.Trim() == "0")
                    {
                        double txtQtyItmVal = Convert.ToDouble(tb3.Text.Trim());
                        double txtPriceItmVal = Convert.ToDouble(tb1.Text.Trim());
                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                        string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                    }
                    else
                    {
                        double txtQtyItmVal = Convert.ToDouble(tb3.Text.Trim());
                        double txtPriceItmVal = Convert.ToDouble(tb1.Text.Trim());
                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                        string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                    }
                }
            }
            else
            {
                if (tb3.Text.Trim() == "")
                {
                    tb3.Text = "0";
                    if (tb1.Text.Trim() == "0")
                    {
                        double txtQtyItmVal = Convert.ToDouble(tb3.Text.Trim());
                        double txtPriceItmVal = Convert.ToDouble(tb1.Text.Trim());
                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                        string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                    }
                    else
                    {
                        double txtQtyItmVal = Convert.ToDouble(tb3.Text.Trim());
                        double txtPriceItmVal = Convert.ToDouble(tb1.Text.Trim());
                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                        string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                    }
                }
                else
                {
                    if (tb1.Text.Trim() == "0")
                    {
                        double txtQtyItmVal = Convert.ToDouble(tb3.Text.Trim());
                        double txtPriceItmVal = Convert.ToDouble(tb1.Text.Trim());
                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                        string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                    }
                    else
                    {
                        double txtQtyItmVal = Convert.ToDouble(tb3.Text.Trim());
                        double txtPriceItmVal = Convert.ToDouble(tb1.Text.Trim());
                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                        string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                    }
                }
            }
            if (tb2.Text.Trim() != "Rp0")
            {
                txtTotalPrice_TextChanged(sender, e);
            }
            else
            {
                txtGrandTotal.Text = "0";
            }
        }

        protected void txtTotalPrice_TextChanged(object sender, EventArgs e)
        {
            int rowIndex = 0;
            decimal counter = 0;
            if (ViewState["Curtbl"] != null)
            {
                DataTable dt = (DataTable)ViewState["Curtbl"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                        if (txtItemTotalPrice.Text.Trim() != "")
                        {
                            counter += decimal.Parse(Regex.Replace(txtItemTotalPrice.Text.Trim(), @"[^\d.]", ""));
                        }
                        else
                        {
                            counter += 0;
                        }
                        rowIndex++;
                    }
                    Results = counter;
                    txtGrandTotal.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", Results);
                }
            }
        }
        const string GTotalState = "TotalPrice_cnst";
        public decimal Results
        {
            get { return Convert.ToDecimal(ViewState["GTotalState"]); }
            set { ViewState["GTotalState"] = value; }
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
                    DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                    TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                    TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                    TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                    TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                    if (txtItemQty.Text.Trim() != "0" && txtItemPrice.Text.Trim() != "0" && txtItemBox.Text.Trim() != "0" && txtItemTotalPrice.Text.Trim() != "Rp0")
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
                if(checkDtl == 1)
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

        private string GetSupplierName(int id)
        {
            string supName = "";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT SupplierName FROM Supplier WHERE IdSupplier = " + id + "", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        supName = dr.GetString(dr.GetOrdinal("SupplierName"));
                        return supName;
                    }
                    else
                    {
                        return supName;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (lblStatusPage.Text.Trim() != "Retur Pembelian" && lblHeaderPage.Text.Trim() != "Retur Pembelian")
            {
                if (lblStatusPage.Text.Trim() != "Edit Pembelian" && lblHeaderPage.Text.Trim() != "Edit Pembelian")
                {
                    DropDownList ddlNamaBarangCheck = (DropDownList)rptPembelianDtl.Items[0].FindControl("ddlNamaBarang");
                    pmDtlList = new List<PembelianDtl>();
                    pmbObj = new PembelianMst();
                    int counterJmlBox = 0;
                    if (ddlNamaBarangCheck.Enabled == true)
                    {
                        int rowIndex = 0;
                        DateTime tglBeliStr = DateTime.ParseExact(txtCldBrgBeli.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        DateTime tglJthTmpStr = DateTime.ParseExact(txtCldJtuhTempo.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        if (ViewState["Curtbl"] != null)
                        {
                            DataTable dt = (DataTable)ViewState["Curtbl"];
                            //BindDDLBarang(dt.Rows.Count);
                            if (checkControlDetail())
                            {
                                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                                pmbObj.NoInvoicePembelian = txtNoInvoice.Text.Trim().ToUpper();
                                pmbObj.IdSupplier = Convert.ToInt32(ddlSupName.SelectedValue.Trim());
                                pmbObj.TglPembelian = tglBeliStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                pmbObj.TglJatuhTmpPembelian = tglJthTmpStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                pmbObj.JenisTagihan = ddlJenisTagihan.SelectedValue.Trim();
                                pmbObj.NoRekPembayaran = txtBrgNameBank.Text.Trim();
                                pmbObj.IdInventory = Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim());
                                pmbObj.TotalTagihanPembelian = decimal.Parse(Regex.Replace(txtGrandTotal.Text.Trim(), @"[^\d.]", ""));
                                if (pmbObj.TotalTagihanPembelian != 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                        TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                                        TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                                        TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                                        TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
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
                                                    double txtPriceItmVal = Convert.ToDouble(txtItemPrice.Text.Trim());
                                                    string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                                    string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                                    PembelianDtl pmbDtl = new PembelianDtl();
                                                    pmbDtl.IdPembelianMst = GetIDPmbMst() + 1;
                                                    pmbDtl.IdBarang = (int)dr["IdBarang"];
                                                    pmbDtl.NamaBarangPembelianDtl = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                                    pmbDtl.HargaPembelianDtl = txtPriceItmStr; //harus 2 angka dibelakang koma
                                                    pmbDtl.TotalHargaPembelianDtl = decimal.Parse(Regex.Replace(txtItemTotalPrice.Text.Trim(), @"[^\d.]", ""));
                                                    pmbDtl.JumlahBoxPembelianDtl = Convert.ToInt32(txtItemBox.Text.Trim());
                                                    pmbDtl.QtyBarangPembelianDtl = txtQtyItmStr; //harus 2 angka dibelakang koma
                                                    pmDtlList.Add(pmbDtl);
                                                    counterJmlBox += pmbDtl.JumlahBoxPembelianDtl;
                                                    //tambah proses ngecek kapasitas gudang 

                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        { throw ex; }
                                        finally { sqlConnection.Close(); sqlConnection.Dispose(); }
                                        rowIndex++;
                                    }
                                    if (!pmDtlList.GroupBy(a => a.NamaBarangPembelianDtl).Any(c => c.Count() > 1))
                                    {
                                        if (counterJmlBox <= Convert.ToInt32(txtSisaKapasitasGudang.Text.Trim()))
                                        {
                                            Barang brgCounter = new Barang();
                                            sqlConnection = new SqlConnection(connectionString);
                                            try
                                            {
                                                sqlConnection.Open();
                                                for (int i = 0; i < pmDtlList.Count; i++)
                                                {
                                                    SqlCommand command = new SqlCommand("insert into POPembelianDtl" + "(IdPOPembelianMst,IdBarang,PembelianDtlCreatedBy,PembelianDtlCreatedDate,PembelianDtlUpdatedBy,PembelianDtlUpdatedDate,HargaPOPembelianDtl,TotalHargaPOPembelianDtl,JumlahBoxPOPembelianDtl,QtyBarangPOPembelianDtl) values " + "(" + pmDtlList[i].IdPembelianMst + "," + pmDtlList[i].IdBarang + ",'" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "'," + pmDtlList[i].HargaPembelianDtl + "," + pmDtlList[i].TotalHargaPembelianDtl + "," + pmDtlList[i].JumlahBoxPembelianDtl + "," + pmDtlList[i].QtyBarangPembelianDtl + ")", sqlConnection);
                                                    command.ExecuteNonQuery();
                                                    SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang AND IdSupplier = @IdSupplier AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                                    commNama.Parameters.AddWithValue("@IdBarang", pmDtlList[i].IdBarang);
                                                    commNama.Parameters.AddWithValue("@IdSupplier", pmbObj.IdSupplier);
                                                    commNama.Parameters.AddWithValue("@IdInventory", pmbObj.IdInventory);
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
                                                    SqlCommand strQuerEditBarang = new SqlCommand("update Barang set BarangQuantity=@BarangQuantity,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate,BarangTanggalBeli=@BarangTanggalBeli,BarangJmlBox=@BarangJmlBox where IdBarang=@IdBarang AND IdInventory=@IdInventory AND IdSupplier=@IdSupplier", sqlConnection);
                                                    strQuerEditBarang.Parameters.AddWithValue("@IdBarang", pmDtlList[i].IdBarang);
                                                    strQuerEditBarang.Parameters.AddWithValue("@IdInventory", pmbObj.IdInventory);
                                                    strQuerEditBarang.Parameters.AddWithValue("@IdSupplier", pmbObj.IdSupplier);
                                                    strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                                    strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                                    strQuerEditBarang.Parameters.AddWithValue("@BarangTanggalBeli", pmbObj.TglPembelian);
                                                    strQuerEditBarang.Parameters.AddWithValue("@BarangQuantity", brgCounter.BarangQty + Convert.ToDecimal(pmDtlList[i].QtyBarangPembelianDtl));
                                                    strQuerEditBarang.Parameters.AddWithValue("@BarangJmlBox", brgCounter.BarangJmlBox + Convert.ToDecimal(pmDtlList[i].JumlahBoxPembelianDtl));
                                                    strQuerEditBarang.ExecuteNonQuery();
                                                    updateKapasitasGudangNew(pmDtlList[i], pmbObj);
                                                }
                                                SqlCommand commandPembelianMst = new SqlCommand("insert into POPembelianMst" + "(TanggalPOPembelian,JenisTagihan,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IdSupplier,NoInvoicePembelian,TanggalPOJatuhTmpPembelian,TotalTagihanPembelian,NoRekeningPembayaran,IdInventory,StatusInvoicePembelianMst) values " + "('" + pmbObj.TglPembelian + "','" + pmbObj.JenisTagihan + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "'," + pmbObj.IdSupplier + ",'" + pmbObj.NoInvoicePembelian + "','" + pmbObj.TglJatuhTmpPembelian + "'," + pmbObj.TotalTagihanPembelian + ",'" + pmbObj.NoRekPembayaran.ToUpper() + "'," + pmbObj.IdInventory + ",'NORMAL')", sqlConnection);
                                                commandPembelianMst.ExecuteNonQuery();
                                                sqlConnection.Close();
                                                sqlConnection.Dispose();
                                                if (pmbObj.JenisTagihan.Trim() == "KONTRABON")
                                                {
                                                    string startDate = txtCldBrgBeli.Text.Trim();
                                                    string startTime = String.Format("{0:HH:mm:ss}", DateTime.Now);
                                                    DateTime dateStart = new DateTime(Convert.ToInt32(startDate.Substring(6, 4)), Convert.ToInt32(startDate.Substring(0, 2)), Convert.ToInt32(startDate.Substring(3, 2)), Convert.ToInt32(startTime.Substring(0, 2)), Convert.ToInt32(startTime.Substring(3, 2)), Convert.ToInt32(startTime.Substring(6, 2)));
                                                    string endDate = txtCldJtuhTempo.Text.Trim();
                                                    string endTime = String.Format("{0:HH:mm:ss}", DateTime.Now.AddMinutes(15));
                                                    DateTime dateEnd = new DateTime(Convert.ToInt32(endDate.Substring(6, 4)), Convert.ToInt32(endDate.Substring(0, 2)), Convert.ToInt32(endDate.Substring(3, 2)), Convert.ToInt32(endTime.Substring(0, 2)), Convert.ToInt32(endTime.Substring(3, 2)), Convert.ToInt32(endTime.Substring(6, 2)));
                                                    _backgroundJobServer = new BackgroundJobServer();
                                                    TimeSpan delayTime = dateEnd.AddDays(-1) - DateTime.Now;
                                                    BackgroundJob.Schedule<EmailSender>(x => x.backgroundNotifJob(pmbObj), delayTime);
                                                    BackgroundJob.Schedule<EmailSender>(x => x.Send("Pembelian - " + pmbObj.NoInvoicePembelian + " " + GetSupplierName(pmbObj.IdSupplier), pmbObj), delayTime);
                                                    _backgroundJobServer.Dispose();
                                                    //backgroundNotifJob(pmbObj);
                                                    insertToReminder(pmbObj, endDate);
                                                }
                                                Response.Redirect("PembelianPage.aspx", false);
                                            }
                                            catch (Exception ex)
                                            { throw ex; }
                                        }
                                        else
                                        {
                                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Kapasitas Gudang Tidak Mencukupi!" + "');", true);
                                        }
                                    }
                                    else
                                    {
                                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nama Barang Tidak Boleh Sama Dalam 1 Transaksi!" + "');", true);
                                    }
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Seluruh Data Harus Terpenuhi Dahulu!" + "');", true);
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
                    DropDownList ddlNamaBarangCheck = (DropDownList)rptPembelianDtl.Items[0].FindControl("ddlNamaBarang");
                    pmDtlList = new List<PembelianDtl>();
                    pmbObj = new PembelianMst();
                    if (ddlNamaBarangCheck.Enabled == true)
                    {
                        int id;
                        int.TryParse(Request.QueryString["id"], out id);
                        int rowIndex = 0;
                        connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                        int counterJmlBox = 0;
                        pmDtlList = new List<PembelianDtl>();
                        List<PembelianDtl> pmDtlListBaru = new List<PembelianDtl>();
                        pmbObj = new PembelianMst();
                        pmbObj.IdPembelian = id;
                        DateTime tglBeliStr = DateTime.ParseExact(txtCldBrgBeli.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        DateTime tglJthTmpStr = DateTime.ParseExact(txtCldJtuhTempo.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        pmbObj.IdSupplier = Convert.ToInt32(ddlSupName.SelectedValue);
                        pmbObj.IdInventory = Convert.ToInt32(ddlNamaGudang.SelectedValue);
                        pmbObj.JenisTagihan = ddlJenisTagihan.SelectedValue.Trim();
                        pmbObj.NoInvoicePembelian = txtNoInvoice.Text.Trim();
                        pmbObj.NoRekPembayaran = txtBrgNameBank.Text.Trim();
                        pmbObj.TotalTagihanPembelian = decimal.Parse(Regex.Replace(txtGrandTotal.Text.Trim(), @"[^\d.]", ""));
                        pmbObj.TglPembelian = tglBeliStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        pmbObj.TglJatuhTmpPembelian = tglJthTmpStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        if (ViewState["Curtbl"] != null)
                        {
                            if (pmbObj.TotalTagihanPembelian != 0)
                            {
                                DataTable dt = (DataTable)ViewState["Curtbl"];
                                if (checkControlDetail())
                                {
                                    for (int j = 0; j < rptPembelianDtl.Items.Count; j++)
                                    {
                                        DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                        TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                                        TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                                        TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                                        TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                                        sqlConnection = new SqlConnection(connectionString);
                                        try
                                        {
                                            sqlConnection.Open();
                                            SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang AND IdSupplier = @IdSupplier AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                            commNama.Parameters.AddWithValue("@IdBarang", ddlNamaBarang.SelectedValue.Trim());
                                            commNama.Parameters.AddWithValue("@IdSupplier", Convert.ToInt32(ddlSupName.SelectedValue.Trim()));
                                            commNama.Parameters.AddWithValue("@IdInventory", Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()));
                                            using (SqlDataReader dr = commNama.ExecuteReader())
                                            {
                                                if (dr.Read())
                                                {
                                                    double txtQtyItmVal = Convert.ToDouble(txtItemQty.Text.Trim());
                                                    double txtPriceItmVal = Convert.ToDouble(txtItemPrice.Text.Trim());
                                                    string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                                    string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                                    PembelianDtl pmbDtl = new PembelianDtl();
                                                    pmbDtl.IdPembelianMst = pmbObj.IdPembelian;
                                                    pmbDtl.IdBarang = (int)dr["IdBarang"];
                                                    pmbDtl.NamaBarangPembelianDtl = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                                    pmbDtl.HargaPembelianDtl = txtPriceItmStr; //harus 2 angka dibelakang koma
                                                    pmbDtl.TotalHargaPembelianDtl = decimal.Parse(Regex.Replace(txtItemTotalPrice.Text.Trim(), @"[^\d.]", ""));
                                                    pmbDtl.JumlahBoxPembelianDtl = Convert.ToInt32(txtItemBox.Text.Trim());
                                                    pmbDtl.QtyBarangPembelianDtl = txtQtyItmStr; //harus 2 angka dibelakang koma
                                                    pmDtlList.Add(pmbDtl);
                                                    counterJmlBox += pmbDtl.JumlahBoxPembelianDtl;
                                                    //tambah proses ngecek kapasitas gudang
                                                    //updateQtyBarangRetur(pmbDtl, pmbObj);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        { throw ex; }
                                        finally { sqlConnection.Close(); sqlConnection.Dispose(); }
                                        rowIndex++;
                                    }
                                    if (!pmDtlList.GroupBy(a => a.IdBarang).Any(c => c.Count() > 1))
                                    {
                                        if (checkGudangKapasitas())
                                        {
                                            DataTable dt1 = (DataTable)ViewState["Rtrtbl"];
                                            for (int i = 0; i < dt1.Rows.Count; i++)
                                            {
                                                PembelianDtl pmbDtl = new PembelianDtl();
                                                double txtQtyItmVal = Convert.ToDouble(dt1.Rows[i]["txtItemQty"].ToString());
                                                double txtPriceItmVal = Convert.ToDouble(dt1.Rows[i]["txtPrice"].ToString());
                                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                                pmbDtl.IdPembelianMst = id;
                                                pmbDtl.IdBarang = Convert.ToInt32(dt1.Rows[i]["ddlNamaBarang"].ToString());
                                                pmbDtl.QtyBarangPembelianDtl = txtQtyItmStr;
                                                pmbDtl.HargaPembelianDtl = txtPriceItmStr;
                                                pmbDtl.JumlahBoxPembelianDtl = Convert.ToInt32(dt1.Rows[i]["txtItemBox"].ToString());
                                                pmbDtl.TotalHargaPembelianDtl = decimal.Parse(Regex.Replace(dt1.Rows[i]["txtTotalPrice"].ToString(), @"[^\d.]", ""));
                                                updateQtyBarangRetur(pmbDtl, pmbObj);
                                                deleteUpdateBarang(pmbDtl, pmbObj);
                                            }
                                            Barang brgCounter = new Barang();
                                            sqlConnection = new SqlConnection(connectionString);
                                            try
                                            {
                                                sqlConnection.Open();
                                                for (int i = 0; i < pmDtlList.Count; i++)
                                                {
                                                    SqlCommand command = new SqlCommand("insert into POPembelianDtl" + "(IdPOPembelianMst,IdBarang,PembelianDtlCreatedBy,PembelianDtlCreatedDate,PembelianDtlUpdatedBy,PembelianDtlUpdatedDate,HargaPOPembelianDtl,TotalHargaPOPembelianDtl,JumlahBoxPOPembelianDtl,QtyBarangPOPembelianDtl) values " + "(" + pmDtlList[i].IdPembelianMst + "," + pmDtlList[i].IdBarang + ",'" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "'," + pmDtlList[i].HargaPembelianDtl + "," + pmDtlList[i].TotalHargaPembelianDtl + "," + pmDtlList[i].JumlahBoxPembelianDtl + "," + pmDtlList[i].QtyBarangPembelianDtl + ")", sqlConnection);
                                                    command.ExecuteNonQuery();
                                                    SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang AND IdSupplier = @IdSupplier AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                                    commNama.Parameters.AddWithValue("@IdBarang", pmDtlList[i].IdBarang);
                                                    commNama.Parameters.AddWithValue("@IdSupplier", pmbObj.IdSupplier);
                                                    commNama.Parameters.AddWithValue("@IdInventory", pmbObj.IdInventory);
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
                                                    SqlCommand strQuerEditBarang = new SqlCommand("update Barang set BarangQuantity=@BarangQuantity,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate,BarangTanggalBeli=@BarangTanggalBeli,BarangJmlBox=@BarangJmlBox where IdBarang=@IdBarang AND IdInventory=@IdInventory AND IdSupplier=@IdSupplier", sqlConnection);
                                                    strQuerEditBarang.Parameters.AddWithValue("@IdBarang", pmDtlList[i].IdBarang);
                                                    strQuerEditBarang.Parameters.AddWithValue("@IdInventory", pmbObj.IdInventory);
                                                    strQuerEditBarang.Parameters.AddWithValue("@IdSupplier", pmbObj.IdSupplier);
                                                    strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                                    strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                                    strQuerEditBarang.Parameters.AddWithValue("@BarangTanggalBeli", pmbObj.TglPembelian);
                                                    strQuerEditBarang.Parameters.AddWithValue("@BarangQuantity", brgCounter.BarangQty + Convert.ToDecimal(pmDtlList[i].QtyBarangPembelianDtl));
                                                    strQuerEditBarang.Parameters.AddWithValue("@BarangJmlBox", brgCounter.BarangJmlBox + Convert.ToDecimal(pmDtlList[i].JumlahBoxPembelianDtl));
                                                    strQuerEditBarang.ExecuteNonQuery();
                                                    updateKapasitasGudangNew(pmDtlList[i], pmbObj);
                                                }
                                                SqlCommand commandPembelianMst = new SqlCommand("update POPembelianMst set NoInvoicePembelian=@NoInvoicePembelian,IdSupplier=@IdSupplier,TanggalPOPembelian=@TanggalPOPembelian,JenisTagihan=@JenisTagihan,TanggalPOJatuhTmpPembelian=@TanggalPOJatuhTmpPembelian,NoRekeningPembayaran=@NoRekeningPembayaran,IdInventory=@IdInventory,TotalTagihanPembelian=@TotalTagihanPembelian,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate where IdPOPembelianMst=@IdPOPembelianMst", sqlConnection);
                                                commandPembelianMst.Parameters.AddWithValue("@NoInvoicePembelian", pmbObj.NoInvoicePembelian);
                                                commandPembelianMst.Parameters.AddWithValue("@IdSupplier", pmbObj.IdSupplier);
                                                commandPembelianMst.Parameters.AddWithValue("@TanggalPOPembelian", pmbObj.TglPembelian);
                                                commandPembelianMst.Parameters.AddWithValue("@JenisTagihan", pmbObj.JenisTagihan);
                                                commandPembelianMst.Parameters.AddWithValue("@TanggalPOJatuhTmpPembelian", pmbObj.TglJatuhTmpPembelian);
                                                commandPembelianMst.Parameters.AddWithValue("@NoRekeningPembayaran", pmbObj.NoRekPembayaran);
                                                commandPembelianMst.Parameters.AddWithValue("@IdInventory", pmbObj.IdInventory);
                                                commandPembelianMst.Parameters.AddWithValue("@TotalTagihanPembelian", pmbObj.TotalTagihanPembelian);
                                                commandPembelianMst.Parameters.AddWithValue("@UpdatedBy", ((Role)Session["Role"]).EmailEmp);
                                                commandPembelianMst.Parameters.AddWithValue("@UpdatedDate", DateTime.Now.Date.ToString());
                                                commandPembelianMst.Parameters.AddWithValue("@IdPOPembelianMst", pmbObj.IdPembelian);
                                                commandPembelianMst.ExecuteNonQuery();
                                                sqlConnection.Close();
                                                sqlConnection.Dispose();
                                                if (pmbObj.JenisTagihan.Trim() == "KONTRABON")
                                                {
                                                    deleteFromReminder(pmbObj);
                                                    string startDate = txtCldBrgBeli.Text.Trim();
                                                    string startTime = String.Format("{0:HH:mm:ss}", DateTime.Now);
                                                    DateTime dateStart = new DateTime(Convert.ToInt32(startDate.Substring(6, 4)), Convert.ToInt32(startDate.Substring(0, 2)), Convert.ToInt32(startDate.Substring(3, 2)), Convert.ToInt32(startTime.Substring(0, 2)), Convert.ToInt32(startTime.Substring(3, 2)), Convert.ToInt32(startTime.Substring(6, 2)));
                                                    string endDate = txtCldJtuhTempo.Text.Trim();
                                                    string endTime = String.Format("{0:HH:mm:ss}", DateTime.Now.AddMinutes(15));
                                                    DateTime dateEnd = new DateTime(Convert.ToInt32(endDate.Substring(6, 4)), Convert.ToInt32(endDate.Substring(0, 2)), Convert.ToInt32(endDate.Substring(3, 2)), Convert.ToInt32(endTime.Substring(0, 2)), Convert.ToInt32(endTime.Substring(3, 2)), Convert.ToInt32(endTime.Substring(6, 2)));
                                                    _backgroundJobServer = new BackgroundJobServer();
                                                    TimeSpan delayTime = dateEnd.AddDays(-1) - DateTime.Now;
                                                    BackgroundJob.Schedule<EmailSender>(x => x.backgroundNotifJob(pmbObj), delayTime);
                                                    BackgroundJob.Schedule<EmailSender>(x => x.Send("Pembelian Update - " + pmbObj.NoInvoicePembelian + " " + GetSupplierName(pmbObj.IdSupplier), pmbObj), delayTime);
                                                    _backgroundJobServer.Dispose();
                                                    //backgroundNotifJob(pmbObj);
                                                    insertToReminder(pmbObj, endDate);
                                                }
                                                Response.Redirect("PembelianPage.aspx", false);
                                            }
                                            catch (Exception ex)
                                            { throw ex; }
                                        }
                                        else
                                        {
                                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Kapasitas Gudang Tidak Mencukupi!" + "');", true);
                                        }
                                    }
                                    else
                                    {
                                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nama Barang Tidak Boleh Sama Dalam 1 Transaksi!" + "');", true);
                                    }
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Seluruh Data Harus Terpenuhi Dahulu!" + "');", true);
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
            }
            else
            {
                int id;
                int.TryParse(Request.QueryString["id"], out id);
                int rowIndex = 0;
                pmDtlList = new List<PembelianDtl>();
                List<PembelianDtl> pmDtlListBaru = new List<PembelianDtl>();
                pmbObj = new PembelianMst();
                pmbObj.IdPembelian = id;
                DateTime tglBeliStr = DateTime.ParseExact(txtCldBrgBeli.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                pmbObj.IdSupplier = Convert.ToInt32(ddlSupName.SelectedValue);
                pmbObj.IdInventory = Convert.ToInt32(ddlNamaGudang.SelectedValue);
                pmbObj.TotalTagihanPembelian = decimal.Parse(Regex.Replace(txtGrandTotal.Text.Trim(), @"[^\d.]", ""));
                pmbObj.TglPembelian = tglBeliStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (ViewState["Curtbl"] != null)
                {
                    if (pmbObj.TotalTagihanPembelian != 0)
                    {
                        DataTable dt = (DataTable)ViewState["Curtbl"];
                        if (checkControlDetail())
                        {
                            for (int j = 0; j < rptPembelianDtl.Items.Count; j++)
                            {
                                PembelianDtl pmbDtlBaru = new PembelianDtl();
                                DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                                TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                                TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                                TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                                double txtQtyItmVal = Convert.ToDouble(txtItemQty.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(txtItemPrice.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                pmbDtlBaru.IdPembelianMst = id;
                                pmbDtlBaru.IdBarang = Convert.ToInt32(ddlNamaBarang.SelectedValue.Trim());
                                //pmbDtlBaru.NamaBarangPembelianDtl = ddlNamaBarang.SelectedValue;
                                pmbDtlBaru.QtyBarangPembelianDtl = txtQtyItmStr;
                                pmbDtlBaru.HargaPembelianDtl = txtPriceItmStr;
                                pmbDtlBaru.TotalHargaPembelianDtl = decimal.Parse(Regex.Replace(txtItemTotalPrice.Text.Trim(), @"[^\d.]", ""));
                                pmbDtlBaru.JumlahBoxPembelianDtl = Convert.ToInt32(txtItemBox.Text.Trim());
                                pmDtlListBaru.Add(pmbDtlBaru);
                                //updateQtyBarangRetur(pmbDtlBaru, pmbObj);
                                rowIndex++;
                            }
                            if (!pmDtlListBaru.GroupBy(a => a.IdBarang).Any(c => c.Count() > 1))
                            {
                                if (checkGudangKapasitas())
                                {
                                    for (int i = 0; i < pmDtlListBaru.Count; i++)
                                    {
                                        //PembelianDtl pmbDtl = new PembelianDtl();
                                        //pmbDtl.IdPembelianMst = id;
                                        //pmbDtl.NamaBarangPembelianDtl = dt.Rows[i]["ddlNamaBarang"].ToString();
                                        //pmbDtl.QtyBarangPembelianDtl = dt.Rows[i]["txtItemQty"].ToString();
                                        //pmbDtl.HargaPembelianDtl = dt.Rows[i]["txtPrice"].ToString();
                                        //pmbDtl.JumlahBoxPembelianDtl = Convert.ToInt32(dt.Rows[i]["txtItemBox"].ToString());
                                        //pmbDtl.TotalHargaPembelianDtl = decimal.Parse(Regex.Replace(dt.Rows[i]["txtTotalPrice"].ToString(), @"[^\d.]", ""));
                                        //pmDtlList.Add(pmbDtl);
                                        updateQtyBarangRetur(pmDtlListBaru[i], pmbObj);
                                    }
                                    Barang brgCounter = new Barang();
                                    sqlConnection = new SqlConnection(connectionString);
                                    try
                                    {
                                        sqlConnection.Open();
                                        for (int i = 0; i < pmDtlListBaru.Count; i++)
                                        {
                                            SqlCommand command = new SqlCommand("insert into POPembelianReturDtl" + "(IdPOPembelianMst,IdBarang,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,HargaPOPembelianReturDtl,TotalHargaPOPembelianReturDtl,JumlahBoxPOPembelianReturDtl,QtyBarangPOPembelianReturDtl) values " + "(" + pmDtlListBaru[i].IdPembelianMst + "," + pmDtlListBaru[i].IdBarang + ",'" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "'," + pmDtlListBaru[i].HargaPembelianDtl + "," + pmDtlListBaru[i].TotalHargaPembelianDtl + "," + pmDtlListBaru[i].JumlahBoxPembelianDtl + "," + pmDtlListBaru[i].QtyBarangPembelianDtl + ")", sqlConnection);
                                            command.ExecuteNonQuery();
                                            //SqlCommand command = new SqlCommand("update POPembelianDtl set QtyBarangPOPembelianDtl=@QtyBarangPOPembelianDtl,JumlahBoxPOPembelianDtl = @JumlahBoxPOPembelianDtl,TotalHargaPOPembelianDtl=@TotalHargaPOPembelianDtl,PembelianDtlUpdatedBy=@PembelianDtlUpdatedBy,PembelianDtlUpdatedDate=@PembelianDtlUpdatedDate where IdPOPembelianMst=@IdPOPembelianMst AND IdBarang=@IdBarang", sqlConnection);
                                            //command.Parameters.AddWithValue("@QtyBarangPOPembelianDtl", pmDtlListBaru[i].QtyBarangPembelianDtl);
                                            //command.Parameters.AddWithValue("@JumlahBoxPOPembelianDtl", pmDtlListBaru[i].JumlahBoxPembelianDtl);
                                            //command.Parameters.AddWithValue("@TotalHargaPOPembelianDtl", pmDtlListBaru[i].TotalHargaPembelianDtl);
                                            //command.Parameters.AddWithValue("@PembelianDtlUpdatedBy", ((Role)Session["Role"]).EmailEmp);
                                            //command.Parameters.AddWithValue("@PembelianDtlUpdatedDate", DateTime.Now.Date.ToString());
                                            //command.Parameters.AddWithValue("@IdPOPembelianMst", pmbObj.IdPembelian);
                                            //command.Parameters.AddWithValue("@IdBarang", pmDtlListBaru[i].IdBarang);
                                            //command.ExecuteNonQuery();
                                            //SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang AND IdSupplier = @IdSupplier AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                            //commNama.Parameters.AddWithValue("@IdBarang", pmDtlListBaru[i].IdBarang);
                                            //commNama.Parameters.AddWithValue("@IdSupplier", pmbObj.IdSupplier);
                                            //commNama.Parameters.AddWithValue("@IdInventory", pmbObj.IdInventory);
                                            //SqlDataReader dr = commNama.ExecuteReader();
                                            //if (dr.Read())
                                            //{
                                            //    brgCounter.IdBarang = (int)dr["IdBarang"];
                                            //    brgCounter.NamaBarang = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                            //    brgCounter.BarangQty = (decimal)dr["BarangQuantity"];
                                            //    brgCounter.BarangCreatedBy = dr.GetString(dr.GetOrdinal("BarangCreatedBy"));
                                            //    brgCounter.BarangCreatedDate = dr.GetString(dr.GetOrdinal("BarangCreatedDate"));
                                            //    brgCounter.BarangUpdateBy = dr.GetString(dr.GetOrdinal("BarangUpdateBy"));
                                            //    brgCounter.BarangUpdateDate = dr.GetString(dr.GetOrdinal("BarangUpdateDate"));
                                            //    //brgCounter.BarangHargaJual = (decimal)dr["BarangHargaJual"];
                                            //    brgCounter.BarangSatuan = dr.GetString(dr.GetOrdinal("BarangSatuan"));
                                            //    brgCounter.IdSupplier = (int)dr["IdSupplier"];
                                            //    brgCounter.BarangStatus = dr.GetString(dr.GetOrdinal("BarangStatus"));
                                            //    //brgCounter.BarangTglBeli = dr.GetString(dr.GetOrdinal("BarangTanggalBeli"));
                                            //    brgCounter.IdInventory = (int)dr["IdInventory"];
                                            //    brgCounter.BarangJmlBox = (int)dr["BarangJmlBox"];
                                            //    brgCounter.BarangItemCode = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                                            //    dr.Close();
                                            //    dr.Dispose();
                                            //}
                                            //SqlCommand strQuerEditBarang = new SqlCommand("update Barang set BarangQuantity=@BarangQuantity,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate,BarangTanggalBeli=@BarangTanggalBeli,BarangJmlBox=@BarangJmlBox where IdBarang=@IdBarang AND IdInventory=@IdInventory AND IdSupplier=@IdSupplier", sqlConnection);
                                            //strQuerEditBarang.Parameters.AddWithValue("@IdBarang", pmDtlListBaru[i].IdBarang);
                                            //strQuerEditBarang.Parameters.AddWithValue("@IdInventory", pmbObj.IdInventory);
                                            //strQuerEditBarang.Parameters.AddWithValue("@IdSupplier", pmbObj.IdSupplier);
                                            //strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                            //strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                            //strQuerEditBarang.Parameters.AddWithValue("@BarangTanggalBeli", pmbObj.TglPembelian);
                                            //strQuerEditBarang.Parameters.AddWithValue("@BarangQuantity", brgCounter.BarangQty + Convert.ToDecimal(pmDtlListBaru[i].QtyBarangPembelianDtl));
                                            //strQuerEditBarang.Parameters.AddWithValue("@BarangJmlBox", brgCounter.BarangJmlBox + Convert.ToDecimal(pmDtlListBaru[i].JumlahBoxPembelianDtl));
                                            //strQuerEditBarang.ExecuteNonQuery();
                                            //updateKapasitasGudangNew(pmDtlListBaru[i], pmbObj);
                                        }
                                        SqlCommand commandPembelianMst = new SqlCommand("update POPembelianMst set StatusInvoicePembelianMst = @StatusInvoicePembelianMst,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate where IdPOPembelianMst=@IdPOPembelianMst", sqlConnection);
                                        commandPembelianMst.Parameters.AddWithValue("@StatusInvoicePembelianMst", "RETUR");
                                        commandPembelianMst.Parameters.AddWithValue("@UpdatedBy", ((Role)Session["Role"]).EmailEmp);
                                        commandPembelianMst.Parameters.AddWithValue("@UpdatedDate", DateTime.Now.Date.ToString());
                                        commandPembelianMst.Parameters.AddWithValue("@IdPOPembelianMst", pmbObj.IdPembelian);
                                        commandPembelianMst.ExecuteNonQuery();
                                        sqlConnection.Close();
                                        sqlConnection.Dispose();
                                        Response.Redirect("PembelianPage.aspx", false);
                                    }
                                    catch (Exception ex)
                                    { throw ex; }
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Kapasitas Gudang Tidak Mencukupi!" + "');", true);
                                }
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Nama Barang Tidak Boleh Sama Dalam 1 Transaksi!" + "');", true);
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Seluruh Data Harus Terpenuhi Dahulu!" + "');", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Seluruh Data Harus Terpenuhi Dahulu!" + "');", true);
                    }
                }
            }
        }

        protected void deleteUpdateBarang(PembelianDtl pmbDtl,PembelianMst pmbMST)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("DELETE FROM POPembelianDtl WHERE IdBarang = "+ pmbDtl.IdBarang+ " AND IdPoPembelianMst = "+pmbMST.IdPembelian+" ", sqlConnection);
                commNama.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        public class EmailSender
        {
            SqlConnection sqlConnection;
            String connectionString;
            [AutomaticRetry(Attempts = 0)]
            public void Send(string msg,PembelianMst pmObj)
            {
                using (MailMessage mm = new MailMessage("yogamandiri.utama@gmail.com", "yogamandiriutama@yahoo.com"))
                {
                    mm.Subject = msg;
                    mm.Body = "<p> " + msg + " Akan Jatuh Tempo Pada Hari Ini dengan total Pembelian " + string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", pmObj.TotalTagihanPembelian) + " dan dikirimkan ke No Rekening " + pmObj.NoRekPembayaran + " Mohon Segera Untuk Di Selesaikan</p><br /><p>Pesan ini di generate otomatis dan dimohon untuk tidak dibalas, Terima Kasih</p>";
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("yogamandiri.utama@gmail.com", "gwgnjpdawyrayohz");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
            }

            [AutomaticRetry(Attempts = 0)]
            public void backgroundNotifJob(PembelianMst pmbObj)
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                try
                {
                    sqlConnection.Open();
                    SqlCommand commNama = new SqlCommand("insert into Notification" + "(SubjectNotification,BodyNotification,TanggalNotification,StatusNotification) values " + "('" + pmbObj.NoInvoicePembelian + "-Pembelian','Pembelian Akan Jatuh Tempo Dalam 1 Hari Kedepan Mohon Segera Di Selesaikan, Terima Kasih','" + pmbObj.TglJatuhTmpPembelian + "','UNREAD')", sqlConnection);
                    commNama.ExecuteNonQuery();
                }
                catch (Exception ex)
                { throw ex; }
                finally { sqlConnection.Close(); sqlConnection.Dispose(); }
            }
        }

        protected void insertToReminder(PembelianMst pmbMst, string endStg)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                DateTime timeStart = new DateTime(Convert.ToInt32(endStg.Substring(6, 4)), Convert.ToInt32(endStg.Substring(0, 2)), Convert.ToInt32(endStg.Substring(3, 2)), 8, 0, 0);
                DateTime timeEnd = new DateTime(Convert.ToInt32(endStg.Substring(6, 4)), Convert.ToInt32(endStg.Substring(0, 2)), Convert.ToInt32(endStg.Substring(3, 2)), 17, 0, 0);
                SqlCommand commNama = new SqlCommand("insert into EventDP" + "(Name,EventStart,EventEnd) values " + "('Pembelian - " + pmbMst.NoInvoicePembelian + " Jatuh Tempo','" + timeStart + "','" + timeEnd + "')", sqlConnection);
                commNama.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void deleteFromReminder(PembelianMst pmbMst)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("DELETE FROM EventDP WHERE Name = 'Pembelian - "+ pmbMst.NoInvoicePembelian.Trim() + " Jatuh Tempo'", sqlConnection);
                commNama.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected bool checkGudangKapasitas()
        {
            bool checkCpct = false;
            int rowIndex = 0;
            int cpctTmp = 0;
            int cpctOldTmp = 0;
            if (ViewState["Curtbl"] != null)
            {
                DataTable dt = (DataTable)ViewState["Curtbl"];
                //BindDDLBarang(dt.Rows.Count);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                        TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                        TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                        TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                        TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                        cpctTmp += Convert.ToInt32(txtItemBox.Text.Trim());
                        cpctOldTmp += Convert.ToInt32(dt.Rows[i]["txtItemBox"].ToString());
                        rowIndex++;
                    }
                    if((Convert.ToInt32(txtSisaKapasitasGudang.Text.Trim()) + cpctOldTmp) - cpctTmp >= 0)
                    {
                        checkCpct = true;
                        return checkCpct;
                    }
                    else
                    {
                        return checkCpct;
                    }
                }
                else
                {
                    return checkCpct;
                }
            }
            else
            {
                return checkCpct;
            }
        }
        protected void updateQtyBarangRetur(PembelianDtl pmbDtlList, PembelianMst pmbMstList)
        {
            int capacityTmp = 0;
            int jmlBoxTmp = 0;
            decimal qtyTmp = 0;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT IvnSisaCapacity FROM Inventory WHERE IdInventory = @IdInventory ", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", pmbMstList.IdInventory);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        capacityTmp = (int)dr["IvnSisaCapacity"];
                        capacityTmp += pmbDtlList.JumlahBoxPembelianDtl;
                        SqlCommand strQuerEditInventory = new SqlCommand("update Inventory set IvnSisaCapacity=@IvnSisaCapacity,IvnUpdateBy=@IvnUpdateBy,IvnUpdateDate=@IvnUpdateDate where IdInventory=@IdInventory", sqlConnection);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnSisaCapacity", capacityTmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateBy", ((Role)Session["Role"]).EmailEmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateDate", DateTime.Now.Date.ToString());
                        strQuerEditInventory.Parameters.AddWithValue("@IdInventory", pmbMstList.IdInventory);
                        strQuerEditInventory.ExecuteNonQuery();
                    }
                }
                SqlCommand CommBrg = new SqlCommand("SELECT BarangQuantity,BarangJmlBox FROM Barang WHERE IdBarang = @IdBarang AND IdSupplier = @IdSupplier AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE'", sqlConnection);
                CommBrg.Parameters.AddWithValue("@IdBarang", pmbDtlList.IdBarang);
                CommBrg.Parameters.AddWithValue("@IdSupplier", pmbMstList.IdSupplier);
                CommBrg.Parameters.AddWithValue("@IdInventory", pmbMstList.IdInventory);
                using (SqlDataReader dr = CommBrg.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        qtyTmp = (decimal)dr["BarangQuantity"];
                        qtyTmp -= Convert.ToDecimal(pmbDtlList.QtyBarangPembelianDtl);
                        jmlBoxTmp = (int)dr["BarangJmlBox"];
                        jmlBoxTmp -= pmbDtlList.JumlahBoxPembelianDtl;
                        SqlCommand strQuerEditInventory = new SqlCommand("UPDATE Barang set BarangQuantity=@BarangQuantity, BarangJmlBox = @BarangJmlBox ,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate where IdBarang=@IdBarang", sqlConnection);
                        strQuerEditInventory.Parameters.AddWithValue("@BarangQuantity", qtyTmp);
                        strQuerEditInventory.Parameters.AddWithValue("@BarangJmlBox", jmlBoxTmp);
                        strQuerEditInventory.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                        strQuerEditInventory.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                        strQuerEditInventory.Parameters.AddWithValue("@IdBarang", pmbDtlList.IdBarang);
                        strQuerEditInventory.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        private void updateKapasitasGudangNew(PembelianDtl pmbDtlList,PembelianMst pmbMstList)
        {
            int capacityTmp = 0;
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT IvnSisaCapacity FROM Inventory WHERE IdInventory = @IdInventory ", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", pmbMstList.IdInventory);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        capacityTmp = (int)dr["IvnSisaCapacity"];
                        capacityTmp -= pmbDtlList.JumlahBoxPembelianDtl;
                        SqlCommand strQuerEditInventory = new SqlCommand("update Inventory set IvnSisaCapacity=@IvnSisaCapacity,IvnUpdateBy=@IvnUpdateBy,IvnUpdateDate=@IvnUpdateDate where IdInventory=@IdInventory", sqlConnection);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnSisaCapacity", capacityTmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateBy", ((Role)Session["Role"]).EmailEmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateDate", DateTime.Now.Date.ToString());
                        strQuerEditInventory.Parameters.AddWithValue("@IdInventory", pmbMstList.IdInventory);
                        strQuerEditInventory.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
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
                            DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                            TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                            TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                            TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                            TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                            //txtItemCode.Text = (Convert.ToInt32(GetItemCode()) + i).ToString();
                            //txtItemName.Text = dt.Rows[i]["txtItemName"].ToString();
                            BindDDLBarang(i, 0);
                            txtItemQty.Text = dt.Rows[i]["txtItemQty"].ToString();
                            txtItemPrice.Text = dt.Rows[i]["txtPrice"].ToString();
                            txtItemBox.Text = dt.Rows[i]["txtItemBox"].ToString();
                            txtItemTotalPrice.Text = dt.Rows[i]["txtTotalPrice"].ToString();
                            rowIndex++;
                        }
                    }
                }
            }
            txtSisaKapasitasGudang.Text = GetSisaKapasitasInv(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()));
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
                            DropDownList ddlNamaBarang = (DropDownList)rptPembelianDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                            TextBox txtItemQty = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemQty");
                            TextBox txtItemPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtPrice");
                            TextBox txtItemBox = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtItemBox");
                            TextBox txtItemTotalPrice = (TextBox)rptPembelianDtl.Items[rowIndex].FindControl("txtTotalPrice");
                            //txtItemCode.Text = (Convert.ToInt32(GetItemCode()) + i).ToString();
                            //txtItemName.Text = dt.Rows[i]["txtItemName"].ToString();
                            BindDDLBarang(i, 0);
                            txtItemQty.Text = dt.Rows[i]["txtItemQty"].ToString();
                            txtItemPrice.Text = dt.Rows[i]["txtPrice"].ToString();
                            txtItemBox.Text = dt.Rows[i]["txtItemBox"].ToString();
                            txtItemTotalPrice.Text = dt.Rows[i]["txtTotalPrice"].ToString();
                            rowIndex++;
                        }
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

        protected void txtItemBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb1 = ((TextBox)(sender));
            RepeaterItem rp1 = ((RepeaterItem)(tb1.NamingContainer));
            DropDownList tb2 = (DropDownList)rp1.FindControl("ddlNamaBarang");
            if (lblStatusPage.Text == "Retur Pembelian" && lblHeaderPage.Text == "Retur Pembelian")
            {
                if (ViewState["Curtbl"] != null)
                {
                    DataTable dt = (DataTable)ViewState["Curtbl"];
                    DataTable dt1 = (DataTable)ViewState["Rtrtbl"];
                    if (tb1.Text.Trim() != "")
                    {
                        var test = (from item in dt1.AsEnumerable()
                                    where item.Field<int>("ddlNamaBarang") == Convert.ToInt32(tb2.SelectedValue.Trim())
                                    select item.Field<string>("txtItemBox")).FirstOrDefault();
                        if (Convert.ToInt32(test) < Convert.ToInt32(tb1.Text.Trim()))
                        {
                            tb1.Text = "0";
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Box Melebihi Nota Pembelian!" + "');", true);
                        }
                        else
                        {
                            if (!checkBoxItem(Convert.ToInt32(tb2.SelectedValue.Trim()), Convert.ToInt32(tb1.Text.Trim())))
                            {
                                tb1.Text = "0";
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Stock Barang!" + "');", true);
                            }
                        }
                    }
                    else
                    {
                        tb1.Text = "0";
                    }
                }
            }
            else
            {
                if (tb1.Text.Trim() == "")
                {
                    tb1.Text = "0";
                }
                if(Convert.ToInt32(tb1.Text.Trim()) == 0)
                {
                    tb1.Text = "0";
                }
            }
        }
        private bool checkBoxItem(int idBarang, int JmlBox)
        {
            bool setBoxItem = false;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT BarangJmlBox FROM Barang WHERE IdBarang = @IdBarang", sqlConnection);
                commNama.Parameters.AddWithValue("@IdBarang", idBarang);
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

        protected void ddlNamaBarang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblStatusPage.Text == "Retur Pembelian" && lblHeaderPage.Text == "Retur Pembelian")
            {
                if (ViewState["Curtbl"] != null)
                {
                    DataTable dt = (DataTable)ViewState["Curtbl"];
                    DataTable dt1 = (DataTable)ViewState["Rtrtbl"];
                    DropDownList ddlNmBrg = ((DropDownList)(sender));
                    RepeaterItem rp1 = ((RepeaterItem)(ddlNmBrg.NamingContainer));
                    TextBox tb1 = (TextBox)rp1.FindControl("txtItemQty");
                    TextBox tb2 = (TextBox)rp1.FindControl("txtPrice");
                    TextBox tb3 = (TextBox)rp1.FindControl("txtItemBox");
                    TextBox tb4 = (TextBox)rp1.FindControl("txtTotalPrice");
                    var test = (from item in dt1.AsEnumerable()
                                where item.Field<int>("ddlNamaBarang") == Convert.ToInt32(ddlNmBrg.SelectedValue.Trim())
                                select item.Field<string>("txtPrice")).FirstOrDefault();
                    tb1.Text = "0";
                    tb2.Text = test;
                    tb3.Text = "0";
                    tb4.Text = "Rp0";
                    txtGrandTotal.Text = "0";
                }
            }
        }
    }
}