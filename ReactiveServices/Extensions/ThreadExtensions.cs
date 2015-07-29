using System;
using System.Diagnostics;
using System.Threading;

namespace ReactiveServices.Extensions
{
    public static class ThreadExtensions
    {
        public static void Run(this Thread thread, ThreadPriority priority = ThreadPriority.Normal)
        {
            thread.IsBackground = true;
            thread.Priority = priority;
            thread.Start();
        }

        public static void SleepWhile(this Thread thread, Func<bool> conditionSatified, TimeSpan timeout, Action timeoutAction)
        {
            var sw = new Stopwatch();
            sw.Start();
            while ((sw.Elapsed < timeout) && conditionSatified())
            {
                Thread.Sleep(1);
            }
            sw.Stop();
            if (sw.Elapsed >= timeout)
                timeoutAction();
        }

        public static bool IsRunningOnMono(this Thread thread)
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}
