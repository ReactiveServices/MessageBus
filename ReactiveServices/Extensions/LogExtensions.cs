using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace ReactiveServices.Extensions
{
    public static class LogExtensions
    {
        public static IEnumerable<string> Entries(this Logger log)
        {
            MemoryTarget memoryLog;

            if (LogManager.Configuration == null)
                throw new ArgumentException("NLog configuration could not be loaded!");

            var target = LogManager.Configuration.FindTargetByName("memory");
            if (target is AsyncTargetWrapper)
                memoryLog = (target as AsyncTargetWrapper).WrappedTarget as MemoryTarget;
            else
                memoryLog = target as MemoryTarget;

            if (memoryLog == null)
                throw new ArgumentException("Memory target named 'memory' not found on NLog configuration!");

            return memoryLog.Logs;
        }

        public static void WriteRuntimeInfo(this Logger log)
        {
            if (Thread.CurrentThread.IsRunningOnMono())
                log.Info("Running on Mono!");
            else
                log.Info("Running on .NET Framework!");
        }
    }
}
