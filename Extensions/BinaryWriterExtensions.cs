using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameCore
{
    public static class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, IBinarySerializable val) => val.Write(writer);
    }
}
