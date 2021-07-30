using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Nodes;

namespace GenericNodes.Visual.Interfaces {
    public interface IGenericField {

        void SetData(NodeVisual nodeVisual, DataField data, IGenericFieldParent fieldParent);
        
        void Destroy();

        void RebuildLinks();
    }
}