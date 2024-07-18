using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResSpy
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static void InvokeTool(string toolName, string toolPara)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = toolName,
                Arguments = toolPara,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            Process scriptProcess = new Process();
            scriptProcess.StartInfo = processInfo;
            scriptProcess.Start();
            Console.WriteLine(scriptProcess.StandardOutput.ReadToEnd());
            Console.Write("Replacing resources... Please wait...");
            scriptProcess.WaitForExit();

        }
        static void Main(string[] args)
        {
            // 获取命令行参数
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            // 检查是否有足够的参数
            if (commandLineArgs.Length > 2)
            {
                string yourprogram = commandLineArgs[1];
                string resFilePath = commandLineArgs[2];

                Console.WriteLine("your program: " + yourprogram);
                Console.WriteLine("resource file: " + resFilePath);
                InvokeTool("tools\\ResourceHacker", "-open " + yourprogram + " -save " + yourprogram + " -res \"" + resFilePath + "\" -action addoverwrite -mask *");
                InvokeTool("tools\\ResourceHacker", "-open " + yourprogram + "\" -save \"tmp\\VERSIONINFO.res\" -action extract -mask VERSIONINFO");
                InvokeTool("tools\\ResourceHacker", "-open \"" + resFilePath + "\" -save \"tmp\\MANIFEST.res\" -action extract -mask MANIFEST");
                InvokeTool("tools\\ResourceHacker", "-open " + yourprogram + " -save " + yourprogram + " -res \"tmp\\VERSIONINFO.res\" -action addoverwrite -mask *");
                InvokeTool("tools\\ResourceHacker", "-open " + yourprogram + " -save " + yourprogram + " -res \"tmp\\MANIFEST.res\" -action addoverwrite -mask *");
                Console.WriteLine("\r\nReplacing Completed!");
            }
            else
            {
                Console.WriteLine("usage: ResSpy.exe [your program] [resource file]");
            }

        }
    }
}
