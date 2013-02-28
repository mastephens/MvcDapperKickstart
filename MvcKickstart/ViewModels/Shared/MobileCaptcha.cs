using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using MvcKickstart.Infrastructure.Extensions;

namespace MvcKickstart.ViewModels.Shared
{
    public class MobileCaptcha
    {
        Random random = new Random();

        public void GenerateCaptcha(MathType? mathType = null)
        {

            Math1 = random.Next(1, 9);
            Math2 = random.Next(1, 9);
            if (mathType == null || mathType.Value == MathType.Random)
            {
                int mathTypeIdx = random.Next(0, 3);
                var values = Enum.GetValues(typeof (MathType)).Cast<MathType>().ToArray();
                mathType = values[mathTypeIdx];
            }
           
            switch (mathType)
            {
                case MathType.Plus:
                    MathAnswer = Math1 + Math2;
                    MathOperator = "+";
                    break;
                case MathType.Minus:
                    MathAnswer = Math1 - Math2;
                    MathOperator = "-";
                    break;
                case MathType.Multiply:
                    MathAnswer = Math1 * Math2;
                    MathOperator = "*";
                    break;
            }

            string stringToEncode = string.Format(pattern, MathAnswer, DateTime.Today.Year,
                                                  DateTime.Today.Month, DateTime.Today.Day, salt);

            EncodedValue = stringToEncode.ToMD5Hash();
        }

        public bool ValidateCaptcha(int? answer, string encodedValue)
        {
            string stringToEncode = string.Format(pattern, answer, DateTime.Today.Year,
                                                 DateTime.Today.Month, DateTime.Today.Day, salt);

            string encodedAnswer = stringToEncode.ToMD5Hash();
            if (!string.IsNullOrWhiteSpace(encodedValue) && answer != null && encodedValue == encodedAnswer) return true;
            return false;
        }

        private string salt = ConfigurationManager.AppSettings["MobileCaptchaSalt"];
        private string pattern = "{0}_{1}_{2}_{3}-{4}";

        public int Math1 { get; set; }
        public int Math2 { get; set; }
        
        public enum MathType
        {
            Random,
            Plus,
            Minus,
            Multiply
        }

        public string MathOperator { get; set; }
        public string EncodedValue { get; set; }
        public int MathAnswer { get; set; }

        // ====================================================================
        // Creates the bitmap image.
        // ====================================================================
        private Image GenerateImage(int number, int width = 16, int height = 16)
        {
            // Create a new 32-bit bitmap image.
            Bitmap bitmap = new Bitmap(
              width,
              height,
              PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, width, height);

            // Fill in the background.
            HatchBrush hatchBrush = new HatchBrush(
              HatchStyle.SmallConfetti,
              Color.LightGray,
              Color.White);
            g.FillRectangle(hatchBrush, rect);

            // Set up the text font.
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new Font(
                  "serif",
                  fontSize,
                  FontStyle.Bold);
                size = g.MeasureString(number.ToString(), font);
            } while (size.Width > rect.Width);

            // Set up the text format.
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Create a path using the text and warp it randomly.
            GraphicsPath path = new GraphicsPath();
            path.AddString(
              number.ToString(),
              font.FontFamily,
              (int)font.Style,
              font.Size, rect,
              format);
            float v = 4F;
            PointF[] points =
      {
        new PointF(
          this.random.Next(rect.Width) / v,
          this.random.Next(rect.Height) / v),
        new PointF(
          rect.Width - this.random.Next(rect.Width) / v,
          this.random.Next(rect.Height) / v),
        new PointF(
          this.random.Next(rect.Width) / v,
          rect.Height - this.random.Next(rect.Height) / v),
        new PointF(
          rect.Width - this.random.Next(rect.Width) / v,
          rect.Height - this.random.Next(rect.Height) / v)
      };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            // Draw the text.
            hatchBrush = new HatchBrush(
              HatchStyle.LargeConfetti,
              Color.LightGray,
              Color.DarkGray);
            g.FillPath(hatchBrush, path);

            // Add some random noise.
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = this.random.Next(rect.Width);
                int y = this.random.Next(rect.Height);
                int w = this.random.Next(m / 50);
                int h = this.random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            // Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            // Set the image.
            return bitmap;
        }
    }
}