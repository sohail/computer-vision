/*  src/Program.cs
    Written by, q@softrobot.pk */

using System;
using System.IO;

namespace cannyEdgeDetection
{ 
    class Program
    {
        static void Main(string[] args)
        {
            String[] arguments = Environment.GetCommandLineArgs();            
        						
			if (arguments.Length > 1 && File.Exists(arguments[1]))
			{	
                Uri uri = new Uri(arguments[1], UriKind.RelativeOrAbsolute);            
                
                jpeg obj = new jpeg(ref uri);

                if (arguments.Length > 2)
                {
                    obj.process(Convert.ToUInt32(arguments[2], 10));
                }
                else
                {
                    obj.process(1);
                }
            } 
            else
            {
                Console.WriteLine("Example Usage : dotnet run <path to image file>");
                Console.WriteLine("Either the image file does not exit or you did not provide the name of the image file to process.");
            }
        }
    }
}
