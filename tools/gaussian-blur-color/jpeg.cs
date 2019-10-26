/*  src/Jpegs.cs
    Written by, q@softrobot.pk */

using System;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace cannyEdgeDetection
{
    public class jpeg
    {
        private Uri uri; 
        private JpegBitmapDecoder decoder;
        private int sizeOfSinglePixelInNumberOfBytes;
        
        public jpeg(ref Uri r)
        {
            Console.WriteLine("Processing " + r.ToString());

            uri = r;

            Image image = Image.FromFile(uri.ToString(), true);
            sizeOfSinglePixelInNumberOfBytes = Image.GetPixelFormatSize(image.PixelFormat) / 8;
            NewMethod(image);
        }

        private static void NewMethod(Image image)
        {
            image.Dispose();
        }

        public void process(uint npasses)
        {
            decoder = new JpegBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];  

            Console.WriteLine("DPI-Y(Vertical Resolution) = " + bitmapSource.DpiY + ", DPI-X(Horizontal Resolution) = " + bitmapSource.DpiX);
            Console.WriteLine("PixelHeight = " + bitmapSource.PixelHeight + ", PixelWidth = " + bitmapSource.PixelWidth);
            Console.WriteLine("PixelFormat in number of bytes(bitDepth) = " + sizeOfSinglePixelInNumberOfBytes);            

            byte[] pixels = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth * sizeOfSinglePixelInNumberOfBytes];
            byte[] pixels_convoluted = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth * sizeOfSinglePixelInNumberOfBytes]; 

            bitmapSource.CopyPixels(pixels, bitmapSource.PixelWidth * sizeOfSinglePixelInNumberOfBytes, 0);

            for (var i = 0; i < pixels.Length; i++)
            {
                pixels_convoluted[i] = pixels[i]; 
            }
                                
            double[,] gussiankernel = sundry.gussianKernel(Tuple.Create(5,5), 1.4);

            Console.WriteLine("Performing " + npasses.ToString() + " passes");

            for (var i = 0; i < npasses; i++)
            {  
                Console.Write(".");
                sundry.gussianBlur(ref pixels, ref pixels_convoluted, bitmapSource.PixelHeight, bitmapSource.PixelWidth, sizeOfSinglePixelInNumberOfBytes, ref gussiankernel);
                for (var j = 0; j < pixels.Length; j++)
                {
                    pixels[j] = pixels_convoluted[j];
                    pixels_convoluted[j] = 0;
                }
            }
                        
            save("parking"+npasses.ToString()+".jpg", ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY, bitmapSource.Format, bitmapSource.Palette, bitmapSource.PixelWidth * sizeOfSinglePixelInNumberOfBytes);

            Console.WriteLine("\ndone!");
        }

        private void save(String path, ref byte[] pixels_convoluted, int width, int height, double DpiX, double DpiY, System.Windows.Media.PixelFormat Format, BitmapPalette Palette, int stride)
        {
            BitmapSource image = BitmapSource.Create(width, height, DpiX, DpiY, Format, Palette, pixels_convoluted, stride);
            FileStream stream = new FileStream(path, FileMode.Create);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(stream);
        } 
    }
}