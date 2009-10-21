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
	/// 
	/// </summary>
	[Serializable]
	public class Srv : IRecord
	{
		// an MX record is a domain name and an integer preference
        private readonly string hostname;
		private readonly int		priority;
		private readonly int		weight;
		private readonly int		port;

		// expose these fields public read/only
        public string Hostname { get { return this.hostname; } }
		public int Priority		{ get { return this.priority; }}
		public int Weight { get { return this.weight; } }
		public int Port { get { return this.port; } }

				
		/// <summary>
		/// Constructs an MX record by reading bytes from a return message
		/// </summary>
		/// <param name="pointer">A logical pointer to the bytes holding the record</param>
        public Srv(Pointer pointer)
		{
			this.priority = pointer.ReadShort();
			this.weight = pointer.ReadShort();
			this.port = pointer.ReadShort();
            this.hostname = pointer.ReadDomain();
		}

		public override string ToString()
		{
            return string.Format("Priority: {0}, Weight: {1}, Port: {2}, Host: {3}", this.priority.ToString(), this.weight.ToString(), this.port.ToString(), this.hostname);
		}
	}
}
