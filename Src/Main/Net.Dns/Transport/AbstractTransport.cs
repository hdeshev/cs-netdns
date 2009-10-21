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

namespace Net.Dns.Transport
{
	/// <summary>
	/// Summary description for AbstractTransport.
	/// </summary>
	public abstract class AbstractTransport : ITransport
	{
		protected readonly IPEndPoint endpoint;
		protected static int uniqueId;

		protected AbstractTransport(IPEndPoint endpoint)
		{
			this.endpoint = endpoint;
		}
		protected AbstractTransport(IPAddress address, int port)
		{
			this.endpoint = new IPEndPoint(address, port);
		}
		protected AbstractTransport(IPAddress address)
		{
			this.endpoint = new IPEndPoint(address, 53);
		}

		public abstract byte[] SendRequest(byte[] requestMessage);

	}
}
