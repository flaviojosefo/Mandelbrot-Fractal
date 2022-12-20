using System.Runtime.Versioning;
using System.Runtime.InteropServices;

namespace Mandelbrot_Fractal {

    [SupportedOSPlatform("windows")]
    internal sealed class CudaFractal : Fractal {

        // OpenGL image dimensions (window and texture!)
        private static readonly int glWidth = 1280;
        private static readonly int glHeight = 720;

        // Used to verify OpenGL usability
        public bool UseOpenGL { get; set; }

        public override string Name => "Cuda_Fractal.bmp";

        public CudaFractal() { }

        public override double Generate() {

            // If the user wishes to use OpenGL do not generate a fractal image
            if (UseOpenGL)
                return 0d;

            // Display an initial informational message
            string initMessage = $"Generating fractal with GPU (CUDA) of {sizeX}x{sizeY} pixels with {MAX_ITERATIONS} iterations...\n\n";
            initMessage += $"Visible Coordinates\n    x: ({x0}; {x1})\n    y: ({y0}; {y1})\n";
            Console.WriteLine(initMessage);

            return GenerateFractalBMP(sizeX, sizeY, x0, x1, y0, y1, pixelWidth, pixelHeight, MAX_ITERATIONS, colorMap, colorMap.Length, Name);
        }

        public override void Display() {

            // Check if user wishes to use OpenGL for the display
            if (UseOpenGL) {

                // Call the OpenGL version of this program
                GenerateFractalOpenGL(glWidth, glHeight);

                Console.Write("\nPress any key to close...");

            } else {

                // Call the regular display function
                base.Display();
            }
        }

        [DllImport("CudaMandelbrot", EntryPoint = "generateFractalBMP")]
        private static extern double GenerateFractalBMP( int sizeX, int sizeY,
                                                         double x0, double x1,
                                                         double y0, double y1,
                                                         double pixelWidth, double pixelHeight,
                                                         int maxIters,
                                                         int[] colors, int colorsAmount,
                                                         string fileName);

        [DllImport("CudaMandelbrot", EntryPoint = "mandelbrotFractalOpenGL")]
        private static extern void GenerateFractalOpenGL(int width, int height);
    }
}
