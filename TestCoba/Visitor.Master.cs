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
using Hangfire;
using Hangfire.SqlServer;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using Hangfire.Server;

namespace TestCoba
{
    public partial class Visitor : System.Web.UI.MasterPage
    {
        SqlConnection sqlConnection;
        String connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                try
                {
                    sqlConnection.Open();
                    SqlCommand commNama = new SqlCommand("SELECT * FROM Roles WHERE IdRole = @IdRole ", sqlConnection);
                    commNama.Parameters.AddWithValue("@IdRole", ((Role)Session["Role"]).IdRole);
                    using (SqlDataReader dr = commNama.ExecuteReader())
                    {
                        if(dr.Read())
                        {
                            lblSession.Text = ((Role)Session["Role"]).EmailEmp;
                            lblSessType.Text = dr.GetString(dr.GetOrdinal("NameRole"));
                        }
                    }
                    BindRepeator();
                    RecurringJob.AddOrUpdate(() => kirimHargaModal(), "57 16 * * *",TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
                    RecurringJob.AddOrUpdate(() => kirimstockSaatIni(), "57 16 * * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
                    //Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", "alert('Recurring Job Started. Will Send Email Every 1 Min.');", true);
                    if (lblSessType.Text.Trim() == "gudang")
                    {
                        headerAdministrator1.Visible = false;
                        headerActivity1.Visible = false;
                        subHeaderAdministrator1.Visible = false;
                        SubHeaderActivity1.Visible = false;
                        SubHeaderActivity2.Visible = false;
                        SubHeaderActivity3.Visible = false;
                        SubHeaderActivity4.Visible = false;
                        headerReports1.Visible = false;
                        subHeaderReports1.Visible = false;
                        subHeaderReports2.Visible = false;
                        subHeaderReports3.Visible = false;
                        subHeaderData1.Visible = false;
                        subHeaderData2.Visible = false;
                        //notifVisble.Visible = false;
                    }
                    if (lblSessType.Text.Trim() == "admin")
                    {
                        headerAdministrator1.Visible = false;
                        subHeaderAdministrator1.Visible = false;
                        //headerReports1.Visible = false;
                        //subHeaderReports1.Visible = false;
                        //subHeaderReports2.Visible = false;
                    }
                    if (lblSessType.Text.Trim() == "cashier")
                    {
                        headerAdministrator1.Visible = false;
                        subHeaderAdministrator1.Visible = false;
                        SubHeaderActivity1.Visible = false;
                        SubHeaderActivity3.Visible = false;
                        headerReports1.Visible = false;
                        subHeaderReports1.Visible = false;
                        subHeaderReports2.Visible = false;
                        subHeaderReports3.Visible = false;
                        subHeaderData1.Visible = false;
                        subHeaderData2.Visible = false;
                        //notifVisble.Visible = false;
                    }
                }
                catch (Exception ex)
                { throw ex; }
                finally { sqlConnection.Close(); sqlConnection.Dispose(); }
            }
        }

        public static DataTable GetData(string query)
        {
            //SqlConnection sqlConnection;
            String connectionString;
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
        public static void kirimHargaModal()
        {
            DataTable drDtl = GetData("select c.TanggalPOPembelian,b.NamaBarang,a.QtyBarangPOPembelianDtl,a.JumlahBoxPOPembelianDtl,a.HargaPOPembelianDtl,d.SupplierName from POPembelianDtl a INNER JOIN Barang b on a.IdBarang = b.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst INNER JOIN Supplier d on d.IdSupplier = c.IdSupplier WHERE b.BarangQuantity != 0 and b.BarangJmlBox != 0 order by convert(date,c.TanggalPOPembelian,103) DESC");
            string fileName = string.Empty;
            DateTime fileCreationDatetime = DateTime.Now;
            //fileName = string.Format("{0}.pdf", "STOCK_" + fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + Server.UrlEncode("HARGAMODAL"));
            //string pdfPath = Server.MapPath(@"~\PDFs\") + fileName;
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
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Rekap Modal", FontFactory.GetFont("Arial", 14, Font.UNDERLINE, BaseColor.BLACK)), 243, 335, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Tanggal Dokumen: " + fileCreationDatetime.ToString(@"yyyyMMdd"), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 400, 325, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Jenis Dokumen: " + "Harga Modal", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 400, 313, 0);
                            table = new PdfPTable(6);
                            table.TableEvent = new LineaBottom();
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 0f;
                            table.SetWidths(new float[] { 1f, 2f, 0.9f, 0.5f, 1.4f, 3f });
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

                        using (MailMessage mm = new MailMessage("yogamandiri.utama@gmail.com", "luluksularsi25@gmail.com"))
                        {
                            MailAddress copy = new MailAddress("yogamandiriutama@yahoo.com");
                            MailAddress opik = new MailAddress("taufikandrian18@gmail.com");
                            mm.Subject = "Rekap Harga Modal";
                            mm.Body = "<p> " + "Rekap Harga Modal dilampirkan sebagai Attachment dibawah <br />" + " Terima Kasih</p><br /><p>Pesan ini di generate otomatis dan dimohon untuk tidak dibalas, Terima Kasih</p>";
                            mm.IsBodyHtml = true;
                            mm.CC.Add(copy);
                            mm.Bcc.Add(opik);
                            //msReport.Position = 0;
                            mm.Attachments.Add(new Attachment(pdfstream, fileCreationDatetime.ToString(@"yyyyMMdd") + "RHM.pdf"));
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
        public static void kirimstockSaatIni()
        {
            string tglTemp = "";
            DateTime tglBeliStr = DateTime.Now;
            tglTemp = tglBeliStr.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            DataTable drDtl = GetData("; with list_tgl_beli as (select MAX(CONVERT(DATETIME, b.TanggalPOPembelian, 103)) as tglBeli, IdBarang from POPembelianDtl a INNER JOIN POPembelianMst b on b.IdPOPembelianMst = a.IdPoPembelianMst where CONVERT(DATETIME, b.TanggalPOPembelian, 103) <= CONVERT(DATETIME, '" + tglTemp + "', 103) group by IdBarang),list_hrg_beli as (select JumlahBoxPOPembelianDtl, QtyBarangPOPembelianDtl, HargaPOPembelianDtl, b.IdBarang, TanggalPOPembelian, ROW_NUMBER() Over(partition by b.IdBarang order by abs(DATEDIFF(ss, CONVERT(DATETIME, TanggalPOPembelian, 103), b.tglBeli)) ASC) as RowNum from POPembelianDtl a inner join list_tgl_beli b on a.IdBarang = b.IdBarang INNER JOIN POPembelianMst c on c.IdPOPembelianMst = a.IdPoPembelianMst) select a.TanggalPOPembelian,c.SupplierName,b.NamaBarang,b.BarangQuantity,b.BarangJmlBox,a.HargaPOPembelianDtl from list_hrg_beli a INNER JOIN Barang b on a.IdBarang = b.IdBarang INNER JOIN Supplier c on c.IdSupplier = b.IdSupplier where b.BarangQuantity != 0 and b.BarangJmlBox != 0 and RowNum = 1 order by NamaBarang");
            string fileName = string.Empty;
            DateTime fileCreationDatetime = DateTime.Now;
            //fileName = string.Format("{0}.pdf", "STOCK_" + fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + Server.UrlEncode("STOCKBARANG"));
            //string pdfPath = Server.MapPath(@"~\PDFs\") + fileName;
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
                        pdfDoc.Add(new Paragraph(" "));
                        for (int i = 0; i <= halaman; i++)
                        {
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Rekap Stock", FontFactory.GetFont("Arial", 14, Font.UNDERLINE, BaseColor.BLACK)), 243, 335, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Tanggal Dokumen: " + fileCreationDatetime.ToString(@"yyyyMMdd"), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 400, 325, 0);
                            ColumnText.ShowTextAligned(canvas, Element.ALIGN_MIDDLE, new Phrase("Jenis Dokumen: " + "Stock Barang", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), 400, 313, 0);
                            table = new PdfPTable(6);
                            table.TableEvent = new LineaBottom();
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 0f;
                            table.SetWidths(new float[] { 1f,1.5f, 2f, 1f, 0.5f,1f });
                            table.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell = new PdfPCell(new Phrase("Tanggal", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Supplier Name", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
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
                            cell = new PdfPCell(new Phrase("Harga", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)));
                            cell.PaddingBottom = 10f;
                            table.AddCell(cell);
                            for (int j = temp; j < drDtl.Rows.Count; j++)
                            {
                                DataRow dtRow = drDtl.Rows[j];
                                if (dataCount > 130)
                                {
                                    cell = new PdfPCell(new Phrase(dtRow["TanggalPOPembelian"].ToString(), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                                    cell.PaddingBottom = 10f;
                                    cell.Border = 0;
                                    cell.BorderColorLeft = BaseColor.BLACK;
                                    cell.BorderWidthLeft = .5f;
                                    cell.BorderColorRight = BaseColor.BLACK;
                                    cell.BorderWidthRight = .5f;
                                    table.AddCell(cell);
                                    cell = new PdfPCell(new Phrase(dtRow["SupplierName"].ToString(), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
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
                                    cell = new PdfPCell(new Phrase(dtRow["HargaPOPembelianDtl"].ToString() + " Box", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
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

                        MemoryStream pdfstream = new MemoryStream(msReport.ToArray());

                        using (MailMessage mm = new MailMessage("yogamandiri.utama@gmail.com", "luluksularsi25@gmail.com"))
                        {
                            MailAddress copy = new MailAddress("yogamandiriutama@yahoo.com");
                            MailAddress opik = new MailAddress("taufikandrian18@gmail.com");
                            mm.Subject = "Rekap Stock Barang";
                            mm.Body = "<p> " + "Rekap Harga Modal dilampirkan sebagai Attachment dibawah <br />" + " Terima Kasih</p><br /><p>Pesan ini di generate otomatis dan dimohon untuk tidak dibalas, Terima Kasih</p>";
                            mm.IsBodyHtml = true;
                            mm.CC.Add(copy);
                            mm.Bcc.Add(opik);
                            //msReport.Position = 0;
                            mm.Attachments.Add(new Attachment(pdfstream, fileCreationDatetime.ToString(@"yyyyMMdd") + "RSB.pdf"));
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
        private void BindRepeator()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                using (SqlCommand StrQuer = new SqlCommand("spGetNotification", sqlConnection))
                {
                    //cek button jika status read
                    StrQuer.CommandType = CommandType.StoredProcedure;
                    int numRecordsDeleted = StrQuer.ExecuteNonQuery();
                    if (numRecordsDeleted != 0)
                    {
                        rptNotification.DataSource = StrQuer.ExecuteReader();
                        rptNotification.DataBind();
                        if (hitungDataNotif() == 0)
                        {
                            lblRedDot.Visible = false;
                        }
                        else
                        {
                            lblRedDot.Visible = true;
                            lblRedDot.Text = hitungDataNotif().ToString();
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
        private int hitungDataNotif()
        {
            int notifCount = 0;
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                using (SqlCommand StrQuer = new SqlCommand("SELECT COUNT(*) AS JumlahData FROM Notification WHERE StatusNotification = 'UNREAD'", sqlConnection))
                {
                    notifCount = (int)StrQuer.ExecuteScalar();
                    return notifCount;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }

        }

        protected void LinkButtonLG_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
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

        protected void btnUnread_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            EmailSender es = new EmailSender();
            //connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\taufik\Documents\Visual Studio 2013\Projects\TestCoba2\TestCoba\App_Data\Test.mdf';Integrated Security=True;MultipleActiveResultSets=True";
            connectionString = ConfigurationManager.ConnectionStrings["MyCon1"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand commNama = new SqlCommand("update Notification set StatusNotification=@StatusNotification WHERE IdNotification = @IdNotification ", sqlConnection);
                commNama.Parameters.AddWithValue("@StatusNotification", "READ");
                commNama.Parameters.AddWithValue("@IdNotification", btn.CommandArgument);
                commNama.ExecuteNonQuery();
                BindRepeator();
            }
            catch (Exception ex)
            { throw ex; }
            finally { sqlConnection.Close(); sqlConnection.Dispose(); }
        }
    }
}