using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextlevel3 : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            //SceneTransition.instance.FadeToScene("LvlXL");
            SceneManager.LoadSceneAsync(5);

        }



    }
}
