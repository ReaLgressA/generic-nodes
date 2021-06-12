using System;
using System.Collections;
using System.Collections.Generic;
using Mech.Data;

namespace Mech.Fields {
    public static class DataFieldFactory {

        private static readonly Dictionary<DataType, Func<Hashtable, DataField>> creatorMethods =
            new Dictionary<DataType, Func<Hashtable, DataField>>(DataTypeComparer.Instance) {
                {DataType.Bool, ht => new BoolDataField().InitializeFromHashtable(ht) },
                {DataType.Int, ht => new IntDataField().InitializeFromHashtable(ht) },
                {DataType.Float, ht => new FloatDataField().InitializeFromHashtable(ht) },
                {DataType.String, ht => new StringDataField().InitializeFromHashtable(ht) },
                {DataType.Text, ht => new TextDataField().InitializeFromHashtable(ht) },
                {DataType.Enum, ht => new EnumDataField(CurrentGraphScheme).InitializeFromHashtable(ht)}
            };

        public static GraphScheme CurrentGraphScheme { get; set; } = null;

        public static DataField CreateFromHashtable(Hashtable ht) {

            DataType type = ht.GetEnum("Type", DataType.Undefined);
            return (type != DataType.Undefined ? creatorMethods[type]?.Invoke(ht) : null);
        }
    }
}