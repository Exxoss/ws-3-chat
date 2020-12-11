using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServerData

{
    [Serializable]
    public class Packet
    {

        public List<string> Clist;
        public int packetInt;
        public bool packetBool;
        public string senderID;
        public PacketType packetType;

        // Constructeur
        public Packet(PacketType type, string senderID)
        {
            Clist = new List<string>();
            this.senderID = senderID;
            this.packetType = type;

        }

        // Déserializer
        public Packet(byte[] packetbytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(packetbytes);
            Packet p = (Packet)bf.Deserialize(ms);
            ms.Close();
            this.Clist = p.Clist;
            this.packetInt = p.packetInt;
            this.packetBool = p.packetBool;
            this.senderID = p.senderID;
            this.packetType = p.packetType;

        }

        // Serializer
        public byte[] ToBytes()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this);
            byte[] bytes = ms.ToArray();

            ms.Close();
            return bytes;

        }
    }
}
