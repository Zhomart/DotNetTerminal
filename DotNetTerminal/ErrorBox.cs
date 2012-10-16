using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTerminal
{
    class ErrorBox : Box
    {
        public bool Running { get; set; }

        public string Text { get; set; }

        public ErrorBox(Application app)
            : base(app, "Error")
        {

            Width = 60;
            Height = 6;
            X = app.Width / 2 - Width / 2;
            Y = app.Height / 2 - Height / 2;

            backgroundColor = ConsoleColor.Gray;
            borderColor = ConsoleColor.Black;
        }

        public void run(string text)
        {
            Console.CursorVisible = false;
            
            Text = text;

            Running = true;

            draw();
            while (Running)
            {
                var key_info = app.readKey();
                var key = key_info.Key;

                if (key == ConsoleKey.Escape) break;

                switch (key)
                {
                    case ConsoleKey.Escape:
                        Running = false;
                        break;
                    case ConsoleKey.Enter:
                        Running = false;
                        break;
                }

            }
            app.DrawPanels();
            Console.CursorVisible = true;
        }

        void drawLine(string text, int top)
        {
            SetCursorPosition(Width / 2 - text.Length / 2, top + 2);
            Console.Write(text);
        }

        void drawText()
        {
            lock (app.locker)
            {
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = borderColor;

                drawLine(Text, 0);
            }
        }

        override public void draw()
        {
            base.draw();

            drawText();
        }
    }
}
