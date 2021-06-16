using System.Collections;

namespace GenericNodes.Mech.Data {
    public class NodeId : IJsonInterface {
        public static readonly NodeId None = new NodeId {Id = -1};

        private int Id { get; set; } = -1;
        
        public void ToJsonObject(Hashtable ht) {
            ht["Id"] = Id;
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Id = ht.GetInt32("Id", Id);
        }
    }
}