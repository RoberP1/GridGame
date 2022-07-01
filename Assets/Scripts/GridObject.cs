using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField] public class GridObject
{
    public Vector2 direction;
    public GameObject box;
    public GridObject(Vector2 direction, GameObject box)
    {
        this.direction = direction;
        this.box = box;
    }
}
