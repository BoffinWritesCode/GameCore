using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GameCore
{
    public interface IBinarySerializable
    {
        void Write(BinaryWriter writer);
        void Read(BinaryReader reader);
        int GetByteLength();
    }
}
