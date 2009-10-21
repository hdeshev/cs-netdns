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
	/// The DNS CLASS (RFC1035 3.2.4/5)
	/// Internet will be the one we'll be using (IN), the others are for completeness
	/// </summary>
	public enum DnsClass
	{
		None = 0, 
		
		/// <summary>
		/// the Internet
		/// </summary>
		IN = 1, 

		/// <summary>
		/// the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
		/// </summary>
		CS = 2, 

		/// <summary>
		/// the CHAOS class
		/// </summary>
		CH = 3, 

		/// <summary>
		/// Hesiod [Dyer 87]
		/// </summary>
		HS = 4,

		/// <summary>
		/// any class
		/// </summary>
		ANY = 255
	}

}
