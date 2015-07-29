using System;
using System.Diagnostics;
using System.Threading;
using NLog;
using PostSharp.Patterns.Diagnostics;

namespace ReactiveServices.Extensions
{
    public static class ProcessExtensions
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        [Log]
        [LogException]
        public static void Start(
            this Process process, 
            string command, 
            string arguments, 
            bool runInASeparateConsoleWindow,
            bool requireAdministratorPriviledges)
        {
            var isRunningOnMono = Thread.CurrentThread.IsRunningOnMono();

            var runInTheSameConsoleWindow = !runInASeparateConsoleWindow;

            process.StartInfo = new ProcessStartInfo
            {
                UseShellExecute = runInASeparateConsoleWindow,

                CreateNoWindow = runInTheSameConsoleWindow,
                RedirectStandardOutput = runInTheSameConsoleWindow,
                RedirectStandardError = runInTheSameConsoleWindow,

                Arguments =
                    isRunningOnMono
                        ? String.Format("{0} {1}", command, arguments)
                        : arguments,
                Verb = requireAdministratorPriviledges ? "runas" : null,
                FileName = (isRunningOnMono ? "mono" : command)
            };

            if (runInTheSameConsoleWindow)
            {
                process.OutputDataReceived += (s, e) => Console.WriteLine("> " + e.Data);
                process.ErrorDataReceived += (s, e) => Console.WriteLine(">> " + e.Data);
            }

            process.Start();

            if (runInTheSameConsoleWindow)
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }

            Log.Info("Process '{0}' started with arguments: {1}", process.StartInfo.FileName, process.StartInfo.Arguments);
        }
    }
}
