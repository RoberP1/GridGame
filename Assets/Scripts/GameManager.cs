using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int targets;
    [SerializeField] private int targetsComplate;

    [Header("UI")]
    [SerializeField] private GameObject oWin;

    public static event Action OnUndo;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Target.Oncomplate += targetComplate;
        Target.OnUndoComplate += targetUnDo;
        GridController.OnTargetSet += addTarget;

    }
    private void OnDisable()
    {
        Target.Oncomplate -= targetComplate;
        Target.OnUndoComplate -= targetUnDo;
        GridController.OnTargetSet -= addTarget;
    }
    void Start()
    {
        oWin.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void addTarget()
    {
        targets++;
    }

    void targetComplate()
    {
        targetsComplate++;
        if (targetsComplate == targets)
        {
            finish();
        }
    }
    void targetUnDo()
    {
        targetsComplate--;
    }

    private void finish()
    {
        Debug.Log("Finish");
        oWin.SetActive(true);
    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Undo()
    {
        OnUndo?.Invoke();
    }
}
