using System;
using System.IO;

namespace Net.Common
{
	public delegate void DataAvailableHandler();

	/// <summary>
	/// A ReadWriteStream supports writing and reading at the same time. This stream is platform independed Byte Order stream. 
	/// However by default we assume x86 hardware, which means reading in Host Byte Order (little endian) and writing in Network Byte Order (big endian)
	/// </summary>
	public class ReadWriteStream : Stream, ICloneable
	{
		private const int IncreaseSize = 1024;
		protected byte[] stream;
		protected long length;
		protected long readPosition;
		protected long writePosition;
		public event DataAvailableHandler DataAvailable;

		public ReadWriteStream()  
		{
			this.stream = new byte[1024];
			this.readPosition = 0;
			this.writePosition = 0;
			this.length = 0;
		}

		public ReadWriteStream(byte[] data)
		{
			this.stream = data;
			this.readPosition = 0;
			this.writePosition = data.Length;
			this.length = data.Length;
		}
		public ReadWriteStream(byte[] data, long readPosition, long writePosition)
		{
			this.stream = data;
			this.readPosition = readPosition;
			this.writePosition = writePosition;
			this.length = data.Length;
		}
		public ReadWriteStream(byte[] data, long readPosition, long writePosition, long length)
		{
			this.stream = data;
			this.readPosition = readPosition;
			this.writePosition = writePosition;
			this.length = length;
		}

		/// <summary>
		/// An 'deep' copied ReadWriteStream instance including any 'DataAvailable' listners
		/// </summary>
		/// <returns></returns>
		public ReadWriteStream Copy()
		{	
			ReadWriteStream rws = new ReadWriteStream(this.stream, this.readPosition, this.writePosition, this.length);
			rws.DataAvailable = this.DataAvailable;	//attach DataAvailable listners

			return rws;	
		}
		/// <summary>
		/// A 'deep' copied ReadWriteStream of this instance without any 'DataAvailable' listners.
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return new ReadWriteStream(this.stream, this.readPosition, this.writePosition, this.length);
		}

		public override long Length { get { return this.length; } }
		public override bool CanRead { get { return true; } }
		public override bool CanWrite { get { return true; } }
		public override bool CanSeek { get { return true; } }
		public override void Close() { }
		public override void Flush() { }


		[Obsolete("Use ReadPosition of WritePosition", true)]
		public override long Position { get { return 0; } set { } }
		public long ReadPosition
		{
			get { return this.readPosition; }
			set { this.readPosition = value; }
		}
		public long WritePosition
		{
			get { return this.writePosition; }
			set { this.writePosition = value; }
		}

		[Obsolete("Use ReadSeek of WriteSeek", true)]
		public override long Seek(long offset, SeekOrigin origin) { return 0; }

		public override int Read(byte[] buffer, int offset, int count)
		{
			long length = count;
			if (this.length - this.readPosition < count)
				length = (long)this.length - this.readPosition;

			Array.Copy(this.stream, this.readPosition, buffer, offset, length);
			this.readPosition += length;
			return (int) length;
		}
		public override int ReadByte()
		{
			return this.stream[this.readPosition++];
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			CheckStreamSize(count);
			Array.Copy(buffer, offset, this.stream, this.writePosition, count);
			this.writePosition += count;
			this.length += count;
			OnDataAvailable();
		}

		public override void WriteByte(byte value)
		{
			CheckStreamSize(1);
			this.stream[this.writePosition++] = value;
			this.length++;
			OnDataAvailable();
		}

		protected void CheckStreamSize(int length)
		{
			if (this.writePosition + length > this.length)
				SetLength(this.writePosition + length + ReadWriteStream.IncreaseSize);
			
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public override void SetLength(long newLength)
		{
			if (newLength < this.length)
				throw new ArgumentOutOfRangeException("newLength", newLength, "Truncating of data in the stream is not supported. The value of 'newLength' needs to be larger or equal to 'ReadWriteStream.Length'");

			byte[] buffer = new byte[newLength];
			Array.Copy(this.stream, 0, buffer, 0, this.length);
			this.stream = buffer;
		}


		protected void OnDataAvailable()
		{
			if (this.DataAvailable != null)
				this.DataAvailable();
		}

		public byte[] GetBytes()
		{
			byte[] buffer = new byte[this.length];
			Array.Copy(this.stream, 0, this.buffer, 0, this.length);
			return buffer;
		}
	}

	public enum EndianType
	{
		/// <summary>
		/// LittleEndian uses left to right reading/writing for numbers. This method is standard used on x86 hardware
		/// </summary>
		LittleEndian,

		/// <summary>
		/// BigEndian uses right-to-left notation for numbers. BigEndian is also known als Network Byte Order.
		/// </summary>
		BigEndian
	}
}
