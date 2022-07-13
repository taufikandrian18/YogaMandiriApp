using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.SqlClient;

namespace TestCoba
{
    public class ITextEventsRekapPnj : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;

        //SqlConnection sqlConnection;
        String connectionString;

        #region Fields
        private string _header;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
                throw de;
            }
            catch (System.IO.IOException ioe)
            {
                throw ioe;
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
        public string parametro { get; set; }
        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);
            iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 18f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Phrase p1Header = new Phrase("Rekap Penjualan Bulan "+parametro, baseFontBig);


            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(1);
            pdfTab.SetWidths(new float[] { 1f });
            pdfTab.WidthPercentage = 80;

            //We will have to create separate cells to include image logo and 2 separate strings
            //Row 1
            PdfPCell pdfCell1 = new PdfPCell();
            PdfPCell pdfCell2 = new PdfPCell(p1Header);
            PdfPCell pdfCell3 = new PdfPCell();
            String text = "Page " + writer.PageNumber + " of ";

            //Add paging to header
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(document.PageSize.GetRight(80), document.PageSize.GetTop(20));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 8);
                //Adds "12" in Page 1 of 12
                cb.AddTemplate(headerTemplate, document.PageSize.GetRight(80) + len, document.PageSize.GetTop(20));
            }
            //Add paging to footer
            //{
            //    String sign1 = "    Penerima             Supir            Kepala Gudang              Hormat Kami";
            //    String sign2 = " (...............) (................) (...................)      (..................)";
            //    cb.BeginText();
            //    cb.SetFontAndSize(bf, 10);
            //    cb.SetTextMatrix(document.PageSize.GetRight(550), document.PageSize.GetBottom(100));
            //    cb.ShowText(sign1);
            //    cb.EndText();
            //    //float len = bf.GetWidthPoint(sign1, 8);
            //    //cb.AddTemplate(footerTemplate, document.PageSize.GetRight(500) + len, document.PageSize.GetBottom(80));
            //    cb.BeginText();
            //    cb.SetFontAndSize(bf, 10);
            //    cb.SetTextMatrix(document.PageSize.GetRight(550), document.PageSize.GetBottom(50));
            //    cb.ShowText(sign2);
            //    cb.EndText();
            //    //float len2 = bf.GetWidthPoint(sign2, 8);
            //    //cb.AddTemplate(footerTemplate, document.PageSize.GetRight(500) + len, document.PageSize.GetBottom(30));
            //}

            //Row 2
            //PdfPCell pdfCell4 = new PdfPCell(new Phrase("Jln. Kebon Kopi No.19,Bandung - Jawa Barat" + "\n" + "Telp. (022) 6042373 - (022) 6026865 Fax. (022) 6026548" + "\n" + "E-Mail : yogamandiriutama@gmail.com", baseFontNormal));

            //Row 3 
            //PdfPCell pdfCell5 = new PdfPCell(new Phrase("Date:" + PrintTime.ToShortDateString(), baseFontBig));
            //PdfPCell pdfCell6 = new PdfPCell();
            //PdfPCell pdfCell7 = new PdfPCell(new Phrase("TIME:" + string.Format("{0:t}", DateTime.Now), baseFontBig));

            //set the alignment of all three cells and set border to 0
            pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;

            pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            //pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
            //pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            //pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            //pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;

            //pdfCell4.Colspan = 3;

            pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            pdfCell3.Border = 0;
            //pdfCell4.Border = 0;
            //pdfCell5.Border = 0;
            //pdfCell6.Border = 0;
            //pdfCell7.Border = 0;

            //add all three cells into PdfTable
            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(pdfCell3);
            //pdfTab.AddCell(pdfCell4);
            //pdfTab.AddCell(pdfCell5);
            //pdfTab.AddCell(pdfCell6);
            //pdfTab.AddCell(pdfCell7);

            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 70;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;    

            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 10, writer.DirectContent);
            //set pdfContent value

            //Move the pointer and draw line to separate header section from rest of page
            //cb.MoveTo(40, document.PageSize.Height - 70);
            //cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 70);
            //cb.Stroke();

            //Move the pointer and draw line to separate footer section from rest of page
            //cb.MoveTo(40, document.PageSize.GetBottom(120));
            //cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(120));
            //cb.Stroke();
        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 8);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            headerTemplate.EndText();

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            footerTemplate.EndText();
        }
    }
}