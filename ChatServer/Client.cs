using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Net.IO;

namespace ChatServer
{
    class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader;     

        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(ClientSocket.GetStream());

            var opcode = _packetReader.ReadByte();
            Username = _packetReader.ReadMessage();

            Console.WriteLine($"{DateTime.Now}: Client has connected with the username: {Username}");

            Task.Run(() => Process());
        }

        void Process()
        {
            while (true)
            {
                try
                {
                    var opCode = _packetReader.ReadByte();
                    switch (opCode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"{DateTime.Now}: {Username} says: {msg}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{Username}] says: {msg}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"{DateTime.Now}: {UID.ToString()} {Username} has disconnected");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocket.Close();
                    
                }
            }
        }
    }
}
