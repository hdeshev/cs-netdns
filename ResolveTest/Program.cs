using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Net.Common;
using Net.Dns;
using Net.Dns.Transport;
using Network;

namespace ResolveTest
{
	class Program
	{
		static void Main(string[] args)
		{
			//prepare DNS query
			var domains = new[] { "wildbit.com", "_domainkey.wildbit.com", "m._domainkey.wildbit.com" };
			foreach (var domain in domains)
			{
				var result = DnsTxtQuery(domain);
				Console.WriteLine("TXT query for {0} got us: {1}", domain, result);
			}
		}

		private static string DnsTxtQuery(string domain)
		{
			Request req = new Request();
			req.AddQuestion(new Question(domain, DnsType.TXT));

			var discover = new Discover();
			ITransport transport = new UdpTransport(discover.DnsServers[0]);
			
			var resolver = new Resolver(transport);
			Response response = resolver.Lookup(req);

			if (response.ReturnCode != ReturnCode.Success)
				throw new Exception("Could not query the DNS server");

			var answer = response.Answers[0];
			
			if (answer.Type != DnsType.TXT)
				throw new Exception("Invalid DNS response!");

			var data = answer.Record as Txt;
			return data.Text;
		}
	}
}
