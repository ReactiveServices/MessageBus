using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using PostSharp.Patterns.Diagnostics;

namespace ReactiveServices.Extensions
{
    public static class AppDomainExtensions
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        [Log]
        [LogException]
        public static void LogExceptions(this AppDomain appDomain)
        {
            if (!Thread.CurrentThread.IsRunningOnMono())
            {
                appDomain.FirstChanceException += (s, e) => Task.Run(() =>
                {
                    // It is necessary to log the exception on a secondary thread, after a delay, 
                    // in order to have the complete stack trace and apply the NLog FileLoadException filter on it
                    Thread.Sleep(100);

                    if (!IsExceptionOfInterest(e.Exception))
                        return;

                    Log.Debug(e.Exception, "First chance exception!");
                });
            }
            appDomain.UnhandledException += (s, e) =>
            {
                if (!IsExceptionOfInterest(e.ExceptionObject as Exception))
                    return;

                Log.Fatal(e.ExceptionObject as Exception, "Unhandled exception!");
            };
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                if (!IsExceptionOfInterest(e.Exception))
                    return;

                Log.Error(e.Exception, "Unobserved task exception!");
            };
        }

        private static bool IsExceptionOfInterest(Exception e)
        {
            var exception = e.ToString();
            var isOfInterest = !IsNLogException(exception);
            isOfInterest = isOfInterest && !IsSymbolResolverException(exception);
            isOfInterest = isOfInterest && !IsThreadAbortException(exception);
            return isOfInterest;
        }

        private static bool IsThreadAbortException(string exception)
        {
            return exception.Contains("System.Threading.ThreadAbortException");
        }

        private static bool IsNLogException(string exception)
        {
            var nLogNamespace = typeof(Logger).Namespace;
            return exception.Contains(String.Format("at {0}.", nLogNamespace));
        }
        
        private static bool IsSymbolResolverException(string exception)
        {
            const string symbolResolverTypeName = "ReactiveServices.Configuration.TypeResolution.SymbolResolver";
            return exception.Contains(String.Format("{0}.LoadAssemblyByFullName", symbolResolverTypeName));
        }
    }
}
