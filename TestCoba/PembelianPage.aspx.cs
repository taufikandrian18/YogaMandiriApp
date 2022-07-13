using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;

namespace TestCoba
{
    public partial class PembelianPage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["Role"] != null)
                {
                    DataTable dt1 = checkDatabaseHasData();
                    var test = (from item in dt1.AsEnumerable()
                                select item.Field<int>("IdInventory")).FirstOrDefault();
                    if (test != 0)
                    {
                        int year = DateTime.Now.Year;
                        for (int i = year - 20; i <= year + 50; i++)
                        {
                            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(i.ToString());
                            ddlYears.Items.Add(li);
                        }
                        ddlYears.Items.FindByText(year.ToString()).Selected = true;
                        Role r = new Role();
                        r = (Role)Session["Role"];
                        if (r.IdRole == 2)
                        {
                            btnValVsbl.Visible = false;
                            thVal.ColSpan = 0;
                            //thVal.Visible = false;
                        }
                    }
                    else
                    {
                        Response.Redirect("Index.aspx");
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }
        private DataTable checkDatabaseHasData()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Inventory");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        private void BindRepeator(string tanggalBeli,string bulan,string tahun)
        {
            string queryFilParam = "SELECT * FROM POPembelianMst a INNER JOIN Supplier b on a.IdSupplier = b.IdSupplier INNER JOIN Inventory c on a.IdInventory = c.IdInventory WHERE SUBSTRING(TanggalPOPembelian,1,2) = '"+tanggalBeli+"' AND SUBSTRING(TanggalPOPembelian,4,2) = '" + bulan + "' AND SUBSTRING(TanggalPOPembelian,7,4) = '" + tahun + "' ";
            string queryJmlPemb = "SELECT COALESCE(SUM(TotalTagihanPembelian),0) as TotalPembelian FROM POPembelianMst a INNER JOIN Supplier b on a.IdSupplier = b.IdSupplier INNER JOIN Inventory c on a.IdInventory = c.IdInventory WHERE SUBSTRING(TanggalPOPembelian,1,2) = '" + tanggalBeli + "' AND SUBSTRING(TanggalPOPembelian,4,2) = '" + bulan + "' AND SUBSTRING(TanggalPOPembelian,7,4) = '" + tahun + "' ";
            string queryJmlQty = "SELECT COALESCE(SUM(B.QtyBarangPOPembelianDtl),0) as TotalQTY FROM POPembelianMst A INNER JOIN POPembelianDtl B on A.IdPOPembelianMst = B.IdPoPembelianMst WHERE SUBSTRING(TanggalPOPembelian,1,2) = '" + tanggalBeli + "' AND SUBSTRING(TanggalPOPembelian,4,2) = '" + bulan + "' AND SUBSTRING(TanggalPOPembelian,7,4) = '" + tahun + "' ";
            string queryJmlPembRtr = "SELECT COALESCE(SUM(a.TotalHargaPOPembelianReturDtl),0) as TotalPembelianRetur FROM POPembelianReturDtl a INNER JOIN POPembelianMst b on a.IdPoPembelianMst = b.IdPoPembelianMst WHERE SUBSTRING(b.TanggalPOPembelian,1,2) = '" + tanggalBeli + "' AND SUBSTRING(b.TanggalPOPembelian,4,2) = '" + bulan + "' AND SUBSTRING(b.TanggalPOPembelian,7,4) = '" + tahun + "' ";
            string queryJmlQtyRtr = "SELECT COALESCE(SUM(a.QtyBarangPOPembelianReturDtl),0) as TotalQTYReturRetur FROM POPembelianReturDtl a INNER JOIN POPembelianMst b on a.IdPoPembelianMst = b.IdPoPembelianMst WHERE SUBSTRING(b.TanggalPOPembelian,1,2) = '" + tanggalBeli + "' AND SUBSTRING(b.TanggalPOPembelian,4,2) = '" + bulan + "' AND SUBSTRING(b.TanggalPOPembelian,7,4) = '" + tahun + "' ";
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                if (ddlStatusInvoice.SelectedValue.Trim() != "" && ddlSortBy.SelectedValue.Trim() != "")
                {
                    decimal GTotPmb = 0;
                    decimal GTotQty = 0;
                    string queryParam = "AND a.StatusInvoicePembelianMst = '" + ddlStatusInvoice.SelectedValue + "'";
                    string queryOrderPrm = " ORDER BY " + ddlSortBy.SelectedValue.Trim() + "";
                    using (SqlCommand StrQuer = new SqlCommand(queryFilParam+queryParam+queryOrderPrm, sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        RepeaterListPembelian.DataSource = StrQuer.ExecuteReader();
                        RepeaterListPembelian.DataBind();
                    }
                    using (SqlCommand StrQuer = new SqlCommand(queryJmlPemb + queryParam, sqlConnection))
                    {
                        using (SqlDataReader drTotPmb = StrQuer.ExecuteReader())
                        {
                            if (drTotPmb.Read())
                            {
                                GTotPmb = (decimal)drTotPmb["TotalPembelian"];
                                using (SqlCommand StrQuerRtr = new SqlCommand(queryJmlPembRtr, sqlConnection))
                                {
                                    using (SqlDataReader drTotPmbRtr = StrQuerRtr.ExecuteReader())
                                    {
                                        if (drTotPmbRtr.Read())
                                        {
                                            var GTotPmbRtr = (decimal)drTotPmbRtr["TotalPembelianRetur"];
                                            GTPembelian.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPmb - GTotPmbRtr);
                                        }
                                        else
                                        {
                                            GTPembelian.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPmb);
                                        }
                                    }
                                }
                                //GTPembelian.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPmb);
                            }
                            else
                            {
                                GTPembelian.Text = "0";
                            }
                        }
                    }
                    using (SqlCommand StrQuer = new SqlCommand(queryJmlQty + queryParam, sqlConnection))
                    {
                        using (SqlDataReader drTotQty = StrQuer.ExecuteReader())
                        {
                            if (drTotQty.Read())
                            {
                                GTotQty = (decimal)drTotQty["TotalQTY"];
                                using (SqlCommand StrQuerRtr = new SqlCommand(queryJmlQtyRtr, sqlConnection))
                                {
                                    using (SqlDataReader drTotPmbRtr = StrQuerRtr.ExecuteReader())
                                    {
                                        if (drTotPmbRtr.Read())
                                        {
                                            var GTotPmbRtr = (decimal)drTotPmbRtr["TotalQTYReturRetur"];
                                            GTQty.Text = string.Format("{0:n}", GTotQty - GTotPmbRtr) + " KG";
                                        }
                                        else
                                        {
                                            GTQty.Text = string.Format("{0:n}", GTotQty) + " KG";
                                        }
                                    }
                                }
                                //GTQty.Text = string.Format("{0:n}", GTotQty) + " KG";
                            }
                            else
                            {
                                GTQty.Text = "0";
                            }
                        }
                    }
                }
                else
                {
                    decimal GTotPmb = 0;
                    decimal GTotQty = 0;
                    string queryParam = "";
                    string queryOrderPrm = " ORDER BY a.NoInvoicePembelian";
                    if (ddlStatusInvoice.SelectedValue.Trim() != "")
                    {
                        queryParam += " AND a.StatusInvoicePembelianMst = '" + ddlStatusInvoice.SelectedValue + "'";
                    }
                    if (ddlSortBy.SelectedValue.Trim() != "")
                    {
                        queryOrderPrm = " ORDER BY " + ddlSortBy.SelectedValue.Trim() + "";
                    }
                    string finalFill = queryFilParam+queryParam+queryOrderPrm;
                    using (SqlCommand StrQuer = new SqlCommand(finalFill, sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        RepeaterListPembelian.DataSource = StrQuer.ExecuteReader();
                        RepeaterListPembelian.DataBind();
                    }
                    using (SqlCommand StrQuer = new SqlCommand(queryJmlPemb + queryParam, sqlConnection))
                    {
                        using (SqlDataReader drTotPmb = StrQuer.ExecuteReader())
                        {
                            if (drTotPmb.Read())
                            {
                                GTotPmb = (decimal)drTotPmb["TotalPembelian"];
                                using (SqlCommand StrQuerRtr = new SqlCommand(queryJmlPembRtr, sqlConnection))
                                {
                                    using (SqlDataReader drTotPmbRtr = StrQuerRtr.ExecuteReader())
                                    {
                                        if (drTotPmbRtr.Read())
                                        {
                                            var GTotPmbRtr = (decimal)drTotPmbRtr["TotalPembelianRetur"];
                                            GTPembelian.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPmb - GTotPmbRtr);
                                        }
                                        else
                                        {
                                            GTPembelian.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPmb);
                                        }
                                    }
                                }
                                //GTPembelian.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPmb);
                            }
                            else
                            {
                                GTPembelian.Text = "0";
                            }
                        }
                    }
                    using (SqlCommand StrQuer = new SqlCommand(queryJmlQty + queryParam, sqlConnection))
                    {
                        using (SqlDataReader drTotQty = StrQuer.ExecuteReader())
                        {
                            if (drTotQty.Read())
                            {
                                GTotQty = (decimal)drTotQty["TotalQTY"];
                                using (SqlCommand StrQuerRtr = new SqlCommand(queryJmlQtyRtr, sqlConnection))
                                {
                                    using (SqlDataReader drTotPmbRtr = StrQuerRtr.ExecuteReader())
                                    {
                                        if (drTotPmbRtr.Read())
                                        {
                                            var GTotPmbRtr = (decimal)drTotPmbRtr["TotalQTYReturRetur"];
                                            GTQty.Text = string.Format("{0:n}", GTotQty - GTotPmbRtr) + " KG";
                                        }
                                        else
                                        {
                                            GTQty.Text = string.Format("{0:n}", GTotQty) + " KG";
                                        }
                                    }
                                }
                                //GTQty.Text = string.Format("{0:n}", GTotQty) + " KG";
                            }
                            else
                            {
                                GTQty.Text = "0";
                            }
                        }
                    }
                }
                txtSearch.Enabled = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            BindRepeator(ddlTanggal.SelectedValue.Trim(),ddlMonths.SelectedValue.Trim(), ddlYears.SelectedValue.Trim());
        }

        private DataTable GetData(string query)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            SqlCommand cmd = new SqlCommand(query);
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        protected void btnCetakGnrt_Click(object sender, EventArgs e)
        {
            Button btn = (Button)(sender);
            DataRow drMst = GetData("SELECT * FROM POPembelianMst a INNER JOIN Supplier b on a.IdSupplier = b.IdSupplier WHERE IdPOPembelianMst = " + btn.CommandArgument).Rows[0];
            DataTable drDtl = new DataTable();
            if (drMst["StatusInvoicePembelianMst"].ToString().Trim() != "RETUR")
            {
                drDtl = GetData("SELECT B.BarangItemCode,B.NamaBarang,A.QtyBarangPOPembelianDtl,A.JumlahBoxPOPembelianDtl,A.HargaPOPembelianDtl,A.TotalHargaPOPembelianDtl FROM POPembelianDtl A INNER JOIN Barang B ON A.IdBarang = B.IdBarang WHERE A.IdPOPembelianMst = " + btn.CommandArgument);
            }
            else
            {
                drDtl = GetData("; with table1 as (SELECT b.IdBarang,b.QtyBarangPOPembelianReturDtl,b.HargaPOPembelianReturDtl,b.JumlahBoxPOPembelianReturDtl,b.TotalHargaPOPembelianReturDtl FROM POPembelianDtl a LEFT JOIN POPembelianReturDtl b on a.IdBarang = b.IdBarang WHERE a.IdPOPembelianMst = '"+ btn.CommandArgument + "' and b.IdPOPembelianMst IS NOT NULL), table2 as (select * from POPembelianDtl where IdPOPembelianMst = '"+ btn.CommandArgument + "') select c.BarangItemCode,a.IdBarang,c.NamaBarang,a.QtyBarangPOPembelianDtl-COALESCE(b.QtyBarangPOPembelianReturDtl,0) as QtyBarangPOPembelianDtl,a.HargaPOPembelianDtl-COALESCE(b.HargaPOPembelianReturDtl,0) as HargaPOPembelianDtl,a.JumlahBoxPOPembelianDtl-COALESCE(b.JumlahBoxPOPembelianReturDtl,0) as JumlahBoxPOPembelianDtl,a.TotalHargaPOPembelianDtl-COALESCE(b.TotalHargaPOPembelianReturDtl,0) as TotalHargaPOPembelianDtl from table2 a left join table1 b on a.IdBarang = b.IdBarang INNER JOIN Barang c on a.IdBarang = c.IdBarang WHERE a.QtyBarangPOPembelianDtl-COALESCE(b.QtyBarangPOPembelianReturDtl,0) != 0  GROUP BY c.BarangItemCode,a.IdBarang,c.NamaBarang,a.QtyBarangPOPembelianDtl,b.QtyBarangPOPembelianReturDtl,a.HargaPOPembelianDtl,b.HargaPOPembelianReturDtl,a.JumlahBoxPOPembelianDtl,b.JumlahBoxPOPembelianReturDtl,a.TotalHargaPOPembelianDtl,b.TotalHargaPOPembelianReturDtl");
            }   
            string fileName = string.Empty;
            DateTime fileCreationDatetime = DateTime.Now;
            fileName = string.Format("{0}.pdf", "Pembelian_" + fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + Server.UrlEncode(drMst["NoInvoicePembelian"].ToString()));
            string pdfPath = Server.MapPath(@"~\PDFs\") + fileName;
                //step 1
            using (FileStream msReport = new FileStream(pdfPath, FileMode.Create))
            {
                using (Document pdfDoc = new Document(PageSize.A5.Rotate()))
                {
                    try
                    {
                        // step 2
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                        pdfWriter.PageEvent = new TestCoba.ITextEvents() { parametro = Convert.ToInt32(btn.CommandArgument) };
                        //open the stream 
                        pdfDoc.Open();
                        int halaman = 0;
                        int temp = 0;
                        int nomer = 1;
                        float dataCount = 269;
                        PdfContentByte canvas = pdfWriter.DirectContent;
                        PdfPCell cell = null;
                        PdfPTable table = null;

                        for (int i = 0; i <= halaman; i++)
                        {
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Faktur Pembelian", FontFactory.GetFont("Courier", 18, Font.BOLD, BaseColor.BLACK)), 210, 330, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("No Faktur", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 370, 315, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 485, 315, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(drMst["NoInvoicePembelian"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 495, 315, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Tanggal", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 370, 303, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 485, 303, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(drMst["TanggalPOPembelian"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 495, 303, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Jns Tagihan", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 370, 291, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 485, 291, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(drMst["JenisTagihan"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 495, 291, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Jatuh Tempo", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 370, 279, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 485, 279, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(drMst["TanggalPOJatuhTmpPembelian"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 495, 279, 0);
                            pdfDoc.Add(new Paragraph(" "));
                            table = new PdfPTable(6);
                            table.WidthPercentage = 100;
                            table.SetWidths(new float[] { 0.3f, 2.6f, 1.2f, 0.5f, 1.5f, 2f });
                            table.TotalWidth = 400f;
                            table.SpacingBefore = 100f;
                            table.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell = new PdfPCell(new Phrase("No", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Nama Barang", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Qty Per Kg", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Box", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Harga Per Kg", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Total", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                            for (int j = temp; j < drDtl.Rows.Count; j++)
                            {
                                DataRow dtRow = drDtl.Rows[j];
                                if (dataCount > 190)
                                {
                                    cell = new PdfPCell(new Phrase(nomer.ToString(), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(dtRow["NamaBarang"].ToString(), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(dtRow["QtyBarangPOPembelianDtl"].ToString() + " Kg", FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(dtRow["JumlahBoxPOPembelianDtl"].ToString(), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(string.Format("{0:Rp ###,###,##0.00;(Rp ###,###,##0.00);'Rp 0'}", dtRow["HargaPOPembelianDtl"]), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(string.Format("{0:Rp ###,###,##0.00;(Rp ###,###,##0.00);'Rp 0'}", dtRow["TotalHargaPOPembelianDtl"]), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    table.AddCell(cell);
                                    dataCount -= 20;
                                    temp++;
                                    nomer++;
                                }
                                else
                                {
                                    if (j != drDtl.Rows.Count)
                                    {
                                        halaman += 1;
                                        temp = j;
                                        j = drDtl.Rows.Count;
                                        dataCount = 269;
                                    }
                                }
                            }
                            pdfDoc.Add(table);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(drMst["NoRekeningPembayaran"].ToString(), FontFactory.GetFont("Courier", 10, Font.UNDERLINE, BaseColor.BLACK)), 255, 125, 0);
                            if (temp != drDtl.Rows.Count)
                            {
                                pdfDoc.NewPage();
                            }
                            else
                            {
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Total Harga", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)), 365, 130, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(":", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)), 435, 130, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", drMst["TotalTagihanPembelian"]), FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)), 445, 130, 0);
                                pdfDoc.NewPage();
                            }
                        }
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
        private static void DrawLine(PdfWriter writer, float x1, float y1, float x2, float y2, BaseColor color)
        {
            PdfContentByte contentByte = writer.DirectContent;
            contentByte.SetColorStroke(color);
            contentByte.MoveTo(x1, y1);
            contentByte.LineTo(x2, y2);
            contentByte.Stroke();
        }
        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 1f;
            return cell;
        }
        private static PdfPCell ImageCell(string path, float scale, int align)
        {
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path));
            image.ScalePercent(scale);
            PdfPCell cell = new PdfPCell(image);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 0f;
            cell.PaddingTop = 0f;
            return cell;
        }
        protected void TestPdf()
        {
            Button btn = null; //(Button)(sender);
            DataRow drMst = GetData("SELECT * FROM POPembelianMst a INNER JOIN Supplier b on a.IdSupplier = b.IdSupplier WHERE IdPOPembelianMst = " + btn.CommandArgument).Rows[0];
            DataTable drDtl = GetData("SELECT B.BarangItemCode,B.NamaBarang,A.QtyBarangPOPembelianDtl,A.JumlahBoxPOPembelianDtl,A.HargaPOPembelianDtl,A.TotalHargaPOPembelianDtl FROM POPembelianDtl A INNER JOIN Barang B ON A.IdBarang = B.IdBarang WHERE A.IdPoPembelianMst = " + btn.CommandArgument);
            Document document = new Document(PageSize.A5.Rotate(), 1f, 1f, 1f, 1f);
            Font NormalFont = FontFactory.GetFont("Courier", 12, Font.NORMAL, BaseColor.BLACK);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                Phrase phrase = null;
                PdfPCell cell = null;
                PdfPTable table = null;
                BaseColor color = null;

                document.Open();

                //Header Table
                table = new PdfPTable(3);
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.SetWidths(new float[] { 1f, 1f, 1f });

                //Company Name and Address
                Chunk supName = new Chunk(drMst["SupplierName"].ToString() + "\n", FontFactory.GetFont("Courier", 21, Font.BOLD, BaseColor.BLACK));
                Chunk supAddr = new Chunk(drMst["SupplierAddress"].ToString() + "\n", FontFactory.GetFont("Courier", 12, Font.NORMAL, BaseColor.BLACK));
                Chunk supPhone = new Chunk(drMst["SupplierPhone"].ToString() + "\n", FontFactory.GetFont("Courier", 12, Font.NORMAL, BaseColor.BLACK));
                Chunk supEmail = new Chunk(drMst["SupplierEmail"].ToString() + "\n", FontFactory.GetFont("Courier", 12, Font.NORMAL, BaseColor.BLACK));
                phrase = new Phrase();
                phrase.Add(supName);
                phrase.Add(supAddr);
                phrase.Add(supPhone);
                phrase.Add(supEmail);
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER);
                cell.Colspan = 3;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                table.AddCell(cell);

                //Separater Line
                color = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#A9A9A9"));
                DrawLine(writer, 25f, document.Top - 79f, document.PageSize.Width - 25f, document.Top - 79f, color);
                DrawLine(writer, 25f, document.Top - 80f, document.PageSize.Width - 25f, document.Top - 80f, color);
                document.Add(table);

                table = new PdfPTable(1);
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.SetWidths(new float[] { 1f });
                table.SpacingBefore = 20f;

                //Judul Faktur
                cell = PhraseCell(new Phrase("Faktur Pembelian", FontFactory.GetFont("Courier", 16, Font.UNDERLINE, BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                //cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                //cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                ////Name
                //Chunk pmbInv = new Chunk(drMst["NoInvoicePembelian"].ToString() + "\n\n", FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK));
                //Chunk pmbTgl = new Chunk(drMst["TanggalPOPembelian"].ToString() + "\n", FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK));
                //Chunk pmbTglTmp = new Chunk(drMst["TanggalPOJatuhTmpPembelian"].ToString() + "\n", FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK));
                //Chunk pmbJenis = new Chunk(drMst["JenisTagihan"].ToString() + "\n", FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK));
                //Chunk pmbNoRek = new Chunk(drMst["NoRekeningPembayaran"].ToString() + "\n", FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK));
                //phrase = new Phrase();
                //phrase.Add(pmbInv);
                //phrase.Add(pmbTgl);
                //phrase.Add(pmbTglTmp);
                //phrase.Add(pmbJenis);
                //phrase.Add(pmbNoRek);
                //cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT);
                //cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                //table.AddCell(cell);
                //document.Add(table);

                //DrawLine(writer, 160f, 80f, 160f, 690f, BaseColor.BLACK);
                //DrawLine(writer, 115f, document.Top - 200f, document.PageSize.Width - 100f, document.Top - 200f, BaseColor.BLACK);

                table = new PdfPTable(2);
                table.SetWidths(new float[] { 1f, 1f });
                table.TotalWidth = 200f;
                table.LockedWidth = true;
                table.SpacingBefore = 15f;
                table.HorizontalAlignment = Element.ALIGN_RIGHT;

                //Employee Id
                table.AddCell(PhraseCell(new Phrase("No Invoice:", FontFactory.GetFont("Courier", 8, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(drMst["NoInvoicePembelian"].ToString(), FontFactory.GetFont("Courier", 8, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Tanggal Pembelian:", FontFactory.GetFont("Courier", 8, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(drMst["TanggalPOPembelian"].ToString(), FontFactory.GetFont("Courier", 8, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Jenis Tagihan:", FontFactory.GetFont("Courier", 8, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(drMst["JenisTagihan"].ToString(), FontFactory.GetFont("Courier", 8, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Tanggal Jatuh Tempo:", FontFactory.GetFont("Courier", 8, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(drMst["TanggalPOJatuhTmpPembelian"].ToString(), FontFactory.GetFont("Courier", 8, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(6);
                table.SetWidths(new float[] { 0.7f, 2.4f, 0.5f, 0.4f, 1f, 1f });
                table.TotalWidth = 300f;
                table.SpacingBefore = 15f;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                cell = new PdfPCell(new Phrase("Item Code", FontFactory.GetFont("Courier", 8, Font.BOLD, BaseColor.BLACK)));
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Nama Barang", FontFactory.GetFont("Courier", 8, Font.BOLD, BaseColor.BLACK)));
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Qty / Kg", FontFactory.GetFont("Courier", 8, Font.BOLD, BaseColor.BLACK)));
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Box", FontFactory.GetFont("Courier", 8, Font.BOLD, BaseColor.BLACK)));
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Harga / Kg", FontFactory.GetFont("Courier", 8, Font.BOLD, BaseColor.BLACK)));
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Total Harga", FontFactory.GetFont("Courier", 8, Font.BOLD, BaseColor.BLACK)));
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                for (int i = 0; i < drDtl.Rows.Count; i++)
                {
                    DataRow dtRow = drDtl.Rows[i];
                    cell = new PdfPCell(new Phrase(dtRow["BarangItemCode"].ToString(), FontFactory.GetFont("Courier", 8, Font.NORMAL, BaseColor.BLACK)));
                    cell.PaddingBottom = 10f;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtRow["NamaBarang"].ToString(), FontFactory.GetFont("Courier", 8, Font.NORMAL, BaseColor.BLACK)));
                    cell.PaddingBottom = 10f;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtRow["QtyBarangPOPembelianDtl"].ToString(), FontFactory.GetFont("Courier", 8, Font.NORMAL, BaseColor.BLACK)));
                    cell.PaddingBottom = 10f;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtRow["JumlahBoxPOPembelianDtl"].ToString(), FontFactory.GetFont("Courier", 8, Font.NORMAL, BaseColor.BLACK)));
                    cell.PaddingBottom = 10f;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", dtRow["HargaPOPembelianDtl"]), FontFactory.GetFont("Courier", 8, Font.NORMAL, BaseColor.BLACK)));
                    cell.PaddingBottom = 10f;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", dtRow["TotalHargaPOPembelianDtl"]), FontFactory.GetFont("Courier", 8, Font.NORMAL, BaseColor.BLACK)));
                    cell.PaddingBottom = 10f;
                    table.AddCell(cell);
                }
                document.Add(table);

                table = new PdfPTable(2);
                table.SetWidths(new float[] { 1f, 1f });
                table.TotalWidth = 280f;
                table.LockedWidth = true;
                table.SpacingBefore = 15f;
                table.HorizontalAlignment = Element.ALIGN_RIGHT;

                table.AddCell(PhraseCell(new Phrase("Total Harga:", FontFactory.GetFont("Courier", 10, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT));
                table.AddCell(PhraseCell(new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", drMst["TotalTagihanPembelian"]), FontFactory.GetFont("Courier", 10, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(1);
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.SetWidths(new float[] { 1f });
                table.SpacingBefore = 15f;

                //Judul Faktur
                cell = PhraseCell(new Phrase(drMst["NoRekeningPembayaran"].ToString(), FontFactory.GetFont("Courier", 10, Font.UNDERLINE, BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                //cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                //cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                //document.Add(table);

                //Separater Line
                color = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#A9A9A9"));
                DrawLine(writer, 25f, document.Top - 329f, document.PageSize.Width - 25f, document.Top - 329f, color);
                DrawLine(writer, 25f, document.Top - 330f, document.PageSize.Width - 25f, document.Top - 330f, color);

                //table = new PdfPTable(1);
                //table.HorizontalAlignment = Element.ALIGN_LEFT;
                //table.SetWidths(new float[] { 1f });
                //table.SpacingBefore = 5f;
                //Chunk pnrmHead = new Chunk("Penerima\n\n", FontFactory.GetFont("Courier", 8, Font.BOLD, BaseColor.BLACK));
                //Chunk pnrmFoot = new Chunk("(.......................)", FontFactory.GetFont("Courier", 8, Font.NORMAL, BaseColor.BLACK));
                //phrase = new Phrase();
                //phrase.Add(pnrmHead);
                //phrase.Add(pnrmFoot);
                cell = PhraseCell(new Phrase("    Penerima                          Supir                           Kepala Gudang                                       Hormat Kami\n\n\n\n\n", FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("(.......................)             (.......................)                 (.......................)                                     (.......................)", FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                table.AddCell(cell);
                document.Add(table);
                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + drMst["NoInvoicePembelian"].ToString());
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
                Response.Close();
            }
        }

        protected void RepeaterListPembelian_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Role r = new Role();
            r = (Role)Session["Role"];
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (r.IdRole == 2)
                {
                    //HtmlTableCell tdValue1 = (HtmlTableCell)e.Item.FindControl("tdVal1");
                    HtmlTableCell tdValue2 = (HtmlTableCell)e.Item.FindControl("tdVal2");
                    HtmlTableCell tdValue3 = (HtmlTableCell)e.Item.FindControl("tdVal3");
                    //tdValue1.Visible = false;
                    tdValue2.Visible = false;
                    tdValue3.Visible = false;
                }
                else
                {
                    HtmlTableCell tdValue3 = (HtmlTableCell)e.Item.FindControl("tdVal3");
                    HtmlTableCell tdValue2 = (HtmlTableCell)e.Item.FindControl("pmbStatus");
                    if (tdValue2.InnerText.Trim() == "RETUR")
                    {
                        tdValue3.Visible = false;
                    }
                }
            }
        }
    }
}