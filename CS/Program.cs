using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pastel;
using static LouConsoleUI.Color;

namespace LouConsoleUI
{
    internal class Program
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        //public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;//resize

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            Console.CancelKeyPress += Console_CancelKeyPress;



            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                //DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);//resize
            }

            Program pg = new Program();
            pg.Hello();
            Thread th = new Thread(AI);
            th.Start();
            pg.UserMessage();


            Console.Read();

        }
        static void Console_CancelKeyPress()
        {
            Process[] python = Process.GetProcessesByName("Python");
            Process[] lou = Process.GetProcessesByName("Lou");
            foreach (Process p in python) p.Kill();
            foreach (Process p in lou) p.Kill();
        }
        // Делаем перегрузку метода, чтобы наш слушатель событий не выдавал ошибку
        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console_CancelKeyPress();
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Process[] python = Process.GetProcessesByName("Python");
            Process[] lou = Process.GetProcessesByName("Lou");
            foreach (Process p in python) p.Kill();
            foreach (Process p in lou) p.Kill();


        }
        void Hello()
        {
            Console.WriteLine("\t\t\t\t\t╭───────────────────────────────────────╮".Pastel("#de2c10"));
            Console.Write("\t\t\t\t\t│ ".Pastel("#de2c10"));
            Console.Write("Лу приветствует вас и готов к работе!".Pastel("#18d8e1"));
            //╰─────────────────╯
            Console.Write(" │".Pastel("#de2c10"));
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t\t╰───────────────────────────────────────╯".Pastel("#de2c10"));
            Console.WriteLine("\n");
        }
        public static void Color() 
        { }
        static bool NoFirstItera = false;
        void AssictentMessage(string MessageAI, bool end)
        {
            if (!NoFirstItera && MessageAI != "")
            {
                Console.WriteLine(" Ассистент:".Pastel("#789f16"));
                NoFirstItera=true;
                Console.Write("   ");
                string[] s = MessageAI.Split('\n'); 
                for (int i = 1; i < s.Length-1; i++)
                    Console.Write($"{YELLOW}{s[i]}");

            }
            else
                Console.Write($"{YELLOW}{MessageAI}");

            if (end)
            {
                NoFirstItera = false;
                Console.WriteLine();
            }
        }
        static string UserQuest = "";
        void UserMessage()
        {
            Console.Write(" Ты:\n".Pastel("#1846e1"));
            Console.Write("   ");
            Console.Write($"{CYAN}");
            Server sv = new Server();
            UserQuest = Console.ReadLine();
            sv.Send(UserQuest);
            NoFirstItera = false;
        }
        void UserMessage(bool PythonRecognize, string Message)
        {
            Console.Write(" Ты:\n".Pastel("#1846e1"));
            Console.Write("   ");
            Console.Write($"{CYAN}");
            if (PythonRecognize) Console.WriteLine();
            Server sv = new Server();
            UserQuest = Console.ReadLine();
            sv.Send(UserQuest);
        }
        static void AI()
        {
            Program pg = new Program();

            ProcessStartInfo procInfo = new ProcessStartInfo();
            // исполняемый файл программы - браузер хром
            procInfo.FileName = @"python";
            // аргументы запуска - адрес интернет-ресурса
            procInfo.Arguments = @"D:\Progects\GO\WWD\main.py";
            procInfo.UseShellExecute = false;
            procInfo.CreateNoWindow = true;
            Process.Start(procInfo);

            ProcessStartInfo startInfo = new ProcessStartInfo("D:\\Progects\\GO\\Lou\\Lou.exe");
            startInfo.WorkingDirectory = @"D:\Progects\GO\Lou";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.Verb = "runas";

            using (Process process = Process.Start(startInfo))
            {
                byte[] buffer = new byte[2024];
                int bytesRead;

                while ((bytesRead = process.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string result = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (result == "<1!Endt-Call>EndBott")
                    {
                        pg.AssictentMessage("", true);
                        pg.UserMessage();
                    }
                    if (result == "<!1UserREsposeMEssage))>") pg.UserMessage(true, result.Replace("<!1UserREsposeMEssage))>", ""));
                    else
                    {
                        result = result.Replace($"<!1UserREsposeMEssage))> {UserQuest}", "");
                        result = result.Replace("<1!Endt-Call>EndBott", "");
                        pg.AssictentMessage(result, false);
                    }
                }
                //string error;
                //while ((error = process.StandardError.ReadLine()) != null)
                //{
                //    broadcastL(Color.RED + error + Color.NORMAL);
                //}
                //process.WaitForExit();

                // Process the output as needed
            }
        }
    }
}
