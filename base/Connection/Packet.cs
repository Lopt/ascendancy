namespace Core.Connection
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.IO.Compression;

    /// <summary>
    /// Packet.
    /// Which contains (in this order):
    /// 1 Byte for the Method
    /// 2 Byte for the Content Size
    /// 0-65535 Bytes for the Content (depending on the Content Size)
    /// </summary>
    public class Packet
    {
        /// <summary>
        /// Gets or sets the type of the method.
        /// </summary>
        /// <value>The type of the method.</value>
        public MethodType MethodType
        {
            get
            {
                return (MethodType)BitConverter.ToInt16(ByteMethodType, 0);
            }
            set
            {
                ByteMethodType = BitConverter.GetBytes((short)value);
            }
        }

        /// <summary>
        /// The type of the method as byte array.
        /// </summary>
        private Byte[] ByteMethodType = new byte[2];

        /// <summary>
        /// Gets the size of the content.
        /// </summary>
        /// <value>The size of the content.</value>
        public int ContentSize
        {
            get
            {
                return ByteContent.Length;
            }
        }

        /// <summary>
        /// Gets the size of the content as byte array.
        /// </summary>
        /// <value>The size of the byte content.</value>
        private Byte[] ByteContentSize
        {
            get
            {
                return BitConverter.GetBytes(ByteContent.Length);
            }
        }

        /// <summary>
        /// The content as byte array.
        /// </summary>
        private Byte[] ByteContent;

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content
        {
            get
            {
                return Encoding.UTF8.GetString(ByteContent, 0, ByteContent.Length);
            }

            set
            {
                ByteContent = Encoding.UTF8.GetBytes(value);
            }
        }

        /// <summary>
        /// Send this packet at the specified stream.
        /// </summary>
        /// <param name="stream">Stream (TCP Socket Stream e.g.).</param>
        public void Send(Stream stream)
        {
            stream.Write(ByteMethodType, 0, ByteMethodType.Length);
            stream.Write(ByteContentSize, 0, ByteContentSize.Length);
            stream.Write(ByteContent, 0, ByteContent.Length);
        }

        /// <summary>
        /// Receives an packet from the specified stream.
        /// </summary>
        /// <param name="stream">Stream (TCP Socket Stream e.g.).</param>
        /// <returns>Packet which was received</returns>
        public static Packet Receive(Stream stream)
        {
            var packetOut = new Packet();
            stream.Read(packetOut.ByteMethodType, 0, 2);
            byte[] size = new byte[4];
            stream.Read(size, 0, size.Length);
            packetOut.ByteContent = new byte[BitConverter.ToInt32(size, 0)];
            stream.Read(packetOut.ByteContent, 0, BitConverter.ToInt32(size, 0));
            return packetOut;
        }

        /// <summary>
        /// converts an string to an byte array
        /// </summary>
        /// <returns>byte array.</returns>
        /// <param name="str">The String.</param>
        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// converts an byte array to an string
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="bytes">Byte Array.</param>
        private static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Connection.Packet"/> class.
        /// </summary>
        public Packet()
        {
        }
    }
}