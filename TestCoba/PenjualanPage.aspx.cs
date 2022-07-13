using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;

namespace TestCoba
{
    public partial class PenjualanPage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
        private void BindRepeator(string tanggalBeli, string bulan, string tahun)
        {
            string queryFilParam = "SELECT * FROM POPenjualanMst a INNER JOIN Inventory c on a.IdInventory = c.IdInventory INNER JOIN Customer d on a.IdCustomer = d.IdCustomer WHERE SUBSTRING(TanggalPOPenjualan,1,2) = '" + tanggalBeli + "' AND SUBSTRING(TanggalPOPenjualan,4,2) = '" + bulan + "' AND SUBSTRING(TanggalPOPenjualan,7,4) = '" + tahun + "' ";
            string queryJmlPenj = "SELECT COALESCE(SUM(a.TotalTagihanPenjualan),0) as TotalPenjualan FROM POPenjualanMst a INNER JOIN Inventory c on a.IdInventory = c.IdInventory INNER JOIN Customer d on a.IdCustomer = d.IdCustomer WHERE SUBSTRING(TanggalPOPenjualan,1,2) = '" + tanggalBeli + "' AND SUBSTRING(TanggalPOPenjualan,4,2) = '" + bulan + "' AND SUBSTRING(TanggalPOPenjualan,7,4) = '" + tahun + "' ";
            string queryJmlQty = "SELECT COALESCE(SUM(B.QtyBarangPOPenjualanDtl),0) as TotalQTY FROM POPenjualanMst A INNER JOIN POPenjualanDtl B on A.IdPOPenjualanMst = B.IdPOPenjualanMst WHERE SUBSTRING(TanggalPOPenjualan,1,2) = '" + tanggalBeli + "' AND SUBSTRING(TanggalPOPenjualan,4,2) = '" + bulan + "' AND SUBSTRING(TanggalPOPenjualan,7,4) = '" + tahun + "' ";
            string queryJmlBox = "SELECT COALESCE(SUM(B.JumlahBoxPOPenjualanDtl),0) as TotalBox FROM POPenjualanMst A INNER JOIN POPenjualanDtl B on A.IdPOPenjualanMst = B.IdPOPenjualanMst WHERE SUBSTRING(TanggalPOPenjualan,1,2) = '" + tanggalBeli + "' AND SUBSTRING(TanggalPOPenjualan,4,2) = '" + bulan + "' AND SUBSTRING(TanggalPOPenjualan,7,4) = '" + tahun + "' ";
            string queryJmlPenjRtr = "SELECT COALESCE(SUM(a.TotalHargaPOPenjualanReturDtl),0) as TotalPenjualanRetur FROM POPenjualanReturDtl a INNER JOIN POPenjualanMst b on a.IdPOPenjualanMst = b.IdPOPenjualanMst WHERE SUBSTRING(b.TanggalPOPenjualan,1,2) = '" + tanggalBeli + "' AND SUBSTRING(b.TanggalPOPenjualan,4,2) = '" + bulan + "' AND SUBSTRING(b.TanggalPOPenjualan,7,4) = '" + tahun + "' ";
            string queryJmlQtyRtr = "SELECT COALESCE(SUM(a.QtyBarangPOPenjualanReturDtl),0) as TotalQTYRetur FROM POPenjualanReturDtl a INNER JOIN POPenjualanMst b on a.IdPOPenjualanMst = b.IdPOPenjualanMst WHERE SUBSTRING(b.TanggalPOPenjualan,1,2) = '" + tanggalBeli + "' AND SUBSTRING(b.TanggalPOPenjualan,4,2) = '" + bulan + "' AND SUBSTRING(b.TanggalPOPenjualan,7,4) = '" + tahun + "' ";
            string queryJmlBoxRtr = "SELECT COALESCE(SUM(a.JumlahBoxPOPenjualanReturDtl),0) as TotalBoxRtr FROM POPenjualanReturDtl a INNER JOIN POPenjualanMst b on a.IdPOPenjualanMst = b.IdPOPenjualanMst WHERE SUBSTRING(b.TanggalPOPenjualan,1,2) = '" + tanggalBeli + "' AND SUBSTRING(b.TanggalPOPenjualan,4,2) = '" + bulan + "' AND SUBSTRING(b.TanggalPOPenjualan,7,4) = '" + tahun + "' ";

            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                if (ddlStatusInvoice.SelectedValue.Trim() != "" && ddlSortBy.SelectedValue.Trim() != "")
                {
                    decimal GTotPnj = 0;
                    decimal GTotQty = 0;
                    int GTotBox = 0;
                    string queryParam = "AND a.StatusInvoicePenjualan = '" + ddlStatusInvoice.SelectedValue + "'";
                    string queryOrderPrm = " ORDER BY " + ddlSortBy.SelectedValue.Trim() + "";
                    using (SqlCommand StrQuer = new SqlCommand(queryFilParam + queryParam + queryOrderPrm, sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        RepeaterListPembelian.DataSource = StrQuer.ExecuteReader();
                        RepeaterListPembelian.DataBind();
                    }
                    using (SqlCommand StrQuer = new SqlCommand(queryJmlPenj + queryParam, sqlConnection))
                    {
                        using (SqlDataReader drTotPnj = StrQuer.ExecuteReader())
                        {
                            if (drTotPnj.Read())
                            {
                                GTotPnj = (decimal)drTotPnj["TotalPenjualan"];
                                using (SqlCommand StrQuerRtr = new SqlCommand(queryJmlPenjRtr, sqlConnection))
                                {
                                    using (SqlDataReader drTotPnjRtr = StrQuerRtr.ExecuteReader())
                                    {
                                        if (drTotPnjRtr.Read())
                                        {
                                            var GTotPnjRtr = (decimal)drTotPnjRtr["TotalPenjualanRetur"];
                                            GTPenjualan.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPnj - GTotPnjRtr);
                                        }
                                        else
                                        {
                                            GTPenjualan.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPnj);
                                        }
                                    }
                                }
                                //GTPenjualan.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPnj);
                            }
                            else
                            {
                                GTPenjualan.Text = "0";
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
                                    using (SqlDataReader drTotPnjRtr = StrQuerRtr.ExecuteReader())
                                    {
                                        if (drTotPnjRtr.Read())
                                        {
                                            var GTotPnjRtr = (decimal)drTotPnjRtr["TotalQTYRetur"];
                                            GTQty.Text = string.Format("{0:n}", GTotQty - GTotPnjRtr) + " KG";
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
                    using (SqlCommand StrQuer = new SqlCommand(queryJmlBox + queryParam, sqlConnection))
                    {
                        using (SqlDataReader drTotBox = StrQuer.ExecuteReader())
                        {
                            if (drTotBox.Read())
                            {
                                GTotBox = (int)drTotBox["TotalBox"];
                                using (SqlCommand StrQuerRtr = new SqlCommand(queryJmlBoxRtr, sqlConnection))
                                {
                                    using (SqlDataReader drTotPnjRtr = StrQuerRtr.ExecuteReader())
                                    {
                                        if (drTotPnjRtr.Read())
                                        {
                                            var GTotPnjRtr = (int)drTotPnjRtr["TotalBoxRtr"];
                                            GTBox.Text = (GTotBox - GTotPnjRtr).ToString() + " Box";
                                        }
                                        else
                                        {
                                            GTBox.Text = GTotBox.ToString() + " Box";
                                        }
                                    }
                                }
                                //GTBox.Text = GTotBox.ToString() + " Box";
                            }
                            else
                            {
                                GTBox.Text = "0";
                            }
                        }
                    }
                }
                else
                {
                    decimal GTotPnj = 0;
                    decimal GTotQty = 0;
                    int GTotBox = 0;
                    string queryParam = "";
                    string queryOrderPrm = " ORDER BY a.NoInvoicePenjualan";
                    if (ddlStatusInvoice.SelectedValue.Trim() != "")
                    {
                        queryParam += " AND a.StatusInvoicePenjualan = '" + ddlStatusInvoice.SelectedValue + "'";
                    }
                    if (ddlSortBy.SelectedValue.Trim() != "")
                    {
                        queryOrderPrm = " ORDER BY " + ddlSortBy.SelectedValue.Trim() + "";
                    }
                    string finalFill = queryFilParam + queryParam + queryOrderPrm;
                    using (SqlCommand StrQuer = new SqlCommand(finalFill, sqlConnection))
                    {
                        //StrQuer.CommandType = CommandType.StoredProcedure;
                        RepeaterListPembelian.DataSource = StrQuer.ExecuteReader();
                        RepeaterListPembelian.DataBind();
                    }
                    using (SqlCommand StrQuer = new SqlCommand(queryJmlPenj + queryParam, sqlConnection))
                    {
                        using (SqlDataReader drTotPnj = StrQuer.ExecuteReader())
                        {
                            if (drTotPnj.Read())
                            {
                                GTotPnj = (decimal)drTotPnj["TotalPenjualan"];
                                using (SqlCommand StrQuerRtr = new SqlCommand(queryJmlPenjRtr, sqlConnection))
                                {
                                    using (SqlDataReader drTotPnjRtr = StrQuerRtr.ExecuteReader())
                                    {
                                        if (drTotPnjRtr.Read())
                                        {
                                            var GTotPnjRtr = (decimal)drTotPnjRtr["TotalPenjualanRetur"];
                                            GTPenjualan.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPnj - GTotPnjRtr);
                                        }
                                        else
                                        {
                                            GTPenjualan.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPnj);
                                        }
                                    }
                                }
                                //GTPenjualan.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPnj);
                            }
                            else
                            {
                                GTPenjualan.Text = "0";
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
                                    using (SqlDataReader drTotPnjRtr = StrQuerRtr.ExecuteReader())
                                    {
                                        if (drTotPnjRtr.Read())
                                        {
                                            var GTotPnjRtr = (decimal)drTotPnjRtr["TotalQTYRetur"];
                                            GTQty.Text = string.Format("{0:n}", GTotQty - GTotPnjRtr) + " KG";
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
                    using (SqlCommand StrQuer = new SqlCommand(queryJmlBox + queryParam, sqlConnection))
                    {
                        using (SqlDataReader drTotBox = StrQuer.ExecuteReader())
                        {
                            if (drTotBox.Read())
                            {
                                GTotBox = (int)drTotBox["TotalBox"];
                                using (SqlCommand StrQuerRtr = new SqlCommand(queryJmlBoxRtr, sqlConnection))
                                {
                                    using (SqlDataReader drTotPnjRtr = StrQuerRtr.ExecuteReader())
                                    {
                                        if (drTotPnjRtr.Read())
                                        {
                                            var GTotPnjRtr = (int)drTotPnjRtr["TotalBoxRtr"];
                                            GTBox.Text = (GTotBox - GTotPnjRtr).ToString() + " Box";
                                        }
                                        else
                                        {
                                            GTBox.Text = GTotBox.ToString() + " Box";
                                        }
                                    }
                                }
                                //GTBox.Text = GTotBox.ToString() + " Box";
                            }
                            else
                            {
                                GTBox.Text = "0";
                            }
                        }
                    }
                }
                txtSearch.Enabled = true;
                btnRkpBulan.Enabled = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        public class LineaBottom : IPdfPTableEvent
        {


            #region IPdfPTableEvent Members

            void IPdfPTableEvent.TableLayout(PdfPTable table, float[][] widths, float[] heights, int headerRows, int rowStart, PdfContentByte[] canvases)
            {
                int columns;
                Rectangle rect;
                int footer = widths.Length - table.FooterRows;
                int header = table.HeaderRows - table.FooterRows + 1;
                int ultima = footer - 1;
                if (ultima != -1)
                {
                    columns = widths[ultima].Length - 1;
                    rect = new Rectangle(widths[ultima][0], heights[ultima], widths[footer - 1][columns], heights[ultima + 1]);
                    rect.BorderColor = BaseColor.BLACK;
                    rect.BorderWidth = 1;
                    rect.Border = Rectangle.TOP_BORDER;
                    canvases[PdfPTable.BASECANVAS].Rectangle(rect);
                }
            }

            #endregion
        }

        protected void btnCetakGnrt_Click(object sender, EventArgs e)
        {
            Button btn = (Button)(sender);
            DataRow drMst = GetData("SELECT * FROM POPenjualanMst a INNER JOIN Customer b on a.IdCustomer = b.IdCustomer WHERE IdPOPenjualanMst = " + btn.CommandArgument).Rows[0];
            DataRow drTot = GetData("; with table1 as (SELECT b.IdPOPenjualanMst,b.IdBarang,b.QtyBarangPOPenjualanReturDtl,b.HargaPOPenjualanReturDtl,b.JumlahBoxPOPenjualanReturDtl,b.TotalHargaPOPenjualanReturDtl FROM POPenjualanDtl a LEFT JOIN POPenjualanReturDtl b on a.IdBarang = b.IdBarang WHERE b.IdPOPenjualanMst = '" + btn.CommandArgument + "' and b.IdPOPenjualanMst IS NOT NULL group by b.IdPOPenjualanMst,b.IdBarang,b.QtyBarangPOPenjualanReturDtl,b.HargaPOPenjualanReturDtl,b.JumlahBoxPOPenjualanReturDtl,b.TotalHargaPOPenjualanReturDtl), table2 as (select * from POPenjualanDtl where IdPOPenjualanMst = '" + btn.CommandArgument + "') select SUM(a.TotalHargaPOPenjualanDtl-COALESCE(b.TotalHargaPOPenjualanReturDtl,0)) as TotalHargaPOPenjualanDtl from table2 a left join table1 b on a.IdBarang = b.IdBarang INNER JOIN Barang c on a.IdBarang = c.IdBarang WHERE a.QtyBarangPOPenjualanDtl-COALESCE(b.QtyBarangPOPenjualanReturDtl,0) != 0").Rows[0];
            DataTable drDtl = new DataTable();
            decimal totalRetur = 0;
            if (drMst["StatusInvoicePenjualan"].ToString().Trim() != "RETUR")
            {
                drDtl = GetData("SELECT B.BarangItemCode,B.NamaBarang,A.QtyBarangPOPenjualanDtl,A.JumlahBoxPOPenjualanDtl,A.HargaPOPenjualanDtl,A.TotalHargaPOPenjualanDtl FROM POPenjualanDtl A INNER JOIN Barang B ON A.IdBarang = B.IdBarang WHERE A.IdPOPenjualanMst = " + btn.CommandArgument);
                string fileName = string.Empty;
                Regex regex = new Regex(@"[<>:""/\\|?*]"); //.Replace("Inpu|t", "-");
                DateTime fileCreationDatetime = DateTime.Now;
                fileName = string.Format("{0}.pdf", "Penjualan_" + fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + Server.UrlEncode(drMst["NoInvoicePenjualan"].ToString()));
                string pdfPath = Server.MapPath(@"~\PDFs\") + fileName;
                var pgSize = new iTextSharp.text.Rectangle(595.276f, 396.85f);
                using (FileStream msReport = new FileStream(pdfPath, FileMode.Create))
                {
                    using (Document pdfDoc = new Document(PageSize.A5.Rotate()))
                    {
                        try
                        {
                            // step 2
                            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                            pdfWriter.PageEvent = new TestCoba.ITextEventsPnj() { parametro = Convert.ToInt32(btn.CommandArgument) };

                            //open the stream 
                            pdfDoc.Open();
                            int halaman = 0;
                            int temp = 0;
                            int nomer = 1;
                            float dataCount = 269;
                            PdfContentByte canvas = pdfWriter.DirectContent;
                            PdfPCell cell = null;
                            PdfPTable table = null;
                            PdfPTable tableHeader = null;
                            PdfPCell cellHeader = null;

                            for (int i = 0; i <= halaman; i++)
                            {
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Faktur Penjualan", FontFactory.GetFont("Courier", 18, Font.BOLD, BaseColor.BLACK)), 210, 335, 0);

                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase("Kepada", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 40, 316, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 150, 316, 0);

                                //ColumnText.ShowTextAligned(canvas, Element.RECTANGLE, new Phrase(drMst["CustomerName"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 160, 321, 0);

                                pdfDoc.Add(new Paragraph(" "));
                                tableHeader = new PdfPTable(1);
                                tableHeader.WidthPercentage = 49;
                                tableHeader.SpacingBefore = 36f;
                                tableHeader.SetWidths(new float[] { 1f });
                                tableHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellHeader = new PdfPCell(new Phrase(drMst["CustomerName"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)));
                                cellHeader.Border = 0;
                                cellHeader.PaddingLeft = 125f;
                                cellHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                                tableHeader.AddCell(cellHeader);
                                pdfDoc.Add(tableHeader);


                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase("No Faktur", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 40, 294, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 150, 294, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(drMst["NoInvoicePenjualan"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 160, 294, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase("Alamat", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 40, 282, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 150, 282, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(drMst["CustomerAddress"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 160, 282, 0);

                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase("Tanggal", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 375, 306, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 485, 306, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(drMst["TanggalPOPenjualan"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 495, 306, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase("Jns Pembayaran", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 375, 294, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 485, 294, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(drMst["JenisTagihan"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 495, 294, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase("Jatuh Tempo", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 375, 282, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 485, 282, 0);
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(drMst["TanggalPOJatuhTmpPenjualan"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 495, 282, 0);
                                pdfDoc.Add(new Paragraph(" "));
                                table = new PdfPTable(6);
                                table.TableEvent = new LineaBottom();
                                table.WidthPercentage = 100;
                                table.SetWidths(new float[] { 0.3f, 2.6f, 1.2f, 0.5f, 1.5f, 2f });
                                //table.TotalWidth = 400f;
                                if (tableHeader.TotalHeight <= 15)
                                {
                                    table.SpacingBefore = 20f;
                                }
                                if (tableHeader.TotalHeight > 15)
                                {
                                    table.SpacingBefore = 10f;
                                }
                                table.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell = new PdfPCell(new Phrase("No", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                                cell.PaddingBottom = 10f;
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                table.AddCell(cell);
                                cell = new PdfPCell(new Phrase("Nama Barang", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                                cell.PaddingBottom = 10f;
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                table.AddCell(cell);
                                cell = new PdfPCell(new Phrase("Jumlah Per Kg", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                                cell.PaddingBottom = 10f;
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                table.AddCell(cell);
                                cell = new PdfPCell(new Phrase("Box", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                                cell.PaddingBottom = 10f;
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                table.AddCell(cell);
                                cell = new PdfPCell(new Phrase("Harga per Kg", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
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
                                        cell.Border = 0;
                                        cell.BorderColorLeft = BaseColor.BLACK;
                                        cell.BorderWidthLeft = .5f;
                                        cell.BorderColorRight = BaseColor.BLACK;
                                        cell.BorderWidthRight = .5f;
                                        table.AddCell(cell);
                                        cell = new PdfPCell(new Phrase(dtRow["NamaBarang"].ToString(), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.PaddingBottom = 10f;
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = 0;
                                        cell.BorderColorLeft = BaseColor.BLACK;
                                        cell.BorderWidthLeft = .5f;
                                        cell.BorderColorRight = BaseColor.BLACK;
                                        cell.BorderWidthRight = .5f;
                                        table.AddCell(cell);
                                        cell = new PdfPCell(new Phrase(dtRow["QtyBarangPOPenjualanDtl"].ToString() + " Kg", FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.PaddingBottom = 10f;
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell.Border = 0;
                                        cell.BorderColorLeft = BaseColor.BLACK;
                                        cell.BorderWidthLeft = .5f;
                                        cell.BorderColorRight = BaseColor.BLACK;
                                        cell.BorderWidthRight = .5f;
                                        table.AddCell(cell);
                                        cell = new PdfPCell(new Phrase(dtRow["JumlahBoxPOPenjualanDtl"].ToString(), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.PaddingBottom = 10f;
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell.Border = 0;
                                        cell.BorderColorLeft = BaseColor.BLACK;
                                        cell.BorderWidthLeft = .5f;
                                        cell.BorderColorRight = BaseColor.BLACK;
                                        cell.BorderWidthRight = .5f;
                                        table.AddCell(cell);
                                        cell = new PdfPCell(new Phrase(string.Format("{0:Rp ###,###,##0.00;(Rp ###,###,##0.00);'Rp 0'}", dtRow["HargaPOPenjualanDtl"]), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.PaddingBottom = 10f;
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell.Border = 0;
                                        cell.BorderColorLeft = BaseColor.BLACK;
                                        cell.BorderWidthLeft = .5f;
                                        cell.BorderColorRight = BaseColor.BLACK;
                                        cell.BorderWidthRight = .5f;
                                        table.AddCell(cell);
                                        cell = new PdfPCell(new Phrase(string.Format("{0:Rp ###,###,##0.00;(Rp ###,###,##0.00);'Rp 0'}", dtRow["TotalHargaPOPenjualanDtl"]), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.PaddingBottom = 10f;
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell.Border = 0;
                                        cell.BorderColorLeft = BaseColor.BLACK;
                                        cell.BorderWidthLeft = .5f;
                                        cell.BorderColorRight = BaseColor.BLACK;
                                        cell.BorderWidthRight = .5f;
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
                                ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(drMst["NoRekeningPembayaran"].ToString() + " / Luluk Sularsi", FontFactory.GetFont("Courier", 10, Font.UNDERLINE, BaseColor.BLACK)), 40, 130, 0);
                                if (temp != drDtl.Rows.Count)
                                {
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Note : ", FontFactory.GetFont("Courier", 10, Font.UNDERLINE, BaseColor.BLACK)), 40, 145, 0);
                                    pdfDoc.NewPage();
                                }
                                else
                                {
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Note : ", FontFactory.GetFont("Courier", 10, Font.UNDERLINE, BaseColor.BLACK)), 40, dataCount - 45, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Total", FontFactory.GetFont("Courier", 10, Font.BOLD, BaseColor.BLACK)), 375, dataCount - 45, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(":", FontFactory.GetFont("Courier", 10, Font.BOLD, BaseColor.BLACK)), 445, dataCount - 45, 0);
                                    if (drMst["StatusInvoicePenjualan"].ToString().Trim() != "RETUR")
                                    {
                                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", drMst["TotalTagihanPenjualan"]), FontFactory.GetFont("Courier", 10, Font.BOLD, BaseColor.BLACK)), 460, dataCount - 45, 0);
                                    }
                                    else
                                    {
                                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", totalRetur), FontFactory.GetFont("Courier", 10, Font.BOLD, BaseColor.BLACK)), 460, dataCount - 45, 0);
                                    }
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
            else
            {
                drDtl = GetData("; with table1 as (SELECT b.IdPOPenjualanMst,b.IdBarang,b.QtyBarangPOPenjualanReturDtl,b.HargaPOPenjualanReturDtl,b.JumlahBoxPOPenjualanReturDtl,b.TotalHargaPOPenjualanReturDtl FROM POPenjualanDtl a LEFT JOIN POPenjualanReturDtl b on a.IdBarang = b.IdBarang WHERE b.IdPOPenjualanMst = '"+ btn.CommandArgument + "' and b.IdPOPenjualanMst IS NOT NULL group by b.IdPOPenjualanMst,b.IdBarang,b.QtyBarangPOPenjualanReturDtl,b.HargaPOPenjualanReturDtl,b.JumlahBoxPOPenjualanReturDtl,b.TotalHargaPOPenjualanReturDtl), table2 as (select * from POPenjualanDtl where IdPOPenjualanMst = '"+ btn.CommandArgument + "') select c.BarangItemCode,a.IdBarang,c.NamaBarang,a.QtyBarangPOPenjualanDtl-COALESCE(b.QtyBarangPOPenjualanReturDtl,0) as QtyBarangPOPenjualanDtl,a.HargaPOPenjualanDtl,a.JumlahBoxPOPenjualanDtl-COALESCE(b.JumlahBoxPOPenjualanReturDtl,0) as JumlahBoxPOPenjualanDtl,a.TotalHargaPOPenjualanDtl-COALESCE(b.TotalHargaPOPenjualanReturDtl,0) as TotalHargaPOPenjualanDtl from table2 a left join table1 b on a.IdBarang = b.IdBarang INNER JOIN Barang c on a.IdBarang = c.IdBarang WHERE a.QtyBarangPOPenjualanDtl-COALESCE(b.QtyBarangPOPenjualanReturDtl,0) != 0  GROUP BY c.BarangItemCode,a.IdBarang,c.NamaBarang,a.QtyBarangPOPenjualanDtl,b.QtyBarangPOPenjualanReturDtl,a.HargaPOPenjualanDtl,b.HargaPOPenjualanReturDtl,a.JumlahBoxPOPenjualanDtl,b.JumlahBoxPOPenjualanReturDtl,a.TotalHargaPOPenjualanDtl,b.TotalHargaPOPenjualanReturDtl");
                if (drDtl.Rows.Count > 0)
                {
                    totalRetur = (decimal)drTot["TotalHargaPOPenjualanDtl"];
                    string fileName = string.Empty;
                    Regex regex = new Regex(@"[<>:""/\\|?*]"); //.Replace("Inpu|t", "-");
                    DateTime fileCreationDatetime = DateTime.Now;
                    fileName = string.Format("{0}.pdf", "Penjualan_" + fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + Server.UrlEncode(drMst["NoInvoicePenjualan"].ToString()));
                    string pdfPath = Server.MapPath(@"~\PDFs\") + fileName;
                    var pgSize = new iTextSharp.text.Rectangle(595.276f, 396.85f);
                    using (FileStream msReport = new FileStream(pdfPath, FileMode.Create))
                    {
                        using (Document pdfDoc = new Document(PageSize.A5.Rotate()))
                        {
                            try
                            {
                                // step 2
                                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                                pdfWriter.PageEvent = new TestCoba.ITextEventsPnj() { parametro = Convert.ToInt32(btn.CommandArgument) };

                                //open the stream 
                                pdfDoc.Open();
                                int halaman = 0;
                                int temp = 0;
                                int nomer = 1;
                                float dataCount = 269;
                                PdfContentByte canvas = pdfWriter.DirectContent;
                                PdfPCell cell = null;
                                PdfPTable table = null;
                                PdfPTable tableHeader = null;
                                PdfPCell cellHeader = null;

                                for (int i = 0; i <= halaman; i++)
                                {
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Faktur Penjualan", FontFactory.GetFont("Courier", 18, Font.BOLD, BaseColor.BLACK)), 210, 335, 0);

                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase("Kepada", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 40, 316, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 150, 316, 0);

                                    //ColumnText.ShowTextAligned(canvas, Element.RECTANGLE, new Phrase(drMst["CustomerName"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 160, 321, 0);

                                    pdfDoc.Add(new Paragraph(" "));
                                    tableHeader = new PdfPTable(1);
                                    tableHeader.WidthPercentage = 49;
                                    tableHeader.SpacingBefore = 36f;
                                    tableHeader.SetWidths(new float[] { 1f });
                                    tableHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cellHeader = new PdfPCell(new Phrase(drMst["CustomerName"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)));
                                    cellHeader.Border = 0;
                                    cellHeader.PaddingLeft = 125f;
                                    cellHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                                    tableHeader.AddCell(cellHeader);
                                    pdfDoc.Add(tableHeader);


                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase("No Faktur", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 40, 294, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 150, 294, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(drMst["NoInvoicePenjualan"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 160, 294, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase("Alamat", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 40, 282, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 150, 282, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(drMst["CustomerAddress"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 160, 282, 0);

                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase("Tanggal", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 375, 306, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 485, 306, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(drMst["TanggalPOPenjualan"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 495, 306, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase("Jns Pembayaran", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 375, 294, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 485, 294, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(drMst["JenisTagihan"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 495, 294, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase("Jatuh Tempo", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 375, 282, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(":", FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 485, 282, 0);
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_JUSTIFIED, new Phrase(drMst["TanggalPOJatuhTmpPenjualan"].ToString(), FontFactory.GetFont("Courier", 11, BaseColor.BLACK)), 495, 282, 0);
                                    pdfDoc.Add(new Paragraph(" "));
                                    table = new PdfPTable(6);
                                    table.TableEvent = new LineaBottom();
                                    table.WidthPercentage = 100;
                                    table.SetWidths(new float[] { 0.3f, 2.6f, 1.2f, 0.5f, 1.5f, 2f });
                                    //table.TotalWidth = 400f;
                                    if (tableHeader.TotalHeight <= 15)
                                    {
                                        table.SpacingBefore = 20f;
                                    }
                                    if (tableHeader.TotalHeight > 15)
                                    {
                                        table.SpacingBefore = 10f;
                                    }
                                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                                    cell = new PdfPCell(new Phrase("No", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase("Nama Barang", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase("Jumlah Per Kg", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase("Box", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase("Harga per Kg", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
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
                                            cell.Border = 0;
                                            cell.BorderColorLeft = BaseColor.BLACK;
                                            cell.BorderWidthLeft = .5f;
                                            cell.BorderColorRight = BaseColor.BLACK;
                                            cell.BorderWidthRight = .5f;
                                            table.AddCell(cell);
                                            cell = new PdfPCell(new Phrase(dtRow["NamaBarang"].ToString(), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                            cell.PaddingBottom = 10f;
                                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                            cell.Border = 0;
                                            cell.BorderColorLeft = BaseColor.BLACK;
                                            cell.BorderWidthLeft = .5f;
                                            cell.BorderColorRight = BaseColor.BLACK;
                                            cell.BorderWidthRight = .5f;
                                            table.AddCell(cell);
                                            cell = new PdfPCell(new Phrase(dtRow["QtyBarangPOPenjualanDtl"].ToString() + " Kg", FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                            cell.PaddingBottom = 10f;
                                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            cell.Border = 0;
                                            cell.BorderColorLeft = BaseColor.BLACK;
                                            cell.BorderWidthLeft = .5f;
                                            cell.BorderColorRight = BaseColor.BLACK;
                                            cell.BorderWidthRight = .5f;
                                            table.AddCell(cell);
                                            cell = new PdfPCell(new Phrase(dtRow["JumlahBoxPOPenjualanDtl"].ToString(), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                            cell.PaddingBottom = 10f;
                                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            cell.Border = 0;
                                            cell.BorderColorLeft = BaseColor.BLACK;
                                            cell.BorderWidthLeft = .5f;
                                            cell.BorderColorRight = BaseColor.BLACK;
                                            cell.BorderWidthRight = .5f;
                                            table.AddCell(cell);
                                            cell = new PdfPCell(new Phrase(string.Format("{0:Rp ###,###,##0.00;(Rp ###,###,##0.00);'Rp 0'}", dtRow["HargaPOPenjualanDtl"]), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                            cell.PaddingBottom = 10f;
                                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            cell.Border = 0;
                                            cell.BorderColorLeft = BaseColor.BLACK;
                                            cell.BorderWidthLeft = .5f;
                                            cell.BorderColorRight = BaseColor.BLACK;
                                            cell.BorderWidthRight = .5f;
                                            table.AddCell(cell);
                                            cell = new PdfPCell(new Phrase(string.Format("{0:Rp ###,###,##0.00;(Rp ###,###,##0.00);'Rp 0'}", dtRow["TotalHargaPOPenjualanDtl"]), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                            cell.PaddingBottom = 10f;
                                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            cell.Border = 0;
                                            cell.BorderColorLeft = BaseColor.BLACK;
                                            cell.BorderWidthLeft = .5f;
                                            cell.BorderColorRight = BaseColor.BLACK;
                                            cell.BorderWidthRight = .5f;
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
                                    ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(drMst["NoRekeningPembayaran"].ToString() + " / Luluk Sularsi", FontFactory.GetFont("Courier", 10, Font.UNDERLINE, BaseColor.BLACK)), 40, 130, 0);
                                    if (temp != drDtl.Rows.Count)
                                    {
                                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Note : ", FontFactory.GetFont("Courier", 10, Font.UNDERLINE, BaseColor.BLACK)), 40, 145, 0);
                                        pdfDoc.NewPage();
                                    }
                                    else
                                    {
                                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Note : ", FontFactory.GetFont("Courier", 10, Font.UNDERLINE, BaseColor.BLACK)), 40, dataCount - 45, 0);
                                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Total", FontFactory.GetFont("Courier", 10, Font.BOLD, BaseColor.BLACK)), 375, dataCount - 45, 0);
                                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(":", FontFactory.GetFont("Courier", 10, Font.BOLD, BaseColor.BLACK)), 445, dataCount - 45, 0);
                                        if (drMst["StatusInvoicePenjualan"].ToString().Trim() != "RETUR")
                                        {
                                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", drMst["TotalTagihanPenjualan"]), FontFactory.GetFont("Courier", 10, Font.BOLD, BaseColor.BLACK)), 460, dataCount - 45, 0);
                                        }
                                        else
                                        {
                                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase(string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", totalRetur), FontFactory.GetFont("Courier", 10, Font.BOLD, BaseColor.BLACK)), 460, dataCount - 45, 0);
                                        }
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
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Barang Kosong Tidak Boleh Dicetak!" + "');", true);
                }
            }
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

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            BindRepeator(ddlTanggal.SelectedValue.Trim(), ddlMonths.SelectedValue.Trim(), ddlYears.SelectedValue.Trim());
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
                    HtmlTableCell tdValue2 = (HtmlTableCell)e.Item.FindControl("pnjStatus");
                    HtmlTableCell tdValueCicil = (HtmlTableCell)e.Item.FindControl("tdValCicilan");
                    HtmlTableCell tdValueTagihan = (HtmlTableCell)e.Item.FindControl("pnjTagihan");
                    HtmlTableCell tdValueID = (HtmlTableCell)e.Item.FindControl("tdIdVal");
                    if (tdValue2.InnerText.Trim() == "RETUR")
                    {
                        tdValue3.Visible = false;
                    }
                    if (tdValueTagihan.InnerText.Trim() == "CICILAN")
                    {
                        if(checkSisaPembayaran(tdValueID.InnerText.Trim()))
                        {
                            tdValueCicil.Visible = false;
                        }
                        else
                        {
                            tdValueCicil.Visible = true;
                        }
                    }
                    else
                    {
                        tdValueCicil.Visible = false;
                    }
                }
            }
        }

        protected bool checkSisaPembayaran(string id)
        {
            bool checkSisa = false;
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                string queryJmlCicilan = "SELECT COALESCE(SUM(NilaiPembayaran),0) as TotalPembayaran FROM Cicilan WHERE IdPOPenjualanMst = '" + id + "'";
                using (SqlCommand StrQuer = new SqlCommand(queryJmlCicilan, sqlConnection))
                {
                    using (SqlDataReader drTotPmb = StrQuer.ExecuteReader())
                    {
                        if (drTotPmb.Read())
                        {
                            var nilaiPembayaran = (decimal)drTotPmb["TotalPembayaran"];
                            if (getTotalTagihan(id) - nilaiPembayaran == 0)
                            {
                                checkSisa = true;
                                return checkSisa;
                            }
                            else
                            {
                                return checkSisa;
                            }
                        }
                        else
                        {
                            return checkSisa;
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
        protected decimal getTotalTagihan(string id)
        {
            decimal jmlTagihan = 0;
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                string queryJmlTagihan = "SELECT TotalTagihanPenjualan FROM POPenjualanMst WHERE IdPOPenjualanMst = '" + id + "'";
                using (SqlCommand StrQuer = new SqlCommand(queryJmlTagihan, sqlConnection))
                {
                    using (SqlDataReader drTotPmb = StrQuer.ExecuteReader())
                    {
                        if (drTotPmb.Read())
                        {
                            jmlTagihan = (decimal)drTotPmb["TotalTagihanPenjualan"];
                            return jmlTagihan;
                        }
                        else
                        {
                            return jmlTagihan;
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

        protected void btnRkpBulan_Click(object sender, EventArgs e)
        {
            DataTable drDtl = new DataTable();
            drDtl = GetData("select b.IdPOPenjualanMst,c.CustomerName,sum(a.JumlahBoxPOPenjualanDtl) as jumlahbox, sum(a.QtyBarangPOPenjualanDtl) as jumlahqty, sum(a.TotalHargaPOPenjualanDtl) as jumlahpenj from POPenjualanDtl a inner join POPenjualanMst b on a.IdPOPenjualanMst = b.IdPOPenjualanMst INNER JOIN Customer c on c.IdCustomer = b.IdCustomer where SUBSTRING(TanggalPOPenjualan,4,2) = '"+ddlMonths.SelectedValue.Trim()+ "' AND SUBSTRING(TanggalPOPenjualan,7,4) = '" + ddlYears.SelectedValue.Trim() + "' group by b.IdPOPenjualanMst,c.CustomerName");
            string fileName = string.Empty;
            Regex regex = new Regex(@"[<>:""/\\|?*]"); //.Replace("Inpu|t", "-");
            DateTime fileCreationDatetime = DateTime.Now;
            fileName = string.Format("{0}.pdf", "Rekap_Bulanan_Penjualan_" + fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + Server.UrlEncode(ddlMonths.SelectedValue.Trim()));
            string pdfPath = Server.MapPath(@"~\PDFs\") + fileName;
            var pgSize = new iTextSharp.text.Rectangle(595.276f, 396.85f);
            using (FileStream msReport = new FileStream(pdfPath, FileMode.Create))
            {
                using (Document pdfDoc = new Document(PageSize.A5.Rotate()))
                {
                    try
                    {
                        connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                        sqlConnection = new SqlConnection(connectionString);
                        sqlConnection.Open();
                        string queryJmlPenj = "SELECT COALESCE(SUM(a.TotalTagihanPenjualan),0) as TotalPenjualan FROM POPenjualanMst a INNER JOIN Inventory c on a.IdInventory = c.IdInventory INNER JOIN Customer d on a.IdCustomer = d.IdCustomer WHERE SUBSTRING(TanggalPOPenjualan,4,2) = '" + ddlMonths.SelectedValue.Trim() + "' AND SUBSTRING(TanggalPOPenjualan,7,4) = '" + ddlYears.SelectedValue.Trim() + "' ";
                        string queryJmlPenjRtr = "SELECT COALESCE(SUM(a.TotalHargaPOPenjualanReturDtl),0) as TotalPenjualanRetur FROM POPenjualanReturDtl a INNER JOIN POPenjualanMst b on a.IdPOPenjualanMst = b.IdPOPenjualanMst WHERE SUBSTRING(b.TanggalPOPenjualan,4,2) = '" + ddlMonths.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalPOPenjualan,7,4) = '" + ddlYears.SelectedValue.Trim() + "' ";
                        string queryJmlQty = "SELECT COALESCE(SUM(B.QtyBarangPOPenjualanDtl),0) as TotalQTY FROM POPenjualanMst A INNER JOIN POPenjualanDtl B on A.IdPOPenjualanMst = B.IdPOPenjualanMst WHERE SUBSTRING(TanggalPOPenjualan,4,2) = '" + ddlMonths.SelectedValue.Trim() + "' AND SUBSTRING(TanggalPOPenjualan,7,4) = '" + ddlYears.SelectedValue.Trim() + "' ";
                        string queryJmlBox = "SELECT COALESCE(SUM(B.JumlahBoxPOPenjualanDtl),0) as TotalBox FROM POPenjualanMst A INNER JOIN POPenjualanDtl B on A.IdPOPenjualanMst = B.IdPOPenjualanMst WHERE SUBSTRING(TanggalPOPenjualan,4,2) = '" + ddlMonths.SelectedValue.Trim() + "' AND SUBSTRING(TanggalPOPenjualan,7,4) = '" + ddlYears.SelectedValue.Trim() + "' ";
                        string queryJmlQtyRtr = "SELECT COALESCE(SUM(a.QtyBarangPOPenjualanReturDtl),0) as TotalQTYRetur FROM POPenjualanReturDtl a INNER JOIN POPenjualanMst b on a.IdPOPenjualanMst = b.IdPOPenjualanMst WHERE SUBSTRING(b.TanggalPOPenjualan,4,2) = '" + ddlMonths.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalPOPenjualan,7,4) = '" + ddlYears.SelectedValue.Trim() + "' ";
                        string queryJmlBoxRtr = "SELECT COALESCE(SUM(a.JumlahBoxPOPenjualanReturDtl),0) as TotalBoxRtr FROM POPenjualanReturDtl a INNER JOIN POPenjualanMst b on a.IdPOPenjualanMst = b.IdPOPenjualanMst WHERE SUBSTRING(b.TanggalPOPenjualan,4,2) = '" + ddlMonths.SelectedValue.Trim() + "' AND SUBSTRING(b.TanggalPOPenjualan,7,4) = '" + ddlYears.SelectedValue.Trim() + "' ";
                        decimal GTotPnj = 0;
                        decimal GTotQty = 0;
                        int GTotBox = 0;
                        string jumPenj = "";
                        string jumBox = "";
                        string jumQty = "";
                        using (SqlCommand StrQuer = new SqlCommand(queryJmlPenj, sqlConnection))
                        {
                            using (SqlDataReader drTotPnj = StrQuer.ExecuteReader())
                            {
                                if (drTotPnj.Read())
                                {
                                    GTotPnj = (decimal)drTotPnj["TotalPenjualan"];
                                    using (SqlCommand StrQuerRtr = new SqlCommand(queryJmlPenjRtr, sqlConnection))
                                    {
                                        using (SqlDataReader drTotPnjRtr = StrQuerRtr.ExecuteReader())
                                        {
                                            if (drTotPnjRtr.Read())
                                            {
                                                var GTotPnjRtr = (decimal)drTotPnjRtr["TotalPenjualanRetur"];
                                                jumPenj = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPnj - GTotPnjRtr);
                                            }
                                            else
                                            {
                                                jumPenj = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPnj);
                                            }
                                        }
                                    }
                                    //GTPenjualan.Text = string.Format("{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}", GTotPnj);
                                }
                                else
                                {
                                    jumPenj = "0";
                                }
                            }
                        }
                        using (SqlCommand StrQuer = new SqlCommand(queryJmlQty, sqlConnection))
                        {
                            using (SqlDataReader drTotQty = StrQuer.ExecuteReader())
                            {
                                if (drTotQty.Read())
                                {
                                    GTotQty = (decimal)drTotQty["TotalQTY"];
                                    using (SqlCommand StrQuerRtr = new SqlCommand(queryJmlQtyRtr, sqlConnection))
                                    {
                                        using (SqlDataReader drTotPnjRtr = StrQuerRtr.ExecuteReader())
                                        {
                                            if (drTotPnjRtr.Read())
                                            {
                                                var GTotPnjRtr = (decimal)drTotPnjRtr["TotalQTYRetur"];
                                                jumQty = string.Format("{0:n}", GTotQty - GTotPnjRtr) + " KG";
                                            }
                                            else
                                            {
                                                jumQty = string.Format("{0:n}", GTotQty) + " KG";
                                            }
                                        }
                                    }
                                    //GTQty.Text = string.Format("{0:n}", GTotQty) + " KG";
                                }
                                else
                                {
                                    jumQty = "0";
                                }
                            }
                        }
                        using (SqlCommand StrQuer = new SqlCommand(queryJmlBox, sqlConnection))
                        {
                            using (SqlDataReader drTotBox = StrQuer.ExecuteReader())
                            {
                                if (drTotBox.Read())
                                {
                                    GTotBox = (int)drTotBox["TotalBox"];
                                    using (SqlCommand StrQuerRtr = new SqlCommand(queryJmlBoxRtr, sqlConnection))
                                    {
                                        using (SqlDataReader drTotPnjRtr = StrQuerRtr.ExecuteReader())
                                        {
                                            if (drTotPnjRtr.Read())
                                            {
                                                var GTotPnjRtr = (int)drTotPnjRtr["TotalBoxRtr"];
                                                jumBox = (GTotBox - GTotPnjRtr).ToString() + " Box";
                                            }
                                            else
                                            {
                                                jumBox = GTotBox.ToString() + " Box";
                                            }
                                        }
                                    }
                                    //GTBox.Text = GTotBox.ToString() + " Box";
                                }
                                else
                                {
                                    jumBox = "0";
                                }
                            }
                        }
                        // step 2
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                        pdfWriter.PageEvent = new TestCoba.ITextEventsRekapPnj() { parametro = ddlMonths.SelectedItem.Text.Trim() };

                        //open the stream 
                        pdfDoc.Open();
                        int halaman = 0;
                        int temp = 0;
                        int nomer = 1;
                        float dataCount = 210;
                        PdfContentByte canvas = pdfWriter.DirectContent;
                        PdfPCell cell = null;
                        PdfPTable table = null;
                        PdfPTable tableHeader = null;
                        //PdfPCell cellHeader = null;

                        for (int i = 0; i <= halaman; i++)
                        {
                            //pdfDoc.Add(new Paragraph(" "));
                            tableHeader = new PdfPTable(1);
                            tableHeader.WidthPercentage = 100;
                            tableHeader.SpacingBefore = 0f;
                            tableHeader.SetWidths(new float[] { 1f });
                            tableHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfDoc.Add(tableHeader);

                            pdfDoc.Add(new Paragraph(" "));
                            table = new PdfPTable(5);
                            table.TableEvent = new LineaBottom();
                            table.WidthPercentage = 100;
                            table.SetWidths(new float[] { 0.8f, 2.9f, 1.2f, 1f, 2.2f });
                            table.TotalWidth = 400f;
                            if (tableHeader.TotalHeight <= 15)
                            {
                                table.SpacingBefore = 20f;
                            }
                            if (tableHeader.TotalHeight > 15)
                            {
                                table.SpacingBefore = 10f;
                            }
                            table.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell = new PdfPCell(new Phrase("No", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Customer", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Jumlah Per Kg", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Box", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Harga per Kg", FontFactory.GetFont("Courier", 11, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                            for (int j = temp; j < drDtl.Rows.Count; j++)
                            {
                                DataRow dtRow = drDtl.Rows[j];
                                if (dataCount > 5)
                                {
                                    cell = new PdfPCell(new Phrase(nomer.ToString(), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(dtRow["CustomerName"].ToString(), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(dtRow["jumlahqty"].ToString() + " Kg", FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(dtRow["jumlahbox"].ToString() + " box", FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(string.Format("{0:Rp ###,###,##0.00;(Rp ###,###,##0.00);'Rp 0'}", dtRow["jumlahpenj"]), FontFactory.GetFont("Courier", 10, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
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
                                        dataCount = 210;
                                    }
                                }
                            }
                            pdfDoc.Add(table);
                        }
                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Jumlah Box :  " + jumBox + ", Jumlah Qty : " + jumQty + ", Jumlah Penjualan : " + jumPenj, FontFactory.GetFont("Courier", 9, Font.BOLD, BaseColor.BLACK)), 65, dataCount + 95, 0);
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
                    finally { sqlConnection.Close(); sqlConnection.Dispose(); }
                }
            }
        }
    }
}