using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor.Experimental.GraphView;

public class PenaltyUpdater : MonoBehaviour
{
    public Transform center;
    public float radius = 3.0f;
    public uint numberOfPoints = 20;
    [SerializeField]
    private List<Vector3> searchPositions = new List<Vector3>();
    [SerializeField]
    private List<Vector3> searchNodes = new List<Vector3>();

    private void Start()
    {
        UpdatePenalties();
    }


    public void UpdateSearchPoints()
    {
        searchPositions.Clear();
        searchNodes.Clear();
        RandomPointsInsideCircle(numberOfPoints, center.position, ref searchPositions);

    }

    public void UpdatePenalties()
    {
        foreach(Vector3 pos in searchPositions)
        {
            var node1 = AstarPath.active.GetNearest(pos, NNConstraint.None).node;
            Vector3 node_pos = (Vector3)node1.position;
            searchNodes.Add(node_pos);
            
            node1.Penalty = (uint)Random.Range(200, 1000);
            //node1.Penalty = (uint)50;
        }

        // Recalculate all grid connections
        // This is required because we have updated the walkability of some nodes
        var gg = AstarPath.active.data.gridGraph;
        gg.GetNodes(node => gg.CalculateConnections((GridNodeBase)node));

        // If you are only updating one or a few nodes you may want to use
        // gg.CalculateConnectionsForCellAndNeighbours only on those nodes instead for performance.
    }


    #region UI
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center.position, radius);

        if (searchPositions.Count > 0)
        {
            foreach (Vector3 pos in searchPositions)
            {
                Gizmos.DrawLine(pos, pos + new Vector3(0, 0.2f, 0));
            }
        }

        if (searchNodes.Count > 0)
        { 
            foreach (Vector3 pos in searchNodes)
            {
                Gizmos.DrawSphere(pos, 0.02f);
            }
        }
    }
    #endregion


    #region Private Functions
    private void RandomPointsInsideCircle(uint nrPoint, Vector3 center, ref List<Vector3> searchPositions)
    {
        for (int i = 0; i < nrPoint; i++)
        {
            Vector2 randPos2D = Random.insideUnitCircle * radius;
            Vector3 randPos3D = new Vector3(randPos2D.x, 0, randPos2D.y);
            Vector3 randPos = randPos3D + center;
            searchPositions.Add(randPos);
        }
    }
    #endregion
}
