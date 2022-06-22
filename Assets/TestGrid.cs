using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    public Grid<int> grid;
    public int x, y;
    public float cellsize;
    public Vector3 origin;
    public int a, b;
        
    void Start()
    {
        grid = new Grid<int>(x, y, cellsize, origin);
        Debug.Log(grid.GetValue(a, b));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
