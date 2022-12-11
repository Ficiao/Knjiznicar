using System;
using System.Threading;

namespace KnjiznicarLoginServer
{
    class Program
    {
        private static bool isRunning = false;

        static void Main(string[] args)
        {
            Console.Title = "ServerConsole";
            isRunning = true;

            GlobalInitializer.Initialize();
            OverworldServer.Instance.Connect(26953, ServerStart);

            Console.ReadKey();
        }

        public static void ServerStart()
        {
            Server.Start(50, 26950);
        }
    }
}
