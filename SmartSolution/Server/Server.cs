using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ServerData;

namespace Server
{
    class Server
    {
        static Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static List<ClientData> _clients = new List<ClientData>();

        public static void Main(string[] args)
        {
            // Binding du socket listner
            listenerSocket.Bind(new IPEndPoint(
                    IPAddress.Parse(ServerConfig.Instance.ServerIpAdresse),
                    ServerConfig.Instance.ServerPort
                ));

            // Lancement du Socket listner
            Thread listenThread = new Thread(() => {
                while (true)
                {
                    listenerSocket.Listen(0);
                    // Ajout d'un nouveau client au chat
                    _clients.Add(new ClientData(listenerSocket.Accept()));
                }
            });
            listenThread.Start();

            // Visualisation du lancement du server
            Console.WriteLine("Server Started at : {0}:{1} ",
                ServerConfig.Instance.ServerIpAdresse,
                ServerConfig.Instance.ServerPort);
        }

        // Reception d'un message envoyer par un client en binaires
        public static void Data_IN(object cSocket)
        {

            Socket clientSocket = (Socket)cSocket;
            byte[] Buffer;
            int readBytes;
            while (true)
            {
                try
                {
                    Buffer = new byte[clientSocket.SendBufferSize];
                    readBytes = clientSocket.Receive(Buffer);
                    if (readBytes > 0)
                    {
                        //handle data
                        Packet packet = new Packet(Buffer);
                        DataManager(packet);

                    }

                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Disconnected Client {0}...", ex.Message);
                }

            }

        }

        // Envoi du packet reçu aux clients connectés
        public static void DataManager(Packet p)
        {
            switch (p.packetType)
            {
                case PacketType.chat:
                    foreach (ClientData c in _clients)
                    {
                        c.clientSocket.Send(p.ToBytes());
                    }
                    break;
            }

        }
    }
}
