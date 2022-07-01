using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridLevel", menuName = "ScriptableObjects/GridScriptableObject")]
public class GridScriptableObject : ScriptableObject
{
    public int x, y;
    public float cellsize;
    public Vector3 origin;

    public Vector2Int Player;
    public Vector2Int[] Boxes,targets,Block;


}
