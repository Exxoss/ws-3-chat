using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerData;

namespace Server
{
    /*
     * Représente un client enregistré sur le server
     */
    class ClientData
    {
        public Socket clientSocket;
        public Thread clientThread;

        public string id;

        public ClientData()
        {
            id = Guid.NewGuid().ToString();
            clientThread = new Thread(Server.Data_IN);
            clientThread.Start(clientSocket);
            sendRegistrationPacket();
        }

        // Surcharge du client socket
        public ClientData(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            clientThread = new Thread(Server.Data_IN);
            clientThread.Start(clientSocket);
            sendRegistrationPacket();
        }

        private void sendRegistrationPacket()
        {
            Packet p = new Packet(PacketType.Registration, "server");
            p.Clist.Add(id);

            clientSocket.Send(p.ToBytes());

        }
    }
}
