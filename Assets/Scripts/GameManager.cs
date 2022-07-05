using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int targets;
    [SerializeField] private int targetsComplate;

    [SerializeField] private Text lvlName;
    [SerializeField] private Text MoveCount;
    
    public UnityEvent OnFinish;
    public static event Action OnFinished;

    public static event Action OnUndo;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Target.Oncomplate += targetComplate;
        Target.OnUndoComplate += targetUnDo;
        GridController.OnTargetSet += addTarget;
        GridController.OncurrentMoveChange += UpdateMovTxt;

    }
    private void OnDisable()
    {
        Target.Oncomplate -= targetComplate;
        Target.OnUndoComplate -= targetUnDo;
        GridController.OnTargetSet -= addTarget;
        GridController.OncurrentMoveChange -= UpdateMovTxt;
    }
    void Start()
    {
        lvlName.text = SceneManager.GetActiveScene().name;
    }

    void UpdateMovTxt(int move)
    {
        MoveCount.text = "Move: " + move;
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
        OnFinish?.Invoke();
        OnFinished?.Invoke();

    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void Undo()
    {
        OnUndo?.Invoke();
    }
    public static void MainMenuBtn()
    {
        SceneManager.LoadScene(0);
    }
}
