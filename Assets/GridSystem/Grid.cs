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

    public List<Vertex> vertexList = new List<Vertex>();
    // Start is called before the first frame update

    public Vector3[] vertices;
    public Material material;
    public GridCell[,] gridcells;

    int[] triangl;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        float offsetX = 0;
        float offsetY = 0;
        int nr = 11;
        float step = 1f;

        //vertices = new Vector3[nr, nr];
        vertices = new Vector3[nr * nr];
        int vertex_id = 0;
        for (int i = 0; i < nr; i++)
        {
            for (int j = 0; j < nr; j++)
            {
                float x = offsetX + i * step;
                float y = offsetY + j * step;
                float z = Mathf.Sin(x) + Mathf.Cos(y);
                //InstantiateGridCell(new Vector3(x, z, y));
                //points.Add(new Vector3(x, y, z));
                vertices[vertex_id]= (new Vector3(x, y, z));
                vertexList.Add(new Vertex(vertex_id, new Vector3(x, y, z), new Vector3(i, 0, j)));
                vertex_id++;
            }
        }
        Debug.Log(vertexList.Count);

        gridcells = new GridCell[nr-1,nr-1];
        int cell_id = 0;
        for (int i = 0; i < gridcells.GetLength(0); i++)
        {
            for (int j = 0; j < gridcells.GetLength(1); j++)
            {
                // For each Cell
                GridCell gc = new GridCell(cell_id, new Vector3(i, 0, j));

                gc.bounds = new Vertex[4]
                {
                    vertexList[i + j * (nr - 1)],
                    vertexList[ (i+1) + j * (nr - 1)],
                    vertexList[i + (j+1) * (nr - 1)],
                    vertexList[ (i+1) + (j+1) * (nr - 1)]
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



        List<int> tria = new List<int>();
        //int[] tria = new int[] {};

        foreach (GridCell cell in gridcells)
        {
            //cell.triangles[0].ReturnVertexIds();
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

        UpdateMesh();
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangl;

        mesh.RecalculateNormals();
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