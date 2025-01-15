using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MAinMenu : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.Stop("mainmusic");
        AudioManager.instance.Play("menumusic");
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan Çýkýldý"); 
        Application.Quit();
    }
    /*public void SettingsMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }*/

}
