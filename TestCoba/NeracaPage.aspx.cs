using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace TestCoba
{
    public partial class NeracaPage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        Neraca nrcObj;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["Role"] != null)
                {
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void ddlCheckBulanTahun_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCheckMtY = ((DropDownList)(sender));
            ddlPilihPeriode.Items.Clear(); 
            if(ddlCheckMtY.SelectedValue.Trim() != "")
            {
                bindDropdownPeriode(ddlCheckMtY.SelectedValue.Trim());
            }
        }

        protected void bindDropdownPeriode(string ddlValue)
        {
            if (ddlValue.Trim() == "Tahun")
            {
                int year = DateTime.Now.Year;
                for (int i = year - 20; i <= year + 50; i++)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(i.ToString());
                    ddlPilihPeriode.Items.Add(li);
                }
                ddlPilihPeriode.Items.FindByText(year.ToString()).Selected = true;
                Label2.Text = "Periode : ";
                lblBlnVis.Visible = false;
                ddlBlnVis.Visible = false;
                lblThnVis.Visible = false;
                ddlThnVis.Visible = false;
            }
            if(ddlValue.Trim() == "Bulan")
            {
                lblBlnVis.Visible = false;
                ddlBlnVis.Visible = false;
                ddlPilihPeriode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Januari", "01"));
                ddlPilihPeriode.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Februari", "02"));
                ddlPilihPeriode.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Maret", "03"));
                ddlPilihPeriode.Items.Insert(3, new System.Web.UI.WebControls.ListItem("April", "04"));
                ddlPilihPeriode.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Mei", "05"));
                ddlPilihPeriode.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Juni", "06"));
                ddlPilihPeriode.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Juli", "07"));
                ddlPilihPeriode.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Agustus", "08"));
                ddlPilihPeriode.Items.Insert(8, new System.Web.UI.WebControls.ListItem("September", "09"));
                ddlPilihPeriode.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oktober", "10"));
                ddlPilihPeriode.Items.Insert(10, new System.Web.UI.WebControls.ListItem("November", "11"));
                ddlPilihPeriode.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Desember", "12"));
                lblThnVis.Visible = true;
                ddlThnVis.Visible = true;
                int year = DateTime.Now.Year;
                for (int i = year - 20; i <= year + 50; i++)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(i.ToString());
                    ddlThnVis.Items.Add(li);
                }
                ddlThnVis.Items.FindByText(year.ToString()).Selected = true;
                Label2.Text = "Bulan : ";
            }
            if(ddlValue.Trim() == "Tanggal")
            {
                ddlPilihPeriode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("01", "01"));
                ddlPilihPeriode.Items.Insert(1, new System.Web.UI.WebControls.ListItem("02", "02"));
                ddlPilihPeriode.Items.Insert(2, new System.Web.UI.WebControls.ListItem("03", "03"));
                ddlPilihPeriode.Items.Insert(3, new System.Web.UI.WebControls.ListItem("04", "04"));
                ddlPilihPeriode.Items.Insert(4, new System.Web.UI.WebControls.ListItem("05", "05"));
                ddlPilihPeriode.Items.Insert(5, new System.Web.UI.WebControls.ListItem("06", "06"));
                ddlPilihPeriode.Items.Insert(6, new System.Web.UI.WebControls.ListItem("07", "07"));
                ddlPilihPeriode.Items.Insert(7, new System.Web.UI.WebControls.ListItem("08", "08"));
                ddlPilihPeriode.Items.Insert(8, new System.Web.UI.WebControls.ListItem("09", "09"));
                ddlPilihPeriode.Items.Insert(9, new System.Web.UI.WebControls.ListItem("10", "10"));
                ddlPilihPeriode.Items.Insert(10, new System.Web.UI.WebControls.ListItem("11", "11"));
                ddlPilihPeriode.Items.Insert(11, new System.Web.UI.WebControls.ListItem("12", "12"));
                ddlPilihPeriode.Items.Insert(12, new System.Web.UI.WebControls.ListItem("13", "13"));
                ddlPilihPeriode.Items.Insert(13, new System.Web.UI.WebControls.ListItem("14", "14"));
                ddlPilihPeriode.Items.Insert(14, new System.Web.UI.WebControls.ListItem("15", "15"));
                ddlPilihPeriode.Items.Insert(15, new System.Web.UI.WebControls.ListItem("16", "16"));
                ddlPilihPeriode.Items.Insert(16, new System.Web.UI.WebControls.ListItem("17", "17"));
                ddlPilihPeriode.Items.Insert(17, new System.Web.UI.WebControls.ListItem("18", "18"));
                ddlPilihPeriode.Items.Insert(18, new System.Web.UI.WebControls.ListItem("19", "19"));
                ddlPilihPeriode.Items.Insert(19, new System.Web.UI.WebControls.ListItem("20", "20"));
                ddlPilihPeriode.Items.Insert(20, new System.Web.UI.WebControls.ListItem("21", "21"));
                ddlPilihPeriode.Items.Insert(21, new System.Web.UI.WebControls.ListItem("22", "22"));
                ddlPilihPeriode.Items.Insert(22, new System.Web.UI.WebControls.ListItem("23", "23"));
                ddlPilihPeriode.Items.Insert(23, new System.Web.UI.WebControls.ListItem("24", "24"));
                ddlPilihPeriode.Items.Insert(24, new System.Web.UI.WebControls.ListItem("25", "25"));
                ddlPilihPeriode.Items.Insert(25, new System.Web.UI.WebControls.ListItem("26", "26"));
                ddlPilihPeriode.Items.Insert(26, new System.Web.UI.WebControls.ListItem("27", "27"));
                ddlPilihPeriode.Items.Insert(27, new System.Web.UI.WebControls.ListItem("28", "28"));
                ddlPilihPeriode.Items.Insert(28, new System.Web.UI.WebControls.ListItem("29", "29"));
                ddlPilihPeriode.Items.Insert(29, new System.Web.UI.WebControls.ListItem("30", "30"));
                ddlPilihPeriode.Items.Insert(30, new System.Web.UI.WebControls.ListItem("31", "31"));
                lblBlnVis.Visible = true;
                ddlBlnVis.Visible = true;
                ddlBlnVis.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Januari", "01"));
                ddlBlnVis.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Februari", "02"));
                ddlBlnVis.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Maret", "03"));
                ddlBlnVis.Items.Insert(3, new System.Web.UI.WebControls.ListItem("April", "04"));
                ddlBlnVis.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Mei", "05"));
                ddlBlnVis.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Juni", "06"));
                ddlBlnVis.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Juli", "07"));
                ddlBlnVis.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Agustus", "08"));
                ddlBlnVis.Items.Insert(8, new System.Web.UI.WebControls.ListItem("September", "09"));
                ddlBlnVis.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oktober", "10"));
                ddlBlnVis.Items.Insert(10, new System.Web.UI.WebControls.ListItem("November", "11"));
                ddlBlnVis.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Desember", "12"));
                lblThnVis.Visible = true;
                ddlThnVis.Visible = true;
                int year = DateTime.Now.Year;
                for (int i = year - 20; i <= year + 50; i++)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(i.ToString());
                    ddlThnVis.Items.Add(li);
                }
                ddlThnVis.Items.FindByText(year.ToString()).Selected = true;
                Label2.Text = "Tanggal : ";
            }
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            DataTable dtBiaya = new DataTable();
            DataTable dtPajak = new DataTable();
            if(ddlCheckBulanTahun.SelectedValue.Trim() != "")
            {
                try
                {
                    sqlConnection.Open();
                    if(ddlCheckBulanTahun.SelectedValue.Trim() == "Tanggal")
                    {
                        nrcObj = new Neraca();
                        nrcObj.JenisNeraca = "Tanggal";
                        //string pnjVal = "0";
                        //string pmbVal = "0";
                        string lbktrQtyVal = "0";
                        string lbKtrVal = "0";
                        string jmlBiayaVal = "0";
                        string lbStlhPajakVal = "0";
                        //decimal pnjDec = 0;
                        //decimal pmbDec = 0;
                        decimal lbKtrDecQty = 0;
                        decimal lbKtrDec = 0;
                        decimal jmlBiayaDec = 0;
                        decimal lbStlhPajakDec = 0;
                        string queryLbKtr = "; with list_tgl_beli as(select MAX(CONVERT(DATETIME,b.TanggalPOPembelian,103)) as tglBeli,IdBarang from POPembelianDtl a INNER JOIN POPembelianMst b on b.IdPOPembelianMst = a.IdPoPembelianMst where CONVERT(DATETIME,b.TanggalPOPembelian,103) <= CONVERT(DATETIME,'"+ ddlPilihPeriode.SelectedValue.Trim() + "/"+ ddlBlnVis.SelectedValue.Trim() + "/"+ ddlThnVis.SelectedValue.Trim() + "',103) group by IdBarang),list_hrg_beli as(select  HargaPOPembelianDtl,b.IdBarang,TanggalPOPembelian,ROW_NUMBER() Over(partition by b.IdBarang order by abs(DATEDIFF(ss, CONVERT(DATETIME,TanggalPOPembelian,103), b.tglBeli)) asc) as RowNum from POPembelianDtl a inner join list_tgl_beli b on a.IdBarang = b.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst) select COALESCE(SUM(CAST((c.QtyBarangPOPenjualanDtl * (c.HargaPOPenjualanDtl-f.HargaPOPembelianDtl))as decimal(18,2))),0) as TotSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN list_hrg_beli f on f.IdBarang = c.IdBarang INNER JOIN Barang g on g.IdBarang = f.IdBarang INNER JOIN Supplier h on h.IdSupplier = g.IdSupplier where SUBSTRING(a.TanggalPOPenjualan,1,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,4,2) = '"+ ddlBlnVis.SelectedValue.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '"+ ddlThnVis.SelectedValue.Trim() + "' AND StatusInvoicePenjualan != 'RETUR' AND RowNum = 1";
                        //string queryLbKtr = "SELECT COALESCE(SUM(CAST((c.QtyBarangPOPenjualanDtl * (c.HargaPOPenjualanDtl-f.HargaPOPembelianDtl))as decimal(18,2))),0) as TotSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN Barang d on d.IdBarang = c.IdBarang INNER JOIN Supplier e on e.IdSupplier = d.IdSupplier INNER JOIN POPembelianDtl f on f.IdBarang = d.IdBarang WHERE SUBSTRING(a.TanggalPOPenjualan,1,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,4,2) = '" + ddlBlnVis.SelectedValue.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        //string queryJmlQty = "SELECT COALESCE(SUM(c.QtyBarangPOPenjualanDtl),0) as TotalQTY FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN Barang d on d.IdBarang = c.IdBarang INNER JOIN Supplier e on e.IdSupplier = d.IdSupplier INNER JOIN (select ts.IdBarang, MAX(CAST(ts.PembelianDtlUpdatedDate as date)) as tgl, MAX(ts.HargaPOPembelianDtl) as hrg from POPembelianDtl ts group by IdBarang) f on f.IdBarang = d.IdBarang WHERE SUBSTRING(a.TanggalPOPenjualan,1,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,4,2) = '" + ddlBlnVis.SelectedValue.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        string queryJmlQty = ";with table1 as (SELECT COALESCE(SUM(B.QtyBarangPOPenjualanDtl),0) as TotalQTY FROM POPenjualanMst A INNER JOIN POPenjualanDtl B on A.IdPOPenjualanMst = B.IdPOPenjualanMst WHERE SUBSTRING(TanggalPOPenjualan,1,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(TanggalPOPenjualan,4,2) = '" + ddlBlnVis.SelectedValue.Trim() + "' AND SUBSTRING(TanggalPOPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'), table2 as (SELECT COALESCE(SUM(a.QtyBarangPOPenjualanReturDtl),0) as TotalQTYRetur FROM POPenjualanReturDtl a INNER JOIN POPenjualanMst b on a.IdPOPenjualanMst = b.IdPOPenjualanMst WHERE SUBSTRING(b.TanggalPOPenjualan,1,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalPOPenjualan,4,2) = '" + ddlBlnVis.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalPOPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "') SELECT a.TotalQTY - b.TotalQTYRetur as TotalQTY from table1 a, table2 b";
                        //string queryTotPmb = "SELECT COALESCE(SUM(a.TotalHargaPOPembelianDtl),0) as TotalPembelian FROM POPembelianDtl a INNER JOIN Barang b on b.IdBarang = a.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst WHERE SUBSTRING(TanggalPOJatuhTmpPembelian,1,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(TanggalPOJatuhTmpPembelian,4,2) = '" + ddlBlnVis.SelectedValue.Trim() + "' AND SUBSTRING(TanggalPOJatuhTmpPembelian,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        //string queryTotPnj = "SELECT COALESCE(SUM(a.TotalHargaPOPenjualanDtl),0) as TotalPenjualan FROM POPenjualanDtl a INNER JOIN Barang b on b.IdBarang = a.IdBarang INNER JOIN POPenjualanMst c on c.IdPOPenjualanMst = a.IdPOPenjualanMst WHERE SUBSTRING(TanggalPOJatuhTmpPenjualan,1,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(TanggalPOJatuhTmpPenjualan,4,2) = '" + ddlBlnVis.SelectedValue.Trim() + "' AND SUBSTRING(TanggalPOJatuhTmpPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        //SqlCommand commPmb = new SqlCommand(queryTotPmb, sqlConnection);
                        //using (SqlDataReader drPmb = commPmb.ExecuteReader())
                        //{
                        //    if (drPmb.Read())
                        //    {
                        //        var GTotPmb = (decimal)drPmb["TotalPembelian"];
                        //        pmbDec = GTotPmb;
                        //        if (GTotPmb != 0)
                        //        {
                        //            GTPembelian.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPmb);
                        //            nrcObj.NilaiHPP = GTotPmb;
                        //        }
                        //        else
                        //        {
                        //            GTPembelian.Text = pmbVal;
                        //            nrcObj.NilaiHPP = pmbDec;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        GTPembelian.Text = pmbVal;
                        //        nrcObj.NilaiHPP = pmbDec;
                        //    }
                        //}
                        //SqlCommand commPnj = new SqlCommand(queryTotPnj, sqlConnection);
                        //using (SqlDataReader drPnj = commPnj.ExecuteReader())
                        //{
                        //    if (drPnj.Read())
                        //    {
                        //        var GTotPnj = (decimal)drPnj["TotalPenjualan"];
                        //        pnjDec = GTotPnj;
                        //        if (GTotPnj != 0)
                        //        {
                        //            GTPenjualan.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPnj);
                        //            nrcObj.NilaiPenjualan = GTotPnj;
                        //        }
                        //        else
                        //        {
                        //            GTPenjualan.Text = pnjVal;
                        //            nrcObj.NilaiPenjualan = pnjDec;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        GTPenjualan.Text = pnjVal;
                        //        nrcObj.NilaiPenjualan = pnjDec;
                        //    }
                        //}
                        //txtLabaKotor.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", pnjDec - pmbDec);
                        //nrcObj.NilaiLabaKotor = pnjDec - pmbDec;
                        SqlCommand commLbKtrQty = new SqlCommand(queryJmlQty, sqlConnection);
                        using (SqlDataReader drLbKtrQty = commLbKtrQty.ExecuteReader())
                        {
                            if (drLbKtrQty.Read())
                            {
                                var GTotLbKtrQty = (decimal)drLbKtrQty["TotalQTY"];
                                lbKtrDecQty = GTotLbKtrQty;
                                if (GTotLbKtrQty != 0)
                                {
                                    lblQtyVal.Text = string.Format("{0:n}", GTotLbKtrQty + " KG");
                                    nrcObj.NilaiSelisihQty = GTotLbKtrQty;
                                }
                                else
                                {
                                    lblQtyVal.Text = lbktrQtyVal;
                                    nrcObj.NilaiSelisihQty = lbKtrDecQty;
                                }
                            }
                            else
                            {
                                lblQtyVal.Text = lbktrQtyVal;
                                nrcObj.NilaiSelisihQty = lbKtrDecQty;
                            }
                        }
                        SqlCommand commLbKtr = new SqlCommand(queryLbKtr, sqlConnection);
                        using (SqlDataReader drLbKtr = commLbKtr.ExecuteReader())
                        {
                            if (drLbKtr.Read())
                            {
                                var GTotLbKtr = (decimal)drLbKtr["TotSelisih"];
                                lbKtrDec = GTotLbKtr;
                                if (GTotLbKtr != 0)
                                {
                                    txtLabaKotor.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotLbKtr);
                                    nrcObj.NilaiLabaKotor = GTotLbKtr;
                                }
                                else
                                {
                                    txtLabaKotor.Text = lbKtrVal;
                                    nrcObj.NilaiLabaKotor = lbKtrDec;
                                }
                            }
                            else
                            {
                                txtLabaKotor.Text = lbKtrVal;
                                nrcObj.NilaiLabaKotor = lbKtrDec;
                            }
                        }
                        string biayaPrm = "SELECT b.JenisBiayaMst,SUM(a.HargaBiayaDtl) as JumlahBiaya FROM TblBiayaDtl a INNER JOIN TblBiayaMst b on b.IdBiayaMst = a.IdBiayaMst WHERE b.JenisBiayaMst != 'Pajak' AND SUBSTRING(b.TanggalBiayaMst,1,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalBiayaMst,4,2) = '" + ddlBlnVis.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalBiayaMst,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "' GROUP BY b.JenisBiayaMst";
                        using (SqlCommand strBiaya = new SqlCommand(biayaPrm, sqlConnection))
                        {
                            //StrQuer.CommandType = CommandType.StoredProcedure;
                            rptListBiaya.DataSource = strBiaya.ExecuteReader();
                            rptListBiaya.DataBind();
                        }
                        using (SqlDataAdapter biyAdapter = new SqlDataAdapter())
                        {
                            biyAdapter.SelectCommand = new SqlCommand(biayaPrm, sqlConnection);
                            biyAdapter.Fill(dtBiaya);
                            ViewState["Biytbl"] = dtBiaya;
                        }
                        string jmlBiaya = "SELECT COALESCE(SUM(a.HargaBiayaDtl),0) as JumlahBiaya FROM TblBiayaDtl a INNER JOIN TblBiayaMst b on b.IdBiayaMst = a.IdBiayaMst WHERE b.JenisBiayaMst != 'Pajak' AND SUBSTRING(b.TanggalBiayaMst,1,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalBiayaMst,4,2) = '" + ddlBlnVis.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalBiayaMst,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        SqlCommand commJmlBiaya = new SqlCommand(jmlBiaya, sqlConnection);
                        using (SqlDataReader drJmlBiaya = commJmlBiaya.ExecuteReader())
                        {
                            if (drJmlBiaya.Read())
                            {
                                var GTotJmlBiaya = (decimal)drJmlBiaya["JumlahBiaya"];
                                jmlBiayaDec = GTotJmlBiaya;
                                if (GTotJmlBiaya != 0)
                                {
                                    GTBiaya.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotJmlBiaya);
                                    nrcObj.NilaiTotalBiaya = GTotJmlBiaya;
                                }
                                else
                                {
                                    GTBiaya.Text = jmlBiayaVal;
                                    nrcObj.NilaiTotalBiaya = jmlBiayaDec;
                                }
                            }
                            else
                            {
                                GTBiaya.Text = jmlBiayaVal;
                                nrcObj.NilaiTotalBiaya = jmlBiayaDec;
                            }
                        }
                        GTLaba.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", lbKtrDec - jmlBiayaDec);
                        nrcObj.NilaiJumlahLabaDanBiaya = lbKtrDec - jmlBiayaDec;
                        string pajakPrm = "SELECT b.JenisBiayaMst,SUM(a.HargaBiayaDtl) as JumlahBiaya FROM TblBiayaDtl a INNER JOIN TblBiayaMst b on b.IdBiayaMst = a.IdBiayaMst WHERE b.JenisBiayaMst = 'Pajak' AND SUBSTRING(b.TanggalBiayaMst,1,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalBiayaMst,4,2) = '" + ddlBlnVis.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalBiayaMst,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "' GROUP BY b.JenisBiayaMst";
                        using (SqlCommand commStrPajak = new SqlCommand(pajakPrm, sqlConnection))
                        {
                            //StrQuer.CommandType = CommandType.StoredProcedure;
                            rptListPajak.DataSource = commStrPajak.ExecuteReader();
                            rptListPajak.DataBind();
                        }
                        using (SqlDataAdapter pjkAdapter = new SqlDataAdapter())
                        {
                            pjkAdapter.SelectCommand = new SqlCommand(pajakPrm, sqlConnection);
                            pjkAdapter.Fill(dtPajak);
                            ViewState["Pjktbl"] = dtPajak;
                        }
                        string queryLbStlhPjk = "SELECT COALESCE(SUM(a.HargaBiayaDtl),0) as JumlahBiaya FROM TblBiayaDtl a INNER JOIN TblBiayaMst b on b.IdBiayaMst = a.IdBiayaMst WHERE b.JenisBiayaMst = 'Pajak' AND SUBSTRING(b.TanggalBiayaMst,1,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalBiayaMst,4,2) = '" + ddlBlnVis.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalBiayaMst,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        SqlCommand commLbStlhPjk = new SqlCommand(queryLbStlhPjk, sqlConnection);
                        using (SqlDataReader drLbStlhPjk = commLbStlhPjk.ExecuteReader())
                        {
                            if (drLbStlhPjk.Read())
                            {
                                var GTotLbPjk = (decimal)drLbStlhPjk["JumlahBiaya"];
                                lbStlhPajakDec = GTotLbPjk;
                                if (GTotLbPjk != 0)
                                {
                                    GTSetelahPajak.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", (lbKtrDec - jmlBiayaDec) - GTotLbPjk);
                                    nrcObj.NilaiTotalPajak = GTotLbPjk;
                                    nrcObj.NilaiTotalSetelahPajak = (lbKtrDec - jmlBiayaDec) - GTotLbPjk;
                                }
                                else
                                {
                                    GTSetelahPajak.Text = lbStlhPajakVal;
                                    nrcObj.NilaiTotalPajak = 0;
                                    nrcObj.NilaiTotalSetelahPajak = (lbKtrDec - jmlBiayaDec) - nrcObj.NilaiTotalPajak;
                                }
                            }
                            else
                            {
                                GTSetelahPajak.Text = lbStlhPajakVal;
                                nrcObj.NilaiTotalPajak = 0;
                                nrcObj.NilaiTotalSetelahPajak = (lbKtrDec - jmlBiayaDec) - nrcObj.NilaiTotalPajak;
                            }
                        }
                        if (nrcObj.NilaiTotalSetelahPajak > 0)
                        {
                            GTRounding.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", Math.Round(nrcObj.NilaiTotalSetelahPajak));
                        }
                        else
                        {
                            GTRounding.Text = "- " + string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", Math.Round(nrcObj.NilaiTotalSetelahPajak));
                        }
                        //GTRounding.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", Math.Round(decimal.Parse(Regex.Replace(GTSetelahPajak.Text.Trim(), @"[^\d.,]", ""))));
                        nrcObj.NilaiTotalSetelahRounding = decimal.Parse(Regex.Replace(GTSetelahPajak.Text.Trim(), @"[^\d.,]", ""));
                        if (decimal.Parse(Regex.Replace(GTRounding.Text.Trim(), @"[^\d.,]", "")) != 0)
                        {
                            btnCetakGnrt.Enabled = true;
                            ViewState["NrcObject"] = nrcObj;
                        }
                        else
                        {
                            btnCetakGnrt.Enabled = false;
                        }
                    }
                    else if (ddlCheckBulanTahun.SelectedValue.Trim() == "Bulan")
                    {
                        nrcObj = new Neraca();
                        nrcObj.JenisNeraca = "Bulan";
                        //string pnjVal = "0";
                        //string pmbVal = "0";
                        string lbkKtrValQty = "0";
                        string lbKtrVal = "0";
                        string jmlBiayaVal = "0";
                        string lbStlhPajakVal = "0";
                        //decimal pnjDec = 0;
                        //decimal pmbDec = 0;
                        decimal lbKtrDecQty = 0;
                        decimal lbKtrDec = 0;
                        decimal jmlBiayaDec = 0;
                        decimal lbStlhPajakDec = 0;
                        string bulanTest = "0";
                        string tahunTemp = ddlThnVis.SelectedValue.Trim();
                        if (Convert.ToInt32(ddlPilihPeriode.SelectedValue.Trim()) == 12)
                        {
                            bulanTest = "01";
                            tahunTemp = (Convert.ToInt32(ddlThnVis.SelectedValue.Trim()) + 1).ToString();
                        }
                        else
                        {
                            if(Convert.ToInt32(ddlPilihPeriode.SelectedValue.Trim()) > 9)
                            {
                                bulanTest = (Convert.ToInt32(ddlPilihPeriode.SelectedValue.Trim()) + 1).ToString();
                            }
                            else
                            {
                                bulanTest = bulanTest + (Convert.ToInt32(ddlPilihPeriode.SelectedValue.Trim()) + 1).ToString();
                            }
                        }
                        var test = new List<decimal>();
                        int days = DateTime.DaysInMonth(Convert.ToInt32(ddlThnVis.SelectedValue.Trim()), Convert.ToInt32(ddlPilihPeriode.SelectedValue.Trim()));
                        for(int i = 1; i <= days; i++)
                        {
                            var tanggalString = "";
                            if(i<10)
                            {
                                tanggalString = "0" + i.ToString();
                            }
                            else
                            {
                                tanggalString = i.ToString();
                            }
                            string queryLbKtrTest = "; with list_tgl_beli as(select MAX(CONVERT(DATETIME,b.TanggalPOPembelian,103)) as tglBeli,IdBarang from POPembelianDtl a INNER JOIN POPembelianMst b on b.IdPOPembelianMst = a.IdPoPembelianMst where CONVERT(DATETIME,b.TanggalPOPembelian,103) <= CONVERT(DATETIME,'" + tanggalString.Trim() + "/" + ddlPilihPeriode.SelectedValue.Trim() + "/" + ddlThnVis.SelectedValue.Trim() + "',103) group by IdBarang),list_hrg_beli as(select  HargaPOPembelianDtl,b.IdBarang,TanggalPOPembelian,ROW_NUMBER() Over(partition by b.IdBarang order by abs(DATEDIFF(ss, CONVERT(DATETIME,TanggalPOPembelian,103), b.tglBeli)) asc) as RowNum from POPembelianDtl a inner join list_tgl_beli b on a.IdBarang = b.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst) select COALESCE(SUM(CAST((c.QtyBarangPOPenjualanDtl * (c.HargaPOPenjualanDtl-f.HargaPOPembelianDtl))as decimal(18,2))),0) as TotSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN list_hrg_beli f on f.IdBarang = c.IdBarang INNER JOIN Barang g on g.IdBarang = f.IdBarang INNER JOIN Supplier h on h.IdSupplier = g.IdSupplier where SUBSTRING(a.TanggalPOPenjualan,1,2) = '" + tanggalString.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "' AND StatusInvoicePenjualan != 'RETUR' AND RowNum = 1";
                            SqlCommand commLbKtrTest = new SqlCommand(queryLbKtrTest, sqlConnection);
                            using (SqlDataReader drLbKtr = commLbKtrTest.ExecuteReader())
                            {
                                if (drLbKtr.Read())
                                {
                                    var GTotLbKtr = (decimal)drLbKtr["TotSelisih"];
                                    test.Add(GTotLbKtr);
                                }
                            }
                        }
                        string queryLbKtr = "; with list_tgl_beli as(select MAX(CONVERT(DATETIME,b.TanggalPOPembelian,103)) as tglBeli,IdBarang from POPembelianDtl a INNER JOIN POPembelianMst b on b.IdPOPembelianMst = a.IdPoPembelianMst where CONVERT(DATETIME,b.TanggalPOPembelian,103) <= CONVERT(DATETIME,'01/" + bulanTest + "/" + tahunTemp + "',103) group by IdBarang),list_hrg_beli as(select  HargaPOPembelianDtl,b.IdBarang,TanggalPOPembelian,ROW_NUMBER() Over(partition by b.IdBarang order by abs(DATEDIFF(ss, CONVERT(DATETIME,TanggalPOPembelian,103), b.tglBeli)) asc) as RowNum from POPembelianDtl a inner join list_tgl_beli b on a.IdBarang = b.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst) select COALESCE(SUM(CAST((c.QtyBarangPOPenjualanDtl * (c.HargaPOPenjualanDtl-f.HargaPOPembelianDtl))as decimal(18,2))),0) as TotSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN list_hrg_beli f on f.IdBarang = c.IdBarang INNER JOIN Barang g on g.IdBarang = f.IdBarang INNER JOIN Supplier h on h.IdSupplier = g.IdSupplier where SUBSTRING(a.TanggalPOPenjualan,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "' AND StatusInvoicePenjualan != 'RETUR' AND RowNum = 1";
                        //string queryLbKtr = "SELECT COALESCE(SUM(CAST((c.QtyBarangPOPenjualanDtl * (c.HargaPOPenjualanDtl-f.HargaPOPembelianDtl))as decimal(18,2))),0) as TotSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN Barang d on d.IdBarang = c.IdBarang INNER JOIN Supplier e on e.IdSupplier = d.IdSupplier INNER JOIN POPembelianDtl f on f.IdBarang = d.IdBarang WHERE SUBSTRING(a.TanggalPOPenjualan,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        //string queryLbKtr = "SELECT COALESCE(SUM(CAST((c.QtyBarangPOPenjualanDtl * (c.HargaPOPenjualanDtl-f.hrg))as decimal(18,2))),0) as TotSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN Barang d on d.IdBarang = c.IdBarang INNER JOIN Supplier e on e.IdSupplier = d.IdSupplier INNER JOIN (select ts.IdBarang, MAX(CAST(ts.PembelianDtlUpdatedDate as date)) as tgl, MAX(ts.HargaPOPembelianDtl) as hrg from POPembelianDtl ts group by IdBarang) f on f.IdBarang = d.IdBarang WHERE SUBSTRING(a.TanggalPOPenjualan,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        //string queryJmlQty = "SELECT COALESCE(SUM(c.QtyBarangPOPenjualanDtl),0) as TotalQTY FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN Barang d on d.IdBarang = c.IdBarang INNER JOIN Supplier e on e.IdSupplier = d.IdSupplier INNER JOIN (select ts.IdBarang, MAX(CAST(ts.PembelianDtlUpdatedDate as date)) as tgl, MAX(ts.HargaPOPembelianDtl) as hrg from POPembelianDtl ts group by IdBarang) f on f.IdBarang = d.IdBarang WHERE SUBSTRING(a.TanggalPOPenjualan,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        string queryJmlQty = ";with table1 as (SELECT COALESCE(SUM(B.QtyBarangPOPenjualanDtl),0) as TotalQTY FROM POPenjualanMst A INNER JOIN POPenjualanDtl B on A.IdPOPenjualanMst = B.IdPOPenjualanMst WHERE SUBSTRING(TanggalPOPenjualan,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(TanggalPOPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'), table2 as (SELECT COALESCE(SUM(a.QtyBarangPOPenjualanReturDtl),0) as TotalQTYRetur FROM POPenjualanReturDtl a INNER JOIN POPenjualanMst b on a.IdPOPenjualanMst = b.IdPOPenjualanMst WHERE SUBSTRING(b.TanggalPOPenjualan,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalPOPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "') SELECT a.TotalQTY - b.TotalQTYRetur as TotalQTY from table1 a, table2 b";
                        //string queryTotPmb = "SELECT COALESCE(SUM(a.TotalHargaPOPembelianDtl),0) as TotalPembelian FROM POPembelianDtl a INNER JOIN Barang b on b.IdBarang = a.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst WHERE SUBSTRING(TanggalPOJatuhTmpPembelian,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(TanggalPOJatuhTmpPembelian,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        //string queryTotPnj = "SELECT COALESCE(SUM(a.TotalHargaPOPenjualanDtl),0) as TotalPenjualan FROM POPenjualanDtl a INNER JOIN Barang b on b.IdBarang = a.IdBarang INNER JOIN POPenjualanMst c on c.IdPOPenjualanMst = a.IdPOPenjualanMst WHERE SUBSTRING(TanggalPOJatuhTmpPenjualan,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(TanggalPOJatuhTmpPenjualan,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        //SqlCommand commPmb = new SqlCommand(queryTotPmb, sqlConnection);
                        //using (SqlDataReader drPmb = commPmb.ExecuteReader())
                        //{
                        //    if (drPmb.Read())
                        //    {
                        //        var GTotPmb = (decimal)drPmb["TotalPembelian"];
                        //        pmbDec = GTotPmb;
                        //        if (GTotPmb != 0)
                        //        {
                        //            GTPembelian.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPmb);
                        //            nrcObj.NilaiHPP = GTotPmb;
                        //        }
                        //        else
                        //        {
                        //            GTPembelian.Text = pmbVal;
                        //            nrcObj.NilaiHPP = pmbDec;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        GTPembelian.Text = pmbVal;
                        //        nrcObj.NilaiHPP = pmbDec;
                        //    }
                        //}
                        //SqlCommand commPnj = new SqlCommand(queryTotPnj, sqlConnection);
                        //using (SqlDataReader drPnj = commPnj.ExecuteReader())
                        //{
                        //    if (drPnj.Read())
                        //    {
                        //        var GTotPnj = (decimal)drPnj["TotalPenjualan"];
                        //        pnjDec = GTotPnj;
                        //        if (GTotPnj != 0)
                        //        {
                        //            GTPenjualan.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPnj);
                        //            nrcObj.NilaiPenjualan = GTotPnj;
                        //        }
                        //        else
                        //        {
                        //            GTPenjualan.Text = pnjVal;
                        //            nrcObj.NilaiPenjualan = pnjDec;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        GTPenjualan.Text = pnjVal;
                        //        nrcObj.NilaiPenjualan = pnjDec;
                        //    }
                        //}
                        SqlCommand commLbKtrQty = new SqlCommand(queryJmlQty, sqlConnection);
                        using (SqlDataReader drLbKtrQty = commLbKtrQty.ExecuteReader())
                        {
                            if (drLbKtrQty.Read())
                            {
                                var GTotLbKtrQty = (decimal)drLbKtrQty["TotalQTY"];
                                lbKtrDecQty = GTotLbKtrQty;
                                if (GTotLbKtrQty != 0)
                                {
                                    lblQtyVal.Text = string.Format("{0:n}", GTotLbKtrQty + " KG");
                                    nrcObj.NilaiSelisihQty = GTotLbKtrQty;
                                }
                                else
                                {
                                    lblQtyVal.Text = lbkKtrValQty;
                                    nrcObj.NilaiSelisihQty = lbKtrDecQty;
                                }
                            }
                            else
                            {
                                lblQtyVal.Text = lbkKtrValQty;
                                nrcObj.NilaiSelisihQty = lbKtrDecQty;
                            }
                        }
                        SqlCommand commLbKtr = new SqlCommand(queryLbKtr, sqlConnection);
                        using (SqlDataReader drLbKtr = commLbKtr.ExecuteReader())
                        {
                            if (drLbKtr.Read())
                            {
                                var GTotLbKtr = (decimal)drLbKtr["TotSelisih"];
                                lbKtrDec = GTotLbKtr;
                                if (GTotLbKtr != 0)
                                {
                                    txtLabaKotor.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotLbKtr);
                                    nrcObj.NilaiLabaKotor = GTotLbKtr;
                                }
                                else
                                {
                                    txtLabaKotor.Text = lbKtrVal;
                                    nrcObj.NilaiLabaKotor = lbKtrDec;
                                }
                            }
                            else
                            {
                                txtLabaKotor.Text = lbKtrVal;
                                nrcObj.NilaiLabaKotor = lbKtrDec;
                            }
                        }
                        txtLabaKotor.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", test.Sum());
                        nrcObj.NilaiLabaKotor = test.Sum();
                        lbKtrDec = test.Sum();
                        //txtLabaKotor.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", pnjDec - pmbDec);
                        //nrcObj.NilaiLabaKotor = pnjDec - pmbDec;
                        string biayaPrm = "SELECT b.JenisBiayaMst,SUM(a.HargaBiayaDtl) as JumlahBiaya FROM TblBiayaDtl a INNER JOIN TblBiayaMst b on b.IdBiayaMst = a.IdBiayaMst WHERE b.JenisBiayaMst != 'Pajak' AND SUBSTRING(b.TanggalBiayaMst,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalBiayaMst,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "' GROUP BY b.JenisBiayaMst";
                        using (SqlCommand strBiaya = new SqlCommand(biayaPrm, sqlConnection))
                        {
                            //StrQuer.CommandType = CommandType.StoredProcedure;
                            rptListBiaya.DataSource = strBiaya.ExecuteReader();
                            rptListBiaya.DataBind();
                        }
                        using (SqlDataAdapter biyAdapter = new SqlDataAdapter())
                        {
                            biyAdapter.SelectCommand = new SqlCommand(biayaPrm, sqlConnection);
                            biyAdapter.Fill(dtBiaya);
                            ViewState["Biytbl"] = dtBiaya;
                        }
                        string jmlBiaya = "SELECT COALESCE(SUM(a.HargaBiayaDtl),0) as JumlahBiaya FROM TblBiayaDtl a INNER JOIN TblBiayaMst b on b.IdBiayaMst = a.IdBiayaMst WHERE b.JenisBiayaMst != 'Pajak' AND SUBSTRING(b.TanggalBiayaMst,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalBiayaMst,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        SqlCommand commJmlBiaya = new SqlCommand(jmlBiaya,sqlConnection);
                        using (SqlDataReader drJmlBiaya = commJmlBiaya.ExecuteReader())
                        {
                            if (drJmlBiaya.Read())
                            {
                                var GTotJmlBiaya = (decimal)drJmlBiaya["JumlahBiaya"];
                                jmlBiayaDec = GTotJmlBiaya;
                                if (GTotJmlBiaya != 0)
                                {
                                    GTBiaya.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotJmlBiaya);
                                    nrcObj.NilaiTotalBiaya = GTotJmlBiaya;
                                }
                                else
                                {
                                    GTBiaya.Text = jmlBiayaVal;
                                    nrcObj.NilaiTotalBiaya = jmlBiayaDec;
                                }
                            }
                            else
                            {
                                GTBiaya.Text = jmlBiayaVal;
                                nrcObj.NilaiTotalBiaya = jmlBiayaDec;
                            }
                        }
                        GTLaba.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", lbKtrDec - jmlBiayaDec);
                        nrcObj.NilaiJumlahLabaDanBiaya = lbKtrDec - jmlBiayaDec;
                        string pajakPrm = "SELECT b.JenisBiayaMst,SUM(a.HargaBiayaDtl) as JumlahBiaya FROM TblBiayaDtl a INNER JOIN TblBiayaMst b on b.IdBiayaMst = a.IdBiayaMst WHERE b.JenisBiayaMst = 'Pajak' AND SUBSTRING(b.TanggalBiayaMst,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalBiayaMst,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "' GROUP BY b.JenisBiayaMst";
                        using (SqlCommand commStrPajak = new SqlCommand(pajakPrm, sqlConnection))
                        {
                            //StrQuer.CommandType = CommandType.StoredProcedure;
                            rptListPajak.DataSource = commStrPajak.ExecuteReader();
                            rptListPajak.DataBind();
                        }
                        using (SqlDataAdapter pjkAdapter = new SqlDataAdapter())
                        {
                            pjkAdapter.SelectCommand = new SqlCommand(pajakPrm, sqlConnection);
                            pjkAdapter.Fill(dtPajak);
                            ViewState["Pjktbl"] = dtPajak;
                        }
                        string queryLbStlhPjk = "SELECT COALESCE(SUM(a.HargaBiayaDtl),0) as JumlahBiaya FROM TblBiayaDtl a INNER JOIN TblBiayaMst b on b.IdBiayaMst = a.IdBiayaMst WHERE b.JenisBiayaMst = 'Pajak' AND SUBSTRING(b.TanggalBiayaMst,4,2) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalBiayaMst,7,4) = '" + ddlThnVis.SelectedValue.Trim() + "'";
                        SqlCommand commLbStlhPjk = new SqlCommand(queryLbStlhPjk, sqlConnection);
                        using(SqlDataReader drLbStlhPjk = commLbStlhPjk.ExecuteReader())
                        {
                            if (drLbStlhPjk.Read())
                            {
                                var GTotLbPjk = (decimal)drLbStlhPjk["JumlahBiaya"];
                                lbStlhPajakDec = GTotLbPjk;
                                if (GTotLbPjk != 0)
                                {
                                    GTSetelahPajak.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", (lbKtrDec - jmlBiayaDec) - GTotLbPjk);
                                    nrcObj.NilaiTotalPajak = GTotLbPjk;
                                    nrcObj.NilaiTotalSetelahPajak = (lbKtrDec - jmlBiayaDec) - GTotLbPjk;
                                }
                                else
                                {
                                    GTSetelahPajak.Text = lbStlhPajakVal;
                                    nrcObj.NilaiTotalPajak = 0;
                                    nrcObj.NilaiTotalSetelahPajak = (lbKtrDec - jmlBiayaDec) - nrcObj.NilaiTotalPajak;
                                }
                            }
                            else
                            {
                                GTSetelahPajak.Text = lbStlhPajakVal;
                                nrcObj.NilaiTotalPajak = 0;
                                nrcObj.NilaiTotalSetelahPajak = (lbKtrDec - jmlBiayaDec) - nrcObj.NilaiTotalPajak;
                            }
                        }
                        if (nrcObj.NilaiTotalSetelahPajak > 0)
                        {
                            GTRounding.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", Math.Round(nrcObj.NilaiTotalSetelahPajak));
                        }
                        else
                        {
                            GTRounding.Text = "- " + string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", Math.Round(nrcObj.NilaiTotalSetelahPajak));
                        }
                        nrcObj.NilaiTotalSetelahRounding = decimal.Parse(Regex.Replace(GTSetelahPajak.Text.Trim(), @"[^\d.,]", ""));
                        if(decimal.Parse(Regex.Replace(GTRounding.Text.Trim(), @"[^\d.,]", "")) != 0)
                        {
                            btnCetakGnrt.Enabled = true;
                            ViewState["NrcObject"] = nrcObj;
                        }
                        else
                        {
                            btnCetakGnrt.Enabled = false;
                        }
                    }
                    else
                    {
                        nrcObj = new Neraca();
                        nrcObj.JenisNeraca = "Tahun";
                        //string pnjVal = "0";
                        //string pmbVal = "0";
                        string lbKtrValQty = "0";
                        string lbKtrVal = "0";
                        string jmlBiayaVal = "0";
                        string lbStlhPajakVal = "0";
                        //decimal pnjDec = 0;
                        //decimal pmbDec = 0;
                        decimal lbKtrDecQty = 0;
                        decimal lbKtrDec = 0;
                        decimal jmlBiayaDec = 0;
                        decimal lbStlhPajakDec = 0;
                        string tahunTest =  (Convert.ToInt32(ddlPilihPeriode.SelectedValue.Trim()) + 1).ToString();
                        var test = new List<decimal>();
                        for(int j = 1; j <= 12;j++)
                        {
                            int days = DateTime.DaysInMonth(Convert.ToInt32(ddlPilihPeriode.SelectedValue.Trim()),j);
                            var bulanString = "";
                            if(j<10)
                            {
                                bulanString = "0" + j.ToString();
                            }
                            else
                            {
                                bulanString = j.ToString();
                            }
                            for (int i = 1; i <= days; i++)
                            {
                                var tanggalString = "";
                                if (i < 10)
                                {
                                    tanggalString = "0" + i.ToString();
                                }
                                else
                                {
                                    tanggalString = i.ToString();
                                }
                                string queryLbKtrTest = "; with list_tgl_beli as(select MAX(CONVERT(DATETIME,b.TanggalPOPembelian,103)) as tglBeli,IdBarang from POPembelianDtl a INNER JOIN POPembelianMst b on b.IdPOPembelianMst = a.IdPoPembelianMst where CONVERT(DATETIME,b.TanggalPOPembelian,103) <= CONVERT(DATETIME,'" + tanggalString.Trim() + "/" + bulanString.Trim() + "/" + ddlPilihPeriode.SelectedValue.Trim() + "',103) group by IdBarang),list_hrg_beli as(select  HargaPOPembelianDtl,b.IdBarang,TanggalPOPembelian,ROW_NUMBER() Over(partition by b.IdBarang order by abs(DATEDIFF(ss, CONVERT(DATETIME,TanggalPOPembelian,103), b.tglBeli)) asc) as RowNum from POPembelianDtl a inner join list_tgl_beli b on a.IdBarang = b.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst) select COALESCE(SUM(CAST((c.QtyBarangPOPenjualanDtl * (c.HargaPOPenjualanDtl-f.HargaPOPembelianDtl))as decimal(18,2))),0) as TotSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN list_hrg_beli f on f.IdBarang = c.IdBarang INNER JOIN Barang g on g.IdBarang = f.IdBarang INNER JOIN Supplier h on h.IdSupplier = g.IdSupplier where SUBSTRING(a.TanggalPOPenjualan,1,2) = '" + tanggalString.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,4,2) = '" + bulanString.Trim() + "' AND SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND StatusInvoicePenjualan != 'RETUR' AND RowNum = 1";
                                SqlCommand commLbKtrTest = new SqlCommand(queryLbKtrTest, sqlConnection);
                                using (SqlDataReader drLbKtr = commLbKtrTest.ExecuteReader())
                                {
                                    if (drLbKtr.Read())
                                    {
                                        var GTotLbKtr = (decimal)drLbKtr["TotSelisih"];
                                        test.Add(GTotLbKtr);
                                    }
                                }
                            }
                        }
                        string queryLbKtr = "; with list_tgl_beli as(select MAX(CONVERT(DATETIME,b.TanggalPOPembelian,103)) as tglBeli,IdBarang from POPembelianDtl a INNER JOIN POPembelianMst b on b.IdPOPembelianMst = a.IdPoPembelianMst where CONVERT(DATETIME,b.TanggalPOPembelian,103) <= CONVERT(DATETIME,'01/01/" + tahunTest + "',103) group by IdBarang),list_hrg_beli as(select  HargaPOPembelianDtl,b.IdBarang,TanggalPOPembelian,ROW_NUMBER() Over(partition by b.IdBarang order by abs(DATEDIFF(ss, CONVERT(DATETIME,TanggalPOPembelian,103), b.tglBeli)) asc) as RowNum from POPembelianDtl a inner join list_tgl_beli b on a.IdBarang = b.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst) select COALESCE(SUM(CAST((c.QtyBarangPOPenjualanDtl * (c.HargaPOPenjualanDtl-f.HargaPOPembelianDtl))as decimal(18,2))),0) as TotSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN list_hrg_beli f on f.IdBarang = c.IdBarang INNER JOIN Barang g on g.IdBarang = f.IdBarang INNER JOIN Supplier h on h.IdSupplier = g.IdSupplier where SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' AND StatusInvoicePenjualan != 'RETUR' AND RowNum = 1";
                        //string queryLbKtr = "SELECT COALESCE(SUM(CAST((c.QtyBarangPOPenjualanDtl * (c.HargaPOPenjualanDtl-f.HargaPOPembelianDtl))as decimal(18,2))),0) as TotSelisih FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN Barang d on d.IdBarang = c.IdBarang INNER JOIN Supplier e on e.IdSupplier = d.IdSupplier INNER JOIN POPembelianDtl f on f.IdBarang = d.IdBarang WHERE SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + ddlPilihPeriode.SelectedValue.Trim() + "'";
                        //string queryJmlQty = "SELECT COALESCE(SUM(c.QtyBarangPOPenjualanDtl),0) as TotalQTY FROM POPenjualanMst a INNER JOIN Customer b on b.IdCustomer = a.IdCustomer INNER JOIN POPenjualanDtl c on c.IdPOPenjualanMst = a.IdPOPenjualanMst INNER JOIN Barang d on d.IdBarang = c.IdBarang INNER JOIN Supplier e on e.IdSupplier = d.IdSupplier INNER JOIN POPembelianDtl f on f.IdBarang = d.IdBarang WHERE SUBSTRING(a.TanggalPOPenjualan,7,4) = '" + ddlPilihPeriode.SelectedValue.Trim() + "'";
                        string queryJmlQty = ";with table1 as (SELECT COALESCE(SUM(B.QtyBarangPOPenjualanDtl),0) as TotalQTY FROM POPenjualanMst A INNER JOIN POPenjualanDtl B on A.IdPOPenjualanMst = B.IdPOPenjualanMst WHERE SUBSTRING(TanggalPOPenjualan,7,4) = '" + ddlPilihPeriode.SelectedValue.Trim() + "'), table2 as (SELECT COALESCE(SUM(a.QtyBarangPOPenjualanReturDtl),0) as TotalQTYRetur FROM POPenjualanReturDtl a INNER JOIN POPenjualanMst b on a.IdPOPenjualanMst = b.IdPOPenjualanMst WHERE SUBSTRING(b.TanggalPOPenjualan,7,4) = '" + ddlPilihPeriode.SelectedValue.Trim() + "') SELECT a.TotalQTY - b.TotalQTYRetur as TotalQTY from table1 a, table2 b";
                        //string queryTotPmb = "SELECT COALESCE(SUM(a.TotalHargaPOPembelianDtl),0) as TotalPembelian FROM POPembelianDtl a INNER JOIN Barang b on b.IdBarang = a.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst WHERE SUBSTRING(TanggalPOJatuhTmpPembelian,7,4) = '" + ddlPilihPeriode.SelectedValue.Trim() + "'";
                        //string queryTotPnj = "SELECT COALESCE(SUM(a.TotalHargaPOPenjualanDtl),0) as TotalPenjualan FROM POPenjualanDtl a INNER JOIN Barang b on b.IdBarang = a.IdBarang INNER JOIN POPenjualanMst c on c.IdPOPenjualanMst = a.IdPOPenjualanMst WHERE SUBSTRING(TanggalPOJatuhTmpPenjualan,7,4) = '" + ddlPilihPeriode.SelectedValue.Trim() + "'";
                        //SqlCommand commPmb = new SqlCommand(queryTotPmb, sqlConnection);
                        //using (SqlDataReader drPmb = commPmb.ExecuteReader())
                        //{
                        //    if (drPmb.Read())
                        //    {
                        //        var GTotPmb = (decimal)drPmb["TotalPembelian"];
                        //        pmbDec = GTotPmb;
                        //        if(GTotPmb != 0)
                        //        {
                        //            GTPembelian.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPmb);
                        //            nrcObj.NilaiHPP = GTotPmb;
                        //        }
                        //        else
                        //        {
                        //            GTPembelian.Text = pmbVal;
                        //            nrcObj.NilaiHPP = pmbDec;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        GTPembelian.Text = pmbVal;
                        //        nrcObj.NilaiHPP = pmbDec;
                        //    }
                        //}
                        //SqlCommand commPnj = new SqlCommand(queryTotPnj, sqlConnection);
                        //using (SqlDataReader drPnj = commPnj.ExecuteReader())
                        //{
                        //    if (drPnj.Read())
                        //    {
                        //        var GTotPnj = (decimal)drPnj["TotalPenjualan"];
                        //        pnjDec = GTotPnj;
                        //        if (GTotPnj != 0)
                        //        {
                        //            GTPenjualan.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPnj);
                        //            nrcObj.NilaiPenjualan = GTotPnj;
                        //        }
                        //        else
                        //        {
                        //            GTPenjualan.Text = pnjVal;
                        //            nrcObj.NilaiPenjualan = pnjDec;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        GTPenjualan.Text = pnjVal;
                        //        nrcObj.NilaiPenjualan = pnjDec;
                        //    }
                        //}
                        //txtLabaKotor.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", pnjDec - pmbDec);
                        //nrcObj.NilaiLabaKotor = pnjDec - pmbDec;
                        SqlCommand commLbKtrQty = new SqlCommand(queryJmlQty, sqlConnection);
                        using (SqlDataReader drLbKtrQty = commLbKtrQty.ExecuteReader())
                        {
                            if (drLbKtrQty.Read())
                            {
                                var GTotLbKtrQty = (decimal)drLbKtrQty["TotalQTY"];
                                lbKtrDecQty = GTotLbKtrQty;
                                if (GTotLbKtrQty != 0)
                                {
                                    lblQtyVal.Text = string.Format("{0:n}", GTotLbKtrQty + " KG");
                                    nrcObj.NilaiSelisihQty = GTotLbKtrQty;
                                }
                                else
                                {
                                    lblQtyVal.Text = lbKtrValQty;
                                    nrcObj.NilaiSelisihQty = lbKtrDecQty;
                                }
                            }
                            else
                            {
                                lblQtyVal.Text = lbKtrValQty;
                                nrcObj.NilaiSelisihQty = lbKtrDecQty;
                            }
                        }
                        SqlCommand commLbKtr = new SqlCommand(queryLbKtr, sqlConnection);
                        using (SqlDataReader drLbKtr = commLbKtr.ExecuteReader())
                        {
                            if (drLbKtr.Read())
                            {
                                var GTotLbKtr = (decimal)drLbKtr["TotSelisih"];
                                lbKtrDec = GTotLbKtr;
                                if (GTotLbKtr != 0)
                                {
                                    txtLabaKotor.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotLbKtr);
                                    nrcObj.NilaiLabaKotor = GTotLbKtr;
                                }
                                else
                                {
                                    txtLabaKotor.Text = lbKtrVal;
                                    nrcObj.NilaiLabaKotor = lbKtrDec;
                                }
                            }
                            else
                            {
                                txtLabaKotor.Text = lbKtrVal;
                                nrcObj.NilaiLabaKotor = lbKtrDec;
                            }
                        }
                        txtLabaKotor.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", test.Sum());
                        nrcObj.NilaiLabaKotor = test.Sum();
                        lbKtrDec = test.Sum();
                        string biayaPrm = "SELECT b.JenisBiayaMst,SUM(a.HargaBiayaDtl) as JumlahBiaya FROM TblBiayaDtl a INNER JOIN TblBiayaMst b on b.IdBiayaMst = a.IdBiayaMst WHERE b.JenisBiayaMst != 'Pajak' AND SUBSTRING(b.TanggalBiayaMst,7,4) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' GROUP BY b.JenisBiayaMst";
                        using (SqlCommand strBiaya = new SqlCommand(biayaPrm, sqlConnection))
                        {
                            //StrQuer.CommandType = CommandType.StoredProcedure;
                            rptListBiaya.DataSource = strBiaya.ExecuteReader();
                            rptListBiaya.DataBind();
                        }
                        using (SqlDataAdapter biyAdapter = new SqlDataAdapter())
                        {
                            biyAdapter.SelectCommand = new SqlCommand(biayaPrm, sqlConnection);
                            biyAdapter.Fill(dtBiaya);
                            ViewState["Biytbl"] = dtBiaya;
                        }
                        string jmlBiaya = "SELECT COALESCE(SUM(a.HargaBiayaDtl),0) as JumlahBiaya FROM TblBiayaDtl a INNER JOIN TblBiayaMst b on b.IdBiayaMst = a.IdBiayaMst WHERE b.JenisBiayaMst != 'Pajak' AND SUBSTRING(b.TanggalBiayaMst,7,4) = '" + ddlPilihPeriode.SelectedValue.Trim() + "'";
                        SqlCommand commJmlBiaya = new SqlCommand(jmlBiaya, sqlConnection);
                        using (SqlDataReader drJmlBiaya = commJmlBiaya.ExecuteReader())
                        {
                            if (drJmlBiaya.Read())
                            {
                                var GTotJmlBiaya = (decimal)drJmlBiaya["JumlahBiaya"];
                                jmlBiayaDec = GTotJmlBiaya;
                                if (GTotJmlBiaya != 0)
                                {
                                    GTBiaya.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotJmlBiaya);
                                    nrcObj.NilaiTotalBiaya = GTotJmlBiaya;
                                }
                                else
                                {
                                    GTBiaya.Text = jmlBiayaVal;
                                    nrcObj.NilaiTotalBiaya = jmlBiayaDec;
                                }
                            }
                            else
                            {
                                GTBiaya.Text = jmlBiayaVal;
                                nrcObj.NilaiTotalBiaya = jmlBiayaDec;
                            }
                        }
                        GTLaba.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", lbKtrDec - jmlBiayaDec);
                        nrcObj.NilaiJumlahLabaDanBiaya = lbKtrDec - jmlBiayaDec;
                        string pajakPrm = "SELECT b.JenisBiayaMst,SUM(a.HargaBiayaDtl) as JumlahBiaya FROM TblBiayaDtl a INNER JOIN TblBiayaMst b on b.IdBiayaMst = a.IdBiayaMst WHERE b.JenisBiayaMst = 'Pajak' AND SUBSTRING(b.TanggalBiayaMst,7,4) = '" + ddlPilihPeriode.SelectedValue.Trim() + "' GROUP BY b.JenisBiayaMst";
                        using (SqlCommand commStrPajak = new SqlCommand(pajakPrm, sqlConnection))
                        {
                            //StrQuer.CommandType = CommandType.StoredProcedure;
                            rptListPajak.DataSource = commStrPajak.ExecuteReader();
                            rptListPajak.DataBind();
                        }
                        using (SqlDataAdapter pjkAdapter = new SqlDataAdapter())
                        {
                            pjkAdapter.SelectCommand = new SqlCommand(pajakPrm, sqlConnection);
                            pjkAdapter.Fill(dtPajak);
                            ViewState["Pjktbl"] = dtPajak;
                        }
                        string queryLbStlhPjk = "SELECT COALESCE(SUM(a.HargaBiayaDtl),0) as JumlahBiaya FROM TblBiayaDtl a INNER JOIN TblBiayaMst b on b.IdBiayaMst = a.IdBiayaMst WHERE b.JenisBiayaMst = 'Pajak' AND SUBSTRING(b.TanggalBiayaMst,7,4) = '" + ddlPilihPeriode.SelectedValue.Trim() + "'";
                        SqlCommand commLbStlhPjk = new SqlCommand(queryLbStlhPjk, sqlConnection);
                        using (SqlDataReader drLbStlhPjk = commLbStlhPjk.ExecuteReader())
                        {
                            if (drLbStlhPjk.Read())
                            {
                                var GTotLbPjk = (decimal)drLbStlhPjk["JumlahBiaya"];
                                lbStlhPajakDec = GTotLbPjk;
                                if (GTotLbPjk != 0)
                                {
                                    GTSetelahPajak.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", (lbKtrDec - jmlBiayaDec) - GTotLbPjk);
                                    nrcObj.NilaiTotalPajak = GTotLbPjk;
                                    nrcObj.NilaiTotalSetelahPajak = (lbKtrDec - jmlBiayaDec) - GTotLbPjk;
                                }
                                else
                                {
                                    GTSetelahPajak.Text = lbStlhPajakVal;
                                    nrcObj.NilaiTotalPajak = 0;
                                    nrcObj.NilaiTotalSetelahPajak = (lbKtrDec - jmlBiayaDec) - nrcObj.NilaiTotalPajak;
                                }
                            }
                            else
                            {
                                GTSetelahPajak.Text = lbStlhPajakVal;
                                nrcObj.NilaiTotalPajak = 0;
                                nrcObj.NilaiTotalSetelahPajak = (lbKtrDec - jmlBiayaDec) - nrcObj.NilaiTotalPajak;
                            }
                        }
                        if (nrcObj.NilaiTotalSetelahPajak > 0)
                        {
                            GTRounding.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", Math.Round(nrcObj.NilaiTotalSetelahPajak));
                        }
                        else
                        {
                            GTRounding.Text = "- " + string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", Math.Round(nrcObj.NilaiTotalSetelahPajak));
                        }
                        //GTRounding.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", Math.Round(decimal.Parse(Regex.Replace(GTSetelahPajak.Text.Trim(), @"[^\d.,]", ""))));
                        nrcObj.NilaiTotalSetelahRounding = decimal.Parse(Regex.Replace(GTSetelahPajak.Text.Trim(), @"[^\d.,]", ""));
                        if (decimal.Parse(Regex.Replace(GTRounding.Text.Trim(), @"[^\d.,]", "")) != 0)
                        {
                            btnCetakGnrt.Enabled = true;
                            ViewState["NrcObject"] = nrcObj;
                        }
                        else
                        {
                            btnCetakGnrt.Enabled = false;
                        }
                    }
                }
                catch (Exception ex)
                { throw ex; }
                finally { sqlConnection.Close(); sqlConnection.Dispose(); }
            }
        }

        protected void btnCetakGnrt_Click(object sender, EventArgs e)
        {
            Button btn = (Button)(sender);
            if (ViewState["Biytbl"] != null)
            {
                DataTable dtBiaya = (DataTable)ViewState["Biytbl"];
                if (ViewState["Pjktbl"] != null)
                {
                    DataTable dtPajak = (DataTable)ViewState["Pjktbl"];
                    if (ViewState["NrcObject"] != null)
                    {
                        nrcObj = (Neraca)ViewState["NrcObject"];
                        string fileName = string.Empty;
                        DateTime fileCreationDatetime = DateTime.Now;
                        fileName = string.Format("{0}.pdf", "Neraca_" + fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + ddlCheckBulanTahun.SelectedValue.Trim() + "_" + ddlPilihPeriode.SelectedValue.Trim());
                        string pdfPath = Server.MapPath(@"~\PDFs\") + fileName;
                        using (FileStream msReport = new FileStream(pdfPath, FileMode.Create))
                        {
                            using (Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 140f, 10f))
                            {
                                try
                                {
                                    // step 2
                                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                                    pdfWriter.PageEvent = new TestCoba.ITextEventNrc();

                                    //open the stream 
                                    pdfDoc.Open();
                                    PdfContentByte canvas = pdfWriter.DirectContent;

                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Neraca Laba Rugi" + " Periode " + ddlCheckBulanTahun.SelectedValue.Trim() + "-" + ddlPilihPeriode.SelectedValue.Trim(), FontFactory.GetFont("Arial", 14, Font.UNDERLINE, BaseColor.BLACK)), 180, 750, 0);
                                    //ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("1", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 50, 650, 0);
                                    //ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Penjualan", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 70, 650, 0);
                                    //ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", nrcObj.NilaiPenjualan), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 450, 650, 0);

                                    //ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("2", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 50, 630, 0);
                                    //ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Harga Pokok Penjualan", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 70, 630, 0);
                                    //ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", nrcObj.NilaiHPP), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 450, 630, 0);

                                    //ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("_________________(-)", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 450, 610, 0);

                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("1", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 50, 650, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Laba Kotor", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 70, 650, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", nrcObj.NilaiLabaKotor), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.RED)), 450, 650, 0);

                                    float xBiy = 630;
                                    int nomorIndex = 0;
                                    decimal niLaiTotBiayaDec = 0;

                                    for (int i = 0; i < dtBiaya.Rows.Count;i++)
                                    {
                                        xBiy -= 20;
                                        nomorIndex = (i + 1) + 1;
                                        var namaDtBiaya = (from item in dtBiaya.AsEnumerable()
                                                           select item.Field<string>("JenisBiayaMst")).ToList();
                                        var nilaiDtBiaya = (from item in dtBiaya.AsEnumerable()
                                                            select item.Field<decimal>("JumlahBiaya")).ToList();
                                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(Convert.ToString((i + 1) + 1), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 50, xBiy, 0);
                                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(Convert.ToString(namaDtBiaya[i]), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 70, xBiy, 0);
                                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", nilaiDtBiaya[i]), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 450, xBiy, 0);
                                        niLaiTotBiayaDec += nilaiDtBiaya[i];
                                    }
                                    xBiy -= 20;
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("_________________(+)", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 450, xBiy, 0);
                                    xBiy -= 40;
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Jumlah", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 70, xBiy, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", niLaiTotBiayaDec), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.RED)), 450, xBiy, 0);
                                    xBiy -= 40;
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(Convert.ToString(nomorIndex + 2), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 50, xBiy, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Laba", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 70, xBiy, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", nrcObj.NilaiJumlahLabaDanBiaya), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 450, xBiy, 0);
                                    xBiy -= 20;
                                    decimal niLaiTotPajakDec = 0;
                                    for (int i = 0; i < dtPajak.Rows.Count; i++)
                                    {
                                        nomorIndex += 2;
                                        var namaDtPajak = (from item in dtPajak.AsEnumerable()
                                                           select item.Field<string>("JenisBiayaMst")).ToList();
                                        var nilaiDtPajak = (from item in dtPajak.AsEnumerable()
                                                            select item.Field<decimal>("JumlahBiaya")).ToList();
                                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(Convert.ToString(nomorIndex + 1), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 50, xBiy, 0);
                                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(Convert.ToString(namaDtPajak[i]), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 70, xBiy, 0);
                                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", nilaiDtPajak[i]), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 450, xBiy, 0);
                                        niLaiTotPajakDec += nilaiDtPajak[i];
                                        xBiy -= 20;
                                    }
                                    nomorIndex += 1;
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("_________________(-)", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 450, xBiy, 0);
                                    xBiy -= 40;
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(Convert.ToString(nomorIndex + 1), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 50, xBiy, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Laba Setelah Pajak", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 70, xBiy, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", nrcObj.NilaiTotalSetelahPajak), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.RED)), 450, xBiy, 0);
                                    xBiy -= 20;
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Dibulatkan Penuh", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 70, xBiy, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", nrcObj.NilaiTotalSetelahRounding), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.RED)), 450, xBiy, 0);
                                    //msReport.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                                    pdfDoc.Close();
                                    //byte[] bytes = memoryStream.ToArray();
                                    //memoryStream.Close();
                                    msReport.Close();
                                    Response.Clear();
                                    Response.ContentType = "application/pdf";
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                                    Response.ContentType = "application/pdf";
                                    Response.Buffer = true;
                                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    Response.WriteFile(pdfPath);
                                    //Response.BinaryWrite(bytes);
                                    Response.End();
                                    Response.Close();
                                }
                                catch (Exception ex)
                                {
                                    //handle exception
                                    throw ex;
                                }
                                finally
                                {
                                }
                            }
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Data Kosong!" + "');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Data Pajak Kosong!" + "');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Data Biaya Kosong!" + "');", true);
            }
        }
    }
}