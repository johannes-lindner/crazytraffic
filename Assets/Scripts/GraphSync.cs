using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GraphSync : MonoBehaviour
{
    [SerializeField]
    public List<SerializableGraphNode> nodes = new List<SerializableGraphNode>();


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllNodes()
    {
        var gg = AstarPath.active.data.gridGraph;

        gg.GetNodes(node => {
        // Here is a node
            nodes.Add(new SerializableGraphNode(node));
            
        });

        
    }

    public void ExportGraph()
    {
        var gg = AstarPath.active.data.gridGraph;

        foreach(GraphNode node in gg.nodes)
        {

        }
    }


}

[System.Serializable]
public class SerializableGraphNode
{
    [SerializeField]
    uint Area;
    [SerializeField]
    bool Destroyed;
    [SerializeField]
    uint Flags;
    [SerializeField]
    uint GraphIndex;
    [SerializeField]
    int NodeIndex;
    [SerializeField]
    uint Penalty;
    [SerializeField]
    uint Tag;
    [SerializeField]
    bool Walkable;
    [SerializeField]
    Vector3 position;

    public SerializableGraphNode(GraphNode node)
    {
        Area = node.Area;
        Destroyed = node.Destroyed;
        Flags = node.Flags;
        GraphIndex = node.GraphIndex;
        NodeIndex = node.NodeIndex;
        Penalty = node.Penalty;
        Tag = node.Tag;
        Walkable = node.Walkable;
        position = (Vector3) node.position;
    }
}
