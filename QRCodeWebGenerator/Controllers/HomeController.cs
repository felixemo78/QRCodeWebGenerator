using Microsoft.Ajax.Utilities;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace QRCodeWebGenerator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

  

        [HttpPost]
        public ActionResult Index(string text)
        {
            Bitmap qrCodeImage = GenerateQRCodeImage(text);

            // Convert the QR code image to a Base64 string
            string base64Image = ConvertImageToBase64(qrCodeImage);

            // Pass the Base64 image string to the view
            ViewBag.QRCodeImage = $"data:image/png;base64,{base64Image}";

            return View();
        }

        private Bitmap GenerateQRCodeImage(string text,bool withLogo = true)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            if (withLogo)
            {
                Bitmap logoImage = new Bitmap(Properties.Resources.NC_logo);

                // Calculate the position to place the logo at the center of the QR code
                int logoPosX = (qrCodeImage.Width - logoImage.Width) / 2;
                int logoPosY = (qrCodeImage.Height - logoImage.Height) / 2;

                // Merge the logo image with the QR code
                using (Graphics graphics = Graphics.FromImage(qrCodeImage))
                {
                    graphics.DrawImage(logoImage, new Point(logoPosX, logoPosY));
                }
            }

            return qrCodeImage;
        }

        private string ConvertImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}