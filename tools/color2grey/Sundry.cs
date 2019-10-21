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
      