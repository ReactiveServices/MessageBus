using System;
using System.ServiceProcess;

namespace ReactiveServices.Extensions
{
    public static class WindowsService
    {
        public static void Start(string serviceName, TimeSpan timeout)
        {
            var service = new ServiceController(serviceName);
            try
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                throw new Exception(String.Format("Could not start service {0} within {1} milliseconds! Check if you have administrator privileges.", serviceName, timeout.TotalMilliseconds));
            }
        }

        public static void Stop(string serviceName, TimeSpan timeout)
        {
            var service = new ServiceController(serviceName);
            try
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch
            {
                throw new Exception(String.Format("Could not stop service {0} within {1} milliseconds! Check if you have administrator privileges.", serviceName, timeout.TotalMilliseconds));
            }
        }

        public static void Restart(string serviceName, TimeSpan timeout)
        {
            var service = new ServiceController(serviceName);
            try
            {
                var millisec1 = TimeSpan.FromTicks(Environment.TickCount);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                var millisec2 = TimeSpan.FromTicks(Environment.TickCount);
                timeout = timeout - (millisec2 - millisec1);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                throw new Exception(String.Format("Could not restart service {0} within {1} milliseconds! Check if you have administrator privileges.", serviceName, timeout.TotalMilliseconds));
            }
        }
    }
}
