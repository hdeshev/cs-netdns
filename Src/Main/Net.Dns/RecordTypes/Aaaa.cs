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
	public class Aaaa : IRecord
	{
		// An AAAA record consists simply of an 16 bytes array indicating an IP Address
		protected IPAddress ipAddress;

		// expose this IP address r/o to the world
		public IPAddress IPAddress
		{
			get { return ipAddress; }
		}

		/// <summary>
		/// Constructs an AAAA record by reading bytes from a return message
		/// </summary>
		/// <param name="pointer">A logical pointer to the bytes holding the record</param>
		public Aaaa(Pointer pointer)
		{
			byte[] b = pointer.ReadBytes(16);
			ipAddress = new IPAddress(b);
		}

		public override string ToString()
		{
			return ipAddress.ToString();
		}
	}
}
