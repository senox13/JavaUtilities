using System.Linq;
using NUnit.Framework;
using JavaUtilities;


namespace JavaUtilitiesTests{
    [TestFixture]
    public class JavaLikeStreamTests{
        /*
         * Fields
         */
        public static readonly short TEST_SHORT = 24000;
        public static readonly byte[] SHORT_BYTES = {0x5D, 0xC0};
        public static readonly int TEST_INT = 210000000;
        public static readonly byte[] INT_BYTES = {0x0C, 0x84, 0x58, 0x80};
        public static readonly long TEST_LONG = 9223000000000000000L;
        public static readonly byte[] LONG_BYTES = {0x7F, 0xFE, 0xAD, 0xA2,
                                                    0x6B, 0x6D, 0x80, 0x00};
        public static readonly float TEST_FLOAT = 9.81f;
        public static readonly byte[] FLOAT_BYTES = {0x41, 0x1C, 0xF5, 0xC3};
        public static readonly double TEST_DOUBLE = 9.81d;
        public static readonly byte[] DOUBLE_BYTES = {0x40, 0x23, 0x9E, 0xB8,
                                                      0x51, 0xEb, 0x85, 0x1F};
        public static readonly string TEST_STRING = "Hello, \0!";
        public static readonly byte[] STRING_BYTES = {0x00, 0x0A, 0x48, 0x65,
                                                      0x6C, 0x6C, 0x6F, 0x2C,
                                                      0x20, 0xC0, 0x80, 0x21};
        
        /*
         * Test methods
         */
        [Test]
        public void WriteShort(){
            using JavaLikeStream stream = new JavaLikeStream();
            stream.WriteShort(TEST_SHORT);
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            Assert.IsTrue(bytes.SequenceEqual(SHORT_BYTES));
        }
        
        [Test]
        public void ReadShort(){
            using JavaLikeStream stream = new JavaLikeStream(SHORT_BYTES);
            short value = stream.ReadShort();
            Assert.AreEqual(TEST_SHORT, value);
        }
        
        [Test]
        public void WriteInt(){
            using JavaLikeStream stream = new JavaLikeStream();
            stream.WriteInt(TEST_INT);
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            Assert.IsTrue(bytes.SequenceEqual(INT_BYTES));
        }
        
        [Test]
        public void ReadInt(){
            using JavaLikeStream stream = new JavaLikeStream(INT_BYTES);
            int value = stream.ReadInt();
            Assert.AreEqual(TEST_INT, value);
        }
        
        [Test]
        public void WriteLong(){
            using JavaLikeStream stream = new JavaLikeStream();
            stream.WriteLong(TEST_LONG);
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            Assert.IsTrue(bytes.SequenceEqual(LONG_BYTES));
        }
        
        [Test]
        public void ReadLong(){
            using JavaLikeStream stream = new JavaLikeStream(LONG_BYTES);
            long value = stream.ReadLong();
            Assert.AreEqual(TEST_LONG, value);
        }
        
        [Test]
        public void WriteFloat(){
            using JavaLikeStream stream = new JavaLikeStream();
            stream.WriteFloat(TEST_FLOAT);
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            Assert.IsTrue(bytes.SequenceEqual(FLOAT_BYTES));
        }
        
        [Test]
        public void ReadFloat(){
            using JavaLikeStream stream = new JavaLikeStream(FLOAT_BYTES);
            float value = stream.ReadFloat();
            Assert.AreEqual(TEST_FLOAT, value);
        }
        
        [Test]
        public void WriteDouble(){
            using JavaLikeStream stream = new JavaLikeStream();
            stream.WriteDouble(TEST_DOUBLE);
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            Assert.IsTrue(bytes.SequenceEqual(DOUBLE_BYTES));
        }
        
        [Test]
        public void ReadDouble(){
            using JavaLikeStream stream = new JavaLikeStream(DOUBLE_BYTES);
            double value = stream.ReadDouble();
            Assert.AreEqual(TEST_DOUBLE, value);
        }
        
        [Test]
        public void WriteUTF(){
            using JavaLikeStream stream = new JavaLikeStream();
            stream.WriteUTF(TEST_STRING);
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            Assert.IsTrue(bytes.SequenceEqual(STRING_BYTES));
        }
        
        [Test]
        public void ReadUTF(){
            using JavaLikeStream stream = new JavaLikeStream(STRING_BYTES);
            string value = stream.ReadUTF();
            Assert.AreEqual(TEST_STRING, value);
        }
    }
}
