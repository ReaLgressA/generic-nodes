using System.Collections;

namespace GenericNodes.Mech.Data {
    public class NodeId : IJsonInterface {
        public static readonly NodeId None = new NodeId {Id = -1};

        public int Id { get; private set; } = -1;
        
        public NodeId() {}

        public NodeId(int id) {
            Id = id;
        }
        
        public void ToJsonObject(Hashtable ht) {
            ht["Id"] = Id;
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Id = ht.GetInt32("Id", Id);
        }
    }
}