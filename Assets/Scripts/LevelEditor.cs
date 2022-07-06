using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] GameObject NEPlayers, NEBoxes,NETargets;
    public InputAction Click;
    public InputAction NumbersInput;
    private GridController gridController;
    [SerializeField]int numberInput;
    [SerializeField] GameObject playerActive;
    [SerializeField] Vector2Int playerActiveCords;
    
    private void OnEnable()
    {
        Click.Enable();
        Click.performed += _ =>getWorldPos();
        NumbersInput.Enable();
        NumbersInput.performed += _ => NumberPress();
    }
    private void OnDisable()
    {
        Click.Disable();
        Click.performed -= _ => getWorldPos();
        NumbersInput.Disable();
        NumbersInput.performed -= _ => NumberPress();
    }
    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        StartCoroutine(FindPlayer());
    }


    // Update is called once per frame
    void Update()
    {




    }

    private void getWorldPos()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        worldPos.z = 0;
        gridController.grid.GetXY(worldPos, out int x, out int y);
        if (x >= 1 && y >= 1 && x < gridController.gridSize.x - 1 && y < gridController.gridSize.y - 1)
        {
            Debug.Log(new Vector2Int(x, y));
            GameObject value = gridController.gameObjectsGrid.GetValue(worldPos);
            switch (numberInput)
            {

                case 0:
                    if (value == null) return;
                    if (value == playerActive) playerActive = null;

                    Destroy(value);
                    gridController.gameObjectsGrid.SetValue(worldPos, null);
                    gridController.grid.SetValue(worldPos, 0);
                    break;
                case 1:

                    if (playerActive != null) return;
                    if (value == null) gridController.AddTile(worldPos, numberInput);
                    else
                    {
                        Destroy(value);
                        gridController.AddTile(worldPos, numberInput);
                    }

                    playerActive = GameObject.FindGameObjectWithTag("Player");
                    playerActiveCords = new Vector2Int(x, y);

                    break;
                default:
                    if (value == null) gridController.AddTile(worldPos, numberInput);
                    else
                    {
                        if (value == playerActive) playerActive = null;
                        Destroy(value);
                        gridController.AddTile(worldPos, numberInput);
                    }
                    break;
            }
        }
        
        
    }
    void NumberPress()
    {
        int.TryParse(NumbersInput.ReadValue<float>().ToString(), out numberInput) ;
        Debug.Log(numberInput);
    }
    public void NumberPress(int number)
    {
        numberInput = number;
        Debug.Log(numberInput);
    }
    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(0.01f);
        playerActive = GameObject.FindGameObjectWithTag("Player");
        int x = gridController.gridScriptableObject.Player.x;
        int y = gridController.gridScriptableObject.Player.x;

        gridController.gameObjectsGrid.SetValue(x, y, playerActive);
        playerActiveCords = new Vector2Int(x, y);
    }

    public void SaveGrid()
    {
        if (playerActive == null)
        {
            Destroy(Instantiate(NEPlayers), 3);
        }
        else
        {
            List<Vector2Int> boxes, targets, borders= new List<Vector2Int>();
            gridController.GetAll(out boxes, out targets, out borders);
            Debug.Log("boxes " + boxes.Count);
            Debug.Log("targets " + targets.Count);
            Debug.Log("blocks " + borders.Count);
            if (targets.Count == 0)
            {
                Destroy(Instantiate(NETargets), 3);
            }
            else if (boxes.Count < targets.Count)
            {
                Destroy(Instantiate(NEBoxes), 3);
            }
            else
            {
                GridScriptableObject grid = gridController.gridScriptableObject;

                grid.Boxes = boxes;
                grid.targets = targets;
                grid.Block = borders;
                grid.Player = playerActiveCords;
                
            }

        }
    }
}
