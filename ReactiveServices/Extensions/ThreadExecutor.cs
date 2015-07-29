using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveServices.Extensions
{
    public sealed class ThreadExecutor : IDisposable
    {
        private class BackgroundThread
        {
            public BackgroundThread(string name, Thread thread, CancellationToken? cancellationToken)
            {
                Name = name;
                Thread = thread;
                CancellationToken = cancellationToken;
            }

            private string Name { get; set; }
            public Thread Thread { get; private set; }
            private CancellationToken? CancellationToken { get; set; }
        }

        private readonly List<BackgroundThread> BackgroundThreads = new List<BackgroundThread>();

        public void Repeat(string name, Action action, Action<Exception> onException, int interval = 100, ThreadPriority priority = ThreadPriority.Normal, CancellationToken? cancellationToken = null)
        {
            var thread = new Thread(
                () =>
                {
                    try
                    {
                        while (true)
                        {
                            if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
                                break;

                            Sleep(interval);
                            action();
                        }
                    }
                    catch (Exception e)
                    {
                        onException(e);
                    }
                }
            );
            thread.Run(priority);
            Register(thread, cancellationToken, name);
        }

        public void Run(string name, Action action, Action<Exception> onException, ThreadPriority priority = ThreadPriority.Normal, CancellationToken? cancellationToken = null)
        {
            var thread = new Thread(
                () =>
                {
                    try
                    {
                        if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
                            return;
                        action();
                    }
                    catch (Exception e)
                    {
                        onException(e);
                    }
                }
            );
            thread.Run(priority);
            Register(thread, cancellationToken, name);
        }

        private void Register(Thread thread, CancellationToken? cancellationToken, string name)
        {
            lock (BackgroundThreads)
            {
                BackgroundThreads.RemoveAll(t => !t.Thread.IsAlive);
                thread.Name = name;
                BackgroundThreads.Add(new BackgroundThread(name, thread, cancellationToken));
            }
        }

        private static void Sleep(int interval)
        {
            try
            {
                Thread.Sleep(interval);
            }
            catch (ThreadAbortException)
            {
            }
        }

        public void WaitAll(TimeSpan timeout)
        {
            var sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                lock (BackgroundThreads)
                {
                    BackgroundThreads.RemoveAll(t => !t.Thread.IsAlive);
                    if (BackgroundThreads.Count == 0)
                        break;
                }
                if (sw.Elapsed > timeout)
                {
                    sw.Stop();
                    var threadName = "Unknown";
                    try
                    {
                        lock (BackgroundThreads)
                        {
                            threadName = BackgroundThreads.First().Thread.Name;
                        }
                    }
                    catch
                    {
                    }
                    throw new TimeoutException(String.Format("All background threads should have been stopped within {0} seconds. Thread {1} could not be stopped!", timeout.TotalSeconds, threadName));
                }
                Thread.Sleep(100);
            }
            sw.Stop();
        }

        public async Task WaitAllAsync(TimeSpan timeout)
        {
            await Task.Run(() => WaitAll(timeout));
        }

        public void AbortAll()
        {
            lock (BackgroundThreads)
            {
                BackgroundThreads.RemoveAll(t => !t.Thread.IsAlive);
                foreach (var backgroundThread in BackgroundThreads)
                    backgroundThread.Thread.Abort();
            }
        }

        public void Dispose()
        {
            AbortAll();
        }
    }
}
