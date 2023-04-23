# **Mandelbrot Fractal**

## Author

- **[Flávio Santos](https://github.com/flaviojosefo) - a22100771**

## Summary

This project was built on C# with the main goal of generating a Mandelbrot Fractal,
outputting it as an image file (`.png` or `.bmp`).

Three different ways of generating fractals are implemented (Single Thread, Multithread and CUDA), 
with the last one serving as an extra, since the objective was the successful parallelization
of the algorithm through a *Divide and Conquer* type strategy using the ***CPU***.

**Disclaimer:** Due to the use of specific methods present on this project it can only run on the `Windows OS`.
Running it on different operating systems will result in the display of an error on the console.

## Instructions

### Menu Controls

- Use the `Up` and `Down` arrows on the keyboard to move the cursor;
- Press `Enter` to select a desired option.

### Fractal Generation Comparison

- Select `Start Comparison`;
- Select the first fractal type you wish to generate;
- Select the second fractal type you wish to generate;
- Await calculations, image creation and results display;
- Once done, press `Enter` to go back to the Main Menu.

### OpenGL Interactive App

- Select `Start OpenGL Fractal`;
- **Click** anywhere on the fractal with the `Left Mouse Button` and **drag** the `Mouse` to **move** the fractal;
- Use the `Mouse Wheel` to **zoom in / out** on the `Mouse`'s current position;
- **Hold** `Shift` and use the `Mouse Wheel` to **increase/decrease** the number of **iterations**;
- **Click** anywhere on the fractal with the `Right Mouse Button` to reset the fractal to its starting conditions.
- **Close** the `OpenGL` window to go back to the Main Menu on the `Console`.

### Changing Fractal Settings

- The following settings can be changed in `Fractal.cs`:
	- `MAX_ITERATIONS`: change the number of iterations;
	- `size`: control image dimensions;
	- `x0`/`x1` & `y0`/`y1`: control the fractal viewing coordinates.

- The following settings can be changed in `CudaFractal.cs`:
	- `glWidth` & `glHeight`: control OpenGL window and fractal texture dimensions.

## Discussion

As previously stated, Fractal generation is guaranteed through the use of **3** distinct solutions:

1. **Single Thread**  
 Implemented in `C#`, this option generates a Mandelbrot fractal with the specified conditions in a `serial` 
 fashion, writing the output to a `.png` file. It is the slowest of all options.

2. **Multithread**  
 Also implemented in `C#`, this option introduces a `parallelized` version of the algorithm, which also outputs to a
 `.png` file. It spreads workload between multiple threads, by dividing **image line creation** through the use of a 
 `Parallel.For` method on the `y` axis. The program will always use `CPU_Thread_Count - 1`.

3. **CUDA**  
 Developed as an extra to this project, the CUDA solution was built on `C`. It takes advantage of the `GPU`'s power
 in order to calculate each pixel's color `individually`, presenting a much higher processing speed than all the other
 methods. Due to the use of this specific language and technology, it outputs to a `.bmp` file and can only run on 
 `NVIDIA GPUs`.

Also in this project is an `OpenGL/CUDA` interop based solution. It takes advantage of the graphical properties of OpenGL
in order to display a **real-time**, interactable version of a fractal. It uses CUDA for the processing of a texture to 
be displayed on a `QUAD` right in front of an OpenGL (ortographic) `Camera`. 
Due to some limitations, this app does not support window resizing, although it does support any window size, as long 
as it has been declared previously. The dafault size is `1280 x 720`, which also corresponds to the texture's size.

Something extra to note is that this last CUDA implementation is slightly different from the `BMP` generation one, 
as mandelbrot iterations return a smoothed out color through the use of several `log2` functions. The color 
palette is also different and hardcoded on the `dll`.

## Results

Each fractal was generated `5x` using dimensions of `4096 x 4096` and `1000` iterations.
To analyze and compare more interesting results, the application was run on **2** separate computers.

### Desktop PC

The following results were obtained on the following hardware:

 - **CPU: `Intel Core i5-7600K CPU @ 3.80GHz`**
 - **GPU: `NVIDIA GeForce GTX 1080`**

#### Avg Time for Fractal Generation
   Serial C#  |  Parallel C#  |   CUDA C  |
:-----------: | :-----------: | :-------: |
 28 159.44 ms |  8 102.99 ms  | 383.63 ms |
 
 It comes as no surprise that `CUDA C` was the fastest, being ~`21x` faster than `Parallel C#` and over `73x` faster
 than `Serial C#`!
 
 The real surprise perhaps, is the relatively small difference between `Parallel C#` and `Serial C#`, which is only of 
 about `3.5x`. However, this behaviour can be explained by the fact that the `CPU` on this system only has `4 threads`,
 meaning that the program is only using `3` for the calculations' spread, which seems to relate closely to the number 
 previously obtained.

### Laptop

The following results were obtained on the following hardware:

 - **CPU: `Intel Core i7-8750H CPU @ 2.2GHz`**
 - **GPU: `NVIDIA GeForce GTX 1070`**

### Avg Time for Fractal Generation
   Serial C#  |  Parallel C#  |   CUDA C  |
:-----------: | :-----------: | :-------: |
 29 674.71 ms |  3 538.21 ms  | 455.27 ms |
 
 This system possesses a downgraded GPU in comparison to the previous one; however the result is not too far off, 
 and as expected it still does much better than the CPU (over `65x` when compared with `Serial C#`).
 
 The difference between CPU implementations however is now much bigger than previously, since we're now dealing with one
 containing `12 threads` (meaning the program uses `11`). This time, it's over `8.3x`, which not only proves that the
 parallelization is working correctly, but that the more threads we have, the faster the fractal will be generated,
 meaning that other computers with larger amounts of threads will have even better numbers that the ones showcased.
 
 Another interesting thing to note is the difference in speed between `Serial C#` on the `Desktop` and `Laptop`.
 Although small (Desktop was only `5%` faster), it can be explained by the fact that the Laptop's CPU has slighly
 worse single thread speed, which is compensated by the amount of cores/threads.

## Conclusions

This project was extremely interesting and gratifying.

It allowed me to explore and overcome different parallelization issues on C# (something that was hard due to the way 
the image file needs to be written) and introduced me to something extremely interesting (Mandelbrot Fractals), which
give off a certain excitement when we get things to work and are able to visualize them.

As an extra, this project provided my first contact with both `CUDA` and `OpenGL`, which makes things a lot more 
interesting since they become interactable and should come in handy for the next project.

The code from which the `CudaMandelbrot.dll` originated is located on `/extras/kernel.cu`.

## Thanks

I'd like to thank our teacher [José Rogado](https://github.com/jrogado) for all his invaluable input and teachings 
throughout the making of this project.
