using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject cube;
    public int[][] matrix;
    [SerializeField]
    public int widt = 20;
    [SerializeField]
    public int height = 20;
    // Start is called before the first frame update
    void Start()
    {
        float offsetX = 0;
        float offsetY = 0;
        int nr = 100;
        float step = 0.1f;

        for(int i = 0; i<nr; i++)
        {
            for (int j = 0; j<nr; j++)
            {
                float x = offsetX + i * step;
                float y = offsetY + j * step;
                float z = Mathf.Sin(x)+Mathf.Cos(y);
                InstantiateGridCell(new Vector3(x, z, y));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiateGridCell(Vector3 position)
    {
        GridCell gridCell = new GridCell();
        gridCell.obj = Instantiate(cube, position, Quaternion.identity);
        gridCell.obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        
    }

}

public class GridCell
{
    public int id;
    public GameObject obj;
    public Vector3 gridPosition;
    public Vector3 position;
    // Neighbours
}
