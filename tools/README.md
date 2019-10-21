# computer-vision

This folder contains tools to help try reduce the size of images, remove noise from them or anything and every thing which can speedup the process of image processing.   

### color2grey
Converts a JPEG image to grayscale 8 bit TIFF image.

To run ...
````bash
dotnet run assets/parking.jpg assets/parking.tiff
````

### gaussian-blur-grey
It reduces noise and detail

To run ...
 ````bash
dotnet run <path to TIFF file> [n] 
````

[n] optional number of passes

