using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Jugnoon.Utility
{
    /// <summary>
    ///  Utility Class Responsible for Image Processing / Crop / Resize / Compress etc
    /// </summary>
    public class Image
    {
        public static Bitmap CreateThumbnail(string lcFilename, int targetSize)
        {
            Bitmap loBMP = null;
            Bitmap bmpOut = null;
            try
            {
                loBMP = new Bitmap(lcFilename);
                ImageFormat loFormat = loBMP.RawFormat;
                Size newSize = CalculateDimensions(loBMP.Size, targetSize);
                bmpOut = new Bitmap(newSize.Width, newSize.Height);
                Graphics canvas = Graphics.FromImage(bmpOut);
                canvas.SmoothingMode = SmoothingMode.AntiAlias;
                canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                canvas.DrawImage(loBMP, new Rectangle(new Point(0, 0), newSize));
            }
            finally
            {
                loBMP.Dispose();
            }

            return bmpOut;
        }

        public static Bitmap CreateThumbnail(string lcFilename, int width, int height)
        {
            if (height == 0)
            {
                return CreateThumbnail(lcFilename, width);
            }
            else
            {

                Bitmap loBMP = null;
                Bitmap bmpOut = null;
                try
                {
                    loBMP = new Bitmap(lcFilename);
                    ImageFormat loFormat = loBMP.RawFormat;
                    Size newSize = new Size();
                    newSize.Height = height;
                    newSize.Width = width;
                    bmpOut = new Bitmap(newSize.Width, newSize.Height);
                    Graphics canvas = Graphics.FromImage(bmpOut);
                    canvas.SmoothingMode = SmoothingMode.AntiAlias;
                    canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    canvas.DrawImage(loBMP, new Rectangle(new Point(0, 0), newSize));
                }
                finally
                {
                    loBMP.Dispose();
                }

                return bmpOut;
            }

        }

        public static void CreateCropThumbnail(string lcFilename, int width, int height, string coverPath, bool targettop = false)
        {
            SaveCroppedImage(new Bitmap(lcFilename), width, height, coverPath, targettop);
        }

        private static Size CalculateDimensions(Size oldSize, int targetSize)
        {
            Size newSize = new Size();
            if (oldSize.Height > oldSize.Width)
            {
                newSize.Width = Convert.ToInt32(oldSize.Width * (Convert.ToSingle(targetSize) / Convert.ToSingle(oldSize.Height)));
                newSize.Height = targetSize;
            }
            else
            {
                newSize.Width = targetSize;

                newSize.Height = Convert.ToInt32(oldSize.Height * (Convert.ToSingle(targetSize) / Convert.ToSingle(oldSize.Width)));
            }
            return newSize;
        }

        /// <summary>
        /// Compress image in size based on quality (0 - 100)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="img"></param>
        /// <param name="quality"></param>
        public static void SaveJpeg(string path, Bitmap img, long quality)
        {
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            ImageCodecInfo jpegCodec = getEncoderInfo("image/jpeg");
            if (jpegCodec == null)
            {
                return;
            }

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        /// <summary>
        /// Generate resized png image
        /// </summary>
        /// <param name="path"></param>
        /// <param name="img"></param>
        /// <param name="quality"></param>
        public static void SavePNG(string path, Bitmap img, long quality)
        {
            // Encoder parameter for image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = getEncoderInfo("image/png");

            if (jpegCodec == null)
            {
                return;
            }

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }


        private static ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i <= codecs.Length - 1; i++)
            {
                if (codecs[i].MimeType == mimeType)
                {
                    return codecs[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Utility function for generating different size thumbnails
        /// </summary>
        /// <param name="path"></param>
        /// <param name="thumbpath"></param>
        /// <param name="midthumbpath"></param>
        /// <param name="thumbwidth"></param>
        /// <param name="thumbheight"></param>
        /// <param name="midthumbwidth"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public static string Generate_Thumbs(string path, string thumbpath, string midthumbpath, int thumbwidth, int thumbheight, int midthumbwidth, long quality)
        {
            if (!string.IsNullOrEmpty(thumbpath))
            {
                Bitmap mp = null;
                Copy_Photo(path, thumbpath);
                mp = CreateThumbnail(path, thumbwidth, thumbheight);
                if (mp == null)
                {
                    return "";
                }
                mp.Save(thumbpath);
                // Mid thumbs
                SaveJpeg(thumbpath, mp, quality);
                mp.Dispose();
            }
            if (!string.IsNullOrEmpty(midthumbpath))
            {
                Bitmap midmp = null;
                Copy_Photo(path, midthumbpath);
                midmp = CreateThumbnail(path, midthumbwidth);
                if (midmp == null)
                {
                    return "";
                }
                midmp.Save(midthumbpath);
                // Thumbs
                SaveJpeg(midthumbpath, midmp, quality);
                midmp.Dispose();
            }
            return "1";
        }

        /// <summary>
        /// Utility function for generating different size thumbnails
        /// </summary>
        /// <param name="path"></param>
        /// <param name="thumbpath"></param>
        /// <param name="midthumbpath"></param>
        /// <param name="thumbwidth"></param>
        /// <param name="thumbheight"></param>
        /// <param name="midthumbwidth"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public static string Generate_Thumbs(string path, string thumbpath, string midthumbpath, int thumbwidth, int thumbheight, int midthumbwidth, int midthumbheight, long quality)
        {
            if (!string.IsNullOrEmpty(thumbpath))
            {
                Bitmap mp = null;
                Copy_Photo(path, thumbpath);
                mp = CreateThumbnail(path, thumbwidth, thumbheight);
                if (mp == null)
                {
                    return "";
                }
                mp.Save(thumbpath);
                // Mid thumbs
                SaveJpeg(thumbpath, mp, quality);
                mp.Dispose();
            }
            if (!string.IsNullOrEmpty(midthumbpath))
            {
                Bitmap midmp = null;
                Copy_Photo(path, midthumbpath);
                midmp = CreateThumbnail(path, midthumbwidth, midthumbheight);
                if (midmp == null)
                {
                    return "";
                }
                midmp.Save(midthumbpath);
                // Thumbs
                SaveJpeg(midthumbpath, midmp, quality);
                midmp.Dispose();
            }
            return "1";
        }

        private static void Copy_Photo(string original_path, string new_path)
        {
            FileInfo TheFile = new FileInfo(original_path);
            if (TheFile.Exists)
            {
                File.Copy(original_path, new_path);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }


        /// <summary>
        /// Generate cropped image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="filePath"></param>
        /// <param name="targettop"></param>
        /// <returns></returns>
        public static bool SaveCroppedImage(System.Drawing.Image image, int maxWidth, int maxHeight, string filePath, bool targettop = false)
        {
            ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders()
                                     .Where(codecInfo =>
                                     codecInfo.MimeType == "image/jpeg").First();
            System.Drawing.Image finalImage = image;
            System.Drawing.Bitmap bitmap = null;
            try
            {
                int left = 0;
                int top = 0;
                int srcWidth = maxWidth;
                int srcHeight = maxHeight;
                bitmap = new System.Drawing.Bitmap(maxWidth, maxHeight);
                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                if (image.Width > image.Height)
                {
                    srcWidth = (int)(Math.Round(image.Height * croppedWidthToHeight));
                    if (srcWidth < image.Width)
                    {
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                    else
                    {
                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                }
                else
                {
                    srcHeight = (int)(Math.Round(image.Width * croppedHeightToWidth));
                    if (srcHeight < image.Height)
                    {
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                    else
                    {
                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }

                    if (targettop)
                    {
                        // reset top / left = 0
                        top = 0;
                        left = 0;
                    }
                }

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    new Rectangle(left, top, srcWidth, srcHeight), GraphicsUnit.Pixel);
                }
                finalImage = bitmap;
            }
            catch
            {
            }
            try
            {
                using (EncoderParameters encParams = new EncoderParameters(1))
                {
                    encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)100);
                    //quality should be in the range 
                    //[0..100] .. 100 for max, 0 for min (0 best compression)
                    finalImage.Save(filePath, jpgInfo, encParams);
                    return true;
                }
            }
            catch { }
            if (bitmap != null)
            {
                bitmap.Dispose();
            }
            return false;
        }

        /// <summary>
        /// Generate resized image based on width / heigh in real-time online by providing file path
        /// </summary>
        /// <param name="lcFilename"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static byte[] Generate_Online_Thumbnail(string lcFilename, int Width, int Height)
        {
            Bitmap loBMP = null;
            Bitmap bmpOut = null;
            try
            {
                loBMP = new Bitmap(lcFilename);
                ImageFormat loFormat = loBMP.RawFormat;
                Size newSize = new Size();
                newSize.Height = Height;
                newSize.Width = Width;
                bmpOut = new Bitmap(newSize.Width, newSize.Height);
                Graphics canvas = Graphics.FromImage(bmpOut);
                canvas.SmoothingMode = SmoothingMode.AntiAlias;
                canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                canvas.DrawImage(loBMP, new Rectangle(new Point(0, 0), newSize));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var imageStream = new MemoryStream();
            byte[] imageContent;
            if (lcFilename.StartsWith("http"))
            {
                string _fileName = lcFilename.Replace("thumbs/", "");
                // download and save image in memory stream
                var client = new WebClient();
                byte[] imageBytes = client.DownloadData(_fileName);
                imageContent = Jugnoon.Utility.Image.Generate_Online_Thumbnail(new MemoryStream(imageBytes), Width, Height);
                imageStream.Position = 0;
                imageStream.Read(imageContent, 0, (int)imageStream.Length);
            }
            else
            {
                bmpOut.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                imageContent = new Byte[imageStream.Length];
                imageStream.Position = 0;
                imageStream.Read(imageContent, 0, (int)imageStream.Length);
            }

            return imageContent;
        }

        /// <summary>
        /// Generate resized image based on width / heigh in real-time online by providing file stream
        /// </summary>
        /// <param name="lcFilename"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static byte[] Generate_Online_Thumbnail(Stream lcFilename, int Width, int Height)
        {
            Bitmap loBMP = null;
            Bitmap bmpOut = null;
            try
            {
                loBMP = new Bitmap(lcFilename);
                ImageFormat loFormat = loBMP.RawFormat;
                Size newSize = new Size();
                newSize.Height = Height;
                newSize.Width = Width;
                bmpOut = new Bitmap(newSize.Width, newSize.Height);
                Graphics canvas = Graphics.FromImage(bmpOut);
                canvas.SmoothingMode = SmoothingMode.AntiAlias;
                canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                canvas.DrawImage(loBMP, new Rectangle(new Point(0, 0), newSize));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var imageStream = new MemoryStream();
            bmpOut.Save(imageStream, ImageFormat.Jpeg);

            byte[] imageContent = new Byte[imageStream.Length];
            imageStream.Position = 0;
            imageStream.Read(imageContent, 0, (int)imageStream.Length);
            return imageContent;
        }
    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
