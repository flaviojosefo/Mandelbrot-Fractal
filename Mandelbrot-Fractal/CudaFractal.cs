using System.Runtime.Versioning;
using System.Runtime.InteropServices;

namespace Mandelbrot_Fractal {

    [SupportedOSPlatform("windows")]
    internal sealed class CudaFractal : Fractal {

        private readonly bool useGPU;
        private readonly bool useOPEN_GL;

        public override string Name => "Cuda_Fractal.bmp";

        public CudaFractal(bool useGPU = true, bool useOPEN_GL = false) {

            this.useGPU = useGPU;
            this.useOPEN_GL = useOPEN_GL;
        }

        public override void Generate() {

            // Display an an initial informational message
            string cpuOrGpu = useGPU ? "GPU (CUDA)" : "CPU";
            string initMessage = $"Generating fractal with {cpuOrGpu} of {sizeX}x{sizeY} pixels with {MAX_ITERATIONS} iterations\n\n";
            initMessage += $"Visible Coordinates\n    x: ({x0}; {x1})\n    y: ({y0}; {y1})\n";
            Console.WriteLine(initMessage);

            GenerateFractal(useGPU, sizeX, sizeY, x0, x1, y0, y1, pixelWidth, pixelHeight, MAX_ITERATIONS, colorMap, colorMap.Length, Name);
        }

        [DllImport("CudaMandelbrot", EntryPoint = "generate_fractal")]
        private static extern void GenerateFractal(bool useGpu,
                                                   int sizeX, int sizeY,
                                                   double x0, double x1,
                                                   double y0, double y1,
                                                   double pixelWidth, double pixelHeight,
                                                   int maxIters,
                                                   int[] colors, int colorsAmount,
                                                   string fileName);

        // ----- OPENGL + CUDA -----
        // https://www.informit.com/articles/article.aspx?p=2455391&seqNum=2
        // https://youtu.be/_41LCMFpsFs?t=1666
        // https://www.nvidia.com/content/GTC/documents/1055_GTC09.pdf
        // https://stackoverflow.com/questions/5107694/how-do-i-add-a-reference-to-an-unmanaged-c-project-called-by-a-c-sharp-project/5107759#comment16513325_5107759

        public override void Display() {

            if (useOPEN_GL) {

                // Open OPEN_GL window and display fractal with CUDA C

            } else {

                base.Display();
            }
        }
    }
}
