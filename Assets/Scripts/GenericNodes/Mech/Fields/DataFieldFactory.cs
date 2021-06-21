using System;
using System.Collections;
using System.Collections.Generic;
using GenericNodes.Mech.Data;

namespace GenericNodes.Mech.Fields {
    public static class DataFieldFactory {

        private static readonly Dictionary<DataType, Func<Hashtable, DataField>> creatorMethods =
            new Dictionary<DataType, Func<Hashtable, DataField>>(DataTypeComparer.Instance) {
                {DataType.Bool, ht => new BoolDataField().Construct(ht) },
                {DataType.Int, ht => new IntDataField().Construct(ht) },
                {DataType.Float, ht => new FloatDataField().Construct(ht) },
                {DataType.String, ht => new StringDataField().Construct(ht) },
                {DataType.Text, ht => new TextDataField().Construct(ht) },
                {DataType.Enum, ht => new EnumDataField(CurrentGraphScheme).Construct(ht) },
                {DataType.NodeId, ht => new NodeIdDataField().Construct(ht) },
                {DataType.CustomObject, ht => new CustomObjectDataField(CurrentGraphScheme).Construct(ht) }
            };

        public static GraphScheme CurrentGraphScheme { get; set; } = null;

        public static DataField CreateFromHashtable(Hashtable ht) {
            DataType type = ht.GetEnum("Type", DataType.Undefined);
            return (type != DataType.Undefined ? creatorMethods[type]?.Invoke(ht) : null);
        }
    }
}