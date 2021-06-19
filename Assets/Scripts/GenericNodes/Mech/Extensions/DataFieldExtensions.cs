using System.Collections.Generic;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.Fields;

namespace GenericNodes.Mech.Extensions {
    public static class DataFieldExtensions {

        public static List<GenericArrayElement> CloneElements(this List<GenericArrayElement> elementsList,
                                                              GenericArrayDataField arrayDataField) {
            List<GenericArrayElement> fields = new List<GenericArrayElement>(elementsList.Count);
            for (int i = 0; i < elementsList.Count; ++i) {
                fields.Add(elementsList[i].Clone(arrayDataField));
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
    }
}