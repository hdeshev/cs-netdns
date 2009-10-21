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
	/// (RFC1035 4.1.1) These are the return codes the server can send back
	/// </summary>
	public enum ReturnCode
	{
		/// <summary>
		/// No Error condition
		/// </summary>
		Success = 0,

		/// <summary>
		/// The name server was unable to interpret the query.
		/// </summary>
		FormatError = 1,

		/// <summary>
		/// The name server was unable to process this query due to a problem with the name server.
		/// </summary>
		ServerFailure = 2,

		/// <summary>
		/// Meaningful only for responses from an authoritative name server, this code signifies that the 
		/// domain name referenced in the query does not exist.
		/// </summary>
		NameError = 3,

		/// <summary>
		/// The name server does not support the requested kind of query
		/// </summary>
		NotImplemented = 4,

		/// <summary>
		/// The name server refuses to perform the specified operation for policy reasons.  
		/// For example, a name server may not wish to provide the information to the particular 
		/// requester, or a name server may not wish to perform a particular operation 
		/// (e.g., zone transfer) for particular data.
		/// </summary>
		Refused = 5,

		/// <summary>
		/// Reserved for future use.
		/// </summary>
		Other = 6
	}
}
