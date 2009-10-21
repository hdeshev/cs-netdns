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
using System.IO;

namespace Net.Common
{
    /// <summary>
    /// ByteStream is the replacement of the 'Pointer' class which is 'readonly'.
    /// Bytes stream reads and writes in network byte order (big endian). x86 hardware is by default little endian.
    /// This stream is basicly a wrapper with c# versions of the common 'C' htons, htonl, ntohs and ntohl macros (host-to-network)
    /// </summary>
    class ByteStream : Stream
    {
        protected ByteStreamAccess access = ByteStreamAccess.Read;
        protected byte[] stream;
        protected long position;
        protected long maxLength = long.MaxValue;

        public ByteStream(byte[] data)
        {
            this.stream = data;
            this.position = 0;
        }
        public ByteStream(byte[] data, long position)
        {
            this.stream = data;
            this.position = position;
        }
        public ByteStream(byte[] data, long position, ByteStreamAccess access)
        {
            this.stream = data;
            this.position = position;
            this.access = access;
        }

        /// <summary>
        /// Position of the stream pointer
        /// </summary>
        public override long Position
        {
            get { return this.position; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("position", "Position can't be negative");
                if (value > this.stream.Length)
                    throw new ArgumentOutOfRangeException("position");

                this.position = value;
            }
        }

        /// <summary>
        /// Returns a (deep) copy of the ByteStream instance
        /// </summary>
        /// <returns>A new ByteStream instance</returns>
        public ByteStream Copy()
        {
            return new ByteStream(this.stream, this.position, this.access);
        }

        /// <summary>
        /// Return the next byte in the stream with changing the position pointer
        /// </summary>
        /// <returns>next byte in stream</returns>
        public byte Peek()
        {
            if (!CanRead)
                throw new IOException("The stream is opened in 'write' access. Reading is not possible");

            return this.stream[this.position];
        }

        /// <summary>
        /// Positions the pointer position in relation with 'origin'.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin: this.position = offset; break;
                case SeekOrigin.Current: this.position += offset; break;
                case SeekOrigin.End: this.position = this.Length - offset; break;
            }
            return this.position;
        }
        
        /// <summary>
        /// Sets the maximum length of the stream
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            this.maxLength = value;
        }

        /// <summary>
        /// Length of the stream
        /// </summary>
        public override long Length { get { return this.stream.Length; } }
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!CanRead)
                throw new IOException("The stream is opened in 'write' access. Reading is not possible");

            long length = count;
            if (this.stream.Length - this.position < count)
                length = (long)this.stream.Length - this.position;

            Array.Copy(this.stream, this.position, buffer, offset, Length);
            this.position += length;
            return (int) length;
        }

        /// <summary>
        /// Reads a single byte from the stream
        /// </summary>
        /// <returns></returns>
        public new byte ReadByte()
        {
            if (!CanRead)
                throw new IOException("The stream is opened in 'write' access. Reading is not possible");

            return this.stream[this.position++];
        }

        /// <summary>
        /// Return heximal encoding of the byte
        /// </summary>
        /// <returns></returns>
        public string ReadBinaryCharacter()
        {
            return ReadByte().ToString("X2");
        }
        /// <summary>
        /// Reads a 16-bit integer from the stream
        /// </summary>
        /// <returns>A 16-bit integer (short)</returns>
        public short ReadShort()
        {
            byte[] data = new byte[2];
            this.Read(data, 0, 2);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);    //reverse byte order

            return BitConverter.ToInt16(data, 2);
        }

        /// <summary>
        /// Reads a 32-bit integer from the stream
        /// </summary>
        /// <returns>A 32-bit integer (int)</returns>
        public int ReadInt()
        {
            byte[] data = new byte[4];
            this.Read(data, 0, 4);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);    //reverse byte order

            return BitConverter.ToInt32(data, 2);

        }

        /// <summary>
        /// Reads a 64-bit integer from the stream
        /// </summary>
        /// <returns>A 64-bit integer (long)</returns>
        public long ReadLong()
        {
            byte[] data = new byte[8];
            this.Read(data, 0, 8);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);    //reverse byte order

            return BitConverter.ToInt64(data, 2);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!CanWrite)
                throw new IOException("The stream is opened in 'read' access. Writing is not possible");

            long length = count;
            if (length > (buffer.Length - offset))
                length = buffer.Length - offset;

            //Read the 'buffer' till the stream maximum is reached
            if (length > this.maxLength)
                length = this.maxLength - this.stream.Length;

            if (length == 0)
                throw new IOException("Maximum size of stream is reached!");

            byte[] data = new byte[this.stream.Length + length];

            //Merge current stream with 'buffer' array
            Array.Copy(this.stream, 0, data, 0, this.stream.Length);
            Array.Copy(buffer, offset, data, this.stream.Length, length);

            //assign new byte array and reset position
            this.stream = data;
            this.position = data.Length;

        }
        /// <summary>
        /// Writes a single (raw) byte to the stream
        /// </summary>
        /// <param name="value"></param>
        public override void WriteByte(byte value)
        {
            if (!CanWrite)
                throw new IOException("The stream is opened in 'read' access. Writing is not possible");

            if (this.stream.Length + 1 > this.maxLength)
                throw new IOException("Maximum size of stream is reached!");

            ArrayList s = new ArrayList(this.stream.Length + 1);
            s.AddRange(this.stream);
            s.Add(value);
            byte[] data = new byte[s.Count];
            data.CopyTo(data, 0);

            //assign new byte array and reset position
            this.stream = data;
            this.position = data.Length;

        }

        /// <summary>
        /// Writes a 16 bit integer in network byte order to the stream
        /// </summary>
        /// <param name="value">Value to write</param>
        public void WriteShort(short value)
        {
            byte[] v = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(v);   //reverse byte

            this.Write(v, 0, v.Length);
        }

        /// <summary>
        /// Writes an 32-bit integer in network byte order to the stream
        /// </summary>
        /// <param name="value">32-bit value to write</param>
        public void WriteInt(int value)
        {
            byte[] v = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(v);   //reverse byte

            this.Write(v, 0, v.Length);
        }

        /// <summary>
        /// Writes an 64-bit integer in network byte order (big endian) to the stream
        /// </summary>
        /// <param name="value"></param>
        public void WriteLong(long value)
        {
            byte[] v = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(v);   //reverse byte

            this.Write(v, 0, v.Length);
        }


        public override void Close() { }
        public override void Flush() { }

        public override bool CanRead { get { return this.access == ByteStreamAccess.Read; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanWrite { get { return this.access == ByteStreamAccess.Write; } }
        public override bool CanTimeout { get { return false; } }

    }

    public enum ByteStreamAccess
    {
        Read,
        Write
    }
}

