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
    public partial class AddBiayaPage : System.Web.UI.Page
    {
        BiayaMst biayaObj;
        List<BiayaDtl> byListDtl;
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
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand commNama = new SqlCommand("SELECT * FROM TblBiayaMst WHERE IdBiayaMst = @IdBiayaMst ", sqlConnection);
                        commNama.Parameters.AddWithValue("@IdBiayaMst", id);
                        using (SqlDataReader dr = commNama.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                biayaObj = new BiayaMst();
                                biayaObj.IdBiayaMst = id;
                                var GTotal = (decimal)dr["TotalBiayaMst"];
                                lblHeaderPage.Text = "Update Biaya";
                                lblStatusPage.Text = "Update Biaya";
                                biayaObj.JenisBiayaMst = dr.GetString(dr.GetOrdinal("JenisBiayaMst"));
                                ddlJenisBiaya.SelectedValue = dr.GetString(dr.GetOrdinal("JenisBiayaMst"));
                                ddlJenisBiaya.Enabled = false;
                                DateTime tglBiayaObj = DateTime.ParseExact(dr.GetString(dr.GetOrdinal("TanggalBiayaMst")), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                biayaObj.TanggalBiayaMst = dr.GetString(dr.GetOrdinal("TanggalBiayaMst"));
                                txtCldBrgBeli.Text = tglBiayaObj.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                txtCldBrgBeli.Enabled = false;
                                txtGrandTotal.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotal);
                                getDetailBiaya(id);
                            }
                            else
                            {
                                BindRepeater();
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

        private void getDetailBiaya(int id)
        {
            DataTable counter = new DataTable();
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("SELECT * FROM TblBiayaDtl WHERE IdBiayaMst = @IdBiayaMst", sqlConnection);
                commNama.Parameters.AddWithValue("@IdBiayaMst", id);
                SqlDataAdapter pmbDtlAdapter = new SqlDataAdapter();
                pmbDtlAdapter.SelectCommand = commNama;
                pmbDtlAdapter.Fill(counter);
                List<DataRow> dtRowList = counter.AsEnumerable().ToList();
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        DataTable biayaDtl = new DataTable();
                        biayaDtl.Clear();
                        biayaDtl.Columns.Add("IdBiayaDtl", typeof(int));
                        biayaDtl.Columns.Add("txtItemName", typeof(string));
                        biayaDtl.Columns.Add("txtTotalPrice", typeof(string));
                        for (int i = 0; i < counter.Rows.Count; i++)
                        {
                            DataRow drBiayaDtl = biayaDtl.NewRow();
                            drBiayaDtl["IdBiayaDtl"] = dtRowList[i]["IdBiayaDtl"];
                            drBiayaDtl["txtItemName"] = dtRowList[i]["NamaBiayaDtl"];
                            drBiayaDtl["txtTotalPrice"] = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", dtRowList[i]["HargaBiayaDtl"]);
                            biayaDtl.Rows.Add(drBiayaDtl);
                        }
                        ViewState["Curtbl"] = biayaDtl;
                        //ViewState["ReturTbl"] = pmbDtl;
                        rptPembiayaan.DataSource = biayaDtl;
                        rptPembiayaan.DataBind();
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
                        TextBox txtItemName = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtItemName");
                        TextBox txtItemTotalPrice = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtTotalPrice");
                        LinkButton lnkBtnAdd = (LinkButton)rptPembiayaan.Items[rowIndex].FindControl("btnadd");
                        LinkButton lnkBtnDel = (LinkButton)rptPembiayaan.Items[rowIndex].FindControl("btndel");
                        txtItemName.Text = dt.Rows[i]["txtItemName"].ToString();
                        txtItemTotalPrice.Text = dt.Rows[i]["txtTotalPrice"].ToString();
                        lnkBtnAdd.Visible = false;
                        lnkBtnDel.Visible = false;
                        rowIndex++;
                    }
                }
            }
        }

        private void BindRepeater()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("txtItemName", typeof(string));
            dt.Columns.Add("txtTotalPrice", typeof(string));
            DataRow dr = dt.NewRow();
            dr["txtItemName"] = string.Empty;
            dr["txtTotalPrice"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["Curtbl"] = dt;
            rptPembiayaan.DataSource = dt;
            rptPembiayaan.DataBind();
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            LinkButton btn = (sender as LinkButton);
            RepeaterItem itemCheck = btn.NamingContainer as RepeaterItem;
            int indexCheck = itemCheck.ItemIndex;
            TextBox txtItemQtyCheck = (TextBox)itemCheck.FindControl("txtItemName");
            TextBox txtItemPriceCheck = (TextBox)itemCheck.FindControl("txtTotalPrice");
            if (txtItemQtyCheck.Text.Trim() != "" && txtItemPriceCheck.Text.Trim() != "" && txtItemPriceCheck.Text.Trim() != "0")
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
                            TextBox txtItemName = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtItemName");
                            TextBox txtItemTotalPrice = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtTotalPrice");
                            drCurrentRow = dt.NewRow();
                            dt.Rows[i - 1]["txtItemName"] = txtItemName.Text;
                            dt.Rows[i - 1]["txtTotalPrice"] = txtItemTotalPrice.Text;
                            rowIndex++;
                        }
                        dt.Rows.Add(drCurrentRow);
                        ViewState["Curtbl"] = dt;
                        rptPembiayaan.DataSource = dt;
                        rptPembiayaan.DataBind();
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
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Seluruh Data Harus Terpenuhi Dahulu!" + "');", true);
            }
        }

        protected void btndel_Click(object sender, EventArgs e)
        {
            RepeaterItem item = (sender as LinkButton).NamingContainer as RepeaterItem;

            int rowIndex = 0;
            int rowID = item.ItemIndex;
            if (ViewState["Curtbl"] != null)
            {
                DataTable dt = (DataTable)ViewState["Curtbl"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 1; i <= rptPembiayaan.Items.Count; i++)
                    {
                        TextBox txtItemName = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtItemName");
                        TextBox txtItemTotalPrice = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtTotalPrice");
                        dt.Rows[i - 1]["txtItemName"] = txtItemName.Text;
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
                rptPembiayaan.DataSource = dt;
                rptPembiayaan.DataBind();
            }
            SetOldData();
            txtTotalPrice_TextChanged(sender, e);
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
                        TextBox txtItemName = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtItemName");
                        TextBox txtItemTotalPrice = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtTotalPrice");
                        txtItemName.Text = dt.Rows[i]["txtItemName"].ToString();
                        txtItemTotalPrice.Text = dt.Rows[i]["txtTotalPrice"].ToString();
                        rowIndex++;
                    }
                }
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
                        TextBox txtItemTotalPrice = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtTotalPrice");
                        if (txtItemTotalPrice.Text.Trim() != "")
                        {
                            counter += decimal.Parse(Regex.Replace(txtItemTotalPrice.Text.Trim(), @"[^\d.,]", ""));
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

        protected void bckBtn_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
                Response.Redirect((string)refUrl);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (lblStatusPage.Text.Trim() != "Update Biaya" && lblHeaderPage.Text.Trim() != "Update Biaya")
            {
                biayaObj = new BiayaMst();
                byListDtl = new List<BiayaDtl>();
                int rowIndex = 0;
                if (ViewState["Curtbl"] != null)
                {
                    DataTable dt = (DataTable)ViewState["Curtbl"];
                    if (checkControlDetail())
                    {
                        DateTime tglBeliStr = DateTime.ParseExact(txtCldBrgBeli.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                        sqlConnection = new SqlConnection(connectionString);
                        biayaObj.JenisBiayaMst = ddlJenisBiaya.SelectedValue.Trim();
                        biayaObj.TanggalBiayaMst = tglBeliStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        biayaObj.TotalBiayaMst = decimal.Parse(Regex.Replace(txtGrandTotal.Text.Trim(), @"[^\d.,]", ""));
                        if (ddlJenisBiaya.SelectedValue.Trim() == "Biaya Peralatan")
                        {
                            biayaObj.AkunBiayaMst = "-";
                        }
                        if (ddlJenisBiaya.SelectedValue.Trim() == "Biaya Produksi")
                        {
                            biayaObj.AkunBiayaMst = "-";
                        }
                        if (ddlJenisBiaya.SelectedValue.Trim() == "Biaya Operasional")
                        {
                            biayaObj.AkunBiayaMst = "-";
                        }
                        if (ddlJenisBiaya.SelectedValue.Trim() == "Pajak")
                        {
                            biayaObj.AkunBiayaMst = "-";
                        }
                        biayaObj.CreatedBy = ((Role)Session["Role"]).EmailEmp;
                        biayaObj.CreatedDate = DateTime.Now.Date.ToString();
                        biayaObj.UpdatedBy = ((Role)Session["Role"]).EmailEmp;
                        biayaObj.UpdatedDate = DateTime.Now.Date.ToString();
                        if (biayaObj.TotalBiayaMst != 0)
                        {
                            try
                            {
                                sqlConnection.Open();
                                SqlCommand commandBiayaMst = new SqlCommand("insert into TblBiayaMst" + "(JenisBiayaMst,TanggalBiayaMst,AkunBiayaMst,TotalBiayaMst,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate) values " + "('" + biayaObj.JenisBiayaMst + "','" + biayaObj.TanggalBiayaMst + "','" + biayaObj.AkunBiayaMst + "'," + biayaObj.TotalBiayaMst.ToString().Trim().Replace(",",".") + ",'" + biayaObj.CreatedBy + "','" + biayaObj.CreatedDate + "','" + biayaObj.UpdatedBy + "','" + biayaObj.UpdatedDate + "')", sqlConnection);
                                commandBiayaMst.ExecuteNonQuery();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    TextBox txtItemName = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtItemName");
                                    TextBox txtItemTotalPrice = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtTotalPrice");
                                    double txtPriceItmVal = Convert.ToDouble(txtItemTotalPrice.Text.Trim());
                                    string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                    BiayaDtl byDtl = new BiayaDtl();
                                    byDtl.NamaBiayaDtl = txtItemName.Text.Trim();
                                    byDtl.HargaBiayaDtl = Convert.ToDecimal(txtPriceItmStr.Trim().Replace(".", ","));
                                    byDtl.IdBiayaMst = getIDBiayaMst(sqlConnection);
                                    byDtl.CreatedBy = ((Role)Session["Role"]).EmailEmp;
                                    byDtl.CreatedDate = DateTime.Now.Date.ToString();
                                    byDtl.UpdatedBy = ((Role)Session["Role"]).EmailEmp;
                                    byDtl.UpdatedDate = DateTime.Now.Date.ToString();
                                    SqlCommand commandBiayaDtl = new SqlCommand("insert into TblBiayaDtl" + "(NamaBiayaDtl,HargaBiayaDtl,IdBiayaMst,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate) values " + "('" + byDtl.NamaBiayaDtl + "'," + byDtl.HargaBiayaDtl.ToString().Trim().Replace(",", ".") + ",'" + byDtl.IdBiayaMst + "','" + byDtl.CreatedBy + "','" + byDtl.CreatedDate + "','" + byDtl.UpdatedBy + "','" + byDtl.UpdatedDate + "')", sqlConnection);
                                    commandBiayaDtl.ExecuteNonQuery();
                                    rowIndex++;
                                }
                            }
                            catch (Exception ex)
                            { throw ex; }
                            finally { sqlConnection.Close(); sqlConnection.Dispose(); Response.Redirect("BiayaPage.aspx"); }
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
                else
                {
                    Response.Write("ViewState Value is Null");
                }
            }
            else
            {
                int id;
                int.TryParse(Request.QueryString["id"], out id);
                byListDtl = new List<BiayaDtl>();
                int rowIndex = 0;
                if (ViewState["Curtbl"] != null)
                {
                    DataTable dt = (DataTable)ViewState["Curtbl"];
                    if (checkControlDetail())
                    {
                        connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                        sqlConnection = new SqlConnection(connectionString);
                        if (decimal.Parse(Regex.Replace(txtGrandTotal.Text.Trim(), @"[^\d.,]", "")) != 0)
                        {
                            try
                            {
                                sqlConnection.Open();
                                SqlCommand commandMst = new SqlCommand("update TblBiayaMst set TotalBiayaMst=@TotalBiayaMst,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate where IdBiayaMst=@IdBiayaMst", sqlConnection);
                                commandMst.Parameters.AddWithValue("@TotalBiayaMst", decimal.Parse(Regex.Replace(txtGrandTotal.Text.Trim(), @"[^\d.,]", "")));
                                commandMst.Parameters.AddWithValue("@UpdatedBy", ((Role)Session["Role"]).EmailEmp);
                                commandMst.Parameters.AddWithValue("@UpdatedDate", DateTime.Now.Date.ToString());
                                commandMst.Parameters.AddWithValue("@IdBiayaMst", id);
                                commandMst.ExecuteNonQuery();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    TextBox txtItemName = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtItemName");
                                    TextBox txtItemTotalPrice = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtTotalPrice");
                                    double txtPriceItmVal = Convert.ToDouble(Regex.Replace(txtItemTotalPrice.Text.Trim(), @"[^\d.,]", ""));
                                    string txtPriceItmStr = string.Format("{0:0.00}", txtPriceItmVal);
                                    BiayaDtl byDtl = new BiayaDtl();
                                    byDtl.IdBiayaDtl = Convert.ToInt32(dt.Rows[i]["IdBiayaDtl"]);
                                    byDtl.NamaBiayaDtl = txtItemName.Text.Trim();
                                    byDtl.HargaBiayaDtl = Convert.ToDecimal(txtPriceItmStr.Trim().Replace(".", ","));
                                    byDtl.IdBiayaMst = id;
                                    byDtl.CreatedBy = ((Role)Session["Role"]).EmailEmp;
                                    byDtl.CreatedDate = DateTime.Now.Date.ToString();
                                    byDtl.UpdatedBy = ((Role)Session["Role"]).EmailEmp;
                                    byDtl.UpdatedDate = DateTime.Now.Date.ToString();
                                    SqlCommand command = new SqlCommand("update TblBiayaDtl set NamaBiayaDtl=@NamaBiayaDtl,HargaBiayaDtl=@HargaBiayaDtl,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate where IdBiayaDtl=@IdBiayaDtl", sqlConnection);
                                    command.Parameters.AddWithValue("@NamaBiayaDtl", byDtl.NamaBiayaDtl);
                                    command.Parameters.AddWithValue("@HargaBiayaDtl", byDtl.HargaBiayaDtl.ToString().Trim().Replace(",", "."));
                                    command.Parameters.AddWithValue("@UpdatedBy", ((Role)Session["Role"]).EmailEmp);
                                    command.Parameters.AddWithValue("@UpdatedDate", DateTime.Now.Date.ToString());
                                    command.Parameters.AddWithValue("@IdBiayaDtl", byDtl.IdBiayaDtl);
                                    command.ExecuteNonQuery();
                                    rowIndex++;
                                }
                            }
                            catch (Exception ex)
                            { throw ex; }
                            finally { sqlConnection.Close(); sqlConnection.Dispose(); Response.Redirect("BiayaPage.aspx"); }
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
                else
                {
                    Response.Write("ViewState Value is Null");
                }
            }
        }

        private int getIDBiayaMst(SqlConnection sqlConnection)
        {
            int idBiayaMst = 0;
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT TOP 1 * FROM TblBiayaMst ORDER BY IdBiayaMst DESC", sqlConnection);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        idBiayaMst = (int)dr["IdBiayaMst"];
                        return idBiayaMst;
                    }
                    else
                    {
                        return idBiayaMst;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
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
                    TextBox txtItemName = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtItemName");
                    TextBox txtItemTotalPrice = (TextBox)rptPembiayaan.Items[rowIndex].FindControl("txtTotalPrice");
                    if (txtItemName.Text.Trim() != "" && txtItemTotalPrice.Text.Trim() != "0" && txtItemTotalPrice.Text.Trim() != "" && txtCldBrgBeli.Text.Trim() != "mm/dd/yyyy")
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
    }
}