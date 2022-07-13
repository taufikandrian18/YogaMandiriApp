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
    public partial class AddPenjualanPage : System.Web.UI.Page
    {
        private BackgroundJobServer _backgroundJobServer;
        PenjualanMst pnjObj;
        List<PenjualanDtl> pnDtlList;
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
                    if (Request.QueryString.AllKeys.Count() > 1)
                    {
                        editText = Request.QueryString["status"];
                    }
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand commNama = new SqlCommand("SELECT * FROM POPenjualanMst WHERE IdPOPenjualanMst = @IdPOPenjualanMst ", sqlConnection);
                        commNama.Parameters.AddWithValue("@IdPOPenjualanMst", id);
                        using (SqlDataReader dr = commNama.ExecuteReader())
                        {

                            string cust = "Select IdCustomer as ID, CustomerName + '-(' + CustomerAddress + ')' as TEXT from Customer WHERE CustomerStatus = 'ACTIVE' ORDER BY CustomerName";
                            //string sup = "Select * from Supplier WHERE SupplierStatus = 'ACTIVE'";
                            string com = "Select * from Inventory WHERE InvStatus = 'ACTIVE'";
                            //SqlDataAdapter supAdpt = new SqlDataAdapter(sup, sqlConnection);
                            SqlDataAdapter custAdpt = new SqlDataAdapter(cust, sqlConnection);
                            SqlDataAdapter adpt = new SqlDataAdapter(com, sqlConnection);
                            DataTable dt = new DataTable();
                            //DataTable dtSup = new DataTable();
                            DataTable dtCust = new DataTable();
                            adpt.Fill(dt);
                            //supAdpt.Fill(dtSup);
                            custAdpt.Fill(dtCust);
                            ddlNamaGudang.DataSource = dt;
                            ddlNamaGudang.DataBind();
                            ddlNamaGudang.DataTextField = "IvnStorage";
                            ddlNamaGudang.DataValueField = "IdInventory";
                            ddlNamaGudang.DataBind();
                            //ddlSupName.DataSource = dtSup;
                            //ddlSupName.DataBind();
                            //ddlSupName.DataTextField = "SupplierName";
                            //ddlSupName.DataValueField = "IdSupplier";
                            //ddlSupName.DataBind();
                            ddlCustName.DataSource = dtCust;
                            ddlCustName.DataBind();
                            ddlCustName.DataTextField = "TEXT";
                            ddlCustName.DataValueField = "ID";
                            ddlCustName.DataBind();
                            if (dr.Read())
                            {
                                pnjObj = new PenjualanMst();
                                pnjObj.IdPOPenjualanMst = id;
                                //var supId = (int)dr["IdSupplier"];
                                //pnjObj.IdSupplier = supId;
                                var strg = (int)dr["IdInventory"];
                                pnjObj.IdInventory = strg;
                                var custVar = (int)dr["IdCustomer"];
                                pnjObj.IdCustomer = custVar;
                                var GTotal = (decimal)dr["TotalTagihanPenjualan"];
                                pnjObj.TotalTagihanPenjualan = GTotal;
                                DateTime tglBeliStr = DateTime.ParseExact(dr.GetString(dr.GetOrdinal("TanggalPOPenjualan")), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                DateTime tglJthTmpStr = DateTime.ParseExact(dr.GetString(dr.GetOrdinal("TanggalPOJatuhTmpPenjualan")), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                txtNoInvoice.Text = dr.GetString(dr.GetOrdinal("NoInvoicePenjualan"));
                                pnjObj.NoInvoicePenjualan = dr.GetString(dr.GetOrdinal("NoInvoicePenjualan"));
                                ddlCustName.SelectedValue = custVar.ToString();
                                //ddlSupName.SelectedValue = supId.ToString();
                                //ddlSupName.Enabled = false;
                                txtCldBrgBeli.Text = tglBeliStr.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                pnjObj.TanggalPOPenjualan = dr.GetString(dr.GetOrdinal("TanggalPOPenjualan"));
                                ddlJenisTagihan.SelectedValue = dr.GetString(dr.GetOrdinal("JenisTagihan"));
                                pnjObj.JenisTagihan = dr.GetString(dr.GetOrdinal("JenisTagihan"));
                                txtCldJtuhTempo.Text = tglJthTmpStr.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                pnjObj.TanggalPOJatuhTmpPenjualan = dr.GetString(dr.GetOrdinal("TanggalPOJatuhTmpPenjualan"));
                                txtBrgNameBank.Text = dr.GetString(dr.GetOrdinal("NoRekeningPembayaran"));
                                pnjObj.NoRekeningPembayaran = dr.GetString(dr.GetOrdinal("NoRekeningPembayaran"));
                                ddlNamaGudang.SelectedValue = strg.ToString();
                                txtGrandTotal.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotal);
                                if (editText.Trim() != "")
                                {
                                    lblHeaderPage.Text = "Edit Penjualan";
                                    lblStatusPage.Text = "Edit Penjualan";
                                }
                                else
                                {
                                    lblHeaderPage.Text = "Retur Penjualan";
                                    lblStatusPage.Text = "Retur Penjualan";
                                    txtNoInvoice.Enabled = false;
                                    ddlCustName.Enabled = false;
                                    txtCldBrgBeli.Enabled = false;
                                    ddlJenisTagihan.Enabled = false;
                                    txtCldJtuhTempo.Enabled = false;
                                    txtBrgNameBank.Enabled = false;
                                    ddlNamaGudang.Enabled = false;
                                    txtGrandTotal.Enabled = false;
                                }
                                //txtSisaKapasitasGudang.Text = GetSisaKapasitasInv(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()));
                                getDtlPenjualan(id, editText.Trim());
                            }
                            else
                            {
                                if (getPenjCode().Trim() == "")
                                {
                                    txtNoInvoice.Text = DateTime.Now.Date.ToString("MM/dd/yy", CultureInfo.InvariantCulture).Replace(@"/", string.Empty) + "001";
                                }
                                else
                                {
                                    txtNoInvoice.Text = getPenjCode();
                                }
                                //txtSisaKapasitasGudang.Text = GetSisaKapasitasInv(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()));
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
        protected void getDtlPenjualan(int id,string qsText)
        {
            DataTable counter = new DataTable();
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT * FROM POPenjualanDtl WHERE IdPOPenjualanMst = @IdPOPenjualanMst", sqlConnection);
                commNama.Parameters.AddWithValue("@IdPOPenjualanMst", id);
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
                            drPmbDtl["txtItemQty"] = dtRowList[i]["QtyBarangPOPenjualanDtl"];
                            drPmbDtl["txtPrice"] = Convert.ToInt32(dtRowList[i]["HargaPOPenjualanDtl"]).ToString();
                            drPmbDtl["txtItemBox"] = dtRowList[i]["JumlahBoxPOPenjualanDtl"];
                            drPmbDtl["txtTotalPrice"] = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", dtRowList[i]["TotalHargaPOPenjualanDtl"]);
                            pmbDtl.Rows.Add(drPmbDtl);
                        }
                        //DataTable dtRtrtbl = new DataTable();
                        //dtRtrtbl.Clear();
                        //dtRtrtbl.Columns.Add("ddlNamaBarang", typeof(int));
                        //dtRtrtbl.Columns.Add("txtItemQty", typeof(string));
                        //dtRtrtbl.Columns.Add("txtPrice", typeof(string));
                        //dtRtrtbl.Columns.Add("txtItemBox", typeof(string));
                        //dtRtrtbl.Columns.Add("txtTotalPrice", typeof(string));
                        //DataRow drRowRtrtbl = dtRtrtbl.NewRow();
                        ////dr["txtItemCode"] = string.Empty;
                        ////dr["txtItemName"] = string.Empty;
                        //drRowRtrtbl["ddlNamaBarang"] = 0;
                        //drRowRtrtbl["txtItemQty"] = string.Empty;
                        //drRowRtrtbl["txtPrice"] = string.Empty;
                        //drRowRtrtbl["txtItemBox"] = string.Empty;
                        //drRowRtrtbl["txtTotalPrice"] = string.Empty;
                        //dtRtrtbl.Rows.Add(drRowRtrtbl);
                        ViewState["Curtbl"] = pmbDtl;
                        ViewState["Rtrtbl"] = pmbDtl;
                        //ViewState["ReturTbl"] = pmbDtl;
                        rptPenjualanDtl.DataSource = pmbDtl;
                        rptPenjualanDtl.DataBind();
                    }
                }
                SetOldDataUpdate(id);
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
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
                        DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                        TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                        TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                        TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                        TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
                        LinkButton lnkBtnAdd = (LinkButton)rptPenjualanDtl.Items[rowIndex].FindControl("btnadd");
                        LinkButton lnkBtnDel = (LinkButton)rptPenjualanDtl.Items[rowIndex].FindControl("btndel");
                        //txtItemCode.Text = (Convert.ToInt32(GetItemCode()) + i).ToString();
                        //txtItemName.Text = dt.Rows[i]["txtItemName"].ToString();
                        //ddlNamaBarang.SelectedIndex = (Convert.ToInt32(dt.Rows[i]["txtItemName"].ToString()));
                        if (dt.Rows[i]["ddlNamaBarang"].ToString().Trim() == "")
                        {
                            BindDDLBarangUpdate(i, 0,id);
                        }
                        else
                        {
                            BindDDLBarangUpdate(i, (Convert.ToInt32(dt.Rows[i]["ddlNamaBarang"].ToString())), id);
                        }
                        //ddlNamaBarang.Enabled = false;
                        txtItemQty.Text = dt.Rows[i]["txtItemQty"].ToString();
                        txtItemPrice.Text = dt.Rows[i]["txtPrice"].ToString();
                        if (lblStatusPage.Text.Trim() == "Retur Penjualan" && lblHeaderPage.Text.Trim() == "Retur Penjualan")
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
        protected void BindDDLBarangUpdate(int numRow, int slctdIndx,int idPenjualanMst)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                if (lblStatusPage.Text.Trim() != "Edit Penjualan" && lblHeaderPage.Text.Trim() != "Edit Penjualan")
                {
                    sqlConnection.Open();
                    DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[numRow].FindControl("ddlNamaBarang");
                    SqlCommand commNama = new SqlCommand("SELECT a.IdBarang as ID, a.NamaBarang + '-(' + b.SupplierName + ') QTY : ('+ CAST(a.BarangQuantity as nvarchar) +') BOX : ('+ CAST(a.BarangJmlBox as nvarchar) +')' as TEXT FROM Barang a INNER JOIN Supplier b on a.IdSupplier = b.IdSupplier INNER JOIN POPenjualanDtl c on c.IdBarang = a.IdBarang  WHERE IdPOPenjualanMst=@IdPOPenjualanMst AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE'", sqlConnection);
                    commNama.Parameters.AddWithValue("@IdInventory", ddlNamaGudang.SelectedValue.Trim());
                    //commNama.Parameters.AddWithValue("@IdSupplier", ddlSupName.SelectedValue.Trim());
                    //commNama.Parameters.AddWithValue("@IdBarang", slctdIndx);
                    commNama.Parameters.AddWithValue("@IdPOPenjualanMst", idPenjualanMst);
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
                    //List<string[]> textDdl = new List<string[]>();
                    sqlConnection.Open();
                    DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[numRow].FindControl("ddlNamaBarang");
                    SqlCommand commNama = new SqlCommand("SELECT a.IdBarang as ID, a.NamaBarang + '-(' + b.SupplierName + ') ,QTY : ('+ CAST(a.BarangQuantity as nvarchar) +') ,BOX : ('+ CAST(a.BarangJmlBox as nvarchar) +')' as TEXT FROM Barang a INNER JOIN Supplier b on a.IdSupplier = b.IdSupplier  WHERE IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' AND a.BarangQuantity != 0 OR a.IdBarang = @IdBarang  ORDER BY NamaBarang", sqlConnection);
                    commNama.Parameters.AddWithValue("@IdInventory", ddlNamaGudang.SelectedValue.Trim());
                    //commNama.Parameters.AddWithValue("@IdSupplier", ddlSupName.SelectedValue.Trim());
                    commNama.Parameters.AddWithValue("@IdBarang", slctdIndx);
                    //commNama.Parameters.AddWithValue("@IdPOPenjualanMst", idPenjualanMst);
                    SqlDataAdapter brgAdapter = new SqlDataAdapter();
                    brgAdapter.SelectCommand = commNama;
                    DataTable dtBrg = new DataTable();
                    brgAdapter.Fill(dtBrg);
                    //string[] words;
                    //for (int i = 0; i < dtBrg.Rows.Count; i++)
                    //{
                    //    words = dtBrg.Rows[i]["TEXT"].ToString().Trim().Split(',');
                    //    textDdl.Add(words);

                    //}
                    if (dtBrg.Rows.Count != 0)
                    {
                        ddlNamaBarang.DataSource = dtBrg;
                        ddlNamaBarang.DataBind();
                        ddlNamaBarang.DataTextField = "TEXT";
                        ddlNamaBarang.DataValueField = "ID";
                        ddlNamaBarang.DataBind();
                        //var item = ddlNamaBarang.Items.FindByValue(slctdIndx.ToString());
                        //if (item != null)
                        //    item.Selected = true;
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
        protected void BindDDLBarang(int numRow, int slctdIndx)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[numRow].FindControl("ddlNamaBarang");
                SqlCommand commNama = new SqlCommand("SELECT a.IdBarang as ID, a.NamaBarang + '-(' + b.SupplierName + ') QTY : ('+ CAST(a.BarangQuantity as nvarchar) +') BOX : ('+ CAST(a.BarangJmlBox as nvarchar) +')' as TEXT FROM Barang a INNER JOIN Supplier b on a.IdSupplier = b.IdSupplier  WHERE IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' AND a.BarangQuantity != 0  ORDER BY NamaBarang", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", ddlNamaGudang.SelectedValue.Trim());
                //commNama.Parameters.AddWithValue("@IdSupplier", ddlSupName.SelectedValue.Trim());
                SqlDataAdapter brgAdapter = new SqlDataAdapter();
                brgAdapter.SelectCommand = commNama;
                DataTable dtBrg = new DataTable();
                brgAdapter.Fill(dtBrg);
                if (dtBrg.Rows.Count != 0)
                {
                    TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[numRow].FindControl("txtItemQty");
                    TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[numRow].FindControl("txtPrice");
                    TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[numRow].FindControl("txtItemBox");
                    ddlNamaBarang.DataSource = dtBrg;
                    ddlNamaBarang.DataBind();
                    ddlNamaBarang.Enabled = true;
                    txtItemPrice.Enabled = true;
                    txtItemQty.Enabled = true;
                    txtItemBox.Enabled = true;
                    ddlNamaBarang.DataTextField = "TEXT";
                    ddlNamaBarang.DataValueField = "ID";
                    ddlNamaBarang.DataBind();
                    if (slctdIndx != 0)
                    {
                        ddlNamaBarang.SelectedValue = slctdIndx.ToString();
                    }

                }
                else
                {
                    BindRepeater();
                    DropDownList ddlNewBarang = (DropDownList)rptPenjualanDtl.Items[0].FindControl("ddlNamaBarang");
                    TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[0].FindControl("txtItemQty");
                    TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[0].FindControl("txtPrice");
                    TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[0].FindControl("txtItemBox");
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
            rptPenjualanDtl.DataSource = dt;
            rptPenjualanDtl.DataBind();

        }

        private bool checkBoxItem(int idInventory,int idSupplier,int JmlBox)
        {
            bool setBoxItem = false;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT BarangJmlBox FROM Barang WHERE IdInventory = @IdInventory AND IdBarang = @IdBarang", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", idInventory);
                commNama.Parameters.AddWithValue("@IdBarang", idSupplier);
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

        private bool checkQtyItem(int idInventory,int idSupplier,decimal qtyBrg)
        {
            bool setBoxItem = false;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT BarangQuantity FROM Barang WHERE IdInventory = @IdInventory AND IdBarang = @IdBarang", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", idInventory);
                commNama.Parameters.AddWithValue("@IdBarang", idSupplier);
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
                        DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                        TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                        TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                        TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                        TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
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
                        txtItemPrice.Text = dt.Rows[i]["txtPrice"].ToString();
                        txtItemBox.Text = dt.Rows[i]["txtItemBox"].ToString();
                        txtItemTotalPrice.Text = dt.Rows[i]["txtTotalPrice"].ToString();
                        rowIndex++;
                    }
                }
            }
        }

        private string GetCustName(int id)
        {
            string custName = "";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT CustomerName FROM Customer WHERE IdCustomer = " + id + "", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        custName = dr.GetString(dr.GetOrdinal("CustomerName"));
                        return custName;
                    }
                    else
                    {
                        return custName;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            DropDownList ddlNamaBarangCheck = (DropDownList)rptPenjualanDtl.Items[0].FindControl("ddlNamaBarang");
            LinkButton btn = (sender as LinkButton);
            RepeaterItem itemCheck = btn.NamingContainer as RepeaterItem;
            int indexCheck = itemCheck.ItemIndex;
            TextBox txtItemQtyCheck = (TextBox)itemCheck.FindControl("txtItemQty");
            TextBox txtItemPriceCheck = (TextBox)itemCheck.FindControl("txtPrice");
            TextBox txtItemBoxCheck = (TextBox)itemCheck.FindControl("txtItemBox");
            DropDownList ddlNamaBarangRpt = (DropDownList)itemCheck.FindControl("ddlNamaBarang");
            if (((DropDownList)rptPenjualanDtl.Items[indexCheck].FindControl("ddlNamaBarang")).Enabled == true)
            {
                if (txtItemQtyCheck.Text.Trim() != "" && txtItemQtyCheck.Text.Trim() != "0" && txtItemPriceCheck.Text.Trim() != "" && txtItemPriceCheck.Text.Trim() != "0" && txtItemBoxCheck.Text.Trim() != "" && txtItemBoxCheck.Text.Trim() != "0")
                {
                    if (checkQtyItem(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()), Convert.ToInt32(ddlNamaBarangRpt.SelectedValue.Trim()), decimal.Parse(txtItemQtyCheck.Text.Trim())))
                    {
                        if (checkBoxItem(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()), Convert.ToInt32(ddlNamaBarangRpt.SelectedValue.Trim()), Convert.ToInt32(txtItemBoxCheck.Text.Trim())))
                        {
                            if (ddlNamaBarangCheck.Items.Count > rptPenjualanDtl.Items.Count || (lblStatusPage.Text.Trim() == "Edit Penjualan" && lblHeaderPage.Text.Trim() == "Edit Penjualan"))
                            {
                                if (lblStatusPage.Text.Trim() != "Retur Penjualan" && lblHeaderPage.Text.Trim() != "Retur Penjualan")
                                {
                                    if (lblStatusPage.Text.Trim() != "Edit Penjualan" && lblHeaderPage.Text.Trim() != "Edit Penjualan")
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
                                                    DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                                    TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                                                    TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                                                    TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                                                    TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
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
                                                rptPenjualanDtl.DataSource = dt;
                                                rptPenjualanDtl.DataBind();
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
                                            DataRow drCurrentRow = null;
                                            if (dt.Rows.Count > 0)
                                            {
                                                for (int i = 1; i <= dt.Rows.Count; i++)
                                                {
                                                    //TextBox txtItemCode = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemCode");
                                                    //TextBox txtItemName = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemName");
                                                    DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                                    TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                                                    TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                                                    TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                                                    TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
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
                                                rptPenjualanDtl.DataSource = dt;
                                                rptPenjualanDtl.DataBind();
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
                                        DataRow drCurrentRow = null;
                                        if (dt.Rows.Count > 0)
                                        {
                                            for (int i = 1; i <= dt.Rows.Count; i++)
                                            {
                                                //TextBox txtItemCode = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemCode");
                                                //TextBox txtItemName = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemName");
                                                DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                                TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                                                TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                                                TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                                                TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
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
                                            rptPenjualanDtl.DataSource = dt;
                                            rptPenjualanDtl.DataBind();
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

        protected void btndel_Click(object sender, EventArgs e)
        {
            RepeaterItem item = (sender as LinkButton).NamingContainer as RepeaterItem;
            LinkButton lnkButton = sender as LinkButton;
            int RepeaterItemIndex = ((RepeaterItem)lnkButton.NamingContainer).ItemIndex;
            if (RepeaterItemIndex != 0)
            {
                TextBox txtItemQtyCheck = (TextBox)rptPenjualanDtl.Items[RepeaterItemIndex - 1].FindControl("txtItemQty");
                TextBox txtItemPriceCheck = (TextBox)rptPenjualanDtl.Items[RepeaterItemIndex - 1].FindControl("txtPrice");
                TextBox txtItemBoxCheck = (TextBox)rptPenjualanDtl.Items[RepeaterItemIndex - 1].FindControl("txtItemBox");
                DropDownList ddlNamaBarangRpt = (DropDownList)rptPenjualanDtl.Items[RepeaterItemIndex - 1].FindControl("ddlNamaBarang");
                int rowIndex = 0;
                int rowID = item.ItemIndex;
                int id;
                int.TryParse(Request.QueryString["id"], out id);
                if (lblStatusPage.Text.Trim() != "Retur Penjualan" && lblHeaderPage.Text.Trim() != "Retur Penjualan")
                {
                    if (lblStatusPage.Text.Trim() != "Edit Penjualan" && lblHeaderPage.Text.Trim() != "Edit Penjualan")
                    {
                        if (ViewState["Curtbl"] != null)
                        {
                            DataTable dt = (DataTable)ViewState["Curtbl"];
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 1; i <= rptPenjualanDtl.Items.Count; i++)
                                {
                                    //TextBox txtItemCode = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemCode");
                                    //TextBox txtItemName = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemName");
                                    DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                    TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                                    TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                                    TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                                    TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
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
                            rptPenjualanDtl.DataSource = dt;
                            rptPenjualanDtl.DataBind();
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
                                for (int i = 1; i <= rptPenjualanDtl.Items.Count; i++)
                                {
                                    //TextBox txtItemCode = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemCode");
                                    //TextBox txtItemName = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemName");
                                    DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                    TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                                    TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                                    TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                                    TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
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
                            rptPenjualanDtl.DataSource = dt;
                            rptPenjualanDtl.DataBind();
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
                            for (int i = 1; i <= rptPenjualanDtl.Items.Count; i++)
                            {
                                //TextBox txtItemCode = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemCode");
                                //TextBox txtItemName = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemName");
                                DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                                TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                                TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                                TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
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
                        rptPenjualanDtl.DataSource = dt;
                        rptPenjualanDtl.DataBind();
                    }
                    SetOldDataUpdate(id);
                    txtTotalPrice_TextChanged(sender, e);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Baris Tidak Dapat Di Delete Lagi!" + "');", true);
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
                        TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
                        counter += decimal.Parse(Regex.Replace(txtItemTotalPrice.Text.Trim(), @"[^\d.]", ""));
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

        private string getPenjCode()
        {
            string itmCode = "";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT TOP 1 * FROM POPenjualanMst ORDER BY IdPOPenjualanMst DESC", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        var brgItemCode = dr.GetString(dr.GetOrdinal("NoInvoicePenjualan"));
                        itmCode = DateTime.Now.Date.ToString("MM/dd/yy", CultureInfo.InvariantCulture).Replace(@"/", string.Empty) + (Convert.ToInt32(brgItemCode.Substring(6, 3)) + 1).ToString().PadRight(4, '0');
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

        private int GetIDPnjMst()
        {
            int idPmbMst = 0;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT TOP 1 * FROM POPenjualanMst ORDER BY IdPOPenjualanMst DESC", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        idPmbMst = (int)dr["IdPOPenjualanMst"];
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
                        //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb3.Text.Trim()) * decimal.Parse(tb1.Text.Trim()));
                    }
                    else
                    {
                        double txtQtyItmVal = Convert.ToDouble(tb3.Text.Trim());
                        double txtPriceItmVal = Convert.ToDouble(tb1.Text.Trim());
                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                        string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                        //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb3.Text.Trim()) * decimal.Parse(tb1.Text.Trim()));
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
                        //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb3.Text.Trim()) * decimal.Parse(tb1.Text.Trim()));
                    }
                    else
                    {
                        double txtQtyItmVal = Convert.ToDouble(tb3.Text.Trim());
                        double txtPriceItmVal = Convert.ToDouble(tb1.Text.Trim());
                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                        string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                        //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb3.Text.Trim()) * decimal.Parse(tb1.Text.Trim()));
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
                        //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb3.Text.Trim()) * decimal.Parse(tb1.Text.Trim()));
                    }
                    else
                    {
                        double txtQtyItmVal = Convert.ToDouble(tb3.Text.Trim());
                        double txtPriceItmVal = Convert.ToDouble(tb1.Text.Trim());
                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                        string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                        //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb3.Text.Trim()) * decimal.Parse(tb1.Text.Trim()));
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
                        //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb3.Text.Trim()) * decimal.Parse(tb1.Text.Trim()));
                    }
                    else
                    {
                        double txtQtyItmVal = Convert.ToDouble(tb3.Text.Trim());
                        double txtPriceItmVal = Convert.ToDouble(tb1.Text.Trim());
                        string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                        string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                        tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                        //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb3.Text.Trim()) * decimal.Parse(tb1.Text.Trim()));
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

        protected void txtItemQty_TextChanged(object sender, EventArgs e)
        {
            TextBox tb1 = ((TextBox)(sender));

            RepeaterItem rp1 = ((RepeaterItem)(tb1.NamingContainer));


            TextBox tb2 = (TextBox)rp1.FindControl("txtTotalPrice");

            TextBox tb3 = (TextBox)rp1.FindControl("txtPrice");
            DropDownList tb4 = (DropDownList)rp1.FindControl("ddlNamaBarang");
            if (lblStatusPage.Text != "Retur Penjualan" && lblHeaderPage.Text != "Retur Penjualan")
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
                            //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                        }
                        else
                        {
                            double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                            double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                            tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                            //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                            if (!checkQtyItem(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()), Convert.ToInt32(tb4.SelectedValue.Trim()), decimal.Parse(txtQtyItmStr)))
                            {
                                tb1.Text = "0";
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Stock yang ada di Gudang!" + "');", true);
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
                            //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                        }
                        else
                        {
                            double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                            double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                            tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                            //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                            if (!checkQtyItem(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()), Convert.ToInt32(tb4.SelectedValue.Trim()), decimal.Parse(txtQtyItmStr)))
                            {
                                tb1.Text = "0";
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Stock yang ada di Gudang!" + "');", true);
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
                            //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                        }
                        else
                        {
                            double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                            double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                            tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                            //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                            if (!checkQtyItem(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()), Convert.ToInt32(tb4.SelectedValue.Trim()), decimal.Parse(txtQtyItmStr)))
                            {
                                tb1.Text = "0";
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Stock yang ada di Gudang!" + "');", true);
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
                            //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                        }
                        else
                        {
                            double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                            double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                            tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                            //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                            if (!checkQtyItem(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()), Convert.ToInt32(tb4.SelectedValue.Trim()), decimal.Parse(txtQtyItmStr)))
                            {
                                tb1.Text = "0";
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Stock yang ada di Gudang!" + "');", true);
                            }
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
                                //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                            }
                            else
                            {
                                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                                //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                                var test = (from item in dt1.AsEnumerable()
                                            where item.Field<int>("ddlNamaBarang") == Convert.ToInt32(tb4.SelectedValue.Trim())
                                            select item.Field<string>("txtItemQty")).FirstOrDefault();
                                if (Convert.ToDecimal(test) < Convert.ToDecimal(tb1.Text.Trim()))
                                {
                                    tb1.Text = "0";
                                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Nota Penjualan!" + "');", true);
                                }
                                else
                                {
                                    tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
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
                                //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                            }
                            else
                            {
                                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                                //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                                var test = (from item in dt1.AsEnumerable()
                                            where item.Field<int>("ddlNamaBarang") == Convert.ToInt32(tb4.SelectedValue.Trim())
                                            select item.Field<string>("txtItemQty")).FirstOrDefault();
                                if (Convert.ToDecimal(test) < Convert.ToDecimal(tb1.Text.Trim()))
                                {
                                    tb1.Text = "0";
                                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Nota Penjualan!" + "');", true);
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
                                //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                            }
                            else
                            {
                                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                                //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                                var test = (from item in dt1.AsEnumerable()
                                           where item.Field<int>("ddlNamaBarang") == Convert.ToInt32(tb4.SelectedValue.Trim())
                                           select item.Field<string>("txtItemQty")).FirstOrDefault();
                                if (Convert.ToDecimal(test) < Convert.ToDecimal(tb1.Text.Trim()))
                                {
                                    tb1.Text = "0";
                                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Nota Penjualan!" + "');", true);
                                }
                                else
                                {
                                    tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
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
                                //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                            }
                            else
                            {
                                double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
                                //tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(tb1.Text.Trim()) * decimal.Parse(tb3.Text.Trim()));
                                var test = (from item in dt1.AsEnumerable()
                                            where item.Field<int>("ddlNamaBarang") == Convert.ToInt32(tb4.SelectedValue.Trim())
                                            select item.Field<string>("txtItemQty")).FirstOrDefault();
                                if (Convert.ToDecimal(test) < Convert.ToDecimal(tb1.Text.Trim()))
                                {
                                    tb1.Text = "0";
                                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Qty Melebihi Nota Penjualan!" + "');", true);
                                }
                                else
                                {
                                    //double txtQtyItmVal = Convert.ToDouble(tb1.Text.Trim());
                                    //double txtPriceItmVal = Convert.ToDouble(tb3.Text.Trim());
                                    //string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                    //string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                    tb2.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", decimal.Parse(txtQtyItmStr) * decimal.Parse(txtPriceItmStr));
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

        public class EmailSender
        {
            SqlConnection sqlConnection;
            String connectionString;
            [AutomaticRetry(Attempts = 0)]
            public void Send(string msg, PenjualanMst pnObj)
            {
                using (MailMessage mm = new MailMessage("yogamandiri.utama@gmail.com", "yogamandiriutama@yahoo.com"))
                {
                    mm.Subject = msg;
                    mm.Body = "<p> " + msg + " Akan Jatuh Tempo Pada Hari Ini dengan total Penjualan " + string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", pnObj.TotalTagihanPenjualan) + " dan dikirimkan ke No Rekening " + pnObj.NoRekeningPembayaran + " Mohon Segera Untuk Di Selesaikan</p><br /><p>Pesan ini di generate otomatis dan dimohon untuk tidak dibalas, Terima Kasih</p>";
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
            public void backgroundNotifJob(PenjualanMst pnObj)
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                try
                {
                    sqlConnection.Open();
                    SqlCommand commNama = new SqlCommand("insert into Notification" + "(SubjectNotification,BodyNotification,TanggalNotification,StatusNotification) values " + "('" + pnObj.NoInvoicePenjualan + "-Penjualan','Penjualan Akan Jatuh Tempo Dalam 1 Hari Kedepan Mohon Segera Di Selesaikan, Terima Kasih','" + pnObj.TanggalPOJatuhTmpPenjualan + "','UNREAD')", sqlConnection);
                    commNama.ExecuteNonQuery();
                }
                catch (Exception ex)
                { throw ex; }
                finally { sqlConnection.Close(); sqlConnection.Dispose(); }
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
                    DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                    TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                    TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                    TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                    TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
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
                if (checkDtl == 1)
                {
                    if (ddlJenisTagihan.SelectedValue.Trim() == "CICILAN")
                    {
                        if (txtCicilanJumlah.Text.Trim() != "")
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
                        return true;
                    }
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
        private void updateKapasitasGudangNew(PenjualanDtl pnjDtlList, PenjualanMst pnjMstList)
        {
            int capacityTmp = 0;
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT IvnSisaCapacity FROM Inventory WHERE IdInventory = @IdInventory ", sqlConnection);
                commNama.Parameters.AddWithValue("@IdInventory", pnjMstList.IdInventory);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        capacityTmp = (int)dr["IvnSisaCapacity"];
                        capacityTmp += pnjDtlList.JumlahBoxPOPenjualanDtl;
                        SqlCommand strQuerEditInventory = new SqlCommand("update Inventory set IvnSisaCapacity=@IvnSisaCapacity,IvnUpdateBy=@IvnUpdateBy,IvnUpdateDate=@IvnUpdateDate where IdInventory=@IdInventory", sqlConnection);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnSisaCapacity", capacityTmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateBy", ((Role)Session["Role"]).EmailEmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateDate", DateTime.Now.Date.ToString());
                        strQuerEditInventory.Parameters.AddWithValue("@IdInventory", pnjMstList.IdInventory);
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
                            DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                            TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                            TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                            TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                            TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
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
            //txtSisaKapasitasGudang.Text = GetSisaKapasitasInv(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()));
        }

        protected void insertToCicilan(PenjualanMst pnjMst)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                double txtPriceItmVal = Convert.ToDouble(txtCicilanJumlah.Text.Trim());
                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                SqlCommand commNama = new SqlCommand("insert into Cicilan" + "(IdPOPenjualanMst,NilaiPembayaran) values " + "('" + pnjMst.IdPOPenjualanMst + "'," + txtPriceItmVal + ")", sqlConnection);
                commNama.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (lblStatusPage.Text.Trim() != "Retur Penjualan" && lblHeaderPage.Text.Trim() != "Retur Penjualan")
            {
                if (lblStatusPage.Text.Trim() != "Edit Penjualan" && lblHeaderPage.Text.Trim() != "Edit Penjualan")
                {
                    DropDownList ddlNamaBarangCheck = (DropDownList)rptPenjualanDtl.Items[0].FindControl("ddlNamaBarang");
                    pnDtlList = new List<PenjualanDtl>();
                    pnjObj = new PenjualanMst();
                    int counterJmlBox = 0;
                    if (ddlNamaBarangCheck.Enabled == true)
                    {
                        int rowIndex = 0;
                        DateTime tglBeliStr = DateTime.ParseExact(txtCldBrgBeli.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        DateTime tglJthTmpStr = DateTime.ParseExact(txtCldJtuhTempo.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        if (ViewState["Curtbl"] != null)
                        {
                            DataTable dt = (DataTable)ViewState["Curtbl"];
                            if (checkControlDetail())
                            {
                                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                                pnjObj.NoInvoicePenjualan = txtNoInvoice.Text.Trim().ToUpper();
                                //pnjObj.IdSupplier = Convert.ToInt32(ddlSupName.SelectedValue.Trim());
                                pnjObj.TanggalPOPenjualan = tglBeliStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                pnjObj.TanggalPOJatuhTmpPenjualan = tglJthTmpStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                if (ddlJenisTagihan.SelectedValue.Trim() == "CICILAN")
                                {
                                    pnjObj.JenisTagihan = ddlJenisTagihan.SelectedValue.Trim() + " " + ddlCicilan.SelectedValue.Trim() + "X";
                                }
                                else
                                {
                                    pnjObj.JenisTagihan = ddlJenisTagihan.SelectedValue.Trim();
                                }
                                pnjObj.NoRekeningPembayaran = txtBrgNameBank.Text.Trim();
                                pnjObj.IdInventory = Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim());
                                pnjObj.IdCustomer = Convert.ToInt32(ddlCustName.SelectedValue.Trim());
                                pnjObj.TotalTagihanPenjualan = decimal.Parse(Regex.Replace(txtGrandTotal.Text.Trim(), @"[^\d.]", ""));
                                if (pnjObj.TotalTagihanPenjualan != 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                        TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                                        TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                                        TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                                        TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
                                        sqlConnection = new SqlConnection(connectionString);
                                        try
                                        {
                                            sqlConnection.Open();
                                            SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                            commNama.Parameters.AddWithValue("@IdBarang", Convert.ToInt32(ddlNamaBarang.SelectedValue.Trim()));
                                            commNama.Parameters.AddWithValue("@IdInventory", Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()));
                                            using (SqlDataReader dr = commNama.ExecuteReader())
                                            {
                                                if (dr.Read())
                                                {
                                                    double txtQtyItmVal = Convert.ToDouble(txtItemQty.Text.Trim());
                                                    double txtPriceItmVal = Convert.ToDouble(txtItemPrice.Text.Trim());
                                                    string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                                    string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                                    PenjualanDtl pnjDtl = new PenjualanDtl();
                                                    pnjDtl.IdPOPenjualanMst = GetIDPnjMst() + 1;
                                                    pnjDtl.IdBarang = (int)dr["IdBarang"];
                                                    pnjDtl.NamaBarangPOPenjualanDtl = dr.GetString(dr.GetOrdinal("NamaBarang"));
                                                    pnjDtl.HargaPOPenjualanDtl = txtPriceItmStr; //harus 2 angka dibelakang koma
                                                    pnjDtl.TotalHargaPOPenjualanDtl = decimal.Parse(Regex.Replace(txtItemTotalPrice.Text.Trim(), @"[^\d.]", ""));
                                                    pnjDtl.JumlahBoxPOPenjualanDtl = Convert.ToInt32(txtItemBox.Text.Trim());
                                                    pnjDtl.QtyBarangPOPenjualanDtl = txtQtyItmStr; //harus 2 angka dibelakang koma
                                                    pnDtlList.Add(pnjDtl);
                                                    counterJmlBox += pnjDtl.JumlahBoxPOPenjualanDtl;
                                                    //tambah proses ngecek kapasitas gudang
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        { throw ex; }
                                        finally { sqlConnection.Close(); sqlConnection.Dispose(); }
                                        rowIndex++;
                                    }
                                    if (!pnDtlList.GroupBy(a => a.NamaBarangPOPenjualanDtl).Any(c => c.Count() > 1))
                                    {
                                        Barang brgCounter = new Barang();
                                        sqlConnection = new SqlConnection(connectionString);
                                        try
                                        {
                                            sqlConnection.Open();
                                            for (int i = 0; i < pnDtlList.Count; i++)
                                            {
                                                SqlCommand command = new SqlCommand("insert into POPenjualanDtl" + "(IdPOPenjualanMst,IdBarang,PenjualanDtlCreatedBy,PenjualanDtlCreatedDate,PenjualanDtlUpdatedBy,PenjualanDtlUpdatedDate,HargaPOPenjualanDtl,TotalHargaPOPenjualanDtl,JumlahBoxPOPenjualanDtl,QtyBarangPOPenjualanDtl) values " + "(" + pnDtlList[i].IdPOPenjualanMst + "," + pnDtlList[i].IdBarang + ",'" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "'," + pnDtlList[i].HargaPOPenjualanDtl + "," + pnDtlList[i].TotalHargaPOPenjualanDtl + "," + pnDtlList[i].JumlahBoxPOPenjualanDtl + "," + pnDtlList[i].QtyBarangPOPenjualanDtl + ")", sqlConnection);
                                                command.ExecuteNonQuery();
                                                SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                                commNama.Parameters.AddWithValue("@IdBarang", pnDtlList[i].IdBarang);
                                                //commNama.Parameters.AddWithValue("@IdSupplier", pnjObj.IdSupplier);
                                                commNama.Parameters.AddWithValue("@IdInventory", pnjObj.IdInventory);
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
                                                SqlCommand strQuerEditBarang = new SqlCommand("update Barang set BarangQuantity=@BarangQuantity,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate,BarangTanggalBeli=@BarangTanggalBeli,BarangJmlBox=@BarangJmlBox where IdBarang=@IdBarang AND IdInventory=@IdInventory", sqlConnection);
                                                strQuerEditBarang.Parameters.AddWithValue("@IdBarang", pnDtlList[i].IdBarang);
                                                strQuerEditBarang.Parameters.AddWithValue("@IdInventory", pnjObj.IdInventory);
                                                //strQuerEditBarang.Parameters.AddWithValue("@IdSupplier", pnjObj.IdSupplier);
                                                strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                                strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                                strQuerEditBarang.Parameters.AddWithValue("@BarangTanggalBeli", pnjObj.TanggalPOPenjualan);
                                                strQuerEditBarang.Parameters.AddWithValue("@BarangQuantity", brgCounter.BarangQty - Convert.ToDecimal(pnDtlList[i].QtyBarangPOPenjualanDtl));
                                                strQuerEditBarang.Parameters.AddWithValue("@BarangJmlBox", brgCounter.BarangJmlBox - Convert.ToDecimal(pnDtlList[i].JumlahBoxPOPenjualanDtl));
                                                strQuerEditBarang.ExecuteNonQuery();
                                                updateKapasitasGudangNew(pnDtlList[i], pnjObj);
                                            }
                                            SqlCommand commandPembelianMst = new SqlCommand("insert into POPenjualanMst" + "(TanggalPOPenjualan,JenisTagihan,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,NoInvoicePenjualan,TanggalPOJatuhTmpPenjualan,TotalTagihanPenjualan,NoRekeningPembayaran,IdInventory,IdCustomer,StatusInvoicePenjualan) values " + "('" + pnjObj.TanggalPOPenjualan + "','" + pnjObj.JenisTagihan + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + pnjObj.NoInvoicePenjualan + "','" + pnjObj.TanggalPOJatuhTmpPenjualan + "'," + pnjObj.TotalTagihanPenjualan + ",'" + pnjObj.NoRekeningPembayaran.ToUpper() + "'," + pnjObj.IdInventory + "," + pnjObj.IdCustomer + ",'NORMAL')", sqlConnection);
                                            commandPembelianMst.ExecuteNonQuery();
                                            sqlConnection.Close();
                                            sqlConnection.Dispose();
                                            if (pnjObj.JenisTagihan.Trim() == "KONTRABON" || pnjObj.JenisTagihan.Trim() == "TRANSFER" || pnjObj.JenisTagihan.Trim() == "GIRO")
                                            {
                                                string startDate = txtCldBrgBeli.Text.Trim();
                                                string startTime = String.Format("{0:HH:mm:ss}", DateTime.Now);
                                                DateTime dateStart = new DateTime(Convert.ToInt32(startDate.Substring(6, 4)), Convert.ToInt32(startDate.Substring(0, 2)), Convert.ToInt32(startDate.Substring(3, 2)), Convert.ToInt32(startTime.Substring(0, 2)), Convert.ToInt32(startTime.Substring(3, 2)), Convert.ToInt32(startTime.Substring(6, 2)));
                                                string endDate = txtCldJtuhTempo.Text.Trim();
                                                string endTime = String.Format("{0:HH:mm:ss}", DateTime.Now.AddMinutes(15));
                                                DateTime dateEnd = new DateTime(Convert.ToInt32(endDate.Substring(6, 4)), Convert.ToInt32(endDate.Substring(0, 2)), Convert.ToInt32(endDate.Substring(3, 2)), Convert.ToInt32(endTime.Substring(0, 2)), Convert.ToInt32(endTime.Substring(3, 2)), Convert.ToInt32(endTime.Substring(6, 2)));
                                                _backgroundJobServer = new BackgroundJobServer();
                                                TimeSpan delayTime = dateEnd.AddDays(-1) - DateTime.Now;
                                                BackgroundJob.Schedule<EmailSender>(x => x.backgroundNotifJob(pnjObj), delayTime);
                                                BackgroundJob.Schedule<EmailSender>(x => x.Send("Penjualan - " + pnjObj.NoInvoicePenjualan + " " + GetCustName(pnjObj.IdCustomer), pnjObj), delayTime);
                                                _backgroundJobServer.Dispose();
                                                //backgroundNotifJob(pmbObj);
                                                insertToReminder(pnjObj, endDate);
                                            }
                                            if (pnjObj.JenisTagihan.Trim() == "CICILAN")
                                            {
                                                insertToCicilan(pnjObj);
                                                string startDate = txtCldBrgBeli.Text.Trim();
                                                string startTime = String.Format("{0:HH:mm:ss}", DateTime.Now);
                                                DateTime dateStart = new DateTime(Convert.ToInt32(startDate.Substring(6, 4)), Convert.ToInt32(startDate.Substring(0, 2)), Convert.ToInt32(startDate.Substring(3, 2)), Convert.ToInt32(startTime.Substring(0, 2)), Convert.ToInt32(startTime.Substring(3, 2)), Convert.ToInt32(startTime.Substring(6, 2)));
                                                string endDate = txtCldJtuhTempo.Text.Trim();
                                                string endTime = String.Format("{0:HH:mm:ss}", DateTime.Now.AddMinutes(15));
                                                DateTime dateEnd = new DateTime(Convert.ToInt32(endDate.Substring(6, 4)), Convert.ToInt32(endDate.Substring(0, 2)), Convert.ToInt32(endDate.Substring(3, 2)), Convert.ToInt32(endTime.Substring(0, 2)), Convert.ToInt32(endTime.Substring(3, 2)), Convert.ToInt32(endTime.Substring(6, 2)));
                                                TimeSpan delayTime = dateEnd.AddDays(-1) - DateTime.Now;
                                                for (int i = 0; i < Convert.ToInt32(ddlCicilan.SelectedValue.Trim()); i++)
                                                {
                                                    int countCicilan = Convert.ToInt32(ddlCicilan.SelectedValue.Trim());
                                                    _backgroundJobServer = new BackgroundJobServer();
                                                    BackgroundJob.Schedule<EmailSender>(x => x.backgroundNotifJob(pnjObj), new TimeSpan(delayTime.Ticks / countCicilan));
                                                    BackgroundJob.Schedule<EmailSender>(x => x.Send("Penjualan Cicilan ke " + i + 1 + " - " + pnjObj.NoInvoicePenjualan + " " + GetCustName(pnjObj.IdCustomer), pnjObj), new TimeSpan(delayTime.Ticks / countCicilan));
                                                    _backgroundJobServer.Dispose();
                                                    insertToReminder(pnjObj, endDate);
                                                    countCicilan--;
                                                }
                                            }
                                            Response.Redirect("PenjualanPage.aspx", false);
                                        }
                                        catch (Exception ex)
                                        { throw ex; }
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
                    int id;
                    int.TryParse(Request.QueryString["id"], out id);
                    int rowIndex = 0;
                    DropDownList ddlNamaBarangCheck = (DropDownList)rptPenjualanDtl.Items[0].FindControl("ddlNamaBarang");
                    pnDtlList = new List<PenjualanDtl>();
                    pnjObj = new PenjualanMst();
                    int counterJmlBox = 0;
                    if (ddlNamaBarangCheck.Enabled == true)
                    {
                        DateTime tglBeliStr = DateTime.ParseExact(txtCldBrgBeli.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        DateTime tglJthTmpStr = DateTime.ParseExact(txtCldJtuhTempo.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        if (ViewState["Curtbl"] != null)
                        {
                            DataTable dt = (DataTable)ViewState["Curtbl"];
                            if (checkControlDetail())
                            {
                                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                                pnjObj.NoInvoicePenjualan = txtNoInvoice.Text.Trim().ToUpper();
                                //pnjObj.IdSupplier = Convert.ToInt32(ddlSupName.SelectedValue.Trim());
                                pnjObj.IdPOPenjualanMst = id;
                                pnjObj.TanggalPOPenjualan = tglBeliStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                pnjObj.TanggalPOJatuhTmpPenjualan = tglJthTmpStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                if(ddlJenisTagihan.SelectedValue.Trim() == "CICILAN")
                                {
                                    pnjObj.JenisTagihan = ddlJenisTagihan.SelectedValue.Trim() + " " + ddlCicilan.SelectedValue.Trim() + "X";
                                }
                                else
                                {
                                    pnjObj.JenisTagihan = ddlJenisTagihan.SelectedValue.Trim();
                                }
                                pnjObj.NoRekeningPembayaran = txtBrgNameBank.Text.Trim();
                                pnjObj.IdInventory = Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim());
                                pnjObj.IdCustomer = Convert.ToInt32(ddlCustName.SelectedValue.Trim());
                                pnjObj.TotalTagihanPenjualan = decimal.Parse(Regex.Replace(txtGrandTotal.Text.Trim(), @"[^\d.]", ""));
                                if (pnjObj.TotalTagihanPenjualan != 0)
                                {
                                    for (int j = 0; j < rptPenjualanDtl.Items.Count; j++)
                                    {
                                        PenjualanDtl pnjDtlBaru = new PenjualanDtl();
                                        DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                        TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                                        TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                                        TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                                        TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
                                        sqlConnection = new SqlConnection(connectionString);
                                        try
                                        {
                                            sqlConnection.Open();
                                            SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                            commNama.Parameters.AddWithValue("@IdBarang", Convert.ToInt32(ddlNamaBarang.SelectedValue.Trim()));
                                            commNama.Parameters.AddWithValue("@IdInventory", Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()));
                                            using (SqlDataReader dr = commNama.ExecuteReader())
                                            {
                                                if (dr.Read())
                                                {
                                                    double txtQtyItmVal = Convert.ToDouble(txtItemQty.Text.Trim());
                                                    double txtPriceItmVal = Convert.ToDouble(txtItemPrice.Text.Trim());
                                                    string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                                    string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                                    pnjDtlBaru.IdPOPenjualanMst = id;
                                                    pnjDtlBaru.IdBarang = Convert.ToInt32(ddlNamaBarang.SelectedValue);
                                                    //pnjDtlBaru.NamaBarangPOPenjualanDtl = ddlNamaBarang.SelectedValue;
                                                    pnjDtlBaru.QtyBarangPOPenjualanDtl = txtQtyItmStr;
                                                    pnjDtlBaru.HargaPOPenjualanDtl = txtPriceItmStr;
                                                    pnjDtlBaru.TotalHargaPOPenjualanDtl = decimal.Parse(Regex.Replace(txtItemTotalPrice.Text.Trim(), @"[^\d.]", ""));
                                                    pnjDtlBaru.JumlahBoxPOPenjualanDtl = Convert.ToInt32(txtItemBox.Text.Trim());
                                                    pnDtlList.Add(pnjDtlBaru);
                                                    counterJmlBox += pnjDtlBaru.JumlahBoxPOPenjualanDtl;
                                                    //updateQtyBarangRetur(pnjDtlBaru, pnjObj);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        { throw ex; }
                                        finally { sqlConnection.Close(); sqlConnection.Dispose(); }
                                        rowIndex++;
                                    }
                                    if (!pnDtlList.GroupBy(a => a.IdBarang).Any(c => c.Count() > 1))
                                    {
                                        DataTable dt1 = (DataTable)ViewState["Rtrtbl"];
                                        for (int i = 0; i < dt1.Rows.Count; i++)
                                        {
                                            PenjualanDtl pnjDtl = new PenjualanDtl();
                                            double txtQtyItmVal = Convert.ToDouble(dt1.Rows[i]["txtItemQty"].ToString());
                                            double txtPriceItmVal = Convert.ToDouble(dt1.Rows[i]["txtPrice"].ToString());
                                            string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                            string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                            pnjDtl.IdPOPenjualanMst = id;
                                            pnjDtl.IdBarang = Convert.ToInt32(dt1.Rows[i]["ddlNamaBarang"].ToString());
                                            pnjDtl.QtyBarangPOPenjualanDtl = txtQtyItmStr;
                                            pnjDtl.HargaPOPenjualanDtl = txtPriceItmStr;
                                            pnjDtl.JumlahBoxPOPenjualanDtl = Convert.ToInt32(dt1.Rows[i]["txtItemBox"].ToString());
                                            pnjDtl.TotalHargaPOPenjualanDtl = decimal.Parse(Regex.Replace(dt1.Rows[i]["txtTotalPrice"].ToString(), @"[^\d.]", ""));
                                            updateQtyBarangRetur(pnjDtl, pnjObj);
                                            deleteUpdateBarang(pnjDtl, pnjObj);
                                        }
                                        //deleteCicilan if cicilan
                                        //if(ddlJenisTagihan.SelectedValue.Trim() != "CICILAN")
                                        //{
                                        //    deleteFromCicilan(pnjObj);
                                        //}
                                        Barang brgCounter = new Barang();
                                        sqlConnection = new SqlConnection(connectionString);
                                        try
                                        {
                                            sqlConnection.Open();
                                            for (int i = 0; i < pnDtlList.Count; i++)
                                            {
                                                SqlCommand command = new SqlCommand("insert into POPenjualanDtl" + "(IdPOPenjualanMst,IdBarang,PenjualanDtlCreatedBy,PenjualanDtlCreatedDate,PenjualanDtlUpdatedBy,PenjualanDtlUpdatedDate,HargaPOPenjualanDtl,TotalHargaPOPenjualanDtl,JumlahBoxPOPenjualanDtl,QtyBarangPOPenjualanDtl) values " + "(" + pnDtlList[i].IdPOPenjualanMst + "," + pnDtlList[i].IdBarang + ",'" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "'," + pnDtlList[i].HargaPOPenjualanDtl + "," + pnDtlList[i].TotalHargaPOPenjualanDtl + "," + pnDtlList[i].JumlahBoxPOPenjualanDtl + "," + pnDtlList[i].QtyBarangPOPenjualanDtl + ")", sqlConnection);
                                                command.ExecuteNonQuery();
                                                SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                                commNama.Parameters.AddWithValue("@IdBarang", pnDtlList[i].IdBarang);
                                                commNama.Parameters.AddWithValue("@IdInventory", pnjObj.IdInventory);
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
                                                    brgCounter.BarangSatuan = dr.GetString(dr.GetOrdinal("BarangSatuan"));
                                                    brgCounter.IdSupplier = (int)dr["IdSupplier"];
                                                    brgCounter.BarangStatus = dr.GetString(dr.GetOrdinal("BarangStatus"));
                                                    brgCounter.IdInventory = (int)dr["IdInventory"];
                                                    brgCounter.BarangJmlBox = (int)dr["BarangJmlBox"];
                                                    brgCounter.BarangItemCode = dr.GetString(dr.GetOrdinal("BarangItemCode"));
                                                    dr.Close();
                                                    dr.Dispose();
                                                }
                                                SqlCommand strQuerEditBarang = new SqlCommand("update Barang set BarangQuantity=@BarangQuantity,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate,BarangTanggalBeli=@BarangTanggalBeli,BarangJmlBox=@BarangJmlBox where IdBarang=@IdBarang AND IdInventory=@IdInventory", sqlConnection);
                                                strQuerEditBarang.Parameters.AddWithValue("@IdBarang", pnDtlList[i].IdBarang);
                                                strQuerEditBarang.Parameters.AddWithValue("@IdInventory", pnjObj.IdInventory);
                                                strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                                strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                                strQuerEditBarang.Parameters.AddWithValue("@BarangTanggalBeli", pnjObj.TanggalPOPenjualan);
                                                strQuerEditBarang.Parameters.AddWithValue("@BarangQuantity", brgCounter.BarangQty - Convert.ToDecimal(pnDtlList[i].QtyBarangPOPenjualanDtl));
                                                strQuerEditBarang.Parameters.AddWithValue("@BarangJmlBox", brgCounter.BarangJmlBox - Convert.ToDecimal(pnDtlList[i].JumlahBoxPOPenjualanDtl));
                                                strQuerEditBarang.ExecuteNonQuery();
                                                updateKapasitasGudangNew(pnDtlList[i], pnjObj);
                                            }
                                            if(checkCicilan(pnjObj.IdPOPenjualanMst))
                                            {
                                                deleteFromCicilan(pnjObj);
                                            }
                                            sqlConnection = new SqlConnection(connectionString);
                                            sqlConnection.Open();
                                            SqlCommand commandPembelianMst = new SqlCommand("update POPenjualanMst set NoInvoicePenjualan=@NoInvoicePenjualan,IdCustomer=@IdCustomer,TanggalPOPenjualan=@TanggalPOPenjualan,JenisTagihan=@JenisTagihan,TanggalPOJatuhTmpPenjualan=@TanggalPOJatuhTmpPenjualan,NoRekeningPembayaran=@NoRekeningPembayaran,IdInventory=@IdInventory,TotalTagihanPenjualan=@TotalTagihanPenjualan,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate where IdPOPenjualanMst=@IdPOPenjualanMst", sqlConnection);
                                            commandPembelianMst.Parameters.AddWithValue("@NoInvoicePenjualan", pnjObj.NoInvoicePenjualan);
                                            commandPembelianMst.Parameters.AddWithValue("@IdCustomer", pnjObj.IdCustomer);
                                            commandPembelianMst.Parameters.AddWithValue("@TanggalPOPenjualan", pnjObj.TanggalPOPenjualan);
                                            commandPembelianMst.Parameters.AddWithValue("@JenisTagihan", pnjObj.JenisTagihan);
                                            commandPembelianMst.Parameters.AddWithValue("@TanggalPOJatuhTmpPenjualan", pnjObj.TanggalPOJatuhTmpPenjualan);
                                            commandPembelianMst.Parameters.AddWithValue("@NoRekeningPembayaran", pnjObj.NoRekeningPembayaran);
                                            commandPembelianMst.Parameters.AddWithValue("@IdInventory", pnjObj.IdInventory);
                                            commandPembelianMst.Parameters.AddWithValue("@TotalTagihanPenjualan", pnjObj.TotalTagihanPenjualan);
                                            commandPembelianMst.Parameters.AddWithValue("@UpdatedBy", ((Role)Session["Role"]).EmailEmp);
                                            commandPembelianMst.Parameters.AddWithValue("@UpdatedDate", DateTime.Now.Date.ToString());
                                            commandPembelianMst.Parameters.AddWithValue("@IdPOPenjualanMst", pnjObj.IdPOPenjualanMst);
                                            commandPembelianMst.ExecuteNonQuery();
                                            sqlConnection.Close();
                                            sqlConnection.Dispose();
                                            if (pnjObj.JenisTagihan.Trim() == "KONTRABON" || pnjObj.JenisTagihan.Trim() == "TRANSFER" || pnjObj.JenisTagihan.Trim() == "GIRO")
                                            {
                                                deleteFromReminder(pnjObj);
                                                string startDate = txtCldBrgBeli.Text.Trim();
                                                string startTime = String.Format("{0:HH:mm:ss}", DateTime.Now);
                                                DateTime dateStart = new DateTime(Convert.ToInt32(startDate.Substring(6, 4)), Convert.ToInt32(startDate.Substring(0, 2)), Convert.ToInt32(startDate.Substring(3, 2)), Convert.ToInt32(startTime.Substring(0, 2)), Convert.ToInt32(startTime.Substring(3, 2)), Convert.ToInt32(startTime.Substring(6, 2)));
                                                string endDate = txtCldJtuhTempo.Text.Trim();
                                                string endTime = String.Format("{0:HH:mm:ss}", DateTime.Now.AddMinutes(15));
                                                DateTime dateEnd = new DateTime(Convert.ToInt32(endDate.Substring(6, 4)), Convert.ToInt32(endDate.Substring(0, 2)), Convert.ToInt32(endDate.Substring(3, 2)), Convert.ToInt32(endTime.Substring(0, 2)), Convert.ToInt32(endTime.Substring(3, 2)), Convert.ToInt32(endTime.Substring(6, 2)));
                                                _backgroundJobServer = new BackgroundJobServer();
                                                TimeSpan delayTime = dateEnd.AddDays(-1) - DateTime.Now;
                                                BackgroundJob.Schedule<EmailSender>(x => x.backgroundNotifJob(pnjObj), delayTime);
                                                BackgroundJob.Schedule<EmailSender>(x => x.Send("Penjualan Update - " + pnjObj.NoInvoicePenjualan + " " + GetCustName(pnjObj.IdCustomer), pnjObj), delayTime);
                                                _backgroundJobServer.Dispose();
                                                //backgroundNotifJob(pmbObj);
                                                insertToReminder(pnjObj, endDate);
                                            }
                                            if(pnjObj.JenisTagihan.Trim() == "CICILAN")
                                            {
                                                insertToCicilan(pnjObj);
                                                string startDate = txtCldBrgBeli.Text.Trim();
                                                string startTime = String.Format("{0:HH:mm:ss}", DateTime.Now);
                                                DateTime dateStart = new DateTime(Convert.ToInt32(startDate.Substring(6, 4)), Convert.ToInt32(startDate.Substring(0, 2)), Convert.ToInt32(startDate.Substring(3, 2)), Convert.ToInt32(startTime.Substring(0, 2)), Convert.ToInt32(startTime.Substring(3, 2)), Convert.ToInt32(startTime.Substring(6, 2)));
                                                string endDate = txtCldJtuhTempo.Text.Trim();
                                                string endTime = String.Format("{0:HH:mm:ss}", DateTime.Now.AddMinutes(15));
                                                DateTime dateEnd = new DateTime(Convert.ToInt32(endDate.Substring(6, 4)), Convert.ToInt32(endDate.Substring(0, 2)), Convert.ToInt32(endDate.Substring(3, 2)), Convert.ToInt32(endTime.Substring(0, 2)), Convert.ToInt32(endTime.Substring(3, 2)), Convert.ToInt32(endTime.Substring(6, 2)));
                                                TimeSpan delayTime = dateEnd.AddDays(-1) - DateTime.Now;
                                                for (int i = 0; i < Convert.ToInt32(ddlCicilan.SelectedValue.Trim()); i++)
                                                {
                                                    int countCicilan = Convert.ToInt32(ddlCicilan.SelectedValue.Trim());
                                                    deleteFromReminder(pnjObj);
                                                    _backgroundJobServer = new BackgroundJobServer();
                                                    BackgroundJob.Schedule<EmailSender>(x => x.backgroundNotifJob(pnjObj), new TimeSpan(delayTime.Ticks / countCicilan));
                                                    BackgroundJob.Schedule<EmailSender>(x => x.Send("Penjualan Update Cicilan ke "+ i + 1 + " - " + pnjObj.NoInvoicePenjualan + " " + GetCustName(pnjObj.IdCustomer), pnjObj), new TimeSpan(delayTime.Ticks / countCicilan));
                                                    _backgroundJobServer.Dispose();
                                                    insertToReminder(pnjObj, endDate);
                                                    countCicilan--;
                                                }
                                            }
                                            Response.Redirect("PenjualanPage.aspx", false);
                                        }
                                        catch (Exception ex)
                                        { throw ex; }
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
                pnDtlList = new List<PenjualanDtl>();
                List<PenjualanDtl> pnDtlListBaru = new List<PenjualanDtl>();
                pnjObj = new PenjualanMst();
                pnjObj.IdPOPenjualanMst = id;
                DateTime tglBeliStr = DateTime.ParseExact(txtCldBrgBeli.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime tglJualJthTmp = DateTime.ParseExact(txtCldJtuhTempo.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                pnjObj.IdCustomer = Convert.ToInt32(ddlCustName.SelectedValue);
                //pnjObj.IdSupplier = Convert.ToInt32(ddlSupName.SelectedValue);
                pnjObj.IdInventory = Convert.ToInt32(ddlNamaGudang.SelectedValue);
                pnjObj.NoRekeningPembayaran = txtBrgNameBank.Text.Trim();
                pnjObj.JenisTagihan = ddlJenisTagihan.SelectedValue;
                pnjObj.TotalTagihanPenjualan = decimal.Parse(Regex.Replace(txtGrandTotal.Text.Trim(), @"[^\d.]", ""));
                pnjObj.TanggalPOJatuhTmpPenjualan = tglJualJthTmp.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                pnjObj.TanggalPOPenjualan = tglBeliStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (ViewState["Curtbl"] != null)
                {
                    if (pnjObj.TotalTagihanPenjualan != 0)
                    {
                        DataTable dt = (DataTable)ViewState["Curtbl"];
                        if (checkControlDetail())
                        {
                            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                            for (int j = 0; j < rptPenjualanDtl.Items.Count; j++)
                            {
                                PenjualanDtl pnjDtlBaru = new PenjualanDtl();
                                DropDownList ddlNamaBarang = (DropDownList)rptPenjualanDtl.Items[rowIndex].FindControl("ddlNamaBarang");
                                TextBox txtItemQty = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemQty");
                                TextBox txtItemPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtPrice");
                                TextBox txtItemBox = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtItemBox");
                                TextBox txtItemTotalPrice = (TextBox)rptPenjualanDtl.Items[rowIndex].FindControl("txtTotalPrice");
                                double txtQtyItmVal = Convert.ToDouble(txtItemQty.Text.Trim());
                                double txtPriceItmVal = Convert.ToDouble(txtItemPrice.Text.Trim());
                                string txtQtyItmStr = string.Format("{0:0.00}", txtQtyItmVal);
                                string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                pnjDtlBaru.IdPOPenjualanMst = id;
                                pnjDtlBaru.IdBarang = Convert.ToInt32(ddlNamaBarang.SelectedValue);
                                //pnjDtlBaru.NamaBarangPOPenjualanDtl = ddlNamaBarang.SelectedValue;
                                pnjDtlBaru.QtyBarangPOPenjualanDtl = txtQtyItmStr;
                                pnjDtlBaru.HargaPOPenjualanDtl = txtPriceItmStr;
                                pnjDtlBaru.TotalHargaPOPenjualanDtl = decimal.Parse(Regex.Replace(txtItemTotalPrice.Text.Trim(), @"[^\d.]", ""));
                                pnjDtlBaru.JumlahBoxPOPenjualanDtl = Convert.ToInt32(txtItemBox.Text.Trim());
                                pnDtlListBaru.Add(pnjDtlBaru);
                                //updateQtyBarangRetur(pnjDtlBaru, pnjObj);
                                rowIndex++;
                            }
                            if (!pnDtlListBaru.GroupBy(a => a.IdBarang).Any(c => c.Count() > 1))
                            {
                                for (int i = 0; i < pnDtlListBaru.Count; i++)
                                {
                                    updateQtyBarangRetur(pnDtlListBaru[i], pnjObj);
                                    rowIndex++;
                                    //updateQtyBarangRetur(pnjDtl, pnjObj);
                                }
                                Barang brgCounter = new Barang();
                                sqlConnection = new SqlConnection(connectionString);
                                try
                                {
                                    sqlConnection.Open();
                                    for (int i = 0; i < pnDtlListBaru.Count; i++)
                                    {
                                        SqlCommand command = new SqlCommand("insert into POPenjualanReturDtl" + "(IdPOPenjualanMst,IdBarang,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,HargaPOPenjualanReturDtl,TotalHargaPOPenjualanReturDtl,JumlahBoxPOPenjualanReturDtl,QtyBarangPOPenjualanReturDtl) values " + "(" + pnDtlListBaru[i].IdPOPenjualanMst + "," + pnDtlListBaru[i].IdBarang + ",'" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "','" + ((Role)Session["Role"]).EmailEmp + "','" + DateTime.Now.Date.ToString() + "'," + pnDtlListBaru[i].HargaPOPenjualanDtl + "," + pnDtlListBaru[i].TotalHargaPOPenjualanDtl + "," + pnDtlListBaru[i].JumlahBoxPOPenjualanDtl + "," + pnDtlListBaru[i].QtyBarangPOPenjualanDtl + ")", sqlConnection);
                                        command.ExecuteNonQuery();
                                        //SqlCommand command = new SqlCommand("update POPenjualanDtl set QtyBarangPOPenjualanDtl=@QtyBarangPOPenjualanDtl,JumlahBoxPOPenjualanDtl = @JumlahBoxPOPenjualanDtl,TotalHargaPOPenjualanDtl=@TotalHargaPOPenjualanDtl,PenjualanDtlUpdatedBy=@PenjualanDtlUpdatedBy,PenjualanDtlUpdatedDate=@PenjualanDtlUpdatedDate where IdPOPenjualanMst=@IdPOPenjualanMst AND IdBarang=@IdBarang", sqlConnection);
                                        //command.Parameters.AddWithValue("@QtyBarangPOPenjualanDtl", pnDtlListBaru[i].QtyBarangPOPenjualanDtl);
                                        //command.Parameters.AddWithValue("@JumlahBoxPOPenjualanDtl", pnDtlListBaru[i].JumlahBoxPOPenjualanDtl);
                                        //command.Parameters.AddWithValue("@TotalHargaPOPenjualanDtl", pnDtlListBaru[i].TotalHargaPOPenjualanDtl);
                                        //command.Parameters.AddWithValue("@PenjualanDtlUpdatedBy", ((Role)Session["Role"]).EmailEmp);
                                        //command.Parameters.AddWithValue("@PenjualanDtlUpdatedDate", DateTime.Now.Date.ToString());
                                        //command.Parameters.AddWithValue("@IdPOPenjualanMst", pnjObj.IdPOPenjualanMst);
                                        //command.Parameters.AddWithValue("@IdBarang", pnDtlListBaru[i].IdBarang);
                                        //command.ExecuteNonQuery();
                                        //SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE' ", sqlConnection);
                                        //commNama.Parameters.AddWithValue("@IdBarang", pnDtlListBaru[i].IdBarang);
                                        //commNama.Parameters.AddWithValue("@IdInventory", pnjObj.IdInventory);
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
                                        //SqlCommand strQuerEditBarang = new SqlCommand("update Barang set BarangQuantity=@BarangQuantity,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate,BarangTanggalBeli=@BarangTanggalBeli,BarangJmlBox=@BarangJmlBox where IdBarang=@IdBarang AND IdInventory=@IdInventory", sqlConnection);
                                        //strQuerEditBarang.Parameters.AddWithValue("@IdBarang", pnDtlListBaru[i].IdBarang);
                                        //strQuerEditBarang.Parameters.AddWithValue("@IdInventory", pnjObj.IdInventory);
                                        ////strQuerEditBarang.Parameters.AddWithValue("@IdSupplier", pnjObj.IdSupplier);
                                        //strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                        //strQuerEditBarang.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                                        //strQuerEditBarang.Parameters.AddWithValue("@BarangTanggalBeli", pnjObj.TanggalPOPenjualan);
                                        //strQuerEditBarang.Parameters.AddWithValue("@BarangQuantity", brgCounter.BarangQty - Convert.ToDecimal(pnDtlListBaru[i].QtyBarangPOPenjualanDtl));
                                        //strQuerEditBarang.Parameters.AddWithValue("@BarangJmlBox", brgCounter.BarangJmlBox - Convert.ToDecimal(pnDtlListBaru[i].JumlahBoxPOPenjualanDtl));
                                        //strQuerEditBarang.ExecuteNonQuery();
                                        //updateKapasitasGudangNew(pnDtlListBaru[i], pnjObj);
                                    }
                                    SqlCommand commandPembelianMst = new SqlCommand("update POPenjualanMst set StatusInvoicePenjualan = @StatusInvoicePenjualan,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate where IdPOPenjualanMst=@IdPOPenjualanMst", sqlConnection);
                                    commandPembelianMst.Parameters.AddWithValue("@StatusInvoicePenjualan", "RETUR");
                                    commandPembelianMst.Parameters.AddWithValue("@UpdatedBy", ((Role)Session["Role"]).EmailEmp);
                                    commandPembelianMst.Parameters.AddWithValue("@UpdatedDate", DateTime.Now.Date.ToString());
                                    commandPembelianMst.Parameters.AddWithValue("@IdPOPenjualanMst", pnjObj.IdPOPenjualanMst);
                                    commandPembelianMst.ExecuteNonQuery();
                                    sqlConnection.Close();
                                    sqlConnection.Dispose();
                                    Response.Redirect("PenjualanPage.aspx", false);
                                }
                                catch (Exception ex)
                                { throw ex; }
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

        protected bool checkCicilan(int id)
        {
            bool cicilanCheck = false;
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT JenisTagihan FROM POPenjualanMst WHERE IdPOPenjualanMst = '"+id+"'", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        var brgCicilan = dr.GetString(dr.GetOrdinal("JenisTagihan"));
                        if(brgCicilan.Trim() == "CICILAN")
                        {
                            cicilanCheck = true;
                            return cicilanCheck;
                        }
                        else
                        {
                            return cicilanCheck;
                        }
                    }
                    else
                    {
                        return cicilanCheck;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected void deleteFromReminder(PenjualanMst pnjMst)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("DELETE FROM EventDP WHERE Name = 'Penjualan - " + pnjMst.NoInvoicePenjualan.Trim() + " Jatuh Tempo'", sqlConnection);
                commNama.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected void deleteFromCicilan(PenjualanMst pnjMst)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("DELETE FROM Cicilan WHERE IdPOPenjualanMst = '" + pnjMst.IdPOPenjualanMst + "'", sqlConnection);
                commNama.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected void deleteUpdateBarang(PenjualanDtl pnjDtl, PenjualanMst pnjMST)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("DELETE FROM POPenjualanDtl WHERE IdBarang = " + pnjDtl.IdBarang + " AND IdPOPenjualanMst = " + pnjMST.IdPOPenjualanMst + " ", sqlConnection);
                commNama.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected void insertToReminder(PenjualanMst pnjMst, string endStg)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                DateTime timeStart = new DateTime(Convert.ToInt32(endStg.Substring(6, 4)), Convert.ToInt32(endStg.Substring(0, 2)), Convert.ToInt32(endStg.Substring(3, 2)), 8, 0, 0);
                DateTime timeEnd = new DateTime(Convert.ToInt32(endStg.Substring(6, 4)), Convert.ToInt32(endStg.Substring(0, 2)), Convert.ToInt32(endStg.Substring(3, 2)), 17, 0, 0);
                SqlCommand commNama = new SqlCommand("insert into EventDP" + "(Name,EventStart,EventEnd) values " + "('Penjualan - " + pnjMst.NoInvoicePenjualan + " Jatuh Tempo','" + timeStart + "','" + timeEnd + "')", sqlConnection);
                commNama.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected void updateQtyBarangRetur(PenjualanDtl pnjDtlList, PenjualanMst pnjMstList)
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
                commNama.Parameters.AddWithValue("@IdInventory", pnjMstList.IdInventory);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        capacityTmp = (int)dr["IvnSisaCapacity"];
                        capacityTmp -= pnjDtlList.JumlahBoxPOPenjualanDtl;
                        SqlCommand strQuerEditInventory = new SqlCommand("update Inventory set IvnSisaCapacity=@IvnSisaCapacity,IvnUpdateBy=@IvnUpdateBy,IvnUpdateDate=@IvnUpdateDate where IdInventory=@IdInventory", sqlConnection);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnSisaCapacity", capacityTmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateBy", ((Role)Session["Role"]).EmailEmp);
                        strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateDate", DateTime.Now.Date.ToString());
                        strQuerEditInventory.Parameters.AddWithValue("@IdInventory", pnjMstList.IdInventory);
                        strQuerEditInventory.ExecuteNonQuery();
                    }
                }
                SqlCommand CommBrg = new SqlCommand("SELECT BarangQuantity,BarangJmlBox FROM Barang WHERE IdBarang = @IdBarang AND IdInventory = @IdInventory AND BarangStatus = 'ACTIVE'", sqlConnection);
                CommBrg.Parameters.AddWithValue("@IdBarang", pnjDtlList.IdBarang);
                CommBrg.Parameters.AddWithValue("@IdInventory", pnjMstList.IdInventory);
                using (SqlDataReader dr = CommBrg.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        qtyTmp = (decimal)dr["BarangQuantity"];
                        qtyTmp += Convert.ToDecimal(pnjDtlList.QtyBarangPOPenjualanDtl);
                        jmlBoxTmp = (int)dr["BarangJmlBox"];
                        jmlBoxTmp += pnjDtlList.JumlahBoxPOPenjualanDtl;
                        SqlCommand strQuerEditInventory = new SqlCommand("UPDATE Barang set BarangQuantity=@BarangQuantity, BarangJmlBox = @BarangJmlBox ,BarangUpdateBy=@BarangUpdateBy,BarangUpdateDate=@BarangUpdateDate where IdBarang=@IdBarang", sqlConnection);
                        strQuerEditInventory.Parameters.AddWithValue("@BarangQuantity", qtyTmp);
                        strQuerEditInventory.Parameters.AddWithValue("@BarangJmlBox", jmlBoxTmp);
                        strQuerEditInventory.Parameters.AddWithValue("@BarangUpdateBy", ((Role)Session["Role"]).EmailEmp);
                        strQuerEditInventory.Parameters.AddWithValue("@BarangUpdateDate", DateTime.Now.Date.ToString());
                        strQuerEditInventory.Parameters.AddWithValue("@IdBarang", pnjDtlList.IdBarang);
                        strQuerEditInventory.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
        protected void txtItemBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb1 = ((TextBox)(sender));
            RepeaterItem rp1 = ((RepeaterItem)(tb1.NamingContainer));
            DropDownList tb2 = (DropDownList)rp1.FindControl("ddlNamaBarang");
            if (lblStatusPage.Text != "Retur Penjualan" && lblHeaderPage.Text != "Retur Penjualan")
            {
                if (tb1.Text.Trim() != "" && Convert.ToInt32(tb1.Text.Trim()) != 0)
                {
                    if (!checkBoxItem(Convert.ToInt32(ddlNamaGudang.SelectedValue.Trim()), Convert.ToInt32(tb2.SelectedValue.Trim()), Convert.ToInt32(tb1.Text.Trim())))
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
            else
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
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Box Melebihi Stock yang ada di Gudang!" + "');", true);
                        }
                    }
                    else
                    {
                        tb1.Text = "0";
                    }
                }
            }
        }

        protected void ddlNamaBarang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblStatusPage.Text != "Retur Penjualan" && lblHeaderPage.Text != "Retur Penjualan")
            {
                DropDownList ddlNmBrg = ((DropDownList)(sender));
                RepeaterItem rp1 = ((RepeaterItem)(ddlNmBrg.NamingContainer));
                TextBox tb1 = (TextBox)rp1.FindControl("txtItemQty");
                TextBox tb2 = (TextBox)rp1.FindControl("txtPrice");
                TextBox tb3 = (TextBox)rp1.FindControl("txtItemBox");
                TextBox tb4 = (TextBox)rp1.FindControl("txtTotalPrice");
                tb1.Text = "0";
                tb2.Text = "0";
                tb3.Text = "0";
                tb4.Text = "Rp0";
                txtGrandTotal.Text = "0";
            }
            else
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

        protected void bckBtn_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
                Response.Redirect((string)refUrl);
        }

        protected void ddlJenisTagihan_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCheckMtY = ((DropDownList)(sender));
            if (ddlCheckMtY.SelectedValue.Trim() == "CICILAN")
            {
                lblCicilan.Visible = true;
                ddlCicilan.Visible = true;
                ddlCclVis.Visible = true;
                txtCicilanJumlah.Visible = true;
            }
            else
            {
                lblCicilan.Visible = false;
                ddlCicilan.Visible = false;
                ddlCclVis.Visible = false;
                txtCicilanJumlah.Visible = false;
            }
        }
    }
}