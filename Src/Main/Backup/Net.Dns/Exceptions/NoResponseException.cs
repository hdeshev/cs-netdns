/*
Version 2, June 1991

Copyright (C) 1989, 1991 Free Software Foundation, Inc.  
51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA

Everyone is permitted to copy and distribute verbatim copies
of this license document, but changing it is not allowed.

A full copy of the license can be obtained at: http://www.gnu.org/licenses/gpl.txt 
*/
using System;
using System.Runtime.Serialization;

namespace Net.Dns
{
	/// <summary>
	/// Thrown when the server does not respond
	/// </summary>
	[Serializable]
	public class NoResponseException : SystemException
	{
		public NoResponseException()
		{
			// no implementation
		}

		public NoResponseException(Exception innerException) :  base(null, innerException) 
		{
			// no implementation
		}

		public NoResponseException(string message, Exception innerException) : base (message, innerException)
		{
			// no implementation
		}
        
		protected NoResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			// no implementation
		}
	}
}