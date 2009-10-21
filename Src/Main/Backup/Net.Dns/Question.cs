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
using System.Text.RegularExpressions;

namespace Net.Dns
{
	/// <summary>
	/// Represents a DNS Question, comprising of a domain to query, the type of query (QTYPE) and the class
	/// of query (QCLASS). This class is an encapsulation of these three things, and extensive argument checking
	/// in the constructor as this may well be created outside the assembly (public protection)
	/// </summary>
	[Serializable]
	public class Question
	{
		// A question is these three things combined
		private readonly string		domain;
		private readonly DnsType	dnsType;
		private readonly DnsClass	dnsClass;

		// expose them read/only to the world
		public string	Domain		{ get { return domain;		}}
		public DnsType	Type		{ get { return dnsType;	}}
		public DnsClass	Class		{ get { return dnsClass;	}}

        /// <summary>
        /// Construct the question from parameters, checking for safety
        /// </summary>
        /// <param name="domain">the domain name to query eg. bigdevelopments.co.uk</param>
        public Question(string domain) : this(domain, DnsType.Any, DnsClass.IN) { }


        /// <summary>
        /// Construct the question from parameters, checking for safety
        /// </summary>
        /// <param name="domain">the domain name to query eg. bigdevelopments.co.uk</param>
        /// <param name="dnsType">the QTYPE of query eg. DnsType.MX</param>
        public Question(string domain, DnsType dnsType) : this(domain, dnsType, DnsClass.IN) {}

        /// <summary>
		/// Construct the question from parameters, checking for safety
		/// </summary>
		/// <param name="domain">the domain name to query eg. bigdevelopments.co.uk</param>
		/// <param name="dnsType">the QTYPE of query eg. DnsType.MX</param>
		/// <param name="dnsClass">the CLASS of query, invariably DnsClass.IN</param>
		public Question(string domain, DnsType dnsType, DnsClass dnsClass)
		{
			// check the input parameters
			if (domain == null) throw new ArgumentNullException("domain");

			// do a sanity check on the domain name to make sure its legal
			if (domain.Length ==0 || domain.Length>255 || !Regex.IsMatch(domain, @"^[a-z|A-Z|0-9|\-|_]{1,63}(\.[a-z|A-Z|0-9|\-|_]{1,63})+$"))
			{
				// domain names can't be bigger tan 255 chars, and individal labels can't be bigger than 63 chars
				throw new ArgumentException("The supplied domain name was not in the correct form", "domain");
			}

			// sanity check the DnsType parameter
			if (!Enum.IsDefined(typeof(DnsType), dnsType) || dnsType == DnsType.None)
			{
				throw new ArgumentOutOfRangeException("dnsType", "Not a valid value");
			}

			// sanity check the DnsClass parameter
			if (!Enum.IsDefined(typeof(DnsClass), dnsClass) || dnsClass == DnsClass.None)
			{
				throw new ArgumentOutOfRangeException("dnsClass", "Not a valid value");
			}

			//DNS Reverse Pointers are a special type.
			if (dnsType == DnsType.PTR)
			{
				//domain should end in 'IN-ADDR.ARPA'
				if (domain.ToUpper().EndsWith(".IN-ADDR.ARPA") == false)
				{
					//Is domain an IP?
					try
					{
						IPAddress ip = IPAddress.Parse(domain);
						
						//reverse IP and suffix it with '.IN-ADDR.ARPA'
						string[] ib = domain.Split('.');
						domain = string.Format("{3}.{2}.{1}.{0}.IN-ADDR.ARPA", ib[0], ib[1], ib[2], ib[3]);
					}
					catch
					{
						throw new ArgumentException("Not a valid PTR address. Please use an IP or an FQDN PTR", "domain");
					}
				}
			}

			// just remember the values
			this.domain = domain;
			this.dnsType = dnsType;
			this.dnsClass = dnsClass;
		}

		/// <summary>
		/// Construct the question reading from a DNS Server response. Consult RFC1035 4.1.2
		/// for byte-wise details of this structure in byte array form
		/// </summary>
		/// <param name="pointer">a logical pointer to the Question in byte array form</param>
		internal Question(Pointer pointer)
		{
			// extract from the message
			this.domain = pointer.ReadDomain();
			this.dnsType = (DnsType)pointer.ReadShort();
			this.dnsClass = (DnsClass)pointer.ReadShort();
		}
	}
}
