/*
Version 2, June 1991

Copyright (C) 1989, 1991 Free Software Foundation, Inc.  
51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA

Everyone is permitted to copy and distribute verbatim copies
of this license document, but changing it is not allowed.

A full copy of the license can be obtained at: http://www.gnu.org/licenses/gpl.txt 
*/
using System;
using System.Collections;
using System.Net;
using System.Runtime.InteropServices;

namespace Network
{
	/// <summary>
	/// Summary description for HostInformation.
	/// </summary>
	public class Discover
	{
		private readonly string hostname;
		private readonly string domainname;
		private readonly IPAddress[] dnsServers;
		
		private const int MAX_ADAPTER_NAME = 128; 
		private const int MAX_HOSTNAME_LEN = 128; 
		private const int MAX_DOMAIN_NAME_LEN = 128; 
		private const int MAX_SCOPE_ID_LEN = 256; 
		private const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128; 
		private const int MAX_ADAPTER_NAME_LENGTH = 256; 
		private const int MAX_ADAPTER_ADDRESS_LENGTH = 8; 
		private const int DEFAULT_MINIMUM_ENTITIES = 32;

		private const int ERROR_BUFFER_OVERFLOW = 111; 
		private const int ERROR_INSUFFICIENT_BUFFER = 122; 
		private const int ERROR_SUCCESS = 0;

		[DllImport("Iphlpapi.dll", CharSet=CharSet.Auto)]	
		private static extern int GetNetworkParams(Byte[] PFixedInfoBuffer, ref int size); 

		[DllImport("Kernel32.dll", EntryPoint="CopyMemory")] 
		private	static extern void ByteArray_To_FixedInfo(ref FixedInfo	dst, Byte[]	src, int size);	

		[DllImport("Kernel32.dll", EntryPoint="CopyMemory")] 
		private	static extern void IntPtr_To_IPAddrString(ref IPAddrString dst,	IntPtr src,	int	size); 
		
		public Discover()
		{
			int size=0; 
			int r = GetNetworkParams(null, ref size); 
			if ((r != ERROR_SUCCESS) && (r != ERROR_BUFFER_OVERFLOW)) 
				throw new Exception("Error invoking GetNetworkParams() : " + r);
			
			Byte[] buffer= new Byte[size]; 
			r = GetNetworkParams(buffer, ref size); 
			if (r != ERROR_SUCCESS) 
				throw new Exception("Error invoking GetNetworkParams() " + r); 
			
			FixedInfo PFixedInfo= new FixedInfo(); 
			ByteArray_To_FixedInfo(ref PFixedInfo, buffer, Marshal.SizeOf(PFixedInfo)); 

			this.hostname = PFixedInfo.HostName;
			this.domainname = PFixedInfo.DomainName;
			
			ArrayList ips = new ArrayList();
			ips.Add(IPAddress.Parse(PFixedInfo.DnsServerList.IPAddressString));
			
			IPAddrString ListItem = new IPAddrString(); 
			IntPtr ListNext = new IntPtr(); 
			
			ListNext = PFixedInfo.DnsServerList.NextPointer; 
			
			while (ListNext.ToInt32() != 0) 
			{ 
				IntPtr_To_IPAddrString(ref ListItem, ListNext, Marshal.SizeOf(ListItem)); 
				ips.Add(IPAddress.Parse(ListItem.IPAddressString));
				ListNext = ListItem.NextPointer; 
			}			

			this.dnsServers = (IPAddress[]) ips.ToArray(typeof(IPAddress));


		}

		public string Hostname { get { return this.hostname; } }
		public string DomainName { get { return this.domainname; } }
		public IPAddress[] DnsServers { get { return this.dnsServers; } }



		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)] 
		internal struct IPAddrString 
		{ 
			public IntPtr NextPointer; 
		
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=4 * 4)] 
			public String IPAddressString; 
		
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=4 * 4)] 
			public String IPMaskString; 
			public int Context; 
		}


		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)] 
		internal struct FixedInfo 
		{ 
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=Discover.MAX_HOSTNAME_LEN + 4)] 
			public String HostName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Discover.MAX_DOMAIN_NAME_LEN + 4)] 
			public String DomainName; 
		
			public IntPtr CurrentServerList; 
			public IPAddrString DnsServerList; 
			public NodeType_T NodeType;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Discover.MAX_SCOPE_ID_LEN + 4)] 
			public String ScopeId; 
			public int EnableRouting; 
			public int EnableProxy; 
			public int EnableDns; 
		}

		internal enum NodeType_T { Broadcast = 1, PeerToPeer= 2, Mixed= 4, Hybrid= 8 }

	}

}
