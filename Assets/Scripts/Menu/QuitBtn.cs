using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitBtn : MonoBehaviour
{
    public void Quitbtn()
    {
        Application.Quit();
    }
    public void MainmenuBtn()
    {
        SceneManager.LoadScene(0);
    }
}
