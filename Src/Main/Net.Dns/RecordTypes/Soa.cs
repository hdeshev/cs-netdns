/*
Version 2, June 1991

Copyright (C) 1989, 1991 Free Software Foundation, Inc.  
51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA

Everyone is permitted to copy and distribute verbatim copies
of this license document, but changing it is not allowed.

A full copy of the license can be obtained at: http://www.gnu.org/licenses/gpl.txt 
*/
using System;

namespace Net.Dns
{
	/// <summary>
	/// An SOA Resource Record (RR) (RFC1035 3.3.13)
	/// </summary>
	public class Soa : IRecord
	{
		// these fields constitute an SOA RR
		private readonly string	primaryNameServer;
		private readonly string	responsibleMailAddress;
		private readonly int	serial;
		private readonly int	refresh;
		private readonly int	retry;
		private readonly int	expire;
		private readonly int	defaultTtl;

		// expose these fields public read/only
		public string PrimaryNameServer			{ get { return primaryNameServer;		}}
		public string ResponsibleMailAddress	{ get { return responsibleMailAddress; }}
		public int Serial						{ get { return serial;					}}
		public int Refresh						{ get { return refresh;				}}
		public int Retry						{ get { return retry;					}}
		public int Expire						{ get { return expire;					}}
		public int DefaultTtl					{ get { return defaultTtl;				}}

		/// <summary>
		/// Constructs an SOA record by reading bytes from a return message
		/// </summary>
		/// <param name="pointer">A logical pointer to the bytes holding the record</param>
        public Soa(Pointer pointer) 
		{
			// read all fields RFC1035 3.3.13
			primaryNameServer = pointer.ReadDomain();
			responsibleMailAddress = pointer.ReadDomain();
			serial = pointer.ReadInt();
			refresh = pointer.ReadInt();
			retry = pointer.ReadInt();
			expire = pointer.ReadInt();
			defaultTtl = pointer.ReadInt();
		}

		public override string ToString()
		{
			return string.Format("primary name server = {0}\nresponsible mail addr = {1}\nserial  = {2}\nrefresh = {3}\nretry   = {4}\nexpire  = {5}\ndefault TTL = {6}",
				primaryNameServer,
				responsibleMailAddress,
				serial.ToString(),
				refresh.ToString(),
				retry.ToString(),
				expire.ToString(),
				defaultTtl.ToString());
		}
	}
}
