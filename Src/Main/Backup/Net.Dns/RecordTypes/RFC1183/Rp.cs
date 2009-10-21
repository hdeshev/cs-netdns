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
	/// Responsible Person
	/// </summary>
	public class Rp : IRecord
	{
		// the fields exposed outside the assembly
		private readonly string		mailboxHostname;
        private readonly string     textHostname;


        public string MailboxHostname { get { return this.mailboxHostname; } }
        public string TextHostname { get { return this.textHostname; } }

				
		/// <summary>
		/// Constructs a NS record by reading bytes from a return message
		/// </summary>
		/// <param name="pointer">A logical pointer to the bytes holding the record</param>
        internal Rp(Pointer pointer)
		{
            this.mailboxHostname = pointer.ReadDomain();
            this.textHostname = pointer.ReadDomain();
		}

		public override string ToString()
		{
			return string.Format("Mailbox: {0}, Text: {1}", mailboxHostname, textHostname);
		}
	}
}