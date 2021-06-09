using System.Collections;
using UnityEngine;

public class SerializedVector2 : IJsonInterface {
    
    public static readonly SerializedVector2 Zero =  new SerializedVector2();
    
    public Vector2 Value { get; set; }

    public SerializedVector2() {
        Value = Vector2.zero;
    }

    public SerializedVector2(Vector2 value) {
        Value = value;
    }
    
    public static implicit operator Vector2(SerializedVector2 v) => v.Value;
    public static explicit operator SerializedVector2(Vector2 v) => new SerializedVector2(v);
    
    public void ToJsonObject(Hashtable ht) {
        ht["X"] = Value.x;
        ht["Y"] = Value.y;
    }

    public void FromJson(Hashtable ht, bool isAddition = false) {
        Value = new Vector2(ht.GetFloat("X"), ht.GetFloat("Y"));
    }
}