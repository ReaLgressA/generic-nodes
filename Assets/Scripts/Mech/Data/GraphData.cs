namespace Mech.Data {
    public class GraphData {
        public string Type { get; private set; }
        public NodeData Info { get; private set; }

        public GraphData(string type, NodeData info) {
            Type = type;
            Info = info;
        }
        
    }
}