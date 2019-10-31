/*  src/Constants.cs
    Written by, Sohail Qayum Malik [https://sohail.github.io] */
    
namespace cannyEdgeDetection
{
    public static class constants
    {
        /*
            The effect of Gaussian smoothing is to blur an image. The degree of smoothing is determined by the standard deviation.
            Larger standard deviation, of course, require larger convolution kernels in order to be accurately represented.
        */
        public const double GAUSSIAN_KERNEL_SMOOTHENING_STANDARD_DEVIATION = 1.4;

        public const byte SOURCE_FILE = 0;
        public const byte DESTINATION_FILE = 1; 

        public const byte DIRECTION_SOBEL_HORIZONTAL = 0;
        public const byte DIRECTION_SOBEL_VERTICAL = 1;   

        public const byte BLUE = 0;
        public const byte GREEN = 1;
        public const byte RED = 2;

        public const uint THRESHOLD = 0x00000D;   

        public const uint UPPER_THRESHOLD = 0xffffff;        
        public const uint LOWER_THRESHOLD = 0x000000;
        
        public const byte HORIZONTAL = 0;
        public const byte VERTICAL = 1;
        public const byte HIGH_HYSTERYSIS = 0Xff;
        public const byte LOW_HYSTERYSIS = 0x40; 

        public const bool JPEG = true;
        public const bool TIFF = false;    
    }
}