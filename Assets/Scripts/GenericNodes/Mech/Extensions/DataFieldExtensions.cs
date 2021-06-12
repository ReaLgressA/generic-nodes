using System.Collections.Generic;
using Mech.Fields;

namespace GenericNodes.Mech.Extensions {
    public static class DataFieldExtensions {
        public static DataField[] CloneFields(this List<DataField> fieldList) {
            DataField[] fields = new DataField[fieldList.Count];
            for (int i = 0; i < fields.Length; ++i) {
                fields[i] = fieldList[i].Clone();
            }
            return fields;
        }
    }
}