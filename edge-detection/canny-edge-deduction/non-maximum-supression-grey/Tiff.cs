/*  src/Tiff.cs
    Written by, Sohail Qayum Malik [https://sohail.github.io] */

using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace cannyEdgeDetection
{
    public class tiff
    {
        private Uri[] uri;        
        private int sizeOfSinglePixelInNumberOfBytes;
        
        public tiff(ref String[] arguments)
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
            BitmapSource bitmapSource = (new TiffBitmapDecoder(uri[constants.SOURCE_FILE], BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad)).Frames[0];
                        
            Console.WriteLine("Processing " + uri[constants.SOURCE_FILE].ToString() + " & " + uri[constants.DESTINATION_FILE].ToString());
            Console.WriteLine("*\n*");
            Console.WriteLine("DPI-Y(Vertical Resolution) = " + bitmapSource.DpiY + ", DPI-X(Horizontal Resolution) = " + bitmapSource.DpiX);
            Console.WriteLine("PixelHeight = " + bitmapSource.PixelHeight + ", PixelWidth = " + bitmapSource.PixelWidth);
            Console.WriteLine("PixelFormat in number of bytes(bitDepth) = " + sizeOfSinglePixelInNumberOfBytes);            

            byte[] pixels = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth];
            byte[] pixels_source = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth * sizeOfSinglePixelInNumberOfBytes];
            
            double[] pixels_double_horizontally_sobleed = new double[bitmapSource.PixelHeight * bitmapSource.PixelWidth];
            double[] pixels_double_vertically_sobleed = new double[bitmapSource.PixelHeight * bitmapSource.PixelWidth];
            double[] pixels_double_quotient = new double[bitmapSource.PixelHeight * bitmapSource.PixelWidth];
            double[] pixels_double_squared_horizontal_soble = new double[bitmapSource.PixelHeight * bitmapSource.PixelWidth];
            double[] pixels_double_squared_verical_soble = new double[bitmapSource.PixelHeight * bitmapSource.PixelWidth];
            double[] pixels_double_squared_horizontal_vertical_sobel_summed = new double[bitmapSource.PixelHeight * bitmapSource.PixelWidth];
            double[] pixels_double_squared_horizontal_vertical_sobel_squared_summed_sqrted = new double[bitmapSource.PixelHeight * bitmapSource.PixelWidth];
                        
            bitmapSource.CopyPixels(pixels_source, bitmapSource.PixelWidth * sizeOfSinglePixelInNumberOfBytes, 0);

            // For documentation purposes
            /*try
            {
                Console.WriteLine(uri[constants.SOURCE_FILE].GetComponents(UriComponents.AbsoluteUri, UriFormat.Unescaped));
            }
            catch(System.InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }*/

            Console.WriteLine("Performing Sobel horizontal convolution on "+uri[constants.SOURCE_FILE].ToString()+"...");
            sundry.sobel(ref pixels_source, ref pixels_double_horizontally_sobleed, bitmapSource.PixelHeight, bitmapSource.PixelWidth, sizeOfSinglePixelInNumberOfBytes); 
            sundry.double2byte(ref pixels_double_horizontally_sobleed, ref pixels);
            sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString()+"sobel-horizontal.tiff", ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY);
            Console.WriteLine("Saved to file "+uri[constants.DESTINATION_FILE].ToString()+"-sobel-horizontal.tiff.");

            Console.WriteLine("Performing Sobel vertical convolution on "+uri[constants.SOURCE_FILE].ToString()+"...");
            sundry.sobel(ref pixels_source, ref pixels_double_vertically_sobleed, bitmapSource.PixelHeight, bitmapSource.PixelWidth, sizeOfSinglePixelInNumberOfBytes, constants.DIRECTION_SOBEL_VERTICAL);
            sundry.double2byte(ref pixels_double_vertically_sobleed, ref pixels);
            sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString()+"sobel-vertical.tiff", ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY);
            Console.WriteLine("Saved to file "+uri[constants.DESTINATION_FILE].ToString()+"-sobel-vertical.tiff.");

            Console.WriteLine("Getting slope done...");
            sundry.divide(ref pixels_double_horizontally_sobleed, ref pixels_double_vertically_sobleed, ref pixels_double_quotient, sizeOfSinglePixelInNumberOfBytes);
            sundry.double2byte(ref pixels_double_quotient, ref pixels);
            sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString()+"sobel_horizontal_vertical_slope.tiff", ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY);
            Console.WriteLine("Saved to file "+uri.ToString()+"-sobel_horizontal_vertical_slope.tiff");

            Console.WriteLine("Squring "+uri[constants.DESTINATION_FILE].ToString()+"-sobel-horizontal.tiff...");
            sundry.square(ref pixels_double_horizontally_sobleed, ref pixels_double_squared_horizontal_soble, sizeOfSinglePixelInNumberOfBytes);
            sundry.double2byte(ref pixels_double_squared_horizontal_soble, ref pixels);
            sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString()+"sobel_horizontal_squared.tiff", ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY);            
            Console.WriteLine("Saved squred "+uri.ToString()+"-sobel-horizontal.tiff to "+uri.ToString()+"-sobel-horizontal-squared.tiff.");
            Console.WriteLine("Squring "+uri[constants.DESTINATION_FILE].ToString()+"-sobel-vertical.tiff...");
            sundry.square(ref pixels_double_vertically_sobleed, ref pixels_double_squared_verical_soble, sizeOfSinglePixelInNumberOfBytes);
            sundry.double2byte(ref pixels_double_squared_verical_soble, ref pixels);
            sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString()+"sobel_vertical_squared.tiff", ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY); 
            Console.WriteLine("Saved squred "+uri[constants.DESTINATION_FILE].ToString()+"-sobel-vertical.tiff to "+uri.ToString()+"-sobel-vertical-squared.tiff.");

            Console.WriteLine("Summing "+uri[constants.DESTINATION_FILE].ToString()+"-sobel-horizontal-squared.tiff, "+uri[constants.DESTINATION_FILE].ToString()+"-sobel-vertical-squared.tiff...");
            sundry.sum(ref pixels_double_squared_horizontal_soble, ref pixels_double_squared_verical_soble, ref pixels_double_squared_horizontal_vertical_sobel_summed, sizeOfSinglePixelInNumberOfBytes);
            sundry.double2byte(ref pixels_double_squared_horizontal_vertical_sobel_summed, ref pixels);
            sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString()+"sobel_horizontal_vertical_squared_summed.tiff", ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY);
            Console.WriteLine("Saved into "+uri[constants.DESTINATION_FILE].ToString()+"sobel_horizontal_vertical_squared_summed.tiff.");

            Console.WriteLine("Taking square root of "+uri[constants.DESTINATION_FILE].ToString()+"sobel_horizontal_vertical_squared_summed.tiff...");
            sundry.sqrt(ref pixels_double_squared_horizontal_vertical_sobel_summed, ref pixels_double_squared_horizontal_vertical_sobel_squared_summed_sqrted, sizeOfSinglePixelInNumberOfBytes);
            sundry.double2byte(ref pixels_double_squared_horizontal_vertical_sobel_squared_summed_sqrted, ref pixels);
            sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString()+"sobel_horizontal_vertical_squared_summed_sqrted.tiff", ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY);
            Console.WriteLine("Saved into "+uri[constants.DESTINATION_FILE].ToString()+"sobel_horizontal_vertical_squared_summed_sqrted.tiff.");
            
            Console.WriteLine("About to perform non-maximum-supression on file "+uri[constants.DESTINATION_FILE].ToString()+"sobel_horizontal_vertical_squared_summed_sqrted.tiff, which holds the magnitudes and file "+uri[constants.DESTINATION_FILE].ToString()+"sobel_horizontal_vertical_slope.tiff, which holds the directions or slopes...");
            sundry.non_maximum_supression(ref pixels_double_squared_horizontal_vertical_sobel_squared_summed_sqrted, ref pixels_double_squared_horizontal_soble, ref pixels, bitmapSource.PixelHeight, bitmapSource.PixelWidth, sizeOfSinglePixelInNumberOfBytes);
            Console.WriteLine("Done performing non-maximum-supression...");
            sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString()+"-non-maximum-suppressed.tiff", ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY);
            Console.WriteLine("Saved to file "+uri[constants.DESTINATION_FILE].ToString()+"-non-maximum-suppressed.tiff");
                                                
            Console.WriteLine("Done doing " + uri[constants.DESTINATION_FILE].ToString());
        }       
    }
}