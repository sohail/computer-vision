# computer-vision
For a computer an image is a 2D matrix of numbers(for color image, each R G and B channel has its own 2D matrix). Each number is just a brightness value. These brightness values work/group together to form spatial features/structures.

An image is considered a mathematical function of x, y and outcome of this consideration is, that under the pretext of **Image Processing** different mathematical operations(such as **derivation**) can be performed on an image to extract spatial features/structures and classify or assign a label to an image.

In **Image Processing**, taking a derivative of an image is called building **Convolutional Neural Network** of an image. The **CNN** is the process of dividing whole image in patches/sections and replacing a whole patch/section with a single intensity/brightness/strength value in the hidden/underlying layer of set of pixels called **neurons** and then connecting each **neuron** to the subsequnt hidden/underlying layers(taking the second and third derivative or differentiate the derivative even further more).

An image can be divided in sections or pathces under the assumption that pixels which are close together are part of same feature or structure. In order to identify several fetures we need several kernels/filter(kernel/filter is a small matrix of weights).

> **Excerpts from the talk on [Learning from Machines](https://www.youtube.com/watch?v=02cFoST40K8&t=747s) by, [Ashi Krishnan](https://ashi.io)**.. 

The job of an image classifier is to reshape it's input which is square of pixels into output which is a probability distribution. It perfom this reshaping through series of convolution filters. 

-  Each neuron in a convolutional layer has a receptive field. This is a small patch of the previous layer from which it takes its input. 
- Each convolutional layer applies a filter. Specifically, it applies an image kernel. 
- A kernel is matrix of numbers, where each number represents the weight of the corresponding input neuron. 
- Each pixel in each neuron's receptive field is multiplied by this weight, and then we sum them all to produce this neuron's value.
- The same filter is applied for every neuron across a layer. And the values in that filter are learned during training. 
 
**Training**
We feed the classifier a labeled image —something where we know what's in it— it outputs predictions, we math to figure out how wrong that prediction was and then we math again, nudging each and every single filter in the direction that would have produced a better result the term for that is: gradient descent.
> **Excerpts end here**.

