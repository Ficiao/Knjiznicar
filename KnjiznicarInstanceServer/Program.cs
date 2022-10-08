using System;
using System.Threading;

namespace KnjiznicarInstanceServer
{
    class Program
    {
        private static bool isRunning = false;
        static void Main(string[] args)
        {
            Console.Title = "ServerConsole";
            isRunning = true;

            Server.Start(50, 26950);

            Console.ReadKey();
        }
    }
}
