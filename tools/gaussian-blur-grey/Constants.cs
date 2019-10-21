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
    }
}