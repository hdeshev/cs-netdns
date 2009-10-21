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
	/// A Host Info  Resource Record (RR) (RFC1035 3.3.11)
	/// </summary>
	public class HInfo : IRecord
	{
		// the fields exposed outside the assembly
		protected  string		cpu;
		protected  string		os;

		// expose this domain name address r/o to the world
		public string Cpu { get { return this.cpu; } }
		public string OS { get { return this.os; } }
				
		/// <summary>
		/// Constructs a HINFO record by reading bytes from a return message
		/// </summary>
		/// <param name="pointer">A logical pointer to the bytes holding the record</param>
		public HInfo(Pointer pointer)
		{
			this.cpu = pointer.ReadString();
			this.os = pointer.ReadString();
		}

		public override string ToString()
		{
			return string.Format("CPU: {0}, OS: {1}", this.cpu, this.os);
		}
	}
}