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
	/// The DNS TYPE (RFC1035 3.2.2/3)
	/// </summary>
	public enum DnsType
	{
		None = 0, 
		/// <summary>
		/// a host address
		/// </summary>
		A = 1, 
		
		/// <summary>
		/// an authoritative name server
		/// </summary>
		NS = 2, 

		/// <summary>
		/// a mail destination (Obsolete - use MX)
		/// </summary>
		MD = 3,

		/// <summary>
		/// a mail forwarder (Obsolete - use MX)
		/// </summary>
		MF = 4,

		/// <summary>
		/// the canonical name for an alias
		/// </summary>
		Cname = 5,

		/// <summary>
		/// marks the start of a zone of authority
		/// </summary>
		SOA = 6, 

		
		/// <summary>
		/// a mailbox domain name (EXPERIMENTAL)
		/// </summary>
		MB = 7,

		/// <summary>
		/// a mail group member (EXPERIMENTAL)
		/// </summary>
		MG = 8,

		
		/// <summary>
		/// a mail rename domain name (EXPERIMENTAL)
		/// </summary>
		MR = 9,

		/// <summary>
		/// a null RR (EXPERIMENTAL)
		/// </summary>
		NULL = 10,

		/// <summary>
		/// a well known service description
		/// </summary>
		WKS = 11,

		/// <summary>
		/// a domain name pointer
		/// </summary>
		PTR = 12,

		/// <summary>
		/// host information
		/// </summary>
		HINFO = 13,

		/// <summary>
		/// mailbox or mail list information
		/// </summary>
		MINFO = 14,

		/// <summary>
		/// mail exchange
		/// </summary>
		MX = 15,

		/// <summary>
		/// text strings
		/// </summary>
		TXT = 16,

        /// <summary>
        /// Responsible Person
        /// </summary>
        RP = 17,

        /// <summary>
        /// AFS Data Base location
        /// </summary>
        AFSDB = 18,


		AAAA = 28,

		/// <summary>
		/// Geographicly location parameters
		/// </summary>
		LOC = 29,

		/// <summary>
		/// SSH Fingerprint
		/// </summary>
		SSHFP = 44,

		/// <summary>
		/// Service
		/// </summary>
		SRV = 33,


		//SPECIAL TYPES
		/// <summary>
		/// Incremental zone transfer
		/// </summary>
		IXFR = 251,

		/// <summary>
		/// A request for a transfer of an entire zone
		/// </summary>
		AFRX = 252,

		/// <summary>
		/// A request for mailbox-related records (MB, MG or MR)
		/// </summary>
		MAILB = 253,

		/// <summary>
		/// A request for mail agent RRs (Obsolete - see MX)
		/// </summary>
		MAILA = 254,

		/// <summary>
		/// A request for all records
		/// </summary>
		Any = 255
	}
}
