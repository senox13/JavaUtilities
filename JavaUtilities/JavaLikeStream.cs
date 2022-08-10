using System;
using System.IO;
using System.Text;
using System.Collections.Generic;


namespace JavaUtilities{
    /// <summary>
    /// Provides methods for writing to and reading from streams, using
    /// formats that follow Java's DataOutputStream and DataInputStream
    /// specifications.
    /// </summary>
    public class JavaLikeStream : Stream{
        /*
         * Fields
         */
        private readonly Stream wrapped;
        private readonly bool leaveOpen;


        /*
         * Properties
         */
        /// <inheritdoc/>
        public override bool CanRead => wrapped.CanRead;
        /// <inheritdoc/>
        public override bool CanSeek => wrapped.CanSeek;
        /// <inheritdoc/>
        public override bool CanWrite => wrapped.CanWrite;
        /// <inheritdoc/>
        public override long Length => wrapped.Length;
        /// <inheritdoc/>
        public override long Position{
            get => wrapped.Position;
            set => wrapped.Position = value;
        }


        /*
         * Constructors
         */
        /// <summary>
        /// Constructs a new, empty <see cref="JavaLikeStream"/>.
        /// </summary>
        public JavaLikeStream()
            :this(new MemoryStream()){}

        /// <summary>
        /// Constructs a new <see cref="JavaLikeStream"/> with the given
        /// initial contents.
        /// </summary>
        /// <param name="data"></param>
        public JavaLikeStream(byte[] data)
            :this(new MemoryStream(data)){}

        /// <summary>
        /// Constructs a new <see cref="JavaLikeStream"/>, wrapping the given
        /// <see cref="Stream"/> to allow reading/writing to it using Java's
        /// encoding. JavaLikeStreams created with this constructor will close
        /// their wrapped stream when closed.
        /// </summary>
        /// <param name="toWrap">The stream to wrap</param>
        public JavaLikeStream(Stream toWrap)
            :this(toWrap, false){}

        /// <summary>
        /// Constructs a new <see cref="JavaLikeStream"/>, wrapping the given
        /// <see cref="Stream"/> to allow reading/writing to it using Java's
        /// encoding, optionally leaving the wrapped stream open when the
        /// enclosing stream is closed.
        /// </summary>
        /// <param name="toWrap">The stream to wrap</param>
        /// <param name="leaveOpenIn">Whether the wrapped stream should be
        /// left open when the enclosing stream is closed</param>
        public JavaLikeStream(Stream toWrap, bool leaveOpenIn){
            wrapped = toWrap;
            leaveOpen = leaveOpenIn;
        }


        /*
         * Stream override methods
         */
        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin){
            return wrapped.Seek(offset, origin);
        }

        /// <inheritdoc/>
        public override void SetLength(long value){
            wrapped.SetLength(value);
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count){
            return wrapped.Read(buffer, offset, count);
        }

        /// <inheritdoc/>
        public override int ReadByte(){
            return wrapped.ReadByte();
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count){
            wrapped.Write(buffer, offset, count);
        }

        /// <inheritdoc/>
        public override void WriteByte(byte value){
            wrapped.WriteByte(value);
        }
            
        /// <inheritdoc/>
        public override void Flush(){
            wrapped.Flush();
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing){
            if(!leaveOpen){
                wrapped.Dispose();
            }
        }


        /*
         * Instance methods
         */
        /// <summary>
        /// Reads a short from the given stream, most significant byte first
        /// </summary>
        /// <returns>The parsed short</returns>
        public short ReadShort(){
            byte[] bytes = new byte[sizeof(short)];
            Read(bytes, 0, bytes.Length);
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }
        
        /// <summary>
        /// Writes the given short to the given stream, most significant byte first
        /// </summary>
        /// <param name="value">The value to write to the stream</param>
        public void WriteShort(short value){
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            Write(bytes, 0, bytes.Length);
        }
        
        /// <summary>
        /// Reads an int from the given stream, most significant bytes first
        /// </summary>
        /// <returns>The parsed int</returns>
        public int ReadInt(){
            byte[] bytes = new byte[sizeof(int)];
            Read(bytes, 0, sizeof(int));
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
        
        /// <summary>
        /// Writes the specified int to the given stream, most signigicant bytes first
        /// </summary>
        /// <param name="value">The int to write to the stream</param>
        public void WriteInt(int value){
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            Write(bytes, 0, bytes.Length);
        }
        
        /// <summary>
        /// Reads a long from the given stream, most significant bytes first
        /// </summary>
        /// <returns>The parsed long</returns>
        public long ReadLong(){
            byte[] bytes = new byte[sizeof(long)];
            Read(bytes, 0, bytes.Length);
            Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }
        
        /// <summary>
        /// Writes the given long to the specified stream, most siginficant bytes first
        /// </summary>
        /// <param name="value">The value to write to the given stream</param>
        public void WriteLong(long value){
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            Write(bytes, 0, bytes.Length);
        }
        
        /// <summary>
        /// Reads a float from the given stream, most significant bytes first
        /// </summary>
        /// <returns>The parsed float</returns>
        public float ReadFloat(){
            byte[] bytes = new byte[sizeof(float)];
            Read(bytes, 0, bytes.Length);
            Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0);
        }
        
        /// <summary>
        /// Writes the given float to the specified stream
        /// </summary>
        /// <param name="value">The value to write to the given stream</param>
        public void WriteFloat(float value){
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            Write(bytes, 0, bytes.Length);
        }
        
        /// <summary>
        /// Reads a double from the given stream, most significant bytes first
        /// </summary>
        /// <returns>The parsed double</returns>
        public double ReadDouble(){
            byte[] bytes = new byte[sizeof(double)];
            Read(bytes, 0, bytes.Length);
            Array.Reverse(bytes);
            return BitConverter.ToDouble(bytes, 0);
        }
        
        /// <summary>
        /// Writes the given double to the specified stream
        /// </summary>
        /// <param name="value">The parsed value</param>
        public void WriteDouble(double value){
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            Write(bytes, 0, bytes.Length);
        }
        
        /// <summary>
        /// Reads a string from the given stream using a modified UTF-8 encoding, first by reading its
        /// length as a short, as though by calling ReadShort, then by reading the number of characters
        /// given by that length. Any nul characters are expected to have been replaced with the
        /// sequence 0xc0, 0x80.
        /// </summary>
        /// <returns>The parsed string</returns>
        public string ReadUTF(){
            short byte_length = ReadShort();
            byte[] bytes = new byte[byte_length];
            Read(bytes, 0, byte_length);
            List<byte> mod_bytes = new List<byte>(bytes);
            for(int i=0; i<mod_bytes.Count-1; i++){
                if(mod_bytes[i] == 0xC0 && mod_bytes[i+1] == 0x80){
                    mod_bytes[i] = 0;
                    mod_bytes.RemoveAt(i+1);
                }
            }
            return Encoding.UTF8.GetString(mod_bytes.ToArray());
        }
        
        /// <summary>
        /// Writes the given string to the specified stream using a modified UTF-8 encoding. First the
        /// length in bytes of the encoded string is written as a short, as though by calling WriteShort,
        /// then the UTF-8 representation of the string is written. Any nul characters in the string will
        /// be replaced with the byte sequence 0xC0, 0x80.
        /// </summary>
        /// <param name="value">The value to encode and write</param>
        public void WriteUTF(string value){
            List<byte> encoded = new List<byte>(Encoding.UTF8.GetBytes(value));
            for(int i=0; i<encoded.Count; i++){
                if(encoded[i] == 0){
                    encoded[i] = 0xC0;
                    encoded.Insert(i+1, 0x80);
                }
            }
            if(encoded.Count > short.MaxValue)
                throw new ArgumentException("Given string has an encoded length longer than short.MaxValue", nameof(value));
            WriteShort((short)encoded.Count);
            Write(encoded.ToArray(), 0, encoded.Count);
        }
    }
}
