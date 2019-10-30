# Hough Transform
The **Hough Transform** works with edge detected images. This implementation of **Hough Transform**, tries to find the missing section/s of a line. 

To run ...

````bash
dotnet run assets/mandymoore.tiff assets/houghed.tiff
````

### The basics of line from a point

Our edge detected image is **mandymoore.tiff**, it has two pixels, they both lie on a same line/edge. Both of these pixels are not connected by an edge, meaning the whole section of edge which suppose to connect both of these points/pixels is missing. Our objective is to find that missing section between these two points.

Sice we've the coordinates of both of these points we can use **Two Point formula** to find the straight directly connecting both of these points. What I want to emphasize here that we can come up with a method to find missing parts of an edge and we'll be able to find out the missing parts of an edge sooner than later but, what will happen if there are more than few pairs of such points with missing sections and than each one of these points has the potentional to lie at the intersection of two or more such edges/lines. In **xy** space working Like this is may not be impossible but it is very difficult.

### The "mc" space

There are two such parameters with which a line can be completely defined, these parameters are slope(m) and intercept(c) of a line. A whole edge with many edge points in **xy** space is just a single point in **mc** space, **Hough Transform** assign weights to all of those edge points which are on the line/edge identified by parameters **m**, **c** and leave all other pixels out of this process of votes/weights.