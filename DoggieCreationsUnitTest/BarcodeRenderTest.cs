using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Zen.Barcode;
using Zen.Barcode.SSRS;

namespace DoggieCreationsUnitTest
{
    [TestClass]
    public class BarcodeRenderTest
    {
        [TestMethod]
        public void TestBarcode()
        {
            var barcodeImageBuilder = new BarcodeImageBuilder();
            barcodeImageBuilder.Text = "0123456789";
            barcodeImageBuilder.Symbology = BarcodeSymbology.Code128;

            var imageBytes = barcodeImageBuilder.GetBarcodeImage();

            Image image = null;
            try
            {
                image = Image.FromStream(new MemoryStream(imageBytes));
            }
            catch (ArgumentException)
            {

            }
            var filename = string.Format(@"{0}.jpg", barcodeImageBuilder.Text);

            image.Save(filename);
        }

        [TestMethod]
        public void TestPDF()
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Barcode generator";
            XFont font = new XFont("Verdana", 16);

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Draw the text
            gfx.DrawString("Onderstaande barcode heeft uniek nummer: 0123456789", font, XBrushes.Black,
              new XRect(20, 20, 200, 20),
              XStringFormats.TopLeft);
            var image = XImage.FromFile("testImage.jpg");

            gfx.DrawImage(image, new XRect(20, 40, image.PixelWidth, image.PixelHeight));
            // Save the document...
            const string filename = "HelloWorld_tempfile.pdf";
            document.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }

        [TestMethod]
        public void TestMail()
        {
            MailMessage mail = new MailMessage(new MailAddress("username@gmail.com", "Je Moeder"),
                new MailAddress("username@gmail.com", "Je Moeder"));

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("username@gmail.com", "password"),
                EnableSsl = true
            };
            mail.Subject = "Test Barcode Generator, PDF Generator en SMTP sender";
            mail.Body = "Hallo daar, hier ff een testje :-)";
            mail.Attachments.Add(new Attachment("HelloWorld_tempfile.pdf"));
            client.Send(mail);
        }
    }
}
