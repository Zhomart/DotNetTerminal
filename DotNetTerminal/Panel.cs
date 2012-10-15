using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTerminal
{
    class Panel
    {
        Application app;

        public int X { get; set; }
        public int Y { get; set; }

        ConsoleColor backgroundColor = ConsoleColor.Blue;
        ConsoleColor borderColor = ConsoleColor.Cyan;

        public int Width { get { return app.Width / 2;  } }
        public int Height { get { return app.Height - 2; } } // 1 for command line, 1 for tips

        public Panel(Application app)
        {
            this.app = app;
            X = 0;
            Y = 0;
        }

        public void draw()
        {
            fillBackground();

            drawBorders();

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.SetCursorPosition(Width / 4 - 2, 1);
            Console.Write("Name");
            Console.SetCursorPosition(3*Width / 4 - 2, 1);
            Console.Write("Name");
        }

        void fillBackground()
        {
            Console.BackgroundColor = backgroundColor;
            for (int i = 0; i < Height; ++i)
            {
                Console.SetCursorPosition(0, i);
                for (int j = 0; j < Width; ++j) Console.Write(" ");
            }
        }

        void drawBorders()
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = borderColor;

            Console.SetCursorPosition(0, 0);
            Console.Write("╔");
            Console.SetCursorPosition(0, Height - 1);
            Console.Write("╚");
            for (int i = 1; i < Width - 1; ++i)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("═");
                Console.SetCursorPosition(i, Height - 1);
                Console.Write("═");
            }
            Console.SetCursorPosition(Width - 1, 0);
            Console.Write("╗");
            Console.SetCursorPosition(Width - 1, Height - 1);
            Console.Write("╝");
            for (int i = 1; i < Height - 1; ++i)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("║");
                Console.SetCursorPosition(Width - 1, i);
                Console.Write("║");
            }

            for (int i = 1; i < Height - 3; ++i)
            {
                Console.SetCursorPosition(Width / 2, i);
                Console.Write("│");
            }
            
            for (int i = 1; i < Width - 1; ++i)
            {
                Console.SetCursorPosition(i, Height - 3);
                Console.Write("─");
            }


        }

    }
}
