using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MG_SceneManager : MonoBehaviour {
    void Awake(){
        //DontDestroyOnLoad(this.gameObject);
    }
    public void ButtonSalirToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void ButtonStart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ButtonExit()
    {
        Application.Quit();
    }
    public void ButtonPlayerWon()
    {
        SceneManager.LoadScene("WinningScene");
    }
    public void ButtonPlayerLost()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void InternetLinkButton(string _link)
    {
        Application.OpenURL(_link);
    }
}
