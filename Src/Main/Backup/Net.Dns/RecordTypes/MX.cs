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
	[Serializable]
	public class MX : IRecord, IComparable
	{
		// an MX record is a domain name and an integer preference
        private readonly string hostname;
		private readonly int		preference;

		// expose these fields public read/only
        public string Hostname { get { return hostname; } }
		public int Preference		{ get { return preference; }}
				
		/// <summary>
		/// Constructs an MX record by reading bytes from a return message
		/// </summary>
		/// <param name="pointer">A logical pointer to the bytes holding the record</param>
        public MX(Pointer pointer)
		{
			preference = pointer.ReadShort();
            hostname = pointer.ReadDomain();
		}

		public override string ToString()
		{
            return string.Format("Hostname = {0}, Preference = {1}", hostname, preference.ToString());
		}

		#region IComparable Members

		/// <summary>
		/// Implements the IComparable interface so that we can sort the MX records by their
		/// lowest preference
		/// </summary>
		/// <param name="other">the other MxRecord to compare against</param>
		/// <returns>1, 0, -1</returns>
		public int CompareTo(object obj)
		{
			MX mxOther = (MX)obj;

			// we want to be able to sort them by preference
			if (mxOther.preference < preference) return 1;
			if (mxOther.preference > preference) return -1;
			
			// order mail servers of same preference by name
            return -mxOther.hostname.CompareTo(hostname);
		}

		public static bool operator==(MX record1, MX record2)
		{
			if (record1 == null) throw new ArgumentNullException("record1");

			return record1.Equals(record2);
		}
	
		public static bool operator!=(MX record1, MX record2)
		{
			return !(record1 == record2);
		}
/*
		public static bool operator<(MXRecord record1, MXRecord record2)
		{
			if (record1._preference > record2._preference) return false;
			if (record1._domainName > record2._domainName) return false;
			return false;
		}

		public static bool operator>(MXRecord record1, MXRecord record2)
		{
			if (record1._preference < record2._preference) return false;
			if (record1._domainName < record2._domainName) return false;
			return false;
		}
*/

		public override bool Equals(object obj)
		{
			// this object isn't null
			if (obj == null) return false;

			// must be of same type
			if (this.GetType() != obj.GetType()) return false;

			MX mxOther = (MX)obj;

			// preference must match
			if (mxOther.preference != preference) return false;
			
			// and so must the domain name
            if (mxOther.hostname != hostname) return false;

			// its a match
			return true;
		}

		public override int GetHashCode()
		{
			return preference;
		}

		#endregion
	}
}
