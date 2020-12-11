using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ServerData;

namespace Client
{
    class Client
    {
        public static Socket socket;
        public static string name;
        public static string id;


        public static void Main(string[] args)
        {
            // Définition du pseudo
            Console.WriteLine("Entrer votre prénom :");
            name = Console.ReadLine();

            // Connection au socket listner
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint i = new IPEndPoint(
                IPAddress.Parse(ServerConfig.Instance.ServerIpAdresse),
                ServerConfig.Instance.ServerPort);

            try
            {
                socket.Connect(i);

                // Visualisation
                ConsoleColor c = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("--> Connected to the server as : {0}", name);
                Console.ForegroundColor = c;
            }
            catch
            {
                Console.WriteLine("Connection unavailable ...");
                Thread.Sleep(1000);

            }

            // Surveillance du serveur pour les messages des autres clients 
            Thread t = new Thread(Data_received);
            t.Start();

            // Envoi d'un message sur le server
            while (true)
            {
                Console.Write(":>");
                string input = Console.ReadLine();
                Packet p = new Packet(PacketType.chat, id);
                p.Clist.Add(name);
                p.Clist.Add(input);
                socket.Send(p.ToBytes());
            }

        }

        /*
         * Reception de données envoyés par le server
         */
        static void Data_received()
        {
            byte[] Buffer;
            int readBytes;
            while (true)
            {
                try
                {
                    Buffer = new byte[socket.SendBufferSize];
                    readBytes = socket.Receive(Buffer);
                    if (readBytes > 0)
                    {
                        DataManager(new Packet(Buffer));
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Le serveur est déconnecté" + ex.Message);
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
        }

        /*
         * Affichage des données binaires 
         */
        public static void DataManager(Packet p)
        {
            switch (p.packetType)
            {
                case PacketType.Registration:
                    id = p.Clist[0];
                    break;
                case PacketType.chat:
                    ConsoleColor c = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(p.Clist[0] + " : " + p.Clist[1]);
                    Console.ForegroundColor = c;
                    break;
            }
        }
    }
}
