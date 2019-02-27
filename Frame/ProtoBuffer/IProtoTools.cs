using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

public class IProtoTools {

    public static byte[] Serialize(IExtensible msg) {
        byte[] result;

        using (var stream = new MemoryStream())
        {
            Serializer.Serialize(stream, msg);
            result = stream.ToArray();
        }

        return result;
    }

    public static IExtensible Deserialize<IExtensible>(byte[] msg) {
        IExtensible result;

        using (var stream = new MemoryStream(msg))
        {
            result = Serializer.Deserialize<IExtensible>(stream);
        }

        return result;
    }
}
