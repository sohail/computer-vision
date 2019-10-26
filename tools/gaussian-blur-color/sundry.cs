/*  src/Sundry.cs
    Written by, q@softrobot.pk */

using System;

namespace cannyEdgeDetection
{ 
    public static class sundry
    {                
        public static double[,] gussianKernel(Tuple<int, int> size, double st)
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

        public static void gussianBlur(ref byte[] pixels, ref byte[] pixels_convoluted, int pixelHeight, int pixelWidth, int sizeOfSinglePixelInNumberOfBytes, ref double[,] kernel)
        {
            convolution(ref pixels, ref pixels_convoluted, pixelHeight, pixelWidth, sizeOfSinglePixelInNumberOfBytes, ref kernel);
        }  

        public static void sobel()
        {
            int[,] horizontal = { {-1, -2, -1}, 
                                  {0, 0, 0}, 
                                  {1, 2, 1}     }; 

            int[,] vertical = { {-1, 0, 1}, 
                                {-2, 0, 2}, 
                                {-1, 0, 1} };  

        }

        public static void convolution(ref byte[] pixels, ref byte[] pixels_convoluted, int height, int width, int sizeOfSinglePixelInNumberOfBytes, ref double [,] kernel)
        {
            int h = kernel.GetLength(0)/2, w = kernel.GetLength(1)/2;

            for (var m = 0; m < (height - kernel.GetLength(0) - 1); m++)
            {
                for (var n = 0; n < (width - kernel.GetLength(1) - 1); n++)
                {                    
                    double sum_blue = 0, sum_green = 0, sum_red = 0;
                                    
                    for (var i = 0; i < kernel.GetLength(0); i++)
                    {
                        for (var j = 0; j < kernel.GetLength(1); j++)
                        {
                            double pixel_blue  = pixels[(m + i)*width*sizeOfSinglePixelInNumberOfBytes + (n + j)*sizeOfSinglePixelInNumberOfBytes + 0] * kernel[i, j];
                            double pixel_green = pixels[(m + i)*width*sizeOfSinglePixelInNumberOfBytes + (n + j)*sizeOfSinglePixelInNumberOfBytes + 1] * kernel[i, j];
                            double pixel_red   = pixels[(m + i)*width*sizeOfSinglePixelInNumberOfBytes + (n + j)*sizeOfSinglePixelInNumberOfBytes + 2] * kernel[i, j];

                            sum_blue  += pixel_blue;
                            sum_green += pixel_green;
                            sum_red   += pixel_red;                                                        
                        }
                    }
                                        
                    if (sum_blue <= 0xff) 
                    {
                        pixels_convoluted[(m + h)*width*sizeOfSinglePixelInNumberOfBytes + (n + w)*sizeOfSinglePixelInNumberOfBytes + 0] = (byte)sum_blue; // blue                    
                    }
                    else 
                    {
                        pixels_convoluted[(m + h)*width*sizeOfSinglePixelInNumberOfBytes + (n + w)*sizeOfSinglePixelInNumberOfBytes + 0] = 0; // blue   
                    }

                    if (sum_green <= 0xff)
                    {                     
                        pixels_convoluted[(m + h)*width*sizeOfSinglePixelInNumberOfBytes + (n + w)*sizeOfSinglePixelInNumberOfBytes + 1] = (byte)sum_green; // green
                    }
                    else
                    {
                        pixels_convoluted[(m + h)*width*sizeOfSinglePixelInNumberOfBytes + (n + w)*sizeOfSinglePixelInNumberOfBytes + 1] = 0; // green
                    }

                    if (sum_red <= 0xff)
                    {                     
                        pixels_convoluted[(m + h)*width*sizeOfSinglePixelInNumberOfBytes + (n + w)*sizeOfSinglePixelInNumberOfBytes + 2] = (byte)sum_red; // red
                    }
                    else
                    {
                        pixels_convoluted[(m + h)*width*sizeOfSinglePixelInNumberOfBytes + (n + w)*sizeOfSinglePixelInNumberOfBytes + 2] = 0; // red      
                    }                                        
                }
            }             
        }      
    }
}
      