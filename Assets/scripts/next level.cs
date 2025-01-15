using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextlevel : MonoBehaviour

{
    //[SerializeField] Animator fadeanimator;
    private void Start()
    {
        //fadeanimator = GetComponent<Animator>();
        AudioManager.instance.Play("mainmusic");
        //fadeanimator.SetTrigger("startscene");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            //fadeanimator.SetTrigger("endscene");
            SceneManager.LoadSceneAsync(3);
            
        }



    }
}
