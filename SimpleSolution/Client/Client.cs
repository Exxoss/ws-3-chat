using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Client
    {
        static void Main(string[] args)
        {
            Socket socket = SeConnecter();
            EcouterReseau(socket);
        }

        private static Socket SeConnecter()
        {
            Console.OutputEncoding = Encoding.UTF8;

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ipep);

            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect to server.");

                Console.WriteLine(e.ToString());


            }
            return server;
        }
        private static void EcouterReseau(Socket server)
        {
            //waiting for serveur message
            byte[] data = new byte[1024];
            int recv = server.Receive(data);

            string input, stringData;

            stringData = Encoding.UTF8.GetString(data, 0, recv);

            Console.WriteLine(stringData);

            while (true)
            {
                // waiting for user entry
                Console.Write(": ");
                input = Console.ReadLine();

                // send user entry in socket (client message)
                Console.WriteLine("Client: " + input);
                server.Send(Encoding.UTF8.GetBytes(input));

                //catch exit string to disconnect
                if (input == "exit") {
                    Deconnecter(server);
                    break;
                }

                //waiting for serveur message
                data = new byte[1024];
                recv = server.Receive(data);

                // Print serveur received message
                stringData = Encoding.UTF8.GetString(data, 0, recv);
                Console.WriteLine("Server: " + stringData);
            }

        }
        private static void Deconnecter(Socket socket)
        {
            Console.WriteLine("Disconnecting from server...");
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            Console.WriteLine("Disconnected!");

            Console.ReadLine();
        }
    }
}
