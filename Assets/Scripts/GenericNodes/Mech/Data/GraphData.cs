namespace GenericNodes.Mech.Data {
    public class GraphData {
        public string Type { get; private set; }
        public NodeData Info { get; private set; }
        
        public GraphScheme Scheme { get; private set; }

        public GraphData(string type, NodeData info, GraphScheme scheme) {
            Type = type;
            Info = info;
            Scheme = scheme;
        }
    }
}