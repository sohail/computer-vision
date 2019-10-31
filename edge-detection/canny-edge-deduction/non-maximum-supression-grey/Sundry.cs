    /* src/Sundry.cs
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
        // 1rad × 180/π = 57.296° Where π = 3.14.
        // 180° has 3.14 radians in degrees or 3.14*57.296° = 180°
        public static void non_maximum_supression(ref double[] sum, ref double[] direction, ref byte[] result, int height, int width, int sizeOfSinglePixelInNumberOfBytes, bool format = constants.TIFF)        
        {
            for (var y = 0; y < height; y++) 
            { 
                for (var x = 0; x < width; x++)
                {   
                    bool found = true;

                    double currentDirection = Math.Atan(direction[y*width + x])*(180/Math.PI);

                     while(currentDirection < 0)
                     { 
                         currentDirection+=180;
                     }

                     direction[y*width + x] = currentDirection; 

                     if (sum[y*width + x] < constants.THRESHOLD) 
                     {
                         continue;
                     } 

                     if(currentDirection>22.5 && currentDirection<=67.5)
                     {
                         //if(pos.y<workImg.rows-1 && pos.x<workImg.cols-1 && *itMag<=sum.at<float>(pos.y+1, pos.x+1))                         
                         if (((x<(width - 1)) && (y<(height - 1))) && (sum[y*width + x] <= sum[(y + 1)*width + (x + 1)]))
                         {
                             found = false;
                         }
                         //if(pos.y>0 && pos.x>0 && *itMag<=sum.at<float>(pos.y-1, pos.x-1))
                         if (((x>0) && (y>0)) && (sum[y*width + x] <= sum[(y - 1)*width + (x - 1)]))
                         {
                            found = false;
                         }
                     } 
                     else if(currentDirection>67.5 && currentDirection<=112.5)
                     {
                         //if(pos.y<workImg.rows-1 && *itMag<=sum.at<float>(pos.y+1, pos.x))
                         if ((y<(height - 1)) && (sum[y*width + x] <= sum[(y + 1)*width + x]))
                         {
                             found = false;
                         }
                         //if(pos.y>0 && *itMag<=sum.at<float>(pos.y-1, pos.x))
                        if ((y>0) && (sum[y*width + x] <= sum[(y - 1)*width + x]))
                        {
                            found = false;
                        }     

                     }                      
                     else if(currentDirection>112.5 && currentDirection<=157.5)
                     {       
                        // if(pos.y>0 && pos.x<workImg.cols-1 && *itMag<=sum.at<float>(pos.y-1, pos.x+1))               
                        if (((x<(width - 1)) && (y > 0)) && (sum[y*width + x] <= sum[(y - 1)*width + (x + 1)]))
                        {
                           found = false;
                        }
                        // if(pos.y<workImg.rows-1 && pos.x>0 && *itMag<=sum.at<float>(pos.y+1, pos.x-1))
                        if (((x>0) && (y<(height - 1))) && (sum[y*width + x] <= sum[(y + 1)*width + (x - 1)]))
                        {
                            found = false;
                        }
                     }
                     else
                     {
                         // if(pos.x<workImg.cols-1 && *itMag<=sum.at<float>(pos.y, pos.x+1))
                         if ((x<(width - 1)) && (sum[y*width + x] <= sum[y*width + (x + 1)]))
                         {
                             found = false;
                         } 
                         // if(pos.x>0 && *itMag<=sum.at<float>(pos.y, pos.x-1))
                         if ((x>0) && (sum[y*width + x] <= sum[y*width + (x - 1)]))
                         {
                             found = false;
                         }     
                     }


                     if (found)
                     {   
                         result[y*width*sizeOfSinglePixelInNumberOfBytes + x*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = 0xff;
                         if (format == constants.JPEG)
                         {
                            result[y*width*sizeOfSinglePixelInNumberOfBytes + x*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = 0xff;
                            result[y*width*sizeOfSinglePixelInNumberOfBytes + x*sizeOfSinglePixelInNumberOfBytes + constants.RED] = 0Xff;
                         }                       
                     }
                     else
                     {
                         result[y*width*sizeOfSinglePixelInNumberOfBytes + x*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = 0;
                         if (format == constants.JPEG)
                         {
                            result[y*width*sizeOfSinglePixelInNumberOfBytes + x*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = 0xff;
                            result[y*width*sizeOfSinglePixelInNumberOfBytes + x*sizeOfSinglePixelInNumberOfBytes + constants.RED] = 0Xff;
                         }                       
                     }
                }
            }
        } 

        public static void double2byte(ref double[] source, ref byte[] destination)
        {
            if (source.Length != destination.Length)
            {
                return;
            }

            for (var i = 0; i < destination.Length; i++)
            {
                if (source[i] < 256)
                {
                    destination[i] = (byte)source[i];
                }
                else
                {
                    destination[i] = 0;
                }
            }

        }
        public static void sqrt(ref double[] pixels, ref double[] pixels_sqrt, int sizeOfSinglePixelInNumberOfBytes)
        {
            if (pixels.Length != pixels_sqrt.Length)
            {
                return;
            }

            for (var i = 0; i < pixels_sqrt.Length; i++)
            {
                pixels_sqrt[i] = Math.Sqrt(pixels[i]);
            }
        }        
        public static void sum(ref double[] pixels_augend, ref double[] pixels_addend, ref double[] pixels_sumed, int sizeOfSinglePixelInNumberOfBytes)
        {
            if ((pixels_augend.Length + pixels_addend.Length)/2 != pixels_sumed.Length)
            {
                return;
            }

            for (var i = 0; i < pixels_sumed.Length; i++)
            {
                pixels_sumed[i] = (pixels_augend[i] + pixels_addend[i]);
            }
        }
        public static void square(ref double[] pixels, ref double[] pixels_squared, int sizeOfSinglePixelInNumberOfBytes)
        {            
            if (pixels.Length != pixels_squared.Length)
            {
                return;
            }

            for (var i = 0; i < pixels_squared.Length; i++) 
            { 
                pixels_squared[i] = (pixels[i]*pixels[i]);                                
            }
        }
        public static void divide(ref double[] pixels_sobel_horizontal, ref double[] pixels_sobel_vertical, ref double[] pixels_slope, int sizeOfSinglePixelInNumberOfBytes)
        {
            //double dividend = 0, divisor = 0, quotient = 0;

            if ((pixels_sobel_horizontal.Length + pixels_sobel_vertical.Length)/2 != pixels_slope.Length)
            {
                return;
            } 

            for (var i = 0; i < pixels_slope.Length; i++)
            {
                if (pixels_sobel_vertical[i] != 0)
                {
                    pixels_slope[i] = (pixels_sobel_horizontal[i] / pixels_sobel_vertical[i]);
                }
                else
                {
                    pixels_slope[i] = 0;
                }
            }           
        }
        public static void sobel(ref byte[] pixels, ref double[] pixels_convoluted, int pixelHeight, int pixelWidth, int sizeOfSinglePixelInNumberOfBytes, byte direction = constants.DIRECTION_SOBEL_HORIZONTAL)
        {
            double[,] horizontal = { {-1, -2, -1}, 
                                  {0, 0, 0}, 
                                  {1, 2, 1}     }; 

            double[,] vertical = { {-1, 0, 1}, 
                                {-2, 0, 2}, 
                                {-1, 0, 1} };

             if (direction == constants.DIRECTION_SOBEL_HORIZONTAL)
             {
                 convolution(ref pixels, ref pixels_convoluted, pixelHeight, pixelWidth, sizeOfSinglePixelInNumberOfBytes, ref horizontal);
             }   
             else
             {
                 convolution(ref pixels, ref pixels_convoluted, pixelHeight, pixelWidth, sizeOfSinglePixelInNumberOfBytes, ref vertical);
             }                  
        }

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
        public static void convolution(ref byte[] pixels, ref double[] pixels_convoluted, int height, int width, int sizeOfSinglePixelInNumberOfBytes, ref double [,] kernel)
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
                        pixels_convoluted[(m + h)*width + (n + w)] = sum_pixel_convoluted;
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
      