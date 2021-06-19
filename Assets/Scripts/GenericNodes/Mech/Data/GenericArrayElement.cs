using System;
using System.Collections.Generic;
using GenericNodes.Mech.Extensions;
using GenericNodes.Mech.Fields;

namespace GenericNodes.Mech.Data {
    [Serializable]
    public class GenericArrayElement {
        public List<DataField> Fields { get; private set; } = new List<DataField>();
        public GenericArrayDataField ArrayDataField { get; }
        
        public GenericArrayElement(GenericArrayDataField arrayDataField) {
            ArrayDataField = arrayDataField;
            NodeDescription dataType = ArrayDataField.CustomDataType;
            DataField[] fields = dataType.Fields.CloneFields();
            for (int i = 0; i < fields.Length; ++i) {
                Fields.Add(fields[i]);
            }
        }

        public GenericArrayElement Clone(GenericArrayDataField arrayDataField) {
            GenericArrayElement arrayElement = new GenericArrayElement(arrayDataField) {
                Fields = new List<DataField>(Fields.CloneFields())
            };
            return arrayElement;
        }
    }
}