using System.Collections.Generic;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.Fields;

namespace GenericNodes.Mech.Extensions {
    public static class DataFieldExtensions {

        public static List<CustomObjectDataField> CloneElements(this List<CustomObjectDataField> elementsList) {
            List<CustomObjectDataField> fields = new List<CustomObjectDataField>(elementsList.Count);
            for (int i = 0; i < elementsList.Count; ++i) {
                fields.Add(elementsList[i].Clone() as CustomObjectDataField);
            }
            return fields;
        }
        
        public static DataField[] CloneFields(this List<DataField> fieldList) {
            DataField[] fields = new DataField[fieldList.Count];
            for (int i = 0; i < fields.Length; ++i) {
                fields[i] = fieldList[i].Clone();
            }
            return fields;
        }
        
        public static DataField[] CloneFields(this DataField[] fieldList) {
            DataField[] fields = new DataField[fieldList.Length];
            for (int i = 0; i < fields.Length; ++i) {
                fields[i] = fieldList[i].Clone();
            }
            return fields;
        }
    }
}