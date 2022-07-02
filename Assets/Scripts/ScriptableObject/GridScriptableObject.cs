using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridLevel", menuName = "ScriptableObjects/GridScriptableObject")]
public class GridScriptableObject : ScriptableObject
{
    public Vector2Int Player;
    public List<Vector2Int> Boxes = new List<Vector2Int>();
    public List<Vector2Int> targets = new List<Vector2Int>();
    public List<Vector2Int> Block = new List<Vector2Int>();
}
