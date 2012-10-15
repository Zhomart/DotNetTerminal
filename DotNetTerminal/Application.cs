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
            readKey();
        }

        void draw()
        {
            leftPanel.draw();
        }
    }
}
