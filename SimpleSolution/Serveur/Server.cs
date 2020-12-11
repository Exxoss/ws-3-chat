using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Serveur
{
    class Server
    {
        // static Socket client; //static Socket newsock;
        static IPEndPoint clientep;

        static void Main(string[] args)
        {
            Socket socket = SeConnecter();
            Socket client = AccepterConnection(socket);
            EcouterReseau(client);
            Deconnecter(socket);
        }
        private static Socket SeConnecter()
        {
            Console.OutputEncoding = Encoding.UTF8;

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            newsock.Bind(ipep);
            newsock.Listen(10);

            Console.WriteLine("Serveur disponible à l'écoute ...");

            return newsock;
        }
        private static Socket AccepterConnection(Socket socket)
        {
            Socket client = socket.Accept();
            clientep = (IPEndPoint)client.RemoteEndPoint;

            Console.WriteLine("Connecté avec l'adresse {0} et port {1}", clientep.Address, clientep.Port);

            return client;
        }

        private static void EcouterReseau(Socket client)
        {
            string input;
            int recv;

            string welcome = "Bienvenue sur le serveur ...";

            // send message in socket
            byte[] data = new byte[1024];
            data = Encoding.UTF8.GetBytes(welcome);
            client.Send(data, data.Length, SocketFlags.None);


            while (true)
            {

                // waiting for client message
                recv = client.Receive(data);

                // catch exit message from client
                if (Encoding.UTF8.GetString(data, 0, recv) == "exit")
                    break;

                // Print client received message
                Console.WriteLine("Client: " + Encoding.UTF8.GetString(data, 0, recv));

                // waiting for user entry
                Console.Write(": ");
                input = Console.ReadLine();

                // Send user entry in socket (server message)
                Console.WriteLine("Server : " + input);
                client.Send(Encoding.UTF8.GetBytes(input));
            }
        }
        private static void Deconnecter(Socket socket)
        {
            Console.WriteLine("Disconnected from {0}", clientep.Address);
            socket.Close();

            Console.ReadLine();
        }
    }
}
