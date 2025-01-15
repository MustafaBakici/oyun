using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiostopper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Stop("menumusic");
        AudioManager.instance.Stop("mainmusic");
    }

}
