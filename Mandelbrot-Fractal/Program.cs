namespace Mandelbrot_Fractal {

    internal sealed class Program {

        static void Main(string[] args) {

            // Verify if the current OS is Windows
            if (OperatingSystem.IsWindows()) {

                // Create and display a fractal
                Fractal f = new CudaFractal(true);
                f.Generate();
                f.Display();

            } else {

                // Print a message if the user is not on Windows
                Console.WriteLine("This application is only supported on Windows OS");
            }

            // Read user input, so prevent the console from closing automatically
            Console.ReadKey(true);
        }
    }
}