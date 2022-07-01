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
    // Start is called before the first frame update
    private void OnEnable()
    {
        Target.Oncomplate += targetComplate;
        GridController.OnTargetSet += addTarget;

    }
    private void OnDisable()
    {
        Target.Oncomplate -= targetComplate;
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

    private void finish()
    {
        Debug.Log("Finish");
        oWin.SetActive(true);
    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
