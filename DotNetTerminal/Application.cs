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

        Panel currentPanel;

        public Application() 
        {
            Width = 80;
            Height = 25;

            Console.SetWindowSize(Width, Height);

            leftPanel = new Panel(this, "C:\\");
            rightPanel = new Panel(this, "C:\\soft");

            rightPanel.X = Width / 2;
        }

        public ConsoleKeyInfo readKey() {
            return Console.ReadKey(true);
        }

        public void run()
        {
            draw();

            currentPanel = leftPanel;
            currentPanel.Focused = true;

            currentPanel.updateSelected(0);
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(0, Height - 2);
                ConsoleKeyInfo key_info = readKey();
                ConsoleKey key = key_info.Key;

                if (key == ConsoleKey.Enter)
                {
                    log("                                     ");
                }

                if (key == ConsoleKey.F10) break;
                if (key == ConsoleKey.Escape) break; // Test only

                if (key == ConsoleKey.F1 && (key_info.Modifiers == ConsoleModifiers.Alt || key_info.Modifiers == null))
                    leftPanel.SelectDrive();
                if (key == ConsoleKey.F2 && (key_info.Modifiers == ConsoleModifiers.Alt || key_info.Modifiers == null))
                    rightPanel.SelectDrive();

                if (key == ConsoleKey.F1 && key_info.Modifiers == ConsoleModifiers.Control)
                {
                    leftPanel.Visible = !leftPanel.Visible;
                    if (currentPanel == leftPanel && !leftPanel.Visible && rightPanel.Visible)
                    {
                        currentPanel.Focused = false;
                        currentPanel = rightPanel;
                        currentPanel.Focused = true;
                        currentPanel.updateSelected();
                    }
                    leftPanel.draw();
                }

                if (key == ConsoleKey.F2 && key_info.Modifiers == ConsoleModifiers.Control)
                {
                    rightPanel.Visible = !rightPanel.Visible;
                    if (currentPanel == rightPanel && !rightPanel.Visible && leftPanel.Visible)
                    {
                        currentPanel.Focused = false;
                        currentPanel = leftPanel;
                        currentPanel.Focused = true;
                        currentPanel.updateSelected();
                    }
                    else currentPanel = null;
                    rightPanel.draw();
                }

                if (null == currentPanel)
                    currentPanel = leftPanel.Visible ? leftPanel : (rightPanel.Visible ? rightPanel : null);

                if (!leftPanel.Visible && !rightPanel.Visible) currentPanel = null;
                if (null == currentPanel) continue;

                if (key == ConsoleKey.F3) log("not implemented");
                if (key == ConsoleKey.F4) log("not implemented");
                if (key == ConsoleKey.F5) log("not implemented");
                if (key == ConsoleKey.F6) log("not implemented");
                if (key == ConsoleKey.F7) log("not implemented");
                if (key == ConsoleKey.F8) log("not implemented");
                if (key == ConsoleKey.F9) log("not implemented");

                if (key == ConsoleKey.UpArrow) currentPanel.selectPrevFile();
                if (key == ConsoleKey.DownArrow) currentPanel.selectNextFile();
                if (key == ConsoleKey.LeftArrow) currentPanel.selectLeft();
                if (key == ConsoleKey.RightArrow) currentPanel.selectRight();

                if (key == ConsoleKey.Tab)
                {
                    currentPanel.Focused = false;
                    currentPanel.updateSelected();

                    currentPanel = currentPanel == leftPanel ? rightPanel : leftPanel;

                    currentPanel.Focused = true;
                    currentPanel.updateSelected();
                }

                if (key == ConsoleKey.Enter) currentPanel.Action();
            }
        }

        void drawFooterMenu(string key, string text)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(key);

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(text);
        }

        void drawFooter()
        {
            Console.SetCursorPosition(0, Height - 1);

            drawFooterMenu("1", "Left  ");
            drawFooterMenu(" 2", "Right ");
            drawFooterMenu(" 3", "View  ");
            drawFooterMenu(" 4", "Edit  ");
            drawFooterMenu(" 5", "Copy  ");
            drawFooterMenu(" 6", "Move  ");
            drawFooterMenu(" 7", "MkDir ");
            drawFooterMenu(" 8", "Find  ");
            drawFooterMenu(" 9", "Info  ");
            drawFooterMenu(" 10", "Quit ");


        }

        void draw()
        {
            drawFooter();

            leftPanel.draw();
            rightPanel.draw();
        }

        public void log(string s)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, Height - 2);
            Console.Write(s);
        }
    }
}
