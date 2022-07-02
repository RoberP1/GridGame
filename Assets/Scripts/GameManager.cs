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
    
    public UnityEvent OnFinish;

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
        lvlName.text = SceneManager.GetActiveScene().name;
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
        OnFinish?.Invoke();

    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
