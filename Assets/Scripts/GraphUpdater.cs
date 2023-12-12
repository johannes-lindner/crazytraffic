using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.PlayerLoop;
using UnityEditor;


public class GraphUpdater : MonoBehaviour
{
    public GridGraph graph;

    public Collider UpdateGraphElement;
    public Transform SearchPosition;

    Vector3 node_modified_position = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        var node1 = AstarPath.active.GetNearest(SearchPosition.position, NNConstraint.None).node;
        node_modified_position = (Vector3) node1.position;
        //var node2 = AstarPath.active.GetNearest(SearchPosition + Vector3.right, NNConstraint.None).node;

        node1.Penalty = (uint) 1000;


        //graph.
        //AstarPath.active.Scan(graphToScan);

        // Recalculate only the first and third graphs
        //var graphsToScan = new[] { AstarPath.active.data.graphs[0], AstarPath.active.data.graphs[2] };
        //AstarPath.active.Scan(graphsToScan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(node_modified_position, 0.2f);
    }




    public void UpdateNodePenalty(GraphNode node, int penalty)
    {
        node.Penalty = (uint) penalty;
    }
}
