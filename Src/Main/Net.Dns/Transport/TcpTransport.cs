/*
Version 2, June 1991

Copyright (C) 1989, 1991 Free Software Foundation, Inc.  
51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA

Everyone is permitted to copy and distribute verbatim copies
of this license document, but changing it is not allowed.

A full copy of the license can be obtained at: http://www.gnu.org/licenses/gpl.txt 
*/
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Net.Dns;

namespace Net.Dns.Transport
{
	/// <summary>
	/// Summary description for UdpTransport.
	/// </summary>
	public class TcpTransport : AbstractTransport
	{

		public TcpTransport(IPEndPoint endpoint) : base(endpoint) {}
		public TcpTransport(IPAddress address) : base(address) {}
		public TcpTransport(IPAddress address, int port) : base(address, port) {}


        public override byte[] SendRequest(byte[] requestMessage)
        {
            short dataLength = (short)requestMessage.Length;
            byte[] buffer = new byte[2 + dataLength];
            int offset = 0;
            WriteShort(dataLength, buffer, ref offset);
            Array.Copy(requestMessage, 0, buffer, offset, requestMessage.Length);

            TcpClient socket = new TcpClient();
            socket.Connect(this.endpoint);

            // send the request, may throw IOException/SocketException
            NetworkStream stream = socket.GetStream();
            stream.Write(buffer, 0, buffer.Length);

            // wait the first 2 bytes of response (the length)
            // we'll just re-use our send buffer for this 2 bytes
            int expected = stream.Read(buffer, 0, 2);
            if (expected != 2)
            {
                // dns server doesn't give us any data
                throw new IOException("DNS server is not responding");
            }

            // ok, receive all packets
            offset = 0;
            int totalLength = ReadShort(buffer, ref offset);
            byte[] receiveBuffer = new byte[totalLength];
            int receivedSoFar = 0;
            while (receivedSoFar < totalLength)
            {
                receivedSoFar += stream.Read(receiveBuffer, receivedSoFar, totalLength - receivedSoFar);
            }

            stream.Close();
            socket.Close();

            // return the received data
            return receiveBuffer;

        }

        private void WriteShort(short toWrite, byte[] data, ref int offset)
        {
            data[offset++] = (byte)(toWrite >> 8);
            data[offset++] = (byte)(toWrite);
        }
		private int ReadShort(byte[] buf, ref int offset)
		{
			return (int) (buf[offset++] << 8 | buf[offset++]);
		}

	}
}
