using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Net.IO
{
    
    internal class PacketBuilder
    {
        private MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opCode)
        {
            _ms.WriteByte(opCode);
        }

        public void WriteMessage(string str)
        {
            var msgLenght = str.Length;
            _ms.Write(BitConverter.GetBytes(msgLenght));
            _ms.Write(Encoding.ASCII.GetBytes(str));
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
