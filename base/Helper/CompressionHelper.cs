
namespace Core.Helper
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.ServiceModel.Channels;
    using Ionic.Zlib;

    /// <summary>
    /// Compresses or decompresses byte arrays using GZipStream
    /// </summary>
    public static class CompressionHelper
    {
        /// <summary>
        /// The maximum size of an packet
        /// </summary>
        const int MAX_PACKET_SIZE = 4096 * 4;

        /// <summary>
        /// Compress the specified inputData.
        /// </summary>
        /// <param name="inputData">Input data.</param>
        /// <returns>compressed byte array</returns>
        public static byte[] Compress(byte[] inputData)
        {
            var zlib = new ZlibCodec(Ionic.Zlib.CompressionMode.Compress);
            zlib.CompressLevel = Ionic.Zlib.CompressionLevel.BestCompression;
            zlib.InputBuffer = inputData;
            zlib.OutputBuffer = new byte[MAX_PACKET_SIZE];
            zlib.NextIn = 0;
            zlib.AvailableBytesIn = inputData.Length;
            zlib.NextOut = 0;
            zlib.AvailableBytesOut = MAX_PACKET_SIZE;
            zlib.Deflate(FlushType.Finish);
            var output = new byte[zlib.TotalBytesOut];
            Array.Copy(zlib.OutputBuffer, output, (int)zlib.TotalBytesOut);
            return output;
        }

        /// <summary>
        /// Decompress the specified inputData.
        /// </summary>
        /// <param name="inputData">Input data.</param>
        /// <returns>decompressed byte array</returns>
        public static byte[] Decompress(byte[] inputData)
        {
            var zlib = new ZlibCodec(Ionic.Zlib.CompressionMode.Decompress);
            zlib.InputBuffer = inputData;
            zlib.OutputBuffer = new byte[MAX_PACKET_SIZE];
            zlib.NextIn = 0;
            zlib.AvailableBytesIn = inputData.Length;
            zlib.NextOut = 0;
            zlib.AvailableBytesOut = MAX_PACKET_SIZE;
            zlib.Inflate(FlushType.Finish);
            var output = new byte[zlib.TotalBytesOut];
            Array.Copy(zlib.OutputBuffer, output, (int)zlib.TotalBytesOut);
            return output;
        }
    }
}