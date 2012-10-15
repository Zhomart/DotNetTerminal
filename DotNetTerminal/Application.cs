using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTerminal
{
    class Application
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Panel leftPanel { get; set; }
        public Panel rightPanel { get; set; }

        public Application() 
        {
            Width = 80;
            Height = 25;

            Console.SetWindowSize(Width, Height);

            leftPanel = new Panel(this);
        }

        public ConsoleKeyInfo readKey() {
            return Console.ReadKey(false);
        }

        public void run()
        {
            draw();
            Console.SetCursorPosition(0, Height - 2);
            readKey();
        }

        void drawFooter()
        {
            Console.SetCursorPosition(0, Height - 1);
        }

        void draw()
        {
            leftPanel.draw();
        }
    }
}
