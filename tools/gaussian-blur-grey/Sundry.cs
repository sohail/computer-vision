/*  src/Sundry.cs
    Written by, Sohail Qayum Malik [https://sohail.github.io] */

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace cannyEdgeDetection
{ 
    public static class sundry
    {   
        /* 
            When detecting edges it is important to catch as many edges as possible. Image noise creates false edges so it is important that we filter out the image noise before the actual process of edge detection(convolution).
            Smoothening the image(or filtering out the image noise) is done using the Gaussian Kernel
         */
        public static double[,] gaussianKernel(Tuple<int, int> size, double st)
        {
            double sum = 0;
            double[,] kernel = new double[size.Item1, size.Item2]; 

            int mX = size.Item1/2;
            int mY = size.Item2/2;    
            
            for ( int y = 0; y < size.Item2; y++ )
            {
                for ( int x = 0; x < size.Item1; x++)
                {
                    kernel[x, y] = Math.Exp( (-0.5) * (Math.Pow((x - mX)/st, 2) + Math.Pow((y - mY)/st, 2)) )  / (2 * Math.PI * Math.Pow(st, 2));        

                    sum += kernel[x, y];
                }
            }

            // Normalize
            for ( int y = 0; y < size.Item2; y++ )
            {
                for ( int x = 0; x < size.Item1; x++)
                {
                    kernel[x, y] /= sum;     
                }
            }

            return kernel;
        }

        public static void convolution(ref byte[] pixels, ref byte[] pixels_convoluted, int height, int width, int sizeOfSinglePixelInNumberOfBytes, ref double [,] kernel)
        {
            int h = kernel.GetLength(0)/2, w = kernel.GetLength(1)/2;

            for (var m = 0; m < (height - kernel.GetLength(0) - 1); m++)
            {
                for (var n = 0; n < (width - kernel.GetLength(1) - 1); n++)
                {                    
                    double sum_pixel_convoluted = 0;
                                                        
                    for (var i = 0; i < kernel.GetLength(0); i++)
                    {
                        for (var j = 0; j < kernel.GetLength(1); j++)
                        {
                            double pixel_convoluted = pixels[(m + i)*width + (n + j)] *  kernel[i, j];
                            sum_pixel_convoluted += pixel_convoluted;                            
                        }
                    }

                    if (sum_pixel_convoluted <= 0xff)
                    {
                        pixels_convoluted[(m + h)*width + (n + w)] = (byte)sum_pixel_convoluted;
                    }                                                                                                    
                }
            }
        }     

        public static void save2tiff(String path, ref byte[] pixels, int width, int height, double DpiX, double DpiY)
		{
            PixelFormat myPixelFormat = new PixelFormat();
            myPixelFormat = PixelFormats.Gray8;

            List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
            colors.Add(System.Windows.Media.Colors.Gray);

            BitmapPalette myPalette = new BitmapPalette(colors);
                     
			BitmapSource image = BitmapSource.Create(width, height, DpiX, DpiY, myPixelFormat, myPalette, pixels, width);
            FileStream stream = new FileStream(path, FileMode.Create);	
			TiffBitmapEncoder encoder = new TiffBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));
		    encoder.Save(stream);
		}

        public static bool color2grey(ref byte[] pixels_source, ref byte[] pixels_destination, int sizeOfSinglePixelInNumberOfBytes)
        {            
            if ((pixels_source.Length/pixels_destination.Length) != sizeOfSinglePixelInNumberOfBytes)
            {
                return false;
            }

            for (var m = 0; m < pixels_source.Length; m+=sizeOfSinglePixelInNumberOfBytes)
            {
                long sum = 0;

                for (var n = 0; n < sizeOfSinglePixelInNumberOfBytes; n++)
                {
                    sum += pixels_source[m+n]; 
                }

                long quotient = sum / sizeOfSinglePixelInNumberOfBytes;

                if (quotient > 255)
                {
                    quotient = 0;
                }

                pixels_destination[m/sizeOfSinglePixelInNumberOfBytes] = (byte)quotient;                                                
            }

            return true;
        }      
    }
}
      