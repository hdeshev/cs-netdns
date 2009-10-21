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
	/// An MX (Mail Exchanger) Resource Record (RR) (RFC1035 3.3.9)
	/// </summary>
	public class Ptr : IRecord
	{
		// the fields exposed outside the assembly
        private readonly string hostname;

		// expose this domain name address r/o to the world
        public string Hostname { get { return hostname; } }
				
		/// <summary>
		/// Constructs a NS record by reading bytes from a return message
		/// </summary>
		/// <param name="pointer">A logical pointer to the bytes holding the record</param>
        public Ptr(Pointer pointer)
		{
            hostname = pointer.ReadDomain();
		}

		public override string ToString()
		{
            return hostname;
		}
	}
}