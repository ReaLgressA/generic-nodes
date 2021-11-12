using System.Collections.Generic;

namespace GenericNodes.Mech.Fields {
    public enum DataType {
        Undefined = 0,
        String,
        Int,
        Float,
        Bool,
        Text,
        Enum,
        NodeId,
        GenericArray,
        MultiTypeArray,
        CustomObject,
        SpriteAsset,
        LocalizedText
    }
    
    public class DataTypeComparer : IEqualityComparer<DataType> {
        private static DataTypeComparer instance;
        public static DataTypeComparer Instance => instance ??= new DataTypeComparer();
        public bool Equals(DataType x, DataType y) { return x == y; }
        public int GetHashCode(DataType x) { return (int)x; }
    }
}