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
	/// A Name Server Resource Record (RR) (RFC1035 3.3.11)
	/// </summary>
	public class SshFP : IRecord
	{
		// the fields exposed outside the assembly
		private readonly string	algorithm;
		private readonly string fingerPrintType;
		private readonly string fingerPrint;

		// expose this domain name address r/o to the world
		public string Algorithm	{ get { return algorithm; } }
		public string FingerPrintType { get { return this.fingerPrintType; } }
		public string FingerPrint { get { return this.fingerPrintType; } }
				
		/// <summary>
		/// Constructs a NS record by reading bytes from a return message
		/// </summary>
		/// <param name="pointer">A logical pointer to the bytes holding the record</param>
        public SshFP(Pointer pointer)
		{
			//detect algorithm
			byte a = pointer.ReadByte();
			switch(a)
			{
				case 0: this.algorithm = "Reserved"; break;
				case 1: this.algorithm = "RSA"; break;
				case 2: this.algorithm = "DSS"; break;
				default: this.algorithm = "Unknown"; break;
			}

			//detect fingerprint type
			byte fpt = pointer.ReadByte();
			switch(fpt)
			{
				case 0: this.fingerPrintType = "Reserved"; break;
				case 1: this.fingerPrintType = "RSA-1"; break;
				default: this.fingerPrintType = "Unknown"; break;
			}

			this.fingerPrint = pointer.ReadString();	//read the fingerprint (hex format)
		}

		public override string ToString()
		{
			return string.Format("Algorithm: {0}, {1} key: {2}", this.algorithm, this.fingerPrintType, this.fingerPrint);
		}
	}
}