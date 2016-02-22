namespace Tests
{
    using NUnit.Framework;
    using Core.Models;
    /// <summary>
    /// Compression helper tests.
    /// </summary>
    [TestFixture]
    public class CompressionHelperTests
    {
        /// <summary>
        /// Tests the Compression and Decompression.
        /// </summary>
        [Test]
        public void CompressionDecompression()
        {
            // Test for a single Word
            string input = "Teststring";
            var inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            var bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            var output = Core.Helper.CompressionHelper.Decompress(bytes);
            Assert.AreEqual(inputbytes, output);
            Assert.AreNotEqual(bytes, inputbytes);

            // Test for an long string
            input = "this is a long text who serves as a test for the compressionHelper class";
            inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            output = Core.Helper.CompressionHelper.Decompress(bytes);
            Assert.AreEqual(inputbytes, output);
            Assert.AreNotEqual(bytes, inputbytes);

            // Test vor an Very long String
            input = "this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something";
            inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            output = Core.Helper.CompressionHelper.Decompress(bytes);
            Assert.AreEqual(inputbytes, output);
            Assert.AreNotEqual(bytes, inputbytes);

            // Test for a long string with additional format comands
            input = "this is a long text who serves as a test for the compressionHelper class\t\tthis is a long text who serves as a test for the compressionHelper class";
            inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            output = Core.Helper.CompressionHelper.Decompress(bytes);
            Assert.AreEqual(inputbytes, output);
            Assert.AreNotEqual(bytes, inputbytes);

            // Test for an Empty string
            input = string.Empty;
            inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            output = Core.Helper.CompressionHelper.Decompress(bytes);
            Assert.AreEqual(inputbytes, output);
            Assert.AreNotEqual(bytes, inputbytes);

            // Test for own Byte data
            byte[] bytedata =
                {
                    1,
                    2,
                    3,
                    15,
                    16,
                    189,
                    200,
                    255,
                    69,
                    89,
                    79,
                    69,
                    49,
                    59,
                    69,
                    39,
                    1,
                    159,
                    35,
                    57,
                    179,
                    123,
                    198,
                    169,
                };
            bytes = Core.Helper.CompressionHelper.Compress(bytedata);
            output = Core.Helper.CompressionHelper.Decompress(bytes);
            Assert.AreEqual(bytedata, output);
            Assert.AreNotEqual(bytes, inputbytes);
        }
    }
}