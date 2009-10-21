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
	/// A Response is a logical representation of the byte data returned from a DNS query
	/// </summary>
	public class Response
	{
		// these are fields we're interested in from the message
		private readonly ReturnCode			returnCode;
		private readonly bool				authoritativeAnswer;
		private readonly bool				recursionAvailable;
		private readonly bool				truncated;
		private readonly Question[]			questions;
		private readonly Answer[]			answers;
		private readonly NameServer[]		nameServers;
		private readonly AdditionalRecord[]	additionalRecords;

		// these fields are readonly outside the assembly - use r/o properties
		public ReturnCode ReturnCode				{ get { return returnCode;				}}
		public bool AuthoritativeAnswer				{ get { return authoritativeAnswer;		}}
		public bool RecursionAvailable				{ get { return recursionAvailable;		}}
		public bool MessageTruncated				{ get { return truncated;				}}
		public Question[] Questions					{ get { return questions;				}}
		public Answer[] Answers						{ get { return answers;					}}
		public NameServer[] NameServers				{ get { return nameServers;				}}
		public AdditionalRecord[] AdditionalRecords	{ get { return additionalRecords;		}}

		/// <summary>
		/// Construct a Response object from the supplied byte array
		/// </summary>
		/// <param name="message">a byte array returned from a DNS server query</param>
		internal Response(byte[] message)
		{
			// the bit flags are in bytes 2 and 3
			byte flags1 = message[2];
			byte flags2 = message[3];

			// get return code from lowest 4 bits of byte 3
			int returnCode_ = flags2 & 15;
				
			// if its in the reserved section, set to other
			if (returnCode_ > 6) returnCode_ = 6;
			returnCode = (ReturnCode)returnCode_;

			// other bit flags
			authoritativeAnswer = ((flags1 & 4) != 0);
			recursionAvailable = ((flags2 & 128) != 0);
			truncated = ((flags1 & 2) != 0);

			// create the arrays of response objects
			questions = new Question[GetShort(message, 4)];
			answers = new Answer[GetShort(message, 6)];
			nameServers = new NameServer[GetShort(message, 8)];
			additionalRecords = new AdditionalRecord[GetShort(message, 10)];

			// need a pointer to do this, position just after the header
			Pointer pointer = new Pointer(message, 12);

			// and now populate them, they always follow this order
			for (int index = 0; index < questions.Length; index++)
			{
				try
				{
					// try to build a quesion from the response
					questions[index] = new Question(pointer);
				}
				catch (Exception ex)
				{
					// something grim has happened, we can't continue
					throw new InvalidResponseException(ex);
				}
			}
			for (int index = 0; index < answers.Length; index++)
			{
				answers[index] = new Answer(pointer);
			}
			for (int index = 0; index < nameServers.Length; index++)
			{
				nameServers[index] = new NameServer(pointer);
			}
			for (int index = 0; index < additionalRecords.Length; index++)
			{
				additionalRecords[index] = new AdditionalRecord(pointer);
			}
		}
	
		/// <summary>
		/// Convert 2 bytes to a short. It would have been nice to use BitConverter for this,
		/// it however reads the bytes in the wrong order (at least on Windows)
		/// </summary>
		/// <param name="message">byte array to look in</param>
		/// <param name="position">position to look at</param>
		/// <returns>short representation of the two bytes</returns>
		private static short GetShort(byte[] message, int position)
		{
			return (short)(message[position]<<8 | message[position+1]);
		}
	}
}
