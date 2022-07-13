using System;
using System.Globalization;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using Hangfire;
using Hangfire.SqlServer;
using System.Net;
using System.Net.Mail;

namespace TestCoba
{
    public partial class ItemPage : System.Web.UI.Page
    {
        SqlConnection sqlConnection;
        String connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Role"] != null)
                {
                    connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                    sqlConnection = new SqlConnection(connectionString);
                    try
                    {
                        string sup = "Select * from Supplier WHERE SupplierStatus = 'ACTIVE'";
                        string com = "Select * from Inventory WHERE InvStatus = 'ACTIVE'";
                        SqlDataAdapter adpt = new SqlDataAdapter(com, sqlConnection);
                        SqlDataAdapter supAdpt = new SqlDataAdapter(sup, sqlConnection);
                        DataTable dtSup = new DataTable();
                        DataTable dt = new DataTable();
                        adpt.Fill(dt);
                        supAdpt.Fill(dtSup);
                        ddlSupplier.DataSource = dtSup;
                        ddlSupplier.DataBind();
                        ddlInventory.DataSource = dt;
                        ddlInventory.DataBind();
                        BindRepeator();
                        Role r = new Role();
                        r = (Role)Session["Role"];
                        if (r.IdRole == 4 || r.IdRole == 2)
                        {
                            tmbhBrgBtn.Visible = false;
                            tmbhBrgOlahanBtn.Visible = false;
                            thVal.Visible = false;
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
        private void BindRepeator()
        {
            try
            {
                //connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\taufik\Documents\Visual Studio 2013\Projects\TestCoba2\TestCoba\App_Data\Test.mdf';Integrated Security=True";
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                using (SqlCommand StrQuer = new SqlCommand("spGetBarang", sqlConnection))
                {
                    StrQuer.CommandType = CommandType.StoredProcedure;
                    RepeaterListItm.DataSource = StrQuer.ExecuteReader();
                    RepeaterListItm.DataBind();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
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
        private void updateKapasitasGudang(string idBrg,SqlConnection sqlConnection)
        {
            int capacityTmp = 0;
            try
            {
                SqlCommand commNama = new SqlCommand("SELECT * FROM Barang WHERE IdBarang = @IdBarang ", sqlConnection);
                commNama.Parameters.AddWithValue("@IdBarang", idBrg);
                using (SqlDataReader dr = commNama.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        SqlCommand checkCommInv = new SqlCommand("SELECT IvnSisaCapacity FROM Inventory WHERE IdInventory = @IdInventory ", sqlConnection);
                        checkCommInv.Parameters.AddWithValue("@IdInventory", dr["IdInventory"]);
                        using(SqlDataReader drInv = checkCommInv.ExecuteReader())
                        {
                            if(drInv.Read())
                            {
                                int boxJml = (int)dr["BarangJmlBox"];
                                capacityTmp = (int)drInv["IvnSisaCapacity"];
                                capacityTmp += boxJml;
                                SqlCommand strQuerEditInventory = new SqlCommand("update Inventory set IvnSisaCapacity=@IvnSisaCapacity,IvnUpdateBy=@IvnUpdateBy,IvnUpdateDate=@IvnUpdateDate where IdInventory=@IdInventory", sqlConnection);
                                strQuerEditInventory.Parameters.AddWithValue("@IvnSisaCapacity", capacityTmp);
                                strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateBy", ((Role)Session["Role"]).EmailEmp);
                                strQuerEditInventory.Parameters.AddWithValue("@IvnUpdateDate", DateTime.Now.Date.ToString());
                                strQuerEditInventory.Parameters.AddWithValue("@IdInventory", dr["IdInventory"]);
                                strQuerEditInventory.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string queryFilParam = "SELECT B.BarangItemCode, B.IdBarang, B.NamaBarang , S.SupplierName, B.BarangTanggalBeli, B.BarangQuantity, B.BarangSatuan, B.BarangJmlBox, INV.IvnStorage,B.BarangCategory from Barang B INNER JOIN Supplier S ON B.IdSupplier = S.IdSupplier INNER JOIN Inventory INV on B.IdInventory = INV.IdInventory WHERE ";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                if (ddlSupplier.SelectedValue.Trim() != "" && ddlInventory.SelectedValue.Trim() != "" && ddlBrgCategory.SelectedValue.Trim() != "" && ddlSortBy.SelectedValue.Trim() != "")
                {
                    string queryParam = "B.IdSupplier = '" + ddlSupplier.SelectedValue + "' AND B.IdInventory = '" + ddlInventory.SelectedValue + "' AND B.BarangCategory = '" + ddlBrgCategory.SelectedValue + "'";
                    string queryOrderPrm = " ORDER BY " + ddlSortBy.SelectedValue.Trim() + "";
                    string finalFill = queryFilParam.Substring(0, queryFilParam.Trim().Length - 5);
                    if (queryParam.Trim() != "")
                    {
                        finalFill += "WHERE " + queryParam.Substring(0, queryParam.Trim().Length);
                    }
                    SqlCommand bindDataFilComm = new SqlCommand(finalFill + queryOrderPrm, sqlConnection);
                    SqlDataAdapter fillAdpt = new SqlDataAdapter(bindDataFilComm);
                    DataTable dtFill = new DataTable();
                    fillAdpt.Fill(dtFill);
                    RepeaterListItm.DataSource = dtFill;
                    RepeaterListItm.DataBind();
                }
                else
                {
                    string queryParam = "";
                    string queryOrderPrm = " ORDER BY B.BarangItemCode";
                    if(ddlSupplier.SelectedValue.Trim() != "")
                    {
                        queryParam += "B.IdSupplier = '" + ddlSupplier.SelectedValue + "' AND ";
                    }
                    if(ddlInventory.SelectedValue.Trim() != "")
                    {
                        queryParam += "B.IdInventory = '" + ddlInventory.SelectedValue + "' AND ";
                    }
                    if(ddlBrgCategory.SelectedValue.Trim() != "")
                    {
                        queryParam += "B.BarangCategory = '" + ddlBrgCategory.SelectedValue + "' AND ";
                    }
                    if(ddlSortBy.SelectedValue.Trim() != "")
                    {
                        queryOrderPrm = " ORDER BY " + ddlSortBy.SelectedValue.Trim() + "";
                    }
                    string finalFill = queryFilParam.Substring(0, queryFilParam.Trim().Length - 5);
                    if (queryParam.Trim() != "")
                    {
                        finalFill += "WHERE " + queryParam.Substring(0, queryParam.Trim().Length - 3);
                    }
                    SqlCommand bindDataFilComm = new SqlCommand(finalFill + queryOrderPrm, sqlConnection);
                    SqlDataAdapter fillAdpt = new SqlDataAdapter(bindDataFilComm);
                    DataTable dtFill = new DataTable();
                    fillAdpt.Fill(dtFill);
                    RepeaterListItm.DataSource = dtFill;
                    RepeaterListItm.DataBind();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }

        protected void RepeaterListItm_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Role r = new Role();
            r = (Role)Session["Role"];
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (r.IdRole == 4 || r.IdRole == 2)
                {
                    LinkButton btn = (LinkButton)e.Item.FindControl("lnkUpdate");
                    HtmlTableCell tdValue2 = (HtmlTableCell)e.Item.FindControl("brgCategory");
                    HtmlTableCell tdValue1 = (HtmlTableCell)e.Item.FindControl("tdVal");
                    if (tdValue2.InnerText.Trim() == "PABRIKAN" || tdValue2.InnerText.Trim() == "OLAHAN")
                    {
                        btn.Visible = false;
                    }
                    tdValue1.Visible = false;
                }
                else
                {
                    LinkButton btn = (LinkButton)e.Item.FindControl("lnkUpdate");
                    HtmlTableCell tdValue2 = (HtmlTableCell)e.Item.FindControl("brgCategory");
                    if (tdValue2.InnerText.Trim() == "PABRIKAN")
                    {
                        btn.Visible = false;
                    }
                }
            }
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string link = btn.CommandArgument;
            Response.Redirect("AddBarangOlahan.aspx?id="+link);
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
            DataTable drDtl = GetData("SELECT a.BarangItemCode,a.BarangTanggalBeli,a.NamaBarang,a.BarangQuantity,a.BarangJmlBox,b.IvnSisaCapacity FROM Barang a INNER JOIN Inventory b on a.IdInventory = b.IdInventory WHERE a.BarangQuantity != 0 and a.BarangJmlBox != 0 ORDER BY a.NamaBarang");
            string fileName = string.Empty;
            DateTime fileCreationDatetime = DateTime.Now;
            fileName = string.Format("{0}.pdf", "STOCK_" + fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + Server.UrlEncode("STOCKBARANG"));
            string pdfPath = Server.MapPath(@"~\PDFs\") + fileName;
            using (FileStream msReport = new FileStream(pdfPath, FileMode.Create))
            {
                using (Document pdfDoc = new Document(PageSize.A5.Rotate(), 10f, 10f, 140f, 10f))
                {
                    try
                    {
                        // step 2
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                        pdfWriter.PageEvent = new TestCoba.ITextEventStock();

                        //open the stream 
                        pdfDoc.Open();
                        int halaman = 0;
                        int temp = 0;
                        float dataCount = 230;
                        PdfContentByte canvas = pdfWriter.DirectContent;
                        PdfPCell cell = null;
                        PdfPTable table = null;
                        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                        pdfDoc.Add(new Paragraph(" "));
                        for (int i = 0; i <= halaman; i++)
                        {
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Rekap Stock", FontFactory.GetFont("Arial", 14, Font.UNDERLINE, BaseColor.BLACK)), 243, 335, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Tanggal Dokumen: " + fileCreationDatetime.ToString(@"yyyyMMdd"), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 400, 325, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Jenis Dokumen: " + "Stock Barang", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 400, 313, 0);
                            table = new PdfPTable(4);
                            table.TableEvent = new LineaBottom();
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 0f;
                            table.SetWidths(new float[] { 1f,3f,1f,1f });
                            table.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell = new PdfPCell(new Phrase("Tanggal", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Nama Barang", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Qty / Kg", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Box", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            table.AddCell(cell);
                            for (int j = temp; j < drDtl.Rows.Count; j++)
                            {
                                DataRow dtRow = drDtl.Rows[j];
                                if (dataCount > 65)
                                {
                                    cell = new PdfPCell(new Phrase(dtRow["BarangTanggalBeli"].ToString(), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    string[] values = { dtRow["NamaBarang"].ToString().ToLower().Trim() };
                                    foreach (var value in values)
                                        cell = new PdfPCell(new Phrase(ti.ToTitleCase(value), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(dtRow["BarangQuantity"].ToString() + " Kg", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(dtRow["BarangJmlBox"].ToString() + " Box", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    dataCount -= 20;
                                    temp++;
                                }
                                else
                                {
                                    if (j != drDtl.Rows.Count)
                                    {
                                        halaman += 1;
                                        temp = j;
                                        j = drDtl.Rows.Count;
                                        dataCount = 230;
                                    }
                                }
                            }
                            pdfDoc.Add(table);
                            pdfDoc.NewPage();
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

        protected void btnCetakModal_Click(object sender, EventArgs e)
        {
            DataTable drDtl = GetData("select c.TanggalPOPembelian,b.NamaBarang,a.QtyBarangPOPembelianDtl,a.JumlahBoxPOPembelianDtl,a.HargaPOPembelianDtl,d.SupplierName from POPembelianDtl a INNER JOIN Barang b on a.IdBarang = b.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst INNER JOIN Supplier d on d.IdSupplier = c.IdSupplier WHERE b.BarangQuantity != 0 and b.BarangJmlBox != 0 order by convert(date,c.TanggalPOPembelian,103)");
            string fileName = string.Empty;
            DateTime fileCreationDatetime = DateTime.Now;
            fileName = string.Format("{0}.pdf", "STOCK_" + fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + Server.UrlEncode("HARGAMODAL"));
            string pdfPath = Server.MapPath(@"~\PDFs\") + fileName;
            using (MemoryStream msReport = new MemoryStream())
            {
                using (Document pdfDoc = new Document(PageSize.A5.Rotate(), 10f, 10f, 140f, 10f))
                {
                    try
                    {
                        // step 2
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                        pdfWriter.PageEvent = new TestCoba.ITextEventStock();

                        //open the stream 
                        pdfDoc.Open();
                        int halaman = 0;
                        int temp = 0;
                        float dataCount = 230;
                        PdfContentByte canvas = pdfWriter.DirectContent;
                        PdfPCell cell = null;
                        PdfPTable table = null;
                        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                        TextInfo tiSup = CultureInfo.CurrentCulture.TextInfo;
                        pdfDoc.Add(new Paragraph(" "));
                        for (int i = 0; i <= halaman; i++)
                        {
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Rekap Stock", FontFactory.GetFont("Arial", 14, Font.UNDERLINE, BaseColor.BLACK)), 243, 335, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Tanggal Dokumen: " + fileCreationDatetime.ToString(@"yyyyMMdd"), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 400, 325, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Jenis Dokumen: " + "Harga Modal", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 400, 313, 0);
                            table = new PdfPTable(6);
                            table.TableEvent = new LineaBottom();
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 0f;
                            table.SetWidths(new float[] { 1f,2f,0.9f,0.5f,1.4f,3f });
                            table.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell = new PdfPCell(new Phrase("Tanggal", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Nama Barang", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Qty / Kg", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Box", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Harga Modal", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Supplier", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            table.AddCell(cell);
                            for (int j = temp; j < drDtl.Rows.Count; j++)
                            {
                                DataRow dtRow = drDtl.Rows[j];
                                if (dataCount > 65)
                                {
                                    cell = new PdfPCell(new Phrase(dtRow["TanggalPOPembelian"].ToString(), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    string[] values = { dtRow["NamaBarang"].ToString().ToLower().Trim() };
                                    foreach (var value in values)
                                        cell = new PdfPCell(new Phrase(ti.ToTitleCase(value), FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(dtRow["QtyBarangPOPembelianDtl"].ToString(), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(dtRow["JumlahBoxPOPembelianDtl"].ToString(), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase("Rp. " + dtRow["HargaPOPembelianDtl"].ToString(), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    string[] values1 = { dtRow["SupplierName"].ToString().ToLower().Trim() };
                                    foreach (var value in values1)
                                        cell = new PdfPCell(new Phrase(tiSup.ToTitleCase(value), FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    dataCount -= 20;
                                    temp++;
                                }
                                else
                                {
                                    if (j != drDtl.Rows.Count)
                                    {
                                        halaman += 1;
                                        temp = j;
                                        j = drDtl.Rows.Count;
                                        dataCount = 230;
                                    }
                                }
                            }
                            pdfDoc.Add(table);
                            pdfDoc.NewPage();
                        }

                        //msReport.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                        pdfDoc.Close();
                        //byte[] bytes = memoryStream.ToArray();
                        //memoryStream.Close();

                        MemoryStream pdfstream = new MemoryStream(msReport.ToArray());

                        using (MailMessage mm = new MailMessage("yogamandiri.utama@gmail.com", "taufikandrian18@gmail.com"))
                        {
                            mm.Subject = "Rekap Harga Modal";
                            mm.Body = "<p> " + "Rekap Harga Modal dilampirkan sebagai Attachment dibawah <br />" + " Terima Kasih</p><br /><p>Pesan ini di generate otomatis dan dimohon untuk tidak dibalas, Terima Kasih</p>";
                            mm.IsBodyHtml = true;
                            //msReport.Position = 0;
                            mm.Attachments.Add(new Attachment(pdfstream, "test.pdf"));
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            NetworkCredential NetworkCred = new NetworkCredential("yogamandiri.utama@gmail.com", "gwgnjpdawyrayohz");
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = 587;
                            smtp.Send(mm);
                            pdfstream.Close();
                        }


                        msReport.Close();


                        //Response.Clear();
                        //Response.ContentType = "application/pdf";
                        //Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                        //Response.ContentType = "application/pdf";
                        //Response.Buffer = true;
                        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        //Response.WriteFile(pdfPath);
                        ////Response.BinaryWrite(bytes);
                        //Response.End();
                        //Response.Close();
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
    }
}