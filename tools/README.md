# computer-vision

This folder contains tools to help try reduce the size of images, remove noise from them or anything and every thing which can speedup the process of image processing.   

### color2grey
Converts a JPEG image to grayscale 8 bit TIFF image.

To run ...
````bash
dotnet run assets/parking.jpg assets/parking.tiff
````

### gaussian-blur-grey
It reduces noise or detail and thus smoothen a grey-scale image

To run ...
 ````bash
dotnet run <path to TIFF file> [n] 
````

[n] optional number of passes. Each additional pass will increase the total time this program takes to blur an image


### gaussian-blur-color
It reduces noise or detail and thus smoothen a color image

To run ...
 ````bash
dotnet run <path to JPEG file> [n] 
````

[n] optional number of passes. Each additional pass will increase the total time this program takes to blur an image


