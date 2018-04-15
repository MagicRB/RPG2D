using UnityEngine;
using System.Runtime.Serialization;
using System.Collections;

namespace RPG2D.SerializationSurrogates
{
    public class Vector2IntSerializationSurrogate : ISerializationSurrogate
    {
        // Method called to serialize a Vector2Int object
        public void GetObjectData(System.Object obj,SerializationInfo info, StreamingContext context)
        {
 
            Vector2Int v2 = (Vector2Int)obj;
            info.AddValue("x", v2.x);
            info.AddValue("y", v2.y);
        }
 
        // Method called to deserialize a Vector2Int object
        public System.Object SetObjectData(System.Object obj,SerializationInfo info,
            StreamingContext context,ISurrogateSelector selector)
        {
 
            Vector2Int v2 = (Vector2Int)obj;
            v2.x = (int)info.GetValue("x", typeof(int));
            v2.y = (int)info.GetValue("y", typeof(int));
            obj = v2;
            return obj;
        }
    }
}