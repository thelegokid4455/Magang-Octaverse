using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    AudioSource aS;
    bool useMusic;

    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        useMusic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(useMusic)
        {
            aS.enabled = true;
        }
        else
        {
            aS.enabled = false;
        }
    }

    public void ToggleMusic()
    {
        if (useMusic) useMusic = false; else useMusic = true;
    }
}
