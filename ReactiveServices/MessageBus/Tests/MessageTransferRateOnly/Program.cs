using System;
using ReactiveServices.Configuration;
using ReactiveServices.MessageBus;

namespace MessageTransferRateOnly
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start!");
            Console.ReadKey();
            Console.WriteLine("Running...");
            DependencyResolver.Reset();
            DependencyResolver.Initialize();
            var x = new ReactiveServices.MessageBus.Tests.UnitTests.MessageBusTests();
            x.TestMessageTransferRate("WithPublishConfirms", "Generics", StorageType.Persistent, SubscriptionMode.Shared);
            Console.WriteLine("Finished!");
            Console.ReadKey();
        }
    }
}
