/*  src/Program.cs
    Written by, Sohail Qayum Malik [https://sohail.github.io] */

using System;
using System.IO;

namespace cannyEdgeDetection
{ 
    class Program
    {
        static void Main(string[] args)
        {
            String[] arguments = Environment.GetCommandLineArgs();            
        						
			if (!(arguments.Length > 2))
			{
                Console.WriteLine("Example Usage : dotnet run <path of gaussian-blured image file> <name of destination sobel convoluted(horizontal-vertical), squared, summed file>");	  

                return;
            }

            if (!File.Exists(arguments[1]))
            {
                Console.WriteLine("The image file \""+arguments[1]+"\" does not exit at that specified path."); 

                return;  
            }

            tiff obj = new tiff(ref arguments);

            obj.process();             
        }           
    }
}

