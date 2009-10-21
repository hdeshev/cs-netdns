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
using System.Net.Sockets;
using Net.Dns;

namespace Net.Dns
{
	/// <summary>
	/// Summary description for Dns.
	/// </summary>
	public class Resolver
	{
        protected ITransport transport;

		/// <summary>
		/// </summary>
		public Resolver()
		{
		}

        public Resolver(ITransport transport)
        {
            this.transport = transport;
        }

        public ITransport Transport
        {
            get { return this.transport; }
            set { this.transport = value; }
        }

		/// <summary>
		/// Shorthand form to make MX querying easier, essentially wraps up the retreival
		/// of the MX records, and sorts them by preference
		/// </summary>
		/// <param name="domain">domain name to retreive MX RRs for</param>
		/// <param name="dnsServer">the server we're going to ask</param>
		/// <returns>An array of MXRecords</returns>
        /*
		public static MX[] MXLookup(string domain, IPAddress dnsServer)
		{
			// check the inputs
			if (domain == null) throw new ArgumentNullException("domain");
			if (dnsServer == null)  throw new ArgumentNullException("dnsServer");

			// create a request for this
			Request request = new Request();

			// add one question - the MX IN lookup for the supplied domain
			request.AddQuestion(new Question(domain, DnsType.MX, DnsClass.IN));
			
			// fire it off
			Response response = Lookup(request, dnsServer);

			// if we didn't get a response, then return null
			if (response == null) return null;
				
			// create a growable array of MX records
			ArrayList resourceRecords = new ArrayList();

			// add each of the answers to the array
			foreach (Answer answer in response.Answers)
			{
				// if the answer is an MX record
				if (answer.Record.GetType() == typeof(MX))
				{
					// add it to our array
					resourceRecords.Add(answer.Record);
				}
			}

			// create array of MX records
			MX[] mxRecords = new MX[resourceRecords.Count];

			// copy from the array list
			resourceRecords.CopyTo(mxRecords);

			// sort into lowest preference order
			Array.Sort(mxRecords);

			// and return
			return mxRecords;
		}
         */

		/// <summary>
		/// The principal look up function, which sends a request message to the given
		/// DNS server and collects a response. This implementation re-sends the message
		/// via UDP up to two times in the event of no response/packet loss
		/// </summary>
		/// <param name="request">The logical request to send to the server</param>
		/// <param name="dnsServer">The IP address of the DNS server we are querying</param>
		/// <returns>The logical response from the DNS server or null if no response</returns>
		public Response Lookup(Request request)
		{
			// check the inputs
			if (request == null) throw new ArgumentNullException("request");
			//if (dnsServer == null) throw new ArgumentNullException("dnsServer");
			
			// We will not catch exceptions here, rather just refer them to the caller
		
			// get the message
			byte[] requestMessage = request.GetMessage();

			// send the request and get the response
            //ITransport transport = new UdpTransport(dnsServer, 53);
            //ITransport transport = new TcpTransport(dnsServer, 53);
			byte[] responseMessage = transport.SendRequest(requestMessage);

			// and populate a response object from that and return it
			return new Response(responseMessage);
		}
	}
}
