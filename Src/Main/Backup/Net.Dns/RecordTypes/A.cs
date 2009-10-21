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

namespace Net.Dns
{
	/// <summary>
	/// ANAME Resource Record (RR) (RFC1035 3.4.1)
	/// </summary>
	public class A : IRecord
	{
		// An ANAME records consists simply of an IP address
		protected IPAddress ipAddress;

		// expose this IP address r/o to the world
		public IPAddress IPAddress
		{
			get { return ipAddress; }
		}

		/// <summary>
		/// Constructs an ANAME record by reading bytes from a return message
		/// </summary>
		/// <param name="pointer">A logical pointer to the bytes holding the record</param>
		public A(Pointer pointer)
		{
            byte[] bytes = pointer.ReadBytes(4);
            ipAddress = new IPAddress(bytes);
		}

		public override string ToString()
		{
			return ipAddress.ToString();
		}
	}
}
