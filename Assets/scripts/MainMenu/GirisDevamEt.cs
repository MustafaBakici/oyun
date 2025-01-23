using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GirisDevamEt : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        //SceneTransition.instance.FadeToScene("depo");
        SceneManager.LoadSceneAsync(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
