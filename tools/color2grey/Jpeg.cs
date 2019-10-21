/*  src/Jpegs.cs
    Written by, Sohail Qayum Malik [https://sohail.github.io] */

using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace cannyEdgeDetection
{
    public class jpeg
    {
        private Uri[] uri;
        //private JpegBitmapDecoder decoder;
        private int sizeOfSinglePixelInNumberOfBytes;
        
        public jpeg(ref String[] arguments)
        {           
            uri = new Uri[]{new Uri(arguments[1], UriKind.RelativeOrAbsolute), new Uri(arguments[2], UriKind.RelativeOrAbsolute)};
                    
            Image image = Image.FromFile(uri[constants.SOURCE_FILE].ToString(), true);
            sizeOfSinglePixelInNumberOfBytes = Image.GetPixelFormatSize(image.PixelFormat) / 8;
            NewMethod(image);            
        }

        private static void NewMethod(Image image)
        {
            image.Dispose();
        }

        public void process()
        {
            BitmapSource bitmapSource = (new JpegBitmapDecoder(uri[constants.SOURCE_FILE], BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default)).Frames[0];
            
            Console.WriteLine("Processing " + uri[constants.SOURCE_FILE].ToString() + " & " + uri[constants.DESTINATION_FILE].ToString());
            Console.WriteLine("*\n*");
            Console.WriteLine("DPI-Y(Vertical Resolution) = " + bitmapSource.DpiY + ", DPI-X(Horizontal Resolution) = " + bitmapSource.DpiX);
            Console.WriteLine("PixelHeight = " + bitmapSource.PixelHeight + ", PixelWidth = " + bitmapSource.PixelWidth);
            Console.WriteLine("PixelFormat in number of bytes(bitDepth) = " + sizeOfSinglePixelInNumberOfBytes);            

            byte[] pixels_source = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth * sizeOfSinglePixelInNumberOfBytes];
            byte[] pixels_destination = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth]; 

            bitmapSource.CopyPixels(pixels_source, bitmapSource.PixelWidth * sizeOfSinglePixelInNumberOfBytes, 0);
        
            Console.WriteLine("Converting from color to grey...");

            if (sundry.color2grey(ref pixels_source, ref pixels_destination, sizeOfSinglePixelInNumberOfBytes))
            {                                        
                Console.WriteLine("Done.");

                sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString(), ref pixels_destination, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY);

                Console.WriteLine("Saved to file "+uri[constants.DESTINATION_FILE].ToString());
            }
            else
            {
                 Console.WriteLine("Not done!! call to \"color2grey\" failed.");
            }
        }       
    }
}