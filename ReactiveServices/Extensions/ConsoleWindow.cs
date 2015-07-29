using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ReactiveServices.Extensions
{
    public static class ConsoleWindow 
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public static bool IsVisible { get; private set; }

        public static void Show()
        {

            //Get a pointer to the forground window.  The idea here is that
            //IF the user is starting our application from an existing console
            //shell, that shell will be the uppermost window.  We'll get it
            //and attach to it
            var ptr = GetForegroundWindow();

            int u;

            GetWindowThreadProcessId(ptr, out u);

            var process = Process.GetProcessById(u);

            if (process.ProcessName == "cmd")    //Is the uppermost window a cmd process?
            {
                AttachConsole(process.Id);
            }
            else
            {
                //no console AND we're in console mode ... create a new console.
                AllocConsole();
            }

            IsVisible = true;
        }

        public static void Hide()
        {
            FreeConsole();
        }
    }
}
