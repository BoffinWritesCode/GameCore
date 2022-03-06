using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameCore
{
    public static class BinaryReaderExtensions
    {
        public static IBinarySerializable ReadBinarySerializable<T>(this BinaryReader reader) where T : IBinarySerializable
        {
            T val = default;
            val.Read(reader);
            return val;
        }

        public static IBinarySerializable ReadBinarySerializable<T>(this BinaryReader reader, T obj) where T : IBinarySerializable
        {
            obj.Read(reader);
            return obj;
        }
    }
}
