/*  src/Tiff.cs
    Written by, Sohail Qayum Malik */

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
            Point[] points = new Point[constants.VERTICAL];
            BitmapSource bitmapSource = (new TiffBitmapDecoder(uri[constants.SOURCE_FILE], BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad)).Frames[0];
                        
            Console.WriteLine("Processing " + uri[constants.SOURCE_FILE].ToString() + " & " + uri[constants.DESTINATION_FILE].ToString());
            Console.WriteLine("*\n*");
            Console.WriteLine("DPI-Y(Vertical Resolution) = " + bitmapSource.DpiY + ", DPI-X(Horizontal Resolution) = " + bitmapSource.DpiX);
            Console.WriteLine("PixelHeight = " + bitmapSource.PixelHeight + ", PixelWidth = " + bitmapSource.PixelWidth);
            Console.WriteLine("PixelFormat in number of bytes(bitDepth) = " + sizeOfSinglePixelInNumberOfBytes);            
            
            byte[] source = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth];
            byte[] destination = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth];

            byte[,] accumulator = new byte[sundry.get_diagnal_length(bitmapSource.PixelHeight, bitmapSource.PixelWidth)*2, 90*2 + 1];

            /*for (var m = 0; m < accumulator.GetLength(0); m++)
            {
                for (var n = 0; n < accumulator.GetLength(1); n++)
                {
                    accumulator[m, n] = 0;
                }
            }*/
                                  
            bitmapSource.CopyPixels(source, bitmapSource.PixelWidth * sizeOfSinglePixelInNumberOfBytes, 0);

            sundry.hough_transform(ref source, ref destination, ref accumulator, bitmapSource.PixelHeight, bitmapSource.PixelWidth);

            //sundry.fill_edges(ref source, ref accumulator, bitmapSource.PixelHeight, bitmapSource.PixelWidth);

            byte[] foo = new byte[accumulator.GetLength(0)*accumulator.GetLength(1)];

            //int i = 1;

            for (var m = 0; m < accumulator.GetLength(0); m++)
            {
                for (var n = 0; n < accumulator.GetLength(1); n++)
                {
                    foo[m*accumulator.GetLength(1) + n] = accumulator[m, n];

                    /*if (accumulator[m, n] > 0)
                    {
                        // -90 to 0 and then from 1 to 90, for one point
                        // -90 to 0 and then from 1 to 90, for the other point
                        Console.Write((i).ToString()+") "+accumulator[m, n].ToString()+" ");
                        i++;

                        if (accumulator[m, n] >= constants.VOTES)
                        {
                            Console.WriteLine("**************************************** FOUND **********************");
                        }                        
                    }*/                   
                }
            }
        
            sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString(), ref foo, accumulator.GetLength(1) /*bitmapSource.PixelWidth*/, accumulator.GetLength(0) /*bitmapSource.PixelHeight*/, bitmapSource.DpiX, bitmapSource.DpiY);

            points = sundry.fill_edges(ref source, ref accumulator, bitmapSource.PixelHeight, bitmapSource.PixelWidth);

            Console.WriteLine(points[0].X.ToString()+" -- "+points[0].Y.ToString());
            Console.WriteLine(points[1].X.ToString()+" -- "+points[1].Y.ToString());

            //sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString(), ref foo, accumulator.GetLength(0) /*bitmapSource.PixelHeight*/, accumulator.GetLength(1) /*bitmapSource.PixelWidth*/, bitmapSource.DpiX, bitmapSource.DpiY);
            
            // For documentation purposes
            /*try
            {
                Console.WriteLine(uri[constants.SOURCE_FILE].GetComponents(UriComponents.AbsoluteUri, UriFormat.Unescaped));
            }
            catch(System.InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }*/

            sundry.save2tiff(uri[constants.DESTINATION_FILE].ToString()+"-soni.tiff", ref source, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY);                                                
            Console.WriteLine("\n**\n*\nDone doing " + uri[constants.SOURCE_FILE].ToString());
        }       
    }
}