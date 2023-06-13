    /* src/Sundry.cs
    Written by, Sohail Qayum Malik */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace cannyEdgeDetection
{ 
    public static class sundry
    { 
        static void fill_edges_2(ref byte[] source, int width, int height, ref Point[] points)
        { 
            for (var i = 0; i < points.Length - 1; i++)
            {
                // Case of verical line. Divide by zero (points[i + 1].X - points[i].X)
                // Case of horizontal line.    

                var m = 1; 
                var c = 0;
                
                // To avoid divide by zero, hence the line is vertical
                if ((points[i + 1].X - points[i].X) != 0)
                {
                    m = (points[i + 1].Y - points[i].Y) / (points[i + 1].X - points[i].X);
                }

                // Instead line is horizontal, hence the (points[i + 1].Y - points[i].Y) is zero
                if (m == 0)
                {
                    for (var x = points[i].X; x < points[i + 1].X; x++)
                    {
                        source[points[i].Y*width + x] = constants.HIGH_HYSTERYSIS;
                    }
                }
                else // The line is not horizontal and nor it is vertical. Draw it
                {
                    c = points[i].Y - m*points[i].X;

                    for (var y = points[i].Y + 1; y < points[i + 1].Y; y++)
                    {
                        var x = (y - c)/m;
                        Console.WriteLine("x = "+x.ToString()+", y = "+y.ToString());

                        source[y*width + x] = constants.HIGH_HYSTERYSIS;
                    }
                }

                // The line is not horizontal(points[i + 1].Y - points[i].Y is zero) 
                /*if (m != 0)
                {                                                                       
                    c = points[i].Y - m*points[i].X;

                    for (var y = points[i].Y + 1; y < points[i + 1].Y; y++)
                    {
                        var x = (y - c)/m;
                        Console.WriteLine("x = "+x.ToString()+", y = "+y.ToString());

                        source[y*width + x] = constants.HIGH_HYSTERYSIS;
                    }
                }
                else // The line is horizontal
                {
                    
                }*/

                // y = mx + c --> y - c = mx --> (y - c)/m = x
                /*for (var y = points[i].Y + 1; y < points[i + 1].Y; y++)
                {
                    var x = (y - c)/m;
                    Console.WriteLine("x = "+x.ToString()+", y = "+y.ToString());

                    source[y*width + x] = constants.HIGH_HYSTERYSIS;
                }*/

                // y = mx + c 
                /*if (points[i].X < points[i + 1].X)
                {
                    var x = points[i].X; 

                    for (; x < points[i + 1].X;)
                    {

                        var y = m*x + c;

                        x = x + 1;

                        source[y*width + x] = constants.HIGH_HYSTERYSIS;
                    }                   
                }
                else
                {
                    var x = points[i + 1].X;

                    for (; x < points[i].X;)
                    {
                        var y = m*x + c;

                        x = x + 1;

                        source[y*width + x] = constants.HIGH_HYSTERYSIS;
                    }
                }*/                 
            }
        }

        static Point[] fill_edges_1(ref byte[] source, ref byte[,] accumulator, int width, int height, int m, int n, int callerDistance, int callerAngle)
        {
            int j = 1;

            Point[] points = new Point[constants.VOTES];

            points[0] = new Point(n, m);
             
            n  = n + 1;
            
            while (m < height)
            {                
                while (n < width)
                {  
                    // Vote for only edge pixels                  
                    if (source[m*width + n] != constants.HIGH_HYSTERYSIS)
                    {
                        n = n + 1;    
                        continue;                           
                    } 

                    for (var i = -90; i < (90 + 1); i++)
                    {
                        /*if (source[m*width + n] != constants.HIGH_HYSTERYSIS)
                        {
                            continue;                           
                        }*/

                        var p =  Math.Round(Math.Cos((Math.PI/180)*i)*n + Math.Sin((Math.PI/180)*i)*m);
                        var angle = (int)(90 + i); // m
                        var distance = (int)((accumulator.GetLength(0)/2) + p); // c
                       
                        if (distance == callerDistance && angle == callerAngle)
                        {
                            //Console.WriteLine("Hi...."); 
                            //Console.WriteLine("Yes****"+" -> "+(i - 90).ToString()+" m = "+m.ToString()+", n = "+n.ToString());

                            points[j] = new Point(n, m); 

                            j = j + 1;
                        }

                         /*if (angle == ((int)(90 + i)) && distance == ((int)((accumulator.GetLength(0)/2) + Math.Round(Math.Cos((Math.PI/180)*i)*n + Math.Sin((Math.PI/180)*i)*m))))
                         {
                             Console.WriteLine("Yes");
                             Console.WriteLine(accumulator[((int)((accumulator.GetLength(0)/2) + Math.Round(Math.Cos((Math.PI/180)*i)*n + Math.Sin((Math.PI/180)*i)*m))),  ((int)(90 + i))]);
                         }*/

                         //var p =  Math.Round(Math.Cos((Math.PI/180)*i)*n + Math.Sin((Math.PI/180)*i)*m);

                         //var angle = (int)(90 + i); // m
                         //var distance = (int)((accumulator.GetLength(0)/2) + p); // c     
                     }    

                     n = n + 1;
                }

                n = 0;
                m = m + 1;
            }

            return points;            
        }
        public static Point[] fill_edges(ref byte[] source, ref byte[,] accumulator, int height, int width)
        {
            Point[] points = new Point[constants.VOTES];
            /*for (var m = 0; m < accumulator.GetLength(0); m++)
            {
                for (var n = 0; n < accumulator.GetLength(1); n++)
                {
                    if (accumulator[m, n] > 0)
                    {
                        if (accumulator[m, n] == constants.VOTES)
                        {
                            Console.Write("-----> ");
                        }

                        Console.WriteLine(accumulator[m, n].ToString());
                    }
                }
            }*/

            //save2tiff("canada.tiff", ref source, width, height, 72, 72);
                            
            for (var m = 0; m < height; m++)
            {
                for (var n = 0; n < width; n++)
                { 
                    // Vote for only edge pixels
                    if (source[m*width + n] != constants.HIGH_HYSTERYSIS)
                    {
                        continue;    
                    }
                    
                    // Yes we got an edge pixel

                    for (var i = -90; i < (90 + 1); i++)
                    {
                        // Prepare accumulator array indices
                        var p =  Math.Round(Math.Cos((Math.PI/180)*i)*n + Math.Sin((Math.PI/180)*i)*m);

                        var angle = (int)(90 + i); // m
                        var distance = (int)((accumulator.GetLength(0)/2) + p); // c

                        // constants,VOTES pass through that newly found edge pixel
                        //if (accumulator[((int)((accumulator.GetLength(0)/2) + Math.Round(Math.Cos((Math.PI/180)*i)*n + Math.Sin((Math.PI/180)*i)*m))), ((int)(90 + i))] == constants.VOTES)
                        if (accumulator[distance, angle] == constants.VOTES)
                        {    
                             // We know how many lines pass through this point. Make it as such that it does'nt get eveluated again and again                         
                             accumulator[distance, angle] = 0;

                             //Console.WriteLine(distance.ToString()+" -- "+angle.ToString());

                             // Find all lines that pass through this newly found point
                             // This method will go throuhg each edge pixel, reclaculate the indices into the accumulator array and match these against the [m = angle, c = distance]. If they are same then we've found another point of the same. Record the coordinates of this pixel and all other such pixels in an array of points and return that array    
                             points = fill_edges_1(ref source, ref accumulator, width, height, m, n, distance, angle);

                             // Draw lines. "points" have the coordinates of atleast two pixels, join them together 
                             fill_edges_2(ref source, width, height, ref points);

                             // Draw lines in between all these coordinates held by array "points"                         
                        }
                    }        
                }               
            }

            /*for (var m = 0; m < height; m++)
            {
                for (var n = 0; n < width; n++)
                {
                    if (source[m*width + n] != constants.HIGH_HYSTERYSIS)
                    {
                        continue;
                    }

                    for (var i = -90; i < (90 + 1); i++)
                    {
                        var p =  Math.Round(Math.Cos((Math.PI/180)*i)*n + Math.Sin((Math.PI/180)*i)*m);

                        var angle = (int)(90 + i); // m
                        var distance = (int)((accumulator.GetLength(0)/2) + p); // c

                        if (accumulator[distance, angle] >= constants.VOTES)
                        {
                            Console.WriteLine("\n\nCalled....");
                            fill_edges_1(ref source, ref accumulator, width, height, m, n, distance, angle, constants.VOTES);
                        }
                    }    
                }
            }*/

            /*for (var m = 0; m < height; m++)
            {
                for (var n = 0; n < width; n++)
                {
                    if (source[m*width + n] != constants.HIGH_HYSTERYSIS)
                    {
                        continue;
                    }

                    for (var i = -90; i < (90 + 1); i++)
                    {
                        var p =  Math.Round(Math.Cos((Math.PI/180)*i)*n + Math.Sin((Math.PI/180)*i)*m);

                        var angle = (int)(90 + i); // m
                        var distance = (int)((accumulator.GetLength(0)/2) + p); // c

                        if (accumulator[distance, angle] > 1)
                        {
                            accumulator[distance, angle] -= 1;

                            angle -= 90; // m
                            distance -= (accumulator.GetLength(0)/2); // c

                            Console.WriteLine(angle.ToString() + " --- " + distance.ToString());
                            
                            // y = mx + c;

                            for (var q = m; q < height; q++)
                            {
                                for (var r = n; r < width; r++)
                                {
                                    if (((q*width + r)+angle + distance) < height*width)
                                    {
                                    source[(q*width + r)+angle + distance] = constants.HIGH_HYSTERYSIS - 150;
                                    //Console.WriteLine("DID IT");
                                    }
                                }
                            }
                        }
                    }
                }
            }*/

            return points;
        }

        public static void hough_transform(ref byte[] source, ref byte[] destination, ref byte[,] accumulator, int height, int width)
        {
            //Console.WriteLine(accumulator.GetLength(0));

            /*     
                To get the size of each dimension of 2 diemension arrays
                - accumulator.GentLength(0), number of rows
                - accumulator.GentLength(1), number of columns
            */
            /*
                For each edge pixel these many votes are casted
                from -90 to 0 and then from 1 to 90, for each edge 181 votes get casted 
             */             
            for (var m = 0; m < height; m++)
            {
                for (var n = 0; n < width; n++)
                {
                    // Vote for only edge pixels
                    if (source[m*width + n] != constants.HIGH_HYSTERYSIS)
                    {
                        continue;
                    }
                                        
                    // ((int)(90 + i)).ToString(), from 0(90 + (-90)) to 180(90 + 90)
                    // ((int)(accumulator.GentLength(0) + p)).ToString(), from 0 to accumulator.GentLength(0)*2
                    for (var i = -90; i < (90 + 1); i++) // Angle theta i, then n is x and m is y
                    {
                       var p =  Math.Round(Math.Cos((Math.PI/180)*i)*n + Math.Sin((Math.PI/180)*i)*m);

                       var angle = (int)(90 + i); // Index has to be positive, it is the column index
                       /*
                            sundry.get_diagnal_length(bitmapSource.PixelHeight, bitmapSource.PixelWidth)*2 is the size of accumulator.GetLength(0)
                            p could be any where -1*accumulator.GetLength(0) to accumulator.GetLength(0). distance is a row index it has to be positive 
                            that is you first make it half for -1*accumulator.GetLength(0) p value which would be 0. It is same as above statement where
                            we go the positive index value for column but there 90 was fixed but here the dimensions of the accumulator array changing 
                        */
                       var distance = (int)((accumulator.GetLength(0)/2) + p); // 

                       /* 
                            Both of these two statement blocks should not get executed, if they do then next statement will cause some sort of index out 
                            of memory block type runtime error/exeception
                         */     
                       if (angle > (accumulator.GetLength(1)))
                       {
                           Console.WriteLine("angle = "+angle.ToString());
                       }     

                       if (distance > (accumulator.GetLength(0)))
                       {
                           Console.WriteLine("distance = "+distance.ToString());
                       }

                       accumulator[distance, angle] += 1;

                       //Console.Write(((int)(90 + i)).ToString()+", ");

                       //Console.Write(((int)(accumulator.GetLength(0)/2) + p).ToString()+",("+p.ToString()+"), ");

                       //Console.Write(p.ToString()+", ");
                    }
                }
            }                        
        }
        
        // Length of diagnal of square and rectangle. "h" is for height and "w" is for width
        // h*sqrt(2) or w*sqrt(2)
        // sqrt(h*h + w*w)
        public static int get_diagnal_length(int height, int width)
        {
            if (height == width) // a square d = s * √2
            {
                return (int)Math.Round(height*Math.Sqrt(2)); // 
            }

            return (int)Math.Round(Math.Sqrt(height*height  + width*width)); // Rectangle  d = √(H^2 + W^2)                      
        }
/* ******************************** */
        public static void thresholding_with_hysterysis(ref double[] sum, ref double[] direction, ref byte[] result, int height, int width, int sizeOfSinglePixelInNumberOfBytes, bool format = constants.TIFF)
        {
            bool found;
            double slope, magnitude;

            do
            {                  
                //Console.WriteLine("Hi!");

                found = false;

                for (var m = 0; m < height; m++)
                {
                    for (var n = 0; n < width; n++)
                    {
                        if ((format == constants.JPEG && result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] == constants.HIGH_HYSTERYSIS && result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] == constants.HIGH_HYSTERYSIS && result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.RED] == constants.HIGH_HYSTERYSIS) || (format == constants.TIFF && result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] == constants.HIGH_HYSTERYSIS))
                        {   
                            // Change so that we know we've already done hysterysis for this pixel                     
                            result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = constants.LOW_HYSTERYSIS;
                            if (format == constants.JPEG)
                            {
                                result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = constants.LOW_HYSTERYSIS;
                                result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.RED] = constants.LOW_HYSTERYSIS;
                            }

                            slope = direction[m*width + n];

                            if (slope>22.5 && slope<=67.5)
                            {
                                // Diagonally to the right(upwards)
                                if (m > 1 && n < (width - 2))
                                { 
                                    if ((format == constants.JPEG && result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS && result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] != constants.LOW_HYSTERYSIS && result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.RED] != constants.LOW_HYSTERYSIS) || (format == constants.TIFF && result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS))
                                    {
                                        slope = direction[(m - 1)*width + (n + 1)];
                                        magnitude = sum[(m - 1)*width + (n + 1)];
                            
                                        //slope = direction[(m - 1)*width + (n + 1)];
                                        //magnitude = sum[(m - 1)*width + (n + 1)];
                                    
                                        if (magnitude > constants.LOWER_THRESHOLD && (magnitude > sum[(m - 2)*width + n] && magnitude > sum[m*width + (n + 2)]) && (slope>22.5 && slope<=67.5))
                                        {
                                            result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = constants.HIGH_HYSTERYSIS;
                                            if (format == constants.JPEG)
                                            {
                                                result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = constants.HIGH_HYSTERYSIS;
                                                result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.RED] = constants.HIGH_HYSTERYSIS; 
                                            }

                                            found = true;
                                            //Console.WriteLine("1");
                                        }
                                        /*else
                                        {
                                            //Console.WriteLine("1-flase");
                                            found = false;
                                        }*/
                                    }                                    
                                }

                                // Diagonally to the left(downwards)
                                if (m < (height - 2) && n > 1)
                                {
                                    if ((format == constants.JPEG && result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS && result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] != constants.LOW_HYSTERYSIS && result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.RED] != constants.LOW_HYSTERYSIS) || (format == constants.TIFF && result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS))
                                    {                        
                                        slope = direction[(m + 1)*width + (n - 1)];
                                        magnitude = sum[(m + 1)*width + (n - 1)];

                                        if (magnitude > constants.LOWER_THRESHOLD && (magnitude > sum[m*width + (n - 2)] && magnitude > sum[(m + 2)*width + n]) && (slope>22.5 && slope<=67.5))
                                        {                                            
                                            result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = constants.HIGH_HYSTERYSIS;
                                            if (format == constants.JPEG)
                                            {
                                                result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = constants.HIGH_HYSTERYSIS;
                                                result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.RED] = constants.HIGH_HYSTERYSIS;
                                            }

                                            found = true;
                                            //Console.WriteLine("2");
                                        } 
                                        /*else
                                        {
                                            found = false;
                                            //Console.WriteLine("2-false");
                                        }*/
                                    }
                                }
                            }                     
                            else if(slope>67.5 && slope<=112.5)
                            {
                                // To the left
                                if ((m > 0 && m < (height - 1)) && n > 0)
                                {                                   
                                    if ((format == constants.JPEG && result[m*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS && result[m*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] != constants.LOW_HYSTERYSIS && result[m*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.RED] != constants.LOW_HYSTERYSIS) || (format == constants.TIFF && result[m*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS))
                                    {                                    
                                        slope = direction[m*width + (n - 1)];
                                        magnitude = sum[m*width + (n - 1)];
                                    
                                        if (magnitude > constants.LOWER_THRESHOLD && (magnitude > sum[(m - 1)*width + (n - 1)] && magnitude > sum[(m + 1)*width + (n - 1)]) && (slope>67.5 && slope<=112.5))
                                        {
                                            result[m*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = constants.HIGH_HYSTERYSIS;
                                            if (format == constants.JPEG)
                                            {
                                                result[m*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = constants.HIGH_HYSTERYSIS;
                                                result[m*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.RED] = constants.HIGH_HYSTERYSIS; 
                                            }

                                            found = true;
                                            //Console.WriteLine("3");
                                        }
                                        /*else
                                        {
                                            found = false;
                                            //Console.WriteLine("3-false");
                                        }*/
                                    }     
                                }

                                // To the right
                                if ((m > 0 && m < (height - 1)) && n < (width - 1))
                                {
                                    if ((format == constants.JPEG && result[m*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS && result[m*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] != constants.LOW_HYSTERYSIS && result[m*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.RED] != constants.LOW_HYSTERYSIS) || (format == constants.TIFF && result[m*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS))
                                    {                                    
                                        slope = direction[m*width + (n + 1)];
                                        magnitude = sum[m*width + (n + 1)];

                                        if (magnitude > constants.LOWER_THRESHOLD && (magnitude > sum[(m - 1)*width + (n + 1)] && magnitude > sum[(m + 1)*width + (n + 1)]) && (slope>67.5 && slope<=112.5))
                                        {
                                            result[m*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = constants.HIGH_HYSTERYSIS;
                                            if (format == constants.JPEG)
                                            {
                                                result[m*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = constants.HIGH_HYSTERYSIS;
                                                result[m*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.RED] = constants.HIGH_HYSTERYSIS;
                                            } 

                                            found = true;
                                            //Console.WriteLine("4");
                                        }
                                        /*else
                                        {
                                            found = false;
                                            //Console.WriteLine("4-false");
                                        }*/   
                                    }
                                }
                            }
                            else if (slope>112.5 && slope<=157.5)
                            {                                                               
                                // Diagonally at top
                                if (m > 0 && n > 0)
                                {   
                                    if ((format == constants.JPEG && result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS && result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] != constants.LOW_HYSTERYSIS && result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.RED] != constants.LOW_HYSTERYSIS) || (format == constants.TIFF && result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS))
                                    {                                     
                                        slope = direction[(m - 1)*width + (n - 1)];    
                                        magnitude = sum[(m - 1)*width + (n - 1)];

                                        if (magnitude > constants.LOWER_THRESHOLD && (magnitude > sum[m*width + (n - 2)] && magnitude > sum[(m - 2)*width + n]) && (slope>112.5 && slope<=157.5))
                                        {                                       
                                            result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = constants.HIGH_HYSTERYSIS;
                                            if (format == constants.JPEG)
                                            {
                                                result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = constants.HIGH_HYSTERYSIS;
                                                result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + (n-1)*sizeOfSinglePixelInNumberOfBytes + constants.RED] = constants.HIGH_HYSTERYSIS;
                                            }

                                            found = true;
                                            //Console.WriteLine("5");
                                        }
                                        /*else
                                        {
                                            found = false;
                                            //Console.WriteLine("5-false");
                                        }*/
                                    }
                                }

                                //Diagonally at the bottom 
                                if (m < (height - 1) && n < (width - 1))
                                {   
                                    if ((format == constants.JPEG && result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS && result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] != constants.LOW_HYSTERYSIS && result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.RED] != constants.LOW_HYSTERYSIS) || (format == constants.TIFF && result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS))
                                    {                                                                                                  
                                        slope = direction[(m + 1)*width + (n + 1)];
                                        magnitude = sum[(m + 1)*width + (n + 1)];

                                        if (magnitude > constants.LOWER_THRESHOLD && (slope>112.5 && slope<=157.5) && (magnitude > sum[(m + 2)*width + n] && magnitude > sum[m*width + (n + 2)]))
                                        {
                                            result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = constants.HIGH_HYSTERYSIS;
                                            if (format == constants.JPEG)
                                            {
                                                result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = constants.HIGH_HYSTERYSIS;
                                                result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + (n+1)*sizeOfSinglePixelInNumberOfBytes + constants.RED] = constants.HIGH_HYSTERYSIS;
                                            }

                                            found = true;
                                            //Console.WriteLine("6");
                                        }
                                        /*else
                                        {
                                            found = false;
                                            //Console.WriteLine("6-false");
                                        }*/
                                    }
                                }                                
                            }
                            else 
                            {
                                // Top pixel on the edge
                                if (m > 0 && n < (width - 1))
                                {
                                    //found = false;
                                    
                                    if ((format == constants.JPEG && result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS && result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] != constants.LOW_HYSTERYSIS && result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.RED] != constants.LOW_HYSTERYSIS) || (format == constants.TIFF && result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS))
                                    {                                    
                                        slope = direction[(m - 1)*width + n];
                                        magnitude = sum[(m - 1)*width + n];

                                        if (magnitude > constants.LOWER_THRESHOLD && magnitude > sum[(m - 1)*width + (n + 1)] && magnitude > sum[(m - 1)*width + (n - 1)])
                                        {
                                            if (slope <= 22.5 || slope > 157.5)
                                            {
                                                result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = constants.HIGH_HYSTERYSIS;
                                                if (format == constants.JPEG)
                                                {
                                                    result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = constants.HIGH_HYSTERYSIS;
                                                    result[(m-1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.RED] = constants.HIGH_HYSTERYSIS;
                                                }

                                                found = true;
                                                //Console.WriteLine("7");
                                            }                                            
                                        }                                        
                                    }                                                                        
                                }

                                // Bottom pixel on the edge 
                                if (m < (height - 1) && (n > 0 && n < (width - 1)))
                                {
                                    //found = false;

                                    if ((format == constants.JPEG && result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS && result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] != constants.LOW_HYSTERYSIS && result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.RED] != constants.LOW_HYSTERYSIS) || (format == constants.TIFF && result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] != constants.LOW_HYSTERYSIS))
                                    {
                                        slope = direction[(m + 1)*width + n];
                                        magnitude = sum[(m + 1)*width + n];

                                        if (magnitude > constants.LOWER_THRESHOLD && magnitude > sum[(m + 1)*width + (n + 1)] && magnitude > sum[(m + 1)*width + (n - 1)])
                                        {
                                            if (slope <= 22.5 || slope > 157.5)
                                            {
                                                result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = constants.HIGH_HYSTERYSIS;
                                                if (format == constants.JPEG)
                                                {
                                                    result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = constants.HIGH_HYSTERYSIS;
                                                    result[(m+1)*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.RED] = constants.HIGH_HYSTERYSIS;
                                                }

                                                found = true;
                                                //Console.WriteLine("8");
                                            }
                                        }                                        
                                    }                                      
                                }                           
                            }                            
                        }                                        
                    }
                }
            }
            while (found);

            for (var m = 0; m < height; m++)
            {
                for (var n = 0; n < width; n++)
                {
                    // Change so that we know we've already done hysterysis for this pixel                     
                    if ((format == constants.JPEG && result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] == constants.LOW_HYSTERYSIS && result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] == constants.LOW_HYSTERYSIS && result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.RED] == constants.LOW_HYSTERYSIS) || (format == constants.TIFF && result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] == constants.LOW_HYSTERYSIS))
                    {
                        // Change so that we know we've already done hysterysis for this pixel                     
                        result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = constants.HIGH_HYSTERYSIS;
                        if (format == constants.JPEG)
                        {
                            result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = constants.HIGH_HYSTERYSIS;
                            result[m*width*sizeOfSinglePixelInNumberOfBytes + n*sizeOfSinglePixelInNumberOfBytes + constants.RED] = constants.HIGH_HYSTERYSIS;
                        }
                    }
                }
            }
        }

/* ******************************** */

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
                     /*else
                     {
                         result[y*width*sizeOfSinglePixelInNumberOfBytes + x*sizeOfSinglePixelInNumberOfBytes + constants.BLUE] = 0;
                         if (format == constants.JPEG)
                         {
                            result[y*width*sizeOfSinglePixelInNumberOfBytes + x*sizeOfSinglePixelInNumberOfBytes + constants.GREEN] = 0xff;
                            result[y*width*sizeOfSinglePixelInNumberOfBytes + x*sizeOfSinglePixelInNumberOfBytes + constants.RED] = 0Xff;
                         }                       
                     }*/
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
      