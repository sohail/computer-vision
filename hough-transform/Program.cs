/*  src/Program.cs
    Written by, Sohail Qayum Malik */

// CNN(Convolutional Neural Network)

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
                Console.WriteLine("Example Usage : dotnet run <path of edge detected file> <name of destination file>");	  

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

