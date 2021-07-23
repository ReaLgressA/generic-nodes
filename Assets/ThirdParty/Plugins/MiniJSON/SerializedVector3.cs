using System.Collections;
using UnityEngine;

namespace MiniJSON {
    public class SerializedVector3 : IJsonInterface {
        public static readonly SerializedVector3 Zero = new SerializedVector3();

        public Vector3 Value { get; set; }

        public SerializedVector3() {
            Value = Vector3.zero;
        }

        public SerializedVector3(Vector3 value) {
            Value = value;
        }

        public static implicit operator Vector3(SerializedVector3 v) => v.Value;
        public static explicit operator SerializedVector3(Vector3 v) => new SerializedVector3(v);

        public void ToJsonObject(Hashtable ht) {
            ht["X"] = Value.x;
            ht["Y"] = Value.y;
            ht["Z"] = Value.z;
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Value = new Vector3(ht.GetFloat("X"), ht.GetFloat("Y"), ht.GetFloat("Z"));
        }
    }
}