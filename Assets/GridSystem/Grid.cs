using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    Mesh mesh;
    public GameObject cube;
    public int[][] matrix;
    [SerializeField]
    public int widt = 20;
    [SerializeField]
    public int height = 20;

    [Range(0f, 5f)]
    public float multi = 1;

    public List<Vertex> vertexList = new List<Vertex>();
    // Start is called before the first frame update

    public Vector3[] vertices;
    public Material material;
    public GridCell[,] gridcells;

    int nr;
    int[] triangl;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CalculateMesh();
        AssignGridCells();
        CalculateTriangels();

        UpdateMesh();
    }

    void CalculateMesh()
    {
        float offsetX = 0;
        float offsetY = 0;
        nr = 51;
        float step = 0.4f;

        //vertices = new Vector3[nr, nr];
        vertices = new Vector3[nr * nr];
        vertexList.Clear();
        int vertex_id = 0;
        for (int i = 0; i < nr; i++)
        {
            for (int j = 0; j < nr; j++)
            {
                float x = offsetX + i * step;
                float z = offsetY + j * step;
                float y = multi * Mathf.Sin(x) + multi*Mathf.Cos(z);
                vertices[vertex_id] = (new Vector3(x, y, z));
                vertexList.Add(new Vertex(vertex_id, new Vector3(x, y, z), new Vector3(i, 0, j)));
                vertex_id++;
            }
        }
        Debug.Log(vertexList.Count);
    }

    void AssignGridCells()
    {
        gridcells = new GridCell[nr - 1, nr - 1];
        int cell_id = 0;
        for (int i = 0; i < gridcells.GetLength(0); i++)
        {
            for (int j = 0; j < gridcells.GetLength(1); j++)
            {
                // For each Cell
                GridCell gc = new GridCell(cell_id, new Vector3(i, 0, j));

                gc.bounds = new Vertex[4]
                {
                    vertexList.Find(v => v.gridPosition==new Vector3(i,0,j)),
                    vertexList.Find(v => v.gridPosition==new Vector3(i+1,0,j)),
                    vertexList.Find(v => v.gridPosition==new Vector3(i,0,j+1)),
                    vertexList.Find(v => v.gridPosition==new Vector3(i+1,0,j+1)),
                };



                gc.triangles = new Triangle[2]
                {
                    new Triangle(new Vertex[3]{gc.bounds[0],gc.bounds[1],gc.bounds[2] }),
                    new Triangle(new Vertex[3]{gc.bounds[1],gc.bounds[3],gc.bounds[2] }),
                };


                gridcells[i, j] = gc;
                cell_id++;
            }
        }
    }

    void CalculateTriangels()
    {
        List<int> tria = new List<int>();
        foreach (GridCell cell in gridcells)
        {
            int[] ints = cell.triangles[0].ReturnVertexIds();
            tria.Add(ints[0]);
            tria.Add(ints[1]);
            tria.Add(ints[2]);

            ints = cell.triangles[1].ReturnVertexIds();
            tria.Add(ints[0]);
            tria.Add(ints[1]);
            tria.Add(ints[2]);
        }
        triangl = tria.ToArray();
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangl;

        mesh.RecalculateNormals();
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }

    //private void OnDrawGizmosSelected()
    //{
    //    foreach(Vector3 v in vertices)
    //    {
    //        // Draw a yellow sphere at the transform's position
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawSphere(v, 0.3f);
    //    }
    //    //Gizmos.DrawLineStrip(vertices, false);
    //}

    public void RecalculateMesh()
    {
        CalculateMesh();
        UpdateMesh();
    }

    public void ShowAnimation()
    {
        StartCoroutine(Animation());
    }
    
    IEnumerator Animation()
    {
        int duration = 1000;
        int step = 1;
        int val = 1;
        while (step<duration)
        {
            
            if (multi >= 3 && val==1)
            {
                val = -1;
            }
            if (multi <= 0 && val==-1)
            {
                val = 1;
            }
            multi += val * 0.1f;
            RecalculateMesh();
            step++;
            yield return new WaitForSeconds(0.1f);
        }   
        
    }
}

[System.Serializable]
public class GridCell
{
    public int id;
    public Vector3 gridPosition;
    public Vector3 center;
    public Vertex[] bounds;
    public Triangle[] triangles;

    // Neighbours
    public GridCell(int _id, Vector3 _gridPosition)
    {
        id = _id; gridPosition = _gridPosition;
    }

    public Vector3[] GetBoundPoints()
    {
        List<Vector3> boundPoints = new List<Vector3>();
        foreach(Vertex v in bounds)
        {
            boundPoints.Add(v.position);
        }


        return boundPoints.ToArray();
    }

    public void PrintBounds()
    {
        Debug.Log("v0: " + bounds[0].position + " [" + bounds[0].gridPosition + "]," +
            "v1: " + bounds[1].position + " [" + bounds[1].gridPosition + "]," + 
            "v2: " + bounds[2].position + " [" + bounds[2].gridPosition + "]," + 
            "v3: " + bounds[3].position + " [" + bounds[3].gridPosition + "],");
    }
}


[System.Serializable]
public class Vertex
{
    public int id;
    public Vector3 position;
    public Vector3 gridPosition;

    public Vertex(int id, Vector3 position, Vector3 gridPosition)
    {
        this.id = id;
        this.position = position;
        this.gridPosition = gridPosition;
    }   

}

[System.Serializable]
public class Triangle
{
    public Vertex[] vertices = new Vertex[3];

    public Triangle(Vertex[] _vertices) { vertices = _vertices; }

    public int[] ReturnVertexIds()
    {
        return new int[3] { vertices[0].id, vertices[1].id, vertices[2].id };
    }
}

