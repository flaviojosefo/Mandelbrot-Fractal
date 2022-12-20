using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Versioning;

namespace Mandelbrot_Fractal {

    [SupportedOSPlatform("windows")]
    internal sealed class SerialFractal : Fractal {

        // Set the fractal's name
        public override string Name => "Serial_Fractal.png";

        public SerialFractal() { }

        public override double Generate() {

            // Create a bitmap with the desired width and height
            using Bitmap bitmap = new(size, size);

            // Display an an initial informational message
            string initMessage =  $"Generating SERIAL fractal of {size}x{size} pixels with {MAX_ITERATIONS} iterations...\n\n";
            initMessage += $"Visible Coordinates\n    x: ({x0}; {x1})\n    y: ({y0}; {y1})\n";
            Console.WriteLine(initMessage);

            // Start the timer
            Stopwatch sw = Stopwatch.StartNew();

            // The main fractal generation algorithm
            for (int py = 0; py < size; py++) {

                double cY = y0 + py * pixelHeight;

                for (int px = 0; px < size; px++) {

                    double cX = x0 + px * pixelWidth;
                    int iter = IterateMandel(cX, cY, MAX_ITERATIONS);

                    // Set the color for the current pixel
                    int i = iter % colorMap.Length;
                    int c = iter == -1 ? 0 : colorMap[i];
                    Color color = Color.FromArgb(c);
                    color = Color.FromArgb(255, color.R, color.G, color.B);
                    bitmap.SetPixel(px, py, color);
                }
            }

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
