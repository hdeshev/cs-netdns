/*
Version 2, June 1991

Copyright (C) 1989, 1991 Free Software Foundation, Inc.  
51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA

Everyone is permitted to copy and distribute verbatim copies
of this license document, but changing it is not allowed.

A full copy of the license can be obtained at: http://www.gnu.org/licenses/gpl.txt 
*/
using System;
using System.Net;
using System.Net.Sockets;

namespace Net.Dns.Transport
{
	/// <summary>
	/// Summary description for UdpTransport.
	/// </summary>
	public class UdpTransport : AbstractTransport
	{
		private int		udpRetryAttempts = 2;

		public UdpTransport(IPEndPoint endpoint) : base(endpoint) {}
		public UdpTransport(IPAddress address) : base(address) {}
		public UdpTransport(IPAddress address, int port) : base(address, port) {}

		public int RetryAttemps
		{
			get { return this.udpRetryAttempts; }
			set
			{
				if (value <= 0)
					throw new ArgumentException("RetryAttemps needs a positive ( > 0 ) integer");

				this.udpRetryAttempts = value;
			}
		}

		public override byte[] SendRequest(byte[] requestMessage)
		{
			// UDP can fail - if it does try again keeping track of how many attempts we've made
			int attempts = 0;

			// try repeatedly in case of failure
			while (attempts <= udpRetryAttempts)
			{
				// firstly, uniquely mark this request with an id
				unchecked
				{
					// substitute in an id unique to this lookup, the request has no idea about this
					requestMessage[0] = (byte)(uniqueId >> 8);
					requestMessage[1] = (byte) uniqueId;
				}

				// we'll be send and receiving a UDP packet
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			
				// we will wait at most 1 second for a dns reply
				socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);

				// send it off to the server
				socket.SendTo(requestMessage, requestMessage.Length, SocketFlags.None, this.endpoint);
		
				// RFC1035 states that the maximum size of a UDP datagram is 512 octets (bytes)
				byte[] responseMessage = new byte[512];

				try
				{
					// wait for a response upto 1 second
					socket.Receive(responseMessage);

					// make sure the message returned is ours
					if (responseMessage[0] == requestMessage[0] && responseMessage[1] == requestMessage[1])
					{
						// its a valid response - return it, this is our successful exit point
						return responseMessage;
					}
				}
				catch (SocketException)
				{
					// failure - we better try again, but remember how many attempts
					attempts++;
				}
				finally
				{
					// increase the unique id
					uniqueId++;

					// close the socket
					socket.Close();
				}
			}

			// the operation has failed, this is our unsuccessful exit point
			throw new NoResponseException();
		}

	}
}
