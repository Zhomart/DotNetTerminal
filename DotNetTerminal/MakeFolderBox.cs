using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTerminal
{
    class MakeFolderBox : Box
    {
        public bool Running { get; set; }

        public MakeFolderBox(Application app)
            : base(app, "Make folder")
        {
            Width = app.Width - 2;
            Height = 8;
            X = app.Width / 2 - Width / 2;
            Y = app.Height / 2 - Height / 2;

            backgroundColor = ConsoleColor.Gray;
            borderColor = ConsoleColor.Black;
        }

        public void run()
        {
            Console.CursorVisible = false;

            Running = true;

            draw();
            while (Running)
            {
                var key_info = app.readKey();
                var key = key_info.Key;

                if (key == ConsoleKey.Escape) break;

                switch (key)
                {
                    case ConsoleKey.Enter:
                        Running = false;
                        action(key_info);
                        break;
                }

            }
            app.DrawPanels();
            Console.CursorVisible = true;
        }

        void action(ConsoleKeyInfo info)
        {
        }

        void drawText()
        { 
            lock(app.locker){
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = borderColor;

                var text = "Create the folder";
                SetCursorPosition(3, 2);
                Console.Write(text);

                for (int i = 2; i < Width - 2; ++i)
                {
                    SetCursorPosition(i, 4);
                    Console.Write("─");
                }
            }
        }

        void drawInputBox()
        {
            lock (app.locker)
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.ForegroundColor = ConsoleColor.Black;

                for (int i = 3; i < Width - 3; ++i)
                {
                    SetCursorPosition(i, 3);
                    Console.Write(" ");
                }
            }
        }

        void drawHints()
        {
            var text = "[Enter] - make folder, [Escape] - cancel";
            lock (app.locker)
            {
                SetCursorPosition(Width / 2 - text.Length / 2, Height - 3);

                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = borderColor;
                Console.Write(text);
            }
        }

        override public void draw()
        {
            base.draw();

            drawText();

            drawHints();

            drawInputBox();
        }
    }
}
