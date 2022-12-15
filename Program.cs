using System;
using System.Threading;
using System.Threading.Tasks;

namespace KnjiznicarLoginServer
{
    class Program
    {
        private static bool isRunning = false;

        static void Main(string[] args)
        {
            try
            {
                Console.Title = "ServerConsole";
                isRunning = true;

                GlobalInitializer.Initialize();
                OverworldServer.Instance.Connect(26953, ServerStart);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting server: {ex}");
            }

            Task processTask = Process();
            processTask.Wait();
        }

        public static void ServerStart()
        {
            Server.Start(50, 26950);
        }

        private static async Task Process()
        {
            var isNotCancelled = true;

            while (isNotCancelled)
            {
                //Polling time here
                await Task.Delay(100000);

                //TODO: Do work here and listen for cancel
                Console.WriteLine("I did  some work...");
            }
        }
    }
}
