using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DotNetTerminal
{
    class Application
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Panel leftPanel { get; set; }
        public Panel rightPanel { get; set; }

        public Panel currentPanel;

        public object locker = new object();

        string current_directory;

        string command = "";

        YesNoBox exit_menu;
        MakeFolderBox mkdir_menu;
        AboutBox about_box;

        public char[] chars = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '_', '\\', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.', ',', '-', '=', '+', ' '};

        public Application() 
        {
            Width = 80;
            Height = 25;

            Console.SetWindowSize(Width, Height);

            leftPanel = new Panel(this, "C:\\");
            rightPanel = new Panel(this, "C:\\");

            rightPanel.X = Width / 2;

            exit_menu = new YesNoBox(this, "Quit", "Do you want to Quit DNT?");

            exit_menu.YesPressed = delegate(ConsoleKeyInfo info) { Environment.Exit(0); };

            mkdir_menu = new MakeFolderBox(this);

            about_box = new AboutBox(this);
        }

        public ConsoleKeyInfo readKey() {
            return Console.ReadKey(true);
        }

        public void DrawPanels()
        {
            leftPanel.draw();
            rightPanel.draw();
        }

        public void run()
        {
            leftPanel.Visible = false;
            currentPanel = rightPanel;
            currentPanel.Focused = true;
            current_directory = currentPanel.directory;

            draw();

            currentPanel.updateSelected(0);

            while (true)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                var cdir = current_directory;
                if (current_directory.Length > 10)
                    cdir = current_directory.Substring(0, 3) + "..." + current_directory.Substring(current_directory.Length - 6, 6);
                log(cdir + ">");
                Console.SetCursorPosition(command.Length + cdir.Length + 1, Height - 2);

                ConsoleKeyInfo key_info = readKey();
                ConsoleKey key = key_info.Key;

                if (chars.Contains(key_info.KeyChar))
                {
                    command += key_info.KeyChar;
                    log(cdir + ">"+command);
                    continue;
                }

                if (key == ConsoleKey.Backspace && command.Length > 0)
                {
                    command = command.Substring(0, command.Length - 1);
                    log(cdir + ">" + command+" ");
                    continue;
                }

                switch (key)
                {
                    case ConsoleKey.Enter:
                        if (command.Length > 0)
                        {
                            cdir = current_directory;
                            if (cdir[cdir.Length - 1] != '\\')
                                cdir += '\\';
                            command = command.Trim();
                            var cmd = cdir + command;
                            try
                            {
                                if (File.Exists(cmd))
                                    System.Diagnostics.Process.Start(cmd);
                                else
                                    if (command == "exit") break;
                                    else
                                        System.Diagnostics.Process.Start(command);
                            }
                            catch (Exception ex)
                            {
                            }
                            command = "";
                            log("                                                   ");
                            continue;
                        }
                        break;
                    case ConsoleKey.F10:
                        exit_menu.run();
                        break;
                    case ConsoleKey.F1:
                        if (key_info.Modifiers == ConsoleModifiers.Alt) leftPanel.SelectDrive();
                        if (key_info.Modifiers == ConsoleModifiers.Control)
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
                        break;
                    case ConsoleKey.F2:
                        if (key_info.Modifiers == ConsoleModifiers.Alt) rightPanel.SelectDrive();
                        if (key_info.Modifiers == ConsoleModifiers.Control)
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
                        break;
                }

                if (null == currentPanel)
                    currentPanel = leftPanel.Visible ? leftPanel : (rightPanel.Visible ? rightPanel : null);

                if (!leftPanel.Visible && !rightPanel.Visible) currentPanel = null;
                if (null == currentPanel) continue;

                switch (key)
                {
                    case ConsoleKey.F3:
                        currentPanel.OpenEdit();
                        break;
                    case ConsoleKey.F4:
                        currentPanel.OpenEdit();
                        break;
                    case ConsoleKey.F5: log("         not implemented"); break;
                    case ConsoleKey.F6: log("         not implemented"); break;
                    case ConsoleKey.F7: if (currentPanel != null)mkdir_menu.run(currentPanel.directory); break;
                    case ConsoleKey.F8: log("         not implemented"); break;
                    case ConsoleKey.F9: about_box.run(); break;
                    case ConsoleKey.UpArrow: currentPanel.selectPrevFile(); break;
                    case ConsoleKey.DownArrow: currentPanel.selectNextFile(); break;
                    case ConsoleKey.LeftArrow: currentPanel.selectLeft(); break;
                    case ConsoleKey.RightArrow: currentPanel.selectRight(); break;
                    case ConsoleKey.PageUp: currentPanel.selectLeft(); break;
                    case ConsoleKey.PageDown: currentPanel.selectRight(); break;
                    case ConsoleKey.Home: currentPanel.updateSelected(0); break;
                    case ConsoleKey.End: currentPanel.updateSelected(currentPanel.AllFiles.Count - 1); break;
                    case ConsoleKey.Tab:
                        if (currentPanel == leftPanel || currentPanel == rightPanel)
                        {
                            if (!(currentPanel == leftPanel ? rightPanel : leftPanel).Visible) continue;

                            currentPanel.Focused = false;
                            currentPanel.updateSelected();

                            currentPanel = currentPanel == leftPanel ? rightPanel : leftPanel;

                            currentPanel.Focused = true;
                            currentPanel.updateSelected();

                            current_directory = currentPanel.directory;
                        }
                        break;
                    case ConsoleKey.Enter:
                    currentPanel.Action();
                    current_directory = currentPanel.directory;
                    log("                                                  ");
                    command = "";
                        break;
                }
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
            drawFooterMenu(" 9", "About ");
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
