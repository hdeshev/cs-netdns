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
    /// AFS Data Base location.
    /// The AFS (originally the Andrew File System) system uses the DNS to
    /// map from a domain name to the name of an AFS cell database server.
    /// The DCE Naming service uses the DNS for a similar function: mapping
    /// from the domain name of a cell to authenticated name servers for that
    /// cell.
	/// </summary>
	public class Afsdb : IRecord
	{
		// the fields exposed outside the assembly
		private readonly string	hostname;
        private readonly int pref;

		// expose this domain name address r/o to the world
        public string Hostname { get { return this.hostname; } }
        public int Preference { get { return this.pref; } }

				
		/// <summary>
		/// Constructs a NS record by reading bytes from a return message
		/// </summary>
		/// <param name="pointer">A logical pointer to the bytes holding the record</param>
        internal Afsdb(Pointer pointer)
		{
            pref = pointer.ReadShort();
            hostname = pointer.ReadDomain();
		}

		public override string ToString()
		{
            return string.Format("Preference: {0}, Hostname: {1}", pref, hostname);
		}
	}
}