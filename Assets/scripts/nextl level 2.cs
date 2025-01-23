using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextlevel2 : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            //SceneTransition.instance.FadeToScene("LvlXL");
            SceneManager.LoadSceneAsync(4);

        }



    }
}
