# Hough Transform
The **Hough Transform** works with edge detected images. This implementation of **Hough Transform**, tries to find the missing section/s of a line. 
To run ...

````bash
dotnet run assets/mandymoore.tiff assets/houghed.tiff
````

### The basics of line from a point

Our edge detected image is **mandymoore.tiff**, it has two pixels which represent same edge. Both of these pixels are not connected by an edge, meaning the whole section of edge which suppose to connect both of these points/pixels is missing. Our objective is to find that missing section between these two points.

- Since we've the coordinates of two points of a missing section of line and we can use the two-point formula to find the equation of straight line.
 
What I want to emphasize here that we can come up with a method to find missing parts of an edge and we'll find out the missing parts on and edge sooner than later. What will happen if there are more than few pairs of such points with missing sections and than each one of these points has the potentional to lie at the intersection of two or more such edges. In **xy** space working Like this is may not be impossible but is very difficult.

There are two such parameters with which a line can be completely defined, these parameters are slope and intercept of a line. A whole edge with many edge points in **xy** space is just a single point in **mc**, **Hough Transform** assign weights to all of those edge points which are on the line identified by parameters **m**, **c** and leave all other pixels out of this process of voting.