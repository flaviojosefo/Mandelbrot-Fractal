using System.Diagnostics;

namespace Mandelbrot_Fractal {

    internal abstract class Fractal {

        // Number of iterations
        protected const int MAX_ITERATIONS = 1000;

        // Image dimensions (Multiples of 2 are preferred!)
        protected static readonly int sizeX = 1024;
        protected static readonly int sizeY = 1024;

        // Visible zone coordinates
        protected static readonly double x0 = -2.0d;
        protected static readonly double x1 = 0.5d;
        protected static readonly double y0 = -1.25d;
        protected static readonly double y1 = 1.25d;

        // Pixel size in (x,y) coordinates measure
        protected readonly double pixelWidth = (x1 - x0) / sizeX;
        protected readonly double pixelHeight = (y1 - y0) / sizeY;

        // 64 colors colormap in int format
        protected readonly int[] colorMap = {
            0xff0000, 0xf60800, 0xee1000, 0xe61800,
            0xde2000, 0xd52900, 0xcd3100, 0xc53900,
            0xbd4100, 0xb44a00, 0xac5200, 0xa45a00,
            0x9c6200, 0x946a00, 0x8b7300, 0x837b00,
            0x7b8300, 0x738b00, 0x6a9400, 0x629c00,
            0x5aa400, 0x52ac00, 0x4ab400, 0x41bd00,
            0x39c500, 0x31cd00, 0x29d500, 0x20de00,
            0x18e600, 0x10ee00, 0x08f600, 0x00ff00,
            0x00ff00, 0x00f608, 0x00ee10, 0x00e618,
            0x00de20, 0x00d529, 0x00cd31, 0x00c539,
            0x00bd41, 0x00b44a, 0x00ac52, 0x00a45a,
            0x009c62, 0x00946a, 0x008b73, 0x00837b,
            0x007b83, 0x00738b, 0x006a94, 0x00629c,
            0x005aa4, 0x0052ac, 0x004ab4, 0x0041bd,
            0x0039c5, 0x0031cd, 0x0029d5, 0x0020de,
            0x0018e6, 0x0010ee, 0x0008f6, 0x0000ff
        };

        // The name used on file creation + extension
        public abstract string Name { get; }

        // The method to generate a fractal
        public abstract void Generate();

        // The method to display a fractal
        public virtual void Display() {

            try {
                // Open the generated fractal image
                Process.Start(new ProcessStartInfo($@"{Name}") { UseShellExecute = true });
            } catch (Exception e) {
                // Display a message if the file could not be found
                Console.WriteLine(e.Message);
            }
        }

        // Calculation of complex numbers
        protected static int IterateMandel(double cReal, double cImag, int maxIter) {

            // Init variables
            int n = 0;
            double real = cReal;
            double imag = cImag;

            // Main iteration
            while(n < maxIter) {

                double real2 = real * real;
                double imag2 = imag * imag;
                imag = 2 * real * imag + cImag;
                real = real2 - imag2 + cReal;

                // Diverged after n iterations -> Color pixel
                if (real2 + imag2 > 4.0d)
                    return n;

                // Increment iteration
                n++;
            }

            // Converged after max iterations -> Black pixel
            return -1;
        }
    }
}
