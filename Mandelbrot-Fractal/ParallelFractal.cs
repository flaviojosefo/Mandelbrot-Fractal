using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Drawing.Imaging;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;

namespace Mandelbrot_Fractal {

    [SupportedOSPlatform("windows")]
    internal sealed class ParallelFractal : Fractal {

        public override string Name => "Parallel_Fractal.png";

        public ParallelFractal() { }

        public override double Generate() {

            // Create a bitmap with the desired width and height
            Bitmap bitmap = new(sizeX, sizeY);
            Rectangle rect = new(Point.Empty, bitmap.Size);

            // Display an an initial informational message
            string initMessage = $"Generating PARALLEL fractal of {sizeX}x{sizeY} pixels with {MAX_ITERATIONS} iterations...\n\n";
            initMessage += $"Visible Coordinates\n    x: ({x0}; {x1})\n    y: ({y0}; {y1})\n";
            Console.WriteLine(initMessage);

            // Set Bitmap data to be used in parallel
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            int bpp = (bitmap.PixelFormat == PixelFormat.Format32bppArgb) ? 4 : 3;
            int size = bmpData.Stride * bmpData.Height;
            byte[] data = new byte[size];
            Marshal.Copy(bmpData.Scan0, data, 0, size);

            // Set max degree of parallelism to core count - 1
            ParallelOptions options = new();
            int maxCore = Environment.ProcessorCount - 1;
            options.MaxDegreeOfParallelism = maxCore > 0 ? maxCore : 1;

            // Start the timer
            Stopwatch sw = Stopwatch.StartNew();

            // The main fractal generation algorithm
            Parallel.For(0, sizeY, options, py => {

                double cY = y0 + py * pixelHeight;

                for (int px = 0; px < sizeX; px++) {

                    int index = py * bmpData.Stride + px * bpp;

                    double cX = x0 + px * pixelWidth;
                    int iter = IterateMandel(cX, cY, MAX_ITERATIONS);

                    // Set the color for the current pixel
                    int i = iter % colorMap.Length;
                    int c = iter == -1 ? 0 : colorMap[i];
                    Color color = Color.FromArgb(c);
                    color = Color.FromArgb(255, color.R, color.G, color.B);

                    data[index] = color.B;
                    data[index + 1] = color.G;
                    data[index + 2] = color.R;
                    data[index + 3] = 255;
                }
            });

            // Copy the data to the bitmap
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bitmap.UnlockBits(bmpData);

            // Display a message indicating the time it took to generate the fractal
            double genTime = sw.Elapsed.TotalMilliseconds;
            string genTimeString = genTime.ToString("0.000", CultureInfo.GetCultureInfo("en-US"));
            Console.WriteLine($"Fractal generated in {genTimeString} ms");

            // Stop the timer
            sw.Stop();

            // Save the bitmap as a 'png' file
            bitmap.Save($"{Name}");

            // Return the required generation time
            return genTime;
        }
    }
}
