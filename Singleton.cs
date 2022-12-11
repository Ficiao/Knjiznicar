using KnjiznicarLoginServer.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnjiznicarLoginServer
{
    public abstract class Singleton<T> : Initializer where T : Singleton<T> 
    {
        private static T _instance = null;
        public static T Instance { get => _instance; private set => _instance = value; }

        public void Init()
        {
            Instance = this as T;
        }

        protected virtual void OnApplicationQuit()
        {
            Instance = null;
        }
    }

    public interface Initializer
    {
        public void Init();
    }

    public static class GlobalInitializer
    {
        private static List<Initializer> initializeQueue = new List<Initializer>
        {
            new OverworldServer()
        };

        public static void Initialize()
        {
            foreach(Initializer init in initializeQueue)
            {
                init.Init();
            }
        }
     }
}
