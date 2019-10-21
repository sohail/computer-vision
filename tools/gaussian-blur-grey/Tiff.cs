/*  src/Jpegs.cs
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

        public void process(uint? passes = 1)
        {
            BitmapSource bitmapSource = (new TiffBitmapDecoder(uri[constants.SOURCE_FILE], BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad)).Frames[0];
                        
            Console.WriteLine("Processing " + uri[constants.SOURCE_FILE].ToString() + " & " + uri[constants.DESTINATION_FILE].ToString());
            Console.WriteLine("*\n*");
            Console.WriteLine("DPI-Y(Vertical Resolution) = " + bitmapSource.DpiY + ", DPI-X(Horizontal Resolution) = " + bitmapSource.DpiX);
            Console.WriteLine("PixelHeight = " + bitmapSource.PixelHeight + ", PixelWidth = " + bitmapSource.PixelWidth);
            Console.WriteLine("PixelFormat in number of bytes(bitDepth) = " + sizeOfSinglePixelInNumberOfBytes);            

            byte[] pixels_source = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth * sizeOfSinglePixelInNumberOfBytes];
            byte[] pixels_destination = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth * sizeOfSinglePixelInNumberOfBytes]; 

            double[,] gaussiankernel = sundry.gaussianKernel(Tuple.Create(5,5), constants.GAUSSIAN_KERNEL_SMOOTHENING_STANDARD_DEVIATION);

            bitmapSource.CopyPixels(pixels_source, bitmapSource.PixelWidth * sizeOfSinglePixelInNumberOfBytes, 0);

            Console.WriteLine("Blurring...");
            if (passes > 1)
            {
                Console.WriteLine("Will take time...");
            }

            for (var i = 0; i < passes; i++)
            {  
                Console.Write(".");
                sundry.convolution(ref pixels_source, ref pixels_destination, bitmapSource.PixelHeight, bitmapSource.PixelWidth, sizeOfSinglePixelInNumberOfBytes, ref gaussiankernel);
                for (var j = 0; j < pixels_source.Length; j++)
                {
                    pixels_source[j] = pixels_destination[j];
                    pixels_destination[j] = 0;
                }
            }
                        
            sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString(), ref pixels_source, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY);                    

            Console.WriteLine(" Done blured image saved in file " + uri[constants.DESTINATION_FILE].ToString());
        }       
    }
}