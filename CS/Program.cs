using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
            Console_CancelKeyPress();
            //AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            //Console.CancelKeyPress += Console_CancelKeyPress;



            //IntPtr handle = GetConsoleWindow();
            //IntPtr sysMenu = GetSystemMenu(handle, false);

            //if (handle != IntPtr.Zero)
            //{
            //    DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
            //    //DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
            //    DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
            //    DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);//resize
            //}

            Program pg = new Program();
            pg.Hello();
            Thread th = new Thread(AI);
            th.Start();
            Console.Write(" Ты:\n".Pastel("#1846e1"));
            Console.Write("   ");
            Console.Write($"{CYAN}");
            Thread  usmess = new Thread(UserMessage);
            usmess.Start();


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
                foreach(string mess in s.Skip(1))
                    Console.Write($"{YELLOW}{mess}");

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
        static void UserMessage()
        {
            while (true)
            {
                Server sv = new Server();
                UserQuest = Console.ReadLine();
                if (UserQuest == "!hide") WIndowApi.Hide();
                else
                    sv.Send(UserQuest);
                NoFirstItera = false;
            }
        }
        void UserMessage(string Message)
        {
            if (UserQuest == "")
            {
                Console.Write($"{CYAN}{Message}");
            }
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
                Regex regex = new Regex(@"<!1UserREsposeMEssage\)\)>");
                MatchCollection matches;

                Regex regexShow = new Regex(@"<!WermaPOajaxzaca21!>");
                MatchCollection matchesShow;
                Regex regexHide = new Regex(@"<!UNIIDHIDEC#RESKODLikeUVB76>");
                MatchCollection matchesHide;
                Regex regexClose = new Regex(@"<!@CLoseSystemDADAExit1>");
                MatchCollection matchesClose;


                while ((bytesRead = process.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                {

                    string result = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    matches = regex.Matches(result);

                    matchesShow = regexShow.Matches(result);
                    matchesHide = regexHide.Matches(result);
                    matchesClose = regexClose.Matches(result);

                    if (matchesClose.Count > 0)
                    {
                        Console_CancelKeyPress();
                        Environment.Exit(0);     
                        continue;
                    }
                    if (matchesHide.Count > 0){ WIndowApi.Hide(); result = ""; continue;}
                    if (matchesShow.Count > 0) {WIndowApi.Show(); result = "";  continue;}

                    if (result == "<1!Endt-Call>EndBott")
                    {
                        pg.AssictentMessage("", true);
                        Console.Write(" Ты:\n".Pastel("#1846e1"));
                        Console.Write("   ");
                        Console.Write($"{CYAN}");
                    }
                    else if (matches.Count > 0)
                    {
                        pg.UserMessage(result.Replace("<!1UserREsposeMEssage))>", ""));
                        result = result.Replace($"<!1UserREsposeMEssage))> {UserQuest}", "");
                        result = result.Replace("<1!Endt-Call>EndBott", "");
                        pg.AssictentMessage(result, false);
                    }
                    else
                    {
                        result = result.Replace($"<!1UserREsposeMEssage))> {UserQuest}", "");
                        result = result.Replace("<1!Endt-Call>EndBott", "");
                        pg.AssictentMessage(result, false);
                    }
                    UserQuest = "";
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
