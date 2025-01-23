using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausemenu : MonoBehaviour
{
    public GameObject pauseMenuPrefab;
    public GameObject panel;

    public static bool isPause = false;

    // Start is called before the first frame update
    /*void Start()
    {
        pauseMenuPrefab.SetActive(false);
    }*/

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause) { resumeGame(); }

            else { PauseGame(); }

            

            
        }
    }

    public void PauseGame()
    {
     
        pauseMenuPrefab.SetActive(true);
        panel.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
    }
    public void resumeGame()
    {
     pauseMenuPrefab.SetActive(false);
        panel.SetActive(false);
        Time.timeScale = 1.0f;
        isPause = false;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
        isPause = false;
    }
    
    public void Restart()
    {
        Time.timeScale = 1.0f;
        isPause = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    

    public void QuitGame()
    {
        Debug.Log("Oyundan Çýkýldý");
        Application.Quit();
    }



}
