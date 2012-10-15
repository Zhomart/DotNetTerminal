using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DotNetTerminal
{
    class Panel
    {
        Application app;

        public int X { get; set; }
        public int Y { get; set; }

        ConsoleColor backgroundColor = ConsoleColor.Blue;
        ConsoleColor borderColor = ConsoleColor.Cyan;
        ConsoleColor foregroundColor = ConsoleColor.Cyan;

        string currentDirectory = "c:\\";

        public int Width { get { return app.Width / 2;  } }
        public int Height { get { return app.Height - 2; } } // 1 for command line, 1 for tips

        FileSystemInfo[] files;
        int selectedIndex = 0;
        int showStartIndex = 0;

        public Panel(Application app)
        {
            this.app = app;
            X = 0;
            Y = 0;

            DirectoryInfo currentDirectoryInfo = new DirectoryInfo(currentDirectory);
            files = currentDirectoryInfo.GetFileSystemInfos();

            sortFiles();
        }

        public void sortFiles()
        {
            int n = files.Length;
            for (int i=0;i<n;++i)
                for (int j=i+1;j<n;++j)
                    if ((IsDirectory(files[j]) && !IsDirectory(files[i]))||(IsDirectory(files[j]) && IsDirectory(files[i]) && (files[i].Name.CompareTo(files[j].Name) > 0)))
                    {
                        FileSystemInfo tmp = files[i];
                        files[i] = files[j];
                        files[j] = tmp;
                    }
        }

        public void draw()
        {
            fillBackground();
            drawBorders();
            drawHeaders();

            drawSelectedFileInfo();
            drawDirectoryInfo();

            drawCurrentDirectory();
        }

        public void drawSelectedFileInfo()
        {
            var info = new FileInfo(files[selectedIndex].FullName);

            SetCursorPosition(1, Height - 2);

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(info.Name);

            SetCursorPosition(Width - 15, Height - 2);
            Console.Write(info.CreationTime.ToString("dd.MM.yy HH:mm"));

            if (IsDirectory(info))
            {
                string text = " Folder ";

                SetCursorPosition(Width - 15 - text.Length, Height - 2);
                Console.Write(text);
            }
            else {
                string file_size = formatSize(info.Length);

                SetCursorPosition(Width - 16 - file_size.Length, Height - 2);
                Console.Write(file_size);
            }
        }

        public bool IsDirectory(object info) 
        {
            bool ch = info.GetType() == typeof(DirectoryInfo);
            if (ch) return true;
            if (info.GetType() == typeof(FileInfo))
                return ((FileInfo)info).Attributes.HasFlag(FileAttributes.Directory);
            if (info.GetType() == typeof(FileSystemInfo))
                return ((FileSystemInfo)info).Attributes.HasFlag(FileAttributes.Directory);
            return false;
        }

        public void drawFile(int index)
        {
            int max_count = 18; // in column

            if (index == selectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
            }
            else
            {
                Console.BackgroundColor = backgroundColor;
            }

            Console.ForegroundColor = foregroundColor;

            var info = files[index];

            if (IsDirectory(info))
                Console.ForegroundColor = ConsoleColor.White;

            if (info.Attributes.HasFlag(FileAttributes.Hidden) || info.Attributes.HasFlag(FileAttributes.System))
            {
                if (index == selectedIndex)
                    Console.ForegroundColor = ConsoleColor.Gray;
                else
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
            }

            string name = info.Name;

            int max_width = 19;
            int left = 0, top = 0;

            if (index - showStartIndex >= max_count)
            {
                max_width = 18;
                left = Width / 2 + 1;
                top = index - max_count + 2;
            }
            else
            {
                left = 1;
                top = index + 2;
            }

            SetCursorPosition(left, top);
            for (int i = 0; i < max_width; ++i) Console.Write(" ");

            SetCursorPosition(left, top);
            if (name.Length > max_width) name = name.Substring(0, max_width) + "}";

            Console.Write(name);
        }

        public void drawCurrentDirectory()
        {
            for (int i = 0; i < files.Length;++i )
                drawFile(i);
        }

        void drawDirectoryInfo()
        {
            DirectoryInfo info = new DirectoryInfo(currentDirectory);

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;

            string name = " " + info.Name + " ";

            SetCursorPosition(Width / 2 - name.Length / 2, 0);
            Console.Write(name);

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;

            int fileCount = 0;
            long size = 0;
            foreach (var fileInfo in info.GetFiles())
            {
                fileCount++;
                size += fileInfo.Length;
            }

            string file_size = formatSize(size);

            string files_info = " " + file_size + " in " + fileCount + " files ";

            SetCursorPosition(Width / 2 - files_info.Length / 2, Height - 1);
            Console.Write(files_info);
        }

        void drawHeaders()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            SetCursorPosition(Width / 4 - 2, 1);
            Console.Write("Name");
            SetCursorPosition(3 * Width / 4 - 2, 1);
            Console.Write("Name");
        }

        string formatSize(long size)
        {
            string rr = "B";
            if (size / 1024 >= 1)
            {
                size /= 1024;
                rr = "KB";
            }
            else if (size / (1024 * 1024) >= 1)
            {
                size /= 1024 * 1024;
                rr = "MB";
            }
            else if (size / (1024 * 1024 * 1024) >= 1)
            {
                size /= 1024 * 1024 * 1024;
                rr = "GB";
            }
            return size + rr;
        }

        void fillBackground()
        {
            Console.BackgroundColor = backgroundColor;
            for (int i = 0; i < Height; ++i)
            {
                SetCursorPosition(0, i);
                for (int j = 0; j < Width; ++j) Console.Write(" ");
            }
        }

        void drawBorders()
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = borderColor;

            SetCursorPosition(0, 0);
            Console.Write("╔");
            SetCursorPosition(0, Height - 1);
            Console.Write("╚");
            for (int i = 1; i < Width - 1; ++i)
            {
                SetCursorPosition(i, 0);
                Console.Write("═");
                SetCursorPosition(i, Height - 1);
                Console.Write("═");
            }
            SetCursorPosition(Width - 1, 0);
            Console.Write("╗");
            SetCursorPosition(Width - 1, Height - 1);
            Console.Write("╝");
            for (int i = 1; i < Height - 1; ++i)
            {
                SetCursorPosition(0, i);
                Console.Write("║");
                SetCursorPosition(Width - 1, i);
                Console.Write("║");
            }

            for (int i = 1; i < Height - 3; ++i)
            {
                SetCursorPosition(Width / 2, i);
                Console.Write("│");
            }
            
            for (int i = 1; i < Width - 1; ++i)
            {
                SetCursorPosition(i, Height - 3);
                Console.Write("─");
            }
        }

        void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left + X, top + Y);
        }

    }
}
