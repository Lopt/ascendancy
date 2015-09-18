namespace Core.Helper
{
    using System;
    using System.IO;
    using System.IO.Compression;

    /// <summary>
    /// Compresses or decompresses byte arrays using GZipStream
    /// </summary>
    public static class CompressionHelper
    {
        /// <summary>
        /// Compress the specified inputData.
        /// </summary>
        /// <param name="inputData">Input data.</param>
        public static byte[] Compress(byte[] inputData)
        {
            if (inputData == null)
                throw new ArgumentNullException("inputData must be non-null");

            using (var compressIntoMs = new MemoryStream())
            {
                using (var gzs = new GZipStream(compressIntoMs, CompressionMode.Compress))
                {
                    gzs.Write(inputData, 0, inputData.Length);
                }
                return compressIntoMs.ToArray(); 
            }
        }

        /// <summary>
        /// Decompress the specified inputData.
        /// </summary>
        /// <param name="inputData">Input data.</param>
        public static byte[] Decompress(byte[] inputData)
        {
            if (inputData == null)
                throw new ArgumentNullException("inputData must be non-null");

            using (var compressedMs = new MemoryStream(inputData))
            {
                using (var decompressedMs = new MemoryStream())
                {
                    using (var gzs = new GZipStream(compressedMs, 
                        CompressionMode.Decompress))
                    {
                        gzs.CopyTo(decompressedMs);
                    }
                    return decompressedMs.ToArray(); 
                }
            }
        }
    }
}