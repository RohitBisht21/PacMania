using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    public void Restart()
    {
        Scoremanager.scoreCount = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }
    public void Exit(){
        Application.Quit();
    }
}
