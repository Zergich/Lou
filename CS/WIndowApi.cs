using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LouConsoleUI
{
    public class WIndowApi
    {
        private const int HIDE = 0;
        private const int SHOW = 5;

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static public void Hide() => ShowWindow(GetConsoleWindow(), 0);
        static public void Show() => ShowWindow(GetConsoleWindow(), 5);

    }
}
