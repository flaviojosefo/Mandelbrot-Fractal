using System.Runtime.Versioning;
using System.Runtime.InteropServices;

namespace Mandelbrot_Fractal {

    [SupportedOSPlatform("windows")]
    internal sealed class CudaFractal : Fractal {

        private readonly bool useGPU;
        private readonly bool useOpenGL;

        public override string Name => "Cuda_Fractal.bmp";

        public CudaFractal(bool useGPU = true, bool useOpenGL = false) {

            this.useGPU = useGPU;
            this.useOpenGL = useOpenGL;
        }

        public override void Generate() {

            // If the user wishes to use OpenGL do not generate a fractal image
            if (useOpenGL)
                return;

            // Display an initial informational message
            string cpuOrGpu = useGPU ? "GPU (CUDA)" : "CPU";
            string initMessage = $"Generating fractal with {cpuOrGpu} of {sizeX}x{sizeY} pixels with {MAX_ITERATIONS} iterations\n\n";
            initMessage += $"Visible Coordinates\n    x: ({x0}; {x1})\n    y: ({y0}; {y1})\n";
            Console.WriteLine(initMessage);

            GenerateFractalBMP(useGPU, sizeX, sizeY, x0, x1, y0, y1, pixelWidth, pixelHeight, MAX_ITERATIONS, colorMap, colorMap.Length, Name);
        }

        public override void Display() {

            // Check if user wishes to use OpenGL for the display
            if (useOpenGL) {

                // Call the OpenGL version of this program
                GenerateFractalOpenGL(1280, 720);

                Console.Write("\nPress any key to close...");

            } else {

                // Call the regular display function
                base.Display();
            }
        }

        [DllImport("CudaMandelbrot", EntryPoint = "generateFractalBMP")]
        private static extern void GenerateFractalBMP(bool useGpu,
                                                   int sizeX, int sizeY,
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
