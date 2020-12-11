using System;
namespace ServerData
{
    // Implementation du pattern Singleton pour la configuration du Server
    // Sealed empèche l'héritage de la classe
    public sealed class ServerConfig
    {
        // Attributs
        private string _serverIpAdresse = "127.0.0.1";
        private int _serverPort = 4242;

        // Instance
        private static ServerConfig instance;
        private static readonly object locker = new object();

        // privatisation de l'instance
        private ServerConfig()
        {
        }

        // Propriété du Singleton publique (get only) pour renvoyer l'instance ou instancier
        public static ServerConfig Instance
        {
            get
            {
                lock (locker)
                {
                    if (null == instance)
                    {
                        instance = new ServerConfig();
                    }
                    return instance;
                }
            }
        }

        public string ServerIpAdresse { get { return _serverIpAdresse; } }
        public int ServerPort { get { return _serverPort; } }
    }
}
