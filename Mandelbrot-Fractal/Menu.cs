using System.Runtime.Versioning;

namespace Mandelbrot_Fractal {

    [SupportedOSPlatform("windows")]
    internal sealed class Menu {

        // Console display title
        private const string PROJECT_TITLE = "Mandelbrot Fractal";

        // The fractal options available to the user
        private readonly static string[] fractalOptions = {
            "C# Fractal (Single Thread)",
            "C# Fractal (Multithread)",
            "CUDA Fractal"
        };

        // The menus' titles
        private readonly string[] titles = {
            "----- MAIN MENU -----",
            "--- SELECT FIRST FRACTAL GENERATOR ---",
            "--- SELECT SECOND FRACTAL GENERATOR ---"
        };

        // The menus
        private readonly string[][] menus = {
            // Menu 0 (or main menu)
            new string[] { "Start Comparison",
                           "Start OpenGL Fractal" },
            // Menu 1
            fractalOptions,
            // Menu 2
            fractalOptions
        };

        // All possible fractals
        private readonly Fractal[] fractals = new Fractal[3];

        public Menu(int width, int height) {

            // These methods stop the Console from flickering when clearing
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            // Hide the console's cursor
            Console.CursorVisible = false;

            // Set a project title
            Console.Title = PROJECT_TITLE;

            // Allocate each type of fractal
            fractals[0] = new SerialFractal();
            fractals[1] = new ParallelFractal();
            fractals[2] = new CudaFractal();
        }                 

        // Displays the menus
        public void Display(int cursor = 0, int menu = 0, int f1 = -1) {

            // Clean the console
            Console.Clear();

            int nOptions = menus[menu].Length;

            Console.WriteLine(titles[menu] + '\n');

            // The actual cursor (top) coordinates
            int[] cursorCoord = new int[nOptions + 1];

            for (int i = 0; i < nOptions; i++) {

                cursorCoord[i] = Console.GetCursorPosition().Top;
                Console.WriteLine($"  {i + 1}." + menus[menu][i]);
            }

            cursorCoord[nOptions] = cursorCoord[nOptions - 1] + 2;

            // Write the last option (of the current menu)
            Console.WriteLine($"\n  {nOptions + 1}." + (menu == 0 ? "Exit" : "Back"));

            // Print the 'cursor'
            Console.SetCursorPosition(0, cursorCoord[cursor]);
            Console.Write('►');

            // Read user input
            switch (Console.ReadKey().Key) {

                case ConsoleKey.UpArrow:
                    // Move the cursor up
                    cursor = cursor > 0 ? cursor - 1 : nOptions;
                    break;

                case ConsoleKey.DownArrow:
                    // Move the cursor down
                    cursor = cursor < nOptions ? cursor + 1 : 0;
                    break;

                case ConsoleKey.Enter:
                    SelectOption(ref cursor, ref menu, ref f1);
                    break;
            }

            // Redraw the menu, if we moved the cursor
            Display(cursor, menu, f1);
        }

        // Interprets user selections
        private void SelectOption(ref int cursor, ref int menu, ref int f1) {

            // Check in which menu the user is currently
            switch (menu) {

                // Menu 0 = Main menu
                case 0:

                    // Check on which option the cursor is
                    if (cursor == 0) {

                        // Go to Menu 1
                        menu = 1;

                    } else if (cursor == 1) {

                        // Run OpenGL (and then return to Menu 0)
                        RunOpenGLApp();

                    } else {

                        // Exit the application
                        Environment.Exit(0);
                    }

                    break;

                // Menu 1 = 1st comparison selection
                case 1:

                    // Check on which option the cursor is
                    if (cursor < menus[menu].Length) {

                        // Go to Menu 2 and "save" the selected fractal method
                        menu = 2;
                        f1 = cursor;

                    } else {

                        // Go back to Menu 0 and "unselect" the fractal
                        menu = 0;
                        f1 = -1;
                    }

                    // Place cursor at the top
                    cursor = 0;

                    break;

                // Menu 2 = 2nd comparison selection
                case 2:

                    // Check on which option the cursor is
                    if (cursor < menus[menu].Length) {

                        // Compare fractal generation performances
                        CompareFractals(new int[2] {f1, cursor});

                        // Aftwerwards, go to Menu 0 and "unselect" the saved fractal
                        menu = 0;
                        f1 = -1;

                    } else {

                        // Go back to Menu 1
                        menu = 1;
                    }

                    // Place cursor at the top
                    cursor = 0;

                    break;
            }
        }

        // Generate 2 fractals and compare performance results
        private void CompareFractals(int[] indexes) {

            // Clean the Console
            Console.Clear();

            // Return if both fractals are the same
            if (indexes[0] == indexes[1]) {

                // Display message and await user input
                Console.WriteLine("Selected fractals are the same! Please choose different ones.");
                Console.WriteLine("\n► 1.Back");
                Console.ReadKey();

                return;
            }

            // Generation time for the 2 selected fractals
            double[] genTime = new double[indexes.Length];

            // Loop through both fractals
            for (int i = 0; i < indexes.Length; i++) {

                // If the selected fractal is a CUDAFractal we do not want use openGL
                if (indexes[i] == 2) {
                    ((CudaFractal)fractals[indexes[i]]).UseOpenGL = false;
                }

                Console.WriteLine($"------------ {fractalOptions[indexes[i]]} ------------\n");

                // Generate a fractal and return the ellapsed time in milliseconds
                genTime[i] = fractals[indexes[i]].Generate();

                Console.WriteLine($"\n----------------------------------------------------\n");
            }

            // Show which method was faster and the correspondant performance difference
            if (genTime[0] > genTime[1])
                Console.WriteLine($"{fractalOptions[indexes[1]]} was {genTime[0] / genTime[1]:0.00}x faster than {fractalOptions[indexes[0]]}!");
            else
                Console.WriteLine($"{fractalOptions[indexes[0]]} was {genTime[1] / genTime[0]:0.00}x faster than {fractalOptions[indexes[1]]}!");

            // Display the generated fractals
            fractals[indexes[0]].Display();
            fractals[indexes[1]].Display();

            // Write and wait for user input
            Console.WriteLine("\n► 1.Back");
            Console.ReadKey();
        }

        // Display the OpenGL window with the mandelbrot fractal
        private void RunOpenGLApp() {

            // Clean the Console
            Console.Clear();

            // "Inform" the fractal to run openGL
            ((CudaFractal)fractals[2]).UseOpenGL = true;

            // Run OpenGL
            ((CudaFractal)fractals[2]).Display();
        }
    }
}
