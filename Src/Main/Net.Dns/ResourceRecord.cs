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
	/// Represents a Resource Record as detailed in RFC1035 4.1.3
	/// </summary>
	[Serializable]
	public class ResourceRecord
	{
		// private, constructor initialised fields
		private readonly string		domain;
		private readonly DnsType	dnsType;
		private readonly DnsClass	dnsClass;
		private readonly int		ttl;

		private readonly IRecord	record;

		private readonly int		length;

		// read only properties applicable for all records
		public string		Domain		{ get { return domain;		}}
		public DnsType		Type		{ get { return dnsType;	}}
		public DnsClass		Class		{ get { return dnsClass;	}}
		public int			Ttl			{ get { return ttl;		}}
		public IRecord		Record		{ get { return record;		}}
		public int			Length		{ get { return length;		}}

		/// <summary>
		/// Construct a resource record from a pointer to a byte array
		/// </summary>
		/// <param name="pointer">the position in the byte array of the record</param>
		internal ResourceRecord(Pointer pointer)
		{
			// extract the domain, question type, question class and Ttl
			domain = pointer.ReadDomain();
			dnsType = (DnsType)pointer.ReadShort();
			dnsClass = (DnsClass)pointer.ReadShort();
			ttl = pointer.ReadInt();

			// the next short is the record length
			length = pointer.ReadShort();

			// and create the appropriate RDATA record based on the dnsType
			switch (dnsType)
			{
				case DnsType.A:		record = new A(pointer);	break;
				case DnsType.AAAA:	record = new Aaaa(pointer);	break;
				case DnsType.Cname:	record = new Cname(pointer); break;
				case DnsType.HINFO:	record = new HInfo(pointer); break;
				case DnsType.LOC:	record = new Loc(pointer);	break;
				case DnsType.MX:	record = new MX(pointer);	break;
				case DnsType.NS:	record = new NS(pointer);	break;
				case DnsType.PTR:	record = new Ptr(pointer); break;
				case DnsType.SOA:	record = new Soa(pointer);	break;
				case DnsType.SRV:	record = new Srv(pointer); break;
				case DnsType.SSHFP:	record = new SshFP(pointer); break;
				case DnsType.TXT:	record = new Txt(pointer, length); break;
				case DnsType.NULL:	record = new Null(pointer, length); break;

				//mail related RR types
                case DnsType.MB:	record = new MB(pointer); break;
				case DnsType.MD:	record = new MD(pointer); break;
				case DnsType.MF:	record = new MF(pointer); break;
				case DnsType.MG:	record = new MG(pointer); break;
				case DnsType.MINFO:	record = new MInfo(pointer); break;

                //RFC 1183 RR types
                case DnsType.AFSDB: record = new Afsdb(pointer); break;
                case DnsType.RP:    record = new Rp(pointer); break;
				default:
				{
					// move the pointer over this unrecognised record
					pointer += length;
					break;
				}
			}
		}
	}

	// Answers, Name Servers and Additional Records all share the same RR format
	[Serializable]
	public class Answer : ResourceRecord
	{
		internal Answer(Pointer pointer) : base(pointer) {}
	}

	[Serializable]
	public class NameServer : ResourceRecord
	{
		internal NameServer(Pointer pointer) : base(pointer) {}
	}

	[Serializable]
	public class AdditionalRecord : ResourceRecord
	{
		internal AdditionalRecord(Pointer pointer) : base(pointer) {}
	}
}